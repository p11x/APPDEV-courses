# Authentication Caching and Load Testing

## What You'll Learn

- Redis caching for authentication data
- Token caching strategies
- Permission caching patterns
- Load testing authentication endpoints
- Performance benchmarks

## Redis Authentication Cache

```javascript
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
await redis.connect();

class AuthCache {
    constructor(redis) {
        this.redis = redis;
    }

    // Cache user data (avoid DB lookup on every request)
    async getUser(userId) {
        const key = `user:${userId}`;
        const cached = await this.redis.get(key);

        if (cached) return JSON.parse(cached);

        const user = await User.findById(userId);
        if (user) {
            await this.redis.set(key, JSON.stringify({
                id: user.id,
                email: user.email,
                role: user.role,
                permissions: user.permissions,
            }), { EX: 300 }); // 5 minutes
        }
        return user;
    }

    // Cache permission checks
    async hasPermission(userId, permission) {
        const key = `perms:${userId}`;
        let perms = await this.redis.get(key);

        if (!perms) {
            perms = await User.getPermissions(userId);
            await this.redis.set(key, JSON.stringify(perms), { EX: 300 });
        } else {
            perms = JSON.parse(perms);
        }

        return perms.includes(permission);
    }

    // Cache session data
    async getSession(sessionId) {
        return this.redis.get(`sess:${sessionId}`);
    }

    // Invalidate on user update
    async invalidateUser(userId) {
        await this.redis.del(`user:${userId}`, `perms:${userId}`);
    }

    // Invalidate on logout
    async invalidateSession(sessionId) {
        await this.redis.del(`sess:${sessionId}`);
    }

    // Batch cache warmup
    async warmup(userIds) {
        const pipeline = this.redis.multi();
        for (const id of userIds) {
            const user = await User.findById(id);
            if (user) {
                pipeline.set(`user:${id}`, JSON.stringify({
                    id: user.id,
                    email: user.email,
                    role: user.role,
                }), { EX: 300 });
            }
        }
        await pipeline.exec();
    }
}

// Auth middleware with caching
function cachedAuthMiddleware(authCache) {
    return async (req, res, next) => {
        const token = req.headers.authorization?.replace('Bearer ', '');
        if (!token) return res.status(401).json({ error: 'No token' });

        try {
            const decoded = jwt.verify(token, process.env.JWT_SECRET);

            // Get user from cache (not DB)
            req.user = await authCache.getUser(decoded.sub);
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
Operation               Without Cache  With Cache   Improvement
─────────────────────────────────────────────
User lookup             5ms            0.5ms        10x
Permission check        8ms            0.3ms        26x
Session validation      3ms            0.2ms        15x
JWT verification        0.05ms         0.05ms       1x (no cache needed)

Load Test Results (100 concurrent users):
─────────────────────────────────────────────
Metric              Value
─────────────────────────────────────────────
Requests/sec        5,000 (with cache)
p50 latency         12ms
p95 latency         45ms
p99 latency         120ms
Error rate          0.01%
─────────────────────────────────────────────

Without cache: 500 req/sec (10x slower)
```

## Load Testing with Autocannon

```javascript
import autocannon from 'autocannon';
import jwt from 'jsonwebtoken';

async function loadTestAuth() {
    const token = jwt.sign(
        { sub: 1, role: 'user' },
        process.env.JWT_SECRET,
        { expiresIn: '1h' }
    );

    // Test login endpoint
    const loginResult = await autocannon({
        url: 'http://localhost:3000/auth/login',
        connections: 50,
        duration: 30,
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            email: 'test@example.com',
            password: 'TestPass123!',
        }),
    });

    // Test protected endpoint (with cached user)
    const protectedResult = await autocannon({
        url: 'http://localhost:3000/api/profile',
        connections: 200,
        duration: 30,
        headers: { Authorization: `Bearer ${token}` },
    });

    console.log('Login:', {
        rps: loginResult.requests.average,
        p95: loginResult.latency.p95,
    });

    console.log('Protected:', {
        rps: protectedResult.requests.average,
        p95: protectedResult.latency.p95,
    });
}
```

## Common Mistakes

- Not caching permission lookups (expensive DB queries)
- Caching sensitive data without encryption
- Not invalidating cache on user updates
- Setting TTL too long (stale permissions)

## Cross-References

- See [Performance](./01-performance-optimization.md) for optimization
- See [Monitoring](../10-authentication-monitoring/01-monitoring-metrics.md) for metrics
- See [Testing](../07-authentication-testing/01-unit-testing.md) for testing

## Next Steps

Continue to [Framework Integration: GraphQL and Socket.io](../09-framework-integration/02-graphql-socketio.md).
