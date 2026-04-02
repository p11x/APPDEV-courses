# Database Connection Pooling and Management

## What You'll Learn

- Connection pool architecture and internals
- Pool sizing strategies and formulas
- Pool monitoring and health checks
- Connection lifecycle management
- Pool optimization for different workloads

## Connection Pool Architecture

```
Connection Pool Architecture:
─────────────────────────────────────────────
Application Request
        │
        ▼
┌──────────────────────┐
│   Connection Pool    │
│  ┌────────────────┐  │
│  │  Queue (FIFO)  │  │◄── Waiting requests
│  └────────┬───────┘  │
│           ▼          │
│  ┌────────────────┐  │
│  │  Idle Conns    │  │◄── Ready to use
│  │  [1] [2] [3]  │  │
│  └────────┬───────┘  │
│           ▼          │
│  ┌────────────────┐  │
│  │ Active Conns   │  │◄── In use
│  │  [4] [5]      │  │
│  └────────────────┘  │
└──────────────────────┘
        │
        ▼
   Database Server
```

## PostgreSQL Pool Configuration

```javascript
import { Pool } from 'pg';

const pool = new Pool({
    // Connection settings
    host: process.env.PG_HOST || 'localhost',
    port: parseInt(process.env.PG_PORT || '5432'),
    database: process.env.PG_DATABASE,
    user: process.env.PG_USER,
    password: process.env.PG_PASSWORD,

    // Pool sizing
    max: 20,                      // Maximum pool size
    min: 2,                       // Minimum idle connections

    // Timeouts
    idleTimeoutMillis: 30000,     // Close idle connections after 30s
    connectionTimeoutMillis: 5000, // Wait 5s for connection
    statement_timeout: 30000,     // Query timeout 30s
    query_timeout: 30000,

    // Connection validation
    allowExitOnIdle: true,        // Allow pool to close when idle

    // Application identification
    application_name: 'nodejs-app',
});
```

## MongoDB Pool Configuration

```javascript
import mongoose from 'mongoose';

await mongoose.connect(process.env.MONGODB_URL, {
    // Pool settings
    maxPoolSize: 20,              // Maximum connections
    minPoolSize: 2,               // Minimum connections

    // Timeouts
    serverSelectionTimeoutMS: 5000,
    socketTimeoutMS: 45000,
    connectTimeoutMS: 10000,
    heartbeatFrequencyMS: 10000,

    // Connection management
    maxIdleTimeMS: 30000,
    waitQueueTimeoutMS: 5000,

    // Reliability
    retryWrites: true,
    retryReads: true,

    // Monitoring
    monitorCommands: true,
});
```

## MySQL Pool Configuration

```javascript
import mysql from 'mysql2/promise';

const pool = mysql.createPool({
    host: process.env.MYSQL_HOST,
    user: process.env.MYSQL_USER,
    password: process.env.MYSQL_PASSWORD,
    database: process.env.MYSQL_DATABASE,

    // Pool configuration
    waitForConnections: true,     // Queue requests when pool full
    connectionLimit: 20,          // Max connections
    queueLimit: 0,                // Unlimited queue (0 = no limit)
    maxIdle: 10,                  // Max idle connections
    idleTimeout: 60000,           // Idle timeout (ms)
    acquireTimeout: 60000,        // Max wait for connection
    enableKeepAlive: true,        // Send keep-alive probes
    keepAliveInitialDelay: 0,     // Keep-alive delay (ms)
});
```

## Pool Sizing Formula

```
Pool Size Calculation:
─────────────────────────────────────────────
General formula:
  pool_size = (num_cpus * 2) + effective_spindle_count

For SSD (effective_spindle_count = 1):
  pool_size = (num_cpus * 2) + 1

Workload-based sizing:
─────────────────────────────────────────────
Workload Type          Recommended Pool Size
─────────────────────────────────────────────
CPU-bound operations   num_cpus
I/O-bound operations   num_cpus * 2
Mixed workload         num_cpus * 2 + 1
Read-heavy (80%+)      num_cpus * 3
Write-heavy (80%+)     num_cpus * 2
─────────────────────────────────────────────

Example configurations:
├── 2-core server, read-heavy:  pool.max = 7
├── 4-core server, mixed:       pool.max = 9
├── 8-core server, I/O-heavy:   pool.max = 17
└── 16-core server, read-heavy: pool.max = 49
```

## Pool Monitoring

```javascript
class PoolMonitor {
    constructor(pool, options = {}) {
        this.pool = pool;
        this.interval = options.interval || 30000;
        this.metrics = {
            totalRequests: 0,
            totalWaitTime: 0,
            maxWaitTime: 0,
            timeouts: 0,
            errors: 0,
        };
    }

    start() {
        this.timer = setInterval(() => this.report(), this.interval);
        
        this.pool.on('acquire', () => {
            this.metrics.totalRequests++;
        });

        this.pool.on('error', (err) => {
            this.metrics.errors++;
            console.error('Pool error:', err.message);
        });
    }

    report() {
        const stats = {
            totalConnections: this.pool.totalCount || 0,
            idleConnections: this.pool.idleCount || 0,
            waitingRequests: this.pool.waitingCount || 0,
            ...this.metrics,
        };
        console.log('Pool Stats:', JSON.stringify(stats, null, 2));
    }

    stop() {
        clearInterval(this.timer);
    }
}

// Usage
const monitor = new PoolMonitor(pool, { interval: 30000 });
monitor.start();
```

## Health Checks

```javascript
class PoolHealthCheck {
    constructor(pool, options = {}) {
        this.pool = pool;
        this.checkInterval = options.checkInterval || 10000;
        this.timeout = options.timeout || 5000;
        this.healthy = true;
        this.lastCheck = null;
    }

    async check() {
        try {
            const client = await this.pool.connect();
            try {
                await Promise.race([
                    client.query('SELECT 1'),
                    new Promise((_, reject) =>
                        setTimeout(() => reject(new Error('Health check timeout')), this.timeout)
                    ),
                ]);
                this.healthy = true;
            } finally {
                client.release();
            }
        } catch (err) {
            this.healthy = false;
            console.error('Health check failed:', err.message);
        }
        this.lastCheck = new Date();
        return this.healthy;
    }

    startPeriodicCheck() {
        this.timer = setInterval(() => this.check(), this.checkInterval);
    }

    middleware() {
        return (req, res, next) => {
            if (!this.healthy) {
                return res.status(503).json({
                    error: 'Database unavailable',
                    lastCheck: this.lastCheck,
                });
            }
            next();
        };
    }
}

const healthCheck = new PoolHealthCheck(pool);
healthCheck.startPeriodicCheck();
app.use('/api', healthCheck.middleware());
```

## Connection Retry Strategy

```javascript
class ResilientPool {
    constructor(poolConfig, retryOptions = {}) {
        this.poolConfig = poolConfig;
        this.maxRetries = retryOptions.maxRetries || 3;
        this.retryDelay = retryOptions.retryDelay || 1000;
        this.backoffMultiplier = retryOptions.backoffMultiplier || 2;
        this.pool = null;
    }

    async initialize() {
        let lastError;
        for (let attempt = 1; attempt <= this.maxRetries; attempt++) {
            try {
                this.pool = new Pool(this.poolConfig);
                await this.pool.query('SELECT 1');
                console.log(`Database connected on attempt ${attempt}`);
                return this.pool;
            } catch (err) {
                lastError = err;
                console.error(`Connection attempt ${attempt} failed:`, err.message);
                if (this.pool) {
                    await this.pool.end().catch(() => {});
                }
                const delay = this.retryDelay * Math.pow(this.backoffMultiplier, attempt - 1);
                await new Promise(resolve => setTimeout(resolve, delay));
            }
        }
        throw new Error(`Failed to connect after ${this.maxRetries} attempts: ${lastError.message}`);
    }

    async query(text, params) {
        try {
            return await this.pool.query(text, params);
        } catch (err) {
            if (this.isConnectionError(err)) {
                await this.reconnect();
                return this.pool.query(text, params);
            }
            throw err;
        }
    }

    isConnectionError(err) {
        return ['ECONNREFUSED', 'ECONNRESET', 'PROTOCOL_CONNECTION_LOST'].includes(err.code);
    }

    async reconnect() {
        console.log('Attempting reconnection...');
        await this.pool.end().catch(() => {});
        await this.initialize();
    }
}
```

## Multi-Pool Strategy

```javascript
import { Pool } from 'pg';

class DatabaseManager {
    constructor(config) {
        // Write pool (primary)
        this.writePool = new Pool({
            host: config.primary.host,
            port: config.primary.port,
            database: config.database,
            user: config.user,
            password: config.password,
            max: config.writePoolSize || 10,
        });

        // Read pool(s) (replicas)
        this.readPools = config.replicas.map(replica =>
            new Pool({
                host: replica.host,
                port: replica.port,
                database: config.database,
                user: config.user,
                password: config.password,
                max: config.readPoolSize || 20,
            })
        );

        this.currentReadPool = 0;
    }

    getReadPool() {
        // Round-robin across read replicas
        const pool = this.readPools[this.currentReadPool];
        this.currentReadPool = (this.currentReadPool + 1) % this.readPools.length;
        return pool;
    }

    async write(text, params) {
        return this.writePool.query(text, params);
    }

    async read(text, params) {
        return this.getReadPool().query(text, params);
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

    async shutdown() {
        await Promise.all([
            this.writePool.end(),
            ...this.readPools.map(p => p.end()),
        ]);
    }
}

const db = new DatabaseManager({
    primary: { host: 'primary.db.local', port: 5432 },
    replicas: [
        { host: 'replica1.db.local', port: 5432 },
        { host: 'replica2.db.local', port: 5432 },
    ],
    database: 'myapp',
    user: 'app_user',
    password: process.env.DB_PASSWORD,
});
```

## Pool Configuration Comparison

```
Pool Configuration by Workload:
─────────────────────────────────────────────
                    Web API     Worker     Analytics
─────────────────────────────────────────────
max connections     10-20       5-10       5-15
min idle            2-5         1-2        0-2
idle timeout        30s         60s        120s
conn timeout        5s          10s        30s
query timeout       30s         120s       300s

Key differences:
├── Web API: Fast connections, moderate pool, quick timeouts
├── Worker: Fewer connections, longer timeouts for batch jobs
└── Analytics: Variable pool size, long query timeouts
```

## Best Practices Checklist

- [ ] Size pool based on CPU cores and workload type
- [ ] Set appropriate idle and connection timeouts
- [ ] Monitor pool utilization in production
- [ ] Implement health checks for pool readiness
- [ ] Use connection retry with exponential backoff
- [ ] Separate read/write pools for replica setups
- [ ] Release connections in `finally` blocks
- [ ] Configure `application_name` for monitoring
- [ ] Set up graceful shutdown handlers
- [ ] Log pool events for debugging

## Cross-References

- See [MySQL/MariaDB](./03-mysql-mariadb.md) for MySQL pool specifics
- See [Query Optimization](../02-database-performance-optimization/01-query-optimization.md) for query tuning
- See [Load Balancing](../05-scalability-patterns/01-load-balancing.md) for scaling pools
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for observability

## Next Steps

Continue to [Transaction Management](./05-transaction-management.md) for transaction patterns and strategies.
