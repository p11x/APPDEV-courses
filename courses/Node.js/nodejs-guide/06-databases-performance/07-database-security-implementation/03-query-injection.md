# Query Injection Prevention and Protection

## What You'll Learn

- SQL injection attack patterns
- Parameterized query best practices
- Input sanitization strategies
- Query builder security
- NoSQL injection prevention

## SQL Injection Prevention

```javascript
// DANGEROUS - Never do this
const query = `SELECT * FROM users WHERE id = '${userId}'`;
const query2 = `SELECT * FROM users WHERE name = '${name}'`;

// SAFE - Parameterized queries (PostgreSQL)
const { rows } = await pool.query(
    'SELECT * FROM users WHERE id = $1',
    [userId]
);

// SAFE - Parameterized queries (MySQL)
const [rows] = await pool.execute(
    'SELECT * FROM users WHERE id = ?',
    [userId]
);

// SAFE - Parameterized queries (SQLite)
const stmt = db.prepare('SELECT * FROM users WHERE id = ?');
const row = stmt.get(userId);
```

## Input Validation Layer

```javascript
import { z } from 'zod';

class QueryInputValidator {
    static userId = z.number().int().positive();
    static email = z.string().email().max(255);
    static name = z.string().min(1).max(255).regex(/^[a-zA-Z\s'-]+$/);
    static search = z.string().max(500).transform(s => s.replace(/[%_\\]/g, '\\$&'));
    static page = z.number().int().min(1).max(1000).default(1);
    static limit = z.number().int().min(1).max(100).default(20);

    static validate(schema, data) {
        const result = schema.safeParse(data);
        if (!result.success) {
            throw new ValidationError(result.error.errors);
        }
        return result.data;
    }
}

class ValidationError extends Error {
    constructor(errors) {
        super('Validation failed');
        this.errors = errors;
    }
}

// Usage in route
app.get('/api/users', async (req, res) => {
    try {
        const page = QueryInputValidator.validate(QueryInputValidator.page, Number(req.query.page));
        const limit = QueryInputValidator.validate(QueryInputValidator.limit, Number(req.query.limit));
        const search = req.query.search
            ? QueryInputValidator.validate(QueryInputValidator.search, req.query.search)
            : null;

        let sql = 'SELECT * FROM users';
        const params = [];

        if (search) {
            sql += ' WHERE name ILIKE $1 OR email ILIKE $1';
            params.push(`%${search}%`);
        }

        sql += ` ORDER BY created_at DESC LIMIT $${params.length + 1} OFFSET $${params.length + 2}`;
        params.push(limit, (page - 1) * limit);

        const { rows } = await pool.query(sql, params);
        res.json(rows);
    } catch (err) {
        if (err instanceof ValidationError) {
            return res.status(400).json({ errors: err.errors });
        }
        throw err;
    }
});
```

## MongoDB Injection Prevention

```javascript
// DANGEROUS - $where with user input
const user = await User.findOne({ $where: `this.name === '${name}'` });

// SAFE - Use query operators
const user = await User.findOne({ name });

// DANGEROUS - User-controlled operator injection
const query = JSON.parse(req.query.filter); // { $gt: '' }
const users = await User.find({ role: query });

// SAFE - Sanitize and validate input
function sanitizeMongoQuery(input) {
    if (typeof input !== 'object' || input === null) {
        return input;
    }

    const sanitized = {};
    for (const [key, value] of Object.entries(input)) {
        if (key.startsWith('$')) {
            // Reject operator injection
            throw new Error('Invalid query operator');
        }
        sanitized[key] = typeof value === 'object' ? sanitizeMongoQuery(value) : value;
    }
    return sanitized;
}

const safeQuery = sanitizeMongoQuery(req.query.filter);
const users = await User.find(safeQuery);
```

## Query Builder Security

```javascript
class SecureQueryBuilder {
    constructor(table) {
        this.table = table;
        this.allowedColumns = new Set();
        this.whereClauses = [];
        this.params = [];
    }

    allowColumns(columns) {
        columns.forEach(c => this.allowedColumns.add(c));
        return this;
    }

    where(column, operator, value) {
        if (!this.allowedColumns.has(column)) {
            throw new Error(`Column ${column} not in allowed list`);
        }
        
        const validOperators = ['=', '!=', '>', '<', '>=', '<=', 'IN', 'LIKE'];
        if (!validOperators.includes(operator)) {
            throw new Error(`Invalid operator: ${operator}`);
        }

        this.params.push(value);
        this.whereClauses.push(`${column} ${operator} $${this.params.length}`);
        return this;
    }

    build() {
        let sql = `SELECT * FROM ${this.table}`;
        if (this.whereClauses.length > 0) {
            sql += ` WHERE ${this.whereClauses.join(' AND ')}`;
        }
        return { sql, params: this.params };
    }
}

const { sql, params } = new SecureQueryBuilder('users')
    .allowColumns(['name', 'email', 'role', 'active'])
    .where('active', '=', true)
    .where('role', '=', 'user')
    .build();
```

## Best Practices Checklist

- [ ] Always use parameterized queries
- [ ] Never interpolate user input into SQL
- [ ] Validate and sanitize all inputs
- [ ] Use allowlists for dynamic column/table names
- [ ] Implement input length limits
- [ ] Reject MongoDB operator injection
- [ ] Use ORM/query builders with escaping
- [ ] Test with SQL injection scanners

## Cross-References

- See [Access Control](./02-access-control.md) for authorization
- See [Connection Security](./01-connection-security.md) for connection encryption
- See [Audit Logging](./02-access-control.md#audit-logging) for tracking

## Next Steps

Continue to [Database Encryption](./04-encryption.md) for data protection.
