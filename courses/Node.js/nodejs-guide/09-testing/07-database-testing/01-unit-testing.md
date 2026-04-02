# Database Testing Patterns

## What You'll Learn

- Database unit testing with mocks
- Database integration testing
- Migration testing
- Transaction testing
- Test data management

## Database Unit Testing (Mocked)

```javascript
import { test, describe, beforeEach } from 'node:test';
import assert from 'node:assert/strict';

describe('UserService (mocked DB)', () => {
    let service;
    let mockDb;

    beforeEach(() => {
        mockDb = {
            query: async () => ({ rows: [] }),
            insert: async (data) => ({ id: 1, ...data }),
            update: async (id, data) => ({ id, ...data }),
            delete: async () => ({ rowCount: 1 }),
        };
        service = new UserService(mockDb);
    });

    test('createUser calls db.insert', async () => {
        const data = { name: 'Alice', email: 'alice@test.com' };
        const user = await service.createUser(data);

        assert.equal(user.name, 'Alice');
        assert.ok(user.id);
    });

    test('findByEmail queries database', async () => {
        mockDb.query = async () => ({
            rows: [{ id: 1, name: 'Bob', email: 'bob@test.com' }],
        });

        const user = await service.findByEmail('bob@test.com');
        assert.equal(user.name, 'Bob');
    });

    test('findByEmail returns null when not found', async () => {
        mockDb.query = async () => ({ rows: [] });

        const user = await service.findByEmail('none@test.com');
        assert.equal(user, null);
    });
});
```

## Database Integration Testing (Real DB)

```javascript
import { test, describe, before, after, beforeEach } from 'node:test';
import assert from 'node:assert/strict';
import { Pool } from 'pg';

describe('User Repository (real DB)', () => {
    let pool;
    let repo;

    before(async () => {
        pool = new Pool({
            connectionString: process.env.TEST_DATABASE_URL,
        });

        // Create test schema
        await pool.query(`
            CREATE TABLE IF NOT EXISTS users (
                id SERIAL PRIMARY KEY,
                name VARCHAR(255) NOT NULL,
                email VARCHAR(255) UNIQUE NOT NULL,
                password_hash VARCHAR(255),
                role VARCHAR(50) DEFAULT 'user',
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )
        `);

        repo = new UserRepository(pool);
    });

    beforeEach(async () => {
        // Clean data between tests
        await pool.query('DELETE FROM users');
        await pool.query('ALTER SEQUENCE users_id_seq RESTART WITH 1');
    });

    after(async () => {
        await pool.query('DROP TABLE IF EXISTS users');
        await pool.end();
    });

    test('CRUD operations', async () => {
        // Create
        const user = await repo.create({
            name: 'Alice',
            email: 'alice@test.com',
            password_hash: 'hashed',
        });
        assert.ok(user.id);

        // Read
        const found = await repo.findById(user.id);
        assert.equal(found.name, 'Alice');

        // Update
        const updated = await repo.update(user.id, { name: 'Alice Updated' });
        assert.equal(updated.name, 'Alice Updated');

        // Delete
        await repo.delete(user.id);
        const deleted = await repo.findById(user.id);
        assert.equal(deleted, null);
    });

    test('handles unique constraint violation', async () => {
        await repo.create({ name: 'First', email: 'dup@test.com', password_hash: 'x' });

        await assert.rejects(
            () => repo.create({ name: 'Second', email: 'dup@test.com', password_hash: 'x' }),
            (err) => {
                assert.equal(err.code, '23505'); // PostgreSQL unique violation
                return true;
            }
        );
    });

    test('pagination', async () => {
        for (let i = 0; i < 25; i++) {
            await repo.create({ name: `User ${i}`, email: `u${i}@test.com`, password_hash: 'x' });
        }

        const page1 = await repo.list({ page: 1, limit: 10 });
        assert.equal(page1.data.length, 10);
        assert.equal(page1.total, 25);

        const page3 = await repo.list({ page: 3, limit: 10 });
        assert.equal(page3.data.length, 5);
    });
});
```

## Migration Testing

```javascript
describe('Database Migrations', () => {
    let pool;

    before(async () => {
        pool = new Pool({ connectionString: process.env.TEST_DATABASE_URL });
    });

    after(async () => {
        await pool.end();
    });

    test('migration creates expected tables', async () => {
        // Run migration
        await runMigrations(pool);

        // Verify tables exist
        const { rows } = await pool.query(`
            SELECT table_name FROM information_schema.tables
            WHERE table_schema = 'public'
            ORDER BY table_name
        `);

        const tableNames = rows.map(r => r.table_name);
        assert.ok(tableNames.includes('users'));
        assert.ok(tableNames.includes('products'));
        assert.ok(tableNames.includes('orders'));
    });

    test('migration creates expected columns', async () => {
        const { rows } = await pool.query(`
            SELECT column_name, data_type, is_nullable
            FROM information_schema.columns
            WHERE table_name = 'users'
            ORDER BY ordinal_position
        `);

        const columns = Object.fromEntries(rows.map(r => [r.column_name, r]));
        assert.ok(columns.id);
        assert.equal(columns.email.is_nullable, 'NO');
        assert.equal(columns.role.data_type, 'character varying');
    });

    test('rollback reverts migration', async () => {
        await rollbackMigration(pool);

        const { rows } = await pool.query(`
            SELECT table_name FROM information_schema.tables
            WHERE table_schema = 'public' AND table_name = 'users'
        `);

        assert.equal(rows.length, 0);
    });
});
```

## Transaction Testing

```javascript
describe('Transaction Handling', () => {
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

    test('transfer commits on success', async () => {
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

    test('transfer rolls back on failure', async () => {
        const client = await pool.connect();
        try {
            await client.query('BEGIN');
            await client.query('UPDATE accounts SET balance = balance - 30 WHERE id = 1');
            // Simulate failure
            throw new Error('Simulated failure');
        } catch {
            await client.query('ROLLBACK');
        } finally {
            client.release();
        }

        // Balances unchanged
        const { rows } = await pool.query('SELECT * FROM accounts ORDER BY id');
        assert.equal(parseFloat(rows[0].balance), 100);
        assert.equal(parseFloat(rows[1].balance), 50);
    });
});
```

## Test Data Factories

```javascript
class TestFactory {
    static async createUser(pool, overrides = {}) {
        const user = {
            name: 'Test User',
            email: `user-${Date.now()}@test.com`,
            password_hash: await bcrypt.hash('TestPass123!', 4),
            role: 'user',
            ...overrides,
        };

        const { rows } = await pool.query(
            'INSERT INTO users (name, email, password_hash, role) VALUES ($1, $2, $3, $4) RETURNING *',
            [user.name, user.email, user.password_hash, user.role]
        );

        return rows[0];
    }

    static async createProduct(pool, overrides = {}) {
        const product = {
            name: 'Test Product',
            price: 29.99,
            category: 'test',
            ...overrides,
        };

        const { rows } = await pool.query(
            'INSERT INTO products (name, price, category) VALUES ($1, $2, $3) RETURNING *',
            [product.name, product.price, product.category]
        );

        return rows[0];
    }

    static async seedDatabase(pool, counts = {}) {
        const users = [];
        for (let i = 0; i < (counts.users || 5); i++) {
            users.push(await this.createUser(pool, {
                name: `User ${i}`,
                email: `user${i}@test.com`,
            }));
        }
        return { users };
    }
}

// Usage in tests
test('user can place order', async () => {
    const user = await TestFactory.createUser(pool);
    const product = await TestFactory.createProduct(pool);

    const order = await orderService.create(user.id, [{ productId: product.id, quantity: 1 }]);
    assert.ok(order.id);
});
```

## Best Practices Checklist

- [ ] Use separate test database
- [ ] Clean data between tests
- [ ] Use factories for test data
- [ ] Test transactions and rollbacks
- [ ] Test constraint violations
- [ ] Test migration up and rollback
- [ ] Use connection pooling for tests
- [ ] Set reasonable timeouts

## Cross-References

- See [Integration Testing](../04-integration-testing/01-rest-api.md) for API+DB testing
- See [Unit Testing](../03-unit-testing/01-functions-classes.md) for unit patterns
- See [Performance Testing](../09-security-performance/01-security-testing.md) for DB performance

## Next Steps

Continue to [Async Testing](../08-async-testing/01-promises-async.md).
