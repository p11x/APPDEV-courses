# Microservices Database Patterns

## What You'll Learn

- Database per service pattern
- Saga pattern for distributed transactions
- CQRS implementation
- Event sourcing basics
- Service data synchronization

## Database per Service Pattern

```
Microservices Database Architecture:
─────────────────────────────────────────────
┌──────────┐  ┌──────────┐  ┌──────────┐
│  User    │  │  Order   │  │ Inventory│
│ Service  │  │ Service  │  │ Service  │
└────┬─────┘  └────┬─────┘  └────┬─────┘
     │             │             │
┌────▼─────┐  ┌────▼─────┐  ┌────▼─────┐
│  Users   │  │  Orders  │  │Inventory │
│    DB    │  │    DB    │  │    DB    │
└──────────┘  └──────────┘  └──────────┘
     │             │             │
     └─────────────┼─────────────┘
                   │
            ┌──────▼──────┐
            │ Message Bus │
            │  (Events)   │
            └─────────────┘
```

## Event-Driven Synchronization

```javascript
import { EventEmitter } from 'node:events';

class ServiceEventBus extends EventEmitter {
    constructor() {
        super();
        this.handlers = new Map();
    }

    register(eventType, handler) {
        if (!this.handlers.has(eventType)) {
            this.handlers.set(eventType, []);
        }
        this.handlers.get(eventType).push(handler);
    }

    async emit(eventType, event) {
        const handlers = this.handlers.get(eventType) || [];
        for (const handler of handlers) {
            try {
                await handler(event);
            } catch (err) {
                console.error(`Handler failed for ${eventType}:`, err.message);
            }
        }
    }
}

// Order Service
class OrderService {
    constructor(pool, eventBus) {
        this.pool = pool;
        this.eventBus = eventBus;
    }

    async createOrder(orderData) {
        const client = await this.pool.connect();
        try {
            await client.query('BEGIN');

            const { rows: [order] } = await client.query(
                'INSERT INTO orders (user_id, total, status) VALUES ($1, $2, $3) RETURNING *',
                [orderData.userId, orderData.total, 'pending']
            );

            for (const item of orderData.items) {
                await client.query(
                    'INSERT INTO order_items (order_id, product_id, quantity, price) VALUES ($1, $2, $3, $4)',
                    [order.id, item.productId, item.quantity, item.price]
                );
            }

            await client.query('COMMIT');

            // Emit event for other services
            await this.eventBus.emit('order.created', {
                orderId: order.id,
                userId: orderData.userId,
                items: orderData.items,
                total: orderData.total,
            });

            return order;
        } catch (err) {
            await client.query('ROLLBACK');
            throw err;
        } finally {
            client.release();
        }
    }
}

// Inventory Service (subscribes to order events)
class InventoryService {
    constructor(pool, eventBus) {
        this.pool = pool;
        
        // Subscribe to order events
        eventBus.register('order.created', (event) => this.reserveInventory(event));
        eventBus.register('order.cancelled', (event) => this.releaseInventory(event));
    }

    async reserveInventory(event) {
        for (const item of event.items) {
            const { rowCount } = await this.pool.query(
                'UPDATE inventory SET quantity = quantity - $1 WHERE product_id = $2 AND quantity >= $1',
                [item.quantity, item.productId]
            );

            if (rowCount === 0) {
                throw new Error(`Insufficient inventory for product ${item.productId}`);
            }
        }
    }

    async releaseInventory(event) {
        for (const item of event.items) {
            await this.pool.query(
                'UPDATE inventory SET quantity = quantity + $1 WHERE product_id = $2',
                [item.quantity, item.productId]
            );
        }
    }
}
```

## CQRS Pattern

```javascript
// Command side (writes)
class CommandHandler {
    constructor(writePool) {
        this.writePool = writePool;
    }

    async createProduct(command) {
        const { rows: [product] } = await this.writePool.query(
            'INSERT INTO products (name, price, description) VALUES ($1, $2, $3) RETURNING *',
            [command.name, command.price, command.description]
        );

        // Publish event for read model update
        await this.publishEvent('product.created', product);
        return product;
    }

    async updateProduct(id, command) {
        const { rows: [product] } = await this.writePool.query(
            'UPDATE products SET name = $1, price = $2, description = $3 WHERE id = $4 RETURNING *',
            [command.name, command.price, command.description, id]
        );

        await this.publishEvent('product.updated', product);
        return product;
    }

    async publishEvent(type, data) {
        // Publish to message queue or event bus
        console.log(`Event: ${type}`, data);
    }
}

// Query side (reads)
class QueryHandler {
    constructor(readPool, redis) {
        this.readPool = readPool;
        this.redis = redis;
    }

    async getProduct(id) {
        const cached = await this.redis.get(`product:${id}`);
        if (cached) return JSON.parse(cached);

        const { rows: [product] } = await this.readPool.query(
            'SELECT * FROM product_read_model WHERE id = $1',
            [id]
        );

        if (product) {
            await this.redis.set(`product:${id}`, JSON.stringify(product), { EX: 300 });
        }

        return product;
    }

    async searchProducts(query, filters = {}) {
        const cacheKey = `search:${query}:${JSON.stringify(filters)}`;
        const cached = await this.redis.get(cacheKey);
        if (cached) return JSON.parse(cached);

        let sql = 'SELECT * FROM product_read_model WHERE 1=1';
        const params = [];

        if (query) {
            sql += ' AND (name ILIKE $1 OR description ILIKE $1)';
            params.push(`%${query}%`);
        }
        if (filters.minPrice) {
            sql += ` AND price >= $${params.length + 1}`;
            params.push(filters.minPrice);
        }
        if (filters.maxPrice) {
            sql += ` AND price <= $${params.length + 1}`;
            params.push(filters.maxPrice);
        }

        const { rows } = await this.readPool.query(sql, params);
        await this.redis.set(cacheKey, JSON.stringify(rows), { EX: 60 });

        return rows;
    }
}
```

## Saga Implementation

```javascript
class DistributedSaga {
    constructor() {
        this.steps = [];
    }

    step(name, execute, compensate) {
        this.steps.push({ name, execute, compensate });
        return this;
    }

    async execute(context) {
        const completed = [];

        for (const step of this.steps) {
            try {
                console.log(`Executing step: ${step.name}`);
                const result = await step.execute(context);
                completed.push({ step, result });
                Object.assign(context, result);
            } catch (err) {
                console.error(`Step ${step.name} failed:`, err.message);
                await this.compensate(completed, context);
                throw new Error(`Saga failed at step ${step.name}: ${err.message}`);
            }
        }

        return context;
    }

    async compensate(completed, context) {
        for (const { step, result } of completed.reverse()) {
            try {
                console.log(`Compensating step: ${step.name}`);
                await step.compensate(context, result);
            } catch (err) {
                console.error(`Compensation failed for ${step.name}:`, err.message);
            }
        }
    }
}

// Usage
const orderSaga = new DistributedSaga()
    .step('validate-inventory', 
        async (ctx) => {
            const available = await inventoryService.check(ctx.items);
            if (!available) throw new Error('Insufficient inventory');
            return { inventoryChecked: true };
        },
        async () => { /* no compensation needed */ }
    )
    .step('reserve-inventory',
        async (ctx) => {
            const reservationId = await inventoryService.reserve(ctx.items);
            return { reservationId };
        },
        async (ctx) => {
            await inventoryService.release(ctx.reservationId);
        }
    )
    .step('process-payment',
        async (ctx) => {
            const payment = await paymentService.charge(ctx.userId, ctx.total);
            return { paymentId: payment.id };
        },
        async (ctx) => {
            await paymentService.refund(ctx.paymentId);
        }
    )
    .step('create-order',
        async (ctx) => {
            const order = await orderService.create(ctx);
            return { orderId: order.id };
        },
        async (ctx) => {
            await orderService.cancel(ctx.orderId);
        }
    );

const result = await orderSaga.execute({
    userId: 1,
    items: [{ productId: 101, quantity: 2 }],
    total: 59.99,
});
```

## Best Practices Checklist

- [ ] Each microservice owns its database
- [ ] Use event-driven communication between services
- [ ] Implement saga pattern for distributed transactions
- [ ] Use CQRS for read-heavy workloads
- [ ] Avoid synchronous cross-service database queries
- [ ] Implement idempotent event handlers
- [ ] Use eventual consistency where acceptable
- [ ] Monitor inter-service communication

## Cross-References

- See [Sharding](./02-database-sharding.md) for data partitioning
- See [Transactions](../01-database-integration-patterns/05-transaction-management.md) for transaction patterns
- See [Message Queues](../../../17-message-queues/) for async communication

## Next Steps

Continue to [Database Clustering](./04-database-clustering.md) for high availability.
