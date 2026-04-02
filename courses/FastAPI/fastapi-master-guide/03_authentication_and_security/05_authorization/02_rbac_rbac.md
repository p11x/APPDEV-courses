# Role-Based Access Control

## Overview

RBAC assigns permissions based on user roles, providing scalable authorization management.

## Implementation

### RBAC System

```python
# Example 1: Complete RBAC implementation
from fastapi import FastAPI, Depends, HTTPException
from enum import Enum
from typing import Set

app = FastAPI()

class Role(str, Enum):
    VIEWER = "viewer"
    EDITOR = "editor"
    ADMIN = "admin"
    SUPER_ADMIN = "super_admin"

ROLE_PERMISSIONS = {
    Role.VIEWER: {"read"},
    Role.EDITOR: {"read", "write"},
    Role.ADMIN: {"read", "write", "delete"},
    Role.SUPER_ADMIN: {"read", "write", "delete", "manage_users"}
}

user_roles: dict[int, Role] = {
    1: Role.VIEWER,
    2: Role.EDITOR,
    3: Role.ADMIN
}

def get_user_permissions(user_id: int) -> Set[str]:
    """Get permissions for user role"""
    role = user_roles.get(user_id, Role.VIEWER)
    return ROLE_PERMISSIONS.get(role, set())

def require_role(min_role: Role):
    """Require minimum role level"""
    role_order = [Role.VIEWER, Role.EDITOR, Role.ADMIN, Role.SUPER_ADMIN]
    
    async def checker(user: dict = Depends(get_current_user)):
        user_role = user_roles.get(user["id"], Role.VIEWER)
        if role_order.index(user_role) < role_order.index(min_role):
            raise HTTPException(403, "Insufficient role")
        return user
    
    return checker

def require_permission(permission: str):
    """Require specific permission"""
    async def checker(user: dict = Depends(get_current_user)):
        perms = get_user_permissions(user["id"])
        if permission not in perms:
            raise HTTPException(403, f"Missing permission: {permission}")
        return user
    
    return checker

@app.get("/items/", dependencies=[Depends(require_permission("read"))])
async def list_items():
    return {"items": []}

@app.post("/items/", dependencies=[Depends(require_role(Role.EDITOR))])
async def create_item():
    return {"created": True}

@app.delete("/items/{id}", dependencies=[Depends(require_role(Role.ADMIN))])
async def delete_item(id: int):
    return {"deleted": id}
```

## Summary

RBAC provides scalable authorization by grouping permissions into roles.

## Next Steps

Continue learning about:
- [ABAC](./03_abac_attribute_based.md)
- [Permission Decorators](./04_permission_decorators.md)
