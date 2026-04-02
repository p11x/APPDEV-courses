# Redis Caching Implementation Patterns

## What You'll Learn

- Redis connection and configuration
- Cache-aside, write-through, write-behind patterns
- Redis data structures for caching
- Redis clustering and sentinel setup
- Cache performance optimization

## Redis Client Setup

```bash
npm install redis
```

```javascript
import { createClient } from 'redis';

const redis = createClient({
    url: process.env.REDIS_URL || 'redis://localhost:6379',
    password: process.env.REDIS_PASSWORD,
    socket: {
        connectTimeout: 5000,
        reconnectStrategy: (retries) => {
            if (retries > 10) return new Error('Max retries reached');
            return Math.min(retries * 100, 3000);
        },
    },
    database: 0,
});

redis.on('error', (err) => console.error('Redis error:', err));
redis.on('connect', () => console.log('Redis connected'));
redis.on('reconnecting', () => console.log('Redis reconnecting'));

await redis.connect();
```

## Cache-Aside Pattern

```javascript
class CacheAside {
    constructor(redis, options = {}) {
        this.redis = redis;
        this.defaultTTL = options.defaultTTL || 300;
        this.prefix = options.prefix || 'cache';
    }

    key(id) {
        return `${this.prefix}:${id}`;
    }

    async get(id, fetcher, ttl) {
        const cached = await this.redis.get(this.key(id));
        if (cached) return JSON.parse(cached);

        const data = await fetcher();
        if (data !== null && data !== undefined) {
            await this.redis.set(this.key(id), JSON.stringify(data), {
                EX: ttl || this.defaultTTL,
            });
        }
        return data;
    }

    async set(id, data, ttl) {
        await this.redis.set(this.key(id), JSON.stringify(data), {
            EX: ttl || this.defaultTTL,
        });
    }

    async invalidate(id) {
        await this.redis.del(this.key(id));
    }

    async invalidatePattern(pattern) {
        let cursor = 0;
        do {
            const result = await this.redis.scan(cursor, {
                MATCH: `${this.prefix}:${pattern}`,
                COUNT: 100,
            });
            cursor = result.cursor;
            if (result.keys.length > 0) {
                await this.redis.del(result.keys);
            }
        } while (cursor !== 0);
    }
}
```

## Write-Through Cache

```javascript
class WriteThroughCache {
    constructor(redis, repository) {
        this.redis = redis;
        this.repo = repository;
    }

    async get(id) {
        const key = `entity:${id}`;
        const cached = await this.redis.get(key);
        if (cached) return JSON.parse(cached);

        const data = await this.repo.findById(id);
        if (data) {
            await this.redis.set(key, JSON.stringify(data), { EX: 300 });
        }
        return data;
    }

    async save(id, data) {
        // Write to DB first
        const result = await this.repo.save(id, data);

        // Then update cache (not just invalidate)
        await this.redis.set(`entity:${id}`, JSON.stringify(result), { EX: 300 });

        return result;
    }

    async delete(id) {
        await this.repo.delete(id);
        await this.redis.del(`entity:${id}`);
    }
}
```

## Redis Data Structures for Caching

```javascript
class RedisCacheStructures {
    constructor(redis) {
        this.redis = redis;
    }

    // String cache
    async cacheString(key, value, ttl = 300) {
        await this.redis.set(key, JSON.stringify(value), { EX: ttl });
    }

    // Hash for object caching
    async cacheObject(key, object, ttl = 300) {
        const flat = {};
        for (const [k, v] of Object.entries(object)) {
            flat[k] = typeof v === 'string' ? v : JSON.stringify(v);
        }
        await this.redis.hSet(key, flat);
        await this.redis.expire(key, ttl);
    }

    async getCachedObject(key) {
        return this.redis.hGetAll(key);
    }

    // Sorted set for leaderboards / ranked cache
    async cacheRanked(key, items, ttl = 300) {
        const args = items.flatMap(item => [item.score, item.member]);
        await this.redis.zAdd(key, args);
        await this.redis.expire(key, ttl);
    }

    async getTopN(key, n = 10) {
        return this.redis.zRangeWithScores(key, 0, n - 1, { REV: true });
    }

    // Set for tag-based caching
    async tagCache(tag, key) {
        await this.redis.sAdd(`tag:${tag}`, key);
    }

    async invalidateByTag(tag) {
        const keys = await this.redis.sMembers(`tag:${tag}`);
        if (keys.length > 0) await this.redis.del(keys);
        await this.redis.del(`tag:${tag}`);
    }

    // List for queue-based caching
    async cacheAsQueue(key, items, maxSize = 1000) {
        await this.redis.lPush(key, items.map(i => JSON.stringify(i)));
        await this.redis.lTrim(key, 0, maxSize - 1);
    }
}
```

## Rate Limiting with Redis

```javascript
class RedisRateLimiter {
    constructor(redis) {
        this.redis = redis;
    }

    // Fixed window
    async fixedWindow(key, limit, windowSec) {
        const current = await this.redis.incr(key);
        if (current === 1) await this.redis.expire(key, windowSec);
        return { allowed: current <= limit, remaining: Math.max(0, limit - current), current };
    }

    // Sliding window
    async slidingWindow(key, limit, windowSec) {
        const now = Date.now();
        const windowStart = now - windowSec * 1000;

        const multi = this.redis.multi();
        multi.zRemRangeByScore(key, 0, windowStart);
        multi.zAdd(key, { score: now, value: `${now}:${Math.random()}` });
        multi.zCard(key);
        multi.expire(key, windowSec);

        const results = await multi.exec();
        const count = results[2];

        return { allowed: count <= limit, remaining: Math.max(0, limit - count), count };
    }

    // Token bucket
    async tokenBucket(key, capacity, refillRate, refillPeriod = 1) {
        const script = `
            local key = KEYS[1]
            local capacity = tonumber(ARGV[1])
            local refill_rate = tonumber(ARGV[2])
            local refill_period = tonumber(ARGV[3])
            local now = tonumber(ARGV[4])
            
            local bucket = redis.call('HMGET', key, 'tokens', 'last_refill')
            local tokens = tonumber(bucket[1]) or capacity
            local last_refill = tonumber(bucket[2]) or now
            
            local elapsed = now - last_refill
            local new_tokens = math.min(capacity, tokens + (elapsed / 1000 / refill_period) * refill_rate)
            
            if new_tokens >= 1 then
                redis.call('HMSET', key, 'tokens', new_tokens - 1, 'last_refill', now)
                redis.call('EXPIRE', key, 60)
                return {1, math.floor(new_tokens - 1)}
            else
                redis.call('HMSET', key, 'tokens', new_tokens, 'last_refill', now)
                redis.call('EXPIRE', key, 60)
                return {0, 0}
            end
        `;

        const result = await this.redis.eval(script, {
            keys: [key],
            arguments: [String(capacity), String(refillRate), String(refillPeriod), String(Date.now())],
        });

        return { allowed: result[0] === 1, tokens: result[1] };
    }
}
```

## Redis Cluster Configuration

```javascript
import { createCluster } from 'redis';

const cluster = createCluster({
    rootNodes: [
        { url: 'redis://node1:6379' },
        { url: 'redis://node2:6379' },
        { url: 'redis://node3:6379' },
    ],
    defaults: {
        socket: {
            connectTimeout: 5000,
        },
    },
});

await cluster.connect();

// Usage is identical to single-node client
await cluster.set('key', 'value');
const value = await cluster.get('key');
```

## Best Practices Checklist

- [ ] Set TTL for all cached entries
- [ ] Use SCAN instead of KEYS for pattern matching
- [ ] Use pipeline for multiple operations
- [ ] Monitor Redis memory usage
- [ ] Configure maxmemory-policy (allkeys-lru recommended)
- [ ] Use connection pooling (built into redis package)
- [ ] Handle Redis failures gracefully
- [ ] Use hash tags for cluster key distribution

## Cross-References

- See [In-Memory Caching](./01-in-memory-caching.md) for LRU caching
- See [CDN Caching](./03-cdn-caching.md) for CDN strategies
- See [Cache Invalidation](./04-cache-invalidation.md) for invalidation patterns

## Next Steps

Continue to [CDN Caching](./03-cdn-caching.md) for content delivery strategies.
