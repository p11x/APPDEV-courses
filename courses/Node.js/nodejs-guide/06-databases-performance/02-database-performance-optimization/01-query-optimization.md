# Database Performance Optimization

## What You'll Learn

- Query optimization and indexing
- Connection pool tuning
- Batch operations and bulk updates
- Slow query analysis

## Query Optimization

```javascript
// BAD: N+1 query problem
async function getUsersWithPosts() {
    const users = await User.find();  // 1 query
    for (const user of users) {
        user.posts = await Post.find({ userId: user.id }); // N queries
    }
    return users;
}

// GOOD: Use populate (Mongoose) or JOIN (SQL)
async function getUsersWithPosts() {
    return User.find().populate('posts'); // 1 query
}

// PostgreSQL with JOIN
async function getUsersWithPosts() {
    const { rows } = await pool.query(`
        SELECT u.*, json_agg(p.*) as posts
        FROM users u
        LEFT JOIN posts p ON p.user_id = u.id
        GROUP BY u.id
    `);
    return rows;
}
```

## Indexing Strategies

```sql
-- PostgreSQL indexes
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_created_at ON users(created_at DESC);
CREATE INDEX idx_posts_user_id ON posts(user_id);
CREATE INDEX idx_posts_user_created ON posts(user_id, created_at DESC);

-- Composite index for common queries
CREATE INDEX idx_users_role_active ON users(role, is_active);
```

```javascript
// MongoDB indexes
userSchema.index({ email: 1 });
userSchema.index({ createdAt: -1 });
userSchema.index({ role: 1, isActive: 1 });
postSchema.index({ userId: 1, createdAt: -1 });
```

## Batch Operations

```javascript
// PostgreSQL bulk insert
async function bulkInsert(users) {
    const values = users.map((u, i) => `($${i*3+1}, $${i*3+2}, $${i*3+3})`).join(', ');
    const params = users.flatMap(u => [u.name, u.email, u.age]);
    return pool.query(
        `INSERT INTO users (name, email, age) VALUES ${values}`,
        params
    );
}

// MongoDB bulk write
async function bulkUpdate(updates) {
    const ops = updates.map(u => ({
        updateOne: {
            filter: { _id: u.id },
            update: { $set: u.data }
        }
    }));
    return User.bulkWrite(ops);
}
```

## Connection Pool Tuning

```javascript
const pool = new Pool({
    host: 'localhost',
    port: 5432,
    database: 'myapp',
    max: 20,                    // Max connections
    min: 2,                     // Min idle connections
    idleTimeoutMillis: 30000,   // Close idle after 30s
    connectionTimeoutMillis: 5000,
    statement_timeout: 30000,   // Query timeout
    query_timeout: 30000,
});

// Monitor pool
pool.on('connect', () => console.log('New connection'));
pool.on('error', (err) => console.error('Pool error:', err));
pool.on('remove', () => console.log('Connection removed'));
```

## Best Practices Checklist

- [ ] Add indexes for frequently queried columns
- [ ] Use EXPLAIN ANALYZE to understand query plans
- [ ] Batch inserts/updates instead of individual operations
- [ ] Use connection pooling with appropriate limits
- [ ] Monitor slow queries

## Cross-References

- See [Integration Patterns](./01-mongodb-postgres.md) for database setup
- See [Caching](../04-caching-strategies-implementation/01-in-memory-caching.md) for caching
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for observability

## Next Steps

Continue to [Performance Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md).
