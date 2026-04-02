# Process Monitoring and Management

## What You'll Learn

- Process metrics collection
- Health check implementation
- Process restart strategies
- PM2 and container integration

## Process Metrics

```javascript
class ProcessMonitor {
    constructor(intervalMs = 10000) {
        this.interval = intervalMs;
        this.metrics = [];
    }

    start() {
        this.timer = setInterval(() => this.collect(), this.interval);
    }

    collect() {
        const usage = process.memoryUsage();
        const metric = {
            timestamp: Date.now(),
            memory: {
                rssMB: +(usage.rss / 1024 / 1024).toFixed(1),
                heapUsedMB: +(usage.heapUsed / 1024 / 1024).toFixed(1),
            },
            uptime: process.uptime(),
            cpu: process.cpuUsage(),
        };

        this.metrics.push(metric);
        if (this.metrics.length > 1000) this.metrics.shift();

        return metric;
    }

    stop() {
        clearInterval(this.timer);
    }
}
```

## PM2 Configuration

```javascript
// ecosystem.config.cjs
module.exports = {
    apps: [{
        name: 'api',
        script: './src/server.js',
        instances: 'max',
        exec_mode: 'cluster',
        max_memory_restart: '500M',
        env: { NODE_ENV: 'development' },
        env_production: { NODE_ENV: 'production' },
        error_file: './logs/err.log',
        out_file: './logs/out.log',
        merge_logs: true,
        autorestart: true,
        max_restarts: 10,
        restart_delay: 1000,
    }],
};
```

## Best Practices Checklist

- [ ] Collect metrics at regular intervals
- [ ] Implement health check endpoints
- [ ] Set memory limits for auto-restart
- [ ] Log process lifecycle events
- [ ] Monitor in production with APM tools

## Cross-References

- See [Signal Handling](./01-signal-handling.md) for signals
- See [Graceful Shutdown](./02-graceful-shutdown.md) for shutdown
- See [Error Handling](../11-error-handling/01-error-propagation.md) for errors

## Next Steps

Continue to [Design Patterns](../10-design-patterns/01-creational-patterns.md) for architecture patterns.
