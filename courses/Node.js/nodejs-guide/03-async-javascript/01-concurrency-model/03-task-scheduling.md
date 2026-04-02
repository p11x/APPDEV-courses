# Task Scheduling and Prioritization Strategies

## What You'll Learn

- Scheduling tasks at different priorities
- Yielding control to the event loop
- Batch processing with scheduling
- Performance implications of scheduling choices

## Priority-Based Scheduling

```javascript
// Schedule tasks at different priorities
class TaskScheduler {
    // Critical: nextTick — executes ASAP, highest priority
    static critical(task) {
        process.nextTick(task);
    }

    // High: Promise — executes in microtask queue
    static high(task) {
        Promise.resolve().then(task);
    }

    // Normal: setImmediate — executes in check phase
    static normal(task) {
        setImmediate(task);
    }

    // Low: setTimeout — executes in timers phase
    static low(task) {
        setTimeout(task, 0);
    }
}

// Usage
TaskScheduler.critical(() => console.log('1: Critical'));
TaskScheduler.high(() => console.log('2: High'));
TaskScheduler.normal(() => console.log('3: Normal'));
TaskScheduler.low(() => console.log('4: Low'));

// Output order:
// 1: Critical (nextTick)
// 2: High (Promise)
// 3: Normal (setImmediate)
// 4: Low (setTimeout)
```

## Yielding to Event Loop

```javascript
// Prevent event loop starvation during long operations

// Method 1: setImmediate (best — yields fully)
function yieldToEventLoop() {
    return new Promise(resolve => setImmediate(resolve));
}

// Method 2: setTimeout(0) (yields to timers phase)
function yieldViaTimer() {
    return new Promise(resolve => setTimeout(resolve, 0));
}

// Method 3: queueMicrotask (doesn't yield — runs immediately)
function yieldViaMicrotask() {
    return new Promise(resolve => queueMicrotask(resolve));
}

// Usage: Process large array without blocking
async function processLargeArray(items) {
    const results = [];

    for (let i = 0; i < items.length; i++) {
        results.push(processItem(items[i]));

        // Yield every 100 items
        if (i % 100 === 0) {
            await yieldToEventLoop();
            // Allows pending I/O and timers to execute
        }
    }

    return results;
}
```

## Batch Processing with Microtasks

```javascript
class BatchProcessor {
    constructor(processFn, { batchSize = 100, useMicrotask = true } = {}) {
        this.processFn = processFn;
        this.batchSize = batchSize;
        this.queue = [];
        this.scheduled = false;
        this.useMicrotask = useMicrotask;
    }

    add(item) {
        this.queue.push(item);

        if (!this.scheduled) {
            this.scheduled = true;
            if (this.useMicrotask) {
                queueMicrotask(() => this.flush());
            } else {
                setImmediate(() => this.flush());
            }
        }
    }

    flush() {
        this.scheduled = false;
        while (this.queue.length > 0) {
            const batch = this.queue.splice(0, this.batchSize);
            this.processFn(batch);
        }
    }
}

// Usage
const processor = new BatchProcessor((items) => {
    console.log(`Processing ${items.length} items`);
}, { batchSize: 50 });

for (let i = 0; i < 200; i++) processor.add(i);
// Processes in 4 batches of 50
```

## nextTick Starvation Prevention

```javascript
// DANGER: Infinite nextTick blocks the event loop
function dangerous() {
    process.nextTick(dangerous); // Never yields
}

// SAFE: Use setImmediate for recursive async
function safe() {
    setImmediate(safe); // Yields to event loop each iteration
}

// SAFE: Mix microtasks with macrotasks
async function processRecursively(items) {
    for (const item of items) {
        await processItem(item);
        if (items.indexOf(item) % 100 === 0) {
            await new Promise(r => setImmediate(r)); // Yield
        }
    }
}
```

## Best Practices Checklist

- [ ] Use setImmediate for yielding control in long operations
- [ ] Avoid recursive process.nextTick — use setImmediate instead
- [ ] Use queueMicrotask for high-priority sync batching
- [ ] Batch operations to reduce event loop pressure
- [ ] Monitor event loop lag in production

## Cross-References

- See [Event Loop](./01-event-loop-deep-dive.md) for phase details
- See [Promise Implementation](./02-promise-implementation.md) for Promise internals
- See [Concurrency Control](../08-concurrency-control/01-rate-limiting.md) for rate limiting

## Next Steps

Continue to [Callbacks](../02-callbacks/03-callback-optimization.md) for callback patterns.
