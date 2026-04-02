# Shared Memory and Atomics

## What You'll Learn

- SharedArrayBuffer for cross-thread memory
- Atomics for thread-safe operations
- Lock-free data structures
- Performance implications of shared memory

## SharedArrayBuffer

```javascript
// Create shared memory
const sharedBuffer = new SharedArrayBuffer(1024); // 1KB shared

// Create typed views
const sharedInt = new Int32Array(sharedBuffer);
const sharedFloat = new Float64Array(sharedBuffer);

// Both threads see same memory
sharedInt[0] = 42;
// Other thread: Atomics.load(sharedInt, 0) === 42
```

## Atomics Operations

```javascript
import { Worker, workerData, parentPort, isMainThread } from 'node:worker_threads';

if (isMainThread) {
    const sharedBuffer = new SharedArrayBuffer(16);
    const sharedArray = new Int32Array(sharedBuffer);

    const worker = new Worker(new URL(import.meta.url), {
        workerData: sharedBuffer,
    });

    // Wait for worker to signal
    Atomics.wait(sharedArray, 0, 0); // Blocks until sharedArray[0] !== 0

    console.log('Value:', Atomics.load(sharedArray, 0));
    console.log('Sum:', Atomics.load(sharedArray, 1));
} else {
    const sharedArray = new Int32Array(workerData);

    // Atomic operations (thread-safe)
    Atomics.add(sharedArray, 0, 100);    // Atomic add
    Atomics.add(sharedArray, 1, 200);    // Atomic add to index 1
    Atomics.store(sharedArray, 0, 42);   // Atomic store
    Atomics.compareExchange(sharedArray, 0, 42, 99); // CAS

    // Signal main thread
    Atomics.notify(sharedArray, 0);
}
```

## Lock-Free Counter

```javascript
// Thread-safe counter without locks
class AtomicCounter {
    constructor(sharedBuffer) {
        this.buffer = sharedBuffer;
        this.view = new Int32Array(sharedBuffer);
    }

    increment() {
        return Atomics.add(this.view, 0, 1);
    }

    decrement() {
        return Atomics.sub(this.view, 0, 1);
    }

    get value() {
        return Atomics.load(this.view, 0);
    }

    reset() {
        Atomics.store(this.view, 0, 0);
    }
}

// Usage across workers
const sharedBuffer = new SharedArrayBuffer(4);
const counter = new AtomicCounter(sharedBuffer);

// Each worker increments
counter.increment();
```

## Best Practices Checklist

- [ ] Use Atomics for all shared memory access
- [ ] Prefer lock-free algorithms when possible
- [ ] Use Atomics.wait/notify for synchronization
- [ ] Keep shared memory regions small
- [ ] Test for race conditions thoroughly

## Cross-References

- See [Parallel Processing](./01-parallel-processing.md) for worker basics
- See [Worker Pool](./03-worker-pool-optimization.md) for pool tuning
- See [Concurrency](../14-concurrency-parallelism/01-async-optimization.md) for patterns

## Next Steps

Continue to [Worker Pool Optimization](./03-worker-pool-optimization.md) for pool tuning.
