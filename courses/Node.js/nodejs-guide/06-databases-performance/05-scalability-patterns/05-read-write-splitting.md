# Read/Write Splitting and Connection Scaling

## What You'll Learn

- Automatic read/write query routing
- Connection-level query classification
- Replication-aware routing
- Connection scaling strategies
- Performance benchmarks

## Smart Query Router

```javascript
import { Pool } from 'pg';

class SmartQueryRouter {
    constructor(config) {
        this.writePool = new Pool({
            ...config.primary,
            max: config.writePoolSize || 10,
        });

        this.readPools = config.replicas.map(replica =>
            new Pool({ ...replica, max: config.readPoolSize || 20 })
        );

        this.readIndex = 0;
        this.healthStatus = this.readPools.map(() => true);
    }

    // Classify query as read or write
    classifyQuery(sql) {
        const trimmed = sql.trim().toUpperCase();
        const writeOperations = ['INSERT', 'UPDATE', 'DELETE', 'CREATE', 'ALTER', 'DROP', 'TRUNCATE'];

        // Check for CTEs with writes
        if (trimmed.startsWith('WITH') && /INSERT|UPDATE|DELETE/.test(trimmed)) {
            return 'write';
        }

        if (writeOperations.some(op => trimmed.startsWith(op))) {
            return 'write';
        }

        // SELECT FOR UPDATE is a write-adjacent operation
        if (/FOR\s+UPDATE|FOR\s+NO\s+KEY\s+UPDATE/.test(trimmed)) {
            return 'write';
        }

        return 'read';
    }

    async query(sql, params = [], options = {}) {
        const classification = options.force || this.classifyQuery(sql);

        if (classification === 'write') {
            return this.writePool.query(sql, params);
        }

        return this.getReadPool().query(sql, params);
    }

    getReadPool() {
        for (let i = 0; i < this.readPools.length; i++) {
            const idx = (this.readIndex + i) % this.readPools.length;
            if (this.healthStatus[idx]) {
                this.readIndex = (idx + 1) % this.readPools.length;
                return this.readPools[idx];
            }
        }
        return this.writePool;
    }

    async transaction(callback) {
        const client = await this.writePool.connect();
        try {
            await client.query('BEGIN');
            const result = await callback(client);
            await client.query('COMMIT');
            return result;
        } catch (err) {
            await client.query('ROLLBACK');
            throw err;
        } finally {
            client.release();
        }
    }
}

// Usage
const router = new SmartQueryRouter({
    primary: { host: 'primary.db.local', database: 'myapp', user: 'app', password: process.env.DB_PASS },
    replicas: [
        { host: 'replica1.db.local', database: 'myapp', user: 'app', password: process.env.DB_PASS },
        { host: 'replica2.db.local', database: 'myapp', user: 'app', password: process.env.DB_PASS },
    ],
});

// Auto-routed
await router.query('SELECT * FROM users WHERE id = $1', [1]);           // → read replica
await router.query('INSERT INTO users (name) VALUES ($1)', ['Alice']);  // → primary
await router.query('SELECT * FROM users WHERE id = $1 FOR UPDATE', [1]); // → primary
```

## Express Middleware Integration

```javascript
function queryRouterMiddleware(router) {
    return (req, res, next) => {
        req.db = {
            query: (sql, params, opts) => router.query(sql, params, opts),
            transaction: (cb) => router.transaction(cb),
            write: (sql, params) => router.query(sql, params, { force: 'write' }),
            read: (sql, params) => router.query(sql, params, { force: 'read' }),
        };
        next();
    };
}

app.use(queryRouterMiddleware(router));

app.get('/api/users', async (req, res) => {
    const { rows } = await req.db.read('SELECT * FROM users LIMIT 20');
    res.json(rows);
});

app.post('/api/users', async (req, res) => {
    const { rows } = await req.db.write(
        'INSERT INTO users (name, email) VALUES ($1, $2) RETURNING *',
        [req.body.name, req.body.email]
    );
    res.status(201).json(rows[0]);
});
```

## Connection Scaling Patterns

```
Connection Scaling Strategies:
─────────────────────────────────────────────
Pattern             Connections  Complexity  Use Case
─────────────────────────────────────────────
Single pool         N            Low         Single DB
Read/write split    N + M        Medium      Read-heavy
Connection proxy    1 per node   Medium      Many app nodes
Multiplexing        Few          High        High connection count
Serverless proxy    Variable     Low         Variable load

Connection proxy options:
├── PgBouncer (PostgreSQL)
├── ProxySQL (MySQL)
├── MySQL Router (MySQL)
└── HAProxy (generic TCP)
```

## Best Practices Checklist

- [ ] Implement automatic query classification
- [ ] Route FOR UPDATE to primary
- [ ] Monitor read/write distribution
- [ ] Use connection proxies for large deployments
- [ ] Set appropriate pool sizes per role
- [ ] Handle replication lag gracefully
- [ ] Test failover routing behavior

## Cross-References

- See [Clustering](./04-database-clustering.md) for HA patterns
- See [Read Replicas](../02-database-performance-optimization/05-read-replicas-sharding.md) for replication
- See [Connection Pooling](../01-database-integration-patterns/04-connection-pooling.md) for pooling

## Next Steps

Continue to [Data Processing](../06-data-processing-transformation/01-streaming-data.md) for data pipelines.
