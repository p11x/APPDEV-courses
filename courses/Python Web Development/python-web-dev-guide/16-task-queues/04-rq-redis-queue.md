# RQ (Redis Queue)

## What You'll Learn
- Using RQ for simpler task queue needs
- RQ with Flask and FastAPI
- Managing workers and jobs

## Prerequisites
- Completed task queue basics

## Why RQ?

RQ (Redis Queue) is simpler than Celery:
- No configuration needed
- Built on Redis
- Perfect for small to medium apps

## Installation

```bash
pip install rq redis
```

## Basic RQ Setup

```python
from rq import Queue
from redis import Redis
import time

# Connect to Redis
redis_conn = Redis(host='localhost', port=6379, db=0)
queue = Queue('default', connection=redis_conn)

def send_sms(phone: str, message: str) -> dict:
    """Background task to send SMS"""
    time.sleep(2)  # Simulate SMS sending
    return {'status': 'sent', 'phone': phone}

# Enqueue job
job = queue.enqueue(send_sms, '+1234567890', 'Hello!')
print(f"Job ID: {job.id}")
print(f"Job Status: {job.get_status()}")
```

🔍 **Line-by-Line Breakdown:**
1. `Queue('default', connection=redis_conn)` — Create queue named 'default'
2. `queue.enqueue(send_sms, ...)` — Add function to queue
3. `job.id` — Unique job identifier
4. `job.get_status()` — Check job status (queued, started, finished, failed)

## RQ with Flask

```python
from flask import Flask, jsonify
from rq import Queue
from redis import Redis
import time

app = Flask(__name__)
redis_conn = Redis(host='localhost', port=6379)
task_queue = Queue('tasks', connection=redis_conn)

def generate_thumbnail(image_path: str) -> dict:
    """Generate thumbnail in background"""
    time.sleep(3)  # Simulate processing
    return {
        'image_path': image_path,
        'thumbnail': f'{image_path}_thumb.jpg',
        'status': 'completed'
    }

@app.route('/thumbnail', methods=['POST'])
def create_thumbnail():
    image_path = 'uploads/image.jpg'
    
    # Enqueue the job
    job = task_queue.enqueue(generate_thumbnail, image_path)
    
    return jsonify({
        'job_id': job.id,
        'status': 'queued'
    })

@app.route('/thumbnail/<job_id>')
def check_thumbnail(job_id: str):
    job = task_queue.fetch_job(job_id)
    
    if job is None:
        return jsonify({'error': 'Job not found'}), 404
    
    return jsonify({
        'job_id': job.id,
        'status': job.get_status(),
        'result': job.result if job.is_finished else None
    })

# Start worker: rq worker tasks
```

## RQ with FastAPI

```python
from fastapi import FastAPI, BackgroundTasks
from rq import Queue
from redis import Redis
from pydantic import BaseModel

app = FastAPI()
redis_conn = Redis(host='localhost', port=6379)
task_queue = Queue('fastapi-tasks', connection=redis_conn)

class TaskCreate(BaseModel):
    email: str
    template: str

def send_email_task(email: str, template: str) -> dict:
    """Send email in background"""
    import time
    time.sleep(2)
    return {'sent_to': email, 'template': template}

@app.post("/send-email")
async def send_email(task: TaskCreate, background_tasks: BackgroundTasks):
    job = task_queue.enqueue(send_email_task, task.email, task.template)
    return {"job_id": job.id, "status": "queued"}

@app.get("/job/{job_id}")
async def get_job_status(job_id: str):
    job = task_queue.fetch_job(job_id)
    
    if job is None:
        return {"error": "Job not found"}, 404
    
    return {
        "id": job.id,
        "status": job.get_status(),
        "result": job.result,
        "enqueued_at": job.enqueued_at
    }
```

## Managing Workers

```bash
# Start worker
rq worker default

# Start worker with custom queue
rq worker high-priority low-priority

# Start multiple workers
rq worker default &  # Background worker
rq worker default    # Foreground worker
```

## Job Lifecycle

```python
from rq import Queue
from redis import Redis

redis_conn = Redis()
q = Queue(connection=redis_conn)

# Job states
job = q.enqueue(some_task)

# Check status
job.get_status()  # 'queued', 'started', 'finished', 'failed'

# Wait for completion
job = job.refresh()  # Refresh from Redis
if job.is_finished:
    print(job.result)
elif job.is_failed:
    print(job.exc_info)  # Get error info
```

## Summary
- RQ is simpler than Celery for small projects
- Use `enqueue()` to add tasks
- Use `job.fetch_job()` to check status
- Start workers with `rq worker` command

## Next Steps
→ Continue to `05-celery-advanced-features.md`
