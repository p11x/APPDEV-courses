# Discovery Health Checks

## What You'll Learn

- How health checks work with service discovery
- How to implement liveness and readiness probes
- How to configure check intervals
- How to handle flapping services

## Health Check Implementation

```ts
// health.ts

import express from 'express';

const app = express();

// Liveness — is the process alive?
app.get('/healthz', (req, res) => {
  res.json({ status: 'alive' });
});

// Readiness — can the process handle traffic?
app.get('/readyz', async (req, res) => {
  try {
    await db.query('SELECT 1');
    await redis.ping();
    res.json({ status: 'ready' });
  } catch (err) {
    res.status(503).json({ status: 'not ready', error: err.message });
  }
});

app.listen(3000);
```

## Consul Health Check

```hcl
service {
  name = "user-service"
  check {
    http = "http://localhost:3000/readyz"
    interval = "10s"
    timeout = "2s"
    deregister_critical_service_after = "30s"
  }
}
```

## Next Steps

This concludes Chapter 37. Return to the [guide index](../../index.html).
