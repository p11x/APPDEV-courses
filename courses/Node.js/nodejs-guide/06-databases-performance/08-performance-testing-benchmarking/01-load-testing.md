# Performance Testing and Load Testing

## What You'll Learn

- Load testing with autocannon
- Database performance benchmarking
- Stress testing methodologies
- Performance regression testing

## HTTP Load Testing

```bash
npm install -g autocannon
```

```bash
# Basic load test
autocannon -c 100 -d 30 http://localhost:3000/api/users

# With pipelining
autocannon -c 100 -d 30 -p 10 http://localhost:3000/api/users

# JSON output
autocannon -c 100 -d 30 -j http://localhost:3000/api/users > results.json
```

## Database Benchmark

```javascript
import { performance } from 'node:perf_hooks';

async function benchmarkQuery(pool, query, params, iterations = 1000) {
    // Warmup
    for (let i = 0; i < 10; i++) await pool.query(query, params);

    const timings = [];
    for (let i = 0; i < iterations; i++) {
        const start = performance.now();
        await pool.query(query, params);
        timings.push(performance.now() - start);
    }

    timings.sort((a, b) => a - b);
    return {
        iterations,
        mean: +(timings.reduce((a, b) => a + b) / timings.length).toFixed(3),
        median: +timings[Math.floor(timings.length / 2)].toFixed(3),
        p95: +timings[Math.floor(timings.length * 0.95)].toFixed(3),
        p99: +timings[Math.floor(timings.length * 0.99)].toFixed(3),
    };
}

// Usage
const results = await benchmarkQuery(pool, 'SELECT * FROM users LIMIT 10', []);
console.log(results);
// { iterations: 1000, mean: 2.5, median: 2.1, p95: 4.2, p99: 8.1 }
```

## Best Practices Checklist

- [ ] Load test before production deployment
- [ ] Benchmark database queries
- [ ] Set performance baselines
- [ ] Automate performance testing in CI/CD
- [ ] Monitor for performance regressions

## Cross-References

- See [Performance Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for metrics
- See [Database Performance](../02-database-performance-optimization/01-query-optimization.md) for queries
- See [Scalability](../05-scalability-patterns/01-load-balancing.md) for scaling

## Next Steps

Continue to [Modern Database Technologies](../09-modern-database-technologies/01-nosql-patterns.md).
