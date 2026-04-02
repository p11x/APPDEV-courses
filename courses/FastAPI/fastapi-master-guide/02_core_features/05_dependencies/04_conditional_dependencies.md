# Conditional Dependencies

## Overview

Conditional dependencies execute based on runtime conditions, enabling flexible authentication, feature flags, and environment-specific behavior.

## Feature Flags

### Feature-Gated Dependencies

```python
# Example 1: Feature flag dependencies
from fastapi import FastAPI, Depends, HTTPException

app = FastAPI()

# Feature flags
FEATURES = {
    "new_checkout": True,
    "beta_features": False,
    "premium_only": True
}

def require_feature(feature: str):
    """Dependency factory for feature flags"""
    def checker():
        if not FEATURES.get(feature):
            raise HTTPException(
                status_code=404,
                detail=f"Feature '{feature}' not available"
            )
        return True
    return checker

@app.get("/checkout/new", dependencies=[Depends(require_feature("new_checkout"))])
async def new_checkout():
    """Only available if new_checkout is enabled"""
    return {"checkout": "new version"}

@app.get("/beta/feature", dependencies=[Depends(require_feature("beta_features"))])
async def beta_feature():
    """Only available if beta_features is enabled"""
    return {"feature": "beta"}
```

## Conditional Authentication

### Optional Authentication

```python
# Example 2: Optional authentication
from fastapi import FastAPI, Depends, Header
from typing import Optional

app = FastAPI()

def get_optional_user(authorization: Optional[str] = Header(None)):
    """Optional authentication dependency"""
    if not authorization:
        return None

    if authorization.startswith("Bearer "):
        token = authorization.split(" ")[1]
        if token == "valid-token":
            return {"user_id": 1, "username": "john"}

    return None

def require_auth(user: Optional[dict] = Depends(get_optional_user)):
    """Require authentication"""
    if not user:
        from fastapi import HTTPException
        raise HTTPException(401, "Authentication required")
    return user

@app.get("/public/")
async def public_endpoint(user: Optional[dict] = Depends(get_optional_user)):
    """Public endpoint, enhanced if authenticated"""
    if user:
        return {"message": f"Hello, {user['username']}"}
    return {"message": "Hello, Guest"}

@app.get("/protected/")
async def protected_endpoint(user: dict = Depends(require_auth)):
    """Protected endpoint, requires auth"""
    return {"user": user}
```

## Role-Based Dependencies

### Role Checking

```python
# Example 3: Role-based access
from fastapi import FastAPI, Depends, HTTPException
from typing import List

app = FastAPI()

def get_current_user():
    """Simulated current user"""
    return {"user_id": 1, "role": "admin"}

def require_roles(allowed_roles: List[str]):
    """Dependency factory for role checking"""
    def checker(user: dict = Depends(get_current_user)):
        if user["role"] not in allowed_roles:
            raise HTTPException(
                status_code=403,
                detail=f"Role '{user['role']}' not authorized"
            )
        return user
    return checker

@app.get("/admin/", dependencies=[Depends(require_roles(["admin"]))])
async def admin_only():
    return {"message": "Admin access"}

@app.get("/editor/", dependencies=[Depends(require_roles(["admin", "editor"]))])
async def editor_access():
    return {"message": "Editor access"}

@app.get("/user/", dependencies=[Depends(require_roles(["admin", "editor", "user"]))])
async def user_access():
    return {"message": "User access"}
```

## Environment-Based Dependencies

### Environment Configuration

```python
# Example 4: Environment-specific dependencies
from fastapi import FastAPI, Depends
import os

app = FastAPI()

ENV = os.getenv("ENVIRONMENT", "development")

def get_db_config():
    """Database config based on environment"""
    if ENV == "production":
        return {"url": "postgresql://prod-server/db", "pool_size": 20}
    elif ENV == "staging":
        return {"url": "postgresql://staging-server/db", "pool_size": 10}
    else:
        return {"url": "sqlite:///./dev.db", "pool_size": 5}

@app.get("/db-info/")
async def db_info(config: dict = Depends(get_db_config)):
    return {"environment": ENV, "config": config}
```

## Best Practices

### Conditional Dep Guidelines

```python
# Example 5: Best practices
from fastapi import FastAPI, Depends

app = FastAPI()

# GOOD: Use factories for reusable conditions
def require_permission(permission: str):
    def check(user):
        if permission not in user.get("permissions", []):
            raise HTTPException(403, "Permission denied")
        return user
    return lambda user=Depends(get_current_user): check(user)

# GOOD: Keep conditions simple and testable
def is_feature_enabled(feature: str) -> bool:
    return FEATURES.get(feature, False)
```

## Summary

| Pattern | Use Case | Example |
|---------|----------|---------|
| Feature flags | Gradual rollout | `require_feature("new_ui")` |
| Optional auth | Public + enhanced | `get_optional_user()` |
| Role-based | Access control | `require_roles(["admin"])` |
| Environment | Config by env | `get_db_config()` |

## Next Steps

Continue learning about:
- [Dependency Testing](./05_dependency_testing.md) - Testing deps
- [Middleware](../06_middleware/01_middleware_overview.md) - Request processing
- [CORS Middleware](../06_middleware/02_cors_middleware.md) - Cross-origin
