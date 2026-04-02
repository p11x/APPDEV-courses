# Read Replicas and Sharding Strategies

## What You'll Learn

- Read replica setup and configuration
- Read/write splitting implementation
- Replication lag handling
- Database sharding strategies
- Shard key selection and routing

## Read Replica Architecture

```
Read Replica Architecture:
─────────────────────────────────────────────
┌──────────────┐
│   App Node   │
│  ┌────────┐  │
│  │ Router │  │
│  └──┬───┬─┘  │
│     │   │    │
└─────┼───┼────┘
      │   │
  ┌───▼───▼───┐
  │  Writes   │◄── Primary (read/write)
  │  Primary  │
  └───┬───┬───┘
      │   │    Replication (async/sync)
  ┌───▼┐ ┌▼───┐
  │Rep1│ │Rep2│◄── Replicas (read-only)
  └────┘ └────┘
```

## Read/Write Splitting Implementation

```javascript
import { Pool } from 'pg';

class DatabaseRouter {
    constructor(config) {
        this.writePool = new Pool({
            host: config.primary.host,
            port: config.primary.port || 5432,
            database: config.database,
            user: config.user,
            password: config.password,
            max: config.writePoolSize || 10,
            application_name: 'nodejs-writer',
        });

        this.readPools = config.replicas.map((replica, idx) =>
            new Pool({
                host: replica.host,
                port: replica.port || 5432,
                database: config.database,
                user: config.user,
                password: config.password,
                max: config.readPoolSize || 20,
                application_name: `nodejs-reader-${idx}`,
            })
        );

        this.readIndex = 0;
        this.healthStatus = new Map();
    }

    getReadPool() {
        // Round-robin with health check
        for (let i = 0; i < this.readPools.length; i++) {
            const idx = (this.readIndex + i) % this.readPools.length;
            if (this.healthStatus.get(idx) !== false) {
                this.readIndex = (idx + 1) % this.readPools.length;
                return this.readPools[idx];
            }
        }
        // Fallback to primary
        console.warn('All replicas unhealthy, falling back to primary');
        return this.writePool;
    }

    async write(sql, params) {
        return this.writePool.query(sql, params);
    }

    async read(sql, params, options = {}) {
        const pool = options.preferPrimary ? this.writePool : this.getReadPool();
        return pool.query(sql, params);
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

    async healthCheck() {
        for (let i = 0; i < this.readPools.length; i++) {
            try {
                await this.readPools[i].query('SELECT 1');
                this.healthStatus.set(i, true);
            } catch {
                this.healthStatus.set(i, false);
                console.error(`Replica ${i} unhealthy`);
            }
        }
    }

    async shutdown() {
        await Promise.all([
            this.writePool.end(),
            ...this.readPools.map(p => p.end()),
        ]);
    }
}

const db = new DatabaseRouter({
    primary: { host: 'primary.db.local' },
    replicas: [
        { host: 'replica1.db.local' },
        { host: 'replica2.db.local' },
    ],
    database: 'myapp',
    user: 'app_user',
    password: process.env.DB_PASSWORD,
});
```

## Replication Lag Handling

```javascript
class LagAwareRouter extends DatabaseRouter {
    constructor(config) {
        super(config);
        this.maxAcceptableLag = config.maxLag || 1000; // ms
    }

    async read(sql, params, options = {}) {
        if (options.requireFresh) {
            // Force read from primary
            return this.writePool.query(sql, params);
        }

        // Check replication lag
        const pool = this.getReadPool();
        const { rows } = await pool.query(
            'SELECT EXTRACT(EPOCH FROM (NOW() - pg_last_xact_replay_timestamp())) * 1000 as lag_ms'
        );

        if (rows[0]?.lag_ms > this.maxAcceptableLag) {
            console.warn(`Replication lag ${rows[0].lag_ms}ms exceeds threshold, using primary`);
            return this.writePool.query(sql, params);
        }

        return pool.query(sql, params);
    }
}

// Session-based consistency
class SessionConsistentRouter extends DatabaseRouter {
    constructor(config) {
        super(config);
        this.sessionLSN = new Map(); // Last LSN seen per session
    }

    async read(sql, params, options = {}) {
        const sessionId = options.sessionId;
        const pool = this.getReadPool();

        if (sessionId && this.sessionLSN.has(sessionId)) {
            const targetLSN = this.sessionLSN.get(sessionId);
            const { rows } = await pool.query('SELECT pg_last_wal_receive_lsn() as lsn');
            
            if (rows[0]?.lsn < targetLSN) {
                // Replica hasn't caught up yet
                return this.writePool.query(sql, params);
            }
        }

        return pool.query(sql, params);
    }

    async write(sql, params, options = {}) {
        const result = await this.writePool.query(sql, params);
        
        if (options.sessionId) {
            const { rows } = await this.writePool.query('SELECT pg_current_wal_lsn() as lsn');
            this.sessionLSN.set(options.sessionId, rows[0].lsn);
        }

        return result;
    }
}
```

## Database Sharding

```
Sharding Strategies:
─────────────────────────────────────────────
Strategy          Description             Use Case
─────────────────────────────────────────────
Hash-based        Hash(shard_key) % N     Even distribution
Range-based       Shard by value range    Time-series, ordered data
Directory-based   Lookup table for shard  Complex routing
Geo-based         Shard by geography      Multi-region apps
```

```javascript
class ShardRouter {
    constructor(shards) {
        this.shards = shards; // Array of { pool, range: [min, max] }
    }

    // Hash-based sharding
    getShardByKey(key) {
        const hash = this.hashKey(key);
        const shardIndex = hash % this.shards.length;
        return this.shards[shardIndex];
    }

    hashKey(key) {
        let hash = 0;
        const str = String(key);
        for (let i = 0; i < str.length; i++) {
            hash = ((hash << 5) - hash) + str.charCodeAt(i);
            hash |= 0;
        }
        return Math.abs(hash);
    }

    // Range-based sharding
    getShardByRange(value) {
        for (const shard of this.shards) {
            if (value >= shard.range[0] && value <= shard.range[1]) {
                return shard;
            }
        }
        throw new Error(`No shard found for value: ${value}`);
    }

    async queryByShardKey(sql, params, shardKey) {
        const shard = this.getShardByKey(shardKey);
        return shard.pool.query(sql, params);
    }

    async queryAll(sql, params) {
        const results = await Promise.all(
            this.shards.map(s => s.pool.query(sql, params))
        );
        return results.flatMap(r => r.rows);
    }

    async broadcast(sql, params) {
        return Promise.all(
            this.shards.map(s => s.pool.query(sql, params))
        );
    }
}

// Usage: Shard by user_id
const shards = [
    { pool: new Pool({ host: 'shard1.db.local', database: 'myapp_shard0' }), id: 0 },
    { pool: new Pool({ host: 'shard2.db.local', database: 'myapp_shard1' }), id: 1 },
    { pool: new Pool({ host: 'shard3.db.local', database: 'myapp_shard2' }), id: 2 },
];

const router = new ShardRouter(shards);

// Query specific shard
const user = await router.queryByShardKey(
    'SELECT * FROM users WHERE id = $1',
    [userId],
    userId
);

// Query all shards (scatter-gather)
const allUsers = await router.queryAll(
    'SELECT COUNT(*) as total FROM users',
    []
);
```

## Shard Key Selection Guide

```
Shard Key Selection Criteria:
─────────────────────────────────────────────
Good shard keys:
├── High cardinality (many unique values)
├── Even distribution across shards
├── Frequently used in queries
├── Immutable (doesn't change)
└── Present in related tables (for co-location)

Bad shard keys:
├── Low cardinality (e.g., status: active/inactive)
├── Sequential values (e.g., auto-increment ID)
├── Frequently changing values
├── Not present in related tables
└── Skewed distribution

Examples:
├── user_id       → Good for user-centric apps
├── tenant_id     → Good for multi-tenant SaaS
├── region_code   → Good for geo-distributed apps
├── created_at    → Good for time-series (range sharding)
└── email         → Bad (uneven distribution)
```

## Best Practices Checklist

- [ ] Use read replicas for read-heavy workloads (80%+ reads)
- [ ] Monitor replication lag and set alert thresholds
- [ ] Implement health checks for all replicas
- [ ] Use connection pooling on all replica connections
- [ ] Choose shard keys with high cardinality and even distribution
- [ ] Implement shard-aware queries (avoid scatter-gather when possible)
- [ ] Plan for shard rebalancing as data grows
- [ ] Test failover scenarios regularly
- [ ] Use synchronous replication for critical data

## Cross-References

- See [Connection Pooling](../01-database-integration-patterns/04-connection-pooling.md) for pool management
- See [Load Balancing](../05-scalability-patterns/01-load-balancing.md) for load balancing
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for replica monitoring

## Next Steps

Continue to [Database Monitoring and Profiling](./06-database-monitoring-profiling.md) for monitoring setup.
