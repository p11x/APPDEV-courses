# Async Database Operations

## What You'll Learn

- Async patterns for PostgreSQL, MongoDB, Redis
- Connection pooling strategies
- Transaction handling
- Query optimization

## PostgreSQL Async Operations

```javascript
import { Pool } from 'pg';

const pool = new Pool({
    host: 'localhost',
    port: 5432,
    database: 'mydb',
    max: 20,
    idleTimeoutMillis: 30000,
});

// Query with connection pool
async function getUsers(limit = 20) {
    const { rows } = await pool.query(
        'SELECT * FROM users ORDER BY created_at DESC LIMIT $1',
        [limit]
    );
    return rows;
}

// Transaction
async function transferFunds(fromId, toId, amount) {
    const client = await pool.connect();
    try {
        await client.query('BEGIN');
        await client.query('UPDATE accounts SET balance = balance - $1 WHERE id = $2', [amount, fromId]);
        await client.query('UPDATE accounts SET balance = balance + $1 WHERE id = $2', [amount, toId]);
        await client.query('COMMIT');
    } catch (err) {
        await client.query('ROLLBACK');
        throw err;
    } finally {
        client.release();
    }
}
```

## MongoDB Async Operations

```javascript
import { MongoClient } from 'mongodb';

const client = new MongoClient(process.env.MONGODB_URL);
await client.connect();
const db = client.db('mydb');

// CRUD operations
async function createUser(data) {
    const result = await db.collection('users').insertOne(data);
    return { ...data, _id: result.insertedId };
}

async function findUsers(filter = {}, options = {}) {
    const { limit = 20, skip = 0, sort = { createdAt: -1 } } = options;
    return db.collection('users')
        .find(filter)
        .sort(sort)
        .skip(skip)
        .limit(limit)
        .toArray();
}

async function updateUser(id, data) {
    return db.collection('users').updateOne(
        { _id: new ObjectId(id) },
        { $set: data }
    );
}
```

## Redis Async Operations

```javascript
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
await redis.connect();

// Cache-aside pattern
async function getUser(id) {
    const cacheKey = `user:${id}`;

    // Check cache
    const cached = await redis.get(cacheKey);
    if (cached) return JSON.parse(cached);

    // Fetch from DB
    const user = await db.query('SELECT * FROM users WHERE id = $1', [id]);

    // Store in cache (1 hour TTL)
    await redis.set(cacheKey, JSON.stringify(user), { EX: 3600 });

    return user;
}
```

## Best Practices Checklist

- [ ] Use connection pooling for all databases
- [ ] Implement transactions for multi-step operations
- [ ] Cache frequently accessed data in Redis
- [ ] Handle connection errors gracefully
- [ ] Set appropriate timeouts for queries

## Cross-References

- See [Frameworks](./01-frameworks.md) for HTTP frameworks
- See [API Development](./03-api-development.md) for API patterns
- See [Error Handling](../07-async-error-handling/01-error-propagation.md) for errors

## Next Steps

Continue to [API Development](./03-api-development.md) for async API patterns.
