# Worker Pool Pattern

## What You'll Learn

- What the worker pool pattern is and why it matters
- How to create a fixed pool of reusable worker threads
- How to submit tasks and collect results asynchronously
- How to limit concurrency to match your CPU core count
- How to integrate a worker pool with an HTTP server

## The Problem with Creating Workers on Demand

Creating a worker is expensive. Each `new Worker()` call:

1. Allocates a new V8 isolate (separate heap, separate JIT compiler)
2. Parses and compiles the worker script from scratch
3. Starts a new event loop

This takes **30–80ms** per worker. If you create a new worker for every HTTP request, your server will spend more time spinning up threads than doing actual work.

The solution: **create workers once** at startup, then reuse them.

## The Worker Pool Pattern

A worker pool is a fixed set of worker threads that sit idle until a task arrives. When you submit a task:

1. An idle worker picks it up
2. The worker computes the result
3. The result is returned to the caller
4. The worker goes back to idle, ready for the next task

```
Task Queue         Worker Pool
──────────         ───────────
[task A] ──────→ Worker 1 → result A
[task B] ──────→ Worker 2 → result B
[task C] ───┐     Worker 3 → result C
            │     Worker 4 (idle, waiting)
            └──→ (queued until a worker frees up)
```

## Project Structure

```
04-worker-pool-pattern/
├── main.js          # Creates the pool, submits tasks, starts server
├── pool.js          # Piscina-like pool implementation
└── compute.js       # Worker script — runs heavy computation
```

## The Worker Script

```js
// compute.js — Runs inside each worker thread

import { parentPort } from 'node:worker_threads';

// Listen for tasks from the pool manager
parentPort.on('message', async (task) => {
  const { id, type, data } = task;

  if (type === 'fibonacci') {
    // CPU-bound: recursive Fibonacci
    const start = performance.now();
    const result = fib(data.n);
    const elapsed = (performance.now() - start).toFixed(1);

    parentPort.postMessage({
      id,
      result,
      elapsed: Number(elapsed),
    });
    return;
  }

  if (type === 'factorial') {
    // CPU-bound: large factorial using BigInt for exact arithmetic
    const result = factorial(data.n);
    parentPort.postMessage({ id, result: result.toString() });
    return;
  }

  // Unknown task type — report an error
  parentPort.postMessage({
    id,
    error: `Unknown task type: ${type}`,
  });
});

// Recursive Fibonacci — O(2^n) time complexity
function fib(n) {
  if (n <= 1) return n;
  return fib(n - 1) + fib(n - 2);
}

// Factorial using BigInt — avoids floating-point overflow
function factorial(n) {
  let result = 1n;  // BigInt literal
  for (let i = 2n; i <= BigInt(n); i++) {
    result *= i;
  }
  return result;
}
```

## The Pool Manager

```js
// pool.js — A simple worker pool implementation

import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { EventEmitter } from 'node:events';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Each task gets a unique ID so we can match results to promises
let taskIdCounter = 0;

export class WorkerPool extends EventEmitter {
  constructor(workerScript, numThreads) {
    super();

    this.workerScript = workerScript;      // Absolute path to the worker file
    this.numThreads = numThreads;           // How many workers to create
    this.workers = [];                      // All worker instances
    this.freeWorkers = [];                  // Workers currently idle
    this.taskQueue = [];                    // Pending tasks waiting for a free worker
    this.pendingTasks = new Map();          // taskId → { resolve, reject }

    // Create the worker pool
    this._init();
  }

  _init() {
    for (let i = 0; i < this.numThreads; i++) {
      this._createWorker();
    }
  }

  _createWorker() {
    const worker = new Worker(this.workerScript);

    // When a worker finishes a task, resolve the corresponding promise
    worker.on('message', (result) => {
      const task = this.pendingTasks.get(result.id);
      if (task) {
        this.pendingTasks.delete(result.id);

        if (result.error) {
          task.reject(new Error(result.error));  // Task reported an error
        } else {
          task.resolve(result);                   // Task succeeded
        }
      }

      // Mark this worker as free and process the next queued task
      this._freeWorker(worker);
    });

    // If a worker crashes, create a replacement
    worker.on('error', (err) => {
      this.emit('error', err);
      // Remove the dead worker
      const idx = this.workers.indexOf(worker);
      if (idx !== -1) this.workers.splice(idx, 1);

      // Reject any pending task this worker was handling
      for (const [taskId, task] of this.pendingTasks) {
        task.reject(err);
        this.pendingTasks.delete(taskId);
      }

      // Spawn a replacement worker
      this._createWorker();
    });

    this.workers.push(worker);
    this.freeWorkers.push(worker);  // New worker starts as free
  }

  // Get a free worker, or queue the task if all are busy
  _getWorker() {
    return new Promise((resolve) => {
      if (this.freeWorkers.length > 0) {
        resolve(this.freeWorkers.pop());  // Take a free worker immediately
      } else {
        this.taskQueue.push(resolve);     // No free workers — queue the request
      }
    });
  }

  // Return a worker to the free pool and dequeue the next waiting request
  _freeWorker(worker) {
    if (this.taskQueue.length > 0) {
      // Another task is waiting — give it this worker immediately
      const next = this.taskQueue.shift();
      next(worker);
    } else {
      // No queued tasks — mark the worker as idle
      this.freeWorkers.push(worker);
    }
  }

  // Submit a task and return a promise that resolves with the result
  async runTask(data) {
    const worker = await this._getWorker();  // Wait for a free worker

    const id = taskIdCounter++;  // Unique ID for this task

    return new Promise((resolve, reject) => {
      // Track this promise so we can resolve it when the worker replies
      this.pendingTasks.set(id, { resolve, reject });

      // Send the task to the worker
      worker.postMessage({ id, ...data });
    });
  }

  // Shut down all workers gracefully
  async destroy() {
    const terminationPromises = this.workers.map((w) => w.terminate());
    await Promise.all(terminationPromises);
    this.workers = [];
    this.freeWorkers = [];
    this.taskQueue = [];
    this.pendingTasks.clear();
  }

  // How many workers are currently idle
  get freeCount() {
    return this.freeWorkers.length;
  }

  // How many tasks are queued waiting for a worker
  get pendingCount() {
    return this.taskQueue.length;
  }

  // How many tasks are currently being computed
  get activeCount() {
    return this.pendingTasks.size;
  }
}
```

## Using the Pool with an HTTP Server

```js
// main.js — HTTP server that offloads CPU work to the worker pool

import { createServer } from 'node:http';
import { availableParallelism } from 'node:os';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { WorkerPool } from './pool.js';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Create a pool with one worker per CPU core
// availableParallelism() returns the number of logical CPU cores (Node.js v18.14+)
const numCores = availableParallelism();
const pool = new WorkerPool(resolve(__dirname, 'compute.js'), numCores);

console.log(`Worker pool started with ${numCores} threads`);

// Log pool errors
pool.on('error', (err) => {
  console.error('Pool error:', err.message);
});

const server = createServer(async (req, res) => {
  // Route: GET /fib?n=42
  if (req.url?.startsWith('/fib')) {
    const url = new URL(req.url, `http://${req.headers.host}`);
    const n = parseInt(url.searchParams.get('n') || '40', 10);

    try {
      // Submit the task to the pool — returns a promise
      const result = await pool.runTask({
        type: 'fibonacci',
        data: { n },
      });

      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({
        n,
        fibonacci: result.result,
        elapsed: `${result.elapsed}ms`,
        pool: { active: pool.activeCount, free: pool.freeCount },
      }));
    } catch (err) {
      res.writeHead(500, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ error: err.message }));
    }
    return;
  }

  // Route: GET /factorial?n=1000
  if (req.url?.startsWith('/factorial')) {
    const url = new URL(req.url, `http://${req.headers.host}`);
    const n = parseInt(url.searchParams.get('n') || '100', 10);

    try {
      const result = await pool.runTask({
        type: 'factorial',
        data: { n },
      });

      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({
        n,
        factorial: result.result,
        pool: { active: pool.activeCount, free: pool.freeCount },
      }));
    } catch (err) {
      res.writeHead(500, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ error: err.message }));
    }
    return;
  }

  // Default route
  res.writeHead(200, { 'Content-Type': 'text/plain' });
  res.end(
    `Worker Pool Demo\n` +
    `Cores: ${numCores}\n` +
    `Try: /fib?n=42 or /factorial?n=100`
  );
});

server.listen(3000, () => {
  console.log('Server running on http://localhost:3000');
});

// Graceful shutdown — terminate all workers when the process receives SIGINT
process.on('SIGINT', async () => {
  console.log('\nShutting down...');
  await pool.destroy();
  server.close();
  process.exit(0);
});
```

## How It Works

### Initialization

```js
const pool = new WorkerPool(resolve(__dirname, 'compute.js'), numCores);
```

The pool creates `numCores` workers immediately. Each worker runs `compute.js` and sits idle, waiting for messages.

### Task Submission

```js
const result = await pool.runTask({ type: 'fibonacci', data: { n: 42 } });
```

1. `_getWorker()` checks if any worker is free
2. If yes, it pops a worker off the `freeWorkers` array
3. If no, it adds a callback to the `taskQueue` — the callback will be called when a worker frees up
4. The task is sent to the worker via `postMessage()` with a unique `id`
5. A promise is stored in `pendingTasks` keyed by that `id`

### Result Handling

When the worker finishes, it sends back `{ id, result, elapsed }`. The pool:

1. Looks up `id` in `pendingTasks`
2. Resolves the corresponding promise with the result
3. Calls `_freeWorker()` to return the worker to the idle pool

### Auto-Recovery

If a worker crashes (uncaught exception), the pool:

1. Removes the dead worker from the array
2. Rejects any promises that worker was handling
3. Creates a replacement worker to maintain the pool size

## Using Piscina (Production Alternative)

For production code, consider using [Piscina](https://github.com/piscinajs/piscina) — a well-tested, feature-rich worker pool. It provides the same pattern with less code:

```bash
npm install piscina
```

```js
// piscina-main.js — Using the Piscina library

import Piscina from 'piscina';
import { createServer } from 'node:http';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { availableParallelism } from 'node:os';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Create a Piscina pool — auto-scales up to maxThreads
const pool = new Piscina({
  filename: resolve(__dirname, 'piscina-worker.js'),
  maxThreads: availableParallelism(),
});

const server = createServer(async (req, res) => {
  if (req.url?.startsWith('/fib')) {
    const url = new URL(req.url, `http://${req.headers.host}`);
    const n = parseInt(url.searchParams.get('n') || '40', 10);

    // runTask returns a promise — same pattern as our custom pool
    const result = await pool.run({ n });
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({ n, fibonacci: result }));
    return;
  }

  res.writeHead(200, { 'Content-Type': 'text/plain' });
  res.end('Piscina Worker Pool Demo\nTry: /fib?n=42');
});

server.listen(3000);
```

```js
// piscina-worker.js — Piscina exports a default function

// Piscina expects a default export that receives the task data
export default function ({ n }) {
  function fib(num) {
    if (num <= 1) return num;
    return fib(num - 1) + fib(num - 2);
  }

  return fib(n);
}
```

## Common Mistakes

### Mistake 1: Pool Size Too Large

```js
// WRONG — more workers than cores wastes memory and causes context switching
const pool = new WorkerPool(script, 64);  // 64 workers on an 8-core machine

// CORRECT — match your core count (or slightly less to leave room for the main thread)
import { availableParallelism } from 'node:os';
const pool = new WorkerPool(script, availableParallelism() - 1);
```

### Mistake 2: Not Handling Worker Crashes

```js
// WRONG — if a worker crashes, tasks hang forever
worker.on('message', (result) => {
  resolve(result);  // Never called if worker crashes
});

// CORRECT — also listen for 'error' and reject the promise
worker.on('error', (err) => {
  reject(err);  // Fail fast instead of hanging
});
```

### Mistake 3: Fire-and-Forget Without Awaiting

```js
// WRONG — server responds before the worker finishes
server.on('request', (req, res) => {
  pool.runTask({ type: 'fibonacci', data: { n: 40 } }).then((result) => {
    res.end(JSON.stringify(result));  // res might already be closed
  });
  // Function returns — but promise is still pending!
});

// CORRECT — make the handler async and await the result
server.on('request', async (req, res) => {
  const result = await pool.runTask({ type: 'fibonacci', data: { n: 40 } });
  res.end(JSON.stringify(result));
});
```

## Try It Yourself

### Exercise 1: Add a Third Task Type

Add a `prime-check` task type to `compute.js` that checks if a number is prime. Register the route `GET /prime?n=104729` in `main.js`.

### Exercise 2: Pool Statistics Endpoint

Add a `GET /stats` route that returns JSON with pool statistics: total workers, free workers, active tasks, and queued tasks. Use the pool's `freeCount`, `activeCount`, and `pendingCount` properties.

### Exercise 3: Timeout for Tasks

Add a timeout mechanism to `runTask()`. If a task takes longer than 5 seconds, reject the promise, terminate the slow worker, and spawn a replacement. Use `Promise.race()` with `setTimeout`.

## Next Steps

You can now parallelize CPU work across multiple threads. For scaling network servers across processes, let's explore the cluster module. Continue to [Cluster Basics](../cluster-module/01-cluster-basics.md).
