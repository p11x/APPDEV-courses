# Parallel Processing with Worker Threads

## What You'll Learn

- Creating and managing worker threads
- Communication between main and worker
- SharedArrayBuffer for shared memory
- Worker pool implementation

## Basic Worker Threads

```javascript
// main.js
import { Worker, isMainThread, parentPort, workerData } from 'node:worker_threads';

if (isMainThread) {
    // Main thread: create worker
    const worker = new Worker(new URL(import.meta.url), {
        workerData: { n: 40 },
    });

    worker.on('message', (result) => {
        console.log(`Fibonacci(40) = ${result}`);
    });

    worker.on('error', (err) => console.error('Worker error:', err));
    worker.on('exit', (code) => console.log(`Worker exited: ${code}`));
} else {
    // Worker thread: compute
    const { n } = workerData;

    function fibonacci(n) {
        if (n <= 1) return n;
        return fibonacci(n - 1) + fibonacci(n - 2);
    }

    parentPort.postMessage(fibonacci(n));
}
```

## Worker Pool

```javascript
// worker-pool.js
import { Worker } from 'node:worker_threads';

class WorkerPool {
    constructor(script, size = 4) {
        this.script = script;
        this.workers = [];
        this.queue = [];

        for (let i = 0; i < size; i++) {
            this.addWorker();
        }
    }

    addWorker() {
        const worker = new Worker(this.script);
        worker.busy = false;

        worker.on('message', (result) => {
            worker.resolve(result);
            worker.busy = false;
            this.processQueue();
        });

        worker.on('error', (err) => {
            worker.reject(err);
            worker.busy = false;
        });

        this.workers.push(worker);
    }

    processQueue() {
        if (this.queue.length === 0) return;

        const worker = this.workers.find(w => !w.busy);
        if (!worker) return;

        const { data, resolve, reject } = this.queue.shift();
        worker.busy = true;
        worker.resolve = resolve;
        worker.reject = reject;
        worker.postMessage(data);
    }

    run(data) {
        return new Promise((resolve, reject) => {
            this.queue.push({ data, resolve, reject });
            this.processQueue();
        });
    }

    async terminate() {
        await Promise.all(this.workers.map(w => w.terminate()));
    }
}

// Usage
const pool = new WorkerPool('./compute.js', os.cpus().length);

const results = await Promise.all([
    pool.run({ type: 'fib', n: 40 }),
    pool.run({ type: 'fib', n: 42 }),
    pool.run({ type: 'hash', data: 'a'.repeat(1000000) }),
]);

await pool.terminate();
```

## SharedArrayBuffer

```javascript
// main.js — Shared memory between threads
import { Worker } from 'node:worker_threads';

const sharedBuffer = new SharedArrayBuffer(4); // 4 bytes
const sharedArray = new Int32Array(sharedBuffer);

const worker = new Worker('./counter-worker.js', {
    workerData: sharedBuffer,
});

// Both threads can read/write the same memory
worker.on('message', () => {
    console.log('Counter:', Atomics.load(sharedArray, 0));
});

// counter-worker.js
const { workerData } = require('worker_threads');
const sharedArray = new Int32Array(workerData);

for (let i = 0; i < 1000000; i++) {
    Atomics.add(sharedArray, 0, 1); // Atomic increment
}
parentPort.postMessage('done');
```

## Best Practices Checklist

- [ ] Use worker threads for CPU-bound tasks only
- [ ] Keep data transfers minimal (structured clone)
- [ ] Use SharedArrayBuffer + Atomics for shared state
- [ ] Implement worker pool for reusing threads
- [ ] Handle worker errors and crashes
- [ ] Size pools based on CPU cores

## Cross-References

- See [Shared Memory](./02-shared-memory-atomics.md) for shared state
- See [Worker Pool](./03-worker-pool-optimization.md) for pool tuning
- See [Child Processes](../07-child-processes/01-spawn-exec-fork.md) for process model

## Next Steps

Continue to [Shared Memory and Atomics](./02-shared-memory-atomics.md) for shared state.
