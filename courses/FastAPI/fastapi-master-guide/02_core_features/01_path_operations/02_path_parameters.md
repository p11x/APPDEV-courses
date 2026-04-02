# Path Parameters

## Overview

Path parameters extract values from the URL path itself. They are essential for creating dynamic routes that handle individual resources, hierarchies, and complex URL structures.

## Basic Path Parameters

### Simple Path Parameters

```python
# Example 1: Basic path parameter types
from fastapi import FastAPI, Path
from typing import Optional

app = FastAPI()

# Integer path parameter
@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """
    Type hint 'int' provides:
    - Automatic string-to-int conversion
    - Validation (rejects non-integers)
    - Documentation in OpenAPI schema
    """
    return {"user_id": user_id, "type": type(user_id).__name__}

# String path parameter
@app.get("/files/{file_name}")
async def get_file(file_name: str):
    """String path parameters accept any string value"""
    return {"file_name": file_name}

# Float path parameter
@app.get("/coordinates/{latitude}/{longitude}")
async def get_location(latitude: float, longitude: float):
    """Multiple path parameters of any type"""
    return {"latitude": latitude, "longitude": longitude}

# Boolean path parameter
@app.get("/features/{feature_enabled}")
async def check_feature(feature_enabled: bool):
    """
    Boolean path parameters:
    - true, True, 1 → True
    - false, False, 0 → False
    """
    return {"feature_enabled": feature_enabled}
```

### Path Parameter Validation

```python
# Example 2: Path parameter constraints
from fastapi import FastAPI, Path

app = FastAPI()

@app.get("/items/{item_id}")
async def get_item(
    item_id: int = Path(
        ...,
        title="Item ID",
        description="The unique identifier for the item. Must be between 1 and 10000.",
        ge=1,           # Greater than or equal to 1
        le=10000,       # Less than or equal to 10000
        examples=[1, 42, 999]  # Example values for documentation
    )
):
    """
    Path() provides validation and documentation:
    - ge: Greater than or equal
    - gt: Greater than
    - le: Less than or equal
    - lt: Less than
    """
    return {"item_id": item_id}

@app.get("/users/{username}")
async def get_user(
    username: str = Path(
        ...,
        min_length=3,
        max_length=20,
        regex="^[a-zA-Z0-9_]+$",
        description="Username (3-20 alphanumeric characters or underscore)"
    )
):
    """String path parameter with pattern validation"""
    return {"username": username}

@app.get("/products/{product_code}")
async def get_product(
    product_code: str = Path(
        ...,
        regex="^[A-Z]{2}-\\d{4}$",
        description="Product code format: XX-0000"
    )
):
    """
    Only matches codes like: AB-1234, XY-9999
    Returns 422 for invalid formats
    """
    return {"product_code": product_code}
```

## Special Path Types

### Enum Path Parameters

```python
# Example 3: Enum-based path parameters
from fastapi import FastAPI
from enum import Enum

app = FastAPI()

class Category(str, Enum):
    """
    String enum restricts to predefined values.
    Using str ensures JSON serialization works.
    """
    ELECTRONICS = "electronics"
    CLOTHING = "clothing"
    BOOKS = "books"
    HOME = "home"
    SPORTS = "sports"

class SortOrder(str, Enum):
    ASC = "asc"
    DESC = "desc"

@app.get("/products/{category}")
async def get_products_by_category(category: Category):
    """
    Only accepts: electronics, clothing, books, home, sports
    Returns 422 for invalid values
    """
    return {
        "category": category,
        "value": category.value,
        "message": f"Products in {category.value} category"
    }

@app.get("/products/{category}/sorted/{order}")
async def get_sorted_products(category: Category, order: SortOrder):
    """Multiple enum path parameters"""
    return {
        "category": category.value,
        "order": order.value,
        "products": []
    }

# Access enum values in handler
@app.get("/categories")
async def list_categories():
    """List all available categories from enum"""
    return {
        "categories": [
            {"value": cat.value, "name": cat.name}
            for cat in Category
        ]
    }
```

### UUID Path Parameters

```python
# Example 4: UUID path parameters
from fastapi import FastAPI
from uuid import UUID, uuid4

app = FastAPI()

@app.get("/resources/{resource_id}")
async def get_resource(resource_id: UUID):
    """
    UUID type validates UUID format automatically.
    URL: /resources/550e8400-e29b-41d4-a716-446655440000
    """
    return {
        "resource_id": str(resource_id),
        "version": resource_id.version  # UUID version (1, 4, etc.)
    }

@app.post("/resources/")
async def create_resource():
    """Create resource with generated UUID"""
    resource_id = uuid4()
    return {
        "resource_id": str(resource_id),
        "message": "Resource created"
    }

# UUID in nested resources
@app.get("/organizations/{org_id}/members/{member_id}")
async def get_member(org_id: UUID, member_id: UUID):
    """Multiple UUID path parameters for hierarchical resources"""
    return {
        "organization_id": str(org_id),
        "member_id": str(member_id)
    }
```

### Date Path Parameters

```python
# Example 5: Date and datetime path parameters
from fastapi import FastAPI
from datetime import date, datetime

app = FastAPI()

@app.get("/events/{event_date}")
async def get_events_by_date(event_date: date):
    """
    Date type parses ISO format: 2024-01-15
    URL: /events/2024-01-15
    """
    return {
        "date": event_date.isoformat(),
        "weekday": event_date.strftime("%A")
    }

@app.get("/reports/{report_date}")
async def get_report(report_date: date):
    """
    Date parameters validate proper date formats.
    Returns 422 for invalid dates like 2024-13-45
    """
    return {"report_date": report_date}

@app.get("/logs/{timestamp}")
async def get_logs(timestamp: datetime):
    """
    Datetime type parses ISO format: 2024-01-15T10:30:00
    URL: /logs/2024-01-15T10:30:00
    """
    return {
        "timestamp": timestamp.isoformat(),
        "date": timestamp.date().isoformat(),
        "time": timestamp.time().isoformat()
    }
```

### File Path Parameters

```python
# Example 6: File path with slashes
from fastapi import FastAPI

app = FastAPI()

@app.get("/files/{file_path:path}")
async def read_file(file_path: str):
    """
    :path type allows slashes in the parameter.
    URL: /files/home/user/documents/file.txt
    file_path = "home/user/documents/file.txt"
    """
    return {
        "file_path": file_path,
        "parts": file_path.split("/")
    }

@app.get("/repos/{owner}/{repo}/files/{path:path}")
async def get_repo_file(owner: str, repo: str, path: str):
    """
    Combining regular and path parameters.
    URL: /repos/fastapi/fastapi/files/docs/en/index.md
    """
    return {
        "owner": owner,
        "repo": repo,
        "file_path": path
    }
```

## Multiple Path Parameters

### Hierarchical Routes

```python
# Example 7: Multiple path parameters for nested resources
from fastapi import FastAPI
from uuid import UUID

app = FastAPI()

@app.get("/organizations/{org_id}/projects/{project_id}/tasks/{task_id}")
async def get_task(org_id: UUID, project_id: int, task_id: int):
    """
    Hierarchical resource access.
    URL: /organizations/550e8400-.../projects/42/tasks/7
    """
    return {
        "organization_id": str(org_id),
        "project_id": project_id,
        "task_id": task_id
    }

@app.get("/users/{user_id}/posts/{post_id}/comments/{comment_id}")
async def get_comment(user_id: int, post_id: int, comment_id: int):
    """Social media style nested resources"""
    return {
        "user_id": user_id,
        "post_id": post_id,
        "comment_id": comment_id
    }

# Combining with query parameters
@app.get("/stores/{store_id}/products/{product_id}")
async def get_store_product(
    store_id: int,
    product_id: int,
    include_reviews: bool = False
):
    """
    Path parameters for resource identification.
    Query parameter for optional data.
    """
    result = {
        "store_id": store_id,
        "product_id": product_id
    }
    if include_reviews:
        result["reviews"] = []
    return result
```

## Path Parameter Documentation

### Enhanced Documentation

```python
# Example 8: Comprehensive path parameter documentation
from fastapi import FastAPI, Path
from typing import Optional

app = FastAPI()

@app.get(
    "/items/{item_id}",
    summary="Get an item",
    description="Retrieve a specific item by its unique identifier.",
    response_description="The requested item",
    responses={
        200: {"description": "Item found"},
        404: {"description": "Item not found"},
        422: {"description": "Invalid item ID"}
    }
)
async def get_item(
    item_id: int = Path(
        ...,
        title="Item ID",
        description="Unique numeric identifier for the item",
        examples={
            "basic": {
                "summary": "A typical item ID",
                "value": 42
            },
            "large": {
                "summary": "A large item ID",
                "value": 9999
            }
        }
    )
):
    """
    ## Path Parameter Details

    The `item_id` parameter:
    - Must be a positive integer
    - Range: 1 to 10000
    - Used to look up items in the database

    ### Example Requests
    - GET /items/42
    - GET /items/999
    """
    return {"item_id": item_id}
```

## Common Patterns

### Resource Identification Patterns

```python
# Example 9: Common path parameter patterns
from fastapi import FastAPI, Path
from typing import Literal
import re

app = FastAPI()

# Pattern 1: Sequential IDs
@app.get("/items/{item_id}")
async def get_item_by_id(item_id: int = Path(..., ge=1)):
    """Simple numeric ID pattern"""
    return {"item_id": item_id}

# Pattern 2: Slugs (URL-friendly strings)
@app.get("/articles/{slug}")
async def get_article(
    slug: str = Path(
        ...,
        regex="^[a-z0-9]+(?:-[a-z0-9]+)*$",
        description="URL-friendly slug (lowercase, hyphens)"
    )
):
    """
    Slug pattern: my-article-title
    - Lowercase letters and numbers
    - Hyphens as separators
    """
    return {"slug": slug}

# Pattern 3: Composite keys
@app.get("/versions/{major}.{minor}.{patch}")
async def get_version(major: int, minor: int, patch: int):
    """
    Note: FastAPI doesn't support dots in path params directly.
    Use alternative approaches like:
    - /versions/1.2.3 (as string, parse manually)
    - /versions/1/2/3 (hierarchical)
    - /versions/1-2-3 (hyphenated)
    """
    return {"version": f"{major}.{minor}.{patch}"}

# Alternative with string parsing
@app.get("/versions/{version}")
async def get_version_string(version: str):
    """Parse version string manually"""
    parts = version.split(".")
    if len(parts) != 3 or not all(p.isdigit() for p in parts):
        return {"error": "Invalid version format"}
    return {
        "version": version,
        "major": int(parts[0]),
        "minor": int(parts[1]),
        "patch": int(parts[2])
    }
```

## Error Handling

### Path Parameter Errors

```python
# Example 10: Path parameter error scenarios
from fastapi import FastAPI, Path, HTTPException

app = FastAPI()

@app.get("/items/{item_id}")
async def get_item(item_id: int = Path(..., ge=1)):
    """
    Error scenarios:
    - /items/abc → 422 (not an integer)
    - /items/0 → 422 (less than 1)
    - /items/-5 → 422 (less than 1)
    - /items/99999999999999999999 → 422 (too large)
    """
    if item_id > 10000:
        raise HTTPException(
            status_code=404,
            detail=f"Item {item_id} not found"
        )
    return {"item_id": item_id}

# Error response format:
"""
Request: GET /items/abc
Response: 422 Unprocessable Entity
{
    "detail": [
        {
            "loc": ["path", "item_id"],
            "msg": "Input should be a valid integer, unable to parse string as an integer",
            "type": "int_parsing"
        }
    ]
}

Request: GET /items/0
Response: 422 Unprocessable Entity
{
    "detail": [
        {
            "loc": ["path", "item_id"],
            "msg": "Input should be greater than or equal to 1",
            "type": "greater_than_equal"
        }
    ]
}
"""
```

## Best Practices

### Path Parameter Guidelines

```python
# Example 11: Best practices for path parameters
from fastapi import FastAPI, Path
from uuid import UUID

app = FastAPI()

# GOOD: Use meaningful names
@app.get("/users/{user_id}/orders/{order_id}")
async def get_user_order(user_id: int, order_id: int):
    """Clear, descriptive parameter names"""
    return {"user_id": user_id, "order_id": order_id}

# AVOID: Abbreviated names
# @app.get("/users/{uid}/orders/{oid}")
# def get_order(uid: int, oid: int):

# GOOD: Validate IDs
@app.get("/items/{item_id}")
async def get_item(
    item_id: int = Path(..., ge=1, le=100000)
):
    """Always validate path parameters"""
    return {"item_id": item_id}

# GOOD: Use UUIDs for public-facing IDs
@app.get("/resources/{resource_id}")
async def get_resource(resource_id: UUID):
    """
    UUIDs prevent:
    - Enumeration attacks (guessing valid IDs)
    - Information leakage (sequential IDs reveal count)
    """
    return {"resource_id": str(resource_id)}

# GOOD: Use enums for fixed categories
from enum import Enum

class Status(str, Enum):
    ACTIVE = "active"
    INACTIVE = "inactive"
    PENDING = "pending"

@app.get("/users/status/{status}")
async def get_users_by_status(status: Status):
    """Enums prevent invalid values"""
    return {"status": status.value}
```

## Performance Considerations

```python
# Example 12: Path parameter performance
from fastapi import FastAPI, Path

app = FastAPI()

# Simple path parameters are fast
@app.get("/fast/{item_id}")
async def fast_endpoint(item_id: int):
    """Integer parsing is very fast"""
    return {"item_id": item_id}

# Regex validation adds overhead
@app.get("/validated/{code}")
async def validated_endpoint(
    code: str = Path(..., regex="^[A-Z]{3}-\\d{6}$")
):
    """
    Regex validation is slower but provides better validation.
    Use for inputs that need strict format enforcement.
    """
    return {"code": code}

# Prefer built-in types over custom validation
@app.get("/items/{item_id}")
async def get_item(item_id: int = Path(..., ge=1)):
    """Built-in ge/le checks are faster than custom validators"""
    return {"item_id": item_id}
```

## Summary

| Parameter Type | Syntax | Example URL | Use Case |
|----------------|--------|-------------|----------|
| Integer | `{id: int}` | `/items/42` | Numeric IDs |
| String | `{name: str}` | `/files/readme.txt` | Names, slugs |
| Float | `{lat: float}` | `/geo/40.7/-74.0` | Coordinates |
| UUID | `{id: UUID}` | `/res/550e8400-...` | Public IDs |
| Date | `{date: date}` | `/events/2024-01-15` | Date-based |
| Enum | `{cat: Category}` | `/products/electronics` | Fixed values |
| Path | `{path:path}` | `/files/a/b/c.txt` | File paths |

## Next Steps

Continue learning about:
- [Query Parameters](./03_query_parameters.md) - URL query strings
- [Header Parameters](./04_header_parameters.md) - HTTP headers
- [Complex Routing](./05_complex_routing.md) - Advanced routing patterns
