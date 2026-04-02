# Health Checks

## What You'll Learn

- What health check endpoints are and why they matter
- How to implement liveness and readiness probes
- How to check database and dependency connectivity
- How uptime and memory metrics work
- How Kubernetes and load balancers use health checks

## Why Health Checks?

Load balancers and orchestrators (Kubernetes, Docker Swarm, ECS) need to know if your application is healthy:

- **Liveness** (`/healthz`): Is the process alive? If not, restart it.
- **Readiness** (`/readyz`): Can it handle traffic? If not, remove it from the load balancer.

```
Load Balancer
    │
    ├── GET /healthz → 200 OK → Keep routing traffic
    │
    └── GET /readyz  → 503 → Remove from pool (not ready)
```

## Implementation

```js
// health.js — Health check endpoints with dependency checks

import { createServer } from 'node:http';

// Track application state
const state = {
  startedAt: new Date().toISOString(),
  isShuttingDown: false,
  checks: {
    database: { status: 'unknown', latencyMs: null, lastCheck: null },
    redis: { status: 'unknown', latencyMs: null, lastCheck: null },
  },
};

// Simulated dependency check — replace with real ping
async function checkDatabase() {
  const start = performance.now();
  try {
    // Replace with: await db.query('SELECT 1');
    await new Promise((r) => setTimeout(r, 10));
    state.checks.database = {
      status: 'healthy',
      latencyMs: Math.round(performance.now() - start),
      lastCheck: new Date().toISOString(),
    };
  } catch (err) {
    state.checks.database = {
      status: 'unhealthy',
      error: err.message,
      latencyMs: Math.round(performance.now() - start),
      lastCheck: new Date().toISOString(),
    };
  }
}

async function checkRedis() {
  const start = performance.now();
  try {
    // Replace with: await redis.ping();
    await new Promise((r) => setTimeout(r, 5));
    state.checks.redis = {
      status: 'healthy',
      latencyMs: Math.round(performance.now() - start),
      lastCheck: new Date().toISOString(),
    };
  } catch (err) {
    state.checks.redis = {
      status: 'unhealthy',
      error: err.message,
      latencyMs: Math.round(performance.now() - start),
      lastCheck: new Date().toISOString(),
    };
  }
}

// Run checks every 30 seconds
setInterval(async () => {
  await Promise.all([checkDatabase(), checkRedis()]);
}, 30_000);

// Run initial checks
await Promise.all([checkDatabase(), checkRedis()]);

const server = createServer(async (req, res) => {
  // Liveness probe — is the process alive?
  // This should always return 200 if the process is running
  // If it fails, the orchestrator restarts the process
  if (req.url === '/healthz') {
    if (state.isShuttingDown) {
      res.writeHead(503, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ status: 'shutting_down' }));
      return;
    }

    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({ status: 'alive' }));
    return;
  }

  // Readiness probe — can the app handle traffic?
  // This checks if all dependencies are available
  // If it fails, the load balancer stops sending traffic
  if (req.url === '/readyz') {
    const allHealthy = Object.values(state.checks).every(
      (c) => c.status === 'healthy'
    );

    const isReady = !state.isShuttingDown && allHealthy;

    res.writeHead(isReady ? 200 : 503, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({
      status: isReady ? 'ready' : 'not_ready',
      checks: state.checks,
    }));
    return;
  }

  // Detailed health — full status report
  if (req.url === '/health') {
    const memUsage = process.memoryUsage();

    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({
      status: 'ok',
      uptime: `${Math.round(process.uptime())}s`,
      startedAt: state.startedAt,
      memory: {
        rss: `${(memUsage.rss / 1024 / 1024).toFixed(1)}MB`,
        heapUsed: `${(memUsage.heapUsed / 1024 / 1024).toFixed(1)}MB`,
        heapTotal: `${(memUsage.heapTotal / 1024 / 1024).toFixed(1)}MB`,
        external: `${(memUsage.external / 1024 / 1024).toFixed(1)}MB`,
      },
      nodeVersion: process.version,
      platform: process.platform,
      pid: process.pid,
      checks: state.checks,
    }));
    return;
  }

  res.writeHead(404);
  res.end('Not found');
});

// Graceful shutdown — stop accepting traffic
process.on('SIGTERM', () => {
  state.isShuttingDown = true;
  console.log('SIGTERM received — health checks now return 503');

  // Give the load balancer time to remove us from the pool
  setTimeout(() => {
    server.close(() => {
      console.log('Server closed');
      process.exit(0);
    });
  }, 5000);
});

server.listen(3000, () => {
  console.log('Server on http://localhost:3000');
  console.log('  GET /healthz  — liveness');
  console.log('  GET /readyz   — readiness');
  console.log('  GET /health   — detailed status');
});
```

## How It Works

### Liveness vs Readiness

| Probe | URL | Checks | Failure Action |
|-------|-----|--------|----------------|
| Liveness | `/healthz` | Process alive | Restart the pod |
| Readiness | `/readyz` | Dependencies healthy | Remove from load balancer |

### Kubernetes Configuration

```yaml
# Kubernetes deployment with health checks
apiVersion: apps/v1
kind: Deployment
spec:
  template:
    spec:
      containers:
        - name: app
          livenessProbe:
            httpGet:
              path: /healthz
              port: 3000
            initialDelaySeconds: 10
            periodSeconds: 15
            failureThreshold: 3
          readinessProbe:
            httpGet:
              path: /readyz
              port: 3000
            initialDelaySeconds: 5
            periodSeconds: 10
```

## Common Mistakes

### Mistake 1: Checking Dependencies in Liveness

```js
// WRONG — if DB is down, liveness fails, pod restarts (pointlessly)
app.get('/healthz', async (req, res) => {
  await db.query('SELECT 1');  // If DB is down, this fails
  res.json({ status: 'alive' });
  // Pod restarts, but DB is still down — restart loop!
});

// CORRECT — liveness checks process only, readiness checks dependencies
app.get('/healthz', (req, res) => {
  res.json({ status: 'alive' });  // Always OK if process is running
});
```

### Mistake 2: No Graceful Shutdown Signal

```js
// WRONG — during shutdown, /readyz still returns 200
// Load balancer keeps sending traffic to a dying process

// CORRECT — set isShuttingDown on SIGTERM so /readyz returns 503
process.on('SIGTERM', () => {
  state.isShuttingDown = true;
  // Now /readyz returns 503 — load balancer removes us
});
```

### Mistake 3: Expensive Health Checks

```js
// WRONG — health check runs a full database scan
app.get('/healthz', async (req, res) => {
  const count = await db.query('SELECT COUNT(*) FROM users');  // Slow!
  res.json({ status: 'alive', userCount: count });
});

// CORRECT — lightweight ping only
app.get('/readyz', async (req, res) => {
  await db.query('SELECT 1');  // Fast — just checks connectivity
  res.json({ status: 'ready' });
});
```

## Try It Yourself

### Exercise 1: Test Health Endpoints

Start the server. Hit `/healthz`, `/readyz`, and `/health`. Verify the responses.

### Exercise 2: Simulate Unhealthy Dependency

Modify the database check to always fail. Verify that `/readyz` returns 503 while `/healthz` still returns 200.

### Exercise 3: Graceful Shutdown

Send SIGTERM to the process. Verify that `/readyz` starts returning 503 before the process exits.

## Next Steps

You have monitoring and health checks. For database ORM, continue to [Chapter 22: ORM with Prisma](../../22-orm-prisma/prisma-basics/01-prisma-setup.md).
