# Response Models

## Overview

Response models validate and filter API output, ensuring clients receive consistent, well-structured data while protecting sensitive information.

## Basic Response Models

### Simple Response Model

```python
# Example 1: Basic response model
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Optional, List
from datetime import datetime

app = FastAPI()

class Item(BaseModel):
    id: int
    name: str
    price: float
    description: Optional[str] = None
    created_at: datetime

@app.get("/items/{item_id}", response_model=Item)
async def get_item(item_id: int):
    """
    response_model ensures output matches Item schema.
    """
    return Item(
        id=item_id,
        name="Laptop",
        price=999.99,
        created_at=datetime.now()
    )

@app.get("/items/", response_model=List[Item])
async def list_items():
    """List response model"""
    return [
        Item(id=1, name="Laptop", price=999.99, created_at=datetime.now()),
        Item(id=2, name="Phone", price=499.99, created_at=datetime.now())
    ]
```

### Filtered Response Models

```python
# Example 2: Response model filtering
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import Optional

app = FastAPI()

class UserInternal(BaseModel):
    id: int
    username: str
    email: str
    password_hash: str
    api_key: str
    is_admin: bool

class UserPublic(BaseModel):
    id: int
    username: str
    email: str

@app.get("/users/{user_id}", response_model=UserPublic)
async def get_user(user_id: int):
    """
    Internal model contains sensitive data.
    Public model filters it out.
    """
    return UserInternal(
        id=user_id,
        username="john",
        email="john@example.com",
        password_hash="secret_hash",
        api_key="secret_key",
        is_admin=True
    )

@app.get(
    "/users/{user_id}/basic",
    response_model=UserPublic,
    response_model_include={"id", "username"}
)
async def get_user_basic(user_id: int):
    """Only return specified fields"""
    return UserInternal(
        id=user_id,
        username="john",
        email="john@example.com",
        password_hash="hash",
        api_key="key",
        is_admin=False
    )

@app.get(
    "/users/{user_id}/safe",
    response_model=UserPublic,
    response_model_exclude={"email"}
)
async def get_user_safe(user_id: int):
    """Exclude specified fields"""
    return UserInternal(
        id=user_id,
        username="john",
        email="john@example.com",
        password_hash="hash",
        api_key="key",
        is_admin=False
    )
```

## Response Model Patterns

### Wrapper Models

```python
# Example 3: Response wrapper patterns
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Optional, List, Generic, TypeVar

app = FastAPI()

T = TypeVar('T')

class ApiResponse(BaseModel):
    success: bool
    data: Optional[dict] = None
    error: Optional[str] = None
    message: Optional[str] = None

class PaginatedResponse(BaseModel):
    items: List[dict]
    total: int
    page: int
    per_page: int
    pages: int

@app.get("/items/", response_model=PaginatedResponse)
async def list_items(page: int = 1, per_page: int = 10):
    """Paginated response"""
    items = [{"id": i, "name": f"Item {i}"} for i in range(per_page)]
    return PaginatedResponse(
        items=items,
        total=100,
        page=page,
        per_page=per_page,
        pages=10
    )

@app.get("/items/{item_id}", response_model=ApiResponse)
async def get_item(item_id: int):
    """Wrapped response"""
    return ApiResponse(
        success=True,
        data={"id": item_id, "name": "Laptop"}
    )
```

### Computed Fields

```python
# Example 4: Response with computed fields
from fastapi import FastAPI
from pydantic import BaseModel, computed_field
from typing import List

app = FastAPI()

class Order(BaseModel):
    items: List[float]
    tax_rate: float = 0.1
    discount: float = 0

    @computed_field
    @property
    def subtotal(self) -> float:
        return sum(self.items)

    @computed_field
    @property
    def total(self) -> float:
        return self.subtotal * (1 + self.tax_rate) - self.discount

@app.post("/orders/", response_model=Order)
async def create_order(items: List[float]):
    """Response includes computed fields"""
    return Order(items=items)
```

## Best Practices

### Response Model Guidelines

```python
# Example 5: Best practices
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

app = FastAPI()

# Input model
class ItemCreate(BaseModel):
    name: str = Field(..., min_length=1)
    price: float = Field(..., gt=0)

# Output model
class ItemResponse(BaseModel):
    id: int
    name: str
    price: float
    created_at: datetime

    class Config:
        from_attributes = True

@app.post("/items/", response_model=ItemResponse)
async def create_item(item: ItemCreate):
    """
    Best practices:
    1. Separate input/output models
    2. Always specify response_model
    3. Use Config for ORM compatibility
    """
    return ItemResponse(
        id=1,
        **item.model_dump(),
        created_at=datetime.now()
    )
```

## Summary

| Feature | Usage | Example |
|---------|-------|---------|
| Basic model | `response_model=Item` | Validate output |
| List model | `response_model=List[Item]` | Array responses |
| Include fields | `response_model_include={"id"}` | Select fields |
| Exclude fields | `response_model_exclude={"password"}` | Hide fields |
| Computed | `@computed_field` | Derived values |

## Next Steps

Continue learning about:
- [Problem Details](./04_problem_details.md) - Error responses
- [Exception Handling](./05_exceptions_handling.md) - Error handling
- [Custom Responses](./06_custom_responses.md) - Response types
