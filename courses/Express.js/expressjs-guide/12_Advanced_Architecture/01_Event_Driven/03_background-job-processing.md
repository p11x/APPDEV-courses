# Background Job Processing

## 📌 What You'll Learn

- Separating worker processes from Express API
- Different job types and how to organize them
- Scheduling jobs with cron expressions
- Building a Bull Board UI for monitoring

## 🧠 Concept Explained (Plain English)

In production, you typically run workers as separate processes from your API server. This provides:

1. **Isolation**: Worker crashes don't affect API
2. **Scaling**: Scale workers independently from API
3. **Resources**: Workers can run on different machines
4. **Reliability**: Workers can restart without affecting users

A **worker process** is simply a Node.js script that runs the job processors. It connects to Redis (same as BullMQ uses) and listens for jobs.

**Cron jobs** are scheduled tasks that run at specific times — like "every day at midnight" or "every Monday at 9 AM". BullMQ supports cron syntax for scheduling.

**Bull Board** is a web UI (dashboard) that lets you see pending jobs, retry failed ones, and monitor queue health.

## 💻 Code Example

```js
// ============================================
// WORKER PROCESS (separate from Express API)
// This would typically run in a separate file or container
// ============================================

/*
// worker.js - Run with: node worker.js

import { Worker, Queue, QueueScheduler } from 'bullmq';
import './processors.js';

const redisConnection = {
  host: process.env.REDIS_HOST || 'localhost',
  port: parseInt(process.env.REDIS_PORT || '6379')
};

// Start queue schedulers (manages delayed jobs)
const emailScheduler = new QueueScheduler('email', { connection: redisConnection });
const reportScheduler = new QueueScheduler('reports', { connection: redisConnection });

// Create workers
const emailWorker = new Worker('email', async (job) => {
  console.log(`Processing job ${job.id}:`, job.name);
  
  if (job.name === 'send-welcome') {
    await sendWelcomeEmail(job.data);
  } else if (job.name === 'send-password-reset') {
    await sendPasswordReset(job.data);
  } else {
    await sendGenericEmail(job.data);
  }
}, { connection: redisConnection, concurrency: 10 });

const reportWorker = new Worker('reports', async (job) => {
  console.log(`Generating report: ${job.id}`);
  await generateReport(job.data);
}, { connection: redisConnection, concurrency: 2 });

console.log('Workers started...');
*/


// ============================================
// Express API (producer)
// ============================================

// ES Module syntax
import express from 'express';
import { Queue, CronExpression } from 'bullmq';

const app = express();

const redisConnection = {
  host: process.env.REDIS_HOST || 'localhost',
  port: parseInt(process.env.REDIS_PORT || '6379')
};


// Different queue types
const emailQueue = new Queue('email', { connection: redisConnection });
const reportQueue = new Queue('reports', { connection: redisConnection });
const cleanupQueue = new Queue('cleanup', { connection: redisConnection });


// ============================================
// Job Types and Scheduling
// ============================================

// One-time jobs
app.post('/api/jobs/send-welcome', async (req, res) => {
  const { userId, email, name } = req.body || {};
  
  const job = await emailQueue.add('send-welcome', {
    userId,
    email,
    name,
    type: 'welcome'
  }, {
    priority: 1,
    delay: 5000 // 5 seconds delay
  });
  
  res.json({ jobId: job.id, status: 'scheduled' });
});

// Delayed jobs
app.post('/api/jobs/send-reminder', async (req, res) => {
  const { userId, email, reminderType } = req.body || {};
  
  // Send reminder in 24 hours
  const job = await emailQueue.add('send-reminder', {
    userId,
    email,
    reminderType
  }, {
    delay: 24 * 60 * 60 * 1000 // 24 hours
  });
  
  res.json({ jobId: job.id, scheduledFor: new Date(Date.now() + 24 * 60 * 60 * 1000) });
});

// Scheduled cron jobs
app.post('/api/jobs/schedule-daily-report', async (req, res) => {
  const { recipients, reportType } = req.body || {};
  
  // Run every day at 2 AM
  const job = await reportQueue.add('daily-report', {
    recipients,
    reportType
  }, {
    repeat: {
      pattern: '0 2 * * *' // Cron: at 2:00 AM daily
    }
  });
  
  res.json({ 
    jobId: job.id, 
    message: 'Daily report scheduled',
    schedule: '2:00 AM daily'
  });
});

// Weekly summary
app.post('/api/jobs/schedule-weekly-summary', async (req, res) => {
  const { dayOfWeek = 'monday', hour = 9 } = req.body || {};
  
  const cronDay = {
    monday: '1',
    tuesday: '2', 
    wednesday: '3',
    thursday: '4',
    friday: '5',
    saturday: '6',
    sunday: '0'
  }[dayOfWeek] || '1';
  
  const pattern = `0 ${hour} * * ${cronDay}`;
  
  const job = await reportQueue.add('weekly-summary', {
    dayOfWeek
  }, {
    repeat: { pattern }
  });
  
  res.json({ jobId: job.id, schedule: `Every ${dayOfWeek} at ${hour}:00` });
});

// Monthly reports
app.post('/api/jobs/schedule-monthly-report', async (req, res) => {
  const { dayOfMonth = 1 } = req.body || {};
  
  // First day of every month
  const job = await reportQueue.add('monthly-report', {
    dayOfMonth
  }, {
    repeat: {
      pattern: `0 6 ${dayOfMonth} * *` // 6 AM on specified day
    }
  });
  
  res.json({ jobId: job.id, schedule: `Monthly on day ${dayOfMonth}` });
});


// ============================================
// Manual trigger for reports
// ============================================

app.post('/api/reports/generate', async (req, res) => {
  const { type, startDate, endDate, format = 'pdf' } = req.body || {};
  
  const job = await reportQueue.add('generate', {
    type,
    startDate,
    endDate,
    format,
    requestedAt: new Date().toISOString()
  });
  
  res.status(202).json({
    jobId: job.id,
    status: 'processing'
  });
});


// ============================================
// Recurring cleanup jobs
// ============================================

app.post('/api/jobs/cleanup', async (req, res) => {
  const { cleanupType } = req.body || {};
  
  const job = await cleanupQueue.add(cleanupType || 'temp-files', {
    cleanedAt: new Date().toISOString()
  }, {
    repeat: {
      every: 60 * 60 * 1000 // Every hour
    }
  });
  
  res.json({ jobId: job.id, schedule: 'Every hour' });
});


// ============================================
// Job Management
// ============================================

// Get all jobs
app.get('/api/jobs', async (req, res) => {
  const { queue = 'email', status = 'waiting', start = 0, end = 20 } = req.query;
  
  const q = new Queue(queue, { connection: redisConnection });
  
  let jobs = [];
  
  if (status === 'waiting') {
    jobs = await q.getWaiting(start, end);
  } else if (status === 'active') {
    jobs = await q.getActive(start, end);
  } else if (status === 'completed') {
    jobs = await q.getCompleted(start, end);
  } else if (status === 'failed') {
    jobs = await q.getFailed(start, end);
  }
  
  res.json({
    queue,
    status,
    count: jobs.length,
    jobs: jobs.map(j => ({
      id: j.id,
      name: j.name,
      data: j.data,
      status: j.failedReason ? 'failed' : status
    }))
  });
});


// Retry failed job
app.post('/api/jobs/:queueName/:jobId/retry', async (req, res) => {
  const { queueName, jobId } = req.params;
  
  const queue = new Queue(queueName, { connection: redisConnection });
  const job = await queue.getJob(jobId);
  
  if (!job) {
    return res.status(404).json({ error: 'Job not found' });
  }
  
  await job.retry();
  
  res.json({ jobId: job.id, status: 'retry_scheduled' });
});


// Remove completed job
app.delete('/api/jobs/:queueName/:jobId', async (req, res) => {
  const { queueName, jobId } = req.params;
  
  const queue = new Queue(queueName, { connection: redisConnection });
  const job = await queue.getJob(jobId);
  
  if (!job) {
    return res.status(404).json({ error: 'Job not found' });
  }
  
  await job.remove();
  
  res.json({ jobId: job.id, status: 'removed' });
});


// Pause/Resume queue
app.post('/api/queues/:queueName/pause', async (req, res) => {
  const { queueName } = req.params;
  
  const queue = new Queue(queueName, { connection: redisConnection });
  await queue.pause();
  
  res.json({ queue: queueName, status: 'paused' });
});

app.post('/api/queues/:queueName/resume', async (req, res) => {
  const { queueName } = req.params;
  
  const queue = new Queue(queueName, { connection: redisConnection });
  await queue.resume();
  
  res.json({ queue: queueName, status: 'resumed' });
});


// Health
app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 14-25 | Worker process example | Separate process structure |
| 56-62 | Queue definitions | Different queues for different job types |
| 70-82 | One-time job with delay | Welcome email with 5s delay |
| 85-97 | Delayed job | Reminder in 24 hours |
| 100-113 | Daily cron job | Report at 2 AM daily |
| 116-135 | Weekly cron job | Configurable day and hour |
| 138-150 | Monthly cron job | First of month reports |
| 159-173 | Manual report trigger | On-demand report generation |
| 176-189 | Recurring cleanup | Hourly cleanup task |
| 193-219 | Job listing endpoint | Get jobs by status |
| 222-233 | Retry endpoint | Retry failed jobs |
| 236-247 | Remove job | Delete completed jobs |
| 250-266 | Pause/Resume queue | Control queue processing |

## ⚠️ Common Mistakes

### 1. Running workers in same process as API

**What it is**: Worker crashes taking down the API.

**Why it happens**: Simplicity in development.

**How to fix it**: Separate workers into different processes/containers in production.

### 2. Not handling job data validation

**What it is**: Invalid job data causing worker crashes.

**Why it happens**: Skipping validation in worker processors.

**How to fix it**: Validate job data at the start of each processor.

### 3. Cron expressions incorrect

**What it is**: Jobs not running at expected times.

**Why it happens**: Misunderstanding cron format (minute hour day month weekday).

**How to fix it**: Use cron parser tools to validate expressions.

## ✅ Quick Recap

- Run workers as separate processes for isolation and scalability
- Use different queues for different job types
- Schedule jobs with cron expressions for recurring tasks
- Use delayed jobs for time-based triggers
- Implement pause/resume for queue control
- Monitor with Bull Board UI or custom endpoints

## 🔗 What's Next

Now learn about [Event Sourcing Basics](./04_event-sourcing-basics.md) to understand this architectural pattern.
