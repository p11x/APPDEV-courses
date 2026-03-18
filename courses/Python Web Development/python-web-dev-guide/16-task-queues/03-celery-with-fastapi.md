# Celery with FastAPI

## What You'll Learn
- Integrating Celery with FastAPI
- Async task handling
- Background task API in FastAPI

## Prerequisites
- Completed FastAPI folder and task queue basics

## Using FastAPI Background Tasks

FastAPI has built-in background task support:

```python
from fastapi import FastAPI, BackgroundTasks

app = FastAPI()

def send_email_notification(email: str, message: str) -> None:
    """Function to send email"""
    import time
    time.sleep(2)  # Simulate email sending
    print(f"Email sent to {email}: {message}")

@app.post("/notify/")
async def notify_user(email: str, message: str, background_tasks: BackgroundTasks):
    """Send notification using background tasks"""
    background_tasks.add_task(send_email_notification, email, message)
    return {"message": "Notification queued"}
```

🔍 **Line-by-Line Breakdown:**
1. `BackgroundTasks` — FastAPI's built-in background task manager
2. `background_tasks.add_task()` — Add function to background execution
3. The response returns immediately while email sends in background

## Celery with FastAPI

```bash
pip install celery
```

```python
from fastapi import FastAPI, BackgroundTasks
from celery import Celery
from celery.result import AsyncResult

app = FastAPI()

# Configure Celery
celery_app = Celery(
    'worker',
    broker='redis://localhost:6379/0',
    backend='redis://localhost:6379/0'
)

@celery_app.task
def process_image(image_id: int, operations: list) -> dict:
    """Process image in background"""
    import time
    time.sleep(5)  # Simulate processing
    
    return {
        'image_id': image_id,
        'operations': operations,
        'status': 'completed',
        'output_url': f'/processed/{image_id}.jpg'
    }

@app.post("/images/{image_id}/process")
async def process_image_endpoint(image_id: int, operations: list):
    """Queue image processing task"""
    task = process_image.delay(image_id, operations)
    return {"task_id": task.id, "status": "queued"}

@app.get("/tasks/{task_id}")
async def get_task_status(task_id: str):
    """Get task status"""
    result = AsyncResult(task_id, app=celery_app)
    return {
        "task_id": task_id,
        "status": result.state,
        "result": result.result if result.ready() else None
    }
```

## Async Celery Tasks

```python
import asyncio
from fastapi import FastAPI
from celery import Celery

app = FastAPI()
celery = Celery('tasks', broker='redis://localhost:6379/0')

@celery.task
async def fetch_external_data(api_url: str) -> dict:
    """Async task to fetch external API data"""
    # Use asyncio for HTTP requests
    async with asyncio.timeout(10):
        import aiohttp
        async with aiohttp.ClientSession() as session:
            async with session.get(api_url) as response:
                data = await response.json()
                return {'url': api_url, 'data': data}

@app.post("/fetch-data")
async def fetch_data(api_url: str):
    """Queue async data fetch"""
    task = fetch_external_data.delay(api_url)
    return {"task_id": task.id}

@app.get("/fetch-data/{task_id}")
async def check_fetch_status(task_id: str):
    """Check fetch task status"""
    from celery.result import AsyncResult
    result = AsyncResult(task_id)
    return {"status": result.state, "result": result.result}
```

## Task Chains and Groups

```python
from celery import chain, group

@celery.task
def step_one(data: str) -> dict:
    """First processing step"""
    return {'step': 1, 'data': data}

@celery.task
def step_two(data: dict) -> dict:
    """Second processing step"""
    return {'step': 2, 'data': data['data']}

@celery.task
def step_three(data: dict) -> dict:
    """Third processing step"""
    return {'step': 3, 'data': data['data']}

@app.post("/process-chain")
async def process_chain(data: str):
    """Run tasks in sequence (chain)"""
    # Execute step_one -> step_two -> step_three
    result = chain(step_one.s(data), step_two.s(), step_three.s()).apply_async()
    return {"task_id": result.id}

@app.post("/process-parallel")
async def process_parallel(items: list):
    """Run tasks in parallel (group)"""
    # Execute all tasks simultaneously
    result = group(step_one.s(item) for item in items).apply_async()
    return {"task_id": result.id}
```

## Summary
- Use FastAPI BackgroundTasks for simple async operations
- Use Celery for complex, persistent task queues
- Use chains for sequential tasks
- Use groups for parallel execution

## Next Steps
→ Continue to `04-rq-redis-queue.md`
