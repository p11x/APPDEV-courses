# Worker Pool Implementation and Optimization

## What You'll Learn

- Production-ready worker pool design
- Dynamic pool sizing
- Task queuing and scheduling
- Monitoring and metrics

## Production Worker Pool

```javascript
import { Worker } from 'node:worker_threads';
import os from 'node:os';

class ProductionWorkerPool {
    constructor(options = {}) {
        this.script = options.script;
        this.minWorkers = options.minWorkers || 2;
        this.maxWorkers = options.maxWorkers || os.cpus().length;
        this.idleTimeout = options.idleTimeout || 30000;

        this.workers = [];
        this.taskQueue = [];
        this.metrics = { completed: 0, failed: 0, total: 0 };

        // Initialize minimum workers
        for (let i = 0; i < this.minWorkers; i++) {
            this.createWorker();
        }
    }

    createWorker() {
        const worker = new Worker(this.script);
        worker.busy = false;
        worker.idleSince = Date.now();

        worker.on('message', (result) => {
            this.metrics.completed++;
            worker.resolve(result);
            worker.busy = false;
            worker.idleSince = Date.now();
            this.scheduleNext();
        });

        worker.on('error', (err) => {
            this.metrics.failed++;
            worker.reject(err);
            worker.busy = false;
            this.removeWorker(worker);
            this.createWorker(); // Replace crashed worker
            this.scheduleNext();
        });

        this.workers.push(worker);
        return worker;
    }

    removeWorker(worker) {
        worker.terminate();
        this.workers = this.workers.filter(w => w !== worker);
    }

    scheduleNext() {
        if (this.taskQueue.length === 0) return;

        const idleWorker = this.workers.find(w => !w.busy);
        if (!idleWorker) {
            // Scale up if under max
            if (this.workers.length < this.maxWorkers) {
                const newWorker = this.createWorker();
                this.assignTask(newWorker);
            }
            return;
        }

        this.assignTask(idleWorker);
    }

    assignTask(worker) {
        if (this.taskQueue.length === 0) return;

        const { data, resolve, reject } = this.taskQueue.shift();
        worker.busy = true;
        worker.resolve = resolve;
        worker.reject = reject;
        worker.postMessage(data);
    }

    run(data) {
        this.metrics.total++;

        return new Promise((resolve, reject) => {
            this.taskQueue.push({ data, resolve, reject });
            this.scheduleNext();
        });
    }

    // Scale down idle workers
    pruneIdle() {
        const now = Date.now();
        for (const worker of this.workers) {
            if (!worker.busy &&
                this.workers.length > this.minWorkers &&
                now - worker.idleSince > this.idleTimeout) {
                this.removeWorker(worker);
            }
        }
    }

    getStats() {
        return {
            workers: this.workers.length,
            busy: this.workers.filter(w => w.busy).length,
            idle: this.workers.filter(w => !w.busy).length,
            queued: this.taskQueue.length,
            metrics: { ...this.metrics },
        };
    }

    async terminate() {
        await Promise.all(this.workers.map(w => w.terminate()));
    }
}
```

## Best Practices Checklist

- [ ] Set min/max worker bounds
- [ ] Implement idle worker pruning
- [ ] Monitor pool statistics
- [ ] Replace crashed workers automatically
- [ ] Size pools based on CPU cores
- [ ] Test under load for optimal sizing

## Cross-References

- See [Parallel Processing](./01-parallel-processing.md) for worker basics
- See [Shared Memory](./02-shared-memory-atomics.md) for shared state
- See [Concurrency](../14-concurrency-parallelism/01-async-optimization.md) for patterns

## Next Steps

Continue to [Process Lifecycle](../09-process-lifecycle/01-signal-handling.md) for lifecycle management.
