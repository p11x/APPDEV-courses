# GraphQL Subscriptions

## What You'll Learn

- What GraphQL subscriptions are and when to use them
- How to set up subscriptions with graphql-yoga
- How to use a simple pub/sub system for event broadcasting
- How subscriptions use WebSocket transport
- How to subscribe to specific events in GraphiQL

## What Are Subscriptions?

Queries read data once. Mutations write data once. **Subscriptions** listen for data changes over time — the server pushes events to the client whenever something happens.

Subscriptions are GraphQL's answer to real-time updates. Under the hood, they use WebSockets.

```
Query:      Client → Server → Response (one-time)
Mutation:   Client → Server → Change → Response (one-time)
Subscription: Client ↔ Server → Events pushed continuously
```

## Server with Subscriptions

```js
// server.js — GraphQL server with queries, mutations, and subscriptions

import { createServer } from 'node:http';
import { createSchema, createYoga, createPubSub } from 'graphql-yoga';

// createPubSub() creates a simple in-memory publish/subscribe system
// In production, use a Redis-backed pub/sub for multi-server setups
const pubSub = createPubSub();

const messages = [];
let nextId = 1;

const typeDefs = /* GraphQL */ `
  type Query {
    messages: [Message!]!
  }

  type Mutation {
    sendMessage(input: SendMessageInput!): Message!
  }

  type Subscription {
    # Subscribe to all new messages
    messageAdded: Message!
    # Subscribe to messages in a specific channel
    messageAddedToChannel(channel: String!): Message!
  }

  input SendMessageInput {
    text: String!
    channel: String!
    sender: String!
  }

  type Message {
    id: ID!
    text: String!
    channel: String!
    sender: String!
    createdAt: String!
  }
`;

const resolvers = {
  Query: {
    messages: () => messages,
  },

  Mutation: {
    sendMessage: (_, { input }) => {
      const message = {
        id: String(nextId++),
        text: input.text,
        channel: input.channel,
        sender: input.sender,
        createdAt: new Date().toISOString(),
      };
      messages.push(message);

      // Publish the event — any subscribed clients will receive this
      // The topic name must match the subscription topic below
      pubSub.publish('MESSAGE_ADDED', { messageAdded: message });

      // Also publish to the channel-specific topic
      pubSub.publish(`MESSAGE_ADDED_TO_CHANNEL:${input.channel}`, {
        messageAddedToChannel: message,
      });

      return message;
    },
  },

  Subscription: {
    messageAdded: {
      // subscribe returns an AsyncIterable — Yoga handles the WebSocket framing
      subscribe: () => pubSub.subscribe('MESSAGE_ADDED'),
      // resolve transforms the published value before sending to the client
      resolve: (event) => event.messageAdded,
    },

    messageAddedToChannel: {
      subscribe: (_, args) => {
        // Dynamic topic based on the channel argument
        return pubSub.subscribe(`MESSAGE_ADDED_TO_CHANNEL:${args.channel}`);
      },
      resolve: (event) => event.messageAddedToChannel,
    },
  },
};

const schema = createSchema({ typeDefs, resolvers });
const yoga = createYoga({ schema });
const server = createServer(yoga);

server.listen(4000, () => {
  console.log('GraphQL server on http://localhost:4000/graphql');
  console.log('Open GraphiQL and try the subscription');
});
```

## Testing Subscriptions

Subscriptions require **two GraphiQL tabs** — one to subscribe, one to send messages.

### Tab 1: Subscribe

```graphql
subscription {
  messageAdded {
    id
    text
    sender
    channel
    createdAt
  }
}
```

Click the "Play" button. The subscription waits for events.

### Tab 2: Send a Message

```graphql
mutation {
  sendMessage(input: { text: "Hello!", channel: "general", sender: "Alice" }) {
    id
  }
}
```

Click "Play". Switch back to Tab 1 — the message appears in the subscription stream.

### Channel-Specific Subscription

Tab 1:

```graphql
subscription {
  messageAddedToChannel(channel: "random") {
    text
    sender
  }
}
```

This only receives messages sent to the "random" channel.

## How It Works

### Pub/Sub Pattern

```
Mutation (sendMessage)
  │
  ├── pubSub.publish('MESSAGE_ADDED', data)
  │     │
  │     └── All subscribers on 'MESSAGE_ADDED' receive the event
  │
  └── pubSub.publish('MESSAGE_ADDED_TO_CHANNEL:general', data)
        │
        └── Only subscribers on that specific channel topic receive it
```

### The subscribe() Resolver

Each subscription field has a `subscribe` function that returns an `AsyncIterable`. Yoga converts this into WebSocket messages. The `resolve` function transforms each event before it reaches the client.

### WebSocket Transport

Subscriptions use WebSocket connections. When you send a subscription query:

1. Client opens a WebSocket to the GraphQL server
2. Server starts the `subscribe()` async iterator
3. Each `pubSub.publish()` call sends a message through the WebSocket
4. When the client unsubscribes, the iterator completes

## Common Mistakes

### Mistake 1: Not Publishing After Mutation

```js
// WRONG — mutation creates data but never publishes the event
sendMessage: (_, { input }) => {
  const message = { ... };
  messages.push(message);
  return message;  // Subscription clients never see this!
};

// CORRECT — publish after adding to the store
sendMessage: (_, { input }) => {
  const message = { ... };
  messages.push(message);
  pubSub.publish('MESSAGE_ADDED', { messageAdded: message });  // Notify subscribers
  return message;
};
```

### Mistake 2: Using In-Memory Pub/Sub in Production

```js
// WRONG — in-memory pub/sub only works with a single server process
const pubSub = createPubSub();  // If you run 4 cluster workers, subscribers on
// other workers never receive events

// CORRECT — use Redis for multi-process pub/sub
import { createPubSub } from 'graphql-yoga';
import { createRedisEventTarget } from '@graphql-yoga/redis-event-target';
import { createClient } from 'redis';

const publishClient = createClient({ url: 'redis://localhost:6379' });
const subscribeClient = publishClient.duplicate();
await Promise.all([publishClient.connect(), subscribeClient.connect()]);

const pubSub = createPubSub({
  eventTarget: createRedisEventTarget({ publishClient, subscribeClient }),
});
```

### Mistake 3: Missing resolve on Subscription

```js
// WRONG — subscribe returns an event object, but the client expects messageAdded
Subscription: {
  messageAdded: {
    subscribe: () => pubSub.subscribe('MESSAGE_ADDED'),
    // Missing resolve — client gets { messageAdded: { messageAdded: { ... } } }
  },
}

// CORRECT — extract the nested field
Subscription: {
  messageAdded: {
    subscribe: () => pubSub.subscribe('MESSAGE_ADDED'),
    resolve: (event) => event.messageAdded,  // Unwrap the payload
  },
}
```

## Try It Yourself

### Exercise 1: Typing Indicator

Add a `userTyping(sender: String!, channel: String!)` mutation that publishes a `USER_TYPING` event. Add a `typingIndicator(channel: String!)` subscription that clients can listen on.

### Exercise 2: User Joined/Left

Add `joinChannel(channel: String!, user: String!)` and `leaveChannel(channel: String!, user: String!)` mutations. Publish events that notify other users in the channel.

### Exercise 3: Message Reactions

Add a `reactToMessage(messageId: ID!, emoji: String!)` mutation. Publish a `MESSAGE_REACTION` event. Add a `messageReactions(messageId: ID!)` subscription.

## Next Steps

You have real-time GraphQL. For solving the N+1 query problem in nested resolvers, continue to [DataLoader](../data-layer/01-dataloader.md).
