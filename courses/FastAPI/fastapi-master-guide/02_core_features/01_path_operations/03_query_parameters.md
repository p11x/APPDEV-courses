# Query Parameters

## Overview

Query parameters appear after the `?` in a URL and are used for filtering, pagination, sorting, and optional configuration. FastAPI provides powerful validation and documentation for query parameters.

## Basic Query Parameters

### Simple Query Parameters

```python
# Example 1: Basic query parameters
from fastapi import FastAPI, Query
from typing import Optional

app = FastAPI()

# Query parameters with defaults
@app.get("/items/")
async def list_items(
    skip: int = 0,    # Default: 0
    limit: int = 10   # Default: 10
):
    """
    Query parameters come after '?' in URL.
    URL: /items/?skip=20&limit=50
    If not provided, uses default values.
    """
    return {"skip": skip, "limit": limit}

# Optional query parameters
@app.get("/search/")
async def search(
    q: Optional[str] = None,        # Optional search query
    category: Optional[str] = None,  # Optional category filter
    min_price: Optional[float] = None,
    max_price: Optional[float] = None
):
    """
    Optional parameters default to None.
    URL: /search/?q=laptop&category=electronics
    """
    return {
        "query": q,
        "category": category,
        "price_range": {"min": min_price, "max": max_price}
    }

# Required query parameters
@app.get("/filter/")
async def filter_items(
    # ... (Ellipsis) makes the parameter required
    category: str = Query(..., description="Category to filter by")
):
    """
    Required query parameters must be provided.
    Returns 422 if missing.
    """
    return {"category": category}
```

### Query Parameter Validation

```python
# Example 2: Query parameter constraints
from fastapi import FastAPI, Query
from typing import Optional

app = FastAPI()

@app.get("/products/")
async def list_products(
    # Numeric validation
    page: int = Query(
        1,
        ge=1,                    # >= 1
        description="Page number (starts at 1)"
    ),
    page_size: int = Query(
        20,
        ge=1,                    # >= 1
        le=100,                  # <= 100
        description="Items per page (max 100)"
    ),

    # String validation
    sort_by: str = Query(
        "created_at",
        regex="^(name|price|created_at)$",  # Must match pattern
        description="Sort field"
    ),

    # String length validation
    search: Optional[str] = Query(
        None,
        min_length=1,
        max_length=200,
        description="Search query (1-200 chars)"
    )
):
    """
    Validation options:
    - ge/le: Numeric bounds
    - min_length/max_length: String length
    - regex: Pattern matching
    """
    return {
        "page": page,
        "page_size": page_size,
        "sort_by": sort_by,
        "search": search
    }
```

## List Query Parameters

### Multiple Values

```python
# Example 3: List query parameters
from fastapi import FastAPI, Query
from typing import List, Optional

app = FastAPI()

@app.get("/items/")
async def get_items_by_tags(
    # List of values from repeated query params
    tags: List[str] = Query(
        [],
        description="Filter by tags"
    )
):
    """
    URL: /items/?tags=python&tags=fastapi&tags=tutorial
    Result: tags = ["python", "fastapi", "tutorial"]
    """
    return {"tags": tags}

@app.get("/products/")
async def list_products(
    categories: List[str] = Query(
        ["electronics"],  # Default value
        description="Product categories"
    ),
    price_ranges: List[float] = Query(
        [],
        min_length=0,
        max_length=5,
        description="Price range filters"
    )
):
    """
    Lists with defaults and length validation.
    URL: /products/?categories=clothing&categories=home
    """
    return {"categories": categories, "price_ranges": price_ranges}
```

## Boolean Parameters

### Boolean Query Parameters

```python
# Example 4: Boolean query parameters
from fastapi import FastAPI, Query
from typing import Optional

app = FastAPI()

@app.get("/items/")
async def list_items(
    in_stock: bool = Query(
        False,
        description="Filter by stock status"
    ),
    featured: Optional[bool] = Query(
        None,
        description="Filter featured items"
    ),
    include_deleted: bool = Query(
        False,
        alias="includeDeleted",
        description="Include soft-deleted items"
    )
):
    """
    Boolean parameters accept various values:
    - True: true, True, 1, on, yes
    - False: false, False, 0, off, no
    
    URL: /items/?in_stock=true&featured=false
    """
    return {
        "in_stock": in_stock,
        "featured": featured,
        "include_deleted": include_deleted
    }
```

## Parameter Aliases and Deprecation

### Aliases and Deprecation

```python
# Example 5: Aliases and deprecated parameters
from fastapi import FastAPI, Query
from typing import Optional

app = FastAPI()

@app.get("/items/")
async def list_items(
    # Alias for URL parameter name
    search_term: str = Query(
        ...,
        alias="q",  # URL uses 'q' instead of 'search_term'
        description="Search term"
    ),
    # Deprecated parameter
    old_filter: Optional[str] = Query(
        None,
        deprecated=True,
        description="Deprecated: use 'filter' instead"
    ),
    # Include in docs but hidden
    internal_flag: bool = Query(
        False,
        include_in_schema=False  # Not shown in API docs
    )
):
    """
    Aliases help with URL readability.
    URL: /items/?q=laptop
    
    Deprecated parameters show warnings in docs.
    """
    return {"search_term": search_term, "old_filter": old_filter}
```

## Query vs Path Parameters

### Combining Both

```python
# Example 6: Query and path parameters together
from fastapi import FastAPI, Path, Query
from typing import Optional

app = FastAPI()

@app.get("/users/{user_id}/orders")
async def get_user_orders(
    # Path parameter: from URL path
    user_id: int = Path(..., ge=1, description="User ID"),
    # Query parameters: from URL query string
    status: Optional[str] = Query(None, description="Order status filter"),
    limit: int = Query(10, ge=1, le=50),
    offset: int = Query(0, ge=0)
):
    """
    Path params: resource identification
    Query params: filtering, pagination
    
    URL: /users/42/orders?status=completed&limit=20
    """
    return {
        "user_id": user_id,
        "status": status,
        "limit": limit,
        "offset": offset
    }

@app.get("/stores/{store_id}/products/{category}")
async def get_store_products(
    store_id: int = Path(..., ge=1),
    category: str = Path(...),
    sort: str = Query("name", regex="^(name|price|date)$"),
    order: str = Query("asc", regex="^(asc|desc)$")
):
    """Multiple path params with query params"""
    return {
        "store_id": store_id,
        "category": category,
        "sort": sort,
        "order": order
    }
```

## Advanced Query Parameters

### Complex Validation

```python
# Example 7: Advanced query parameter patterns
from fastapi import FastAPI, Query, HTTPException
from typing import Optional, List
from datetime import date
from enum import Enum

app = FastAPI()

class SortField(str, Enum):
    NAME = "name"
    PRICE = "price"
    DATE = "created_at"

class SortOrder(str, Enum):
    ASC = "asc"
    DESC = "desc"

@app.get("/products/")
async def list_products(
    # Enum parameters
    sort_by: SortField = Query(SortField.DATE),
    order: SortOrder = Query(SortOrder.DESC),

    # Date range
    start_date: Optional[date] = Query(None),
    end_date: Optional[date] = Query(None),

    # Multiple filters
    categories: List[str] = Query([]),
    min_price: Optional[float] = Query(None, ge=0),
    max_price: Optional[float] = Query(None, ge=0),

    # Pagination
    page: int = Query(1, ge=1),
    page_size: int = Query(20, ge=1, le=100)
):
    """
    Complex filtering with multiple parameter types.
    URL: /products/?sort_by=price&order=asc&categories=electronics&min_price=100
    """
    # Validate date range
    if start_date and end_date and end_date < start_date:
        raise HTTPException(400, "end_date must be after start_date")

    # Validate price range
    if min_price and max_price and max_price < min_price:
        raise HTTPException(400, "max_price must be >= min_price")

    return {
        "sort": {"field": sort_by.value, "order": order.value},
        "date_range": {"start": start_date, "end": end_date},
        "filters": {
            "categories": categories,
            "price": {"min": min_price, "max": max_price}
        },
        "pagination": {"page": page, "page_size": page_size}
    }
```

## Query Parameter Patterns

### Pagination Pattern

```python
# Example 8: Pagination patterns
from fastapi import FastAPI, Query
from typing import List, Dict, Any

app = FastAPI()

# Offset-based pagination
@app.get("/items/offset/")
async def list_items_offset(
    offset: int = Query(0, ge=0, description="Number of items to skip"),
    limit: int = Query(20, ge=1, le=100, description="Max items to return")
):
    """
    Traditional pagination: skip N items, return M items.
    URL: /items/offset/?offset=40&limit=20
    """
    # Simulated data
    all_items = list(range(100))
    items = all_items[offset:offset + limit]

    return {
        "items": items,
        "pagination": {
            "offset": offset,
            "limit": limit,
            "total": len(all_items),
            "has_more": offset + limit < len(all_items)
        }
    }

# Page-based pagination
@app.get("/items/page/")
async def list_items_page(
    page: int = Query(1, ge=1, description="Page number"),
    per_page: int = Query(20, ge=1, le=100, description="Items per page")
):
    """
    Page-based pagination: more user-friendly.
    URL: /items/page/?page=3&per_page=20
    """
    all_items = list(range(100))
    total_pages = (len(all_items) + per_page - 1) // per_page
    offset = (page - 1) * per_page
    items = all_items[offset:offset + per_page]

    return {
        "items": items,
        "pagination": {
            "page": page,
            "per_page": per_page,
            "total_items": len(all_items),
            "total_pages": total_pages,
            "has_next": page < total_pages,
            "has_prev": page > 1
        }
    }

# Cursor-based pagination
@app.get("/items/cursor/")
async def list_items_cursor(
    cursor: Optional[str] = Query(None, description="Pagination cursor"),
    limit: int = Query(20, ge=1, le=100)
):
    """
    Cursor-based pagination: efficient for large datasets.
    URL: /items/cursor/?cursor=abc123&limit=20
    """
    # In real app, decode cursor to get position
    start = int(cursor) if cursor else 0
    all_items = list(range(1000))
    items = all_items[start:start + limit]
    next_cursor = str(start + limit) if start + limit < len(all_items) else None

    return {
        "items": items,
        "pagination": {
            "next_cursor": next_cursor,
            "limit": limit,
            "has_more": next_cursor is not None
        }
    }
```

### Sorting Pattern

```python
# Example 9: Sorting patterns
from fastapi import FastAPI, Query
from typing import Optional, List
from enum import Enum

app = FastAPI()

class SortField(str, Enum):
    NAME = "name"
    PRICE = "price"
    CREATED = "created_at"
    UPDATED = "updated_at"

@app.get("/products/")
async def list_products(
    sort_by: SortField = Query(
        SortField.CREATED,
        description="Field to sort by"
    ),
    order: str = Query(
        "desc",
        regex="^(asc|desc)$",
        description="Sort order"
    )
):
    """
    Sorting with enum validation.
    URL: /products/?sort_by=price&order=asc
    """
    return {
        "sort": {
            "field": sort_by.value,
            "order": order
        },
        "products": []
    }

# Multiple sort fields
@app.get("/items/")
async def list_items(
    sort: List[str] = Query(
        ["-created_at"],  # Default: newest first (- prefix = desc)
        description="Sort fields (prefix - for descending)"
    )
):
    """
    Multiple sort fields: ?sort=name&sort=-price
    Prefix - means descending.
    """
    return {"sort": sort}
```

## Error Handling

### Query Parameter Errors

```python
# Example 10: Query parameter error scenarios
from fastapi import FastAPI, Query, HTTPException
from typing import Optional

app = FastAPI()

@app.get("/items/")
async def list_items(
    page: int = Query(..., ge=1),
    limit: int = Query(10, ge=1, le=100)
):
    """
    Error scenarios:
    - /items/ → 422 (page is required)
    - /items/?page=abc → 422 (not an integer)
    - /items/?page=0 → 422 (less than 1)
    - /items/?page=1&limit=200 → 422 (limit exceeds max)
    """
    return {"page": page, "limit": limit}

# Error response format:
"""
Request: GET /items/?page=abc
Response: 422 Unprocessable Entity
{
    "detail": [
        {
            "loc": ["query", "page"],
            "msg": "Input should be a valid integer",
            "type": "int_parsing"
        }
    ]
}

Request: GET /items/?page=1&limit=200
Response: 422 Unprocessable Entity
{
    "detail": [
        {
            "loc": ["query", "limit"],
            "msg": "Input should be less than or equal to 100",
            "type": "less_than_equal"
        }
    ]
}
"""
```

## Best Practices

### Query Parameter Guidelines

```python
# Example 11: Best practices
from fastapi import FastAPI, Query
from typing import Optional, List

app = FastAPI()

# GOOD: Use descriptive names
@app.get("/users/")
async def list_users(
    is_active: Optional[bool] = Query(None),
    search_query: Optional[str] = Query(None, alias="q"),
    page_size: int = Query(20, le=100)
):
    """Clear, descriptive parameter names"""
    return {"users": []}

# GOOD: Provide defaults for optional parameters
@app.get("/items/")
async def list_items(
    skip: int = Query(0, ge=0),
    limit: int = Query(20, ge=1, le=100)
):
    """All parameters have sensible defaults"""
    return {"items": []}

# GOOD: Use aliases for shorter URLs
@app.get("/search/")
async def search(
    query: str = Query(..., alias="q", min_length=1),
    page: int = Query(1, alias="p", ge=1)
):
    """
    URL: /search/?q=laptop&p=2
    Shorter than: /search/?query=laptop&page=2
    """
    return {"query": query, "page": page}

# GOOD: Document parameters
@app.get("/reports/")
async def get_report(
    start_date: str = Query(
        ...,
        description="Start date in ISO format (YYYY-MM-DD)",
        examples=["2024-01-01", "2024-06-15"]
    ),
    end_date: str = Query(
        ...,
        description="End date in ISO format (YYYY-MM-DD)",
        examples=["2024-12-31", "2024-06-30"]
    )
):
    """Well-documented parameters help API consumers"""
    return {"start": start_date, "end": end_date}
```

## Summary

| Feature | Description | Example |
|---------|-------------|---------|
| Default values | Optional with fallback | `skip: int = 0` |
| Required | Must be provided | `q: str = Query(...)` |
| Validation | Constraints on values | `ge=1, le=100` |
| Lists | Multiple values | `tags: List[str]` |
| Aliases | Shorter URL names | `alias="q"` |
| Deprecation | Mark old params | `deprecated=True` |

## Next Steps

Continue learning about:
- [Header Parameters](./04_header_parameters.md) - HTTP headers
- [Complex Routing](./05_complex_routing.md) - Advanced routing
- [Request Body](../03_request_body/01_basic_request_body.md) - JSON body handling
