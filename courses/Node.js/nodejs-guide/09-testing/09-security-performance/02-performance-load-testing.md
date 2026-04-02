# Performance and Load Testing

## What You'll Learn

- Response time testing
- Memory usage testing
- Load testing with autocannon
- Performance regression testing

## Response Time Testing

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { performance } from 'node:perf_hooks';
import request from 'supertest';

describe('Response Time Tests', () => {
    let app;

    before(async () => {
        app = await createTestApp();
    });

    test('API responds within 200ms', async () => {
        const start = performance.now();

        await request(app)
            .get('/api/users')
            .set('Authorization', `Bearer ${authToken}`);

        const elapsed = performance.now() - start;
        assert.ok(elapsed < 200, `Response took ${elapsed.toFixed(0)}ms`);
    });

    test('database query responds within 100ms', async () => {
        const start = performance.now();
        await db.query('SELECT * FROM users LIMIT 100');
        const elapsed = performance.now() - start;

        assert.ok(elapsed < 100, `Query took ${elapsed.toFixed(0)}ms`);
    });

    test('JWT verification is under 1ms', () => {
        const token = jwt.sign({ sub: 1 }, 'test-secret');

        const iterations = 10000;
        const start = performance.now();
        for (let i = 0; i < iterations; i++) {
            jwt.verify(token, 'test-secret');
        }
        const perVerify = (performance.now() - start) / iterations;

        assert.ok(perVerify < 1, `Verify took ${perVerify.toFixed(3)}ms`);
    });
});
```

## Load Testing

```javascript
import autocannon from 'autocannon';

describe('Load Tests', () => {
    test('handles 100 concurrent users', async () => {
        const token = jwt.sign({ sub: 1 }, process.env.JWT_SECRET);

        const result = await autocannon({
            url: 'http://localhost:3000/api/users',
            connections: 100,
            duration: 30,
            headers: { Authorization: `Bearer ${token}` },
        });

        assert.ok(result.latency.p95 < 500, 'p95 latency should be <500ms');
        assert.ok(result.errors === 0, 'Should have no errors');
        assert.ok(result.throughput.average > 100, 'Should handle >100 req/s');
    });

    test('login endpoint handles load', async () => {
        const result = await autocannon({
            url: 'http://localhost:3000/auth/login',
            connections: 50,
            duration: 15,
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                email: 'test@example.com',
                password: 'TestPass123!',
            }),
        });

        assert.ok(result.latency.p95 < 1000, 'Login p95 should be <1s');
    });
});
```

## Memory Usage Testing

```javascript
describe('Memory Tests', () => {
    test('memory stays within limits', async () => {
        const before = process.memoryUsage().heapUsed;

        for (let i = 0; i < 10000; i++) {
            await processItem({ id: i, data: 'x'.repeat(100) });
        }

        global.gc?.();
        const after = process.memoryUsage().heapUsed;
        const growthMB = (after - before) / 1024 / 1024;

        assert.ok(growthMB < 50, `Memory grew ${growthMB.toFixed(1)}MB`);
    });

    test('streaming does not leak memory', async () => {
        const before = process.memoryUsage().heapUsed;

        const stream = createReadStream('large-file.txt');
        for await (const chunk of stream) {
            // Process chunk
        }

        global.gc?.();
        const after = process.memoryUsage().heapUsed;
        const growthMB = (after - before) / 1024 / 1024;

        assert.ok(growthMB < 10, `Stream leaked ${growthMB.toFixed(1)}MB`);
    });
});
```

## Performance Regression Detection

```javascript
describe('Performance Regression', () => {
    test('no regression from baseline', async () => {
        const baseline = {
            p50: 50,
            p95: 150,
            p99: 300,
        };

        const result = await autocannon({
            url: 'http://localhost:3000/api/users',
            connections: 50,
            duration: 10,
        });

        const regressionThreshold = 1.2; // 20% tolerance

        assert.ok(
            result.latency.p50 < baseline.p50 * regressionThreshold,
            `p50 regressed: ${result.latency.p50}ms vs baseline ${baseline.p50}ms`
        );
        assert.ok(
            result.latency.p95 < baseline.p95 * regressionThreshold,
            `p95 regressed: ${result.latency.p95}ms vs baseline ${baseline.p95}ms`
        );
    });
});
```

## Common Mistakes

- Not measuring with production-like data
- Not setting performance budgets
- Not testing under concurrent load
- Not detecting memory leaks

## Cross-References

- See [Security Testing](./01-security-testing.md) for security tests
- See [Testing Automation](../10-testing-automation/01-ci-cd-integration.md) for CI integration
- See [Testing Production](../11-testing-production/01-canary-feature-flags.md) for prod testing

## Next Steps

Continue to [Testing Automation: Parallelization](../10-testing-automation/02-parallelization-setup.md).
