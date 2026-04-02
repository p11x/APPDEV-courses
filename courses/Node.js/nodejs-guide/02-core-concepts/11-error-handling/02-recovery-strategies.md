# Error Recovery: Retry, Circuit Breaker, and Fallback

## What You'll Learn

- Retry with exponential backoff
- Circuit breaker pattern
- Fallback strategies
- Rate limiting error responses

## Retry with Exponential Backoff

```javascript
async function retry(fn, { maxRetries = 3, baseDelay = 1000 } = {}) {
    for (let attempt = 1; attempt <= maxRetries; attempt++) {
        try {
            return await fn();
        } catch (err) {
            if (attempt === maxRetries) throw err;

            const delay = baseDelay * Math.pow(2, attempt - 1);
            const jitter = Math.random() * delay * 0.1;

            console.warn(`Attempt ${attempt} failed, retrying in ${delay + jitter}ms`);
            await new Promise(r => setTimeout(r, delay + jitter));
        }
    }
}

// Usage
const data = await retry(() => fetchExternalAPI(), {
    maxRetries: 3,
    baseDelay: 1000,
});
```

## Circuit Breaker

```javascript
class CircuitBreaker {
    constructor(fn, options = {}) {
        this.fn = fn;
        this.failureThreshold = options.failureThreshold || 5;
        this.resetTimeout = options.resetTimeout || 30000;
        this.state = 'CLOSED';
        this.failureCount = 0;
        this.lastFailureTime = null;
    }

    async call(...args) {
        if (this.state === 'OPEN') {
            if (Date.now() - this.lastFailureTime >= this.resetTimeout) {
                this.state = 'HALF_OPEN';
            } else {
                throw new Error('Circuit breaker is OPEN');
            }
        }

        try {
            const result = await this.fn(...args);
            this.onSuccess();
            return result;
        } catch (err) {
            this.onFailure();
            throw err;
        }
    }

    onSuccess() {
        this.failureCount = 0;
        this.state = 'CLOSED';
    }

    onFailure() {
        this.failureCount++;
        this.lastFailureTime = Date.now();

        if (this.failureCount >= this.failureThreshold) {
            this.state = 'OPEN';
            console.error('Circuit breaker OPENED');
        }
    }
}

// Usage
const breaker = new CircuitBreaker(fetchExternalAPI, {
    failureThreshold: 5,
    resetTimeout: 30000,
});

try {
    const data = await breaker.call();
} catch (err) {
    // Use fallback
    const cached = await cache.get('fallback-data');
}
```

## Fallback Strategy

```javascript
async function withFallback(primaryFn, fallbackFn) {
    try {
        return await primaryFn();
    } catch (err) {
        console.warn('Primary failed, using fallback:', err.message);
        return await fallbackFn();
    }
}

// Usage
const userData = await withFallback(
    () => fetchFromAPI(userId),
    () => fetchFromCache(userId),
);
```

## Best Practices Checklist

- [ ] Use exponential backoff with jitter for retries
- [ ] Implement circuit breaker for external services
- [ ] Provide fallback values for non-critical data
- [ ] Set reasonable retry limits (don't retry forever)
- [ ] Log all retry attempts and circuit breaker state changes

## Cross-References

- See [Error Propagation](./01-error-propagation.md) for error classes
- See [Graceful Degradation](./03-graceful-degradation.md) for degradation
- See [Caching](../13-caching-strategies/01-in-memory-caching.md) for fallback cache

## Next Steps

Continue to [Graceful Degradation](./03-graceful-degradation.md) for fallback patterns.
