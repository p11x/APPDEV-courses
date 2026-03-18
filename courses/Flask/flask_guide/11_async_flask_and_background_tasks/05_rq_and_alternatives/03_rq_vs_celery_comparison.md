<!-- FILE: 11_async_flask_and_background_tasks/05_rq_and_alternatives/03_rq_vs_celery_comparison.md -->

## Overview

Choosing between Celery and RQ is an important architectural decision. This file provides a comprehensive comparison, helping you understand the trade-offs and choose the right tool for your specific use case.

## Prerequisites

- Basic understanding of background task queues
- Knowledge of either Celery or RQ

## Core Concepts

### Feature Comparison

| Feature | Celery | RQ | Notes |
|---------|--------|-----|-------|
| **Complexity** | High | Low | RQ is much simpler |
| **Setup Time** | 30+ min | 5 min | RQ wins for quick start |
| **Dependencies** | Multiple | redis only | RQ has fewer dependencies |
| **Message Broker** | Redis, RabbitMQ, SQS | Redis only | Celery is more flexible |
| **Result Backend** | Multiple options | Redis only | Celery supports more |
| **Task Routing** | Complex | Simple | Celery for advanced routing |
| **Scheduling** | Built-in (Beat) | External | Celery has native scheduling |
| **Monitoring** | Flower | Built-in | RQ has basic web UI |
| **Scalability** | Excellent | Good | Celery scales better |
| **Priority Queues** | Complex | Simple | Both support it |
| **Task Chaining** | Built-in | Manual | Celery has primitives |
| **Periodic Tasks** | Built-in | No | Need cron with RQ |
| **Error Handling** | Advanced | Basic | Celery has retries built-in |
| **Worker Management** | Dynamic | Manual | Celery is more flexible |

### Architecture Comparison

**RQ Architecture:**
```
Flask → Redis (queue) → RQ Worker → Redis (result)
```

**Celery Architecture:**
```
Flask → Broker → Celery Worker → Result Backend
                    ↑
              Celery Beat (scheduler)
```

## Code Walkthrough

### RQ Implementation Example

```python
# rq_example.py
from flask import Flask
from flask_rq2 import RQ
from rq import Queue
from redis import Redis

app = Flask(__name__)
rq = RQ()
rq.init_app(app)

# Simple task
@rq.job()
def add(a, b):
    return a + b

# Enqueue
@app.route("/add/<int:a>/<int:b>")
def add_route(a, b):
    job = add.queue(a, b)  # Simple!
    return {"job_id": job.id}
```

### Celery Implementation Example

```python
# celery_example.py
from flask import Flask
from celery import Celery

app = Flask(__name__)
celery = Celery("app", broker="redis://localhost:6379/0")

# Complex configuration
celery.conf.update(
    task_serializer="json",
    accept_content=["json"],
    result_serializer="json",
    timezone="UTC",
    enable_utc=True,
    beat_schedule={
        "every-hour": {
            "task": "tasks.hourly_task",
            "schedule": 3600.0,
        },
    }
)

# Task with retries
@celery.task(bind=True, max_retries=3)
def add(self, a, b):
    try:
        return a + b
    except Exception as e:
        raise self.retry(exc=e, countdown=60)

# Enqueue
@app.route("/add/<int:a>/<int:b>")
def add_route(a, b):
    job = add.delay(a, b)  # Simple too, but more options available
    return {"job_id": job.id}
```

### Decision Framework

Use this decision tree to choose:

```
                    START
                      │
                      ▼
         ┌────────────────────────┐
         │ Need scheduled tasks?  │
         └────────────────────────┘
               │              │
              YES             NO
               │              │
               ▼              ▼
    ┌──────────────┐   ┌─────────────────┐
    │ Need Rabbit  │   │ Team experience?│
    │ or SQS?      │   └─────────────────┘
    └──────────────┘         │        │
          │                 HIGH     LOW
          │                   │        │
         YES                NO        │
          │                  │        │
          ▼                  ▼        ▼
    ┌──────────┐      ┌─────────┐ ┌──────┐
    │  CELERY  │      │  CELERY │ │  RQ  │
    └──────────┘      └─────────┘ └──────┘
```

### Use Case Recommendations

#### Choose RQ if:

**1. Small to medium application**
```python
# Example: Small startup with < 1000 daily tasks
# RQ is perfect - simple, easy to maintain
@rq.job()
def send_welcome_email(user_id):
    # Quick email send
    email_service.send(user_id)
```

**2. Redis is your only infrastructure**
```python
# Already using Redis? RQ uses Redis for everything
# No need to add RabbitMQ or other brokers
redis_url = os.environ.get("REDIS_URL")
# RQ handles queue + results + worker registry
```

**3. Simple background jobs**
```python
# Just need fire-and-forget tasks
job = my_task.queue(data)
# No complex routing, scheduling, or chaining needed
```

**4. Learning background tasks**
```python
# New to task queues? RQ is much easier to learn
# Simple API, less configuration, clearer error messages
```

#### Choose Celery if:

**1. Enterprise features needed**
```python
# Need: task prioritization, routing, complex workflows
celery.conf.task_routes = {
    'tasks.high_priority.*': {'queue': 'high'},
    'tasks.low_priority.*': {'queue': 'low'},
    'tasks.email.*': {'queue': 'email', 'routing_key': 'email'},
}
```

**2. Multiple message brokers**
```python
# Need RabbitMQ or SQS (maybe for AWS integration)
broker = 'amqp://user:pass@rabbitmqhost:5672//'
celery = Celery('app', broker=broker)
```

**3. Built-in scheduling**
```python
# Need periodic tasks without external cron
celery.conf.beat_schedule = {
    'daily-backup': {
        'task': 'tasks.backup',
        'schedule': crontab(hour=2, minute=0),  # 2 AM daily
    },
}
```

**4. Complex task workflows**
```python
# Need chains, groups, chords
from celery import chain, group, chord

# Chain: sequential
chain(task1.s(), task2.s(), task3.s())

# Group: parallel
group(task1.s(i) for i in range(10))

# Chord: parallel then callback
chord([task1.s(), task2.s()])(callback.s())
```

**5. High scalability required**
```python
# Need to scale across multiple machines
# Celery supports distributed workers, result backends
celery -A app worker --concurrency=10 -Q high,low,default
# Can add workers on different machines easily
```

**6. Advanced error handling**
```python
# Need automatic retries with backoff
@celery.task(
    autoretry_for=(Exception,),
    retry_backoff=True,
    retry_backoff_max=600,
    retry_jitter=True,
    max_retries=5
)
def reliable_task(data):
    # Automatically retries with exponential backoff
    pass
```

### Migration: RQ to Celery

If you outgrow RQ, here's how to migrate:

```python
# OLD: RQ task
@rq.job(timeout=60)
def send_email(to, subject, body):
    email_service.send(to, subject, body)
    return {"status": "sent"}

# NEW: Celery task (more features)
@celery.task(
    bind=True,
    max_retries=3,
    default_retry_delay=60,
    autoretry_for=(SMTPError, ConnectionError),
    retry_backoff=True
)
def send_email(self, to, subject, body):
    try:
        email_service.send(to, subject, body)
        return {"status": "sent"}
    except (SMTPError, ConnectionError) as e:
        raise self.retry(exc=e)
```

### Scaling Comparison

**RQ scaling:**
```bash
# Add more workers (same machine or different)
rq worker high-priority
rq worker high-priority
rq worker low-priority
# Works but manual
```

**Celery scaling:**
```bash
# Distributed workers with more control
celery -A app worker --hostname=worker1@%h -Q high
celery -A app worker --hostname=worker2@%h -Q high
celery -A app worker --hostname=worker3@%h -Q low,default
# Can also use remote machines easily
```

## Quick Reference

| Scenario | Recommended |
|----------|-------------|
| Small app, simple tasks | RQ |
| Learning task queues | RQ |
| Already using Redis only | RQ |
| Need scheduling | Celery |
| Complex workflows | Celery |
| Enterprise features | Celery |
| High scalability | Celery |
| Multiple brokers needed | Celery |

## Summary

- **RQ**: Choose for simplicity, small-medium projects, Redis-only environments
- **Celery**: Choose for complex needs, scheduling, enterprise features, high scalability

Both are production-ready. RQ is simpler to start with; Celery offers more features as your needs grow.

## Next Steps

This concludes Topic 11 on Async Flask and Background Tasks. You now have a comprehensive understanding of:

1. **Async fundamentals** - Sync vs async Python, async/await basics
2. **Async Flask** - Installing Flask async, writing async routes, async database queries
3. **Celery basics** - What is Celery, Redis setup, creating tasks
4. **Celery advanced** - Task chaining, periodic tasks, Flower monitoring
5. **RQ alternatives** - RQ basics, Flask integration, RQ vs Celery

You can now proceed to [Topic 12: Caching and Performance](../12_caching_and_performance/01_caching_concepts/01_what_is_caching.md) to learn about optimizing your Flask applications.