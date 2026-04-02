# API Testing

## What You'll Learn

- How to test REST APIs comprehensively
- How to test error responses
- How to test pagination
- How to test authentication

## API Test Suite

```ts
// tests/api/users.test.ts

import { describe, it, expect } from 'vitest';
import { app } from '../../src/app.js';

const BASE = 'http://localhost:3000';

describe('Users API', () => {
  describe('GET /api/users', () => {
    it('returns paginated users', async () => {
      const res = await fetch(`${BASE}/api/users?page=1&limit=10`);
      const data = await res.json();

      expect(res.status).toBe(200);
      expect(data.items).toBeInstanceOf(Array);
      expect(data.items.length).toBeLessThanOrEqual(10);
      expect(data.total).toBeDefined();
    });
  });

  describe('POST /api/users', () => {
    it('validates required fields', async () => {
      const res = await fetch(`${BASE}/api/users`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({}),
      });

      expect(res.status).toBe(400);
      const error = await res.json();
      expect(error.details).toBeDefined();
    });

    it('rejects duplicate email', async () => {
      await fetch(`${BASE}/api/users`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: 'Alice', email: 'alice@test.com' }),
      });

      const res = await fetch(`${BASE}/api/users`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: 'Alice 2', email: 'alice@test.com' }),
      });

      expect(res.status).toBe(409);
    });
  });
});
```

## Next Steps

For best practices, continue to [Contract Best Practices](./05-contract-best-practices.md).
