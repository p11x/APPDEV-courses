# Production Async Performance Monitoring

## What You'll Learn

- Monitoring async operations in production
- Event loop lag tracking
- Async operation metrics
- Alerting on performance degradation

## Event Loop Monitoring

```javascript
import { monitorEventLoopDelay, eventLoopUtilization } from 'node:perf_hooks';

class AsyncMonitor {
    constructor() {
        this.histogram = monitorEventLoopDelay({ resolution: 20 });
        this.histogram.enable();
        this.lastELU = eventLoopUtilization();
    }

    getMetrics() {
        const elu = eventLoopUtilization(this.lastELU);
        this.lastELU = eventLoopUtilization();

        return {
            eventLoop: {
                lagMeanMs: +(this.histogram.mean / 1e6).toFixed(2),
                lagP99Ms: +(this.histogram.percentile(99) / 1e6).toFixed(2),
                utilization: +(elu.utilization * 100).toFixed(1) + '%',
            },
            memory: {
                rssMB: +(process.memoryUsage().rss / 1024 / 1024).toFixed(1),
                heapUsedMB: +(process.memoryUsage().heapUsed / 1024 / 1024).toFixed(1),
            },
        };
    }

    middleware() {
        return (req, res, next) => {
            const start = performance.now();
            res.on('finish', () => {
                const duration = performance.now() - start;
                console.log(`[${req.method}] ${req.path} ${res.statusCode} ${duration.toFixed(1)}ms`);
            });
            next();
        };
    }
}
```

## Best Practices Checklist

- [ ] Monitor event loop lag in production
- [ ] Track async operation durations
- [ ] Set alert thresholds for p95/p99 latency
- [ ] Log async operation context for debugging
- [ ] Use structured logging for metrics

## Cross-References

- See [Memory/CPU Patterns](./01-memory-cpu-patterns.md) for performance
- See [Benchmarking](./02-benchmarking.md) for measurement
- See [Async Debugging](../10-async-debugging/01-async-stack-traces.md) for debugging

## Next Steps

Continue to [Future of Async](../12-future-async/01-upcoming-features.md) for future features.
