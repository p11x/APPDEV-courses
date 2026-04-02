# Observer and Publisher-Subscriber Patterns

## What You'll Learn

- Observer pattern implementation
- Publisher-Subscriber architecture
- Event-driven programming principles
- Real-world event system examples

## Observer Pattern

```javascript
import { EventEmitter } from 'node:events';

class OrderService extends EventEmitter {
    constructor() {
        super();
        this.orders = [];
    }

    async createOrder(data) {
        const order = { id: Date.now(), ...data, status: 'created' };
        this.orders.push(order);

        // Notify observers
        this.emit('order.created', order);
        return order;
    }

    async updateStatus(id, status) {
        const order = this.orders.find(o => o.id === id);
        if (!order) throw new Error('Order not found');

        order.status = status;
        this.emit('order.updated', { id, status });
        this.emit(`order.status.${status}`, order);

        return order;
    }
}

// Observers
const orderService = new OrderService();

orderService.on('order.created', (order) => {
    console.log(`Order ${order.id} created`);
    inventory.reserve(order.items);
});

orderService.on('order.status.shipped', (order) => {
    email.send(order.userId, 'Your order has shipped!');
});

orderService.on('order.status.delivered', (order) => {
    email.send(order.userId, 'Order delivered. Leave a review!');
});
```

## Publisher-Subscriber with Topics

```javascript
class PubSub {
    constructor() {
        this.topics = new Map();
    }

    subscribe(topic, handler) {
        if (!this.topics.has(topic)) {
            this.topics.set(topic, new Set());
        }
        this.topics.get(topic).add(handler);

        // Return unsubscribe function
        return () => this.topics.get(topic)?.delete(handler);
    }

    publish(topic, data) {
        const handlers = this.topics.get(topic);
        if (!handlers) return;

        const event = { topic, data, timestamp: Date.now() };
        for (const handler of handlers) {
            try {
                handler(event);
            } catch (err) {
                console.error(`Handler error for ${topic}:`, err);
            }
        }
    }

    unsubscribeAll(topic) {
        this.topics.delete(topic);
    }
}

// Usage
const pubsub = new PubSub();

const unsub = pubsub.subscribe('user.created', (event) => {
    console.log(`New user: ${event.data.name}`);
});

pubsub.publish('user.created', { name: 'Alice', id: 1 });
unsub(); // Unsubscribe
```

## Async Event Processing

```javascript
class AsyncEventBus {
    constructor() {
        this.handlers = new Map();
    }

    on(event, handler) {
        if (!this.handlers.has(event)) {
            this.handlers.set(event, []);
        }
        this.handlers.get(event).push(handler);
    }

    async emit(event, data) {
        const handlers = this.handlers.get(event) || [];
        const results = await Promise.allSettled(
            handlers.map(h => h(data))
        );

        // Report failures
        results.forEach((result, i) => {
            if (result.status === 'rejected') {
                console.error(`Handler ${i} failed:`, result.reason);
            }
        });
    }
}

// Usage
const bus = new AsyncEventBus();

bus.on('data.received', async (data) => {
    await saveToDatabase(data);
});

bus.on('data.received', async (data) => {
    await sendToAnalytics(data);
});

await bus.emit('data.received', { value: 42 });
```

## Best Practices Checklist

- [ ] Use descriptive event names (e.g., 'user.created' not 'data')
- [ ] Always return unsubscribe functions
- [ ] Handle errors in event handlers
- [ ] Use async event processing for I/O-heavy handlers
- [ ] Document all events and their payloads

## Cross-References

- See [Event Emitters](./02-event-emitter-advanced.md) for EventEmitter details
- See [Memory Management](./03-event-memory-management.md) for cleanup
- See [Error Handling](../07-async-error-handling/01-error-propagation.md) for errors

## Next Steps

Continue to [Event Emitter Advanced](./02-event-emitter-advanced.md) for advanced patterns.
