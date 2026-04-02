# Authentication Monitoring and Observability

## What You'll Learn

- Authentication metrics and KPIs
- Authentication logging and audit trails
- Security monitoring and threat detection
- User behavior analytics
- Incident response patterns

## Authentication Metrics

```javascript
import { Counter, Histogram, Gauge } from 'prom-client';

// Authentication metrics
const authAttempts = new Counter({
    name: 'auth_attempts_total',
    help: 'Total authentication attempts',
    labelNames: ['method', 'result'], // method: login|token_refresh, result: success|failure
});

const authDuration = new Histogram({
    name: 'auth_duration_seconds',
    help: 'Authentication operation duration',
    labelNames: ['operation'], // operation: login|verify|refresh
    buckets: [0.01, 0.05, 0.1, 0.5, 1, 2, 5],
});

const activeSessions = new Gauge({
    name: 'auth_active_sessions',
    help: 'Number of active sessions',
});

const tokenRefreshRate = new Counter({
    name: 'auth_token_refresh_total',
    help: 'Token refresh operations',
    labelNames: ['result'],
});

const failedLogins = new Counter({
    name: 'auth_failed_logins_total',
    help: 'Failed login attempts',
    labelNames: ['reason'], // reason: invalid_password|user_not_found|account_locked
});

// Usage in auth handlers
app.post('/auth/login', async (req, res) => {
    const start = performance.now();

    try {
        const user = await authenticateUser(req.body);
        authAttempts.inc({ method: 'login', result: 'success' });
        authDuration.observe({ operation: 'login' }, (performance.now() - start) / 1000);

        res.json({ token: generateToken(user) });
    } catch (err) {
        authAttempts.inc({ method: 'login', result: 'failure' });
        failedLogins.inc({ reason: err.reason || 'unknown' });
        res.status(401).json({ error: err.message });
    }
});
```

## Audit Logging

```javascript
class AuthAuditLogger {
    constructor(pool) {
        this.pool = pool;
    }

    async init() {
        await this.pool.query(`
            CREATE TABLE IF NOT EXISTS auth_audit_log (
                id BIGSERIAL PRIMARY KEY,
                event_type VARCHAR(50) NOT NULL,
                user_id INTEGER,
                email VARCHAR(255),
                ip_address INET,
                user_agent TEXT,
                success BOOLEAN,
                failure_reason VARCHAR(255),
                metadata JSONB,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX idx_auth_audit_user ON auth_audit_log(user_id, created_at);
            CREATE INDEX idx_auth_audit_type ON auth_audit_log(event_type, created_at);
            CREATE INDEX idx_auth_audit_ip ON auth_audit_log(ip_address, created_at);
        `);
    }

    async log(event) {
        await this.pool.query(
            `INSERT INTO auth_audit_log
             (event_type, user_id, email, ip_address, user_agent, success, failure_reason, metadata)
             VALUES ($1, $2, $3, $4, $5, $6, $7, $8)`,
            [
                event.type, event.userId, event.email,
                event.ip, event.userAgent,
                event.success, event.failureReason,
                JSON.stringify(event.metadata || {}),
            ]
        );
    }

    async getLoginHistory(userId, limit = 50) {
        const { rows } = await this.pool.query(
            `SELECT * FROM auth_audit_log
             WHERE user_id = $1 AND event_type = 'login'
             ORDER BY created_at DESC LIMIT $2`,
            [userId, limit]
        );
        return rows;
    }

    async getSuspiciousActivity(ipAddress, hours = 24) {
        const { rows } = await this.pool.query(
            `SELECT event_type, COUNT(*) as count
             FROM auth_audit_log
             WHERE ip_address = $1
               AND created_at > NOW() - INTERVAL '${hours} hours'
               AND success = false
             GROUP BY event_type
             HAVING COUNT(*) > 10`,
            [ipAddress]
        );
        return rows;
    }
}

// Middleware
function auditMiddleware(auditLogger) {
    return (req, res, next) => {
        const originalEnd = res.end;

        res.end = function (...args) {
            const event = {
                type: req.path.includes('login') ? 'login' :
                    req.path.includes('refresh') ? 'token_refresh' : 'auth_check',
                userId: req.user?.id,
                email: req.body?.email,
                ip: req.ip,
                userAgent: req.headers['user-agent'],
                success: res.statusCode < 400,
                failureReason: res.statusCode >= 400 ? res.statusMessage : null,
            };

            auditLogger.log(event).catch(console.error);
            originalEnd.apply(res, args);
        };

        next();
    };
}
```

## Threat Detection

```javascript
class AuthThreatDetector {
    constructor(redis, auditLogger) {
        this.redis = redis;
        this.audit = auditLogger;
    }

    async detectBruteForce(ip) {
        const key = `brute:${ip}`;
        const attempts = await this.redis.incr(key);
        if (attempts === 1) await this.redis.expire(key, 900); // 15 min window

        return {
            detected: attempts > 10,
            attempts,
            action: attempts > 20 ? 'block' : attempts > 10 ? 'warn' : 'allow',
        };
    }

    async detectCredentialStuffing(email, ip) {
        const ipKey = `stuffing:ip:${ip}`;
        const emailKey = `stuffing:email:${email}`;

        const ipAttempts = await this.redis.incr(ipKey);
        const emailAttempts = await this.redis.incr(emailKey);

        if (ipAttempts === 1) await this.redis.expire(ipKey, 3600);
        if (emailAttempts === 1) await this.redis.expire(emailKey, 3600);

        return {
            detected: ipAttempts > 50 || emailAttempts > 5,
            ipAttempts,
            emailAttempts,
        };
    }

    async detectAccountTakeover(userId, ip, userAgent) {
        const key = `ato:${userId}`;
        const lastLogin = await this.redis.get(key);

        if (lastLogin) {
            const prev = JSON.parse(lastLogin);
            if (prev.ip !== ip && prev.userAgent !== userAgent) {
                return {
                    detected: true,
                    previous: prev,
                    current: { ip, userAgent },
                };
            }
        }

        await this.redis.set(key, JSON.stringify({ ip, userAgent, time: Date.now() }), { EX: 86400 });
        return { detected: false };
    }
}
```

## Best Practices Checklist

- [ ] Track authentication success/failure rates
- [ ] Log all authentication events with context
- [ ] Monitor failed login spikes
- [ ] Detect brute force and credential stuffing
- [ ] Alert on account takeover indicators
- [ ] Expose /metrics endpoint for Prometheus
- [ ] Set up dashboards for auth KPIs
- [ ] Retain audit logs for compliance

## Cross-References

- See [Security](../06-authentication-security/01-security-headers.md) for hardening
- See [Deployment](../11-authentication-deployment/01-production-deployment.md) for production setup
- See [Testing](../07-authentication-testing/01-unit-testing.md) for testing

## Next Steps

Continue to [Deployment and Operations](../11-authentication-deployment/01-production-deployment.md).
