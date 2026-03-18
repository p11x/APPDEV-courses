# Celery Advanced Features

## What You'll Learn
- Retry mechanisms
- Task scheduling
- Rate limiting
- Task routing
- Monitoring with Flower

## Prerequisites
- Completed Celery with Flask/FastAPI

## Task Retries

```python
from celery import Celery
from celery.exceptions import MaxRetriesExceededError

app = Celery('tasks', broker='redis://localhost:6379/0')

@app.task(bind=True, max_retries=3, default_retry_delay=60)
def fetch_data_with_retry(self, url: str) -> dict:
    """Task that retries on failure"""
    import requests
    
    try:
        response = requests.get(url, timeout=5)
        response.raise_for_status()
        return {'url': url, 'data': response.json()}
    except requests.RequestException as exc:
        # Retry the task
        try:
            raise self.retry(exc=exc)
        except MaxRetriesExceededError:
            return {'error': f'Failed after 3 retries: {exc}'}
```

🔍 **Line-by-Line Breakdown:**
1. `bind=True` — Gives access to task instance (self)
2. `max_retries=3` — Maximum retry attempts
3. `default_retry_delay=60` — Wait 60 seconds between retries
4. `self.retry(exc=exc)` — Retry the task with original exception

## Scheduled Tasks

```python
from celery import Celery
from celery.schedules import crontab

app = Celery('tasks', broker='redis://localhost:6379/0')

# Celery Beat schedule
app.conf.beat_schedule = {
    'daily-user-report': {
        'task': 'tasks.generate_daily_report',
        'schedule': crontab(hour=8, minute=0),  # 8 AM daily
    },
    'weekly-cleanup': {
        'task': 'tasks.weekly_cleanup',
        'schedule': crontab(hour=2, minute=0, day_of_week='sunday'),  # Sundays at 2 AM
    },
    'every-30-minutes': {
        'task': 'tasks.sync_data',
        'schedule': 1800,  # 30 minutes in seconds
    },
}

@app.task
def generate_daily_report() -> dict:
    """Generate daily report"""
    return {'report': 'generated', 'date': '2024-01-15'}

@app.task
def weekly_cleanup() -> dict:
    """Clean up old data"""
    return {'cleaned': True}

@app.task
def sync_data() -> dict:
    """Sync external data"""
    return {'synced': True}

# Run: celery -A tasks beat
```

## Rate Limiting

```python
@app.task(rate_limit='10/m')  # 10 tasks per minute
def send_sms(phone: str, message: str) -> dict:
    """Rate limited SMS sending"""
    return {'sent': True, 'phone': phone}

@app.task(rate_limit='100/s')  # 100 tasks per second
def process_webhook(event: dict) -> dict:
    """Process webhooks with rate limit"""
    return {'processed': True}
```

## Task Routing

```python
from celery import Celery

app = Celery('tasks', broker='redis://localhost:6379/0')

# Define queues
app.conf.task_routes = {
    'tasks.send_email': {'queue': 'emails'},
    'tasks.process_video': {'queue': 'videos'},
    'tasks.generate_report': {'queue': 'reports'},
}

app.conf.task_default_queue = 'default'

@app.task(queue='emails')
def send_email(to: str, subject: str) -> dict:
    """Email task"""
    return {'sent': True}

@app.task(queue='videos')
def process_video(video_id: int) -> dict:
    """Video processing task"""
    return {'processed': video_id}

# Start workers for specific queues:
# celery -A tasks worker -Q emails
# celery -A tasks worker -Q videos
```

## Task Priority

```python
from celery import Celery

app = Celery('tasks', broker='redis://localhost:6379/0')

@app.task
def low_priority_task() -> str:
    """Low priority task"""
    return 'low'

@app.task
def high_priority_task() -> str:
    """High priority task"""
    return 'high'

# Using priorities (0-9, 9 is highest)
high_priority_task.apply_async(priority=9)
low_priority_task.apply_async(priority=1)
```

## Monitoring with Flower

```bash
pip install flower
celery -A tasks flower --port=5555
```

```python
# Flower provides:
# - Real-time monitoring
# - Task history
# - Worker status
# - Statistics
```

## Summary
- Use retries for transient failures
- Use Celery Beat for scheduling
- Implement rate limiting for external APIs
- Route tasks to specific workers
- Monitor with Flower

## Next Steps
→ Move to `17-security/` folder
