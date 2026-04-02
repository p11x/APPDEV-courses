# API Versioning Strategies

## Overview

API versioning enables evolution while maintaining backward compatibility.

## Versioning Strategies

### URL Path Versioning

```python
# Example 1: URL path versioning
from fastapi import FastAPI, APIRouter

app = FastAPI()

# Version 1
v1 = APIRouter(prefix="/api/v1", tags=["v1"])

@v1.get("/users")
async def v1_users():
    return {"version": 1, "users": []}

@v1.get("/items/{item_id}")
async def v1_item(item_id: int):
    return {"version": 1, "id": item_id}

# Version 2
v2 = APIRouter(prefix="/api/v2", tags=["v2"])

@v2.get("/users")
async def v2_users(page: int = 1, limit: int = 20):
    return {
        "version": 2,
        "users": [],
        "pagination": {"page": page, "limit": limit}
    }

@v2.get("/items/{item_id}")
async def v2_item(item_id: int, include_details: bool = False):
    result = {"version": 2, "id": item_id}
    if include_details:
        result["details"] = {}
    return result

app.include_router(v1)
app.include_router(v2)
```

### Header Versioning

```python
# Example 2: Header-based versioning
from fastapi import FastAPI, Header, HTTPException

app = FastAPI()

@app.get("/users")
async def list_users(x_api_version: str = Header("1")):
    """Version determined by header"""
    if x_api_version == "1":
        return {"version": 1, "users": []}
    elif x_api_version == "2":
        return {"version": 2, "users": [], "meta": {}}
    else:
        raise HTTPException(400, f"Unsupported version: {x_api_version}")
```

### Query Parameter Versioning

```python
# Example 3: Query parameter versioning
@app.get("/users")
async def list_users(version: int = 1):
    """Version as query parameter"""
    if version == 1:
        return {"users": []}
    elif version == 2:
        return {"users": [], "pagination": {}}
    else:
        raise HTTPException(400, f"Unsupported version: {version}")
```

## Deprecation Strategy

```python
# Example 4: API deprecation
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import warnings

app = FastAPI()

# Deprecated endpoint
@app.get("/api/v1/old-endpoint", deprecated=True)
async def old_endpoint():
    """
    Deprecated: Use /api/v2/new-endpoint instead.
    Will be removed in version 3.0.
    """
    return JSONResponse(
        content={"data": "old"},
        headers={
            "Deprecation": "true",
            "Sunset": "2024-12-31",
            "Link": '</api/v2/new-endpoint>; rel="successor-version"'
        }
    )

@app.middleware("http")
async def deprecation_middleware(request: Request, call_next):
    """Add deprecation headers"""
    response = await call_next(request)

    if "/v1/" in request.url.path:
        response.headers["Warning"] = '299 - "Deprecated API version"'

    return response
```

## Summary

API versioning enables safe evolution of APIs.

## Next Steps

Continue learning about:
- [API Documentation Tools](./08_api_documentation_tools.md)
- [API Testing Tools](./09_api_testing_tools.md)
