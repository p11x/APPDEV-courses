# Database Integration Patterns with Node.js

## What You'll Learn

- MongoDB async operations with Mongoose
- PostgreSQL with node-postgres (pg)
- Database connection pooling strategies
- ORM vs native driver comparison

## MongoDB with Mongoose

```bash
npm install mongoose
```

```javascript
import mongoose from 'mongoose';

// Connection
await mongoose.connect(process.env.MONGODB_URL, {
    maxPoolSize: 10,
    serverSelectionTimeoutMS: 5000,
    socketTimeoutMS: 45000,
});

// Schema definition
const userSchema = new mongoose.Schema({
    name: { type: String, required: true, trim: true },
    email: { type: String, required: true, unique: true, lowercase: true },
    age: { type: Number, min: 0, max: 150 },
    role: { type: String, enum: ['user', 'admin'], default: 'user' },
    createdAt: { type: Date, default: Date.now },
});

// Indexes
userSchema.index({ email: 1 });
userSchema.index({ createdAt: -1 });

// Model
const User = mongoose.model('User', userSchema);

// CRUD Operations
async function createUser(data) {
    return User.create(data);
}

async function findUsers(filter = {}, options = {}) {
    const { limit = 20, skip = 0, sort = { createdAt: -1 } } = options;
    return User.find(filter).sort(sort).skip(skip).limit(limit).lean();
}

async function updateUser(id, data) {
    return User.findByIdAndUpdate(id, data, { new: true, runValidators: true });
}

async function deleteUser(id) {
    return User.findByIdAndDelete(id);
}
```

## PostgreSQL with node-postgres

```bash
npm install pg
```

```javascript
import { Pool } from 'pg';

// Connection pool
const pool = new Pool({
    host: process.env.PG_HOST || 'localhost',
    port: process.env.PG_PORT || 5432,
    database: process.env.PG_DATABASE,
    user: process.env.PG_USER,
    password: process.env.PG_PASSWORD,
    max: 20,                    // Max connections
    idleTimeoutMillis: 30000,
    connectionTimeoutMillis: 5000,
});

// Query helper
async function query(text, params) {
    const start = Date.now();
    const result = await pool.query(text, params);
    const duration = Date.now() - start;
    console.log('Query:', { text, duration, rows: result.rowCount });
    return result;
}

// CRUD Operations
async function createUser(data) {
    const { rows } = await query(
        'INSERT INTO users (name, email, age) VALUES ($1, $2, $3) RETURNING *',
        [data.name, data.email, data.age]
    );
    return rows[0];
}

async function findUsers(options = {}) {
    const { limit = 20, offset = 0, orderBy = 'created_at', order = 'DESC' } = options;
    const { rows } = await query(
        `SELECT * FROM users ORDER BY ${orderBy} ${order} LIMIT $1 OFFSET $2`,
        [limit, offset]
    );
    return rows;
}

async function findUserById(id) {
    const { rows } = await query('SELECT * FROM users WHERE id = $1', [id]);
    return rows[0] || null;
}

async function updateUser(id, data) {
    const fields = Object.keys(data).map((key, i) => `${key} = $${i + 2}`);
    const values = [id, ...Object.values(data)];
    const { rows } = await query(
        `UPDATE users SET ${fields.join(', ')} WHERE id = $1 RETURNING *`,
        values
    );
    return rows[0];
}

async function deleteUser(id) {
    const { rowCount } = await query('DELETE FROM users WHERE id = $1', [id]);
    return rowCount > 0;
}
```

## Connection Pool Comparison

```
Connection Pool Configuration:
─────────────────────────────────────────────
                    PostgreSQL    MongoDB    SQLite
Pool enabled        Yes          Yes        N/A
Max connections     20 (tune)    10 (tune)  N/A
Idle timeout        30s          30s        N/A
Connection timeout  5s           5s         N/A
Retry on failure    Yes          Yes        N/A

Recommended pool sizes:
├── Small app (< 100 concurrent):  5-10 connections
├── Medium app (< 1000 concurrent): 10-20 connections
├── Large app (< 10000 concurrent): 20-50 connections
└── Formula: pool_size = (num_cpus * 2) + effective_spindle_count
```

## Transaction Management

```javascript
// PostgreSQL transaction
async function transferFunds(fromId, toId, amount) {
    const client = await pool.connect();
    try {
        await client.query('BEGIN');

        await client.query(
            'UPDATE accounts SET balance = balance - $1 WHERE id = $2',
            [amount, fromId]
        );

        await client.query(
            'UPDATE accounts SET balance = balance + $1 WHERE id = $2',
            [amount, toId]
        );

        await client.query('COMMIT');
        return { success: true };
    } catch (err) {
        await client.query('ROLLBACK');
        throw err;
    } finally {
        client.release();
    }
}

// MongoDB transaction
async function transferFundsMongo(fromId, toId, amount) {
    const session = await mongoose.startSession();
    try {
        session.startTransaction();

        await Account.findByIdAndUpdate(
            fromId,
            { $inc: { balance: -amount } },
            { session }
        );

        await Account.findByIdAndUpdate(
            toId,
            { $inc: { balance: amount } },
            { session }
        );

        await session.commitTransaction();
        return { success: true };
    } catch (err) {
        await session.abortTransaction();
        throw err;
    } finally {
        session.endSession();
    }
}
```

## Best Practices Checklist

- [ ] Use connection pooling for all databases
- [ ] Use parameterized queries (never string interpolation)
- [ ] Implement transactions for multi-step operations
- [ ] Set appropriate pool sizes based on workload
- [ ] Handle connection errors gracefully
- [ ] Monitor connection pool usage

## Cross-References

- See [Database Performance](./02-database-performance.md) for optimization
- See [Caching Strategies](../04-caching-strategies-implementation/01-in-memory-caching.md) for caching
- See [Database Security](../07-database-security-implementation/01-connection-security.md) for security

## Next Steps

Continue to [Database Performance](./02-database-performance.md) for query optimization.
