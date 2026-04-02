# CSRF Protection, Session Hijacking Prevention, and Brute Force Mitigation

## What You'll Learn

- CSRF protection implementation
- Session hijacking prevention
- Brute force attack mitigation
- Timing attack prevention

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
        path: '/',
    },
    size: 64,
    getSessionIdentifier: (req) => req.sessionID,
});

// Generate token for forms/API
app.get('/api/csrf-token', (req, res) => {
    const token = generateCsrfToken(req, res);
    res.json({ csrfToken: token });
});

// Verify on state-changing requests
app.post('/api/users',
    doubleCsrfProtection,
    async (req, res) => {
        // CSRF verified automatically
        const user = await createUser(req.body);
        res.status(201).json(user);
    }
);

app.put('/api/users/:id',
    doubleCsrfProtection,
    async (req, res) => {
        const user = await updateUser(req.params.id, req.body);
        res.json(user);
    }
);

// For SPAs: include CSRF token in requests
// Frontend:
// const { csrfToken } = await fetch('/api/csrf-token').then(r => r.json());
// fetch('/api/users', {
//     method: 'POST',
//     headers: {
//         'Content-Type': 'application/json',
//         'X-CSRF-Token': csrfToken,
//     },
//     body: JSON.stringify(data),
// });
```

## Session Hijacking Prevention

```javascript
import session from 'express-session';
import RedisStore from 'connect-redis';
import { createClient } from 'redis';

const redisClient = createClient({ url: process.env.REDIS_URL });
await redisClient.connect();

// Secure session configuration
app.use(session({
    store: new RedisStore({
        client: redisClient,
        prefix: 'sess:',
        ttl: 1800, // 30 minutes
    }),
    secret: process.env.SESSION_SECRET,
    name: '__sid', // Don't use default 'connect.sid'
    resave: false,
    saveUninitialized: false,
    rolling: true, // Reset expiry on each request
    cookie: {
        httpOnly: true,
        secure: true,
        sameSite: 'strict',
        maxAge: 30 * 60 * 1000,
        domain: '.example.com',
        path: '/',
    },
}));

// Regenerate session ID on privilege escalation
app.post('/auth/login', async (req, res) => {
    const user = await authenticateUser(req.body);

    // CRITICAL: Regenerate to prevent session fixation
    req.session.regenerate((err) => {
        if (err) return res.status(500).json({ error: 'Session error' });

        req.session.userId = user.id;
        req.session.role = user.role;

        // Store device fingerprint for anomaly detection
        req.session.fingerprint = {
            userAgent: req.headers['user-agent'],
            ip: req.ip,
            createdAt: Date.now(),
        };

        res.json({ success: true });
    });
});

// Detect session anomalies
app.use((req, res, next) => {
    if (req.session.userId && req.session.fingerprint) {
        const fp = req.session.fingerprint;

        // Check user agent hasn't changed
        if (fp.userAgent !== req.headers['user-agent']) {
            console.warn('Session anomaly: user agent changed');
            req.session.destroy();
            return res.status(401).json({ error: 'Session invalidated' });
        }

        // Optional: Check IP hasn't changed too drastically
        // (be careful with mobile users on different networks)
    }
    next();
});

// Session timeout on inactivity
app.use((req, res, next) => {
    if (req.session.lastActivity) {
        const inactive = Date.now() - req.session.lastActivity;
        if (inactive > 30 * 60 * 1000) { // 30 minutes
            req.session.destroy();
            return res.status(401).json({ error: 'Session expired' });
        }
    }
    req.session.lastActivity = Date.now();
    next();
});
```

## Brute Force Protection

```javascript
import rateLimit from 'express-rate-limit';
import RedisStore from 'rate-limit-redis';

// IP-based rate limiting
const loginLimiter = rateLimit({
    store: new RedisStore({
        sendCommand: (...args) => redisClient.sendCommand(args),
        prefix: 'rl:login:',
    }),
    windowMs: 15 * 60 * 1000, // 15 minutes
    max: 5, // 5 attempts per IP
    standardHeaders: true,
    legacyHeaders: false,
    keyGenerator: (req) => req.ip,
    handler: (req, res) => {
        res.status(429).json({
            error: 'Too many login attempts',
            retryAfter: Math.ceil(req.rateLimit.resetTime / 1000),
        });
    },
});

// Account-based lockout
class AccountLocker {
    constructor(redis) {
        this.redis = redis;
    }

    async checkLockout(email) {
        const locked = await this.redis.get(`lockout:${email}`);
        if (locked) {
            const ttl = await this.redis.ttl(`lockout:${email}`);
            throw new Error(`Account locked. Try again in ${Math.ceil(ttl / 60)} minutes`);
        }
    }

    async recordFailure(email) {
        const key = `failures:${email}`;
        const count = await this.redis.incr(key);

        if (count === 1) await this.redis.expire(key, 900); // 15 min window

        if (count >= 5) {
            await this.redis.set(`lockout:${email}`, '1', { EX: 900 });
            await this.redis.del(key);
            throw new Error('Account locked for 15 minutes');
        }

        return { remaining: 5 - count };
    }

    async clearFailures(email) {
        await this.redis.del(`failures:${email}`);
    }
}

const locker = new AccountLocker(redisClient);

app.post('/auth/login', loginLimiter, async (req, res) => {
    const { email, password } = req.body;

    try {
        await locker.checkLockout(email);

        const user = await User.findByEmail(email);
        if (!user || !await bcrypt.compare(password, user.passwordHash)) {
            const { remaining } = await locker.recordFailure(email);
            return res.status(401).json({
                error: 'Invalid credentials',
                attemptsRemaining: remaining,
            });
        }

        await locker.clearFailures(email);
        const token = generateToken(user);
        res.json({ token });
    } catch (err) {
        res.status(429).json({ error: err.message });
    }
});
```

## Timing Attack Prevention

```javascript
import { timingSafeEqual } from 'node:crypto';

// BAD: Leaks timing information
async function unsafeCompare(input, stored) {
    if (input.length !== stored.length) return false; // Timing leak!
    for (let i = 0; i < input.length; i++) {
        if (input[i] !== stored[i]) return false; // Timing leak!
    }
    return true;
}

// GOOD: Constant-time comparison
function safeCompare(a, b) {
    const bufA = Buffer.from(a);
    const bufB = Buffer.from(b);

    if (bufA.length !== bufB.length) {
        // Compare against dummy to maintain constant time
        return timingSafeEqual(bufA, Buffer.alloc(bufA.length, 0));
    }

    return timingSafeEqual(bufA, bufB);
}

// Login with timing-safe comparison
app.post('/auth/login', async (req, res) => {
    const { email, password } = req.body;
    const user = await User.findByEmail(email);

    // bcrypt.compare is already timing-safe
    if (user && await bcrypt.compare(password, user.passwordHash)) {
        res.json({ token: generateToken(user) });
    } else {
        // Use same response time for valid and invalid users
        if (!user) {
            await bcrypt.compare(password, DUMMY_HASH);
        }
        res.status(401).json({ error: 'Invalid credentials' });
    }
});
```

## Common Mistakes

- Not regenerating session ID on login (session fixation)
- Using default session cookie name (easy to target)
- Not implementing account lockout (brute force vulnerable)
- Early return on password comparison (timing leak)

## Cross-References

- See [Security Headers](./01-security-headers.md) for HTTP headers
- See [Testing](../07-authentication-testing/01-unit-testing.md) for security tests
- See [Monitoring](../10-authentication-monitoring/01-monitoring-metrics.md) for threat detection

## Next Steps

Continue to [Testing: Security and CI/CD](../07-authentication-testing/02-security-ci-testing.md).
