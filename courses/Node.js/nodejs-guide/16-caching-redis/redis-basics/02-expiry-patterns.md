# Expiry Patterns

## What You'll Learn

- How Redis key expiration works (TTL)
- What the cache-aside pattern is and how to implement it
- What the write-through pattern is and when to use it
- How to handle cache invalidation correctly
- How to prevent cache stampedes with locks

## Key Expiration (TTL)

Every Redis key can have a **Time To Live (TTL)**. When the TTL reaches zero, Redis deletes the key automatically. This is the foundation of caching — store data temporarily, and it disappears when stale.

```js
// ttl-basics.js — Setting and checking TTL

import Redis from 'ioredis';
const redis = new Redis();

await redis.set('temp', 'value', 'EX', 30);  // Expires in 30 seconds

// Check remaining TTL
const ttl = await redis.ttl('temp');  // ~30
console.log('TTL:', ttl);

// Extend TTL
await redis.expire('temp', 60);  // Now expires in 60 seconds

// Remove TTL (make the key persistent)
await redis.persist('temp');  // No longer expires

redis.disconnect();
```

### EX vs PX

```js
// EX = seconds, PX = milliseconds
await redis.set('key', 'value', 'EX', 60);   // 60 seconds
await redis.set('key', 'value', 'PX', 60000); // 60000 milliseconds (same)
```

## Cache-Aside Pattern

The most common caching strategy. The application checks the cache first; on a miss, it queries the database and populates the cache.

```
Read request
    │
    ▼
Check Redis cache
    │
    ├── Cache HIT ──→ Return cached data
    │
    └── Cache MISS ──→ Query database
                          │
                          ▼
                     Store in Redis (with TTL)
                          │
                          ▼
                     Return data
```

```js
// cache-aside.js — Cache-aside pattern with Express and Redis

import { createServer } from 'node:http';
import Redis from 'ioredis';

const redis = new Redis();

// Simulated database — slow query
async function getUserFromDB(id) {
  console.log(`[DB] Querying user ${id}...`);
  await new Promise((r) => setTimeout(r, 200));  // Simulate 200ms DB latency

  const users = {
    '1': { id: '1', name: 'Alice', email: 'alice@example.com' },
    '2': { id: '2', name: 'Bob', email: 'bob@example.com' },
  };

  return users[id] || null;
}

// Cache-aside: check cache first, then DB, then populate cache
async function getUser(id) {
  const cacheKey = `app:users:${id}`;

  // Step 1: Check the cache
  const cached = await redis.get(cacheKey);
  if (cached) {
    console.log(`[CACHE] HIT for user ${id}`);
    return JSON.parse(cached);  // Parse the JSON string back to an object
  }

  // Step 2: Cache miss — query the database
  console.log(`[CACHE] MISS for user ${id}`);
  const user = await getUserFromDB(id);

  if (user) {
    // Step 3: Store in cache with TTL
    // Set TTL to 5 minutes — adjust based on how stale data can be
    await redis.set(cacheKey, JSON.stringify(user), 'EX', 300);
  }

  return user;
}

// HTTP server
const server = createServer(async (req, res) => {
  const match = req.url?.match(/^\/users\/(\d+)$/);

  if (match) {
    const user = await getUser(match[1]);

    if (user) {
      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify(user));
    } else {
      res.writeHead(404, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ error: 'User not found' }));
    }
    return;
  }

  res.writeHead(404);
  res.end('Not found');
});

server.listen(3000, () => {
  console.log('Server on http://localhost:3000');
  console.log('Try: curl http://localhost:3000/users/1');
});
```

### Testing

```bash
# First request — cache miss, slow
time curl http://localhost:3000/users/1
# ~200ms

# Second request — cache hit, fast
time curl http://localhost:3000/users/1
# ~2ms
```

## Write-Through Pattern

In write-through, you update the cache **every time you write to the database**. The cache is always in sync with the database.

```js
// write-through.js — Always keep cache in sync on writes

import Redis from 'ioredis';
const redis = new Redis();

// Simulated database
const db = new Map();

async function saveUser(id, data) {
  // Step 1: Write to database
  db.set(id, data);
  console.log('[DB] Saved user', id);

  // Step 2: Immediately update the cache
  const cacheKey = `app:users:${id}`;
  await redis.set(cacheKey, JSON.stringify(data), 'EX', 300);
  console.log('[CACHE] Updated user', id);

  return data;
}

async function getUser(id) {
  const cacheKey = `app:users:${id}`;
  const cached = await redis.get(cacheKey);

  if (cached) {
    return JSON.parse(cached);
  }

  const user = db.get(id) || null;
  if (user) {
    await redis.set(cacheKey, JSON.stringify(user), 'EX', 300);
  }
  return user;
}

// Usage
await saveUser('1', { name: 'Alice', email: 'alice@example.com' });

const user = await getUser('1');  // Always cache hit after save
console.log('Got user:', user);

redis.disconnect();
```

## Cache Invalidation

When data changes, the cache must be updated or removed. There are three strategies:

| Strategy | How It Works | Risk |
|----------|-------------|------|
| TTL expiry | Key expires automatically | Stale data until TTL |
| Delete on write | `del(key)` after DB update | Cache stampede on next read |
| Update on write | `set(key, value)` after DB update | Always consistent (preferred) |

```js
// invalidation.js — Invalidate cache when data changes

import Redis from 'ioredis';
const redis = new Redis();

async function updateUser(id, updates) {
  // Update database
  console.log('[DB] Updating user', id);

  // Strategy: delete the cache key — next read will repopulate
  await redis.del(`app:users:${id}`);
  console.log('[CACHE] Invalidated user', id);

  // Alternative: update the cache directly
  // await redis.set(`app:users:${id}`, JSON.stringify(updated), 'EX', 300);
}

await updateUser('1', { name: 'Alice Updated' });

redis.disconnect();
```

## Preventing Cache Stampede

A **cache stampede** happens when a popular cached key expires and many requests simultaneously try to regenerate it, all hitting the database.

```js
// stampede-lock.js — Use a lock to prevent cache stampede

import Redis from 'ioredis';
const redis = new Redis();

async function getUserWithLock(id) {
  const cacheKey = `app:users:${id}`;
  const lockKey = `lock:users:${id}`;

  // Check cache
  const cached = await redis.get(cacheKey);
  if (cached) return JSON.parse(cached);

  // Try to acquire a lock — SET NX (set if not exists) with expiry
  const lockAcquired = await redis.set(lockKey, '1', 'EX', 10, 'NX');
  // NX = only set if the key does NOT exist (atomic check-and-set)

  if (lockAcquired) {
    // We got the lock — regenerate the cache
    console.log('[LOCK] Acquired lock, regenerating cache');
    const user = await fetchUserFromDB(id);
    await redis.set(cacheKey, JSON.stringify(user), 'EX', 300);
    await redis.del(lockKey);  // Release the lock
    return user;
  } else {
    // Another request is regenerating — wait and retry
    console.log('[LOCK] Waiting for cache regeneration...');
    await new Promise((r) => setTimeout(r, 100));
    return getUserWithLock(id);  // Retry (cache should be populated now)
  }
}

async function fetchUserFromDB(id) {
  await new Promise((r) => setTimeout(r, 200));
  return { id, name: 'Alice', email: 'alice@example.com' };
}

// Simulate 5 concurrent requests for the same user
const results = await Promise.all([
  getUserWithLock('1'),
  getUserWithLock('1'),
  getUserWithLock('1'),
  getUserWithLock('1'),
  getUserWithLock('1'),
]);

console.log(`Got ${results.length} results (only 1 DB query)`);

redis.disconnect();
```

## How It Works

### Cache Stampede Prevention

```
Request A ──→ Cache miss ──→ Acquire lock ──→ Query DB ──→ Populate cache
Request B ──→ Cache miss ──→ Lock taken ──→ Wait 100ms ──→ Cache hit
Request C ──→ Cache miss ──→ Lock taken ──→ Wait 100ms ──→ Cache hit
```

Only one request hits the database. The others wait briefly and then read from the newly populated cache.

## Common Mistakes

### Mistake 1: Caching Null Results Without Shorter TTL

```js
// WRONG — if a user does not exist, caching null for 5 minutes
// means the DB is never checked again even if the user is created
const user = await getUserFromDB(id);
await redis.set(key, JSON.stringify(user), 'EX', 300);  // null cached for 5 minutes

// CORRECT — cache null with a shorter TTL
const user = await getUserFromDB(id);
if (user) {
  await redis.set(key, JSON.stringify(user), 'EX', 300);
} else {
  await redis.set(key, 'null', 'EX', 30);  // Cache null for 30 seconds only
}
```

### Mistake 2: Forgetting to Invalidate on Delete

```js
// WRONG — deleting from DB but leaving cache intact
async function deleteUser(id) {
  db.delete(id);  // DB updated
  // Cache still has the old data!
}

// CORRECT — invalidate cache on delete
async function deleteUser(id) {
  db.delete(id);
  await redis.del(`app:users:${id}`);
}
```

### Mistake 3: No TTL on Cached Data

```js
// WRONG — cache never expires, data goes stale permanently
await redis.set(key, JSON.stringify(data));

// CORRECT — always set a TTL
await redis.set(key, JSON.stringify(data), 'EX', 300);
```

## Try It Yourself

### Exercise 1: Cache a List Endpoint

Implement cache-aside for a `GET /users?page=1&limit=10` endpoint. The cache key should include the query parameters.

### Exercise 2: Write-Through with Invalidation

Implement save, update, and delete operations that always update or invalidate the cache. Verify that after an update, the cached value reflects the change.

### Exercise 3: Stampede Test

Simulate 50 concurrent requests for the same uncached key. Without the lock, count how many DB queries fire. With the lock, verify only 1 DB query fires.

## Next Steps

You understand caching and expiry. For storing sessions in Redis, continue to [Session Store](./03-session-store.md).
