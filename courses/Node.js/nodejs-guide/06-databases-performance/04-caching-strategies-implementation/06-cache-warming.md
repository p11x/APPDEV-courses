# Cache Warming and Preloading Strategies

## What You'll Learn

- Cache warming techniques
- Preloading patterns for hot data
- Scheduled cache refresh
- On-demand cache population
- Cache warming performance optimization

## Cache Warming Manager

```javascript
class CacheWarmingManager {
    constructor(redis, pool, options = {}) {
        this.redis = redis;
        this.pool = pool;
        this.warmupInterval = options.interval || 300000;
        this.batchSize = options.batchSize || 500;
    }

    async warmOnStartup() {
        console.log('Warming caches on startup...');
        const start = Date.now();

        await Promise.allSettled([
            this.warmHotData(),
            this.warmConfigurations(),
            this.warmAggregations(),
            this.warmLookupTables(),
        ]);

        console.log(`Startup cache warming completed in ${Date.now() - start}ms`);
    }

    async warmHotData() {
        // Most accessed data
        const [products, categories, featured] = await Promise.all([
            this.pool.query('SELECT * FROM products WHERE active = true ORDER BY views DESC LIMIT 200'),
            this.pool.query('SELECT * FROM categories WHERE active = true'),
            this.pool.query('SELECT * FROM products WHERE featured = true AND active = true'),
        ]);

        const pipeline = this.redis.multi();

        for (const p of products.rows) {
            pipeline.set(`product:${p.id}`, JSON.stringify(p), { EX: 3600 });
        }
        for (const c of categories.rows) {
            pipeline.set(`category:${c.id}`, JSON.stringify(c), { EX: 7200 });
        }
        pipeline.set('products:featured', JSON.stringify(featured.rows), { EX: 1800 });

        await pipeline.exec();
        console.log(`Warmed ${products.rows.length} products, ${categories.rows.length} categories`);
    }

    async warmConfigurations() {
        const { rows } = await this.pool.query('SELECT * FROM app_config');
        const pipeline = this.redis.multi();

        for (const config of rows) {
            pipeline.set(`config:${config.key}`, config.value, { EX: 86400 });
        }

        await pipeline.exec();
    }

    async warmAggregations() {
        const aggregations = await Promise.all([
            this.pool.query('SELECT COUNT(*) as total FROM users'),
            this.pool.query('SELECT COUNT(*) as total FROM products WHERE active = true'),
            this.pool.query(`
                SELECT DATE(created_at) as date, COUNT(*) as count, SUM(total) as revenue
                FROM orders WHERE created_at > NOW() - INTERVAL '30 days'
                GROUP BY DATE(created_at) ORDER BY date DESC
            `),
            this.pool.query(`
                SELECT category_id, COUNT(*) as product_count
                FROM products WHERE active = true
                GROUP BY category_id
            `),
        ]);

        await this.redis.mSet({
            'stats:total_users': JSON.stringify(aggregations[0].rows[0]),
            'stats:total_products': JSON.stringify(aggregations[1].rows[0]),
            'stats:daily_orders': JSON.stringify(aggregations[2].rows),
            'stats:products_by_category': JSON.stringify(aggregations[3].rows),
        });

        // Set expiration on all
        const pipeline = this.redis.multi();
        pipeline.expire('stats:total_users', 300);
        pipeline.expire('stats:total_products', 300);
        pipeline.expire('stats:daily_orders', 600);
        pipeline.expire('stats:products_by_category', 600);
        await pipeline.exec();
    }

    async warmLookupTables() {
        const tables = ['countries', 'currencies', 'timezones', 'languages'];
        
        for (const table of tables) {
            try {
                const { rows } = await this.pool.query(`SELECT * FROM ${table}`);
                await this.redis.set(`lookup:${table}`, JSON.stringify(rows), { EX: 86400 });
            } catch {
                // Table might not exist
            }
        }
    }

    startScheduledWarming() {
        this.warmOnStartup();
        this.timer = setInterval(() => this.warmOnStartup(), this.warmupInterval);
    }

    stop() {
        clearInterval(this.timer);
    }
}
```

## Incremental Cache Warming

```javascript
class IncrementalCacheWarmer {
    constructor(redis, pool) {
        this.redis = redis;
        this.pool = pool;
    }

    async warmByRange(table, keyPrefix, options = {}) {
        const { idColumn = 'id', batchSize = 1000, columns = '*' } = options;
        
        let lastId = 0;
        let totalWarmed = 0;

        while (true) {
            const { rows } = await this.pool.query(
                `SELECT ${columns} FROM ${table} WHERE ${idColumn} > $1 ORDER BY ${idColumn} LIMIT $2`,
                [lastId, batchSize]
            );

            if (rows.length === 0) break;

            const pipeline = this.redis.multi();
            for (const row of rows) {
                pipeline.set(`${keyPrefix}:${row[idColumn]}`, JSON.stringify(row), { EX: 3600 });
            }
            await pipeline.exec();

            lastId = rows[rows.length - 1][idColumn];
            totalWarmed += rows.length;

            // Yield to event loop between batches
            await new Promise(r => setTimeout(r, 0));
        }

        return totalWarmed;
    }

    async warmByAccessFrequency(table, keyPrefix) {
        // Warm most frequently accessed items first
        const { rows } = await this.pool.query(`
            SELECT t.*, COALESCE(a.access_count, 0) as access_count
            FROM ${table} t
            LEFT JOIN access_log a ON a.entity_type = '${table}' AND a.entity_id = t.id
            WHERE t.active = true
            ORDER BY a.access_count DESC NULLS LAST
            LIMIT 5000
        `);

        const pipeline = this.redis.multi();
        for (const row of rows) {
            // Longer TTL for more frequently accessed items
            const ttl = row.access_count > 100 ? 3600 : row.access_count > 10 ? 1800 : 600;
            pipeline.set(`${keyPrefix}:${row.id}`, JSON.stringify(row), { EX: ttl });
        }
        await pipeline.exec();

        return rows.length;
    }
}
```

## On-Demand Warmup Middleware

```javascript
function warmupMiddleware(cache, warmupFn, options = {}) {
    const ttl = options.ttl || 300;
    const staleTtl = options.staleTtl || 60;

    return async (req, res, next) => {
        const cacheKey = `route:${req.originalUrl}`;
        const cached = await cache.redis.get(cacheKey);

        if (cached) {
            const data = JSON.parse(cached);
            const age = (Date.now() - data._cachedAt) / 1000;

            // If approaching expiration, warm in background
            if (age > ttl - staleTtl) {
                warmupFn(req).catch(() => {});
            }

            res.set('X-Cache', 'HIT');
            res.set('X-Cache-Age', String(Math.floor(age)));
            return res.json(data.payload);
        }

        // Miss - fetch and cache
        const data = await warmupFn(req);
        await cache.redis.set(cacheKey, JSON.stringify({
            payload: data,
            _cachedAt: Date.now(),
        }), { EX: ttl });

        res.set('X-Cache', 'MISS');
        res.json(data);
    };
}

// Usage
app.get('/api/products', warmupMiddleware(cache, async (req) => {
    const { rows } = await pool.query(
        'SELECT * FROM products WHERE active = true ORDER BY created_at DESC LIMIT 50'
    );
    return rows;
}, { ttl: 300, staleTtl: 60 }));
```

## Best Practices Checklist

- [ ] Warm caches on application startup
- [ ] Schedule periodic cache refresh for hot data
- [ ] Use incremental warming for large datasets
- [ ] Warm by access frequency (most popular first)
- [ ] Use pipeline for batch cache operations
- [ ] Monitor warming performance and adjust batch sizes
- [ ] Handle warming failures gracefully

## Cross-References

- See [Distributed Caching](./05-distributed-caching.md) for distributed patterns
- See [Redis Caching](./02-redis-caching.md) for Redis implementation
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for cache metrics

## Next Steps

Continue to [Scalability Patterns](../05-scalability-patterns/01-load-balancing.md) for scaling strategies.
