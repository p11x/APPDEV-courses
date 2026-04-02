# Cron Jobs with BullMQ

## What You'll Learn

- How to schedule recurring jobs with BullMQ
- How cron expressions work and how to write them
- How to use the `repeat` option for interval-based and cron-based scheduling
- How to manage and remove repeatable jobs
- How to prevent duplicate cron job runs with `repeatJobKey`

## Scheduling Recurring Jobs

BullMQ can add jobs automatically on a schedule. You do not need a separate cron library — BullMQ handles it with Redis.

```js
// cron-setup.js — Schedule recurring jobs

import { Queue, Worker, QueueEvents } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };
const reportQueue = new Queue('reports', { connection });

// === Interval-Based Scheduling ===

// Add a job that repeats every 30 seconds
const intervalJob = await reportQueue.add(
  'health-check',
  { service: 'api' },
  {
    repeat: {
      every: 30_000,  // Run every 30 seconds
    },
    // Do not pile up — if the previous run is still processing, skip this one
    jobId: 'health-check-api',  // Unique ID prevents duplicates
  }
);

// === Cron-Based Scheduling ===

// Add a job using a cron expression
const cronJob = await reportQueue.add(
  'daily-report',
  { reportType: 'sales' },
  {
    repeat: {
      pattern: '0 9 * * *',  // Every day at 9:00 AM
      // Cron format: minute hour day-of-month month day-of-week
      //   0 9 * * * = at minute 0, hour 9, every day, every month, every weekday
      tz: 'America/New_York',  // Timezone (uses Intl)
    },
    // Cleanup old runs to save memory
    removeOnComplete: { count: 10 },
    removeOnFail: { count: 50 },
  }
);

// === More Cron Patterns ===

// Every minute:     '* * * * *'
// Every 5 minutes:  '*/5 * * * *'
// Every hour:       '0 * * * *'
// Every midnight:   '0 0 * * *'
// Weekdays at 9 AM: '0 9 * * 1-5'
// First of month:   '0 0 1 * *'

console.log('Cron jobs scheduled');

// Worker processes the scheduled jobs
const worker = new Worker(
  'reports',
  async (job) => {
    if (job.name === 'health-check') {
      console.log(`[${new Date().toISOString()}] Health check for ${job.data.service}`);
      // Simulate health check
      return { status: 'healthy', latency: Math.random() * 100 };
    }

    if (job.name === 'daily-report') {
      console.log(`[${new Date().toISOString()}] Generating daily report...`);
      await new Promise((r) => setTimeout(r, 2000));  // Simulate report generation
      return { rows: 1500, generatedAt: new Date().toISOString() };
    }
  },
  { connection }
);

worker.on('completed', (job, result) => {
  console.log(`Job ${job.name} completed:`, result);
});

console.log('Worker started — press Ctrl+C to stop');

// Graceful shutdown
process.on('SIGINT', async () => {
  await worker.close();
  await reportQueue.close();
  process.exit(0);
});
```

## Managing Repeatable Jobs

```js
// manage-cron.js — List and remove repeatable jobs

import { Queue } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };
const queue = new Queue('reports', { connection });

// List all repeatable jobs
const repeatable = await queue.getRepeatableJobs();

console.log('Repeatable jobs:');
for (const job of repeatable) {
  console.log(`  ${job.name} — pattern: ${job.pattern}, next: ${new Date(job.next)}`);
}

// Remove a specific repeatable job by its key
// The key is a hash of the job name + repeat options
await queue.removeRepeatableByKey('daily-report:::0 9 * * *:::America/New_York');

// Or remove by matching the repeat options
await queue.removeRepeatable('daily-report', {
  pattern: '0 9 * * *',
  tz: 'America/New_York',
});

console.log('Removed daily-report cron job');

await queue.close();
```

## How It Works

### How BullMQ Schedules Repeating Jobs

1. When you add a job with `repeat: { pattern: '...' }`, BullMQ calculates the next execution time
2. It adds a **delayed** job with that timestamp
3. When the timestamp arrives, BullMQ moves the job to the **waiting** list
4. A worker picks it up and processes it
5. BullMQ calculates the next execution time and adds another delayed job
6. This cycle repeats forever

### The jobId Trick

If you use the same `jobId` for a repeating job, BullMQ deduplicates. Without `jobId`, if you restart your app, you might create duplicate cron schedules.

```js
// CORRECT — stable jobId prevents duplicate schedules on restart
await queue.add('report', data, {
  repeat: { pattern: '0 9 * * *' },
  jobId: 'daily-report-9am',  // Same ID on every restart
});
```

## Common Mistakes

### Mistake 1: No Timezone

```js
// WRONG — cron runs at server's local time, which may differ from your intent
await queue.add('report', data, {
  repeat: { pattern: '0 9 * * *' },  // 9 AM in what timezone?
});

// CORRECT — always specify timezone
await queue.add('report', data, {
  repeat: { pattern: '0 9 * * *', tz: 'America/New_York' },
});
```

### Mistake 2: Duplicate Schedules on Restart

```js
// WRONG — each app start adds another repeating job
// After 10 restarts, 10 reports fire at 9 AM
app.listen(3000, async () => {
  await queue.add('report', data, { repeat: { pattern: '0 9 * * *' } });
});

// CORRECT — use a stable jobId or check before adding
app.listen(3000, async () => {
  await queue.add('report', data, {
    repeat: { pattern: '0 9 * * *' },
    jobId: 'daily-report',  // Deduplicates
  });
});
```

### Mistake 3: Overlapping Runs

```js
// WRONG — if a job takes 10 minutes but repeats every 5 minutes,
// two instances run simultaneously
await queue.add('long-task', data, {
  repeat: { every: 300_000 },
});

// CORRECT — use a unique jobId so BullMQ skips if the previous run is still active
await queue.add('long-task', data, {
  repeat: { every: 300_000 },
  jobId: 'long-task-unique',
  // BullMQ will not add a new instance if one with this jobId is active or waiting
});
```

## Try It Yourself

### Exercise 1: Multi-Schedule

Schedule three jobs: one every 10 seconds, one every minute (at :00), and one every hour (at :00:00). Log when each fires.

### Exercise 2: Cleanup Cron

Schedule a job that runs every hour and removes completed jobs older than 24 hours using `queue.clean()`.

### Exercise 3: Pause/Resume

Pause the queue so cron jobs stop executing. Add a few scheduled jobs. Resume the queue and verify they fire.

## Next Steps

You can schedule recurring jobs. For decoupling services with queues, continue to [Producer-Consumer Pattern](../patterns/01-producer-consumer.md).
