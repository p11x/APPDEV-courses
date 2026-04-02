# Express.js Production Deployment

## What You'll Learn

- Production deployment best practices
- Environment configuration
- Health checks and monitoring
- Deployment strategies

## Production Configuration

```javascript
// src/config.js
import { z } from 'zod';

const envSchema = z.object({
    PORT: z.coerce.number().default(3000),
    NODE_ENV: z.enum(['development', 'production', 'test']).default('development'),
    DATABASE_URL: z.string().url(),
    JWT_SECRET: z.string().min(32),
    REDIS_URL: z.string().url().optional(),
});

export const config = envSchema.parse(process.env);
```

## Health Check Endpoint

```javascript
app.get('/health', (req, res) => {
    res.json({
        status: 'healthy',
        uptime: process.uptime(),
        timestamp: new Date().toISOString(),
        version: process.env.npm_package_version,
    });
});

app.get('/ready', async (req, res) => {
    try {
        await db.ping();
        res.json({ status: 'ready' });
    } catch {
        res.status(503).json({ status: 'not ready' });
    }
});
```

## Graceful Shutdown

```javascript
const server = app.listen(config.PORT);

const shutdown = async (signal) => {
    console.log(`${signal} received, shutting down...`);
    server.close(() => console.log('HTTP server closed'));
    await db.end();
    await redis.quit();
    process.exit(0);
};

process.on('SIGTERM', () => shutdown('SIGTERM'));
process.on('SIGINT', () => shutdown('SIGINT'));
```

## PM2 Configuration

```javascript
// ecosystem.config.cjs
module.exports = {
    apps: [{
        name: 'api',
        script: './dist/server.js',
        instances: 'max',
        exec_mode: 'cluster',
        max_memory_restart: '500M',
        env_production: { NODE_ENV: 'production' },
        error_file: './logs/err.log',
        out_file: './logs/out.log',
        autorestart: true,
    }],
};
```

## Best Practices Checklist

- [ ] Use environment variables for configuration
- [ ] Implement health check endpoints
- [ ] Handle graceful shutdown
- [ ] Use process manager (PM2)
- [ ] Monitor application metrics

## Cross-References

- See [Container](../13-container-orchestration/01-docker-setup.md) for Docker
- See [Monitoring](../14-monitoring-observability/01-apm-setup.md) for observability
- See [Security](../05-security-implementation/01-helmet-cors.md) for security

## Next Steps

This completes Chapter 5 of the Node.js guide. Proceed to [Chapter 6: Databases](../../06-databases/) for data persistence.
