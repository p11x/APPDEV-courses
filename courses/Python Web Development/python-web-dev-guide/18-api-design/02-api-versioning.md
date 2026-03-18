# API Versioning

## What You'll Learn
- Why API versioning matters
- Versioning strategies
- Implementing versioning in FastAPI

## Prerequisites
- Completed RESTful API design

## Why Version?

API versioning allows:
- Backward compatibility
- Adding new features
- Deprecating old endpoints

## Versioning Strategies

| Strategy | URL Example | Pros |
|----------|-------------|------|
| URL Path | /api/v1/users | Clear, simple |
| Query Param | /api/users?version=1 | Less URL clutter |
| Header | Accept: version=1 | Clean URLs |

## URL Path Versioning (Recommended)

```python
from fastapi import FastAPI, APIRouter
from pydantic import BaseModel

app = FastAPI()

# V1 Router
v1_router = APIRouter(prefix="/v1")

class UserV1(BaseModel):
    id: int
    name: str

@v1_router.get("/users")
async def get_users_v1():
    """Get users - V1 response"""
    return [
        {"id": 1, "name": "John"},
        {"id": 2, "name": "Jane"}
    ]

# V2 Router
v2_router = APIRouter(prefix="/v2")

class UserV2(BaseModel):
    id: int
    name: str
    email: str
    created_at: str

@v2_router.get("/users")
async def get_users_v2():
    """Get users - V2 response with more fields"""
    return [
        {"id": 1, "name": "John", "email": "john@example.com", "created_at": "2024-01-01"},
        {"id": 2, "name": "Jane", "email": "jane@example.com", "created_at": "2024-01-02"}
    ]

# Include routers
app.include_router(v1_router, prefix="/api")
app.include_router(v2_router, prefix="/api")
```

## Header Versioning

```python
from fastapi import FastAPI, Header, HTTPException

app = FastAPI()

@app.get("/users")
async def get_users(version: str = Header(None, alias="X-API-Version")):
    if version == "1":
        return {"version": "1", "users": [{"id": 1, "name": "John"}]}
    elif version == "2":
        return {"version": "2", "users": [{"id": 1, "name": "John", "email": "john@example.com"}]}
    else:
        # Default to latest
        return {"version": "2", "users": [{"id": 1, "name": "John", "email": "john@example.com"}]}
```

## Versioning with Deprecation

```python
from fastapi import FastAPI, APIRouter
from datetime import datetime

app = FastAPI()

# V1 - Deprecated
v1 = APIRouter(prefix="/v1")

@v1.get("/users")
async def get_users_v1():
    """⚠️ DEPRECATED: Use /v2/users instead"""
    return {
        "deprecated": True,
        "message": "This endpoint will be removed on 2025-01-01",
        "users": [{"id": 1, "name": "John"}]
    }

# V2 - Current
v2 = APIRouter(prefix="/v2")

@v2.get("/users")
async def get_users_v2():
    return {"users": [{"id": 1, "name": "John", "email": "john@example.com"}]}

app.include_router(v1)
app.include_router(v2)

@app.get("/info")
async def api_info():
    return {
        "versions": {
            "v1": {"status": "deprecated", "sunset": "2025-01-01"},
            "v2": {"status": "current"}
        }
    }
```

## Summary
- Version APIs from the start
- Use URL path versioning
- Deprecate gracefully with notice

## Next Steps
→ Continue to `03-error-handling.md`
