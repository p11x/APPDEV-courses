# Circuit Breaker Setup

## What You'll Learn

- What a circuit breaker is
- How to implement a circuit breaker in Node.js
- How circuit breaker states work
- How to use circuit breakers with HTTP calls

## What Is a Circuit Breaker?

A circuit breaker prevents cascading failures by stopping calls to a failing service.

```
CLOSED (normal) → requests pass through
  → Failure threshold reached
OPEN (tripping) → requests fail immediately (no call made)
  → Timeout expires
HALF-OPEN → one test request allowed
  → Success → CLOSED
  → Failure → OPEN again
```

## Implementation

```ts
// circuit-breaker.ts

enum CircuitState {
  CLOSED = 'CLOSED',
  OPEN = 'OPEN',
  HALF_OPEN = 'HALF_OPEN',
}

class CircuitBreaker {
  private state: CircuitState = CircuitState.CLOSED;
  private failureCount = 0;
  private lastFailureTime = 0;

  constructor(
    private readonly threshold: number = 5,
    private readonly resetTimeout: number = 30_000
  ) {}

  async execute<T>(fn: () => Promise<T>): Promise<T> {
    if (this.state === CircuitState.OPEN) {
      if (Date.now() - this.lastFailureTime > this.resetTimeout) {
        this.state = CircuitState.HALF_OPEN;
      } else {
        throw new Error('Circuit is OPEN — request blocked');
      }
    }

    try {
      const result = await fn();

      if (this.state === CircuitState.HALF_OPEN) {
        this.state = CircuitState.CLOSED;
        this.failureCount = 0;
      }

      return result;
    } catch (err) {
      this.failureCount++;
      this.lastFailureTime = Date.now();

      if (this.failureCount >= this.threshold) {
        this.state = CircuitState.OPEN;
      }

      throw err;
    }
  }

  getState() {
    return this.state;
  }
}

// Usage
const breaker = new CircuitBreaker(3, 30_000);

async function fetchUser(id: string) {
  return breaker.execute(async () => {
    const res = await fetch(`http://user-service/users/${id}`);
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    return res.json();
  });
}
```

## Next Steps

For patterns, continue to [Hystrix Patterns](./02-hystrix-patterns.md).
