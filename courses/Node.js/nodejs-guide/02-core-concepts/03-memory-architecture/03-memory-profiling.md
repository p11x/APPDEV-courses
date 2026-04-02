# Memory Profiling Tools and Leak Detection

## What You'll Learn

- Taking and analyzing heap snapshots
- Detecting memory leaks programmatically
- Using Chrome DevTools for memory analysis
- Production memory monitoring

## Heap Snapshots

```bash
# Signal-based (production)
node --heapsnapshot-signal=SIGUSR2 app.js &
kill -USR2 $!  # Creates .heapsnapshot file

# CLI-based
node --prof app.js
```

```javascript
// Programmatic heap snapshots
import v8 from 'node:v8';

// Write heap snapshot to file
const snapshotPath = v8.writeHeapSnapshot();
console.log(`Snapshot written: ${snapshotPath}`);

// Get heap statistics
const stats = v8.getHeapStatistics();
console.log({
    totalHeapSize: `${(stats.total_heap_size / 1024 / 1024).toFixed(1)} MB`,
    usedHeapSize: `${(stats.used_heap_size / 1024 / 1024).toFixed(1)} MB`,
    heapSizeLimit: `${(stats.heap_size_limit / 1024 / 1024).toFixed(1)} MB`,
    totalAvailableSize: `${(stats.total_available_size / 1024 / 1024).toFixed(1)} MB`,
});
```

## Leak Detection

### Common Leak Patterns

```javascript
// LEAK 1: Unbounded cache
const cache = new Map();
function process(id, data) {
    cache.set(id, expensiveComputation(data)); // Never evicts!
}

// FIX: LRU cache with size limit
class LRUCache {
    constructor(max) { this.max = max; this.cache = new Map(); }
    get(key) {
        if (!this.cache.has(key)) return undefined;
        const val = this.cache.get(key);
        this.cache.delete(key);
        this.cache.set(key, val); // Move to end
        return val;
    }
    set(key, val) {
        if (this.cache.has(key)) this.cache.delete(key);
        else if (this.cache.size >= this.max) this.cache.delete(this.cache.keys().next().value);
        this.cache.set(key, val);
    }
}

// LEAK 2: Event listener accumulation
emitter.on('data', handler); // Added but never removed

// FIX: Remove listeners or use once
emitter.once('data', handler);
// Or: req.on('close', () => emitter.off('data', handler));

// LEAK 3: Closure capturing large objects
function processLarge(data) {
    const bigArray = new Array(1000000).fill('x');
    return () => bigArray.length; // bigArray stays alive
}

// FIX: Extract only what's needed
function processLargeSafe(data) {
    const bigArray = new Array(1000000).fill('x');
    const len = bigArray.length;
    return () => len; // bigArray can be GC'd
}

// LEAK 4: Global variables
function handler() {
    result = compute(); // Missing const/let!
}

// FIX: Use strict mode + const/let
'use strict';
function handler() {
    const result = compute();
}
```

### Automated Leak Detection

```javascript
class LeakDetector {
    constructor(thresholdMB = 10) {
        this.thresholdMB = thresholdMB;
        this.snapshots = [];
    }

    snapshot(label) {
        if (global.gc) global.gc();
        this.snapshots.push({
            label,
            timestamp: Date.now(),
            heapUsed: process.memoryUsage().heapUsed,
        });
    }

    detect() {
        if (this.snapshots.length < 2) return null;
        const first = this.snapshots[0];
        const last = this.snapshots[this.snapshots.length - 1];
        const growthMB = (last.heapUsed - first.heapUsed) / 1024 / 1024;

        return {
            growthMB: growthMB.toFixed(1),
            isLeak: growthMB > this.thresholdMB,
            period: `${((last.timestamp - first.timestamp) / 1000).toFixed(0)}s`,
        };
    }
}

// Usage
const detector = new LeakDetector();
detector.snapshot('baseline');

// ... run workload ...

detector.snapshot('after-workload');
const result = detector.detect();
if (result?.isLeak) {
    console.error(`LEAK DETECTED: ${result.growthMB}MB growth in ${result.period}`);
}
```

## WeakRef and FinalizationRegistry

```javascript
// WeakRef — weak reference that doesn't prevent GC
const cache = new Map();

function getCached(key) {
    const ref = cache.get(key);
    if (ref) {
        const val = ref.deref();
        if (val) return val;
    }
    const val = expensiveCompute(key);
    cache.set(key, new WeakRef(val));
    return val;
}

// FinalizationRegistry — cleanup callback when GC'd
const registry = new FinalizationRegistry((key) => {
    console.log(`Resource ${key} was garbage collected`);
});

function openResource(path) {
    const resource = { path, handle: openFile(path) };
    registry.register(resource, path);
    return resource;
}
```

## Best Practices Checklist

- [ ] Take heap snapshots before and after operations
- [ ] Monitor heap growth over time
- [ ] Use WeakRef for caches where possible
- [ ] Always clean up event listeners
- [ ] Use FinalizationRegistry for resource cleanup
- [ ] Run with --expose-gc in tests

## Cross-References

- See [Heap and Stack](./01-heap-stack-allocation.md) for memory layout
- See [Garbage Collection](./02-garbage-collection.md) for GC details
- See [Error Handling](../11-error-handling/01-error-propagation.md) for error patterns

## Next Steps

Continue to [Event Emitters Advanced](../04-event-emitters-advanced/01-eventemitter-fundamentals.md) for event patterns.
