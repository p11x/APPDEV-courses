# Integration Testing

## What You'll Learn

- How to write integration tests
- How to test with real databases
- How to test API endpoints
- How to manage test data

## API Integration Test

```ts
// tests/api/users.test.ts

import { describe, it, beforeAll, afterAll, beforeEach } from 'vitest';
import { createApp } from '../../src/app.js';
import { setupTestDb, teardownTestDb } from '../helpers/db.js';

describe('Users API', () => {
  let app, server, port;

  beforeAll(async () => {
    const db = await setupTestDb();
    app = createApp(db);
    server = app.listen(0);
    port = server.address().port;
  });

  afterAll(async () => {
    server.close();
    await teardownTestDb();
  });

  it('GET /users returns list', async () => {
    const res = await fetch(`http://localhost:${port}/api/users`);
    expect(res.status).toBe(200);
    expect(await res.json()).toBeInstanceOf(Array);
  });

  it('POST /users creates user', async () => {
    const res = await fetch(`http://localhost:${port}/api/users`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name: 'Bob', email: 'bob@example.com' }),
    });

    expect(res.status).toBe(201);
    const user = await res.json();
    expect(user.name).toBe('Bob');
  });
});
```

## Next Steps

For E2E testing, continue to [E2E Testing](./04-e2e-testing.md).
