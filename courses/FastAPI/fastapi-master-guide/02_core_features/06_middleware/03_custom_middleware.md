# Custom Middleware

## Overview

Custom middleware enables request/response processing for logging, authentication, rate limiting, and other cross-cutting concerns.

## Building Custom Middleware

### Request Processing Middleware

```python
# Example 1: Request enrichment middleware
from fastapi import FastAPI, Request
from starlette.middleware.base import BaseHTTPMiddleware
import uuid
import time

app = FastAPI()

@app.middleware("http")
async def enrich_request(request: Request, call_next):
    """Add metadata to request"""
    # Add request ID
    request.state.request_id = str(uuid.uuid4())
    request.state.start_time = time.time()

    # Process request
    response = await call_next(request)

    # Add headers
    response.headers["X-Request-ID"] = request.state.request_id

    return response

@app.get("/items/")
async def list_items(request: Request):
    return {
        "items": [],
        "request_id": request.state.request_id
    }
```

### Authentication Middleware

```python
# Example 2: Authentication middleware
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

app = FastAPI()

PUBLIC_PATHS = ["/", "/health", "/docs", "/openapi.json"]

@app.middleware("http")
async def auth_middleware(request: Request, call_next):
    """Check authentication for protected routes"""
    # Skip public paths
    if request.url.path in PUBLIC_PATHS:
        return await call_next(request)

    # Check authorization header
    auth = request.headers.get("Authorization")
    if not auth:
        return JSONResponse(
            status_code=401,
            content={"error": "Authorization required"}
        )

    if not auth.startswith("Bearer "):
        return JSONResponse(
            status_code=401,
            content={"error": "Invalid authorization format"}
        )

    # Process request
    return await call_next(request)

@app.get("/items/")
async def list_items():
    return {"items": []}

@app.get("/health")
async def health():
    return {"status": "healthy"}
```

### Rate Limiting Middleware

```python
# Example 3: Rate limiting
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import time
from collections import defaultdict

app = FastAPI()

# Rate limit storage
rate_limits = defaultdict(list)
RATE_LIMIT = 100  # Requests per minute
WINDOW = 60  # Seconds

@app.middleware("http")
async def rate_limit_middleware(request: Request, call_next):
    """Simple rate limiting"""
    client_ip = request.client.host
    current_time = time.time()

    # Clean old requests
    rate_limits[client_ip] = [
        t for t in rate_limits[client_ip]
        if current_time - t < WINDOW
    ]

    # Check limit
    if len(rate_limits[client_ip]) >= RATE_LIMIT:
        return JSONResponse(
            status_code=429,
            content={"error": "Rate limit exceeded"},
            headers={"Retry-After": str(WINDOW)}
        )

    # Record request
    rate_limits[client_ip].append(current_time)

    return await call_next(request)

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Class-Based Middleware

### Custom Middleware Class

```python
# Example 4: Class-based middleware
from fastapi import FastAPI
from starlette.middleware.base import BaseHTTPMiddleware
from starlette.requests import Request
from starlette.responses import Response
import logging

logger = logging.getLogger(__name__)

class RequestLoggerMiddleware(BaseHTTPMiddleware):
    """Log all requests with details"""

    async def dispatch(self, request: Request, call_next):
        # Log request
        logger.info(
            f"Request: {request.method} {request.url.path} "
            f"from {request.client.host}"
        )

        # Process
        response = await call_next(request)

        # Log response
        logger.info(
            f"Response: {response.status_code} "
            f"for {request.method} {request.url.path}"
        )

        return response

class TimingMiddleware(BaseHTTPMiddleware):
    """Add timing information"""

    async def dispatch(self, request: Request, call_next):
        import time
        start = time.time()

        response = await call_next(request)

        duration = time.time() - start
        response.headers["X-Process-Time"] = str(round(duration, 4))

        return response

app = FastAPI()
app.add_middleware(RequestLoggerMiddleware)
app.add_middleware(TimingMiddleware)

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Middleware with Dependencies

### Combining Middleware and Dependencies

```python
# Example 5: Middleware with request state
from fastapi import FastAPI, Request, Depends
import uuid

app = FastAPI()

@app.middleware("http")
async def set_request_context(request: Request, call_next):
    """Set request context"""
    request.state.request_id = str(uuid.uuid4())
    request.state.user_id = None  # Set by auth

    return await call_next(request)

def get_request_id(request: Request):
    """Dependency to get request ID"""
    return request.state.request_id

@app.get("/items/")
async def list_items(request_id: str = Depends(get_request_id)):
    return {"items": [], "request_id": request_id}
```

## Best Practices

### Middleware Guidelines

```python
# Example 6: Best practices
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

app = FastAPI()

@app.middleware("http")
async def best_practice_middleware(request: Request, call_next):
    """
    Best practices:
    1. Keep middleware focused
    2. Handle errors gracefully
    3. Don't modify request/response unnecessarily
    4. Use request.state for passing data
    """
    try:
        response = await call_next(request)
        return response
    except Exception as e:
        return JSONResponse(
            status_code=500,
            content={"error": "Internal server error"}
        )
```

## Summary

| Pattern | Use Case | Example |
|---------|----------|---------|
| `@app.middleware("http")` | Function middleware | Request logging |
| `BaseHTTPMiddleware` | Class middleware | Complex logic |
| `request.state` | Pass data | Request ID |
| Error handling | Catch exceptions | Global error handler |

## Next Steps

Continue learning about:
- [Error Middleware](./04_error_middleware.md) - Error handling
- [Middleware Performance](./05_middleware_performance.md) - Optimization
- [Dependencies](../05_dependencies/01_dependency_injection_basics.md) - DI patterns
