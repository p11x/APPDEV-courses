# Why Worker Threads

## What You'll Learn

- Why Node.js is single-threaded by default
- What CPU-bound and I/O-bound tasks are
- How the default thread pool limits heavy computation
- When you should reach for worker threads
- How worker threads differ from clustering

## The Single-Thread Problem

Node.js runs your JavaScript on a single thread called the **main thread**. This means only one piece of code executes at a time. For most web servers this is fine because Node.js spends most of its time waiting for databases, file systems, and network requests — these are **I/O-bound** tasks, and the event loop handles them efficiently.

But some tasks are **CPU-bound**: they keep the processor busy doing math, parsing, or computation. When the main thread runs a CPU-bound task, it blocks everything else — no new requests, no callbacks, no timers — until the work finishes.

## CPU-Bound vs I/O-Bound

| Type | Example | Node.js handles it |
|------|---------|-------------------|
| I/O-bound | Database query, HTTP request, file read | Very well (event loop) |
| CPU-bound | Image resize, large JSON parse, Fibonacci, crypto hashing | Poorly (blocks main thread) |

## The libuv Thread Pool

Node.js uses a C library called **libuv** internally. libuv has a thread pool of **4 threads** (configurable up to 1024 via `UV_THREADPOOL_SIZE`). Some Node.js APIs automatically use this pool:

- `node:fs` file operations (read, write)
- `node:crypto` functions (pbkdf2, scrypt)
- DNS lookups

But your own custom CPU-heavy JavaScript code runs on the **main thread only**. There is no automatic offloading.

## Demo: Blocking the Main Thread

```js
// blocking-demo.js — Shows how CPU work freezes the server

import { createServer } from 'node:http';

// A deliberately slow CPU-bound function
// Calculates Fibonacci recursively — exponential time complexity
function fibonacci(n) {
  if (n <= 1) return n;              // Base case: fib(0) = 0, fib(1) = 1
  return fibonacci(n - 1) + fibonacci(n - 2); // Recursive case
}

const server = createServer((req, res) => {
  if (req.url === '/fib') {
    const start = performance.now();               // High-resolution timer
    const result = fibonacci(42);                   // ~1–2 seconds of CPU work
    const elapsed = (performance.now() - start).toFixed(1);

    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({ result, elapsed: `${elapsed}ms` }));
    return;
  }

  res.writeHead(200, { 'Content-Type': 'text/plain' });
  res.end('Hello from main thread!');
});

server.listen(3000, () => {
  console.log('Server listening on http://localhost:3000');
  console.log('Try hitting /fib from two browser tabs simultaneously');
});
```

### The Problem

If you open two browser tabs and hit `GET /fib` at the same time, the second request will **wait** until the first finishes. The event loop is stuck computing Fibonacci — it cannot accept new connections or run other callbacks.

```
Tab 1 → GET /fib  → starts computing... (2000ms)
Tab 2 → GET /fib  → waiting...          (blocked)
Tab 1 → responds with result
Tab 2 → starts computing...             (now it runs)
```

This is unacceptable for a production server. Every request is serialized.

## How Worker Threads Solve This

A **worker thread** is a separate JavaScript execution environment that runs in its own V8 isolate. It has its own:

- Memory heap (isolated by default, but can be shared)
- Event loop
- Set of built-in modules

The main thread can create a worker, send it data, let it do the heavy computation, and receive the result — all without blocking the main event loop.

```
Main Thread                Worker Thread
────────────               ─────────────
accept request
send data to worker ──────→ receive data
  (non-blocking)             compute fibonacci
accept next request          post result back
  (still responsive!)  ←──── receive result
send response
```

## When to Use Worker Threads

Use worker threads when you have:

1. **CPU-intensive computation** — parsing, compression, image processing, machine learning inference
2. **Synchronous blocking code** — libraries or algorithms that cannot be made async
3. **Parallel processing** — splitting a large dataset across multiple cores

Do **not** use worker threads for:

- Database queries (already async I/O)
- HTTP requests (already async I/O)
- File system access (already async, uses libuv pool)
- Anything the event loop handles well

> See: ../01-introduction/what-is-nodejs/02-event-loop.md for a refresher on how the event loop processes I/O.

## How It Works

1. The **main thread** creates a worker using `new Worker()`
2. The worker loads a separate JavaScript file and runs it in its own V8 context
3. Communication happens through **messages** — the main thread calls `worker.postMessage()` and the worker calls `parentPort.postMessage()`
4. Messages are copied by default (structured clone), so each thread has its own data
5. For shared data, you can use `SharedArrayBuffer` (covered in [Shared Memory](./03-shared-memory.md))

## Common Mistakes

### Mistake 1: Using Workers for I/O Tasks

```js
// WRONG — workers add overhead for I/O; the event loop already handles this
import { Worker } from 'node:worker_threads';

// This is pointless — reading a file is already non-blocking
const worker = new Worker('./read-file.js');

// CORRECT — just use async file APIs
import { readFile } from 'node:fs/promises';
const data = await readFile('big-file.txt', 'utf-8');
```

### Mistake 2: Creating a Worker for Every Request

```js
// WRONG — creating a worker is expensive (new V8 isolate, new heap)
server.on('request', (req, res) => {
  const worker = new Worker('./compute.js'); // ~50ms startup overhead per request
  worker.on('message', (result) => {
    res.end(String(result));
    worker.terminate();
  });
});

// CORRECT — use a pool of pre-created workers (see worker-pool-pattern)
```

### Mistake 3: Expecting Shared Memory by Default

```js
// WRONG — data is copied, not shared, by default
import { Worker } from 'node:worker_threads';

const myData = { count: 0 };
const worker = new Worker('./worker.js', { workerData: myData });

// Changing myData here will NOT affect the worker's copy
myData.count = 999;
// The worker still sees count: 0

// CORRECT — use SharedArrayBuffer for shared memory (see shared-memory)
```

## Try It Yourself

### Exercise 1: Measure Blocking

Run the blocking demo above. Open two terminals and send requests at the same time using `curl`. Measure how long the second request takes:

```bash
# Terminal 1
curl http://localhost:3000/fib

# Terminal 2 (send at the same time)
curl http://localhost:3000/fib
```

Record the total time for each request. Notice the second request is delayed.

### Exercise 2: Identify CPU-Bound Code

Look at a project you have worked on. Identify at least two functions that are CPU-bound (they do heavy computation, not I/O). Write down why each one blocks the main thread.

### Exercise 3: Research

Read the official Node.js `worker_threads` documentation at `node:worker_threads`. List three properties available on `workerData` and explain what `parentPort` does.

## Next Steps

You understand why workers exist and when to use them. Let's learn how to create them. Continue to [Creating Workers](./02-creating-workers.md).
