# Permissions System

## Overview

A permissions system controls what actions users can perform. This guide covers implementing flexible permissions in FastAPI.

## Basic Permissions

### Permission Definition

```python
# Example 1: Basic permissions system
from fastapi import FastAPI, Depends, HTTPException
from enum import Enum
from typing import Set

app = FastAPI()

class Permission(str, Enum):
    """Application permissions"""
    READ = "read"
    WRITE = "write"
    DELETE = "delete"
    ADMIN = "admin"
    MANAGE_USERS = "manage_users"
    MANAGE_ROLES = "manage_roles"

# User permissions storage
user_permissions: dict[int, Set[Permission]] = {
    1: {Permission.READ},
    2: {Permission.READ, Permission.WRITE},
    3: {Permission.READ, Permission.WRITE, Permission.DELETE, Permission.ADMIN},
}

def get_user_permissions(user_id: int) -> Set[Permission]:
    """Get permissions for user"""
    return user_permissions.get(user_id, set())

def require_permission(permission: Permission):
    """
    Dependency to check permission.
    """
    async def checker(user: dict = Depends(get_current_user)):
        perms = get_user_permissions(user["id"])

        if permission not in perms:
            raise HTTPException(
                status_code=403,
                detail=f"Permission denied: {permission.value}"
            )

        return user

    return checker

@app.get("/items/", dependencies=[Depends(require_permission(Permission.READ))])
async def list_items():
    return {"items": []}

@app.post("/items/", dependencies=[Depends(require_permission(Permission.WRITE))])
async def create_item():
    return {"created": True}

@app.delete("/items/{id}", dependencies=[Depends(require_permission(Permission.DELETE))])
async def delete_item(id: int):
    return {"deleted": id}
```

## Permission Checking

### Multiple Permission Checks

```python
# Example 2: Advanced permission checking
from fastapi import FastAPI, Depends, HTTPException
from typing import List

app = FastAPI()

def require_any_permission(*permissions: Permission):
    """
    Require ANY of the specified permissions.
    """
    async def checker(user: dict = Depends(get_current_user)):
        user_perms = get_user_permissions(user["id"])

        if not any(p in user_perms for p in permissions):
            raise HTTPException(
                status_code=403,
                detail=f"Requires one of: {[p.value for p in permissions]}"
            )

        return user

    return checker

def require_all_permissions(*permissions: Permission):
    """
    Require ALL of the specified permissions.
    """
    async def checker(user: dict = Depends(get_current_user)):
        user_perms = get_user_permissions(user["id"])

        missing = [p for p in permissions if p not in user_perms]
        if missing:
            raise HTTPException(
                status_code=403,
                detail=f"Missing permissions: {[p.value for p in missing]}"
            )

        return user

    return checker

@app.get("/items/",
    dependencies=[Depends(require_any_permission(Permission.READ, Permission.ADMIN))]
)
async def list_items():
    """Requires read OR admin permission"""
    return {"items": []}

@app.post("/items/",
    dependencies=[Depends(require_all_permissions(Permission.WRITE, Permission.ADMIN))]
)
async def create_item():
    """Requires write AND admin permission"""
    return {"created": True}
```

## Best Practices

### Permission Guidelines

```python
# Example 3: Permission best practices
"""
Permission System Best Practices:

1. Principle of Least Privilege
   - Grant minimum permissions needed

2. Clear Permission Names
   - Use descriptive, consistent naming

3. Permission Groups
   - Group related permissions

4. Audit Logging
   - Log permission checks and denials

5. Regular Review
   - Review and update permissions periodically
"""

from fastapi import FastAPI

app = FastAPI()

# Well-structured permissions
PERMISSIONS = {
    "items:read": "View items",
    "items:write": "Create/edit items",
    "items:delete": "Delete items",
    "users:read": "View users",
    "users:write": "Manage users",
    "admin:access": "Admin panel access",
}
```

## Summary

| Feature | Implementation | Use Case |
|---------|----------------|----------|
| Single permission | `require_permission(P)` | Specific access |
| Any permission | `require_any_permission(P1, P2)` | Alternative access |
| All permissions | `require_all_permissions(P1, P2)` | Combined access |

## Next Steps

Continue learning about:
- [RBAC](./02_rbac_rbac.md) - Role-based access
- [ABAC](./03_abac_attribute_based.md) - Attribute-based access
- [Resource Ownership](./06_resource_ownership.md) - Ownership checks
