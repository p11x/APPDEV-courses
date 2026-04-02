# DataLoader

## What You'll Learn

- What the N+1 problem is and why it matters in GraphQL
- How DataLoader batches and caches database queries
- How to create and use a DataLoader instance per request
- How DataLoader deduplicates identical keys in a single batch
- How to integrate DataLoader with a real database

## The N+1 Problem

GraphQL's power — nested fields resolved independently — creates a performance trap. Consider this query:

```graphql
query {
  posts {          # 1 query to get all posts
    title
    author {       # N queries — one per post to get its author
      name
    }
  }
}
```

If there are 50 posts, this fires **51 database queries**: 1 for posts + 50 for individual authors. This is the **N+1 problem**.

```
Without DataLoader:
  Query posts           → SELECT * FROM posts                    (1 query)
  Get author of post 1  → SELECT * FROM users WHERE id = 1      (1 query)
  Get author of post 2  → SELECT * FROM users WHERE id = 2      (1 query)
  ...50 more queries...

With DataLoader:
  Query posts           → SELECT * FROM posts                    (1 query)
  Batch all author IDs  → SELECT * FROM users WHERE id IN (1,2…) (1 query)
                                                            Total: 2 queries
```

## DataLoader Implementation

```js
// dataloader-demo.js — Batching and caching with DataLoader

import DataLoader from 'dataloader';

// Simulate a database — each call logs to show batching in action
const db = {
  users: [
    { id: '1', name: 'Alice', email: 'alice@example.com' },
    { id: '2', name: 'Bob', email: 'bob@example.com' },
    { id: '3', name: 'Charlie', email: 'charlie@example.com' },
  ],

  posts: [
    { id: '1', title: 'GraphQL Intro', authorId: '1' },
    { id: '2', title: 'Node.js Tips', authorId: '2' },
    { id: '3', title: 'DataLoader Magic', authorId: '1' },
    { id: '4', title: 'Performance', authorId: '3' },
  ],
};

// Create a DataLoader for batching user lookups by ID
// The batch function receives an array of keys and returns an array of results
// The order of results MUST match the order of keys
const userLoader = new DataLoader(async (ids) => {
  console.log(`Batch query: fetching users [${ids.join(', ')}]`);

  // Fetch all users in a single query
  const users = db.users.filter((u) => ids.includes(u.id));

  // DataLoader requires results in the SAME order as the input keys
  // Map each key to its corresponding user (or null if not found)
  return ids.map((id) => users.find((u) => u.id === id) || null);
});

// Create a DataLoader for batching posts by author ID
const postsByAuthorLoader = new DataLoader(async (authorIds) => {
  console.log(`Batch query: fetching posts for authors [${authorIds.join(', ')}]`);

  const allPosts = db.posts.filter((p) => authorIds.includes(p.authorId));

  // Return an array of arrays — one result set per key
  return authorIds.map((id) => allPosts.filter((p) => p.authorId === id));
});

// === Simulating the GraphQL resolver flow ===

async function resolvePost(post) {
  // Each call to userLoader.load() queues the ID
  // DataLoader collects all IDs in the same tick and fires ONE batch query
  const author = await userLoader.load(post.authorId);
  return { title: post.title, author: author.name };
}

async function main() {
  // Fetch all posts
  const posts = db.posts;

  // Resolve each post's author — DataLoader batches these automatically
  const results = await Promise.all(posts.map(resolvePost));

  console.log('\nResults:');
  for (const r of results) {
    console.log(`  "${r.title}" by ${r.author}`);
  }

  // === Caching ===

  // DataLoader caches results within a single request
  // Loading the same key twice does NOT fire a second query
  console.log('\nCache test:');
  const alice = await userLoader.load('1');
  const aliceAgain = await userLoader.load('1');  // Cache hit — no query
  console.log(`  Same object? ${alice === aliceAgain}`);  // true

  // Clear the cache if needed (e.g., after a mutation updates the user)
  userLoader.clear('1');  // Remove one key from cache
  userLoader.clearAll();  // Clear the entire cache
}

main();
```

### Output

```
Batch query: fetching users [1, 2, 1, 3]

Results:
  "GraphQL Intro" by Alice
  "Node.js Tips" by Bob
  "DataLoader Magic" by Alice
  "Performance" by Charlie

Cache test:
  Same object? true
```

Notice: even though 4 posts reference 3 unique users, DataLoader deduplicates the keys. The batch receives `[1, 2, 1, 3]` but we only query 3 users.

## DataLoader in a GraphQL Server

```js
// server.js — GraphQL server with DataLoader per request

import { createServer } from 'node:http';
import { createSchema, createYoga } from 'graphql-yoga';
import DataLoader from 'dataloader';

// Simulated database
const db = {
  users: [
    { id: '1', name: 'Alice' },
    { id: '2', name: 'Bob' },
  ],
  posts: [
    { id: '1', title: 'Post 1', authorId: '1' },
    { id: '2', title: 'Post 2', authorId: '2' },
    { id: '3', title: 'Post 3', authorId: '1' },
  ],
};

const typeDefs = /* GraphQL */ `
  type Query {
    posts: [Post!]!
    user(id: ID!): User
  }
  type Post {
    id: ID!
    title: String!
    author: User!
  }
  type User {
    id: ID!
    name: String!
    posts: [Post!]!
  }
`;

const resolvers = {
  Query: {
    posts: (_, __, context) => db.posts,
    user: (_, { id }, context) => context.loaders.user.load(id),
  },

  Post: {
    author: (post, _, context) => context.loaders.user.load(post.authorId),
  },

  User: {
    posts: (user, _, context) => context.loaders.postsByAuthor.load(user.id),
  },
};

const schema = createSchema({ typeDefs, resolvers });

const yoga = createYoga({
  schema,
  // context is created per request — each request gets its own DataLoader instances
  context: () => ({
    loaders: {
      user: new DataLoader(async (ids) => {
        console.log('Batch user query:', ids);
        const users = db.users.filter((u) => ids.includes(u.id));
        return ids.map((id) => users.find((u) => u.id === id) || null);
      }),
      postsByAuthor: new DataLoader(async (authorIds) => {
        console.log('Batch posts query:', authorIds);
        const allPosts = db.posts.filter((p) => authorIds.includes(p.authorId));
        return authorIds.map((id) => allPosts.filter((p) => p.authorId === id));
      }),
    },
  }),
});

const server = createServer(yoga);
server.listen(4000, () => {
  console.log('GraphQL server with DataLoader on http://localhost:4000/graphql');
});
```

## How It Works

### The Batch Function

```js
const userLoader = new DataLoader(async (keys) => {
  // keys = ['1', '2', '1', '3'] (duplicates included)
  // Must return results in the SAME order as keys
  const users = await db.query('SELECT * FROM users WHERE id IN (?)', [keys]);
  return keys.map((key) => users.find((u) => u.id === key) || null);
});
```

1. All calls to `userLoader.load(id)` in the same event loop tick are collected
2. The batch function receives the deduplicated keys
3. You run ONE database query for all keys
4. Results are returned in key order — DataLoader distributes them to each `load()` caller

### Per-Request Instances

DataLoader caches results. If you reuse a loader across requests, stale data from a previous request can leak. Always create **new loaders per request** in the `context` function.

### Caching

DataLoader caches by key within a request:

```js
const user1 = await userLoader.load('1');  // Batch query fired
const user1Again = await userLoader.load('1');  // Cache hit — no query
```

After a mutation updates a user, clear the cache:

```js
userLoader.clear('1');  // Next load('1') will batch again
```

## Common Mistakes

### Mistake 1: Reusing DataLoader Across Requests

```js
// WRONG — cache leaks data between requests
const userLoader = new DataLoader(batchFn);

const yoga = createYoga({
  context: () => ({ loaders: { user: userLoader } }), // Same loader for all requests!
});

// CORRECT — new loader per request
const yoga = createYoga({
  context: () => ({
    loaders: {
      user: new DataLoader(batchFn),  // Fresh cache every request
    },
  }),
});
```

### Mistake 2: Result Order Mismatch

```js
// WRONG — results not in key order
new DataLoader(async (ids) => {
  const users = await db.query('SELECT * FROM users WHERE id IN (?)', [ids]);
  return users;  // Database may return users in a different order
});

// CORRECT — map results back to key order
new DataLoader(async (ids) => {
  const users = await db.query('SELECT * FROM users WHERE id IN (?)', [ids]);
  return ids.map((id) => users.find((u) => u.id === id) || null);
});
```

### Mistake 3: Using loadMany When load Suffices

```js
// loadMany is useful when you have all keys upfront
const users = await userLoader.loadMany(['1', '2', '3']);

// But for individual lookups, just use load — they are batched automatically
const user1 = await userLoader.load('1');
const user2 = await userLoader.load('2');
// Both are batched into one query within the same tick
```

## Try It Yourself

### Exercise 1: Add Comments Loader

Add a `Comment` type with `id`, `text`, `postId`, and `authorId`. Create a DataLoader that batches comment lookups by post ID. Query `{ posts { title comments { text author { name } } } }` and count how many database queries fire.

### Exercise 2: Measure Performance

Create a before/after test: run the same nested query without DataLoader (N+1 queries) and with DataLoader (batched). Log the query count difference.

### Exercise 3: Clear Cache After Mutation

Write a mutation that updates a user's name. After the update, clear the user loader's cache for that user ID. Verify that a subsequent query returns the updated name.

## Next Steps

You can optimise GraphQL queries. For securing your GraphQL API, continue to [Auth in GraphQL](./02-auth-in-graphql.md).
