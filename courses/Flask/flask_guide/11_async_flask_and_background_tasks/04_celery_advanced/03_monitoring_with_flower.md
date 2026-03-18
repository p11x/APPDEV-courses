<!-- FILE: 11_async_flask_and_background_tasks/04_celery_advanced/03_monitoring_with_flower.md -->

## Overview

Flower is a real-time web-based monitoring tool for Celery. It provides a visual interface to inspect workers, monitor task execution, view task history, and manage workers. This file teaches you how to install, configure, and use Flower effectively.

## Prerequisites

- Celery installed and running
- Redis as broker
- Basic understanding of Celery workers

## Core Concepts

### What is Flower?

Flower provides:
- **Real-time monitoring**: See workers, tasks, and queues in real-time
- **Worker management**: Start/stop workers from the UI
- **Task history**: View completed and failed tasks
- **Statistics**: Performance metrics and graphs
- **Remote control**: Control workers and tasks via API

### Flower Architecture

```
┌─────────────────────────────────────────┐
│              Flower UI                   │
│         (Web Interface)                  │
│           Port 5555                      │
└─────────────────┬───────────────────────┘
                  │
                  │ HTTP/WebSocket
                  ▼
┌─────────────────────────────────────────┐
│          Celery Workers                 │
│         (Task Processors)               │
└─────────────────────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────┐
│           Message Broker                │
│            (Redis)                      │
└─────────────────────────────────────────┘
```

## Code Walkthrough

### Step 1: Install Flower

```bash
# Install Flower
pip install flower

# Or with Redis support
pip install flower[redis]
```

### Step 2: Start Flower

```bash
# Basic usage - connect to default Redis
celery -A app.celery flower

# Specify broker URL
celery -A app.celery flower --broker=redis://localhost:6379/0

# Specify port
celery -A app.celery flower --port=5555

# Enable authentication
celery -A app.celery flower --username=admin --password=secret

# Enable SSL
celery -A app.celery flower --certfile=/path/to/cert.pem --keyfile=/path/to/key.pem

# All options together
celery -A app.celery flower \
  --broker=redis://localhost:6379/0 \
  --port=5555 \
  --url_prefix=flower \
  --max_tasks=10000
```

### Step 3: Configure Flower in Celery

```python
# app/celery_app.py
from celery import Celery

celery = Celery(
    "app",
    broker="redis://localhost:6379/0",
    backend="redis://localhost:6379/0"
)

# ----------------------------------------
# Flower Configuration Options
# ----------------------------------------

# Enable events (required for Flower)
celery.conf.task_send_sent_event = True
celery.conf.worker_send_task_events = True

# Task track started (shows STARTED state)
celery.conf.task_track_started = True

# Timezone
celery.conf.timezone = "UTC"
celery.conf.enable_utc = True
```

### Step 4: Using the Flower Web Interface

Once running, open your browser to `http://localhost:5555`:

```
┌─────────────────────────────────────────────────────────────────┐
│  Flower - Celery Monitor                                        │
├─────────────────────────────────────────────────────────────────┤
│  [Dashboard] [Workers] [Tasks] [Queues] [Broker] [Monitor]     │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  Dashboard                                                      │
│  ───────────────────────────────────────────────────────────── │
│                                                                 │
│  Workers: 2 active                                              │
│  Pending Tasks: 5                                               │
│  Completed Today: 1,234                                         │
│  Failed Today: 12                                               │
│                                                                 │
│  [Workers Tab] ─ Shows all workers and their status           │
│  [Tasks Tab] ─ Shows task history and details                 │
│  [Queues Tab] ─ Shows message queues and lengths              │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Step 5: Flower API

Flower provides a REST API for programmatic access:

```bash
# Get list of workers
curl http://localhost:5555/api/workers

# Get worker info
curl http://localhost:5555/api/workers/worker1

# Get active tasks
curl http://localhost:5555/api/tasks

# Get task info by ID
curl http://localhost:5555/api/task/abc123

# Get queue info
curl http://localhost:5555/api/queues

# Get stats
curl http://localhost:5555/api/stats

# Get reserved tasks (queued but not started)
curl http://localhost:5555/api/task/reserved
```

### Step 6: Programmatic Access with Flower

```python
# monitor.py
from flower.client import Client

class CeleryMonitor:
    """Programmatic access to Celery via Flower API"""
    
    def __init__(self, url="http://localhost:5555"):
        self.url = url
        self.client = Client(url)
    
    def get_workers(self):
        """Get all active workers"""
        return self.client.workers()
    
    def get_worker_stats(self, worker_name):
        """Get statistics for a specific worker"""
        return self.client.worker_stats(worker_name)
    
    def get_active_tasks(self):
        """Get currently running tasks"""
        return self.client.active()
    
    def get_scheduled_tasks(self):
        """Get scheduled (queued) tasks"""
        return self.client.scheduled()
    
    def get_reserved_tasks(self):
        """Get reserved tasks"""
        return self.client.reserved()
    
    def get_task_info(self, task_id):
        """Get info about a specific task"""
        return self.client.task_info(task_id)
    
    def revoke_task(self, task_id, terminate=False):
        """Revoke (cancel) a task"""
        return self.client.revoke(task_id, terminate=terminate)
    
    def get_queue_length(self, queue_name):
        """Get number of tasks in queue"""
        return self.client.queue_length(queue_name)
    
    def get_stats(self):
        """Get overall Celery statistics"""
        return self.client.stats()


# Example usage
if __name__ == "__main__":
    monitor = CeleryMonitor()
    
    # Print active workers
    workers = monitor.get_workers()
    print("Active workers:")
    for name, info in workers.items():
        print(f"  {name}: {info.get('status', 'unknown')}")
    
    # Print active tasks
    tasks = monitor.get_active_tasks()
    print("\nActive tasks:")
    for task_id, info in tasks.items():
        print(f"  {task_id}: {info.get('name', 'unknown')}")
    
    # Get task result
    if tasks:
        task_id = list(tasks.keys())[0]
        info = monitor.get_task_info(task_id)
        print(f"\nTask {task_id}: {info}")
```

### Step 7: Flask Routes for Monitoring

```python
# app/monitoring.py
from flask import Blueprint, jsonify
from celery import Celery

monitoring_bp = Blueprint("monitoring", __name__)

# Get Celery instance (assuming it's exposed)
from app import celery_app

@monitoring_bp.route("/api/workers")
def list_workers():
    """List all workers"""
    inspect = celery_app.control.inspect()
    workers = inspect.active()
    return jsonify(workers or {})

@monitoring_bp.route("/api/workers/<worker_name>")
def worker_info(worker_name):
    """Get info for specific worker"""
    inspect = celery_app.control.inspect(worker_name)
    return jsonify({
        "active": inspect.active(),
        "stats": inspect.stats(),
        "registered": inspect.registered(),
    })

@monitoring_bp.route("/api/tasks/active")
def active_tasks():
    """Get currently running tasks"""
    inspect = celery_app.control.inspect()
    active = inspect.active()
    return jsonify(active or {})

@monitoring_bp.route("/api/tasks/scheduled")
def scheduled_tasks():
    """Get scheduled tasks"""
    inspect = celery_app.control.inspect()
    scheduled = inspect.scheduled()
    return jsonify(scheduled or {})

@monitoring_bp.route("/api/tasks/<task_id>")
def task_status(task_id):
    """Get status of a specific task"""
    from celery.result import AsyncResult
    result = AsyncResult(task_id)
    return jsonify({
        "task_id": task_id,
        "status": result.state,
        "result": result.result if result.ready() else None,
        "info": str(result.info) if result.info else None,
    })

@monitoring_bp.route("/api/tasks/<task_id>/revoke", methods=["POST"])
def revoke_task(task_id):
    """Revoke a task"""
    celery_app.control.revoke(task_id, terminate=True)
    return jsonify({"task_id": task_id, "status": "revoked"})

@monitoring_bp.route("/api/queues")
def list_queues():
    """List all queues"""
    inspect = celery_app.control.inspect()
    active_queues = inspect.active_queues()
    return jsonify(active_queues or {})

@monitoring_bp.route("/api/stats")
def get_stats():
    """Get Celery statistics"""
    inspect = celery_app.control.inspect()
    stats = inspect.stats()
    return jsonify(stats or {})
```

### Step 8: Monitoring with Custom Metrics

```python
# app/tasks_with_metrics.py
from app import celery_app
import time
import logging

logger = logging.getLogger(__name__)

@celery_app.task(bind=True, name="app.tasks.monitored_task")
def monitored_task(self, data):
    """
    Task with custom metrics and logging
    """
    task_id = self.request.id
    task_name = "monitored_task"
    
    logger.info(f"Task {task_id} started")
    
    try:
        # Record start time
        start_time = time.time()
        
        # Do work
        result = process_data(data)
        
        # Calculate duration
        duration = time.time() - start_time
        
        # Log success
        logger.info(
            f"Task {task_id} completed successfully in {duration:.2f}s"
        )
        
        return {
            "status": "success",
            "result": result,
            "duration": duration
        }
        
    except Exception as e:
        # Log failure
        logger.error(f"Task {task_id} failed: {str(e)}")
        
        # Update task state for better monitoring
        self.update_state(
            state="FAILURE",
            meta={
                "error": str(e),
                "task_id": task_id
            }
        )
        
        raise

def process_data(data):
    """Example processing function"""
    time.sleep(2)  # Simulate work
    return {"processed": True, "data": data}
```

### Production Deployment

```bash
# Production Flower command with all options
celery -A app.celery_app flower \
  --port=5555 \
  --broker=redis://:password@redis-host:6379/0 \
  --url_prefix=monitor \
  --max_tasks=100000 \
  --persistent=True \
  --db=/var/lib/flower/flower.db \
  --max_memory=512000

# With SSL/TLS
celery -A app.celery_app flower \
  --certfile=/etc/ssl/certs/flower.crt \
  --keyfile=/etc/ssl/private/flower.key \
  --broker=redis://:password@redis-host:6379/0

# Using systemd service (production)
# /etc/systemd/system/flower.service
[Unit]
Description=Flower Celery Monitor
After=network.target

[Service]
Type=simple
User=celery
WorkingDirectory=/opt/app
ExecStart=/opt/app/venv/bin/celery -A app.celery_app flower --port=5555
Restart=on-failure

[Install]
WantedBy=multi-user.target
```

## Common Mistakes

### ❌ Not enabling events

```python
# Flower won't work without events
celery.conf.worker_send_task_events = False  # WRONG!
```

### ✅ Enable events

```python
# CORRECT: Enable events
celery.conf.worker_send_task_events = True
celery.conf.task_send_sent_event = True
```

### ❌ Forgetting to open firewall

```bash
# Can't access Flower!
# Need to open port 5555
sudo ufw allow 5555/tcp
```

### ✅ Configure firewall

```bash
# Allow access to Flower port
sudo ufw allow 5555/tcp comment "Flower monitoring"
```

### ❌ Using default port in production

```bash
# Security risk: default port is well-known
celery -A app flower  # Port 5555
```

### ✅ Use authentication and custom port

```bash
# Secure Flower with auth
celery -A app flower \
  --port=5555 \
  --username=admin \
  --password=strong_password
```

## Quick Reference

| Command | Description |
|---------|-------------|
| `celery -A app flower` | Start Flower |
| `celery -A app flower --port=5555` | Custom port |
| `celery -A app flower --url_prefix=flower` | URL prefix |
| `celery -A app flower --broker=redis://...` | Custom broker |

**Flower API endpoints:**

| Endpoint | Description |
|----------|-------------|
| `/api/workers` | List workers |
| `/api/tasks` | List tasks |
| `/api/queues` | List queues |
| `/api/stats` | Celery statistics |
| `/api/task/<id>` | Task details |
| `/api/workers/<name>` | Worker details |

## Next Steps

Continue to [05_rq_and_alternatives/01_what_is_rq.md](../05_rq_and_alternatives/01_what_is_rq.md) to learn about RQ (Redis Queue), a simpler alternative to Celery.