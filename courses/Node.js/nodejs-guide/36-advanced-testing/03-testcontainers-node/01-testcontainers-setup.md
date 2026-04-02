# Testcontainers Setup

## What You'll Learn

- What Testcontainers is and why it's useful
- How to set up Testcontainers with Node.js
- How to spin up databases for testing
- How Testcontainers compares to mocking

## What Is Testcontainers?

Testcontainers spins up real Docker containers for tests. Instead of mocking a database, you test against a real one.

| Approach | Pros | Cons |
|----------|------|------|
| Mocking | Fast, isolated | Not realistic |
| Shared DB | Realistic | State pollution |
| Testcontainers | Realistic + isolated | Slower (Docker startup) |

## Setup

```bash
npm install -D testcontainers
```

## Basic Usage

```ts
// tests/db.test.ts

import { describe, it, beforeAll, afterAll } from 'vitest';
import { PostgreSqlContainer, StartedPostgreSqlContainer } from '@testcontainers/postgresql';
import { Pool } from 'pg';

describe('Database', () => {
  let container: StartedPostgreSqlContainer;
  let pool: Pool;

  beforeAll(async () => {
    // Start PostgreSQL container
    container = await new PostgreSqlContainer('postgres:16').start();

    // Create connection pool
    pool = new Pool({
      host: container.getHost(),
      port: container.getPort(),
      database: container.getDatabase(),
      user: container.getUsername(),
      password: container.getPassword(),
    });

    // Run migrations
    await pool.query(`
      CREATE TABLE users (
        id SERIAL PRIMARY KEY,
        name TEXT NOT NULL,
        email TEXT UNIQUE NOT NULL
      )
    `);
  }, 60_000);  // 60s timeout for container startup

  afterAll(async () => {
    await pool.end();
    await container.stop();
  });

  it('creates a user', async () => {
    const result = await pool.query(
      'INSERT INTO users (name, email) VALUES ($1, $2) RETURNING *',
      ['Alice', 'alice@example.com']
    );

    expect(result.rows[0].name).toBe('Alice');
    expect(result.rows[0].email).toBe('alice@example.com');
  });

  it('finds a user', async () => {
    const result = await pool.query(
      'SELECT * FROM users WHERE email = $1',
      ['alice@example.com']
    );

    expect(result.rows).toHaveLength(1);
  });
});
```

## Next Steps

For PostgreSQL, continue to [Testcontainers PostgreSQL](./02-testcontainers-postgres.md).
