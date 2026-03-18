<!-- FILE: 11_async_flask_and_background_tasks/05_rq_and_alternatives/02_setting_up_rq_with_flask.md -->

## Overview

This file teaches you how to integrate RQ (Redis Queue) with Flask applications. You'll learn about Flask-RQ2 (the recommended Flask extension for RQ), how to configure workers, and how to create a complete background task system.

## Prerequisites

- Flask installed
- Redis installed and running
- Basic understanding of RQ concepts

## Core Concepts

### Flask-RQ2

Flask-RQ2 is a Flask extension that provides RQ integration. It offers:
- Automatic Redis connection management
- Management commands for workers
- Decorators for task functions
- Web dashboard

### Key Differences from Celery

| Aspect | Celery | RQ |
|--------|--------|-----|
| Configuration | Complex | Simple |
| Multiple queues | Complex | Built-in |
| Scheduling | Built-in | External cron |
| Results | Multiple backends | Redis only |

## Code Walkthrough

### Step 1: Install Dependencies

```bash
# Install Flask and RQ
pip install Flask>=3.0
pip install rq
pip install flask-rq2

# Install Redis
pip install redis
```

### Step 2: Flask Application Setup

```python
# app/__init__.py
from flask import Flask
from flask_rq2 import RQ
import os

# Create RQ instance
rq = RQ()

def create_app(config_name="development"):
    """Flask application factory"""
    app = Flask(__name__)
    
    # Load configuration
    app.config["RQ_DEFAULT_TIMEOUT"] = 360  # Job timeout in seconds
    app.config["RQ_RESULT_TTL"] = 86400    # Result TTL in seconds
    app.config["RQ_JOB_CLASS"] = None       # Custom job class
    
    # Redis configuration
    redis_url = os.environ.get("REDIS_URL", "redis://localhost:6379/0")
    app.config["RQ_REDIS_URL"] = redis_url
    
    # Initialize RQ with app
    rq.init_app(app)
    
    # Register blueprints
    from app import routes
    app.register_blueprint(routes.bp)
    
    return app
```

### Step 3: Define Tasks

```python
# app/tasks.py
from app import rq, create_app
import time
import random

# Get the Flask app
app = create_app()

# ============================================
# Task 1: Send Welcome Email
# ============================================

@rq.job(timeout=60, result_ttl=3600)
def send_welcome_email(user_id, email, username):
    """
    Send a welcome email to new user
    
    Args:
        user_id: User's ID
        email: User's email
        username: User's username
    """
    print(f"Sending welcome email to {email}...")
    
    # Simulate email sending
    time.sleep(2)
    
    # In production, use actual email service
    # email_service.send_welcome(email, username)
    
    return {
        "status": "sent",
        "user_id": user_id,
        "email": email
    }

# ============================================
# Task 2: Process Image
# ============================================

@rq.job(timeout=300, result_ttl=7200)
def process_image(image_path, operations):
    """
    Process an image with multiple operations
    
    Args:
        image_path: Path to image file
        operations: List of operations (resize, filter, etc.)
    """
    print(f"Processing image: {image_path}")
    print(f"Operations: {operations}")
    
    # Simulate processing
    for op in operations:
        print(f"  Applying {op}...")
        time.sleep(1)
    
    output_path = f"/processed/{image_path.split('/')[-1]}"
    
    return {
        "status": "completed",
        "input": image_path,
        "output": output_path,
        "operations": operations
    }

# ============================================
# Task 3: Generate Report
# ============================================

@rq.job(timeout=600, result_ttl=86400)
def generate_report(report_type, start_date, end_date, user_id):
    """
    Generate a report
    
    Args:
        report_type: Type of report
        start_date: Report start date
        end_date: Report end date
        user_id: User requesting report
    """
    print(f"Generating {report_type} report...")
    print(f"Period: {start_date} to {end_date}")
    print(f"Requested by: {user_id}")
    
    # Simulate report generation
    time.sleep(5)
    
    report_data = {
        "total_records": random.randint(100, 10000),
        "summary": {
            "revenue": random.randint(1000, 100000),
            "expenses": random.randint(500, 50000)
        }
    }
    
    return {
        "status": "completed",
        "report_type": report_type,
        "report_data": report_data,
        "download_url": f"/reports/{report_type}_{user_id}.pdf"
    }

# ============================================
# Task 4: Long-running calculation
# ============================================

@rq.job(timeout=1800, result_ttl=3600)
def complex_calculation(n):
    """
    Complex calculation task
    
    Args:
        n: Input number
    """
    print(f"Calculating for n={n}...")
    
    # Simulate heavy computation
    result = 0
    for i in range(n):
        result += i ** 2
        if i % 10000 == 0:
            print(f"  Progress: {i}/{n}")
            time.sleep(0.1)
    
    return {
        "input": n,
        "result": result,
        "steps": n
    }
```

### Step 4: Flask Routes

```python
# app/routes.py
from flask import Blueprint, jsonify, request, render_template
from flask_rq2 import get_queue
from app import create_app
from app.tasks import (
    send_welcome_email,
    process_image,
    generate_report,
    complex_calculation
)

# Get RQ from app
app = create_app()
rq = app.extensions["rq"]

bp = Blueprint("main", __name__)

@bp.route("/")
def index():
    return jsonify({
        "message": "Flask + RQ API",
        "endpoints": {
            "register": "/register (POST)",
            "process_image": "/process-image (POST)",
            "generate_report": "/generate-report (POST)",
            "calculate": "/calculate (POST)",
            "job_status": "/job/<job_id> (GET)"
        }
    })

# ============================================
# Route: Register User
# ============================================

@bp.route("/register", methods=["POST"])
def register():
    """
    Register a new user and queue welcome email
    
    Request JSON:
        {
            "username": "john",
            "email": "john@example.com"
        }
    """
    data = request.get_json()
    username = data.get("username")
    email = data.get("email")
    
    # Save user to database (simplified)
    user_id = 123
    
    # Enqueue welcome email
    job = send_welcome_email.queue(
        user_id,
        email,
        username,
        timeout=60,
        result_ttl=3600
    )
    
    return jsonify({
        "message": "User registered",
        "user_id": user_id,
        "email_job_id": job.id,
        "status": "processing"
    })

# ============================================
# Route: Process Image
# ============================================

@bp.route("/process-image", methods=["POST"])
def process_image_route():
    """
    Process an image
    
    Request JSON:
        {
            "image_path": "/uploads/photo.jpg",
            "operations": ["resize", "filter", "thumbnail"]
        }
    """
    data = request.get_json()
    image_path = data.get("image_path")
    operations = data.get("operations", ["resize"])
    
    # Enqueue image processing
    job = process_image.queue(
        image_path,
        operations,
        timeout=300,
        result_ttl=7200
    )
    
    return jsonify({
        "job_id": job.id,
        "status": "queued",
        "image_path": image_path
    })

# ============================================
# Route: Generate Report
# ============================================

@bp.route("/generate-report", methods=["POST"])
def generate_report_route():
    """
    Generate a report
    
    Request JSON:
        {
            "type": "sales",
            "start_date": "2024-01-01",
            "end_date": "2024-01-31"
        }
    """
    data = request.get_json()
    report_type = data.get("type")
    start_date = data.get("start_date")
    end_date = data.get("end_date")
    user_id = 123  # Get from session
    
    # Enqueue report generation
    job = generate_report.queue(
        report_type,
        start_date,
        end_date,
        user_id,
        timeout=600,
        result_ttl=86400
    )
    
    return jsonify({
        "job_id": job.id,
        "status": "queued",
        "report_type": report_type
    })

# ============================================
# Route: Complex Calculation
# ============================================

@bp.route("/calculate", methods=["POST"])
def calculate_route():
    """
    Start complex calculation
    
    Request JSON:
        {
            "n": 100000
        }
    """
    data = request.get_json()
    n = data.get("n", 1000)
    
    job = complex_calculation.queue(
        n,
        timeout=1800,
        result_ttl=3600
    )
    
    return jsonify({
        "job_id": job.id,
        "status": "queued",
        "n": n
    })

# ============================================
# Route: Job Status
# ============================================

@bp.route("/job/<job_id>")
def job_status(job_id):
    """Get status and result of a job"""
    from rq.job import Job
    from redis import Redis
    
    redis_conn = Redis()
    
    try:
        job = Job.fetch(job_id, connection=redis_conn)
    except:
        return jsonify({"error": "Job not found"}), 404
    
    response = {
        "job_id": job.id,
        "status": job.status,
        "created_at": job.created_at.isoformat() if job.created_at else None,
        "enqueued_at": job.enqueued_at.isoformat() if job.enqueued_at else None,
    }
    
    # Add result if available
    if job.is_finished:
        response["result"] = job.return_value
    elif job.is_failed:
        response["error"] = job.exc_info
    
    # Add progress if available
    if job.meta.get("progress"):
        response["progress"] = job.meta["progress"]
    
    return jsonify(response)

# ============================================
# Route: Queue Status
# ============================================

@bp.route("/queues")
def queues():
    """Get status of all queues"""
    from rq import Queue
    from redis import Redis
    
    redis_conn = Redis()
    
    # Get all queues
    all_queues = Queue.all(connection=redis_conn)
    
    queue_info = []
    for queue in all_queues:
        queue_info.append({
            "name": queue.name,
            "count": len(queue),
            "jobs": [job.id for job in queue.jobs[:5]]  # First 5 jobs
        })
    
    return jsonify({
        "queues": queue_info,
        "total_queues": len(all_queues)
    })
```

### Step 5: Running the Application

**Start Redis:**

```bash
# macOS
brew services start redis

# Ubuntu/Debian
sudo systemctl start redis

# Or run directly
redis-server
```

**Start Flask:**

```bash
python run.py
```

**Start RQ Worker:**

```bash
# Start worker on default queue
rq worker

# Start worker with custom queue
rq worker high-priority low-priority

# Start with multiple workers
rq worker --workers 4

# Start with verbose output
rq worker -v
```

**Start RQ Dashboard:**

```bash
# Start dashboard on port 5001
rq dashboard -p 5001
```

### Testing

```bash
# Register user (queues welcome email)
curl -X POST http://localhost:5000/register \
  -H "Content-Type: application/json" \
  -d '{"username": "john", "email": "john@example.com"}'

# Process image
curl -X POST http://localhost:5000/process-image \
  -H "Content-Type: application/json" \
  -d '{"image_path": "/uploads/photo.jpg", "operations": ["resize", "thumbnail"]}'

# Generate report
curl -X POST http://localhost:5000/generate-report \
  -H "Content-Type: application/json" \
  -d '{"type": "sales", "start_date": "2024-01-01", "end_date": "2024-01-31"}'

# Calculate
curl -X POST http://localhost:5000/calculate \
  -H "Content-Type: application/json" \
  -d '{"n": 10000}'

# Check job status (replace JOB_ID)
curl http://localhost:5000/job/JOB_ID
```

## Common Mistakes

### ❌ Forgetting to initialize RQ

```python
# WRONG: RQ not initialized
app = Flask(__name__)
# Missing: rq.init_app(app)
```

### ✅ Initialize RQ properly

```python
# CORRECT
rq = RQ()
rq.init_app(app)
```

### ❌ Not using @rq.job decorator

```python
# WRONG: Just a regular function
def send_email(to, subject):
    pass
```

### ✅ Use @rq.job decorator

```python
# CORRECT
@rq.job()
def send_email(to, subject):
    pass
```

### ❌ Blocking in routes

```python
# WRONG: Wait for job to complete
@app.route("/process")
def process():
    job = slow_task.queue()
    result = job.get()  # BLOCKS!
    return jsonify(result)
```

### ✅ Return job ID

```python
# CORRECT
@app.route("/process")
def process():
    job = slow_task.queue()
    return jsonify({"job_id": job.id})

@app.route("/result/<job_id>")
def result(job_id):
    job = Job.fetch(job_id)
    if job.is_finished:
        return jsonify(job.return_value)
    return jsonify({"status": job.status})
```

## Quick Reference

**Flask-RQ2 configuration:**

```python
app.config["RQ_DEFAULT_TIMEOUT"] = 300  # Job timeout
app.config["RQ_RESULT_TTL"] = 86400     # Result TTL
app.config["RQ_REDIS_URL"] = "redis://localhost:6379/0"
```

**Task decorator:**

```python
@rq.job(timeout=60, result_ttl=3600)
def my_task(arg):
    pass
```

**Commands:**

```bash
# Start worker
rq worker

# Start dashboard
rq dashboard -p 5001
```

## Next Steps

Continue to [03_rq_vs_celery_comparison.md](03_rq_vs_celery_comparison.md) to get a detailed comparison and help decide which task queue is right for your project.