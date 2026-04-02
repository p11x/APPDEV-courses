# Cache Invalidation Patterns and Strategies

## What You'll Learn

- TTL-based invalidation
- Event-driven invalidation
- Version-based invalidation
- Tag-based invalidation
- Consistency patterns

## TTL-Based Invalidation

```javascript
class TTLCache {
    constructor(redis) {
        this.redis = redis;
    }

    async set(key, value, ttl) {
        await this.redis.set(key, JSON.stringify(value), { EX: ttl });
    }

    // Stale-while-revalidate pattern
    async getWithSWR(key, fetcher, ttl = 300, staleTtl = 3600) {
        const cached = await this.redis.get(key);
        
        if (cached) {
            const data = JSON.parse(cached);
            
            // If still fresh, return directly
            if (data._expiresAt > Date.now()) {
                return data.value;
            }
            
            // If stale, return stale and refresh in background
            if (data._staleUntil > Date.now()) {
                this.refreshInBackground(key, fetcher, ttl, staleTtl);
                return data.value;
            }
        }

        // Miss - fetch synchronously
        const value = await fetcher();
        await this.setWithSWR(key, value, ttl, staleTtl);
        return value;
    }

    async setWithSWR(key, value, ttl, staleTtl) {
        const data = {
            value,
            _expiresAt: Date.now() + ttl * 1000,
            _staleUntil: Date.now() + (ttl + staleTtl) * 1000,
        };
        await this.redis.set(key, JSON.stringify(data), { EX: ttl + staleTtl });
    }

    async refreshInBackground(key, fetcher, ttl, staleTtl) {
        try {
            const value = await fetcher();
            await this.setWithSWR(key, value, ttl, staleTtl);
        } catch (err) {
            console.error('Background refresh failed:', err.message);
        }
    }
}
```

## Event-Driven Invalidation

```javascript
import { EventEmitter } from 'node:events';

class EventDrivenCache extends EventEmitter {
    constructor(redis) {
        super();
        this.redis = redis;
        this.subscriptions = new Map();
    }

    async set(key, value, ttl, tags = []) {
        await this.redis.set(key, JSON.stringify(value), { EX: ttl });

        // Track tags for this key
        for (const tag of tags) {
            await this.redis.sAdd(`tag:${tag}`, key);
            await this.redis.expire(`tag:${tag}`, ttl);
        }

        // Emit set event
        this.emit('cache:set', { key, tags });
    }

    async invalidate(key) {
        await this.redis.del(key);
        this.emit('cache:invalidate', { key });
    }

    async invalidateByTag(tag) {
        const keys = await this.redis.sMembers(`tag:${tag}`);
        if (keys.length > 0) {
            await this.redis.del(keys);
            await this.redis.del(`tag:${tag}`);
            this.emit('cache:invalidate:tag', { tag, keys });
        }
    }

    // Subscribe to invalidation events (cross-instance)
    async subscribeToInvalidation(pubsub) {
        await pubsub.subscribe('cache:invalidate', (message) => {
            const { key } = JSON.parse(message);
            this.redis.del(key); // Local invalidation
        });

        await pubsub.subscribe('cache:invalidate:tag', (message) => {
            const { tag } = JSON.parse(message);
            this.invalidateByTag(tag);
        });
    }

    // Publish invalidation event
    async publishInvalidation(pubsub, type, data) {
        await pubsub.publish(type, JSON.stringify(data));
    }
}
```

## Version-Based Invalidation

```javascript
class VersionedCache {
    constructor(redis) {
        this.redis = redis;
    }

    async get(key, fetcher, ttl = 300) {
        const versionKey = `version:${key}`;
        const currentVersion = await this.redis.get(versionKey) || '1';

        const cacheKey = `${key}:v${currentVersion}`;
        const cached = await this.redis.get(cacheKey);

        if (cached) return JSON.parse(cached);

        const data = await fetcher();
        await this.redis.set(cacheKey, JSON.stringify(data), { EX: ttl });
        return data;
    }

    async invalidate(key) {
        // Increment version - old entries will expire naturally
        const versionKey = `version:${key}`;
        await this.redis.incr(versionKey);
    }

    async invalidateAll(prefix) {
        let cursor = 0;
        do {
            const result = await this.redis.scan(cursor, {
                MATCH: `version:${prefix}:*`,
                COUNT: 100,
            });
            cursor = result.cursor;
            for (const key of result.keys) {
                await this.redis.incr(key);
            }
        } while (cursor !== 0);
    }
}
```

## Consistency Patterns Comparison

```
Cache Consistency Patterns:
─────────────────────────────────────────────
Pattern          Latency  Consistency  Complexity
─────────────────────────────────────────────
Cache-aside      Low      Eventually   Low
Write-through    Medium   Strong       Medium
Write-behind     Low      Eventually   High
Read-through     Medium   Eventually   Medium
Refresh-ahead    Low      Near-real    Medium

Invalidation Strategies:
├── TTL: Simple, eventual consistency, over-caching
├── Event: Strong consistency, requires pub/sub
├── Version: Good for config, no race conditions
├── Tag: Flexible, good for related data groups
└── Hybrid: Combine TTL + events for best of both
```

## Best Practices Checklist

- [ ] Set TTL for all cached entries (never infinite cache)
- [ ] Use event-driven invalidation for strong consistency
- [ ] Use version-based invalidation for configuration data
- [ ] Implement stale-while-revalidate for non-critical data
- [ ] Handle cache stampede with locks or probabilistic early expiration
- [ ] Use tag-based invalidation for related data groups
- [ ] Monitor cache hit/miss rates

## Cross-References

- See [Redis Caching](./02-redis-caching.md) for Redis implementation
- See [In-Memory Caching](./01-in-memory-caching.md) for local caching
- See [Distributed Caching](./05-distributed-caching.md) for multi-instance caching

## Next Steps

Continue to [Distributed Caching](./05-distributed-caching.md) for multi-node caching.
