# Database Clustering and High Availability

## What You'll Learn

- PostgreSQL streaming replication
- Automatic failover patterns
- Connection management with HA
- Health checking and monitoring
- Cluster management strategies

## PostgreSQL Streaming Replication

```
PostgreSQL HA Architecture:
─────────────────────────────────────────────
┌──────────────────────────────┐
│       Application Layer      │
│  ┌────────────────────────┐  │
│  │  Connection Manager    │  │
│  │  (with failover)       │  │
│  └───────────┬────────────┘  │
└──────────────┼───────────────┘
               │
    ┌──────────┼──────────┐
    │          │          │
┌───▼───┐  ┌──▼───┐  ┌───▼───┐
│Primary│  │Rep-1 │  │Rep-2 │
│  R/W  │──│ R/O  │──│ R/O  │
└───────┘  └──────┘  └───────┘
    │          │          │
    └──────────┼──────────┘
               │
        ┌──────▼──────┐
        │   Witness   │
        │   Server    │
        └─────────────┘
```

## HA Connection Manager

```javascript
import { Pool } from 'pg';

class HAPoolManager {
    constructor(config) {
        this.primaryConfig = config.primary;
        this.replicaConfigs = config.replicas;
        this.failoverTimeout = config.failoverTimeout || 5000;
        this.healthCheckInterval = config.healthCheckInterval || 10000;

        this.primaryPool = null;
        this.replicaPools = [];
        this.currentReplicaIndex = 0;
        this.primaryHealthy = false;
        this.replicaHealth = [];
    }

    async initialize() {
        this.primaryPool = await this.createPool(this.primaryConfig, 'primary');
        this.replicaPools = await Promise.all(
            this.replicaConfigs.map((config, i) =>
                this.createPool(config, `replica-${i}`)
            )
        );
        this.replicaHealth = this.replicaPools.map(() => true);

        await this.healthCheck();
        this.startHealthChecks();
    }

    async createPool(config, name) {
        const pool = new Pool({
            host: config.host,
            port: config.port || 5432,
            database: config.database,
            user: config.user,
            password: config.password,
            max: config.max || 20,
            connectionTimeoutMillis: this.failoverTimeout,
            application_name: `nodejs-ha-${name}`,
        });

        pool.on('error', (err) => {
            console.error(`Pool ${name} error:`, err.message);
        });

        return pool;
    }

    async healthCheck() {
        // Check primary
        try {
            await Promise.race([
                this.primaryPool.query('SELECT 1'),
                new Promise((_, reject) =>
                    setTimeout(() => reject(new Error('Timeout')), this.failoverTimeout)
                ),
            ]);
            this.primaryHealthy = true;
        } catch {
            this.primaryHealthy = false;
            console.error('Primary database unhealthy');
        }

        // Check replicas
        await Promise.all(
            this.replicaPools.map(async (pool, i) => {
                try {
                    await pool.query('SELECT 1');
                    this.replicaHealth[i] = true;
                } catch {
                    this.replicaHealth[i] = false;
                    console.error(`Replica ${i} unhealthy`);
                }
            })
        );
    }

    startHealthChecks() {
        this.healthTimer = setInterval(
            () => this.healthCheck(),
            this.healthCheckInterval
        );
    }

    getReplica() {
        for (let i = 0; i < this.replicaPools.length; i++) {
            const idx = (this.currentReplicaIndex + i) % this.replicaPools.length;
            if (this.replicaHealth[idx]) {
                this.currentReplicaIndex = (idx + 1) % this.replicaPools.length;
                return this.replicaPools[idx];
            }
        }
        // Fallback to primary
        return this.primaryPool;
    }

    async write(sql, params) {
        if (!this.primaryHealthy) {
            throw new Error('Primary database unavailable');
        }
        return this.primaryPool.query(sql, params);
    }

    async read(sql, params) {
        return this.getReplica().query(sql, params);
    }

    async transaction(callback) {
        if (!this.primaryHealthy) {
            throw new Error('Primary database unavailable - cannot start transaction');
        }

        const client = await this.primaryPool.connect();
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

    getStatus() {
        return {
            primary: { healthy: this.primaryHealthy },
            replicas: this.replicaPools.map((_, i) => ({
                healthy: this.replicaHealth[i],
            })),
        };
    }

    async shutdown() {
        clearInterval(this.healthTimer);
        await Promise.all([
            this.primaryPool?.end(),
            ...this.replicaPools.map(p => p.end()),
        ]);
    }
}

// Usage
const haPool = new HAPoolManager({
    primary: { host: 'primary.db.local', database: 'myapp', user: 'app', password: process.env.DB_PASS },
    replicas: [
        { host: 'replica1.db.local', database: 'myapp', user: 'app', password: process.env.DB_PASS },
        { host: 'replica2.db.local', database: 'myapp', user: 'app', password: process.env.DB_PASS },
    ],
    failoverTimeout: 5000,
    healthCheckInterval: 10000,
});

await haPool.initialize();

// Health endpoint
app.get('/health/db', (req, res) => {
    res.json(haPool.getStatus());
});
```

## Replication Lag Monitor

```javascript
class ReplicationLagMonitor {
    constructor(haPool) {
        this.haPool = haPool;
        this.maxAcceptableLag = 1000; // ms
    }

    async checkLag(replicaIndex = 0) {
        const replicaPool = this.haPool.replicaPools[replicaIndex];
        if (!replicaPool) return null;

        try {
            const { rows } = await replicaPool.query(`
                SELECT 
                    CASE 
                        WHEN pg_last_wal_receive_lsn() = pg_last_wal_replay_lsn() 
                        THEN 0
                        ELSE EXTRACT(EPOCH FROM now() - pg_last_xact_replay_timestamp()) * 1000
                    END as lag_ms
            `);

            return {
                replica: replicaIndex,
                lagMs: parseFloat(rows[0].lag_ms),
                acceptable: parseFloat(rows[0].lag_ms) <= this.maxAcceptableLag,
            };
        } catch (err) {
            return { replica: replicaIndex, error: err.message };
        }
    }

    async checkAllReplicas() {
        const results = await Promise.all(
            this.haPool.replicaPools.map((_, i) => this.checkLag(i))
        );
        return results;
    }

    middleware() {
        return async (req, res, next) => {
            if (req.query.requireFresh === 'true') {
                const lag = await this.checkLag(0);
                if (lag && !lag.acceptable) {
                    console.warn(`Replication lag ${lag.lagMs}ms, routing to primary`);
                    req.forcePrimary = true;
                }
            }
            next();
        };
    }
}
```

## Best Practices Checklist

- [ ] Implement health checks for all database nodes
- [ ] Use connection pooling with failover support
- [ ] Monitor replication lag continuously
- [ ] Route reads to replicas only if lag is acceptable
- [ ] Use synchronous replication for critical writes
- [ ] Implement automatic failover (repmgr, Patroni)
- [ ] Test failover scenarios regularly
- [ ] Keep at least 2 replicas in different availability zones

## Cross-References

- See [Read Replicas](../02-database-performance-optimization/05-read-replicas-sharding.md) for replication
- See [Load Balancing](./01-load-balancing.md) for load balancer setup
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for observability

## Next Steps

Continue to [Read/Write Splitting](./05-read-write-splitting.md) for query routing.
