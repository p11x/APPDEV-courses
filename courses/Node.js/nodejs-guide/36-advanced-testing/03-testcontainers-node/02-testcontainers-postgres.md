# Testcontainers with PostgreSQL

## What You'll Learn

- How to test with real PostgreSQL
- How to run migrations in tests
- How to seed test data
- How to handle container lifecycle

## Full Example

```ts
// tests/postgres.test.ts

import { describe, it, beforeAll, afterAll, beforeEach } from 'vitest';
import { PostgreSqlContainer } from '@testcontainers/postgresql';
import { Pool } from 'pg';

describe('PostgreSQL Tests', () => {
  let container;
  let pool: Pool;

  beforeAll(async () => {
    container = await new PostgreSqlContainer('postgres:16')
      .withDatabase('testdb')
      .withUsername('test')
      .withPassword('test')
      .start();

    pool = new Pool({
      connectionString: container.getConnectionUri(),
    });

    // Run migrations
    await pool.query(`
      CREATE TABLE users (
        id SERIAL PRIMARY KEY,
        name TEXT NOT NULL,
        email TEXT UNIQUE NOT NULL,
        created_at TIMESTAMPTZ DEFAULT NOW()
      )
    `);
  }, 60_000);

  beforeEach(async () => {
    // Clean data between tests
    await pool.query('DELETE FROM users');
  });

  afterAll(async () => {
    await pool.end();
    await container.stop();
  });

  it('inserts and retrieves a user', async () => {
    await pool.query(
      'INSERT INTO users (name, email) VALUES ($1, $2)',
      ['Alice', 'alice@test.com']
    );

    const { rows } = await pool.query('SELECT * FROM users');
    expect(rows).toHaveLength(1);
    expect(rows[0].name).toBe('Alice');
  });
});
```

## Next Steps

For MongoDB, continue to [Testcontainers MongoDB](./03-testcontainers-mongodb.md).
