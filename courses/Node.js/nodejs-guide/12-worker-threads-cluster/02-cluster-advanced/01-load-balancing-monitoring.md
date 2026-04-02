# Cluster Load Balancing, Fault Tolerance, and Monitoring

## What You'll Learn

- Advanced cluster load balancing strategies
- Zero-downtime graceful shutdown
- Worker process monitoring and metrics
- Cluster auto-scaling
- Cluster debugging and troubleshooting

## Advanced Cluster with Graceful Shutdown

```js
// cluster-advanced.js — Production cluster with graceful shutdown
import cluster from 'node:cluster';
import { availableParallelism } from 'node:os';
import { createServer } from 'node:http';

const SHUTDOWN_TIMEOUT = 30000; // 30 seconds

if (cluster.isPrimary) {
    const numWorkers = process.env.WORKERS
        ? parseInt(process.env.WORKERS)
        : availableParallelism();

    console.log(`Primary ${process.pid} starting ${numWorkers} workers`);

    // Track worker states
    const workers = new Map();

    function forkWorker() {
        const worker = cluster.fork();
        workers.set(worker.id, {
            pid: worker.process.pid,
            startedAt: Date.now(),
            restarts: 0,
        });
        return worker;
    }

    // Fork initial workers
    for (let i = 0; i < numWorkers; i++) {
        forkWorker();
    }

    // Handle worker crashes with restart limit
    cluster.on('exit', (worker, code, signal) => {
        const info = workers.get(worker.id);
        workers.delete(worker.id);

        console.log(`Worker ${worker.process.pid} exited (code: ${code})`);

        if (code !== 0 && info && info.restarts < 5) {
            const newWorker = forkWorker();
            workers.set(newWorker.id, { ...info, restarts: info.restarts + 1 });
            console.log(`Restarted worker (attempt ${info.restarts + 1})`);
        } else if (info?.restarts >= 5) {
            console.error(`Worker exceeded restart limit, not restarting`);
        }
    });

    // Graceful shutdown of all workers
    async function shutdownAll(signal) {
        console.log(`\n${signal} received, shutting down ${workers.size} workers...`);

        const shutdownPromises = [];

        for (const [id, worker] of Object.entries(cluster.workers)) {
            if (worker) {
                // Send shutdown signal to worker
                worker.send({ type: 'shutdown' });

                shutdownPromises.push(
                    new Promise((resolve) => {
                        const timeout = setTimeout(() => {
                            console.log(`Force killing worker ${worker.process.pid}`);
                            worker.kill('SIGKILL');
                            resolve();
                        }, SHUTDOWN_TIMEOUT);

                        worker.on('exit', () => {
                            clearTimeout(timeout);
                            resolve();
                        });
                    })
                );
            }
        }

        await Promise.all(shutdownPromises);
        console.log('All workers shut down');
        process.exit(0);
    }

    process.on('SIGTERM', () => shutdownAll('SIGTERM'));
    process.on('SIGINT', () => shutdownAll('SIGINT'));

    // Status endpoint (primary manages this)
    const statusServer = createServer((req, res) => {
        if (req.url === '/cluster/status') {
            const status = {
                pid: process.pid,
                workers: [...workers.entries()].map(([id, info]) => ({
                    id,
                    pid: info.pid,
                    uptime: Math.floor((Date.now() - info.startedAt) / 1000),
                    restarts: info.restarts,
                })),
            };
            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify(status, null, 2));
        }
    });
    statusServer.listen(9999);

} else {
    // Worker process
    const server = createServer((req, res) => {
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({
            worker: process.pid,
            timestamp: new Date().toISOString(),
        }));
    });

    server.listen(3000);

    // Graceful shutdown handler
    let isShuttingDown = false;

    process.on('message', (msg) => {
        if (msg.type === 'shutdown' && !isShuttingDown) {
            isShuttingDown = true;
            console.log(`Worker ${process.pid} received shutdown signal`);

            server.close(() => {
                console.log(`Worker ${process.pid} closed all connections`);
                process.exit(0);
            });

            // Force close after timeout
            setTimeout(() => {
                console.log(`Worker ${process.pid} force exiting`);
                process.exit(1);
            }, SHUTDOWN_TIMEOUT - 1000);
        }
    });

    process.on('SIGTERM', () => {
        if (!isShuttingDown) {
            isShuttingDown = true;
            server.close(() => process.exit(0));
        }
    });
}
```

## Custom Load Balancing

```js
// custom-lb.js — Primary process with custom load balancing logic
import cluster from 'node:cluster';
import net from 'node:net';

if (cluster.isPrimary) {
    const workers = [];
    const WORKER_COUNT = 4;

    // Strategy: least-connections
    let currentIndex = 0;

    function getNextWorker() {
        // Find worker with fewest active connections
        let minConnections = Infinity;
        let selectedWorker = workers[0];

        for (const worker of workers) {
            if (worker.connections < minConnections) {
                minConnections = worker.connections;
                selectedWorker = worker;
            }
        }

        selectedWorker.connections++;
        return selectedWorker;
    }

    // Fork workers
    for (let i = 0; i < WORKER_COUNT; i++) {
        const worker = cluster.fork();
        workers.push({
            worker,
            connections: 0,
            pid: worker.process.pid,
        });

        worker.on('message', (msg) => {
            if (msg.type === 'connection-done') {
                const w = workers.find(w => w.pid === msg.pid);
                if (w) w.connections = Math.max(0, w.connections - 1);
            }
        });
    }

    // Manual TCP load balancing (alternative to OS-level)
    const server = net.createServer({ pauseOnConnect: true }, (socket) => {
        const selected = getNextWorker();
        selected.worker.send('sticky-connection', socket);
    });

    server.listen(3000, () => {
        console.log('Load balancer listening on port 3000');
    });

} else {
    // Worker receives connections from primary
    process.on('message', (msg, socket) => {
        if (msg === 'sticky-connection' && socket) {
            // Handle the connection
            const server = require('net').createServer();
            server.emit('connection', socket);
            socket.resume();
        }
    });
}
```

## Cluster Monitoring

```js
// cluster-monitor.js — Monitor cluster health and metrics
import cluster from 'node:cluster';

class ClusterMonitor {
    constructor() {
        this.metrics = new Map();
    }

    start() {
        if (!cluster.isPrimary) return;

        // Collect metrics every 10 seconds
        setInterval(() => this.collect(), 10000);

        // Track worker events
        cluster.on('fork', (worker) => {
            console.log(`[Monitor] Worker ${worker.process.pid} forked`);
        });

        cluster.on('online', (worker) => {
            console.log(`[Monitor] Worker ${worker.process.pid} online`);
        });

        cluster.on('exit', (worker, code, signal) => {
            console.log(`[Monitor] Worker ${worker.process.pid} exited (code: ${code})`);
        });
    }

    async collect() {
        const stats = {
            timestamp: new Date().toISOString(),
            workers: [],
        };

        for (const id in cluster.workers) {
            const worker = cluster.workers[id];
            if (!worker) continue;

            // Request stats from each worker
            worker.send({ type: 'collect-metrics' });
        }

        return stats;
    }

    getStatus() {
        const workers = [];
        for (const id in cluster.workers) {
            const w = cluster.workers[id];
            if (w) {
                workers.push({
                    id: w.id,
                    pid: w.process.pid,
                    state: w.state,
                });
            }
        }
        return {
            primaryPid: process.pid,
            workerCount: workers.length,
            workers,
        };
    }
}

const monitor = new ClusterMonitor();
monitor.start();

// Expose status via HTTP
import { createServer } from 'node:http';
createServer((req, res) => {
    if (req.url === '/status') {
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify(monitor.getStatus(), null, 2));
    }
}).listen(9999);
```

## Common Mistakes

- Not handling graceful shutdown (drops in-flight requests)
- Not monitoring worker health (silent crashes)
- Using global state across workers (each has separate memory)
- Not limiting worker restarts (infinite crash loops)

## Try It Yourself

### Exercise 1: Graceful Shutdown
Start the cluster, send 10 concurrent requests, then send SIGTERM. Verify no requests are dropped.

### Exercise 2: Crash Recovery
Kill a worker with `kill -9 <pid>` and verify it's automatically restarted.

### Exercise 3: Load Distribution
Make 100 requests and verify they're distributed evenly across workers.

## Next Steps

Continue to [Parallel Processing](../03-parallel-processing/01-cpu-parallelization.md).
