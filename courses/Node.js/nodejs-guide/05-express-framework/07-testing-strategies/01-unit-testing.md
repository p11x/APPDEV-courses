# Express.js Testing Strategies

## What You'll Learn

- Unit testing routes and middleware
- Integration testing Express apps
- Mock request/response objects
- Testing async operations

## Unit Testing Routes

```javascript
import { describe, it, before, after } from 'node:test';
import assert from 'node:assert/strict';
import { createServer } from 'node:http';
import app from '../src/app.js';

describe('User API', () => {
    let server;
    let baseURL;

    before(async () => {
        server = createServer(app);
        await new Promise(resolve => server.listen(0, resolve));
        const { port } = server.address();
        baseURL = `http://localhost:${port}`;
    });

    after(() => server.close());

    it('GET /api/users returns list', async () => {
        const res = await fetch(`${baseURL}/api/users`);
        assert.equal(res.status, 200);
        const body = await res.json();
        assert.ok(Array.isArray(body.data));
    });

    it('POST /api/users creates user', async () => {
        const res = await fetch(`${baseURL}/api/users`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: 'Test', email: 'test@example.com' }),
        });
        assert.equal(res.status, 201);
    });

    it('validates input', async () => {
        const res = await fetch(`${baseURL}/api/users`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: '' }),
        });
        assert.equal(res.status, 400);
    });
});
```

## Testing Middleware

```javascript
describe('Rate Limit Middleware', () => {
    it('blocks after limit exceeded', async () => {
        const requests = Array.from({ length: 101 }, () =>
            fetch(`${baseURL}/api/data`)
        );

        const results = await Promise.all(requests);
        const blocked = results.filter(r => r.status === 429);
        assert.ok(blocked.length > 0);
    });
});
```

## Best Practices Checklist

- [ ] Test all routes with expected responses
- [ ] Test error handling paths
- [ ] Test authentication and authorization
- [ ] Use real HTTP requests for integration tests
- [ ] Clean up test data after tests

## Cross-References

- See [Error Handling](../08-error-handling/01-centralized-errors.md) for error patterns
- See [Security](../05-security-implementation/01-helmet-cors.md) for security testing
- See [Architecture](../01-express-architecture/01-lifecycle-deep-dive.md) for request flow

## Next Steps

Continue to [Error Handling](../08-error-handling/01-centralized-errors.md) for error patterns.
