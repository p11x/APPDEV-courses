# Performance Monitoring

## What You'll Learn

- Monitoring application performance in production
- Custom metrics with Prometheus
- Grafana dashboards

## Prometheus Metrics

```bash
npm install prom-client
```

```js
// metrics.js
import { collectDefaultMetrics, Registry, Histogram, Counter } from 'prom-client';

const register = new Registry();
collectDefaultMetrics({ register });

export const httpRequestDuration = new Histogram({
  name: 'http_request_duration_seconds',
  help: 'Duration of HTTP requests in seconds',
  labelNames: ['method', 'route', 'status'],
  registers: [register],
});

export const httpRequestsTotal = new Counter({
  name: 'http_requests_total',
  help: 'Total number of HTTP requests',
  labelNames: ['method', 'route', 'status'],
  registers: [register],
});

// Metrics endpoint
app.get('/metrics', async (req, res) => {
  res.set('Content-Type', register.contentType);
  res.end(await register.metrics());
});

// Middleware to record metrics
app.use((req, res, next) => {
  const start = process.hrtime.bigint();
  res.on('finish', () => {
    const duration = Number(process.hrtime.bigint() - start) / 1e9;
    const labels = {
      method: req.method,
      route: req.route?.path || req.path,
      status: res.statusCode,
    };
    httpRequestDuration.observe(labels, duration);
    httpRequestsTotal.inc(labels);
  });
  next();
});
```

## Grafana Dashboard

Import the dashboard JSON or create panels for:
- Request rate (requests/sec)
- Response latency (p50, p95, p99)
- Error rate (5xx / total)
- Memory usage
- Event loop lag

## Next Steps

This concludes the capstone project. Return to the [full guide index](../../index.html).
