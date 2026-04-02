# CPU Efficiency, Scalability, and Concurrency Models

## What You'll Learn

- Scaling Node.js across CPU cores
- Worker threads for CPU-bound tasks
- Cluster module for horizontal scaling
- Load balancing strategies

## Multi-Core Scaling with Cluster

```javascript
// cluster-server.js — Scale across all CPU cores

import cluster from 'node:cluster';
import os from 'node:os';
import { createServer } from 'node:http';

const numCPUs = os.cpus().length;

if (cluster.isPrimary) {
    console.log(`Primary ${process.pid} starting ${numCPUs} workers`);
    
    // Fork workers
    for (let i = 0; i < numCPUs; i++) {
        cluster.fork();
    }
    
    // Restart crashed workers
    cluster.on('exit', (worker, code, signal) => {
        console.log(`Worker ${worker.process.pid} died (${signal || code})`);
        cluster.fork();
    });
    
    // Graceful shutdown
    process.on('SIGTERM', () => {
        console.log('SIGTERM received, shutting down workers...');
        for (const worker of Object.values(cluster.workers)) {
            worker.process.kill('SIGTERM');
        }
    });
} else {
    const server = createServer((req, res) => {
        res.writeHead(200);
        res.end(`Worker ${process.pid}\n`);
    });
    
    server.listen(3000, () => {
        console.log(`Worker ${process.pid} listening on port 3000`);
    });
}
```

## Worker Threads for CPU-Bound Tasks

```javascript
// worker-pool.js — Thread pool for CPU-intensive work

import { Worker, isMainThread, parentPort, workerData } from 'node:worker_threads';

if (isMainThread) {
    // ── Main Thread ──────────────────────────────
    
    class WorkerPool {
        constructor(script, numThreads = 4) {
            this.script = script;
            this.workers = [];
            this.taskQueue = [];
            
            for (let i = 0; i < numThreads; i++) {
                this.addWorker();
            }
        }
        
        addWorker() {
            const worker = new Worker(this.script);
            
            worker.on('message', (result) => {
                worker.resolve(result);
                worker.busy = false;
                this.processQueue();
            });
            
            worker.on('error', (err) => {
                worker.reject(err);
                worker.busy = false;
            });
            
            worker.busy = false;
            this.workers.push(worker);
        }
        
        processQueue() {
            if (this.taskQueue.length === 0) return;
            
            const freeWorker = this.workers.find(w => !w.busy);
            if (!freeWorker) return;
            
            const { data, resolve, reject } = this.taskQueue.shift();
            freeWorker.busy = true;
            freeWorker.resolve = resolve;
            freeWorker.reject = reject;
            freeWorker.postMessage(data);
        }
        
        run(data) {
            return new Promise((resolve, reject) => {
                this.taskQueue.push({ data, resolve, reject });
                this.processQueue();
            });
        }
        
        terminate() {
            return Promise.all(this.workers.map(w => w.terminate()));
        }
    }
    
    // Usage
    const pool = new WorkerPool(new URL('./worker-task.js', import.meta.url), 4);
    
    const results = await Promise.all([
        pool.run({ type: 'fibonacci', n: 40 }),
        pool.run({ type: 'fibonacci', n: 42 }),
        pool.run({ type: 'hash', data: 'a'.repeat(1000000) }),
        pool.run({ type: 'sort', data: Array.from({ length: 1000000 }, () => Math.random()) }),
    ]);
    
    console.log(results);
    await pool.terminate();
    
} else {
    // ── Worker Thread ────────────────────────────
    
    parentPort.on('message', ({ type, n, data }) => {
        let result;
        
        switch (type) {
            case 'fibonacci':
                result = fib(n);
                break;
            case 'hash':
                result = require('node:crypto').createHash('sha256').update(data).digest('hex');
                break;
            case 'sort':
                result = data.sort((a, b) => a - b).length;
                break;
        }
        
        parentPort.postMessage(result);
    });
    
    function fib(n) {
        if (n <= 1) return n;
        return fib(n - 1) + fib(n - 2);
    }
}
```

## Concurrency Patterns Comparison

```
Concurrency Model Comparison:
─────────────────────────────────────────────
Single Thread:
├── Simple, no race conditions
├── Limited to 1 CPU core
├── Blocks on CPU-intensive work
└── Best for: I/O-bound APIs

Cluster:
├── Multiple processes
├── Uses all CPU cores
├── Shared-nothing (use Redis for sessions)
├── Automatic restart on crash
└── Best for: HTTP servers

Worker Threads:
├── Shared memory (SharedArrayBuffer)
├── Fine-grained parallelism
├── No automatic restart
├── Best for: CPU-bound computation

Child Processes:
├── Separate processes
├── Full isolation
├── Higher overhead than workers
├── Best for: Running external programs
```

## Best Practices Checklist

- [ ] Use cluster module for HTTP servers
- [ ] Use worker threads for CPU-intensive tasks
- [ ] Implement graceful shutdown in cluster mode
- [ ] Use sticky sessions for WebSocket in cluster
- [ ] Monitor worker health and restart on failure
- [ ] Keep worker thread count ≤ CPU core count

## Cross-References

- See [Performance Characteristics](./01-performance-characteristics.md) for memory patterns
- See [Benchmark Profiling](./03-benchmark-profiling.md) for measurement
- See [Event Loop Mechanics](../06-event-loop-mechanics/01-event-loop-deep-dive.md) for async model

## Next Steps

Continue to [Benchmark Profiling](./03-benchmark-profiling.md) for measurement techniques.
