# Path Dependencies

## Overview

Path dependencies apply to all routes within a router or path prefix, enabling shared validation, authentication, or processing for route groups.

## Router-Level Dependencies

### Shared Dependencies on Routers

```python
# Example 1: Router with shared dependencies
from fastapi import FastAPI, APIRouter, Depends, HTTPException, Header

app = FastAPI()

def verify_api_key(x_api_key: str = Header(...)):
    """Dependency for API key verification"""
    if x_api_key != "valid-api-key":
        raise HTTPException(403, "Invalid API key")
    return x_api_key

def verify_admin(x_admin_key: str = Header(...)):
    """Dependency for admin verification"""
    if x_admin_key != "admin-key":
        raise HTTPException(403, "Admin access required")
    return x_admin_key

# Public routes (no auth required)
public_router = APIRouter(prefix="/public", tags=["public"])

@public_router.get("/info")
async def public_info():
    return {"message": "Public endpoint"}

# Protected routes (API key required)
protected_router = APIRouter(
    prefix="/api",
    tags=["api"],
    dependencies=[Depends(verify_api_key)]  # All routes require API key
)

@protected_router.get("/items")
async def list_items():
    return {"items": []}

@protected_router.get("/items/{item_id}")
async def get_item(item_id: int):
    return {"item_id": item_id}

# Admin routes (admin key required)
admin_router = APIRouter(
    prefix="/admin",
    tags=["admin"],
    dependencies=[Depends(verify_admin)]  # All routes require admin
)

@admin_router.get("/stats")
async def admin_stats():
    return {"users": 100, "items": 500}

@admin_router.post("/reset")
async def admin_reset():
    return {"message": "Reset complete"}

# Include routers
app.include_router(public_router)
app.include_router(protected_router)
app.include_router(admin_router)
```

## App-Level Dependencies

### Global Dependencies

```python
# Example 2: App-level dependencies
from fastapi import FastAPI, Depends, Request
from fastapi.responses import JSONResponse
import time

app = FastAPI()

async def log_request(request: Request):
    """Dependency that logs all requests"""
    start = time.time()
    print(f"➡️ {request.method} {request.url.path}")
    return start

async def add_timing(request: Request, start: float = Depends(log_request)):
    """Dependency that adds timing"""
    request.state.start_time = start

# Add dependencies at app level
app = FastAPI(dependencies=[Depends(log_request)])

@app.get("/items/")
async def list_items():
    return {"items": []}

@app.get("/users/")
async def list_users():
    return {"users": []}

# All routes get the dependency automatically
```

## Nested Path Dependencies

### Hierarchical Dependencies

```python
# Example 3: Nested path dependencies
from fastapi import FastAPI, APIRouter, Depends, Path

app = FastAPI()

# Level 1: Organization
def get_organization(org_id: int = Path(...)):
    """Get organization by ID"""
    if org_id > 100:
        raise HTTPException(404, "Organization not found")
    return {"org_id": org_id, "name": f"Org {org_id}"}

# Level 2: Project
def get_project(
    org: dict = Depends(get_organization),
    project_id: int = Path(...)
):
    """Get project within organization"""
    return {
        "project_id": project_id,
        "org_id": org["org_id"]
    }

# Level 3: Task
def get_task(
    project: dict = Depends(get_project),
    task_id: int = Path(...)
):
    """Get task within project"""
    return {
        "task_id": task_id,
        "project_id": project["project_id"]
    }

@app.get("/orgs/{org_id}/projects/{project_id}/tasks/{task_id}")
async def get_full_task(task: dict = Depends(get_task)):
    """
    Dependencies chain:
    get_organization → get_project → get_task
    """
    return task
```

## Best Practices

### Path Dependency Guidelines

```python
# Example 4: Best practices
from fastapi import FastAPI, APIRouter, Depends

app = FastAPI()

# Separate concerns
def authenticate():
    """Handle authentication"""
    pass

def authorize():
    """Handle authorization"""
    pass

def validate_tenant():
    """Handle tenant validation"""
    pass

# Group by concern
api_router = APIRouter(
    prefix="/api",
    dependencies=[Depends(authenticate)]  # All API routes require auth
)

admin_router = APIRouter(
    prefix="/admin",
    dependencies=[
        Depends(authenticate),
        Depends(authorize)  # Admin requires both auth and authorization
    ]
)

app.include_router(api_router)
app.include_router(admin_router)
```

## Summary

| Scope | Usage | Example |
|-------|-------|---------|
| App-level | `FastAPI(dependencies=[...])` | Global middleware |
| Router-level | `APIRouter(dependencies=[...])` | Group auth |
| Route-level | `Depends()` in params | Per-route deps |

## Next Steps

Continue learning about:
- [Sub Dependencies](./03_sub_dependencies.md) - Nested deps
- [Conditional Dependencies](./04_conditional_dependencies.md) - Dynamic deps
- [Dependency Testing](./05_dependency_testing.md) - Testing deps
