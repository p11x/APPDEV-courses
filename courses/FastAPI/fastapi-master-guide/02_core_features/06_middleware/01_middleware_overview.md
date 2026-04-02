# Middleware Overview

## Overview

Middleware processes every request before it reaches route handlers and every response before it's sent to clients. It's ideal for cross-cutting concerns like logging, authentication, and CORS.

## Basic Middleware

### Simple Middleware

```python
# Example 1: Basic middleware
from fastapi import FastAPI, Request
from starlette.middleware.base import BaseHTTPMiddleware
import time

app = FastAPI()

@app.middleware("http")
async def timing_middleware(request: Request, call_next):
    """
    Middleware that measures request processing time.
    Runs for every HTTP request.
    """
    start = time.time()

    # Process request
    response = await call_next(request)

    # Add timing header
    duration = time.time() - start
    response.headers["X-Process-Time"] = str(round(duration, 4))

    return response

@app.get("/items/")
async def list_items():
    return {"items": []}
```

### Logging Middleware

```python
# Example 2: Request logging
from fastapi import FastAPI, Request
import logging

app = FastAPI()
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

@app.middleware("http")
async def log_requests(request: Request, call_next):
    """Log all requests"""
    # Log request
    logger.info(f"➡️ {request.method} {request.url.path}")

    # Process
    response = await call_next(request)

    # Log response
    logger.info(f"⬅️ {response.status_code}")

    return response

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Middleware Patterns

### Request/Response Processing

```python
# Example 3: Request and response modification
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import uuid

app = FastAPI()

@app.middleware("http")
async def add_request_id(request: Request, call_next):
    """Add unique ID to each request"""
    # Generate request ID
    request_id = str(uuid.uuid4())
    request.state.request_id = request_id

    # Process request
    response = await call_next(request)

    # Add to response
    response.headers["X-Request-ID"] = request_id

    return response

@app.middleware("http")
async def add_security_headers(request: Request, call_next):
    """Add security headers"""
    response = await call_next(request)

    response.headers["X-Content-Type-Options"] = "nosniff"
    response.headers["X-Frame-Options"] = "DENY"
    response.headers["X-XSS-Protection"] = "1; mode=block"

    return response

@app.get("/items/")
async def list_items(request: Request):
    return {"items": [], "request_id": request.state.request_id}
```

### Error Handling Middleware

```python
# Example 4: Global error handling
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

app = FastAPI()

@app.middleware("http")
async def error_handler(request: Request, call_next):
    """Catch all exceptions"""
    try:
        return await call_next(request)
    except Exception as e:
        return JSONResponse(
            status_code=500,
            content={
                "error": "Internal server error",
                "message": str(e)
            }
        )

@app.get("/error/")
async def trigger_error():
    raise ValueError("Test error")
```

## Class-Based Middleware

### Custom Middleware Class

```python
# Example 5: Class-based middleware
from fastapi import FastAPI
from starlette.middleware.base import BaseHTTPMiddleware
from starlette.requests import Request
from starlette.responses import Response

class CustomHeaderMiddleware(BaseHTTPMiddleware):
    """Class-based middleware for custom headers"""

    def __init__(self, app, header_value: str = "FastAPI"):
        super().__init__(app)
        self.header_value = header_value

    async def dispatch(self, request: Request, call_next):
        # Pre-processing
        response = await call_next(request)

        # Post-processing
        response.headers["X-Custom-Header"] = self.header_value

        return response

app = FastAPI()
app.add_middleware(CustomHeaderMiddleware, header_value="MyApp")

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Middleware Order

### Execution Order

```python
# Example 6: Middleware execution order
from fastapi import FastAPI, Request

app = FastAPI()

@app.middleware("http")
async def first_middleware(request: Request, call_next):
    """Executes first (outermost)"""
    print("1. Before first")
    response = await call_next(request)
    print("5. After first")
    return response

@app.middleware("http")
async def second_middleware(request: Request, call_next):
    """Executes second"""
    print("2. Before second")
    response = await call_next(request)
    print("4. After second")
    return response

@app.get("/items/")
async def list_items():
    """Route handler executes third"""
    print("3. In handler")
    return {"items": []}

# Order:
# 1. Before first
# 2. Before second
# 3. In handler
# 4. After second
# 5. After first
```

## Best Practices

### Middleware Guidelines

```python
# Example 7: Best practices
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import time

app = FastAPI()

@app.middleware("http")
async def best_practice_middleware(request: Request, call_next):
    """
    Best practices:
    1. Keep middleware simple
    2. Handle errors gracefully
    3. Don't block the event loop
    4. Use appropriate middleware order
    """
    try:
        start = time.time()
        response = await call_next(request)
        duration = time.time() - start

        response.headers["X-Process-Time"] = str(round(duration, 4))
        return response

    except Exception as e:
        return JSONResponse(
            status_code=500,
            content={"error": str(e)}
        )
```

## Summary

| Feature | Usage | Example |
|---------|-------|---------|
| HTTP middleware | `@app.middleware("http")` | Timing, logging |
| Class middleware | `BaseHTTPMiddleware` | Complex logic |
| Order | Registration order | First registered = outermost |
| Request state | `request.state` | Pass data to handlers |

## Next Steps

Continue learning about:
- [CORS Middleware](./02_cors_middleware.md) - Cross-origin requests
- [Custom Middleware](./03_custom_middleware.md) - Custom processing
- [Error Middleware](./04_error_middleware.md) - Error handling
