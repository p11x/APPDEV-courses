# Pagination and Filtering

## What You'll Learn
- Offset-based pagination
- Cursor-based pagination
- Filtering and sorting

## Prerequisites
- Completed error handling

## Offset Pagination

```python
from fastapi import FastAPI, Query
from pydantic import BaseModel
from typing import List, Optional

app = FastAPI()

class User(BaseModel):
    id: int
    name: str
    email: str

users_db = [
    User(id=i, name=f"User {i}", email=f"user{i}@example.com")
    for i in range(1, 101)
]

@app.get("/users")
async def get_users(
    skip: int = Query(0, ge=0),
    limit: int = Query(10, ge=1, le=100),
    search: Optional[str] = None
):
    """Get users with pagination"""
    # Filter
    users = users_db
    if search:
        users = [u for u in users if search.lower() in u.name.lower()]
    
    # Get total
    total = len(users)
    
    # Paginate
    paginated = users[skip:skip + limit]
    
    return {
        "data": paginated,
        "pagination": {
            "total": total,
            "skip": skip,
            "limit": limit,
            "has_more": skip + limit < total
        }
    }
```

## Cursor-Based Pagination

```python
from fastapi import FastAPI, Query
from pydantic import BaseModel
from typing import Optional, List

app = FastAPI()

class PageResult(BaseModel):
    data: List[dict]
    next_cursor: Optional[str] = None
    has_more: bool = False

@app.get("/posts")
async def get_posts(cursor: Optional[str] = None, limit: int = 10):
    """Cursor-based pagination for large datasets"""
    # In production, use database cursor
    posts = [{"id": i, "title": f"Post {i}"} for i in range(1, 101)]
    
    # Find start index
    start = 0
    if cursor:
        start = int(cursor)
    
    # Get page
    page = posts[start:start + limit]
    
    return {
        "data": page,
        "next_cursor": str(start + limit) if len(page) == limit else None,
        "has_more": len(page) == limit
    }
```

## Filtering and Sorting

```python
from fastapi import FastAPI, Query
from enum import Enum

app = FastAPI()

class SortOrder(str, Enum):
    asc = "asc"
    desc = "desc"

@app.get("/products")
async def get_products(
    # Filters
    category: Optional[str] = None,
    min_price: Optional[float] = None,
    max_price: Optional[float] = None,
    in_stock: Optional[bool] = None,
    # Sorting
    sort_by: str = "id",
    order: SortOrder = SortOrder.asc,
    # Pagination
    page: int = 1,
    per_page: int = 20
):
    """Advanced filtering and sorting"""
    products = [
        {"id": i, "name": f"Product {i}", "category": "A" if i % 2 == 0 else "B", 
         "price": i * 10, "in_stock": i % 3 != 0}
        for i in range(1, 101)
    ]
    
    # Filter
    if category:
        products = [p for p in products if p["category"] == category]
    if min_price is not None:
        products = [p for p in products if p["price"] >= min_price]
    if max_price is not None:
        products = [p for p in products if p["price"] <= max_price]
    if in_stock is not None:
        products = [p for p in products if p["in_stock"] == in_stock]
    
    # Sort
    reverse = order == SortOrder.desc
    products.sort(key=lambda x: x.get(sort_by, 0), reverse=reverse)
    
    # Paginate
    start = (page - 1) * per_page
    paginated = products[start:start + per_page]
    
    return {
        "data": paginated,
        "total": len(products),
        "page": page,
        "per_page": per_page
    }
```

## Summary
- Use offset pagination for small datasets
- Use cursor pagination for large datasets
- Support filtering and sorting
- Include pagination metadata

## Next Steps
→ Continue to `05-hateoas-and-rich-responses.md`
