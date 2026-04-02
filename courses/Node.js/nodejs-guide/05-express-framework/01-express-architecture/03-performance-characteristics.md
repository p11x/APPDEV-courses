# Express.js Performance Characteristics and Scaling

## What You'll Learn

- Express performance bottlenecks
- Memory usage patterns
- Scaling strategies
- Benchmarking Express applications

## Performance Benchmarks

```
Express vs Other Frameworks (req/sec):
─────────────────────────────────────────────
Fastify     ████████████████████████████  55,000
Hono        ██████████████████████████████ 58,000
Koa         ██████████████████████        40,000
Express     ████████████████████          35,000
Hapi        ██████████████                25,000

Express overhead breakdown:
├── Middleware chain:  ~0.1ms (10 middleware)
├── Route matching:    ~0.02ms
├── JSON stringify:    ~0.01ms per KB
├── Response headers:  ~0.005ms
└── Total framework:   ~0.15ms per request
```

## Memory Usage Patterns

```javascript
// Monitor Express memory usage
app.use((req, res, next) => {
    const memBefore = process.memoryUsage().heapUsed;
    res.on('finish', () => {
        const memAfter = process.memoryUsage().heapUsed;
        const delta = memAfter - memBefore;
        if (delta > 1024 * 1024) { // > 1MB
            console.warn(`High memory: ${req.method} ${req.path} +${(delta/1024/1024).toFixed(1)}MB`);
        }
    });
    next();
});
```

## Scaling Strategies

```javascript
// Cluster mode for multi-core utilization
import cluster from 'node:cluster';
import os from 'node:os';

if (cluster.isPrimary) {
    for (let i = 0; i < os.cpus().length; i++) {
        cluster.fork();
    }
    cluster.on('exit', (worker) => cluster.fork());
} else {
    app.listen(3000);
}
```

## Best Practices Checklist

- [ ] Use Fastify for high-performance APIs
- [ ] Implement clustering for multi-core
- [ ] Monitor request processing time
- [ ] Use response compression
- [ ] Implement caching for repeated queries

## Cross-References

- See [Lifecycle](./01-lifecycle-deep-dive.md) for request flow
- See [Performance](../06-performance-optimization/01-caching-strategies.md) for optimization
- See [Monitoring](../14-monitoring-observability/01-apm-setup.md) for observability

## Next Steps

Continue to [Advanced Routing](../02-advanced-routing/01-route-parameters.md) for routing.
