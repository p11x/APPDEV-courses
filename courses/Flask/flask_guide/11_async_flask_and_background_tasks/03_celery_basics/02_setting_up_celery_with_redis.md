<!-- FILE: 11_async_flask_and_background_tasks/03_celery_basics/02_setting_up_celery_with_redis.md -->

## Overview

Redis is the most popular message broker for Celery due to its simplicity, performance, and widespread adoption. This file walks you through setting up Redis, configuring Celery to use it, and integrating everything with your Flask application using the application factory pattern.

## Prerequisites

- Python installed
- Basic knowledge of Flask application factory pattern
- Understanding of Celery basics from the previous file

## Core Concepts

### Why Redis?

| Feature | Redis | RabbitMQ |
|---------|-------|----------|
| Setup complexity | Easy | Medium |
| Performance | Very high | High |
| Persistence | Optional | Yes |
| Memory usage | Higher | Lower |
| Learning curve | Low | Medium |

Redis serves dual purposes in Celery:
1. **Message Broker** - Stores pending tasks in queues
2. **Result Backend** - Stores task results and states

### Celery with Flask Application Factory

When using Flask's application factory pattern (`create_app()`), Celery needs to be configured differently. The recommended approach is:

1. Create Celery instance outside the app
2. Initialize it with the app using `init_app()`
3. Define tasks in a separate module or within the factory

## Code Walkthrough

### Step 1: Install Dependencies

```bash
# Install Flask and Celery
pip install Flask>=3.0 celery[redis] redis

# The [redis] extra includes:
# - redis library for result backend
# - redis library for broker
```

### Step 2: Create the Project Structure

```
project/
├── app/
│   ├── __init__.py      # Flask app factory + Celery setup
│   ├── tasks.py         # Celery tasks
│   ├── models.py        # Database models
│   └── routes.py        # Flask routes
├── celery_app.py       # Celery instance (optional, for CLI)
├── run.py              # Entry point
└── config.py           # Configuration
```

### Step 3: Configuration File

```python
# config.py
import os

class Config:
    """Base configuration"""
    SECRET_KEY = os.environ.get("SECRET_KEY") or "dev-secret-key"
    
    # Celery Configuration
    # Redis URL format: redis://:[password]@host:port/database
    CELERY_BROKER_URL = os.environ.get("CELERY_BROKER_URL") or "redis://localhost:6379/0"
    CELERY_RESULT_BACKEND = os.environ.get("CELERY_RESULT_BACKEND") or "redis://localhost:6379/0"
    
    # Serialization
    CELERY_TASK_SERIALIZER = "json"
    CELERY_RESULT_SERIALIZER = "json"
    CELERY_ACCEPT_CONTENT = ["json"]
    
    # Timezones
    CELERY_TIMEZONE = "UTC"
    CELERY_ENABLE_UTC = True
    
    # Task settings
    CELERY_TASK_TRACK_STARTED = True  # Track task start time
    CELERY_TASK_TIME_LIMIT = 3600    # Hard limit: 1 hour
    CELERY_TASK_SOFT_TIME_LIMIT = 3000  # Soft limit: 50 minutes
    
    # Result settings
    CELERY_RESULT_EXPIRES = 86400  # Results expire after 24 hours

class DevelopmentConfig(Config):
    """Development configuration"""
    DEBUG = True

class ProductionConfig(Config):
    """Production configuration"""
    DEBUG = False
    # Use a separate Redis database for production
    CELERY_BROKER_URL = os.environ.get("CELERY_BROKER_URL")
    CELERY_RESULT_BACKEND = os.environ.get("CELERY_RESULT_BACKEND")

class TestingConfig(Config):
    """Testing configuration"""
    TESTING = True
    # Use in-memory results for testing
    CELERY_TASK_ALWAYS_EAGER = True  # Run tasks synchronously in tests
    CELERY_RESULT_BACKEND = "cache+memory://"

config = {
    "development": DevelopmentConfig,
    "production": ProductionConfig,
    "testing": TestingConfig,
    "default": DevelopmentConfig
}
```

### Step 4: Create the Flask App Factory with Celery

```python
# app/__init__.py
from flask import Flask
from celery import Celery
from config import config

# ============================================
# Create Celery instance (outside app factory)
# ============================================
def make_celery(app=None):
    """
    Create Celery instance and configure it with Flask app
    
    This pattern allows:
    - Celery to be created independently
    - Multiple Flask apps (testing, production)
    - Access to Flask config in tasks
    """
    # Create Celery instance
    celery = Celery(
        "app",
        broker=config["development"].CELERY_BROKER_URL,
        backend=config["development"].CELERY_RESULT_BACKEND
    )
    
    # Update with default config
    celery.conf.update(
        task_serializer=config["development"].CELERY_TASK_SERIALIZER,
        result_serializer=config["development"].CELERY_RESULT_SERIALIZER,
        accept_content=config["development"].CELERY_ACCEPT_CONTENT,
        timezone=config["development"].CELERY_TIMEZONE,
        enable_utc=config["development"].CELERY_ENABLE_UTC,
    )
    
    # If app provided, use its config and context
    if app:
        # Import Flask context for tasks
        celery.conf.update(app.config)
        
        # Auto-discover tasks in the app's tasks module
        # This imports app.tasks to register @celery.task decorated functions
        celery.autodiscover_tasks(["app"])
        
        class ContextTask(celery.Task):
            """Task that runs within Flask application context"""
            def __call__(self, *args, **kwargs):
                # Create Flask app context for this task
                with app.app_context():
                    return self.run(*args, **kwargs)
        
        celery.Task = ContextTask
    
    return celery

# ============================================
# Flask Application Factory
# ============================================
def create_app(config_name="default"):
    """
    Flask application factory
    
    Usage:
        app = create_app("development")
    """
    app = Flask(__name__)
    
    # Load configuration
    app.config.from_object(config[config_name])
    
    # Initialize extensions
    # (Add your Flask extensions here: db, login, etc.)
    
    # Initialize Celery with this app
    # This makes Flask config available in tasks
    celery = make_celery(app)
    
    # Register blueprints
    # (Add your blueprints here)
    from app import routes
    app.register_blueprint(routes.bp)
    
    return app, celery
```

### Step 5: Define Celery Tasks

```python
# app/tasks.py
from app import create_app
import time
import random

# Get Flask app and celery (must be imported after create_app)
app, celery = create_app("development")

# ============================================
# Example Tasks
# ============================================

@celery.task(name="app.tasks.process_image")
def process_image(image_id, operation="resize"):
    """
    Process an image in the background
    
    Args:
        image_id: ID of the image to process
        operation: Type of operation (resize, filter, thumbnail)
    
    Returns:
        dict with status and output path
    """
    print(f"Processing image {image_id} with {operation}...")
    
    # Simulate image processing time
    time.sleep(random.uniform(2, 5))
    
    output_path = f"/processed/{operation}_{image_id}.jpg"
    
    return {
        "status": "completed",
        "image_id": image_id,
        "operation": operation,
        "output_path": output_path
    }

@celery.task(name="app.tasks.send_welcome_email")
def send_welcome_email(user_id, email, username):
    """
    Send a welcome email to a new user
    
    Args:
        user_id: User's ID
        email: User's email address
        username: User's username
    """
    print(f"Sending welcome email to {email}...")
    
    # Simulate email sending
    time.sleep(1)
    
    # In production, use actual email sending
    # email_service.send(to=email, subject="Welcome!", body=...)
    
    return {
        "status": "sent",
        "user_id": user_id,
        "email": email
    }

@celery.task(name="app.tasks.generate_report")
def generate_report(report_type, start_date, end_date, user_id):
    """
    Generate a report in the background
    
    This is a long-running task suitable for Celery
    """
    print(f"Generating {report_type} report for user {user_id}...")
    
    # Simulate report generation time
    time.sleep(10)
    
    report_path = f"/reports/{report_type}_{user_id}_{start_date}_{end_date}.pdf"
    
    return {
        "status": "completed",
        "report_type": report_type,
        "report_path": report_path,
        "generated_by": user_id
    }

@celery.task(name="app.tasks.cleanup_old_data", bind=True)
def cleanup_old_data(self, days=30):
    """
    Periodic cleanup task with retries
    
    Args:
        days: Delete records older than this many days
    """
    try:
        print(f"Cleaning up data older than {days} days...")
        
        # Simulate cleanup work
        time.sleep(3)
        
        deleted_count = random.randint(100, 1000)
        
        return {
            "status": "completed",
            "deleted_count": deleted_count
        }
    except Exception as e:
        # Retry on failure
        raise self.retry(exc=e, countdown=300)  # Retry after 5 minutes

# ============================================
# Chained Tasks Example
# ============================================

@celery.task(name="app.tasks.upload_and_process")
def upload_and_process(file_path):
    """
    Chain multiple operations together
    """
    # Step 1: Validate file
    print(f"Validating {file_path}...")
    time.sleep(1)
    
    # Step 2: Process file
    print(f"Processing {file_path}...")
    time.sleep(2)
    
    # Step 3: Notify user
    print(f"Notifying user about {file_path}...")
    
    return {"status": "completed", "file_path": file_path}
```

### Step 6: Flask Routes

```python
# app/routes.py
from flask import Blueprint, jsonify, request
from app import create_app
import os

# Create blueprint
bp = Blueprint("main", __name__)

# Get celery instance
_, celery = create_app("development")

# Import tasks (this registers them with Celery)
from app import tasks

@bp.route("/")
def index():
    return jsonify({
        "message": "Flask + Celery API",
        "version": "1.0"
    })

@bp.route("/process-image", methods=["POST"])
def process_image_route():
    """
    Start image processing task
    
    Request JSON:
        {
            "image_id": "123",
            "operation": "resize"
        }
    """
    data = request.get_json()
    image_id = data.get("image_id")
    operation = data.get("operation", "resize")
    
    # Queue the task (returns immediately)
    task = celery.send_task(
        "app.tasks.process_image",
        args=[image_id],
        kwargs={"operation": operation}
    )
    
    return jsonify({
        "task_id": task.id,
        "status": "processing"
    })

@bp.route("/register-user", methods=["POST"])
def register_user():
    """
    Register a new user and send welcome email
    
    Request JSON:
        {
            "username": "john",
            "email": "john@example.com"
        }
    """
    data = request.get_json()
    username = data.get("username")
    email = data.get("email")
    
    # In production: Save user to database first
    user_id = 123  # Simulated
    
    # Queue welcome email (doesn't wait)
    task = celery.send_task(
        "app.tasks.send_welcome_email",
        args=[user_id, email, username]
    )
    
    return jsonify({
        "message": "User registered",
        "user_id": user_id,
        "task_id": task.id
    })

@bp.route("/generate-report", methods=["POST"])
def generate_report_route():
    """
    Start report generation
    
    Request JSON:
        {
            "type": "sales",
            "start_date": "2024-01-01",
            "end_date": "2024-01-31"
        }
    """
    data = request.get_json()
    
    user_id = 123  # In production: get from session
    
    task = celery.send_task(
        "app.tasks.generate_report",
        args=[data["type"], data["start_date"], data["end_date"], user_id]
    )
    
    return jsonify({
        "task_id": task.id,
        "status": "processing"
    })

@bp.route("/task-status/<task_id>")
def get_task_status(task_id):
    """
    Check the status of a task
    
    Returns:
        - PENDING: Task not started
        - STARTED: Task is running
        - SUCCESS: Task completed
        - FAILURE: Task failed
    """
    task = celery.AsyncResult(task_id)
    
    response = {
        "task_id": task.id,
        "status": task.state,
    }
    
    # Include result if available
    if task.ready():
        response["result"] = task.result
    
    # Include info if task started
    if task.info:
        response["info"] = str(task.info)
    
    return jsonify(response)
```

### Step 7: Entry Point

```python
# run.py
from app import create_app

# Create the Flask app
app, celery = create_app("development")

if __name__ == "__main__":
    # Run Flask development server
    app.run(debug=True, port=5000)
```

### Running the Application

**Terminal 1 - Start Redis**:

```bash
# Start Redis
redis-server

# Or on macOS
brew services start redis

# Verify Redis is running
redis-cli ping
# Should return: PONG
```

**Terminal 2 - Start Celery Worker**:

```bash
# Navigate to project directory
cd /path/to/project

# Start worker with verbose output
celery -A app.celery worker --loglevel=info

# Or if using the app factory pattern:
celery -A run.celery worker --loglevel=info

# For multiple workers (production):
# celery -A run.celery worker --concurrency=4 --loglevel=info
```

**Terminal 3 - Start Flask**:

```bash
python run.py
```

### Testing the Setup

```bash
# Test task submission
curl -X POST http://localhost:5000/process-image \
  -H "Content-Type: application/json" \
  -d '{"image_id": "123", "operation": "resize"}'

# Check task status (replace TASK_ID)
curl http://localhost:5000/task-status/<TASK_ID>
```

## Common Mistakes

### ❌ Creating Celery inside the app factory

```python
# WRONG: Celery created inside create_app
def create_app():
    app = Flask(__name__)
    celery = Celery("app")  # Created here - not ideal
    return app
```

### ✅ Create Celery outside, init with app

```python
# CORRECT: Celery created outside, configured in app
celery = Celery("app")

def create_app():
    app = Flask(__name__)
    celery.conf.update(app.config)
    return app
```

### ❌ Forgetting to autodiscover tasks

```python
# WRONG: Tasks not registered
celery = Celery("app")
# Tasks in tasks.py won't be found!
```

### ✅ Autodiscover tasks

```python
# CORRECT: Autodiscover tasks
celery = Celery("app")
celery.autodiscover_tasks(["app"])  # Finds tasks in app/tasks.py
```

## Quick Reference

**Environment variables:**

```bash
export CELERY_BROKER_URL="redis://localhost:6379/0"
export CELERY_RESULT_BACKEND="redis://localhost:6379/0"
export FLASK_APP="run.py"
```

**Celery commands:**

```bash
# Start worker
celery -A run.celery worker --loglevel=info

# Start beat (scheduler)
celery -A run.celery beat --loglevel=info

# Monitor tasks
celery -A run.celery inspect active
celery -A run.celery inspect stats
```

## Next Steps

Continue to [03_creating_tasks.md](03_creating_tasks.md) to learn more about creating and managing Celery tasks.