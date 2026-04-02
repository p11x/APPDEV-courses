# Integration Testing Async Flows

## What You'll Learn

- Testing HTTP endpoints
- Testing async middleware
- Database integration testing
- End-to-end async testing

## HTTP Endpoint Testing

```javascript
import { describe, it, before, after } from 'node:test';
import assert from 'node:assert/strict';
import { createServer } from 'node:http';
import app from '../src/server.js';

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
        const user = await res.json();
        assert.equal(user.name, 'Test');
    });

    it('POST /api/users validates input', async () => {
        const res = await fetch(`${baseURL}/api/users`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: '' }),
        });
        assert.equal(res.status, 400);
    });
});
```

## Testing Async Flows

```javascript
describe('User registration flow', () => {
    it('should complete full registration', async () => {
        // 1. Register
        const registerRes = await fetch(`${baseURL}/api/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                name: 'Alice',
                email: 'alice@example.com',
                password: 'SecurePass123!',
            }),
        });
        assert.equal(registerRes.status, 201);
        const { token } = await registerRes.json();

        // 2. Verify token works
        const profileRes = await fetch(`${baseURL}/api/profile`, {
            headers: { Authorization: `Bearer ${token}` },
        });
        assert.equal(profileRes.status, 200);
        const profile = await profileRes.json();
        assert.equal(profile.email, 'alice@example.com');

        // 3. Verify email is sent (mock check)
        assert.ok(emailMock.sent.some(e => e.to === 'alice@example.com'));
    });
});
```

## Best Practices Checklist

- [ ] Start/stop server in before/after hooks
- [ ] Test complete async flows end-to-end
- [ ] Clean up test data after tests
- [ ] Use real databases in integration tests
- [ ] Test error responses and edge cases

## Cross-References

- See [Unit Testing](./01-unit-testing.md) for unit tests
- See [Mocking](./03-mocking-strategies.md) for mocking strategies
- See [Error Handling](../07-async-error-handling/01-error-propagation.md) for errors

## Next Steps

Continue to [Mocking Strategies](./03-mocking-strategies.md) for advanced mocking.
