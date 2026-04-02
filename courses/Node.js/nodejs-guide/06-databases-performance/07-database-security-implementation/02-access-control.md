# Database Access Control and Permissions

## What You'll Learn

- Database role-based access control
- Row-level security (RLS)
- Application-level authorization
- Principle of least privilege
- Audit logging for access control

## PostgreSQL Role-Based Access

```sql
-- Create application roles
CREATE ROLE app_readonly;
CREATE ROLE app_readwrite;
CREATE ROLE app_admin;

-- Grant schema usage
GRANT USAGE ON SCHEMA public TO app_readonly, app_readwrite, app_admin;

-- Readonly permissions
GRANT SELECT ON ALL TABLES IN SCHEMA public TO app_readonly;

-- Read-write permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO app_readwrite;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO app_readwrite;

-- Admin permissions
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO app_admin;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO app_admin;

-- Create application users
CREATE USER app_service WITH PASSWORD 'secure_password';
GRANT app_readwrite TO app_service;

CREATE USER app_readonly_user WITH PASSWORD 'readonly_password';
GRANT app_readonly TO app_readonly_user;

-- Default privileges for future tables
ALTER DEFAULT PRIVILEGES IN SCHEMA public
    GRANT SELECT ON TABLES TO app_readonly;

ALTER DEFAULT PRIVILEGES IN SCHEMA public
    GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO app_readwrite;
```

## Row-Level Security

```sql
-- Enable RLS
ALTER TABLE orders ENABLE ROW LEVEL SECURITY;

-- Users can only see their own orders
CREATE POLICY user_orders ON orders
    FOR SELECT
    USING (user_id = current_setting('app.current_user_id')::integer);

-- Admins can see all orders
CREATE POLICY admin_orders ON orders
    FOR ALL
    USING (current_setting('app.current_role') = 'admin');

-- Service accounts bypass RLS
ALTER TABLE orders FORCE ROW LEVEL SECURITY;
```

```javascript
// Set context for RLS
async function queryAsUser(pool, userId, sql, params) {
    const client = await pool.connect();
    try {
        await client.query(`SET app.current_user_id = '${userId}'`);
        return await client.query(sql, params);
    } finally {
        await client.query('RESET app.current_user_id');
        client.release();
    }
}
```

## Application-Level Authorization

```javascript
class DatabaseAuthMiddleware {
    constructor(pool) {
        this.pool = pool;
        this.permissions = {
            user: ['read:own', 'update:own'],
            moderator: ['read:own', 'update:own', 'read:any', 'moderate:any'],
            admin: ['read:any', 'write:any', 'delete:any', 'moderate:any'],
        };
    }

    checkPermission(role, action) {
        return this.permissions[role]?.includes(action) || false;
    }

    enforce(action) {
        return (req, res, next) => {
            if (!req.user) {
                return res.status(401).json({ error: 'Authentication required' });
            }

            if (!this.checkPermission(req.user.role, action)) {
                return res.status(403).json({ error: 'Insufficient permissions' });
            }

            next();
        };
    }

    // Row-level enforcement in application
    enforceOwnership(table, idParam = 'id', ownerColumn = 'user_id') {
        return async (req, res, next) => {
            const { rows } = await this.pool.query(
                `SELECT ${ownerColumn} FROM ${table} WHERE id = $1`,
                [req.params[idParam]]
            );

            if (rows.length === 0) {
                return res.status(404).json({ error: 'Not found' });
            }

            if (rows[0][ownerColumn] !== req.user.id && req.user.role !== 'admin') {
                return res.status(403).json({ error: 'Access denied' });
            }

            next();
        };
    }
}

// Usage
const dbAuth = new DatabaseAuthMiddleware(pool);

app.get('/api/users/:id',
    dbAuth.enforce('read:any'),
    async (req, res) => {
        const { rows } = await pool.query('SELECT * FROM users WHERE id = $1', [req.params.id]);
        res.json(rows[0]);
    }
);

app.put('/api/posts/:id',
    dbAuth.enforce('update:own'),
    dbAuth.enforceOwnership('posts', 'id', 'author_id'),
    async (req, res) => {
        const { rows } = await pool.query(
            'UPDATE posts SET title = $1, content = $2 WHERE id = $3 RETURNING *',
            [req.body.title, req.body.content, req.params.id]
        );
        res.json(rows[0]);
    }
);
```

## Audit Logging

```javascript
class AuditLogger {
    constructor(pool) {
        this.pool = pool;
    }

    async init() {
        await this.pool.query(`
            CREATE TABLE IF NOT EXISTS audit_log (
                id SERIAL PRIMARY KEY,
                table_name VARCHAR(255),
                record_id INTEGER,
                action VARCHAR(50),
                old_data JSONB,
                new_data JSONB,
                user_id INTEGER,
                ip_address INET,
                user_agent TEXT,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )
        `);
    }

    async log(entry) {
        await this.pool.query(
            `INSERT INTO audit_log (table_name, record_id, action, old_data, new_data, user_id, ip_address, user_agent)
             VALUES ($1, $2, $3, $4, $5, $6, $7, $8)`,
            [entry.table, entry.recordId, entry.action, entry.oldData, entry.newData,
             entry.userId, entry.ipAddress, entry.userAgent]
        );
    }

    middleware(table) {
        return async (req, res, next) => {
            const originalEnd = res.end;
            res.end = function (...args) {
                if (res.statusCode >= 200 && res.statusCode < 300) {
                    auditLogger.log({
                        table,
                        recordId: req.params.id,
                        action: req.method,
                        newData: req.body,
                        userId: req.user?.id,
                        ipAddress: req.ip,
                        userAgent: req.headers['user-agent'],
                    });
                }
                originalEnd.apply(res, args);
            };
            next();
        };
    }
}
```

## Best Practices Checklist

- [ ] Use separate database users per environment
- [ ] Apply principle of least privilege
- [ ] Enable row-level security for multi-tenant apps
- [ ] Log all sensitive data access
- [ ] Use application-level authorization checks
- [ ] Regularly audit database permissions
- [ ] Rotate database passwords periodically

## Cross-References

- See [Connection Security](./01-connection-security.md) for connection encryption
- See [Query Injection](./03-query-injection.md) for injection prevention
- See [Encryption](./04-encryption.md) for data encryption

## Next Steps

Continue to [Query Injection Prevention](./03-query-injection.md) for SQL injection protection.
