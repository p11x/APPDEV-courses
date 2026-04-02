# CPU and Memory Optimization Techniques

## What You'll Learn

- CPU profiling and hotspot identification
- Memory optimization strategies
- Object allocation reduction
- V8 optimization patterns

## CPU Optimization

```javascript
// Profile hot functions
node --prof app.js
node --prof-process isolate-*.log

// Chrome DevTools
node --inspect app.js
// chrome://inspect → CPU Profile
```

### Optimization Patterns

```javascript
// 1. Avoid repeated computation
const memoize = (fn) => {
    const cache = new Map();
    return (...args) => {
        const key = JSON.stringify(args);
        if (cache.has(key)) return cache.get(key);
        const result = fn(...args);
        cache.set(key, result);
        return result;
    };
};

const expensiveCompute = memoize((n) => {
    let result = 0;
    for (let i = 0; i < n * 1000000; i++) result += Math.sqrt(i);
    return result;
});

// 2. Use typed arrays for numeric data
const numbers = new Float64Array(1000000); // Faster than regular arrays

// 3. Avoid object creation in hot loops
// BAD: Creates 1M objects
for (let i = 0; i < 1000000; i++) {
    results.push({ id: i, value: compute(i) });
}

// GOOD: Reuse object
const item = { id: 0, value: 0 };
for (let i = 0; i < 1000000; i++) {
    item.id = i;
    item.value = compute(i);
    results.push({ ...item });
}
```

## Memory Optimization

```javascript
// 1. Object pooling
class Pool {
    constructor(create, reset, size = 100) {
        this.items = Array.from({ length: size }, create);
        this.reset = reset;
    }

    acquire() {
        return this.items.pop() || this.items.constructor();
    }

    release(item) {
        this.reset(item);
        this.items.push(item);
    }
}

// 2. WeakRef for caches
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

// 3. Use streams for large data
// BAD: Load entire file
const data = fs.readFileSync('huge.csv');

// GOOD: Stream processing
fs.createReadStream('huge.csv')
    .pipe(csvParser())
    .pipe(processRow());
```

## Best Practices Checklist

- [ ] Profile before optimizing
- [ ] Avoid object allocation in hot loops
- [ ] Use typed arrays for numeric data
- [ ] Implement object pooling for frequent allocations
- [ ] Use WeakRef for caches
- [ ] Use streams for large data processing

## Cross-References

- See [I/O Optimization](./02-io-database-optimization.md) for I/O patterns
- See [Monitoring](./03-monitoring-metrics.md) for production metrics
- See [Memory Architecture](../03-memory-architecture/01-heap-stack-allocation.md) for memory

## Next Steps

Continue to [I/O Optimization](./02-io-database-optimization.md) for I/O patterns.
