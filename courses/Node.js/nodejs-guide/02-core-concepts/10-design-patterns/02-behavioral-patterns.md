# Behavioral Patterns: Observer, Strategy, and Middleware

## What You'll Learn

- Observer pattern (EventEmitter)
- Strategy pattern for algorithms
- Middleware pattern for request processing
- Real-world implementation examples

## Observer Pattern

```javascript
import { EventEmitter } from 'node:events';

class OrderService extends EventEmitter {
    async createOrder(orderData) {
        const order = { id: Date.now(), ...orderData, status: 'created' };

        // Business logic
        this.emit('order.created', order);

        // Notify subscribers
        await this.notifyInventory(order);
        await this.notifyEmail(order);

        return order;
    }

    async notifyInventory(order) { /* ... */ }
    async notifyEmail(order) { /* ... */ }
}

// Subscribers
orderService.on('order.created', (order) => {
    inventory.reserve(order.items);
});

orderService.on('order.created', (order) => {
    email.send(order.userId, 'Order confirmed');
});
```

## Strategy Pattern

```javascript
// Different compression strategies
class CompressionContext {
    constructor(strategy) {
        this.strategy = strategy;
    }

    compress(data) {
        return this.strategy(data);
    }

    setStrategy(strategy) {
        this.strategy = strategy;
    }
}

const strategies = {
    gzip: (data) => require('zlib').gzipSync(data),
    deflate: (data) => require('zlib').deflateSync(data),
    brotli: (data) => require('zlib').brotliCompressSync(data),
    none: (data) => data,
};

const compressor = new CompressionContext(strategies.gzip);
const compressed = compressor.compress(Buffer.from('hello world'));

// Switch strategy at runtime
compressor.setStrategy(strategies.brotli);
const brotliCompressed = compressor.compress(Buffer.from('hello world'));
```

## Middleware Pattern

```javascript
class MiddlewarePipeline {
    constructor() {
        this.middlewares = [];
    }

    use(fn) {
        this.middlewares.push(fn);
        return this;
    }

    async execute(context) {
        let index = 0;

        const next = async () => {
            if (index < this.middlewares.length) {
                const middleware = this.middlewares[index++];
                await middleware(context, next);
            }
        };

        await next();
        return context;
    }
}

// Usage
const pipeline = new MiddlewarePipeline();

pipeline.use(async (ctx, next) => {
    ctx.startTime = Date.now();
    await next();
    ctx.duration = Date.now() - ctx.startTime;
});

pipeline.use(async (ctx, next) => {
    ctx.authenticated = true;
    await next();
});

pipeline.use(async (ctx) => {
    ctx.result = 'processed';
});

const result = await pipeline.execute({});
// { startTime: ..., duration: ..., authenticated: true, result: 'processed' }
```

## Best Practices Checklist

- [ ] Use Observer for loose coupling between components
- [ ] Use Strategy when algorithm varies at runtime
- [ ] Use Middleware for cross-cutting concerns
- [ ] Keep strategies stateless when possible

## Cross-References

- See [Creational Patterns](./01-creational-patterns.md) for Singleton/Factory
- See [Structural Patterns](./03-structural-patterns.md) for Adapter/Decorator
- See [Event Emitters](../04-event-emitters-advanced/01-eventemitter-fundamentals.md) for events

## Next Steps

Continue to [Structural Patterns](./03-structural-patterns.md) for Adapter and Decorator.
