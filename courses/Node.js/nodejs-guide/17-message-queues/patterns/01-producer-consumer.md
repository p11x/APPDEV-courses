# Producer-Consumer Pattern

## What You'll Learn

- What the producer-consumer pattern is and when to use it
- How to decouple HTTP request handling from background processing
- How to report job progress back to the client
- How to design job data shapes for flexibility
- How to monitor queue health

## The Pattern

A **producer** adds jobs to a queue. A **consumer** (worker) processes them. They do not know about each other — the queue is the intermediary.

```
Producer (API Server)          Consumer (Worker Process)
─────────────────────          ────────────────────────
POST /orders                   Process order
  → Add job to queue           → Charge payment
  → Return 202 Accepted       → Update inventory
                               → Send confirmation email
```

This is essential for:
- **Decoupling** — API and worker can scale independently
- **Reliability** — jobs survive crashes (stored in Redis)
- **User experience** — user gets an immediate response

## Project Structure

```
01-producer-consumer/
├── server.js        # Express API that produces jobs
├── worker.js        # Background worker that consumes jobs
└── monitor.js       # Queue monitoring utility
```

## Producer: Express API

```js
// server.js — API that accepts requests and adds jobs to the queue

import express from 'express';
import { Queue, QueueEvents } from 'bullmq';

const app = express();
app.use(express.json());

const connection = { host: '127.0.0.1', port: 6379 };

// Create a queue instance for adding jobs
const orderQueue = new Queue('orders', { connection });

// POST /orders — create an order (adds a job, returns immediately)
app.post('/orders', async (req, res) => {
  const { items, customerEmail } = req.body;

  if (!items?.length || !customerEmail) {
    return res.status(400).json({ error: 'items and customerEmail required' });
  }

  // Calculate total
  const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);

  // Add the job to the queue
  const job = await orderQueue.add('process-order', {
    orderId: `ord_${Date.now()}`,
    items,
    customerEmail,
    total,
    createdAt: new Date().toISOString(),
  }, {
    // Cleanup: keep only recent jobs
    removeOnComplete: { count: 100 },
    removeOnFail: { count: 200 },
    attempts: 3,  // Retry up to 3 times on failure
    backoff: { type: 'exponential', delay: 2000 },
  });

  // Return 202 Accepted — the order is being processed asynchronously
  res.status(202).json({
    message: 'Order accepted',
    jobId: job.id,
    statusUrl: `/orders/${job.id}/status`,
  });
});

// GET /orders/:jobId/status — check job status
app.get('/orders/:jobId/status', async (req, res) => {
  const job = await orderQueue.getJob(req.params.jobId);

  if (!job) {
    return res.status(404).json({ error: 'Job not found' });
  }

  const state = await job.getState();  // 'waiting', 'active', 'completed', 'failed', 'delayed'

  res.json({
    jobId: job.id,
    state,
    progress: job.progress,      // 0-100 from worker's job.updateProgress()
    data: job.data,
    result: job.returnvalue,     // Set after completion
    failedReason: job.failedReason,  // Error message if failed
    attemptsMade: job.attemptsMade,
  });
});

// GET /queue/stats — queue health
app.get('/queue/stats', async (req, res) => {
  const counts = await orderQueue.getJobCounts(
    'waiting', 'active', 'completed', 'failed', 'delayed'
  );
  res.json(counts);
});

app.listen(3000, () => {
  console.log('Producer API on http://localhost:3000');
});

// Graceful shutdown
process.on('SIGINT', async () => {
  await orderQueue.close();
  process.exit(0);
});
```

## Consumer: Background Worker

```js
// worker.js — Processes orders from the queue

import { Worker } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };

const worker = new Worker(
  'orders',
  async (job) => {
    const { orderId, items, customerEmail, total } = job.data;

    console.log(`Processing order ${orderId} for ${customerEmail}`);

    // Step 1: Validate order (20% of progress)
    await job.updateProgress(10);
    await validateOrder(items);

    await job.updateProgress(20);

    // Step 2: Charge payment (40%)
    console.log(`  Charging $${total}...`);
    await simulateDelay(1000);
    await job.updateProgress(40);

    // Step 3: Update inventory (60%)
    console.log('  Updating inventory...');
    await simulateDelay(800);
    await job.updateProgress(60);

    // Step 4: Send confirmation email (80%)
    console.log(`  Sending confirmation to ${customerEmail}...`);
    await simulateDelay(500);
    await job.updateProgress(80);

    // Step 5: Finalize (100%)
    await job.updateProgress(100);
    console.log(`  Order ${orderId} complete!`);

    return {
      orderId,
      status: 'completed',
      completedAt: new Date().toISOString(),
    };
  },
  {
    connection,
    concurrency: 3,  // Process 3 orders simultaneously
  }
);

async function validateOrder(items) {
  // Simulate validation
  await simulateDelay(300);
  for (const item of items) {
    if (item.quantity <= 0) throw new Error(`Invalid quantity for ${item.name}`);
  }
}

function simulateDelay(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

worker.on('completed', (job, result) => {
  console.log(`Order ${result.orderId} completed successfully`);
});

worker.on('failed', (job, err) => {
  console.error(`Order ${job.data.orderId} failed: ${err.message}`);
});

console.log('Order worker started — waiting for orders...');

process.on('SIGINT', async () => {
  await worker.close();
  process.exit(0);
});
```

## Monitor: Queue Dashboard

```js
// monitor.js — Print queue stats every 5 seconds

import { Queue } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };
const queue = new Queue('orders', { connection });

async function printStats() {
  const counts = await queue.getJobCounts(
    'waiting', 'active', 'completed', 'failed', 'delayed'
  );

  console.clear();
  console.log('=== Order Queue Monitor ===');
  console.log(`  Waiting:    ${counts.waiting}`);
  console.log(`  Active:     ${counts.active}`);
  console.log(`  Completed:  ${counts.completed}`);
  console.log(`  Failed:     ${counts.failed}`);
  console.log(`  Delayed:    ${counts.delayed}`);
  console.log('==========================');
}

// Print stats every 5 seconds
const interval = setInterval(printStats, 5000);
printStats();

process.on('SIGINT', async () => {
  clearInterval(interval);
  await queue.close();
  process.exit(0);
});
```

## Running the System

```bash
# Terminal 1: start the worker
node worker.js

# Terminal 2: start the API
node server.js

# Terminal 3: monitor the queue
node monitor.js

# Terminal 4: send orders
curl -X POST http://localhost:3000/orders \
  -H "Content-Type: application/json" \
  -d '{"items":[{"name":"Widget","price":9.99,"quantity":2}],"customerEmail":"alice@test.com"}'

# Check job status
curl http://localhost:3000/orders/<jobId>/status
```

## How It Works

### Request Flow

```
Client → POST /orders → Add job to queue → Return 202
                                        ↓
Worker picks up job → Process → Complete → Result stored in Redis
                                        ↓
Client → GET /orders/:id/status → Return result
```

### The 202 Accepted Pattern

The API returns `202 Accepted` (not `200 OK`) to signal that the request was accepted but not yet processed. The client polls the status endpoint or uses WebSockets (Chapter 14) to get the result.

## Common Mistakes

### Mistake 1: Processing in the Request Handler

```js
// WRONG — user waits 30 seconds for the order to process
app.post('/orders', async (req, res) => {
  await processOrder(req.body);  // Blocks the response
  res.json({ done: true });
});

// CORRECT — add to queue, return immediately
app.post('/orders', async (req, res) => {
  const job = await queue.add('process-order', req.body);
  res.status(202).json({ jobId: job.id });
});
```

### Mistake 2: No Dead Letter Handling

```js
// WRONG — if all retries fail, the job disappears silently
worker.on('failed', (job) => {
  // No logging or alerting — lost orders!
});

// CORRECT — log and alert on permanent failures
worker.on('failed', async (job, err) => {
  if (job.attemptsMade + 1 >= job.opts.attempts) {
    console.error(`PERMANENT FAILURE: Order ${job.data.orderId} — ${err.message}`);
    // Alert ops team
  }
});
```

### Mistake 3: Tightly Coupled Job Data

```js
// WRONG — including the database connection in job data
await queue.add('task', { db: databaseConnection, userId: '1' });
// Cannot serialize — throws when saving to Redis

// CORRECT — only pass serializable identifiers
await queue.add('task', { userId: '1' });
// Worker creates its own DB connection
```

## Try It Yourself

### Exercise 1: Email Queue

Create a producer that accepts `POST /send-email` with `{ to, subject, body }`. The worker simulates sending the email with a 2-second delay. Return a status URL that the client can poll.

### Exercise 2: Batch Import

Create `POST /import` that accepts a CSV file path. The worker reads the file and processes each row as a separate job. Report overall progress.

### Exercise 3: Priority Queue

Add jobs with different priorities. High-priority jobs (e.g., password reset emails) are processed before low-priority ones (e.g., newsletter).

## Next Steps

You have a reliable producer-consumer system. For handling permanently failed messages, continue to [Dead Letter Queue](./02-dead-letter-queue.md).
