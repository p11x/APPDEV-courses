<!-- FILE: 11_async_flask_and_background_tasks/01_async_fundamentals/02_async_await_basics.md -->

## Overview

The `async` and `await` keywords are the foundation of asynchronous programming in Python. This file teaches you how to define async functions, use `await` to pause execution without blocking, and combine multiple async operations efficiently using `asyncio.gather()`.

## Prerequisites

- Basic Python knowledge (functions, variables, loops)
- Understanding of sync vs async code from the previous file

## Core Concepts

### The `async` Keyword

The `async` keyword transforms a regular function into an asynchronous function (also called a "coroutine"). An async function:

- Must be called using `await` or passed to the event loop
- Returns a coroutine object when called (not the actual return value)
- Can use `await` inside its body to pause execution

```python
# This is an async function
async def fetch_data():
    # Inside an async function, we can use await
    result = await some_async_operation()
    return result
```

When you call `fetch_data()`, it doesn't run immediately—instead, it returns a coroutine object that must be awaited or scheduled.

### The `await` Keyword

The `await` keyword pauses the current async function until the awaited operation completes:

```python
async def process():
    print("Starting...")
    
    # This pauses process() until get_data() completes
    # The event loop can run other tasks during this wait
    data = await get_data()
    
    print(f"Got data: {data}")
    return data
```

Key points about `await`:
- Can only be used inside an `async` function
- Suspends execution of the current function (not the entire program)
- Returns the result of the awaited coroutine
- Raises an exception if the awaited coroutine raises one

### Running Async Code

To actually execute async functions, you need the event loop:

```python
import asyncio

async def hello():
    print("Hello!")
    await asyncio.sleep(1)
    print("World!")

# Method 1: asyncio.run() (Python 3.7+)
asyncio.run(hello())

# Method 2: get_event_loop() (older code)
loop = asyncio.get_event_loop()
loop.run_until_complete(hello())
```

### Awaiting Multiple Tasks Concurrently

The `asyncio.gather()` function runs multiple coroutines concurrently and waits for all of them to complete:

```python
import asyncio

async def task_a():
    await asyncio.sleep(2)
    return "A"

async def task_b():
    await asyncio.sleep(1)
    return "B"

async def task_c():
    await asyncio.sleep(3)
    return "C"

async def main():
    # Run all three tasks concurrently
    results = await asyncio.gather(task_a(), task_b(), task_c())
    print(results)  # ['A', 'B', 'C']

asyncio.run(main())
# Total time: ~3 seconds (the longest task)
# NOT 6 seconds (2+1+3) which it would be if sequential
```

### Creating Tasks

`asyncio.create_task()` schedules a coroutine to run concurrently without waiting for it to complete:

```python
async def main():
    # Schedule task_a to run but don't wait for it
    task = asyncio.create_task(task_a())
    
    # Do other work while task_a runs in background
    print("Doing other work...")
    await asyncio.sleep(0.5)
    
    # Now wait for the task to complete
    result = await task
    print(f"Task result: {result}")
```

## Code Walkthrough

Let's build a practical example: fetching data from multiple APIs concurrently.

```python
# async_example.py
import asyncio
import time

# Simulate async API calls with different delays
async def fetch_user(user_id: int) -> dict:
    """Fetch user data from a mock API"""
    print(f"Fetching user {user_id}...")
    await asyncio.sleep(2)  # Simulates 2 second API call
    return {"id": user_id, "name": f"User {user_id}"}

async def fetch_posts(user_id: int) -> list:
    """Fetch posts from a mock API"""
    print(f"Fetching posts for user {user_id}...")
    await asyncio.sleep(1.5)  # Simulates 1.5 second API call
    return [{"post_id": 1}, {"post_id": 2}, {"post_id": 3}]

async def fetch_comments(post_id: int) -> list:
    """Fetch comments for a post"""
    print(f"Fetching comments for post {post_id}...")
    await asyncio.sleep(0.5)  # Simulates 0.5 second API call
    return [{"comment_id": 1}, {"comment_id": 2}]

async def get_user_profile(user_id: int) -> dict:
    """
    Fetch complete user profile: user data + posts + comments
    This demonstrates sequential await (one after another)
    """
    start = time.time()
    
    user = await fetch_user(user_id)
    posts = await fetch_posts(user_id)
    comments = await fetch_comments(posts[0]["post_id"])
    
    elapsed = time.time() - start
    
    return {
        "user": user,
        "posts": posts,
        "comments": comments,
        "fetch_time": f"{elapsed:.2f}s"
    }

async def get_all_profiles(user_ids: list) -> list:
    """
    Fetch multiple user profiles concurrently
    This demonstrates parallel await with gather
    """
    start = time.time()
    
    # Create tasks for all users
    tasks = [get_user_profile(uid) for uid in user_ids]
    
    # Wait for ALL tasks to complete concurrently
    profiles = await asyncio.gather(*tasks)
    
    elapsed = time.time() - start
    
    print(f"Fetched {len(profiles)} profiles in {elapsed:.2f}s")
    return profiles

async def demo_individual_fetches():
    """Demo: fetching each resource separately"""
    print("\n=== Individual Fetches (Sequential) ===")
    
    # This fetches user 1, then user 2, then user 3 - one after another
    for user_id in [1, 2, 3]:
        profile = await get_user_profile(user_id)
        print(f"Got profile for user {user_id}")

async def demo_concurrent_fetches():
    """Demo: fetching multiple users at once"""
    print("\n=== Concurrent Fetches (Parallel) ===")
    
    # This fetches all 3 users in parallel
    profiles = await get_all_profiles([1, 2, 3])
    
    for profile in profiles:
        print(f"User {profile['user']['id']}: {profile['fetch_time']}")

# Run the demo
async def main():
    await demo_individual_fetches()
    await demo_concurrent_fetches()

if __name__ == "__main__":
    asyncio.run(main())
```

### Running the Example

```bash
python async_example.py
```

**Expected Output:**

```
=== Individual Fetches (Sequential) ===
Fetching user 1...
Got profile for user 1
Fetching user 2...
Got profile for user 2
Fetching user 3...
Got profile for user 3

=== Concurrent Fetches (Parallel) ===
Fetching user 1...
Fetching user 2...
Fetching user 3...
Fetching posts for user 1...
Fetching posts for user 2...
Fetching posts for user 3...
Fetching comments for post 1...
Fetching comments for post 1...
Fetching comments for post 1...
Fetched 3 profiles in 4.01s
User 1: 4.00s
User 2: 4.00s
User 3: 4.00s
```

Notice:
- Individual fetches: ~12 seconds total (4s × 3 users, sequential)
- Concurrent fetches: ~4 seconds total (all run in parallel)

### Line-by-Line Breakdown

- `async def fetch_user(user_id: int) -> dict:` - Declares an async function that returns a dictionary
- `await asyncio.sleep(2)` - Non-blocking sleep; event loop can run other code
- `async def get_user_profile(user_id: int) -> dict:` - Fetches user + posts + comments sequentially
- `await fetch_user(user_id)` - Waits for user data, then continues
- `await asyncio.gather(*tasks)` - Runs all tasks concurrently and waits for all to complete

## Common Mistakes

### ❌ Forgetting to await

```python
# WRONG: Forgetting await
async def get_data():
    result = fetch_data()  # Returns a coroutine, not the data!
    return result  # Will fail or return unexpected type
```

### ✅ Always await async functions

```python
# CORRECT: Await the coroutine
async def get_data():
    result = await fetch_data()  # Actually gets the data
    return result  # Returns the actual data
```

### ❌ Using time.sleep instead of asyncio.sleep

```python
# WRONG: Blocking sleep stops entire event loop
async def slow_operation():
    time.sleep(10)  # Blocks everything for 10 seconds!
```

### ✅ Use asyncio.sleep for non-blocking delays

```python
# CORRECT: Non-blocking sleep
async def slow_operation():
    await asyncio.sleep(10)  # Other code can run during these 10 seconds
```

### ❌ Not handling exceptions in gather

```python
# PROBLEM: If one task fails, gather cancels all others
async def risky_task():
    raise ValueError("Oops!")

async def main():
    results = await asyncio.gather(risky_task(), task_b())  # Both fail!
```

### ✅ Handle exceptions with return_exceptions

```python
# CORRECT: Handle exceptions gracefully
async def main():
    results = await asyncio.gather(
        risky_task(),
        task_b(),
        return_exceptions=True  # Exceptions become values in results
    )
    
    for i, result in enumerate(results):
        if isinstance(result, Exception):
            print(f"Task {i} failed: {result}")
        else:
            print(f"Task {i} succeeded: {result}")
```

## Quick Reference

| Pattern | Code |
|----------|------|
| Define async function | `async def func():` |
| Call async function | `result = await func()` |
| Run async code | `asyncio.run(main())` |
| Run tasks concurrently | `await asyncio.gather(*tasks)` |
| Schedule without waiting | `task = asyncio.create_task(coro())` |
| Non-blocking sleep | `await asyncio.sleep(seconds)` |
| Handle gather exceptions | `await asyncio.gather(*tasks, return_exceptions=True)` |

## Next Steps

Continue to [03_when_to_use_async.md](03_when_to_use_async.md) to learn when async programming is appropriate and when stick with synchronous code.