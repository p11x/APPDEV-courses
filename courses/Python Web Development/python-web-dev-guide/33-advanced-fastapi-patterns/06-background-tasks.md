# Background Tasks in FastAPI

## What You'll Learn
- Background task fundamentals
- Using `BackgroundTasks` for fire-and-forget operations
- Task dependencies and execution order
- Passing data to background tasks
- Differences between background tasks and Celery

## Prerequisites
- Completed `05-websockets.md` — WebSocket communication
- Understanding of async/await in Python 3.11+

## What Are Background Tasks?

Background tasks run after returning a response to the client. The client doesn't wait for them to complete:

```
Client Request ──▶ Process ──▶ Response (fast!) ──▶ Client happy
                           │
                           └──▶ Background Task (runs after)
```

**Use cases:**
- Sending welcome emails after signup
- Generating reports
- Processing uploaded files
- Clearing caches
- Logging/analytics

## Using BackgroundTasks

```python
from fastapi import FastAPI, BackgroundTasks
from pydantic import BaseModel
import time

app = FastAPI()

def send_welcome_email(email: str) -> None:
    """Simulated email sending function (runs in background)."""
    # In real app: send email via SMTP/API
    time.sleep(2)  # Simulate email sending
    print(f"📧 Welcome email sent to {email}")

class UserCreate(BaseModel):
    email: str
    username: str

@app.post("/users", status_code=201)
async def create_user(
    user: UserCreate,
    background_tasks: BackgroundTasks
):
    """Create user and send welcome email in background."""
    # Save user to database (synchronous for simplicity)
    new_user = {"id": 123, "email": user.email, "username": user.username}
    
    # Add background task - runs AFTER response is sent
    background_tasks.add_task(send_welcome_email, user.email)
    
    return {
        "message": "User created successfully!",
        "user": new_user
    }
```

🔍 **Line-by-Line Breakdown:**
1. `background_tasks: BackgroundTasks` — FastAPI injects BackgroundTasks object
2. `background_tasks.add_task(...)` — Schedules function to run after response
3. Function runs after HTTP response is already sent to client

## Multiple Background Tasks with Order

Tasks run in the order added:

```python
from fastapi import FastAPI, BackgroundTasks

def task_1():
    print("1. First task runs")

def task_2():
    print("2. Second task runs")

def task_3():
    print("3. Third task runs")

@app.post("/process")
async def process_data(background_tasks: BackgroundTasks):
    # Tasks execute in this exact order
    background_tasks.add_task(task_1)
    background_tasks.add_task(task_2)
    background_tasks.add_task(task_3)
    
    return {"message": "Processing started"}
```

## Background Tasks with Parameters

```python
from fastapi import FastAPI, BackgroundTasks
from pydantic import BaseModel

def process_upload(
    file_id: str,
    user_id: int,
    operation: str
) -> None:
    """Background task that takes parameters."""
    print(f"Processing file {file_id} for user {user_id}")
    
    if operation == "resize":
        print("  Resizing image...")
    elif operation == "compress":
        print("  Compressing file...")
    
    print("  Done!")

class UploadRequest(BaseModel):
    file_id: str
    user_id: int
    operation: str = "resize"

@app.post("/upload")
async def upload_file(
    request: UploadRequest,
    background_tasks: BackgroundTasks
):
    """Start file processing in background."""
    # add_task passes arguments to the function
    background_tasks.add_task(
        process_upload,
        request.file_id,
        request.user_id,
        request.operation
    )
    
    return {"message": "Upload accepted, processing in background"}

# Example: Using kwargs
background_tasks.add_task(
    process_upload,
    file_id="abc123",
    user_id=42,
    operation="compress"
)
```

## Calling Async Functions in Background

```python
from fastapi import FastAPI, BackgroundTasks
import asyncio

async def async_email_sender(email: str) -> None:
    """Async function for sending emails."""
    # Simulate async API call
    await asyncio.sleep(2)
    print(f"✅ Email sent to {email}")

async def async_database_cleanup() -> None:
    """Async function for database operations."""
    await asyncio.sleep(1)
    print("🧹 Database cleanup complete")

@app.post("/register")
async def register(
    email: str,
    background_tasks: BackgroundTasks
):
    """Register with async background tasks."""
    # Can add both sync and async functions
    background_tasks.add_task(async_email_sender, email)
    background_tasks.add_task(async_database_cleanup)
    
    return {"message": "Registered!"}
```

## Background Tasks vs Celery

| Feature | BackgroundTasks | Celery |
|---------|-----------------|--------|
| **Setup** | Built-in | Requires Redis/RabbitMQ |
| **Persistence** | Lost on restart | Survives restarts |
| **Queue** | Single queue | Multiple queues |
| **Scheduling** | No | Yes (celery beat) |
| **Scalability** | Single process | Multi-machine |
| **Use case** | Simple, low-volume | Production, high-volume |

## Best Practice: Return Task ID

```python
from fastapi import FastAPI, BackgroundTasks
from datetime import datetime
from uuid import uuid4
import asyncio

# Store task statuses (use Redis in production)
tasks: dict[str, dict] = {}

def long_task(task_id: str) -> None:
    """Simulated long-running task."""
    tasks[task_id]["status"] = "running"
    
    import time
    time.sleep(5)  # Simulate work
    
    tasks[task_id]["status"] = "completed"
    tasks[task_id]["completed_at"] = datetime.utcnow().isoformat()

@app.post("/start-task")
async def start_task(
    name: str,
    background_tasks: BackgroundTasks
):
    """Start background task and return task ID."""
    task_id = str(uuid4())
    
    tasks[task_id] = {
        "name": name,
        "status": "pending",
        "created_at": datetime.utcnow().isoformat()
    }
    
    background_tasks.add_task(long_task, task_id)
    
    return {
        "task_id": task_id,
        "status": "pending",
        "check_url": f"/tasks/{task_id}"
    }

@app.get("/tasks/{task_id}")
async def get_task_status(task_id: str):
    """Check task status."""
    if task_id not in tasks:
        return {"error": "Task not found"}
    
    return tasks[task_id]
```

## Production Considerations

- **Task failures**: Background tasks don't return errors to client — implement logging and alerting
- **Timeouts**: Long-running tasks may timeout if your proxy (nginx) has timeout settings
- **Idempotency**: Design tasks to be idempotent (safe to run twice)
- **Scaling**: For high-volume, use Celery or similar task queue

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Raising exceptions in background tasks

**Wrong:**
```python
def broken_task():
    raise ValueError("This will crash silently!")
```

**Why it fails:** Exception is swallowed, no error returned to client.

**Fix:**
```python
def safe_task():
    try:
        # Task logic
        pass
    except Exception as e:
        print(f"Task failed: {e}")  # Log the error
        # Or send to error tracking
```

### ❌ Mistake 2: Using slow sync functions

**Wrong:**
```python
def slow_sync_task():
    time.sleep(10)  # Blocks event loop!
```

**Why it fails:** Blocks handling of other requests.

**Fix:**
```python
async def slow_async_task():
    await asyncio.sleep(10)  # Non-blocking
```

### ❌ Mistake 3: Forgetting task is fire-and-forget

**Wrong:**
```python
@app.post("/checkout")
async def checkout(background_tasks: BackgroundTasks):
    # Returns immediately!
    background_tasks.add_task(process_payment, payment_id)
    return {"status": "processing"}
```

**Why it fails:** Client might think payment succeeded when it hasn't.

**Fix:**
```python
@app.post("/checkout")
async def checkout(background_tasks: BackgroundTasks):
    # Start processing
    background_tasks.add_task(process_payment, payment_id)
    
    return {
        "status": "processing",
        "task_id": payment_id,
        "webhook": f"/webhooks/payment/{payment_id}"
    }
```

## Summary

- BackgroundTasks runs functions after HTTP response is sent
- Add tasks with `background_tasks.add_task(func, *args)`
- Tasks run in order they were added
- Good for simple, low-volume background work
- For production systems, consider Celery with Redis/RabbitMQ

## Next Steps

→ Continue to `07-advanced-dependency-injection.md` to learn advanced dependency patterns in FastAPI.
