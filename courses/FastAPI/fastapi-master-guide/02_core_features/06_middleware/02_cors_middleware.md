# CORS Middleware

## Overview

CORS (Cross-Origin Resource Sharing) middleware handles browser security restrictions, allowing controlled access to your API from different domains.

## Basic CORS Setup

### Simple CORS Configuration

```python
# Example 1: Basic CORS setup
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

# Add CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:3000", "https://myapp.com"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/items/")
async def list_items():
    return {"items": []}

@app.post("/items/")
async def create_item(name: str):
    return {"name": name}
```

### Development CORS

```python
# Example 2: Development CORS (permissive)
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

# Development: Allow all origins
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Allow any origin
    allow_credentials=False,  # Must be False with allow_origins=["*"]
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/items/")
async def list_items():
    return {"items": []}
```

### Production CORS

```python
# Example 3: Production CORS (restrictive)
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
import os

app = FastAPI()

# Production: Specific origins only
ALLOWED_ORIGINS = os.getenv("ALLOWED_ORIGINS", "").split(",")

app.add_middleware(
    CORSMiddleware,
    allow_origins=ALLOWED_ORIGINS,
    allow_credentials=True,
    allow_methods=["GET", "POST", "PUT", "DELETE"],
    allow_headers=["Authorization", "Content-Type"],
    expose_headers=["X-Total-Count", "X-Request-ID"],
    max_age=600,  # Cache preflight for 10 minutes
)

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## CORS Configuration Options

### Detailed Configuration

```python
# Example 4: Complete CORS configuration
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    # Origins
    allow_origins=[
        "https://example.com",
        "https://app.example.com",
        "http://localhost:3000",
    ],

    # Credentials (cookies, auth headers)
    allow_credentials=True,

    # HTTP methods
    allow_methods=["GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS"],

    # Request headers
    allow_headers=[
        "Authorization",
        "Content-Type",
        "X-Requested-With",
        "X-API-Key",
    ],

    # Response headers exposed to browser
    expose_headers=[
        "X-Total-Count",
        "X-Request-ID",
        "X-RateLimit-Remaining",
    ],

    # Preflight cache duration (seconds)
    max_age=3600,
)

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Handling CORS Issues

### Common CORS Problems

```python
# Example 5: Debugging CORS
from fastapi import FastAPI, Request
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:3000"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.middleware("http")
async def log_cors(request: Request, call_next):
    """Log CORS-related headers"""
    origin = request.headers.get("origin")
    print(f"Origin: {origin}")

    response = await call_next(request)

    print(f"Access-Control-Allow-Origin: {response.headers.get('access-control-allow-origin')}")

    return response

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Best Practices

### CORS Guidelines

```python
# Example 6: Best practices
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
import os

app = FastAPI()

# Environment-specific CORS
ENV = os.getenv("ENVIRONMENT", "development")

if ENV == "production":
    # Restrictive CORS for production
    app.add_middleware(
        CORSMiddleware,
        allow_origins=os.getenv("ALLOWED_ORIGINS", "").split(","),
        allow_credentials=True,
        allow_methods=["GET", "POST", "PUT", "DELETE"],
        allow_headers=["Authorization", "Content-Type"],
    )
else:
    # Permissive CORS for development
    app.add_middleware(
        CORSMiddleware,
        allow_origins=["*"],
        allow_credentials=False,
        allow_methods=["*"],
        allow_headers=["*"],
    )

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Summary

| Option | Purpose | Example |
|--------|---------|---------|
| `allow_origins` | Allowed domains | `["https://example.com"]` |
| `allow_credentials` | Allow cookies/auth | `True` |
| `allow_methods` | HTTP methods | `["GET", "POST"]` |
| `allow_headers` | Request headers | `["Authorization"]` |
| `expose_headers` | Response headers | `["X-Total-Count"]` |
| `max_age` | Preflight cache | `3600` (seconds) |

## Next Steps

Continue learning about:
- [Custom Middleware](./03_custom_middleware.md) - Custom processing
- [Error Middleware](./04_error_middleware.md) - Error handling
- [Middleware Performance](./05_middleware_performance.md) - Optimization
