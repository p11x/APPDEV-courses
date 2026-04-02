# Stream Concurrency and Parallelism

## What You'll Learn

- Parallel stream processing
- Worker thread integration with streams
- Stream-based concurrent operations
- Backpressure in parallel pipelines
- Performance scaling strategies

## Parallel Stream Processing

```javascript
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

class ParallelTransform extends Transform {
    constructor(transformFn, concurrency = 4) {
        super({ objectMode: true });
        this.transformFn = transformFn;
        this.concurrency = concurrency;
        this.running = 0;
        this.queue = [];
        this.callback = null;
    }

    _transform(chunk, encoding, callback) {
        if (this.running >= this.concurrency) {
            this.queue.push({ chunk, callback });
            return;
        }

        this.running++;
        this.processChunk(chunk, callback);
    }

    async processChunk(chunk, callback) {
        try {
            const result = await this.transformFn(chunk);
            this.push(result);
            callback();
        } catch (err) {
            callback(err);
        } finally {
            this.running--;
            this.drainQueue();
        }
    }

    drainQueue() {
        while (this.queue.length > 0 && this.running < this.concurrency) {
            const { chunk, callback } = this.queue.shift();
            this.running++;
            this.processChunk(chunk, callback);
        }
    }

    _flush(callback) {
        if (this.running === 0) callback();
        else this.callback = callback;
    }
}

// Usage: parallel file processing
const expensiveTransform = new ParallelTransform(async (record) => {
    // Simulate expensive async operation
    await new Promise(r => setTimeout(r, 10));
    return { ...record, processed: true };
}, 8); // 8 concurrent operations

await pipeline(
    createReadStream('data.jsonl'),
    expensiveTransform,
    createWriteStream('processed.jsonl')
);
```

## Worker Thread Stream Processing

```javascript
// main.js
import { Worker } from 'node:worker_threads';
import { Transform } from 'node:stream';

class WorkerPoolTransform extends Transform {
    constructor(workerScript, poolSize = 4) {
        super({ objectMode: true });
        this.workers = [];
        this.available = [];
        this.queue = [];

        for (let i = 0; i < poolSize; i++) {
            const worker = new Worker(workerScript);
            worker.on('message', (result) => this.onWorkerDone(worker, result));
            worker.on('error', (err) => this.destroy(err));
            this.workers.push(worker);
            this.available.push(worker);
        }
    }

    _transform(chunk, encoding, callback) {
        if (this.available.length > 0) {
            const worker = this.available.pop();
            worker.postMessage(chunk);
            this.currentCallback = callback;
        } else {
            this.queue.push({ chunk, callback });
        }
    }

    onWorkerDone(worker, result) {
        this.push(result);
        this.currentCallback();

        if (this.queue.length > 0) {
            const { chunk, callback } = this.queue.shift();
            worker.postMessage(chunk);
            this.currentCallback = callback;
        } else {
            this.available.push(worker);
        }
    }

    _flush(callback) {
        Promise.all(this.workers.map(w => w.terminate())).then(() => callback());
    }
}

// worker.js
import { parentPort } from 'node:worker_threads';

parentPort.on('message', (data) => {
    // Expensive computation
    const result = { ...data, computed: heavyComputation(data) };
    parentPort.postMessage(result);
});

function heavyComputation(data) {
    let sum = 0;
    for (let i = 0; i < 1e6; i++) sum += Math.random();
    return sum;
}
```

## Concurrent Pipeline

```javascript
async function concurrentPipeline(source, stages, concurrency = 4) {
    const { Readable, Transform, PassThrough } = await import('node:stream');
    const { pipeline } = await import('node:stream/promises');

    // Fan-out: distribute work across workers
    const fanOut = new Transform({
        objectMode: true,
        transform(chunk, encoding, callback) {
            // Round-robin to available workers
            const workerIndex = this._counter = (this._counter || 0) % concurrency;
            this._counter++;
            callback(null, { worker: workerIndex, data: chunk });
        }
    });

    // Fan-in: collect results
    const fanIn = new PassThrough({ objectMode: true });

    const workers = Array.from({ length: concurrency }, (_, i) => {
        return new Transform({
            objectMode: true,
            transform(chunk, encoding, callback) {
                if (chunk.worker === i) {
                    stages(chunk.data).then(result => {
                        fanIn.write(result);
                        callback();
                    }).catch(callback);
                } else {
                    callback(null, chunk); // Pass through
                }
            }
        });
    });

    // Chain workers in sequence
    const chain = workers.reduce((prev, curr) => prev.pipe(curr), source);

    return fanIn;
}
```

## Best Practices Checklist

- [ ] Use worker threads for CPU-bound stream operations
- [ ] Pool workers to avoid creation overhead
- [ ] Respect backpressure in parallel pipelines
- [ ] Set concurrency based on CPU cores and workload
- [ ] Monitor queue depth for bottlenecks
- [ ] Handle worker errors gracefully

## Cross-References

- See [Pipeline](../01-streams-architecture/01-duplex-passthrough-pipeline.md) for pipeline basics
- See [Backpressure](../01-streams-architecture/02-backpressure-performance.md) for flow control
- See [Performance](../08-stream-performance-optimization/01-profiling-memory.md) for profiling

## Next Steps

Continue to [Error Handling](../07-stream-error-handling/01-error-patterns.md).
