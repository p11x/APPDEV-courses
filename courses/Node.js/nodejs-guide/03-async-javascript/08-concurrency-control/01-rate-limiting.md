# Rate Limiting and Throttling

## What You'll Learn

- Token bucket rate limiter
- Sliding window rate limiting
- API throttling patterns
- Debouncing and throttling

## Token Bucket Rate Limiter

```javascript
class TokenBucket {
    constructor(capacity, refillRate) {
        this.capacity = capacity;
        this.tokens = capacity;
        this.refillRate = refillRate; // tokens per second
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

    async acquire(tokens = 1) {
        while (!this.tryConsume(tokens)) {
            const waitTime = ((tokens - this.tokens) / this.refillRate) * 1000;
            await new Promise(r => setTimeout(r, waitTime));
        }
    }
}

// Usage: 100 requests per minute
const limiter = new TokenBucket(100, 100 / 60);

async function makeRequest(url) {
    await limiter.acquire();
    return fetch(url);
}
```

## Async Throttling

```javascript
function throttle(fn, delay) {
    let lastCall = 0;
    let pending = null;

    return async function(...args) {
        const now = Date.now();

        if (now - lastCall >= delay) {
            lastCall = now;
            return fn(...args);
        }

        // Schedule for later
        if (!pending) {
            pending = new Promise(resolve => {
                setTimeout(async () => {
                    lastCall = Date.now();
                    pending = null;
                    resolve(await fn(...args));
                }, delay - (now - lastCall));
            });
        }

        return pending;
    };
}

// Usage: Max 1 API call per second
const throttledFetch = throttle(fetch, 1000);
```

## Async Debouncing

```javascript
function debounce(fn, delay) {
    let timer = null;
    let pending = null;

    return function(...args) {
        if (timer) clearTimeout(timer);

        if (!pending) {
            pending = new Promise(resolve => {
                timer = setTimeout(async () => {
                    timer = null;
                    const result = await fn(...args);
                    pending = null;
                    resolve(result);
                }, delay);
            });
        }

        return pending;
    };
}

// Usage: Search after user stops typing (300ms)
const debouncedSearch = debounce(async (query) => {
    const results = await searchAPI(query);
    displayResults(results);
}, 300);
```

## Best Practices Checklist

- [ ] Use token bucket for API rate limiting
- [ ] Implement throttling for external API calls
- [ ] Use debouncing for user input handling
- [ ] Set appropriate limits based on API quotas
- [ ] Monitor rate limit hits in production

## Cross-References

- See [Batch Processing](./02-batch-processing.md) for batching patterns
- See [Concurrency Control](./03-concurrency-control.md) for synchronization
- See [Error Boundaries](../07-async-error-handling/02-error-boundaries.md) for retry

## Next Steps

Continue to [Batch Processing](./02-batch-processing.md) for batching patterns.
