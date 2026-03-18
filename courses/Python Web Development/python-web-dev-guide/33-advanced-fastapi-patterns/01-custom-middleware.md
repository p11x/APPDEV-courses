# Custom Middleware in FastAPI

## What You'll Learn
- FastAPI's middleware system and request/response lifecycle
- Creating custom middleware for cross-cutting concerns
- Implementing request logging, metrics, and authentication
- Middleware ordering and dependencies
- Using ASGI middleware libraries

## Prerequisites
- Completed `04-fastapi/04-dependency-injection.md` — FastAPI dependency system
- Understanding of async/await in Python 3.11+
- Basic understanding of HTTP requests and responses

## FastAPI Middleware Architecture

Middleware in FastAPI sits between the client and your route handlers. Every request passes through middleware twice — once before reaching your endpoint, and once after when generating the response.

```
┌─────────────────────────────────────────────────────────────────┐
│                        FastAPI Application                        │
│                                                                  │
│  Request ──▶  Middleware 1  ──▶  Middleware 2  ──▶  Endpoint  │
│                  │                      │                  │       │
│                  ▼                      ▼                  ▼       │
│              Response ◀── Middleware 2 ◀── Middleware 1 ◀──     │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## Creating Custom Middleware

FastAPI provides two ways to create middleware:

### 1. Using the @app.middleware Decorator (Simplest)

```python
from fastapi import FastAPI, Request
from starlette.middleware.base import BaseHTTPMiddleware
import time

app = FastAPI()

@app.middleware("http")
async def add_process_time_header(request: Request, call_next):
    """Add X-Process-Time header to every response."""
    start_time = time.perf_counter()
    
    # Process request
    response = await call_next(request)
    
    # Calculate processing time
    process_time = time.perf_counter() - start_time
    
    # Add header to response
    response.headers["X-Process-Time"] = str(process_time)
    
    return response

@app.get("/items/{item_id}")
async def read_item(item_id: int):
    return {"item_id": item_id, "name": "Test Item"}
```

🔍 **Line-by-Line Breakdown:**
1. `@app.middleware("http")` — Registers function as HTTP middleware
2. `request: Request` — Starlette's request object containing all request data
3. `call_next` — Callable that passes request to next middleware/handler
4. `start_time = time.perf_counter()` — High-resolution timer before processing
5. `await call_next(request)` — Calls the actual endpoint, returns response
6. `response.headers[...]` — Modifying response headers

### 2. Using BaseHTTPMiddleware Class (More Control)

```python
from starlette.middleware.base import BaseHTTPMiddleware
from fastapi import FastAPI, Request, Response
import logging
from typing import Callable

logger = logging.getLogger(__name__)

class RequestLoggingMiddleware(BaseHTTPMiddleware):
    """Middleware for detailed request/response logging."""
    
    async def dispatch(
        self, 
        request: Request, 
        call_next: Callable
    ) -> Response:
        # Log incoming request
        logger.info(f"Incoming request: {request.method} {request.url.path}")
        
        # Process request
        response = await call_next(request)
        
        # Log response
        logger.info(
            f"Response: {request.method} {request.url.path} "
            f"status={response.status_code}"
        )
        
        return response

app = FastAPI()
app.add_middleware(RequestLoggingMiddleware)
```

## Real-World Middleware Examples

### 1. Request ID Middleware (Correlation IDs)

```python
import uuid
from contextvars import ContextVar
from starlette.middleware.base import BaseHTTPMiddleware

# Thread-safe context for request ID
request_id_context: ContextVar[str] = ContextVar("request_id", default="")

class RequestIDMiddleware(BaseHTTPMiddleware):
    """Add unique request ID to every request for tracing."""
    
    async def dispatch(self, request: Request, call_next) -> Response:
        # Generate or extract request ID
        request_id = request.headers.get("X-Request-ID") or str(uuid.uuid4())
        
        # Store in context for access in endpoints
        token = request_id_context.set(request_id)
        
        # Add to response headers for client tracing
        response = await call_next(request)
        response.headers["X-Request-ID"] = request_id
        
        # Reset context
        request_id_context.reset(token)
        
        return response

def get_request_id() -> str:
    """Get current request ID in endpoint."""
    return request_id_context.get()

# Usage in endpoint
@app.get("/users/{user_id}")
async def get_user(user_id: int):
    request_id = get_request_id()
    logger.info(f"Fetching user {user_id}", extra={"request_id": request_id})
    return {"user_id": user_id, "request_id": request_id}
```

### 2. Authentication Middleware

```python
from fastapi import HTTPException, Security
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials

security = HTTPBearer()

class AuthMiddleware(BaseHTTPMiddleware):
    """Extract and validate JWT token."""
    
    async def dispatch(self, request: Request, call_next) -> Response:
        # Skip auth for public paths
        public_paths = ["/docs", "/openapi.json", "/health"]
        if any(request.url.path.startswith(path) for path in public_paths):
            return await call_next(request)
        
        # Extract token
        auth_header = request.headers.get("Authorization")
        
        if not auth_header or not auth_header.startswith("Bearer "):
            raise HTTPException(status_code=401, detail="Missing auth token")
        
        token = auth_header.replace("Bearer ", "")
        
        # Validate token (simplified)
        try:
            payload = jwt.decode(token, "secret", algorithms=["HS256"])
            request.state.user = payload
        except jwt.PyJWTError:
            raise HTTPException(status_code=401, detail="Invalid token")
        
        return await call_next(request)

# In endpoint, access user from state
@app.get("/profile")
async def get_profile(request: Request):
    user = request.state.user
    return {"user_id": user["sub"], "email": user.get("email")}
```

### 3. Rate Limiting Middleware

```python
from collections import defaultdict
from datetime import datetime, timedelta

class RateLimitMiddleware(BaseHTTPMiddleware):
    """Simple in-memory rate limiting."""
    
    def __init__(self, app, requests_per_minute: int = 60):
        super().__init__(app)
        self.requests_per_minute = requests_per_minute
        self.requests: dict[str, list[datetime]] = defaultdict(list)
    
    async def dispatch(self, request: Request, call_next) -> Response:
        client_ip = request.client.host if request.client else "unknown"
        
        # Clean old requests
        cutoff = datetime.utcnow() - timedelta(minutes=1)
        self.requests[client_ip] = [
            req_time for req_time in self.requests[client_ip]
            if req_time > cutoff
        ]
        
        # Check rate limit
        if len(self.requests[client_ip]) >= self.requests_per_minute:
            return Response(
                content='{"error": "Rate limit exceeded"}',
                status_code=429,
                media_type="application/json"
            )
        
        # Record this request
        self.requests[client_ip].append(datetime.utcnow())
        
        return await call_next(request)
```

## Using Third-Party ASGI Middleware

Starlette and other libraries provide pre-built middleware:

```python
from starlette.middleware.cors import CORSMiddleware
from starlette.middleware.gzip import GZipMiddleware
from starlette.middleware.sessions import SessionMiddleware

app = FastAPI()

# CORS - must be added first
app.add_middleware(
    CORSMiddleware,
    allow_origins=["https://example.com"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# GZip compression
app.add_middleware(GZipMiddleware, minimum_size=1000)

# Sessions
app.add_middleware(
    SessionMiddleware,
    secret_key="your-secret-key",
    max_age=3600
)
```

## Middleware Ordering

Middleware order matters! They execute in the order added:

```python
# This order:
app.add_middleware(LoggingMiddleware)     # 1st - runs first on request
app.add_middleware(AuthMiddleware)       # 2nd
app.add_middleware(RateLimitMiddleware)   # 3rd

# Request flow:
# 1. LoggingMiddleware processes first
# 2. AuthMiddleware processes second  
# 3. RateLimitMiddleware processes third
# 4. Endpoint runs
# 5. RateLimitMiddleware processes response
# 6. AuthMiddleware processes response
# 7. LoggingMiddleware processes response
```

## Production Considerations

- **Performance overhead**: Every middleware adds latency. Keep it minimal.
- **Middleware vs dependencies**: Use middleware for things that apply to ALL requests. Use dependencies for route-specific logic.
- **Exception handling**: Middleware should catch exceptions from `call_next` and return appropriate responses.
- **Async/sync mixing**: Be careful mixing sync and async code in middleware.

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Blocking calls in async middleware

**Wrong:**
```python
import time

@app.middleware("http")
async def slow_middleware(request: Request, call_next):
    time.sleep(1)  # BLOCKS the event loop!
    return await call_next(request)
```

**Why it fails:** `time.sleep()` blocks the entire event loop, making all concurrent requests slow.

**Fix:**
```python
import asyncio

@app.middleware("http")
async def fast_middleware(request: Request, call_next):
    await asyncio.sleep(1)  # Non-blocking!
    return await call_next(request)
```

### ❌ Mistake 2: Not calling call_next

**Wrong:**
```python
@app.middleware("http")
async def broken_middleware(request: Request, call_next):
    # Never calls the endpoint!
    return Response(content="You shall not pass!")
```

**Why it fails:** The request never reaches the endpoint. This middleware intercepts everything.

**Fix:**
```python
@app.middleware("http")
async def working_middleware(request: Request, call_next):
    # Process first, then continue
    response = await call_next(request)
    return response
```

### ❌ Mistake 3: Heavy computation in middleware

**Wrong:**
```python
import json

@app.middleware("http")
async def heavy_middleware(request: Request, call_next):
    # Process request body multiple times
    body = await request.body()
    data = json.loads(body)  # Heavy parsing
    # ... lots of processing
    return await call_next(request)
```

**Why it fails:** Middleware runs on every request. Heavy processing slows everything down.

**Fix:** Use lightweight middleware and defer heavy work to background tasks:
```python
@app.middleware("http")
async def light_middleware(request: Request, call_next):
    # Just add a header, fast!
    response = await call_next(request)
    response.headers["X-Processed"] = "true"
    return response
```

## Summary

- FastAPI middleware processes requests before and after endpoints
- Use `@app.middleware()` for simple cases, `BaseHTTPMiddleware` for complex logic
- Common uses: logging, auth, rate limiting, request ID tracking
- Middleware order matters — first added runs first on request, last added runs first on response
- Keep middleware lightweight to avoid performance impact

## Next Steps

→ Continue to `02-lifespan-events.md` to learn about managing application lifecycle events in FastAPI.
