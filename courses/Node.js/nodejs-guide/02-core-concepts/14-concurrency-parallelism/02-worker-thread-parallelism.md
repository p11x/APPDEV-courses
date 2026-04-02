# Worker Thread Parallelism Patterns

## What You'll Learn

- CPU parallelism with worker threads
- Data parallelism patterns
- Pipeline parallelism
- Performance benchmarks

## Data Parallelism

```javascript
import { Worker } from 'node:worker_threads';

// Split data across workers
async function parallelMap(items, workerScript, numWorkers = 4) {
    const chunkSize = Math.ceil(items.length / numWorkers);
    const workers = [];

    for (let i = 0; i < numWorkers; i++) {
        const chunk = items.slice(i * chunkSize, (i + 1) * chunkSize);
        workers.push(runWorker(workerScript, chunk));
    }

    const results = await Promise.all(workers);
    return results.flat();
}

// Benchmark: 1M items
// Single thread: 1200ms
// 4 workers: 350ms (~3.4x speedup)
```

## Pipeline Parallelism

```javascript
// Each stage runs in its own worker
const stages = [
    './workers/parse.js',
    './workers/validate.js',
    './workers/transform.js',
    './workers/save.js',
];

async function pipeline(input) {
    let data = input;
    for (const stage of stages) {
        data = await runWorker(stage, data);
    }
    return data;
}
```

## Performance Comparison

```
Parallelism Model Comparison (1M items):
─────────────────────────────────────────────
Sequential:        ████████████████████  1200ms
Promise.all:       ████████████████████  1150ms (still single thread)
4 Worker Threads:  ████████              350ms  (3.4x speedup)
8 Worker Threads:  █████                 280ms  (4.3x speedup)

Key insight: Workers help CPU-bound, not I/O-bound
I/O-bound work benefits from async, not threads
```

## Best Practices Checklist

- [ ] Use workers only for CPU-bound tasks
- [ ] Size worker pools based on CPU cores
- [ ] Minimize data transfer between threads
- [ ] Use SharedArrayBuffer for large shared data
- [ ] Benchmark to verify parallelism benefit

## Cross-References

- See [Async Optimization](./01-async-optimization.md) for I/O patterns
- See [Concurrency Control](./03-concurrency-control.md) for synchronization
- See [Worker Threads](../08-worker-threads/01-parallel-processing.md) for worker details

## Next Steps

Continue to [Concurrency Control](./03-concurrency-control.md) for synchronization patterns.
