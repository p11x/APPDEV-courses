# Health Check Endpoints

## 📌 What You'll Learn

- The difference between /health, /ready, and /live endpoints
- How to implement proper health checks for Kubernetes and Docker
- What to include in each type of health check
- How health checks enable load balancer and container orchestration

## 🧠 Concept Explained (Plain English)

Health checks are endpoints that external systems call to determine if your application is working correctly. Think of them as a doctor checking your pulse — quick, simple tests that give a yes/no answer about overall health.

There are three types you'll commonly see:

**Liveness probes** (often `/live` or `/health/live`) answer: "Is the process running?" If this fails, the container is restarted. This is for detecting if your application has crashed or is in an unrecoverable state.

**Readiness probes** (often `/ready` or `/health/ready`) answer: "Is this instance ready to receive traffic?" If this fails, the instance is removed from the load balancer. This is for cases where your app is running but not ready — maybe it's still connecting to databases, warming up caches, or waiting for dependent services.

**Health endpoints** (often `/health` or `/healthz`) combine both concepts or provide a comprehensive status check including dependencies.

Why does this matter? In containerized environments (Docker, Kubernetes), the orchestrator constantly pings these endpoints to decide whether to route traffic to your container or restart it. Without proper health checks, a single failing component could take down your entire service.

The key insight is that these should be **lightweight** — don't make database calls or complex calculations. A health check that takes 5 seconds defeats the purpose.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();

// Simulated dependencies (in real code, these would be actual connections)
const dbConnection = { connected: true };
const redisConnection = { connected: true };
let isStartingUp = true;

// ============================================
// Liveness Probe (/live or /healthz)
// ============================================
// Answers: "Is the process running?"
// Kubernetes uses this to know if container should be restarted
// Should be extremely simple - just check if process is alive
app.get('/health/live', (req, res) => {
  // Simple process check - if this endpoint responds, process is running
  res.status(200).json({ 
    status: 'ok',
    timestamp: new Date().toISOString()
  });
});

// Alternative: /healthz (Kubernetes convention)
app.get('/healthz', (req, res) => {
  res.status(200).send('ok');
});


// ============================================
// Readiness Probe (/ready)
// ============================================
// Answers: "Is this instance ready to receive traffic?"
// Kubernetes uses this to remove unhealthy pods from load balancer
// Check dependencies: databases, caches, external services
app.get('/health/ready', async (req, res) => {
  const checks = {
    db: false,
    redis: false,
    startup: !isStartingUp
  };
  
  try {
    // Check database connection
    // In real code: await db.query('SELECT 1');
    checks.db = dbConnection.connected;
  } catch (error) {
    console.error('DB health check failed:', error.message);
  }
  
  try {
    // Check Redis connection
    // In real code: await redis.ping();
    checks.redis = redisConnection.connected;
  } catch (error) {
    console.error('Redis health check failed:', error.message);
  }
  
  // All critical dependencies must be healthy
  const allHealthy = Object.values(checks).every(v => v === true);
  
  res.status(allHealthy ? 200 : 503).json({
    status: allHealthy ? 'ready' : 'not_ready',
    checks,
    timestamp: new Date().toISOString()
  });
});


// ============================================
// Combined Health Endpoint (/health)
// ============================================
// Comprehensive health status for monitoring tools
app.get('/health', (req, res) => {
  const health = {
    status: 'ok',
    uptime: process.uptime(),
    timestamp: new Date().toISOString(),
    version: process.env.APP_VERSION || '1.0.0',
    environment: process.env.NODE_ENV || 'development'
  };
  
  res.status(200).json(health);
});


// ============================================
// Detailed Health with Component Status
// ============================================
// For more detailed monitoring dashboards
app.get('/health/detailed', async (req, res) => {
  const start = Date.now();
  const components = {};
  
  // Check database
  try {
    const dbStart = Date.now();
    // await db.query('SELECT 1');
    components.database = {
      status: 'healthy',
      latency_ms: Date.now() - dbStart,
      connected: dbConnection.connected
    };
  } catch (error) {
    components.database = {
      status: 'unhealthy',
      error: error.message
    };
  }
  
  // Check Redis
  try {
    const redisStart = Date.now();
    // await redis.ping();
    components.redis = {
      status: 'healthy',
      latency_ms: Date.now() - redisStart,
      connected: redisConnection.connected
    };
  } catch (error) {
    components.redis = {
      status: 'unhealthy',
      error: error.message
    };
  }
  
  // Check memory usage
  const memoryUsage = process.memoryUsage();
  const heapUsedPercent = (memoryUsage.heapUsed / memoryUsage.heapTotal) * 100;
  
  components.memory = {
    status: heapUsedPercent > 90 ? 'degraded' : 'healthy',
    heap_used_mb: Math.round(memoryUsage.heapUsed / 1024 / 1024),
    heap_total_mb: Math.round(memoryUsage.heapTotal / 1024 / 1024),
    heap_used_percent: Math.round(heapUsedPercent)
  };
  
  // Check event loop lag
  const eventLoopLag = Date.now() - start;
  components.event_loop = {
    status: eventLoopLag > 100 ? 'degraded' : 'healthy',
    lag_ms: eventLoopLag
  };
  
  // Overall status
  const allHealthy = Object.values(components).every(
    c => c.status === 'healthy'
  );
  
  res.status(allHealthy ? 200 : 503).json({
    status: allHealthy ? 'healthy' : 'unhealthy',
    components,
    timestamp: new Date().toISOString()
  });
});


// ============================================
// Example API Routes
// ============================================
app.get('/api/users', (req, res) => {
  res.json([{ id: 1, name: 'John' }]);
});

// Simulate startup delay
setTimeout(() => {
  isStartingUp = false;
  console.log('Application ready to serve traffic');
}, 5000);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 18-23 | `/health/live` endpoint | Simple liveness probe — if it responds, process is alive |
| 26-28 | `/healthz` endpoint | Kubernetes convention, returns plain "ok" |
| 36-63 | `/health/ready` endpoint | Readiness probe — checks dependencies |
| 43-45 | Database check | Tests actual DB connectivity |
| 50-52 | Redis check | Tests Redis/cache connectivity |
| 57 | `allHealthy` check | Fails if ANY dependency is down |
| 60-63 | Returns 503 if not ready | Tells load balancer to remove from rotation |
| 71-78 | `/health` endpoint | Basic health with uptime and version |
| 85-128 | `/health/detailed` | Component-level status for monitoring dashboards |
| 107-117 | Database check with latency | Measures how long DB query takes |
| 119-129 | Memory check | Reports heap usage percentage |
| 131-135 | Event loop check | Detects if event loop is blocked |

## ⚠️ Common Mistakes

### 1. Making health checks too expensive

**What it is**: Health endpoints that query databases, call external APIs, or do heavy computation.

**Why it happens**: Developers want comprehensive checks but don't realize the impact.

**How to fix it**: Liveness checks should literally just return 200 OK. Readiness checks can do light dependency checks but should have short timeouts.

### 2. Not returning correct HTTP status codes

**What it is**: Returning 200 for unhealthy status, confusing orchestrators.

**Why it happens**: Not understanding how Kubernetes/Docker interprets status codes.

**How to fix it**: Return 200 when healthy, 503 when not ready. Liveness should always return 200 unless the process is crashed.

### 3. Blocking health checks during startup

**What it is**: Readiness probe fails until all initialization is complete, but liveness also fails.

**Why it happens**: Not distinguishing between "starting up" and "crashed".

**How to fix it**: Use a flag to track startup state. Liveness should still pass during startup (process IS running), but readiness should fail until fully initialized.

## ✅ Quick Recap

- **Liveness** (`/live`): Is the process running? Return 200 if yes, no dependency checks
- **Readiness** (`/ready`): Can I receive traffic? Check databases, caches, and dependencies
- **Health** (`/health`): General status with uptime, version, and basic info
- **Detailed** (`/health/detailed`): Component-level status for monitoring dashboards
- Health checks should be fast (< 100ms) and never block
- Return 503 when not ready so load balancers remove the instance from rotation

## 🔗 What's Next

Now that you can monitor health, learn about [Distributed Tracing with OpenTelemetry](./02_distributed-tracing-opentelemetry.md) to trace requests across services.
