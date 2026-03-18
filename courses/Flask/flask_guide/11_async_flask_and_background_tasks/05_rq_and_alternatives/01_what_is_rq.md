<!-- FILE: 11_async_flask_and_background_tasks/05_rq_and_alternatives/01_what_is_rq.md -->

## Overview

RQ (Redis Queue) is a simpler, more lightweight alternative to Celery for handling background tasks. It's built specifically for Python and Redis, making it much easier to set up and use. This file introduces RQ, explains when to use it, and provides a comparison with Celery.

## Prerequisites

- Basic Python knowledge
- Understanding of background tasks concept
- Redis installed

## Core Concepts

### What is RQ?

RQ (Redis Queue) is a Python library for queueing tasks and processing them in the background with workers. It's built on top of Redis and provides a simpler API than Celery.

Key features:
- **Simple**: Minimal configuration needed
- **Pythonic**: Pure Python, no dependencies beyond Redis
- **Lightweight**: Easy to understand and debug
- **Web interface**: Built-in dashboard for monitoring
- **Job serialization**: Uses pickle or JSON

### How RQ Works

```
┌─────────────────────────────────────────────────────────────┐
│                      FLASK APPLICATION                        │
│                                                              │
│  1. User submits job                                         │
│  2. Flask enqueues job to Redis                             │
│  3. Flask returns job ID                                    │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                        REDIS                                  │
│                                                              │
│  - Stores job queue                                          │
│  - Stores job results                                        │
│  - Stores worker registry                                    │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                     RQ WORKER                                 │
│                                                              │
│  - Pulls jobs from Redis                                    │
│  - Executes job functions                                    │
│  - Updates job status                                        │
└─────────────────────────────────────────────────────────────┘
```

### RQ vs Celery

| Feature | RQ | Celery |
|---------|-----|--------|
| Setup complexity | Very low | High |
| Dependencies | redis | redis, kombu, etc. |
| Message broker | Redis only | Redis, RabbitMQ, SQS |
| Scheduling | External (cron) | Built-in (Beat) |
| Web UI | Built-in | Flower |
| Scalability | Moderate | Excellent |
| Features | Basic | Advanced |
| Learning curve | Low | High |
| Production readiness | Small-medium | Enterprise |

### When to Use RQ

**Use RQ when:**
- You're building a smaller application
- You only need Redis as your broker
- You don't need complex task routing
- You want simpler debugging
- You're new to background tasks

**Use Celery when:**
- You need multiple message brokers
- You need scheduled/periodic tasks
- You need complex task routing
- You're building a large-scale system
- You need enterprise features

## Code Walkthrough

### Installing RQ

```bash
# Install RQ
pip install rq

# Install Flask-RQ (optional, provides Flask integration)
pip install flask-rq2

# Install Redis (if not already installed)
# macOS
brew install redis

# Ubuntu/Debian
sudo apt-get install redis-server
```

### Basic RQ Example

```python
# tasks.py
import time
import random
from rq import get_current_job

# ============================================
# Simple task
# ============================================

def add_numbers(a, b):
    """Simple addition task"""
    time.sleep(2)  # Simulate slow operation
    return a + b

# ============================================
# Task with progress tracking
# ============================================

def process_data(data):
    """
    Task that tracks progress
    
    RQ provides job object to track progress
    """
    job = get_current_job()
    
    total_steps = len(data)
    
    results = []
    for i, item in enumerate(data):
        # Process item
        result = item * 2
        results.append(result)
        
        # Update progress (0-100%)
        progress = int((i + 1) / total_steps * 100)
        job.meta["progress"] = progress
        job.save_meta()
    
    return {
        "results": results,
        "total": len(results)
    }

# ============================================
# Task with error handling
# ============================================

def risky_operation(value):
    """Task that might fail"""
    if value < 0:
        raise ValueError("Value must be positive!")
    
    time.sleep(1)
    return value * 2

# ============================================
# Long-running task
# ============================================

def generate_report(report_type):
    """Long-running report generation"""
    time.sleep(10)  # Simulate report generation
    
    return {
        "report_type": report_type,
        "generated_at": time.time(),
        "status": "completed"
    }
```

### Enqueuing Tasks

```python
# enqueue_examples.py
from redis import Redis
from rq import Queue
from tasks import add_numbers, process_data, risky_operation, generate_report

# Connect to Redis
redis_conn = Redis()

# Create queue
queue = Queue(connection=redis_conn)

# ============================================
# Enqueue simple task
# ============================================

# Fire and forget - returns immediately
job = queue.enqueue(add_numbers, 10, 20)
print(f"Job ID: {job.id}")  # e.g., "a1b2c3d4..."
print(f"Job status: {job.status}")  # "queued"

# Wait for result
result = job.return_value()  # Blocks until done
print(f"Result: {result}")  # 30

# ============================================
# Enqueue with options
# ============================================

# With timeout (fail if not done in 60 seconds)
job = queue.enqueue(
    add_numbers,
    args=(10, 20),
    timeout=60,
    result_ttl=3600,  # Store result for 1 hour
    job_timeout=60
)

# With job meta (custom data)
job = queue.enqueue(
    generate_report,
    args=("sales",),
    meta={
        "requested_by": "user@example.com",
        "report_format": "pdf"
    }
)

# With custom job ID (prevent duplicates)
job = queue.enqueue(
    generate_report,
    args=("monthly",),
    job_id="monthly-report-2024-01",
    result_ttl=86400  # 24 hours
)

# ============================================
# Enqueue with delayed execution
# ============================================

from datetime import datetime, timedelta

# Schedule for 5 minutes from now
job = queue.enqueue_in(
    timedelta(minutes=5),
    add_numbers,
    args=(10, 20)
)

# Schedule for specific time
job = queue.enqueue_at(
    datetime(2024, 1, 15, 12, 0, 0),  # Jan 15, 2024 at noon
    add_numbers,
    args=(10, 20)
)

# ============================================
# Check job status
# ============================================

job = queue.enqueue(add_numbers, 10, 20)

# Different ways to check status
print(f"Status: {job.status}")  # queued, started, finished, failed
print(f"Is queued: {job.is_queued}")
print(f"Is started: {job.is_started}")
print(f"Is finished: {job.is_finished}")
print(f"Is failed: {job.is_failed}")

# Get result (non-blocking)
if job.is_finished:
    print(f"Result: {job.return_value}")
else:
    print(f"Status: {job.status}")

# Get job from ID
job = queue.fetch_job("job-id")

# Get job from Redis
from rq.job import Job
job = Job.fetch("job-id", connection=redis_conn)
```

### Monitoring Jobs

```python
# monitoring.py
from redis import Redis
from rq import Queue
from rq.registry import StartedJobRegistry, FinishedJobRegistry, FailedJobRegistry

redis_conn = Redis()
queue = Queue(connection=redis_conn)

# ============================================
# Queue Information
# ============================================

# Get job IDs in queue
job_ids = queue.job_ids
print(f"Jobs in queue: {len(job_ids)}")

# Get all jobs in queue
jobs = queue.jobs

# Get queue length
length = len(queue)
print(f"Queue length: {length}")

# Get count of jobs in different states
# Note: RQ tracks these in registries
started_registry = StartedJobRegistry(queue=queue)
finished_registry = FinishedJobRegistry(queue=queue)
failed_registry = FailedJobRegistry(queue=queue)

print(f"Started jobs: {len(started_registry)}")
print(f"Finished jobs: {len(finished_registry)}")
print(f"Failed jobs: {len(failed_registry)}")

# Get job IDs from registry
started_job_ids = started_registry.get_job_ids()
finished_job_ids = finished_registry.get_job_ids()
failed_job_ids = failed_registry.get_job_ids()

# ============================================
# Worker Information
# ============================================

from rq.worker import Worker

# Get all workers
workers = Worker.all(connection=redis_conn)
print(f"Active workers: {len(workers)}")

for worker in workers:
    print(f"  Worker: {worker.name}")
    print(f"    State: {worker.get_state()}")
    print(f"    Current job: {worker.get_current_job_id()}")
    print(f"    Queues: {worker.queue_names()}")

# ============================================
# Job Information
# ============================================

# Get specific job
job = queue.fetch_job("job-id")

if job:
    print(f"Job ID: {job.id}")
    print(f"Status: {job.status}")
    print(f"Function: {job.func_name}")
    print(f"Args: {job.args}")
    print(f"Kwargs: {job.kwargs}")
    print(f"Created at: {job.created_at}")
    print(f"Enqueued at: {job.enqueued_at}")
    print(f"Started at: {job.started_at}")
    print(f"Finished at: {job.ended_at}")
    print(f"Result: {job.return_value}")
    print(f"Exc info: {job.exc_info}")
```

## Common Mistakes

### ❌ Using RQ for complex scheduling

```python
# PROBLEM: RQ doesn't have built-in scheduling
# You'll need external cron jobs
```

### ✅ Use cron or external scheduler

```python
# SOLUTION: Use cron for periodic tasks
# In crontab:
# 0 * * * * /usr/bin/python -c "from tasks import hourly_task; enqueue_hourly()"
```

### ❌ Not handling job result TTL

```python
# PROBLEM: Results stored forever (memory issue)
job = queue.enqueue(expensive_task)
# Result stays in Redis forever!
```

### ✅ Set result TTL

```python
# CORRECT: Set result expiration
job = queue.enqueue(
    expensive_task,
    result_ttl=3600  # Delete result after 1 hour
)
```

### ❌ Blocking with get() in web requests

```python
# PROBLEM: Blocks request!
@app.route("/process")
def process():
    job = queue.enqueue(heavy_task)
    result = job.get()  # Wait here!
    return jsonify(result)
```

### ✅ Return job ID, check later

```python
# CORRECT: Return immediately
@app.route("/process")
def process():
    job = queue.enqueue(heavy_task)
    return jsonify({"job_id": job.id})

@app.route("/status/<job_id>")
def status(job_id):
    job = queue.fetch_job(job_id)
    if job.is_finished:
        return jsonify({"status": "done", "result": job.return_value})
    return jsonify({"status": job.status})
```

## Quick Reference

| Command | Description |
|---------|-------------|
| `rq worker` | Start worker |
| `rq info` | Show queue info |
| `rq dashboard` | Start web dashboard |

**Python API:**

```python
from rq import Queue

queue = Queue()

# Enqueue job
job = queue.enqueue(func, arg1, arg2)

# Get result
result = job.return_value  # or job.result

# Check status
job.is_queued, job.is_started, job.is_finished, job.is_failed
```

## Next Steps

Continue to [02_setting_up_rq_with_flask.md](02_setting_up_rq_with_flask.md) to learn how to integrate RQ with Flask.