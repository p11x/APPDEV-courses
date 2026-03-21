# Graceful Shutdown

## 📌 What You'll Learn

- Why graceful shutdown matters for production applications
- How to handle SIGTERM and SIGINT signals properly
- How to drain in-flight requests before closing
- Properly closing database connections and cleaning up resources

## 🧠 Concept Explained (Plain English)

When you run an Express app in production (especially in containers), the orchestrator (like Kubernetes, Docker Swarm, or a process manager like PM2) will send signals to your process when it needs to stop or restart. The two most common signals are:

- **SIGTERM**: "Please stop gracefully" — this is how Kubernetes tells a pod to terminate
- **SIGINT**: "Please stop" — this is what happens when you press Ctrl+C in your terminal

If you just call `process.exit()` immediately, bad things happen:
- In-flight requests fail mid-response
- Database connections hang
- Clients get connection errors
- Data might be corrupted

**Graceful shutdown** means:
1. Stop accepting new connections
2. Let existing requests finish (drain in-flight requests)
3. Close database connections
4. Flush logs
5. Exit cleanly

The key concept is **draining** — instead of immediately killing the process, you wait for requests to complete. This is controlled by a **grace period** (usually 30 seconds in Kubernetes) during which:
- Load balancer removes the instance from rotation (via readiness probe)
- Existing requests continue
- No new requests are accepted
- After grace period, remaining requests are forced to close

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import { createServer } from 'http';

const app = express();

// Track in-flight requests
let activeRequests = 0;
let isShuttingDown = false;

// Simulated database connection
const db = {
  connected: true,
  async close() {
    console.log('Closing database connections...');
    await new Promise(resolve => setTimeout(resolve, 500));
    this.connected = false;
    console.log('Database connections closed');
  }
};

// Simulated cache/redis connection  
const redis = {
  connected: true,
  async close() {
    console.log('Closing Redis connection...');
    await new Promise(resolve => setTimeout(resolve, 200));
    this.connected = false;
    console.log('Redis connection closed');
  }
};


// Middleware to track active requests
app.use((req, res, next) => {
  // If we're shutting down, reject new requests
  if (isShuttingDown) {
    res.status(503).setHeader('Connection', 'close');
    return res.json({ error: 'Service is restarting' });
  }
  
  activeRequests++;
  
  // Decrement when request finishes
  res.on('finish', () => {
    activeRequests--;
  });
  
  next();
});


// Example routes
app.get('/api/users', async (req, res) => {
  // Simulate database query
  await new Promise(resolve => setTimeout(resolve, 100));
  res.json([{ id: 1, name: 'John' }]);
});

app.get('/api/products', async (req, res) => {
  await new Promise(resolve => setTimeout(resolve, 50));
  res.json([{ id: 1, name: 'Widget', price: 9.99 }]);
});

app.post('/api/orders', async (req, res) => {
  await new Promise(resolve => setTimeout(resolve, 200));
  res.status(201).json({ id: 'order-123', status: 'created' });
});


// Health check endpoints
app.get('/health/live', (req, res) => {
  res.status(200).json({ status: 'alive' });
});

app.get('/health/ready', (req, res) => {
  if (isShuttingDown) {
    return res.status(503).json({ status: 'not_ready', reason: 'shutting_down' });
  }
  if (!db.connected || !redis.connected) {
    return res.status(503).json({ status: 'not_ready', reason: 'dependencies_unavailable' });
  }
  res.status(200).json({ status: 'ready' });
});


// ============================================
// Graceful Shutdown Handler
// ============================================

// Create HTTP server
const server = createServer(app);

const PORT = process.env.PORT || 3000;
const GRACE_PERIOD = 30000; // 30 seconds

server.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});


// Signal handlers
process.on('SIGTERM', () => {
  console.log('Received SIGTERM - Starting graceful shutdown');
  shutdown('SIGTERM');
});

process.on('SIGINT', () => {
  console.log('Received SIGINT - Starting graceful shutdown');
  shutdown('SIGINT');
});


async function shutdown(signal) {
  const startTime = Date.now();
  
  // 1. Stop accepting new connections
  isShuttingDown = true;
  
  console.log('No longer accepting new connections');
  
  // 2. Update health endpoint to return not ready
  // (handled by middleware and /health/ready check)
  
  // 3. Wait for in-flight requests to complete
  console.log(`Waiting for ${activeRequests} in-flight requests to complete...`);
  
  const maxWaitTime = GRACE_PERIOD - (Date.now() - startTime);
  
  try {
    await waitForRequestsToComplete(maxWaitTime);
    console.log('All in-flight requests completed');
  } catch (error) {
    console.log(`Grace period nearly up, ${activeRequests} requests still active`);
  }
  
  // 4. Close database connections
  try {
    await db.close();
  } catch (error) {
    console.error('Error closing database:', error);
  }
  
  // 5. Close Redis/cache connections
  try {
    await redis.close();
  } catch (error) {
    console.error('Error closing Redis:', error);
  }
  
  // 6. Close HTTP server
  console.log('Closing HTTP server...');
  await new Promise((resolve, reject) => {
    server.close((err) => {
      if (err) reject(err);
      else resolve();
    });
  });
  
  console.log('HTTP server closed');
  
  // 7. Exit process
  console.log(`Graceful shutdown complete in ${Date.now() - startTime}ms`);
  process.exit(0);
}


function waitForRequestsToComplete(maxWaitTime) {
  return new Promise((resolve, reject) => {
    // If no active requests, resolve immediately
    if (activeRequests === 0) {
      return resolve();
    }
    
    // Check every 100ms if requests are complete
    const checkInterval = setInterval(() => {
      if (activeRequests === 0) {
        clearInterval(checkInterval);
        resolve();
      }
    }, 100);
    
    // Timeout after grace period
    setTimeout(() => {
      clearInterval(checkInterval);
      reject(new Error('Grace period expired'));
    }, maxWaitTime);
  });
}


// ============================================
// Unhandled error/rejection handlers
// ============================================

process.on('uncaughtException', (err) => {
  console.error('Uncaught Exception:', err);
  // Force exit after logging
  process.exit(1);
});

process.on('unhandledRejection', (reason, promise) => {
  console.error('Unhandled Rejection at:', promise, 'reason:', reason);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 17-24 | `db` object | Simulated DB with close method |
| 28-34 | `redis` object | Simulated Redis with close method |
| 40-52 | Request tracking middleware | Counts active requests, rejects if shutting down |
| 45-49 | `res.on('finish')` | Decrements counter when response completes |
| 54-67 | Routes | Example endpoints |
| 84-100 | Health endpoints | `/live` always returns 200, `/ready` checks shutdown state |
| 108-113 | Server setup | Creates HTTP server |
| 121-130 | Signal handlers | Catches SIGTERM and SIGINT |
| 132-166 | `shutdown()` function | Orchestrates graceful shutdown |
| 139 | `isShuttingDown = true` | Stops accepting new connections |
| 148-155 | Request draining | Waits for in-flight requests to complete |
| 158-163 | Close DB | Clean database connections |
| 166-170 | Close Redis | Clean cache connections |
| 173-181 | Close server | Stops accepting new connections |
| 187-197 | Helper function | Polls activeRequests until zero or timeout |

## ⚠️ Common Mistakes

### 1. Not updating health checks during shutdown

**What it is**: Load balancer continues sending requests while shutdown is in progress.

**Why it happens**: Forgetting to modify the readiness probe to fail during shutdown.

**How to fix it**: Return 503 from `/health/ready` when `isShuttingDown` is true.

### 2. Waiting forever for requests

**What it is**: Process never exits because requests keep coming or don't complete.

**Why it happens**: Not setting a timeout for the graceful period.

**How to fix it**: Always set a maximum wait time. After timeout, force exit even if requests are pending.

### 3. Forgetting to close connections

**What it is**: Database connections stay open, causing connection leaks.

**Why it happens**: Only handling SIGTERM but not cleaning up resources.

**How to fix it**: Explicitly close all connections (DB, Redis, external services) in the shutdown handler.

## ✅ Quick Recap

- Handle SIGTERM and SIGINT signals to catch termination requests
- Set `isShuttingDown` flag to stop accepting new connections
- Wait for in-flight requests to complete before closing
- Set a grace period timeout to prevent waiting forever
- Close all external connections (databases, caches) on shutdown
- Update health endpoints to reflect shutting down state

## 🔗 What's Next

Now that you can gracefully shut down, learn about [Circuit Breaker Pattern](./02_circuit-breaker-pattern.md) to prevent cascading failures.
