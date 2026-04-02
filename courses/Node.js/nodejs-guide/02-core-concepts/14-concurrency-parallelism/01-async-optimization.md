# Async Operation Optimization

## What You'll Learn

- Promise.all vs Promise.allSettled patterns
- Async concurrency control
- Queue-based processing
- Backpressure handling

## Parallel Execution

```javascript
// Promise.all — fail fast
const results = await Promise.all([
    fetchUser(id),
    fetchOrders(id),
    fetchRecommendations(id),
]);

// Promise.allSettled — collect all results
const results = await Promise.allSettled([
    service1.call(),
    service2.call(),
    service3.call(),
]);

const succeeded = results.filter(r => r.status === 'fulfilled').map(r => r.value);
const failed = results.filter(r => r.status === 'rejected').map(r => r.reason);
```

## Concurrency Control

```javascript
class AsyncQueue {
    constructor(concurrency = 5) {
        this.concurrency = concurrency;
        this.running = 0;
        this.queue = [];
    }

    async add(fn) {
        return new Promise((resolve, reject) => {
            this.queue.push({ fn, resolve, reject });
            this.process();
        });
    }

    async process() {
        while (this.running < this.concurrency && this.queue.length > 0) {
            const { fn, resolve, reject } = this.queue.shift();
            this.running++;

            try {
                const result = await fn();
                resolve(result);
            } catch (err) {
                reject(err);
            } finally {
                this.running--;
                this.process();
            }
        }
    }
}

// Usage: Process 100 items with 5 concurrent operations
const queue = new AsyncQueue(5);

const results = await Promise.all(
    items.map(item => queue.add(() => processItem(item)))
);
```

## Batch Processing

```javascript
async function batchProcess(items, fn, batchSize = 100) {
    const results = [];

    for (let i = 0; i < items.length; i += batchSize) {
        const batch = items.slice(i, i + batchSize);
        const batchResults = await Promise.all(batch.map(fn));
        results.push(...batchResults);

        // Yield to event loop between batches
        await new Promise(r => setImmediate(r));
    }

    return results;
}

// Usage
const users = await batchProcess(userIds, async (id) => {
    return await getUser(id);
}, 50);
```

## Best Practices Checklist

- [ ] Use Promise.all for parallel independent operations
- [ ] Use Promise.allSettled when some failures are acceptable
- [ ] Control concurrency to avoid overwhelming resources
- [ ] Batch large operations with yields between batches
- [ ] Monitor queue depth and processing times

## Cross-References

- See [Worker Threads](./02-worker-thread-parallelism.md) for CPU parallelism
- See [Concurrency Control](./03-concurrency-control.md) for synchronization
- See [Performance](../12-performance-optimization/01-cpu-memory-optimization.md) for optimization

## Next Steps

Continue to [Worker Thread Parallelism](./02-worker-thread-parallelism.md) for CPU parallelism.
