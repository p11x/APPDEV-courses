# Child Process Deployment and Operations

## What You'll Learn

- Production child process configuration
- Process monitoring in production
- Docker deployment with child processes
- Operational runbooks
- Health checks

## Production Configuration

```js
// config/processes.js — Production child process configuration
export default {
    processes: {
        worker: {
            command: 'node',
            args: ['dist/worker.js'],
            env: { NODE_ENV: 'production' },
            maxRestarts: 5,
            restartDelay: 1000,
            timeout: 0, // No timeout for long-running processes
        },
        scheduler: {
            command: 'node',
            args: ['dist/scheduler.js'],
            env: { NODE_ENV: 'production' },
            maxRestarts: 3,
            restartDelay: 2000,
            timeout: 0,
        },
    },

    limits: {
        maxMemoryMB: 512,
        maxCpuPercent: 80,
        maxProcesses: 10,
    },
};
```

## Docker with Child Processes

```dockerfile
# Dockerfile — Multi-process container
FROM node:20-alpine

WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY dist/ ./dist/

# Supervisor to manage multiple processes
RUN apk add --no-cache supervisor
COPY supervisord.conf /etc/supervisor/conf.d/supervisord.conf

CMD ["supervisord", "-c", "/etc/supervisor/conf.d/supervisord.conf"]
```

```ini
; supervisord.conf
[program:api]
command=node dist/server.js
autorestart=true
stdout_logfile=/dev/stdout
stderr_logfile=/dev/stderr

[program:worker]
command=node dist/worker.js
autorestart=true
stdout_logfile=/dev/stdout
stderr_logfile=/dev/stderr
```

## Operational Runbook

```
Child Process Operations Runbook:
─────────────────────────────────────────────

Process Crash Loop:
1. Check logs: docker logs <container> | grep -i process
2. Check memory: /health endpoint
3. Reduce concurrent processes
4. Increase restart delay
5. Check for OOM: dmesg | grep -i oom

High Memory Usage:
1. List processes: ps aux | grep node
2. Check per-process: /proc/<pid>/status
3. Kill memory-heavy processes
4. Reduce maxMemoryMB limit
5. Implement process recycling

Zombie Processes:
1. Check: ps aux | grep defunct
2. Find parent: ps -o ppid= <pid>
3. Kill parent: kill -9 <parent_pid>
4. Clean up: wait for init to reap
```

## Common Mistakes

- Not configuring resource limits in production
- Not implementing process recycling (memory leaks)
- Not monitoring child process health
- Not handling Docker signal propagation

## Try It Yourself

### Exercise 1: Docker Deploy
Deploy a multi-process app with Docker and supervisor.

### Exercise 2: Health Check
Implement health checks for all child processes.

### Exercise 3: Resource Monitoring
Monitor memory usage of child processes and set alerts.

## Next Steps

Continue to [Integration](../11-integration/01-express-integration.md).
