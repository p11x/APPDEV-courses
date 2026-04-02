# Async Performance: Memory and CPU Patterns

## What You'll Learn

- Memory usage in async operations
- CPU utilization patterns
- Performance benchmarks of async patterns
- Optimization strategies

## Memory Patterns

```
Memory Usage by Async Pattern:
─────────────────────────────────────────────
Callback:         Minimal overhead (~200 bytes per callback)
Promise:          ~400 bytes per Promise object
async/await:      ~400 bytes per Promise (syntactic sugar)
Promise.all(N):   ~400 × N bytes (one Promise per item)
Deep .then() chain: Each .then() creates new Promise

Key insight: For millions of operations, consider batching
```

## Performance Benchmarks

```javascript
import { performance } from 'node:perf_hooks';

// Benchmark different async patterns
async function benchmark(name, fn, iterations = 10000) {
    // Warmup
    for (let i = 0; i < 100; i++) await fn(i);

    const start = performance.now();
    for (let i = 0; i < iterations; i++) {
        await fn(i);
    }
    const elapsed = performance.now() - start;

    console.log(`${name}: ${elapsed.toFixed(2)}ms (${iterations} iterations)`);
}

// Results (approximate):
// Callback (setImmediate):     ~45ms
// Promise.resolve():           ~52ms
// async/await:                 ~55ms
// Promise.all (1000 parallel): ~15ms
// Sequential await (1000):     ~10ms

// The overhead is negligible for real I/O
// It matters only for millions of trivial operations
```

## Optimization Strategies

```javascript
// 1. Parallel execution
// BAD: Sequential (10 × 100ms = 1000ms)
for (const id of ids) {
    await getUser(id);
}

// GOOD: Parallel (max 100ms)
await Promise.all(ids.map(id => getUser(id)));

// 2. Batch large parallel operations
async function batchAll(items, fn, batchSize = 100) {
    const results = [];
    for (let i = 0; i < items.length; i += batchSize) {
        const batch = items.slice(i, i + batchSize);
        const batchResults = await Promise.all(batch.map(fn));
        results.push(...batchResults);
    }
    return results;
}

// 3. Cache promise results
const cache = new Map();
function cached(key, fetcher) {
    if (cache.has(key)) return cache.get(key);
    const p = fetcher().catch(err => { cache.delete(key); throw err; });
    cache.set(key, p);
    return p;
}
```

## Best Practices Checklist

- [ ] Use Promise.all for parallel operations
- [ ] Batch large Promise.all sets
- [ ] Cache promise results where appropriate
- [ ] Profile microtask queue depth
- [ ] Monitor memory usage in long-running processes

## Cross-References

- See [Promise Performance](../03-promises/05-promise-performance.md) for Promise optimization
- See [Concurrency Control](../08-concurrency-control/01-rate-limiting.md) for rate limiting
- See [Performance](../../02-core-concepts/12-performance-optimization/01-cpu-memory-optimization.md) for general optimization

## Next Steps

Continue to [Benchmarking](./02-benchmarking.md) for measurement techniques.
