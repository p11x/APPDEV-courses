# Cron Basics

## What You'll Learn

- What cron expressions are and how to write them
- How to schedule recurring tasks with `node-cron`
- How to start, stop, and manage scheduled jobs
- How to handle timezones in cron schedules
- How to test cron jobs during development

## What Is Cron?

Cron is a time-based job scheduler. You define **when** a task should run using a **cron expression**, and the scheduler runs it automatically.

```
┌───────── minute (0–59)
│ ┌─────── hour (0–23)
│ │ ┌───── day of month (1–31)
│ │ │ ┌─── month (1–12)
│ │ │ │ ┌─ day of week (0–7, 0 and 7 = Sunday)
│ │ │ │ │
* * * * *  command
```

## Project Setup

```bash
mkdir cron-demo && cd cron-demo
npm init -y
npm install node-cron
```

## Basic Scheduling

```js
// scheduler.js — Schedule tasks with node-cron

import cron from 'node-cron';

// Schedule a task to run every minute
const job1 = cron.schedule('* * * * *', () => {
  console.log(`[${new Date().toISOString()}] Running every minute`);
});

// Schedule at a specific time — every day at 9:30 AM
const job2 = cron.schedule('30 9 * * *', () => {
  console.log('Running at 9:30 AM daily');
});

// Every hour at minute 0
const job3 = cron.schedule('0 * * * *', () => {
  console.log('Running at the top of every hour');
});

// Every weekday (Mon-Fri) at 8:00 AM
const job4 = cron.schedule('0 8 * * 1-5', () => {
  console.log('Running at 8 AM on weekdays');
});

// Every 5 minutes
const job5 = cron.schedule('*/5 * * * *', () => {
  console.log('Running every 5 minutes');
});

// First day of every month at midnight
const job6 = cron.schedule('0 0 1 * *', () => {
  console.log('Running on the 1st of each month');
});

console.log('Jobs scheduled. Press Ctrl+C to stop.');
```

## Cron Expression Reference

```
*    *    *    *    *    *
┬    ┬    ┬    ┬    ┬    ┬
│    │    │    │    │    └─ Day of week (0-7, 0 or 7 = Sun)
│    │    │    │    └───── Month (1-12)
│    │    │    └────────── Day of month (1-31)
│    │    └─────────────── Hour (0-23)
│    └──────────────────── Minute (0-59)
└───────────────────────── Second (optional, node-cron)
```

| Expression | Meaning |
|-----------|---------|
| `* * * * *` | Every minute |
| `*/5 * * * *` | Every 5 minutes |
| `0 * * * *` | Every hour |
| `0 0 * * *` | Every day at midnight |
| `0 9 * * 1-5` | Weekdays at 9 AM |
| `0 0 1 * *` | First of month |
| `0 0 * * 0` | Every Sunday |
| `30 23 * * *` | Every day at 11:30 PM |

## Managing Jobs

```js
// manage-jobs.js — Start, stop, and control scheduled jobs

import cron from 'node-cron';

// Create a job but do not start it immediately
const job = cron.schedule('*/10 * * * * *', () => {
  console.log('Job running at', new Date().toISOString());
}, {
  scheduled: false,  // Do not start automatically
});

// Start the job
job.start();
console.log('Job started');

// Stop the job (pause, not destroy)
setTimeout(() => {
  job.stop();
  console.log('Job stopped');
}, 30_000);

// Restart after a delay
setTimeout(() => {
  job.start();
  console.log('Job restarted');
}, 45_000);

// Destroy the job permanently
setTimeout(() => {
  job.stop();  // Must stop before destroying
  console.log('Job destroyed');
}, 60_000);
```

## Timezone Support

```js
// timezone.js — Schedule jobs in specific timezones

import cron from 'node-cron';

// Run at 9 AM Eastern Time (regardless of server timezone)
cron.schedule('0 9 * * *', () => {
  console.log('9 AM in New York');
}, {
  timezone: 'America/New_York',
});

// Run at 9 AM Pacific Time
cron.schedule('0 9 * * *', () => {
  console.log('9 AM in Los Angeles');
}, {
  timezone: 'America/Los_Angeles',
});

// Run at 9 AM Tokyo Time
cron.schedule('0 9 * * *', () => {
  console.log('9 AM in Tokyo');
}, {
  timezone: 'Asia/Tokyo',
});

console.log('Timezone-aware jobs scheduled');
```

### Common Timezones

```
America/New_York    (EST/EDT)
America/Los_Angeles (PST/PDT)
Europe/London       (GMT/BST)
Europe/Paris        (CET/CEST)
Asia/Tokyo          (JST)
Asia/Shanghai       (CST)
UTC                 (Coordinated Universal Time)
```

## How It Works

### The Scheduler Loop

Internally, `node-cron` uses `setTimeout` to check every second whether any jobs should run:

```
1. Parse cron expression → list of matching times
2. Calculate ms until next match
3. setTimeout(callback, delay)
4. When timeout fires, execute callback
5. Repeat step 2
```

### Validation

```js
// Validate a cron expression before using it
const isValid = cron.validate('*/5 * * * *');    // true
const isInvalid = cron.validate('60 * * * *');   // false (minute > 59)
```

## Common Mistakes

### Mistake 1: No Timezone

```js
// WRONG — uses server's local time, which may differ from your intent
cron.schedule('0 9 * * *', () => {
  console.log('9 AM — but in what timezone?');
});

// CORRECT — specify timezone explicitly
cron.schedule('0 9 * * *', () => {
  console.log('9 AM Eastern');
}, { timezone: 'America/New_York' });
```

### Mistake 2: Overlapping Runs

```js
// WRONG — if the task takes 3 minutes but runs every minute, they overlap
cron.schedule('* * * * *', async () => {
  await longRunningTask();  // Takes 3 minutes
  // While this runs, 2 more instances start
});

// CORRECT — use a lock or schedule less frequently
let isRunning = false;
cron.schedule('* * * * *', async () => {
  if (isRunning) return;  // Skip if previous run is still going
  isRunning = true;
  try {
    await longRunningTask();
  } finally {
    isRunning = false;
  }
});
```

### Mistake 3: Starting Multiple Schedules on Restart

```js
// WRONG — each restart adds another schedule
app.listen(3000, () => {
  cron.schedule('0 9 * * *', dailyJob);  // After 10 restarts, 10 jobs run at 9 AM
});

// CORRECT — ensure only one instance runs (use distributed locks for clustering)
// Or use a job queue like BullMQ (Chapter 17) for reliable scheduling
```

## Try It Yourself

### Exercise 1: Log Every 10 Seconds

Schedule a job that logs the current time every 10 seconds. Run it for 1 minute and verify 6 log entries.

### Exercise 2: Weekday Report

Schedule a job for 9 AM weekdays. Print the current day of the week and a "sending daily report" message.

### Exercise 3: Multiple Timezones

Schedule the same task for 9 AM in New York, London, and Tokyo. Print which one fires first based on your server time.

## Next Steps

You can schedule recurring tasks. For preventing duplicate runs in distributed systems, continue to [Job Locking](./02-job-locking.md).
