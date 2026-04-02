# Event Loop Implementation Deep Dive

## What You'll Learn

- Detailed event loop phase diagram
- Task queue mechanics and timing
- Microtask vs macrotask scheduling
- Performance implications of the concurrency model

## The Event Loop Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      EVENT LOOP                              │
│                                                             │
│   ┌─────────────────────────────────────────────────┐      │
│   │  ┌──────────┐                                   │      │
│   │  │  timers  │  setTimeout, setInterval          │      │
│   │  └────┬─────┘                                   │      │
│   │       ▼                                         │      │
│   │  ┌──────────┐                                   │      │
│   │  │ pending  │  I/O callbacks deferred           │      │
│   │  │callbacks │  from previous iteration          │      │
│   │  └────┬─────┘                                   │      │
│   │       ▼                                         │      │
│   │  ┌──────────┐                                   │      │
│   │  │  poll    │  Retrieve new I/O events          │      │
│   │  │          │  Execute I/O callbacks             │      │
│   │  └────┬─────┘                                   │      │
│   │       ▼                                         │      │
│   │  ┌──────────┐                                   │      │
│   │  │  check   │  setImmediate callbacks           │      │
│   │  └────┬─────┘                                   │      │
│   │       ▼                                         │      │
│   │  ┌──────────┐                                   │      │
│   │  │  close   │  socket.on('close')              │      │
│   │  │callbacks │                                   │      │
│   │  └────┬─────┘                                   │      │
│   │       │                                         │      │
│   │       ▼                                         │      │
│   │  Between EVERY phase:                            │      │
│   │  ┌─────────────────────────────────────┐        │      │
│   │  │  MICROTASK QUEUE DRAIN              │        │      │
│   │  │  1. process.nextTick() (highest)    │        │      │
│   │  │  2. Promise reactions               │        │      │
│   │  │  3. queueMicrotask()                │        │      │
│   │  └─────────────────────────────────────┘        │      │
│   └─────────────────────────────────────────────────┘      │
└─────────────────────────────────────────────────────────────┘
```

## Phase Details

### Phase 1: Timers

```javascript
// Timers phase: executes callbacks scheduled by setTimeout/setInterval
// Timer expiration is MINIMUM delay, not guaranteed

const start = Date.now();

setTimeout(() => {
    console.log(`Actual delay: ${Date.now() - start}ms`);
}, 100);

// If event loop is busy, timer fires later
while (Date.now() - start < 200) {} // Blocks for 200ms
// Timer fires at ~200ms, not 100ms
```

### Phase 2: Pending Callbacks

```javascript
// Executes I/O callbacks deferred from previous iteration
// Handles system-level callbacks (TCP errors, some DNS)
// Rarely used directly by application code
```

### Phase 3: Poll

```javascript
// Poll phase: retrieve new I/O events and execute callbacks
// This is where most application code runs

import { readFile } from 'node:fs/promises';

// I/O callback executes in poll phase
readFile('./data.txt', 'utf-8').then(data => {
    console.log('File read — poll phase');

    // From poll, we can schedule:
    setTimeout(() => console.log('Timer'), 0);    // Next timers phase
    setImmediate(() => console.log('Immediate'));  // Next check phase
});
```

### Phase 4: Check

```javascript
// Check phase: executes setImmediate callbacks
// Runs immediately after poll phase completes

setImmediate(() => console.log('Immediate 1 — check phase'));
setImmediate(() => console.log('Immediate 2 — check phase'));
// Both execute in same check phase, in order
```

### Phase 5: Close Callbacks

```javascript
// Close callbacks: socket.on('close'), etc.
import { createServer } from 'node:net';

const server = createServer();
server.listen(0);
server.close();
server.on('close', () => {
    console.log('Server closed — close callbacks phase');
});
```

## Microtask vs Macrotask

```
Priority Queue (highest to lowest):
─────────────────────────────────────────────
MICROTASKS (run between EVERY phase):
├── process.nextTick()     — Highest priority
├── Promise.then()         — High priority
├── Promise.catch()        — High priority
├── Promise.finally()      — High priority
└── queueMicrotask()       — Same as Promise

MACROTASKS (one per event loop iteration):
├── setTimeout callbacks   — Timers phase
├── setInterval callbacks  — Timers phase
├── I/O callbacks          — Poll phase
├── setImmediate callbacks — Check phase
└── close callbacks        — Close phase
```

```javascript
console.log('1: sync start');

setTimeout(() => console.log('2: setTimeout (macrotask)'), 0);
setImmediate(() => console.log('3: setImmediate (macrotask)'));
Promise.resolve().then(() => console.log('4: Promise (microtask)'));
queueMicrotask(() => console.log('5: queueMicrotask (microtask)'));
process.nextTick(() => console.log('6: nextTick (highest microtask)'));
setImmediate(() => console.log('7: setImmediate 2'));

console.log('8: sync end');

// Output:
// 1: sync start
// 8: sync end
// 6: nextTick (highest microtask)
// 4: Promise (microtask)
// 5: queueMicrotask (microtask)
// 3: setImmediate (macrotask)
// 7: setImmediate 2
// 2: setTimeout (macrotask — order varies with setImmediate)
```

## Microtask Drain Between Phases

```javascript
// Microtasks run BETWEEN EVERY PHASE
// Queue is fully drained before next phase

console.log('Start');

setTimeout(() => {
    console.log('Timeout');
    Promise.resolve().then(() => console.log('Promise in Timeout'));
    process.nextTick(() => console.log('nextTick in Timeout'));
}, 0);

Promise.resolve().then(() => {
    console.log('Promise 1');
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
// Nested nextTick
// Nested Promise
// Timeout
// nextTick in Timeout
// Promise in Timeout
```

## nextTick vs setImmediate Ordering

```javascript
import { readFile } from 'node:fs';

// In I/O callback: setImmediate ALWAYS runs before setTimeout
readFile(__filename, () => {
    setTimeout(() => console.log('timeout'), 0);
    setImmediate(() => console.log('immediate'));
    // Output: immediate, timeout (always)
});

// In main module: order is NON-DETERMINISTIC
setTimeout(() => console.log('timeout'), 0);
setImmediate(() => console.log('immediate'));
// Output: depends on system load (could be either order)
```

## Performance Implications

```
Event Loop Performance Thresholds:
─────────────────────────────────────────────
< 10ms    │ Acceptable — responsive application
10-50ms   │ Noticeable — slight UI lag possible
50-100ms  │ Degraded — user may notice delays
100-500ms │ Poor — significant user impact
> 500ms   │ Critical — application appears frozen
> 1000ms  │ Severe — connections may timeout
```

## Best Practices Checklist

- [ ] Keep event loop iterations under 10ms
- [ ] Use setImmediate for yielding control
- [ ] Avoid recursive process.nextTick (starves event loop)
- [ ] Monitor event loop lag in production
- [ ] Use worker threads for CPU-bound tasks
- [ ] Understand that setTimeout(fn, 0) is NOT immediate

## Cross-References

- See [Promise Implementation](./02-promise-implementation.md) for Promise internals
- See [Task Scheduling](./03-task-scheduling.md) for scheduling strategies
- See [Callbacks](../02-callbacks/01-what-are-callbacks.md) for callback patterns
- See [Promises](../03-promises/01-promise-basics.md) for Promise basics

## Next Steps

Continue to [Promise Implementation](./02-promise-implementation.md) for Promise internals.
