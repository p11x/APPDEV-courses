# Batch Processing with Async Control

## What You'll Learn

- Batch processing patterns
- Parallel vs sequential execution
- Resource limiting for async operations
- Queue-based processing

## Batch Processing

```javascript
async function batchProcess(items, fn, { batchSize = 100, concurrency = 5 } = {}) {
    const results = [];

    for (let i = 0; i < items.length; i += batchSize) {
        const batch = items.slice(i, i + batchSize);

        // Process batch with limited concurrency
        const batchResults = await Promise.all(
            batch.map(item => fn(item))
        );

        results.push(...batchResults);

        // Yield to event loop between batches
        await new Promise(r => setImmediate(r));
    }

    return results;
}

// Usage
const users = await batchProcess(userIds, async (id) => {
    return await getUser(id);
}, { batchSize: 50, concurrency: 10 });
```

## Async Queue

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

    get pending() { return this.queue.length; }
    get active() { return this.running; }
}

// Usage
const queue = new AsyncQueue(3);

// All run with max 3 concurrent operations
const results = await Promise.all(
    items.map(item => queue.add(() => processItem(item)))
);
```

## Best Practices Checklist

- [ ] Use batch processing for large datasets
- [ ] Set concurrency limits based on resources
- [ ] Yield to event loop between batches
- [ ] Monitor queue depth and processing times
- [ ] Handle partial failures in batches

## Cross-References

- See [Rate Limiting](./01-rate-limiting.md) for rate limits
- See [Concurrency Control](./03-concurrency-control.md) for synchronization
- See [Performance](../11-async-performance/01-memory-cpu-patterns.md) for optimization

## Next Steps

Continue to [Concurrency Control](./03-concurrency-control.md) for synchronization.
