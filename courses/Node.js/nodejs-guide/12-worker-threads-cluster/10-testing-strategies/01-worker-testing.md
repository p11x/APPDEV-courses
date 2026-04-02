# Worker Thread Testing Strategies

## What You'll Learn

- Unit testing worker threads
- Integration testing worker pools
- Performance testing worker configurations
- CI/CD integration for worker tests
- Testing error scenarios

## Unit Testing Workers

```js
// test/worker-pool.test.js
import { test, describe, before, after } from 'node:test';
import assert from 'node:assert/strict';
import { WorkerPool } from '../lib/worker-pool.js';

describe('WorkerPool', () => {
    let pool;

    before(async () => {
        pool = new WorkerPool('./workers/compute.js', { size: 2 });
        await pool.start();
    });

    after(async () => {
        await pool.terminate();
    });

    test('executes fibonacci task', async () => {
        const result = await pool.execute({
            type: 'fibonacci',
            data: 30,
        });
        assert.equal(result, 832040);
    });

    test('handles multiple concurrent tasks', async () => {
        const tasks = Array.from({ length: 10 }, (_, i) =>
            pool.execute({ type: 'fibonacci', data: 20 + i })
        );

        const results = await Promise.all(tasks);
        assert.equal(results.length, 10);
        results.forEach(r => assert.ok(typeof r === 'number'));
    });

    test('handles task errors gracefully', async () => {
        await assert.rejects(
            () => pool.execute({ type: 'invalid-type', data: null }),
            { message: /Unknown task type/ }
        );
    });

    test('tracks pool statistics', async () => {
        await pool.execute({ type: 'fibonacci', data: 10 });
        const stats = pool.getStats();

        assert.equal(stats.poolSize, 2);
        assert.ok(stats.completed >= 1);
    });

    test('handles worker crash and replacement', async () => {
        // Send a task that causes a crash
        await assert.rejects(
            () => pool.execute({ type: 'crash', data: null }),
        );

        // Pool should still work after crash
        const result = await pool.execute({ type: 'fibonacci', data: 5 });
        assert.equal(result, 5);
    });

    test('respects pool size limit', async () => {
        const stats = pool.getStats();
        assert.ok(stats.activeWorkers <= stats.poolSize);
    });
});
```

## Performance Testing Workers

```js
// test/worker-performance.test.js
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { performance } from 'node:perf_hooks';
import { WorkerPool } from '../lib/worker-pool.js';

describe('Worker Performance', () => {
    test('parallel execution is faster than sequential', async () => {
        const tasks = Array.from({ length: 8 }, () => ({
            type: 'fibonacci',
            data: 35,
        }));

        // Sequential
        const seqStart = performance.now();
        for (const task of tasks) {
            const pool = new WorkerPool('./workers/compute.js', { size: 1 });
            await pool.start();
            await pool.execute(task);
            await pool.terminate();
        }
        const seqTime = performance.now() - seqStart;

        // Parallel
        const parPool = new WorkerPool('./workers/compute.js', { size: 4 });
        await parPool.start();
        const parStart = performance.now();
        await Promise.all(tasks.map(t => parPool.execute(t)));
        const parTime = performance.now() - parStart;
        await parPool.terminate();

        console.log(`Sequential: ${seqTime.toFixed(0)}ms, Parallel: ${parTime.toFixed(0)}ms`);
        console.log(`Speedup: ${(seqTime / parTime).toFixed(1)}x`);

        assert.ok(parTime < seqTime, 'Parallel should be faster');
    });

    test('pool size affects throughput', async () => {
        const results = [];

        for (const size of [1, 2, 4]) {
            const pool = new WorkerPool('./workers/compute.js', { size });
            await pool.start();

            const start = performance.now();
            await Promise.all(
                Array.from({ length: 20 }, () =>
                    pool.execute({ type: 'fibonacci', data: 30 })
                )
            );
            const elapsed = performance.now() - start;

            results.push({ size, elapsed: Math.round(elapsed) });
            await pool.terminate();
        }

        console.table(results);

        // Larger pool should be faster (up to a point)
        assert.ok(results[2].elapsed < results[0].elapsed);
    });
});
```

## CI/CD Integration

```yaml
# .github/workflows/worker-tests.yml
name: Worker Thread Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        node-version: [18, 20, 22]

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: ${{ matrix.node-version }}

      - run: npm ci

      - name: Unit Tests
        run: npm test -- --test-name-pattern="Worker"

      - name: Performance Tests
        run: npm run test:performance
        env:
          CI: true

      - name: Integration Tests
        run: npm run test:integration
```

## Common Mistakes

- Not terminating workers in tests (resource leak)
- Not testing error scenarios (crashes, timeouts)
- Not testing with different pool sizes
- Not running performance tests in CI

## Try It Yourself

### Exercise 1: Test Error Handling
Write tests for worker timeout, crash recovery, and invalid task types.

### Exercise 2: Benchmark Pool Sizes
Test pool sizes 1, 2, 4, 8 and find the optimal configuration.

### Exercise 3: CI Integration
Add worker tests to your CI pipeline with multiple Node.js versions.

## Next Steps

Continue to [Deployment and Operations](../11-deployment-operations/01-production-deployment.md).
