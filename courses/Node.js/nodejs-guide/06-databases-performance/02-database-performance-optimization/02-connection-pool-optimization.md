# Connection Pool Tuning and Optimization

## What You'll Learn

- Pool sizing strategies for different workloads
- Connection lifecycle optimization
- Pool monitoring and metrics
- Dynamic pool scaling
- Connection multiplexing patterns

## Pool Sizing Deep Dive

```
Pool Sizing Formula:
─────────────────────────────────────────────
PostgreSQL:
  pool_size = (num_cpus * 2) + effective_spindle_count
  
  SSD:  pool_size = (cores * 2) + 1
  HDD:  pool_size = (cores * 2) + spindles

Workload-specific:
  Read-heavy (90%+ reads):    pool_size = cores * 3
  Write-heavy (90%+ writes):  pool_size = cores * 2
  Mixed workload:             pool_size = cores * 2 + 1
  OLTP (transactional):       pool_size = cores * 2
  OLAP (analytics):           pool_size = cores (fewer, longer)

Practical limits:
  ┌────────────────────────────────────────┐
  │ Database Server    Max Pool Size       │
  ├────────────────────────────────────────┤
  │ 2-core DB server   10-15 connections   │
  │ 4-core DB server   20-30 connections   │
  │ 8-core DB server   40-60 connections   │
  │ 16-core DB server  80-120 connections  │
  └────────────────────────────────────────┘
  
  Rule: Total pool sizes across all app instances 
        should not exceed database max_connections * 0.8
```

## PostgreSQL Pool Optimization

```javascript
import { Pool } from 'pg';

const optimizedPool = new Pool({
    host: process.env.PG_HOST,
    database: process.env.PG_DATABASE,
    user: process.env.PG_USER,
    password: process.env.PG_PASSWORD,

    // Sizing
    max: parseInt(process.env.PG_POOL_MAX || '20'),
    min: parseInt(process.env.PG_POOL_MIN || '2'),

    // Timeouts
    idleTimeoutMillis: 30000,
    connectionTimeoutMillis: 5000,
    statement_timeout: 30000,
    query_timeout: 30000,

    // Connection lifecycle
    allowExitOnIdle: true,
    application_name: `nodejs-${process.env.NODE_ENV || 'development'}`,
});

// Query wrapper with timing
async function optimizedQuery(text, params) {
    const start = performance.now();
    try {
        const result = await pool.query(text, params);
        const duration = performance.now() - start;

        if (duration > 1000) {
            console.warn(`Slow query (${duration.toFixed(1)}ms):`, text.slice(0, 100));
        }

        return result;
    } catch (err) {
        const duration = performance.now() - start;
        console.error(`Query failed (${duration.toFixed(1)}ms):`, err.message);
        throw err;
    }
}
```

## Dynamic Pool Scaling

```javascript
class AdaptivePool {
    constructor(baseConfig) {
        this.baseConfig = baseConfig;
        this.currentMax = baseConfig.max || 10;
        this.minConnections = baseConfig.min || 2;
        this.maxConnections = baseConfig.maxLimit || 50;
        this.scaleUpThreshold = 0.8;   // 80% utilization
        this.scaleDownThreshold = 0.3; // 30% utilization
        this.checkInterval = 30000;
        this.metrics = { activeRequests: 0, totalRequests: 0, waitTime: 0 };
    }

    async initialize() {
        this.pool = new Pool({ ...this.baseConfig, max: this.currentMax });
        this.startMonitoring();
        return this.pool;
    }

    startMonitoring() {
        this.monitorTimer = setInterval(() => this.evaluate(), this.checkInterval);

        this.pool.on('acquire', () => {
            this.metrics.activeRequests++;
            this.metrics.totalRequests++;
        });

        this.pool.on('release', () => {
            this.metrics.activeRequests--;
        });
    }

    evaluate() {
        const utilization = this.metrics.activeRequests / this.currentMax;

        if (utilization > this.scaleUpThreshold && this.currentMax < this.maxConnections) {
            this.scaleUp();
        } else if (utilization < this.scaleDownThreshold && this.currentMax > this.minConnections) {
            this.scaleDown();
        }

        this.metrics = { activeRequests: 0, totalRequests: 0, waitTime: 0 };
    }

    scaleUp() {
        const newSize = Math.min(this.currentMax + 5, this.maxConnections);
        console.log(`Scaling pool: ${this.currentMax} → ${newSize}`);
        this.currentMax = newSize;
        this.recreatePool();
    }

    scaleDown() {
        const newSize = Math.max(this.currentMax - 2, this.minConnections);
        console.log(`Scaling pool: ${this.currentMax} → ${newSize}`);
        this.currentMax = newSize;
        this.recreatePool();
    }

    async recreatePool() {
        const oldPool = this.pool;
        this.pool = new Pool({ ...this.baseConfig, max: this.currentMax });
        this.startMonitoring();
        // Old pool will drain naturally as connections are released
        setTimeout(() => oldPool.end().catch(() => {}), 60000);
    }
}
```

## Connection Reuse Patterns

```javascript
// Request-scoped connection for transactional work
class RequestConnection {
    constructor(pool) {
        this.pool = pool;
        this.connection = null;
    }

    async acquire() {
        if (!this.connection) {
            this.connection = await this.pool.connect();
        }
        return this.connection;
    }

    async query(text, params) {
        const conn = await this.acquire();
        return conn.query(text, params);
    }

    async release() {
        if (this.connection) {
            this.connection.release();
            this.connection = null;
        }
    }
}

// Express middleware for request-scoped connections
function connectionMiddleware(pool) {
    return (req, res, next) => {
        req.db = new RequestConnection(pool);
        res.on('finish', () => req.db.release());
        res.on('close', () => req.db.release());
        next();
    };
}

app.use(connectionMiddleware(pool));

// Use in routes
app.post('/api/orders', async (req, res) => {
    const conn = req.db;
    try {
        await conn.query('BEGIN');
        await conn.query('INSERT INTO orders (user_id) VALUES ($1)', [req.user.id]);
        await conn.query('COMMIT');
        res.json({ success: true });
    } catch (err) {
        await conn.query('ROLLBACK');
        throw err;
    }
    // Connection auto-released by middleware
});
```

## Pool Metrics Dashboard

```javascript
class PoolMetrics {
    constructor(pool) {
        this.pool = pool;
        this.samples = [];
        this.maxSamples = 1000;
    }

    collect() {
        const sample = {
            timestamp: Date.now(),
            totalCount: this.pool.totalCount || 0,
            idleCount: this.pool.idleCount || 0,
            waitingCount: this.pool.waitingCount || 0,
            activeCount: (this.pool.totalCount || 0) - (this.pool.idleCount || 0),
        };
        
        this.samples.push(sample);
        if (this.samples.length > this.maxSamples) {
            this.samples.shift();
        }
        
        return sample;
    }

    getStats() {
        if (this.samples.length === 0) return null;

        const recent = this.samples.slice(-100);
        const avgActive = recent.reduce((s, r) => s + r.activeCount, 0) / recent.length;
        const maxWaiting = Math.max(...recent.map(r => r.waitingCount));

        return {
            current: this.collect(),
            averages: {
                active: +avgActive.toFixed(1),
                utilization: +((avgActive / (this.pool.totalCount || 1)) * 100).toFixed(1) + '%',
            },
            peaks: {
                maxWaiting,
            },
        };
    }

    startAutoCollect(interval = 10000) {
        this.timer = setInterval(() => this.collect(), interval);
    }

    stop() {
        clearInterval(this.timer);
    }
}

// API endpoint for metrics
app.get('/api/metrics/pool', (req, res) => {
    res.json(poolMetrics.getStats());
});
```

## Best Practices Checklist

- [ ] Size pool based on database server capacity and CPU cores
- [ ] Set total pool size across instances < 80% of DB max_connections
- [ ] Monitor pool utilization and adjust dynamically
- [ ] Use request-scoped connections for transactional operations
- [ ] Set appropriate idle and connection timeouts
- [ ] Log slow queries for optimization
- [ ] Expose pool metrics for monitoring dashboards
- [ ] Test pool configuration under production-like load

## Cross-References

- See [Connection Pooling](../01-database-integration-patterns/04-connection-pooling.md) for pooling basics
- See [Query Optimization](./01-query-optimization.md) for query tuning
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for observability

## Next Steps

Continue to [Database Caching Strategies](./03-database-caching-strategies.md) for caching implementation.
