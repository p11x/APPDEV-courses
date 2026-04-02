# WebSocket and Contract Testing (Pact)

## What You'll Learn

- WebSocket API testing
- gRPC API testing patterns
- API contract testing with Pact
- API documentation testing

## WebSocket Testing

```javascript
import { test, describe, before, after } from 'node:test';
import assert from 'node:assert/strict';
import { WebSocket } from 'ws';
import { createServer } from 'node:http';

describe('WebSocket API', () => {
    let server;
    let wsServer;
    let wsUrl;

    before(async () => {
        const httpServer = createServer();
        const { WebSocketServer } = await import('ws');
        wsServer = new WebSocketServer({ server: httpServer });

        wsServer.on('connection', (ws) => {
            ws.on('message', (data) => {
                const msg = JSON.parse(data.toString());
                if (msg.type === 'echo') {
                    ws.send(JSON.stringify({ type: 'echo', data: msg.data }));
                } else if (msg.type === 'broadcast') {
                    wsServer.clients.forEach(client => {
                        client.send(JSON.stringify({ type: 'broadcast', data: msg.data }));
                    });
                }
            });
        });

        await new Promise(r => httpServer.listen(0, r));
        server = httpServer;
        wsUrl = `ws://localhost:${server.address().port}`;
    });

    after(async () => {
        wsServer.close();
        server.close();
    });

    test('connects and receives echo', async () => {
        const ws = new WebSocket(wsUrl);

        const message = new Promise((resolve) => {
            ws.on('message', (data) => resolve(JSON.parse(data.toString())));
        });

        await new Promise(r => ws.on('open', r));
        ws.send(JSON.stringify({ type: 'echo', data: 'hello' }));

        const response = await message;
        assert.equal(response.type, 'echo');
        assert.equal(response.data, 'hello');

        ws.close();
    });

    test('broadcasts to all clients', async () => {
        const ws1 = new WebSocket(wsUrl);
        const ws2 = new WebSocket(wsUrl);

        await Promise.all([
            new Promise(r => ws1.on('open', r)),
            new Promise(r => ws2.on('open', r)),
        ]);

        const received = new Promise((resolve) => {
            ws2.on('message', (data) => resolve(JSON.parse(data.toString())));
        });

        ws1.send(JSON.stringify({ type: 'broadcast', data: 'hello all' }));

        const msg = await received;
        assert.equal(msg.data, 'hello all');

        ws1.close();
        ws2.close();
    });

    test('handles connection close', async () => {
        const ws = new WebSocket(wsUrl);
        await new Promise(r => ws.on('open', r));

        const closed = new Promise((resolve) => {
            ws.on('close', resolve);
        });

        ws.close();
        await closed;
    });
});
```

## Contract Testing with Pact

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

    test('POST /users creates user', async () => {
        await provider.addInteraction({
            state: 'no user with email exists',
            uponReceiving: 'a request to create a user',
            withRequest: {
                method: 'POST',
                path: '/users',
                headers: { 'Content-Type': 'application/json' },
                body: {
                    name: 'Bob',
                    email: 'bob@example.com',
                },
            },
            willRespondWith: {
                status: 201,
                headers: { 'Content-Type': 'application/json' },
                body: {
                    id: 2,
                    name: 'Bob',
                    email: 'bob@example.com',
                },
            },
        });

        const res = await fetch(`${provider.mockService.baseUrl}/users`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: 'Bob', email: 'bob@example.com' }),
        });

        assert.equal(res.status, 201);
    });
});
```

## API Documentation Testing

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import SwaggerParser from '@apidevtools/swagger-parser';

describe('API Documentation', () => {
    test('OpenAPI spec is valid', async () => {
        const api = await SwaggerParser.validate('./docs/openapi.yaml');
        assert.ok(api.info.title);
        assert.ok(api.paths);
    });

    test('all endpoints are documented', async () => {
        const api = await SwaggerParser.parse('./docs/openapi.yaml');
        const documentedPaths = Object.keys(api.paths);

        // Check that all implemented routes are documented
        const implementedRoutes = getImplementedRoutes();
        for (const route of implementedRoutes) {
            assert.ok(
                documentedPaths.some(p => route.startsWith(p.replace(/{[^}]+}/g, ':'))),
                `Route ${route} is not documented`
            );
        }
    });
});
```

## Common Mistakes

- Not testing WebSocket error handling
- Not verifying contract compatibility between services
- Not keeping API docs in sync with implementation
- Not testing WebSocket reconnection logic

## Cross-References

- See [REST/GraphQL](./01-rest-graphql.md) for HTTP testing
- See [Integration Testing](../04-integration-testing/02-microservices-filesystem.md) for services
- See [Testing Automation](../10-testing-automation/01-ci-cd-integration.md) for CI/CD

## Next Steps

Continue to [Database Testing: Migrations and Transactions](../07-database-testing/02-migrations-transactions.md).
