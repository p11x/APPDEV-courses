# SQLite, ORM Comparison, and Connection Management

## What You'll Learn

- SQLite for development and testing
- ORM vs native driver selection
- Connection pool optimization
- Database error handling and recovery

## SQLite with better-sqlite3

```bash
npm install better-sqlite3
```

```javascript
import Database from 'better-sqlite3';

const db = new Database('app.db', { verbose: console.log });

// Enable WAL mode for better concurrency
db.pragma('journal_mode = WAL');
db.pragma('foreign_keys = ON');

// Create tables
db.exec(`
    CREATE TABLE IF NOT EXISTS users (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        email TEXT UNIQUE NOT NULL,
        created_at DATETIME DEFAULT CURRENT_TIMESTAMP
    )
`);

// CRUD operations (synchronous — fast for SQLite)
function createUser(data) {
    const stmt = db.prepare('INSERT INTO users (name, email) VALUES (?, ?)');
    return stmt.run(data.name, data.email);
}

function findUsers(limit = 20) {
    const stmt = db.prepare('SELECT * FROM users ORDER BY created_at DESC LIMIT ?');
    return stmt.all(limit);
}

function findUserById(id) {
    const stmt = db.prepare('SELECT * FROM users WHERE id = ?');
    return stmt.get(id);
}

// Transactions
const transfer = db.transaction((fromId, toId, amount) => {
    db.prepare('UPDATE accounts SET balance = balance - ? WHERE id = ?').run(amount, fromId);
    db.prepare('UPDATE accounts SET balance = balance + ? WHERE id = ?').run(amount, toId);
});

transfer(1, 2, 100);
```

## ORM vs Native Driver Comparison

```
Feature          Prisma       Mongoose     pg (native)  Drizzle
─────────────────────────────────────────────────────────────────
Type safety      Excellent    Good         Manual       Excellent
Performance      Good         Good         Best         Best
Learning curve   Medium       Low          Medium       Medium
Migration        Built-in     Manual       Manual       Built-in
Query builder    Yes          Yes          No           Yes
Relationships    Excellent    Good         Manual       Good
Database support Multiple     MongoDB      PostgreSQL   Multiple
Schema sync      Yes          Yes          No           Yes
```

## Error Handling Patterns

```javascript
class DatabaseError extends Error {
    constructor(message, code, originalError) {
        super(message);
        this.code = code;
        this.originalError = originalError;
    }
}

async function safeQuery(pool, text, params) {
    try {
        return await pool.query(text, params);
    } catch (err) {
        if (err.code === '23505') {
            throw new DatabaseError('Duplicate entry', 'DUPLICATE', err);
        }
        if (err.code === 'ECONNREFUSED') {
            throw new DatabaseError('Database unavailable', 'CONNECTION_ERROR', err);
        }
        throw new DatabaseError('Query failed', 'QUERY_ERROR', err);
    }
}
```

## Best Practices Checklist

- [ ] Use SQLite for development/testing
- [ ] Choose ORM based on team experience and project needs
- [ ] Implement connection retry logic
- [ ] Use transactions for multi-step operations
- [ ] Handle database-specific error codes

## Cross-References

- See [MongoDB/Postgres](./01-mongodb-postgres.md) for driver setup
- See [Database Performance](./02-database-performance.md) for optimization
- See [Database Security](../07-database-security-implementation/01-connection-security.md) for security

## Next Steps

Continue to [Database Performance](./02-database-performance.md) for optimization.
