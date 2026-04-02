# Orchestration Pattern

## What You'll Learn

- How orchestration-based sagas work
- How to build a saga orchestrator
- How to handle timeouts and retries

## Orchestrator

```ts
// saga-orchestrator.ts

class OrderSagaOrchestrator {
  async execute(order: Order) {
    const sagaId = crypto.randomUUID();

    const steps = [
      {
        name: 'reserve-inventory',
        action: () => this.callService('inventory-service', '/reserve', order),
        compensate: () => this.callService('inventory-service', '/release', order),
        timeout: 5000,
      },
      {
        name: 'charge-payment',
        action: () => this.callService('payment-service', '/charge', order),
        compensate: () => this.callService('payment-service', '/refund', order),
        timeout: 10_000,
      },
      {
        name: 'create-shipment',
        action: () => this.callService('shipping-service', '/ship', order),
        compensate: () => this.callService('shipping-service', '/cancel', order),
        timeout: 5000,
      },
    ];

    const completed = [];

    for (const step of steps) {
      try {
        await Promise.race([
          step.action(),
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error(`${step.name} timeout`)), step.timeout)
          ),
        ]);
        completed.push(step);
      } catch (err) {
        // Compensate in reverse
        for (const s of completed.reverse()) {
          await s.compensate();
        }
        throw err;
      }
    }
  }
}
```

## Next Steps

For aggregation, continue to [API Aggregation Pattern](./04-api-aggregation-pattern.md).
