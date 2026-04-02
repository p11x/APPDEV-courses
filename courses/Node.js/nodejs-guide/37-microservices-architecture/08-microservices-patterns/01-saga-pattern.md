# Saga Pattern

## What You'll Learn

- What the saga pattern is
- How to implement choreography-based sagas
- How to implement orchestration-based sagas
- How to handle compensation (rollback)

## What Is a Saga?

A saga manages distributed transactions across multiple services. If any step fails, previous steps are **compensated** (undone).

```
Order Saga:
  1. Create Order         → compensate: Cancel Order
  2. Reserve Inventory    → compensate: Release Inventory
  3. Charge Payment       → compensate: Refund Payment
  4. Ship Order           → compensate: Cancel Shipment
```

## Choreography (Event-Based)

```ts
// Each service listens for events and reacts

// Order Service
eventBus.on('OrderCreated', async (order) => {
  await inventoryService.reserve(order.items);
});

// Inventory Service
eventBus.on('InventoryReserved', async (order) => {
  await paymentService.charge(order);
});

// Payment Service
eventBus.on('PaymentCharged', async (order) => {
  await shippingService.ship(order);
});

// Compensation — if payment fails
eventBus.on('PaymentFailed', async (order) => {
  await inventoryService.release(order.items);  // Compensate step 2
  await orderService.cancel(order.id);          // Compensate step 1
});
```

## Orchestration (Central Coordinator)

```ts
class OrderSaga {
  async execute(order: Order) {
    const steps = [
      { action: () => this.createOrder(order), compensate: () => this.cancelOrder(order.id) },
      { action: () => this.reserveInventory(order), compensate: () => this.releaseInventory(order) },
      { action: () => this.chargePayment(order), compensate: () => this.refundPayment(order) },
    ];

    const completed = [];

    for (const step of steps) {
      try {
        await step.action();
        completed.push(step);
      } catch (err) {
        // Compensate in reverse order
        for (const completedStep of completed.reverse()) {
          await completedStep.compensate();
        }
        throw err;
      }
    }
  }
}
```

## Next Steps

For choreography, continue to [Choreography Pattern](./02-choreography-pattern.md).
