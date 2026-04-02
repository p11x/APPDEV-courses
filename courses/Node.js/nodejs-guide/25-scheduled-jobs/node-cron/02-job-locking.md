# Job Locking

## What You'll Learn

- Why distributed job locking is needed
- How to implement a Redis-based lock with SETNX
- How to prevent duplicate job execution across multiple processes
- How to handle lock expiration and cleanup
- How to use the lock in cron jobs

## The Problem

When you scale your app to multiple processes (cluster, Docker replicas, PM2 instances), each process runs the same cron job. A "send daily report" job runs 4 times instead of once.

```
Process 1: ──→ send daily report ✓
Process 2: ──→ send daily report ✓  ← DUPLICATE!
Process 3: ──→ send daily report ✓  ← DUPLICATE!
Process 4: ──→ send daily report ✓  ← DUPLICATE!
```

A **lock** ensures only one process executes the job.

## Redis Lock Implementation

```js
// lock.js — Distributed lock using Redis

import Redis from 'ioredis';

const redis = new Redis();

/**
 * Acquire a lock. Returns a release function if acquired, null if not.
 * @param {string} lockKey - Unique key for this lock
 * @param {number} ttlMs - Lock expiration in milliseconds
 * @returns {Promise<Function|null>} Release function or null
 */
export async function acquireLock(lockKey, ttlMs = 30_000) {
  const lockValue = `${process.pid}:${Date.now()}`;

  // SETNX = "SET if Not eXists" — atomic check-and-set
  // If the key does not exist, set it with an expiry
  // If it already exists, return null (lock is held by someone else)
  const result = await redis.set(
    `lock:${lockKey}`,
    lockValue,
    'PX', ttlMs,    // PX = milliseconds expiry
    'NX'             // NX = only set if not exists
  );

  if (result !== 'OK') {
    // Lock is held by another process
    return null;
  }

  // Return a function that releases the lock
  // Only release if we still own the lock (value has not changed)
  return async () => {
    const currentValue = await redis.get(`lock:${lockKey}`);
    if (currentValue === lockValue) {
      await redis.del(`lock:${lockKey}`);
    }
    // If the value changed, the lock expired and someone else took it — do not delete
  };
}
```

## Using the Lock in Cron Jobs

```js
// scheduled-locked.js — Cron job with distributed locking

import cron from 'node-cron';
import { acquireLock } from './lock.js';

// Send daily report — only one instance should run this
cron.schedule('0 9 * * *', async () => {
  const release = await acquireLock('daily-report', 60_000);

  if (!release) {
    console.log(`[${process.pid}] Another instance is handling the daily report`);
    return;
  }

  try {
    console.log(`[${process.pid}] Generating daily report...`);
    await generateDailyReport();
    console.log(`[${process.pid}] Report sent!`);
  } catch (err) {
    console.error(`[${process.pid}] Report failed:`, err.message);
  } finally {
    await release();  // Always release the lock
  }
}, { timezone: 'America/New_York' });

// Cleanup job — every hour, only one instance
cron.schedule('0 * * * *', async () => {
  const release = await acquireLock('cleanup', 55 * 60_000);

  if (!release) return;  // Another instance is handling cleanup

  try {
    console.log(`[${process.pid}] Running cleanup...`);
    await cleanupOldRecords();
  } finally {
    await release();
  }
});

async function generateDailyReport() {
  // Simulate report generation
  await new Promise((r) => setTimeout(r, 2000));
}

async function cleanupOldRecords() {
  await new Promise((r) => setTimeout(r, 1000));
}
```

## Testing the Lock

```js
// test-lock.js — Run 4 instances to verify only 1 runs the job

import cron from 'node-cron';
import { acquireLock } from './lock.js';

cron.schedule('*/10 * * * * *', async () => {
  const release = await acquireLock('test-job', 5000);

  if (release) {
    console.log(`[${process.pid}] Lock acquired — running job`);
    await new Promise((r) => setTimeout(r, 2000));  // Simulate work
    await release();
    console.log(`[${process.pid}] Job done, lock released`);
  } else {
    console.log(`[${process.pid}] Lock held by another instance — skipping`);
  }
});

console.log(`Process ${process.pid} started`);
```

Run multiple instances:

```bash
# Terminal 1
node test-lock.js

# Terminal 2
node test-lock.js

# Terminal 3
node test-lock.js
```

You will see only one instance runs the job each time.

## How It Works

### The Lock Algorithm

```
SET lock:daily-report "12345:1705312200" PX 60000 NX
    │
    ├── Returns "OK" → Lock acquired
    │
    └── Returns null → Lock held by someone else, skip
```

### Lock Expiration (TTL)

The TTL prevents deadlocks. If a process crashes while holding the lock, the lock automatically expires after `ttlMs`. The next process can acquire it.

Set TTL longer than your job's expected duration but shorter than the retry interval.

## Common Mistakes

### Mistake 1: No TTL on the Lock

```js
// WRONG — if the process crashes, the lock is never released
await redis.set('lock:job', value, 'NX');  // No expiry!

// CORRECT — always set an expiry
await redis.set('lock:job', value, 'PX', 30000, 'NX');
```

### Mistake 2: Forgetting to Release the Lock

```js
// WRONG — if the job throws, lock is never released
const release = await acquireLock('job');
await doWork();  // Throws — release() never called
await release();

// CORRECT — use try/finally
const release = await acquireLock('job');
try {
  await doWork();
} finally {
  await release();
}
```

### Mistake 3: Deleting a Lock You Do Not Own

```js
// WRONG — blindly delete the lock
await redis.del('lock:job');  // Deletes even if another process re-acquired it

// CORRECT — check the value before deleting
const currentValue = await redis.get('lock:job');
if (currentValue === myLockValue) {
  await redis.del('lock:job');
}
```

## Try It Yourself

### Exercise 1: Verify Uniqueness

Run 4 instances of `test-lock.js`. Verify that only 1 instance runs the job each cycle.

### Exercise 2: Crash Recovery

Acquire a lock, then kill the process (SIGKILL). Verify that another instance acquires the lock after the TTL expires.

### Exercise 3: Lock with Retry

Implement `acquireLockWithRetry(key, ttl, maxRetries, retryDelay)` that retries acquiring the lock up to `maxRetries` times.

## Next Steps

You understand distributed locking. For timezone handling, continue to [Timezone Handling](./03-timezone-handling.md).
