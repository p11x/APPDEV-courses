# Redis Setup

## What You'll Learn

- What Redis is and why it is used for caching
- How to install and connect to Redis with ioredis
- How to use basic commands: get, set, del, keys
- How to design a key naming strategy
- How to handle connection errors and reconnection

## What Is Redis?

Redis (Remote Dictionary Server) is an **in-memory key-value store**. It stores data in RAM, making reads and writes extremely fast (sub-millisecond). It is commonly used as:

- A **cache** for database query results
- A **session store** for user sessions
- A **message broker** for pub/sub
- A **rate limiter** for API throttling

Redis stores data as key-value pairs. Keys are strings. Values can be strings, lists, sets, hashes, sorted sets, and more.

## Installing Redis

```bash
# macOS
brew install redis
brew services start redis

# Ubuntu/Debian
sudo apt install redis-server
sudo systemctl start redis

# Docker (any platform)
docker run -d --name redis -p 6379:6379 redis:7-alpine

# Verify Redis is running
redis-cli ping
# Should respond: PONG
```

## Project Setup

```bash
mkdir redis-demo && cd redis-demo
npm init -y
npm install ioredis
```

Add `"type": "module"` to `package.json`.

## Connecting to Redis

```js
// connect.js — Connect to Redis and run basic operations

import Redis from 'ioredis';

// Create a Redis client — connects to localhost:6379 by default
const redis = new Redis({
  host: '127.0.0.1',  // Redis server hostname
  port: 6379,          // Redis server port
  db: 0,               // Database number (0-15, default is 0)

  // Reconnection strategy — retry with exponential backoff
  retryStrategy(times) {
    const delay = Math.min(times * 100, 3000);  // Max 3 seconds between retries
    return delay;
  },

  // Do not queue commands while reconnecting
  enableOfflineQueue: false,
});

// Handle connection events
redis.on('connect', () => {
  console.log('Connected to Redis');
});

redis.on('error', (err) => {
  console.error('Redis error:', err.message);
});

redis.on('close', () => {
  console.log('Redis connection closed');
});

// === Basic Operations ===

async function main() {
  // SET — store a key-value pair
  // EX = expire in seconds (TTL)
  await redis.set('greeting', 'Hello, Redis!', 'EX', 60);
  console.log('Set greeting with 60s TTL');

  // GET — retrieve a value by key
  const value = await redis.get('greeting');
  console.log('Got greeting:', value);
  // "Hello, Redis!"

  // EXISTS — check if a key exists (returns 1 or 0)
  const exists = await redis.exists('greeting');
  console.log('greeting exists:', exists);
  // 1

  // TTL — check remaining time-to-live in seconds
  const ttl = await redis.ttl('greeting');
  console.log('TTL remaining:', ttl);
  // ~60

  // DEL — delete one or more keys
  await redis.del('greeting');
  console.log('Deleted greeting');

  // Confirm deletion
  const afterDelete = await redis.get('greeting');
  console.log('After delete:', afterDelete);
  // null

  // === Working with Multiple Keys ===

  // MSET — set multiple keys at once
  await redis.mset('user:1:name', 'Alice', 'user:1:email', 'alice@example.com');

  // MGET — get multiple keys at once
  const [name, email] = await redis.mget('user:1:name', 'user:1:email');
  console.log('User 1:', name, email);
  // "Alice" "alice@example.com"

  // KEYS — find keys matching a pattern (DANGEROUS in production — see below)
  const keys = await redis.keys('user:1:*');
  console.log('User 1 keys:', keys);
  // ["user:1:name", "user:1:email"]

  // Clean up
  await redis.del('user:1:name', 'user:1:email');

  // Close the connection when done
  redis.disconnect();
}

main().catch(console.error);
```

## Key Naming Strategy

Use **namespaced colons** to organize keys:

```
app:users:1:name
app:users:1:email
app:sessions:abc123
app:cache:api:/users:query
```

Rules:
- Prefix all keys with your app name (`app:`)
- Use colons to separate namespaces
- Include the resource type and ID
- Be consistent — every developer on the team follows the same pattern

```js
// keys.js — Key naming helper

import Redis from 'ioredis';

const redis = new Redis();

// Build key names programmatically — avoids typos
function userKey(id, field) {
  return `app:users:${id}:${field}`;
}

function sessionKey(token) {
  return `app:sessions:${token}`;
}

function cacheKey(endpoint, params) {
  // Sort params to ensure consistent key regardless of argument order
  const sorted = Object.entries(params).sort().map(([k, v]) => `${k}=${v}`).join('&');
  return `app:cache:${endpoint}:${sorted}`;
}

async function main() {
  await redis.set(userKey('1', 'name'), 'Alice');
  await redis.set(userKey('1', 'email'), 'alice@example.com');
  await redis.set(sessionKey('tok_abc'), JSON.stringify({ userId: '1' }));
  await redis.set(cacheKey('/api/users', { page: 1, limit: 10 }), '{"users":[]}');

  // Scan for all user:1 keys
  const keys = await redis.keys(userKey('1', '*'));
  console.log('User 1 keys:', keys);

  redis.disconnect();
}

main();
```

## Avoiding the KEYS Command

```js
// WRONG — KEYS scans ALL keys in the database (O(N) operation)
// On a production Redis with millions of keys, this blocks everything
const keys = await redis.keys('app:*');

// CORRECT — use SCAN for incremental iteration
async function scanKeys(pattern) {
  const keys = [];
  let cursor = '0';

  do {
    // SCAN returns [nextCursor, matchingKeys[]]
    const [next, batch] = await redis.scan(cursor, 'MATCH', pattern, 'COUNT', 100);
    cursor = next;
    keys.push(...batch);
  } while (cursor !== '0');  // cursor '0' means iteration is complete

  return keys;
}

// Usage
const allAppKeys = await scanKeys('app:*');
console.log('Found', allAppKeys.length, 'keys');
```

## How It Works

### Connection Lifecycle

```
Your App                Redis Server
   │                        │
   │── TCP connect ────────→│
   │←── PONG ───────────────│  (handshake)
   │                        │
   │── SET key value ──────→│
   │←── OK ─────────────────│
   │                        │
   │── GET key ────────────→│
   │←── "value" ────────────│
   │                        │
   │── Connection lost ─────│
   │── Retry (100ms) ───────│
   │── Retry (200ms) ───────│
   │── Retry (300ms) ───────│
   │── Reconnected! ────────│
```

### ioredis vs redis

| Package | Notes |
|---------|-------|
| `ioredis` | More features, cluster support, Lua scripting, better TypeScript |
| `redis` | Official Node.js client, simpler API |

This guide uses `ioredis` for its richer feature set.

## Common Mistakes

### Mistake 1: No Error Handling

```js
// WRONG — if Redis is down, the app crashes with unhandled error
const redis = new Redis();
await redis.set('key', 'value');  // Throws if Redis is unreachable

// CORRECT — handle connection errors
redis.on('error', (err) => {
  console.error('Redis error:', err.message);
  // Decide: retry, fallback, alert, etc.
});
```

### Mistake 2: Unbounded Key Names

```js
// WRONG — user input in key names without sanitisation
await redis.set(`cache:${userInput}`, data);
// If userInput contains spaces, colons, or special chars, it corrupts your key namespace

// CORRECT — sanitise or hash dynamic parts
import { createHash } from 'node:crypto';
const hash = createHash('sha256').update(userInput).digest('hex').slice(0, 16);
await redis.set(`cache:${hash}`, data);
```

### Mistake 3: Not Disconnecting in Scripts

```js
// WRONG — Node.js process hangs because Redis connection keeps it alive
const redis = new Redis();
await redis.set('key', 'value');
console.log('Done');
// Process does not exit!

// CORRECT — disconnect when done
await redis.set('key', 'value');
redis.disconnect();
```

## Try It Yourself

### Exercise 1: Store and Retrieve

Store a user object (name, email, age) as separate Redis keys (`user:1:name`, etc.). Retrieve all fields and print the reconstructed object.

### Exercise 2: Key Expiry

Set a key with a 10-second TTL. Print its TTL every 2 seconds until it expires. Verify that `get` returns `null` after expiry.

### Exercise 3: Scan Pattern

Insert 1000 keys with pattern `test:item:N`. Use `SCAN` (not `KEYS`) to find all of them. Count how many SCAN iterations were needed.

## Next Steps

You can connect to Redis and store key-value pairs. Let's explore expiry and caching patterns. Continue to [Expiry Patterns](./02-expiry-patterns.md).
