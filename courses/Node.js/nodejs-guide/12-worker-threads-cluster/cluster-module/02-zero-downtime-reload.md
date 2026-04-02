# Zero-Downtime Reload

## What You'll Learn

- What zero-downtime reload means and why it matters
- How to restart workers one at a time (rolling restart)
- How to use IPC signals to trigger a graceful reload
- How to wait for new workers to be ready before killing old ones
- How to handle in-flight requests during a restart

## The Problem

When you deploy new code to a clustered Node.js server, you need to restart your workers. If you kill all workers at once, every in-flight request is dropped and clients get connection errors. In production, this means lost sales, corrupted data, and unhappy users.

**Zero-downtime reload** solves this by replacing workers one at a time:

1. Start a new worker
2. Wait for it to be ready (listening on the port)
3. Stop an old worker (after it finishes its current requests)
4. Repeat until all old workers are replaced

During this process, at least one worker is always available to handle requests.

```
Time ──────────────────────────────────────────────────→

Old Workers:  [Worker 1]  [Worker 1]  [Worker 1→stopping]  [Worker 2→stopping]
               [Worker 2]  [Worker 2]     [Worker 3]           [Worker 3]
               [Worker 3]  [Worker 3]     [Worker 4]           [Worker 4]
                              [Worker 5]

New Workers:                  [Worker 5]   [Worker 6]           [Worker 7]
                                                        (all old workers gone)
```

## Project Structure

```
02-zero-downtime-reload/
├── server.js        # Primary process with reload logic
└── worker.js        # Worker process with graceful shutdown
```

## The Primary Process with Reload Support

```js
// server.js — Primary process that supports zero-downtime reload

import cluster from 'node:cluster';
import { availableParallelism } from 'node:os';
import { createInterface } from 'node:readline';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

if (cluster.isPrimary) {
  const numWorkers = availableParallelism() || 2;

  console.log(`Primary ${process.pid} starting with ${numWorkers} workers`);
  console.log('Send SIGHUP (or press "r" + Enter) to reload all workers\n');

  // Track which workers are old (scheduled for replacement) vs new
  const oldWorkers = new Set();

  // Fork the initial set of workers
  for (let i = 0; i < numWorkers; i++) {
    cluster.fork();
  }

  // Log when workers come online
  cluster.on('online', (worker) => {
    console.log(`Worker ${worker.process.pid} is online`);
  });

  // Handle worker exits — only replace workers that were NOT intentionally killed
  cluster.on('exit', (worker, code, signal) => {
    const wasIntentional = oldWorkers.has(worker.process.pid);

    if (wasIntentional) {
      // This worker was killed as part of a reload — do not replace it
      oldWorkers.delete(worker.process.pid);
      console.log(`Old worker ${worker.process.pid} exited (replaced)`);
    } else if (code !== 0) {
      // Unexpected crash — replace immediately
      console.log(`Worker ${worker.process.pid} crashed — restarting`);
      cluster.fork();
    }
  });

  // Rolling reload: replace workers one at a time
  async function reloadWorkers() {
    console.log('\n--- Starting rolling reload ---');

    const currentWorkers = Object.values(cluster.workers).filter(Boolean);

    for (const worker of currentWorkers) {
      // Mark this worker as "old" so we know not to restart it after it exits
      oldWorkers.add(worker.process.pid);

      // Send a graceful shutdown signal to the worker via IPC
      // The worker will stop accepting new connections and finish existing ones
      worker.send({ type: 'shutdown' });

      // Wait for the old worker to exit before starting the next replacement
      // This ensures we only replace one worker at a time
      await new Promise((resolve) => {
        worker.on('exit', resolve);
      });

      // Fork a replacement worker and wait for it to be ready
      const newWorker = cluster.fork();

      await new Promise((resolve) => {
        newWorker.on('online', resolve);
      });

      // Small delay to let the new worker fully bind to the port
      await delay(500);

      console.log(`Replaced old worker with new worker ${newWorker.process.pid}`);
    }

    console.log('--- Reload complete ---\n');
  }

  // Trigger reload via SIGHUP signal (kill -HUP <primary_pid>)
  process.on('SIGHUP', () => {
    console.log('Received SIGHUP — reloading workers...');
    reloadWorkers().catch(console.error);
  });

  // Also allow pressing "r" + Enter for convenience during development
  const rl = createInterface({ input: process.stdin, output: process.stdout });
  rl.on('line', (input) => {
    if (input.trim() === 'r') {
      console.log('Reload requested via stdin');
      reloadWorkers().catch(console.error);
    }
  });

  // Graceful shutdown: kill all workers on SIGINT (Ctrl+C)
  process.on('SIGINT', () => {
    console.log('\nShutting down all workers...');
    const shutdownPromises = Object.values(cluster.workers)
      .filter(Boolean)
      .map((worker) => {
        return new Promise((resolve) => {
          worker.on('exit', resolve);
          worker.send({ type: 'shutdown' });

          // Force-kill after 10 seconds if the worker does not exit
          setTimeout(() => {
            worker.kill();
            resolve();
          }, 10_000);
        });
      });

    Promise.all(shutdownPromises).then(() => {
      console.log('All workers shut down');
      process.exit(0);
    });
  });
} else {
  // Worker process — import the server
  await import('./worker.js');
}

// Utility: pause for N milliseconds
function delay(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}
```

## The Worker with Graceful Shutdown

```js
// worker.js — Worker process that handles requests and graceful shutdown

import { createServer } from 'node:http';

const workerId = process.pid;
let isShuttingDown = false;            // Flag: stop accepting new requests
let activeConnections = 0;             // Count of in-flight requests

// Create the HTTP server
const server = createServer((req, res) => {
  // If we are shutting down, reject new requests immediately
  if (isShuttingDown) {
    res.writeHead(503, {
      'Content-Type': 'application/json',
      'Connection': 'close',              // Tell the client to close the connection
    });
    res.end(JSON.stringify({
      error: 'Server is shutting down — retry your request',
    }));
    return;
  }

  activeConnections++;  // Track that we have one more in-flight request

  // Ensure activeConnections is decremented when the response finishes
  res.on('finish', () => {
    activeConnections--;
  });

  // Route: GET / — simple response
  if (req.url === '/') {
    // Simulate a short processing delay (e.g., database query)
    setTimeout(() => {
      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({
        message: 'Hello!',
        worker: workerId,
        timestamp: new Date().toISOString(),
      }));
    }, 100);  // 100ms simulated work
    return;
  }

  // Route: GET /slow — longer response to test in-flight request handling
  if (req.url === '/slow') {
    setTimeout(() => {
      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({
        message: 'Slow response',
        worker: workerId,
        elapsed: '2000ms',
      }));
    }, 2000);  // 2 second simulated work
    return;
  }

  // Default
  res.writeHead(200, { 'Content-Type': 'text/plain' });
  res.end(`Worker ${workerId}`);
});

// Start listening
const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
  console.log(`Worker ${workerId} listening on port ${PORT}`);
});

// Graceful shutdown handler — triggered by IPC message from primary
process.on('message', (msg) => {
  if (msg.type === 'shutdown') {
    console.log(`Worker ${workerId} received shutdown signal`);
    gracefulShutdown();
  }
});

function gracefulShutdown() {
  isShuttingDown = true;  // Stop accepting new requests

  // Also respond to SIGTERM (e.g., from Docker, systemd, or kill command)
  // This function handles both IPC and signal-based shutdown

  // Stop accepting new TCP connections
  // Existing connections with in-flight requests continue until their responses finish
  server.close(() => {
    console.log(`Worker ${workerId}: all connections closed, exiting`);
    process.exit(0);  // Clean exit — primary will not restart us
  });

  // If no requests are active, server.close() fires immediately
  // If requests are active, it waits for them to finish

  // Safety timeout: force exit after 15 seconds regardless
  // Prevents hanging if a connection never closes (e.g., long-polling)
  setTimeout(() => {
    console.log(`Worker ${workerId}: force exiting after timeout`);
    process.exit(1);
  }, 15_000).unref();  // unref() prevents this timer from keeping the process alive
}

// Also handle SIGTERM for external shutdown signals
process.on('SIGTERM', gracefulShutdown);
```

## Running and Testing

### Start the Server

```bash
node server.js
```

### Trigger a Reload

**Option 1:** Press `r` + Enter in the terminal running the primary process.

**Option 2:** Send a SIGHUP signal from another terminal:

```bash
# Find the primary PID from the console output, then:
kill -HUP <primary_pid>
```

### Test During Reload

Open two terminals. In one, send a slow request that takes 2 seconds. In the other, trigger a reload:

```bash
# Terminal 1 — send a slow request
curl http://localhost:3000/slow

# Terminal 2 — trigger reload while the request is in flight
kill -HUP $(pgrep -f "node server.js" | head -1)
```

The slow request completes because the old worker waits for it to finish before exiting.

### Verify Zero Downtime

Run a loop that hits the server continuously and check for any failures:

```bash
# Send 100 requests, count failures
failures=0
for i in $(seq 1 100); do
  status=$(curl -s -o /dev/null -w "%{http_code}" http://localhost:3000/)
  if [ "$status" != "200" ]; then
    failures=$((failures + 1))
  fi
done
echo "Failures: $failures"
```

Expected result: `Failures: 0` (some requests may return 503 during the brief window when a worker is shutting down — this is acceptable in most deployments).

## How It Works

### Rolling Restart Sequence

1. **Primary** receives a reload signal (SIGHUP or `r` key)
2. For each old worker:
   a. Primary sends `{ type: 'shutdown' }` via IPC
   b. Worker sets `isShuttingDown = true` — new requests get 503
   c. Worker calls `server.close()` — stops accepting new TCP connections
   d. Worker waits for in-flight requests to finish (up to 15 seconds)
   e. Worker exits with code 0
   f. Primary sees the exit, checks that this was an intentional shutdown
   g. Primary forks a new worker as a replacement
3. After all old workers are replaced, reload is complete

### Why Wait for Each Worker?

If you kill all workers at once and fork replacements simultaneously, there is a brief window where no worker is available. By replacing one at a time, at least N-1 workers are always serving requests.

### The 503 Response

When a worker is shutting down, it responds with HTTP 503 (Service Unavailable). Load balancers and reverse proxies (like nginx) interpret 503 as "try another backend" and will retry the request on a different worker automatically.

## Common Mistakes

### Mistake 1: Not Waiting for In-Flight Requests

```js
// WRONG — kills workers immediately, dropping active connections
process.on('SIGHUP', () => {
  for (const id in cluster.workers) {
    cluster.workers[id].kill();  // Instant death — no graceful shutdown
  }
  // Fork new workers
});

// CORRECT — send a shutdown message and wait for exit
process.on('SIGHUP', () => {
  for (const id in cluster.workers) {
    cluster.workers[id].send({ type: 'shutdown' });
    // Wait for the worker to call process.exit(0)
  }
});
```

### Mistake 2: Forking All Workers Before Killing Old Ones

```js
// WRONG — briefly have 2x workers, double memory usage
const old = Object.values(cluster.workers);
for (let i = 0; i < numWorkers; i++) {
  cluster.fork();  // Now have 2N workers
}
old.forEach((w) => w.send({ type: 'shutdown' }));

// CORRECT — replace one at a time: kill old → fork new → repeat
for (const worker of old) {
  worker.send({ type: 'shutdown' });
  await new Promise((r) => worker.on('exit', r));
  const newWorker = cluster.fork();
  await new Promise((r) => newWorker.on('online', r));
}
```

### Mistake 3: Not Cleaning Up the Old Workers Set

```js
// WRONG — oldWorkers Set grows forever, memory leak
oldWorkers.add(worker.process.pid);
// Never removed on exit

// CORRECT — delete from the set in the 'exit' handler
cluster.on('exit', (worker) => {
  oldWorkers.delete(worker.process.pid);  // Clean up
});
```

## Try It Yourself

### Exercise 1: Add a Health Check Route

Add a `GET /health` route to `worker.js` that returns `200` when healthy and `503` when `isShuttingDown` is true. This lets load balancers automatically remove shutting-down workers from rotation.

### Exercise 2: Reload on File Change

Use `node:fs.watch()` in the primary process to watch the `worker.js` file. When the file changes, automatically trigger a rolling reload. This gives you auto-reload during development.

### Exercise 3: Drain Timeout Metric

Add a metric that tracks how long each worker takes to drain (time from shutdown signal to exit). Log this for each worker during a reload. Identify if any requests are approaching the 15-second force-kill timeout.

## Next Steps

You now understand worker threads, clustering, and zero-downtime deployment patterns. Combine these with the deployment knowledge from Chapter 10 to build production-ready Node.js applications.

> See: ../10-deployment/docker/01-dockerfile.md for containerized deployment strategies that work with clustered applications.
