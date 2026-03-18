# Celery Basics

## What You'll Learn
- Task queue fundamentals
- Setting up Celery with Redis
- Defining and executing tasks
- Task scheduling
- Error handling

## Prerequisites
- Understanding of async operations
- Redis knowledge

## What Is Celery?

Celery is a distributed task queue for Python that handles asynchronous task execution:

```
Web Request ──▶ Celery Broker (Redis) ──▶ Worker Process
                                    │
                                    ▼
                              Execute Task
                                    │
                                    ▼
                              Store Result
```

## Installation

```bash
pip install celery redis
```

## Configuration

```python
# celery_config.py
from celery import Celery

app = Celery('myapp')

app.conf.update(
    broker_url='redis://localhost:6379/0',
    result_backend='redis://localhost:6379/1',
    task_serializer='json',
    accept_content=['json'],
    result_serializer='json',
    timezone='UTC',
    enable_utc=True,
)
```

## Defining Tasks

```python
# tasks.py
from .celery_config import app

@app.task
def send_email(to: str, subject: str, body: str) -> dict:
    """Send email task."""
    print(f"Sending email to {to}")
    # Email sending logic here
    return {"status": "sent", "to": to}

@app.task
def process_image(image_id: int) -> dict:
    """Process uploaded image."""
    import time
    time.sleep(5)  # Simulate processing
    return {"status": "processed", "image_id": image_id}

@app.task(bind=True, max_retries=3)
def fetch_data(self, url: str) -> dict:
    """Task with retry logic."""
    import requests
    try:
        response = requests.get(url)
        return {"data": response.json()}
    except Exception as e:
        # Retry on failure
        raise self.retry(exc=e, countdown=60)
```

## Executing Tasks

```python
# Call asynchronously
result = send_email.delay("user@example.com", "Hello", "Body")

# Check status
print(result.status)  # PENDING, SUCCESS, FAILURE

# Get result
data = result.get(timeout=10)

# Async check
if result.ready():
    data = result.get()
```

## Scheduling

```python
# celery_config.py
app.conf.beat_schedule = {
    'daily-report': {
        'task': 'tasks.generate_daily_report',
        'schedule': 86400,  # Every 24 hours
    },
    'every-hour': {
        'task': 'tasks.cleanup_old_data',
        'schedule': 3600,
    },
}
```

## Running Celery

```bash
# Start worker
celery -A myapp worker -l info

# Start beat scheduler
celery -A myapp beat -l info
```

## Summary

- Celery handles asynchronous task execution
- Use Redis as message broker
- Define tasks with @app.task decorator
- Execute with .delay() for async execution
- Use beat for scheduled tasks
