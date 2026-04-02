# Memory Management and Garbage Collection

## What You'll Learn

- V8 generational garbage collection in detail
- Memory allocation strategies
- Garbage collection tuning
- Buffer management and external memory

## V8 Garbage Collection Deep Dive

### Generational Hypothesis

```
The Generational Hypothesis:
─────────────────────────────────────────────
"Most objects die young"

V8 exploits this by:
1. New objects allocated in "New Space" (young gen)
2. Survivors promoted to "Old Space" (old gen)
3. Young gen GC (Scavenge) is fast and frequent
4. Old gen GC (Mark-Sweep) is slow but infrequent
```

### Scavenge (Young Generation)

```
New Space Layout:
─────────────────────────────────────────────
┌─────────────────────────────────────────┐
│  From-Space          To-Space           │
│  ┌──────────┐       ┌──────────┐       │
│  │ Active   │       │ Empty    │       │
│  │ objects  │  ──►  │ (target) │       │
│  └──────────┘       └──────────┘       │
│                                         │
│  Scavenge: Copy survivors to To-Space   │
│  Then swap: To becomes From             │
│  Fast: ~1ms for typical heaps           │
└─────────────────────────────────────────┘
```

### Mark-Sweep-Compact (Old Generation)

```
Old Space GC Phases:
─────────────────────────────────────────────
1. MARKING (incremental/concurrent)
   ├── Start from root objects (global, stack)
   ├── Trace all reachable objects
   ├── Mark each reachable object
   └── Unmarked = garbage

2. SWEEPING
   ├── Scan memory for unmarked objects
   ├── Add to free list
   └── Object memory reclaimed

3. COMPACTION (optional)
   ├── Move live objects together
   ├── Eliminate fragmentation
   └── Improves allocation speed
```

## Buffer Management

### Buffer.alloc vs Buffer.allocUnsafe

```javascript
// Buffer.alloc — Safe, zeroed memory
const safeBuf = Buffer.alloc(1024);
// Allocates 1KB, filled with zeros
// Slower but no data leaks

// Buffer.allocUnsafe — Fast, uninitialized memory
const unsafeBuf = Buffer.allocUnsafe(1024);
// Allocates 1KB, may contain old data
// Faster but security risk

// In Node.js 22+:
// Buffer.allocUnsafe is partially safe
// Old data from other processes is zeroed
// But data from same process may remain
```

### Buffer Pool

```javascript
// Node.js uses an internal buffer pool for small Buffers
// Pool size: 8KB (adjustable via --pending-deprecation)

// Small Buffer (< 8KB): Uses pool
const small = Buffer.alloc(100); // From pool

// Large Buffer (> 8KB): Direct allocation
const large = Buffer.alloc(10000); // Direct heap

// Check if using pool
console.log(Buffer.poolSize); // 8192
```

### Streaming Memory Management

```javascript
// BAD: Load entire file into memory
const fs = require('node:fs');
const data = fs.readFileSync('huge-file.txt'); // 2GB file = 2GB RAM

// GOOD: Stream processing
const { pipeline } = require('node:stream/promises');
const { createReadStream, createWriteStream } = require('node:fs');
const { Transform } = require('node:stream');

const toUpperCase = new Transform({
    transform(chunk, encoding, callback) {
        callback(null, chunk.toString().toUpperCase());
    }
});

await pipeline(
    createReadStream('huge-file.txt'),
    toUpperCase,
    createWriteStream('output.txt')
);
// Memory usage: ~64KB (highWaterMark) regardless of file size
```

## GC Tuning

### V8 Flags for GC

```bash
# Memory limit (default ~1.5GB on 64-bit)
node --max-old-space-size=4096 app.js  # 4GB

# New space size
node --max-semi-space-size=16 app.js  # 16MB

# Enable GC traces
node --trace-gc app.js

# Enable verbose GC traces
node --trace-gc-verbose app.js

# Expose GC for manual triggering
node --expose-gc app.js
```

### Manual GC Triggering

```javascript
// Only works with --expose-gc flag
if (global.gc) {
    console.log('Before GC:', process.memoryUsage().heapUsed / 1024 / 1024, 'MB');
    global.gc();
    console.log('After GC:', process.memoryUsage().heapUsed / 1024 / 1024, 'MB');
} else {
    console.log('Run with --expose-gc to enable manual GC');
}

// Useful in tests to get accurate memory measurements
// NOT recommended for production use
```

### GC Event Monitoring

```javascript
// Monitor GC events in production
const { PerformanceObserver } = require('node:perf_hooks');

const obs = new PerformanceObserver((items) => {
    for (const entry of items.getEntries()) {
        console.log(`GC: ${entry.kind} — ${entry.duration.toFixed(2)}ms`);
        // kind: 1 = Scavenge (young gen)
        // kind: 2 = Mark-Sweep-Compact (old gen)
        // kind: 4 = Incremental marking
        // kind: 8 = Process weak callbacks
    }
});

obs.observe({ entryTypes: ['gc'], buffered: true });

// Also via --trace-gc flag:
// [GC] Scavenge 1.2ms
// [GC] Mark-Sweep-Compact 15.3ms
```

## External Memory and ArrayBuffers

```javascript
// External memory is tracked separately from V8 heap

// Buffer external memory
const buf = Buffer.alloc(1024 * 1024); // 1MB
console.log(process.memoryUsage());
// external: ~1MB (not in V8 heap)

// ArrayBuffer
const ab = new ArrayBuffer(1024 * 1024); // 1MB
console.log(process.memoryUsage());
// arrayBuffers: ~1MB

// SharedArrayBuffer (shared between workers)
const sab = new SharedArrayBuffer(1024 * 1024);
// Tracked in arrayBuffers
```

## Best Practices Checklist

- [ ] Use streams for large data processing
- [ ] Prefer Buffer.alloc over Buffer.allocUnsafe
- [ ] Set appropriate --max-old-space-size
- [ ] Monitor external memory usage
- [ ] Use object pooling for frequently created objects
- [ ] Avoid circular references that prevent GC
- [ ] Clean up event listeners and timers
- [ ] Take heap snapshots to detect leaks
- [ ] Use WeakRef for caches where appropriate
- [ ] Run --expose-gc in tests for accurate measurements

## Cross-References

- See [V8 Internals](./01-v8-internals.md) for compilation pipeline
- See [C++ Bindings](./02-cpp-bindings-native-modules.md) for native integration
- See [Memory Optimization](../13-v8-engine-practice/03-memory-optimization.md) for leak patterns
- See [Performance Deep Dive](../09-performance-deep-dive/01-performance-characteristics.md) for monitoring

## Next Steps

Continue to [Event Loop Mechanics](../06-event-loop-mechanics/02-task-scheduling-prioritization.md) for async execution details.
