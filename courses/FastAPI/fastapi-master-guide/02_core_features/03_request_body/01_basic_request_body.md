# Basic Request Body

## Overview

Request bodies carry data from client to server, typically as JSON. FastAPI uses Pydantic models to validate, parse, and document request bodies automatically.

## Simple Request Body

### Basic JSON Body

```python
# Example 1: Basic request body with Pydantic
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import Optional

app = FastAPI()

class Item(BaseModel):
    """Request body model"""
    name: str = Field(..., min_length=1, max_length=100)
    description: Optional[str] = None
    price: float = Field(..., gt=0)
    tax: float = Field(0.0, ge=0)

@app.post("/items/")
async def create_item(item: Item):
    """
    FastAPI automatically:
    1. Reads JSON request body
    2. Validates against Item model
    3. Returns 422 if validation fails
    4. Parses into Python object
    """
    return {
        "name": item.name,
        "price_with_tax": item.price + item.tax
    }

# Valid request:
# POST /items/
# {"name": "Laptop", "price": 999.99, "tax": 99.99}

# Invalid request returns 422:
# {"name": "", "price": -10}
```

### Required vs Optional Fields

```python
# Example 2: Field requirements
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import Optional, List

app = FastAPI()

class UserCreate(BaseModel):
    # Required fields (no default)
    username: str = Field(..., min_length=3)
    email: str

    # Optional with None default
    bio: Optional[str] = None

    # Optional with value default
    is_active: bool = True

    # List with default
    tags: List[str] = Field(default_factory=list)

@app.post("/users/")
async def create_user(user: UserCreate):
    """
    Field requirement rules:
    - No default = required
    - Default value = optional
    - Optional[T] = can be None
    """
    return user.model_dump()
```

## Multiple Body Parameters

### Separate Body Objects

```python
# Example 3: Multiple body parameters
from fastapi import FastAPI, Body
from pydantic import BaseModel

app = FastAPI()

class Item(BaseModel):
    name: str
    price: float

class User(BaseModel):
    username: str
    email: str

@app.put("/items/{item_id}")
async def update_item(
    item_id: int,
    item: Item,
    user: User,
    importance: int = Body(..., ge=1, le=5)
):
    """
    Multiple body parameters are combined.
    Request body:
    {
        "item": {"name": "Laptop", "price": 999},
        "user": {"username": "john", "email": "john@example.com"},
        "importance": 3
    }
    """
    return {
        "item_id": item_id,
        "item": item.model_dump(),
        "user": user.model_dump(),
        "importance": importance
    }
```

### Embedded Body

```python
# Example 4: Single body with embed
from fastapi import FastAPI, Body
from pydantic import BaseModel

app = FastAPI()

class Item(BaseModel):
    name: str
    price: float

@app.post("/items/")
async def create_item(
    item: Item = Body(..., embed=True)
):
    """
    embed=True wraps body in named key.
    Request body:
    {"item": {"name": "Laptop", "price": 999}}
    Instead of:
    {"name": "Laptop", "price": 999}
    """
    return {"item": item.model_dump()}
```

## Body with Other Parameters

### Combining Parameters

```python
# Example 5: Body with path and query params
from fastapi import FastAPI, Path, Query
from pydantic import BaseModel
from typing import Optional

app = FastAPI()

class ItemUpdate(BaseModel):
    name: Optional[str] = None
    price: Optional[float] = None

@app.patch("/stores/{store_id}/items/{item_id}")
async def update_store_item(
    # Path parameters
    store_id: int = Path(..., ge=1),
    item_id: int = Path(..., ge=1),
    # Query parameter
    notify: bool = Query(False),
    # Request body
    item: ItemUpdate = ...
):
    """
    FastAPI determines parameter source:
    - Path: in URL path
    - Query: after ?
    - Body: in JSON request body
    """
    return {
        "store_id": store_id,
        "item_id": item_id,
        "notify": notify,
        "updates": item.model_dump(exclude_unset=True)
    }
```

## Nested Request Bodies

### Complex Nested Models

```python
# Example 6: Nested request bodies
from fastapi import FastAPI
from pydantic import BaseModel, EmailStr
from typing import List, Optional

app = FastAPI()

class Address(BaseModel):
    street: str
    city: str
    country: str
    postal_code: str

class ContactInfo(BaseModel):
    email: EmailStr
    phone: Optional[str] = None
    address: Address

class UserCreate(BaseModel):
    username: str
    contact: ContactInfo
    tags: List[str] = []

@app.post("/users/")
async def create_user(user: UserCreate):
    """
    Nested models are validated recursively.
    Each level must be valid.
    """
    return {
        "username": user.username,
        "email": user.contact.email,
        "city": user.contact.address.city
    }
```

## Error Handling

### Validation Errors

```python
# Example 7: Validation error responses
from fastapi import FastAPI
from pydantic import BaseModel, Field

app = FastAPI()

class StrictItem(BaseModel):
    name: str = Field(..., min_length=1, max_length=50)
    price: float = Field(..., gt=0, le=1000000)
    quantity: int = Field(..., ge=0, le=10000)

@app.post("/items/")
async def create_item(item: StrictItem):
    """
    Validation errors return 422 with details:
    {
        "detail": [
            {
                "loc": ["body", "name"],
                "msg": "String should have at least 1 character",
                "type": "string_too_short"
            }
        ]
    }
    """
    return item.model_dump()
```

## Best Practices

### Request Body Guidelines

```python
# Example 8: Best practices
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

app = FastAPI()

# Separate models for input/output
class ItemCreate(BaseModel):
    """Input model - what client sends"""
    name: str = Field(..., min_length=1, max_length=100)
    price: float = Field(..., gt=0)
    tags: List[str] = Field(default_factory=list, max_length=10)

class ItemResponse(BaseModel):
    """Output model - what server returns"""
    id: int
    name: str
    price: float
    tags: List[str]
    created_at: datetime

# Use Field for validation
class UserCreate(BaseModel):
    username: str = Field(
        ...,
        min_length=3,
        max_length=20,
        pattern=r'^[a-zA-Z0-9_]+$',
        description="Username (3-20 alphanumeric chars)"
    )
    email: str = Field(..., description="Valid email address")
    password: str = Field(..., min_length=8)

@app.post("/items/", response_model=ItemResponse)
async def create_item(item: ItemCreate):
    """
    Best practices:
    1. Separate input/output models
    2. Use Field for constraints
    3. Document with descriptions
    4. Provide sensible defaults
    """
    return ItemResponse(
        id=1,
        **item.model_dump(),
        created_at=datetime.now()
    )
```

## Summary

| Feature | Description | Example |
|---------|-------------|---------|
| Pydantic Model | JSON body parsing | `item: Item` |
| Optional Fields | Fields with defaults | `description: Optional[str] = None` |
| Multiple Bodies | Combine body objects | `item: Item, user: User` |
| Embed | Wrap in named key | `Body(..., embed=True)` |
| Validation | Automatic checking | Field constraints |

## Next Steps

Continue learning about:
- [File Uploads](./02_file_uploads.md) - File handling
- [Multipart Form Data](./03_multipart_form_data.md) - Form submissions
- [Streaming Requests](./04_streaming_requests.md) - Large data handling
