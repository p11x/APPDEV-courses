# Circuit Breaker Best Practices

## What You'll Learn

- When to use circuit breakers
- How to configure thresholds
- How to handle fallbacks
- How to test circuit breakers

## Fallback Strategy

```ts
class CircuitBreaker {
  async execute<T>(fn: () => Promise<T>, fallback?: () => T): Promise<T> {
    if (this.state === CircuitState.OPEN) {
      if (fallback) return fallback();
      throw new Error('Circuit is OPEN');
    }

    try {
      return await fn();
    } catch (err) {
      this.failureCount++;
      if (this.failureCount >= this.threshold) {
        this.state = CircuitState.OPEN;
      }
      if (fallback) return fallback();
      throw err;
    }
  }
}

// Usage with fallback
const user = await breaker.execute(
  () => fetchUser(id),
  () => ({ id, name: 'Unknown', cached: true })  // Fallback to cached/default
);
```

## Testing

```ts
describe('CircuitBreaker', () => {
  it('opens after threshold failures', async () => {
    const breaker = new CircuitBreaker(3, 1000);
    const failingFn = () => Promise.reject(new Error('fail'));

    for (let i = 0; i < 3; i++) {
      await breaker.execute(failingFn).catch(() => {});
    }

    expect(breaker.getState()).toBe('OPEN');
  });
});
```

## Next Steps

For monitoring, continue to [Circuit Breaker Monitoring](./05-circuit-breaker-monitoring.md).
