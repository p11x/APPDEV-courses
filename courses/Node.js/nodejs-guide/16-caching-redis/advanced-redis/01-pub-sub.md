# Redis Pub/Sub

## What You'll Learn

- What publish/subscribe (pub/sub) messaging is
- How to use Redis pub/sub with ioredis
- Why you need separate clients for publishing and subscribing
- How to use channel patterns for wildcard subscriptions
- How Redis pub/sub compares to WebSocket broadcasting

## What Is Pub/Sub?

Publish/subscribe is a messaging pattern where **publishers** send messages to **channels** without knowing who receives them. **Subscribers** listen to channels without knowing who sends messages.

```
Publisher ──→ Channel: "notifications" ──→ Subscriber A
                                        ──→ Subscriber B
                                        ──→ Subscriber C
```

This decouples services — the publisher and subscriber do not need to know about each other.

## Basic Pub/Sub

```js
// pubsub-basic.js — Redis pub/sub with separate clients

import Redis from 'ioredis';

// IMPORTANT: A client that subscribes to channels CANNOT issue regular commands.
// It enters "subscriber mode" and can only subscribe/unsubscribe.
// You need SEPARATE clients for publishing and subscribing.

const subscriber = new Redis();
const publisher = new Redis();

// === Subscriber ===

// Subscribe to a channel
await subscriber.subscribe('notifications');

// 'message' fires when a message arrives on any subscribed channel
subscriber.on('message', (channel, message) => {
  console.log(`[${channel}] ${message}`);
});

console.log('Subscriber listening on "notifications"');

// Wait a moment for subscription to be registered
await new Promise((r) => setTimeout(r, 100));

// === Publisher ===

// Publish a message to a channel
// The return value is the number of subscribers that received the message
const receivers = await publisher.publish('notifications', 'Hello, subscribers!');
console.log(`Message delivered to ${receivers} subscriber(s)`);

await publisher.publish('notifications', 'Another message');

// Pattern subscription — subscribe to channels matching a pattern
await subscriber.psubscribe('app:*');  // Matches app:users, app:orders, etc.

subscriber.on('pmessage', (pattern, channel, message) => {
  console.log(`[pattern:${pattern}] [${channel}] ${message}`);
});

await publisher.publish('app:users', JSON.stringify({ event: 'created', id: 1 }));
await publisher.publish('app:orders', JSON.stringify({ event: 'placed', id: 42 }));

// Clean up
await new Promise((r) => setTimeout(r, 500));
subscriber.disconnect();
publisher.disconnect();
```

### Output

```
Subscriber listening on "notifications"
Message delivered to 1 subscriber(s)
[notifications] Hello, subscribers!
[notifications] Another message
[pattern:app:*] [app:users] {"event":"created","id":1}
[pattern:app:*] [app:orders] {"event":"placed","id":42}
```

## Real-World Example: Cross-Process Notifications

```js
// notification-service.js — A service that publishes notifications

import Redis from 'ioredis';
import { createServer } from 'node:http';

const publisher = new Redis();

const server = createServer(async (req, res) => {
  if (req.url === '/notify' && req.method === 'POST') {
    let body = '';
    req.on('data', (chunk) => { body += chunk; });
    req.on('end', () => {
      const { channel, message } = JSON.parse(body);

      // Publish to Redis — any subscriber on any process/machine receives it
      publisher.publish(channel, JSON.stringify({
        message,
        timestamp: new Date().toISOString(),
      }));

      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ sent: true }));
    });
    return;
  }

  res.writeHead(404);
  res.end('Not found');
});

server.listen(3001, () => {
  console.log('Notification service on http://localhost:3001');
});
```

```js
// listener.js — A process that subscribes to notifications

import Redis from 'ioredis';

const subscriber = new Redis();

// Subscribe to multiple channels
await subscriber.subscribe('notifications', 'alerts', 'system');

subscriber.on('message', (channel, message) => {
  const data = JSON.parse(message);
  console.log(`[${channel}] ${data.message} (${data.timestamp})`);
});

console.log('Listening for notifications, alerts, and system messages');

// Keep the process alive
process.on('SIGINT', () => {
  subscriber.disconnect();
  process.exit();
});
```

### Testing

```bash
# In terminal 1: start the listener
node listener.js

# In terminal 2: send notifications
curl -X POST http://localhost:3001/notify \
  -H "Content-Type: application/json" \
  -d '{"channel":"notifications","message":"New user signed up"}'
```

## How It Works

### Why Separate Clients?

When a Redis client calls `subscribe()`, it enters **subscriber mode**. In this mode, it can only:
- `subscribe` / `unsubscribe`
- `psubscribe` / `punsubscribe`

It **cannot** run `get`, `set`, or any other command. That is why you need a dedicated subscriber client and a separate publisher/client for regular operations.

### Channel Patterns

| Pattern | Matches |
|---------|---------|
| `notifications` | Exact channel name |
| `app:*` | `app:users`, `app:orders`, `app:logs` |
| `user:?:status` | `user:1:status`, `user:a:status` (single character) |
| `app:*:error` | `app:users:error`, `app:orders:error` |

### Limitations

Redis pub/sub is **fire-and-forget**. If no subscriber is listening when a message is published, the message is lost. For guaranteed delivery, use a message queue like BullMQ (Chapter 17).

## Common Mistakes

### Mistake 1: Using the Same Client for Subscribe and Commands

```js
// WRONG — subscriber client cannot run get/set after subscribing
const redis = new Redis();
await redis.subscribe('channel');
await redis.get('key');  // Throws: "Connection in subscriber mode"

// CORRECT — separate clients
const subscriber = new Redis();
const publisher = new Redis();  // Use this for get/set/publish
await subscriber.subscribe('channel');
```

### Mistake 2: Not Handling JSON Parsing

```js
// WRONG — assume messages are strings
subscriber.on('message', (channel, message) => {
  console.log(message.length);  // Works for strings, fails for JSON
});

// CORRECT — parse if you know the format
subscriber.on('message', (channel, message) => {
  try {
    const data = JSON.parse(message);
    console.log(data);
  } catch {
    console.log(message);  // Plain string
  }
});
```

### Mistake 3: Not Unsubscribing

```js
// WRONG — subscriber stays subscribed even after it should stop
await subscriber.subscribe('channel');
// Process keeps running forever

// CORRECT — unsubscribe when done
await subscriber.unsubscribe('channel');
subscriber.disconnect();
```

## Try It Yourself

### Exercise 1: Chat with Pub/Sub

Build a simple chat system: one process publishes messages to a `chat:room` channel, another subscribes and prints them.

### Exercise 2: Pattern Matching

Subscribe to `events:*`. Publish to `events:login`, `events:logout`, and `events:purchase`. Verify that all three are received.

### Exercise 3: Multi-Process

Start 3 subscriber processes. Publish one message. Verify all 3 receive it. Kill one, publish again, verify only 2 receive it.

## Next Steps

You understand pub/sub messaging. For limiting API requests with Redis, continue to [Rate Limiting with Redis](./02-rate-limiting-redis.md).
