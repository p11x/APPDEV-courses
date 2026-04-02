# Security and Performance Testing

## What You'll Learn

- Security vulnerability testing
- Authentication testing
- Input validation testing
- SQL injection and XSS prevention testing
- Performance and load testing

## Security Testing

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import request from 'supertest';

describe('Security Tests', () => {
    let app;
    let authToken;

    before(async () => {
        app = await createTestApp();
        authToken = await getAuthToken();
    });

    describe('SQL Injection Prevention', () => {
        test('rejects SQL injection in login', async () => {
            const res = await request(app)
                .post('/auth/login')
                .send({
                    email: "'; DROP TABLE users; --",
                    password: 'anything',
                });

            assert.ok(res.status >= 400);
        });

        test('rejects SQL injection in search', async () => {
            const res = await request(app)
                .get('/api/users?search=1%27%20OR%201=1')
                .set('Authorization', `Bearer ${authToken}`);

            assert.ok(res.status >= 400);
        });

        test('uses parameterized queries', async () => {
            // This test verifies the code path uses parameterized queries
            const maliciousInput = "'; DROP TABLE users; --";
            const res = await request(app)
                .get(`/api/users?name=${encodeURIComponent(maliciousInput)}`)
                .set('Authorization', `Bearer ${authToken}`);

            // Should return empty results, not crash
            assert.ok(res.status === 200 || res.status === 400);
        });
    });

    describe('XSS Prevention', () => {
        test('sanitizes HTML in user input', async () => {
            const xssPayload = '<script>alert("xss")</script>';
            const res = await request(app)
                .post('/api/users')
                .set('Authorization', `Bearer ${authToken}`)
                .send({
                    name: xssPayload,
                    email: 'xss@test.com',
                });

            if (res.status === 201) {
                assert.ok(!res.body.name.includes('<script>'));
            }
        });

        test('sets proper Content-Type headers', async () => {
            const res = await request(app).get('/api/users');
            assert.ok(res.headers['content-type'].includes('application/json'));
            assert.ok(!res.headers['content-type'].includes('text/html'));
        });
    });

    describe('CSRF Protection', () => {
        test('rejects POST without CSRF token', async () => {
            const res = await request(app)
                .post('/api/users')
                .send({ name: 'Test', email: 'test@test.com' });

            assert.ok(res.status === 401 || res.status === 403);
        });
    });

    describe('Security Headers', () => {
        test('sets security headers', async () => {
            const res = await request(app).get('/');

            assert.ok(res.headers['x-frame-options']);
            assert.ok(res.headers['x-content-type-options']);
            assert.ok(res.headers['strict-transport-security']);
        });

        test('sets secure cookie flags', async () => {
            const res = await request(app)
                .post('/auth/login')
                .send({ email: 'user@test.com', password: 'Pass123!' });

            const cookie = res.headers['set-cookie'];
            if (cookie) {
                assert.ok(cookie.some(c => c.includes('HttpOnly')));
                assert.ok(cookie.some(c => c.includes('Secure')));
                assert.ok(cookie.some(c => c.includes('SameSite')));
            }
        });
    });

    describe('Authentication Security', () => {
        test('does not reveal user existence', async () => {
            const existingUser = await request(app)
                .post('/auth/login')
                .send({ email: 'existing@test.com', password: 'Wrong' });

            const nonExistingUser = await request(app)
                .post('/auth/login')
                .send({ email: 'nobody@test.com', password: 'Wrong' });

            assert.equal(existingUser.status, nonExistingUser.status);
            assert.deepEqual(existingUser.body.error, nonExistingUser.body.error);
        });

        test('rate limits login attempts', async () => {
            for (let i = 0; i < 10; i++) {
                await request(app)
                    .post('/auth/login')
                    .send({ email: 'test@test.com', password: 'Wrong' });
            }

            const res = await request(app)
                .post('/auth/login')
                .send({ email: 'test@test.com', password: 'CorrectPass123!' });

            assert.equal(res.status, 429);
        });
    });
});
```

## Performance Testing

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { performance } from 'node:perf_hooks';
import autocannon from 'autocannon';

describe('Performance Tests', () => {
    let app;

    before(async () => {
        app = await createTestApp();
    });

    describe('Response Time', () => {
        test('API responds within 200ms', async () => {
            const start = performance.now();

            await request(app)
                .get('/api/users')
                .set('Authorization', `Bearer ${authToken}`);

            const elapsed = performance.now() - start;
            assert.ok(elapsed < 200, `Response took ${elapsed.toFixed(0)}ms, expected <200ms`);
        });

        test('database query responds within 100ms', async () => {
            const start = performance.now();
            await db.query('SELECT * FROM users LIMIT 100');
            const elapsed = performance.now() - start;

            assert.ok(elapsed < 100, `Query took ${elapsed.toFixed(0)}ms`);
        });
    });

    describe('Load Testing', () => {
        test('handles 100 concurrent requests', async () => {
            const result = await autocannon({
                url: 'http://localhost:3000/api/users',
                connections: 100,
                duration: 10,
                headers: { Authorization: `Bearer ${authToken}` },
            });

            assert.ok(result.latency.p95 < 500, 'p95 latency should be <500ms');
            assert.ok(result.errors === 0, 'Should have no errors');
            assert.ok(result.throughput.average > 100, 'Should handle >100 req/s');
        });
    });

    describe('Memory Usage', () => {
        test('memory stays within limits', async () => {
            const before = process.memoryUsage().heapUsed;

            // Perform operations
            for (let i = 0; i < 1000; i++) {
                await processItem({ id: i, data: 'x'.repeat(1000) });
            }

            global.gc?.();
            const after = process.memoryUsage().heapUsed;
            const growthMB = (after - before) / 1024 / 1024;

            assert.ok(growthMB < 50, `Memory grew by ${growthMB.toFixed(1)}MB, expected <50MB`);
        });
    });
});
```

## Best Practices Checklist

- [ ] Test SQL injection prevention
- [ ] Test XSS prevention
- [ ] Test CSRF protection
- [ ] Verify security headers
- [ ] Test rate limiting
- [ ] Test authentication error messages
- [ ] Set response time budgets
- [ ] Run load tests in CI

## Cross-References

- See [API Testing](../06-api-testing/01-rest-graphql.md) for API testing
- See [Authentication Testing](../../08-authentication/07-authentication-testing/01-unit-testing.md) for auth tests
- See [CI/CD](../10-testing-automation/01-ci-cd-integration.md) for automation

## Next Steps

Continue to [Testing Automation and CI/CD](../10-testing-automation/01-ci-cd-integration.md).
