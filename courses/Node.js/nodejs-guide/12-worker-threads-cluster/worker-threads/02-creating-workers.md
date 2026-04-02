# Creating Workers

## What You'll Learn

- How worker threads work at the V8 and libuv level
- Four progressive implementations from basic to production-optimized
- How to benchmark worker thread overhead and communication latency
- Security considerations for shared memory and message passing
- Comprehensive testing for multi-threaded code
- Production deployment patterns for worker pools

---

## Part 1: Core Concepts Deep Dive

### Theoretical Foundations

A **worker thread** is a separate JavaScript execution environment within the same Node.js process. Unlike child processes (which are separate OS processes), workers share the same heap memory space — but each has its own V8 isolate, event loop, and set of built-in modules.

**Process vs Thread vs Worker Thread:**

| Concept | Memory | Creation Cost | Communication |
|---------|--------|--------------|---------------|
| OS Process | Separate heap | ~10-50ms, ~10MB | IPC (slow, serialized) |
| OS Thread | Shared heap | ~1-5ms, ~1MB | Shared memory (fast, dangerous) |
| Node.js Worker | Own V8 isolate, shared process | ~30-80ms, ~30MB | postMessage (structured clone) or SharedArrayBuffer |

**Why Worker Threads Exist:**

Node.js's event loop runs on a single thread. CPU-bound work (Fibonacci, image processing, JSON parsing of large files) blocks the event loop, making the server unresponsive. Worker threads offload this work to separate V8 isolates while the main thread continues handling requests.

**The Structured Clone Algorithm:**

When you call `worker.postMessage(data)`, the data is copied using the structured clone algorithm:

- Supported: Plain objects, arrays, strings, numbers, booleans, Dates, RegExps, ArrayBuffers, typed arrays, Maps, Sets, Blobs
- NOT supported: Functions, class instances (methods lost), DOM nodes, symbols, weak references

### Architectural Patterns

**When to Use Workers — Decision Tree:**

```
Is the task CPU-bound?
├── Yes → Is it a single computation or many?
│   ├── Single → Use one worker
│   └── Many → Use a worker pool (Chapter 12.04)
└── No → Is it I/O-bound?
    ├── Yes → Do NOT use workers — the event loop handles I/O efficiently
    └── No → Is it a third-party synchronous API?
        ├── Yes → Use a worker to avoid blocking
        └── No → Reconsider if you need workers at all
```

### Comparative Analysis Matrix

| Approach | Latency | Throughput | Memory | Complexity | Shared State |
|----------|---------|-----------|--------|------------|-------------|
| `postMessage` | ~0.1ms (small) | High | Duplicated | Low | No |
| `SharedArrayBuffer` | ~0.001ms | Very high | Shared | High | Yes (with Atomics) |
| MessageChannel | ~0.1ms | High | Duplicated | Medium | No |
| Child process IPC | ~1-5ms | Medium | Separate | Medium | No |

---

## Part 2: Progressive Code Examples

### Level 1 — Minimal Working Example

The smallest possible worker — send data, receive a result:

```js
// main-minimal.js — Minimal worker setup

import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Create a worker — it runs the specified file in a separate V8 isolate
const worker = new Worker(resolve(__dirname, 'worker-minimal.js'), {
  workerData: { n: 30 },  // Data sent to the worker (structured clone)
});

// Listen for the result
worker.on('message', (result) => {
  console.log(`Result: ${result}`);
});

worker.on('error', (err) => {
  console.error('Worker error:', err.message);
});
```

```js
// worker-minimal.js — Minimal worker

import { parentPort, workerData } from 'node:worker_threads';

// Compute and send result back
const result = workerData.n * 2;
parentPort.postMessage(result);
```

```bash
node main-minimal.js
# Output: Result: 60
```

### Level 2 — Production-Ready

Worker with error handling, timeouts, progress reporting, and cleanup:

```js
// main-production.js — Production-ready worker management

import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

/**
 * Run a worker with timeout and error handling.
 * @param {string} script - Path to worker script
 * @param {*} data - Data to send to worker
 * @param {number} timeoutMs - Max execution time
 * @returns {Promise<*>} Worker result
 */
function runWorker(script, data, timeoutMs = 30_000) {
  return new Promise((resolve, reject) => {
    const worker = new Worker(script, {
      workerData: data,
    });

    // Timeout — kill the worker if it takes too long
    const timer = setTimeout(() => {
      worker.terminate();  // Force-kill the worker
      reject(new Error(`Worker timed out after ${timeoutMs}ms`));
    }, timeoutMs);

    worker.on('message', (result) => {
      clearTimeout(timer);
      resolve(result);
    });

    worker.on('error', (err) => {
      clearTimeout(timer);
      reject(err);
    });

    worker.on('exit', (code) => {
      clearTimeout(timer);
      if (code !== 0) {
        reject(new Error(`Worker exited with code ${code}`));
      }
    });

    // Handle progress updates
    worker.on('message', (msg) => {
      if (msg?.type === 'progress') {
        process.stdout.write(`\rProgress: ${msg.percent}%`);
      }
    });
  });
}

// Usage
try {
  const result = await runWorker(
    resolve(__dirname, 'worker-production.js'),
    { task: 'factorial', n: 100_000 },
    10_000
  );
  console.log(`\nResult: ${result.digits} digits`);
} catch (err) {
  console.error(`\nError: ${err.message}`);
}
```

```js
// worker-production.js — Worker with progress reporting

import { parentPort, workerData } from 'node:worker_threads';

function computeFactorial(n) {
  let result = 1n;  // BigInt for large numbers

  for (let i = 2n; i <= BigInt(n); i++) {
    result *= i;

    // Report progress every 10,000 iterations
    if (i % 10_000n === 0n) {
      parentPort.postMessage({
        type: 'progress',
        percent: Number((i * 100n) / BigInt(n)),
      });
    }
  }

  return result;
}

try {
  const { task, n } = workerData;

  if (task === 'factorial') {
    const result = computeFactorial(n);
    parentPort.postMessage({
      type: 'result',
      digits: result.toString().length,
    });
  } else {
    throw new Error(`Unknown task: ${task}`);
  }
} catch (err) {
  // Send error back to main thread instead of crashing
  parentPort.postMessage({
    type: 'error',
    message: err.message,
  });
}
```

### Level 3 — Enterprise-Grade

A task dispatcher with typed workers, request-response correlation, and graceful shutdown:

```js
// enterprise-dispatcher.js — Enterprise worker task dispatcher

import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { EventEmitter } from 'node:events';

const __dirname = dirname(fileURLToPath(import.meta.url));

let taskIdCounter = 0;

class WorkerDispatcher extends EventEmitter {
  constructor(workerScript, options = {}) {
    super();

    this.workerScript = workerScript;
    this.maxWorkers = options.maxWorkers || 4;
    this.taskTimeout = options.taskTimeout || 30_000;
    this.workers = [];
    this.freeWorkers = [];
    this.pendingTasks = new Map();   // taskId → { resolve, reject, timer }
    this.taskQueue = [];              // Tasks waiting for a free worker
    this.stats = { submitted: 0, completed: 0, failed: 0, timedOut: 0 };

    this._init();
  }

  _init() {
    for (let i = 0; i < this.maxWorkers; i++) {
      this._createWorker();
    }
  }

  _createWorker() {
    const worker = new Worker(this.workerScript);

    worker.on('message', (msg) => {
      if (msg.type === 'progress') {
        this.emit('progress', msg);
        return;
      }

      const task = this.pendingTasks.get(msg.taskId);
      if (!task) return;

      clearTimeout(task.timer);
      this.pendingTasks.delete(msg.taskId);

      if (msg.type === 'error') {
        this.stats.failed++;
        task.reject(new Error(msg.message));
      } else {
        this.stats.completed++;
        task.resolve(msg.result);
      }

      this._freeWorker(worker);
    });

    worker.on('error', (err) => {
      this.emit('error', err);
      // Replace the dead worker
      const idx = this.workers.indexOf(worker);
      if (idx !== -1) this.workers.splice(idx, 1);

      // Reject all tasks this worker was handling
      for (const [taskId, task] of this.pendingTasks) {
        task.reject(err);
        this.pendingTasks.delete(taskId);
      }

      this._createWorker();
    });

    this.workers.push(worker);
    this.freeWorkers.push(worker);
  }

  _getWorker() {
    return new Promise((resolve) => {
      if (this.freeWorkers.length > 0) {
        resolve(this.freeWorkers.pop());
      } else {
        this.taskQueue.push(resolve);
      }
    });
  }

  _freeWorker(worker) {
    if (this.taskQueue.length > 0) {
      const next = this.taskQueue.shift();
      next(worker);
    } else {
      this.freeWorkers.push(worker);
    }
  }

  async submit(type, data) {
    const worker = await this._getWorker();
    const taskId = taskIdCounter++;
    this.stats.submitted++;

    return new Promise((resolve, reject) => {
      const timer = setTimeout(() => {
        this.pendingTasks.delete(taskId);
        this.stats.timedOut++;
        worker.terminate();
        this._createWorker();  // Replace the timed-out worker
        reject(new Error(`Task ${taskId} timed out`));
      }, this.taskTimeout);

      this.pendingTasks.set(taskId, { resolve, reject, timer });
      worker.postMessage({ taskId, type, data });
    });
  }

  getStats() {
    return {
      ...this.stats,
      activeWorkers: this.workers.length,
      freeWorkers: this.freeWorkers.length,
      queuedTasks: this.taskQueue.length,
      pendingTasks: this.pendingTasks.size,
    };
  }

  async shutdown() {
    // Stop accepting new tasks
    this.taskQueue.length = 0;

    // Wait for pending tasks to complete (with timeout)
    const timeout = setTimeout(() => {
      // Force-kill any remaining workers
      for (const worker of this.workers) {
        worker.terminate();
      }
    }, 10_000);

    while (this.pendingTasks.size > 0) {
      await new Promise((r) => setTimeout(r, 100));
    }

    clearTimeout(timeout);

    // Terminate all workers
    await Promise.all(this.workers.map((w) => w.terminate()));
    this.workers = [];
    this.freeWorkers = [];
  }
}

// Usage
const dispatcher = new Dispatcher(
  resolve(__dirname, 'enterprise-worker.js'),
  { maxWorkers: 4, taskTimeout: 15_000 }
);

// Submit multiple tasks
const results = await Promise.all([
  dispatcher.submit('fibonacci', { n: 40 }),
  dispatcher.submit('fibonacci', { n: 41 }),
  dispatcher.submit('fibonacci', { n: 42 }),
]);

console.log('Results:', results);
console.log('Stats:', dispatcher.getStats());

await dispatcher.shutdown();
```

### Level 4 — Performance-Optimized

Worker with shared memory (SharedArrayBuffer) for zero-copy data transfer:

```js
// optimized-main.js — SharedArrayBuffer for zero-copy communication

import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Benchmark: postMessage vs SharedArrayBuffer
const iterations = 1_000_000;
const sharedBuffer = new SharedArrayBuffer(4);  // 4 bytes for one Int32
const sharedArray = new Int32Array(sharedBuffer);

// === Benchmark 1: postMessage (copy-based) ===
const startCopy = performance.now();

const worker1 = new Worker(resolve(__dirname, 'bench-copy-worker.js'), {
  workerData: { iterations },
});

await new Promise((resolve) => {
  worker1.on('message', (count) => {
    console.log(`postMessage: ${count} increments`);
    resolve();
  });
});

const copyTime = performance.now() - startCopy;

// === Benchmark 2: SharedArrayBuffer (zero-copy) ===
Atomics.store(sharedArray, 0, 0);
const startShared = performance.now();

const worker2 = new Worker(resolve(__dirname, 'bench-shared-worker.js'), {
  workerData: { buffer: sharedBuffer, iterations },
});

await new Promise((resolve) => {
  worker2.on('message', () => {
    console.log(`SharedArrayBuffer: ${Atomics.load(sharedArray, 0)} increments`);
    resolve();
  });
});

const sharedTime = performance.now() - startShared;

// Results
console.log(`\npostMessage:    ${copyTime.toFixed(1)}ms`);
console.log(`SharedArray:    ${sharedTime.toFixed(1)}ms`);
console.log(`Speedup:        ${(copyTime / sharedTime).toFixed(1)}x`);
```

**Typical results:**

| Method | Time for 1M increments | Overhead |
|--------|----------------------|----------|
| postMessage round-trips | ~8,000ms | ~8μs/message |
| SharedArrayBuffer | ~15ms | ~15ns/increment |
| **Speedup** | **~530x** | |

---

## Part 3: Performance & Optimization

### Profiling Techniques

**Measure worker creation overhead:**

```js
import { Worker } from 'node:worker_threads';
import { performance } from 'node:perf_hooks';

const start = performance.now();
const worker = new Worker('./empty-worker.js');

worker.on('online', () => {
  const elapsed = performance.now() - start;
  console.log(`Worker creation: ${elapsed.toFixed(1)}ms`);  // Typically 30-80ms
  worker.terminate();
});
```

**Profile message latency:**

```js
const iterations = 10_000;
const start = performance.now();

for (let i = 0; i < iterations; i++) {
  worker.postMessage(i);
  // Wait for response (simplified)
}

const elapsed = performance.now() - start;
console.log(`Avg message latency: ${(elapsed / iterations * 1000).toFixed(1)}μs`);
// Typical: 50-200μs per round-trip for small messages
```

### Benchmark Comparison

| Data Size | postMessage | SharedArrayBuffer | Best Choice |
|-----------|------------|-------------------|-------------|
| < 1KB | 0.05ms | 0.01ms | SharedArrayBuffer |
| 1KB–100KB | 0.5ms | 0.01ms | SharedArrayBuffer |
| 100KB–10MB | 5ms | 0.01ms | SharedArrayBuffer |
| > 10MB | 50ms+ | 0.01ms | SharedArrayBuffer |

### Memory Management

- Each worker allocates ~30-50MB for its V8 isolate
- SharedArrayBuffer memory is shared (not duplicated)
- postMessage duplicates data — large messages consume 2x memory (sender + receiver)
- Always terminate workers when done — they hold memory until explicitly terminated

---

## Part 4: Security Fortress

### Vulnerability Patterns

**Data leakage through shared memory:**

```js
// RISK: SharedArrayBuffer contents are accessible to ALL workers
const secret = new SharedArrayBuffer(32);
// Worker A writes sensitive data → Worker B can read it

// MITIGATION: Do not put secrets in shared memory
// Use postMessage for sensitive data (it's copied, not shared)
```

**Denial of service via worker exhaustion:**

```js
// RISK: Attacker triggers unlimited worker creation
app.get('/compute', (req, res) => {
  const worker = new Worker('./compute.js');  // Creates a new worker per request!
  // Attack: 10,000 requests = 10,000 workers = 300GB+ memory
});

// MITIGATION: Use a fixed worker pool
const pool = new WorkerPool('./compute.js', { maxWorkers: 4 });
```

### Audit Checklist

- [ ] Worker count is bounded (pool pattern)
- [ ] Workers have timeouts (prevent infinite computation)
- [ ] Worker errors do not crash the main process
- [ ] No secrets in SharedArrayBuffer
- [ ] Worker scripts are not dynamically generated from user input
- [ ] Workers are terminated on shutdown

---

## Part 5: Testing Pyramid

### Unit Tests

```js
// worker-unit.test.js
import { describe, it } from 'node:test';
import assert from 'node:assert';
import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

describe('Worker', () => {
  it('should compute fibonacci(10) = 55', async () => {
    const worker = new Worker(resolve(__dirname, 'fibonacci.js'), {
      workerData: { n: 10 },
    });

    const result = await new Promise((resolve, reject) => {
      worker.on('message', resolve);
      worker.on('error', reject);
      setTimeout(() => { worker.terminate(); reject(new Error('timeout')); }, 5000);
    });

    assert.strictEqual(result.value, 55);
  });

  it('should handle invalid input', async () => {
    const worker = new Worker(resolve(__dirname, 'fibonacci.js'), {
      workerData: { n: -1 },
    });

    const result = await new Promise((resolve) => {
      worker.on('message', resolve);
      worker.on('error', (err) => resolve({ error: err.message }));
    });

    // Worker should handle gracefully, not crash
    assert.ok(result);
  });
});
```

### Integration Tests

```js
// worker-integration.test.js
import { describe, it } from 'node:test';
import assert from 'node:assert';

describe('Worker Pool', () => {
  it('should process tasks concurrently', async () => {
    const pool = new WorkerPool('./compute.js', { maxWorkers: 4 });

    const start = performance.now();
    const results = await Promise.all([
      pool.submit('sleep', { ms: 100 }),
      pool.submit('sleep', { ms: 100 }),
      pool.submit('sleep', { ms: 100 }),
      pool.submit('sleep', { ms: 100 }),
    ]);
    const elapsed = performance.now() - start;

    // 4 tasks at 100ms each = ~100ms total (parallel), not 400ms (sequential)
    assert.ok(elapsed < 200, `Expected parallel execution, got ${elapsed}ms`);

    await pool.shutdown();
  });
});
```

### CI Integration

```yaml
jobs:
  test:
    strategy:
      matrix:
        node-version: [18, 20, 22]
    steps:
      - run: npm test
      # Worker tests need all Node.js versions due to API differences
```

---

## Part 6: Production Operations

### Deployment Blueprint

```yaml
# docker-compose.yml
services:
  api:
    build: .
    environment:
      WORKER_POOL_SIZE: 4          # Match CPU cores
      WORKER_TASK_TIMEOUT: 30000   # 30 seconds
    deploy:
      resources:
        limits:
          memory: 2G               # 4 workers × ~500MB each
          cpus: '4'
```

### Monitoring

```js
// Expose worker metrics
app.get('/metrics/workers', (req, res) => {
  res.json({
    pool: dispatcher.getStats(),
    memory: process.memoryUsage(),
  });
});
```

### Operational Runbook

**Symptom: Worker pool exhausted**
1. Check: Are workers crashing? Check logs for unhandled errors
2. Check: Are tasks timing out? Increase `taskTimeout`
3. Check: Is the pool too small? Increase `maxWorkers`
4. Action: Scale horizontally (more instances) rather than vertically (more workers per instance)

**Symptom: Memory growing unboundedly**
1. Check: Are workers being terminated after use?
2. Check: Is `postMessage` sending large objects repeatedly?
3. Action: Use SharedArrayBuffer for large data, terminate idle workers

---

## Self-Assessment Quiz

**Q1:** What is the main difference between a worker thread and a child process?

<details>
<summary>Answer</summary>
A worker thread runs in the same OS process (shared memory space possible, faster communication). A child process is a separate OS process (isolated memory, slower IPC communication).
</details>

**Q2:** Why must you use absolute paths when creating a worker?

<details>
<summary>Answer</summary>
Relative paths resolve from the current working directory (`process.cwd()`), not from the file's location. This causes "Cannot find module" errors when the script is run from a different directory.
</details>

**Q3:** What happens if you pass a function to `worker.postMessage()`?

<details>
<summary>Answer</summary>
A `DataCloneError` is thrown. The structured clone algorithm cannot serialize functions — only plain data types are supported.
</details>

---

## Hands-On Challenges

### Challenge 1: Image Resizer (Easy)
Create a worker that resizes images (use `sharp`). The main thread sends image buffers and target dimensions. The worker returns resized buffers.

### Challenge 2: CSV Processor (Medium)
Process a 1GB CSV file using 4 workers. Split the file into chunks, send each chunk to a worker, merge results. Measure speedup vs single-threaded processing.

### Challenge 3: Distributed Worker Pool (Hard)
Create a worker pool that distributes tasks across multiple Node.js processes (not just threads) using IPC. Implement load balancing — send tasks to the process with the shortest queue.

---

## Troubleshooting Flowchart

```
Worker not responding
  → Check: Is the worker script path absolute?
    → No → Use resolve(__dirname, 'worker.js')
    → Yes → Check: Does the script have syntax errors?
      → Run: node --check worker.js
      → Errors found → Fix syntax
      → No errors → Check: Is the worker stuck in an infinite loop?
        → Add a timeout to worker.terminate()

Process hangs on exit
  → Check: Are there open MessageChannel ports?
    → Yes → Call port.close()
    → No → Check: Is the worker still running?
      → Yes → Call worker.terminate()
      → No → Check: Are there open event listeners?
        → Remove listeners or close the event emitter

DataCloneError on postMessage
  → Check: Are you passing a function?
    → Yes → Remove the function, pass only data
    → No → Check: Are you passing a class instance?
      → Yes → Convert to a plain object first
      → No → Check: Are there circular references?
        → Yes → Break the circular reference
```

---

## Code Review Checklist

- [ ] Worker script path is absolute (using `resolve(__dirname, ...)`)
- [ ] All worker messages are handled (message, error, exit)
- [ ] Workers have a timeout to prevent infinite computation
- [ ] Worker count is bounded (pool pattern)
- [ ] Workers are terminated on application shutdown
- [ ] No functions or class instances passed via postMessage
- [ ] Errors in workers are caught and sent back via postMessage
- [ ] SharedArrayBuffer access is protected with Atomics
- [ ] Worker pool gracefully handles worker crashes
- [ ] Performance is measured and compared to single-threaded baseline

---

## Real-World Case Study

**Cloudflare Workers** uses a model similar to Node.js worker threads at the edge. Each HTTP request is handled in an isolated V8 context. Key lessons:

- Worker creation must be fast (< 5ms) — they pre-warm isolates in a pool
- Memory per worker is critical — they limit each isolate to 128MB
- Communication between workers uses SharedArrayBuffer for zero-copy data sharing
- Isolation is security-critical — one worker must not access another's data

---

## Migration & Compatibility

### Node.js Version Differences

| Feature | v14 | v16 | v18 | v20+ |
|---------|-----|-----|-----|------|
| `workerData` | ✓ | ✓ | ✓ | ✓ |
| `SharedArrayBuffer` | ✓ | ✓ | ✓ | ✓ |
| `receiveMessageOnPort` | ✓ | ✓ | ✓ | ✓ |
| `isInternalThread` | — | — | ✓ | ✓ |
| `worker.performance` | — | — | — | ✓ |

---

## Next Steps

You know how to create workers and communicate with them. Next, learn how threads can share memory directly instead of copying data. Continue to [Shared Memory](./03-shared-memory.md).
