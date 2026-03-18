# Mobile Backend Best Practices

## What You'll Learn

- Best practices for mobile backends
- Performance optimization
- Security considerations
- Scalability patterns

## Prerequisites

- Completed `08-push-notification-strategies.md`

## Introduction

This guide covers essential best practices for building robust mobile backends.

## API Design Best Practices

### Versioning

```python
from fastapi import FastAPI, APIRouter
from versioning import VersionedFastAPI


app = FastAPI(title="Mobile API")

# Version 1
v1 = APIRouter(prefix="/v1")


@v1.get("/users/{user_id}")
async def get_user(user_id: int):
    return {"id": user_id, "name": "User"}


# Version 2 (with breaking changes)
v2 = APIRouter(prefix="/v2")


@v2.get("/users/{user_id}")
async def get_user_v2(user_id: int):
    # Different response format
    return {
        "data": {
            "id": user_id,
            "full_name": "User Name",
            "profile": {
                "avatar": "url",
                "bio": "bio",
            },
        }
    }


app.include_router(v1)
app.include_router(v2)
```

### Response Compression

```python
from fastapi.middleware.gzip import GZipMiddleware


app = FastAPI()

# Compress responses larger than 500 bytes
app.add_middleware(GZipMiddleware, minimum_size=500)
```

## Security Best Practices

```python
from fastapi import FastAPI, Security, HTTPException
from fastapi.security import APIKeyHeader
import rate_limiter


app = FastAPI()

# Rate limiting
rate_limiter = rate_limiter.LimitStore()


@app.middleware("http")
async def rate_limit(request, call_next):
    client_id = request.client.host
    if not rate_limiter.check(client_id):
        raise HTTPException(status_code=429, detail="Rate limited")
    rate_limiter.increment(client_id)
    return await call_next(request)


# API Key authentication
api_key_header = APIKeyHeader(name="X-API-Key")


@app.get("/api/protected")
async def protected_route(api_key: str = Security(api_key_header)):
    if api_key != os.environ["API_KEY"]:
        raise HTTPException(status_code=403, detail="Invalid API key")
    return {"data": "secret"}
```

## Caching

```python
from fastapi import FastAPI
from fastapi.responses import JSONResponse
import cache


app = FastAPI()

# Simple in-memory cache
cache_store = {}


@app.get("/api/users/{user_id}")
async def get_user(user_id: int):
    cache_key = f"user:{user_id}"
    
    # Check cache
    if cache_key in cache_store:
        return JSONResponse(
            content=cache_store[cache_key],
            headers={"X-Cache": "HIT"},
        )
    
    # Fetch from database
    user_data = {"id": user_id, "name": "User"}
    
    # Store in cache
    cache_store[cache_key] = user_data
    
    return JSONResponse(
        content=user_data,
        headers={"X-Cache": "MISS"},
    )


@app.on_event("startup")
async def startup():
    # Initialize Redis cache in production
    pass
```

## Error Handling

```python
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse


app = FastAPI()


@app.exception_handler(Exception)
async def global_exception_handler(request: Request, exc: Exception):
    return JSONResponse(
        status_code=500,
        content={
            "error": "Internal server error",
            "message": str(exc) if os.environ.get("DEBUG") else None,
        },
    )
```

## Logging

```python
import logging
from fastapi import FastAPI


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


app = FastAPI()


@app.middleware("http")
async def log_requests(request, call_next):
    logger.info(f"{request.method} {request.url.path}")
    response = await call_next(request)
    logger.info(f"Status: {response.status_code}")
    return response
```

## Summary

- Version your API from the start
- Implement rate limiting
- Use caching appropriately
- Handle errors gracefully
- Log important events

## Next Steps

This concludes the Mobile and Cross-Platform Backends section. Continue to folder 69 (Additional Real Projects) for more project examples.
