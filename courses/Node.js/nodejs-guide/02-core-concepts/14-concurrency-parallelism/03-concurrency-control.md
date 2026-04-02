# Concurrency Control Mechanisms

## What You'll Learn

- Mutex (mutual exclusion) implementation
- Semaphore for resource limiting
- Deadlock prevention
- Rate limiting patterns

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

async function updateCounter() {
    return mutex.run(async () => {
        const current = await redis.get('counter');
        await redis.set('counter', parseInt(current) + 1);
    });
}
```

## Semaphore

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

## Rate Limiter

```javascript
class TokenBucket {
    constructor(capacity, refillRate) {
        this.capacity = capacity;
        this.tokens = capacity;
        this.refillRate = refillRate;
        this.lastRefill = Date.now();
    }

    tryConsume(tokens = 1) {
        this.refill();
        if (this.tokens >= tokens) {
            this.tokens -= tokens;
            return true;
        }
        return false;
    }

    refill() {
        const now = Date.now();
        const elapsed = (now - this.lastRefill) / 1000;
        this.tokens = Math.min(this.capacity, this.tokens + elapsed * this.refillRate);
        this.lastRefill = now;
    }
}
```

## Best Practices Checklist

- [ ] Use mutex for exclusive access to shared resources
- [ ] Use semaphore to limit concurrent operations
- [ ] Always release locks in finally blocks
- [ ] Avoid nested locks (prevent deadlocks)
- [ ] Use rate limiters for external API calls

## Cross-References

- See [Async Optimization](./01-async-optimization.md) for async patterns
- See [Worker Parallelism](./02-worker-thread-parallelism.md) for parallelism
- See [Error Handling](../11-error-handling/01-error-propagation.md) for error patterns

## Next Steps

This completes Chapter 2 of the Node.js guide. Proceed to [Chapter 3: Async JavaScript](../../03-async-javascript/) for async patterns.
