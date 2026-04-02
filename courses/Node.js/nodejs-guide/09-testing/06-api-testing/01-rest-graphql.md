# REST and GraphQL API Testing

## What You'll Learn

- REST API testing with Supertest
- GraphQL API testing
- WebSocket API testing
- API contract testing
- API performance testing

## REST API Testing (Comprehensive)

```javascript
import { test, describe, before, after } from 'node:test';
import assert from 'node:assert/strict';
import request from 'supertest';

describe('Products API', () => {
    let app;
    let adminToken;
    let userToken;

    before(async () => {
        app = await createTestApp();
        adminToken = await getAuthToken('admin@test.com', 'AdminPass123!');
        userToken = await getAuthToken('user@test.com', 'UserPass123!');
    });

    describe('CRUD Operations', () => {
        let productId;

        test('POST /api/products - creates product (admin)', async () => {
            const res = await request(app)
                .post('/api/products')
                .set('Authorization', `Bearer ${adminToken}`)
                .send({
                    name: 'Test Product',
                    price: 29.99,
                    category: 'electronics',
                });

            assert.equal(res.status, 201);
            assert.ok(res.body.id);
            productId = res.body.id;
        });

        test('POST /api/products - rejects non-admin', async () => {
            const res = await request(app)
                .post('/api/products')
                .set('Authorization', `Bearer ${userToken}`)
                .send({ name: 'Unauthorized', price: 10 });

            assert.equal(res.status, 403);
        });

        test('GET /api/products - lists with pagination', async () => {
            const res = await request(app)
                .get('/api/products?page=1&limit=5')
                .set('Authorization', `Bearer ${userToken}`);

            assert.equal(res.status, 200);
            assert.ok(res.body.data.length <= 5);
            assert.ok(res.body.total >= 0);
        });

        test('GET /api/products/:id - returns product', async () => {
            const res = await request(app)
                .get(`/api/products/${productId}`)
                .set('Authorization', `Bearer ${userToken}`);

            assert.equal(res.status, 200);
            assert.equal(res.body.name, 'Test Product');
        });

        test('PUT /api/products/:id - updates product', async () => {
            const res = await request(app)
                .put(`/api/products/${productId}`)
                .set('Authorization', `Bearer ${adminToken}`)
                .send({ price: 39.99 });

            assert.equal(res.status, 200);
            assert.equal(res.body.price, 39.99);
        });

        test('DELETE /api/products/:id - deletes product', async () => {
            const res = await request(app)
                .delete(`/api/products/${productId}`)
                .set('Authorization', `Bearer ${adminToken}`);

            assert.equal(res.status, 204);
        });
    });

    describe('Validation', () => {
        test('rejects missing required fields', async () => {
            const res = await request(app)
                .post('/api/products')
                .set('Authorization', `Bearer ${adminToken}`)
                .send({});

            assert.equal(res.status, 400);
            assert.ok(res.body.errors);
            assert.ok(res.body.errors.some(e => e.field === 'name'));
            assert.ok(res.body.errors.some(e => e.field === 'price'));
        });

        test('rejects invalid price', async () => {
            const res = await request(app)
                .post('/api/products')
                .set('Authorization', `Bearer ${adminToken}`)
                .send({ name: 'Test', price: -10 });

            assert.equal(res.status, 400);
        });
    });
});
```

## GraphQL API Testing

```javascript
import { test, describe, before } from 'node:test';
import assert from 'node:assert/strict';
import request from 'supertest';

describe('GraphQL API', () => {
    let app;

    before(async () => {
        app = await createGraphQLApp();
    });

    test('queries users', async () => {
        const res = await request(app)
            .post('/graphql')
            .send({
                query: `
                    query {
                        users {
                            id
                            name
                            email
                        }
                    }
                `,
            });

        assert.equal(res.status, 200);
        assert.ok(res.body.data.users);
        assert.ok(Array.isArray(res.body.data.users));
    });

    test('queries single user by ID', async () => {
        const res = await request(app)
            .post('/graphql')
            .send({
                query: `
                    query GetUser($id: ID!) {
                        user(id: $id) {
                            id
                            name
                            posts {
                                title
                            }
                        }
                    }
                `,
                variables: { id: '1' },
            });

        assert.equal(res.status, 200);
        assert.ok(res.body.data.user);
    });

    test('creates user with mutation', async () => {
        const res = await request(app)
            .post('/graphql')
            .set('Authorization', `Bearer ${adminToken}`)
            .send({
                query: `
                    mutation CreateUser($input: CreateUserInput!) {
                        createUser(input: $input) {
                            id
                            name
                            email
                        }
                    }
                `,
                variables: {
                    input: {
                        name: 'GraphQL User',
                        email: `graphql-${Date.now()}@test.com`,
                    },
                },
            });

        assert.equal(res.status, 200);
        assert.ok(res.body.data.createUser.id);
    });

    test('handles GraphQL errors', async () => {
        const res = await request(app)
            .post('/graphql')
            .send({
                query: `
                    query {
                        user(id: "nonexistent") {
                            id
                        }
                    }
                `,
            });

        assert.equal(res.status, 200);
        assert.ok(res.body.errors);
        assert.equal(res.body.errors[0].message, 'User not found');
    });

    test('enforces authentication', async () => {
        const res = await request(app)
            .post('/graphql')
            .send({
                query: `mutation { deleteUser(id: "1") }`,
            });

        assert.equal(res.status, 200);
        assert.ok(res.body.errors);
        assert.ok(res.body.errors[0].message.includes('authenticated'));
    });
});
```

## WebSocket API Testing

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { WebSocket } from 'ws';

describe('WebSocket API', () => {
    const WS_URL = 'ws://localhost:3000/ws';

    test('connects and receives messages', async () => {
        const ws = new WebSocket(WS_URL);

        const messagePromise = new Promise((resolve) => {
            ws.on('message', (data) => {
                resolve(JSON.parse(data.toString()));
            });
        });

        ws.on('open', () => {
            ws.send(JSON.stringify({ type: 'subscribe', channel: 'updates' }));
        });

        const message = await messagePromise;
        assert.equal(message.type, 'subscribed');
        ws.close();
    });

    test('handles authentication', async () => {
        const ws = new WebSocket(WS_URL, {
            headers: { Authorization: 'Bearer invalid-token' },
        });

        const closePromise = new Promise((resolve) => {
            ws.on('close', (code) => resolve(code));
        });

        const code = await closePromise;
        assert.equal(code, 4001); // Unauthorized
    });

    test('broadcasts to all clients', async () => {
        const ws1 = new WebSocket(WS_URL);
        const ws2 = new WebSocket(WS_URL);

        await Promise.all([
            new Promise(r => ws1.on('open', r)),
            new Promise(r => ws2.on('open', r)),
        ]);

        const received = new Promise((resolve) => {
            ws2.on('message', (data) => resolve(JSON.parse(data.toString())));
        });

        ws1.send(JSON.stringify({ type: 'broadcast', message: 'Hello all' }));

        const msg = await received;
        assert.equal(msg.message, 'Hello all');

        ws1.close();
        ws2.close();
    });
});
```

## API Contract Testing (Pact)

```javascript
import { Pact } from '@pact-foundation/pact';
import path from 'node:path';

const provider = new Pact({
    consumer: 'FrontendApp',
    provider: 'UserAPI',
    port: 1234,
    log: path.resolve(process.cwd(), 'logs', 'pact.log'),
    dir: path.resolve(process.cwd(), 'pacts'),
});

describe('User API Contract', () => {
    before(() => provider.setup());
    after(() => provider.finalize());

    test('GET /users/:id returns user', async () => {
        await provider.addInteraction({
            state: 'user with ID 1 exists',
            uponReceiving: 'a request for user 1',
            withRequest: {
                method: 'GET',
                path: '/users/1',
                headers: { Authorization: 'Bearer token' },
            },
            willRespondWith: {
                status: 200,
                headers: { 'Content-Type': 'application/json' },
                body: {
                    id: 1,
                    name: 'Alice',
                    email: 'alice@example.com',
                },
            },
        });

        const res = await fetch(`${provider.mockService.baseUrl}/users/1`, {
            headers: { Authorization: 'Bearer token' },
        });

        const user = await res.json();
        assert.equal(user.name, 'Alice');
    });
});
```

## Best Practices Checklist

- [ ] Test all HTTP methods and status codes
- [ ] Test authentication and authorization
- [ ] Test input validation
- [ ] Test pagination and filtering
- [ ] Test GraphQL queries and mutations
- [ ] Test WebSocket connections and messages
- [ ] Use contract testing for service boundaries
- [ ] Test API response schemas

## Cross-References

- See [Integration Testing](../04-integration-testing/01-rest-api.md) for integration patterns
- See [Database Testing](../07-database-testing/01-unit-testing.md) for DB testing
- See [Performance Testing](../09-security-performance/01-security-testing.md) for load testing

## Next Steps

Continue to [Database Testing](../07-database-testing/01-unit-testing.md).
