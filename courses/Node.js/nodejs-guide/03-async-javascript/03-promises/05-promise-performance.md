# Promise Performance Optimization

## What You'll Learn

- Promise creation overhead
- Microtask queue optimization
- Parallel vs sequential execution
- Memory considerations for promises

## Performance Characteristics

```
Promise Operation Benchmarks:
─────────────────────────────────────────────
new Promise()          ████████  ~0.01ms (creation overhead)
Promise.resolve()      ████      ~0.005ms (faster than constructor)
.then() handler        ████████  ~0.01ms (microtask scheduling)
Promise.all(100)       ████████████████ ~0.05ms (scheduling only)
await (single)         ████████  ~0.01ms (microtask overhead)
1000 sequential awaits ████████████████████ ~10ms (serial)
1000 parallel awaits   ████████  ~15ms (but actual I/O is parallel)

Key insight: Promise overhead is negligible for real I/O operations
The overhead matters only for millions of trivial promises
```

## Optimization Strategies

```javascript
// 1. Use Promise.all for parallel operations
// BAD: Sequential (10 × 100ms = 1000ms)
for (const id of ids) {
    const user = await getUser(id);
}

// GOOD: Parallel (max 100ms)
const users = await Promise.all(ids.map(id => getUser(id)));

// 2. Batch large Promise.all operations
async function batchPromiseAll(items, fn, batchSize = 100) {
    const results = [];
    for (let i = 0; i < items.length; i += batchSize) {
        const batch = items.slice(i, i + batchSize);
        const batchResults = await Promise.all(batch.map(fn));
        results.push(...batchResults);
    }
    return results;
}

// 3. Cache resolved promises
const cache = new Map();
function getCached(key, fetcher) {
    if (cache.has(key)) return cache.get(key);
    const promise = fetcher().catch(err => {
        cache.delete(key); // Remove failed promises
        throw err;
    });
    cache.set(key, promise);
    return promise;
}

// 4. Use Promise.resolve for synchronous values
// BAD: Unnecessary Promise construction
function getValue() {
    return new Promise(resolve => resolve(42));
}

// GOOD: Direct Promise.resolve
function getValue() {
    return Promise.resolve(42);
}
```

## Memory Considerations

```javascript
// Each .then() creates a new Promise object
// Deep chains create many intermediate objects

// For very long chains, consider:
// 1. Using a loop instead of chaining
// 2. Awaiting directly in async functions

// Deep chain (creates N promise objects)
let p = Promise.resolve(0);
for (let i = 0; i < 10000; i++) {
    p = p.then(x => x + 1);
}

// Async function (single promise per call)
async function sum(n) {
    let total = 0;
    for (let i = 0; i < n; i++) total++;
    return total;
}
```

## Best Practices Checklist

- [ ] Use Promise.all for parallel operations
- [ ] Batch large Promise.all sets to avoid OOM
- [ ] Cache promise results where appropriate
- [ ] Avoid unnecessary Promise constructor wrapping
- [ ] Profile microtask queue depth in production

## Cross-References

- See [Promise Combinators](./03-promise-combinators.md) for combinators
- See [Promise Debugging](./04-promise-debugging.md) for debugging
- See [Concurrency Control](../08-concurrency-control/01-rate-limiting.md) for rate limiting
- See [Async Performance](../11-async-performance/01-memory-cpu-patterns.md) for profiling

## Next Steps

Continue to [Async/Await](../04-async-await/04-async-composition.md) for async patterns.
