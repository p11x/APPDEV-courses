# Authentication Security Implementation

## What You'll Learn

- Security headers for authentication
- CSRF protection implementation
- Session hijacking prevention
- Rate limiting and brute force protection
- Timing attack prevention

## Security Headers

```javascript
import helmet from 'helmet';

app.use(helmet());

// Authentication-specific headers
app.use((req, res, next) => {
    // Prevent clickjacking
    res.setHeader('X-Frame-Options', 'DENY');

    // Prevent MIME type sniffing
    res.setHeader('X-Content-Type-Options', 'nosniff');

    // Enable XSS filter
    res.setHeader('X-XSS-Protection', '1; mode=block');

    // Strict transport security
    res.setHeader('Strict-Transport-Security', 'max-age=31536000; includeSubDomains');

    // Content security policy
    res.setHeader('Content-Security-Policy', "default-src 'self'");

    // Referrer policy
    res.setHeader('Referrer-Policy', 'strict-origin-when-cross-origin');

    // Permissions policy
    res.setHeader('Permissions-Policy', 'camera=(), microphone=(), geolocation=()');

    next();
});
```

## CSRF Protection

```javascript
import { doubleCsrf } from 'csrf-csrf';

const { generateCsrfToken, doubleCsrfProtection } = doubleCsrf({
    getSecret: () => process.env.CSRF_SECRET,
    cookieName: '__csrf',
    cookieOptions: {
        httpOnly: true,
        sameSite: 'strict',
        secure: process.env.NODE_ENV === 'production',
    },
    size: 64,
    getSessionIdentifier: (req) => req.sessionID,
});

// Generate token for forms
app.get('/login', (req, res) => {
    const csrfToken = generateCsrfToken(req, res);
    res.render('login', { csrfToken });
});

// Verify on POST
app.post('/login', doubleCsrfProtection, async (req, res) => {
    // CSRF verified automatically
    const { email, password } = req.body;
    // ... authentication logic
});

// For SPA: provide CSRF token via API
app.get('/api/csrf-token', (req, res) => {
    const token = generateCsrfToken(req, res);
    res.json({ csrfToken: token });
});
```

## Session Hijacking Prevention

```javascript
import session from 'express-session';
import { createClient } from 'redis';
import RedisStore from 'connect-redis';

const redisClient = createClient({ url: process.env.REDIS_URL });
await redisClient.connect();

app.use(session({
    store: new RedisStore({ client: redisClient, prefix: 'sess:' }),
    secret: process.env.SESSION_SECRET,
    name: '__sid', // Don't use default 'connect.sid'
    resave: false,
    saveUninitialized: false,
    rolling: true, // Reset expiry on each request
    cookie: {
        httpOnly: true,       // No JavaScript access
        secure: true,         // HTTPS only
        sameSite: 'strict',   // No cross-site sending
        maxAge: 30 * 60 * 1000, // 30 minutes
        domain: '.example.com',
        path: '/',
    },
}));

// Regenerate session ID on privilege escalation
app.post('/login', async (req, res) => {
    const user = await authenticateUser(req.body);

    // Regenerate session to prevent fixation
    req.session.regenerate((err) => {
        if (err) return res.status(500).json({ error: 'Session error' });

        req.session.userId = user.id;
        req.session.userAgent = req.headers['user-agent'];
        req.session.ip = req.ip;

        res.json({ success: true });
    });
});

// Detect session anomalies
app.use((req, res, next) => {
    if (req.session.userId) {
        if (req.session.userAgent !== req.headers['user-agent']) {
            req.session.destroy();
            return res.status(401).json({ error: 'Session invalidated' });
        }
    }
    next();
});
```

## Rate Limiting

```javascript
import rateLimit from 'express-rate-limit';
import RedisStore from 'rate-limit-redis';

// Login rate limiting
const loginLimiter = rateLimit({
    store: new RedisStore({
        sendCommand: (...args) => redisClient.sendCommand(args),
        prefix: 'rl:login:',
    }),
    windowMs: 15 * 60 * 1000, // 15 minutes
    max: 5, // 5 attempts per window
    standardHeaders: true,
    legacyHeaders: false,
    keyGenerator: (req) => req.ip,
    handler: (req, res) => {
        res.status(429).json({
            error: 'Too many login attempts. Please try again later.',
            retryAfter: Math.ceil(req.rateLimit.resetTime / 1000),
        });
    },
});

app.post('/auth/login', loginLimiter, async (req, res) => {
    // Login logic
});

// API rate limiting
const apiLimiter = rateLimit({
    windowMs: 60 * 1000, // 1 minute
    max: 100,
    keyGenerator: (req) => req.user?.id || req.ip,
});

app.use('/api', apiLimiter);
```

## Timing Attack Prevention

```javascript
import { timingSafeEqual } from 'node:crypto';

// BAD: Early return leaks timing information
async function badCompare(input, stored) {
    if (input.length !== stored.length) return false; // Timing leak
    for (let i = 0; i < input.length; i++) {
        if (input[i] !== stored[i]) return false; // Timing leak
    }
    return true;
}

// GOOD: Constant-time comparison
function safeCompare(a, b) {
    const bufA = Buffer.from(a);
    const bufB = Buffer.from(b);

    if (bufA.length !== bufB.length) {
        // Compare against dummy to maintain constant time
        return timingSafeEqual(bufA, Buffer.alloc(bufA.length));
    }

    return timingSafeEqual(bufA, bufB);
}

// Login with timing-safe comparison
app.post('/login', async (req, res) => {
    const { email, password } = req.body;

    const user = await db.users.findByEmail(email);

    // Use bcrypt.compare — it's already timing-safe
    if (user && await bcrypt.compare(password, user.passwordHash)) {
        // Success
    } else {
        // Use same response time for valid and invalid users
        if (!user) {
            await bcrypt.compare(password, '$2b$12$dummyhashfordummycomparison');
        }
        res.status(401).json({ error: 'Invalid credentials' });
    }
});
```

## Best Practices Checklist

- [ ] Set all security headers (HSTS, CSP, X-Frame-Options)
- [ ] Implement CSRF protection for state-changing operations
- [ ] Use httpOnly, secure, sameSite cookies
- [ ] Regenerate session ID on login
- [ ] Implement rate limiting on all auth endpoints
- [ ] Use timing-safe comparison for secrets
- [ ] Detect and prevent session fixation attacks
- [ ] Log all authentication security events

## Cross-References

- See [Testing](../07-authentication-testing/01-unit-testing.md) for security testing
- See [Monitoring](../10-authentication-monitoring/01-monitoring-metrics.md) for threat detection
- See [Deployment](../11-authentication-deployment/01-production-deployment.md) for production hardening

## Next Steps

Continue to [Authentication Testing](../07-authentication-testing/01-unit-testing.md).
