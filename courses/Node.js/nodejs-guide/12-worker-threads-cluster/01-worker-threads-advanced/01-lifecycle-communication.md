# Worker Thread Lifecycle, Communication, and Error Handling

## What You'll Learn

- Worker thread lifecycle management
- Message passing protocols between threads
- SharedArrayBuffer and Atomics for zero-copy data sharing
- Error handling and recovery strategies for workers
- Worker thread performance benchmarking

## Worker Thread Lifecycle

```
Worker Thread Lifecycle:
─────────────────────────────────────────────
Main Thread                    Worker Thread
    │                              │
    │── new Worker(file) ─────────►│  (init)
    │                              │── Loads script
    │◄── 'online' event ──────────│  (ready)
    │                              │── Executes code
    │── worker.postMessage() ─────►│  (processing)
    │◄── 'message' event ─────────│  (result)
    │                              │
    │── worker.terminate() ───────►│  (shutdown)
    │◄── 'exit' event ────────────│  (done)
    │── OR ─── 'error' event ─────│  (crashed)
    │         'messageerror' ─────│  (bad data)
```

## Advanced Worker Communication

```js
// workers/compute.js — Worker that handles multiple task types
import { parentPort, workerData, threadId } from 'node:worker_threads';

console.log(`Worker ${threadId} started`);

// Task handlers registry
const handlers = {
    fibonacci: (n) => {
        if (n <= 1) return n;
        let a = 0, b = 1;
        for (let i = 2; i <= n; i++) {
            [a, b] = [b, a + b];
        }
        return b;
    },

    primeFactors: (n) => {
        const factors = [];
        let d = 2;
        while (n > 1) {
            while (n % d === 0) {
                factors.push(d);
                n /= d;
            }
            d++;
        }
        return factors;
    },

    matrixMultiply: ({ a, b }) => {
        const rows = a.length;
        const cols = b[0].length;
        const inner = b.length;
        const result = Array.from({ length: rows }, () => new Array(cols).fill(0));

        for (let i = 0; i < rows; i++) {
            for (let j = 0; j < cols; j++) {
                for (let k = 0; k < inner; k++) {
                    result[i][j] += a[i][k] * b[k][j];
                }
            }
        }
        return result;
    },
};

// Message handler — receives tasks from main thread
parentPort.on('message', async ({ id, type, data }) => {
    try {
        const handler = handlers[type];
        if (!handler) {
            parentPort.postMessage({
                id,
                error: `Unknown task type: ${type}`,
            });
            return;
        }

        const start = performance.now();
        const result = handler(data);
        const elapsed = performance.now() - start;

        parentPort.postMessage({
            id,
            result,
            elapsed: +elapsed.toFixed(2),
            threadId,
        });
    } catch (err) {
        parentPort.postMessage({
            id,
            error: err.message,
            stack: err.stack,
        });
    }
});

// Handle termination gracefully
parentPort.on('close', () => {
    console.log(`Worker ${threadId} shutting down`);
});
```

## Main Thread Worker Manager

```js
// lib/worker-manager.js — Manages communication with a single worker
import { Worker } from 'node:worker_threads';
import { randomUUID } from 'node:crypto';
import { EventEmitter } from 'node:events';

class WorkerManager extends EventEmitter {
    constructor(workerPath, options = {}) {
        super();
        this.workerPath = workerPath;
        this.options = options;
        this.pending = new Map(); // id → { resolve, reject, timeout }
        this.worker = null;
        this.ready = false;
    }

    async start() {
        return new Promise((resolve, reject) => {
            this.worker = new Worker(this.workerPath, this.options);

            this.worker.on('online', () => {
                this.ready = true;
                this.emit('ready');
                resolve();
            });

            this.worker.on('message', (msg) => {
                const pending = this.pending.get(msg.id);
                if (pending) {
                    clearTimeout(pending.timeout);
                    this.pending.delete(msg.id);

                    if (msg.error) {
                        pending.reject(new Error(msg.error));
                    } else {
                        pending.resolve(msg);
                    }
                }
                this.emit('message', msg);
            });

            this.worker.on('error', (err) => {
                this.emit('error', err);
                // Reject all pending tasks
                for (const [id, pending] of this.pending) {
                    pending.reject(err);
                }
                this.pending.clear();
            });

            this.worker.on('exit', (code) => {
                this.ready = false;
                this.emit('exit', code);

                if (code !== 0) {
                    reject(new Error(`Worker exited with code ${code}`));
                }
            });
        });
    }

    async execute(type, data, timeoutMs = 30000) {
        if (!this.ready) throw new Error('Worker not ready');

        const id = randomUUID();

        return new Promise((resolve, reject) => {
            const timeout = setTimeout(() => {
                this.pending.delete(id);
                reject(new Error(`Task ${id} timed out after ${timeoutMs}ms`));
            }, timeoutMs);

            this.pending.set(id, { resolve, reject, timeout });
            this.worker.postMessage({ id, type, data });
        });
    }

    get pendingCount() {
        return this.pending.size;
    }

    async terminate() {
        if (this.worker) {
            // Wait for pending tasks or force terminate after timeout
            if (this.pending.size > 0) {
                await Promise.race([
                    Promise.all([...this.pending.values()].map(p =>
                        new Promise(r => { p.resolve = r; p.reject = r; })
                    )),
                    new Promise(r => setTimeout(r, 5000)),
                ]);
            }
            await this.worker.terminate();
            this.worker = null;
            this.ready = false;
        }
    }
}

export { WorkerManager };
```

## SharedArrayBuffer and Atomics

```js
// shared-memory-demo.js — Zero-copy data sharing between threads
import { Worker, isMainThread, workerData, parentPort } from 'node:worker_threads';

if (isMainThread) {
    // Main thread: create shared buffer
    const sharedBuffer = new SharedArrayBuffer(1024); // 1KB shared
    const sharedArray = new Int32Array(sharedBuffer);
    const lock = new Int32Array(new SharedArrayBuffer(4)); // Lock variable

    // Initialize shared data
    sharedArray[0] = 0; // Counter at index 0

    const worker = new Worker('./shared-memory-demo.js', {
        workerData: { sharedBuffer, lock },
    });

    // Main thread increments counter
    for (let i = 0; i < 100000; i++) {
        // Acquire lock
        while (Atomics.compareExchange(lock, 0, 0, 1) !== 0) {
            Atomics.wait(lock, 0, 1); // Wait if locked
        }

        sharedArray[0]++; // Critical section

        // Release lock
        Atomics.store(lock, 0, 0);
        Atomics.notify(lock, 0, 1);
    }

    worker.on('message', () => {
        console.log(`Final counter: ${sharedArray[0]}`);
        console.log(`Expected: 200000 (100000 from each thread)`);
    });

} else {
    // Worker thread: access same shared buffer
    const { sharedBuffer, lock } = workerData;
    const sharedArray = new Int32Array(sharedBuffer);

    for (let i = 0; i < 100000; i++) {
        // Acquire lock
        while (Atomics.compareExchange(lock, 0, 0, 1) !== 0) {
            Atomics.wait(lock, 0, 1);
        }

        sharedArray[0]++; // Critical section

        // Release lock
        Atomics.store(lock, 0, 0);
        Atomics.notify(lock, 0, 1);
    }

    parentPort.postMessage('done');
}
```

## Error Handling and Recovery

```js
// lib/resilient-worker.js — Worker with automatic restart on crash
import { Worker } from 'node:worker_threads';
import { EventEmitter } from 'node:events';

class ResilientWorker extends EventEmitter {
    constructor(workerPath, options = {}) {
        super();
        this.workerPath = workerPath;
        this.options = options;
        this.maxRestarts = options.maxRestarts || 5;
        this.restartDelay = options.restartDelay || 1000;
        this.restartCount = 0;
        this.worker = null;
        this.start();
    }

    async start() {
        this.worker = new Worker(this.workerPath, this.options);

        this.worker.on('message', (msg) => this.emit('message', msg));

        this.worker.on('error', (err) => {
            this.emit('error', err);
            this.handleCrash(err);
        });

        this.worker.on('exit', (code) => {
            this.emit('exit', code);
            if (code !== 0) {
                this.handleCrash(new Error(`Exit code ${code}`));
            }
        });
    }

    async handleCrash(error) {
        this.restartCount++;

        if (this.restartCount > this.maxRestarts) {
            this.emit('maxRestarts', error);
            return;
        }

        const delay = this.restartDelay * Math.pow(2, this.restartCount - 1);
        console.log(`Worker restarting in ${delay}ms (attempt ${this.restartCount})`);

        await new Promise(r => setTimeout(r, delay));
        await this.start();
        this.emit('restarted', this.restartCount);
    }

    postMessage(msg) {
        if (this.worker) this.worker.postMessage(msg);
    }

    async terminate() {
        if (this.worker) await this.worker.terminate();
    }
}
```

## Performance Benchmarks

```
Worker Thread Performance (fibonacci(40)):
─────────────────────────────────────────────
Scenario                      Time      Main Thread Blocked
─────────────────────────────────────────────
Main thread (blocking)        1100ms    Yes (1100ms)
Single worker                 1100ms    No (0ms)
Worker pool (4 workers)       280ms     No (0ms)
SharedArrayBuffer (4 threads) 260ms     No (0ms)

Worker creation overhead:     ~50ms per worker
Message passing overhead:     ~0.01ms per message (small payloads)
SharedArrayBuffer overhead:   ~0.001ms per access (no copy)
```

## Common Mistakes

- Not handling worker errors (causes silent crashes)
- Not setting timeouts on worker tasks (hangs forever)
- Creating a new worker per request (50ms overhead each)
- Using shared memory without proper synchronization (race conditions)

## Try It Yourself

### Exercise 1: Build a Task Dispatcher
Create a main thread that dispatches 100 fibonacci tasks to a single worker and measures total time.

### Exercise 2: Shared Counter
Use SharedArrayBuffer to implement a counter incremented by 4 worker threads without race conditions.

### Exercise 3: Error Recovery
Implement a resilient worker that restarts 3 times before giving up.

## Next Steps

Continue to [Worker Pool Pattern](./04-worker-pool-pattern.md) for production-ready worker management.
