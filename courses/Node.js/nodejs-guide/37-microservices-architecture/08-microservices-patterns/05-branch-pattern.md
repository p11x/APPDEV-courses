# Branch Pattern

## What You'll Learn

- What the branch pattern is
- How to handle conditional saga flows
- How to implement parallel compensation

## Implementation

```ts
// Branch pattern — different flows based on conditions

class OrderSaga {
  async execute(order: Order) {
    // Step 1: Check inventory
    const available = await inventoryService.check(order.items);

    if (available) {
      // Path A: Process immediately
      await this.processImmediately(order);
    } else {
      // Path B: Backorder
      await this.processBackorder(order);
    }
  }

  async processImmediately(order: Order) {
    await paymentService.charge(order);
    await shippingService.ship(order);
  }

  async processBackorder(order: Order) {
    await backorderService.create(order);
    await notificationService.notify(order.userId, 'Item on backorder');
  }
}
```

## Next Steps

For antipatterns, continue to [Antipatterns](./06-antipatterns.md).
