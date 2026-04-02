# Cache Invalidation Strategies

## What You'll Learn

- TTL-based invalidation
- Event-driven invalidation
- Cache warming patterns
- Multi-layer caching

## Invalidation Strategies

```javascript
// 1. TTL-based (time-based expiration)
await redis.set('data:1', value, { EX: 3600 }); // Expires in 1 hour

// 2. Event-driven (invalidate on write)
async function updateUser(id, data) {
    await db.update(id, data);
    await redis.del(`user:${id}`); // Invalidate immediately
    await redis.del('users:list');  // Invalidate related caches
}

// 3. Tag-based invalidation
class TagCache {
    constructor(redis) {
        this.redis = redis;
    }

    async set(key, value, tags = [], ttl = 3600) {
        await this.redis.set(key, JSON.stringify(value), { EX: ttl });

        // Track tags
        for (const tag of tags) {
            await this.redis.sAdd(`tag:${tag}`, key);
        }
    }

    async invalidateTag(tag) {
        const keys = await this.redis.sMembers(`tag:${tag}`);
        if (keys.length) await this.redis.del(keys);
        await this.redis.del(`tag:${tag}`);
    }
}
```

## Cache Warming

```javascript
// Pre-warm cache on startup
async function warmCache() {
    console.log('Warming cache...');
    const users = await db.query('SELECT * FROM users WHERE active = true');

    for (const user of users) {
        await redis.set(`user:${user.id}`, JSON.stringify(user), { EX: 3600 });
    }

    console.log(`Cache warmed: ${users.length} users`);
}

// Run on startup
await warmCache();
```

## Multi-Layer Cache

```javascript
async function getWithLayers(key, fetcher) {
    // L1: Local memory (fastest)
    const local = memoryCache.get(key);
    if (local) return local;

    // L2: Redis (shared)
    const cached = await redis.get(key);
    if (cached) {
        const value = JSON.parse(cached);
        memoryCache.set(key, value); // Populate L1
        return value;
    }

    // L3: Database (slowest)
    const value = await fetcher();
    await redis.set(key, JSON.stringify(value), { EX: 3600 });
    memoryCache.set(key, value);

    return value;
}
```

## Best Practices Checklist

- [ ] Use TTL as baseline protection
- [ ] Invalidate on writes, not just reads
- [ ] Implement cache warming for hot data
- [ ] Use multi-layer caching for performance
- [ ] Monitor cache invalidation patterns

## Cross-References

- See [In-Memory Caching](./01-in-memory-caching.md) for local caching
- See [Redis Caching](./02-redis-distributed.md) for distributed caching
- See [Performance](../12-performance-optimization/01-cpu-memory-optimization.md) for optimization

## Next Steps

Continue to [Concurrency and Parallelism](../14-concurrency-parallelism/01-async-optimization.md) for async patterns.
