# Concurrency Control Mechanisms

## What You'll Learn

- Mutex (mutual exclusion) implementation
- Semaphore for resource limiting
- Deadlock prevention strategies
- Async lock patterns

## Mutex Implementation

```javascript
class Mutex {
    constructor() {
        this.locked = false;
        this.queue = [];
    }

    async acquire() {
        if (!this.locked) {
            this.locked = true;
            return;
        }

        return new Promise(resolve => this.queue.push(resolve));
    }

    release() {
        if (this.queue.length > 0) {
            const next = this.queue.shift();
            next();
        } else {
            this.locked = false;
        }
    }

    async run(fn) {
        await this.acquire();
        try {
            return await fn();
        } finally {
            this.release();
        }
    }
}

// Usage: Protect shared resource
const mutex = new Mutex();
let counter = 0;

async function increment() {
    return mutex.run(async () => {
        const current = counter;
        await new Promise(r => setTimeout(r, 10)); // Simulate async
        counter = current + 1;
    });
}

await Promise.all(Array.from({ length: 100 }, () => increment()));
console.log(counter); // 100 (no race condition)
```

## Semaphore Implementation

```javascript
class Semaphore {
    constructor(max) {
        this.max = max;
        this.current = 0;
        this.queue = [];
    }

    async acquire() {
        if (this.current < this.max) {
            this.current++;
            return;
        }

        return new Promise(resolve => this.queue.push(resolve));
    }

    release() {
        if (this.queue.length > 0) {
            const next = this.queue.shift();
            next();
        } else {
            this.current--;
        }
    }

    async run(fn) {
        await this.acquire();
        try {
            return await fn();
        } finally {
            this.release();
        }
    }
}

// Usage: Limit concurrent database connections
const semaphore = new Semaphore(10);

async function query(sql) {
    return semaphore.run(async () => {
        return await db.query(sql);
    });
}
```

## Best Practices Checklist

- [ ] Use mutex for exclusive access to shared resources
- [ ] Use semaphore to limit concurrent operations
- [ ] Always release locks in finally blocks
- [ ] Avoid nested locks to prevent deadlocks
- [ ] Monitor lock contention in production

## Cross-References

- See [Rate Limiting](./01-rate-limiting.md) for rate limits
- See [Batch Processing](./02-batch-processing.md) for batching
- See [Error Boundaries](../07-async-error-handling/02-error-boundaries.md) for retry

## Next Steps

Continue to [Async Testing](../09-async-testing/01-unit-testing.md) for testing patterns.
