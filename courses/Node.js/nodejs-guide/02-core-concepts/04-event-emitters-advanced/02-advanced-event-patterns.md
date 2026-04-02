# Advanced Event Patterns: Pub/Sub, Observer, and Middleware

## What You'll Learn

- Publish/Subscribe pattern implementation
- Observer pattern variations
- Event middleware pipeline
- Async event processing

## Pub/Sub Pattern

```javascript
import { EventEmitter } from 'node:events';

class PubSub {
    constructor() {
        this.emitter = new EventEmitter();
        this.topics = new Map();
    }

    subscribe(topic, handler) {
        if (!this.topics.has(topic)) this.topics.set(topic, new Set());
        this.topics.get(topic).add(handler);
        this.emitter.on(topic, handler);

        return () => this.unsubscribe(topic, handler);
    }

    publish(topic, data) {
        this.emitter.emit(topic, {
            topic,
            data,
            timestamp: Date.now(),
        });
    }

    unsubscribe(topic, handler) {
        this.topics.get(topic)?.delete(handler);
        this.emitter.off(topic, handler);
    }

    getSubscribers(topic) {
        return this.topics.get(topic)?.size || 0;
    }
}

// Usage
const pubsub = new PubSub();

const unsub = pubsub.subscribe('user.created', (event) => {
    console.log(`New user: ${event.data.name}`);
});

pubsub.publish('user.created', { name: 'Alice', id: 1 });
pubsub.publish('user.created', { name: 'Bob', id: 2 });

unsub(); // Unsubscribe
```

## Event Middleware Pipeline

```javascript
import { EventEmitter } from 'node:events';

class MiddlewareEmitter extends EventEmitter {
    constructor() {
        super();
        this.middlewares = [];
    }

    use(fn) {
        this.middlewares.push(fn);
        return this;
    }

    async emitWithMiddleware(event, data) {
        let current = data;

        for (const middleware of this.middlewares) {
            current = await middleware(event, current);
            if (current === null) return; // Middleware stopped pipeline
        }

        this.emit(event, current);
    }
}

// Usage
const emitter = new MiddlewareEmitter();

emitter.use(async (event, data) => {
    console.log(`[LOG] ${event}:`, data);
    return data;
});

emitter.use(async (event, data) => {
    if (!data.userId) throw new Error('userId required');
    return { ...data, validated: true };
});

emitter.use(async (event, data) => {
    return { ...data, timestamp: Date.now() };
});

emitter.on('user.create', (data) => {
    console.log('Creating user:', data);
});

await emitter.emitWithMiddleware('user.create', { userId: 1, name: 'Alice' });
```

## Async Event Processing with Backpressure

```javascript
import { EventEmitter } from 'node:events';

class AsyncEventProcessor extends EventEmitter {
    constructor(concurrency = 5) {
        super();
        this.concurrency = concurrency;
        this.queue = [];
        this.running = 0;
    }

    async enqueue(event, data) {
        return new Promise((resolve, reject) => {
            this.queue.push({ event, data, resolve, reject });
            this.process();
        });
    }

    async process() {
        while (this.running < this.concurrency && this.queue.length > 0) {
            const task = this.queue.shift();
            this.running++;

            try {
                const result = await this.handle(task.event, task.data);
                task.resolve(result);
            } catch (err) {
                task.reject(err);
            } finally {
                this.running--;
                this.process();
            }
        }
    }

    async handle(event, data) {
        this.emit('process', { event, data });
        // Your handler logic
        return { processed: true, event, data };
    }
}
```

## Best Practices Checklist

- [ ] Use descriptive event names (e.g., 'user.created' not 'data')
- [ ] Return unsubscribe functions from subscribe
- [ ] Implement backpressure for high-throughput events
- [ ] Use middleware for cross-cutting concerns
- [ ] Log all emitted events in development

## Cross-References

- See [Fundamentals](./01-eventemitter-fundamentals.md) for EventEmitter basics
- See [Memory Management](./03-event-memory-management.md) for leak prevention
- See [Design Patterns](../10-design-patterns/01-creational-patterns.md) for patterns

## Next Steps

Continue to [Event Memory Management](./03-event-memory-management.md) for leak prevention.
