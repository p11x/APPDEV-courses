# Advanced Testing Strategies for NodeMark

## What You'll Build In This File

End-to-end testing with real database, performance testing, security testing, and CI/CD test automation.

## E2E Testing with Real Database

```javascript
// tests/e2e/setup.js — E2E test setup with real database
import { before, after, beforeEach } from 'node:test';
import { Pool } from 'pg';
import { createApp } from '../../src/index.js';

export let app;
export let pool;
export let testUser;
export let authToken;

before(async () => {
    // Create test database connection
    pool = new Pool({
        connectionString: process.env.TEST_DATABASE_URL,
    });

    // Run migrations
    await runMigrations(pool);

    // Create test app
    app = await createApp({ testing: true });
});

after(async () => {
    await pool.query('DROP TABLE IF EXISTS bookmark_tags, bookmarks, tags, users CASCADE');
    await pool.end();
});

beforeEach(async () => {
    // Clean data between tests
    await pool.query('DELETE FROM bookmark_tags');
    await pool.query('DELETE FROM bookmarks');
    await pool.query('DELETE FROM tags');
    await pool.query('DELETE FROM users');

    // Create test user
    const bcrypt = await import('bcrypt');
    const hash = await bcrypt.hash('TestPass123!', 4);
    const { rows } = await pool.query(
        'INSERT INTO users (name, email, password_hash) VALUES ($1, $2, $3) RETURNING *',
        ['Test User', 'test@example.com', hash]
    );
    testUser = rows[0];

    // Get auth token
    const jwt = await import('jsonwebtoken');
    authToken = jwt.sign(
        { sub: testUser.id, email: testUser.email },
        process.env.JWT_SECRET || 'test-secret',
        { expiresIn: '1h' }
    );
});
```

```javascript
// tests/e2e/bookmarks.e2e.test.js — Full E2E bookmark tests
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import request from 'supertest';
import { app, pool, authToken, testUser } from './setup.js';

describe('Bookmark E2E Flow', () => {
    test('complete bookmark lifecycle', async () => {
        // 1. Create bookmark with tags
        const createRes = await request(app)
            .post('/bookmarks')
            .set('Authorization', `Bearer ${authToken}`)
            .send({
                title: 'Node.js Docs',
                url: 'https://nodejs.org/docs',
                description: 'Official Node.js documentation',
                tags: ['javascript', 'nodejs'],
            });

        assert.equal(createRes.status, 201);
        assert.ok(createRes.body.id);
        assert.equal(createRes.body.tags.length, 2);
        const bookmarkId = createRes.body.id;

        // 2. List bookmarks
        const listRes = await request(app)
            .get('/bookmarks')
            .set('Authorization', `Bearer ${authToken}`);

        assert.equal(listRes.status, 200);
        assert.equal(listRes.body.length, 1);

        // 3. Get single bookmark
        const getRes = await request(app)
            .get(`/bookmarks/${bookmarkId}`)
            .set('Authorization', `Bearer ${authToken}`);

        assert.equal(getRes.status, 200);
        assert.equal(getRes.body.title, 'Node.js Docs');

        // 4. Update bookmark
        const updateRes = await request(app)
            .put(`/bookmarks/${bookmarkId}`)
            .set('Authorization', `Bearer ${authToken}`)
            .send({ title: 'Updated Title' });

        assert.equal(updateRes.status, 200);
        assert.equal(updateRes.body.title, 'Updated Title');

        // 5. Delete bookmark
        const deleteRes = await request(app)
            .delete(`/bookmarks/${bookmarkId}`)
            .set('Authorization', `Bearer ${authToken}`);

        assert.equal(deleteRes.status, 204);

        // 6. Verify deletion
        const getDeleted = await request(app)
            .get(`/bookmarks/${bookmarkId}`)
            .set('Authorization', `Bearer ${authToken}`);

        assert.equal(getDeleted.status, 404);
    });

    test('prevents accessing other users bookmarks', async () => {
        // Create bookmark for testUser
        const createRes = await request(app)
            .post('/bookmarks')
            .set('Authorization', `Bearer ${authToken}`)
            .send({ title: 'Private', url: 'https://private.com' });

        // Create another user
        const bcrypt = await import('bcrypt');
        const hash = await bcrypt.hash('OtherPass123!', 4);
        const { rows: [otherUser] } = await pool.query(
            'INSERT INTO users (name, email, password_hash) VALUES ($1, $2, $3) RETURNING *',
            ['Other User', 'other@example.com', hash]
        );

        const jwt = await import('jsonwebtoken');
        const otherToken = jwt.sign(
            { sub: otherUser.id },
            process.env.JWT_SECRET || 'test-secret'
        );

        // Try to access testUser's bookmark
        const res = await request(app)
            .get(`/bookmarks/${createRes.body.id}`)
            .set('Authorization', `Bearer ${otherToken}`);

        assert.equal(res.status, 404); // Should not find it
    });
});
```

## Performance Testing

```javascript
// tests/performance/load-test.js — Load testing with autocannon
import autocannon from 'autocannon';
import jwt from 'jsonwebtoken';

const token = jwt.sign({ sub: 1 }, process.env.JWT_SECRET || 'test-secret');

const result = await autocannon({
    url: 'http://localhost:3000/bookmarks',
    connections: 50,
    duration: 30,
    headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json',
    },
    requests: [
        {
            method: 'GET',
            path: '/bookmarks',
        },
    ],
});

console.log('Results:');
console.log(`  Requests/sec: ${result.requests.average}`);
console.log(`  Latency p50: ${result.latency.p50}ms`);
console.log(`  Latency p95: ${result.latency.p95}ms`);
console.log(`  Latency p99: ${result.latency.p99}ms`);
console.log(`  Errors: ${result.errors}`);

// Performance budget
assert.ok(result.latency.p95 < 500, 'p95 latency should be under 500ms');
assert.ok(result.errors === 0, 'Should have no errors');
```

## Security Testing

```javascript
// tests/security/security.test.js — Security vulnerability tests
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import request from 'supertest';

describe('Security Tests', () => {
    test('SQL injection prevention', async () => {
        const res = await request(app)
            .post('/auth/login')
            .send({
                email: "'; DROP TABLE users; --",
                password: 'anything',
            });

        assert.ok(res.status >= 400);
    });

    test('XSS prevention in bookmark titles', async () => {
        const res = await request(app)
            .post('/bookmarks')
            .set('Authorization', `Bearer ${authToken}`)
            .send({
                title: '<script>alert("xss")</script>',
                url: 'https://example.com',
            });

        if (res.status === 201) {
            assert.ok(!res.body.title.includes('<script>'));
        }
    });

    test('security headers present', async () => {
        const res = await request(app).get('/health');

        assert.ok(res.headers['x-frame-options']);
        assert.ok(res.headers['x-content-type-options']);
    });

    test('does not reveal user existence', async () => {
        const existingUser = await request(app)
            .post('/auth/login')
            .send({ email: 'test@example.com', password: 'Wrong' });

        const nonExistingUser = await request(app)
            .post('/auth/login')
            .send({ email: 'nobody@example.com', password: 'Wrong' });

        assert.equal(existingUser.status, nonExistingUser.status);
    });
});
```

## CI/CD Test Automation

```yaml
# .github/workflows/test.yml
name: Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    services:
      postgres:
        image: postgres:15
        env:
          POSTGRES_DB: test_db
          POSTGRES_USER: test
          POSTGRES_PASSWORD: test
        ports: ['5432:5432']
        options: --health-cmd pg_isready

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '20' }
      - run: npm ci

      - name: Unit + Integration Tests
        run: npm test -- --coverage
        env:
          TEST_DATABASE_URL: postgresql://test:test@localhost:5432/test_db

      - name: Upload Coverage
        uses: codecov/codecov-action@v3

  e2e:
    needs: test
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '20' }
      - run: npm ci
      - run: npm run test:e2e

  security:
    needs: test
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: npm audit --audit-level=high
      - run: npm run test:security
```

## How It Connects

- E2E testing follows [09-testing/05-end-to-end-testing/](../../../09-testing/05-end-to-end-testing/)
- Security testing follows [09-testing/09-security-performance/](../../../09-testing/09-security-performance/)
- CI/CD follows [26-cicd-github-actions](../../../26-cicd-github-actions/)

## Common Mistakes

- Not cleaning up test data between tests
- Testing against production database
- Not running security tests in CI
- Ignoring performance regression

## Try It Yourself

### Exercise 1: Write E2E Tests
Write E2E tests for the tag filtering feature.

### Exercise 2: Set Performance Budget
Set a performance budget and verify it in tests.

### Exercise 3: Add Security Scan
Add Trivy container scanning to the CI pipeline.

## Next Steps

Continue to [17-advanced-security/01-owasp-hardening.md](../17-advanced-security/01-owasp-hardening.md).
