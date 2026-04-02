# Middleware Performance

## Overview

Middleware runs on every request, so performance is critical. This guide covers optimization techniques, common bottlenecks, and best practices.

## Performance Impact

### Measuring Middleware Overhead

```python
# Example 1: Measuring middleware performance
from fastapi import FastAPI, Request
import time
import asyncio

app = FastAPI()

# Track middleware timing
timings = []

@app.middleware("http")
async def timing_middleware(request: Request, call_next):
    """Measure middleware overhead"""
    start = time.perf_counter()

    response = await call_next(request)

    duration = time.perf_counter() - start
    timings.append({
        "path": request.url.path,
        "duration": duration
    })

    response.headers["X-Process-Time"] = str(round(duration, 6))
    return response

@app.get("/items/")
async def list_items():
    return {"items": []}

@app.get("/timings/")
async def get_timings():
    """View performance metrics"""
    if not timings:
        return {"message": "No timings recorded"}

    avg = sum(t["duration"] for t in timings) / len(timings)
    return {
        "total_requests": len(timings),
        "average_duration": round(avg, 6),
        "recent": timings[-10:]
    }
```

## Optimization Techniques

### Async Operations

```python
# Example 2: Async middleware patterns
from fastapi import FastAPI, Request
import asyncio
import httpx

app = FastAPI()

@app.middleware("http")
async def async_middleware(request: Request, call_next):
    """
    Use async for I/O operations.
    Never block the event loop.
    """
    # GOOD: Async HTTP call
    async with httpx.AsyncClient() as client:
        pass  # Non-blocking

    # BAD: Blocking call
    # import requests
    # requests.get("...")  # Blocks event loop!

    return await call_next(request)

@app.get("/items/")
async def list_items():
    return {"items": []}
```

### Minimal Processing

```python
# Example 3: Efficient middleware
from fastapi import FastAPI, Request
import time

app = FastAPI()

@app.middleware("http")
async def efficient_middleware(request: Request, call_next):
    """
    Keep middleware lean:
    1. Minimal computation
    2. Avoid unnecessary allocations
    3. Skip processing for static files
    """
    # Skip for static files
    if request.url.path.startswith("/static/"):
        return await call_next(request)

    # Minimal processing
    start = time.time()
    response = await call_next(request)
    response.headers["X-Time"] = str(time.time() - start)

    return response

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Conditional Middleware

### Skip Unnecessary Processing

```python
# Example 4: Conditional middleware
from fastapi import FastAPI, Request

app = FastAPI()

# Paths to skip
SKIP_PATHS = {"/health", "/metrics", "/docs", "/openapi.json"}

@app.middleware("http")
async def conditional_middleware(request: Request, call_next):
    """Skip middleware for certain paths"""
    if request.url.path in SKIP_PATHS:
        return await call_next(request)

    # Full processing for other paths
    response = await call_next(request)
    response.headers["X-Processed"] = "true"
    return response

@app.get("/health")
async def health():
    return {"status": "healthy"}

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Caching in Middleware

### Response Caching

```python
# Example 5: Middleware caching
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import hashlib
import json

app = FastAPI()

# Simple cache
cache = {}
CACHE_TTL = 60  # seconds

@app.middleware("http")
async def cache_middleware(request: Request, call_next):
    """Cache GET responses"""
    # Only cache GET requests
    if request.method != "GET":
        return await call_next(request)

    # Create cache key
    cache_key = hashlib.md5(
        str(request.url).encode()
    ).hexdigest()

    # Check cache
    if cache_key in cache:
        return JSONResponse(content=cache[cache_key])

    # Process request
    response = await call_next(request)

    # Cache response (simplified)
    if response.status_code == 200:
        cache[cache_key] = {"cached": True}

    return response

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Best Practices

### Performance Guidelines

```python
# Example 6: Best practices
from fastapi import FastAPI, Request

app = FastAPI()

@app.middleware("http")
async def performant_middleware(request: Request, call_next):
    """
    Performance best practices:
    1. Use async for I/O
    2. Skip unnecessary paths
    3. Minimize allocations
    4. Cache when possible
    5. Measure and monitor
    """
    # Skip health checks
    if request.url.path in ("/health", "/metrics"):
        return await call_next(request)

    # Minimal processing
    response = await call_next(request)
    return response
```

## Common Bottlenecks

### Avoid These Patterns

```python
# Example 7: Anti-patterns to avoid
from fastapi import FastAPI, Request
import time

app = FastAPI()

# BAD: Blocking operations
@app.middleware("http")
async def blocking_middleware(request: Request, call_next):
    # DON'T: time.sleep()  # Blocks event loop
    # DON'T: requests.get()  # Synchronous HTTP
    # DON'T: file.read()  # Synchronous I/O
    return await call_next(request)

# BAD: Heavy computation
@app.middleware("http")
async def heavy_middleware(request: Request, call_next):
    # DON'T: Complex algorithms
    # DON'T: Large data processing
    return await call_next(request)

# GOOD: Keep it simple
@app.middleware("http")
async def simple_middleware(request: Request, call_next):
    return await call_next(request)

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Summary

| Optimization | Impact | Example |
|--------------|--------|---------|
| Async I/O | High | Use `httpx.AsyncClient` |
| Skip paths | Medium | Skip `/health`, `/static/` |
| Minimal logic | Medium | Avoid heavy computation |
| Caching | High | Cache GET responses |
| Measure | Essential | Track timing |

## Next Steps

Continue learning about:
- [Dependencies](../05_dependencies/01_dependency_injection_basics.md) - DI patterns
- [Testing](../../../01_getting_started/02_setup_and_installation/03_development_environment.md) - Testing setup
