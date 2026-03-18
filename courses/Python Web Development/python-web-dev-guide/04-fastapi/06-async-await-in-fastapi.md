# Async/Await in FastAPI

## What You'll Learn
- What asynchronous programming means
- How async/await works in Python
- When to use async def vs def
- Making async database calls
- Running blocking code in async contexts

## Prerequisites
- Completed FastAPI Authentication

## What Is Asynchronous Programming?

Imagine a restaurant. A **synchronous** waiter takes one order, walks to the kitchen, stands there waiting for the food, brings it back, then takes the next order. Every other customer waits the entire time.

An **asynchronous** waiter takes an order, submits it to the kitchen, and while the food is being prepared, takes orders from other tables. When the food is ready, they deliver it.

FastAPI is the async waiter. While one request is waiting for a database, FastAPI handles other incoming requests.

## The async and await Keywords

```python
import asyncio

async def fetch_user_data(user_id: int) -> dict:
    """Simulate an async function."""
    await asyncio.sleep(1)  # Simulates a slow database or API call
    return {"user_id": user_id, "name": "Alice"}

# Running the async function
async def main():
    result = await fetch_user_data(1)
    print(result)

asyncio.run(main())
```

🔍 **Line-by-Line Breakdown:**

1. `async def fetch_user_data(...)` — The `async` keyword transforms this into a **coroutine** — a special function that can pause and resume
2. `await asyncio.sleep(1)` — `await` means "pause this function here and let other code run while we wait." This simulates a 1-second delay
3. Without `await`, Python would block everything for 1 second. With `await`, FastAPI handles other requests during that second

## async def vs def in FastAPI

FastAPI supports both:

```python
from fastapi import FastAPI
import asyncio

app = FastAPI()

# ASYNC route - use when awaiting other async operations
@app.get("/async/users")
async def get_users_async() -> list[dict]:
    # This is async - FastAPI won't block other requests
    await asyncio.sleep(0.5)  # Simulate async I/O
    return [{"id": 1, "name": "Alice"}]

# REGULAR route - use for simple synchronous operations
@app.get("/sync/users")
def get_users_sync() -> list[dict]:
    # This is synchronous - blocks the thread
    # Good for CPU-bound tasks
    return [{"id": 1, "name": "Alice"}]
```

## When to Use async def

Use `async def` when your code:
- Calls async libraries (databases, HTTP clients)
- Waits for I/O (files, network)

Use regular `def` when your code:
- Does CPU-bound work (calculations, data processing)
- Uses synchronous libraries

## Async Database Calls

```python
from fastapi import FastAPI, Depends
import asyncio

app = FastAPI()

# Simulated async database
class AsyncDatabase:
    async def fetch_users(self) -> list[dict]:
        await asyncio.sleep(0.1)  # Simulate DB query
        return [
            {"id": 1, "name": "Alice"},
            {"id": 2, "name": "Bob"}
        ]
    
    async def fetch_user(self, user_id: int) -> dict | None:
        await asyncio.sleep(0.1)
        users = await self.fetch_users()
        return next((u for u in users if u["id"] == user_id), None)

# Dependency
async def get_db() -> AsyncDatabase:
    return AsyncDatabase()

# Async route
@app.get("/users")
async def get_users(db: AsyncDatabase = Depends(get_db)) -> list[dict]:
    users = await db.fetch_users()
    return users

@app.get("/users/{user_id}")
async def get_user(user_id: int, db: AsyncDatabase = Depends(get_db)) -> dict:
    user = await db.fetch_user(user_id)
    if user is None:
        return {"error": "User not found"}
    return user
```

## Using async with httpx

`httpx` is an async HTTP client:

```python
import httpx
from fastapi import FastAPI, HTTPException

app = FastAPI()

async def fetch_external_api(url: str) -> dict:
    """Fetch data from external API asynchronously."""
    async with httpx.AsyncClient() as client:
        response = await client.get(url)
        response.raise_for_status()
        return response.json()

@app.get("/external-users")
async def get_external_users() -> list[dict]:
    """Fetch users from JSONPlaceholder API."""
    try:
        users = await fetch_external_api("https://jsonplaceholder.typicode.com/users")
        return users[:3]  # Return first 3
    except httpx.HTTPError as e:
        raise HTTPException(status_code=500, detail=str(e))

@app.get("/external-posts/{user_id}")
async def get_user_posts(user_id: int) -> list[dict]:
    """Fetch posts for a specific user."""
    async with httpx.AsyncClient() as client:
        response = await client.get(
            f"https://jsonplaceholder.typicode.com/posts",
            params={"userId": user_id}
        )
        return response.json()
```

## Running Blocking Code

If you have blocking (synchronous) code in an async endpoint, use `run_in_executor`:

```python
from fastapi import FastAPI
from concurrent.futures import ThreadPoolExecutor
import time

app = FastAPI()
executor = ThreadPoolExecutor()

def sync_function(n: int) -> int:
    """Synchronous function that takes time."""
    time.sleep(1)  # Blocking call
    return n * 2

@app.get("/blocking/{n}")
async def call_blocking(n: int) -> dict:
    """Call blocking function without blocking the event loop."""
    loop = asyncio.get_event_loop()
    result = await loop.run_in_executor(executor, sync_function, n)
    return {"input": n, "result": result}

# Alternative: use asyncio.to_thread (Python 3.9+)
@app.get("/blocking2/{n}")
async def call_blocking2(n: int) -> dict:
    """Simpler way to run sync code in async context."""
    result = await asyncio.to_thread(sync_function, n)
    return {"input": n, "result": result}
```

## Background Tasks

Run tasks after returning a response:

```python
from fastapi import FastAPI, BackgroundTasks

app = FastAPI()

def send_email(email: str, message: str) -> None:
    """Simulated email sending."""
    print(f"Sending email to {email}: {message}")

@app.post("/send-notification")
async def send_notification(
    background_tasks: BackgroundTasks,
    email: str,
    message: str
) -> dict:
    """Send notification asynchronously."""
    background_tasks.add_task(send_email, email, message)
    return {"message": "Notification will be sent"}
```

## Summary
- **async/await** allows handling multiple requests simultaneously
- Use `async def` when awaiting other async operations
- Use regular `def` for CPU-bound or synchronous code
- Use `asyncio.to_thread()` or `run_in_executor()` to run blocking code
- Use **BackgroundTasks** for post-response processing

## Next Steps
→ Continue to `07-building-a-rest-api.md` to apply these concepts in building a complete REST API.
