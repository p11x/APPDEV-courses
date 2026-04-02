# Memory Leaks

## What You'll Learn

- What a memory leak is and how it manifests in Node.js
- How to take heap snapshots with `--heap-snapshot`
- How to use Chrome DevTools Memory tab to find leaks
- Common memory leak patterns and how to fix them
- How to monitor memory usage in production

## What Is a Memory Leak?

A memory leak occurs when your application allocates memory but never releases it. Over time, memory usage grows until the process runs out of memory and crashes.

```
Normal:  [██████░░░░░░░░░░░░░░] 30% — stable
Leak:    [████████████████████] 95% — growing, about to crash
```

## Taking Heap Snapshots

```bash
# Start with heap snapshot support
node --heapsnapshot-signal=SIGUSR2 server.js

# Send signal to take a snapshot
kill -USR2 $(pgrep -f "node server.js")
# Creates a file like Heap.20240115.103000.12345.heapsnapshot
```

Or take a snapshot programmatically:

```js
// Take a heap snapshot from code
import { writeHeapSnapshot } from 'node:v8';

// Write a heap snapshot file
writeHeapSnapshot();
// Creates Heap.20240115.103000.12345.heapsnapshot

// Custom filename
writeHeapSnapshot('my-snapshot.heapsnapshot');
```

## Analyzing with Chrome DevTools

1. Open Chrome → `chrome://inspect`
2. Connect to your Node.js process
3. Go to the **Memory** tab
4. Click **Take heap snapshot**
5. Use the app for a while
6. Take another snapshot
7. Select **Comparison** view — compare the two snapshots

### What to Look For

| Sign | Meaning |
|------|---------|
| Objects growing in count | Leak — objects are created but not released |
| Large retained size | One object holds a lot of memory |
| Detached DOM nodes | Nodes removed from DOM but still referenced |

## Common Leak Patterns

### Leak 1: Growing Cache Without Bounds

```js
// LEAK — cache grows forever
const cache = {};

function getData(key) {
  if (!cache[key]) {
    cache[key] = expensiveOperation(key);
  }
  return cache[key];
}

// Every unique key adds to cache — memory grows without limit

// FIX — use an LRU cache with a size limit
import { LRUCache } from 'lru-cache';  // npm install lru-cache

const cache = new LRUCache({
  max: 1000,          // Maximum 1000 entries
  ttl: 1000 * 60 * 5, // 5 minute TTL
});
```

### Leak 2: Event Listener Accumulation

```js
// LEAK — listeners added but never removed
function handleConnection(socket) {
  const onData = (data) => { /* ... */ };
  socket.on('data', onData);
  // Never removed — even after socket closes
}

// FIX — remove listeners when done
function handleConnection(socket) {
  const onData = (data) => { /* ... */ };
  socket.on('data', onData);

  socket.on('close', () => {
    socket.removeListener('data', onData);
  });
}
```

### Leak 3: Closure References

```js
// LEAK — closure captures large data
function processFile(filePath) {
  const largeBuffer = readFileSync(filePath);  // 100MB buffer

  return () => {
    // This function captures largeBuffer in its closure
    // Even if we only need a tiny piece of data
    return largeBuffer.length;
  };
}

const getLength = processFile('huge-file.bin');
// largeBuffer stays in memory as long as getLength exists

// FIX — extract only what you need
function processFile(filePath) {
  const largeBuffer = readFileSync(filePath);
  const length = largeBuffer.length;
  // largeBuffer can be garbage collected after this function returns

  return () => length;  // Only captures the number, not the buffer
}
```

## Monitoring Memory

```js
// memory-monitor.js — Log memory usage periodically

function logMemory() {
  const usage = process.memoryUsage();

  console.log({
    rss: `${(usage.rss / 1024 / 1024).toFixed(1)}MB`,         // Total memory
    heapUsed: `${(usage.heapUsed / 1024 / 1024).toFixed(1)}MB`, // JS heap used
    heapTotal: `${(usage.heapTotal / 1024 / 1024).toFixed(1)}MB`, // JS heap total
    external: `${(usage.external / 1024 / 1024).toFixed(1)}MB`,  // C++ objects
  });
}

// Log every 5 seconds
setInterval(logMemory, 5000);

// Log on low memory warning
process.on('warning', (warning) => {
  if (warning.name === 'MaxListenersExceededWarning') {
    console.error('MEMORY WARNING:', warning.message);
  }
});
```

## How It Works

### Garbage Collection

Node.js uses V8's garbage collector. It frees memory that is no longer reachable:

```
Root (global, stack)
  │
  ├── obj1 ──→ obj2 ──→ obj3
  │
  └── obj4

obj5 (unreachable → GC frees it)
```

A leak occurs when unreachable objects are still referenced (e.g., in a growing array or closure).

### process.memoryUsage()

| Field | Meaning |
|-------|---------|
| `rss` | Resident Set Size — total memory allocated |
| `heapUsed` | JS heap actually used |
| `heapTotal` | JS heap allocated (may be larger than used) |
| `external` | Memory used by C++ objects (Buffers, etc.) |

## Common Mistakes

### Mistake 1: Global Arrays That Never Shrink

```js
// WRONG — logs array grows forever
const logs = [];
app.use((req, res, next) => {
  logs.push({ url: req.url, time: Date.now() });
  next();
});

// CORRECT — rotate logs
const MAX_LOGS = 10000;
app.use((req, res, next) => {
  logs.push({ url: req.url, time: Date.now() });
  if (logs.length > MAX_LOGS) logs.shift();  // Remove oldest
  next();
});
```

### Mistake 2: Not Cleaning Up Timers

```js
// WRONG — timer runs forever even when no longer needed
const interval = setInterval(() => {
  updateDashboard();
}, 1000);

// CORRECT — clear the interval when done
const interval = setInterval(updateDashboard, 1000);
// Later:
clearInterval(interval);
```

### Mistake 3: Storing References to Request Objects

```js
// WRONG — each request object is stored and never released
const recentRequests = [];
app.use((req, res, next) => {
  recentRequests.push(req);  // req contains the entire request body, headers, etc.
  next();
});

// CORRECT — extract only the data you need
const recentRequests = [];
app.use((req, res, next) => {
  recentRequests.push({ method: req.method, url: req.url, time: Date.now() });
  next();
});
```

## Try It Yourself

### Exercise 1: Create a Leak

Write a function that adds 1MB strings to an array on every call. Call it 100 times. Watch `process.memoryUsage()` grow.

### Exercise 2: Find the Leak

Take a heap snapshot before and after calling the leaky function. Use DevTools Comparison view to find the growing objects.

### Exercise 3: Fix the Leak

Replace the growing array with an LRU cache. Verify memory stays stable.

## Next Steps

You can find and fix memory leaks. For email sending, continue to [Chapter 24: Email Sending](../../24-email-sending/nodemailer/01-nodemailer-setup.md).
