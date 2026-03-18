<!-- FILE: 11_async_flask_and_background_tasks/02_async_views/01_installing_flask_async.md -->

## Overview

Flask 3.0+ has native support for asynchronous route handlers, meaning you can define routes with `async def` and Flask will automatically run them in an async context. This file explains what's needed to enable async Flask, which dependencies to install, and how to verify your setup is working correctly.

## Prerequisites

- Flask 3.0 or higher installed
- Python 3.10+ (recommended for best async support)
- Basic understanding of async/await concepts

## Core Concepts

### Flask 3.x Async Support

Starting with Flask 3.0 (released in 2023), Flask has built-in support for async route handlers. This means you can simply write:

```python
@app.route("/api/data")
async def get_data():
    result = await fetch_data()
    return jsonify(result)
```

Flask automatically detects async functions and handles them using Python's asyncio event loop.

### Python Version Requirements

| Python Version | Async Support | Notes |
|----------------|---------------|-------|
| Python 3.8 | Partial | Works but some features missing |
| Python 3.9+ | Full | Recommended |
| Python 3.10+ | Full | Best performance and features |

> **💡 Tip:** Python 3.11+ includes significant performance improvements for async code. Upgrade if possible.

### Required Dependencies

For async Flask to work, you need:

1. **Flask 3.x+** - The core framework with async support
2. **Python's asyncio** - Built into Python 3.4+, no extra install needed
3. **An async-compatible web server** - For production deployment
4. **Optional: async libraries** - For HTTP requests, database access, etc.

### Installing Dependencies

```bash
# Create a virtual environment (recommended)
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate

# Install Flask 3.x (this includes async support)
pip install Flask>=3.0

# Install async HTTP client (recommended for async routes)
pip install httpx

# Install aiohttp as alternative
pip install aiohttp

# For production, use hypercorn or uvicorn as ASGI server
pip install hypercorn
```

### How Flask Handles Async

When Flask receives a request to an async route:

1. Flask detects the route handler is an async function
2. Flask creates/gets an asyncio event loop
3. Flask schedules the async route handler as a coroutine
4. The event loop runs the coroutine (including all `await` calls)
5. When the coroutine completes, Flask sends the response

This all happens automatically—you don't need to call `asyncio.run()` yourself.

## Code Walkthrough

### Step 1: Verify Your Installation

First, let's check what version of Flask you have and verify async support:

```python
# check_version.py
import flask
import sys

print(f"Python version: {sys.version}")
print(f"Flask version: {flask.__version__}")
print(f"Flask >= 3.0? {flask.__version__ >= '3.0'}")

# Check if asyncio is available
import asyncio
print(f"Asyncio available: {asyncio.iscoroutinefunction}")
```

Run it:

```bash
python check_version.py
```

**Expected output:**

```
Python version: 3.11.4 (main, ...)
Flask version: 3.0.0
Flask >= 3.0? True
Asyncio available: <function iscoroutinefunction at 0x...>
```

### Step 2: Create Your First Async Flask App

```python
# app.py
from flask import Flask, jsonify
import asyncio
import time

app = Flask(__name__)

# ============================================
# Regular synchronous route (still works!)
# ============================================
@app.route("/sync")
def sync_route():
    """A regular sync route - no changes needed"""
    return jsonify({
        "type": "sync",
        "message": "This is a regular synchronous route"
    })

# ============================================
# Asynchronous route (new in Flask 3.x)
# ============================================
async def fetch_data_from_api():
    """Simulates an async I/O operation (like an API call)"""
    await asyncio.sleep(1)  # Non-blocking delay
    return {"source": "api", "data": [1, 2, 3]}

@app.route("/async")
async def async_route():
    """
    An async route - Flask automatically handles this
    
    Key points:
    - async def instead of def
    - await for I/O operations
    - Flask handles the event loop internally
    """
    result = await fetch_data_from_api()  # Non-blocking!
    return jsonify({
        "type": "async",
        "message": "This is an asynchronous route",
        "result": result
    })

# ============================================
# Route that fetches multiple things concurrently
# ============================================
async def get_user_data(user_id):
    """Simulates fetching user data"""
    await asyncio.sleep(0.5)
    return {"id": user_id, "name": f"User {user_id}"}

async def get_user_orders(user_id):
    """Simulates fetching user orders"""
    await asyncio.sleep(0.7)
    return [{"order_id": 1}, {"order_id": 2}]

async def get_user_preferences(user_id):
    """Simulates fetching user preferences"""
    await asyncio.sleep(0.3)
    return {"theme": "dark", "notifications": True}

@app.route("/user-dashboard/<int:user_id>")
async def user_dashboard(user_id):
    """
    Fetch multiple things concurrently - much faster!
    
    Without async: 0.5 + 0.7 + 0.3 = 1.5 seconds
    With async: max(0.5, 0.7, 0.3) = 0.7 seconds
    """
    user, orders, preferences = await asyncio.gather(
        get_user_data(user_id),
        get_user_orders(user_id),
        get_user_preferences(user_id)
    )
    
    return jsonify({
        "user": user,
        "orders": orders,
        "preferences": preferences
    })

if __name__ == "__main__":
    # Development server - auto-reloads on changes
    app.run(debug=True, port=5000)
```

### Step 3: Run and Test

```bash
# Run the app
python app.py

# Test sync route
curl http://localhost:5000/sync

# Test async route
curl http://localhost:5000/async

# Test concurrent fetching
curl http://localhost:5000/user-dashboard/1
```

**Expected responses:**

```json
// GET /sync
{"message": "This is a regular synchronous route", "type": "sync"}

// GET /async
{"message": "This is an asynchronous route", "result": {"data": [1, 2, 3], "source": "api"}, "type": "async"}

// GET /user-dashboard/1
{"orders": [{"order_id": 1}, {"order_id": 2}], "preferences": {"notifications": true, "theme": "dark"}, "user": {"id": 1, "name": "User 1"}}
```

### Line-by-Line Breakdown

- `async def async_route():` - Declares this as an async route handler
- `await fetch_data_from_api()` - Awaits the async function, non-blocking
- `await asyncio.gather(*tasks)` - Runs multiple async operations concurrently
- `app.run(debug=True, port=5000)` - Development server works with async routes

### Step 4: Verify Async Is Actually Working

Let's create a test to confirm async is non-blocking:

```python
# test_async.py
import asyncio
import time
from flask import Flask, jsonify

app = Flask(__name__)

async def slow_operation(name, delay):
    """Simulates a slow I/O operation"""
    print(f"{name} starting...")
    await asyncio.sleep(delay)
    print(f"{name} finished!")
    return f"{name} done"

@app.route("/test-concurrent")
async def test_concurrent():
    """Test that multiple async operations run concurrently"""
    start = time.time()
    
    # Run 3 tasks that each take 1 second
    results = await asyncio.gather(
        slow_operation("task1", 1),
        slow_operation("task2", 1),
        slow_operation("task_output", 1),
    )
    
    elapsed = time.time() - start
    
    return jsonify({
        "results": results,
        "elapsed_seconds": elapsed,
        "note": "If async works, elapsed should be ~1 second (not 3)"
    })

@app.route("/test-sequential")
async def test_sequential():
    """Test sequential execution (what NOT to do)"""
    start = time.time()
    
    # Run 3 tasks sequentially - takes 3 seconds
    result1 = await slow_operation("task1", 1)
    result2 = await slow_operation("task2", 1)
    result3 = await slow_operation("task3", 1)
    
    elapsed = time.time() - start
    
    return jsonify({
        "results": [result1, result2, result3],
        "elapsed_seconds": elapsed,
        "note": "This is sequential - takes 3 seconds"
    })

if __name__ == "__main__":
    app.run(debug=True, port=5000)
```

Run both routes and compare the timing:

```bash
# Test concurrent - should take ~1 second
time curl http://localhost:5000/test-concurrent

# Test sequential - should take ~3 seconds
time curl http://localhost:5000/test-sequential
```

## Common Mistakes

### ❌ Using Flask < 3.0

```bash
# OLD: Flask 2.x doesn't support native async routes
pip install Flask==2.3.0

# You'll get an error:
# AssertionError: view function not async
```

### ✅ Use Flask 3.0+

```bash
# CORRECT: Flask 3.0+ supports async
pip install "Flask>=3.0"
```

### ❌ Using sync time.sleep

```python
# WRONG: Blocks the event loop
async def slow_route():
    time.sleep(5)  # Blocks everything!
    return "done"
```

### ✅ Use asyncio.sleep

```python
# CORRECT: Non-blocking sleep
async def slow_route():
    await asyncio.sleep(5)  # Other code can run
    return "done"
```

### ❌ Mixing sync and async incorrectly

```python
# WRONG: Calling async function without await
async def get_data():
    return await fetch_api()

@app.route("/broken")
def broken_route():
    result = get_data()  # Returns a coroutine, not the data!
    return result  # Will be a coroutine object, not JSON
```

### ✅ Properly await async functions

```python
# CORRECT: Await the async function
@app.route("/works")
async def working_route():
    result = await get_data()  # Actually gets the data
    return jsonify(result)
```

## Quick Reference

| Task | Command |
|------|---------|
| Check Flask version | `python -c "import flask; print(flask.__version__)"` |
| Install Flask 3.x | `pip install "Flask>=3.0"` |
| Install httpx | `pip install httpx` |
| Install hypercorn (prod server) | `pip install hypercorn` |
| Run with hypercorn | `hypercorn app:app --host 0.0.0.0 --port 5000` |

**Minimum requirements:**
- Flask >= 3.0
- Python >= 3.8 (3.10+ recommended)

## Next Steps

Continue to [02_writing_async_route_handlers.md](02_writing_async_route_handlers.md) to learn more advanced async route patterns and best practices.