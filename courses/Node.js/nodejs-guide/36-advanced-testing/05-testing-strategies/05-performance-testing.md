# Performance Testing

## What You'll Learn

- How to load test Node.js APIs
- How to use autocannon for HTTP benchmarking
- How to set performance budgets
- How to detect performance regressions

## Load Testing

```bash
npm install -g autocannon
```

```ts
// tests/perf/load.test.ts

import { describe, it } from 'vitest';
import autocannon from 'autocannon';

describe('Performance', () => {
  it('handles 1000 req/s at p99 < 100ms', async () => {
    const result = await autocannon({
      url: 'http://localhost:3000/api/users',
      connections: 100,
      duration: 10,
    });

    expect(result.requests.average).toBeGreaterThan(1000);
    expect(result.latency.p99).toBeLessThan(100);
  });
});
```

## Stress Testing

```bash
# High concurrency
autocannon -c 500 -d 30 http://localhost:3000/api/users

# Keep-alive connections
autocannon -c 100 -d 10 -k http://localhost:3000/api/users
```

## Next Steps

For chaos testing, continue to [Chaos Testing](./06-chaos-testing.md).
