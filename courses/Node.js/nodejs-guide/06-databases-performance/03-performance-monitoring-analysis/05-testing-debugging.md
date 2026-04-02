# Performance Testing and Debugging

## What You'll Learn

- Performance test frameworks and setup
- Load testing strategies
- Performance profiling with Node.js
- Debugging performance bottlenecks
- Performance optimization workflow

## Performance Test Framework

```javascript
import { performance } from 'node:perf_hooks';
import autocannon from 'autocannon';

class PerformanceTestSuite {
    constructor(config) {
        this.baseUrl = config.baseUrl;
        this.results = [];
    }

    async runLoadTest(endpoint, options = {}) {
        const result = await autocannon({
            url: `${this.baseUrl}${endpoint}`,
            connections: options.connections || 100,
            duration: options.duration || 30,
            pipelining: options.pipelining || 1,
            method: options.method || 'GET',
            headers: options.headers || {},
            body: options.body ? JSON.stringify(options.body) : undefined,
        });

        const testResult = {
            endpoint,
            options,
            summary: {
                requests: {
                    total: result.requests.total,
                    average: result.requests.average,
                    sent: result.requests.sent,
                },
                latency: {
                    average: result.latency.average,
                    p50: result.latency.p50,
                    p95: result.latency.p95,
                    p99: result.latency.p99,
                },
                throughput: {
                    average: result.throughput.average,
                    sent: result.throughput.sent,
                },
                errors: result.errors,
                timeouts: result.timeouts,
                mismatches: result.mismatches,
            },
        };

        this.results.push(testResult);
        return testResult;
    }

    async runDatabaseBenchmark(pool, queries) {
        const results = [];

        for (const { name, sql, params } of queries) {
            const timings = [];
            
            // Warmup
            for (let i = 0; i < 10; i++) {
                await pool.query(sql, params);
            }

            // Benchmark
            for (let i = 0; i < 1000; i++) {
                const start = performance.now();
                await pool.query(sql, params);
                timings.push(performance.now() - start);
            }

            timings.sort((a, b) => a - b);

            results.push({
                query: name,
                iterations: timings.length,
                mean: +(timings.reduce((a, b) => a + b) / timings.length).toFixed(3),
                median: +timings[Math.floor(timings.length / 2)].toFixed(3),
                p95: +timings[Math.floor(timings.length * 0.95)].toFixed(3),
                p99: +timings[Math.floor(timings.length * 0.99)].toFixed(3),
                min: +timings[0].toFixed(3),
                max: +timings[timings.length - 1].toFixed(3),
            });
        }

        return results;
    }

    generateReport() {
        return {
            timestamp: new Date().toISOString(),
            tests: this.results,
        };
    }
}
```

## CPU Profiling

```javascript
import { Session } from 'node:inspector';
import fs from 'node:fs';

class CPUProfiler {
    constructor() {
        this.session = new Session();
        this.session.connect();
    }

    async start() {
        return new Promise((resolve) => {
            this.session.post('Profiler.enable', () => {
                this.session.post('Profiler.start', resolve);
            });
        });
    }

    async stop(outputPath) {
        return new Promise((resolve) => {
            this.session.post('Profiler.stop', (err, { profile }) => {
                if (err) {
                    resolve(null);
                    return;
                }
                fs.writeFileSync(
                    outputPath || 'cpu-profile.json',
                    JSON.stringify(profile)
                );
                resolve(profile);
            });
        });
    }
}

// Usage
const profiler = new CPUProfiler();
await profiler.start();

// Run code to profile
await someExpensiveOperation();

await profiler.stop('profile.json');
console.log('CPU profile saved to profile.json');
// Load in Chrome DevTools for visualization
```

## Heap Snapshot

```javascript
import v8 from 'node:v8';
import fs from 'node:fs';

class HeapProfiler {
    static takeSnapshot(filename) {
        const snapshotStream = v8.writeHeapSnapshot(filename);
        console.log(`Heap snapshot written to: ${snapshotStream}`);
        return snapshotStream;
    }

    static getHeapStatistics() {
        return v8.getHeapStatistics();
    }

    static getHeapSpaceStatistics() {
        return v8.getHeapSpaceStatistics();
    }

    static async analyzeHeap() {
        const stats = v8.getHeapStatistics();
        const spaces = v8.getHeapSpaceStatistics();

        return {
            heap: {
                totalMB: +(stats.total_heap_size / 1024 / 1024).toFixed(1),
                usedMB: +(stats.used_heap_size / 1024 / 1024).toFixed(1),
                limitMB: +(stats.heap_size_limit / 1024 / 1024).toFixed(1),
                utilization: +((stats.used_heap_size / stats.total_heap_size) * 100).toFixed(1),
            },
            spaces: spaces.map(s => ({
                name: s.space_name,
                sizeMB: +(s.space_size / 1024 / 1024).toFixed(1),
                usedMB: +(s.space_used_size / 1024 / 1024).toFixed(1),
                availableMB: +(s.space_available_size / 1024 / 1024).toFixed(1),
            })),
        };
    }
}
```

## Performance Debugging Checklist

```javascript
class PerformanceDebugger {
    constructor(app) {
        this.app = app;
        this.findings = [];
    }

    async runDiagnostics() {
        await this.checkEventLoop();
        await this.checkMemory();
        await this.checkConnections();
        await this.checkHandles();

        return {
            findings: this.findings,
            severity: this.findings.some(f => f.severity === 'critical') ? 'critical'
                : this.findings.some(f => f.severity === 'warning') ? 'warning'
                : 'ok',
        };
    }

    checkEventLoop() {
        const { monitorEventLoopDelay } = require('node:perf_hooks');
        const histogram = monitorEventLoopDelay({ resolution: 20 });
        histogram.enable();

        return new Promise((resolve) => {
            setTimeout(() => {
                const p99 = histogram.percentile(99) / 1e6;
                if (p99 > 100) {
                    this.findings.push({
                        severity: 'critical',
                        category: 'eventloop',
                        message: `Event loop p99 lag: ${p99.toFixed(1)}ms`,
                        suggestion: 'Profile with --inspect to find blocking operations',
                    });
                }
                histogram.disable();
                resolve();
            }, 5000);
        });
    }

    checkMemory() {
        const usage = process.memoryUsage();
        const heapUtilization = (usage.heapUsed / usage.heapTotal) * 100;

        if (heapUtilization > 85) {
            this.findings.push({
                severity: 'critical',
                category: 'memory',
                message: `Heap utilization: ${heapUtilization.toFixed(1)}%`,
                suggestion: 'Take heap snapshot and analyze for memory leaks',
            });
        } else if (heapUtilization > 70) {
            this.findings.push({
                severity: 'warning',
                category: 'memory',
                message: `Heap utilization: ${heapUtilization.toFixed(1)}%`,
                suggestion: 'Monitor heap usage trend for potential leaks',
            });
        }
    }

    checkConnections() {
        // Check active handles and requests
        const handles = process._getActiveHandles?.()?.length || 0;
        const requests = process._getActiveRequests?.()?.length || 0;

        if (handles > 100) {
            this.findings.push({
                severity: 'warning',
                category: 'connections',
                message: `Active handles: ${handles}`,
                suggestion: 'Check for unclosed connections, sockets, or timers',
            });
        }
    }

    checkHandles() {
        // Check if there are too many file descriptors
        try {
            const { readlinkSync } = require('node:fs');
            const fdCount = require('node:fs')
                .readdirSync(`/proc/${process.pid}/fd`)
                .length;
            
            if (fdCount > 1000) {
                this.findings.push({
                    severity: 'warning',
                    category: 'handles',
                    message: `Open file descriptors: ${fdCount}`,
                    suggestion: 'Check for resource leaks (unclosed files, sockets)',
                });
            }
        } catch {
            // Not on Linux, skip
        }
    }
}
```

## Best Practices Checklist

- [ ] Run load tests before production deployment
- [ ] Profile CPU with `--inspect` flag for hot paths
- [ ] Take heap snapshots to diagnose memory leaks
- [ ] Monitor event loop lag in production
- [ ] Set performance budgets for key endpoints
- [ ] Automate performance tests in CI/CD
- [ ] Use flame graphs to visualize CPU profiles
- [ ] Test with production-like data volumes

## Cross-References

- See [Load Testing](../08-performance-testing-benchmarking/01-load-testing.md) for load testing
- See [Memory/CPU Monitoring](./02-memory-cpu-monitoring.md) for resource monitoring
- See [Regression Detection](./04-regression-detection.md) for regression analysis

## Next Steps

Continue to [Caching Strategies](../04-caching-strategies-implementation/01-in-memory-caching.md) for caching implementation.
