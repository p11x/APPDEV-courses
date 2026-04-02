# REST API and Database Integration Testing

## What You'll Learn

- REST API integration testing with Supertest
- Database integration testing patterns
- Test database setup and teardown
- Transaction-based test isolation
- Service integration testing

## REST API Integration Testing

```javascript
import { test, describe, before, after } from 'node:test';
import assert from 'node:assert/strict';
import request from 'supertest';
import { createApp } from '../../src/app.js';

describe('Users API', () => {
    let app;
    let authToken;
    let testUserId;

    before(async () => {
        app = await createApp({ testing: true });

        // Get auth token
        const res = await request(app)
            .post('/auth/login')
            .send({ email: 'test@example.com', password: 'TestPass123!' });

        authToken = res.body.accessToken;
    });

    after(async () => {
        await app.cleanup();
    });

    describe('POST /api/users', () => {
        test('creates user with valid data', async () => {
            const res = await request(app)
                .post('/api/users')
                .set('Authorization', `Bearer ${authToken}`)
                .send({
                    name: 'New User',
                    email: 'new@example.com',
                    password: 'SecurePass123!',
                });

            assert.equal(res.status, 201);
            assert.ok(res.body.id);
            assert.equal(res.body.name, 'New User');
            assert.ok(!res.body.password); // No password in response

            testUserId = res.body.id;
        });

        test('rejects invalid email', async () => {
            const res = await request(app)
                .post('/api/users')
                .set('Authorization', `Bearer ${authToken}`)
                .send({
                    name: 'Bad User',
                    email: 'invalid-email',
                    password: 'SecurePass123!',
                });

            assert.equal(res.status, 400);
            assert.ok(res.body.error);
        });

        test('rejects duplicate email', async () => {
            const res = await request(app)
                .post('/api/users')
                .set('Authorization', `Bearer ${authToken}`)
                .send({
                    name: 'Duplicate',
                    email: 'new@example.com',
                    password: 'SecurePass123!',
                });

            assert.equal(res.status, 409);
        });

        test('rejects without auth token', async () => {
            const res = await request(app)
                .post('/api/users')
                .send({ name: 'No Auth', email: 'no@auth.com' });

            assert.equal(res.status, 401);
        });
    });

    describe('GET /api/users/:id', () => {
        test('returns user by ID', async () => {
            const res = await request(app)
                .get(`/api/users/${testUserId}`)
                .set('Authorization', `Bearer ${authToken}`);

            assert.equal(res.status, 200);
            assert.equal(res.body.id, testUserId);
            assert.equal(res.body.name, 'New User');
        });

        test('returns 404 for non-existent user', async () => {
            const res = await request(app)
                .get('/api/users/99999')
                .set('Authorization', `Bearer ${authToken}`);

            assert.equal(res.status, 404);
        });
    });

    describe('GET /api/users', () => {
        test('returns paginated list', async () => {
            const res = await request(app)
                .get('/api/users?page=1&limit=10')
                .set('Authorization', `Bearer ${authToken}`);

            assert.equal(res.status, 200);
            assert.ok(Array.isArray(res.body.data));
            assert.ok(res.body.pagination);
        });

        test('filters by role', async () => {
            const res = await request(app)
                .get('/api/users?role=admin')
                .set('Authorization', `Bearer ${authToken}`);

            assert.equal(res.status, 200);
            res.body.data.forEach(user => {
                assert.equal(user.role, 'admin');
            });
        });
    });
});
```

## Database Integration Testing

```javascript
import { test, describe, before, after, beforeEach } from 'node:test';
import assert from 'node:assert/strict';
import { Pool } from 'pg';

describe('User Repository', () => {
    let pool;
    let repo;

    before(async () => {
        pool = new Pool({
            host: process.env.TEST_DB_HOST || 'localhost',
            port: 5432,
            database: 'test_db',
            user: 'test_user',
            password: 'test_pass',
        });

        // Run migrations
        await pool.query(`
            CREATE TABLE IF NOT EXISTS users (
                id SERIAL PRIMARY KEY,
                name VARCHAR(255) NOT NULL,
                email VARCHAR(255) UNIQUE NOT NULL,
                role VARCHAR(50) DEFAULT 'user',
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )
        `);

        repo = new UserRepository(pool);
    });

    beforeEach(async () => {
        // Clean slate for each test
        await pool.query('DELETE FROM users');
    });

    after(async () => {
        await pool.query('DROP TABLE IF EXISTS users');
        await pool.end();
    });

    test('creates user', async () => {
        const user = await repo.create({
            name: 'Alice',
            email: 'alice@test.com',
        });

        assert.ok(user.id);
        assert.equal(user.name, 'Alice');
        assert.equal(user.email, 'alice@test.com');
    });

    test('finds user by ID', async () => {
        const created = await repo.create({ name: 'Bob', email: 'bob@test.com' });
        const found = await repo.findById(created.id);

        assert.equal(found.id, created.id);
        assert.equal(found.name, 'Bob');
    });

    test('returns null for non-existent ID', async () => {
        const found = await repo.findById(99999);
        assert.equal(found, null);
    });

    test('finds user by email', async () => {
        await repo.create({ name: 'Carol', email: 'carol@test.com' });
        const found = await repo.findByEmail('carol@test.com');

        assert.equal(found.name, 'Carol');
    });

    test('updates user', async () => {
        const user = await repo.create({ name: 'Dave', email: 'dave@test.com' });
        const updated = await repo.update(user.id, { name: 'David' });

        assert.equal(updated.name, 'David');
    });

    test('deletes user', async () => {
        const user = await repo.create({ name: 'Eve', email: 'eve@test.com' });
        await repo.delete(user.id);

        const found = await repo.findById(user.id);
        assert.equal(found, null);
    });

    test('lists users with pagination', async () => {
        for (let i = 0; i < 15; i++) {
            await repo.create({ name: `User ${i}`, email: `user${i}@test.com` });
        }

        const page1 = await repo.list({ page: 1, limit: 10 });
        assert.equal(page1.data.length, 10);
        assert.equal(page1.total, 15);

        const page2 = await repo.list({ page: 2, limit: 10 });
        assert.equal(page2.data.length, 5);
    });
});
```

## Transaction-Based Test Isolation

```javascript
describe('Transaction Isolation', () => {
    let pool;
    let client;

    before(async () => {
        pool = new Pool({ /* config */ });
    });

    beforeEach(async () => {
        // Start transaction before each test
        client = await pool.connect();
        await client.query('BEGIN');
    });

    afterEach(async () => {
        // Rollback after each test (automatic cleanup)
        await client.query('ROLLBACK');
        client.release();
    });

    after(async () => {
        await pool.end();
    });

    test('creates and queries within transaction', async () => {
        await client.query(
            'INSERT INTO users (name, email) VALUES ($1, $2)',
            ['Test', 'test@test.com']
        );

        const { rows } = await client.query('SELECT * FROM users');
        assert.equal(rows.length, 1);
    });

    test('previous test data is rolled back', async () => {
        const { rows } = await client.query('SELECT * FROM users');
        assert.equal(rows.length, 0); // Clean because of rollback
    });
});
```

## Best Practices Checklist

- [ ] Use separate test database
- [ ] Clean data between tests
- [ ] Use transactions for isolation when possible
- [ ] Test all HTTP status codes
- [ ] Test authentication/authorization
- [ ] Test pagination and filtering
- [ ] Test error responses
- [ ] Run integration tests separately from unit tests

## Cross-References

- See [Unit Testing](../03-unit-testing/01-functions-classes.md) for unit patterns
- See [Database Testing](../07-database-testing/01-unit-testing.md) for DB patterns
- See [API Testing](../06-api-testing/01-rest-graphql.md) for API patterns

## Next Steps

Continue to [Database Testing](../07-database-testing/01-unit-testing.md).
