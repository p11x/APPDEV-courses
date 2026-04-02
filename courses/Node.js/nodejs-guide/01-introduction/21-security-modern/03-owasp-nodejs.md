# OWASP Top 10 for Node.js Applications

## What You'll Learn

- OWASP Top 10 vulnerabilities in Node.js context
- Prevention strategies and code examples
- Security testing approaches
- Compliance checklist

## OWASP Top 10 Mapping

### A01: Broken Access Control

```javascript
// VULNERABLE: No authorization check
app.get('/api/users/:id', async (req, res) => {
    const user = await db.getUser(req.params.id);
    res.json(user); // Any user can access any profile!
});

// SECURE: Check authorization
app.get('/api/users/:id', authenticate, async (req, res) => {
    if (req.user.id !== req.params.id && req.user.role !== 'admin') {
        return res.status(403).json({ error: 'Forbidden' });
    }
    const user = await db.getUser(req.params.id);
    res.json(user);
});
```

### A02: Cryptographic Failures

```javascript
// VULNERABLE: Weak hashing
const hash = crypto.createHash('md5').update(password).digest('hex');

// SECURE: Strong hashing with bcrypt
import bcrypt from 'bcrypt';
const hash = await bcrypt.hash(password, 12);
const valid = await bcrypt.compare(password, hash);

// SECURE: Use crypto for random values
const token = crypto.randomBytes(32).toString('hex');
```

### A03: Injection

```javascript
// VULNERABLE: SQL injection
const users = await db.query(`SELECT * FROM users WHERE id = '${userId}'`);

// SECURE: Parameterized queries
const users = await db.query('SELECT * FROM users WHERE id = $1', [userId]);

// SECURE: Using ORM
const user = await prisma.user.findUnique({ where: { id: userId } });
```

### A04: Insecure Design

```javascript
// VULNERABLE: No rate limiting on password reset
app.post('/api/reset-password', async (req, res) => {
    await sendResetEmail(req.body.email);
    res.json({ message: 'Email sent' });
});

// SECURE: Rate limited + token-based
app.post('/api/reset-password',
    rateLimit({ capacity: 3, refillRate: 0.01 }),
    async (req, res) => {
        const token = crypto.randomBytes(32).toString('hex');
        await saveResetToken(req.body.email, token, Date.now() + 3600000);
        await sendResetEmail(req.body.email, token);
        res.json({ message: 'If email exists, reset link sent' });
    }
);
```

### A05: Security Misconfiguration

```javascript
// VULNERABLE: Stack traces in production
app.use((err, req, res, next) => {
    res.status(500).json({ error: err.message, stack: err.stack });
});

// SECURE: Hide stack traces
app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(500).json({
        error: process.env.NODE_ENV === 'production'
            ? 'Internal server error'
            : err.message,
    });
});
```

### A06: Vulnerable Components

```bash
# Regularly audit dependencies
npm audit

# Keep dependencies updated
npm outdated
npm update

# Use lock files
git add package-lock.json
```

### A07: Identification and Authentication Failures

```javascript
// SECURE: Account lockout
const loginAttempts = new Map();

app.post('/api/login', async (req, res) => {
    const key = req.ip + req.body.email;
    const attempts = loginAttempts.get(key) || { count: 0, lockUntil: 0 };
    
    if (Date.now() < attempts.lockUntil) {
        return res.status(429).json({ error: 'Account locked. Try later.' });
    }
    
    const user = await authenticateUser(req.body.email, req.body.password);
    
    if (!user) {
        attempts.count++;
        if (attempts.count >= 5) {
            attempts.lockUntil = Date.now() + 900000; // 15 min lock
        }
        loginAttempts.set(key, attempts);
        return res.status(401).json({ error: 'Invalid credentials' });
    }
    
    loginAttempts.delete(key);
    const tokens = generateTokens(user);
    res.json(tokens);
});
```

### A08: Software and Data Integrity Failures

```javascript
// SECURE: Verify webhook signatures
app.post('/webhook', express.raw({ type: 'application/json' }), (req, res) => {
    const signature = req.headers['x-hub-signature-256'];
    const expected = 'sha256=' + crypto
        .createHmac('sha256', WEBHOOK_SECRET)
        .update(req.body)
        .digest('hex');
    
    if (!crypto.timingSafeEqual(Buffer.from(signature), Buffer.from(expected))) {
        return res.status(401).json({ error: 'Invalid signature' });
    }
    
    // Process webhook...
    res.json({ received: true });
});
```

### A09: Security Logging Failures

```javascript
// SECURE: Structured security logging
import pino from 'pino';
const logger = pino();

function logSecurityEvent(event, req, details = {}) {
    logger.warn({
        event,
        ip: req.ip,
        method: req.method,
        path: req.path,
        userAgent: req.headers['user-agent'],
        userId: req.user?.id,
        timestamp: new Date().toISOString(),
        ...details,
    });
}

// Usage
app.post('/api/login', async (req, res) => {
    if (!user) {
        logSecurityEvent('LOGIN_FAILED', req, { email: req.body.email });
    }
});
```

### A10: Server-Side Request Forgery (SSRF)

```javascript
// VULNERABLE: Unvalidated URL
app.post('/api/fetch', async (req, res) => {
    const response = await fetch(req.body.url); // Attacker sends internal URL!
    res.send(await response.text());
});

// SECURE: Allowlist domains
const ALLOWED_DOMAINS = ['api.example.com', 'cdn.example.com'];

app.post('/api/fetch', async (req, res) => {
    const url = new URL(req.body.url);
    
    if (!ALLOWED_DOMAINS.includes(url.hostname)) {
        return res.status(400).json({ error: 'Domain not allowed' });
    }
    
    if (['127.0.0.1', 'localhost', '0.0.0.0'].includes(url.hostname)) {
        return res.status(400).json({ error: 'Internal addresses not allowed' });
    }
    
    const response = await fetch(url.toString());
    res.send(await response.text());
});
```

## Security Audit Checklist

```
Node.js Security Audit:
─────────────────────────────────────────────
□ Input Validation
  □ All user input validated
  □ SQL queries parameterized
  □ No eval() or Function() with user input
  □ File paths sanitized

□ Authentication
  □ Strong password hashing (bcrypt, argon2)
  □ Rate limiting on auth endpoints
  □ Account lockout implemented
  □ JWT secrets from environment

□ Authorization
  □ Access control on all endpoints
  □ Horizontal privilege escalation prevented
  □ Admin routes protected

□ Dependencies
  □ npm audit clean
  □ No known vulnerabilities
  □ Dependencies up to date
  □ Lock file committed

□ Configuration
  □ Helmet.js configured
  □ CORS restricted to specific origins
  □ Error messages don't leak info
  □ Debug mode disabled in production

□ Logging
  □ Security events logged
  □ No secrets in logs
  □ Log aggregation configured
```

## Best Practices Checklist

- [ ] Follow OWASP Top 10 for all endpoints
- [ ] Use parameterized queries or ORMs
- [ ] Validate all input at every boundary
- [ ] Implement proper error handling
- [ ] Run security audits in CI/CD
- [ ] Keep all dependencies updated
- [ ] Use HTTPS everywhere
- [ ] Implement proper CORS configuration

## Cross-References

- See [Security Headers](./01-security-headers-deps.md) for HTTP headers
- See [Auth and Rate Limiting](./02-auth-rate-limiting.md) for authentication
- See [Security](../../../19-security-rate-limiting/) for comprehensive security guide

## Next Steps

Continue to [Node.js Roadmap](../22-roadmap-future/01-upcoming-features.md) for future features.
