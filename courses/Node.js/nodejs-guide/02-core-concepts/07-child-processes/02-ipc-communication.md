# Inter-Process Communication Patterns

## What You'll Learn

- IPC message passing patterns
- Shared memory between processes
- Stream-based communication
- Process clustering for scaling

## IPC Message Patterns

```javascript
// request-response pattern
// parent.js
const child = fork('./service.js');

function sendRequest(type, data) {
    return new Promise((resolve, reject) => {
        const id = Date.now();
        child.send({ id, type, data });

        const handler = (msg) => {
            if (msg.id === id) {
                child.off('message', handler);
                if (msg.error) reject(new Error(msg.error));
                else resolve(msg.result);
            }
        };
        child.on('message', handler);
    });
}

const result = await sendRequest('compute', { a: 5, b: 3 });

// service.js
process.on('message', (msg) => {
    try {
        const result = msg.data.a + msg.data.b;
        process.send({ id: msg.id, result });
    } catch (err) {
        process.send({ id: msg.id, error: err.message });
    }
});
```

## Process Clustering

```javascript
import cluster from 'node:cluster';
import os from 'node:os';

if (cluster.isPrimary) {
    const workers = new Map();

    for (let i = 0; i < os.cpus().length; i++) {
        const worker = cluster.fork();
        workers.set(worker.id, worker);

        worker.on('message', (msg) => {
            // Broadcast to other workers
            for (const [id, w] of workers) {
                if (id !== worker.id) w.send(msg);
            }
        });
    }

    cluster.on('exit', (worker) => {
        workers.delete(worker.id);
        const newWorker = cluster.fork();
        workers.set(newWorker.id, newWorker);
    });
} else {
    require('./server.js');
}
```

## Best Practices Checklist

- [ ] Use message IDs for request-response matching
- [ ] Handle child process crashes gracefully
- [ ] Set IPC channel limits to prevent memory issues
- [ ] Use serializable data only (no functions/closures)
- [ ] Implement heartbeats for long-lived processes

## Cross-References

- See [spawn/exec/fork](./01-spawn-exec-fork.md) for process creation
- See [Worker Threads](../08-worker-threads/01-parallel-processing.md) for parallelism
- See [Concurrency](../14-concurrency-parallelism/01-async-optimization.md) for patterns

## Next Steps

Continue to [Worker Threads](../08-worker-threads/01-parallel-processing.md) for parallelism.
