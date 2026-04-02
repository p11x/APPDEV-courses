# Event Emitter Advanced Patterns

## What You'll Learn

- Custom event emitter creation
- Event middleware pipeline
- Error event handling patterns
- Event performance optimization

## Custom Event System

```javascript
class TypedEventEmitter {
    constructor() {
        this.listeners = new Map();
    }

    on(event, handler) {
        if (!this.listeners.has(event)) {
            this.listeners.set(event, new Set());
        }
        this.listeners.get(event).add(handler);
        return () => this.off(event, handler);
    }

    once(event, handler) {
        const wrapped = (...args) => {
            this.off(event, wrapped);
            handler(...args);
        };
        return this.on(event, wrapped);
    }

    off(event, handler) {
        this.listeners.get(event)?.delete(handler);
    }

    emit(event, ...args) {
        const handlers = this.listeners.get(event);
        if (!handlers) return;

        for (const handler of handlers) {
            try {
                handler(...args);
            } catch (err) {
                // Don't let handler errors crash the emitter
                this.emit('error', err);
            }
        }
    }

    removeAllListeners(event) {
        if (event) {
            this.listeners.delete(event);
        } else {
            this.listeners.clear();
        }
    }

    listenerCount(event) {
        return this.listeners.get(event)?.size || 0;
    }
}
```

## Event Middleware Pipeline

```javascript
class MiddlewareEmitter {
    constructor() {
        this.middlewares = [];
        this.handlers = new Map();
    }

    use(fn) {
        this.middlewares.push(fn);
        return this;
    }

    on(event, handler) {
        if (!this.handlers.has(event)) {
            this.handlers.set(event, []);
        }
        this.handlers.get(event).push(handler);
    }

    async emit(event, data) {
        let current = data;

        // Run middlewares
        for (const middleware of this.middlewares) {
            current = await middleware(event, current);
            if (current === null) return; // Stopped by middleware
        }

        // Run handlers
        const handlers = this.handlers.get(event) || [];
        await Promise.allSettled(
            handlers.map(h => h(current))
        );
    }
}

// Usage
const emitter = new MiddlewareEmitter();

emitter.use(async (event, data) => {
    console.log(`[${event}]`, data);
    return data;
});

emitter.use(async (event, data) => {
    if (!data.userId) throw new Error('userId required');
    return { ...data, timestamp: Date.now() };
});

emitter.on('user.action', async (data) => {
    await audit.log(data);
});

await emitter.emit('user.action', { userId: 1, action: 'login' });
```

## Best Practices Checklist

- [ ] Handle errors in event handlers gracefully
- [ ] Use middleware for cross-cutting concerns
- [ ] Implement once() for single-fire events
- [ ] Track listener counts to detect leaks
- [ ] Use async emit for I/O-heavy handlers

## Cross-References

- See [Observer/PubSub](./01-observer-pubsub.md) for patterns
- See [Memory Management](./03-event-memory-management.md) for cleanup
- See [Concurrency Control](../08-concurrency-control/01-rate-limiting.md) for throttling

## Next Steps

Continue to [Event Memory Management](./03-event-memory-management.md) for cleanup.
