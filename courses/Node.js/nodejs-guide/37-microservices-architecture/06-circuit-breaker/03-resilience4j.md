# Resilience4j Patterns

## What You'll Learn

- How Resilience4j patterns apply to Node.js
- How to implement retry with backoff
- How to implement rate limiting
- How to compose resilience patterns

## Composition

```ts
// Combine multiple resilience patterns

class ResilientClient {
  private breaker = new CircuitBreaker(5, 30_000);
  private bulkhead = new Bulkhead(10);
  private rateLimiter = new RateLimiter(100, 60_000);

  async call<T>(fn: () => Promise<T>): Promise<T> {
    return this.rateLimiter.execute(() =>
      this.bulkhead.execute(() =>
        this.breaker.execute(() =>
          this.retry(fn)
        )
      )
    );
  }

  private async retry<T>(fn: () => Promise<T>, maxRetries = 3): Promise<T> {
    for (let i = 0; i < maxRetries; i++) {
      try {
        return await fn();
      } catch (err) {
        if (i === maxRetries - 1) throw err;
        await new Promise((r) => setTimeout(r, Math.pow(2, i) * 1000));
      }
    }
    throw new Error('Unreachable');
  }
}
```

## Next Steps

For best practices, continue to [Circuit Breaker Best Practices](./04-circuit-breaker-best-practices.md).
