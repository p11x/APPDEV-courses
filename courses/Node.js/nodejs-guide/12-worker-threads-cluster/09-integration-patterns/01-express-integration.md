# Worker Thread Integration with Express, Databases, and Streams

## What You'll Learn

- Integrating worker threads with Express.js
- Offloading database operations to workers
- Stream processing with worker threads
- Worker threads with message queues
- Real-world integration patterns

## Express.js Worker Integration

```js
// server.js — Express with worker thread offloading
import express from 'express';
import { WorkerPool } from './lib/worker-pool.js';

const app = express();
app.use(express.json());

// Create worker pool for CPU-heavy tasks
const computePool = new WorkerPool('./workers/compute.js', { size: 4 });
await computePool.start();

// Offload CPU-heavy endpoint to worker
app.post('/api/analyze', async (req, res) => {
    try {
        const result = await computePool.execute({
            type: 'analyze',
            data: req.body,
        });
        res.json({ result });
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// Regular endpoint (no worker needed — I/O bound)
app.get('/api/users', async (req, res) => {
    const users = await db.query('SELECT * FROM users LIMIT 100');
    res.json(users);
});

// Worker stats endpoint
app.get('/api/workers/stats', (req, res) => {
    res.json(computePool.getStats());
});

app.listen(3000);
```

## Database Operations with Workers

```js
// workers/db-worker.js — Heavy database operations in worker
import { parentPort, workerData } from 'node:worker_threads';
import { Pool } from 'pg';

// Each worker gets its own connection pool
const pool = new Pool({
    connectionString: workerData.databaseUrl,
    max: 2, // Smaller pool per worker
});

const handlers = {
    // Heavy aggregation query
    async aggregation({ table, groupBy, dateRange }) {
        const { rows } = await pool.query(`
            SELECT ${groupBy}, COUNT(*) as count, SUM(amount) as total
            FROM ${table}
            WHERE created_at BETWEEN $1 AND $2
            GROUP BY ${groupBy}
            ORDER BY total DESC
        `, [dateRange.start, dateRange.end]);
        return rows;
    },

    // Large data export
    async export({ table, format }) {
        const { rows } = await pool.query(`SELECT * FROM ${table}`);
        if (format === 'csv') {
            const headers = Object.keys(rows[0]).join(',');
            const lines = rows.map(r => Object.values(r).join(','));
            return [headers, ...lines].join('\n');
        }
        return rows;
    },

    // Bulk data transformation
    async transform({ query: sql, transformFn }) {
        const { rows } = await pool.query(sql);
        return rows.map(eval(transformFn)); // Be careful with eval in production
    },
};

parentPort.on('message', async ({ id, type, data }) => {
    try {
        const handler = handlers[type];
        if (!handler) throw new Error(`Unknown type: ${type}`);
        const result = await handler(data);
        parentPort.postMessage({ id, result });
    } catch (err) {
        parentPort.postMessage({ id, error: err.message });
    }
});
```

## Stream Processing with Workers

```js
// stream-worker.js — Process stream chunks in parallel workers
import { Transform } from 'node:stream';
import { Worker } from 'node:worker_threads';

class ParallelTransform extends Transform {
    constructor(workerPath, options = {}) {
        super({ objectMode: true });
        this.concurrency = options.concurrency || 4;
        this.workers = [];
        this.taskQueue = [];
        this.activeTasks = 0;
    }

    async _construct(callback) {
        for (let i = 0; i < this.concurrency; i++) {
            const worker = new Worker(this.workerPath);
            worker.on('message', (msg) => this.onWorkerDone(worker, msg));
            this.workers.push({ worker, busy: false });
        }
        callback();
    }

    _transform(chunk, encoding, callback) {
        const available = this.workers.find(w => !w.busy);
        if (available) {
            available.busy = true;
            available.currentCallback = callback;
            available.worker.postMessage(chunk);
        } else {
            this.taskQueue.push({ chunk, callback });
        }
    }

    onWorkerDone(workerInfo, result) {
        workerInfo.busy = false;
        if (workerInfo.currentCallback) {
            this.push(result);
            workerInfo.currentCallback();
            workerInfo.currentCallback = null;
        }

        // Process queued tasks
        if (this.taskQueue.length > 0) {
            const { chunk, callback } = this.taskQueue.shift();
            workerInfo.busy = true;
            workerInfo.currentCallback = callback;
            workerInfo.worker.postMessage(chunk);
        }
    }

    async _flush(callback) {
        await Promise.all(this.workers.map(w => w.worker.terminate()));
        callback();
    }
}

// Usage
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

await pipeline(
    createReadStream('large-data.jsonl'),
    new LineParser(),
    new ParallelTransform('./workers/transform-worker.js', { concurrency: 4 }),
    new JsonSerializer(),
    createWriteStream('processed.jsonl')
);
```

## Message Queue Integration

```js
// queue-worker.js — Process message queue tasks with worker threads
import { WorkerPool } from './lib/worker-pool.js';
import amqp from 'amqplib';

const pool = new WorkerPool('./workers/queue-handler.js', { size: 4 });
await pool.start();

const connection = await amqp.connect(process.env.RABBITMQ_URL);
const channel = await connection.createChannel();

await channel.assertQueue('tasks', { durable: true });
channel.prefetch(4); // Match pool size

console.log('Waiting for tasks...');

channel.consume('tasks', async (msg) => {
    if (!msg) return;

    try {
        const task = JSON.parse(msg.content.toString());
        const result = await pool.execute(task);
        console.log('Task completed:', result);
        channel.ack(msg);
    } catch (err) {
        console.error('Task failed:', err);
        channel.nack(msg, false, false); // Don't requeue
    }
});
```

## Common Mistakes

- Using workers for I/O-bound database queries (async already handles this)
- Not closing worker database connections on shutdown
- Creating too many workers for stream processing
- Not handling backpressure in parallel stream transforms

## Try It Yourself

### Exercise 1: Express Worker Endpoint
Create an Express endpoint that offloads JSON parsing of a 100MB file to a worker.

### Exercise 2: Parallel CSV Processing
Process a 1GB CSV file with 4 parallel workers. Measure throughput.

### Exercise 3: Queue Worker
Set up RabbitMQ and process 1000 tasks with a 4-worker pool.

## Next Steps

Continue to [Testing Strategies](../10-testing-strategies/01-worker-testing.md).
