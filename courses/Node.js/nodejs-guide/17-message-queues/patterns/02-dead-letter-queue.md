# Dead Letter Queue

## What You'll Learn

- What a dead letter queue (DLQ) is and why it matters
- How to route permanently failed jobs to a DLQ
- How to inspect, reprocess, or discard dead-lettered jobs
- How to set up alerting on poison messages
- How the DLQ prevents data loss

## What Is a Dead Letter Queue?

When a job fails all its retries, it is **permanently failed**. Without a DLQ, it sits in the failed list and is eventually cleaned up — the data is lost.

A **dead letter queue** captures permanently failed jobs so you can inspect them, fix the root cause, and reprocess them.

```
Main Queue → Worker → Failed → Retry → Failed → Retry → Failed (all attempts exhausted)
                                                              ↓
                                                    Dead Letter Queue (DLQ)
                                                              ↓
                                                    Manual inspection / reprocessing
```

## Implementation

```js
// dlq-demo.js — Dead letter queue pattern with BullMQ

import { Queue, Worker, QueueEvents } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };

// Main queue
const mainQueue = new Queue('orders', { connection });

// Dead letter queue — captures permanently failed jobs
const dlq = new Queue('orders-dead-letter', { connection });

// Worker processes jobs from the main queue
const worker = new Worker(
  'orders',
  async (job) => {
    const { orderId, email } = job.data;

    console.log(`Processing order ${orderId}...`);

    // Simulate a failure that always happens (e.g., external service down)
    if (email === 'bad@example.com') {
      throw new Error('Payment gateway unavailable');
    }

    await new Promise((r) => setTimeout(r, 500));
    return { orderId, status: 'completed' };
  },
  {
    connection,
    attempts: 3,  // Retry 3 times before permanent failure
    backoff: { type: 'exponential', delay: 1000 },
  }
);

// When a job permanently fails, route it to the DLQ
worker.on('failed', async (job, err) => {
  if (!job) return;  // job can be null for some error scenarios

  const isPermanentlyFailed = job.attemptsMade + 1 >= (job.opts.attempts || 1);

  if (isPermanentlyFailed) {
    console.log(`Job ${job.id} permanently failed — routing to DLQ`);

    // Add the failed job to the dead letter queue with failure metadata
    await dlq.add(
      job.name,
      {
        // Original job data
        ...job.data,
        // Add failure metadata for debugging
        _dlq: {
          originalJobId: job.id,
          originalQueue: 'orders',
          failedAt: new Date().toISOString(),
          attempts: job.attemptsMade + 1,
          error: err.message,
          stack: err.stack,
        },
      },
      {
        // DLQ jobs do not retry — they are for manual inspection
        attempts: 1,
        removeOnComplete: false,  // Keep DLQ jobs forever (for audit)
      }
    );
  }
});

console.log('Worker started — waiting for jobs...');

// === Test the DLQ ===

// Add some jobs
await mainQueue.add('process-order', {
  orderId: 'ord_001',
  email: 'good@example.com',
});

await mainQueue.add('process-order', {
  orderId: 'ord_002',
  email: 'bad@example.com',  // This will fail and go to DLQ
});

// Wait for processing
await new Promise((r) => setTimeout(r, 10_000));

// Check the DLQ
const deadJobs = await dlq.getJobs();
console.log(`\nDead letter queue has ${deadJobs.length} job(s):`);

for (const job of deadJobs) {
  console.log(`  Job ${job.id}: ${job.data.orderId}`);
  console.log(`    Error: ${job.data._dlq.error}`);
  console.log(`    Attempts: ${job.data._dlq.attempts}`);
  console.log(`    Failed at: ${job.data._dlq.failedAt}`);
}

await worker.close();
await mainQueue.close();
await dlq.close();
```

## Reprocessing Dead-Lettered Jobs

```js
// reprocess-dlq.js — Inspect and reprocess dead-lettered jobs

import { Queue, QueueEvents } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };

const dlq = new Queue('orders-dead-letter', { connection });
const mainQueue = new Queue('orders', { connection });

// List all dead-lettered jobs
const deadJobs = await dlq.getFailed();
const completed = await dlq.getCompleted();

console.log(`DLQ: ${deadJobs.length} failed, ${completed.length} completed`);

// Reprocess a specific job
const allDead = await dlq.getJobs();
for (const job of allDead) {
  console.log(`\nReprocessing: ${job.data.orderId}`);

  // Remove the DLQ metadata before re-adding to the main queue
  const { _dlq, ...originalData } = job.data;

  // Add back to the main queue with fresh retry attempts
  const newJob = await mainQueue.add(job.name, originalData, {
    attempts: 3,
    backoff: { type: 'exponential', delay: 1000 },
  });

  console.log(`  New job ID: ${newJob.id}`);

  // Remove from DLQ
  await job.remove();
  console.log('  Removed from DLQ');
}

await dlq.close();
await mainQueue.close();
```

## Alerting on Poison Messages

```js
// dlq-alert.js — Alert when jobs land in the DLQ

import { Worker } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };

// A worker on the DLQ that alerts on new dead-lettered jobs
const dlqWorker = new Worker(
  'orders-dead-letter',
  async (job) => {
    const { orderId, _dlq } = job.data;

    // Log the dead-lettered job
    console.error('=== DEAD LETTER ALERT ===');
    console.error(`Order: ${orderId}`);
    console.error(`Error: ${_dlq.error}`);
    console.error(`Attempts: ${_dlq.attempts}`);
    console.error(`Original job: ${_dlq.originalJobId}`);
    console.error('========================');

    // In production, send an alert:
    // - Email to ops team
    // - Slack webhook
    // - PagerDuty incident
    // - Sentry event

    // Example: Slack webhook (uncomment and configure)
    // await fetch(process.env.SLACK_WEBHOOK_URL, {
    //   method: 'POST',
    //   headers: { 'Content-Type': 'application/json' },
    //   body: JSON.stringify({
    //     text: `Dead letter alert: Order ${orderId} failed after ${_dlq.attempts} attempts. Error: ${_dlq.error}`,
    //   }),
    // });

    return { alerted: true };
  },
  { connection }
);

dlqWorker.on('completed', (job) => {
  console.log(`DLQ alert processed for job ${job.id}`);
});

console.log('DLQ alert worker started');
```

## How It Works

### The Full Failure Flow

```
Job added → Worker processes → Failure 1 → Retry (1s delay)
  → Worker processes → Failure 2 → Retry (2s delay)
  → Worker processes → Failure 3 → Permanently failed
    → 'failed' event fires
      → Check: all attempts exhausted?
        → Yes: add to DLQ with error metadata
        → No: BullMQ handles retry automatically
```

### DLQ Job Data Structure

```json
{
  "orderId": "ord_002",
  "email": "bad@example.com",
  "_dlq": {
    "originalJobId": "3",
    "originalQueue": "orders",
    "failedAt": "2024-01-15T10:30:00.000Z",
    "attempts": 3,
    "error": "Payment gateway unavailable",
    "stack": "Error: Payment gateway unavailable\n    at ..."
  }
}
```

The `_dlq` metadata preserves everything you need to diagnose and reprocess.

## Common Mistakes

### Mistake 1: No DLQ — Jobs Disappear Silently

```js
// WRONG — permanent failures are lost
worker.on('failed', (job) => {
  console.log('Failed:', job.id);
  // Job is in the failed set but eventually gets cleaned up
});

// CORRECT — route to DLQ for manual review
worker.on('failed', async (job, err) => {
  if (job.attemptsMade + 1 >= job.opts.attempts) {
    await dlq.add(job.name, { ...job.data, _dlq: { error: err.message, ... } });
  }
});
```

### Mistake 2: Retrying DLQ Jobs Automatically

```js
// WRONG — DLQ jobs retrying creates an infinite loop
const dlqWorker = new Worker('dlq', async (job) => {
  // Retrying the same job that already failed 3 times
  await mainQueue.add(job.name, job.data);  // Will fail again!
}, { attempts: 3 });

// CORRECT — DLQ jobs are for manual inspection, not automatic retry
const dlqWorker = new Worker('dlq', async (job) => {
  // Alert a human — they decide whether to reprocess
  await sendAlert(job);
  return { alerted: true };
}, { attempts: 1 });  // DLQ worker should not retry
```

### Mistake 3: Not Cleaning Up Reprocessed Jobs

```js
// WRONG — after reprocessing, the DLQ job stays in Redis forever
const newJob = await mainQueue.add(job.name, job.data);
// DLQ still has the old job — duplicates!

// CORRECT — remove from DLQ after reprocessing
const newJob = await mainQueue.add(job.name, job.data);
await job.remove();  // Remove from DLQ
```

## Try It Yourself

### Exercise 1: DLQ Alert Dashboard

Create a `GET /dlq` endpoint that lists all dead-lettered jobs with their error messages and timestamps.

### Exercise 2: Selective Reprocessing

Add a `POST /dlq/:id/reprocess` endpoint that takes a dead-lettered job ID, removes the DLQ metadata, and re-adds it to the main queue.

### Exercise 3: DLQ Metrics

Track how many jobs land in the DLQ per hour. Alert if the rate exceeds a threshold (e.g., 10 jobs/hour).

## Next Steps

You understand dead letter queues. For file upload handling, continue to [Chapter 18: File Uploads](../../18-file-uploads/local-uploads/01-multer-setup.md).
