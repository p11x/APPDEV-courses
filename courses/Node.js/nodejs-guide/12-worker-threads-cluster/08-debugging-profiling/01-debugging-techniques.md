# Worker Thread Debugging and Profiling Techniques

## What You'll Learn

- Debugging worker threads with --inspect
- Memory leak detection in workers
- Performance profiling workers
- Production debugging strategies
- Common worker issues and solutions

## Debugging Worker Threads

```bash
# Debug main thread and workers simultaneously
node --inspect=9229 server.js

# Debug specific worker (separate port)
node --inspect-brk=9230 worker.js

# Debug with source maps
node --inspect --enable-source-maps server.js
```

```js
// debug-worker.js — Worker with debugging support
import { Worker, isMainThread, parentPort } from 'node:worker_threads';

if (isMainThread) {
    const worker = new Worker('./debug-worker.js', {
        // Enable debugging for this worker
        env: {
            ...process.env,
            NODE_OPTIONS: '--inspect-brk=9231', // Each worker gets unique port
        },
    });

    worker.on('message', console.log);
    worker.on('error', console.error);

} else {
    // Add debugging breakpoints
    debugger; // Chrome DevTools will pause here

    // Debug logging
    console.log(`Worker ${process.pid} started`);

    parentPort.on('message', (task) => {
        console.log(`Processing task:`, task);
        // Process task...
    });
}
```

## Memory Leak Detection

```js
// memory-leak-detector.js — Detect memory leaks in workers
import { Worker } from 'node:worker_threads';

class WorkerMemoryMonitor {
    constructor(workerPath, options = {}) {
        this.workerPath = workerPath;
        this.samples = [];
        this.interval = options.interval || 5000;
        this.leakThreshold = options.leakThreshold || 10; // MB growth
    }

    async start() {
        this.worker = new Worker(this.workerPath);

        // Request memory stats periodically
        this.timer = setInterval(() => {
            this.worker.postMessage({ type: 'memory-check' });
        }, this.interval);

        this.worker.on('message', (msg) => {
            if (msg.type === 'memory-stats') {
                this.samples.push({
                    timestamp: Date.now(),
                    heapUsed: msg.heapUsed / 1024 / 1024,
                    rss: msg.rss / 1024 / 1024,
                });

                // Keep last 100 samples
                if (this.samples.length > 100) this.samples.shift();

                this.detectLeak();
            }
        });
    }

    detectLeak() {
        if (this.samples.length < 10) return;

        const first5 = this.samples.slice(0, 5);
        const last5 = this.samples.slice(-5);

        const avgFirst = first5.reduce((s, r) => s + r.heapUsed, 0) / 5;
        const avgLast = last5.reduce((s, r) => s + r.heapUsed, 0) / 5;
        const growth = avgLast - avgFirst;

        if (growth > this.leakThreshold) {
            console.warn(`Potential memory leak detected: +${growth.toFixed(1)}MB growth`);
            this.worker.postMessage({ type: 'heap-snapshot' });
        }
    }

    getReport() {
        return {
            samples: this.samples.length,
            currentHeapMB: this.samples.at(-1)?.heapUsed || 0,
            peakHeapMB: Math.max(...this.samples.map(s => s.heapUsed)),
            growthMB: this.samples.length > 1
                ? this.samples.at(-1).heapUsed - this.samples[0].heapUsed
                : 0,
        };
    }
}
```

## Common Issues and Solutions

```
Worker Thread Troubleshooting:
─────────────────────────────────────────────

Issue: Worker exits immediately with code 0
Cause: Script finishes without waiting for messages
Fix:   Use parentPort.on('message', ...) to keep alive

Issue: Worker not responding to messages
Cause: Missing parentPort.on('message', ...) handler
Fix:   Add message listener in worker script

Issue: "Cannot find module" in worker
Cause: Relative imports resolve differently in workers
Fix:   Use fileURLToPath(import.meta.url) for absolute paths

Issue: Worker uses too much memory
Cause: No resource limits set
Fix:   Set resourceLimits.maxOldGenerationSizeMb

Issue: Worker crashes main thread
Cause: Unhandled error in worker
Fix:   Always add worker.on('error', ...) handler

Issue: Slow worker creation (~50ms)
Cause: New V8 isolate for each worker
Fix:   Use worker pool (pre-create workers)
```

## Production Debugging Checklist

```js
// production-worker-setup.js
import { Worker } from 'node:worker_threads';

function createProductionWorker(path, options = {}) {
    const worker = new Worker(path, {
        resourceLimits: {
            maxOldGenerationSizeMb: options.maxHeapMB || 256,
        },
    });

    // Always handle errors
    worker.on('error', (err) => {
        console.error(`Worker error:`, err);
        metrics.workerErrors.inc();
        // Restart or alert
    });

    // Handle unexpected exit
    worker.on('exit', (code) => {
        if (code !== 0) {
            console.error(`Worker exited with code ${code}`);
            // Restart logic
        }
    });

    // Handle message errors
    worker.on('messageerror', (err) => {
        console.error(`Worker message error:`, err);
    });

    return worker;
}
```

## Common Mistakes

- Not adding error handlers on workers (silent crashes)
- Not setting resource limits (OOM crashes)
- Debugging with production data (security risk)
- Not logging worker lifecycle events

## Try It Yourself

### Exercise 1: Debug a Worker
Start a worker with `--inspect-brk` and step through code in Chrome DevTools.

### Exercise 2: Find a Memory Leak
Create a worker that leaks memory and detect it with the monitor.

### Exercise 3: Handle Crashes
Create a worker that crashes and verify the main thread recovers.

## Next Steps

Continue to [Integration Patterns](../09-integration-patterns/01-express-integration.md).
