# Event Memory Management and Cleanup

## What You'll Learn

- Detecting listener memory leaks
- Proper cleanup patterns
- MaxListeners management
- Production event monitoring

## Memory Leak Prevention

```javascript
import { EventEmitter } from 'node:events';

// LEAK: Listener added per request
class LeakyServer extends EventEmitter {
    handleRequest(req) {
        this.on('data', (data) => { // NEW listener each request!
            processResponse(req, data);
        });
    }
}

// FIX: Track and clean up
class SafeServer extends EventEmitter {
    handleRequest(req) {
        const handler = (data) => processResponse(req, data);
        this.on('data', handler);

        // Remove when request closes
        req.on('close', () => this.off('data', handler));
    }
}

// FIX: Use once() for single-use
class BetterServer extends EventEmitter {
    handleRequest(req) {
        this.once('data', (data) => {
            processResponse(req, data);
        });
    }
}
```

## Scoped Event Management

```javascript
class EventScope {
    constructor(emitter) {
        this.emitter = emitter;
        this.listeners = [];
    }

    on(event, handler) {
        this.emitter.on(event, handler);
        this.listeners.push({ event, handler });
        return this;
    }

    once(event, handler) {
        this.emitter.once(event, handler);
        this.listeners.push({ event, handler });
        return this;
    }

    // Clean up all listeners in this scope
    dispose() {
        for (const { event, handler } of this.listeners) {
            this.emitter.off(event, handler);
        }
        this.listeners = [];
    }
}

// Usage
const emitter = new EventEmitter();

function handleConnection(socket) {
    const scope = new EventScope(emitter);

    scope.on('data', (data) => process(data));
    scope.on('error', (err) => log(err));

    socket.on('close', () => {
        scope.dispose(); // Clean up all listeners
    });
}
```

## MaxListeners Management

```javascript
const emitter = new EventEmitter();

// Set higher limit for known high-listener scenarios
emitter.setMaxListeners(50);

// Monitor for warnings
process.on('warning', (warning) => {
    if (warning.name === 'MaxListenersExceededWarning') {
        console.error('POSSIBLE MEMORY LEAK:', warning.message);
    }
});
```

## Best Practices Checklist

- [ ] Always remove listeners in cleanup handlers
- [ ] Use once() for single-fire events
- [ ] Use scoped event management for connections
- [ ] Set appropriate MaxListeners limits
- [ ] Monitor MaxListenersExceededWarning in production

## Cross-References

- See [Observer/PubSub](./01-observer-pubsub.md) for patterns
- See [Event Emitter Advanced](./02-event-emitter-advanced.md) for advanced patterns
- See [Async Iterators](../06-async-iterators/01-for-await-of.md) for async iteration

## Next Steps

Continue to [Async Iterators](../06-async-iterators/01-for-await-of.md) for async iteration.
