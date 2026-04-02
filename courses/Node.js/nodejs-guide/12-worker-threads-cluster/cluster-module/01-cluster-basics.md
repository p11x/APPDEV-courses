# Cluster Basics

## What You'll Learn

- What the cluster module is and how it differs from worker threads
- How `cluster.fork()` creates child processes that share a port
- How the primary process distributes connections to workers
- How to handle messages between primary and workers
- How to restart crashed workers automatically

## What Is the Cluster Module?

Node.js runs on a single CPU core by default. On a machine with 8 cores, a single Node.js process uses only 12.5% of your CPU. The **cluster** module lets you spawn multiple Node.js processes (called **workers**) that all listen on the **same port**. The operating system distributes incoming connections across them.

This is completely different from worker threads:

| Feature | Worker Threads | Cluster |
|---------|---------------|---------|
| Shared memory | Yes (SharedArrayBuffer) | No (separate processes) |
| Shared port | No | Yes (OS-level load balancing) |
| Use case | CPU-heavy computation | Scaling network servers |
| Overhead | Low (shared V8 heap) | Medium (separate V8 per process) |
| Isolation | Weak (shared memory) | Strong (separate memory space) |

## How Clustering Works

```
                    ┌──────────────────┐
  Client A ───────→ │  Primary Process │
  Client B ───────→ │  (load balancer) │
  Client C ───────→ │                  │
                    └──┬─────┬─────┬──┘
                       │     │     │
              ┌────────┘  ┌──┘  ┌──┘
              ▼           ▼     ▼
         Worker 1    Worker 2   Worker 3
         (port 3000) (port 3000) (port 3000)
```

1. The **primary** process calls `cluster.fork()` to create workers
2. Each **worker** runs `server.listen(3000)` — but they all share port 3000
3. The OS accepts incoming TCP connections and distributes them to workers (round-robin on Linux, Windows)
4. Each worker handles requests independently — if one crashes, others keep serving

## Basic Cluster Server

### Project Structure

```
01-cluster-basics/
├── server.js        # Entry point — primary forks workers
└── worker.js        # Worker process — runs the Express/HTTP server
```

### Primary Process

```js
// server.js — Primary process: forks workers and monitors them

import cluster from 'node:cluster';
import { availableParallelism } from 'node:os';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// cluster.isPrimary is true for the original process that starts first
// Workers are child processes created by cluster.fork()
if (cluster.isPrimary) {
  // Determine how many workers to create
  // Use the number of CPU cores, or fall back to 2
  const numWorkers = availableParallelism() || 2;

  console.log(`Primary process ${process.pid} starting`);
  console.log(`Forking ${numWorkers} workers...\n`);

  // Create one worker per CPU core
  for (let i = 0; i < numWorkers; i++) {
    cluster.fork();  // Spawns a new Node.js process running this same file
  }

  // Listen for workers coming online
  cluster.on('online', (worker) => {
    console.log(`Worker ${worker.process.pid} is online`);
  });

  // Listen for workers exiting — restart if they crash unexpectedly
  cluster.on('exit', (worker, code, signal) => {
    console.log(
      `Worker ${worker.process.pid} exited (code: ${code}, signal: ${signal})`
    );

    // Exit code 0 means clean shutdown — do not restart
    // Any other exit code means a crash — spawn a replacement
    if (code !== 0) {
      console.log('Starting a new worker to replace it...');
      cluster.fork();
    }
  });

  // Listen for messages from workers
  cluster.on('message', (worker, message) => {
    console.log(`Message from worker ${worker.process.pid}:`, message);
  });
} else {
  // This runs inside each worker process — see worker.js
  await import('./worker.js');
}
```

### Worker Process

```js
// worker.js — Each worker runs this file and handles HTTP requests

import { createServer } from 'node:http';

// process.pid is unique per worker — helps identify which worker handled a request
const workerId = process.pid;

const server = createServer((req, res) => {
  // Log which worker is handling this request
  console.log(`Worker ${workerId} handling: ${req.method} ${req.url}`);

  // Simulate some work
  const start = performance.now();

  if (req.url === '/heavy') {
    // Simulate a CPU-heavy task (200ms of synchronous work)
    let sum = 0;
    for (let i = 0; i < 1e8; i++) {
      sum += Math.sqrt(i);
    }

    const elapsed = (performance.now() - start).toFixed(1);
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({
      worker: workerId,
      result: sum,
      elapsed: `${elapsed}ms`,
    }));
    return;
  }

  // Default route
  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.end(JSON.stringify({
    message: 'Hello from cluster!',
    worker: workerId,
    timestamp: new Date().toISOString(),
  }));
});

// All workers listen on the same port — the OS load-balances between them
const PORT = process.env.PORT || 3000;

server.listen(PORT, () => {
  console.log(`Worker ${workerId} listening on port ${PORT}`);
});

// Send a message to the primary process
process.send?.({ type: 'ready', pid: workerId });

// Graceful shutdown on SIGTERM (e.g., from process manager)
process.on('SIGTERM', () => {
  console.log(`Worker ${workerId} shutting down...`);
  server.close(() => {
    process.exit(0);  // Clean exit — primary will not restart us
  });
});
```

### Running the Cluster

```bash
node server.js
```

Output:

```
Primary process 12345 starting
Forking 8 workers...

Worker 12346 is online
Worker 12347 is online
Worker 12348 is online
...
```

### Testing with curl

```bash
# Each request may be handled by a different worker
curl http://localhost:3000/
# {"message":"Hello from cluster!","worker":12346,"timestamp":"2024-..."}

curl http://localhost:3000/
# {"message":"Hello from cluster!","worker":12347,"timestamp":"2024-..."}

curl http://localhost:3000/
# {"message":"Hello from cluster!","worker":12348,"timestamp":"2024-..."}
```

## How It Works

### Step 1: Primary Checks `cluster.isPrimary`

```js
if (cluster.isPrimary) {
  // Runs once in the original process
}
```

When you run `node server.js`, the primary process executes. `cluster.isPrimary` is `true` only for this first process.

### Step 2: Primary Calls `cluster.fork()`

```js
cluster.fork();
```

Each `fork()` spawns a new Node.js process that runs the **same file** again. But this time `cluster.isPrimary` is `false`, so the `else` branch runs — the worker imports `worker.js`.

### Step 3: Workers Listen on the Same Port

```js
server.listen(3000);
```

When multiple processes call `listen()` on the same port, Node.js uses the operating system's socket-sharing mechanism. On Linux, it uses `SO_REUSEPORT`; on Windows, it uses `SO_REUSEADDR`. The OS distributes incoming TCP connections across the workers.

### Step 4: Primary Monitors Workers

```js
cluster.on('exit', (worker, code) => {
  if (code !== 0) cluster.fork();  // Restart crashed workers
});
```

## IPC Messaging Between Primary and Workers

Processes in a cluster can send messages to each other via Inter-Process Communication (IPC).

### Worker to Primary

```js
// Inside worker.js — send a message to the primary
process.send({ type: 'status', activeRequests: 42 });
```

```js
// Inside server.js primary block — receive messages from any worker
cluster.on('message', (worker, message) => {
  console.log(`Worker ${worker.process.pid} says:`, message);
});
```

### Primary to Specific Worker

```js
// Inside server.js primary block — send to one worker
const worker = cluster.fork();
worker.send({ type: 'config', maxConnections: 100 });
```

```js
// Inside worker.js — receive messages from the primary
process.on('message', (message) => {
  if (message.type === 'config') {
    console.log('Max connections:', message.maxConnections);
  }
});
```

### Full IPC Example

```js
// ipc-primary.js — Primary sends config to workers, workers report stats

import cluster from 'node:cluster';
import { availableParallelism } from 'node:os';

if (cluster.isPrimary) {
  const numWorkers = Math.min(availableParallelism(), 4);

  for (let i = 0; i < numWorkers; i++) {
    const worker = cluster.fork();

    // Send configuration to each worker after it comes online
    worker.on('online', () => {
      worker.send({
        type: 'config',
        workerId: i + 1,
        maxConnections: 1000,
      });
    });
  }

  // Collect stats from all workers every 5 seconds
  setInterval(() => {
    for (const id in cluster.workers) {
      cluster.workers[id]?.send({ type: 'request-stats' });
    }
  }, 5000);

  // Print stats received from workers
  cluster.on('message', (worker, msg) => {
    if (msg.type === 'stats') {
      console.log(
        `Worker ${msg.workerId}: ${msg.requestCount} requests handled`
      );
    }
  });
} else {
  // Worker process
  let config = {};
  let requestCount = 0;

  process.on('message', (msg) => {
    if (msg.type === 'config') {
      config = msg;
      console.log(`I am worker ${config.workerId}`);
    } else if (msg.type === 'request-stats') {
      // Reply with our current stats
      process.send({
        type: 'stats',
        workerId: config.workerId,
        requestCount,
      });
    }
  });

  // Start a simple server (imported inline to keep the example self-contained)
  const { createServer } = await import('node:http');
  createServer((req, res) => {
    requestCount++;
    res.writeHead(200);
    res.end(`Worker ${config.workerId || '?'}`);
  }).listen(3000);
}
```

## Common Mistakes

### Mistake 1: Listening on a Port in the Primary

```js
// WRONG — primary should NOT listen on a port; only workers should
if (cluster.isPrimary) {
  const app = express();
  app.listen(3000);  // This blocks workers from sharing the port
  cluster.fork();
}

// CORRECT — primary only manages workers
if (cluster.isPrimary) {
  cluster.fork();
} else {
  const app = express();
  app.listen(3000);  // Only workers listen
}
```

### Mistake 2: Sharing State via Global Variables

```js
// WRONG — each worker has its own memory; globals are NOT shared
let requestCount = 0;

server.on('request', () => {
  requestCount++;  // Each worker increments its OWN copy
});

// CORRECT — use Redis, a database, or IPC messages for shared state
import { createClient } from 'redis';
const redis = createClient();
await redis.connect();

server.on('request', async () => {
  await redis.incr('request-count');  // Single shared counter
});
```

### Mistake 3: Not Handling Graceful Shutdown

```js
// WRONG — workers die immediately, dropping in-flight requests
process.on('SIGTERM', () => {
  process.exit(1);  // Abrupt — requests in progress are lost
});

// CORRECT — stop accepting new requests, finish existing ones, then exit
process.on('SIGTERM', () => {
  server.close(() => {          // Stop accepting new connections
    process.exit(0);            // Exit after all responses are sent
  });
});
```

## Try It Yourself

### Exercise 1: Worker Identification

Start the cluster server. Make 20 `curl` requests and record which `worker` PID handled each one. Is the distribution even?

### Exercise 2: Crash Recovery

Modify `worker.js` to add a `GET /crash` route that throws an uncaught exception. Hit it and verify that the primary process logs the exit and starts a replacement worker. Then hit `GET /` to confirm the new worker is serving requests.

### Exercise 3: Request Counter via IPC

Add IPC messaging so that each worker reports its request count to the primary every 10 seconds. The primary should print a summary of all workers' request counts.

## Next Steps

You know how to scale a server across CPU cores. Now let's learn how to do zero-downtime deployments — restarting workers without dropping connections. Continue to [Zero-Downtime Reload](./02-zero-downtime-reload.md).
