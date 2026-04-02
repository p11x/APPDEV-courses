# RESTful API Design

## Overview

Well-designed REST APIs are intuitive, consistent, and easy to consume. This guide covers REST best practices for FastAPI applications.

## REST Principles

### Resource-Based URLs

```python
# Example 1: RESTful URL design
from fastapi import FastAPI, APIRouter

app = FastAPI()

# GOOD: Resource-based URLs
# GET    /users           - List users
# POST   /users           - Create user
# GET    /users/{id}      - Get user
# PUT    /users/{id}      - Update user
# DELETE /users/{id}      - Delete user

# Nested resources
# GET    /users/{id}/posts        - User's posts
# POST   /users/{id}/posts        - Create post for user
# GET    /users/{id}/posts/{pid}   - Specific post

users_router = APIRouter(prefix="/users", tags=["users"])

@users_router.get("/")
async def list_users():
    return {"users": []}

@users_router.post("/", status_code=201)
async def create_user(user: UserCreate):
    return {"id": 1, **user.dict()}

@users_router.get("/{user_id}")
async def get_user(user_id: int):
    return {"id": user_id}

@users_router.put("/{user_id}")
async def update_user(user_id: int, user: UserUpdate):
    return {"id": user_id, **user.dict()}

@users_router.delete("/{user_id}", status_code=204)
async def delete_user(user_id: int):
    pass

# Nested resources
@users_router.get("/{user_id}/posts")
async def list_user_posts(user_id: int):
    return {"user_id": user_id, "posts": []}

app.include_router(users_router)
```

## HTTP Methods

### Proper Method Usage

```python
# Example 2: HTTP method semantics
from fastapi import FastAPI, status

app = FastAPI()

# GET - Retrieve resource(s) - Safe, Idempotent
@app.get("/items/")
async def list_items():
    """Retrieve collection"""
    return {"items": []}

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    """Retrieve single resource"""
    return {"id": item_id}

# POST - Create resource - Not idempotent
@app.post("/items/", status_code=status.HTTP_201_CREATED)
async def create_item(item: ItemCreate):
    """Create new resource"""
    return {"id": 1, **item.dict()}

# PUT - Replace entire resource - Idempotent
@app.put("/items/{item_id}")
async def replace_item(item_id: int, item: ItemCreate):
    """Replace resource completely"""
    return {"id": item_id, **item.dict()}

# PATCH - Partial update - Not necessarily idempotent
@app.patch("/items/{item_id}")
async def update_item(item_id: int, item: ItemUpdate):
    """Update resource partially"""
    return {"id": item_id, **item.dict(exclude_unset=True)}

# DELETE - Remove resource - Idempotent
@app.delete("/items/{item_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_item(item_id: int):
    """Delete resource"""
    pass

# OPTIONS - Get allowed methods
@app.options("/items/")
async def options_items():
    """Return allowed methods"""
    return {"allow": ["GET", "POST", "OPTIONS"]}
```

## Response Formats

### Consistent Response Structure

```python
# Example 3: Standardized response format
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional, List, Any
from datetime import datetime

app = FastAPI()

class ApiResponse(BaseModel):
    """Standard API response wrapper"""
    success: bool
    data: Optional[Any] = None
    error: Optional[dict] = None
    meta: Optional[dict] = None

class PaginatedResponse(BaseModel):
    """Paginated response structure"""
    items: List[Any]
    total: int
    page: int
    per_page: int
    pages: int

@app.get("/items/")
async def list_items(
    page: int = 1,
    per_page: int = 20
) -> PaginatedResponse:
    """Paginated list endpoint"""
    total = 100  # From database
    items = [{"id": i} for i in range((page-1)*per_page, page*per_page)]

    return PaginatedResponse(
        items=items,
        total=total,
        page=page,
        per_page=per_page,
        pages=(total + per_page - 1) // per_page
    )

@app.get("/items/{item_id}")
async def get_item(item_id: int) -> ApiResponse:
    """Single item response"""
    if item_id > 100:
        raise HTTPException(404, "Item not found")

    return ApiResponse(
        success=True,
        data={"id": item_id, "name": "Item"}
    )
```

## Filtering and Sorting

### Query Parameters

```python
# Example 4: Filtering, sorting, pagination
from fastapi import FastAPI, Query
from typing import Optional, List
from enum import Enum

app = FastAPI()

class SortOrder(str, Enum):
    ASC = "asc"
    DESC = "desc"

class SortField(str, Enum):
    NAME = "name"
    CREATED = "created_at"
    PRICE = "price"

@app.get("/products/")
async def list_products(
    # Filtering
    category: Optional[str] = Query(None, description="Filter by category"),
    min_price: Optional[float] = Query(None, ge=0, description="Minimum price"),
    max_price: Optional[float] = Query(None, gt=0, description="Maximum price"),
    in_stock: Optional[bool] = Query(None, description="Filter by availability"),

    # Searching
    q: Optional[str] = Query(None, min_length=1, description="Search query"),

    # Sorting
    sort_by: SortField = Query(SortField.CREATED, description="Sort field"),
    order: SortOrder = Query(SortOrder.DESC, description="Sort order"),

    # Pagination
    page: int = Query(1, ge=1, description="Page number"),
    per_page: int = Query(20, ge=1, le=100, description="Items per page")
):
    """
    List products with filtering, sorting, and pagination.

    Examples:
    - /products/?category=electronics&min_price=100
    - /products/?q=laptop&sort_by=price&order=asc
    - /products/?page=2&per_page=50
    """
    # Build query
    query = db.query(Product)

    # Apply filters
    if category:
        query = query.filter(Product.category == category)
    if min_price is not None:
        query = query.filter(Product.price >= min_price)
    if max_price is not None:
        query = query.filter(Product.price <= max_price)
    if in_stock is not None:
        query = query.filter(Product.in_stock == in_stock)
    if q:
        query = query.filter(Product.name.ilike(f"%{q}%"))

    # Apply sorting
    sort_column = getattr(Product, sort_by.value)
    if order == SortOrder.DESC:
        query = query.order_by(sort_column.desc())
    else:
        query = query.order_by(sort_column.asc())

    # Apply pagination
    total = query.count()
    items = query.offset((page - 1) * per_page).limit(per_page).all()

    return {
        "items": items,
        "meta": {
            "total": total,
            "page": page,
            "per_page": per_page,
            "pages": (total + per_page - 1) // per_page
        }
    }
```

## HATEOAS

### Hypermedia Links

```python
# Example 5: HATEOAS implementation
from fastapi import FastAPI, Request
from typing import List, Dict

app = FastAPI()

def add_links(item: dict, request: Request) -> dict:
    """Add HATEOAS links to resource"""
    item_id = item["id"]
    base_url = str(request.base_url)

    item["_links"] = {
        "self": {"href": f"{base_url}items/{item_id}"},
        "update": {"href": f"{base_url}items/{item_id}", "method": "PUT"},
        "delete": {"href": f"{base_url}items/{item_id}", "method": "DELETE"},
        "collection": {"href": f"{base_url}items/"}
    }

    return item

@app.get("/items/{item_id}")
async def get_item(item_id: int, request: Request):
    """Get item with HATEOAS links"""
    item = {"id": item_id, "name": "Sample Item"}
    return add_links(item, request)

@app.get("/items/")
async def list_items(request: Request):
    """List items with HATEOAS links"""
    items = [{"id": i, "name": f"Item {i}"} for i in range(1, 11)]

    return {
        "items": [add_links(item, request) for item in items],
        "_links": {
            "self": {"href": f"{request.base_url}items/"},
            "next": {"href": f"{request.base_url}items/?page=2"}
        }
    }
```

## Content Negotiation

### Multiple Formats

```python
# Example 6: Content negotiation
from fastapi import FastAPI, Header, HTTPException
from fastapi.responses import JSONResponse, PlainTextResponse, HTMLResponse

app = FastAPI()

@app.get("/items/{item_id}")
async def get_item(
    item_id: int,
    accept: str = Header("application/json")
):
    """Content negotiation based on Accept header"""
    item = {"id": item_id, "name": "Sample Item"}

    if "application/json" in accept:
        return JSONResponse(content=item)
    elif "text/plain" in accept:
        return PlainTextResponse(content=f"Item {item_id}: {item['name']}")
    elif "text/html" in accept:
        return HTMLResponse(
            content=f"<h1>Item {item_id}</h1><p>{item['name']}</p>"
        )
    else:
        raise HTTPException(406, "Not Acceptable")
```

## Best Practices

### REST API Guidelines

```python
# Example 7: REST best practices checklist
"""
REST API Best Practices:

1. URL Design
   ✓ Use nouns, not verbs
   ✓ Plural resource names
   ✓ Hierarchical relationships
   ✓ Lowercase with hyphens

2. HTTP Methods
   ✓ GET for reading
   ✓ POST for creating
   ✓ PUT for replacing
   ✓ PATCH for updating
   ✓ DELETE for removing

3. Status Codes
   ✓ 200 OK - Successful GET/PUT/PATCH
   ✓ 201 Created - Successful POST
   ✓ 204 No Content - Successful DELETE
   ✓ 400 Bad Request - Validation error
   ✓ 401 Unauthorized - Not authenticated
   ✓ 403 Forbidden - Not authorized
   ✓ 404 Not Found - Resource missing
   ✓ 422 Unprocessable Entity - Validation failed

4. Response Format
   ✓ Consistent structure
   ✓ Include metadata
   ✓ Proper error format
   ✓ HATEOAS links

5. Versioning
   ✓ URL versioning: /v1/items
   ✓ Header versioning: Accept-Version: v1
   ✓ Query parameter: /items?version=1
"""
```

## Summary

| Aspect | Recommendation |
|--------|----------------|
| URLs | Nouns, plural, hierarchical |
| Methods | Semantic HTTP methods |
| Status | Appropriate status codes |
| Response | Consistent format |
| Versioning | URL or header versioning |

## Next Steps

Continue learning about:
- [GraphQL Implementation](../01_advanced_architecture/09_graphql_implementation.md)
- [API Versioning](./03_versioning_strategies.md)
- [Hypermedia APIs](./04_hypermedia_apis.md)
