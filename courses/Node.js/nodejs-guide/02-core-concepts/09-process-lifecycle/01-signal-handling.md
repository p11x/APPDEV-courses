# Signal Handling and Graceful Shutdown

## What You'll Learn

- Handling OS signals (SIGTERM, SIGINT, SIGHUP)
- Graceful shutdown procedures
- Resource cleanup patterns
- Process monitoring

## Signal Handling

```javascript
// SIGINT — Ctrl+C
process.on('SIGINT', () => {
    console.log('\nSIGINT received');
    gracefulShutdown();
});

// SIGTERM — kill command, container stop
process.on('SIGTERM', () => {
    console.log('SIGTERM received');
    gracefulShutdown();
});

// SIGHUP — terminal closed, config reload trigger
process.on('SIGHUP', () => {
    console.log('SIGHUP received — reloading config');
    reloadConfig();
});

// Uncaught exceptions
process.on('uncaughtException', (err) => {
    console.error('Uncaught:', err);
    process.exit(1);
});

// Unhandled rejections
process.on('unhandledRejection', (reason) => {
    console.error('Unhandled rejection:', reason);
    process.exit(1);
});
```

## Graceful Shutdown

```javascript
let server;
const connections = new Set();

async function gracefulShutdown() {
    console.log('Starting graceful shutdown...');

    // 1. Stop accepting new connections
    server.close(() => console.log('HTTP server closed'));

    // 2. Wait for existing connections to finish
    for (const conn of connections) {
        conn.end();
    }

    // 3. Close database connections
    await db.close();
    console.log('Database closed');

    // 4. Close cache connections
    await redis.quit();
    console.log('Redis closed');

    // 5. Force exit after timeout
    setTimeout(() => {
        console.error('Forced shutdown after timeout');
        process.exit(1);
    }, 10000);

    process.exit(0);
}

// Track connections
server = http.createServer(handler);
server.on('connection', (conn) => {
    connections.add(conn);
    conn.on('close', () => connections.delete(conn));
});
```

## Health Checks

```javascript
// Health check endpoint
app.get('/health', (req, res) => {
    const health = {
        status: 'healthy',
        uptime: process.uptime(),
        memory: process.memoryUsage(),
        timestamp: new Date().toISOString(),
    };
    res.json(health);
});

// Readiness check (can accept traffic?)
app.get('/ready', async (req, res) => {
    try {
        await db.ping();
        await redis.ping();
        res.json({ status: 'ready' });
    } catch {
        res.status(503).json({ status: 'not ready' });
    }
});
```

## Best Practices Checklist

- [ ] Handle SIGTERM and SIGINT for graceful shutdown
- [ ] Close all connections before exiting
- [ ] Set a shutdown timeout (force exit after N seconds)
- [ ] Implement health and readiness endpoints
- [ ] Log shutdown process for debugging
- [ ] Test shutdown behavior in containers

## Cross-References

- See [Graceful Shutdown](./02-graceful-shutdown.md) for shutdown patterns
- See [Process Monitoring](./03-process-monitoring.md) for observability
- See [Error Handling](../11-error-handling/01-error-propagation.md) for error patterns

## Next Steps

Continue to [Graceful Shutdown](./02-graceful-shutdown.md) for shutdown patterns.
