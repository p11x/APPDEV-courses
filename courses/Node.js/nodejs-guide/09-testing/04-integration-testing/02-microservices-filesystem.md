# Integration Testing: Microservices, File System, and External Services

## What You'll Learn

- Microservice integration testing
- File system integration testing
- External API mocking
- Message queue integration testing
- Cache integration testing

## Microservice Integration Testing

```javascript
import { test, describe, before, after } from 'node:test';
import assert from 'node:assert/strict';
import express from 'express';
import request from 'supertest';

describe('Microservice Integration', () => {
    let orderService;
    let inventoryService;
    let mockInventory;

    before(async () => {
        // Mock inventory service
        mockInventory = express();
        mockInventory.use(express.json());

        mockInventory.get('/inventory/:productId', (req, res) => {
            const stock = {
                '1': { available: 10 },
                '2': { available: 0 },
            };
            const item = stock[req.params.productId];
            item ? res.json(item) : res.status(404).json({ error: 'Not found' });
        });

        mockInventory.post('/inventory/reserve', (req, res) => {
            res.json({ reserved: true, reservationId: 'res-123' });
        });

        const inventoryServer = mockInventory.listen(3001);
        process.env.INVENTORY_URL = 'http://localhost:3001';

        orderService = await createOrderService();
    });

    after(async () => {
        // Cleanup
    });

    test('creates order when inventory available', async () => {
        const res = await request(orderService)
            .post('/orders')
            .set('Authorization', `Bearer ${authToken}`)
            .send({
                items: [{ productId: '1', quantity: 2 }],
            });

        assert.equal(res.status, 201);
        assert.ok(res.body.id);
        assert.equal(res.body.status, 'confirmed');
    });

    test('rejects order when inventory unavailable', async () => {
        const res = await request(orderService)
            .post('/orders')
            .set('Authorization', `Bearer ${authToken}`)
            .send({
                items: [{ productId: '2', quantity: 1 }],
            });

        assert.equal(res.status, 400);
        assert.ok(res.body.error.includes('inventory'));
    });
});
```

## File System Integration Testing

```javascript
import { test, describe, before, after, beforeEach } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtemp, writeFile, readFile, rm } from 'node:fs/promises';
import { join } from 'node:path';
import { tmpdir } from 'node:os';

describe('File System Integration', () => {
    let tempDir;

    before(async () => {
        tempDir = await mkdtemp(join(tmpdir(), 'test-'));
    });

    after(async () => {
        await rm(tempDir, { recursive: true, force: true });
    });

    test('writes and reads file', async () => {
        const filePath = join(tempDir, 'test.txt');
        await writeFile(filePath, 'Hello, World!');

        const content = await readFile(filePath, 'utf-8');
        assert.equal(content, 'Hello, World!');
    });

    test('handles file not found', async () => {
        await assert.rejects(
            () => readFile(join(tempDir, 'nonexistent.txt')),
            { code: 'ENOENT' }
        );
    });

    test('processes CSV file', async () => {
        const csvPath = join(tempDir, 'data.csv');
        await writeFile(csvPath, 'name,age\nAlice,30\nBob,25\n');

        const records = await parseCSV(csvPath);
        assert.equal(records.length, 2);
        assert.equal(records[0].name, 'Alice');
    });
});
```

## External API Mocking

```javascript
import { test, describe, before, after } from 'node:test';
import assert from 'node:assert/strict';
import { createServer } from 'node:http';

describe('External API Integration', () => {
    let mockServer;
    let mockUrl;

    before(async () => {
        // Create mock server
        mockServer = createServer((req, res) => {
            if (req.url === '/api/users' && req.method === 'GET') {
                res.writeHead(200, { 'Content-Type': 'application/json' });
                res.end(JSON.stringify([{ id: 1, name: 'Mock User' }]));
            } else if (req.url === '/api/users' && req.method === 'POST') {
                let body = '';
                req.on('data', (chunk) => { body += chunk; });
                req.on('end', () => {
                    const user = JSON.parse(body);
                    res.writeHead(201, { 'Content-Type': 'application/json' });
                    res.end(JSON.stringify({ id: 2, ...user }));
                });
            } else {
                res.writeHead(404);
                res.end();
            }
        });

        await new Promise((resolve) => mockServer.listen(0, resolve));
        const port = mockServer.address().port;
        mockUrl = `http://localhost:${port}`;
        process.env.EXTERNAL_API_URL = mockUrl;
    });

    after(async () => {
        await new Promise((resolve) => mockServer.close(resolve));
    });

    test('fetches users from external API', async () => {
        const users = await fetchUsers();
        assert.equal(users.length, 1);
        assert.equal(users[0].name, 'Mock User');
    });

    test('creates user via external API', async () => {
        const user = await createUser({ name: 'New User' });
        assert.equal(user.id, 2);
        assert.equal(user.name, 'New User');
    });
});
```

## Message Queue Integration Testing

```javascript
describe('Message Queue Integration', () => {
    let mockQueue;

    before(async () => {
        mockQueue = {
            messages: [],
            async publish(queue, message) {
                this.messages.push({ queue, message, timestamp: Date.now() });
            },
            async consume(queue, handler) {
                const msgs = this.messages.filter(m => m.queue === queue);
                for (const msg of msgs) {
                    await handler(msg.message);
                }
            },
        };
    });

    test('publishes order created event', async () => {
        const order = { id: 1, userId: 1, total: 100 };
        await mockQueue.publish('orders.created', order);

        assert.equal(mockQueue.messages.length, 1);
        assert.equal(mockQueue.messages[0].queue, 'orders.created');
    });

    test('processes order event', async () => {
        let processed = false;
        await mockQueue.consume('orders.created', async (msg) => {
            processed = true;
            assert.equal(msg.id, 1);
        });
        assert.ok(processed);
    });
});
```

## Common Mistakes

- Not cleaning up temp files/directories
- Not mocking external services (slow, flaky tests)
- Not testing error responses from external APIs
- Not isolating test data between tests

## Cross-References

- See [Integration Testing](./01-rest-api.md) for API testing
- See [Database Testing](../07-database-testing/01-unit-testing.md) for DB testing
- See [API Testing](../06-api-testing/01-rest-graphql.md) for API patterns

## Next Steps

Continue to [E2E: Cypress](../05-end-to-end-testing/02-cypress-user-journeys.md).
