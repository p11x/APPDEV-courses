# Parameters Explained

## Overview

FastAPI provides a powerful parameter system that automatically validates, converts, and documents your API parameters. Understanding how parameters work is essential for building robust APIs.

## Parameter Types in FastAPI

### How FastAPI Determines Parameter Source

FastAPI uses a smart system to determine where each parameter comes from:

```python
# Example 1: Parameter source detection
from fastapi import FastAPI, Path, Query, Header, Cookie, Body
from pydantic import BaseModel

app = FastAPI()

class Item(BaseModel):
    name: str
    price: float

@app.put("/items/{item_id}")
async def update_item(
    # Path parameter: extracted from URL path
    item_id: int = Path(..., description="Item ID from URL"),

    # Query parameter: from URL query string
    q: str | None = Query(None, description="Search query"),

    # Header parameter: from HTTP headers
    x_token: str = Header(..., description="Authentication token"),

    # Cookie parameter: from cookies
    session: str = Cookie(None, description="Session cookie"),

    # Body parameter: from JSON request body
    item: Item = Body(..., description="Item data")
):
    """
    FastAPI determines parameter source by:
    1. Type: Pydantic model → Body
    2. Default: Path, Query, Header, Cookie, Body functions
    3. Simple types + path in route → Path
    4. Simple types + not in path → Query
    """
    return {
        "item_id": item_id,
        "query": q,
        "token": x_token,
        "session": session,
        "item": item.model_dump()
    }
```

## Path Parameters

### Basic Path Parameters

```python
# Example 2: Path parameters in detail
from fastapi import FastAPI, Path
from typing import Optional

app = FastAPI()

# Simple path parameter
@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """
    Type hint 'int' automatically:
    - Converts string from URL to integer
    - Validates it's a valid integer
    - Returns 422 if conversion fails
    """
    return {"user_id": user_id, "type": type(user_id).__name__}

# Multiple path parameters
@app.get("/users/{user_id}/posts/{post_id}")
async def get_user_post(
    user_id: int,
    post_id: int
):
    """Multiple path parameters extracted from URL"""
    return {"user_id": user_id, "post_id": post_id}

# Path parameter with validation
@app.get("/items/{item_id}")
async def get_item(
    item_id: int = Path(
        ...,
        title="Item ID",
        description="The unique identifier for the item",
        ge=1,  # greater than or equal to 1
        le=10000,  # less than or equal to 10000
        examples=[1, 42, 100]  # Example values for documentation
    )
):
    """
    Path() provides additional validation and documentation:
    - ge: Greater than or equal
    - gt: Greater than
    - le: Less than or equal
    - lt: Less than
    """
    return {"item_id": item_id}

# String path parameter with regex validation
@app.get("/files/{file_name}")
async def get_file(
    file_name: str = Path(
        ...,
        min_length=1,
        max_length=255,
        regex="^[a-zA-Z0-9_\\-\\.]+$"
    )
):
    """Path parameters can have string validation"""
    return {"file_name": file_name}

# Path type for file paths (allows slashes)
@app.get("/files/{file_path:path}")
async def read_file(file_path: str):
    """
    :path type allows slashes in the parameter.
    URL: /files/home/user/documents/file.txt
    file_path = "home/user/documents/file.txt"
    """
    return {"file_path": file_path}
```

### Path Parameter Types

```python
# Example 3: Different path parameter types
from fastapi import FastAPI
from enum import Enum
from uuid import UUID
from datetime import date

app = FastAPI()

# Enum path parameter
class Category(str, Enum):
    """Restricts to predefined values"""
    ELECTRONICS = "electronics"
    CLOTHING = "clothing"
    BOOKS = "books"

@app.get("/categories/{category}")
async def get_category(category: Category):
    """
    Only accepts: electronics, clothing, books
    Returns 422 for invalid values
    """
    return {
        "category": category,
        "value": category.value
    }

# UUID path parameter
@app.get("/resources/{resource_id}")
async def get_resource(resource_id: UUID):
    """
    UUID type validates UUID format.
    URL: /resources/550e8400-e29b-41d4-a716-446655440000
    """
    return {"resource_id": str(resource_id)}

# Date path parameter
@app.get("/events/{event_date}")
async def get_event(event_date: date):
    """
    Date type parses date strings.
    URL: /events/2024-01-15
    """
    return {"event_date": event_date.isoformat()}

# Float path parameter
@app.get("/coordinates/{lat}/{lon}")
async def get_location(lat: float, lon: float):
    """Float path parameters for coordinates"""
    return {"latitude": lat, "longitude": lon}
```

## Query Parameters

### Basic Query Parameters

```python
# Example 4: Query parameters in detail
from fastapi import FastAPI, Query
from typing import Optional, List

app = FastAPI()

# Simple query parameters with defaults
@app.get("/items/")
async def list_items(
    skip: int = 0,  # Default value 0
    limit: int = 10  # Default value 10
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
    q: Optional[str] = None,  # Optional with None default
    category: Optional[str] = None,
    min_price: Optional[float] = None,
    max_price: Optional[float] = None
):
    """
    Optional parameters can be omitted.
    URL: /search/?q=laptop&min_price=100
    """
    return {
        "query": q,
        "category": category,
        "price_range": {"min": min_price, "max": max_price}
    }

# Required query parameters
@app.get("/filter/")
async def filter_items(
    # ... makes the parameter required
    category: str = Query(..., min_length=1, max_length=50)
):
    """
    Required query parameters must be provided.
    Returns 422 if missing.
    """
    return {"category": category}
```

### Query Parameter Validation

```python
# Example 5: Query parameter validation
from fastapi import FastAPI, Query
from typing import Optional, List

app = FastAPI()

@app.get("/products/")
async def list_products(
    # Number validation
    page: int = Query(
        1,
        ge=1,  # Must be >= 1
        description="Page number (starts at 1)"
    ),
    page_size: int = Query(
        20,
        ge=1,  # Must be >= 1
        le=100,  # Must be <= 100
        description="Items per page (max 100)"
    ),

    # String validation
    sort_by: str = Query(
        "created_at",
        regex="^(name|price|created_at)$",  # Must match pattern
        description="Sort field"
    ),

    # Enum-like validation using Literal
    order: str = Query(
        "desc",
        regex="^(asc|desc)$",
        description="Sort order"
    ),

    # String length validation
    search: Optional[str] = Query(
        None,
        min_length=1,
        max_length=100,
        description="Search query"
    )
):
    """
    Query parameters support various validations:
    - ge/le: Numeric bounds
    - min_length/max_length: String length
    - regex: Pattern matching
    """
    return {
        "page": page,
        "page_size": page_size,
        "sort_by": sort_by,
        "order": order,
        "search": search
    }

# List query parameters
@app.get("/tags/")
async def get_items_by_tags(
    # List of values
    tags: List[str] = Query(
        [],
        description="Filter by tags"
    )
):
    """
    List parameters accept multiple values.
    URL: /tags/?tags=python&tags=fastapi&tags=tutorial
    Result: tags = ["python", "fastapi", "tutorial"]
    """
    return {"tags": tags}

# Query parameter with alias
@app.get("/items/search")
async def search_items(
    # Alias allows different URL parameter name
    search_term: str = Query(
        ...,
        alias="q",  # URL uses 'q' instead of 'search_term'
        min_length=1,
        description="Search term"
    ),
    # Deprecated parameter
    old_param: Optional[str] = Query(
        None,
        deprecated=True,  # Shows as deprecated in docs
        description="Deprecated: use 'q' instead"
    )
):
    """
    Aliases help with URL readability.
    URL: /items/search?q=laptop
    """
    return {"search_term": search_term}
```

### Boolean Query Parameters

```python
# Example 6: Boolean query parameters
from fastapi import FastAPI, Query
from typing import Optional

app = FastAPI()

@app.get("/items/")
async def list_items(
    # Boolean parameters accept various values
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
    Boolean parameters accept:
    - true, True, 1, on, yes → True
    - false, False, 0, off, no → False
    URL: /items/?in_stock=true&featured=false
    """
    return {
        "in_stock": in_stock,
        "featured": featured,
        "include_deleted": include_deleted
    }
```

## Header Parameters

### Reading Headers

```python
# Example 7: Header parameters
from fastapi import FastAPI, Header
from typing import Optional, List

app = FastAPI()

# Simple header parameter
@app.get("/items/")
async def get_items(
    # Header name uses underscores (converted from hyphens)
    x_token: str = Header(..., description="API token"),
    x_request_id: Optional[str] = Header(None, description="Request tracking ID")
):
    """
    Header parameters extract values from HTTP headers.
    FastAPI converts hyphens to underscores.
    X-Token header → x_token parameter
    """
    return {
        "token": x_token,
        "request_id": x_request_id
    }

# User-Agent header
@app.get("/info")
async def get_info(
    user_agent: Optional[str] = Header(None)
):
    """
    Common headers have dedicated parameters.
    User-Agent header provides client information.
    """
    return {"user_agent": user_agent}

# Accept-Language header
@app.get("/content")
async def get_content(
    accept_language: Optional[str] = Header(None, alias="Accept-Language")
):
    """
    Header with alias for exact name matching.
    """
    return {"language": accept_language}

# Multiple header values
@app.get("/multi-header")
async def multi_header(
    # List for headers with multiple values
    x_custom: List[str] = Header([], description="Multiple values")
):
    """
    Headers can have multiple values.
    X-Custom: value1
    X-Custom: value2
    Result: x_custom = ["value1", "value2"]
    """
    return {"values": x_custom}
```

## Cookie Parameters

### Reading Cookies

```python
# Example 8: Cookie parameters
from fastapi import FastAPI, Cookie
from typing import Optional

app = FastAPI()

# Simple cookie parameter
@app.get("/items/")
async def get_items(
    session_id: str = Cookie(..., description="Session identifier")
):
    """
    Cookie parameters extract values from cookies.
    Returns 422 if required cookie is missing.
    """
    return {"session_id": session_id}

# Optional cookies
@app.get("/user/preferences")
async def get_preferences(
    theme: Optional[str] = Cookie("light", description="UI theme"),
    language: Optional[str] = Cookie("en", description="Language preference")
):
    """Optional cookies with default values"""
    return {"theme": theme, "language": language}

# Setting cookies in response
from fastapi import Response

@app.post("/login")
async def login(response: Response, username: str, password: str):
    """Set cookies in the response"""
    # Validate credentials (simplified)
    if username == "admin" and password == "secret":
        response.set_cookie(
            key="session_id",
            value="abc123",
            httponly=True,  # Not accessible via JavaScript
            secure=True,  # Only sent over HTTPS
            samesite="lax",  # CSRF protection
            max_age=3600  # Expires in 1 hour
        )
        return {"message": "Logged in"}

    return {"error": "Invalid credentials"}
```

## Body Parameters

### Request Body with Pydantic

```python
# Example 9: Body parameters with Pydantic models
from fastapi import FastAPI, Body
from pydantic import BaseModel, Field
from typing import Optional, List

app = FastAPI()

# Pydantic model for request body
class Item(BaseModel):
    """Item model for request body"""
    name: str = Field(..., min_length=1, max_length=100)
    description: Optional[str] = None
    price: float = Field(..., gt=0)
    tags: List[str] = []

# Single body parameter
@app.post("/items/")
async def create_item(item: Item):
    """
    Pydantic models are automatically read from request body.
    JSON body is parsed and validated.
    """
    return {"item": item.model_dump()}

# Multiple body parameters
@app.put("/items/{item_id}")
async def update_item(
    item_id: int,
    # Multiple body parameters
    item: Item = Body(...),
    importance: int = Body(..., ge=1, le=5)
):
    """
    Multiple body parameters are combined.
    Request body: {
        "item": {"name": "...", "price": 10.0},
        "importance": 3
    }
    """
    return {"item_id": item_id, "item": item.model_dump(), "importance": importance}

# Body with embed (single object in body)
class ItemCreate(BaseModel):
    name: str
    price: float

@app.post("/items/embedded")
async def create_item_embedded(
    # Body(...) with embed forces single object structure
    item: ItemCreate = Body(..., embed=True)
):
    """
    embed=True requires:
    {"item": {"name": "...", "price": 10.0}}
    Instead of:
    {"name": "...", "price": 10.0}
    """
    return {"item": item.model_dump()}
```

### Nested Body Models

```python
# Example 10: Nested and complex body models
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import Optional, List, Dict
from datetime import datetime

app = FastAPI()

# Nested models
class Address(BaseModel):
    street: str
    city: str
    state: Optional[str] = None
    country: str
    postal_code: str = Field(..., regex="^[0-9]{5}(-[0-9]{4})?$")

class ContactInfo(BaseModel):
    email: str
    phone: Optional[str] = None
    address: Address

class User(BaseModel):
    username: str = Field(..., min_length=3, max_length=50)
    contact: ContactInfo
    tags: List[str] = []
    metadata: Dict[str, str] = {}

@app.post("/users/")
async def create_user(user: User):
    """
    Nested models are validated recursively.
    FastAPI generates complete JSON schema.
    """
    return {"user": user.model_dump()}

# Model with computed fields
class OrderItem(BaseModel):
    product_id: int
    quantity: int = Field(..., gt=0)
    unit_price: float = Field(..., gt=0)

class Order(BaseModel):
    items: List[OrderItem]
    discount: float = Field(0.0, ge=0, le=1)
    created_at: datetime = Field(default_factory=datetime.now)

    @property
    def subtotal(self) -> float:
        """Calculate subtotal"""
        return sum(item.quantity * item.unit_price for item in self.items)

    @property
    def total(self) -> float:
        """Calculate total with discount"""
        return self.subtotal * (1 - self.discount)

@app.post("/orders/")
async def create_order(order: Order):
    """Order with calculated totals"""
    return {
        "order": order.model_dump(),
        "subtotal": order.subtotal,
        "total": order.total
    }
```

## Combining Parameter Types

### Complete Parameter Example

```python
# Example 11: Combining all parameter types
from fastapi import FastAPI, Path, Query, Header, Cookie, Body, Depends
from pydantic import BaseModel, Field
from typing import Optional, List

app = FastAPI()

class Pagination:
    """Dependency for pagination"""
    def __init__(
        self,
        skip: int = Query(0, ge=0),
        limit: int = Query(20, ge=1, le=100)
    ):
        self.skip = skip
        self.limit = limit

class ItemCreate(BaseModel):
    name: str
    price: float
    tags: List[str] = []

class Item(ItemCreate):
    id: int

@app.put("/stores/{store_id}/items/{item_id}")
async def update_store_item(
    # Path parameters
    store_id: int = Path(..., ge=1, description="Store ID"),
    item_id: int = Path(..., ge=1, description="Item ID"),

    # Query parameters via dependency
    pagination: Pagination = Depends(),

    # Header parameters
    x_api_key: str = Header(..., description="API key"),
    x_request_id: Optional[str] = Header(None),

    # Cookie parameters
    session_id: Optional[str] = Cookie(None),

    # Body parameter
    item: ItemCreate = Body(...)
):
    """
    Complete endpoint demonstrating all parameter types.
    FastAPI handles:
    1. Path extraction and validation
    2. Query parameter parsing
    3. Header extraction
    4. Cookie reading
    5. Body parsing and validation
    """
    return {
        "store_id": store_id,
        "item_id": item_id,
        "pagination": {"skip": pagination.skip, "limit": pagination.limit},
        "api_key": x_api_key[:10] + "...",  # Masked
        "item": item.model_dump()
    }
```

## Parameter Documentation

### Adding Documentation

```python
# Example 12: Parameter documentation
from fastapi import FastAPI, Query, Path, Body
from pydantic import BaseModel, Field
from typing import Optional

app = FastAPI()

@app.get("/items/{item_id}")
async def get_item(
    item_id: int = Path(
        ...,
        title="Item ID",
        description="The unique identifier of the item to retrieve",
        examples={
            "valid": {"summary": "A valid item ID", "value": 42},
            "another": {"summary": "Another valid ID", "value": 100}
        }
    ),
    include_details: bool = Query(
        False,
        title="Include Details",
        description="Whether to include extended details in the response"
    )
):
    """
    Parameters with detailed documentation appear in:
    - Swagger UI (/docs)
    - ReDoc (/redoc)
    - OpenAPI schema
    """
    return {"item_id": item_id, "include_details": include_details}

class Item(BaseModel):
    """Model with field documentation"""
    name: str = Field(
        ...,
        title="Item Name",
        description="The display name of the item",
        examples=["Laptop", "Phone", "Tablet"]
    )
    price: float = Field(
        ...,
        title="Item Price",
        description="Price in USD",
        gt=0,
        examples=[999.99, 49.99]
    )

@app.post("/items/")
async def create_item(
    item: Item = Body(
        ...,
        examples={
            "laptop": {
                "summary": "A laptop example",
                "value": {"name": "MacBook Pro", "price": 1999.99}
            },
            "phone": {
                "summary": "A phone example",
                "value": {"name": "iPhone", "price": 999.99}
            }
        }
    )
):
    """Body with examples shown in documentation"""
    return item
```

## Parameter Best Practices

### Validation Patterns

```python
# Example 13: Common validation patterns
from fastapi import FastAPI, Query, Path
from typing import Optional
from datetime import date

app = FastAPI()

# ID validation pattern
@app.get("/items/{item_id}")
async def get_item(
    item_id: int = Path(..., ge=1, description="Positive item ID")
):
    """Always validate IDs are positive"""
    return {"item_id": item_id}

# Pagination pattern
@app.get("/items/")
async def list_items(
    page: int = Query(1, ge=1, description="Page number"),
    per_page: int = Query(20, ge=1, le=100, description="Items per page")
):
    """Standard pagination parameters"""
    skip = (page - 1) * per_page
    return {"page": page, "per_page": per_page, "skip": skip}

# Date range pattern
@app.get("/reports/")
async def get_report(
    start_date: date = Query(..., description="Start date (YYYY-MM-DD)"),
    end_date: date = Query(..., description="End date (YYYY-MM-DD)")
):
    """Date range validation"""
    if end_date < start_date:
        from fastapi import HTTPException
        raise HTTPException(400, "end_date must be after start_date")
    return {"start": start_date, "end": end_date}

# Search pattern
@app.get("/search/")
async def search(
    q: str = Query(
        ...,
        min_length=1,
        max_length=200,
        description="Search query"
    ),
    limit: int = Query(10, ge=1, le=50)
):
    """Search with query validation"""
    return {"query": q, "limit": limit}

# Sorting pattern
from enum import Enum

class SortField(str, Enum):
    NAME = "name"
    PRICE = "price"
    DATE = "created_at"

class SortOrder(str, Enum):
    ASC = "asc"
    DESC = "desc"

@app.get("/products/")
async def list_products(
    sort_by: SortField = Query(SortField.DATE),
    order: SortOrder = Query(SortOrder.DESC)
):
    """Sorting with enums"""
    return {"sort_by": sort_by, "order": order}
```

### Common Pitfalls

```python
# Example 14: Common parameter mistakes

from fastapi import FastAPI, Query, Path

app = FastAPI()

# PITFALL 1: Missing default for optional parameters
# BAD:
# @app.get("/items/")
# async def list_items(skip: int):  # This is required, not optional!

# GOOD:
@app.get("/items/")
async def list_items(skip: int = 0):  # Has default, so optional
    return {"skip": skip}

# PITFALL 2: Path parameter not in route
# BAD:
# @app.get("/items/")
# async def get_item(item_id: int):  # item_id is query, not path!

# GOOD:
@app.get("/items/{item_id}")
async def get_item(item_id: int):  # Now it's a path parameter
    return {"item_id": item_id}

# PITFALL 3: Wrong type hint
# BAD:
# @app.get("/items/{item_id}")
# async def get_item(item_id: str):  # Accepts any string

# GOOD:
@app.get("/items/{item_id}")
async def get_item(item_id: int):  # Validates it's an integer
    return {"item_id": item_id}

# PITFALL 4: Forgetting Optional for nullable parameters
# BAD:
# @app.get("/search/")
# async def search(q: str = None):  # Won't work as expected

# GOOD:
from typing import Optional
@app.get("/search/")
async def search(q: Optional[str] = None):  # Properly optional
    return {"query": q}

# PITFALL 5: Validation on wrong parameter
# BAD:
# @app.get("/items/{item_id}")
# async def get_item(item_id: int = Query(..., ge=1)):  # Query, not Path!

# GOOD:
from fastapi import Path
@app.get("/items/{item_id}")
async def get_item(item_id: int = Path(..., ge=1)):  # Path validation
    return {"item_id": item_id}
```

## Summary

| Parameter Type | Source | Example URL | Use Case |
|---------------|--------|-------------|----------|
| Path | URL path | `/items/42` | Resource identification |
| Query | After `?` | `/items/?skip=10` | Filtering, pagination |
| Header | HTTP headers | `X-Token: abc123` | Auth, metadata |
| Cookie | Cookies | `session=xyz` | Sessions, preferences |
| Body | Request body | `{"name": "..."}` | Creating/updating data |

## Next Steps

Continue learning about:
- [Path Operations](../../02_core_features/01_path_operations/01_basic_routes.md) - Advanced routing
- [Query Parameters](../../02_core_features/01_path_operations/03_query_parameters.md) - Deep dive into queries
- [Request Body](../../02_core_features/03_request_body/01_basic_request_body.md) - Working with bodies
