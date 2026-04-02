# Performance Monitoring and Metrics

## What You'll Learn

- Application metrics collection
- APM integration patterns
- Alerting and thresholds
- Production monitoring setup

## Metrics Collection

```javascript
import { monitorEventLoopDelay, eventLoopUtilization } from 'node:perf_hooks';

class MetricsCollector {
    constructor() {
        this.histogram = monitorEventLoopDelay({ resolution: 20 });
        this.histogram.enable();
        this.lastELU = eventLoopUtilization();
        this.requestDurations = [];
    }

    middleware() {
        return (req, res, next) => {
            const start = process.hrtime.bigint();
            res.on('finish', () => {
                const duration = Number(process.hrtime.bigint() - start) / 1e6;
                this.requestDurations.push(duration);
                if (this.requestDurations.length > 1000) this.requestDurations.shift();
            });
            next();
        };
    }

    getMetrics() {
        const elu = eventLoopUtilization(this.lastELU);
        this.lastELU = eventLoopUtilization();

        const sorted = [...this.requestDurations].sort((a, b) => a - b);

        return {
            eventLoop: {
                lagMeanMs: +(this.histogram.mean / 1e6).toFixed(2),
                lagP99Ms: +(this.histogram.percentile(99) / 1e6).toFixed(2),
                utilization: +(elu.utilization * 100).toFixed(1) + '%',
            },
            requests: {
                count: sorted.length,
                p50ms: sorted.length ? +sorted[Math.floor(sorted.length * 0.5)]?.toFixed(2) : 0,
                p99ms: sorted.length ? +sorted[Math.floor(sorted.length * 0.99)]?.toFixed(2) : 0,
            },
            memory: {
                rssMB: +(process.memoryUsage().rss / 1024 / 1024).toFixed(1),
                heapUsedMB: +(process.memoryUsage().heapUsed / 1024 / 1024).toFixed(1),
            },
        };
    }
}
```

## Prometheus Format

```javascript
app.get('/metrics', (req, res) => {
    const m = collector.getMetrics();
    res.type('text/plain').send(`
# HELP event_loop_lag_ms Event loop lag in milliseconds
# TYPE event_loop_lag_ms gauge
event_loop_lag_ms ${m.eventLoop.lagMeanMs}

# HELP heap_used_mb Heap memory used in MB
# TYPE heap_used_mb gauge
heap_used_mb ${m.memory.heapUsedMB}

# HELP request_duration_p99_ms Request p99 latency in ms
# TYPE request_duration_p99_ms gauge
request_duration_p99_ms ${m.requests.p99ms}
    `.trim());
});
```

## Best Practices Checklist

- [ ] Collect event loop lag, memory, and request metrics
- [ ] Expose /metrics endpoint for Prometheus
- [ ] Set alert thresholds for key metrics
- [ ] Monitor in production with APM tools (Datadog, New Relic)
- [ ] Log structured metrics for aggregation

## Cross-References

- See [CPU/Memory](./01-cpu-memory-optimization.md) for optimization
- See [I/O Optimization](./02-io-database-optimization.md) for I/O
- See [Process Monitoring](../09-process-lifecycle/03-process-monitoring.md) for lifecycle

## Next Steps

Continue to [Caching Strategies](../13-caching-strategies/01-in-memory-caching.md) for caching.
