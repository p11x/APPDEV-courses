# Async Error Boundary Patterns

## What You'll Learn

- Error boundary implementation
- Retry with exponential backoff
- Circuit breaker pattern
- Fallback strategies

## Error Boundary Class

```javascript
class AsyncErrorBoundary {
    constructor(options = {}) {
        this.onError = options.onError || console.error;
        this.fallback = options.fallback;
    }

    async execute(fn, label = 'operation') {
        try {
            return await fn();
        } catch (err) {
            this.onError(err, label);

            if (this.fallback !== undefined) {
                return typeof this.fallback === 'function'
                    ? this.fallback(err)
                    : this.fallback;
            }

            throw err;
        }
    }
}

// Usage
const boundary = new AsyncErrorBoundary({
    onError: (err, label) => logger.error(`[${label}]`, err),
    fallback: null,
});

const data = await boundary.execute(
    () => fetchExternalAPI(),
    'external-api'
);
```

## Retry with Exponential Backoff

```javascript
async function retry(fn, { maxRetries = 3, baseDelay = 1000, maxDelay = 10000 } = {}) {
    for (let attempt = 1; attempt <= maxRetries; attempt++) {
        try {
            return await fn();
        } catch (err) {
            if (attempt === maxRetries) throw err;

            const delay = Math.min(baseDelay * Math.pow(2, attempt - 1), maxDelay);
            const jitter = Math.random() * delay * 0.1;

            console.warn(`Attempt ${attempt}/${maxRetries} failed, retrying in ${(delay + jitter).toFixed(0)}ms`);
            await new Promise(r => setTimeout(r, delay + jitter));
        }
    }
}

// Usage
const data = await retry(() => fetchExternalAPI(), { maxRetries: 3 });
```

## Circuit Breaker

```javascript
class CircuitBreaker {
    constructor(fn, { threshold = 5, resetTimeout = 30000 } = {}) {
        this.fn = fn;
        this.threshold = threshold;
        this.resetTimeout = resetTimeout;
        this.state = 'CLOSED';
        this.failures = 0;
        this.lastFailure = null;
    }

    async call(...args) {
        if (this.state === 'OPEN') {
            if (Date.now() - this.lastFailure >= this.resetTimeout) {
                this.state = 'HALF_OPEN';
            } else {
                throw new Error('Circuit is OPEN — request blocked');
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
        this.failures = 0;
        this.state = 'CLOSED';
    }

    onFailure() {
        this.failures++;
        this.lastFailure = Date.now();
        if (this.failures >= this.threshold) {
            this.state = 'OPEN';
            console.error('Circuit OPENED');
        }
    }
}

// Usage
const breaker = new CircuitBreaker(fetchExternalAPI, {
    threshold: 5,
    resetTimeout: 30000,
});

try {
    const data = await breaker.call();
} catch (err) {
    // Use cached fallback
    const cached = await cache.get('fallback');
}
```

## Best Practices Checklist

- [ ] Use retry for transient failures
- [ ] Implement circuit breaker for external services
- [ ] Provide fallback values for non-critical data
- [ ] Add jitter to retry delays
- [ ] Log all retry and circuit breaker state changes

## Cross-References

- See [Error Propagation](./01-error-propagation.md) for error classes
- See [Graceful Degradation](./03-graceful-degradation.md) for fallback patterns
- See [Concurrency Control](../08-concurrency-control/01-rate-limiting.md) for rate limiting

## Next Steps

Continue to [Graceful Degradation](./03-graceful-degradation.md) for fallback patterns.
