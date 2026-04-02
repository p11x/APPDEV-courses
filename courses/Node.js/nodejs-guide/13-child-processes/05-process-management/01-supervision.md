# Process Supervision and Management

## What You'll Learn

- Process supervision patterns
- Automatic restart on crash
- Health monitoring
- Resource usage tracking
- Graceful lifecycle management

## Process Supervisor

```js
// lib/process-supervisor.js — Supervise and manage child processes
import { fork, spawn } from 'node:child_process';
import { EventEmitter } from 'node:events';

class ProcessSupervisor extends EventEmitter {
    constructor() {
        super();
        this.processes = new Map();
    }

    add(name, command, args = [], options = {}) {
        this.processes.set(name, {
            command,
            args,
            options,
            process: null,
            status: 'stopped',
            restarts: 0,
            maxRestarts: options.maxRestarts || 5,
            restartDelay: options.restartDelay || 1000,
            startedAt: null,
        });
        return this;
    }

    async start(name) {
        const entry = this.processes.get(name);
        if (!entry) throw new Error(`Unknown process: ${name}`);

        const proc = entry.command.endsWith('.js')
            ? fork(entry.command, entry.args, { silent: true, ...entry.options })
            : spawn(entry.command, entry.args, { stdio: ['pipe', 'pipe', 'pipe'], ...entry.options });

        entry.process = proc;
        entry.status = 'running';
        entry.startedAt = Date.now();

        proc.on('exit', (code, signal) => {
            entry.status = 'exited';
            this.emit('exit', { name, code, signal });

            if (code !== 0 && entry.restarts < entry.maxRestarts) {
                entry.restarts++;
                const delay = entry.restartDelay * Math.pow(2, entry.restarts - 1);
                console.log(`Restarting ${name} in ${delay}ms (attempt ${entry.restarts})`);
                setTimeout(() => this.start(name), delay);
            } else if (entry.restarts >= entry.maxRestarts) {
                this.emit('maxRestarts', { name });
            }
        });

        proc.on('error', (err) => {
            entry.status = 'error';
            this.emit('error', { name, error: err });
        });

        this.emit('start', { name, pid: proc.pid });
        return proc;
    }

    async startAll() {
        for (const name of this.processes.keys()) {
            await this.start(name);
        }
    }

    async stop(name) {
        const entry = this.processes.get(name);
        if (!entry?.process) return;

        return new Promise((resolve) => {
            entry.process.on('exit', () => {
                entry.status = 'stopped';
                resolve();
            });
            entry.process.kill('SIGTERM');

            // Force kill after timeout
            setTimeout(() => {
                if (entry.status !== 'stopped') {
                    entry.process.kill('SIGKILL');
                }
            }, 10000);
        });
    }

    async stopAll() {
        await Promise.all([...this.processes.keys()].map(name => this.stop(name)));
    }

    restart(name) {
        this.stop(name).then(() => {
            const entry = this.processes.get(name);
            if (entry) entry.restarts = 0;
            this.start(name);
        });
    }

    getStatus(name) {
        const entry = this.processes.get(name);
        if (!entry) return null;

        return {
            name,
            status: entry.status,
            pid: entry.process?.pid,
            restarts: entry.restarts,
            uptime: entry.startedAt ? Date.now() - entry.startedAt : 0,
        };
    }

    getAllStatus() {
        return [...this.processes.keys()].map(name => this.getStatus(name));
    }

    // Monitor resource usage
    getResourceUsage(name) {
        const entry = this.processes.get(name);
        if (!entry?.process?.pid) return null;

        try {
            // Read from /proc/[pid]/stat on Linux
            return {
                pid: entry.process.pid,
                name,
                memory: entry.process.memoryUsage?.() || null,
            };
        } catch {
            return null;
        }
    }
}

// Usage
const supervisor = new ProcessSupervisor();

supervisor
    .add('api', 'node', ['src/server.js'], { maxRestarts: 5 })
    .add('worker', 'node', ['src/worker.js'], { maxRestarts: 3 })
    .add('scheduler', 'node', ['src/scheduler.js'], { maxRestarts: 3 });

supervisor.on('exit', ({ name, code }) => {
    console.log(`Process ${name} exited with code ${code}`);
});

supervisor.on('error', ({ name, error }) => {
    console.error(`Process ${name} error:`, error.message);
});

await supervisor.startAll();

// API to manage processes
app.get('/processes', (req, res) => {
    res.json(supervisor.getAllStatus());
});

app.post('/processes/:name/restart', (req, res) => {
    supervisor.restart(req.params.name);
    res.json({ message: `Restarting ${req.params.name}` });
});
```

## Health Check for Child Processes

```js
// lib/process-health.js — Health monitoring for child processes
class ProcessHealthMonitor {
    constructor(supervisor, options = {}) {
        this.supervisor = supervisor;
        this.checkInterval = options.interval || 30000;
        this.thresholds = {
            maxMemoryMB: options.maxMemoryMB || 512,
            maxUptimeNoResponse: options.maxUptimeNoResponse || 60000,
            maxRestarts: options.maxRestarts || 5,
        };
    }

    start() {
        this.timer = setInterval(() => this.check(), this.checkInterval);
    }

    stop() {
        clearInterval(this.timer);
    }

    check() {
        const results = [];

        for (const status of this.supervisor.getAllStatus()) {
            const issues = [];

            if (status.restarts >= this.thresholds.maxRestarts) {
                issues.push(`Exceeded restart limit (${status.restarts})`);
            }

            if (status.status === 'exited') {
                issues.push('Process is not running');
            }

            results.push({
                ...status,
                healthy: issues.length === 0,
                issues,
            });
        }

        const unhealthy = results.filter(r => !r.healthy);
        if (unhealthy.length > 0) {
            console.warn('Unhealthy processes:', unhealthy.map(r => r.name));
        }

        return results;
    }
}
```

## Common Mistakes

- Not implementing exponential backoff for restarts
- Not tracking restart count (infinite restart loops)
- Not handling SIGTERM gracefully (dropped connections)
- Not monitoring child process resource usage

## Try It Yourself

### Exercise 1: Supervisor
Create a supervisor that manages 3 processes and restarts crashed ones.

### Exercise 2: Health Monitor
Implement health checks that alert when processes are unhealthy.

### Exercise 3: Graceful Shutdown
Test that SIGTERM stops all child processes gracefully.

## Next Steps

Continue to [Security](../06-security/01-sandboxing.md).
