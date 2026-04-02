# Worker Thread Performance Optimization and Profiling

## What You'll Learn

- Worker thread performance profiling
- Memory optimization with workers
- Worker pool sizing strategies
- Performance bottleneck identification
- Benchmarking worker configurations

## Worker Performance Profiler

```js
// lib/worker-profiler.js — Profile worker thread performance
import { Worker } from 'node:worker_threads';
import { performance } from 'node:perf_hooks';

class WorkerProfiler {
    constructor(workerPath, options = {}) {
        this.workerPath = workerPath;
        this.poolSize = options.poolSize || 4;
        this.samples = [];
    }

    async profile(taskType, data, iterations = 100) {
        const worker = new Worker(this.workerPath);
        const timings = [];

        // Wait for worker to be ready
        await new Promise(r => worker.on('online', r));

        // Warmup
        for (let i = 0; i < 5; i++) {
            worker.postMessage({ id: 'warmup', type: taskType, data });
            await new Promise(r => worker.on('message', r));
        }

        // Profile
        for (let i = 0; i < iterations; i++) {
            const start = performance.now();

            await new Promise((resolve, reject) => {
                const timeout = setTimeout(() => reject(new Error('Timeout')), 30000);

                worker.once('message', (msg) => {
                    clearTimeout(timeout);
                    timings.push(performance.now() - start);
                    resolve(msg);
                });

                worker.postMessage({ id: i, type: taskType, data });
            });
        }

        await worker.terminate();

        timings.sort((a, b) => a - b);

        return {
            taskType,
            iterations,
            mean: +(timings.reduce((a, b) => a + b) / timings.length).toFixed(3),
            median: +timings[Math.floor(timings.length / 2)].toFixed(3),
            p95: +timings[Math.floor(timings.length * 0.95)].toFixed(3),
            p99: +timings[Math.floor(timings.length * 0.99)].toFixed(3),
            min: +timings[0].toFixed(3),
            max: +timings[timings.length - 1].toFixed(3),
        };
    }

    async comparePoolSizes(taskType, data) {
        const results = [];

        for (const poolSize of [1, 2, 4, 8, 16]) {
            const start = performance.now();

            const workers = Array.from({ length: poolSize }, () => new Worker(this.workerPath));
            await Promise.all(workers.map(w => new Promise(r => w.on('online', r))));

            // Distribute 1000 tasks across pool
            const tasks = Array.from({ length: 1000 }, (_, i) => i);
            let taskIndex = 0;

            const execute = async (worker) => {
                while (taskIndex < tasks.length) {
                    const currentTask = taskIndex++;
                    await new Promise((resolve) => {
                        worker.once('message', resolve);
                        worker.postMessage({ id: currentTask, type: taskType, data });
                    });
                }
            };

            await Promise.all(workers.map(execute));

            const elapsed = performance.now() - start;
            results.push({ poolSize, elapsed: Math.round(elapsed), throughput: Math.round(1000 / elapsed * 1000) });

            await Promise.all(workers.map(w => w.terminate()));
        }

        return results;
    }
}

// Usage
const profiler = new WorkerProfiler('./workers/compute.js');

const result = await profiler.profile('fibonacci', 35, 50);
console.log(result);
// { taskType: 'fibonacci', iterations: 50, mean: 52.3, median: 51.8, ... }

const poolResults = await profiler.comparePoolSizes('fibonacci', 35);
console.table(poolResults);
// ┌───────────┬─────────┬────────────┐
// │ poolSize  │ elapsed │ throughput │
// ├───────────┼─────────┼────────────┤
// │ 1         │ 2615    │ 382        │
// │ 2         │ 1340    │ 746        │
// │ 4         │ 720     │ 1389       │
// │ 8         │ 680     │ 1471       │
// │ 16        │ 690     │ 1449       │
// └───────────┴─────────┴────────────┘
```

## Memory Optimization

```js
// memory-optimization.js — Worker memory management strategies
import { Worker, parentPort, workerData } from 'node:worker_threads';

// Strategy 1: Transfer ownership (zero-copy for ArrayBuffers)
if (isMainThread) {
    const buffer = new ArrayBuffer(1024 * 1024); // 1MB
    const view = new Uint8Array(buffer);
    view.fill(42);

    // Transfer ownership — buffer becomes unusable in main thread
    const worker = new Worker('./memory-optimization.js', {
        workerData: { action: 'process' },
    });
    worker.postMessage({ buffer }, [buffer]); // Transfer list
    // buffer.byteLength is now 0 in main thread

} else {
    parentPort.on('message', ({ buffer }) => {
        // Worker now owns this buffer — no copy occurred
        const view = new Uint8Array(buffer);
        console.log(`Received buffer of ${buffer.byteLength} bytes`);
        console.log(`First byte: ${view[0]}`); // 42
    });
}

// Strategy 2: Reuse buffers across tasks
class BufferPool {
    constructor(size, count) {
        this.buffers = Array.from({ length: count }, () => new ArrayBuffer(size));
        this.available = [...this.buffers];
    }

    acquire() {
        return this.available.pop() || new ArrayBuffer(this.buffers[0].byteLength);
    }

    release(buf) {
        if (this.available.length < this.buffers.length) {
            this.available.push(buf);
        }
    }
}

// Strategy 3: Lazy initialization
parentPort.on('message', async ({ id, type, data }) => {
    // Initialize heavy resources only when needed
    if (type === 'init-model') {
        // Load ML model only once
        if (!globalThis.model) {
            globalThis.model = await loadModel(data.modelPath);
        }
        parentPort.postMessage({ id, result: 'initialized' });
    }
});
```

## Optimal Pool Sizing

```
Pool Size Guidelines:
─────────────────────────────────────────────
Task Type              Optimal Pool Size
─────────────────────────────────────────────
CPU-bound (pure math)  num_cpus (no more)
I/O mixed              num_cpus * 2
Image processing       num_cpus (memory-bound)
File parsing           num_cpus * 1.5
ML inference           num_cpus (GPU if available)

Memory consideration:
├── Each worker: ~30-50MB base heap
├── 4 workers:   ~120-200MB overhead
├── 8 workers:   ~240-400MB overhead
└── Formula: overhead = pool_size * worker_heap_size
```

## Common Mistakes

- Pool size larger than CPU count (context switching overhead)
- Not reusing workers (50ms creation cost per worker)
- Not profiling before optimizing
- Sending large objects through postMessage (use transfer instead)

## Try It Yourself

### Exercise 1: Profile Different Pool Sizes
Profile fibonacci(40) with pool sizes 1, 2, 4, 8. Find the sweet spot.

### Exercise 2: Measure Memory
Monitor heap usage with different pool sizes using `process.memoryUsage()`.

### Exercise 3: Transfer vs Copy
Benchmark transfer list vs default copy for 10MB buffers.

## Next Steps

Continue to [Security and Isolation](../06-security-isolation/01-sandboxing.md).
