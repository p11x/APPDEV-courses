# Express.js Monitoring and Observability

## What You'll Learn

- Application performance monitoring
- Structured logging
- Metrics collection
- Distributed tracing

## Structured Logging with Pino

```bash
npm install pino pino-pretty
```

```javascript
import pino from 'pino';

const logger = pino({
    level: process.env.LOG_LEVEL || 'info',
    transport: process.env.NODE_ENV !== 'production'
        ? { target: 'pino-pretty' }
        : undefined,
});

// Request logging middleware
app.use((req, res, next) => {
    const start = Date.now();

    res.on('finish', () => {
        logger.info({
            method: req.method,
            path: req.path,
            statusCode: res.statusCode,
            duration: Date.now() - start,
            requestId: req.id,
        });
    });

    next();
});
```

## Prometheus Metrics

```bash
npm install prom-client
```

```javascript
import { collectDefaultMetrics, Counter, Histogram } from 'prom-client';

collectDefaultMetrics();

const httpRequests = new Counter({
    name: 'http_requests_total',
    help: 'Total HTTP requests',
    labelNames: ['method', 'path', 'status'],
});

const httpDuration = new Histogram({
    name: 'http_request_duration_ms',
    help: 'Request duration in ms',
    labelNames: ['method', 'path'],
    buckets: [10, 50, 100, 200, 500, 1000],
});

app.use((req, res, next) => {
    const start = Date.now();

    res.on('finish', () => {
        httpRequests.inc({ method: req.method, path: req.path, status: res.statusCode });
        httpDuration.observe({ method: req.method, path: req.path }, Date.now() - start);
    });

    next();
});

app.get('/metrics', async (req, res) => {
    res.set('Content-Type', 'text/plain');
    res.send(await require('prom-client').register.metrics());
});
```

## Best Practices Checklist

- [ ] Use structured logging (JSON)
- [ ] Expose /metrics endpoint for Prometheus
- [ ] Monitor event loop lag
- [ ] Track request duration and status codes
- [ ] Implement distributed tracing

## Cross-References

- See [Architecture](../01-express-architecture/03-performance-characteristics.md) for performance
- See [Error Handling](../08-error-handling/01-centralized-errors.md) for error logging
- See [Container](../13-container-orchestration/01-docker-setup.md) for deployment

## Next Steps

Continue to [Enterprise Implementation](../15-enterprise-implementation/01-auth-patterns.md) for enterprise.
