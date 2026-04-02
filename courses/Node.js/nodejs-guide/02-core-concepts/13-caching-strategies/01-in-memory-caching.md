# In-Memory Caching Patterns

## What You'll Learn

- LRU cache implementation
- TTL-based caching
- Cache invalidation strategies
- Memory-efficient caching

## LRU Cache

```javascript
class LRUCache {
    constructor(maxSize) {
        this.maxSize = maxSize;
        this.cache = new Map();
    }

    get(key) {
        if (!this.cache.has(key)) return undefined;
        const value = this.cache.get(key);
        this.cache.delete(key);
        this.cache.set(key, value); // Move to end (most recent)
        return value;
    }

    set(key, value) {
        if (this.cache.has(key)) {
            this.cache.delete(key);
        } else if (this.cache.size >= this.maxSize) {
            this.cache.delete(this.cache.keys().next().value); // Remove oldest
        }
        this.cache.set(key, value);
    }

    has(key) { return this.cache.has(key); }
    delete(key) { return this.cache.delete(key); }
    clear() { this.cache.clear(); }
    get size() { return this.cache.size; }
}
```

## TTL Cache

```javascript
class TTLCache {
    constructor(defaultTTL = 60000) {
        this.cache = new Map();
        this.defaultTTL = defaultTTL;
    }

    set(key, value, ttl = this.defaultTTL) {
        this.cache.set(key, {
            value,
            expires: Date.now() + ttl,
        });
    }

    get(key) {
        const entry = this.cache.get(key);
        if (!entry) return undefined;
        if (Date.now() > entry.expires) {
            this.cache.delete(key);
            return undefined;
        }
        return entry.value;
    }

    cleanup() {
        const now = Date.now();
        for (const [key, entry] of this.cache) {
            if (now > entry.expires) this.cache.delete(key);
        }
    }
}

// Auto-cleanup every minute
const cache = new TTLCache(60000);
setInterval(() => cache.cleanup(), 60000);
```

## Caching Middleware

```javascript
function cacheMiddleware(ttl = 60000) {
    const cache = new TTLCache(ttl);

    return (req, res, next) => {
        if (req.method !== 'GET') return next();

        const cached = cache.get(req.originalUrl);
        if (cached) {
            return res.json(cached);
        }

        const originalJson = res.json.bind(res);
        res.json = (data) => {
            cache.set(req.originalUrl, data);
            return originalJson(data);
        };

        next();
    };
}

app.use('/api/', cacheMiddleware(30000));
```

## Best Practices Checklist

- [ ] Use LRU eviction for bounded caches
- [ ] Set TTL for all cached entries
- [ ] Implement cache cleanup intervals
- [ ] Cache at appropriate granularity (per-request vs per-data)
- [ ] Monitor cache hit rates

## Cross-References

- See [Redis Caching](./02-redis-distributed.md) for distributed caching
- See [Cache Invalidation](./03-cache-invalidation.md) for invalidation strategies
- See [Performance](../12-performance-optimization/01-cpu-memory-optimization.md) for optimization

## Next Steps

Continue to [Redis Distributed Caching](./02-redis-distributed.md) for distributed caching.
