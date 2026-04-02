# Garbage Collection Algorithms and Timing

## What You'll Learn

- V8 generational garbage collection
- Scavenge and Mark-Sweep-Compact algorithms
- Incremental and concurrent GC
- GC timing and performance impact

## Generational GC Overview

```
V8 Garbage Collection Strategy:
─────────────────────────────────────────────
Objects start in New Space (Young Generation)
│
├── Survived Scavenge? → Promoted to Old Space
│   └── Old Space: Mark-Sweep-Compact (less frequent)
│
└── Not survived → Freed immediately
    └── New Space: Scavenge (very fast, ~1ms)

Key insight: Most objects die young
→ Young gen GC is fast and frequent
→ Old gen GC is slower but infrequent
```

## Scavenge (Young Generation)

```
New Space Layout:
─────────────────────────────────────────────
┌─────────────────────────────────────────┐
│  From-Space          To-Space           │
│  ┌──────────┐       ┌──────────┐       │
│  │ Object A │       │ (empty)  │       │
│  │ Object B │  ──►  │ Object B │       │
│  │ Object C │       │ (A dies) │       │
│  └──────────┘       └──────────┘       │
│                                         │
│  Copy survivors to To-Space             │
│  Swap From/To spaces                    │
│  Fast: ~1ms for typical heaps           │
└─────────────────────────────────────────┘
```

## Mark-Sweep-Compact (Old Generation)

```
Old Space GC Phases:
─────────────────────────────────────────────
1. MARK (incremental — interleaved with JS)
   ├── Start from roots (global, stack)
   ├── Trace all reachable objects
   └── Mark each reachable object

2. SWEEP (concurrent — background thread)
   ├── Scan for unmarked objects
   ├── Add to free list
   └── Reclaim memory

3. COMPACT (when fragmentation is high)
   ├── Move live objects together
   ├── Eliminate fragmentation
   └── Update all pointers
```

## GC Timing

```javascript
// Monitor GC events
const { PerformanceObserver } = require('node:perf_hooks');

const obs = new PerformanceObserver((items) => {
    for (const entry of items.getEntries()) {
        const kind = {
            1: 'Scavenge (young gen)',
            2: 'Mark-Sweep-Compact (old gen)',
            4: 'Incremental marking',
            8: 'Process weak callbacks',
        }[entry.kind] || `Unknown (${entry.kind})`;

        console.log(`GC: ${kind} — ${entry.duration.toFixed(2)}ms`);
    }
});

obs.observe({ entryTypes: ['gc'], buffered: true });

// Trigger GC for testing (requires --expose-gc flag)
if (global.gc) {
    const before = process.memoryUsage().heapUsed;
    global.gc();
    const after = process.memoryUsage().heapUsed;
    console.log(`GC freed: ${((before - after) / 1024 / 1024).toFixed(1)} MB`);
}
```

### GC Flags

```bash
# GC-related V8 flags
node --trace-gc app.js              # Log GC events
node --trace-gc-verbose app.js      # Detailed GC logging
node --expose-gc app.js             # Enable global.gc()
node --max-old-space-size=4096 app.js # 4GB heap limit
node --max-semi-space-size=32 app.js  # 32MB young gen
```

## Memory Pressure Response

```javascript
// Monitor memory pressure
function checkMemoryPressure() {
    const usage = process.memoryUsage();
    const heapUsedPercent = (usage.heapUsed / usage.heapTotal) * 100;

    if (heapUsedPercent > 90) {
        console.error('CRITICAL: Heap > 90% used');
        if (global.gc) global.gc();
    } else if (heapUsedPercent > 80) {
        console.warn('WARNING: Heap > 80% used');
    }

    return heapUsedPercent;
}

setInterval(() => {
    const percent = checkMemoryPressure();
    console.log(`Heap usage: ${percent.toFixed(1)}%`);
}, 10000);
```

## Best Practices Checklist

- [ ] Monitor GC frequency and duration in production
- [ ] Set appropriate heap size limits
- [ ] Avoid creating objects in hot loops
- [ ] Use object pooling for frequently created objects
- [ ] Avoid circular references that prevent GC
- [ ] Clean up event listeners and timers

## Cross-References

- See [Heap and Stack](./01-heap-stack-allocation.md) for memory layout
- See [Memory Profiling](./03-memory-profiling.md) for leak detection
- See [Buffers](../06-buffers-deep-dive/01-buffer-creation-encoding.md) for buffer memory

## Next Steps

Continue to [Memory Profiling](./03-memory-profiling.md) for leak detection techniques.
