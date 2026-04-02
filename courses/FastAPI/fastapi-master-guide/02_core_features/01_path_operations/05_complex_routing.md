# Complex Routing

## Overview

Complex routing patterns handle real-world API requirements including nested resources, versioning, content negotiation, and conditional routing. This guide covers advanced routing techniques.

## Nested Resources

### Hierarchical Routes

```python
# Example 1: Deeply nested resources
from fastapi import FastAPI, APIRouter, Path
from uuid import UUID

app = FastAPI()

# Organization hierarchy
@app.get("/organizations/{org_id}/departments/{dept_id}/employees/{emp_id}")
async def get_employee(
    org_id: UUID = Path(..., description="Organization ID"),
    dept_id: int = Path(..., ge=1, description="Department ID"),
    emp_id: int = Path(..., ge=1, description="Employee ID")
):
    """
    Deep nesting for clear resource hierarchy.
    URL: /organizations/550e8400-.../departments/5/employees/42
    """
    return {
        "organization_id": str(org_id),
        "department_id": dept_id,
        "employee_id": emp_id
    }

# Social media hierarchy
@app.get("/users/{user_id}/posts/{post_id}/comments/{comment_id}")
async def get_comment(
    user_id: int,
    post_id: int,
    comment_id: int
):
    """Social media resource nesting"""
    return {
        "user_id": user_id,
        "post_id": post_id,
        "comment_id": comment_id
    }

# E-commerce hierarchy
@app.get("/stores/{store_id}/categories/{cat_id}/products/{product_id}")
async def get_store_product(
    store_id: int,
    cat_id: int,
    product_id: int
):
    """E-commerce resource hierarchy"""
    return {
        "store_id": store_id,
        "category_id": cat_id,
        "product_id": product_id
    }
```

### Router-Based Nesting

```python
# Example 2: Router-based resource nesting
from fastapi import FastAPI, APIRouter, Depends

app = FastAPI()

# Users router
users_router = APIRouter(prefix="/users", tags=["users"])

@users_router.get("/")
async def list_users():
    return {"users": []}

@users_router.get("/{user_id}")
async def get_user(user_id: int):
    return {"user_id": user_id}

# Posts router (nested under users)
posts_router = APIRouter(prefix="/{user_id}/posts", tags=["posts"])

@posts_router.get("/")
async def list_user_posts(user_id: int):
    return {"user_id": user_id, "posts": []}

@posts_router.get("/{post_id}")
async def get_user_post(user_id: int, post_id: int):
    return {"user_id": user_id, "post_id": post_id}

# Comments router (nested under posts)
comments_router = APIRouter(prefix="/{post_id}/comments", tags=["comments"])

@comments_router.get("/")
async def list_post_comments(user_id: int, post_id: int):
    return {"user_id": user_id, "post_id": post_id, "comments": []}

# Build nested structure
posts_router.include_router(comments_router)
users_router.include_router(posts_router)
app.include_router(users_router)

# Resulting routes:
# GET /users/
# GET /users/{user_id}
# GET /users/{user_id}/posts/
# GET /users/{user_id}/posts/{post_id}
# GET /users/{user_id}/posts/{post_id}/comments/
```

## API Versioning

### URL Path Versioning

```python
# Example 3: URL-based versioning
from fastapi import FastAPI, APIRouter

app = FastAPI()

# Version 1
v1 = APIRouter(prefix="/api/v1", tags=["v1"])

@v1.get("/items")
async def v1_list_items():
    """V1: Simple list response"""
    return {"items": ["item1", "item2"]}

@v1.get("/items/{item_id}")
async def v1_get_item(item_id: int):
    """V1: Basic item retrieval"""
    return {"item_id": item_id, "version": 1}

# Version 2 with enhanced features
v2 = APIRouter(prefix="/api/v2", tags=["v2"])

@v2.get("/items")
async def v2_list_items(skip: int = 0, limit: int = 10):
    """V2: Enhanced with pagination"""
    return {
        "items": [],
        "pagination": {"skip": skip, "limit": limit}
    }

@v2.get("/items/{item_id}")
async def v2_get_item(item_id: int, include_details: bool = False):
    """V2: Enhanced with optional details"""
    result = {"item_id": item_id, "version": 2}
    if include_details:
        result["details"] = {"created": "2024-01-01"}
    return result

# Include both versions
app.include_router(v1)
app.include_router(v2)
```

### Header-Based Versioning

```python
# Example 4: Header-based versioning
from fastapi import FastAPI, Header, HTTPException
from typing import Optional

app = FastAPI()

def get_api_version(x_api_version: str = Header("1")):
    """Dependency for version extraction"""
    if x_api_version not in ["1", "2"]:
        raise HTTPException(400, f"Unsupported version: {x_api_version}")
    return x_api_version

@app.get("/items")
async def list_items(version: str = Depends(get_api_version)):
    """
    Version determined by X-API-Version header.
    Default: version 1
    """
    if version == "1":
        return {"version": 1, "items": ["v1_item"]}
    else:
        return {"version": 2, "items": [{"id": 1, "name": "v2_item"}]}
```

## Route Groups and Tags

### Tag-Based Organization

```python
# Example 5: Route organization with tags
from fastapi import FastAPI, APIRouter

app = FastAPI()

# Define tag metadata
app.openapi_tags = [
    {"name": "users", "description": "User management operations"},
    {"name": "items", "description": "Item CRUD operations"},
    {"name": "orders", "description": "Order processing"},
    {"name": "admin", "description": "Administrative endpoints"}
]

# Users router
users = APIRouter(prefix="/users", tags=["users"])

@users.get("/")
async def list_users():
    return {"users": []}

@users.post("/")
async def create_user():
    return {"message": "User created"}

# Items router
items = APIRouter(prefix="/items", tags=["items"])

@items.get("/")
async def list_items():
    return {"items": []}

@items.get("/{item_id}")
async def get_item(item_id: int):
    return {"item_id": item_id}

# Admin router
admin = APIRouter(
    prefix="/admin",
    tags=["admin"],
    dependencies=[]  # Add auth dependency here
)

@admin.get("/stats")
async def admin_stats():
    return {"users": 100, "items": 500}

# Include all routers
app.include_router(users)
app.include_router(items)
app.include_router(admin)
```

## Conditional Routing

### Feature Flags

```python
# Example 6: Conditional routing
from fastapi import FastAPI, HTTPException, Depends
from typing import Optional

app = FastAPI()

# Feature flags
FEATURE_FLAGS = {
    "new_checkout": True,
    "beta_features": False,
    "maintenance_mode": False
}

def require_feature(feature: str):
    """Dependency to check feature flags"""
    def checker():
        if not FEATURE_FLAGS.get(feature):
            raise HTTPException(404, "Feature not available")
        return True
    return checker

@app.get("/checkout/new")
async def new_checkout(
    _: bool = Depends(require_feature("new_checkout"))
):
    """Only available if new_checkout feature is enabled"""
    return {"checkout": "new version"}

@app.get("/beta/feature")
async def beta_feature(
    _: bool = Depends(require_feature("beta_features"))
):
    """Only available if beta_features is enabled"""
    return {"feature": "beta"}
```

### Environment-Based Routing

```python
# Example 7: Environment-specific routes
from fastapi import FastAPI, APIRouter
import os

app = FastAPI()

ENV = os.getenv("ENVIRONMENT", "development")

# Development-only routes
if ENV == "development":
    dev_router = APIRouter(prefix="/dev", tags=["development"])

    @dev_router.get("/debug")
    async def debug_info():
        return {"environment": "development", "debug": True}

    @dev_router.post("/reset-db")
    async def reset_database():
        return {"message": "Database reset"}

    app.include_router(dev_router)

# Production routes always included
@app.get("/health")
async def health_check():
    return {"status": "healthy", "environment": ENV}
```

## Middleware-Based Routing

### Request Routing

```python
# Example 8: Middleware-based routing
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

app = FastAPI()

@app.middleware("http")
async def route_by_host(request: Request, call_next):
    """
    Route based on request host.
    Useful for multi-tenant applications.
    """
    host = request.headers.get("host", "")

    if "api.example.com" in host:
        # Add prefix for API subdomain
        request.scope["path"] = "/api" + request.url.path
    elif "admin.example.com" in host:
        # Add prefix for admin subdomain
        request.scope["path"] = "/admin" + request.url.path

    return await call_next(request)

@app.get("/api/items")
async def api_items():
    return {"source": "api"}

@app.get("/admin/items")
async def admin_items():
    return {"source": "admin"}
```

## Catch-All Routes

### Wildcard Routes

```python
# Example 9: Catch-all and wildcard routes
from fastapi import FastAPI

app = FastAPI()

# Specific routes first
@app.get("/items/search")
async def search_items():
    return {"action": "search"}

@app.get("/items/featured")
async def featured_items():
    return {"action": "featured"}

# Catch-all comes after specific routes
@app.get("/items/{item_id}")
async def get_item(item_id: int):
    return {"item_id": item_id}

# Path catch-all for file serving
@app.get("/files/{file_path:path}")
async def serve_file(file_path: str):
    """
    :path matches everything including slashes.
    URL: /files/docs/api/v1/readme.md
    """
    return {"file_path": file_path}

# SPA fallback route
@app.get("/{full_path:path}")
async def spa_fallback(full_path: str):
    """
    Catch-all for Single Page Applications.
    Serves index.html for all non-API routes.
    """
    if full_path.startswith("api/"):
        return {"error": "API route not found"}
    return {"message": "Serve SPA index.html", "path": full_path}
```

## Route Precedence

### Understanding Route Matching

```python
# Example 10: Route precedence rules
from fastapi import FastAPI

app = FastAPI()

# Route precedence (from highest to lowest):
# 1. Exact paths
# 2. Path parameters
# 3. Path converters

# Exact path (highest priority)
@app.get("/items/featured")
async def featured_items():
    """Matches /items/featured exactly"""
    return {"type": "featured"}

# Literal path
@app.get("/items/new")
async def new_items():
    """Matches /items/new exactly"""
    return {"type": "new"}

# Path parameter (lower priority)
@app.get("/items/{item_id}")
async def get_item(item_id: str):
    """Matches /items/anything-else"""
    return {"item_id": item_id}

# Path converter (lowest priority)
@app.get("/items/{path:path}")
async def catch_all(path: str):
    """Catches remaining paths"""
    return {"path": path}

# Order matters!
# If /items/{item_id} is defined before /items/featured,
# "featured" would be captured as item_id
```

## Webhook Routing

### Webhook Endpoints

```python
# Example 11: Webhook routing patterns
from fastapi import FastAPI, Request, Header, HTTPException
import hmac
import hashlib

app = FastAPI()

WEBHOOK_SECRET = "your-webhook-secret"

def verify_signature(payload: bytes, signature: str) -> bool:
    """Verify webhook signature"""
    expected = hmac.new(
        WEBHOOK_SECRET.encode(),
        payload,
        hashlib.sha256
    ).hexdigest()
    return hmac.compare_digest(f"sha256={expected}", signature)

@app.post("/webhooks/github")
async def github_webhook(
    request: Request,
    x_hub_signature_256: str = Header(...)
):
    """
    GitHub webhook handler.
    Verifies signature before processing.
    """
    body = await request.body()

    if not verify_signature(body, x_hub_signature_256):
        raise HTTPException(403, "Invalid signature")

    payload = await request.json()
    event = request.headers.get("X-GitHub-Event", "unknown")

    return {"event": event, "processed": True}

@app.post("/webhooks/stripe")
async def stripe_webhook(request: Request):
    """Stripe webhook handler"""
    payload = await request.json()
    event_type = payload.get("type")

    return {"type": event_type, "processed": True}
```

## Performance Considerations

### Route Performance

```python
# Example 12: Performance-optimized routing
from fastapi import FastAPI
from fastapi.responses import ORJSONResponse

app = FastAPI()

# Use ORJSON for faster serialization
@app.get("/fast/items/", response_class=ORJSONResponse)
async def fast_items():
    """ORJSON is 2-3x faster than standard JSON"""
    return {"items": list(range(1000))}

# Avoid deep nesting when possible
# Instead of: /users/{id}/orders/{id}/items/{id}
# Consider: /order-items?user_id=X&order_id=Y

@app.get("/order-items/")
async def get_order_items(user_id: int, order_id: int):
    """
    Flatter URL structure is faster to match
    and easier to understand.
    """
    return {"user_id": user_id, "order_id": order_id}
```

## Summary

| Pattern | Use Case | Example |
|---------|----------|---------|
| Nested Resources | Clear hierarchies | `/users/{id}/posts/{id}` |
| API Versioning | Multiple API versions | `/api/v1/items` |
| Route Groups | Logical organization | `tags=["users"]` |
| Conditional Routes | Feature flags | `Depends(require_feature)` |
| Catch-All | SPA fallback | `/{path:path}` |

## Next Steps

Continue learning about:
- [Data Models](../02_data_models/01_pydantic_basics.md) - Pydantic models
- [Request Body](../03_request_body/01_basic_request_body.md) - JSON handling
- [Dependencies](../05_dependencies/01_dependency_injection_basics.md) - DI patterns
