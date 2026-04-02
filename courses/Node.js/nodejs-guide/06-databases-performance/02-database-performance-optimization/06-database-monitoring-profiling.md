# Database Performance Monitoring and Profiling

## What You'll Learn

- Query performance profiling
- Slow query analysis
- Database metrics collection
- Resource utilization monitoring
- Performance regression detection

## Query Profiler

```javascript
import { performance } from 'node:perf_hooks';

class QueryProfiler {
    constructor(pool) {
        this.pool = pool;
        this.queries = [];
        this.maxHistory = 10000;
        this.slowThreshold = 1000; // ms
    }

    async profile(sql, params = []) {
        const start = performance.now();
        const queryPlan = { sql, params, startTime: Date.now() };

        try {
            const result = await this.pool.query(sql, params);
            queryPlan.duration = performance.now() - start;
            queryPlan.rows = result.rowCount;
            queryPlan.success = true;
            return result;
        } catch (err) {
            queryPlan.duration = performance.now() - start;
            queryPlan.error = err.message;
            queryPlan.success = false;
            throw err;
        } finally {
            this.record(queryPlan);
        }
    }

    record(plan) {
        this.queries.push(plan);
        if (this.queries.length > this.maxHistory) {
            this.queries = this.queries.slice(-this.maxHistory / 2);
        }

        if (plan.duration > this.slowThreshold) {
            console.warn(`Slow query (${plan.duration.toFixed(1)}ms):`, plan.sql.slice(0, 200));
        }
    }

    getSlowQueries(limit = 50) {
        return [...this.queries]
            .filter(q => q.duration > this.slowThreshold)
            .sort((a, b) => b.duration - a.duration)
            .slice(0, limit);
    }

    getStats() {
        const durations = this.queries.map(q => q.duration).sort((a, b) => a - b);
        const failed = this.queries.filter(q => !q.success);

        return {
            total: this.queries.length,
            successRate: ((this.queries.length - failed.length) / this.queries.length * 100).toFixed(1) + '%',
            duration: {
                avg: +(durations.reduce((a, b) => a + b, 0) / durations.length).toFixed(2),
                median: +durations[Math.floor(durations.length / 2)]?.toFixed(2),
                p95: +durations[Math.floor(durations.length * 0.95)]?.toFixed(2),
                p99: +durations[Math.floor(durations.length * 0.99)]?.toFixed(2),
                max: +durations[durations.length - 1]?.toFixed(2),
            },
            slowCount: this.queries.filter(q => q.duration > this.slowThreshold).length,
            errorCount: failed.length,
        };
    }

    getTopQueries(by = 'duration', limit = 10) {
        const grouped = {};
        for (const q of this.queries) {
            const key = q.sql.replace(/\$\d+/g, '?').replace(/\s+/g, ' ').trim();
            if (!grouped[key]) {
                grouped[key] = { sql: key, count: 0, totalDuration: 0, maxDuration: 0 };
            }
            grouped[key].count++;
            grouped[key].totalDuration += q.duration;
            grouped[key].maxDuration = Math.max(grouped[key].maxDuration, q.duration);
        }

        return Object.values(grouped)
            .sort((a, b) => b[by === 'count' ? 'count' : 'totalDuration'] - a[by === 'count' ? 'count' : 'totalDuration'])
            .slice(0, limit)
            .map(q => ({
                ...q,
                avgDuration: +(q.totalDuration / q.count).toFixed(2),
            }));
    }
}
```

## EXPLAIN ANALYZE Integration

```javascript
async function explainQuery(pool, sql, params = []) {
    const explainResult = await pool.query(
        `EXPLAIN (ANALYZE, BUFFERS, FORMAT JSON) ${sql}`,
        params
    );

    const plan = explainResult.rows[0]['QUERY PLAN'][0];

    return {
        plan: plan.Plan,
        planningTime: plan['Planning Time'],
        executionTime: plan['Execution Time'],
        totalCost: plan.Plan['Total Cost'],
        analysis: analyzePlan(plan.Plan),
    };
}

function analyzePlan(plan) {
    const issues = [];

    if (plan['Node Type'] === 'Seq Scan') {
        issues.push({
            severity: 'warning',
            message: `Sequential scan on "${plan['Relation Name']}" - consider adding an index`,
            rows: plan['Plan Rows'],
        });
    }

    if (plan['Node Type'] === 'Nested Loop' && plan['Plan Rows'] > 1000) {
        issues.push({
            severity: 'warning',
            message: 'Nested loop with high row count - consider using hash join',
        });
    }

    if (plan['Actual Rows'] > plan['Plan Rows'] * 10) {
        issues.push({
            severity: 'critical',
            message: `Actual rows (${plan['Actual Rows']}) much higher than estimated (${plan['Plan Rows']}) - statistics may be stale`,
        });
    }

    if (plan.Plans) {
        for (const subPlan of plan.Plans) {
            issues.push(...analyzePlan(subPlan));
        }
    }

    return issues;
}

// Usage
const analysis = await explainQuery(pool, 'SELECT * FROM users WHERE email = $1', ['test@example.com']);
console.log(JSON.stringify(analysis, null, 2));
```

## Database Metrics Collector

```javascript
class DatabaseMetricsCollector {
    constructor(pool, interval = 30000) {
        this.pool = pool;
        this.interval = interval;
        this.metrics = [];
    }

    async collectPostgresMetrics() {
        const [connections, stats, locks, sizes] = await Promise.all([
            this.pool.query(`
                SELECT 
                    count(*) as total,
                    count(*) FILTER (WHERE state = 'active') as active,
                    count(*) FILTER (WHERE state = 'idle') as idle,
                    count(*) FILTER (WHERE state = 'idle in transaction') as idle_in_transaction
                FROM pg_stat_activity
                WHERE pid != pg_backend_pid()
            `),
            this.pool.query(`
                SELECT 
                    sum(numbackends) as connections,
                    sum(xact_commit) as commits,
                    sum(xact_rollback) as rollbacks,
                    sum(blks_read) as disk_reads,
                    sum(blks_hit) as cache_hits,
                    sum(tup_returned) as rows_returned,
                    sum(tup_fetched) as rows_fetched,
                    sum(tup_inserted) as rows_inserted,
                    sum(tup_updated) as rows_updated,
                    sum(tup_deleted) as rows_deleted
                FROM pg_stat_database
                WHERE datname = current_database()
            `),
            this.pool.query(`
                SELECT mode, count(*) as count
                FROM pg_locks
                GROUP BY mode
                ORDER BY count DESC
            `),
            this.pool.query(`
                SELECT 
                    pg_size_pretty(pg_database_size(current_database())) as db_size,
                    pg_size_pretty(pg_total_relation_size('pg_class')) as total_tables_size
            `),
        ]);

        return {
            timestamp: new Date().toISOString(),
            connections: connections.rows[0],
            stats: stats.rows[0],
            locks: locks.rows,
            sizes: sizes.rows[0],
            cacheHitRate: (
                parseInt(stats.rows[0].cache_hits) /
                (parseInt(stats.rows[0].cache_hits) + parseInt(stats.rows[0].disk_reads)) * 100
            ).toFixed(2) + '%',
        };
    }

    start() {
        this.timer = setInterval(async () => {
            try {
                const metrics = await this.collectPostgresMetrics();
                this.metrics.push(metrics);
                if (this.metrics.length > 1000) this.metrics.shift();
            } catch (err) {
                console.error('Metrics collection failed:', err.message);
            }
        }, this.interval);
    }

    stop() {
        clearInterval(this.timer);
    }

    getLatest() {
        return this.metrics[this.metrics.length - 1] || null;
    }
}
```

## Slow Query Analyzer

```javascript
// PostgreSQL pg_stat_statements integration
async function getSlowQueries(pool, limit = 20) {
    const { rows } = await pool.query(`
        SELECT 
            query,
            calls,
            total_exec_time as total_time,
            mean_exec_time as mean_time,
            max_exec_time as max_time,
            stddev_exec_time as stddev_time,
            rows,
            shared_blks_hit,
            shared_blks_read,
            (shared_blks_hit * 100.0 / NULLIF(shared_blks_hit + shared_blks_read, 0)) as cache_hit_rate
        FROM pg_stat_statements
        WHERE query NOT LIKE '%pg_stat%'
        ORDER BY mean_exec_time DESC
        LIMIT $1
    `, [limit]);

    return rows.map(r => ({
        query: r.query,
        calls: parseInt(r.calls),
        totalTime: +parseFloat(r.total_time).toFixed(2),
        meanTime: +parseFloat(r.mean_time).toFixed(2),
        maxTime: +parseFloat(r.max_time).toFixed(2),
        rows: parseInt(r.rows),
        cacheHitRate: +parseFloat(r.cache_hit_rate || 0).toFixed(2),
    }));
}

// Reset statistics
async function resetQueryStats(pool) {
    await pool.query('SELECT pg_stat_statements_reset()');
}
```

## Performance Regression Detection

```javascript
class RegressionDetector {
    constructor() {
        this.baselines = new Map();
    }

    recordBaseline(queryName, stats) {
        this.baselines.set(queryName, {
            meanTime: stats.meanTime,
            p95Time: stats.p95Time,
            timestamp: Date.now(),
        });
    }

    checkRegression(queryName, currentStats) {
        const baseline = this.baselines.get(queryName);
        if (!baseline) return null;

        const meanIncrease = ((currentStats.meanTime - baseline.meanTime) / baseline.meanTime) * 100;
        const p95Increase = ((currentStats.p95Time - baseline.p95Time) / baseline.p95Time) * 100;

        const result = {
            queryName,
            meanTime: { baseline: baseline.meanTime, current: currentStats.meanTime, change: +meanIncrease.toFixed(1) },
            p95Time: { baseline: baseline.p95Time, current: currentStats.p95Time, change: +p95Increase.toFixed(1) },
            regressed: meanIncrease > 20 || p95Increase > 30,
        };

        if (result.regressed) {
            console.warn(`Performance regression detected for "${queryName}": mean +${meanIncrease.toFixed(1)}%, p95 +${p95Increase.toFixed(1)}%`);
        }

        return result;
    }
}
```

## Best Practices Checklist

- [ ] Profile all queries in development
- [ ] Set slow query thresholds (e.g., 1000ms)
- [ ] Use EXPLAIN ANALYZE for slow queries
- [ ] Collect database metrics at regular intervals
- [ ] Monitor cache hit rates (target > 99%)
- [ ] Track connection pool utilization
- [ ] Set baselines for performance regression
- [ ] Alert on lock contention and deadlocks

## Cross-References

- See [APM Setup](../03-performance-monitoring-analysis/01-apm-setup.md) for application monitoring
- See [Query Optimization](./01-query-optimization.md) for query tuning
- See [Performance Testing](../08-performance-testing-benchmarking/01-load-testing.md) for testing

## Next Steps

Continue to [Performance Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for application-level monitoring.
