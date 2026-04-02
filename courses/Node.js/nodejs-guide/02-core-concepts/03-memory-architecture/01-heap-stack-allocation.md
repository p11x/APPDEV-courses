# V8 Heap and Stack Memory Allocation

## What You'll Learn

- V8 memory heap structure
- Stack frames and function call patterns
- Memory allocation strategies
- Performance implications of allocation patterns

## V8 Memory Layout

```
V8 Memory Architecture:
─────────────────────────────────────────────
┌─────────────────────────────────────────┐
│              V8 Process                 │
│  ┌─────────────────────────────────┐   │
│  │          Stack                  │   │
│  │  ┌─────────────────────────┐   │   │
│  │  │ Function call frames    │   │   │
│  │  │ Local variables         │   │   │
│  │  │ Return addresses        │   │   │
│  │  │ Primitive values        │   │   │
│  │  └─────────────────────────┘   │   │
│  │  Limit: ~1MB (per thread)      │   │
│  └─────────────────────────────────┘   │
│                                         │
│  ┌─────────────────────────────────┐   │
│  │          Heap                   │   │
│  │  ┌─────────────────────────┐   │   │
│  │  │ New Space (Young Gen)   │   │   │
│  │  │ ~16MB, fast GC          │   │   │
│  │  └─────────────────────────┘   │   │
│  │  ┌─────────────────────────┐   │   │
│  │  │ Old Space (Old Gen)     │   │   │
│  │  │ ~1.4GB default          │   │   │
│  │  └─────────────────────────┘   │   │
│  │  ┌─────────────────────────┐   │   │
│  │  │ Large Object Space      │   │   │
│  │  │ Objects > 1MB           │   │   │
│  │  └─────────────────────────┘   │   │
│  │  ┌─────────────────────────┐   │   │
│  │  │ Code Space (JIT)        │   │   │
│  │  └─────────────────────────┘   │   │
│  └─────────────────────────────────┘   │
│                                         │
│  ┌─────────────────────────────────┐   │
│  │       External Memory           │   │
│  │  Buffers, C++ objects           │   │
│  └─────────────────────────────────┘   │
└─────────────────────────────────────────┘
```

## Stack vs Heap Allocation

### Stack Allocation (Primitives)

```javascript
// Stack: Small, fixed-size, fast access
// Stored directly in function call frame

function calculate() {
    const x = 42;           // Stack — SMI (Small Integer)
    const y = 3.14;         // Heap — floating point
    const name = 'Alice';   // Heap — string
    const flag = true;      // Stack — boolean
    const empty = null;     // Stack — null

    return x + y; // Stack arithmetic
}
// Stack frame is destroyed when function returns
// Primitive values are freed automatically
```

### Heap Allocation (Objects)

```javascript
// Heap: Dynamic, variable-size, GC-managed

function createUser(name, age) {
    // Object allocated on heap
    const user = {
        name,           // String on heap
        age,            // Number on heap (or SMI on stack)
        created: Date.now(),
        tags: ['user'], // Array on heap
    };

    return user; // Reference returned, object stays on heap
}

const alice = createUser('Alice', 30);
// 'user' object lives on heap
// 'alice' variable on stack holds reference to heap object
```

## Memory Allocation Patterns

### SMI (Small Integer) Optimization

```javascript
// V8 uses "SMIs" for 31-bit signed integers
// SMI: stored unboxed on stack — very fast

const smi = 42;           // SMI — unboxed, fast
const big = 2 ** 31;      // Heap number — boxed, slower
const float = 42.5;       // Heap number — boxed, slower

// Performance difference in loops
function sumSmi(arr) {
    let total = 0; // SMI
    for (let i = 0; i < arr.length; i++) {
        total += arr[i]; // Fast SMI arithmetic
    }
    return total;
}

function sumFloat(arr) {
    let total = 0.0; // Heap number
    for (let i = 0; i < arr.length; i++) {
        total += arr[i]; // Slower boxed arithmetic
    }
    return total;
}
```

### Object Allocation

```javascript
// Inline object allocation (V8 optimized)
function createPoint(x, y) {
    return { x, y }; // V8 allocates inline when shape is consistent
}

// Always same shape — V8 creates single hidden class
const p1 = createPoint(1, 2);
const p2 = createPoint(3, 4);
const p3 = createPoint(5, 6);
// All share same hidden class — optimized access

// BAD: Different shapes — multiple hidden classes
function createBad(x, y) {
    if (x > 0) return { x, y, label: 'positive' };
    return { y, x }; // Different property order
}
```

### Array Allocation

```javascript
// V8 has multiple array representations

// Packed SMI array (fastest)
const packedSmi = [1, 2, 3, 4, 5];

// Packed double array
const packedDouble = [1.1, 2.2, 3.3];

// Holey array (slower — has gaps)
const holey = [];
holey[0] = 1;
holey[100] = 2; // Creates holes 1-99

// Dictionary array (slowest — sparse)
const sparse = [];
sparse[1000000] = 'value';
```

## Measuring Memory Usage

```javascript
// process.memoryUsage() — detailed breakdown
const usage = process.memoryUsage();

console.log({
    // Resident Set Size — total memory allocated
    rss: `${(usage.rss / 1024 / 1024).toFixed(1)} MB`,

    // V8 heap total — total heap size
    heapTotal: `${(usage.heapTotal / 1024 / 1024).toFixed(1)} MB`,

    // V8 heap used — actual used heap
    heapUsed: `${(usage.heapUsed / 1024 / 1024).toFixed(1)} MB`,

    // External — C++ objects bound to JS
    external: `${(usage.external / 1024 / 1024).toFixed(1)} MB`,

    // ArrayBuffers
    arrayBuffers: `${(usage.arrayBuffers / 1024 / 1024).toFixed(1)} MB`,
});

// V8 heap statistics
const v8 = require('node:v8');
const heapStats = v8.getHeapStatistics();
console.log({
    heapSizeLimit: `${(heapStats.heap_size_limit / 1024 / 1024).toFixed(0)} MB`,
    totalHeapSize: `${(heapStats.total_heap_size / 1024 / 1024).toFixed(1)} MB`,
    usedHeapSize: `${(heapStats.used_heap_size / 1024 / 1024).toFixed(1)} MB`,
});
```

## Best Practices Checklist

- [ ] Keep objects small and consistently shaped
- [ ] Use SMI (integer) values when possible
- [ ] Avoid sparse/holey arrays
- [ ] Reuse objects instead of creating new ones in hot loops
- [ ] Use typed arrays for numeric data
- [ ] Monitor heap usage in production

## Cross-References

- See [Garbage Collection](./02-garbage-collection.md) for GC details
- See [Memory Profiling](./03-memory-profiling.md) for leak detection
- See [Buffers Deep Dive](../06-buffers-deep-dive/01-buffer-creation-encoding.md) for buffer memory

## Next Steps

Continue to [Garbage Collection](./02-garbage-collection.md) for GC internals.
