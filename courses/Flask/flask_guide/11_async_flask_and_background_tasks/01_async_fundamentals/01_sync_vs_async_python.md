<!-- FILE: 11_async_flask_and_background_tasks/01_async_fundamentals/01_sync_vs_async_python.md -->

## Overview

Understanding the difference between synchronous and asynchronous programming is essential for building modern Flask applications that can handle many concurrent connections efficiently. This file explains what synchronous (sync) and asynchronous (async) code mean, why they matter for web development, and how Python handles each approach.

## Prerequisites

- Basic Python knowledge (functions, loops, variables)
- Understanding of how web servers handle requests
- Familiarity with the Flask request-response cycle

## Core Concepts

### What is Synchronous Code?

Synchronous code executes line by line, one operation at a time. Each line must complete before the next line begins. When a synchronous function makes a blocking I/O operation (like reading a file, making a network request, or querying a database), the entire program waits—doing nothing—until that operation finishes.

```python
# Synchronous example: fetching data from three APIs one after another
import requests
import time

def fetch_all_data():
    start = time.time()
    
    # Each request blocks until it completes
    result1 = requests.get("https://api.example.com/users")
    result2 = requests.get("https://api.example.com/posts")
    result3 = requests.get("https://api.example.com/comments")
    
    elapsed = time.time() - start
    print(f"Total time: {elapsed:.2f} seconds")
    
    return [result1.json(), result2.json(), result3.json()]
```

If each request takes 1 second, this function takes at least 3 seconds total.

### What is Asynchronous Code?

Asynchronous code allows the program to continue executing while waiting for I/O operations to complete. Instead of blocking, the program can start multiple operations and switch between them as they complete. This is often called "non-blocking" I/O.

Python's `asyncio` module provides tools for writing asynchronous code using the `async` and `await` keywords:

```python
# Asynchronous example: fetching data from three APIs concurrently
import aiohttp
import asyncio
import time

async def fetch_all_data():
    start = time.time()
    
    async with aiohttp.ClientSession() as session:
        # All three requests start "at the same time"
        tasks = [
            session.get("https://api.example.com/users"),
            session.get("https://api.example.com/posts"),
            session.get("https://api.example.com/comments")
        ]
        
        # Wait for all of them to complete
        responses = await asyncio.gather(*tasks)
        
        # Parse all responses
        results = [await r.json() for r in responses]
    
    elapsed = time.time() - start
    print(f"Total time: {elapsed:.2f} seconds")
    
    return results
```

If each request takes 1 second, this function takes only about 1 second total—because all requests run concurrently.

### Why This Matters for Flask

Traditional Flask applications are synchronous. When a user makes a request, the server thread is blocked until the entire response is ready. This works fine for simple applications but becomes problematic when:

- Your application needs to handle thousands of concurrent connections
- Your routes perform slow I/O operations (database queries, external API calls, file processing)
- You want to implement real-time features like WebSockets

Flask 3.0+ supports async views, allowing you to write asynchronous route handlers that can handle I/O operations more efficiently.

### The Event Loop

The **event loop** is the core mechanism that makes async code work. It's a constantly running loop that:

1. Starts asynchronous tasks
2. Monitors when tasks are waiting for I/O
3. Switches to other tasks while I/O is happening
4. Resumes tasks when their I/O completes

Python's `asyncio` module provides the event loop implementation. When you use `await`, you're telling the event loop: "I'm waiting for something—feel free to run other code while I wait."

### Blocking vs Non-Blocking Libraries

Not all Python libraries work asynchronously. When using async Flask, you must use async-compatible libraries:

| Library Type | Sync (Blocking) | Async (Non-blocking) |
|--------------|-----------------|---------------------|
| HTTP Requests | `requests` | `aiohttp`, `httpx` |
| Database | `psycopg2`, `pymysql` | `asyncpg`, `aiomysql` |
| ORM | SQLAlchemy (sync) | SQLAlchemy (async), GINO |
| Redis | `redis` | `aioredis`, `redis.asyncio` |

Using a sync library inside an async function will block the event loop, defeating the purpose of async code.

## Code Walkthrough

Let's compare sync and async implementations of the same Flask route that simulates a slow database query:

### Synchronous Implementation

```python
# app_sync.py
import time
from flask import Flask, jsonify

app = Flask(__name__)

# Simulate a slow database query (1 second)
def get_user_from_db(user_id):
    time.sleep(1)  # This blocks the entire thread!
    return {"id": user_id, "name": f"User {user_id}"}

@app.route("/api/users/<int:user_id>")
def get_user(user_id):
    # This route blocks while get_user_from_db runs
    user = get_user_from_db(user_id)
    return jsonify(user)

if __name__ == "__main__":
    app.run(debug=True, port=5000)
```

**Testing the sync version:**

```bash
# Run the app
python app_sync.py

# In another terminal, test with curl (open 3 tabs and run these simultaneously)
curl http://localhost:5000/api/users/1
# Takes ~1 second

# If you make 3 requests sequentially, it takes ~3 seconds total
time curl http://localhost:5000/api/users/1 && \
time curl http://localhost:5000/api/users/2 && \
time curl http://localhost:5000/api/users/3
```

### Asynchronous Implementation

```python
# app_async.py
import asyncio
from flask import Flask, jsonify

app = Flask(__name__)

# Simulate a slow database query (1 second) - now async!
async def get_user_from_db(user_id):
    await asyncio.sleep(1)  # Non-blocking sleep - yields control to event loop
    return {"id": user_id, "name": f"User {user_id}"}

@app.route("/api/users/<int:user_id>")
async def get_user(user_id):
    # This route can handle other requests while waiting
    user = await get_user_from_db(user_id)
    return jsonify(user)

if __name__ == "__main__":
    # Flask 3.x handles async automatically
    app.run(debug=True, port=5000)
```

**Testing the async version:**

```bash
# Run the app
python app_async.py

# In another terminal, test 3 concurrent requests
# They all complete in ~1 second total (not 3 seconds!)
time curl http://localhost:5000/api/users/1 & \
time curl http://localhost:5000/api/users/2 & \
time curl http://localhost:5000/api/users/3 & \
wait
```

### Line-by-Line Breakdown

**Sync version:**
- `time.sleep(1)` - Blocks the thread for 1 second. No other code can run during this time.
- `@app.route(...)` + `def get_user(user_id)` - A regular sync function that must complete before the next request can be processed.

**Async version:**
- `async def get_user_from_db(user_id)` - Declares this as an async function. Must be called with `await`.
- `await asyncio.sleep(1)` - Non-blocking sleep. The event loop can run other code while waiting.
- `@app.route(...)` + `async def get_user(user_id)` - Flask recognizes async functions and handles them properly with Flask 3.x.

## Common Mistakes

### ❌ Mixing sync and async code incorrectly

```python
# WRONG: Using blocking time.sleep in async function
async def slow_operation():
    time.sleep(5)  # BLOCKS entire event loop!
    return "done"

# WRONG: Not awaiting async functions
async def get_data():
    result = get_user_from_db(1)  # Returns a coroutine object, not the result!
    return result  # Will return a coroutine, not the user data
```

### ✅ Correct async patterns

```python
# CORRECT: Use asyncio.sleep instead of time.sleep
async def slow_operation():
    await asyncio.sleep(5)  # Non-blocking - other code can run
    return "done"

# CORRECT: Always await async functions
async def get_data():
    result = await get_user_from_db(1)  # Actually gets the user data
    return result
```

### ❌ Using sync libraries in async code

```python
# WRONG: Using requests (sync) in async route
@app.route("/fetch")
async def fetch_data():
    response = requests.get("https://api.example.com/data")  # Blocks event loop!
    return jsonify(response.json())
```

### ✅ Using async libraries

```python
# CORRECT: Using httpx (async) in async route
@app.route("/fetch")
async def fetch_data():
    async with httpx.AsyncClient() as client:
        response = await client.get("https://api.example.com/data")
    return jsonify(response.json())
```

## Quick Reference

| Concept | Sync Version | Async Version |
|---------|--------------|---------------|
| Function definition | `def func():` | `async def func():` |
| Calling function | `result = func()` | `result = await func()` |
| Sleep | `time.sleep(1)` | `await asyncio.sleep(1)` |
| HTTP request | `requests.get(url)` | `await client.get(url)` |
| Multiple concurrent tasks | Sequential loop | `await asyncio.gather(*tasks)` |
| Flask route | `@app.route("/")` + `def view():` | `@app.route("/")` + `async def view():` |

**Key commands:**

```bash
# Install async HTTP client
pip install httpx aiohttp

# Run async function (testing)
python -c "import asyncio; asyncio.run(async_function())"
```

## Next Steps

Continue to [02_async_await_basics.md](02_async_await_basics.md) to learn the fundamentals of Python's async/await syntax and how to write your first async code.