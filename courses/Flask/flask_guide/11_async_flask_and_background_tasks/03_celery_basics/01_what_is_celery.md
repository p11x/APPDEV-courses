<!-- FILE: 11_async_flask_and_background_tasks/03_celery_basics/01_what_is_celery.md -->

## Overview

Celery is a distributed task queue that allows you to run background tasks asynchronously. It's the industry standard for handling long-running operations in Python web applications. This file explains what Celery is, how it works, and why you might need it in your Flask applications.

## Prerequisites

- Basic Python knowledge
- Understanding of synchronous vs asynchronous operations
- Familiarity with Flask request-response cycle

## Core Concepts

### The Problem: Blocking Operations

In web applications, certain operations take too long to complete within a single HTTP request:

- **Image/video processing**: Resizing, transcoding, applying filters
- **Sending emails**: SMTP can be slow
- **Generating reports**: Large PDF/Excel exports
- **Machine learning inference**: Model predictions
- **Scheduled tasks**: Daily data cleanup, weekly reports
- **External API calls**: Rate-limited third-party services

If you handle these synchronously, users experience slow response times and your server's concurrency is limited.

### The Solution: Task Queues

A task queue separates the "request" from the "execution":

1. **User makes a request** → Application responds quickly with a "task ID"
2. **Task is queued** → Background worker picks it up
3. **Task runs in background** → Worker processes it independently
4. **Result is stored** → User can check status using the task ID

### What is Celery?

**Celery** is an open-source task queue written in Python. It provides:

- **Distributed execution**: Workers can run on multiple machines
- **Broker support**: Works with Redis, RabbitMQ, Amazon SQS
- **Task scheduling**: Run tasks at specific times or intervals
- **Result backend**: Store task results for later retrieval
- **Error handling**: Automatic retries, timeouts
- **Task prioritization**: Urgent tasks first

### How Celery Works

```
┌─────────────────────────────────────────────────────────────────┐
│                        FLASK APPLICATION                        │
│                                                                 │
│  1. User submits job                                            │
│  2. Flask sends task to broker (Redis/RabbitMQ)                │
│  3. Flask returns task_id to user                              │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      MESSAGE BROKER                             │
│                    (Redis or RabbitMQ)                          │
│                                                                 │
│  - Task queue (FIFO)                                           │
│  - Acts as middleman between Flask and workers                 │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      CELERY WORKERS                             │
│                                                                 │
│  - Separate processes (can be on same or different machine)   │
│  - Pull tasks from broker                                      │
│  - Execute tasks                                               │
│  - Store results in result backend                             │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      RESULT BACKEND                             │
│                   (Redis, Database, etc.)                       │
│                                                                 │
│  - Stores task results                                         │
│  - Allows checking task status                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Celery Components

| Component | Description |
|-----------|-------------|
| **Task** | A unit of work (a Python function decorated with `@celery.task`) |
| **Broker** | Message queue that stores pending tasks (Redis, RabbitMQ) |
| **Worker** | Process that executes tasks from the broker |
| **Result Backend** | Storage for task results and state |
| **Beat** | Scheduler for periodic tasks |

### When to Use Celery

| Use Case | Example |
|----------|---------|
| Email sending | Welcome emails, password resets |
| Image processing | Resizing, thumbnails, filters |
| Data exports | Large CSV/PDF generation |
| External API calls | Third-party webhooks |
| Scheduled tasks | Daily cleanup, weekly reports |
| Heavy computation | Machine learning, data processing |

### Celery vs Other Options

| Feature | Celery | RQ (Redis Queue) | Flask-AsyncIO |
|---------|--------|------------------|---------------|
| Complexity | High | Low | Low |
| Scalability | Excellent | Good | Limited |
| Horizontal scaling | Yes | Yes | No |
| Scheduling | Built-in | No (needs cron) | No |
| Result backend | Multiple | Redis only | In-memory |
| Learning curve | Steep | Easy | Easy |

> **💡 Tip:** For beginners, RQ is simpler. For production systems needing scalability, Celery is the standard choice.

## Code Walkthrough

### Installing Celery

```bash
# Core Celery package
pip install celery

# Redis (as broker and result backend)
pip install redis

# Flask-Celery extension (optional, but helps integration)
pip install flask-celery
```

### A Simple Celery Example

```python
# tasks.py
from celery import Celery

# Create Celery app
# Format: "redis://localhost:6379/0" means:
#   - redis:// protocol
#   - localhost:6379 - Redis server on local machine, port 6379
#   - /0 - Database number 0 (Redis can have multiple databases)
celery_app = Celery(
    "tasks",
    broker="redis://localhost:6379/0",
    backend="redis://localhost:6379/0"
)

# Configure Celery (optional settings)
celery_app.conf.update(
    task_serializer="json",  # How tasks are serialized
    accept_content=["json"],  # What content types to accept
    result_serializer="json",  # How results are serialized
    timezone="UTC",  # Timezone for scheduled tasks
    enable_utc=True,
)

# Define a task
@celery_app.task(name="add_numbers")
def add_numbers(a, b):
    """A simple task that adds two numbers"""
    import time
    print(f"Adding {a} + {b}...")
    time.sleep(5)  # Simulate slow operation
    result = a + b
    print(f"Result: {result}")
    return result

# Define another task
@celery_app.task(name="send_email")
def send_email(to, subject, body):
    """Simulate sending an email"""
    import time
    print(f"Sending email to {to}...")
    time.sleep(2)  # Simulate email sending
    print(f"Email sent!")
    return {"status": "sent", "to": to}

# Define a task with error handling
@celery_app.task(name="risky_operation", bind=True, max_retries=3)
def risky_operation(self, x):
    """Task that can fail and retry"""
    try:
        if x < 0:
            raise ValueError("x must be positive")
        return {"result": x * 2}
    except ValueError as e:
        # Retry after 5 seconds, up to 3 times
        raise self.retry(exc=e, countdown=5)
```

### Running the Components

**Terminal 1 - Start Redis** (if not already running):

```bash
# macOS with Homebrew
brew services start redis

# Ubuntu/Debian
sudo systemctl start redis

# Or run directly
redis-server
```

**Terminal 2 - Start Celery worker**:

```bash
# -A tasks means "app in tasks.py"
# --loglevel=info shows detailed logs
celery -A tasks worker --loglevel=info
```

**Terminal 3 - Run Flask app or test tasks**:

```python
# test_tasks.py - Testing the tasks
from tasks import add_numbers, send_email, risky_operation

# ----------------------------------------
# Method 1: Run synchronously (for testing)
# ----------------------------------------
print("=== Synchronous (blocking) ===")
result = add_numbers(10, 20)  # Blocks for 5 seconds
print(f"Result: {result}")

# ----------------------------------------
# Method 2: Run asynchronously (proper way)
# ----------------------------------------
print("\n=== Asynchronous (non-blocking) ===")
# .delay() queues the task and returns immediately
task = add_numbers.delay(10, 20)

# Check if done
print(f"Task ID: {task.id}")
print(f"Ready: {task.ready()}")  # False initially

# Wait for result (blocks this script, but not the worker)
result = task.get(timeout=10)
print(f"Result: {result}")

# ----------------------------------------
# Method 3: Check task status
# ----------------------------------------
print("\n=== Check status ===")
task = add_numbers.delay(100, 200)

# Various status checks
print(f"ID: {task.id}")
print(f"Status: {task.status}")  # PENDING, STARTED, SUCCESS, FAILURE
print(f"Ready: {task.ready()}")   # True if completed (success or failure)
print(f"Successful: {task.successful()}")  # True if completed without error

# Wait for result with timeout
try:
    result = task.get(timeout=5)
    print(f"Result: {result}")
except Exception as e:
    print(f"Error: {e}")

# ----------------------------------------
# Method 4: Error handling with retries
# ----------------------------------------
print("\n=== Error handling with retries ===")
# This will fail and retry 3 times
task = risky_operation.delay(-5)

# Wait and get final result (will be exception after retries)
try:
    result = task.get(timeout=30)
    print(f"Result: {result}")
except Exception as e:
    print(f"Final error after retries: {e}")
```

### Expected Output

When running the worker (Terminal 2), you'll see:

```
[2024-01-15 10:30:00,000] INFO - celery@worker1 ready.
[2024-01-15 10:30:01,234] INFO - Task add_numbers[a1b2c3d4] received
Adding 10 + 20...
[2024-01-15 10:30:06,234] INFO - Task add_numbers[a1b2c3d4] succeeded in 5.0s: 30
```

## Common Mistakes

### ❌ Not using a broker

```python
# WRONG: No broker configured
celery = Celery("app")  # Will fail!
```

### ✅ Configure a broker

```python
# CORRECT: Configure broker
celery = Celery("app", broker="redis://localhost:6379/0")
```

### ❌ Blocking in tasks

```python
# WRONG: Using time.sleep in Celery task (blocks worker)
@celery_app.task
def slow_task():
    time.sleep(100)  # Worker is blocked for 100 seconds!
```

### ✅ Use Celery's built-in features

```python
# CORRECT: Use Celery's retry mechanism
@celery_app.task
def slow_task():
    # Use retry with countdown instead of sleep
    raise self.retry(countdown=60)  # Retries after 60 seconds
```

### ❌ Not handling errors

```python
# WRONG: No error handling
@celery_app.task
def risky_task(x):
    return process(x)  # If this fails, task just fails
```

### ✅ Add error handling and retries

```python
# CORRECT: Proper error handling
@celery_app.task(bind=True, max_retries=3)
def safe_task(self, x):
    try:
        return process(x)
    except TemporaryError as e:
        raise self.retry(exc=e, countdown=60)  # Retry up to 3 times
```

## Quick Reference

| Component | Command |
|-----------|---------|
| Start worker | `celery -A tasks worker --loglevel=info` |
| Start beat | `celery -A tasks beat --loglevel=info` |
| Inspect tasks | `celery -A tasks inspect active` |
| Purge queue | `celery -A tasks purge` |

**Task invocation methods:**

```python
# Fire and forget (returns immediately)
task.delay(arg1, arg2)

# Explicit async (same as delay)
task.apply_async(arg1, arg2)

# Run synchronously (blocks, for testing)
task.apply(arg1, arg2)

# Schedule for later
task.apply_async(arg1, arg2, countdown=60)  # 60 seconds from now
task.apply_async(arg1, arg2, eta=datetime(2024, 1, 15, 12, 0))  # At specific time
```

**Task states:**

- `PENDING` - Task received, not started
- `STARTED` - Task has started executing
- `SUCCESS` - Task completed successfully
- `FAILURE` - Task raised an exception
- `RETRY` - Task is retrying

## Next Steps

Continue to [02_setting_up_celery_with_redis.md](02_setting_up_celery_with_redis.md) to learn how to set up Celery with Redis and integrate it with Flask.