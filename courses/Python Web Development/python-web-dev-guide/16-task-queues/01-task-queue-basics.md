# Task Queue Basics

## What You'll Learn
- Understanding task queues
- When to use task queues
- Celery and other task queue libraries

## Prerequisites
- Completed caching folder

## What Is a Task Queue?

A task queue is a mechanism for running tasks asynchronously in the background. Think of it like a restaurant kitchen - when an order comes in, it's added to a queue, and the kitchen processes it independently of the front desk.

## Why Use Task Queues?

- **Long-running operations**: Sending emails, processing images, generating reports
- **Background jobs**: Analytics, notifications, cleanup tasks
- **Rate limiting**: Control how many requests are processed
- **Reliability**: Tasks persist until completed

## Popular Python Task Queues

| Library | Broker | Complexity |
|---------|--------|------------|
| Celery | Redis, RabbitMQ | High |
| Huey | Redis, SQLite | Medium |
| RQ (Redis Queue) | Redis | Low |
| Dramatiq | Redis, RabbitMQ | Medium |

## Basic Architecture

```
[User Request] → [Web App] → [Message Broker] → [Worker Process]
                              (Redis/RabbitMQ)    (Background Task)
```

## Installing Celery

```bash
pip install celery redis
```

## Simple Celery Example

```python
from celery import Celery

# Create Celery app
app = Celery('tasks', broker='redis://localhost:6379/0')

@app.task
def send_email(to: str, subject: str, body: str) -> dict:
    """Background task to send email"""
    import time
    time.sleep(2)  # Simulate email sending
    return {'status': 'sent', 'to': to}

# Calling the task (async)
result = send_email.delay('user@example.com', 'Hello', 'Message body')

# Checking status
print(result.ready())  # True if completed
print(result.result)  # Get the result
```

🔍 **Line-by-Line Breakdown:**
1. `Celery('tasks', broker='redis://localhost:6379/0')` — Creates Celery app with Redis as message broker
2. `@app.task` — Decorator that turns function into background task
3. `.delay()` — Sends task to queue (non-blocking)
4. `result.ready()` — Checks if task completed
5. `result.result` — Gets return value of completed task

## Task with Return Value

```python
from celery import Celery
import time

app = Celery('tasks', broker='redis://localhost:6379/0')

@app.task
def generate_report(user_id: int, report_type: str) -> dict:
    """Generate a report in background"""
    # Simulate report generation
    time.sleep(5)
    
    return {
        'user_id': user_id,
        'type': report_type,
        'file_path': f'/reports/{user_id}_{report_type}.pdf',
        'generated_at': '2024-01-15T10:30:00Z'
    }

# In your web route:
@app.post('/reports/generate')
async def request_report(user_id: int, report_type: str):
    # Queue the task
    task = generate_report.delay(user_id, report_type)
    return {'task_id': task.id, 'status': 'processing'}

@app.get('/reports/status/{task_id}')
async def check_report_status(task_id: str):
    from celery.result import AsyncResult
    result = AsyncResult(task_id)
    return {
        'task_id': task_id,
        'status': result.state,
        'result': result.result if result.ready() else None
    }
```

## Summary
- Task queues handle background processing
- Celery is the most popular choice
- Use `.delay()` to queue tasks
- Check status with AsyncResult

## Next Steps
→ Continue to `02-celery-with-flask.md`
