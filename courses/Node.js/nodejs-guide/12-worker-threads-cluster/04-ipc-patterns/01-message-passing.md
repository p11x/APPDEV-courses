# Inter-Process Communication (IPC) Patterns

## What You'll Learn

- Message passing protocols between processes
- Shared memory communication strategies
- Request-response pattern over IPC
- Broadcast and pub-sub over IPC
- IPC performance optimization

## Request-Response over IPC

```js
// lib/ipc-client.js — Request-response over worker IPC
import { randomUUID } from 'node:crypto';
import { EventEmitter } from 'node:events';

class IPCClient extends EventEmitter {
    constructor(process) {
        super();
        this.process = process; // worker or child_process
        this.pending = new Map();
        this.setupListeners();
    }

    setupListeners() {
        this.process.on('message', (msg) => {
            if (msg?.__ipc_id) {
                const pending = this.pending.get(msg.__ipc_id);
                if (pending) {
                    clearTimeout(pending.timeout);
                    this.pending.delete(msg.__ipc_id);

                    if (msg.error) {
                        pending.reject(new Error(msg.error));
                    } else {
                        pending.resolve(msg.data);
                    }
                }
            } else {
                this.emit('notification', msg);
            }
        });
    }

    async request(type, data, timeoutMs = 10000) {
        const id = randomUUID();

        return new Promise((resolve, reject) => {
            const timeout = setTimeout(() => {
                this.pending.delete(id);
                reject(new Error(`IPC request timed out: ${type}`));
            }, timeoutMs);

            this.pending.set(id, { resolve, reject, timeout });
            this.process.send({ __ipc_id: id, type, data });
        });
    }

    notify(type, data) {
        this.process.send({ type, data });
    }
}

// lib/ipc-server.js — Handles requests on the worker side
class IPCServer extends EventEmitter {
    constructor() {
        super();
        this.handlers = new Map();
        this.setupListener();
    }

    setupListener() {
        process.on('message', async (msg) => {
            if (msg?.__ipc_id) {
                const handler = this.handlers.get(msg.type);
                if (!handler) {
                    process.send({
                        __ipc_id: msg.__ipc_id,
                        error: `Unknown handler: ${msg.type}`,
                    });
                    return;
                }

                try {
                    const result = await handler(msg.data);
                    process.send({ __ipc_id: msg.__ipc_id, data: result });
                } catch (err) {
                    process.send({
                        __ipc_id: msg.__ipc_id,
                        error: err.message,
                    });
                }
            } else {
                this.emit('notification', msg);
            }
        });
    }

    on(type, handler) {
        this.handlers.set(type, handler);
        return this;
    }
}

export { IPCClient, IPCServer };
```

## Broadcast to All Workers

```js
// broadcast.js — Primary broadcasts to all workers
import cluster from 'node:cluster';

if (cluster.isPrimary) {
    // Broadcast to all workers
    function broadcast(message) {
        for (const id in cluster.workers) {
            cluster.workers[id]?.send(message);
        }
    }

    // Broadcast config update
    broadcast({ type: 'config-update', data: { maxConnections: 500 } });

    // Broadcast shutdown signal
    broadcast({ type: 'shutdown', gracePeriod: 30000 });

} else {
    // Worker receives broadcasts
    process.on('message', (msg) => {
        switch (msg.type) {
            case 'config-update':
                console.log('Config updated:', msg.data);
                break;
            case 'shutdown':
                console.log('Shutdown requested');
                // Graceful shutdown logic
                break;
        }
    });
}
```

## SharedArrayBuffer Communication Channel

```js
// shared-channel.js — High-performance ring buffer over SharedArrayBuffer
import { Worker, isMainThread, workerData, parentPort } from 'node:worker_threads';

class SharedChannel {
    constructor(buffer) {
        // Layout: [writeIndex:4][readIndex:4][data...]
        this.header = new Int32Array(buffer, 0, 2);
        this.data = new Uint8Array(buffer, 8);
        this.capacity = this.data.length;
    }

    write(message) {
        const encoded = new TextEncoder().encode(JSON.stringify(message));
        const msgLen = encoded.length;

        if (msgLen > this.capacity - 4) {
            throw new Error('Message too large for shared buffer');
        }

        const writeIdx = Atomics.load(this.header, 0);
        const readIdx = Atomics.load(this.header, 1);

        // Check available space
        const available = writeIdx >= readIdx
            ? this.capacity - writeIdx + readIdx
            : readIdx - writeIdx;

        if (available < msgLen + 4) return false; // Buffer full

        // Write length prefix
        const lenView = new DataView(this.data.buffer, 8 + writeIdx, 4);
        lenView.setUint32(0, msgLen, true);

        // Write data
        for (let i = 0; i < msgLen; i++) {
            this.data[(writeIdx + 4 + i) % this.capacity] = encoded[i];
        }

        // Update write index
        Atomics.store(this.header, 0, (writeIdx + 4 + msgLen) % this.capacity);
        Atomics.notify(this.header, 0);

        return true;
    }

    read() {
        const writeIdx = Atomics.load(this.header, 0);
        const readIdx = Atomics.load(this.header, 1);

        if (writeIdx === readIdx) return null; // Empty

        const lenView = new DataView(this.data.buffer, 8 + readIdx, 4);
        const msgLen = lenView.getUint32(0, true);

        const bytes = new Uint8Array(msgLen);
        for (let i = 0; i < msgLen; i++) {
            bytes[i] = this.data[(readIdx + 4 + i) % this.capacity];
        }

        Atomics.store(this.header, 1, (readIdx + 4 + msgLen) % this.capacity);

        return JSON.parse(new TextDecoder().decode(bytes));
    }
}
```

## IPC Performance Benchmarks

```
IPC Performance Comparison:
─────────────────────────────────────────────
Method                    Latency     Throughput
─────────────────────────────────────────────
postMessage (small msg)   0.01ms      100K msg/s
postMessage (1KB)         0.05ms      20K msg/s
postMessage (1MB)         2.5ms       400 msg/s
SharedArrayBuffer          0.001ms    1M ops/s
SharedArrayBuffer + lock   0.01ms     100K ops/s

Recommendation:
├── Small messages (< 1KB): Use postMessage
├── Large data: Use SharedArrayBuffer
├── High frequency: Use SharedArrayBuffer
└── Complex objects: Use postMessage (structured clone)
```

## Common Mistakes

- Not handling message serialization errors
- Sending large objects through postMessage (copies them)
- Not implementing timeouts on IPC requests
- Circular references in messages (structured clone fails)

## Try It Yourself

### Exercise 1: Build IPC Client/Server
Implement the request-response pattern and test with 100 concurrent requests.

### Exercise 2: Broadcast Config
Build a system where the primary broadcasts config updates to all workers.

### Exercise 3: Shared Ring Buffer
Implement the SharedChannel and benchmark it against postMessage.

## Next Steps

Continue to [Performance Optimization](../05-performance-optimization/01-worker-profiling.md).
