# Database Security Implementation

## What You'll Learn

- Database connection security
- SQL injection prevention
- Data encryption patterns
- Database access control

## SQL Injection Prevention

```javascript
// BAD: SQL injection vulnerability
const query = `SELECT * FROM users WHERE id = '${userId}'`;

// GOOD: Parameterized queries
const { rows } = await pool.query(
    'SELECT * FROM users WHERE id = $1',
    [userId]
);

// MongoDB injection prevention
// BAD
const user = await User.findOne({ $where: `this.name === '${name}'` });

// GOOD
const user = await User.findOne({ name });
```

## Connection Security

```javascript
// PostgreSQL SSL connection
const pool = new Pool({
    host: process.env.PG_HOST,
    port: 5432,
    database: process.env.PG_DATABASE,
    user: process.env.PG_USER,
    password: process.env.PG_PASSWORD,
    ssl: {
        rejectUnauthorized: true,
        ca: fs.readFileSync('/path/to/ca-cert.pem'),
        key: fs.readFileSync('/path/to/client-key.pem'),
        cert: fs.readFileSync('/path/to/client-cert.pem'),
    },
});

// MongoDB SSL connection
mongoose.connect(process.env.MONGODB_URL, {
    ssl: true,
    sslValidate: true,
    sslCA: '/path/to/ca-cert.pem',
});
```

## Data Encryption

```javascript
import { createCipheriv, createDecipheriv, randomBytes } from 'node:crypto';

const ALGORITHM = 'aes-256-gcm';
const KEY = Buffer.from(process.env.ENCRYPTION_KEY, 'hex');

function encrypt(text) {
    const iv = randomBytes(16);
    const cipher = createCipheriv(ALGORITHM, KEY, iv);
    let encrypted = cipher.update(text, 'utf8', 'hex');
    encrypted += cipher.final('hex');
    const authTag = cipher.getAuthTag();
    return `${iv.toString('hex')}:${authTag.toString('hex')}:${encrypted}`;
}

function decrypt(encrypted) {
    const [ivHex, authTagHex, encryptedText] = encrypted.split(':');
    const iv = Buffer.from(ivHex, 'hex');
    const authTag = Buffer.from(authTagHex, 'hex');
    const decipher = createDecipheriv(ALGORITHM, KEY, iv);
    decipher.setAuthTag(authTag);
    let decrypted = decipher.update(encryptedText, 'hex', 'utf8');
    decrypted += decipher.final('utf8');
    return decrypted;
}
```

## Best Practices Checklist

- [ ] Always use parameterized queries
- [ ] Use SSL for database connections
- [ ] Encrypt sensitive data at rest
- [ ] Implement database access control
- [ ] Audit database access logs

## Cross-References

- See [Integration Patterns](../01-database-integration-patterns/01-mongodb-postgres.md) for setup
- See [Performance](../02-database-performance-optimization/01-query-optimization.md) for queries
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for audit logs

## Next Steps

Continue to [Performance Testing](../08-performance-testing-benchmarking/01-load-testing.md).
