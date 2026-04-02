# Authentication Performance Optimization

## What You'll Learn

- Authentication caching strategies
- Token optimization techniques
- Database query optimization for auth
- Session optimization patterns
- Load testing and scalability

## Authentication Caching

```javascript
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
await redis.connect();

class AuthCache {
    constructor(redis) {
        this.redis = redis;
        this.userCacheTTL = 300; // 5 minutes
        this.tokenCacheTTL = 60; // 1 minute
    }

    // Cache user data to avoid DB lookups on every request
    async getCachedUser(userId) {
        const cached = await this.redis.get(`user:${userId}`);
        if (cached) return JSON.parse(cached);

        const user = await db.users.findById(userId);
        if (user) {
            await this.redis.set(`user:${userId}`, JSON.stringify({
                id: user.id,
                email: user.email,
                role: user.role,
                permissions: user.permissions,
            }), { EX: this.userCacheTTL });
        }
        return user;
    }

    // Cache permission lookups
    async getCachedPermissions(userId) {
        const cached = await this.redis.get(`perms:${userId}`);
        if (cached) return JSON.parse(cached);

        const perms = await db.permissions.findByUser(userId);
        await this.redis.set(`perms:${userId}`, JSON.stringify(perms), { EX: 300 });
        return perms;
    }

    // Invalidate on user update
    async invalidateUser(userId) {
        await this.redis.del(`user:${userId}`, `perms:${userId}`);
    }

    // Cache JWT denylist check
    async isTokenRevoked(tokenId) {
        return !!(await this.redis.get(`revoked:${tokenId}`));
    }

    async revokeToken(tokenId, ttl) {
        await this.redis.set(`revoked:${tokenId}`, '1', { EX: ttl });
    }
}

// Optimized auth middleware with caching
function optimizedAuthMiddleware(authCache, tokenService) {
    return async (req, res, next) => {
        const token = req.headers.authorization?.replace('Bearer ', '');
        if (!token) return res.status(401).json({ error: 'No token' });

        try {
            // Verify signature (fast, CPU-bound)
            const decoded = tokenService.verifyAccessToken(token);

            // Check revocation (cached in Redis)
            if (await authCache.isTokenRevoked(decoded.jti)) {
                return res.status(401).json({ error: 'Token revoked' });
            }

            // Get user from cache (avoids DB hit)
            req.user = await authCache.getCachedUser(decoded.sub);

            if (!req.user) {
                return res.status(401).json({ error: 'User not found' });
            }

            next();
        } catch (err) {
            res.status(401).json({ error: 'Invalid token' });
        }
    };
}
```

## Performance Benchmarks

```
Authentication Performance Benchmarks:
─────────────────────────────────────────────
Operation               Time        Notes
─────────────────────────────────────────────
bcrypt hash (10 rounds)  ~80ms      CPU-bound
bcrypt compare           ~80ms      CPU-bound
bcrypt hash (12 rounds)  ~320ms     More secure
JWT sign (HS256)         ~0.05ms    Very fast
JWT verify (HS256)       ~0.05ms    Very fast
JWT sign (RS256)         ~1.2ms     Asymmetric
JWT verify (RS256)       ~0.08ms    Fast with public key
Redis GET (cached user)  ~0.5ms     Much faster than DB
DB query (user lookup)   ~5ms       Depends on index
Session lookup (Redis)   ~0.5ms     Fast

Optimization impact:
├── No cache: 100 req/s per instance
├── Redis cache: 5,000 req/s per instance (50x improvement)
└── JWT (stateless): 50,000 req/s per instance
```

## Database Query Optimization

```javascript
// Optimized user lookup with index
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_active ON users(id) WHERE active = true;

// Single query for auth check
async function authenticateUser(email, password) {
    const { rows } = await pool.query(
        `SELECT id, email, password_hash, role, active
         FROM users
         WHERE email = $1 AND active = true`,
        [email]
    );

    if (rows.length === 0) return null;

    const valid = await bcrypt.compare(password, rows[0].password_hash);
    if (!valid) return null;

    return rows[0];
}

// Batch permission loading
async function loadPermissions(userIds) {
    const { rows } = await pool.query(
        `SELECT user_id, permission
         FROM user_permissions
         WHERE user_id = ANY($1)`,
        [userIds]
    );

    const permMap = {};
    for (const row of rows) {
        if (!permMap[row.user_id]) permMap[row.user_id] = [];
        permMap[row.user_id].push(row.permission);
    }
    return permMap;
}
```

## Session Optimization

```javascript
// Use Redis for session storage (fast, scalable)
import session from 'express-session';
import RedisStore from 'connect-redis';

app.use(session({
    store: new RedisStore({
        client: redisClient,
        prefix: 'sess:',
        ttl: 86400, // 1 day
    }),
    secret: process.env.SESSION_SECRET,
    resave: false,           // Don't save unchanged sessions
    saveUninitialized: false, // Don't save empty sessions
    cookie: {
        maxAge: 30 * 60 * 1000, // 30 minutes
        httpOnly: true,
        secure: true,
    },
}));

// Compress session data
app.use((req, res, next) => {
    // Only store essential data in session
    req.session.userId = req.user?.id;
    // Don't store entire user objects
    next();
});
```

## Load Testing Auth Endpoints

```javascript
import autocannon from 'autocannon';

async function loadTestAuth() {
    // Test login endpoint
    const loginResult = await autocannon({
        url: 'http://localhost:3000/auth/login',
        connections: 50,
        duration: 30,
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email: 'test@example.com', password: 'TestPass123!' }),
    });

    console.log('Login:', {
        avgLatency: loginResult.latency.average + 'ms',
        throughput: loginResult.requests.average + ' req/s',
        errors: loginResult.errors,
    });

    // Test token verification endpoint
    const token = await getTestToken();
    const verifyResult = await autocannon({
        url: 'http://localhost:3000/api/profile',
        connections: 200,
        duration: 30,
        headers: { Authorization: `Bearer ${token}` },
    });

    console.log('Verify:', {
        avgLatency: verifyResult.latency.average + 'ms',
        throughput: verifyResult.requests.average + ' req/s',
        errors: verifyResult.errors,
    });
}
```

## Best Practices Checklist

- [ ] Cache user data in Redis (5-minute TTL)
- [ ] Cache permission lookups
- [ ] Use JWT for stateless auth when possible
- [ ] Index email column for user lookups
- [ ] Use Redis for session storage
- [ ] Minimize session data size
- [ ] Load test auth endpoints regularly
- [ ] Monitor auth latency percentiles

## Cross-References

- See [Testing](../07-authentication-testing/01-unit-testing.md) for auth testing
- See [Monitoring](../10-authentication-monitoring/01-monitoring-metrics.md) for metrics
- See [Security](../06-authentication-security/01-security-headers.md) for hardening

## Next Steps

Continue to [Framework Integration](../09-framework-integration/01-express-fastify.md).
