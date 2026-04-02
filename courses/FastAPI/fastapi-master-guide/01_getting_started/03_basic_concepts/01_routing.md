# Routing in FastAPI

## Overview

Routing is the mechanism that maps URLs to Python functions. In FastAPI, routes define how your API responds to different HTTP requests at various endpoints. Understanding routing is fundamental to building any API.

## Basic Route Declaration

### HTTP Method Decorators

FastAPI provides decorators for each HTTP method:

```python
# Example 1: Basic HTTP method decorators
from fastapi import FastAPI

app = FastAPI()

# GET request - Retrieve data
# Used for reading/fetching resources
@app.get("/items")
async def get_items():
    """
    GET requests should be:
    - Safe: Don't modify server state
    - Idempotent: Same request always returns same result
    - Cacheable: Responses can be cached
    """
    return {"items": ["item1", "item2", "item3"]}

# POST request - Create new resource
# Used for creating new data
@app.post("/items")
async def create_item(name: str, price: float):
    """
    POST requests:
    - Create new resources
    - Not idempotent (same request creates multiple resources)
    - Request body contains data
    """
    return {"message": "Item created", "name": name, "price": price}

# PUT request - Replace entire resource
# Used for complete updates
@app.put("/items/{item_id}")
async def replace_item(item_id: int, name: str, price: float):
    """
    PUT requests:
    - Replace entire resource
    - Idempotent (same request produces same result)
    - Client provides complete resource representation
    """
    return {"item_id": item_id, "name": name, "price": price}

# PATCH request - Partial update
# Used for partial modifications
@app.patch("/items/{item_id}")
async def update_item(item_id: int, name: str | None = None):
    """
    PATCH requests:
    - Partial updates to resources
    - Only modified fields needed
    - Not always idempotent
    """
    return {"item_id": item_id, "updated_name": name}

# DELETE request - Remove resource
# Used for deleting data
@app.delete("/items/{item_id}")
async def delete_item(item_id: int):
    """
    DELETE requests:
    - Remove resources
    - Idempotent (deleting twice is same as once)
    - Often returns 204 No Content
    """
    return {"message": f"Item {item_id} deleted"}

# HEAD request - Get headers only
# Same as GET but without response body
@app.head("/items")
async def head_items():
    """
    HEAD requests:
    - Check if resource exists
    - Get metadata without body
    - Useful for checking before download
    """
    # FastAPI handles HEAD automatically for GET routes
    pass

# OPTIONS request - Get allowed methods
# Describes communication options
@app.options("/items")
async def options_items():
    """
    OPTIONS requests:
    - Discover allowed HTTP methods
    - CORS preflight requests
    - API capability discovery
    """
    return {"allowed_methods": ["GET", "POST", "PUT", "DELETE"]}
```

### Route with Multiple Methods

```python
# Example 2: Route accepting multiple HTTP methods
from fastapi import FastAPI

app = FastAPI()

# Single route accepting both GET and POST
@app.api_route("/items", methods=["GET", "POST"])
async def handle_items():
    """
    Sometimes a single endpoint needs to handle
    multiple HTTP methods differently.
    """
    # Note: This approach loses type hints per method
    # Better to use separate decorators in most cases
    return {"message": "Multi-method endpoint"}
```

## Path Parameters

### Basic Path Parameters

```python
# Example 3: Path parameters for dynamic URLs
from fastapi import FastAPI, Path

app = FastAPI()

# Simple path parameter
@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """
    Path parameter {user_id} is extracted from URL.
    Type hint (int) provides automatic validation.

    URL: /users/123
    user_id = 123 (integer)
    """
    return {"user_id": user_id, "username": f"user_{user_id}"}

# Multiple path parameters
@app.get("/users/{user_id}/posts/{post_id}")
async def get_user_post(user_id: int, post_id: int):
    """
    Multiple path parameters can be combined.
    URL: /users/5/posts/42
    user_id = 5, post_id = 42
    """
    return {
        "user_id": user_id,
        "post_id": post_id,
        "title": f"Post {post_id} by User {user_id}"
    }

# Path parameter with validation
@app.get("/items/{item_id}")
async def get_item(
    item_id: int = Path(
        ...,
        title="Item ID",
        description="The unique identifier for the item",
        ge=1,  # greater than or equal to 1
        le=1000  # less than or equal to 1000
    )
):
    """
    Path parameters can include validation rules.
    - ge=1: Must be >= 1
    - le=1000: Must be <= 1000
    """
    return {"item_id": item_id}

# String path parameters
@app.get("/files/{file_path:path}")
async def read_file(file_path: str):
    """
    The :path type allows paths with slashes.
    URL: /files/home/user/documents/file.txt
    file_path = "home/user/documents/file.txt"
    """
    return {"file_path": file_path}
```

### Path Parameter Types

```python
# Example 4: Different path parameter types
from fastapi import FastAPI
from enum import Enum
from typing import Literal

app = FastAPI()

# Enum-based path parameter
class ModelName(str, Enum):
    """
    Enum restricts parameter to predefined values.
    Using str ensures JSON serialization works.
    """
    ALEXNET = "alexnet"
    RESNET = "resnet"
    LENET = "lenet"

@app.get("/models/{model_name}")
async def get_model(model_name: ModelName):
    """
    Only accepts: alexnet, resnet, or lenet
    Returns 422 for invalid values
    """
    if model_name == ModelName.ALEXNET:
        return {"model_name": model_name, "message": "Deep Learning FTW!"}

    if model_name.value == "lenet":
        return {"model_name": model_name, "message": "LeCNN all the images"}

    return {"model_name": model_name, "message": "Have some residuals"}

# Literal type for simple restrictions
@app.get("/status/{status}")
async def get_status(status: Literal["active", "inactive", "pending"]):
    """
    Literal type restricts to specific string values.
    Simpler than enum for few options.
    """
    return {"status": status}

# UUID path parameter
from uuid import UUID

@app.get("/resources/{resource_id}")
async def get_resource(resource_id: UUID):
    """
    UUID type for unique identifiers.
    URL: /resources/550e8400-e29b-41d4-a716-446655440000
    """
    return {"resource_id": str(resource_id)}
```

## Query Parameters

### Basic Query Parameters

```python
# Example 5: Query parameters for filtering and pagination
from fastapi import FastAPI, Query
from typing import Optional

app = FastAPI()

# Simple query parameters
@app.get("/items/")
async def list_items(
    skip: int = 0,
    limit: int = 10
):
    """
    Query parameters come after ? in URL.
    URL: /items/?skip=0&limit=10
    Default values used if not provided.
    """
    return {"skip": skip, "limit": limit}

# Optional query parameters
@app.get("/search/")
async def search_items(
    q: Optional[str] = None,
    category: Optional[str] = None,
    min_price: Optional[float] = None,
    max_price: Optional[float] = None
):
    """
    Optional parameters (with defaults of None) are not required.
    URL: /search/?q=laptop&category=electronics
    """
    return {
        "query": q,
        "category": category,
        "price_range": {"min": min_price, "max": max_price}
    }

# Query parameters with validation
@app.get("/products/")
async def list_products(
    page: int = Query(1, ge=1, description="Page number"),
    page_size: int = Query(20, ge=1, le=100, description="Items per page"),
    sort_by: str = Query(
        "created_at",
        regex="^(name|price|created_at)$",
        description="Sort field"
    ),
    order: str = Query("desc", regex="^(asc|desc)$")
):
    """
    Query parameters with detailed validation:
    - ge=1: Greater than or equal to 1
    - le=100: Less than or equal to 100
    - regex: Must match pattern
    """
    return {
        "page": page,
        "page_size": page_size,
        "sort_by": sort_by,
        "order": order
    }

# Required query parameters
@app.get("/filter/")
async def filter_items(
    # ... makes the parameter required
    category: str = Query(..., min_length=1, max_length=50),
    # Optional with alias
    max_price: Optional[float] = Query(None, alias="maxPrice")
):
    """
    Required query parameters use ... (Ellipsis).
    Aliases allow different URL parameter names.
    URL: /filter/?category=electronics&maxPrice=1000
    """
    return {"category": category, "max_price": max_price}
```

### List Query Parameters

```python
# Example 6: Query parameters as lists
from fastapi import FastAPI, Query
from typing import List, Optional

app = FastAPI()

@app.get("/items/")
async def read_items(
    # Single value
    q: Optional[str] = None,
    # List of values - allows multiple tags
    tags: List[str] = Query([], description="Filter by tags")
):
    """
    List query parameters accept multiple values.
    URL: /items/?tags=python&tags=fastapi&tags=tutorial
    tags = ["python", "fastapi", "tutorial"]
    """
    return {"query": q, "tags": tags}

@app.get("/products/")
async def list_products(
    # List with default values
    categories: List[str] = Query(
        ["electronics", "books"],
        description="Product categories"
    ),
    # List with validation
    price_ranges: List[float] = Query(
        [],
        description="Price range filters",
        min_items=0,
        max_items=5
    )
):
    """
    Lists can have defaults and validation.
    URL: /products/?categories=clothing&categories=home
    """
    return {"categories": categories, "price_ranges": price_ranges}
```

## Request Body

### JSON Request Body

```python
# Example 7: Request body with Pydantic models
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

app = FastAPI()

# Define request body model
class ItemCreate(BaseModel):
    """Model for creating items"""
    name: str = Field(..., min_length=1, max_length=100)
    description: Optional[str] = None
    price: float = Field(..., gt=0)
    tags: List[str] = []

# Simple request body
@app.post("/items/")
async def create_item(item: ItemCreate):
    """
    Request body is automatically parsed from JSON.
    Validation happens automatically based on type hints.
    """
    return {
        "message": "Item created",
        "item": item.model_dump()
    }

# Request body with path and query parameters
@app.put("/items/{item_id}")
async def update_item(
    item_id: int,  # Path parameter
    item: ItemCreate,  # Request body
    notify: bool = False  # Query parameter
):
    """
    FastAPI intelligently determines where each parameter comes from:
    - item_id: Path parameter (in URL)
    - item: Request body (in JSON)
    - notify: Query parameter (after ?)
    """
    return {
        "item_id": item_id,
        "item": item.model_dump(),
        "notify": notify
    }

# Nested request body
class Address(BaseModel):
    street: str
    city: str
    country: str
    postal_code: str

class UserCreate(BaseModel):
    username: str
    email: str
    address: Address  # Nested model
    tags: List[str] = []

@app.post("/users/")
async def create_user(user: UserCreate):
    """
    Nested models are validated recursively.
    FastAPI generates nested JSON schema.
    """
    return {"user": user.model_dump()}
```

## Response Models

### Defining Response Models

```python
# Example 8: Response models for output validation
from fastapi import FastAPI
from pydantic import BaseModel, EmailStr
from typing import List, Optional
from datetime import datetime

app = FastAPI()

# Response model
class UserResponse(BaseModel):
    """Model for user responses (excludes sensitive data)"""
    id: int
    username: str
    email: str
    created_at: datetime
    is_active: bool = True

    class Config:
        from_attributes = True

# Request model
class UserCreate(BaseModel):
    """Model for user creation (includes password)"""
    username: str
    email: str
    password: str  # Input only, never in response

# Endpoint with response model
@app.post("/users/", response_model=UserResponse)
async def create_user(user: UserCreate):
    """
    response_model ensures output matches UserResponse.
    Sensitive fields (like password) are automatically excluded.
    """
    # In real app, hash password and save to database
    return UserResponse(
        id=1,
        username=user.username,
        email=user.email,
        created_at=datetime.now()
    )

# List response model
@app.get("/users/", response_model=List[UserResponse])
async def list_users():
    """Response model can be a list of models"""
    return [
        UserResponse(
            id=1,
            username="alice",
            email="alice@example.com",
            created_at=datetime.now()
        )
    ]

# Exclude unset fields
@app.get("/users/{user_id}", response_model=UserResponse, response_model_exclude_unset=True)
async def get_user(user_id: int):
    """
    response_model_exclude_unset=True excludes fields that weren't set.
    Useful for sparse responses.
    """
    return UserResponse(
        id=user_id,
        username="john",
        email="john@example.com",
        created_at=datetime.now()
        # is_active not set, so excluded from response
    )
```

## Route Organization with Routers

### Using APIRouter

```python
# Example 9: Organizing routes with APIRouter
from fastapi import APIRouter, FastAPI, Depends, HTTPException
from typing import List

app = FastAPI()

# Create router for items
items_router = APIRouter(
    prefix="/items",  # All routes start with /items
    tags=["items"],  # Group in documentation
    responses={404: {"description": "Not found"}}  # Default responses
)

@items_router.get("/")
async def list_items():
    """GET /items/"""
    return {"items": []}

@items_router.get("/{item_id}")
async def get_item(item_id: int):
    """GET /items/{item_id}"""
    return {"item_id": item_id}

@items_router.post("/")
async def create_item(name: str, price: float):
    """POST /items/"""
    return {"name": name, "price": price}

# Create router for users
users_router = APIRouter(
    prefix="/users",
    tags=["users"],
    responses={404: {"description": "Not found"}}
)

@users_router.get("/")
async def list_users():
    """GET /users/"""
    return {"users": []}

@users_router.get("/{user_id}")
async def get_user(user_id: int):
    """GET /users/{user_id}"""
    return {"user_id": user_id}

# Include routers in main app
app.include_router(items_router)
app.include_router(users_router)

# The resulting routes:
# GET /items/
# GET /items/{item_id}
# POST /items/
# GET /users/
# GET /users/{user_id}
```

### Nested Routers

```python
# Example 10: Nested routers for complex hierarchies
from fastapi import APIRouter, FastAPI

app = FastAPI()

# Parent router
api_router = APIRouter(prefix="/api/v1", tags=["api"])

# Child router
users_router = APIRouter(prefix="/users", tags=["users"])

@users_router.get("/")
async def list_users():
    """GET /api/v1/users/"""
    return {"users": []}

@users_router.get("/{user_id}")
async def get_user(user_id: int):
    """GET /api/v1/users/{user_id}"""
    return {"user_id": user_id}

# User posts router (nested under user)
posts_router = APIRouter(prefix="/{user_id}/posts", tags=["posts"])

@posts_router.get("/")
async def list_user_posts(user_id: int):
    """GET /api/v1/users/{user_id}/posts/"""
    return {"user_id": user_id, "posts": []}

# Build nested structure
users_router.include_router(posts_router)
api_router.include_router(users_router)
app.include_router(api_router)

# Final routes:
# GET /api/v1/users/
# GET /api/v1/users/{user_id}
# GET /api/v1/users/{user_id}/posts/
```

## Dependencies in Routes

### Route Dependencies

```python
# Example 11: Using dependencies in routes
from fastapi import FastAPI, Depends, Header, HTTPException
from typing import Optional

app = FastAPI()

# Dependency function
async def verify_token(x_token: str = Header(...)):
    """
    Dependency that verifies authentication token.
    Runs before the route handler.
    """
    if x_token != "valid-token":
        raise HTTPException(status_code=403, detail="Invalid token")
    return x_token

async def get_pagination(
    skip: int = 0,
    limit: int = 10
):
    """Dependency for pagination parameters"""
    return {"skip": skip, "limit": limit}

# Route with dependencies
@app.get("/items/", dependencies=[Depends(verify_token)])
async def list_items(pagination: dict = Depends(get_pagination)):
    """
    - verify_token runs first (authentication)
    - get_pagination runs next (extract parameters)
    - Route handler runs last
    """
    return {"items": [], "pagination": pagination}

# Shared dependencies on router
from fastapi import APIRouter

protected_router = APIRouter(
    prefix="/admin",
    tags=["admin"],
    dependencies=[Depends(verify_token)]  # All routes require token
)

@protected_router.get("/stats")
async def admin_stats():
    """All routes in this router require authentication"""
    return {"users": 100, "items": 500}

app.include_router(protected_router)
```

## Route Configuration

### Route Options

```python
# Example 12: Advanced route configuration
from fastapi import FastAPI, status

app = FastAPI()

# Custom status code
@app.post("/items/", status_code=status.HTTP_201_CREATED)
async def create_item(name: str):
    """Returns 201 Created instead of default 200"""
    return {"name": name}

# Response description
@app.get(
    "/items/{item_id}",
    summary="Get an item",
    description="Retrieve a specific item by its unique identifier",
    response_description="The requested item",
    responses={
        200: {"description": "Item found"},
        404: {"description": "Item not found"}
    }
)
async def get_item(item_id: int):
    """Detailed endpoint documentation"""
    return {"item_id": item_id}

# Deprecated endpoint
@app.get("/old-items/", deprecated=True)
async def old_list_items():
    """
    This endpoint is deprecated.
    Use /items/ instead.
    Marked as deprecated in documentation.
    """
    return {"items": []}

# Include in documentation but mark as hidden
@app.get("/internal/metrics", include_in_schema=False)
async def internal_metrics():
    """
    This endpoint won't appear in auto-generated documentation.
    Useful for internal-only endpoints.
    """
    return {"metrics": {}}
```

## Best Practices

### Route Naming Conventions

```python
# Example 13: RESTful route naming
from fastapi import FastAPI

app = FastAPI()

# GOOD: RESTful naming conventions
@app.get("/users")           # List users
@app.post("/users")          # Create user
@app.get("/users/{id}")      # Get user
@app.put("/users/{id}")      # Replace user
@app.patch("/users/{id}")    # Update user
@app.delete("/users/{id}")   # Delete user

# Nested resources
@app.get("/users/{id}/orders")      # Get user's orders
@app.post("/users/{id}/orders")     # Create order for user

# AVOID: Non-RESTful naming
# @app.get("/getUsers")
# @app.post("/createUser")
# @app.get("/getUser/{id}")
# These use verbs in URLs, which is not RESTful
```

### Common Pitfalls

```python
# Example 14: Common routing mistakes and fixes

from fastapi import FastAPI

app = FastAPI()

# PITFALL 1: Route order matters!
# More specific routes must come before less specific ones

# BAD: This catches all items, including "search"
# @app.get("/items/{item_id}")
# async def get_item(item_id: int):
#     return {"item_id": item_id}

# @app.get("/items/search")
# async def search_items():
#     return {"results": []}

# GOOD: Specific routes first
@app.get("/items/search")
async def search_items():
    return {"results": []}

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    return {"item_id": item_id}

# PITFALL 2: Trailing slashes
# /items/ and /items are different routes

# These are different endpoints!
@app.get("/items")
async def items_no_slash():
    return {"path": "/items"}

@app.get("/items/")
async def items_with_slash():
    return {"path": "/items/"}

# BEST PRACTICE: Be consistent, don't use trailing slashes
```

## Summary

| Concept | Description | Example |
|---------|-------------|---------|
| HTTP Methods | GET, POST, PUT, PATCH, DELETE | `@app.get("/items")` |
| Path Parameters | Dynamic URL segments | `/items/{item_id}` |
| Query Parameters | URL parameters after ? | `/items/?skip=0&limit=10` |
| Request Body | JSON body with Pydantic | `item: ItemCreate` |
| Response Model | Output validation | `response_model=Item` |
| Routers | Route organization | `APIRouter(prefix="/items")` |

## Next Steps

Continue learning about:
- [Request/Response Cycle](./02_request_response_cycle.md) - Understanding the flow
- [Parameters](./03_parameters_explained.md) - Deep dive into parameters
- [Path Operations](../../02_core_features/01_path_operations/01_basic_routes.md) - Advanced routing
