# Graceful Shutdown Procedures

## What You'll Learn

- Shutdown sequence design
- Connection draining strategies
- Timeout management
- Container integration

## Shutdown Sequence

```javascript
class GracefulShutdown {
    constructor() {
        this.handlers = [];
        this.timeout = 10000;
        this.shuttingDown = false;
    }

    register(name, handler, priority = 0) {
        this.handlers.push({ name, handler, priority });
        this.handlers.sort((a, b) => b.priority - a.priority);
    }

    async shutdown(signal) {
        if (this.shuttingDown) return;
        this.shuttingDown = true;

        console.log(`[${signal}] Shutdown initiated`);
        const start = Date.now();

        // Force exit timeout
        const forceExit = setTimeout(() => {
            console.error(`Forced exit after ${this.timeout}ms`);
            process.exit(1);
        }, this.timeout);

        // Run handlers in priority order
        for (const { name, handler } of this.handlers) {
            try {
                console.log(`Shutting down: ${name}`);
                await handler();
                console.log(`Shutdown complete: ${name}`);
            } catch (err) {
                console.error(`Shutdown error (${name}):`, err.message);
            }
        }

        clearTimeout(forceExit);
        console.log(`Graceful shutdown complete in ${Date.now() - start}ms`);
        process.exit(0);
    }
}

// Usage
const shutdown = new GracefulShutdown();

shutdown.register('http-server', async () => {
    await new Promise(resolve => server.close(resolve));
}, 100); // High priority

shutdown.register('database', async () => {
    await db.end();
}, 50);

shutdown.register('cache', async () => {
    await redis.quit();
}, 50);

shutdown.register('logger', async () => {
    await logger.flush();
}, 10); // Low priority

process.on('SIGTERM', () => shutdown.shutdown('SIGTERM'));
process.on('SIGINT', () => shutdown.shutdown('SIGINT'));
```

## Best Practices Checklist

- [ ] Stop accepting new work first
- [ ] Drain existing connections with timeout
- [ ] Close resources in reverse initialization order
- [ ] Set force-exit timeout
- [ ] Log shutdown progress

## Cross-References

- See [Signal Handling](./01-signal-handling.md) for signal basics
- See [Process Monitoring](./03-process-monitoring.md) for observability
- See [Error Handling](../11-error-handling/01-error-propagation.md) for errors

## Next Steps

Continue to [Process Monitoring](./03-process-monitoring.md) for observability.
