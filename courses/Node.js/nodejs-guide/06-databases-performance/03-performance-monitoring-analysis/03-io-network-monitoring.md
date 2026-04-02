# I/O and Network Performance Monitoring

## What You'll Learn

- File system I/O monitoring
- Network latency tracking
- Database connection I/O analysis
- DNS resolution monitoring
- External service latency tracking

## I/O Performance Monitor

```javascript
import { performance, PerformanceObserver } from 'node:perf_hooks';
import fs from 'node:fs/promises';

class IOMonitor {
    constructor() {
        this.operations = [];
        this.maxHistory = 10000;
    }

    async trackFileOp(name, fn) {
        const start = performance.now();
        try {
            const result = await fn();
            this.record(name, performance.now() - start, true);
            return result;
        } catch (err) {
            this.record(name, performance.now() - start, false);
            throw err;
        }
    }

    record(operation, duration, success) {
        this.operations.push({
            operation,
            duration: +duration.toFixed(2),
            success,
            timestamp: Date.now(),
        });

        if (this.operations.length > this.maxHistory) {
            this.operations = this.operations.slice(-this.maxHistory / 2);
        }
    }

    getStats(operation) {
        const filtered = operation
            ? this.operations.filter(o => o.operation === operation)
            : this.operations;

        if (filtered.length === 0) return null;

        const durations = filtered.map(o => o.duration).sort((a, b) => a - b);
        const failed = filtered.filter(o => !o.success);

        return {
            count: filtered.length,
            successRate: +(((filtered.length - failed.length) / filtered.length) * 100).toFixed(1),
            duration: {
                avg: +(durations.reduce((a, b) => a + b) / durations.length).toFixed(2),
                median: +durations[Math.floor(durations.length / 2)].toFixed(2),
                p95: +durations[Math.floor(durations.length * 0.95)].toFixed(2),
                p99: +durations[Math.floor(durations.length * 0.99)].toFixed(2),
                max: +durations[durations.length - 1].toFixed(2),
            },
            errorCount: failed.length,
        };
    }

    getOperationBreakdown() {
        const breakdown = {};
        for (const op of this.operations) {
            if (!breakdown[op.operation]) {
                breakdown[op.operation] = { count: 0, totalDuration: 0, errors: 0 };
            }
            breakdown[op.operation].count++;
            breakdown[op.operation].totalDuration += op.duration;
            if (!op.success) breakdown[op.operation].errors++;
        }

        return Object.entries(breakdown).map(([name, stats]) => ({
            operation: name,
            count: stats.count,
            avgDuration: +(stats.totalDuration / stats.count).toFixed(2),
            totalDuration: +stats.totalDuration.toFixed(2),
            errorRate: +((stats.errors / stats.count) * 100).toFixed(1),
        })).sort((a, b) => b.totalDuration - a.totalDuration);
    }
}
```

## Network Latency Tracker

```javascript
import http from 'node:http';
import https from 'node:https';
import { performance } from 'node:perf_hooks';
import dns from 'node:dns/promises';

class NetworkMonitor {
    constructor() {
        this.latencies = [];
        this.dnsCache = new Map();
    }

    async measureDNS(domain) {
        const start = performance.now();
        try {
            const result = await dns.resolve(domain);
            const duration = performance.now() - start;
            this.dnsCache.set(domain, { resolved: result, duration, timestamp: Date.now() });
            return { domain, duration, addresses: result };
        } catch (err) {
            return { domain, duration: performance.now() - start, error: err.message };
        }
    }

    measureHTTP(url, options = {}) {
        return new Promise((resolve) => {
            const start = performance.now();
            const parsedUrl = new URL(url);
            const transport = parsedUrl.protocol === 'https:' ? https : http;

            const req = transport.request(url, { method: 'GET', timeout: 10000, ...options }, (res) => {
                const timings = { dns: 0, connect: 0, tls: 0, firstByte: 0, total: 0 };
                let dataLength = 0;

                res.on('data', (chunk) => {
                    if (!timings.firstByte) {
                        timings.firstByte = performance.now() - start;
                    }
                    dataLength += chunk.length;
                });

                res.on('end', () => {
                    timings.total = performance.now() - start;
                    const result = {
                        url,
                        statusCode: res.statusCode,
                        timings: Object.fromEntries(
                            Object.entries(timings).map(([k, v]) => [k, +v.toFixed(2)])
                        ),
                        contentLength: dataLength,
                    };
                    this.latencies.push(result);
                    resolve(result);
                });
            });

            req.on('error', (err) => {
                resolve({
                    url,
                    error: err.message,
                    total: +(performance.now() - start).toFixed(2),
                });
            });

            req.end();
        });
    }

    getStats() {
        if (this.latencies.length === 0) return null;
        const totals = this.latencies.map(l => l.timings?.total || 0).sort((a, b) => a - b);

        return {
            totalRequests: this.latencies.length,
            avgLatency: +(totals.reduce((a, b) => a + b) / totals.length).toFixed(2),
            p95Latency: +totals[Math.floor(totals.length * 0.95)].toFixed(2),
            p99Latency: +totals[Math.floor(totals.length * 0.99)].toFixed(2),
        };
    }
}
```

## Database Connection Monitor

```javascript
class DatabaseConnectionMonitor {
    constructor(pool) {
        this.pool = pool;
        this.connectionEvents = [];
    }

    setupListeners() {
        this.pool.on('connect', (client) => {
            this.recordEvent('connect', { pid: client.processID });
        });

        this.pool.on('acquire', () => {
            this.recordEvent('acquire');
        });

        this.pool.on('release', () => {
            this.recordEvent('release');
        });

        this.pool.on('remove', () => {
            this.recordEvent('remove');
        });

        this.pool.on('error', (err) => {
            this.recordEvent('error', { message: err.message });
        });
    }

    recordEvent(type, data = {}) {
        this.connectionEvents.push({
            type,
            ...data,
            timestamp: Date.now(),
        });

        if (this.connectionEvents.length > 10000) {
            this.connectionEvents = this.connectionEvents.slice(-5000);
        }
    }

    getStats(sinceMs = 60000) {
        const since = Date.now() - sinceMs;
        const recent = this.connectionEvents.filter(e => e.timestamp > since);

        const counts = {};
        for (const event of recent) {
            counts[event.type] = (counts[event.type] || 0) + 1;
        }

        return {
            period: `${sinceMs / 1000}s`,
            events: counts,
            errorRate: counts.error
                ? +((counts.error / (counts.acquire || 1)) * 100).toFixed(2)
                : 0,
        };
    }
}
```

## Best Practices Checklist

- [ ] Track I/O operation latency and success rates
- [ ] Monitor DNS resolution times
- [ ] Measure external HTTP service latencies
- [ ] Monitor database connection events
- [ ] Set up alerts for high I/O latency
- [ ] Log slow I/O operations for analysis
- [ ] Use connection keep-alive for HTTP clients
- [ ] Cache DNS resolutions appropriately

## Cross-References

- See [Memory/CPU Monitoring](./02-memory-cpu-monitoring.md) for resource monitoring
- See [APM Setup](./01-apm-setup.md) for application monitoring
- See [Connection Pooling](../01-database-integration-patterns/04-connection-pooling.md) for DB connections

## Next Steps

Continue to [Performance Regression Detection](./04-regression-detection.md) for regression analysis.
