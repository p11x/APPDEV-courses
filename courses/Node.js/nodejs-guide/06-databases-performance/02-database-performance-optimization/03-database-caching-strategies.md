# Database Caching Strategies

## What You'll Learn

- Application-level query caching
- Database query cache patterns
- Cache-aside, write-through, and write-behind patterns
- Query result caching with invalidation
- Prepared statement caching

## Cache-Aside Pattern

```javascript
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
await redis.connect();

class CachedRepository {
    constructor(pool, redis, options = {}) {
        this.pool = pool;
        this.redis = redis;
        this.defaultTTL = options.defaultTTL || 300;
        this.prefix = options.prefix || 'db';
    }

    cacheKey(table, id) {
        return `${this.prefix}:${table}:${id}`;
    }

    async findById(table, id) {
        const key = this.cacheKey(table, id);
        
        // Check cache
        const cached = await this.redis.get(key);
        if (cached) return JSON.parse(cached);

        // Query database
        const { rows } = await this.pool.query(
            `SELECT * FROM ${table} WHERE id = $1`,
            [id]
        );
        
        if (rows.length === 0) return null;

        // Store in cache
        await this.redis.set(key, JSON.stringify(rows[0]), {
            EX: this.defaultTTL,
        });

        return rows[0];
    }

    async update(table, id, data) {
        const fields = Object.keys(data);
        const setClause = fields.map((f, i) => `${f} = $${i + 2}`).join(', ');
        const values = [id, ...Object.values(data)];

        const { rows } = await this.pool.query(
            `UPDATE ${table} SET ${setClause} WHERE id = $1 RETURNING *`,
            values
        );

        // Invalidate cache
        await this.redis.del(this.cacheKey(table, id));

        return rows[0];
    }

    async delete(table, id) {
        await this.pool.query(`DELETE FROM ${table} WHERE id = $1`, [id]);
        await this.redis.del(this.cacheKey(table, id));
    }
}
```

## Write-Through Caching

```javascript
class WriteThroughCache {
    constructor(pool, redis) {
        this.pool = pool;
        this.redis = redis;
    }

    async create(table, data) {
        const fields = Object.keys(data);
        const placeholders = fields.map((_, i) => `$${i + 1}`);
        const values = Object.values(data);

        const { rows } = await this.pool.query(
            `INSERT INTO ${table} (${fields.join(', ')}) VALUES (${placeholders.join(', ')}) RETURNING *`,
            values
        );

        // Write to cache immediately
        const key = `db:${table}:${rows[0].id}`;
        await this.redis.set(key, JSON.stringify(rows[0]), { EX: 300 });

        // Invalidate list caches
        await this.invalidatePattern(`db:${table}:list:*`);

        return rows[0];
    }

    async update(table, id, data) {
        const fields = Object.keys(data);
        const setClause = fields.map((f, i) => `${f} = $${i + 2}`).join(', ');
        const values = [id, ...Object.values(data)];

        const { rows } = await this.pool.query(
            `UPDATE ${table} SET ${setClause} WHERE id = $1 RETURNING *`,
            values
        );

        // Update cache immediately (not just invalidate)
        const key = `db:${table}:${id}`;
        await this.redis.set(key, JSON.stringify(rows[0]), { EX: 300 });

        return rows[0];
    }

    async invalidatePattern(pattern) {
        const keys = await this.redis.keys(pattern);
        if (keys.length > 0) await this.redis.del(keys);
    }
}
```

## Query Result Caching

```javascript
class QueryCache {
    constructor(redis) {
        this.redis = redis;
        this.prefix = 'query';
    }

    queryKey(sql, params) {
        const crypto = await import('node:crypto');
        const hash = crypto
            .createHash('md5')
            .update(sql + JSON.stringify(params))
            .digest('hex');
        return `${this.prefix}:${hash}`;
    }

    async execute(pool, sql, params = [], ttl = 300) {
        const key = await this.queryKey(sql, params);

        const cached = await this.redis.get(key);
        if (cached) return JSON.parse(cached);

        const result = await pool.query(sql, params);

        await this.redis.set(key, JSON.stringify(result.rows), { EX: ttl });

        return result.rows;
    }

    async invalidateTable(table) {
        // Pattern-based invalidation
        const keys = await this.redis.keys(`${this.prefix}:*`);
        // In production, use a mapping of tables to cache keys
        if (keys.length > 0) await this.redis.del(keys);
    }
}

// Usage
const queryCache = new QueryCache(redis);

// Cached query
const users = await queryCache.execute(
    pool,
    'SELECT * FROM users WHERE active = $1 ORDER BY created_at DESC LIMIT 20',
    [true],
    60 // 60 second TTL
);

// Cached aggregation
const stats = await queryCache.execute(
    pool,
    `SELECT DATE(created_at) as date, COUNT(*) as count 
     FROM orders 
     WHERE created_at > NOW() - INTERVAL '30 days' 
     GROUP BY DATE(created_at)`,
    [],
    300 // 5 minute TTL
);
```

## Prepared Statement Caching

```javascript
class PreparedStatementCache {
    constructor(pool) {
        this.pool = pool;
        this.statements = new Map();
    }

    async prepare(name, sql) {
        if (!this.statements.has(name)) {
            await this.pool.query(`PREPARE ${name} AS ${sql}`);
            this.statements.set(name, sql);
        }
    }

    async execute(name, params) {
        const placeholders = params.map((_, i) => `$${i + 1}`).join(', ');
        return this.pool.query(`EXECUTE ${name}(${placeholders})`, params);
    }

    async cleanup() {
        for (const name of this.statements.keys()) {
            await this.pool.query(`DEALLOCATE ${name}`).catch(() => {});
        }
        this.statements.clear();
    }
}

// Usage
const prepared = new PreparedStatementCache(pool);
await prepared.prepare('find_user', 'SELECT * FROM users WHERE id = $1');
const user = await prepared.execute('find_user', [userId]);
```

## Cache Invalidation Strategies

```
Cache Invalidation Patterns:
─────────────────────────────────────────────
Pattern          Consistency   Complexity   Use Case
─────────────────────────────────────────────
TTL-based        Eventually    Low          Read-heavy data
Event-based      Strong        Medium       Critical data
Version-based    Strong        Medium       Configuration
Write-through    Strong        High         Financial data
Lazy invalidation Eventually    Low          Analytics

TTL recommendations:
├── User profiles:     300s (5 min)
├── Product catalog:   600s (10 min)
├── Session data:      3600s (1 hour)
├── Aggregation stats: 60s (1 min)
└── Configuration:     30s (30 sec)
```

## Best Practices Checklist

- [ ] Use cache-aside for read-heavy data
- [ ] Use write-through for frequently updated data
- [ ] Set appropriate TTLs based on data freshness needs
- [ ] Implement cache warming for hot data
- [ ] Monitor cache hit rates (target > 90%)
- [ ] Handle cache failures gracefully (degrade to DB)
- [ ] Use version-based invalidation for configuration data
- [ ] Separate cache namespaces by data type

## Cross-References

- See [Caching Strategies](../04-caching-strategies-implementation/01-in-memory-caching.md) for caching architecture
- See [Redis Caching](../04-caching-strategies-implementation/02-redis-caching.md) for Redis patterns
- See [Query Optimization](./01-query-optimization.md) for query tuning

## Next Steps

Continue to [Batch Operations](./04-batch-operations-bulk.md) for bulk data operations.
