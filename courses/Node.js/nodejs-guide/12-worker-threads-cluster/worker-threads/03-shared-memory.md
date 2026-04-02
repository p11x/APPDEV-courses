# Shared Memory

## What You'll Learn

- What `SharedArrayBuffer` is and how it differs from normal message passing
- How to create and share memory between the main thread and a worker
- What `Atomics` are and why they prevent race conditions
- How to implement a simple lock using `Atomics.wait()` and `Atomics.notify()`
- When shared memory is worth the complexity vs. message passing

## Why Shared Memory?

By default, `postMessage()` copies data. When you send a large array of numbers to a worker, Node.js serializes it, sends it, and deserializes it on the other side. For small messages this is fine. For millions of numbers, the copy overhead can dominate your runtime.

**SharedArrayBuffer** lets two threads read and write the **same** bytes in memory. No copying. No serialization. Both threads see changes instantly.

```
postMessage (default)          SharedArrayBuffer
─────────────────────          ─────────────────
Main: [1, 2, 3]                Main: ──┐
  ↓ serialize + copy              Shared: [1, 2, 3]  ← both threads point here
Worker: [1, 2, 3] (own copy)   Worker: ─┘
```

## Creating a SharedArrayBuffer

A `SharedArrayBuffer` is a fixed-size block of raw binary memory. You create one and pass it to a worker via `postMessage`.

```js
// shared-main.js — Creates shared memory and sends it to a worker

import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Allocate 1024 bytes of shared memory
// This buffer is shared — changes in either thread are visible to the other
const sharedBuffer = new SharedArrayBuffer(1024);

// Create a typed array view over the shared buffer
// Int32Array interprets each 4 bytes as a 32-bit signed integer
// We can store 1024 / 4 = 256 integers
const sharedArray = new Int32Array(sharedBuffer);

// Write initial values to the shared memory
sharedArray[0] = 0;  // This will be our "result" slot
sharedArray[1] = 0;  // This will be our "ready" flag

console.log('Main: sharedArray before worker:', sharedArray[0]);

// Pass the shared buffer to the worker
const worker = new Worker(resolve(__dirname, 'shared-worker.js'), {
  workerData: sharedBuffer,  // SharedArrayBuffer is transferred, NOT copied
});

// Wait for the worker to signal it is done
worker.on('message', (msg) => {
  if (msg === 'done') {
    // Read the value written by the worker — no copy, it is right here in memory
    console.log('Main: sharedArray after worker:', sharedArray[0]);
    worker.terminate();
  }
});
```

```js
// shared-worker.js — Receives shared memory and writes to it

import { workerData, parentPort } from 'node:worker_threads';

// workerData IS the same SharedArrayBuffer the main thread created
const sharedArray = new Int32Array(workerData);

// Simulate expensive computation and write the result directly into shared memory
sharedArray[0] = 42;  // This write is immediately visible to the main thread

// Signal the main thread that we are done
parentPort.postMessage('done');
```

### Running the Example

```bash
node shared-main.js
```

Output:

```
Main: sharedArray before worker: 0
Main: sharedArray after worker: 42
```

## The Race Condition Problem

When two threads read and write the same memory without coordination, **race conditions** occur. The result depends on which thread runs first — and that order is unpredictable.

```js
// race-demo.js — Demonstrates a race condition (DO NOT USE in production)

import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

const sharedBuffer = new SharedArrayBuffer(4);  // 4 bytes = 1 Int32
const counter = new Int32Array(sharedBuffer);
counter[0] = 0;

// Spawn 4 workers, each increments the counter 100,000 times
const workers = [];
const numWorkers = 4;
const incrementsPerWorker = 100_000;

for (let i = 0; i < numWorkers; i++) {
  const w = new Worker(resolve(__dirname, 'race-worker.js'), {
    workerData: { buffer: sharedBuffer, increments: incrementsPerWorker },
  });
  workers.push(w);
}

// Wait for all workers to finish
await Promise.all(
  workers.map(
    (w) =>
      new Promise((resolve) => {
        w.on('exit', resolve);
      })
  )
);

// Expected: 400,000 — but race conditions may produce a lower number
console.log(`Expected: ${numWorkers * incrementsPerWorker}`);
console.log(`Actual:   ${counter[0]}`);
```

```js
// race-worker.js — Increments the shared counter (has a race condition)

import { workerData } from 'node:worker_threads';

const { buffer, increments } = workerData;
const counter = new Int32Array(buffer);

// RACE CONDITION: read → modify → write is NOT atomic
// Another thread can write between our read and our write, losing an increment
for (let i = 0; i < increments; i++) {
  counter[0] = counter[0] + 1;  // Read old value, add 1, write back
  // If two threads read 5 at the same time, both write 6 — one increment is lost
}
```

Running this will often give a number **less than** 400,000 because the increment operation is not atomic.

## Atomics: Safe Concurrent Operations

The `Atomics` object provides **atomic** operations — operations that complete as a single, indivisible step. No other thread can interrupt them.

### Atomics.add

```js
// CORRECT — atomic increment
// Reads the old value, adds `value`, and stores the result — all in one step
Atomics.add(counter, 0, 1);  // Returns the OLD value before the addition
```

### Other Atomics Operations

| Operation | What it does |
|-----------|-------------|
| `Atomics.add(typedArray, index, value)` | Add `value` to element, return old value |
| `Atomics.sub(typedArray, index, value)` | Subtract `value` from element, return old value |
| `Atomics.and(typedArray, index, value)` | Bitwise AND, return old value |
| `Atomics.or(typedArray, index, value)` | Bitwise OR, return old value |
| `Atomics.xor(typedArray, index, value)` | Bitwise XOR, return old value |
| `Atomics.exchange(typedArray, index, value)` | Set element to `value`, return old value |
| `Atomics.compareExchange(typedArray, index, expected, replacement)` | If element equals `expected`, set to `replacement` |
| `Atomics.load(typedArray, index)` | Read the value atomically |
| `Atomics.store(typedArray, index, value)` | Write the value atomically |

### Fixed Race Condition Example

```js
// safe-counter-main.js — Correctly increments a shared counter with Atomics

import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

const sharedBuffer = new SharedArrayBuffer(4);
const counter = new Int32Array(sharedBuffer);
Atomics.store(counter, 0, 0);  // Atomically set initial value to 0

const numWorkers = 4;
const incrementsPerWorker = 100_000;
const workers = [];

for (let i = 0; i < numWorkers; i++) {
  const w = new Worker(resolve(__dirname, 'safe-worker.js'), {
    workerData: { buffer: sharedBuffer, increments: incrementsPerWorker },
  });
  workers.push(w);
}

await Promise.all(
  workers.map(
    (w) =>
      new Promise((resolve) => {
        w.on('exit', resolve);
      })
  )
);

// Always prints 400,000 — no race condition
console.log(`Expected: ${numWorkers * incrementsPerWorker}`);
console.log(`Actual:   ${Atomics.load(counter, 0)}`);
```

```js
// safe-worker.js — Uses Atomics.add for safe increments

import { workerData } from 'node:worker_threads';

const { buffer, increments } = workerData;
const counter = new Int32Array(buffer);

for (let i = 0; i < increments; i++) {
  // Atomics.add is atomic — no other thread can interrupt the read-modify-write
  Atomics.add(counter, 0, 1);
}
```

## Implementing a Lock with Atomics

For complex operations that require multiple steps (not just a single add), you need a **lock** (also called a **mutex**). A lock ensures only one thread enters a critical section at a time.

```js
// lock-main.js — Demonstrates a mutex protecting a critical section

import { Worker } from 'node:worker_threads';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Layout: [0] = lock (0 = unlocked, 1 = locked), [1] = shared counter
const sharedBuffer = new SharedArrayBuffer(8);  // 2 Int32 slots
const shared = new Int32Array(sharedBuffer);
Atomics.store(shared, 0, 0);  // Lock starts unlocked
Atomics.store(shared, 1, 0);  // Counter starts at 0

const workers = [];
const numWorkers = 4;
const incrementsPerWorker = 10_000;

for (let i = 0; i < numWorkers; i++) {
  workers.push(
    new Worker(resolve(__dirname, 'lock-worker.js'), {
      workerData: { buffer: sharedBuffer, increments: incrementsPerWorker },
    })
  );
}

await Promise.all(
  workers.map(
    (w) =>
      new Promise((resolve) => {
        w.on('exit', resolve);
      })
  )
);

console.log(`Expected: ${numWorkers * incrementsPerWorker}`);
console.log(`Actual:   ${shared[1]}`);
```

```js
// lock-worker.js — Uses a spinlock to protect a non-atomic multi-step operation

import { workerData } from 'node:worker_threads';

const { buffer, increments } = workerData;
const shared = new Int32Array(buffer);

// Acquire the lock — spin until we get it
function acquireLock() {
  while (true) {
    // compareExchange: if shared[0] is 0 (unlocked), set it to 1 (locked)
    // Returns the OLD value — if it was 0, we got the lock
    const old = Atomics.compareExchange(shared, 0, 0, 1);
    if (old === 0) return;  // Lock acquired

    // Lock is held by another thread — wait until it is released
    // Atomics.wait suspends this thread until Atomics.notify is called
    Atomics.wait(shared, 0, 1);  // Wait while value is still 1
  }
}

// Release the lock
function releaseLock() {
  Atomics.store(shared, 0, 0);      // Set lock back to 0 (unlocked)
  Atomics.notify(shared, 0, 1);     // Wake up ONE waiting thread
}

for (let i = 0; i < increments; i++) {
  acquireLock();

  // CRITICAL SECTION — only one thread runs this at a time
  // Multiple non-atomic operations are now safe
  const current = shared[1];
  // Simulate some extra work in the critical section
  shared[1] = current + 1;

  releaseLock();
}
```

## How It Works

### The Lock Algorithm

1. **acquireLock()**: Uses `Atomics.compareExchange(shared, 0, 0, 1)` — "if slot 0 is 0, set it to 1 and tell me the old value was 0." If another thread already locked it, the old value is 1, so we wait with `Atomics.wait()`.
2. **Critical section**: Only the thread that holds the lock enters. Any multi-step read-modify-write is safe.
3. **releaseLock()**: Sets slot 0 back to 0 and calls `Atomics.notify()` to wake up one waiting thread.

### Atomics.wait and Atomics.notify

- `Atomics.wait(typedArray, index, expectedValue)` — suspends the calling thread until `Atomics.notify()` is called on the same index, **or** until the value at that index changes from `expectedValue`.
- `Atomics.notify(typedArray, index, count)` — wakes up `count` waiting threads (or all if `count` is `+Infinity`).

## Common Mistakes

### Mistake 1: Using Normal Operations on Shared Memory

```js
// WRONG — not atomic, causes race conditions
sharedArray[0] = sharedArray[0] + 1;

// CORRECT — use Atomics for read-modify-write
Atomics.add(sharedArray, 0, 1);
```

### Mistake 2: Forgetting the Lock in Complex Operations

```js
// WRONG — three separate atomic operations are NOT atomic together
const a = Atomics.load(shared, 0);
const b = Atomics.load(shared, 1);
Atomics.store(shared, 2, a + b);  // Another thread could have changed 0 or 1

// CORRECT — protect the whole sequence with a lock
acquireLock();
const a = shared[0];
const b = shared[1];
shared[2] = a + b;
releaseLock();
```

### Mistake 3: Wrong Type for SharedArrayBuffer

```js
// WRONG — SharedArrayBuffer size must be a multiple of the typed array element size
const buf = new SharedArrayBuffer(3);  // 3 is not divisible by 4
const arr = new Int32Array(buf);       // Throws RangeError

// CORRECT — size is a multiple of 4 (Int32 = 4 bytes)
const buf = new SharedArrayBuffer(4);   // 1 Int32
const arr = new Int32Array(buf);
```

## Try It Yourself

### Exercise 1: Shared Result Array

Create two workers. Give each a `SharedArrayBuffer` of 100 `Int32` slots. Worker 1 fills slots 0–49 with its computed results. Worker 2 fills slots 50–99. The main thread reads the full array after both workers finish.

### Exercise 2: Atomic Sum

Create 4 workers. Each worker adds a different set of 1,000 numbers to a shared `Float64Array` (one slot) using `Atomics.add`. Verify the final sum matches the expected total.

### Exercise 3: Producer-Consumer with Atomics

Build a producer-consumer pattern: one worker writes numbers into a shared buffer, and the main thread reads them. Use `Atomics.wait` and `Atomics.notify` so the main thread blocks until new data is available.

## Next Steps

You understand shared memory and synchronization. Now let's combine workers into a reusable pattern for handling many tasks. Continue to [Worker Pool Pattern](./04-worker-pool-pattern.md).
