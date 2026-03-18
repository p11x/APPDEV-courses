<!-- FILE: 11_async_flask_and_background_tasks/04_celery_advanced/02_scheduled_periodic_tasks.md -->

## Overview

Celery Beat is a scheduler that sends tasks to Celery at specific intervals or times. This file teaches you how to set up periodic tasks, configure complex schedules using crontab syntax, and manage scheduled jobs in production environments.

## Prerequisites

- Celery installed and configured
- Understanding of basic Celery tasks
- Redis or RabbitMQ as message broker

## Core Concepts

### What is Celery Beat?

Celery Beat is a scheduler that spawns processes that send tasks to Celery at regular intervals. It maintains a schedule of tasks to run and when to run them.

Key features:
- Precise scheduling (specific times, days of week)
- Timezone support
- Persistent schedule (stored in broker or database)
- Distributed execution (can run multiple beat processes)

### Schedule Types

| Schedule Type | Description | Example |
|--------------|-------------|----------|
| `interval` | Run every N seconds/minutes/hours | Every 5 minutes |
| `crontab` | Run at specific times (like cron) | Every Monday at 9 AM |
| `solar` | Run at solar events | At sunrise in a specific location |
| `datetime` | Run once at specific datetime | January 1, 2025 at midnight |

### Crontab Expressions

Celery Beat supports cron-like expressions:

| Expression | Meaning |
|------------|---------|
| `*` | Every value |
| `*/15` | Every 15 (units) |
| `30` | Specific value |
| `1,5,10` | Specific values (1, 5, or 10) |
| `1-5` | Range (1, 2, 3, 4, 5) |

Crontab fields: `minute hour day_of_month month day_of_week`

## Code Walkthrough

### Step 1: Configure Celery Beat

```python
# app/celery_app.py
from celery import Celery
from celery.schedules import crontab

def make_celery(app=None):
    celery = Celery(
        "app",
        broker="redis://localhost:6379/0",
        backend="redis://localhost:6379/0"
    )
    
    if app:
        celery.conf.update(app.config)
    
    return celery

# Create celery instance
celery = make_celery()

# ============================================
# Celery Beat Schedule Configuration
# ============================================

celery.conf.beat_schedule = {
    # ----------------------------------------
    # Basic Interval Schedules
    # ----------------------------------------
    
    # Run every minute
    "every-minute": {
        "task": "app.tasks.heartbeat",
        "schedule": 60.0,  # seconds
    },
    
    # Run every 5 minutes
    "every-5-minutes": {
        "task": "app.tasks.cleanup_temp_files",
        "schedule": 300.0,  # 5 * 60 seconds
    },
    
    # Run every hour
    "every-hour": {
        "task": "app.tasks.hourly_report",
        "schedule": 3600.0,
    },
    
    # ----------------------------------------
    # Crontab Schedules
    # ----------------------------------------
    
    # Run daily at midnight
    "daily-midnight": {
        "task": "app.tasks.daily_backup",
        "schedule": crontab(hour=0, minute=0),
    },
    
    # Run daily at 9 AM
    "daily-9am": {
        "task": "app.tasks.morning_notification",
        "schedule": crontab(hour=9, minute=0),
    },
    
    # Run every Monday at 9 AM
    "weekly-monday-9am": {
        "task": "app.tasks.weekly_report",
        "schedule": crontab(hour=9, minute=0, day_of_week=1),
    },
    
    # Run on the 1st of every month at midnight
    "monthly-first": {
        "task": "app.tasks.monthly_archive",
        "schedule": crontab(0, 0, day_of_month=1),
    },
    
    # Run every 15 minutes during business hours (9-5)
    "business-hours-15min": {
        "task": "app.tasks.sync_data",
        "schedule": crontab(minute="*/15", hour="9-17"),
    },
    
    # Run at specific times
    "twice-daily": {
        "task": "app.tasks.sync_data",
        "schedule": crontab(minute=0, hour="9,21"),  # 9 AM and 9 PM
    },
    
    # ----------------------------------------
    # Complex Schedules
    # ----------------------------------------
    
    # Run every 30 seconds (for testing)
    "every-30-seconds": {
        "task": "app.tasks.health_check",
        "schedule": 30.0,
    },
    
    # Run weekdays at specific times
    "weekday-mornings": {
        "task": "app.tasks.weekday_task",
        "schedule": crontab(
            minute=30,
            hour="8-10",
            day_of_week="1-5"  # Monday-Friday
        ),
    },
}
```

### Step 2: Create Periodic Tasks

```python
# app/tasks.py
from app.celery_app import celery
import time
import random
from datetime import datetime

@celery.task(name="app.tasks.heartbeat")
def heartbeat():
    """Simple heartbeat task - runs every minute"""
    print(f"Heartbeat at {datetime.utcnow()}")
    return {
        "status": "alive",
        "timestamp": datetime.utcnow().isoformat()
    }

@celery.task(name="app.tasks.health_check")
def health_check():
    """Check system health"""
    checks = {
        "database": random.choice([True, True, False]),
        "redis": random.choice([True, True, True]),
        "disk_space": True,
        "api": random.choice([True, True, False])
    }
    
    all_healthy = all(checks.values())
    
    return {
        "healthy": all_healthy,
        "checks": checks,
        "timestamp": datetime.utcnow().isoformat()
    }

@celery.task(name="app.tasks.cleanup_temp_files")
def cleanup_temp_files():
    """Clean up temporary files"""
    import os
    import glob
    
    temp_dir = "/tmp/myapp"
    deleted_count = 0
    
    if os.path.exists(temp_dir):
        # Delete files older than 24 hours
        # (simplified example)
        for f in glob.glob(f"{temp_dir}/*"):
            try:
                os.remove(f)
                deleted_count += 1
            except:
                pass
    
    return {
        "deleted_count": deleted_count,
        "timestamp": datetime.utcnow().isoformat()
    }

@celery.task(name="app.tasks.hourly_report")
def hourly_report():
    """Generate hourly statistics report"""
    # In production: query database, generate report
    return {
        "report_type": "hourly",
        "generated_at": datetime.utcnow().isoformat(),
        "metrics": {
            "users_active": random.randint(100, 1000),
            "requests": random.randint(1000, 10000),
            "errors": random.randint(0, 50)
        }
    }

@celery.task(name="app.tasks.daily_backup")
def daily_backup():
    """Perform daily database backup"""
    # In production: backup database to storage
    return {
        "backup_type": "daily",
        "status": "completed",
        "size_mb": random.randint(100, 500),
        "timestamp": datetime.utcnow().isoformat()
    }

@celery.task(name="app.tasks.weekly_report")
def weekly_report():
    """Generate weekly summary report"""
    return {
        "report_type": "weekly",
        "week_start": "2024-01-15",
        "week_end": "2024-01-21",
        "summary": {
            "total_users": random.randint(5000, 10000),
            "total_revenue": random.randint(10000, 50000),
            "active_users": random.randint(1000, 5000)
        }
    }

@celery.task(name="app.tasks.morning_notification")
def morning_notification():
    """Send morning notifications to users"""
    # In production: query users, send emails/push notifications
    return {
        "notification_type": "morning",
        "recipients": random.randint(100, 1000),
        "timestamp": datetime.utcnow().isoformat()
    }

@celery.task(name="app.tasks.monthly_archive")
def monthly_archive():
    """Archive old data monthly"""
    return {
        "action": "monthly_archive",
        "archived_records": random.randint(1000, 10000),
        "timestamp": datetime.utcnow().isoformat()
    }

@celery.task(name="app.tasks.sync_data")
def sync_data():
    """Sync data with external service"""
    return {
        "sync_type": "full",
        "records_synced": random.randint(500, 2000),
        "timestamp": datetime.utcnow().isoformat()
    }

@celery.task(name="app.tasks.weekday_task")
def weekday_task():
    """Task that runs on weekdays"""
    return {
        "task": "weekday_task",
        "day": datetime.utcnow().weekday(),  # 0=Monday, 6=Sunday
        "timestamp": datetime.utcnow().isoformat()
    }
```

### Step 3: Run Celery Beat

```bash
# Start Celery Beat (separate terminal)
celery -A app.celery_app beat --loglevel=info

# For production, you might want:
celery -A app.celery_app beat \
  --loglevel=info \
  --pidfile=/var/run/celerybeat.pid \
  --schedule=/var/run/celerybeat-schedule
```

### Step 4: Dynamic Schedules

Sometimes you need schedules that change based on database or configuration:

```python
# app/schedules.py
from celery import Celery
from celery.schedules import crontab

def create_dynamic_schedule():
    """
    Create schedule dynamically based on configuration
    """
    # This could load from database or config file
    schedule = {}
    
    # Example: Load tasks from database
    # periodic_tasks = PeriodicTask.query.all()
    # for task in periodic_tasks:
    #     schedule[task.name] = {
    #         "task": task.task_name,
    #         "schedule": crontab(minute=task.minute, hour=task.hour),
    #         "args": (task.arg1,),
    #     }
    
    return schedule

# Create Celery with dynamic schedule
celery = Celery("app")

# Update schedule periodically
@celery.task
def reload_schedule():
    """Reload schedule from source"""
    celery.conf.beat_schedule = create_dynamic_schedule()
```

### Step 5: Database-Backed Schedules

For production, you might want to store schedules in a database:

```python
# models.py (SQLAlchemy model)
from flask_sqlalchemy import SQLAlchemy
from datetime import datetime

db = SQLAlchemy()

class PeriodicTask(db.Model):
    """Database-backed periodic task configuration"""
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(100), unique=True)
    task_name = db.Column(db.String(100))
    interval_seconds = db.Column(db.Integer, nullable=True)  # For interval
    crontab_minute = db.Column(db.String(50), nullable=True)  # For crontab
    crontab_hour = db.Column(db.String(50), nullable=True)
    crontab_day_of_week = db.Column(db.String(20), nullable=True)
    crontab_day_of_month = db.Column(db.String(20), nullable=True)
    crontab_month_of_year = db.Column(db.String(20), nullable=True)
    enabled = db.Column(db.Boolean, default=True)
    last_run = db.Column(db.DateTime, nullable=True)
    created_at = db.Column(db.DateTime, default=datetime.utcnow)

# Function to build Celery schedule from database
def get_schedule_from_db():
    """Build Celery schedule from database"""
    from celery.schedules import crontab
    
    tasks = PeriodicTask.query.filter_by(enabled=True).all()
    schedule = {}
    
    for task in tasks:
        if task.interval_seconds:
            schedule[task.name] = {
                "task": task.task_name,
                "schedule": task.interval_seconds,
            }
        elif task.crontab_minute:
            schedule[task.name] = {
                "task": task.task_name,
                "schedule": crontab(
                    minute=task.crontab_minute,
                    hour=task.crontab_hour,
                    day_of_week=task.crontab_day_of_week,
                    day_of_month=task.crontab_day_of_month,
                    month_of_year=task.crontab_month_of_year,
                ),
            }
    
    return schedule
```

### Testing Periodic Tasks

```bash
# Check if beat is running
celery -A app.celery_app inspect scheduled

# View active schedule
celery -A app.celery_app inspect schedule

# Run a task manually
celery -A app.celery_app call daily_backup

# Test crontab schedule
# This should run at next scheduled time
celery -A app.celery_app beat --loglevel=debug
```

## Common Mistakes

### ❌ Using interval for everything

```python
# INEFFICIENT: Using interval for specific times
"run-at-9am": {
    "task": "task",
    "schedule": 3600,  # Every hour, but you have to calculate!
}
```

### ✅ Use crontab for specific times

```python
# CORRECT: Use crontab for specific times
"run-at-9am": {
    "task": "task",
    "schedule": crontab(hour=9, minute=0),  # Exactly at 9 AM
}
```

### ❌ Not handling timezone correctly

```python
# PROBLEM: Timezone issues
celery.conf.timezone = "UTC"  # But schedule is in local time!
```

### ✅ Configure timezone properly

```python
# CORRECT: Use timezone-aware schedules
from celery.schedules import crontab
import pytz

celery.conf.timezone = "America/New_York"
celery.conf.enable_utc = True

# Crontab will use the configured timezone
"daily-9am-nyc": {
    "task": "task",
    "schedule": crontab(hour=9, minute=0),  # 9 AM in NYC timezone
}
```

### ❌ Long-running periodic tasks blocking

```python
# PROBLEM: Task takes longer than interval
"every-minute": {
    "task": "slow_task",  # Takes 2 minutes!
    "schedule": 60.0,
}
# Next task starts before previous finishes!
```

### ✅ Use locks or ensure task finishes

```python
# SOLUTION: Use locks or increase interval
from app.tasks import celery

@celery.task(name="slow_task", bind=True)
def slow_task(self):
    # Use lock to prevent concurrent execution
    lock = redis.lock("slow_task_lock", timeout=3600)
    
    if lock.acquire(blocking=False):
        try:
            # Do work
            pass
        finally:
            lock.release()
    else:
        # Already running
        return "Already running"
```

## Quick Reference

| Schedule | Configuration |
|----------|--------------|
| Every minute | `60.0` |
| Every 5 minutes | `300.0` |
| Every hour | `3600.0` |
| Daily at midnight | `crontab(hour=0, minute=0)` |
| Daily at 9 AM | `crontab(hour=9, minute=0)` |
| Weekly Monday 9 AM | `crontab(hour=9, minute=0, day_of_week=1)` |
| Monthly 1st | `crontab(0, 0, day_of_month=1)` |
| Every 15 min (9-5) | `crontab(minute="*/15", hour="9-17")` |

**Commands:**

```bash
# Start beat
celery -A app.celery_app beat --loglevel=info

# View schedule
celery -A app.celery_app inspect schedule

# Run task manually
celery -A app.celery_app call task_name
```

## Next Steps

Continue to [03_monitoring_with_flower.md](03_monitoring_with_flower.md) to learn how to monitor Celery workers and tasks using Flower.