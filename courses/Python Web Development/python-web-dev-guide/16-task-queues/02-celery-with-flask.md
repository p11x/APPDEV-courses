# Celery with Flask

## What You'll Learn
- Integrating Celery with Flask applications
- Flask application context in tasks
- Monitoring Celery with Flower

## Prerequisites
- Completed task queue basics

## Flask-Celery Integration

```bash
pip install celery flask
```

## Basic Setup

```python
from flask import Flask
from celery import Celery

app = Flask(__name__)
app.config['CELERY_BROKER_URL'] = 'redis://localhost:6379/0'
app.config['CELERY_RESULT_BACKEND'] = 'redis://localhost:6379/0'

celery = Celery(
    app.import_name,
    broker=app.config['CELERY_BROKER_URL'],
    backend=app.config['CELERY_RESULT_BACKEND']
)

celery.conf.update(app.config)

@celery.task
def process_uploaded_file(file_id: int, filename: str) -> dict:
    """Process uploaded file in background"""
    import time
    import os
    
    # Simulate file processing
    time.sleep(3)
    
    return {
        'file_id': file_id,
        'filename': filename,
        'processed': True,
        'size': 1024000
    }

@app.route('/upload', methods=['POST'])
def upload_file():
    file_id = 1
    filename = 'document.pdf'
    
    # Queue the task
    task = process_uploaded_file.delay(file_id, filename)
    
    return {'task_id': task.id, 'status': 'processing'}

@app.route('/task/<task_id>')
def get_task_status(task_id: str):
    from celery.result import AsyncResult
    result = AsyncResult(task_id)
    
    return {
        'task_id': task_id,
        'status': result.state,
        'result': result.result if result.ready() else None
    }
```

🔍 **Line-by-Line Breakdown:**
1. `celery = Celery(...)` — Initialize Celery with Flask config
2. `@celery.task` — Register as background task
3. `.delay()` — Queue task asynchronously
4. `AsyncResult(task_id)` — Check task status

## Using Application Context in Tasks

```python
from flask import Flask
from celery import Celery
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///app.db'
app.config['CELERY_BROKER_URL'] = 'redis://localhost:6379/0'

db = SQLAlchemy(app)
celery = Celery(app.import_name, broker=app.config['CELERY_BROKER_URL'])

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    email = db.Column(db.String(120))

@celery.task
def send_welcome_email(user_id: int) -> dict:
    """Task that needs database access"""
    # Recreate app context for database access
    with app.app_context():
        user = User.query.get(user_id)
        
        if user:
            # Send email logic here
            print(f"Sending welcome email to {user.email}")
            return {'status': 'sent', 'user_id': user_id}
        
        return {'status': 'failed', 'reason': 'User not found'}

@app.route('/register', methods=['POST'])
def register():
    # Create user in database
    user = User(email='new@example.com')
    db.session.add(user)
    db.session.commit()
    
    # Queue welcome email
    send_welcome_email.delay(user.id)
    
    return {'message': 'User registered', 'user_id': user.id}
```

## Periodic Tasks

```python
from celery import Celery
from celery.schedules import crontab

app = Celery('tasks', broker='redis://localhost:6379/0')

# Configure periodic tasks
app.conf.beat_schedule = {
    'daily-cleanup': {
        'task': 'cleanup_old_records',
        'schedule': crontab(hour=2, minute=0),  # 2 AM daily
    },
    'hourly-report': {
        'task': 'generate_hourly_report',
        'schedule': 3600,  # Every hour
    },
}

@celery.task
def cleanup_old_records() -> dict:
    """Clean up old database records"""
    # Cleanup logic
    return {'deleted_count': 42}

@celery.task
def generate_hourly_report() -> dict:
    """Generate hourly analytics"""
    return {'generated': True}
```

## Summary
- Integrate Celery with Flask using Flask-Celery
- Use app context for database access in tasks
- Use Celery Beat for periodic tasks
- Check task status with AsyncResult

## Next Steps
→ Continue to `03-celery-with-fastapi.md`
