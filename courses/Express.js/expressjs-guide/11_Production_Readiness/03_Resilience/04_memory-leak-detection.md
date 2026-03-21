# Memory Leak Detection

## 📌 What You'll Learn

- Common causes of memory leaks in Express applications
- How to use Node.js built-in debugging tools
- How to use clinic.js for profiling and finding leaks
- Techniques for identifying and fixing memory issues

## 🧠 Concept Explained (Plain English)

Memory leaks happen when your application allocates memory but never releases it. Over time, the application uses more and more memory until it crashes or becomes extremely slow. In Node.js/Express, common causes include:

1. **Global variables**: Accidentally creating variables that never get garbage collected
2. **Closures holding references**: Functions that capture variables from outer scopes
3. **Event listeners not removed**: Adding listeners without removing them
4. **Growing caches**: Caches that grow indefinitely
5. **Circular references**: Objects referencing each other (though modern garbage collectors handle many of these)

**Garbage collection** is Node.js's automatic memory management. It periodically frees memory that's no longer being used. A memory leak occurs when the garbage collector can't free memory because something still references it.

**Signs of memory leaks:**
- Memory usage steadily increasing over time
- Restarted pods taking more memory than stable ones
- Garbage collection taking longer or running more frequently
- Node.js OOM (Out of Memory) crashes

**Tools for detection:**
- `--inspect` flag and Chrome DevTools
- `clinic.js` suite (doctor, flame, bubbleprof)
- Node.js heap snapshots
- Built-in `process.memoryUsage()`

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// ============================================
// Memory Monitoring Middleware
// ============================================

// Track memory usage
app.use((req, res, next) => {
  const mem = process.memoryUsage();
  
  // Log memory every 100 requests
  const requestCount = (app.locals.requestCount || 0) + 1;
  app.locals.requestCount = requestCount;
  
  if (requestCount % 100 === 0) {
    console.log(`Request #${requestCount}`);
    console.log(`Heap Used: ${Math.round(mem.heapUsed / 1024 / 1024)} MB`);
    console.log(`Heap Total: ${Math.round(mem.heapTotal / 1024 / 1024)} MB`);
    console.log(`RSS: ${Math.round(mem.rss / 1024 / 1024)} MB`);
    console.log(`External: ${Math.round(mem.external / 1024 / 1024)} MB`);
    console.log('---');
  }
  
  next();
});


// ============================================
// Example of LEAKY code (DO NOT USE IN PRODUCTION)
// ============================================

// PROBLEM 1: Growing array (cache without limit)
const leakyCache = [];
const MAX_CACHE = 1000; // Should enforce this limit

app.get('/api/leaky-users', (req, res) => {
  const user = { 
    id: Math.random(), 
    name: 'User' + leakyCache.length,
    data: new Array(10000).fill('x') // ~80KB per user
  };
  
  // Never removing from array - memory grows indefinitely
  leakyCache.push(user);
  
  res.json({ cached: leakyCache.length });
});


// PROBLEM 2: Closures holding references
const handlers = [];

app.get('/api/leaky-handlers', (req, res) => {
  const largeData = new Array(50000).fill('data');
  
  // This closure holds reference to largeData forever
  const handler = () => {
    console.log('Handler called with:', largeData.length);
  };
  
  // Storing handlers but never removing them
  handlers.push(handler);
  
  res.json({ handlersCount: handlers.length });
});


// PROBLEM 3: Event listeners accumulating
const eventEmitter = new (require('events').EventEmitter)();

app.get('/api/leaky-events', (req, res) => {
  const largeContext = { data: new Array(10000).fill('x') };
  
  // New listener every request, never removed
  eventEmitter.on('request', () => {
    console.log(largeContext.data.length);
  });
  
  res.json({ listenerCount: eventEmitter.listenerCount('request') });
});


// ============================================
// FIXED Examples (Proper Memory Management)
// ============================================

// FIX 1: LRU Cache with size limit
import { LRUCache } from 'lru-cache';

const fixedCache = new LRUCache({
  max: 1000, // Maximum number of items
  ttl: 1000 * 60 * 10 // 10 minute TTL
});

app.get('/api/fixed-users', (req, res) => {
  const user = { 
    id: Math.random(), 
    name: 'User' + fixedCache.size,
    data: new Array(10000).fill('x')
  };
  
  fixedCache.set(user.id.toString(), user);
  
  res.json({ cached: fixedCache.size });
});


// FIX 2: Remove listeners when done
app.get('/api/fixed-events', (req, res) => {
  const listenerCount = eventEmitter.listenerCount('request');
  
  // Can use .once() for one-time listeners
  eventEmitter.once('request', () => {
    console.log('One-time handler');
  });
  
  // Or explicitly remove listeners
  // eventEmitter.removeListener('request', someHandler);
  
  res.json({ listenerCount });
});


// FIX 3: Properly scoped variables
app.get('/api/fixed-scope', (req, res) => {
  // Variable goes out of scope after function returns
  const tempData = new Array(10000).fill('x');
  
  // Do work with tempData...
  const result = tempData.filter(x => x).length;
  
  // tempData automatically eligible for GC after this
  
  res.json({ result });
});


// ============================================
// Memory Profiling Endpoints
// ============================================

// Force garbage collection (if --expose-gc is enabled)
app.get('/gc', (req, res) => {
  if (global.gc) {
    global.gc();
    res.json({ message: 'Garbage collection triggered' });
  } {
    res.status(501).json({ 
      message: 'Run with --expose-gc flag to enable GC control',
      currentMemory: process.memoryUsage()
    });
  }
});

// Get current memory stats
app.get('/memory-stats', (req, res) => {
  const mem = process.memoryUsage();
  
  res.json({
    heapUsed_mb: Math.round(mem.heapUsed / 1024 / 1024),
    heapTotal_mb: Math.round(mem.heapTotal / 1024 / 1024),
    rss_mb: Math.round(mem.rss / 1024 / 1024),
    external_mb: Math.round(mem.external / 1024 / 1024),
    arrayBuffers: mem.arrayBuffers,
    
    // Derived metrics
    heapUsagePercent: Math.round((mem.heapUsed / mem.heapTotal) * 100),
    uptime_seconds: process.uptime()
  });
});


// Generate heap snapshot (for Chrome DevTools)
import { writeFileSync } from 'fs';

app.get('/heap-snapshot', (req, res) => {
  // This only works with --inspect flag and v8 profiling
  const v8 = require('v8');
  
  const snapshot = v8.writeHeapSnapshot();
  
  res.json({ 
    message: 'Heap snapshot written',
    file: snapshot
  });
});


// ============================================
// Routes
// ============================================

app.get('/api/users', (req, res) => {
  res.json([{ id: 1, name: 'John' }]);
});

app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log(`Memory stats available at http://localhost:${PORT}/memory-stats`);
  console.log(`To enable GC control: node --expose-gc ${process.argv[1]}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 13-30 | Memory monitoring middleware | Logs memory stats every 100 requests |
| 17 | `process.memoryUsage()` | Returns heap and memory stats |
| 18-20 | `heapUsed` | Memory in V8 heap (where objects live) |
| 21 | `rss` | Total process memory (includes stack, code, etc.) |
| 45-58 | Leaky cache example | Array that grows forever |
| 63-74 | Closure leak example | Closures holding large data |
| 78-87 | Event listener leak | Listeners accumulating |
| 94-100 | Fixed LRU cache | Proper bounded cache |
| 116-129 | Fixed events | Using `.once()` instead of `.on()` |
| 132-144 | Fixed scope | Variables go out of scope properly |
| 149-160 | `/gc` endpoint | Triggers garbage collection |
| 163-178 | `/memory-stats` endpoint | Returns memory usage data |
| 183-192 | `/heap-snapshot` endpoint | Writes snapshot for profiling |

## ⚠️ Common Mistakes

### 1. Using arrays as caches without limits

**What it is**: Pushing to arrays forever, never removing old items.

**Why it happens**: Simple array.push() is easy, but no eviction policy.

**How to fix it**: Use bounded caches like LRU (Least Recently Used) or add eviction logic.

### 2. Closures capturing large objects

**What it is**: Functions holding references to large data that never gets freed.

**Why it happens**: Closures capture everything from their scope.

**How to fix it**: Extract only what you need from outer scope, or use weak references.

### 3. Not removing event listeners

**What it happens**: Accumulating listeners on long-lived objects.

**Why it happens**: Forgetting to clean up listeners in cleanup handlers.

**How to fix it**: Use `.once()` for one-time handlers, remove listeners in cleanup code.

## ✅ Quick Recap

- Memory leaks in Node.js often come from unbounded caches, closures, and event listeners
- Monitor `process.memoryUsage()` to track heap usage over time
- Use `--inspect` flag and Chrome DevTools for heap snapshots
- Use `clinic.js` for profiling and finding bottlenecks
- Fix leaks by using bounded caches (LRU), avoiding closure capture, and removing listeners

## 🔗 What's Next

Now that you can detect memory leaks, learn about [Zero-Downtime Deployments](./05_zero-downtime-deployments.md) for rolling updates without service interruption.
