# Distributed Caching Implementation

## What You'll Learn

- Distributed cache architecture
- Cache consistency across instances
- Cache stampede prevention
- Cache warming strategies
- Multi-region caching patterns

## Distributed Cache Manager

```javascript
import { createClient } from 'redis';

class DistributedCache {
    constructor(options = {}) {
        this.redis = createClient({ url: options.redisUrl });
        this.localCache = new Map();
        this.localTTL = options.localTTL || 5000; // 5s local cache
        this.lockTimeout = options.lockTimeout || 10000;
    }

    async connect() {
        await this.redis.connect();

        // Subscribe to invalidation events
        this.subscriber = this.redis.duplicate();
        await this.subscriber.connect();
        await this.subscriber.subscribe('cache:invalidate', (message) => {
            const { key } = JSON.parse(message);
            this.localCache.delete(key);
        });
    }

    async get(key, fetcher, options = {}) {
        const ttl = options.ttl || 300;

        // 1. Check local cache
        const local = this.localCache.get(key);
        if (local && local.expires > Date.now()) {
            return local.value;
        }

        // 2. Check Redis
        const cached = await this.redis.get(key);
        if (cached) {
            const data = JSON.parse(cached);
            this.setLocal(key, data, this.localTTL);
            return data;
        }

        // 3. Cache stampede prevention with lock
        const lockKey = `lock:${key}`;
        const lockValue = Math.random().toString(36);

        const acquired = await this.redis.set(lockKey, lockValue, {
            NX: true,
            PX: this.lockTimeout,
        });

        if (!acquired) {
            // Wait and retry
            await new Promise(r => setTimeout(r, 100));
            return this.get(key, fetcher, options);
        }

        try {
            // Double-check after acquiring lock
            const recheck = await this.redis.get(key);
            if (recheck) return JSON.parse(recheck);

            // Fetch from source
            const data = await fetcher();
            await this.redis.set(key, JSON.stringify(data), { EX: ttl });
            this.setLocal(key, data, this.localTTL);
            return data;
        } finally {
            // Release lock (only if we own it)
            const script = `
                if redis.call("GET", KEYS[1]) == ARGV[1] then
                    return redis.call("DEL", KEYS[1])
                end
                return 0
            `;
            await this.redis.eval(script, { keys: [lockKey], arguments: [lockValue] });
        }
    }

    async set(key, value, ttl = 300) {
        await this.redis.set(key, JSON.stringify(value), { EX: ttl });
        this.setLocal(key, value, this.localTTL);

        // Notify other instances
        await this.redis.publish('cache:invalidate', JSON.stringify({ key }));
    }

    async invalidate(key) {
        await this.redis.del(key);
        this.localCache.delete(key);
        await this.redis.publish('cache:invalidate', JSON.stringify({ key }));
    }

    setLocal(key, value, ttl) {
        this.localCache.set(key, { value, expires: Date.now() + ttl });
    }
}
```

## Probabilistic Early Expiration (Cache Stampede Prevention)

```javascript
class ProbabilisticCache {
    constructor(redis) {
        this.redis = redis;
    }

    async get(key, fetcher, ttl = 300, beta = 1) {
        const cached = await this.redis.get(key);

        if (cached) {
            const data = JSON.parse(cached);
            const delta = data.expiresAt - Date.now();

            // Probabilistic early refresh
            // Higher chance of refresh as we approach expiration
            if (delta > 0) {
                const x = (delta / 1000) * beta * Math.log(Math.random());
                if (x > 0) {
                    return data.value;
                }
                // Refresh in background
                this.refreshInBackground(key, fetcher, ttl);
                return data.value;
            }
        }

        // Miss or expired - fetch synchronously
        const value = await fetcher();
        const expiresAt = Date.now() + ttl * 1000;
        await this.redis.set(key, JSON.stringify({ value, expiresAt }), { EX: ttl + 60 });
        return value;
    }

    async refreshInBackground(key, fetcher, ttl) {
        try {
            const value = await fetcher();
            const expiresAt = Date.now() + ttl * 1000;
            await this.redis.set(key, JSON.stringify({ value, expiresAt }), { EX: ttl + 60 });
        } catch (err) {
            // Background refresh failed, stale data still valid
        }
    }
}
```

## Cache Warming

```javascript
class CacheWarmer {
    constructor(redis, pool) {
        this.redis = redis;
        this.pool = pool;
    }

    async warmAll() {
        console.log('Starting cache warming...');
        const start = Date.now();

        await Promise.all([
            this.warmPopularProducts(),
            this.warmActiveUsers(),
            this.warmConfigurations(),
            this.warmAggregations(),
        ]);

        console.log(`Cache warming complete in ${Date.now() - start}ms`);
    }

    async warmPopularProducts() {
        const { rows } = await this.pool.query(
            'SELECT * FROM products WHERE active = true ORDER BY view_count DESC LIMIT 100'
        );

        const pipeline = this.redis.multi();
        for (const product of rows) {
            pipeline.set(`product:${product.id}`, JSON.stringify(product), { EX: 3600 });
        }
        await pipeline.exec();
        console.log(`Warmed ${rows.length} popular products`);
    }

    async warmActiveUsers() {
        const { rows } = await this.pool.query(
            'SELECT id, name, email FROM users WHERE last_login > NOW() - INTERVAL \'7 days\' LIMIT 1000'
        );

        const pipeline = this.redis.multi();
        for (const user of rows) {
            pipeline.set(`user:${user.id}`, JSON.stringify(user), { EX: 1800 });
        }
        await pipeline.exec();
        console.log(`Warmed ${rows.length} active users`);
    }

    async warmConfigurations() {
        const { rows } = await this.pool.query('SELECT * FROM configurations');
        const pipeline = this.redis.multi();
        for (const config of rows) {
            pipeline.set(`config:${config.key}`, config.value, { EX: 86400 });
        }
        await pipeline.exec();
        console.log(`Warmed ${rows.length} configurations`);
    }

    async warmAggregations() {
        const { rows } = await this.pool.query(`
            SELECT DATE(created_at) as date, COUNT(*) as count, SUM(total) as revenue
            FROM orders
            WHERE created_at > NOW() - INTERVAL '30 days'
            GROUP BY DATE(created_at)
        `);

        await this.redis.set('stats:daily_orders', JSON.stringify(rows), { EX: 300 });
        console.log(`Warmed daily order aggregations`);
    }

    // Schedule warming
    scheduleWarming(intervalMs = 300000) { // Every 5 minutes
        this.warmAll();
        this.timer = setInterval(() => this.warmAll(), intervalMs);
    }

    stop() {
        clearInterval(this.timer);
    }
}
```

## Best Practices Checklist

- [ ] Use distributed locks to prevent cache stampede
- [ ] Implement probabilistic early expiration for hot keys
- [ ] Warm critical caches on startup
- [ ] Use pub/sub for cross-instance invalidation
- [ ] Keep local cache TTL short to maintain consistency
- [ ] Monitor cache hit rates across all instances
- [ ] Use Redis Cluster for high availability

## Cross-References

- See [Redis Caching](./02-redis-caching.md) for Redis patterns
- See [Cache Invalidation](./04-cache-invalidation.md) for invalidation strategies
- See [Scalability](../05-scalability-patterns/01-load-balancing.md) for scaling

## Next Steps

Continue to [Cache Warming](./06-cache-warming.md) for preloading strategies.
