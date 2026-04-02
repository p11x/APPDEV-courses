# Authentication Audit Trail and Threat Detection

## What You'll Learn

- Comprehensive audit logging
- Threat detection patterns
- User behavior analytics
- Security incident response

## Audit Trail Implementation

```javascript
class AuthAuditService {
    constructor(pool, redis) {
        this.pool = pool;
        this.redis = redis;
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
                risk_score INTEGER DEFAULT 0,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX idx_audit_user ON auth_audit_log(user_id, created_at);
            CREATE INDEX idx_audit_ip ON auth_audit_log(ip_address, created_at);
            CREATE INDEX idx_audit_type ON auth_audit_log(event_type, created_at);
            CREATE INDEX idx_audit_risk ON auth_audit_log(risk_score, created_at);
        `);
    }

    async log(event) {
        await this.pool.query(
            `INSERT INTO auth_audit_log 
             (event_type, user_id, email, ip_address, user_agent, 
              success, failure_reason, metadata, risk_score)
             VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)`,
            [
                event.type, event.userId, event.email,
                event.ip, event.userAgent,
                event.success, event.failureReason,
                JSON.stringify(event.metadata || {}),
                event.riskScore || 0,
            ]
        );
    }

    async getUserHistory(userId, limit = 50) {
        const { rows } = await this.pool.query(
            `SELECT * FROM auth_audit_log 
             WHERE user_id = $1 
             ORDER BY created_at DESC 
             LIMIT $2`,
            [userId, limit]
        );
        return rows;
    }

    async getSuspiciousActivity(hours = 24) {
        const { rows } = await this.pool.query(`
            SELECT 
                ip_address,
                event_type,
                COUNT(*) as event_count,
                COUNT(*) FILTER (WHERE NOT success) as failure_count,
                MAX(risk_score) as max_risk
            FROM auth_audit_log
            WHERE created_at > NOW() - INTERVAL '${hours} hours'
            GROUP BY ip_address, event_type
            HAVING COUNT(*) FILTER (WHERE NOT success) > 5
               OR MAX(risk_score) > 70
            ORDER BY failure_count DESC
        `);
        return rows;
    }
}
```

## Threat Detection

```javascript
class ThreatDetector {
    constructor(redis, auditService) {
        this.redis = redis;
        this.audit = auditService;
    }

    async detectBruteForce(ip) {
        const key = `brute:${ip}`;
        const attempts = await this.redis.incr(key);
        if (attempts === 1) await this.redis.expire(key, 900);

        return {
            detected: attempts > 10,
            attempts,
            action: attempts > 20 ? 'block' : attempts > 10 ? 'warn' : 'allow',
        };
    }

    async detectCredentialStuffing(email, ip) {
        const ipAttempts = await this.redis.incr(`stuffing:ip:${ip}`);
        const emailAttempts = await this.redis.incr(`stuffing:email:${email}`);

        if (ipAttempts === 1) await this.redis.expire(`stuffing:ip:${ip}`, 3600);
        if (emailAttempts === 1) await this.redis.expire(`stuffing:email:${email}`, 3600);

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

    async detectImpossibleTravel(userId, currentIp) {
        const key = `location:${userId}`;
        const lastLocation = await this.redis.get(key);

        if (!lastLocation) {
            await this.redis.set(key, JSON.stringify({ ip: currentIp, time: Date.now() }), { EX: 3600 });
            return { detected: false };
        }

        const prev = JSON.parse(lastLocation);
        const timeDiff = Date.now() - prev.time;

        // If same user in different IP within 1 hour
        if (prev.ip !== currentIp && timeDiff < 3600000) {
            return {
                detected: true,
                previous: prev,
                current: { ip: currentIp },
                timeDiffMinutes: Math.round(timeDiff / 60000),
            };
        }

        await this.redis.set(key, JSON.stringify({ ip: currentIp, time: Date.now() }), { EX: 3600 });
        return { detected: false };
    }
}
```

## Common Mistakes

- Not logging failed authentication attempts
- Not detecting impossible travel scenarios
- Not alerting on suspicious activity patterns
- Storing sensitive data in audit logs (passwords, tokens)

## Cross-References

- See [Monitoring](./01-monitoring-metrics.md) for metrics
- See [Security](../06-authentication-security/02-csrf-session-protection.md) for hardening
- See [Deployment](../11-authentication-deployment/01-production-deployment.md) for production

## Next Steps

Continue to [Deployment: Config and Health](../11-authentication-deployment/02-config-health.md).
