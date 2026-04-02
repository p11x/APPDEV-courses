# Performance Monitoring and APM Setup

## What You'll Learn

- Application performance monitoring (APM)
- Memory and CPU monitoring
- Database performance metrics
- Prometheus metrics collection

## Application Performance Monitoring

```javascript
import { monitorEventLoopDelay, eventLoopUtilization } from 'node:perf_hooks';

class PerformanceMonitor {
    constructor() {
        this.histogram = monitorEventLoopDelay({ resolution: 20 });
        this.histogram.enable();
        this.lastELU = eventLoopUtilization();
        this.metrics = [];
    }

    middleware() {
        return (req, res, next) => {
            const start = process.hrtime.bigint();

            res.on('finish', () => {
                const duration = Number(process.hrtime.bigint() - start) / 1e6;
                this.recordMetric({
                    method: req.method,
                    path: req.path,
                    status: res.statusCode,
                    duration,
                    timestamp: Date.now(),
                });
            });

            next();
        };
    }

    recordMetric(metric) {
        this.metrics.push(metric);
        if (this.metrics.length > 10000) this.metrics.shift();
    }

    getMetrics() {
        const elu = eventLoopUtilization(this.lastELU);
        this.lastELU = eventLoopUtilization();

        const durations = this.metrics.map(m => m.duration).sort((a, b) => a - b);

        return {
            eventLoop: {
                lagMeanMs: +(this.histogram.mean / 1e6).toFixed(2),
                lagP99Ms: +(this.histogram.percentile(99) / 1e6).toFixed(2),
                utilization: +(elu.utilization * 100).toFixed(1) + '%',
            },
            requests: {
                count: durations.length,
                p50ms: durations.length ? +durations[Math.floor(durations.length * 0.5)]?.toFixed(2) : 0,
                p95ms: durations.length ? +durations[Math.floor(durations.length * 0.95)]?.toFixed(2) : 0,
                p99ms: durations.length ? +durations[Math.floor(durations.length * 0.99)]?.toFixed(2) : 0,
            },
            memory: {
                rssMB: +(process.memoryUsage().rss / 1024 / 1024).toFixed(1),
                heapUsedMB: +(process.memoryUsage().heapUsed / 1024 / 1024).toFixed(1),
            },
        };
    }
}

const monitor = new PerformanceMonitor();
app.use(monitor.middleware());
```

## Prometheus Metrics

```bash
npm install prom-client
```

```javascript
import { collectDefaultMetrics, Counter, Histogram, Gauge } from 'prom-client';

collectDefaultMetrics();

const httpRequestsTotal = new Counter({
    name: 'http_requests_total',
    help: 'Total HTTP requests',
    labelNames: ['method', 'path', 'status'],
});

const httpRequestDuration = new Histogram({
    name: 'http_request_duration_seconds',
    help: 'Request duration in seconds',
    labelNames: ['method', 'path'],
    buckets: [0.01, 0.05, 0.1, 0.5, 1, 5],
});

const activeConnections = new Gauge({
    name: 'active_connections',
    help: 'Active connections',
});

app.use((req, res, next) => {
    const start = process.hrtime.bigint();
    activeConnections.inc();

    res.on('finish', () => {
        const duration = Number(process.hrtime.bigint() - start) / 1e9;
        httpRequestsTotal.inc({ method: req.method, path: req.path, status: res.statusCode });
        httpRequestDuration.observe({ method: req.method, path: req.path }, duration);
        activeConnections.dec();
    });

    next();
});

app.get('/metrics', async (req, res) => {
    res.set('Content-Type', 'text/plain');
    res.send(await require('prom-client').register.metrics());
});
```

## Best Practices Checklist

- [ ] Monitor event loop lag in production
- [ ] Track request duration percentiles (p50, p95, p99)
- [ ] Expose /metrics endpoint for Prometheus
- [ ] Set alert thresholds for key metrics
- [ ] Use structured logging (JSON format)

## Cross-References

- See [Database Performance](../02-database-performance-optimization/01-query-optimization.md) for queries
- See [Caching](../04-caching-strategies-implementation/01-in-memory-caching.md) for caching
- See [Scalability](../05-scalability-patterns/01-load-balancing.md) for scaling

## Next Steps

Continue to [Caching Strategies](../04-caching-strategies-implementation/01-in-memory-caching.md).
