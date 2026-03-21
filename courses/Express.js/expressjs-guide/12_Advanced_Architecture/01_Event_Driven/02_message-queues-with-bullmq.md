# Message Queues with BullMQ

## 📌 What You'll Learn

- What a job queue is and why you need one
- Setting up BullMQ with Redis for background job processing
- Adding jobs to queues and processing them
- Configuring retries, concurrency, and priority

## 🧠 Concept Explained (Plain English)

Sometimes you have tasks that take too long to do in a web request — sending emails, processing images, generating reports, calling external APIs. If you do these synchronously, the user waits and your server blocks.

A **job queue** solves this by:
1. Accepting the task immediately
2. Putting it in a queue (managed by Redis)
3. Returning to the user immediately
3. Processing the task in the background with workers

This is called **asynchronous processing** — the user gets a quick response, and heavy work happens separately.

**BullMQ** is a Node.js library that manages job queues. It uses Redis as the backing store, which makes it fast and durable (jobs survive server restarts).

**Key concepts:**
- **Queue**: A named list of jobs (like "email-queue" or "image-processing-queue")
- **Producer**: Your Express app that adds jobs
- **Worker**: A process that picks up and processes jobs
- **Job**: A unit of work with data and metadata

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import { Queue, Worker, QueueEvents } from 'bullmq';

// In production, use environment variables for connection
const redisConnection = {
  host: process.env.REDIS_HOST || 'localhost',
  port: parseInt(process.env.REDIS_PORT || '6379')
};

const app = express();


// ============================================
// Create Queues
// ============================================

// Email queue for sending emails
const emailQueue = new Queue('email', { 
  connection: redisConnection,
  defaultJobOptions: {
    // Retry failed jobs up to 3 times
    attempts: 3,
    // Delay between retries (backoff)
    backoff: {
      type: 'exponential',
      delay: 1000
    },
    // Remove completed jobs after 1 hour
    removeOnComplete: 1000,
    // Keep failed jobs for debugging
    removeOnFail: false
  }
});

// Image processing queue
const imageQueue = new Queue('image-processing', {
  connection: redisConnection,
  defaultJobOptions: {
    attempts: 2,
    removeOnComplete: 500
  }
});

// Report generation queue
const reportQueue = new Queue('reports', {
  connection: redisConnection,
  defaultJobOptions: {
    priority: 2, // Lower priority
    attempts: 1,
    removeOnComplete: 100
  }
});


// ============================================
// Queue Events (for monitoring)
// ============================================

const emailQueueEvents = new QueueEvents('email', { connection: redisConnection });

emailQueueEvents.on('completed', ({ jobId }) => {
  console.log(`✅ Email job ${jobId} completed`);
});

emailQueueEvents.on('failed', ({ jobId, failedReason }) => {
  console.log(`❌ Email job ${jobId} failed: ${failedReason}`);
});


// ============================================
// Job Processors (Workers)
// ============================================

// Email worker
const emailWorker = new Worker('email', async (job) => {
  console.log(`📧 Processing email job ${job.id}:`, job.data);
  
  const { to, subject, template } = job.data;
  
  // Simulate sending email
  await new Promise(resolve => setTimeout(resolve, 500));
  
  // In production: await sendEmail(to, subject, template);
  
  console.log(`   ✓ Email sent to ${to}`);
  
  return { sent: true, to };
}, { 
  connection: redisConnection,
  concurrency: 5 // Process 5 emails concurrently
});


// Image processing worker
const imageWorker = new Worker('image-processing', async (job) => {
  console.log(`🖼️ Processing image job ${job.id}:`, job.data);
  
  const { imageId, operation } = job.data;
  
  // Simulate image processing
  await new Promise(resolve => setTimeout(resolve, 2000));
  
  return { processed: true, imageId, operation };
}, { 
  connection: redisConnection,
  concurrency: 2
});


// Error handling for workers
emailWorker.on('error', (err) => {
  console.error('Email worker error:', err.message);
});

imageWorker.on('error', (err) => {
  console.error('Image worker error:', err.message);
});


// ============================================
// API Routes - Adding Jobs to Queues
// ============================================

app.post('/api/send-email', express.json(), async (req, res) => {
  const { to, subject, template } = req.body || {};
  
  if (!to || !subject) {
    return res.status(400).json({ error: 'Email and subject required' });
  }
  
  try {
    // Add job to queue
    const job = await emailQueue.add('send-email', {
      to,
      subject,
      template,
      queuedAt: new Date().toISOString()
    }, {
      // Override default options for this specific job
      priority: 1, // Higher priority
    });
    
    res.status(202).json({ 
      message: 'Email queued',
      jobId: job.id,
      status: 'pending'
    });
  } catch (error) {
    console.error('Queue error:', error);
    res.status(500).json({ error: 'Failed to queue email' });
  }
});


app.post('/api/process-image', express.json(), async (req, res) => {
  const { imageId, operation } = req.body || {};
  
  if (!imageId) {
    return res.status(400).json({ error: 'Image ID required' });
  }
  
  try {
    const job = await imageQueue.add('process-image', {
      imageId,
      operation: operation || 'resize',
      queuedAt: new Date().toISOString()
    });
    
    res.status(202).json({
      message: 'Image processing queued',
      jobId: job.id,
      status: 'processing'
    });
  } catch (error) {
    res.status(500).json({ error: 'Failed to queue job' });
  }
});


// Bulk job addition
app.post('/api/send-bulk-emails', express.json(), async (req, res) => {
  const { recipients } = req.body || [];
  
  if (!recipients?.length) {
    return res.status(400).json({ error: 'Recipients array required' });
  }
  
  // Add multiple jobs at once
  const jobs = await emailQueue.addBulk(
    recipients.map((to) => ({
      name: 'send-email',
      data: {
        to,
        subject: 'Bulk Email',
        template: 'bulk',
        queuedAt: new Date().toISOString()
      }
    }))
  );
  
  res.status(202).json({
    message: `${jobs.length} emails queued`,
    jobIds: jobs.map(j => j.id)
  });
});


// Generate report (long-running job)
app.post('/api/generate-report', express.json(), async (req, res) => {
  const { type, startDate, endDate } = req.body || {};
  
  try {
    const job = await reportQueue.add('generate-report', {
      type: type || 'sales',
      startDate,
      endDate,
      requestedAt: new Date().toISOString()
    }, {
      // Lower priority
      priority: 3
    });
    
    res.status(202).json({
      message: 'Report generation started',
      jobId: job.id,
      status: 'processing'
    });
  } catch (error) {
    res.status(500).json({ error: 'Failed to queue report' });
  }
});


// Check job status
app.get('/api/jobs/:queueName/:jobId', async (req, res) => {
  const { queueName, jobId } = req.params;
  
  const queue = new Queue(queueName, { connection: redisConnection });
  const job = await queue.getJob(jobId);
  
  if (!job) {
    return res.status(404).json({ error: 'Job not found' });
  }
  
  const state = await job.getState();
  const progress = job.progress();
  
  res.json({
    id: job.id,
    state,
    progress,
    data: job.data,
    returnedValue: job.returnvalue,
    finishedOn: job.finishedOn,
    processedOn: job.processedOn,
    failedReason: job.failedReason
  });
});


// Get queue statistics
app.get('/api/queue-stats/:queueName', async (req, res) => {
  const { queueName } = req.params;
  
  const queue = new Queue(queueName, { connection: redisConnection });
  
  const [waiting, active, completed, failed, delayed] = await Promise.all([
    queue.getWaitingCount(),
    queue.getActiveCount(),
    queue.getCompletedCount(),
    queue.getFailedCount(),
    queue.getDelayedCount()
  ]);
  
  res.json({
    queue: queueName,
    waiting,
    active,
    completed,
    failed,
    delayed,
    total: waiting + active + completed + failed + delayed
  });
});


// Health check
app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});


// Graceful shutdown
process.on('SIGTERM', async () => {
  console.log('Shutting down workers...');
  await emailWorker.close();
  await imageWorker.close();
  await emailQueue.close();
  await imageQueue.close();
  await reportQueue.close();
  process.exit(0);
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log('\n📝 Test endpoints:');
  console.log(`   curl -X POST http://localhost:${PORT}/api/send-email -H "Content-Type: application/json" -d '{"to":"test@example.com","subject":"Hello"}'`);
  console.log(`   curl -X POST http://localhost:${PORT}/api/process-image -H "Content-Type: application/json" -d '{"imageId":"img-123"}'`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 12-16 | Redis connection config | Configuration for Redis connection |
| 21-32 | `emailQueue` creation | Queue with retry and cleanup options |
| 35-42 | `imageQueue` creation | Separate queue for images |
| 45-50 | `reportQueue` creation | Lower priority queue |
| 56-62 | QueueEvents listener | Monitors job completion/failure |
| 69-83 | Email worker | Processes email jobs with concurrency |
| 87-98 | Image worker | Processes image jobs |
| 105-115 | Error handlers | Catches worker errors |
| 131-143 | `/api/send-email` route | Adds email job to queue |
| 160-174 | `/api/process-image` route | Adds image job |
| 177-193 | `/api/send-bulk-emails` route | Bulk job addition |
| 196-210 | `/api/generate-report` route | Lower priority job |
| 213-230 | Job status endpoint | Check individual job state |
| 233-251 | Queue stats endpoint | Get queue counts |
| 267-274 | Graceful shutdown | Clean up workers on exit |

## ⚠️ Common Mistakes

### 1. Not handling job failures

**What it is**: Failed jobs disappear without logging or alerting.

**Why it happens**: Not configuring retry options or failure handlers.

**How to fix it**: Set `attempts` in job options, add failure event listeners, and monitor failed job counts.

### 2. Too much concurrency

**What it is**: Workers overwhelming databases or external APIs.

**Why it happens**: High concurrency without considering downstream capacity.

**How to fix it**: Set appropriate `concurrency` values, use connection pooling, and add circuit breakers.

### 3. Jobs never completing

**What it is**: Jobs stuck in waiting or active state forever.

**Why it happens**: Worker crashes, Redis connection issues, or unhandled errors.

**How to fix it**: Add stallion checks, set job timeouts, and monitor stuck jobs.

## ✅ Quick Recap

- Job queues decouple long-running tasks from HTTP requests
- BullMQ uses Redis for durable job storage
- Producers add jobs, workers process them
- Configure retries with exponential backoff
- Use priority to control job ordering
- Monitor queues with job counts and event listeners

## 🔗 What's Next

Now learn about [Background Job Processing](./03_background-job-processing.md) to see how to scale workers and manage different job types.
