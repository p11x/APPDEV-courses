# Permission Decorators

## Overview

Permission decorators provide a clean way to enforce authorization on FastAPI endpoints.

## Implementation

```python
# Example 1: Permission decorators
from fastapi import FastAPI, Depends, HTTPException
from functools import wraps
from typing import List, Callable

app = FastAPI()

def require_permissions(*permissions: str):
    """Decorator requiring specific permissions"""
    def decorator(func: Callable):
        @wraps(func)
        async def wrapper(*args, user: dict = Depends(get_current_user), **kwargs):
            user_perms = set(user.get("permissions", []))
            required = set(permissions)

            if not required.issubset(user_perms):
                missing = required - user_perms
                raise HTTPException(
                    403,
                    f"Missing permissions: {', '.join(missing)}"
                )

            return await func(*args, user=user, **kwargs)
        return wrapper
    return decorator

def require_any_permission(*permissions: str):
    """Decorator requiring ANY of the specified permissions"""
    def decorator(func: Callable):
        @wraps(func)
        async def wrapper(*args, user: dict = Depends(get_current_user), **kwargs):
            user_perms = set(user.get("permissions", []))
            required = set(permissions)

            if not user_perms.intersection(required):
                raise HTTPException(
                    403,
                    f"Requires one of: {', '.join(required)}"
                )

            return await func(*args, user=user, **kwargs)
        return wrapper
    return decorator

def require_role(role: str):
    """Decorator requiring specific role"""
    def decorator(func: Callable):
        @wraps(func)
        async def wrapper(*args, user: dict = Depends(get_current_user), **kwargs):
            if user.get("role") != role:
                raise HTTPException(403, f"Requires {role} role")
            return await func(*args, user=user, **kwargs)
        return wrapper
    return decorator

@app.get("/items/")
@require_permissions("items:read")
async def list_items(user: dict = Depends(get_current_user)):
    return {"items": []}

@app.post("/items/")
@require_permissions("items:write")
async def create_item(user: dict = Depends(get_current_user)):
    return {"created": True}

@app.delete("/items/{id}")
@require_role("admin")
async def delete_item(id: int, user: dict = Depends(get_current_user)):
    return {"deleted": id}
```

## Summary

Permission decorators provide clean, declarative authorization for endpoints.

## Next Steps

Continue learning about:
- [Dynamic Permissions](./05_dynamic_permissions.md)
- [Resource Ownership](./06_resource_ownership.md)
