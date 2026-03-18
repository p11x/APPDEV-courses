# Async Background Tasks

## What You'll Learn
- Running background tasks in async apps

## Prerequisites
- Completed Starlette framework

## Background Tasks

```python
from starlette.background import BackgroundTasks

async def send_email(email: str, message: str):
    # Send email logic
    print(f"Sending to {email}: {message}")

async def homepage(request):
    tasks = BackgroundTasks()
    tasks.add_task(send_email, "user@example.com", "Hello!")
    return JSONResponse({"message": "Done"}, background=tasks)
```

## Summary
- Background tasks run after response is sent
- Useful for email, notifications, processing
