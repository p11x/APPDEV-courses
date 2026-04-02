# I/O and Database Optimization

## What You'll Learn

- Connection pooling strategies
- Query optimization patterns
- Batch operations
- Caching for I/O

## Connection Pooling

```javascript
import { Pool } from 'pg';

const pool = new Pool({
    host: process.env.DB_HOST,
    port: 5432,
    database: process.env.DB_NAME,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    max: 20,                    // Max connections
    idleTimeoutMillis: 30000,   // Close idle after 30s
    connectionTimeoutMillis: 5000,
});

// Query with pool
async function getUsers(limit = 20) {
    const { rows } = await pool.query(
        'SELECT * FROM users ORDER BY created_at DESC LIMIT $1',
        [limit]
    );
    return rows;
}
```

## Batch Operations

```javascript
// BAD: Individual inserts
for (const user of users) {
    await db.query('INSERT INTO users (name) VALUES ($1)', [user.name]);
}

// GOOD: Batch insert
const values = users.map((u, i) => `($${i + 1})`).join(', ');
const params = users.map(u => u.name);
await db.query(
    `INSERT INTO users (name) VALUES ${values}`,
    params
);

// BEST: Use COPY for large datasets
const { pipeline } = require('stream');
const { from } = require('pg-copy-streams');

pipeline(
    fs.createReadStream('users.csv'),
    pool.query(from('COPY users FROM STDIN CSV')),
    (err) => console.log(err || 'Import complete')
);
```

## Query Optimization

```javascript
// Use indexes
// CREATE INDEX idx_users_email ON users(email);

// Select only needed columns
// BAD: SELECT * FROM users
// GOOD: SELECT id, name, email FROM users

// Use pagination
const page = parseInt(req.query.page) || 1;
const limit = parseInt(req.query.limit) || 20;
const offset = (page - 1) * limit;

const { rows } = await pool.query(
    'SELECT id, name FROM users ORDER BY id LIMIT $1 OFFSET $2',
    [limit, offset]
);
```

## Best Practices Checklist

- [ ] Use connection pooling for all databases
- [ ] Batch inserts/updates instead of individual operations
- [ ] Add indexes for frequently queried columns
- [ ] Select only needed columns
- [ ] Implement pagination for large result sets
- [ ] Cache frequently accessed queries

## Cross-References

- See [CPU/Memory](./01-cpu-memory-optimization.md) for compute optimization
- See [Caching](../13-caching-strategies/01-in-memory-caching.md) for caching
- See [Monitoring](./03-monitoring-metrics.md) for metrics

## Next Steps

Continue to [Monitoring and Metrics](./03-monitoring-metrics.md) for production observability.
