# Task Scheduling and Prioritization

## What You'll Learn

- Detailed event loop phase execution
- Microtask vs macrotask priority rules
- Scheduling strategies for different task types
- Performance implications of task ordering

## Event Loop Phases In Detail

```
Node.js Event Loop Phases:
─────────────────────────────────────────────
┌──────────────────────────────────────┐
│          timers                     │  setTimeout, setInterval
│  ┌──────────────────────────────┐  │  callbacks scheduled
│  │  pending callbacks           │  │  deferred I/O callbacks
│  │  ┌──────────────────────────┐│  │
│  │  │  poll                   ││  │  I/O callbacks, new I/O
│  │  │  ┌──────────────────────┐││  │  events
│  │  │  │  check              │││  │  setImmediate callbacks
│  │  │  │  ┌──────────────────┐│││  │
│  │  │  │  │  close callbacks ││││  │  socket.on('close')
│  │  │  │  └──────────────────┘│││  │
│  │  │  └──────────────────────┘││  │
│  │  └──────────────────────────┘│  │
│  └──────────────────────────────┘  │
└──────────────────────────────────────┘

Between EVERY phase: Microtask queue is drained
  - process.nextTick() (highest priority)
  - Promise.then() / catch() / finally()
  - queueMicrotask()
```

## Phase-by-Phase Behavior

### Phase 1: Timers

```javascript
// Timers phase: executes setTimeout/setInterval callbacks
// Timer expiration is MINIMUM delay, not guaranteed

const start = Date.now();

setTimeout(() => {
    // Executes in timers phase
    // Actual delay may be longer due to event loop blocking
    console.log(`Actual delay: ${Date.now() - start}ms`);
}, 100);

// Blocking operation delays timer
const blockUntil = Date.now() + 200;
while (Date.now() < blockUntil) {} // Block for 200ms

// Output: Actual delay: ~200ms (not 100ms)
```

### Phase 2: Pending Callbacks

```javascript
// Executes I/O callbacks deferred from previous iteration
// Rarely used directly by application code
// Handles: TCP errors, some DNS callbacks

const net = require('node:net');

// Example: TCP connection error handling
const server = net.createServer();
server.on('error', (err) => {
    // May execute in pending callbacks phase
    console.error('Server error:', err.message);
});
```

### Phase 3: Poll

```javascript
// Poll phase: retrieve new I/O events and execute I/O callbacks
// This is where most of your code executes

const fs = require('node:fs');

// I/O callback executes in poll phase
fs.readFile(__filename, (err, data) => {
    console.log('File read complete — poll phase');
    
    // From poll phase, we can schedule:
    setTimeout(() => console.log('Timer'), 0);     // Next timers phase
    setImmediate(() => console.log('Immediate'));   // Next check phase
});

// If no pending timers or setImmediate:
// Poll phase blocks waiting for I/O events
```

### Phase 4: Check

```javascript
// Check phase: executes setImmediate callbacks
// Runs immediately after poll phase

setImmediate(() => {
    console.log('Immediate 1 — check phase');
});

setImmediate(() => {
    console.log('Immediate 2 — check phase');
});

// Both execute in the same check phase
// Guaranteed order: Immediate 1, then Immediate 2
```

### Phase 5: Close Callbacks

```javascript
// Close callbacks: socket.on('close'), etc.

const { createServer } = require('node:net');

const server = createServer();
server.listen(0, () => {
    server.close();
    server.on('close', () => {
        // Executes in close callbacks phase
        console.log('Server closed — close callbacks phase');
    });
});
```

## Microtask Priority Rules

### Priority Order

```
Microtask Priority (highest to lowest):
─────────────────────────────────────────────
1. process.nextTick()     — Highest priority
   Executes before any other microtask or macrotask

2. Promise reactions      — High priority
   .then(), .catch(), .finally()

3. queueMicrotask()       — Same as Promise
   Standalone microtask scheduling
```

### nextTick vs Promise vs queueMicrotask

```javascript
console.log('1: sync start');

setTimeout(() => console.log('2: setTimeout (macrotask)'), 0);

setImmediate(() => console.log('3: setImmediate (check phase)'));

Promise.resolve().then(() => console.log('4: Promise (microtask)'));

queueMicrotask(() => console.log('5: queueMicrotask (microtask)'));

process.nextTick(() => console.log('6: nextTick (highest priority microtask)'));

console.log('7: sync end');

// Output:
// 1: sync start
// 7: sync end
// 6: nextTick (highest priority microtask)
// 4: Promise (microtask)
// 5: queueMicrotask (microtask)
// 3: setImmediate (check phase)
// 2: setTimeout (macrotask — order varies with setImmediate)
```

### Microtask Drain Between Phases

```javascript
// Microtasks run BETWEEN EVERY PHASE

console.log('Start');

setTimeout(() => {
    console.log('Timeout 1');
    Promise.resolve().then(() => console.log('Promise in Timeout 1'));
    process.nextTick(() => console.log('nextTick in Timeout 1'));
}, 0);

setTimeout(() => {
    console.log('Timeout 2');
}, 0);

Promise.resolve().then(() => {
    console.log('Promise 1');
    // New microtasks from within a microtask also drain
    Promise.resolve().then(() => console.log('Nested Promise'));
    process.nextTick(() => console.log('Nested nextTick'));
});

Promise.resolve().then(() => console.log('Promise 2'));

console.log('End');

// Output:
// Start
// End
// Promise 1
// Promise 2
// Nested nextTick  (nextTick > Promise)
// Nested Promise
// Timeout 1
// nextTick in Timeout 1
// Promise in Timeout 1
// Timeout 2
```

## Scheduling Strategies

### Priority-Based Scheduling

```javascript
// Custom priority scheduler using event loop phases

class TaskScheduler {
    constructor() {
        this.tasks = { critical: [], high: [], normal: [], low: [] };
    }

    // Critical: nextTick — executes ASAP, highest priority
    scheduleCritical(task) {
        process.nextTick(task);
    }

    // High: Promise — executes in microtask queue
    scheduleHigh(task) {
        Promise.resolve().then(task);
    }

    // Normal: setImmediate — executes in check phase
    scheduleNormal(task) {
        setImmediate(task);
    }

    // Low: setTimeout — executes in timers phase
    scheduleLow(task) {
        setTimeout(task, 0);
    }
}
```

### Yielding to Event Loop

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

### Batching with Microtasks

```javascript
// Use microtasks for batching synchronous operations

class BatchProcessor {
    constructor(processFn, { batchSize = 100, delay = 0 } = {}) {
        this.processFn = processFn;
        this.batchSize = batchSize;
        this.delay = delay;
        this.queue = [];
        this.scheduled = false;
    }

    add(item) {
        this.queue.push(item);
        
        if (!this.scheduled) {
            this.scheduled = true;
            
            if (this.delay > 0) {
                // Macrotask: allows other work between batches
                setTimeout(() => this.flush(), this.delay);
            } else {
                // Microtask: batch within same event loop tick
                queueMicrotask(() => this.flush());
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
    console.log(`Processing batch of ${items.length} items`);
}, { batchSize: 50 });

// Add items — they batch automatically
for (let i = 0; i < 200; i++) {
    processor.add({ id: i });
}
// Processes in 4 batches of 50
```

## Common Pitfalls

### Pitfall 1: nextTick Starvation

```javascript
// DANGER: Infinite nextTick blocks the event loop
function bad() {
    process.nextTick(bad); // Never yields to timers/I/O
}

// FIX: Use setImmediate for recursive async
function good() {
    setImmediate(good); // Yields to event loop each iteration
}
```

### Pitfall 2: Microtask Loop

```javascript
// DANGER: Deep microtask chain blocks macrotasks
let count = 0;
function badChain() {
    count++;
    if (count < 10000) {
        Promise.resolve().then(badChain);
    }
}
badChain();
setTimeout(() => console.log('This runs AFTER all 10000 microtasks'), 0);

// FIX: Mix microtasks with macrotasks
async function goodChain() {
    for (let i = 0; i < 10000; i++) {
        await doWork();
        if (i % 100 === 0) {
            await new Promise(r => setImmediate(r)); // Yield
        }
    }
}
```

### Pitfall 3: Timer Ordering Assumptions

```javascript
// WRONG ASSUMPTION: setTimeout always before setImmediate
setTimeout(() => console.log('timeout'), 0);
setImmediate(() => console.log('immediate'));
// Order is NON-DETERMINISTIC in main module

// CORRECT: In I/O callback, setImmediate always first
const fs = require('node:fs');
fs.readFile(__filename, () => {
    setTimeout(() => console.log('timeout'), 0);
    setImmediate(() => console.log('immediate'));
    // Output: immediate, timeout (always)
});
```

## Best Practices Checklist

- [ ] Use setImmediate for yielding control in long operations
- [ ] Avoid recursive process.nextTick — use setImmediate instead
- [ ] Understand that setTimeout(fn, 0) is NOT immediate
- [ ] Use queueMicrotask for high-priority sync batching
- [ ] Use setImmediate for I/O-ordered callbacks
- [ ] Monitor event loop lag in production
- [ ] Batch operations to reduce microtask queue pressure

## Cross-References

- See [Event Loop Deep Dive](./01-event-loop-deep-dive.md) for phase diagrams
- See [Event Loop Debugging](./03-event-loop-debugging.md) for debugging techniques
- See [V8 Engine Practice](../13-v8-engine-practice/02-performance-profiling.md) for profiling
- See [Performance Deep Dive](../09-performance-deep-dive/01-performance-characteristics.md) for optimization

## Next Steps

Continue to [Event Loop Debugging](./03-event-loop-debugging.md) for debugging techniques.
