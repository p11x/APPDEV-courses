# Redis Distributed Caching

## What You'll Learn

- Redis client setup and basic operations
- Distributed caching patterns
- Cache-aside pattern
- Session caching with Redis

## Redis Setup

```javascript
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
redis.on('error', (err) => console.error('Redis error:', err));
await redis.connect();
```

## Basic Operations

```javascript
// String operations
await redis.set('user:1', JSON.stringify(user));
await redis.set('user:1', JSON.stringify(user), { EX: 3600 }); // TTL: 1 hour
const user = JSON.parse(await redis.get('user:1'));

// Hash operations
await redis.hSet('user:1', { name: 'Alice', email: 'alice@example.com' });
const name = await redis.hGet('user:1', 'name');
const all = await redis.hGetAll('user:1');

// List operations
await redis.lPush('queue', JSON.stringify(job));
const job = JSON.parse(await redis.rPop('queue'));

// Set operations
await redis.sAdd('tags:article:1', 'nodejs', 'javascript');
const tags = await redis.sMembers('tags:article:1');

// Delete
await redis.del('user:1');

// Pattern delete
const keys = await redis.keys('session:*');
if (keys.length) await redis.del(keys);
```

## Cache-Aside Pattern

```javascript
async function getUser(id) {
    const cacheKey = `user:${id}`;

    // 1. Check cache
    const cached = await redis.get(cacheKey);
    if (cached) return JSON.parse(cached);

    // 2. Fetch from database
    const user = await db.query('SELECT * FROM users WHERE id = $1', [id]);

    // 3. Store in cache
    await redis.set(cacheKey, JSON.stringify(user), { EX: 3600 });

    return user;
}

async function updateUser(id, data) {
    // 1. Update database
    const user = await db.query('UPDATE users SET ... WHERE id = $1', [id]);

    // 2. Invalidate cache
    await redis.del(`user:${id}`);

    return user;
}
```

## Best Practices Checklist

- [ ] Always set TTL for cached entries
- [ ] Invalidate cache on write operations
- [ ] Use connection pooling for Redis
- [ ] Handle Redis unavailability gracefully
- [ ] Monitor cache hit/miss rates

## Cross-References

- See [In-Memory Caching](./01-in-memory-caching.md) for local caching
- See [Cache Invalidation](./03-cache-invalidation.md) for invalidation
- See [Performance](../12-performance-optimization/02-io-database-optimization.md) for I/O

## Next Steps

Continue to [Cache Invalidation](./03-cache-invalidation.md) for invalidation strategies.
