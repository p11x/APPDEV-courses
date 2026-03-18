# Path and Query Parameters

## What You'll Learn
- Working with path parameters
- Working with query parameters
- Parameter validation
- Optional parameters
- Multiple parameter types

## Prerequisites
- Completed FastAPI Introduction

## Path Parameters

**Path parameters** are parts of the URL that identify specific resources:

```
/users/42        → user_id = 42
/items/apple     → item_name = "apple"
/posts/2024/01   → year = 2024, month = 01
```

### Basic Path Parameters

```python
from fastapi import FastAPI

app = FastAPI()

@app.get("/users/{user_id}")
def get_user(user_id: int) -> dict[str, int]:
    return {"user_id": user_id, "name": "Alice"}

@app.get("/items/{item_id}")
def get_item(item_id: str) -> dict[str, str]:
    return {"item_id": item_id, "name": "Widget"}
```

### Path Parameter Types

FastAPI automatically converts path parameters:

```python
@app.get("/users/{user_id}")
def get_user(user_id: int) -> dict[str, int]:
    # user_id comes as string from URL, FastAPI converts to int
    return {"user_id": user_id}

@app.get("/prices/{price}")
def get_price(price: float) -> dict[str, float]:
    # Accepts: /prices/19.99
    return {"price": price}

@app.get("/files/{file_path:path}")
def get_file(file_path: str) -> dict[str, str]:
    # The :path converter allows slashes
    # Accepts: /files/images/photo.jpg
    return {"file_path": file_path}
```

### Order Matters

Define more specific routes before general ones:

```python
@app.get("/users/me")
def get_current_user() -> dict[str, str]:
    """This must come before /users/{user_id}"""
    return {"user_id": "current", "name": "Alice"}

@app.get("/users/{user_id}")
def get_user(user_id: int) -> dict[str, Any]:
    return {"user_id": user_id}
```

## Query Parameters

**Query parameters** appear after `?` in the URL:

```
/items?skip=0&limit=10
```

- `skip` = 0
- `limit` = 10

### Basic Query Parameters

```python
from fastapi import FastAPI
from typing import Any

app = FastAPI()

fake_items = [
    {"name": "Apple", "price": 1.00},
    {"name": "Banana", "price": 0.50},
    {"name": "Cherry", "price": 2.00},
    {"name": "Date", "price": 3.00}
]

@app.get("/items")
def read_items(skip: int = 0, limit: int = 10) -> list[dict[str, Any]]:
    return fake_items[skip:skip + limit]
```

### Optional Query Parameters

```python
from typing import Optional

@app.get("/items")
def read_items(
    q: Optional[str] = None,
    skip: int = 0,
    limit: int = 10
) -> dict[str, Any]:
    items = fake_items[skip:skip + limit]
    
    if q:
        items = [item for item in items if q.lower() in item["name"].lower()]
    
    return {"items": items, "q": q}
```

Try it:
- `/items` → returns all items
- `/items?q=app` → returns items matching "app"

### Required vs Optional

```python
from fastapi import FastAPI

app = FastAPI()

# Required query parameter (no default)
@app.get("/items")
def read_items(category: str) -> dict[str, str]:
    return {"category": category}

# Optional query parameter (has default)
@app.get("/products")
def read_products(page: int = 1) -> dict[str, int]:
    return {"page": page}
```

## Parameter Validation with Pydantic

Use Pydantic for more complex validation:

```python
from fastapi import FastAPI, Query
from typing import Optional

app = FastAPI()

@app.get("/items")
def read_items(
    # Query parameters with validation
    q: Optional[str] = Query(
        None, 
        min_length=3, 
        max_length=50,
        description="Search query"
    ),
    skip: int = Query(0, ge=0),
    limit: int = Query(10, ge=1, le=100)
) -> dict[str, Any]:
    results = {"skip": skip, "limit": limit}
    if q:
        results["q"] = q
    return results
```

🔍 **Query Parameter Validation:**

1. `Query(default, min_length=...)` — Minimum string length
2. `Query(default, max_length=...)` — Maximum string length
3. `Query(default, ge=...)` — Greater than or equal (for numbers)
4. `Query(default, le=...)` — Less than or equal (for numbers)
5. `Query(default, description="...")` — Description in documentation

## Combining Path and Query Parameters

```python
from fastapi import FastAPI, Path
from typing import Any

app = FastAPI()

users = {
    1: {"name": "Alice", "age": 25},
    2: {"name": "Bob", "age": 30},
    3: {"name": "Charlie", "age": 35}
}

@app.get("/users/{user_id}/posts")
def get_user_posts(
    user_id: int = Path(..., description="The user's ID"),
    published_only: bool = True,
    skip: int = 0,
    limit: int = 10
) -> dict[str, Any]:
    # Simulate posts
    posts = [
        {"id": 1, "title": "Post 1", "published": True},
        {"id": 2, "title": "Post 2", "published": False},
        {"id": 3, "title": "Post 3", "published": True}
    ]
    
    if published_only:
        posts = [p for p in posts if p["published"]]
    
    return {
        "user_id": user_id,
        "user_name": users.get(user_id, {}).get("name", "Unknown"),
        "posts": posts[skip:skip + limit]
    }
```

## Complex Example: Filter and Sort

```python
from fastapi import FastAPI, Query
from typing import Optional, Any
from enum import Enum

app = FastAPI()

products = [
    {"id": 1, "name": "Laptop", "category": "electronics", "price": 999.99},
    {"id": 2, "name": "Phone", "category": "electronics", "price": 599.99},
    {"id": 3, "name": "Shirt", "category": "clothing", "price": 29.99},
    {"id": 4, "name": "Pants", "category": "clothing", "price": 49.99},
    {"id": 5, "name": "Keyboard", "category": "electronics", "price": 79.99}
]

class SortOrder(str, Enum):
    asc = "asc"
    desc = "desc"

@app.get("/products")
def get_products(
    category: Optional[str] = Query(None, description="Filter by category"),
    min_price: Optional[float] = Query(None, ge=0, description="Minimum price"),
    max_price: Optional[float] = Query(None, ge=0, description="Maximum price"),
    sort_by: str = Query("name", regex="^(name|price|id)$", description="Sort field"),
    sort_order: SortOrder = Query(SortOrder.asc, description="Sort direction")
) -> dict[str, Any]:
    filtered = products.copy()
    
    # Filter by category
    if category:
        filtered = [p for p in filtered if p["category"] == category]
    
    # Filter by price range
    if min_price is not None:
        filtered = [p for p in filtered if p["price"] >= min_price]
    if max_price is not None:
        filtered = [p for p in filtered if p["price"] <= max_price]
    
    # Sort
    reverse = sort_order == SortOrder.desc
    filtered.sort(key=lambda x: x[sort_by], reverse=reverse)
    
    return {
        "count": len(filtered),
        "products": filtered
    }
```

Test these URLs:
- `/products` — All products
- `/products?category=electronics` — Only electronics
- `/products?min_price=50&max_price=500` — Price range
- `/products?sort_by=price&sort_order=desc` — Sorted by price descending

## Multiple Values for Query Parameter

```python
from fastapi import FastAPI, Query

app = FastAPI()

@app.get("/items")
def read_items(
    tags: list[str] = Query(default=[])
) -> dict[str, list[str]]:
    return {"tags": tags}
```

Request:
- `/items?tags=python&tags=fastapi` → `["python", "fastapi"]`

## Summary
- **Path parameters** identify specific resources in the URL path (`/items/42`)
- **Query parameters** filter or modify results (`?skip=0&limit=10`)
- Use **type hints** for automatic conversion
- Use **Query** from FastAPI for validation (min/max length, ge/le)
- Use **Path** for path parameter validation
- Combine both in the same endpoint

## Next Steps
→ Continue to `03-request-body-and-pydantic.md` to learn about request bodies and Pydantic validation.
