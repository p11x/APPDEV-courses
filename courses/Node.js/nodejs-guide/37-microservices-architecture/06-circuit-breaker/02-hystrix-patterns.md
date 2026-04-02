# Hystrix Patterns

## What You'll Learn

- What Hystrix patterns are
- How to implement bulkhead isolation
- How to implement timeout with fallback
- How these patterns apply to Node.js

## Bulkhead Pattern

```ts
// Limit concurrent calls to a service

class Bulkhead {
  private active = 0;
  private queue: Array<{ resolve: Function; reject: Function }> = [];

  constructor(private maxConcurrent: number) {}

  async execute<T>(fn: () => Promise<T>): Promise<T> {
    if (this.active >= this.maxConcurrent) {
      // Wait in queue
      await new Promise<void>((resolve) => {
        this.queue.push({ resolve, reject: () => {} });
      });
    }

    this.active++;
    try {
      return await fn();
    } finally {
      this.active--;
      const next = this.queue.shift();
      if (next) next.resolve();
    }
  }
}

// Usage — limit to 10 concurrent calls to payment service
const paymentBulkhead = new Bulkhead(10);

async function chargePayment(order: Order) {
  return paymentBulkhead.execute(() =>
    fetch('http://payment-service/charge', { method: 'POST', body: JSON.stringify(order) })
  );
}
```

## Timeout with Fallback

```ts
async function withTimeout<T>(fn: () => Promise<T>, ms: number, fallback: () => T): Promise<T> {
  return Promise.race([
    fn(),
    new Promise<T>((_, reject) => setTimeout(() => reject(new Error('Timeout')), ms)),
  ]).catch(() => fallback());
}
```

## Next Steps

For Resilience4j, continue to [Resilience4j](./03-resilience4j.md).
