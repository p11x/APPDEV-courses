# BullMQ Repeatable Jobs

## What You'll Learn

- How to schedule recurring jobs with BullMQ
- How to use interval-based and cron-based repeat options
- How to manage and remove repeatable jobs
- How BullMQ repeatable jobs compare to node-cron
- How to prevent duplicate schedules

## Why BullMQ for Scheduling?

`node-cron` is simple but has limitations in production:
- No built-in locking (duplicate runs across processes)
- No retry logic
- No job history

BullMQ provides all of these. If you already use BullMQ for async jobs (Chapter 17), use it for scheduling too.

> See: ../../17-message-queues/bullmq/01-bullmq-setup.md for BullMQ basics.

## Project Setup

```bash
npm install bullmq
```

## Interval-Based Scheduling

```js
// interval-schedule.js — Repeat jobs at fixed intervals

import { Queue, Worker } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };
const queue = new Queue('scheduled', { connection });

// Add a repeatable job — runs every 30 seconds
await queue.add(
  'health-check',
  { service: 'api' },
  {
    repeat: {
      every: 30_000,  // Run every 30 seconds
    },
    // Stable jobId prevents duplicates on app restart
    jobId: 'health-check-api',
    removeOnComplete: { count: 10 },
    removeOnFail: { count: 50 },
  }
);

console.log('Health check scheduled every 30 seconds');

// Worker processes the scheduled jobs
const worker = new Worker(
  'scheduled',
  async (job) => {
    console.log(`[${new Date().toISOString()}] ${job.name}`, job.data);

    // Simulate health check
    const healthy = Math.random() > 0.1;
    if (!healthy) throw new Error('Service unhealthy');

    return { healthy: true, latency: Math.round(Math.random() * 100) };
  },
  { connection }
);

worker.on('completed', (job, result) => {
  console.log(`  → healthy: ${result.healthy}, latency: ${result.latency}ms`);
});

worker.on('failed', (job, err) => {
  console.error(`  → failed: ${err.message}`);
});
```

## Cron-Based Scheduling

```js
// cron-schedule.js — Repeat jobs with cron expressions

import { Queue, Worker } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };
const queue = new Queue('cron-jobs', { connection });

// Daily at 9 AM Eastern
await queue.add(
  'daily-report',
  { type: 'sales' },
  {
    repeat: {
      pattern: '0 9 * * *',          // Cron expression
      tz: 'America/New_York',         // Timezone
    },
    jobId: 'daily-report-sales',      // Stable ID prevents duplicates
    removeOnComplete: { count: 7 },   // Keep last 7 results
  }
);

// Every hour
await queue.add(
  'cleanup',
  {},
  {
    repeat: {
      pattern: '0 * * * *',
    },
    jobId: 'hourly-cleanup',
  }
);

// Weekdays at 8 AM
await queue.add(
  'standup-reminder',
  { channel: 'engineering' },
  {
    repeat: {
      pattern: '0 8 * * 1-5',
      tz: 'America/New_York',
    },
    jobId: 'standup-reminder',
  }
);

console.log('Cron jobs scheduled');

const worker = new Worker(
  'cron-jobs',
  async (job) => {
    console.log(`[${new Date().toISOString()}] Running: ${job.name}`);

    switch (job.name) {
      case 'daily-report':
        await generateReport(job.data.type);
        return { rows: 1500 };
      case 'cleanup':
        await cleanup();
        return { deleted: 42 };
      case 'standup-reminder':
        await sendReminder(job.data.channel);
        return { sent: true };
    }
  },
  { connection }
);

async function generateReport(type) {
  await new Promise((r) => setTimeout(r, 1000));
}

async function cleanup() {
  await new Promise((r) => setTimeout(r, 500));
}

async function sendReminder(channel) {
  await new Promise((r) => setTimeout(r, 200));
}
```

## Managing Repeatable Jobs

```js
// manage-repeatable.js — List and remove repeatable jobs

import { Queue } from 'bullmq';

const connection = { host: '127.0.0.1', port: 6379 };
const queue = new Queue('cron-jobs', { connection });

// List all repeatable jobs
const repeatable = await queue.getRepeatableJobs();

console.log('Repeatable jobs:');
for (const job of repeatable) {
  console.log(`  ${job.name}`);
  console.log(`    Pattern: ${job.pattern}`);
  console.log(`    Timezone: ${job.tz || 'server default'}`);
  console.log(`    Next: ${new Date(job.next).toISOString()}`);
  console.log(`    Key: ${job.key}`);
}

// Remove a specific repeatable job
await queue.removeRepeatable('daily-report', {
  pattern: '0 9 * * *',
  tz: 'America/New_York',
});
console.log('Removed daily-report');

// Remove by key
await queue.removeRepeatableByKey('daily-report:::0 9 * * *:::America/New_York');

await queue.close();
```

## How It Works

### Repeat vs node-cron

| Feature | node-cron | BullMQ repeat |
|---------|----------|---------------|
| Distributed locking | No (manual) | Automatic (Redis) |
| Retry on failure | No | Yes (built-in) |
| Job history | No | Yes (completed/failed) |
| Timezone | Yes | Yes |
| Job queue | No | Yes (priorities, concurrency) |

### Duplicate Prevention

```js
// Without jobId — each restart adds a new repeatable job
await queue.add('report', data, { repeat: { pattern: '0 9 * * *' } });
// After 10 restarts, 10 jobs fire at 9 AM!

// With jobId — deduplicates automatically
await queue.add('report', data, {
  repeat: { pattern: '0 9 * * *' },
  jobId: 'daily-report-9am',  // Same ID on every restart
});
```

## Common Mistakes

### Mistake 1: No jobId on Repeatable Jobs

```js
// WRONG — duplicates on every restart
await queue.add('report', data, { repeat: { every: 60000 } });

// CORRECT — stable jobId
await queue.add('report', data, {
  repeat: { every: 60000 },
  jobId: 'report-minute',
});
```

### Mistake 2: Missing removeOnComplete

```js
// WRONG — old job results accumulate in Redis forever
await queue.add('report', data, { repeat: { every: 60000 } });

// CORRECT — limit stored results
await queue.add('report', data, {
  repeat: { every: 60000 },
  removeOnComplete: { count: 100 },
  removeOnFail: { count: 500 },
});
```

### Mistake 3: Using both node-cron and BullMQ

```js
// WRONG — conflicting schedules
cron.schedule('0 9 * * *', sendReport);
queue.add('report', data, { repeat: { pattern: '0 9 * * *' } });
// Report sent twice!

// CORRECT — use one scheduling system
```

## Try It Yourself

### Exercise 1: Health Monitor

Schedule a health check every 10 seconds. Log the result. Add retry logic: if it fails, retry 3 times.

### Exercise 2: Job Dashboard

Create an endpoint `GET /jobs` that lists all repeatable jobs with their next run time.

### Exercise 3: Pause/Resume

Pause the queue so scheduled jobs stop executing. Wait 30 seconds, then resume. Verify jobs run again.

## Next Steps

You understand scheduled jobs with BullMQ. For CI/CD automation, continue to [Chapter 26: CI/CD with GitHub Actions](../../26-cicd-github-actions/github-actions/01-workflow-basics.md).
