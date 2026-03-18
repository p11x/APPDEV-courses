<!-- FILE: 11_async_flask_and_background_tasks/01_async_fundamentals/03_when_to_use_async.md -->

## Overview

Async programming isn't always the right solution. This file teaches you how to decide when to use async Flask versus synchronous Flask, and when to consider background task systems like Celery instead. You'll learn the trade-offs and get decision-making frameworks for real-world projects.

## Prerequisites

- Understanding of sync vs async code
- Familiarity with async/await syntax
- Basic Flask knowledge

## Core Concepts

### When Async Makes Sense

Async programming provides significant benefits in specific scenarios:

#### 1. I/O-Bound Operations

If your application spends most of its time waiting for external services, async can dramatically improve throughput:

- **External API calls**: Fetching data from third-party APIs
- **Database queries**: Though database performance varies
- **File operations**: Reading/writing files (especially on network storage)
- **HTTP requests**: Calling other microservices
- **WebSocket connections**: Maintaining long-lived connections

```python
# Perfect for async: calling multiple APIs concurrently
@app.route("/aggregate-data")
async def aggregate_data():
    # These run concurrently - total time = slowest request
    user_data = await fetch_user_api()
    order_data = await fetch_order_api()
    inventory_data = await fetch_inventory_api()
    
    return jsonify(merge_data(user_data, order_data, inventory_data))
```

#### 2. High Concurrency Requirements

When you need to handle thousands of simultaneous connections:

- **Real-time applications**: Chat apps, live dashboards, notification systems
- **Streaming services**: Video/audio streaming, Server-Sent Events
- **Microservices**: Services that act as aggregators or proxies
- **WebSocket servers**: Long-lived bidirectional connections

```python
# Handle thousands of WebSocket connections efficiently
@socketio.async_task
async def handle_message(data):
    # Process each message asynchronously
    result = await process_message(data)
    await socketio.emit('response', result)
```

#### 3. Long-Polling and Streaming

When clients hold connections open for extended periods:

```python
# Server-Sent Events - perfect for async
@app.route("/events")
async def events():
    async def generate():
        for update in get_continuous_updates():
            yield f"data: {json.dumps(update)}\n\n"
    
    return Response(generate(), mimetype="text/event-stream")
```

### When Async Is NOT the Right Choice

Async adds complexity. In these cases, stick with synchronous Flask:

#### 1. CPU-Bound Operations

If your application is CPU-intensive (computation, not waiting), async won't help:

- **Image processing**: Using Pillow for transformations
- **Machine learning**: Running model inference
- **Data crunching**: Large calculations
- **File compression**: ZIP, tar operations

```python
# BAD: CPU-bound work in async - blocks the event loop
@app.route("/process-image")
async def process_image():
    # This blocks everything while CPU works!
    image = process_with_pillow(image_data)
    return send_file(image)

# BETTER: Use a background task (Celery) for CPU-bound work
@app.route("/process-image")
def process_image():
    task = process_image_task.delay(image_id)  # Queue it
    return jsonify({"task_id": task.id})
```

#### 2. Simple CRUD Applications

If your app is mostly simple database reads and writes:

- **Blog platforms**: Standard article CRUD
- **Basic admin panels**: Form processing
- **Simple REST APIs**: Straightforward database operations

```python
# Simple CRUD - sync is fine and easier to debug
@app.route("/users/<int:user_id>")
def get_user(user_id):
    user = User.query.get(user_id)  # Quick DB query
    return jsonify({"name": user.name})
```

#### 3. When You're New to Async

Async has a steep learning curve and introduces new categories of bugs:

- Race conditions with shared state
- Forgetting to await (subtle bugs)
- Deadlocks when awaiting locks incorrectly
- Debugging is harder

> **💡 Tip:** For beginners, start with sync Flask and add async only when you have a clear need.

### The Hybrid Approach

Many real-world applications use both sync and async:

```python
# app.py
from flask import Flask
import asyncio

app = Flask(__name__)

# Sync route - simple CRUD
@app.route("/users")
def list_users():
    users = User.query.all()
    return jsonify([u.to_dict() for u in users])

# Async route - external API calls
@app.route("/dashboard")
async def dashboard():
    # These can be slow, so async helps
    stats = await fetch_analytics_stats()
    notifications = await fetch_notifications()
    return jsonify({"stats": stats, "notifications": notifications})
```

### Decision Framework

Use this flowchart to decide:

```
Is your route doing heavy computation?
├── YES → Use background tasks (Celery)
└── NO
    │
    Does your route wait for external services?
    ├── YES
    │   │
    │   Is high concurrency required?
    │   ├── YES → Use async Flask
    │   └── NO → Could go either way (test both)
    │
    └── NO (mostly CPU/DB work)
        │
        Is it simple CRUD?
        ├── YES → Use sync Flask
        └── NO → Profile first, then decide
```

### Performance Comparison

Here's a rough comparison of throughput (requests/second) for different approaches:

| Approach | Concurrency | Use Case |
|----------|-------------|----------|
| Sync Flask + Gunicorn (4 workers) | ~200-500 | Simple CRUD |
| Async Flask (single worker) | ~1,000-5,000 | I/O-heavy APIs |
| Async Flask + Gunicorn | ~5,000-20,000 | High concurrency |
| Sync + Celery workers | Depends on workers | CPU-heavy tasks |

> **⚡ Performance Note:** These numbers are approximate and depend heavily on your specific workload, hardware, and configuration.

## Code Walkthrough

Let's compare sync vs async vs Celery for different scenarios:

### Scenario 1: External API Calls

```python
# ============================================
# SYNC VERSION - Sequential requests
# ============================================
import requests
from flask import Flask, jsonify

app = Flask(__name__)

@app.route("/api/users/<int:user_id>/profile")
def get_user_profile(user_id):
    # These are called one after another - slow!
    user_response = requests.get(f"https://jsonplaceholder.typicode.com/users/{user_id}")
    posts_response = requests.get(f"https://jsonplaceholder.typicode.com/users/{user_id}/posts")
    albums_response = requests.get(f"https://jsonplaceholder.typicode.com/users/{user_id}/albums")
    
    return jsonify({
        "user": user_response.json(),
        "posts_count": len(posts_response.json()),
        "albums_count": len(albums_response.json())
    })

# If each request takes 500ms, total time = 1500ms
```

```python
# ============================================
# ASYNC VERSION - Concurrent requests
# ============================================
import httpx
from flask import Flask, jsonify

app = Flask(__name__)

@app.route("/api/users/<int:user_id>/profile")
async def get_user_profile(user_id):
    async with httpx.AsyncClient() as client:
        # All three requests run concurrently!
        user_task = client.get(f"https://jsonplaceholder.typicode.com/users/{user_id}")
        posts_task = client.get(f"https://jsonplaceholder.typicode.com/users/{user_id}/posts")
        albums_task = client.get(f"https://jsonplaceholder.typicode.com/users/{user_id}/albums")
        
        # Wait for all to complete
        user_response, posts_response, albums_response = await asyncio.gather(
            user_task, posts_task, albums_task
        )
    
    return jsonify({
        "user": user_response.json(),
        "posts_count": len(posts_response.json()),
        "albums_count": len(albums_response.json())
    })

# If each request takes 500ms, total time = ~500ms (they run in parallel)
```

### Scenario 2: CPU-Heavy Processing

```python
# ============================================
# SYNC VERSION - Blocks request
# ============================================
from PIL import Image
from flask import Flask, send_file
import io

app = Flask(__name__)

@app.route("/resize/<image_id>")
def resize_image(image_id):
    # Load and process - takes several seconds
    img = Image.open(f"images/{image_id}.jpg")
    img = img.resize((800, 600))  # CPU-bound work
    
    buffer = io.BytesIO()
    img.save(buffer, "JPEG")
    buffer.seek(0)
    
    return send_file(buffer, mimetype="image/jpeg")

# Problem: All requests wait while this processes!
```

```python
# ============================================
# CELERY VERSION - Background processing
# ============================================
from celery import Celery
from PIL import Image
from flask import Flask, jsonify, send_file

app = Flask(__name__)
celery = Celery("app", broker="redis://localhost:6379/0")

@celery.task
def resize_image_task(image_path, output_size=(800, 600)):
    """Background task - runs in separate process"""
    img = Image.open(image_path)
    img = img.resize(output_size)
    
    output_path = image_path.replace(".jpg", "_resized.jpg")
    img.save(output_path)
    return output_path

@app.route("/resize/<image_id>")
def resize_image(image_id):
    # Queue the task - returns immediately
    task = resize_image_task.delay(f"images/{image_id}.jpg")
    
    return jsonify({
        "task_id": task.id,
        "status": "processing"
    })

@app.route("/resize-status/<task_id>")
def check_status(task_id):
    task = resize_image_task.AsyncResult(task_id)
    return jsonify({
        "status": task.state,
        "result": task.result if task.ready() else None
    })
```

### Line-by-Line Breakdown

- `async with httpx.AsyncClient() as client:` - Creates async HTTP client (auto-closes)
- `user_task = client.get(...)` - Creates request without waiting
- `await asyncio.gather(*tasks)` - Waits for all concurrent requests
- `@celery.task` - Decorator that makes function a background task
- `.delay()` - Queue task for background execution (non-blocking)

## Common Mistakes

### ❌ Using async for everything

```python
# UNNECESSARY: Async for simple DB query
@app.route("/user/<int:user_id>")
async def get_user(user_id):
    user = await db.query(User, user_id)  # Overcomplicated!
    return jsonify({"name": user.name})
```

### ✅ Use sync for simple operations

```python
# SIMPLE: Sync for quick DB query
@app.route("/user/<int:user_id>")
def get_user(user_id):
    user = User.query.get(user_id)  # Much simpler!
    return jsonify({"name": user.name})
```

### ❌ Mixing sync DB drivers in async code

```python
# PROBLEM: Using sync SQLAlchemy in async route
@app.route("/users")
async def get_users():
    users = User.query.all()  # May cause issues in async context!
    return jsonify([u.name for u in users])
```

### ✅ Use async database drivers

```python
# CORRECT: Using async SQLAlchemy
from sqlalchemy.ext.asyncio import AsyncSession

@app.route("/users")
async def get_users():
    async with async_sessionmaker(engine) as session:
        result = await session.execute(select(User))
        users = result.scalars().all()
    return jsonify([u.name for u in users])
```

## Quick Reference

| Scenario | Recommended Approach |
|----------|----------------------|
| Simple CRUD, fast DB queries | Sync Flask |
| External API calls, multiple I/O | Async Flask |
| High concurrency (1000+ connections) | Async Flask |
| CPU-heavy (image processing, ML) | Celery background tasks |
| Long-running jobs (reports, exports) | Celery background tasks |
| Mixed workloads | Hybrid (sync routes + async routes + Celery) |

## Next Steps

Continue to [02_async_views/01_installing_flask_async.md](../02_async_views/01_installing_flask_async.md) to learn how to set up Flask with async support.