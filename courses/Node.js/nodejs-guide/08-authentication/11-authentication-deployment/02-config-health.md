# Authentication Config Management and Health Checks

## What You'll Learn

- Environment configuration management
- Secrets management with vaults
- Health check endpoints
- Graceful shutdown for auth services

## Environment Configuration

```javascript
// config/auth.config.js
import { z } from 'zod';

const authConfigSchema = z.object({
    jwt: z.object({
        accessSecret: z.string().min(32),
        refreshSecret: z.string().min(32),
        accessExpiry: z.string().default('15m'),
        refreshExpiry: z.string().default('7d'),
        algorithm: z.enum(['HS256', 'RS256']).default('HS256'),
        issuer: z.string().default('myapp'),
    }),
    bcrypt: z.object({
        saltRounds: z.number().min(10).max(15).default(12),
    }),
    session: z.object({
        secret: z.string().min(32),
        maxAge: z.number().default(1800000), // 30 min
        secure: z.boolean().default(true),
    }),
    rateLimit: z.object({
        loginWindow: z.number().default(900000), // 15 min
        loginMax: z.number().default(5),
        apiWindow: z.number().default(60000), // 1 min
        apiMax: z.number().default(100),
    }),
    redis: z.object({
        url: z.string().url(),
    }),
});

export const authConfig = authConfigSchema.parse({
    jwt: {
        accessSecret: process.env.JWT_ACCESS_SECRET,
        refreshSecret: process.env.JWT_REFRESH_SECRET,
        accessExpiry: process.env.JWT_ACCESS_EXPIRY,
        refreshExpiry: process.env.JWT_REFRESH_EXPIRY,
        algorithm: process.env.JWT_ALGORITHM,
        issuer: process.env.JWT_ISSUER,
    },
    bcrypt: {
        saltRounds: parseInt(process.env.BCRYPT_ROUNDS),
    },
    session: {
        secret: process.env.SESSION_SECRET,
        maxAge: parseInt(process.env.SESSION_MAX_AGE),
        secure: process.env.NODE_ENV === 'production',
    },
    rateLimit: {
        loginWindow: parseInt(process.env.RATE_LIMIT_WINDOW),
        loginMax: parseInt(process.env.RATE_LIMIT_MAX),
    },
    redis: {
        url: process.env.REDIS_URL,
    },
});
```

## Health Check Endpoints

```javascript
import { Router } from 'express';

const healthRouter = Router();

// Basic health
healthRouter.get('/health', (req, res) => {
    res.json({ status: 'ok', timestamp: new Date().toISOString() });
});

// Detailed health (checks dependencies)
healthRouter.get('/health/detailed', async (req, res) => {
    const checks = {};

    // Database check
    try {
        await pool.query('SELECT 1');
        checks.database = { status: 'ok' };
    } catch (err) {
        checks.database = { status: 'error', message: err.message };
    }

    // Redis check
    try {
        await redis.ping();
        checks.redis = { status: 'ok' };
    } catch (err) {
        checks.redis = { status: 'error', message: err.message };
    }

    // JWT signing check
    try {
        const testToken = jwt.sign({ test: true }, process.env.JWT_ACCESS_SECRET, { expiresIn: '1s' });
        jwt.verify(testToken, process.env.JWT_ACCESS_SECRET);
        checks.jwt = { status: 'ok' };
    } catch (err) {
        checks.jwt = { status: 'error', message: err.message };
    }

    const healthy = Object.values(checks).every(c => c.status === 'ok');

    res.status(healthy ? 200 : 503).json({
        status: healthy ? 'healthy' : 'degraded',
        checks,
        timestamp: new Date().toISOString(),
    });
});

// Readiness (Kubernetes)
healthRouter.get('/ready', async (req, res) => {
    try {
        await pool.query('SELECT 1');
        await redis.ping();
        res.json({ ready: true });
    } catch {
        res.status(503).json({ ready: false });
    }
});

// Liveness (Kubernetes)
healthRouter.get('/alive', (req, res) => {
    res.json({ alive: true });
});
```

## Graceful Shutdown

```javascript
let server;

function startServer(port) {
    server = app.listen(port, () => {
        console.log(`Auth service running on port ${port}`);
    });
    return server;
}

async function gracefulShutdown(signal) {
    console.log(`Received ${signal}, shutting down...`);

    // Stop accepting new connections
    server.close(() => {
        console.log('HTTP server closed');
    });

    const timeout = setTimeout(() => {
        console.error('Forced shutdown');
        process.exit(1);
    }, 30000);

    try {
        await pool?.end();
        console.log('Database connections closed');

        await redis?.quit();
        console.log('Redis connections closed');

        clearTimeout(timeout);
        console.log('Graceful shutdown complete');
        process.exit(0);
    } catch (err) {
        console.error('Shutdown error:', err);
        process.exit(1);
    }
}

process.on('SIGTERM', () => gracefulShutdown('SIGTERM'));
process.on('SIGINT', () => gracefulShutdown('SIGINT'));
```

## Common Mistakes

- Not validating environment variables on startup
- Not implementing health checks for dependencies
- Not handling graceful shutdown (drops in-flight requests)
- Hardcoding secrets in configuration

## Cross-References

- See [Deployment](./01-production-deployment.md) for production setup
- See [Monitoring](../10-authentication-monitoring/01-monitoring-metrics.md) for observability
- See [Security](../06-authentication-security/01-security-headers.md) for hardening

## Next Steps

Continue to [Future Trends: Risk-Based Auth](../12-authentication-future-trends/02-risk-based-auth.md).
