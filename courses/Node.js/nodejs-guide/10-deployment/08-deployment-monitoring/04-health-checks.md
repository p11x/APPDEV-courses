# Health Checks for Deployment

## What You'll Learn

- Liveness vs readiness probes
- Implementing health check endpoints
- Kubernetes health check configuration

## Health Check Endpoint

```js
// health.js

import { createServer } from 'node:http';

app.get('/healthz', (req, res) => {
  res.json({ status: 'healthy', uptime: process.uptime() });
});

app.get('/readyz', async (req, res) => {
  try {
    await db.query('SELECT 1');
    res.json({ status: 'ready' });
  } catch {
    res.status(503).json({ status: 'not ready' });
  }
});
```

## Next Steps

For metrics, continue to [Metrics Collection](./02-metrics-collection.md).
