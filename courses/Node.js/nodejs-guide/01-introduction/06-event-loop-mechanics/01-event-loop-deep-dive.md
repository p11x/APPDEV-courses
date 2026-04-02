# Event Loop Mechanics

## What You'll Learn

- Microtask queue vs macrotask queue differences
- Event loop phases and their execution order
- Task scheduling and prioritization strategies
- Performance implications of event loop behavior

## Event Loop Overview

### The Core Concept

The event loop is the heart of Node.js asynchronous processing:
- Continuously checks for pending tasks
- Executes callbacks when operations complete
- Never blocks the main thread for I/O
- Enables high concurrency with single thread

### Event Loop Flow

```
   ┌───────────────────────────┐
┌─►│           timers          │  setTimeout, setInterval
│  └─────────────┬─────────────┘
│  ┌─────────────┴─────────────┐
│  │     pending callbacks     │  I/O callbacks deferred
│  └─────────────┬─────────────┘
│  ┌─────────────┴─────────────┐
│  │       idle, prepare       │  Internal use only
│  └─────────────┬─────────────┘
│  ┌─────────────┴─────────────┐
│  │           poll            │  Retrieve new I/O events
│  │                           │  Execute I/O callbacks
│  └─────────────┬─────────────┘
│  ┌─────────────┴─────────────┐
│  │           check           │  setImmediate callbacks
│  └─────────────┬─────────────┘
│  ┌─────────────┴─────────────┐
└──┤      close callbacks     │  socket.on('close')
   └───────────────────────────┘
```

## Task Queues Explained

### Microtask Queue

Microtasks have higher priority than macrotasks:
- `Promise.then()` callbacks
- `Promise.catch()` callbacks
- `Promise.finally()` callbacks
- `queueMicrotask()` callbacks
- `process.nextTick()` (even higher priority)

```javascript
// Microtask examples
Promise.resolve().then(() => console.log('Promise 1'));
Promise.resolve().then(() => console.log('Promise 2'));
queueMicrotask(() => console.log('Microtask'));

console.log('Synchronous');

// Output:
// Synchronous
// Promise 1
// Promise 2
// Microtask
```

### Macrotask Queue

Macrotasks are processed one at a time per event loop iteration:
- `setTimeout()` callbacks
- `setInterval()` callbacks
- `setImmediate()` callbacks
- I/O callbacks
- Close callbacks

```javascript
// Macrotask examples
setTimeout(() => console.log('Timeout 1'), 0);
setTimeout(() => console.log('Timeout 2'), 0);
setImmediate(() => console.log('Immediate'));

console.log('Synchronous');

// Output:
// Synchronous
// Timeout 1
// Timeout 2
// Immediate (order with setTimeout varies)
```

## process.nextTick() vs setImmediate()

### process.nextTick()

```javascript
// process.nextTick runs BEFORE any I/O
// Executes in the microtask queue
// Has highest priority among async callbacks

console.log('Start');

setTimeout(() => console.log('Timeout'), 0);
setImmediate(() => console.log('Immediate'));
process.nextTick(() => console.log('NextTick'));
Promise.resolve().then(() => console.log('Promise'));

console.log('End');

// Output:
// Start
// End
// NextTick      (highest priority microtask)
// Promise       (other microtasks)
// Immediate     (macrotask - check phase)
// Timeout       (macrotask - timers phase, order may vary)
```

### setImmediate()

```javascript
// setImmediate runs in the check phase
// Executes after poll phase completes
// Only available in Node.js (not browser)

const fs = require('fs');

fs.readFile(__filename, () => {
    setTimeout(() => console.log('Timeout'), 0);
    setImmediate(() => console.log('Immediate'));
});

// Output (always consistent in I/O callbacks):
// Immediate
// Timeout
```

## Event Loop Phases Deep Dive

### Phase 1: Timers

```javascript
// Timers phase executes setTimeout and setInterval callbacks
// After the specified delay (minimum, not guaranteed)

const start = Date.now();

setTimeout(() => {
    console.log(`Timeout after ${Date.now() - start}ms`);
}, 100);

// Blocking operation delays timer execution
while (Date.now() - start < 200) {
    // Blocks for 200ms
}

// Output: Timeout after ~200ms (delayed by blocking code)
```

### Phase 2: Pending Callbacks

```javascript
// Executes I/O callbacks deferred from previous loop
// Rarely used directly
// Handles system-level callbacks
```

### Phase 3: Poll

```javascript
// Poll phase retrieves new I/O events
// Executes I/O callbacks
// May block waiting for events

const fs = require('fs');

// Poll phase handles this callback
fs.readFile('file.txt', (err, data) => {
    console.log('File read complete');
});

// If no timers and no setImmediate
// Poll phase waits for I/O events
```

### Phase 4: Check

```javascript
// Check phase executes setImmediate callbacks
// Runs immediately after poll phase

setImmediate(() => {
    console.log('Immediate 1');
});

setImmediate(() => {
    console.log('Immediate 2');
});

// Both execute in check phase
```

### Phase 5: Close Callbacks

```javascript
// Close callbacks phase
// Handles socket.on('close') events

const net = require('net');

const server = net.createServer();
server.listen(8080);

server.on('close', () => {
    console.log('Server closed');
});

server.close();
// 'Server closed' in close callbacks phase
```

## Microtask Execution Details

### Microtask Queue Drain

```javascript
// Microtasks run between EVERY phase
// Queue is fully drained before next phase

console.log('Start');

setTimeout(() => {
    console.log('Timeout');
    Promise.resolve().then(() => console.log('Promise in Timeout'));
}, 0);

Promise.resolve().then(() => {
    console.log('Promise 1');
    Promise.resolve().then(() => console.log('Nested Promise'));
});

Promise.resolve().then(() => console.log('Promise 2'));

console.log('End');

// Output:
// Start
// End
// Promise 1
// Promise 2
// Nested Promise
// Timeout
// Promise in Timeout
```

### process.nextTick() Starvation

```javascript
// WARNING: nextTick can starve the event loop
// Use setImmediate for recursive async operations

function recursiveNextTick() {
    process.nextTick(recursiveNextTick);  // BAD: Starves event loop
}

function recursiveImmediate() {
    setImmediate(recursiveImmediate);  // GOOD: Yields to event loop
}

// Correct approach for recursive async
async function processItems(items) {
    for (const item of items) {
        await processItem(item);
        // Allows event loop to process other tasks
    }
}
```

## Practical Examples

### Example 1: Understanding Execution Order

```javascript
console.log('1');

setTimeout(() => {
    console.log('2');
    Promise.resolve().then(() => console.log('3'));
}, 0);

Promise.resolve().then(() => {
    console.log('4');
    setTimeout(() => console.log('5'), 0);
});

process.nextTick(() => console.log('6'));
setImmediate(() => console.log('7'));

console.log('8');

// Output:
// 1
// 8
// 6
// 4
// 7
// 2
// 3
// 5
```

### Example 2: I/O and Timers

```javascript
const fs = require('fs');

fs.readFile(__filename, () => {
    console.log('I/O callback');
    
    setTimeout(() => console.log('Timeout'), 0);
    setImmediate(() => console.log('Immediate'));
    
    process.nextTick(() => console.log('NextTick'));
});

// Output:
// I/O callback
// NextTick
// Immediate
// Timeout
```

### Example 3: Async/Await and Promises

```javascript
async function asyncFunction() {
    console.log('1');
    await Promise.resolve();
    console.log('2');
}

console.log('3');
asyncFunction();
console.log('4');

// Output:
// 3
// 1
// 4
// 2
```

## Performance Implications

### Blocking the Event Loop

```javascript
// BAD: Blocks event loop
function fibonacci(n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

// This blocks ALL async operations
const result = fibonacci(40);
console.log(result);

// GOOD: Use worker threads for CPU-intensive tasks
const { Worker } = require('worker_threads');

function runWorker(n) {
    return new Promise((resolve, reject) => {
        const worker = new Worker('./fibonacci.js', {
            workerData: n
        });
        worker.on('message', resolve);
        worker.on('error', reject);
    });
}
```

### Measuring Event Loop Delay

```javascript
const { monitorEventLoopDelay } = require('perf_hooks');

const histogram = monitorEventLoopDelay({ resolution: 20 });
histogram.enable();

// Do some work
setTimeout(() => {
    console.log(`Mean delay: ${histogram.mean}ns`);
    console.log(`Max delay: ${histogram.max}ns`);
    console.log(`99th percentile: ${histogram.percentile(99)}ns`);
    histogram.disable();
}, 1000);
```

### Event Loop Utilization

```javascript
const { performance, PerformanceObserver } = require('perf_hooks');

const obs = new PerformanceObserver((items) => {
    const entry = items.getEntries()[0];
    console.log(`Event loop utilization: ${(entry.utilization * 100).toFixed(2)}%`);
});

obs.observe({ entryTypes: ['eventloop'], buffered: true });

// Check periodically
setInterval(() => {
    performance.mark('start');
    // ... your code ...
    performance.mark('end');
}, 1000);
```

## Common Patterns

### Pattern 1: Yielding to Event Loop

```javascript
// Yield control back to event loop
function yieldToEventLoop() {
    return new Promise(resolve => setImmediate(resolve));
}

async function processLargeArray(items) {
    const results = [];
    
    for (let i = 0; i < items.length; i++) {
        results.push(await processItem(items[i]));
        
        // Yield every 100 items
        if (i % 100 === 0) {
            await yieldToEventLoop();
        }
    }
    
    return results;
}
```

### Pattern 2: Batching Async Operations

```javascript
// Batch operations to avoid overwhelming event loop
async function batchProcess(items, batchSize = 100) {
    const results = [];
    
    for (let i = 0; i < items.length; i += batchSize) {
        const batch = items.slice(i, i + batchSize);
        const batchResults = await Promise.all(
            batch.map(item => processItem(item))
        );
        results.push(...batchResults);
        
        // Allow other tasks to run
        await new Promise(resolve => setTimeout(resolve, 0));
    }
    
    return results;
}
```

### Pattern 3: Prioritizing Tasks

```javascript
// Use different queues for different priorities
class TaskScheduler {
    constructor() {
        this.highPriority = [];
        this.lowPriority = [];
    }
    
    addHighPriority(task) {
        process.nextTick(task);  // Highest priority
    }
    
    addLowPriority(task) {
        setImmediate(task);  // Lower priority
    }
}
```

## Common Misconceptions

### Myth: setTimeout(fn, 0) executes immediately
**Reality**: It schedules for the next timers phase, after microtasks and current execution.

### Myth: setImmediate always runs before setTimeout
**Reality**: In I/O callbacks, setImmediate runs first. In main module, order is non-deterministic.

### Myth: The event loop is a loop in JavaScript
**Reality**: The event loop is implemented in C++ (libuv), not JavaScript.

### Myth: Async/await makes code non-blocking
**Reality**: Async/await is syntactic sugar for Promises. The code between awaits still runs synchronously.

## Best Practices Checklist

- [ ] Understand microtask vs macrotask priority
- [ ] Avoid blocking the event loop with CPU-intensive code
- [ ] Use worker threads for CPU-bound tasks
- [ ] Monitor event loop delay in production
- [ ] Use setImmediate for yielding control
- [ ] Be careful with recursive process.nextTick
- [ ] Test event loop behavior in your specific scenarios

## Performance Optimization Tips

- Use worker threads for CPU-intensive operations
- Batch I/O operations when possible
- Monitor event loop delay and utilization
- Avoid synchronous file operations
- Use streams for large data processing
- Implement proper error handling to prevent crashes

## Cross-References

- See [V8 Internals](./05-runtime-architecture/01-v8-internals.md) for engine details
- See [Performance Deep Dive](./09-performance-deep-dive.md) for optimization
- See [Use Case Analysis](./07-use-case-analysis.md) for application patterns
- See [Real-world Cases](./11-real-world-cases.md) for production examples

## Next Steps

Now that you understand event loop mechanics, let's analyze when to use Node.js. Continue to [Node.js Use Case Analysis](./07-use-case-analysis.md).