# Memory Usage Monitoring and Optimization Strategies

## What You'll Learn

- V8 heap structure and garbage collection
- Memory leak detection techniques
- Optimization strategies for memory efficiency
- Production memory monitoring setup

## V8 Memory Structure

```
V8 Memory Heap Layout:
─────────────────────────────────────────────
┌─────────────────────────────────────────┐
│            New Space (Semi-space)       │
│  ┌─────────────────────────────────┐   │
│  │  Young Generation Objects       │   │
│  │  ~1-8 MB per semi-space         │   │
│  │  Frequent Scavenge GC           │   │
│  └─────────────────────────────────┘   │
│                                         │
│            Old Space                    │
│  ┌─────────────────────────────────┐   │
│  │  Old Generation Objects         │   │
│  │  Survived multiple Scavenge GC  │   │
│  │  Mark-Sweep-Compact collection  │   │
│  └─────────────────────────────────┘   │
│                                         │
│            Large Object Space           │
│  ┌─────────────────────────────────┐   │
│  │  Objects > size threshold       │   │
│  │  Never moved by GC              │   │
│  └─────────────────────────────────┘   │
│                                         │
│            Code Space                   │
│  ┌─────────────────────────────────┐   │
│  │  JIT-compiled machine code      │   │
│  └─────────────────────────────────┘   │
│                                         │
│            Map Space                    │
│  ┌─────────────────────────────────┐   │
│  │  Hidden classes (object shapes) │   │
│  └─────────────────────────────────┘   │
└─────────────────────────────────────────┘
```

## Garbage Collection Strategies

### Young Generation (Scavenge)

```javascript
// Objects start in New Space (young generation)
function createUser(name) {
    // Allocated in New Space
    const user = { name, created: Date.now() };
    return user;
}

// Scavenge GC runs frequently (~1ms)
// Surviving objects get promoted to Old Space
```

### Old Generation (Mark-Sweep-Compact)

```javascript
// Objects promoted to Old Space after surviving
// Mark-Sweep-Compact runs less frequently but takes longer

// Good: Objects get garbage collected when no longer referenced
function processRequest(data) {
    const temp = { processed: true, data };
    return temp.processed;
} // temp is eligible for GC after function returns

// BAD: Memory leak — objects never released
const cache = new Map();
function processAndCache(id, data) {
    const result = expensiveProcess(data);
    cache.set(id, result); // Never removed!
    return result;
}
```

### Incremental and Concurrent GC

```
V8 GC Optimization (modern versions):
─────────────────────────────────────────────
Incremental GC:
├── Breaks marking into small steps
├── Interleaved with JavaScript execution
├── Reduces pause times from ~100ms to ~5ms
└── Enabled by default in modern V8

Concurrent GC:
├── Runs marking in background thread
├── JavaScript continues executing
├── Further reduces pause times
└── Scavenging done concurrently in V8 11+
```

## Memory Monitoring

### Real-Time Monitoring

Create `memory-monitor.js`:
```javascript
// memory-monitor.js — Comprehensive memory monitoring

import { performance } from 'node:perf_hooks';
import v8 from 'node:v8';

class MemoryMonitor {
    constructor(options = {}) {
        this.interval = options.interval || 5000;
        this.alertThresholdMB = options.alertThresholdMB || 500;
        this.history = [];
        this.maxHistory = options.maxHistory || 100;
    }

    start() {
        this.timer = setInterval(() => this.collect(), this.interval);
        console.log(`Memory monitor started (interval: ${this.interval}ms)`);
    }

    collect() {
        const usage = process.memoryUsage();
        const heap = v8.getHeapStatistics();
        
        const snapshot = {
            timestamp: Date.now(),
            rss: this.toMB(usage.rss),
            heapUsed: this.toMB(usage.heapUsed),
            heapTotal: this.toMB(usage.heapTotal),
            external: this.toMB(usage.external),
            arrayBuffers: this.toMB(usage.arrayBuffers),
            heapLimit: this.toMB(heap.heap_size_limit),
        };

        this.history.push(snapshot);
        if (this.history.length > this.maxHistory) {
            this.history.shift();
        }

        // Alert on high usage
        if (snapshot.heapUsed > this.alertThresholdMB) {
            console.warn(`[ALERT] Heap usage: ${snapshot.heapUsed}MB exceeds ${this.alertThresholdMB}MB`);
        }

        // Detect rapid growth
        if (this.history.length >= 2) {
            const prev = this.history[this.history.length - 2];
            const growth = snapshot.heapUsed - prev.heapUsed;
            if (growth > 50) { // 50MB growth in one interval
                console.warn(`[ALERT] Rapid heap growth: +${growth.toFixed(1)}MB`);
            }
        }

        return snapshot;
    }

    getGrowthRate() {
        if (this.history.length < 2) return 0;
        const first = this.history[0];
        const last = this.history[this.history.length - 1];
        const timeDiffMin = (last.timestamp - first.timestamp) / 60000;
        const heapDiffMB = last.heapUsed - first.heapUsed;
        return timeDiffMin > 0 ? heapDiffMB / timeDiffMin : 0;
    }

    getReport() {
        const current = this.history[this.history.length - 1] || this.collect();
        return {
            current,
            growthRateMBPerMin: this.getGrowthRate().toFixed(2),
            samples: this.history.length,
        };
    }

    toMB(bytes) {
        return +(bytes / 1024 / 1024).toFixed(1);
    }

    stop() {
        clearInterval(this.timer);
    }
}

// Usage
const monitor = new MemoryMonitor({ interval: 10000 });
monitor.start();

// Periodic report
setInterval(() => {
    const report = monitor.getReport();
    console.log('[Memory Report]', report);
}, 30000);
```

## Memory Leak Patterns and Fixes

### Leak Pattern 1: Unbounded Caches

```javascript
// LEAK: Cache grows indefinitely
const cache = new Map();

function getCachedData(key) {
    if (cache.has(key)) return cache.get(key);
    const data = expensiveComputation(key);
    cache.set(key, data);
    return data;
}

// FIX: LRU Cache with size limit
class LRUCache {
    constructor(maxSize) {
        this.maxSize = maxSize;
        this.cache = new Map();
    }

    get(key) {
        if (!this.cache.has(key)) return undefined;
        // Move to end (most recently used)
        const value = this.cache.get(key);
        this.cache.delete(key);
        this.cache.set(key, value);
        return value;
    }

    set(key, value) {
        if (this.cache.has(key)) {
            this.cache.delete(key);
        } else if (this.cache.size >= this.maxSize) {
            // Remove oldest (first item)
            const oldest = this.cache.keys().next().value;
            this.cache.delete(oldest);
        }
        this.cache.set(key, value);
    }
}

const safeCache = new LRUCache(1000);
```

### Leak Pattern 2: Event Listener Accumulation

```javascript
// LEAK: Listeners added but never removed
const EventEmitter = require('node:events');
const emitter = new EventEmitter();

function handleRequest(req) {
    // New listener added per request — never removed!
    emitter.on('data', (data) => {
        processResponse(req, data);
    });
}

// FIX: Remove listeners or use once()
function handleRequestSafe(req) {
    const handler = (data) => processResponse(req, data);
    emitter.once('data', handler); // Auto-removed after first call
    
    // Or manually remove:
    // emitter.on('data', handler);
    // req.on('close', () => emitter.off('data', handler));
}
```

### Leak Pattern 3: Closure References

```javascript
// LEAK: Closure captures large object
function processLargeDataset(dataset) {
    const hugeArray = new Array(1000000).fill('data');
    
    return function handler() {
        // hugeArray stays in memory as long as handler exists
        return hugeArray.length;
    };
}

const handler = processLargeDataset();
// hugeArray is never GC'd because handler references it

// FIX: Extract only what's needed
function processLargeDatasetSafe(dataset) {
    const hugeArray = new Array(1000000).fill('data');
    const length = hugeArray.length;
    // hugeArray can now be GC'd
    
    return function handler() {
        return length; // Only captures the number
    };
}
```

### Leak Pattern 4: Global Variable Accumulation

```javascript
// LEAK: Accidental globals (missing let/const)
function processData(data) {
    result = expensiveComputation(data); // Missing const/let!
    return result;
    // 'result' becomes global, never collected
}

// FIX: Always use strict mode and const/let
'use strict';
function processDataSafe(data) {
    const result = expensiveComputation(data);
    return result;
}
```

### Leak Pattern 5: Timer/Interval Cleanup

```javascript
// LEAK: Intervals never cleared
function startPolling(url) {
    setInterval(() => {
        fetch(url).then(processData);
    }, 5000);
}
// Multiple calls = multiple intervals = memory leak

// FIX: Track and clear timers
class PollingManager {
    constructor() {
        this.intervals = new Map();
    }

    start(key, url, intervalMs = 5000) {
        this.stop(key); // Clear existing
        const id = setInterval(() => {
            fetch(url).then(processData);
        }, intervalMs);
        this.intervals.set(key, id);
    }

    stop(key) {
        const id = this.intervals.get(key);
        if (id) {
            clearInterval(id);
            this.intervals.delete(key);
        }
    }

    stopAll() {
        for (const [key] of this.intervals) {
            this.stop(key);
        }
    }
}
```

## WeakRef and FinalizationRegistry

```javascript
// Using WeakRef for memory-efficient caches
const cache = new Map();

function getCachedData(key) {
    const ref = cache.get(key);
    if (ref) {
        const data = ref.deref();
        if (data) return data;
        // Reference was garbage collected
    }
    
    const data = expensiveComputation(key);
    cache.set(key, new WeakRef(data));
    return data;
}

// Using FinalizationRegistry for cleanup
const registry = new FinalizationRegistry((heldValue) => {
    console.log(`Resource cleaned up: ${heldValue}`);
    // Close file handles, network connections, etc.
});

function openResource(path) {
    const resource = { path, handle: openFile(path) };
    registry.register(resource, path);
    return resource;
}
```

## Heap Snapshot Leak Detection

```javascript
// leak-detector.js — Automated leak detection

import v8 from 'node:v8';
import fs from 'node:fs';

class LeakDetector {
    constructor() {
        this.snapshots = [];
    }

    async takeSnapshot(label) {
        if (global.gc) global.gc(); // Run with --expose-gc
        
        const filename = `heap-${label}-${Date.now()}.heapsnapshot`;
        v8.writeHeapSnapshot(filename);
        
        const stats = v8.getHeapStatistics();
        this.snapshots.push({
            label,
            filename,
            timestamp: Date.now(),
            heapUsed: stats.used_heap_size,
        });
        
        console.log(`Snapshot taken: ${filename} (${(stats.used_heap_size / 1024 / 1024).toFixed(1)}MB)`);
        return filename;
    }

    compare(label1, label2) {
        const s1 = this.snapshots.find(s => s.label === label1);
        const s2 = this.snapshots.find(s => s.label === label2);
        
        if (!s1 || !s2) throw new Error('Snapshot not found');
        
        const diff = s2.heapUsed - s1.heapUsed;
        const diffMB = (diff / 1024 / 1024).toFixed(1);
        
        console.log(`Memory change: ${diff > 0 ? '+' : ''}${diffMB}MB`);
        console.log(`  ${label1}: ${(s1.heapUsed / 1024 / 1024).toFixed(1)}MB`);
        console.log(`  ${label2}: ${(s2.heapUsed / 1024 / 1024).toFixed(1)}MB`);
        
        if (diff > 10 * 1024 * 1024) { // > 10MB growth
            console.warn('WARNING: Significant memory growth detected');
            console.log(`Analyze: Open ${s1.filename} and ${s2.filename} in Chrome DevTools`);
            console.log('Use Comparison view to find growing objects');
        }
        
        return { before: s1, after: s2, diffMB };
    }
}

// Usage
const detector = new LeakDetector();

// Take baseline snapshot
await detector.takeSnapshot('baseline');

// Run operations that might leak
for (let i = 0; i < 100; i++) {
    // ... your operations ...
}

// Take post-operation snapshot
await detector.takeSnapshot('after-ops');

// Compare
detector.compare('baseline', 'after-ops');
```

## Best Practices Checklist

- [ ] Monitor heap usage in production
- [ ] Set `--max-old-space-size` based on available memory
- [ ] Use WeakRef for caches where appropriate
- [ ] Always clean up event listeners and timers
- [ ] Use `const`/`let` — never implicit globals
- [ ] Enable strict mode in all files
- [ ] Take periodic heap snapshots in staging
- [ ] Use LRU caches with size limits
- [ ] Run `--expose-gc` in testing for accurate measurements
- [ ] Profile memory after long-running operations

## Cross-References

- See [Compilation Demonstration](./01-compilation-demonstration.md) for V8 internals
- See [Performance Profiling](./02-performance-profiling.md) for profiling techniques
- See [Performance Deep Dive](../09-performance-deep-dive/01-performance-characteristics.md) for patterns
- See [Runtime Architecture](../05-runtime-architecture/03-memory-management.md) for GC details

## Next Steps

Continue to [Runtime Architecture](../05-runtime-architecture/02-cpp-bindings-native-modules.md) to learn about C++ integration.
