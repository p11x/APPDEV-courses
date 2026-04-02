# Benchmarking Async Code

## What You'll Learn

- Creating reliable async benchmarks
- Measuring async operation latency
- Comparing async patterns
- Production performance monitoring

## Async Benchmark Framework

```javascript
import { performance } from 'node:perf_hooks';

async function bench(name, fn, { iterations = 1000, warmup = 100 } = {}) {
    // Warmup
    for (let i = 0; i < warmup; i++) await fn(i);

    // Collect timings
    const timings = [];
    for (let i = 0; i < iterations; i++) {
        const start = performance.now();
        await fn(i);
        timings.push(performance.now() - start);
    }

    // Calculate statistics
    timings.sort((a, b) => a - b);
    const stats = {
        name,
        iterations,
        mean: +(timings.reduce((a, b) => a + b) / timings.length).toFixed(3),
        median: +timings[Math.floor(timings.length / 2)].toFixed(3),
        p95: +timings[Math.floor(timings.length * 0.95)].toFixed(3),
        p99: +timings[Math.floor(timings.length * 0.99)].toFixed(3),
        min: +timings[0].toFixed(3),
        max: +timings[timings.length - 1].toFixed(3),
    };

    console.log(`${stats.name}: mean=${stats.mean}ms p95=${stats.p95}ms p99=${stats.p99}ms`);
    return stats;
}

// Usage
await bench('Promise.resolve', () => Promise.resolve(42));
await bench('setImmediate', () => new Promise(r => setImmediate(r)));
await bench('setTimeout(0)', () => new Promise(r => setTimeout(r, 0)));
```

## Measuring Async Latency

```javascript
function measureAsync(fn) {
    return async (...args) => {
        const start = performance.now();
        try {
            const result = await fn(...args);
            return {
                result,
                duration: performance.now() - start,
                success: true,
            };
        } catch (error) {
            return {
                error,
                duration: performance.now() - start,
                success: false,
            };
        }
    };
}

// Usage
const measured = measureAsync(() => fetch('/api/users'));
const { result, duration, success } = await measured();
console.log(`Request took ${duration.toFixed(2)}ms`);
```

## Best Practices Checklist

- [ ] Always warm up before benchmarking
- [ ] Run benchmarks multiple times and take median
- [ ] Benchmark in production-like environment
- [ ] Monitor p95 and p99 latencies
- [ ] Set up performance regression alerts

## Cross-References

- See [Memory/CPU Patterns](./01-memory-cpu-patterns.md) for performance characteristics
- See [Production Monitoring](./03-production-monitoring.md) for observability
- See [Promise Performance](../03-promises/05-promise-performance.md) for Promise optimization

## Next Steps

Continue to [Production Monitoring](./03-production-monitoring.md) for observability.
