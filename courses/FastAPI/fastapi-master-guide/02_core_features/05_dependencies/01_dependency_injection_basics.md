# Dependency Injection Basics

## Overview

Dependency injection (DI) in FastAPI enables code reuse, separation of concerns, and easier testing. Dependencies are functions or classes that provide resources to route handlers.

## Simple Dependencies

### Function Dependencies

```python
# Example 1: Basic dependency function
from fastapi import FastAPI, Depends

app = FastAPI()

def get_query_params(
    skip: int = 0,
    limit: int = 10
):
    """Dependency that extracts pagination parameters"""
    return {"skip": skip, "limit": limit}

@app.get("/items/")
async def list_items(
    # Dependency is injected and called before handler
    params: dict = Depends(get_query_params)
):
    """
    Depends() tells FastAPI to call get_query_params first.
    Result is passed as 'params' argument.
    """
    return {
        "items": [],
        "pagination": params
    }

# Multiple endpoints can reuse the same dependency
@app.get("/users/")
async def list_users(
    params: dict = Depends(get_query_params)
):
    """Same dependency, different endpoint"""
    return {
        "users": [],
        "pagination": params
    }
```

### Class Dependencies

```python
# Example 2: Class-based dependencies
from fastapi import FastAPI, Depends

app = FastAPI()

class Pagination:
    """Dependency class for pagination"""
    def __init__(
        self,
        skip: int = 0,
        limit: int = 10
    ):
        self.skip = skip
        self.limit = min(limit, 100)  # Max 100 items

@app.get("/items/")
async def list_items(
    # Class is instantiated with query params
    pagination: Pagination = Depends()
):
    """
    FastAPI creates Pagination instance with query params.
    """
    return {
        "items": [],
        "skip": pagination.skip,
        "limit": pagination.limit
    }
```

## Common Dependency Patterns

### Database Session

```python
# Example 3: Database session dependency
from fastapi import FastAPI, Depends
from typing import Generator

app = FastAPI()

# Simulated database session
class DatabaseSession:
    def __init__(self):
        self.connected = True

    def close(self):
        self.connected = False

    def query(self, model: str):
        return [{"id": 1, "name": "Item"}]

def get_db() -> Generator[DatabaseSession, None, None]:
    """
    Database session dependency.
    Session is created, yielded, then closed.
    """
    db = DatabaseSession()
    try:
        yield db
    finally:
        db.close()

@app.get("/items/")
async def list_items(db: DatabaseSession = Depends(get_db)):
    """Database session is injected"""
    items = db.query("Item")
    return {"items": items}
```

### Authentication

```python
# Example 4: Authentication dependency
from fastapi import FastAPI, Depends, HTTPException, Header
from typing import Optional

app = FastAPI()

def get_current_user(
    authorization: Optional[str] = Header(None)
) -> dict:
    """
    Authentication dependency.
    Validates token and returns user.
    """
    if not authorization:
        raise HTTPException(
            status_code=401,
            detail="Authorization header required"
        )

    if not authorization.startswith("Bearer "):
        raise HTTPException(
            status_code=401,
            detail="Invalid authorization format"
        )

    token = authorization.split(" ")[1]

    # Validate token (simplified)
    if token != "valid-token":
        raise HTTPException(
            status_code=401,
            detail="Invalid token"
        )

    return {"user_id": 1, "username": "john"}

@app.get("/profile/")
async def get_profile(
    user: dict = Depends(get_current_user)
):
    """Protected endpoint"""
    return {"user": user}

@app.get("/settings/")
async def get_settings(
    user: dict = Depends(get_current_user)
):
    """Another protected endpoint"""
    return {"user": user, "settings": {}}
```

## Multiple Dependencies

### Combining Dependencies

```python
# Example 5: Multiple dependencies
from fastapi import FastAPI, Depends, Header
from typing import Optional

app = FastAPI()

def get_pagination(skip: int = 0, limit: int = 10):
    return {"skip": skip, "limit": limit}

def get_sort_params(
    sort_by: str = "id",
    order: str = "asc"
):
    return {"sort_by": sort_by, "order": order}

def get_api_key(x_api_key: str = Header(...)):
    if x_api_key != "valid-key":
        raise HTTPException(403, "Invalid API key")
    return x_api_key

@app.get("/items/")
async def list_items(
    # Multiple dependencies
    pagination: dict = Depends(get_pagination),
    sort: dict = Depends(get_sort_params),
    api_key: str = Depends(get_api_key)
):
    """
    Dependencies are resolved in order.
    All must succeed for handler to execute.
    """
    return {
        "items": [],
        "pagination": pagination,
        "sort": sort
    }
```

## Dependency Scopes

### Singleton vs Request Scope

```python
# Example 6: Dependency caching
from fastapi import FastAPI, Depends
import time

app = FastAPI()

call_count = 0

def get_expensive_resource():
    """
    Expensive dependency.
    Use use_cache=True (default) to avoid re-calling.
    """
    global call_count
    call_count += 1
    time.sleep(0.1)  # Simulate expensive operation
    return {"call_number": call_count}

@app.get("/items/")
async def get_items(
    # Same instance used within single request
    resource: dict = Depends(get_expensive_resource)
):
    return {"items": [], "resource": call_count}

@app.get("/items/{item_id}")
async def get_item(
    item_id: int,
    # use_cache=False forces new instance
    resource: dict = Depends(get_expensive_resource)
):
    return {"item_id": item_id, "resource": call_count}
```

## Best Practices

### DI Guidelines

```python
# Example 7: Best practices
from fastapi import FastAPI, Depends
from typing import Optional

app = FastAPI()

# GOOD: Small, focused dependencies
def get_db_session():
    """Database session - one responsibility"""
    pass

def get_current_user():
    """Authentication - one responsibility"""
    pass

def get_settings():
    """Configuration - one responsibility"""
    pass

# GOOD: Use dependencies for common patterns
@app.get("/items/")
async def list_items(
    db = Depends(get_db_session),
    user = Depends(get_current_user),
    settings = Depends(get_settings)
):
    """
    Benefits:
    1. Code reuse
    2. Separation of concerns
    3. Easy testing
    4. Clear dependencies
    """
    return {"items": []}
```

## Summary

| Feature | Usage | Example |
|---------|-------|---------|
| Function DI | `Depends(func)` | `params = Depends(get_params)` |
| Class DI | `Depends(Class)` | `pag = Depends(Pagination)` |
| Generator DI | Yield resources | `get_db()` with cleanup |
| Multiple DI | Combine deps | Multiple `Depends()` calls |

## Next Steps

Continue learning about:
- [Path Dependencies](./02_path_dependencies.md) - Route-level deps
- [Sub Dependencies](./03_sub_dependencies.md) - Nested deps
- [Conditional Dependencies](./04_conditional_dependencies.md) - Dynamic deps
