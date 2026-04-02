# Unit Testing

## What You'll Learn

- How to write effective unit tests
- How to test pure functions
- How to test async code
- How to organize test files

## Test Structure

```ts
// tests/services/userService.test.ts

import { describe, it, expect, beforeEach, mock } from 'vitest';
import { createUser, getUser, deleteUser } from '../../src/services/userService.js';

// Mock dependencies
const db = {
  query: mock(),
};

describe('UserService', () => {
  beforeEach(() => {
    db.query.mockReset();
  });

  describe('createUser', () => {
    it('creates a user with valid data', async () => {
      db.query.mockResolvedValue({ rows: [{ id: 1, name: 'Alice' }] });

      const user = await createUser({ name: 'Alice', email: 'alice@example.com' });

      expect(user.name).toBe('Alice');
      expect(db.query).toHaveBeenCalledWith(
        expect.stringContaining('INSERT INTO users'),
        expect.arrayContaining(['Alice', 'alice@example.com'])
      );
    });

    it('throws for invalid email', async () => {
      await expect(
        createUser({ name: 'Alice', email: 'invalid' })
      ).rejects.toThrow('Invalid email');
    });
  });
});
```

## Next Steps

For integration testing, continue to [Integration Testing](./03-integration-testing.md).
