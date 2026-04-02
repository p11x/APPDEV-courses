# Caching Strategies Implementation

## What You'll Learn

- In-memory LRU caching
- Redis caching patterns
- Cache invalidation strategies
- Multi-level caching architecture

## In-Memory LRU Cache

```javascript
class LRUCache {
    constructor(maxSize, defaultTTL = 60000) {
        this.maxSize = maxSize;
        this.defaultTTL = defaultTTL;
        this.cache = new Map();
    }

    get(key) {
        const entry = this.cache.get(key);
        if (!entry) return undefined;

        if (Date.now() > entry.expires) {
            this.cache.delete(key);
            return undefined;
        }

        // Move to end (most recently used)
        this.cache.delete(key);
        this.cache.set(key, entry);
        return entry.value;
    }

    set(key, value, ttl = this.defaultTTL) {
        if (this.cache.has(key)) this.cache.delete(key);
        else if (this.cache.size >= this.maxSize) {
            this.cache.delete(this.cache.keys().next().value);
        }
        this.cache.set(key, { value, expires: Date.now() + ttl });
    }

    delete(key) { return this.cache.delete(key); }
    clear() { this.cache.clear(); }
    get size() { return this.cache.size; }
}

// Cache middleware
function cacheMiddleware(cache, ttl = 60000) {
    return (req, res, next) => {
        if (req.method !== 'GET') return next();

        const cached = cache.get(req.originalUrl);
        if (cached) return res.json(cached);

        const originalJson = res.json.bind(res);
        res.json = (data) => {
            cache.set(req.originalUrl, data, ttl);
            return originalJson(data);
        };
        next();
    };
}

const cache = new LRUCache(1000, 30000);
app.use('/api/', cacheMiddleware(cache));
```

## Redis Caching

```javascript
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
await redis.connect();

// Cache-aside pattern
async function getCachedData(key, fetcher, ttl = 3600) {
    const cached = await redis.get(key);
    if (cached) return JSON.parse(cached);

    const data = await fetcher();
    await redis.set(key, JSON.stringify(data), { EX: ttl });
    return data;
}

// Cache middleware with Redis
function redisCacheMiddleware(ttl = 300) {
    return async (req, res, next) => {
        if (req.method !== 'GET') return next();

        const key = `cache:${req.originalUrl}`;
        const cached = await redis.get(key);

        if (cached) {
            return res.json(JSON.parse(cached));
        }

        const originalJson = res.json.bind(res);
        res.json = async (data) => {
            await redis.set(key, JSON.stringify(data), { EX: ttl });
            return originalJson(data);
        };
        next();
    };
}

// Cache invalidation
async function invalidateCache(pattern) {
    const keys = await redis.keys(pattern);
    if (keys.length) await redis.del(keys);
}

// Usage: invalidate on write
app.post('/api/users', async (req, res) => {
    const user = await User.create(req.body);
    await invalidateCache('cache:/api/users*');
    res.status(201).json(user);
});
```

## Multi-Level Cache

```javascript
class MultiLevelCache {
    constructor(l1Cache, l2Cache) {
        this.l1 = l1Cache;  // In-memory (fast)
        this.l2 = l2Cache;  // Redis (shared)
    }

    async get(key) {
        // L1 check
        const l1Value = this.l1.get(key);
        if (l1Value !== undefined) return l1Value;

        // L2 check
        const l2Value = await this.l2.get(key);
        if (l2Value) {
            this.l1.set(key, l2Value); // Populate L1
            return l2Value;
        }

        return undefined;
    }

    async set(key, value, ttl) {
        this.l1.set(key, value, ttl);
        await this.l2.set(key, value, ttl);
    }

    async delete(key) {
        this.l1.delete(key);
        await this.l2.del(key);
    }
}
```

## Best Practices Checklist

- [ ] Use LRU eviction for bounded caches
- [ ] Set TTL for all cached entries
- [ ] Invalidate cache on write operations
- [ ] Use multi-level caching for performance
- [ ] Monitor cache hit/miss rates

## Cross-References

- See [Database Performance](../02-database-performance-optimization/01-query-optimization.md) for queries
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for metrics
- See [Scalability](../05-scalability-patterns/01-load-balancing.md) for scaling

## Next Steps

Continue to [Scalability Patterns](../05-scalability-patterns/01-load-balancing.md).
