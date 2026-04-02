# Basic Routes

## Overview

Routes are the foundation of any FastAPI application. They define how your API responds to HTTP requests at specific URLs. This guide covers all HTTP methods, route decorators, and practical patterns for building robust APIs.

## HTTP Methods

FastAPI provides dedicated decorators for each HTTP method:

```python
# Example 1: All HTTP method decorators
from fastapi import FastAPI

app = FastAPI()

# GET - Retrieve resource(s)
# Safe, idempotent, cacheable
@app.get("/items")
async def list_items():
    """Retrieve all items"""
    return {"items": ["laptop", "phone", "tablet"]}

# POST - Create a new resource
# Not idempotent, request body contains data
@app.post("/items")
async def create_item(name: str, price: float):
    """Create a new item"""
    return {"message": "Item created", "name": name, "price": price}

# PUT - Replace entire resource
# Idempotent, client provides complete representation
@app.put("/items/{item_id}")
async def replace_item(item_id: int, name: str, price: float):
    """Replace an item completely"""
    return {"item_id": item_id, "name": name, "price": price}

# PATCH - Partial update
# Not always idempotent, only modified fields
@app.patch("/items/{item_id}")
async def update_item(item_id: int, name: str | None = None):
    """Update specific fields of an item"""
    return {"item_id": item_id, "updated_name": name}

# DELETE - Remove resource
# Idempotent
@app.delete("/items/{item_id}")
async def delete_item(item_id: int):
    """Delete an item"""
    return {"message": f"Item {item_id} deleted"}

# HEAD - Get headers only (no body)
# Same as GET but returns only headers
@app.head("/items")
async def head_items():
    """Check if items endpoint exists"""
    pass

# OPTIONS - Get allowed methods
# Used for CORS preflight
@app.options("/items")
async def options_items():
    """Return allowed HTTP methods"""
    return {"allowed": ["GET", "POST", "PUT", "DELETE"]}
```

## Route Decorators

### Decorator Configuration

```python
# Example 2: Route decorator options
from fastapi import FastAPI, status

app = FastAPI()

# Basic route with status code
@app.post("/items/", status_code=status.HTTP_201_CREATED)
async def create_item(name: str):
    """Returns 201 Created instead of default 200"""
    return {"name": name}

# Route with tags for documentation grouping
@app.get("/users/", tags=["users"])
async def list_users():
    """Grouped under 'users' tag in docs"""
    return {"users": []}

@app.get("/items/", tags=["items"])
async def list_items():
    """Grouped under 'items' tag in docs"""
    return {"items": []}

# Route with summary and description
@app.get(
    "/products/{product_id}",
    summary="Get a product",
    description="Retrieve a specific product by its unique identifier. "
                "Returns full product details including pricing and availability.",
    response_description="The requested product with all details"
)
async def get_product(product_id: int):
    return {"product_id": product_id}

# Deprecated route
@app.get("/old-endpoint", deprecated=True)
async def old_endpoint():
    """
    This endpoint is deprecated.
    Use /new-endpoint instead.
    Shows as deprecated in Swagger UI.
    """
    return {"message": "Deprecated"}

# Hidden route (not in docs)
@app.get("/internal/health", include_in_schema=False)
async def internal_health():
    """Won't appear in auto-generated documentation"""
    return {"status": "healthy"}
```

## CRUD Operations Pattern

### Complete CRUD API

```python
# Example 3: Complete CRUD operations
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

app = FastAPI()

# Data models
class ItemBase(BaseModel):
    name: str = Field(..., min_length=1, max_length=100)
    description: Optional[str] = None
    price: float = Field(..., gt=0)
    is_available: bool = True

class ItemCreate(ItemBase):
    pass

class ItemUpdate(BaseModel):
    name: Optional[str] = None
    description: Optional[str] = None
    price: Optional[float] = None
    is_available: Optional[bool] = None

class Item(ItemBase):
    id: int
    created_at: datetime
    updated_at: Optional[datetime] = None

    class Config:
        from_attributes = True

# In-memory storage
items_db: dict[int, dict] = {}
current_id = 0

# CREATE
@app.post("/items/", response_model=Item, status_code=201)
async def create_item(item: ItemCreate):
    """
    Create a new item.

    - Validates input using ItemCreate model
    - Returns 201 Created on success
    - Returns 422 if validation fails
    """
    global current_id
    current_id += 1

    new_item = Item(
        id=current_id,
        **item.model_dump(),
        created_at=datetime.now()
    )
    items_db[current_id] = new_item.model_dump()
    return new_item

# READ (list)
@app.get("/items/", response_model=List[Item])
async def list_items(skip: int = 0, limit: int = 10):
    """
    List all items with pagination.

    - Default: first 10 items
    - Supports skip and limit parameters
    """
    items = list(items_db.values())
    return items[skip:skip + limit]

# READ (single)
@app.get("/items/{item_id}", response_model=Item)
async def get_item(item_id: int):
    """
    Get a specific item by ID.

    - Returns 404 if item not found
    - Returns full item details
    """
    if item_id not in items_db:
        raise HTTPException(
            status_code=404,
            detail=f"Item with id {item_id} not found"
        )
    return items_db[item_id]

# UPDATE (full replacement)
@app.put("/items/{item_id}", response_model=Item)
async def replace_item(item_id: int, item: ItemCreate):
    """
    Replace an item completely.

    - All fields required
    - Returns 404 if item not found
    """
    if item_id not in items_db:
        raise HTTPException(status_code=404, detail="Item not found")

    updated = Item(
        id=item_id,
        **item.model_dump(),
        created_at=items_db[item_id]["created_at"],
        updated_at=datetime.now()
    )
    items_db[item_id] = updated.model_dump()
    return updated

# UPDATE (partial)
@app.patch("/items/{item_id}", response_model=Item)
async def update_item(item_id: int, item: ItemUpdate):
    """
    Update specific fields of an item.

    - Only provided fields are updated
    - Returns 404 if item not found
    """
    if item_id not in items_db:
        raise HTTPException(status_code=404, detail="Item not found")

    current = items_db[item_id]
    update_data = item.model_dump(exclude_unset=True)
    current.update(update_data)
    current["updated_at"] = datetime.now()

    return current

# DELETE
@app.delete("/items/{item_id}", status_code=204)
async def delete_item(item_id: int):
    """
    Delete an item.

    - Returns 204 No Content on success
    - Returns 404 if item not found
    """
    if item_id not in items_db:
        raise HTTPException(status_code=404, detail="Item not found")

    del items_db[item_id]
```

## API Versioning

### Version Prefix Pattern

```python
# Example 4: API versioning with routers
from fastapi import FastAPI, APIRouter

app = FastAPI()

# Version 1 router
v1_router = APIRouter(prefix="/api/v1", tags=["v1"])

@v1_router.get("/items")
async def v1_list_items():
    """V1: Returns simple list"""
    return {"items": ["item1", "item2"]}

@v1_router.get("/items/{item_id}")
async def v1_get_item(item_id: int):
    """V1: Simple item retrieval"""
    return {"item_id": item_id}

# Version 2 router with enhanced features
v2_router = APIRouter(prefix="/api/v2", tags=["v2"])

@v2_router.get("/items")
async def v2_list_items(skip: int = 0, limit: int = 10):
    """V2: Enhanced with pagination"""
    return {
        "items": [],
        "pagination": {"skip": skip, "limit": limit, "total": 0}
    }

@v2_router.get("/items/{item_id}")
async def v2_get_item(item_id: int, include_details: bool = False):
    """V2: Enhanced with optional details"""
    result = {"item_id": item_id}
    if include_details:
        result["details"] = {"created": "2024-01-01"}
    return result

# Include both versions
app.include_router(v1_router)
app.include_router(v2_router)

# Routes created:
# GET /api/v1/items
# GET /api/v1/items/{item_id}
# GET /api/v2/items
# GET /api/v2/items/{item_id}
```

### Header-Based Versioning

```python
# Example 5: Version via header
from fastapi import FastAPI, Header, HTTPException

app = FastAPI()

@app.get("/items")
async def list_items(x_api_version: str = Header("1")):
    """
    Version determined by X-API-Version header.
    Default: version 1
    """
    if x_api_version == "1":
        return {"version": 1, "items": ["v1_item1", "v1_item2"]}
    elif x_api_version == "2":
        return {
            "version": 2,
            "items": [
                {"id": 1, "name": "v2_item1"},
                {"id": 2, "name": "v2_item2"}
            ]
        }
    else:
        raise HTTPException(
            status_code=400,
            detail=f"Unsupported API version: {x_api_version}"
        )
```

## Response Configuration

### Status Codes and Response Models

```python
# Example 6: Response configuration
from fastapi import FastAPI, status
from pydantic import BaseModel
from typing import List

app = FastAPI()

class Item(BaseModel):
    id: int
    name: str
    price: float

class ErrorDetail(BaseModel):
    error: str
    message: str

# Custom status codes
@app.post(
    "/items/",
    response_model=Item,
    status_code=status.HTTP_201_CREATED,
    responses={
        400: {"model": ErrorDetail, "description": "Invalid input"},
        409: {"model": ErrorDetail, "description": "Item already exists"}
    }
)
async def create_item(name: str, price: float):
    """
    Create item with multiple response types.

    OpenAPI docs will show:
    - 201: Created item
    - 400: Validation error
    - 409: Conflict error
    """
    return Item(id=1, name=name, price=price)

# List with total count
@app.get(
    "/items/",
    response_model=List[Item],
    status_code=status.HTTP_200_OK
)
async def list_items():
    """Standard 200 OK response"""
    return [Item(id=1, name="Item", price=9.99)]

# No content response
@app.delete(
    "/items/{item_id}",
    status_code=status.HTTP_204_NO_CONTENT
)
async def delete_item(item_id: int):
    """
    204 No Content - successful deletion.
    No response body is sent.
    """
    pass
```

## Best Practices

### RESTful Naming Conventions

```python
# Example 7: RESTful route naming
from fastapi import FastAPI

app = FastAPI()

# GOOD: Nouns for resources, HTTP methods for actions
@app.get("/users")                    # List users
@app.post("/users")                   # Create user
@app.get("/users/{user_id}")          # Get user
@app.put("/users/{user_id}")          # Replace user
@app.patch("/users/{user_id}")        # Update user
@app.delete("/users/{user_id}")       # Delete user

# Nested resources
@app.get("/users/{user_id}/orders")           # Get user's orders
@app.post("/users/{user_id}/orders")          # Create order for user
@app.get("/users/{user_id}/orders/{order_id}")  # Get specific order

# AVOID: Verbs in URLs
# @app.get("/getUsers")
# @app.post("/createUser")
# @app.delete("/deleteUser/{id}")

# AVOID: Singular for collections
# @app.get("/user")  # Should be /users

# AVOID: File extensions
# @app.get("/items.json")
```

### Error Handling Patterns

```python
# Example 8: Consistent error handling
from fastapi import FastAPI, HTTPException, status
from pydantic import BaseModel

app = FastAPI()

class ErrorResponse(BaseModel):
    error: str
    message: str
    details: dict | None = None

@app.get(
    "/items/{item_id}",
    responses={
        404: {"model": ErrorResponse}
    }
)
async def get_item(item_id: int):
    """
    Consistent error responses help API consumers.
    """
    if item_id <= 0:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail={
                "error": "invalid_id",
                "message": "Item ID must be positive",
                "details": {"provided_id": item_id}
            }
        )

    if item_id > 1000:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail={
                "error": "not_found",
                "message": f"Item {item_id} does not exist",
                "details": {"item_id": item_id}
            }
        )

    return {"item_id": item_id}
```

## Common Pitfalls

### Route Order Matters

```python
# Example 9: Route ordering pitfalls
from fastapi import FastAPI

app = FastAPI()

# PITFALL: Specific routes after catch-all routes

# BAD: This catches "search" as item_id
# @app.get("/items/{item_id}")
# async def get_item(item_id: int):
#     return {"item_id": item_id}

# @app.get("/items/search")
# async def search_items():
#     return {"results": []}

# GOOD: Specific routes first
@app.get("/items/search")
async def search_items():
    """Specific route comes first"""
    return {"results": []}

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    """Catch-all route comes after"""
    return {"item_id": item_id}

# PITFALL: Trailing slashes
# /items and /items/ are DIFFERENT routes

@app.get("/items")
async def items_no_slash():
    """Matches /items only"""
    return {"path": "/items"}

@app.get("/items/")
async def items_with_slash():
    """Matches /items/ only"""
    return {"path": "/items/"}

# BEST PRACTICE: Be consistent, avoid trailing slashes
```

## Performance Considerations

```python
# Example 10: Performance-optimized routes
from fastapi import FastAPI
from fastapi.responses import ORJSONResponse
import asyncio

app = FastAPI()

# Use ORJSON for faster JSON serialization
@app.get("/items/", response_class=ORJSONResponse)
async def fast_items():
    """
    ORJSONResponse is 2-3x faster than default JSONResponse.
    Use for high-throughput endpoints.
    """
    return {"items": list(range(1000))}

# Async for I/O-bound operations
@app.get("/external-data")
async def external_data():
    """Async allows concurrent request handling"""
    import httpx
    async with httpx.AsyncClient() as client:
        response = await client.get("https://api.example.com/data")
        return response.json()

# Sync for CPU-bound operations
@app.get("/compute")
def heavy_computation():
    """
    Sync functions block but are simpler.
    Use for CPU-bound work or when async isn't needed.
    """
    result = sum(i * i for i in range(1000000))
    return {"result": result}
```

## Summary

| Aspect | Best Practice |
|--------|---------------|
| Naming | Use nouns, plural for collections |
| Methods | GET=read, POST=create, PUT=replace, PATCH=update, DELETE=remove |
| Status Codes | 200 OK, 201 Created, 204 No Content, 404 Not Found |
| Versioning | Use URL prefix (`/api/v1/`) |
| Error Handling | Consistent error response format |
| Routes | Specific before catch-all |

## Next Steps

Continue learning about:
- [Path Parameters](./02_path_parameters.md) - Dynamic URL segments
- [Query Parameters](./03_query_parameters.md) - URL query strings
- [Header Parameters](./04_header_parameters.md) - HTTP headers
