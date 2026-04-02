# Retries & Backoff

## What You'll Learn

- How to configure automatic job retries in BullMQ
- How exponential backoff prevents thundering herd on failures
- How to handle the `failed` event and inspect error details
- How to manually retry failed jobs
- How to clean up old completed and failed jobs

## Why Retries?

Jobs fail for transient reasons: network timeouts, database locks, rate limits. Without retries, a single temporary failure kills the job permanently. With retries, BullMQ automatically re-queues the job after a delay.

```
Job 1 → Active → Failed → Wait 1s → Active → Failed → Wait 5s → Active → Completed ✓
```

## Retry Configuration

```js
// retry-demo.js — Queue with retry and backoff settings

import { Queue, Worker, QueueEvents } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };

const taskQueue = new Queue('tasks', { connection });

// Add a job with retry options
const job = await taskQueue.add(
  'unreliable-task',
  { input: 'data' },
  {
    attempts: 5,  // Try up to 5 times total (1 initial + 4 retries)

    // Backoff strategy: wait longer between each retry
    backoff: {
      type: 'exponential',  // 1s, 2s, 4s, 8s, ...
      delay: 1000,          // Base delay in milliseconds
    },

    // Remove the job from Redis after it completes (saves memory)
    removeOnComplete: {
      count: 100,  // Keep only the last 100 completed jobs
      age: 3600,   // Remove completed jobs older than 1 hour
    },

    // Keep failed jobs for inspection
    removeOnFail: {
      count: 500,  // Keep up to 500 failed jobs
    },
  }
);

console.log(`Job ${job.id} added with 5 retry attempts`);

// Worker that randomly fails to demonstrate retries
let attemptCount = 0;

const worker = new Worker(
  'tasks',
  async (job) => {
    attemptCount++;
    console.log(`Attempt ${job.attemptsMade + 1} for job ${job.id}`);

    // 60% chance of failure — simulates an unreliable service
    if (Math.random() < 0.6) {
      throw new Error(`Transient failure on attempt ${job.attemptsMade + 1}`);
    }

    return { success: true, attempts: job.attemptsMade + 1 };
  },
  { connection }
);

worker.on('completed', (job, result) => {
  console.log(`Job ${job.id} succeeded after ${result.attempts} attempt(s)`);
});

worker.on('failed', (job, err) => {
  // job.attemptsMade is 0-indexed — add 1 for human-readable count
  const currentAttempt = job.attemptsMade + 1;
  console.log(
    `Job ${job.id} failed on attempt ${currentAttempt}/${job.opts.attempts}: ${err.message}`
  );

  if (currentAttempt >= job.opts.attempts) {
    console.log(`Job ${job.id} exhausted all retries — permanently failed`);
  }
});

// Wait for the job to complete or permanently fail
const queueEvents = new QueueEvents('tasks', { connection });
await queueEvents.waitUntilReady();

try {
  const result = await job.waitUntilFinished(queueEvents);
  console.log('Final result:', result);
} catch (err) {
  console.log('Job permanently failed:', err.message);
}

await worker.close();
await taskQueue.close();
await queueEvents.close();
```

## Backoff Strategies

| Strategy | Formula | Example (delay=1000) |
|----------|---------|---------------------|
| `fixed` | delay × attempt | 1s, 2s, 3s, 4s, 5s |
| `exponential` | delay × 2^attempt | 1s, 2s, 4s, 8s, 16s |
| Custom function | Your formula | Anything you want |

### Custom Backoff

```js
// custom-backoff.js — Custom backoff function

const job = await queue.add('task', data, {
  attempts: 5,
  backoff: {
    type: 'custom',
    // BullMQ calls this function with attemptsMade (0-indexed) and err
    // Return the delay in milliseconds
    delay: (attemptsMade, err) => {
      // Exponential with jitter to prevent thundering herd
      const base = 1000 * Math.pow(2, attemptsMade);
      const jitter = Math.random() * 1000;  // 0-1000ms random jitter
      return base + jitter;
    },
  },
});
```

## Manual Retry

```js
// manual-retry.js — Inspect and retry failed jobs

import { Queue, QueueEvents } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };
const queue = new Queue('tasks', { connection });

// Get all failed jobs
const failedJobs = await queue.getFailed();

console.log(`Found ${failedJobs.length} failed jobs:`);

for (const job of failedJobs) {
  console.log(`  Job ${job.id}: ${job.name} — ${job.failedReason}`);

  // Retry a specific failed job
  await job.retry();
  console.log(`  → Retried job ${job.id}`);

  // Or remove it permanently
  // await job.remove();
}

await queue.close();
```

## Cleaning Up Old Jobs

```js
// cleanup.js — Remove old completed and failed jobs

import { Queue } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };
const queue = new Queue('tasks', { connection });

// Clean jobs older than 1 hour
await queue.clean(3600_000, 100, 'completed');  // (grace, limit, type)
await queue.clean(3600_000, 100, 'failed');

console.log('Old jobs cleaned');

// Drain the queue — remove ALL waiting jobs
// await queue.drain();

// Obliterate — remove the queue and ALL its data from Redis
// await queue.obliterate({ force: true });

await queue.close();
```

## How It Works

### Retry Flow

```
Job fails (attempt 1)
  → BullMQ checks: attemptsMade < job.opts.attempts?
    → Yes: calculate backoff delay
           Add job to delayed set with retry timestamp
           Worker picks up next waiting job
           When delay passes, move job back to waiting
    → No:  Move job to failed set
           Emit 'failed' event
```

### The attemptsMade Counter

- `job.attemptsMade` starts at `0` for the first attempt
- After the first failure, it becomes `1`
- If `attemptsMade + 1 >= attempts`, the job is permanently failed

## Common Mistakes

### Mistake 1: No Backoff

```js
// WRONG — immediate retry hammers the failing service
await queue.add('task', data, { attempts: 10 });

// CORRECT — add exponential backoff
await queue.add('task', data, {
  attempts: 10,
  backoff: { type: 'exponential', delay: 1000 },
});
```

### Mistake 2: Too Many Retries

```js
// WRONG — 100 retries for a permanent error wastes resources
await queue.add('task', data, { attempts: 100 });

// CORRECT — match retries to the error type
// Transient errors (network): 3-5 retries
// Permanent errors (bad data): 0 retries, log and move on
await queue.add('task', data, {
  attempts: 3,
  backoff: { type: 'exponential', delay: 1000 },
});
```

### Mistake 3: Not Handling Permanent Failures

```js
// WRONG — failed jobs accumulate in Redis forever
worker.on('failed', (job) => {
  console.log('Failed:', job.id);
  // No action taken — Redis fills up with failed jobs
});

// CORRECT — alert on permanent failure and clean up
worker.on('failed', async (job, err) => {
  if (job.attemptsMade + 1 >= job.opts.attempts) {
    console.error(`Permanently failed: ${job.id} — alerting...`);
    // Send alert (email, Slack, etc.)
    // Optionally remove the job
    await job.remove();
  }
});
```

## Try It Yourself

### Exercise 1: Simulate Transient Failures

Create a worker that fails 70% of the time. Configure 5 retries with exponential backoff. Log each attempt. How many attempts does it typically take to succeed?

### Exercise 2: Retry Dashboard

Add a route `GET /queue-stats` that returns counts of waiting, active, completed, and failed jobs using `queue.getJobCounts()`.

### Exercise 3: Dead Letter Queue

When a job permanently fails (all retries exhausted), add it to a separate queue called `dead-letter`. Create a worker that logs dead-letter jobs for manual inspection.

## Next Steps

You understand retries. For scheduled recurring jobs, continue to [Cron Jobs](./03-cron-jobs.md).
