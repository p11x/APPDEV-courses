# Database Migration Testing and Transaction Patterns

## What You'll Learn

- Testing database migrations
- Transaction isolation in tests
- Database snapshot and restore
- Performance testing database operations

## Migration Testing

```javascript
import { test, describe, before, after } from 'node:test';
import assert from 'node:assert/strict';
import { Pool } from 'pg';

describe('Database Migrations', () => {
    let pool;

    before(async () => {
        pool = new Pool({ connectionString: process.env.TEST_DATABASE_URL });
    });

    after(async () => {
        await pool.end();
    });

    test('creates expected tables', async () => {
        // Run migrations
        await runMigrations(pool);

        const { rows } = await pool.query(`
            SELECT table_name FROM information_schema.tables
            WHERE table_schema = 'public'
            ORDER BY table_name
        `);

        const tables = rows.map(r => r.table_name);
        assert.ok(tables.includes('users'));
        assert.ok(tables.includes('orders'));
        assert.ok(tables.includes('products'));
    });

    test('creates expected columns', async () => {
        const { rows } = await pool.query(`
            SELECT column_name, data_type, is_nullable
            FROM information_schema.columns
            WHERE table_name = 'users'
            ORDER BY ordinal_position
        `);

        const columns = Object.fromEntries(rows.map(r => [r.column_name, r]));
        assert.ok(columns.id);
        assert.equal(columns.email.is_nullable, 'NO');
        assert.equal(columns.created_at.data_type, 'timestamp without time zone');
    });

    test('creates expected indexes', async () => {
        const { rows } = await pool.query(`
            SELECT indexname, indexdef
            FROM pg_indexes
            WHERE tablename = 'users'
        `);

        const indexNames = rows.map(r => r.indexname);
        assert.ok(indexNames.includes('users_pkey'));
        assert.ok(indexNames.some(n => n.includes('email')));
    });

    test('rollback removes tables', async () => {
        await rollbackMigrations(pool);

        const { rows } = await pool.query(`
            SELECT table_name FROM information_schema.tables
            WHERE table_schema = 'public' AND table_name = 'users'
        `);

        assert.equal(rows.length, 0);
    });
});
```

## Transaction Isolation Testing

```javascript
describe('Transaction Isolation', () => {
    let pool;

    before(async () => {
        pool = new Pool({ connectionString: process.env.TEST_DATABASE_URL });
        await pool.query(`
            CREATE TABLE IF NOT EXISTS accounts (
                id SERIAL PRIMARY KEY,
                balance DECIMAL(10,2) DEFAULT 0
            )
        `);
    });

    beforeEach(async () => {
        await pool.query('DELETE FROM accounts');
        await pool.query(`INSERT INTO accounts (balance) VALUES (100), (50)`);
    });

    after(async () => {
        await pool.query('DROP TABLE IF EXISTS accounts');
        await pool.end();
    });

    test('commits on success', async () => {
        const client = await pool.connect();
        try {
            await client.query('BEGIN');
            await client.query('UPDATE accounts SET balance = balance - 30 WHERE id = 1');
            await client.query('UPDATE accounts SET balance = balance + 30 WHERE id = 2');
            await client.query('COMMIT');
        } finally {
            client.release();
        }

        const { rows } = await pool.query('SELECT * FROM accounts ORDER BY id');
        assert.equal(parseFloat(rows[0].balance), 70);
        assert.equal(parseFloat(rows[1].balance), 80);
    });

    test('rolls back on failure', async () => {
        const client = await pool.connect();
        try {
            await client.query('BEGIN');
            await client.query('UPDATE accounts SET balance = balance - 30 WHERE id = 1');
            throw new Error('Simulated failure');
        } catch {
            await client.query('ROLLBACK');
        } finally {
            client.release();
        }

        const { rows } = await pool.query('SELECT * FROM accounts ORDER BY id');
        assert.equal(parseFloat(rows[0].balance), 100); // Unchanged
    });

    test('uses savepoints for partial rollback', async () => {
        const client = await pool.connect();
        try {
            await client.query('BEGIN');
            await client.query('UPDATE accounts SET balance = balance - 20 WHERE id = 1');

            await client.query('SAVEPOINT before_risky');
            await client.query('UPDATE accounts SET balance = balance - 1000 WHERE id = 1');
            // This would make balance negative — rollback to savepoint
            await client.query('ROLLBACK TO SAVEPOINT before_risky');

            await client.query('COMMIT');
        } finally {
            client.release();
        }

        const { rows } = await pool.query('SELECT * FROM accounts WHERE id = 1');
        assert.equal(parseFloat(rows[0].balance), 80); // Only first update applied
    });
});
```

## Transaction-Based Test Isolation

```javascript
describe('Transaction Isolation for Tests', () => {
    let pool;
    let client;

    before(async () => {
        pool = new Pool({ connectionString: process.env.TEST_DATABASE_URL });
    });

    beforeEach(async () => {
        client = await pool.connect();
        await client.query('BEGIN');
    });

    afterEach(async () => {
        await client.query('ROLLBACK'); // Auto-cleanup
        client.release();
    });

    after(async () => {
        await pool.end();
    });

    test('creates user in transaction', async () => {
        await client.query(
            'INSERT INTO users (name, email) VALUES ($1, $2)',
            ['Test', 'test@test.com']
        );

        const { rows } = await client.query('SELECT * FROM users');
        assert.equal(rows.length, 1);
    });

    test('previous test data is rolled back', async () => {
        const { rows } = await client.query('SELECT * FROM users');
        assert.equal(rows.length, 0); // Clean because of rollback
    });
});
```

## Common Mistakes

- Not testing migration rollback
- Not using transactions for test isolation
- Not testing constraint violations
- Not cleaning up test data between tests

## Cross-References

- See [Database Testing](./01-unit-testing.md) for DB unit testing
- See [Integration Testing](../04-integration-testing/01-rest-api.md) for API+DB testing
- See [Performance Testing](../09-security-performance/01-security-testing.md) for DB performance

## Next Steps

Continue to [Async Testing: Streams and Events](../08-async-testing/02-streams-events.md).
