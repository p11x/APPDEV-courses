# Choreography Pattern

## What You'll Learn

- How choreography-based sagas work
- How to implement event-driven coordination
- When to use choreography vs orchestration

## Implementation

```ts
// Each service publishes and reacts to events

// Order Service
class OrderService {
  async createOrder(order: Order) {
    await db.orders.create(order);
    await eventBus.publish('OrderCreated', order);
  }
}

// Inventory Service — listens for OrderCreated
eventBus.subscribe('OrderCreated', async (order) => {
  try {
    await inventory.reserve(order.items);
    await eventBus.publish('InventoryReserved', order);
  } catch (err) {
    await eventBus.publish('InventoryReservationFailed', { order, error: err.message });
  }
});

// Compensation
eventBus.subscribe('InventoryReservationFailed', async ({ order }) => {
  await db.orders.update(order.id, { status: 'cancelled' });
  await eventBus.publish('OrderCancelled', order);
});
```

## Next Steps

For orchestration, continue to [Orchestration Pattern](./03-orchestration-pattern.md).
