<!-- FILE: 11_async_flask_and_background_tasks/03_celery_basics/03_creating_tasks.md -->

## Overview

This file covers the full spectrum of Celery task creation: basic tasks, tasks with retries, periodic tasks, chaining tasks together, and task routing. You'll learn how to create robust, production-ready tasks that handle errors gracefully and scale well.

## Prerequisites

- Celery installed and configured
- Flask app with Celery integration set up
- Understanding of basic task queue concepts

## Core Concepts

### Task Decorator Options

The `@celery.task` decorator provides many options:

```python
@celery.task(
    name="task_name",           # Custom name (auto-generated if missing)
    bind=True,                 # Pass self (task instance) to function
    autoretry_for=(Exception,), # Auto-retry on exceptions
    retry_backoff=True,        # Exponential backoff
    retry_backoff_max=600,     # Max backoff between retries
    retry_jitter=True,         # Add randomness to prevent thundering herd
    max_retries=3,            # Maximum retry attempts
    default_retry_delay=180,  # Default delay between retries (seconds)
    acks_late=True,           # Acknowledge after completion (not before)
    reject_on_worker_lost=True, # Requeue if worker dies
    time_limit=3600,          # Hard time limit (seconds)
    soft_time_limit=3000,     # Soft time limit (raises exception)
    ignore_result=False,      # Don't store result
    store_errors_even_if_ignored=True, # Store errors even if ignore_result
)
```

### Task Invocation Methods

| Method | Description | Use Case |
|--------|-------------|----------|
| `.delay(arg1, arg2)` | Fire and forget | Most cases |
| `.apply_async(arg1, arg2, countdown=60)` | Delayed execution | Scheduling |
| `.apply_async(arg1, arg2, eta=datetime(...))` | Execute at specific time | One-time scheduling |
| `.apply(arg1, arg2)` | Synchronous (blocking) | Testing/debugging |

## Code Walkthrough

### Basic Task Examples

```python
# app/tasks.py
from app import create_app
import time
import random
import json

app, celery = create_app("development")

# ============================================
# 1. Simple Task (no special options)
# ============================================

@celery.task(name="app.tasks.simple_add")
def simple_add(a, b):
    """The simplest possible task"""
    return a + b

# ============================================
# 2. Task with bind=True (access to self)
# ============================================

@celery.task(bind=True, name="app.tasks.task_with_self")
def task_with_self(self, x, y):
    """
    bind=True gives access to 'self' (the task instance)
    
    This allows:
    - Retrying with self.retry()
    - Accessing request info
    - Custom logging
    """
    # Log current retry count
    retry_count = self.request.retries
    print(f"Attempt #{retry_count + 1} for {x} + {y}")
    
    if random.random() < 0.3:  # 30% chance of failure
        raise ValueError("Random failure!")
    
    return {"result": x + y, "attempts": retry_count + 1}

# ============================================
# 3. Task with automatic retries
# ============================================

@celery.task(
    autoretry_for=(ValueError, ConnectionError),
    retry_backoff=True,
    retry_backoff_max=600,
    retry_jitter=True,
    max_retries=5,
    name="app.tasks.auto_retry_task"
)
def auto_retry_task(url):
    """
    Task that automatically retries on specific exceptions
    
    retry_backoff=True: delays are 1s, 2s, 4s, 8s, 16s... (exponential)
    retry_backoff_max=600: max delay is 10 minutes
    retry_jitter=True: adds randomness to prevent thundering herd
    """
    import requests
    
    response = requests.get(url)
    
    if response.status_code == 500:
        raise ValueError("Server error - will retry")
    
    return {"status": response.status_code, "data": response.json()}

# ============================================
# 4. Task with manual retry
# ============================================

@celery.task(
    bind=True,
    max_retries=3,
    name="app.tasks.manual_retry_task"
)
def manual_retry_task(self, data):
    """
    Task with manual retry control
    
    Use self.retry() when you need custom logic
    to decide whether to retry
    """
    try:
        # Try to process data
        result = process_data(data)
        return result
    except TemporaryFailure as e:
        # Check if we should retry
        if self.request.retries < self.max_retries:
            # Calculate delay based on retry count
            delay = 2 ** self.request.retries  # 1s, 2s, 4s
            
            # Retry with custom delay
            raise self.retry(exc=e, countdown=delay)
        else:
            # Max retries reached - fail permanently
            return {"error": "Max retries exceeded", "original_error": str(e)}

# ============================================
# 5. Task with time limits
# ============================================

@celery.task(
    time_limit=300,      # Hard limit: 5 minutes
    soft_time_limit=240, # Soft limit: 4 minutes
    name="app.tasks.limited_task"
)
def limited_task(data):
    """
    Task with time limits
    
    time_limit: Worker will kill task after this
    soft_time_limit: Raises SoftTimeLimitExceeded exception
    """
    import signal
    
    # Handle soft timeout gracefully
    def timeout_handler(signum, frame):
        raise TimeoutError("Operation timed out")
    
    signal.signal(signal.SIGALRM, timeout_handler)
    signal.alarm(240)  # 4 minutes
    
    result = heavy_computation(data)
    
    signal.alarm(0)  # Cancel alarm
    return result

# ============================================
# 6. Task that ignores result
# ============================================

@celery.task(
    ignore_result=True,  # Don't store result in backend
    name="app.tasks.fire_and_forget"
)
def fire_and_forget(data):
    """
    Task that doesn't need to store result
    
    Use ignore_result=True for:
    - Fire-and-forget notifications
    - Logging tasks
    - Tasks where result doesn't matter
    
    This improves performance
    """
    print(f"Processing {data}...")
    # Do work but don't store result
    return None  # Result won't be stored

# ============================================
# 7. Periodic tasks (Celery Beat)
# ============================================

@celery.task(
    name="app.tasks.daily_cleanup"
)
def daily_cleanup():
    """
    Task that runs daily via Celery Beat
    
    This task itself doesn't have a schedule;
    the schedule is defined in Celery Beat config
    """
    from datetime import datetime
    print(f"Running daily cleanup at {datetime.utcnow()}")
    
    # Perform cleanup
    deleted_count = 0
    
    # Example: Delete old sessions
    # deleted_count = Session.query.filter(...).delete()
    
    return {
        "status": "completed",
        "deleted_count": deleted_count,
        "timestamp": datetime.utcnow().isoformat()
    }

@celery.task(name="app.tasks.hourly_heartbeat")
def hourly_heartbeat():
    """Task that runs every hour"""
    from datetime import datetime
    print(f"Heartbeat at {datetime.utcnow()}")
    return {"status": "alive", "timestamp": datetime.utcnow().isoformat()}

# ============================================
# 8. Chained tasks
# ============================================

@celery.task(name="app.tasks.step_one")
def step_one(data):
    """First step in a pipeline"""
    print(f"Step 1: Processing {data}")
    time.sleep(2)  # Simulate work
    result = data.upper()
    return {"result": result, "next_step": "step_two"}

@celery.task(name="app.tasks.step_two")
def step_two(data):
    """Second step in a pipeline"""
    result = data["result"]
    print(f"Step 2: Transforming {result}")
    time.sleep(1)
    return {"result": result + "_transformed", "next_step": "step_three"}

@celery.task(name="app.tasks.step_three")
def step_three(data):
    """Final step in a pipeline"""
    result = data["result"]
    print(f"Step 3: Finalizing {result}")
    return {"result": result + "_final", "status": "complete"}

# Convenience function for running chain
def run_pipeline(initial_data):
    """Run a chain of tasks"""
    from celery import chain
    
    # Execute step_one -> step_two -> step_three
    result = chain(
        step_one.s(initial_data),
        step_two.s(),
        step_three.s()
    )()
    
    return result

# ============================================
# 9. Grouped tasks (parallel execution)
# ============================================

@celery.task(name="app.tasks.process_item")
def process_item(item_id):
    """Process a single item"""
    time.sleep(1)  # Simulate work
    return {"item_id": item_id, "processed": True}

def process_all_items(item_ids):
    """Process multiple items in parallel"""
    from celery import group
    
    # Create a group of tasks to run in parallel
    tasks = group(process_item.s(item_id) for item_id in item_ids)
    
    # Execute all tasks in parallel
    result = tasks()
    
    return result

# ============================================
# 10. Task routing
# ============================================

@celery.task(
    name="app.tasks.high_priority_task",
    queue="high_priority",
    routing_key="high_priority"
)
def high_priority_task(data):
    """Task that goes to high priority queue"""
    return process_quickly(data)

@celery.task(
    name="app.tasks.low_priority_task",
    queue="low_priority",
    routing_key="low_priority"
)
def low_priority_task(data):
    """Task that goes to low priority queue"""
    return process_slowly(data)
```

### Testing Tasks

```python
# test_tasks.py
from app import create_app
from app.tasks import (
    simple_add, task_with_self, auto_retry_task,
    manual_retry_task, step_one, step_two,
    process_item, process_all_items
)

app, celery = create_app("testing")

def test_simple_task():
    """Test simple task execution"""
    # Synchronous execution (for testing)
    result = simple_add.apply(args=[10, 20])
    print(f"Result: {result.result}")  # 30
    print(f"Status: {result.status}")   # SUCCESS

def test_async_task():
    """Test asynchronous execution"""
    # Queue task (async)
    task = simple_add.delay(10, 20)
    
    # Wait for result
    result = task.get(timeout=10)
    print(f"Result: {result}")  # 30

def test_task_with_retry():
    """Test retry mechanism"""
    # This might fail and retry
    task = task_with_self.delay(1, 2)
    result = task.get(timeout=30)
    print(f"Result: {result}")

def test_chain():
    """Test chained tasks"""
    from celery import chain
    
    # Run chain synchronously for testing
    result = chain(
        step_one.s("hello"),
        step_two.s(),
        step_three.s()
    ).apply()
    
    print(f"Chain result: {result}")  # {"result": "HELLO_transformed_final", "status": "complete"}

def test_group():
    """Test grouped parallel tasks"""
    from celery import group
    
    result = group(
        process_item.s(i) for i in range(5)
    ).apply()
    
    print(f"Results: {result.results}")
    # [<Result: item_0>, <Result: item_1>, ...]

if __name__ == "__main__":
    print("=== Testing Simple Task ===")
    test_simple_task()
    
    print("\n=== Testing Async Task ===")
    test_async_task()
    
    print("\n=== Testing Chain ===")
    test_chain()
    
    print("\n=== Testing Group ===")
    test_group()
```

### Celery Beat Schedule Configuration

```python
# app/celerybeat_schedule.py
from celery.schedules import crontab

# Add this to your Celery configuration
celery.conf.beat_schedule = {
    # Run every minute (for testing)
    "every-minute": {
        "task": "app.tasks.hourly_heartbeat",
        "schedule": 60.0,  # seconds
    },
    
    # Run every hour
    "every-hour": {
        "task": "app.tasks.hourly_heartbeat",
        "schedule": 3600.0,
    },
    
    # Run daily at midnight
    "daily-cleanup": {
        "task": "app.tasks.daily_cleanup",
        "schedule": crontab(hour=0, minute=0),  # Midnight
    },
    
    # Run every Monday at 9 AM
    "weekly-report": {
        "task": "app.tasks.weekly_report",
        "schedule": crontab(hour=9, minute=0, day_of_week=1),
    },
    
    # Run on the 1st of every month
    "monthly-archive": {
        "task": "app.tasks.monthly_archive",
        "schedule": crontab(0, 0, day_of_month=1),
    },
}
```

## Common Mistakes

### ❌ Not using bind=True when needed

```python
# WRONG: Can't retry or access request info
@celery.task
def bad_task(x):
    if x < 0:
        raise Exception("Negative!")  # Can't retry manually
    return x * 2
```

### ✅ Use bind=True for more control

```python
# CORRECT: Can retry and access task info
@celery.task(bind=True)
def good_task(self, x):
    if x < 0:
        raise self.retry(exc=Exception("Negative!"), countdown=5)
    return x * 2
```

### ❌ Blocking in tasks

```python
# WRONG: Using time.sleep blocks the worker!
@celery.task
def slow_task():
    time.sleep(100)  # Worker is blocked!
```

### ✅ Use Celery's built-in retry

```python
# CORRECT: Use retry instead of sleep
@celery.task
def better_task():
    raise self.retry(countdown=60)  # Worker can do other work
```

### ❌ Not handling task errors

```python
# WRONG: Silent failures
@celery.task
def risky_task():
    process()  # If this fails, you won't know
```

### ✅ Always handle errors

```python
# CORRECT: Proper error handling
@celery.task(bind=True, max_retries=3)
def safe_task(self):
    try:
        return process()
    except RecoverableError as e:
        raise self.retry(exc=e)
    except Exception as e:
        # Log and handle final failure
        log_error(e)
        return {"error": str(e)}
```

## Quick Reference

**Task invocation:**

```python
# Fire and forget
task.delay(arg1, arg2)

# With options
task.apply_async(
    args=[arg1, arg2],
    countdown=60,           # Seconds to wait before executing
    eta=datetime(...),     # Execute at specific time
    queue="high_priority", # Route to specific queue
    priority=5,           # Task priority
)

# Synchronous (for testing)
task.apply(args=[arg1, arg2])
```

**Retry options:**

```python
# Manual retry
self.retry(exc=Exception("error"), countdown=60)

# Automatic retry (decorator)
@celery.task(autoretry_for=(Exception,), retry_backoff=True, max_retries=3)
def auto_task():
    pass
```

## Next Steps

Continue to [04_celery_advanced/01_task_chaining_and_groups.md](../04_celery_advanced/01_task_chaining_and_groups.md) to learn about advanced Celery patterns like task chains, groups, and chords.