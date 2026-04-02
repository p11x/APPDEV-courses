# Parallel Processing Patterns with Worker Threads

## What You'll Learn

- CPU-bound task parallelization
- Image and file processing with workers
- Data processing pipelines with workers
- Map-reduce pattern implementation
- Divide and conquer with workers

## Worker Pool for CPU Tasks

```js
// lib/worker-pool.js — Production-ready worker thread pool
import { Worker } from 'node:worker_threads';
import { EventEmitter } from 'node:events';
import os from 'node:os';

class WorkerPool extends EventEmitter {
    constructor(workerPath, options = {}) {
        super();
        this.workerPath = workerPath;
        this.poolSize = options.size || os.availableParallelism() || 4;
        this.workers = [];
        this.taskQueue = [];
        this.activeWorkers = 0;
        this.stats = { completed: 0, failed: 0, totalTime: 0 };
    }

    async start() {
        for (let i = 0; i < this.poolSize; i++) {
            await this.addWorker(i);
        }
        this.emit('ready');
    }

    async addWorker(id) {
        const worker = new Worker(this.workerPath, {
            workerData: { workerId: id },
        });

        const workerInfo = {
            id,
            worker,
            busy: false,
            taskCount: 0,
        };

        worker.on('message', (msg) => {
            workerInfo.busy = false;
            this.activeWorkers--;

            if (workerInfo.currentTask) {
                const { resolve, reject, startTime } = workerInfo.currentTask;
                const elapsed = performance.now() - startTime;

                if (msg.error) {
                    this.stats.failed++;
                    reject(new Error(msg.error));
                } else {
                    this.stats.completed++;
                    this.stats.totalTime += elapsed;
                    resolve(msg.result);
                }

                workerInfo.currentTask = null;
                workerInfo.taskCount++;
            }

            // Process next task in queue
            this.processQueue();
        });

        worker.on('error', (err) => {
            workerInfo.busy = false;
            this.activeWorkers--;
            if (workerInfo.currentTask) {
                workerInfo.currentTask.reject(err);
                workerInfo.currentTask = null;
            }
            this.emit('workerError', { workerId: id, error: err });
            // Replace crashed worker
            this.addWorker(id);
        });

        this.workers[id] = workerInfo;
    }

    async execute(task) {
        return new Promise((resolve, reject) => {
            const taskItem = {
                task,
                resolve,
                reject,
                startTime: performance.now(),
            };

            // Find available worker
            const available = this.workers.find(w => w && !w.busy);
            if (available) {
                this.runTask(available, taskItem);
            } else {
                this.taskQueue.push(taskItem);
            }
        });
    }

    runTask(workerInfo, taskItem) {
        workerInfo.busy = true;
        workerInfo.currentTask = taskItem;
        this.activeWorkers++;
        workerInfo.worker.postMessage(taskItem.task);
    }

    processQueue() {
        while (this.taskQueue.length > 0) {
            const available = this.workers.find(w => w && !w.busy);
            if (!available) break;

            const taskItem = this.taskQueue.shift();
            this.runTask(available, taskItem);
        }
    }

    getStats() {
        return {
            poolSize: this.poolSize,
            activeWorkers: this.activeWorkers,
            queuedTasks: this.taskQueue.length,
            completed: this.stats.completed,
            failed: this.stats.failed,
            avgTime: this.stats.completed > 0
                ? +(this.stats.totalTime / this.stats.completed).toFixed(2)
                : 0,
        };
    }

    async terminate() {
        await Promise.all(
            this.workers.filter(Boolean).map(w => w.worker.terminate())
        );
        this.workers = [];
    }
}

export { WorkerPool };
```

## Parallel Image Processing

```js
// workers/image-processor.js — Image processing worker
import { parentPort, workerData } from 'node:worker_threads';
import sharp from 'sharp';

const handlers = {
    resize: async ({ buffer, width, height }) => {
        return sharp(buffer)
            .resize(width, height, { fit: 'inside' })
            .jpeg({ quality: 80 })
            .toBuffer();
    },

    thumbnail: async ({ buffer, size = 200 }) => {
        return sharp(buffer)
            .resize(size, size, { fit: 'cover' })
            .jpeg({ quality: 70 })
            .toBuffer();
    },

    compress: async ({ buffer, quality = 80 }) => {
        return sharp(buffer)
            .jpeg({ quality })
            .toBuffer();
    },

    metadata: async ({ buffer }) => {
        return sharp(buffer).metadata();
    },
};

parentPort.on('message', async ({ id, type, data }) => {
    try {
        const handler = handlers[type];
        if (!handler) throw new Error(`Unknown type: ${type}`);

        const result = await handler(data);
        parentPort.postMessage({ id, result });
    } catch (err) {
        parentPort.postMessage({ id, error: err.message });
    }
});
```

```js
// Usage: Process 100 images in parallel
import { WorkerPool } from './lib/worker-pool.js';
import { readdir, readFile, writeFile } from 'node:fs/promises';

const pool = new WorkerPool('./workers/image-processor.js', { size: 4 });
await pool.start();

const imageDir = './images';
const files = await readdir(imageDir);
const imageFiles = files.filter(f => /\.(jpg|jpeg|png)$/i.test(f));

console.log(`Processing ${imageFiles.length} images with ${pool.getStats().poolSize} workers`);

const start = performance.now();

const results = await Promise.all(
    imageFiles.map(async (file) => {
        const buffer = await readFile(`${imageDir}/${file}`);
        const thumbnail = await pool.execute({
            type: 'thumbnail',
            data: { buffer, size: 200 },
        });
        await writeFile(`./thumbnails/${file}`, thumbnail);
        return file;
    })
);

const elapsed = performance.now() - start;
console.log(`Processed ${results.length} images in ${elapsed.toFixed(0)}ms`);
console.log(`Average: ${(elapsed / results.length).toFixed(1)}ms per image`);
console.log('Pool stats:', pool.getStats());

await pool.terminate();
```

## Map-Reduce Pattern

```js
// map-reduce.js — Parallel map-reduce with worker threads
import { WorkerPool } from './lib/worker-pool.js';

// Worker for map phase
// workers/map-worker.js
import { parentPort } from 'node:worker_threads';

parentPort.on('message', ({ id, type, data }) => {
    if (type === 'map') {
        // Map: emit key-value pairs
        const words = data.text.toLowerCase().split(/\s+/);
        const pairs = words.map(word => ({ key: word, value: 1 }));
        parentPort.postMessage({ id, result: pairs });
    }
});

// Main: orchestrator
async function wordCount(texts) {
    const pool = new WorkerPool('./workers/map-worker.js', { size: 4 });
    await pool.start();

    // MAP phase: process chunks in parallel
    const mapResults = await Promise.all(
        texts.map(text => pool.execute({ type: 'map', data: { text } }))
    );

    // REDUCE phase: aggregate results
    const counts = {};
    for (const pairs of mapResults) {
        for (const { key, value } of pairs) {
            counts[key] = (counts[key] || 0) + value;
        }
    }

    await pool.terminate();
    return counts;
}

// Usage
const documents = [
    'the quick brown fox jumps over the lazy dog',
    'the lazy dog slept all day long',
    'quick brown foxes are fast',
];

const counts = await wordCount(documents);
console.log(counts);
// { the: 3, quick: 2, brown: 2, fox: 1, jumps: 1, ... }
```

## Performance Comparison

```
Parallel Processing Benchmarks:
─────────────────────────────────────────────
Task                  Sequential  4 Workers   Speedup
─────────────────────────────────────────────
100 image thumbnails  12,400ms    3,200ms     3.9x
Fibonacci(42) x8      8,800ms    2,400ms     3.7x
10K JSON parses       450ms       130ms       3.5x
Word count (100 docs) 890ms       240ms       3.7x
Matrix multiply 500x500 2,100ms   580ms       3.6x

Theoretical max: ~4x on 4 cores (limited by overhead)
```

## Common Mistakes

- Not measuring sequential time first (can't compare speedup)
- Sending large buffers through messages (use SharedArrayBuffer instead)
- Not using a worker pool (creating workers per task is expensive)
- Parallelizing I/O-bound tasks (event loop already handles these)

## Try It Yourself

### Exercise 1: Parallel Fibonacci
Compute fibonacci(42) 8 times in parallel. Compare total time to sequential.

### Exercise 2: Image Batch Processing
Process a directory of 50 images with 4 workers. Measure speedup.

### Exercise 3: Implement Word Count
Build a parallel word counter that processes 100 text files.

## Next Steps

Continue to [IPC Patterns](../04-ipc-patterns/01-message-passing.md).
