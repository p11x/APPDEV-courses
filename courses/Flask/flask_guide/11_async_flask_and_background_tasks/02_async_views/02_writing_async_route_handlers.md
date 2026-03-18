<!-- FILE: 11_async_flask_and_background_tasks/02_async_views/02_writing_async_route_handlers.md -->

## Overview

This file covers advanced patterns for writing async route handlers in Flask. You'll learn how to properly handle errors in async routes, use context variables, work with Flask's request object asynchronously, and implement proper timeout and cancellation handling.

## Prerequisites

- Flask 3.x with async support installed
- Basic understanding of async/await
- Familiarity with Flask routes and request handling

## Core Concepts

### Error Handling in Async Routes

Error handling in async Flask works similarly to sync Flask, but there are some important differences:

```python
@app.route("/async-error")
async def async_error_route():
    try:
        result = await risky_operation()
        return jsonify({"result": result})
    except ValueError as e:
        return jsonify({"error": str(e)}), 400
    except Exception as e:
        # Always catch exceptions - unhandled async exceptions crash the server!
        return jsonify({"error": "Internal error"}), 500
```

### Async Context Variables

Flask uses "context locals" for request globals (like `request`, `g`, `session`). In async routes, these work slightly differently:

```python
from flask import request, g
import asyncio

@app.route("/async-context")
async def async_context_route():
    # Accessing request works as expected
    user_agent = request.headers.get("User-Agent")
    
    # Setting g values works
    g.start_time = asyncio.get_event_loop().time()
    
    # You can await here
    await asyncio.sleep(0.1)
    
    # And access g later
    elapsed = asyncio.get_event_loop().time() - g.start_time
    
    return jsonify({"user_agent": user_agent, "elapsed": elapsed})
```

### Timeouts and Cancellation

Long-running async operations can cause problems. Use `asyncio.timeout()` (Python 3.11+) or `asyncio.wait_for()` to add timeouts:

```python
import asyncio

@app.route("/timeout-example")
async def timeout_route():
    try:
        # Cancel operation after 5 seconds
        result = await asyncio.wait_for(slow_operation(), timeout=5.0)
        return jsonify({"result": result})
    except asyncio.TimeoutError:
        return jsonify({"error": "Operation timed out"}), 504
```

### Background Tasks in Routes

For operations that should continue after the response is sent, use `asyncio.create_task()`:

```python
import asyncio
from flask import Flask, jsonify

app = Flask(__name__)

async def send_notification(user_id, message):
    """Background task - runs after response is sent"""
    await asyncio.sleep(2)  # Simulate sending notification
    print(f"Sent notification to user {user_id}: {message}")

@app.route("/start-task/<int:user_id>")
async def start_task(user_id):
    """
    Start a background task but return immediately
    
    The task will run in the background after response is sent
    """
    message = "Your report is ready!"
    
    # Schedule the task but don't wait for it
    asyncio.create_task(send_notification(user_id, message))
    
    # Return immediately while task runs in background
    return jsonify({
        "message": "Task started",
        "task": "notification"
    })
```

> **⚠️ Warning:** Background tasks started with `create_task()` may be cancelled if the server shuts down. For production, use a proper task queue like Celery.

## Code Walkthrough

Let's build a practical example that demonstrates multiple async patterns:

```python
# app_async_patterns.py
import asyncio
import random
import time
from flask import Flask, jsonify, request, abort

app = Flask(__name__)

# ============================================
# Helper functions to simulate external services
# ============================================

async def fetch_from_database(query: str) -> dict:
    """Simulates an async database query"""
    await asyncio.sleep(random.uniform(0.1, 0.5))
    return {"query": query, "rows": [{"id": 1}, {"id": 2}]}

async def call_external_api(url: str) -> dict:
    """Simulates calling an external API"""
    await asyncio.sleep(random.uniform(0.2, 0.8))
    return {"url": url, "status": "success", "data": [1, 2, 3]}

async def process_data(data: list) -> dict:
    """Simulates CPU-bound processing"""
    await asyncio.sleep(0.3)  # Using await + sleep to simulate work
    return {"processed": len(data), "result": sum(data)}

# ============================================
# Pattern 1: Basic async route
# ============================================

@app.route("/api/simple")
async def simple_async():
    """The simplest async route - just await something"""
    result = await fetch_from_database("SELECT * FROM users")
    return jsonify(result)

# ============================================
# Pattern 2: Multiple concurrent operations
# ============================================

@app.route("/api/dashboard")
async def dashboard():
    """
    Fetch multiple things concurrently for faster response
    
    Time comparison:
    - Sequential: 0.5 + 0.8 + 0.3 = 1.6 seconds
    - Concurrent: max(0.5, 0.8, 0.3) = 0.8 seconds
    """
    # Start all operations concurrently
    db_task = fetch_from_database("SELECT * FROM stats")
    api_task = call_external_api("https://api.example.com/metrics")
    process_task = process_data([1, 2, 3, 4, 5])
    
    # Wait for all to complete
    db_result, api_result, process_result = await asyncio.gather(
        db_task, api_task, process_task
    )
    
    return jsonify({
        "database": db_result,
        "external_api": api_result,
        "processed": process_result
    })

# ============================================
# Pattern 3: Error handling
# ============================================

async def risky_operation(should_fail: bool = False) -> dict:
    """An operation that might fail"""
    await asyncio.sleep(0.5)
    if should_fail:
        raise ValueError("Something went wrong!")
    return {"status": "success"}

@app.route("/api/with-error-handling")
async def error_handling_route():
    """Demonstrates proper error handling in async routes"""
    should_fail = request.args.get("fail", "false").lower() == "true"
    
    try:
        result = await risky_operation(should_fail)
        return jsonify(result)
    except ValueError as e:
        # Return a 400 error with the error message
        return jsonify({"error": str(e)}), 400
    except Exception as e:
        # Catch-all for unexpected errors
        return jsonify({"error": "Internal server error"}), 500

# ============================================
# Pattern 4: Timeouts
# ============================================

async def slow_service_call() -> dict:
    """A slow service that might take too long"""
    await asyncio.sleep(10)  # Simulates a 10-second operation
    return {"status": "done"}

@app.route("/api/with-timeout")
async def timeout_route():
    """Demonstrates timeout handling"""
    try:
        # Cancel if it takes more than 2 seconds
        result = await asyncio.wait_for(slow_service_call(), timeout=2.0)
        return jsonify(result)
    except asyncio.TimeoutError:
        # Return 504 Gateway Timeout
        return jsonify({"error": "Service took too long"}), 504

# ============================================
# Pattern 5: Request validation before async
# ============================================

async def get_user_data(user_id: int) -> dict:
    """Get user data from somewhere"""
    await asyncio.sleep(0.3)
    return {"id": user_id, "name": f"User {user_id}"}

@app.route("/api/users/<int:user_id>")
async def get_user(user_id):
    """
    Validate BEFORE starting async operations
    
    This is more efficient than catching errors later
    """
    # Validate immediately (sync)
    if user_id <= 0:
        abort(400, description="Invalid user ID")
    
    if user_id > 1000:
        abort(404, description="User not found")
    
    # Now do async work
    user_data = await get_user_data(user_id)
    return jsonify(user_data)

# ============================================
# Pattern 6: Streaming responses
# ============================================

async def generate_events():
    """Generate server-sent events"""
    for i in range(5):
        await asyncio.sleep(1)  # Wait 1 second between events
        yield f"data: Event {i}\n\n"

@app.route("/api/stream")
async def stream_events():
    """Stream events to the client"""
    from flask import Response
    return Response(generate_events(), mimetype="text/event-stream")

# ============================================
# Pattern 7: Background task (for simple cases)
# ============================================

async def log_request(request_id: str, path: str):
    """Background logging - doesn't block response"""
    await asyncio.sleep(2)  # Simulate slow logging
    print(f"Logged request {request_id} to {path}")

@app.route("/api/with-background")
async def with_background():
    """Start background task but return immediately"""
    import uuid
    request_id = str(uuid.uuid4())
    
    # Fire and forget - task runs after response sent
    asyncio.create_task(log_request(request_id, request.path))
    
    return jsonify({
        "request_id": request_id,
        "status": "accepted"
    })

# ============================================
# Pattern 8: Mixed sync and async
# ============================================

def sync_calculation(x: int) -> int:
    """A synchronous calculation"""
    return x * 2

async def async_fetch(url: str) -> dict:
    """An async API call"""
    await asyncio.sleep(0.5)
    return {"url": url, "data": "example"}

@app.route("/api/mixed")
async def mixed_route():
    """
    Mix sync and async in the same route
    
    For simple CPU work, just call the sync function
    For I/O, use async functions
    """
    # Sync calculation - no await needed
    calculated = sync_calculation(42)
    
    # Async I/O - must await
    fetched = await async_fetch("https://api.example.com")
    
    return jsonify({
        "calculated": calculated,
        "fetched": fetched
    })

if __name__ == "__main__":
    app.run(debug=True, port=5000)
```

### Line-by-Line Breakdown

- `await asyncio.gather(db_task, api_task, process_task)` - Runs three async operations concurrently
- `asyncio.wait_for(operation, timeout=2.0)` - Cancels operation if it takes longer than 2 seconds
- `asyncio.create_task(function())` - Schedules a task to run in background without waiting
- `async def mixed_route()` + `sync_calculation(42)` - Calls sync function directly (no await needed)
- `yield f"data: ...\n\n"` - Yields data for streaming response

### Testing the Routes

```bash
# Start the app
python app_async_patterns.py

# Test basic async
curl http://localhost:5000/api/simple

# Test concurrent operations (should be faster than sequential)
curl http://localhost:5000/api/dashboard

# Test error handling - success case
curl http://localhost:5000/api/with-error-handling

# Test error handling - failure case
curl http://localhost:5000/api/with-error-handling?fail=true

# Test timeout (will return error after 2 seconds)
curl http://localhost:5000/api/with-timeout

# Test streaming
curl http://localhost:5000/api/stream

# Test mixed sync/async
curl http://localhost:5000/api/mixed
```

## Common Mistakes

### ❌ Not handling exceptions

```python
# WRONG: Unhandled exception crashes the async handler
@app.route("/broken")
async def broken():
    await risky_operation()  # If this raises, server may crash
    return "ok"
```

### ✅ Always handle exceptions

```python
# CORRECT: Handle exceptions properly
@app.route("/fixed")
async def fixed():
    try:
        await risky_operation()
        return "ok"
    except ValueError as e:
        return jsonify({"error": str(e)}), 400
```

### ❌ Using sync libraries

```python
# WRONG: requests is synchronous, blocks event loop
@app.route("/slow")
async def slow_route():
    response = requests.get("https://api.example.com")  # Blocks!
    return jsonify(response.json())
```

### ✅ Use async libraries

```python
# CORRECT: httpx is async
import httpx

@app.route("/fast")
async def fast_route():
    async with httpx.AsyncClient() as client:
        response = await client.get("https://api.example.com")
    return jsonify(response.json())
```

### ❌ Forgetting to await

```python
# WRONG: Forgetting await
@app.route("/forgot")
async def forgot():
    result = some_async_function()  # No await!
    return jsonify(result)  # Returns coroutine object, not data
```

### ✅ Always await

```python
# CORRECT: Always await async functions
@app.route("/correct")
async def correct():
    result = await some_async_function()  # Properly awaited
    return jsonify(result)  # Returns actual data
```

## Quick Reference

| Pattern | Code | When to Use |
|---------|------|-------------|
| Concurrent operations | `await asyncio.gather(task1, task2)` | Multiple I/O operations |
| Timeout | `await asyncio.wait_for(coro, timeout=5)` | External service calls |
| Error handling | `try: ... except: ...` | Any operation that might fail |
| Background task | `asyncio.create_task(task())` | Simple non-critical tasks |
| Streaming | `yield f"data: {msg}\n\n"` | Server-Sent Events |
| Mixed sync/async | Call sync directly, await async | Simple calculations + I/O |

## Next Steps

Continue to [03_async_database_queries.md](03_async_database_queries.md) to learn how to use async database operations with SQLAlchemy in Flask.