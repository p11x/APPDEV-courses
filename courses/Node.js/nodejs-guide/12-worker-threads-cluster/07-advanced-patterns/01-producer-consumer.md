# Advanced Worker Patterns: Producer-Consumer, Pipeline, Farm

## What You'll Learn

- Producer-consumer pattern with workers
- Pipeline pattern for multi-stage processing
- Worker farm pattern for task distribution
- Work stealing for load balancing
- Divide and conquer with workers

## Producer-Consumer Pattern

```js
// producer-consumer.js — Queue-based task distribution
import { Worker } from 'node:worker_threads';
import { EventEmitter } from 'node:events';

class TaskQueue extends EventEmitter {
    constructor() {
        super();
        this.queue = [];
        this.workers = [];
        this.processing = 0;
    }

    addWorker(workerPath) {
        const worker = new Worker(workerPath);
        this.workers.push({ worker, busy: false });
        return this;
    }

    enqueue(task) {
        this.queue.push(task);
        this.processNext();
    }

    processNext() {
        const available = this.workers.find(w => !w.busy);
        if (!available || this.queue.length === 0) return;

        const task = this.queue.shift();
        available.busy = true;
        this.processing++;

        available.worker.once('message', (result) => {
            available.busy = false;
            this.processing--;
            this.emit('taskComplete', { task, result });
            this.processNext(); // Process next queued task
        });

        available.worker.postMessage(task);
    }

    async drain() {
        return new Promise((resolve) => {
            const check = () => {
                if (this.queue.length === 0 && this.processing === 0) {
                    resolve();
                } else {
                    setTimeout(check, 50);
                }
            };
            check();
        });
    }
}
```

## Pipeline Pattern

```js
// pipeline.js — Multi-stage processing pipeline with workers
import { Worker } from 'node:worker_threads';

class WorkerPipeline {
    constructor(stages) {
        // stages: [{ workerPath, concurrency }]
        this.stages = stages.map(stage => ({
            workers: Array.from(
                { length: stage.concurrency || 1 },
                () => new Worker(stage.workerPath)
            ),
            queue: [],
            index: 0,
        }));
    }

    async process(data) {
        let current = data;

        for (let i = 0; i < this.stages.length; i++) {
            const stage = this.stages[i];
            const worker = stage.workers[stage.index % stage.workers.length];
            stage.index++;

            current = await new Promise((resolve, reject) => {
                const timeout = setTimeout(() => reject(new Error('Pipeline stage timeout')), 30000);
                worker.once('message', (msg) => {
                    clearTimeout(timeout);
                    resolve(msg.result);
                });
                worker.postMessage({ stage: i, data: current });
            });
        }

        return current;
    }

    async processBatch(items) {
        return Promise.all(items.map(item => this.process(item)));
    }

    async terminate() {
        for (const stage of this.stages) {
            await Promise.all(stage.workers.map(w => w.terminate()));
        }
    }
}

// Usage: Image processing pipeline
const pipeline = new WorkerPipeline([
    { workerPath: './workers/resize.js', concurrency: 2 },
    { workerPath: './workers/filter.js', concurrency: 2 },
    { workerPath: './workers/compress.js', concurrency: 1 },
]);

const results = await pipeline.processBatch(images);
await pipeline.terminate();
```

## Worker Farm Pattern

```js
// worker-farm.js — Distribute tasks across worker farm
import { Worker } from 'node:worker_threads';
import os from 'node:os';

class WorkerFarm {
    constructor(workerPath, options = {}) {
        this.workerPath = workerPath;
        this.size = options.size || os.availableParallelism() || 4;
        this.workers = [];
        this.taskId = 0;
        this.pending = new Map();
    }

    async start() {
        for (let i = 0; i < this.size; i++) {
            const worker = new Worker(this.workerPath, {
                workerData: { farmId: i },
            });

            worker.on('message', (msg) => {
                const pending = this.pending.get(msg.id);
                if (pending) {
                    this.pending.delete(msg.id);
                    if (msg.error) pending.reject(new Error(msg.error));
                    else pending.resolve(msg.result);
                }
            });

            this.workers.push(worker);
        }
    }

    async execute(task) {
        const id = this.taskId++;

        // Find least busy worker (simple round-robin)
        const worker = this.workers[id % this.workers.length];

        return new Promise((resolve, reject) => {
            const timeout = setTimeout(() => {
                this.pending.delete(id);
                reject(new Error('Task timeout'));
            }, 60000);

            this.pending.set(id, { resolve, reject, timeout });
            worker.postMessage({ id, task });
        });
    }

    async executeAll(tasks) {
        return Promise.all(tasks.map(task => this.execute(task)));
    }

    getStats() {
        return {
            workers: this.workers.length,
            pendingTasks: this.pending.size,
        };
    }

    async terminate() {
        await Promise.all(this.workers.map(w => w.terminate()));
    }
}
```

## Divide and Conquer

```js
// divide-conquer.js — Recursive parallel computation
import { Worker, isMainThread, parentPort, workerData } from 'node:worker_threads';

if (isMainThread) {
    // Merge sort with parallel recursion
    async function parallelMergeSort(arr, depth = 0, maxDepth = 2) {
        if (arr.length <= 1) return arr;

        // Base case: sort sequentially for small arrays or deep recursion
        if (arr.length < 1000 || depth >= maxDepth) {
            return arr.sort((a, b) => a - b);
        }

        const mid = Math.floor(arr.length / 2);
        const left = arr.slice(0, mid);
        const right = arr.slice(mid);

        // Parallel: each half sorted in a worker
        const [sortedLeft, sortedRight] = await Promise.all([
            spawnSort(left, depth + 1, maxDepth),
            spawnSort(right, depth + 1, maxDepth),
        ]);

        // Merge
        return merge(sortedLeft, sortedRight);
    }

    function spawnSort(arr, depth, maxDepth) {
        return new Promise((resolve, reject) => {
            const worker = new Worker('./divide-conquer.js', {
                workerData: { arr, depth, maxDepth },
            });
            worker.on('message', resolve);
            worker.on('error', reject);
        });
    }

    function merge(left, right) {
        const result = [];
        let i = 0, j = 0;
        while (i < left.length && j < right.length) {
            if (left[i] <= right[j]) result.push(left[i++]);
            else result.push(right[j++]);
        }
        return result.concat(left.slice(i), right.slice(j));
    }

    // Usage
    const data = Array.from({ length: 100000 }, () => Math.random() * 100000);
    const sorted = await parallelMergeSort(data);

} else {
    // Worker: recursively sort or merge
    const { arr, depth, maxDepth } = workerData;

    async function sort(arr, depth) {
        if (arr.length <= 1) return arr;
        if (arr.length < 1000 || depth >= maxDepth) {
            return arr.sort((a, b) => a - b);
        }

        const mid = Math.floor(arr.length / 2);
        const [left, right] = await Promise.all([
            spawnSort(arr.slice(0, mid), depth + 1),
            spawnSort(arr.slice(mid), depth + 1),
        ]);

        return merge(left, right);
    }

    const result = await sort(arr, depth);
    parentPort.postMessage(result);
}
```

## Common Mistakes

- Not back-pressuring the task queue (memory grows unbounded)
- Recursive parallelism without depth limit (exponential worker creation)
- Not handling pipeline stage failures
- Ignoring worker farm load imbalance

## Try It Yourself

### Exercise 1: Producer-Consumer
Enqueue 1000 tasks and process with 4 workers. Verify all complete.

### Exercise 2: Pipeline
Build a 3-stage pipeline: parse → transform → serialize.

### Exercise 3: Parallel Sort
Sort 1 million numbers with divide-and-conquer. Compare to sequential sort.

## Next Steps

Continue to [Debugging and Profiling](../08-debugging-profiling/01-debugging-techniques.md).
