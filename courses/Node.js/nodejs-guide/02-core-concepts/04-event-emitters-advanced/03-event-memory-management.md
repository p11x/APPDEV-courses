# Event Emitter Memory Management

## What You'll Learn

- Detecting listener memory leaks
- Proper listener cleanup patterns
- MaxListeners warning management
- Performance optimization

## Memory Leak Prevention

```javascript
import { EventEmitter } from 'node:events';

// LEAK: Listener added per request, never removed
class LeakyServer extends EventEmitter {
    handleRequest(req) {
        this.on('data', (data) => { // NEW listener each request!
            processResponse(req, data);
        });
    }
}

// FIX: Track and clean up listeners
class SafeServer extends EventEmitter {
    handleRequest(req) {
        const handler = (data) => processResponse(req, data);
        this.on('data', handler);

        // Remove when request closes
        req.on('close', () => this.off('data', handler));
    }
}

// FIX: Use once() for single-use listeners
class BetterServer extends EventEmitter {
    handleRequest(req) {
        this.once('data', (data) => {
            processResponse(req, data);
        });
    }
}
```

### Listener Tracking

```javascript
class TrackedEmitter extends EventEmitter {
    constructor() {
        super();
        this._listenerMap = new WeakMap();
    }

    trackOn(event, listener, context) {
        const wrappedListener = (...args) => listener.apply(context, args);
        this._listenerMap.set(listener, wrappedListener);
        this.on(event, wrappedListener);
        return this;
    }

    trackOff(event, listener) {
        const wrapped = this._listenerMap.get(listener);
        if (wrapped) {
            this.off(event, wrapped);
            this._listenerMap.delete(listener);
        }
        return this;
    }

    getListenerReport() {
        const report = {};
        for (const event of this.eventNames()) {
            report[event] = this.listenerCount(event);
        }
        return report;
    }
}
```

## MaxListeners Management

```javascript
// Default limit is 10 — too low for some scenarios
const emitter = new EventEmitter();

// Set higher limit
emitter.setMaxListeners(50);

// Or set globally
EventEmitter.defaultMaxListeners = 20;

// Check for warnings
process.on('warning', (warning) => {
    if (warning.name === 'MaxListenersExceededWarning') {
        console.error('MEMORY LEAK:', warning.message);
    }
});
```

## Performance Optimization

```javascript
// Use prependListener for priority listeners
emitter.prependListener('data', priorityHandler); // Runs first

// Use off() instead of removeAllListeners()
emitter.off('data', specificHandler); // Surgical removal

// Avoid inline functions (can't remove them)
// BAD: emitter.on('data', () => { ... });
// GOOD: const handler = () => { ... }; emitter.on('data', handler);

// Cache listener arrays for hot paths
const listeners = emitter.listeners('data');
for (const listener of listeners) {
    listener(data);
}
```

## Best Practices Checklist

- [ ] Always remove listeners in cleanup handlers
- [ ] Use `once()` for single-fire events
- [ ] Set appropriate MaxListeners limits
- [ ] Monitor MaxListenersExceededWarning
- [ ] Use named functions (not inline) for removable listeners
- [ ] Track listener counts in production

## Cross-References

- See [Fundamentals](./01-eventemitter-fundamentals.md) for EventEmitter basics
- See [Advanced Patterns](./02-advanced-event-patterns.md) for pub/sub patterns
- See [Memory Profiling](../03-memory-architecture/03-memory-profiling.md) for leak detection

## Next Steps

Continue to [Stream Architecture](../05-stream-architecture/01-readable-writable-streams.md) for data streaming.
