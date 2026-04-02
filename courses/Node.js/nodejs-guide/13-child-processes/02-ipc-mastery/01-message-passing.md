# Child Process IPC Mastery

## What You'll Learn

- IPC message passing with fork()
- Bidirectional communication protocols
- Process message serialization
- IPC error handling and recovery
- High-performance IPC patterns

## IPC with fork()

```js
// ipc-parent.js — Parent process with IPC
import { fork } from 'node:child_process';
import { randomUUID } from 'node:crypto';

class ProcessIPC {
    constructor(workerPath, options = {}) {
        this.workerPath = workerPath;
        this.pending = new Map();
        this.process = null;
        this.maxWorkers = options.maxWorkers || 4;
    }

    start() {
        this.process = fork(this.workerPath, [], {
            silent: false,
            env: { ...process.env },
        });

        this.process.on('message', (msg) => {
            if (msg?.__id) {
                const pending = this.pending.get(msg.__id);
                if (pending) {
                    clearTimeout(pending.timeout);
                    this.pending.delete(msg.__id);
                    msg.error ? pending.reject(new Error(msg.error)) : pending.resolve(msg.data);
                }
            }
        });

        this.process.on('error', (err) => {
            for (const [, pending] of this.pending) pending.reject(err);
            this.pending.clear();
        });

        this.process.on('exit', (code) => {
            if (code !== 0) {
                const err = new Error(`Worker exited with code ${code}`);
                for (const [, pending] of this.pending) pending.reject(err);
                this.pending.clear();
            }
        });
    }

    async send(type, data, timeoutMs = 30000) {
        const id = randomUUID();

        return new Promise((resolve, reject) => {
            const timeout = setTimeout(() => {
                this.pending.delete(id);
                reject(new Error(`IPC timeout: ${type}`));
            }, timeoutMs);

            this.pending.set(id, { resolve, reject, timeout });
            this.process.send({ __id: id, type, data });
        });
    }

    async terminate() {
        if (this.process) {
            this.process.send({ __id: 'shutdown', type: 'shutdown' });
            await new Promise(r => setTimeout(r, 1000));
            this.process.kill();
        }
    }
}

// Usage
const ipc = new ProcessIPC('./ipc-worker.js');
ipc.start();

const result = await ipc.send('compute', { operation: 'fibonacci', n: 40 });
console.log('Result:', result);

await ipc.terminate();
```

```js
// ipc-worker.js — Worker process responding to IPC
import { parentPort } from 'node:child_process';

const handlers = {
    fibonacci({ n }) {
        if (n <= 1) return n;
        let a = 0, b = 1;
        for (let i = 2; i <= n; i++) [a, b] = [b, a + b];
        return b;
    },

    primeCheck({ n }) {
        if (n < 2) return false;
        for (let i = 2; i * i <= n; i++) {
            if (n % i === 0) return false;
        }
        return true;
    },

    shutdown() {
        process.exit(0);
    },
};

process.on('message', async (msg) => {
    try {
        const handler = handlers[msg.type];
        if (!handler) throw new Error(`Unknown type: ${msg.type}`);
        const data = handler(msg.data);
        process.send({ __id: msg.__id, data });
    } catch (err) {
        process.send({ __id: msg.__id, error: err.message });
    }
});
```

## Process Pool with IPC

```js
// process-pool.js — Pool of forked processes with IPC
import { fork } from 'node:child_process';
import os from 'node:os';

class ProcessPool {
    constructor(workerPath, size) {
        this.workerPath = workerPath;
        this.size = size || os.availableParallelism() || 4;
        this.workers = [];
        this.taskQueue = [];
        this.taskId = 0;
    }

    async start() {
        for (let i = 0; i < this.size; i++) {
            this.addWorker(i);
        }
    }

    addWorker(id) {
        const worker = fork(this.workerPath);
        const info = { id, worker, busy: false, tasks: 0 };

        worker.on('message', (msg) => {
            info.busy = false;
            const pending = this.pending?.get(msg.__id);
            if (pending) {
                clearTimeout(pending.timeout);
                this.pending.delete(msg.__id);
                msg.error ? pending.reject(new Error(msg.error)) : pending.resolve(msg.data);
            }
            this.processQueue();
        });

        worker.on('exit', () => {
            console.log(`Worker ${id} exited, restarting...`);
            this.addWorker(id);
        });

        this.workers[id] = info;
        this.pending = this.pending || new Map();
    }

    async execute(type, data) {
        return new Promise((resolve, reject) => {
            const id = this.taskId++;
            const task = { id, type, data, resolve, reject };
            const available = this.workers.find(w => w && !w.busy);

            if (available) {
                this.runTask(available, task);
            } else {
                this.taskQueue.push(task);
            }
        });
    }

    runTask(workerInfo, task) {
        workerInfo.busy = true;
        workerInfo.tasks++;

        const timeout = setTimeout(() => {
            task.reject(new Error('Task timeout'));
        }, 30000);

        this.pending.set(task.id, { resolve: task.resolve, reject: task.reject, timeout });
        workerInfo.worker.send({ __id: task.id, type: task.type, data: task.data });
    }

    processQueue() {
        while (this.taskQueue.length > 0) {
            const available = this.workers.find(w => w && !w.busy);
            if (!available) break;
            this.runTask(available, this.taskQueue.shift());
        }
    }

    getStats() {
        return {
            poolSize: this.size,
            busy: this.workers.filter(w => w?.busy).length,
            queued: this.taskQueue.length,
            tasks: this.workers.reduce((s, w) => s + (w?.tasks || 0), 0),
        };
    }

    async terminate() {
        await Promise.all(this.workers.filter(Boolean).map(w => w.worker.kill()));
    }
}
```

## Common Mistakes

- Not handling worker exit (zombie processes)
- Not serializing complex objects correctly (circular refs)
- Not implementing timeouts on IPC requests
- Sending large buffers through IPC (use file or shared memory)

## Try It Yourself

### Exercise 1: Request-Response
Implement IPC client/server and send 100 concurrent tasks.

### Exercise 2: Process Pool
Create a pool of 4 processes and benchmark concurrent fibonacci.

### Exercise 3: Graceful Shutdown
Implement shutdown that waits for pending tasks to complete.

## Next Steps

Continue to [System Commands](../03-system-commands/01-command-execution.md).
