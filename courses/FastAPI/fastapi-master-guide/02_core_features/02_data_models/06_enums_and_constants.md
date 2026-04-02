# Enums and Constants

## Overview

Enums and constants restrict values to predefined options, improving validation, documentation, and code clarity in FastAPI applications.

## Basic Enums

### String Enums

```python
# Example 1: String enums in FastAPI
from fastapi import FastAPI
from pydantic import BaseModel
from enum import Enum

app = FastAPI()

class Status(str, Enum):
    """
    String enum for status values.
    Using str ensures JSON serialization.
    """
    PENDING = "pending"
    ACTIVE = "active"
    INACTIVE = "inactive"
    DELETED = "deleted"

class Priority(str, Enum):
    LOW = "low"
    MEDIUM = "medium"
    HIGH = "high"
    CRITICAL = "critical"

class Task(BaseModel):
    title: str
    status: Status = Status.PENDING
    priority: Priority = Priority.MEDIUM

@app.post("/tasks/")
async def create_task(task: Task):
    """
    Enums restrict values to predefined options.
    Invalid values return 422.
    """
    return {
        "title": task.title,
        "status": task.status.value,
        "priority": task.priority.value
    }

# Valid: {"title": "Fix bug", "status": "active", "priority": "high"}
# Invalid: {"title": "Fix bug", "status": "unknown"} → 422
```

### Integer Enums

```python
# Example 2: Integer enums
from fastapi import FastAPI
from pydantic import BaseModel
from enum import Enum

app = FastAPI()

class HttpStatusCode(int, Enum):
    """HTTP status codes as enum"""
    OK = 200
    CREATED = 201
    BAD_REQUEST = 400
    UNAUTHORIZED = 401
    NOT_FOUND = 404
    INTERNAL_ERROR = 500

class ErrorCode(int, Enum):
    """Application error codes"""
    INVALID_INPUT = 1001
    NOT_AUTHORIZED = 1002
    RESOURCE_NOT_FOUND = 1003
    DUPLICATE_ENTRY = 1004

class Response(BaseModel):
    status_code: HttpStatusCode
    error_code: ErrorCode | None = None
    message: str

@app.get("/status/{code}")
async def get_status(code: HttpStatusCode):
    """Integer enums for numeric values"""
    return {"code": code.value, "name": code.name}
```

## Enum Usage Patterns

### In Path Parameters

```python
# Example 3: Enums in path parameters
from fastapi import FastAPI
from enum import Enum

app = FastAPI()

class Category(str, Enum):
    ELECTRONICS = "electronics"
    CLOTHING = "clothing"
    BOOKS = "books"
    HOME = "home"
    SPORTS = "sports"

@app.get("/products/{category}")
async def get_products_by_category(category: Category):
    """
    Enums in path restrict valid values.
    /products/electronics ✓
    /products/invalid ✗ (422)
    """
    return {
        "category": category.value,
        "products": []
    }

# List available categories
@app.get("/categories")
async def list_categories():
    """Access all enum values"""
    return {
        "categories": [
            {"value": cat.value, "name": cat.name}
            for cat in Category
        ]
    }
```

### In Query Parameters

```python
# Example 4: Enums in query parameters
from fastapi import FastAPI, Query
from enum import Enum

app = FastAPI()

class SortField(str, Enum):
    NAME = "name"
    PRICE = "price"
    DATE = "created_at"

class SortOrder(str, Enum):
    ASC = "asc"
    DESC = "desc"

@app.get("/items/")
async def list_items(
    sort_by: SortField = Query(SortField.DATE),
    order: SortOrder = Query(SortOrder.DESC)
):
    """
    Enums in query parameters for constrained options.
    URL: /items/?sort_by=price&order=asc
    """
    return {
        "sort": {"field": sort_by.value, "order": order.value},
        "items": []
    }
```

## Literal Types

### Using Literal

```python
# Example 5: Literal types as alternatives
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Literal

app = FastAPI()

class Config(BaseModel):
    # Literal restricts to exact values
    environment: Literal["development", "staging", "production"]
    log_level: Literal["debug", "info", "warning", "error"] = "info"

    # Single value literal (constant)
    version: Literal["1.0"] = "1.0"

@app.post("/config/")
async def update_config(config: Config):
    """
    Literal types are simpler than enums.
    Best for small, simple value sets.
    """
    return config.model_dump()

# Discriminated unions with Literal
from typing import Union

class Dog(BaseModel):
    type: Literal["dog"]
    breed: str

class Cat(BaseModel):
    type: Literal["cat"]
    indoor: bool

class Bird(BaseModel):
    type: Literal["bird"]
    can_fly: bool

Pet = Union[Dog, Cat, Bird]

@app.post("/pets/")
async def create_pet(pet: Pet):
    """
    Literal discriminators enable type narrowing.
    FastAPI validates based on 'type' field.
    """
    return {"type": pet.type, "data": pet.model_dump()}
```

## Constants

### Model Constants

```python
# Example 6: Using constants
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import Final

app = FastAPI()

# Module-level constants
API_VERSION: Final[str] = "1.0.0"
MAX_ITEMS_PER_PAGE: Final[int] = 100
DEFAULT_TIMEOUT: Final[int] = 30

class Pagination(BaseModel):
    page: int = Field(1, ge=1)
    per_page: int = Field(
        20,
        ge=1,
        le=MAX_ITEMS_PER_PAGE  # Use constant
    )

@app.get("/items/")
async def list_items(pagination: Pagination = ...):
    """Use constants for configuration values"""
    return {
        "version": API_VERSION,
        "pagination": pagination.model_dump(),
        "items": []
    }
```

## Best Practices

### Enum Guidelines

```python
# Example 7: Best practices for enums
from fastapi import FastAPI
from pydantic import BaseModel
from enum import Enum

app = FastAPI()

# GOOD: Use str enum for JSON compatibility
class Status(str, Enum):
    ACTIVE = "active"
    INACTIVE = "inactive"

# GOOD: Use descriptive names
class PaymentMethod(str, Enum):
    CREDIT_CARD = "credit_card"
    DEBIT_CARD = "debit_card"
    PAYPAL = "paypal"
    BANK_TRANSFER = "bank_transfer"

# GOOD: Document enum values
class UserRole(str, Enum):
    """
    User roles with increasing privileges.
    """
    VIEWER = "viewer"      # Read-only access
    EDITOR = "editor"      # Can edit content
    ADMIN = "admin"        # Full access

# AVOID: Integer enums for APIs (not human-readable)
# class Status(int, Enum):
#     ACTIVE = 1  # Hard to read in JSON

class User(BaseModel):
    username: str
    role: UserRole = UserRole.VIEWER
    payment_method: PaymentMethod

@app.post("/users/")
async def create_user(user: User):
    """
    Best practices:
    1. Use str enums for API-facing values
    2. Use snake_case for enum values
    3. Document enum purpose
    4. Keep enums focused and small
    """
    return user.model_dump()
```

## Summary

| Feature | Use Case | Example |
|---------|----------|---------|
| `str, Enum` | String values | `Status(str, Enum)` |
| `int, Enum` | Numeric values | `HttpCode(int, Enum)` |
| `Literal` | Simple constraints | `Literal["a", "b"]` |
| `Final` | Constants | `MAX_SIZE: Final[int] = 100` |

## Next Steps

Continue learning about:
- [Request Body](../03_request_body/01_basic_request_body.md) - JSON body handling
- [File Uploads](../03_request_body/02_file_uploads.md) - File handling
- [Responses](../04_responses_and_status_codes/01_default_responses.md) - Response formatting
