# Default Responses

## Overview

FastAPI handles response serialization automatically, converting Python objects to JSON and setting appropriate headers. Understanding default response behavior is essential for API design.

## Automatic Response Handling

### Dictionary Responses

```python
# Example 1: Default response types
from fastapi import FastAPI
from typing import List, Dict

app = FastAPI()

@app.get("/dict/")
async def dict_response():
    """
    Dictionaries are automatically converted to JSON.
    Content-Type: application/json
    Status: 200 OK
    """
    return {"message": "Hello", "items": [1, 2, 3]}

@app.get("/list/")
async def list_response():
    """Lists are also converted to JSON"""
    return ["item1", "item2", "item3"]

@app.get("/string/")
async def string_response():
    """Strings are returned as plain text"""
    return "Hello, World!"

@app.get("/none/")
async def none_response():
    """None returns empty response with 204 No Content"""
    return None
```

### Pydantic Model Responses

```python
# Example 2: Model serialization
from fastapi import FastAPI
from pydantic import BaseModel
from typing import List, Optional

app = FastAPI()

class Item(BaseModel):
    id: int
    name: str
    price: float
    description: Optional[str] = None

@app.get("/items/{item_id}", response_model=Item)
async def get_item(item_id: int):
    """
    response_model ensures output matches Item schema.
    Extra fields are excluded.
    """
    return Item(
        id=item_id,
        name="Laptop",
        price=999.99,
        description="A powerful laptop"
    )

@app.get("/items/", response_model=List[Item])
async def list_items():
    """response_model can be a List of models"""
    return [
        Item(id=1, name="Laptop", price=999.99),
        Item(id=2, name="Phone", price=499.99)
    ]
```

## Response Configuration

### Response Model Options

```python
# Example 3: Response model configuration
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Optional, Set

app = FastAPI()

class UserInternal(BaseModel):
    id: int
    username: str
    email: str
    password_hash: str
    api_key: str

class UserPublic(BaseModel):
    id: int
    username: str
    email: str

@app.get(
    "/users/{user_id}",
    response_model=UserPublic,
    response_model_exclude_unset=True
)
async def get_user(user_id: int):
    """
    response_model filters output:
    - Only fields in UserPublic are returned
    - Sensitive fields excluded
    """
    return UserInternal(
        id=user_id,
        username="john",
        email="john@example.com",
        password_hash="secret_hash",
        api_key="secret_key"
    )

@app.get(
    "/users/{user_id}/basic",
    response_model=UserPublic,
    response_model_include={"id", "username"}
)
async def get_user_basic(user_id: int):
    """response_model_include returns only specified fields"""
    return UserInternal(
        id=user_id,
        username="john",
        email="john@example.com",
        password_hash="hash",
        api_key="key"
    )

@app.get(
    "/users/{user_id}/safe",
    response_model=UserPublic,
    response_model_exclude={"email"}
)
async def get_user_safe(user_id: int):
    """response_model_exclude removes specified fields"""
    return UserInternal(
        id=user_id,
        username="john",
        email="john@example.com",
        password_hash="hash",
        api_key="key"
    )
```

## Response Headers

### Custom Headers

```python
# Example 4: Setting response headers
from fastapi import FastAPI, Response

app = FastAPI()

@app.get("/items/")
async def get_items(response: Response):
    """
    Set custom headers using Response object.
    """
    response.headers["X-Total-Count"] = "100"
    response.headers["X-Request-ID"] = "abc123"
    response.headers["Cache-Control"] = "max-age=3600"

    return {"items": []}

@app.get("/items2/")
async def get_items_v2():
    """
    Alternative: return Response directly.
    """
    from fastapi.responses import JSONResponse

    return JSONResponse(
        content={"items": []},
        headers={
            "X-Total-Count": "100",
            "X-Request-ID": "abc123"
        }
    )
```

## Best Practices

### Response Guidelines

```python
# Example 5: Best practices
from fastapi import FastAPI
from pydantic import BaseModel
from typing import List, Optional
from datetime import datetime

app = FastAPI()

# Separate input/output models
class ItemCreate(BaseModel):
    name: str
    price: float

class ItemResponse(BaseModel):
    id: int
    name: str
    price: float
    created_at: datetime

# Consistent response structure
class ApiResponse(BaseModel):
    success: bool
    data: Optional[dict] = None
    error: Optional[str] = None

@app.post("/items/", response_model=ItemResponse)
async def create_item(item: ItemCreate):
    """
    Best practices:
    1. Use separate input/output models
    2. Always specify response_model
    3. Include relevant metadata
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
| Auto-serialize | Dict/Model → JSON | `return {"key": "value"}` |
| Response model | Output validation | `response_model=Item` |
| Exclude fields | Filter output | `response_model_exclude={"password"}` |
| Include fields | Select fields | `response_model_include={"id", "name"}` |
| Custom headers | Response metadata | `response.headers["X-Custom"] = "value"` |

## Next Steps

Continue learning about:
- [Custom Status Codes](./02_custom_status_codes.md) - HTTP status codes
- [Response Models](./03_response_models.md) - Output validation
- [Problem Details](./04_problem_details.md) - Error responses
