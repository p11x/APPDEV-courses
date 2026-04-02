# Stress Testing and Capacity Planning

## What You'll Learn

- Stress testing methodologies
- Capacity planning frameworks
- Breaking point identification
- Resource utilization analysis
- Capacity forecasting

## Stress Test Framework

```javascript
import autocannon from 'autocannon';
import { performance } from 'node:perf_hooks';

class StressTestSuite {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
        this.results = [];
    }

    async runStressTest(endpoint, options = {}) {
        const {
            startConnections = 10,
            maxConnections = 500,
            step = 50,
            duration = 30,
        } = options;

        const results = [];

        for (let connections = startConnections; connections <= maxConnections; connections += step) {
            console.log(`Testing with ${connections} connections...`);

            const result = await autocannon({
                url: `${this.baseUrl}${endpoint}`,
                connections,
                duration,
                pipelining: 1,
            });

            const testResult = {
                connections,
                throughput: result.throughput.average,
                latency: {
                    avg: result.latency.average,
                    p95: result.latency.p95,
                    p99: result.latency.p99,
                },
                errors: result.errors,
                timeouts: result.timeouts,
                requests: result.requests.average,
            };

            results.push(testResult);

            // Stop if error rate > 5%
            if (result.errors / result.requests.total > 0.05) {
                console.log(`Breaking point reached at ${connections} connections`);
                break;
            }
        }

        this.results.push({ endpoint, results });
        return results;
    }

    findBreakingPoint(results) {
        for (let i = 1; i < results.length; i++) {
            const prev = results[i - 1];
            const curr = results[i];

            if (curr.errors > 0 || curr.timeouts > 0 ||
                curr.latency.p99 > prev.latency.p99 * 2) {
                return {
                    connections: prev.connections,
                    throughput: prev.throughput,
                    latency: prev.latency,
                };
            }
        }

        return results[results.length - 1];
    }

    generateReport() {
        return this.results.map(({ endpoint, results }) => ({
            endpoint,
            breakingPoint: this.findBreakingPoint(results),
            summary: {
                maxThroughput: Math.max(...results.map(r => r.throughput)),
                optimalConnections: results.reduce((best, r) =>
                    r.throughput / r.latency.avg > best.throughput / best.latency.avg ? r : best
                ).connections,
            },
            details: results,
        }));
    }
}
```

## Database Stress Testing

```javascript
class DatabaseStressTest {
    constructor(pool) {
        this.pool = pool;
        this.results = [];
    }

    async testConcurrentQueries(sql, params, concurrencyLevels) {
        for (const concurrency of concurrencyLevels) {
            const result = await this.runConcurrent(sql, params, concurrency);
            this.results.push(result);
        }
        return this.results;
    }

    async runConcurrent(sql, params, concurrency, iterations = 100) {
        const timings = [];
        const errors = [];

        const start = performance.now();

        const promises = Array.from({ length: concurrency }, async () => {
            for (let i = 0; i < iterations; i++) {
                const queryStart = performance.now();
                try {
                    await this.pool.query(sql, params);
                    timings.push(performance.now() - queryStart);
                } catch (err) {
                    errors.push(err.message);
                }
            }
        });

        await Promise.all(promises);

        const totalDuration = performance.now() - start;
        timings.sort((a, b) => a - b);

        return {
            concurrency,
            totalQueries: concurrency * iterations,
            totalDuration: +totalDuration.toFixed(2),
            throughput: +((concurrency * iterations) / (totalDuration / 1000)).toFixed(1),
            latency: {
                avg: +(timings.reduce((a, b) => a + b) / timings.length).toFixed(2),
                median: +timings[Math.floor(timings.length / 2)].toFixed(2),
                p95: +timings[Math.floor(timings.length * 0.95)].toFixed(2),
                p99: +timings[Math.floor(timings.length * 0.99)].toFixed(2),
                max: +timings[timings.length - 1].toFixed(2),
            },
            errors: errors.length,
            errorRate: +((errors.length / (concurrency * iterations)) * 100).toFixed(2),
        };
    }
}

// Usage
const dbStress = new DatabaseStressTest(pool);
const results = await dbStress.testConcurrentQueries(
    'SELECT * FROM users WHERE id = $1',
    [1],
    [1, 5, 10, 25, 50, 100]
);
console.table(results);
```

## Capacity Planning Calculator

```javascript
class CapacityPlanner {
    constructor(currentMetrics) {
        this.metrics = currentMetrics;
    }

    estimateCapacity() {
        const {
            avgThroughput,      // requests/sec
            avgLatencyMs,       // ms
            avgCpuPercent,
            avgMemoryMB,
            peakThroughput,
        } = this.metrics;

        // Estimate max throughput based on resource headroom
        const cpuHeadroom = (100 - avgCpuPercent) / avgCpuPercent;
        const memoryHeadroom = (this.metrics.maxMemoryMB - avgMemoryMB) / avgMemoryMB;
        const bottleneck = Math.min(cpuHeadroom, memoryHeadroom);

        const estimatedMaxThroughput = avgThroughput * (1 + bottleneck);

        return {
            current: {
                throughput: avgThroughput,
                latency: avgLatencyMs,
                cpu: avgCpuPercent,
                memory: avgMemoryMB,
            },
            estimated: {
                maxThroughput: +estimatedMaxThroughput.toFixed(0),
                headroomPercent: +(bottleneck * 100).toFixed(1),
            },
            recommendations: this.generateRecommendations(bottleneck),
        };
    }

    generateRecommendations(headroom) {
        const recs = [];

        if (headroom < 0.3) {
            recs.push('Critical: Less than 30% headroom - scale immediately');
            recs.push('Consider horizontal scaling or vertical upgrade');
        } else if (headroom < 0.5) {
            recs.push('Warning: Less than 50% headroom - plan scaling');
        } else {
            recs.push('OK: Sufficient headroom available');
        }

        if (this.metrics.avgCpuPercent > 70) {
            recs.push('CPU utilization high - consider adding cores or optimizing queries');
        }
        if (this.metrics.avgMemoryMB / this.metrics.maxMemoryMB > 0.8) {
            recs.push('Memory utilization high - consider increasing heap or adding RAM');
        }

        return recs;
    }
}
```

## Best Practices Checklist

- [ ] Run stress tests before major deployments
- [ ] Identify breaking points for all critical endpoints
- [ ] Test database under concurrent load
- [ ] Monitor resource utilization during stress tests
- [ ] Establish performance baselines
- [ ] Plan capacity for 2x expected peak load
- [ ] Automate stress testing in CI/CD
- [ ] Document capacity limits and scaling triggers

## Cross-References

- See [Load Testing](./01-load-testing.md) for load testing setup
- See [Regression Detection](../03-performance-monitoring-analysis/04-regression-detection.md) for regression
- See [Scalability](../05-scalability-patterns/01-load-balancing.md) for scaling strategies

## Next Steps

Continue to [Performance Automation](./03-performance-automation.md) for CI/CD integration.
