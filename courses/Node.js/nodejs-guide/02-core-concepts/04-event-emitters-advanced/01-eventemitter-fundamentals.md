# EventEmitter Fundamentals and Implementation

## What You'll Learn

- EventEmitter class API
- Creating custom event-driven classes
- Error event handling
- Memory management for listeners

## EventEmitter API Reference

```javascript
import { EventEmitter } from 'node:events';

const emitter = new EventEmitter();

// Core methods
emitter.on(event, listener);              // Add listener
emitter.once(event, listener);            // Add one-time listener
emitter.off(event, listener);             // Remove listener (alias: removeListener)
emitter.removeAllListeners([event]);      // Remove all listeners
emitter.emit(event, ...args);             // Emit event
emitter.listeners(event);                 // Get listener array
emitter.listenerCount(event);             // Count listeners
emitter.eventNames();                     // Get event names
emitter.prependListener(event, listener); // Add listener to beginning
emitter.prependOnceListener(event, listener);
emitter.setMaxListeners(n);               // Set max (default: 10)
emitter.getMaxListeners();

// Async iterator (Node.js 16+)
for await (const [arg] of EventEmitter.on(emitter, 'event')) {
    console.log(arg);
}
```

## Custom Event-Driven Classes

```javascript
import { EventEmitter } from 'node:events';

class DataProcessor extends EventEmitter {
    constructor() {
        super();
        this.processed = 0;
    }

    async process(data) {
        this.emit('start', { size: data.length });

        try {
            // Validate
            this.emit('validate', data);
            if (!data) throw new Error('No data provided');

            // Transform
            this.emit('transform', data);
            const result = data.toUpperCase();

            // Save
            this.emit('save', result);
            this.processed++;

            this.emit('complete', { result, total: this.processed });
            return result;
        } catch (err) {
            this.emit('error', err);
            throw err;
        }
    }
}

// Usage with error handling
const processor = new DataProcessor();

processor.on('start', ({ size }) => console.log(`Processing ${size} items`));
processor.on('validate', () => console.log('Validating...'));
processor.on('transform', () => console.log('Transforming...'));
processor.on('complete', ({ result, total }) => {
    console.log(`Done: ${result} (total: ${total})`);
});
processor.on('error', (err) => console.error('Error:', err.message));

await processor.process('hello');
await processor.process('world');
```

## Error Event Handling

```javascript
// ALWAYS handle 'error' events — unhandled = process crash
emitter.on('error', (err) => {
    console.error('Emitter error:', err);
});

// Async error handling
class SafeEmitter extends EventEmitter {
    async emitAsync(event, ...args) {
        const listeners = this.listeners(event);
        for (const listener of listeners) {
            try {
                await listener(...args);
            } catch (err) {
                this.emit('error', err);
            }
        }
    }
}
```

## Best Practices Checklist

- [ ] Always handle 'error' events
- [ ] Use `.once()` for single-fire events
- [ ] Remove listeners when done (prevent memory leaks)
- [ ] Set appropriate max listener limits
- [ ] Use descriptive event names
- [ ] Pass objects with metadata to events

## Cross-References

- See [Advanced Patterns](./02-advanced-event-patterns.md) for pub/sub patterns
- See [Memory Management](./03-event-memory-management.md) for leak prevention
- See [Events Module](../02-built-in-modules/02-events-http-modules.md) for basics

## Next Steps

Continue to [Advanced Event Patterns](./02-advanced-event-patterns.md) for pub/sub patterns.
