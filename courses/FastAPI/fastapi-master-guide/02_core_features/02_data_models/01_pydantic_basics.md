# Pydantic Basics

## Overview

Pydantic is the data validation library that powers FastAPI's request/response handling. It uses Python type hints to validate, serialize, and document data automatically.

## Installation

```bash
# Pydantic v2 is included with FastAPI
pip install fastapi

# Or install Pydantic separately
pip install pydantic>=2.0.0
```

## Creating Basic Models

### Simple Pydantic Models

```python
# Example 1: Basic Pydantic model
from pydantic import BaseModel, Field
from typing import Optional
from datetime import datetime

class User(BaseModel):
    """
    Basic user model with automatic validation.
    All fields are required by default.
    """
    id: int
    username: str
    email: str
    is_active: bool = True  # Default value
    created_at: datetime = Field(default_factory=datetime.now)

# Create instance
user = User(id=1, username="john", email="john@example.com")
print(user.model_dump())
# Output: {'id': 1, 'username': 'john', 'email': 'john@example.com', 'is_active': True, 'created_at': '2024-01-01T00:00:00'}

# Validation errors
try:
    invalid_user = User(id="not-an-int", username="john", email="john@example.com")
except Exception as e:
    print(e)
    # Output: 1 validation error for User
    # id
    #   Input should be a valid integer
```

### Using with FastAPI

```python
# Example 2: Pydantic in FastAPI endpoints
from fastapi import FastAPI
from pydantic import BaseModel, EmailStr
from typing import Optional

app = FastAPI()

class ItemCreate(BaseModel):
    """Model for creating items"""
    name: str
    description: Optional[str] = None
    price: float
    tax: float = 0.0

class Item(ItemCreate):
    """Response model with additional fields"""
    id: int

@app.post("/items/", response_model=Item)
async def create_item(item: ItemCreate):
    """
    FastAPI automatically:
    1. Parses JSON request body
    2. Validates against ItemCreate model
    3. Returns 422 if validation fails
    """
    return Item(id=1, **item.model_dump())

# Request body validation
@app.get("/items/{item_id}")
async def get_item(item_id: int) -> Item:
    """
    Return type hint enables response validation.
    """
    return Item(id=item_id, name="Example", price=9.99)
```

## Field Types

### Basic Types

```python
# Example 3: Supported field types
from pydantic import BaseModel
from typing import Optional, List, Dict, Set, Tuple
from datetime import datetime, date, time
from decimal import Decimal
from uuid import UUID

class AllTypes(BaseModel):
    """Demonstrates all basic Pydantic types"""
    
    # Strings
    name: str
    description: Optional[str] = None  # Optional string
    
    # Numbers
    count: int
    price: float
    amount: Decimal  # Precise decimal
    
    # Boolean
    is_active: bool
    
    # Date/Time
    created_at: datetime
    birth_date: date
    alarm_time: time
    
    # UUID
    session_id: UUID
    
    # Collections
    tags: List[str]  # List of strings
    scores: List[int]
    metadata: Dict[str, str]  # Dictionary
    unique_tags: Set[str]  # Set
    
    # Tuples
    coordinates: Tuple[float, float]

# Example usage
data = {
    "name": "Product",
    "count": 10,
    "price": 19.99,
    "amount": Decimal("19.99"),
    "is_active": True,
    "created_at": "2024-01-01T10:00:00",
    "birth_date": "1990-05-15",
    "alarm_time": "08:30:00",
    "session_id": "550e8400-e29b-41d4-a716-446655440000",
    "tags": ["new", "featured"],
    "scores": [95, 87, 92],
    "metadata": {"color": "blue", "size": "large"},
    "unique_tags": ["sale", "new"],
    "coordinates": [40.7128, -74.0060]
}

model = AllTypes(**data)
print(model.model_dump_json())
```

## Model Configuration

### Config Class Options

```python
# Example 4: Model configuration
from pydantic import BaseModel, ConfigDict
from typing import List

class Item(BaseModel):
    """Model with configuration options"""
    
    model_config = ConfigDict(
        # Allow population by field name or alias
        populate_by_name=True,
        
        # Validate default values
        validate_default=True,
        
        # JSON schema extra
        json_schema_extra={
            "examples": [
                {
                    "name": "Laptop",
                    "price": 999.99,
                    "tags": ["electronics"]
                }
            ]
        }
    )
    
    name: str
    price: float
    tags: List[str] = []

# Configuration affects behavior
item = Item(name="Laptop", price=999.99, tags=["electronics"])
print(item.model_json_schema())  # Get JSON schema
```

### Strict Mode

```python
# Example 5: Strict vs lax validation
from pydantic import BaseModel, ConfigDict

# Lax mode (default) - coerces types
class LaxModel(BaseModel):
    count: int
    price: float

lax = LaxModel(count="42", price="19.99")  # Strings coerced to numbers
print(lax)  # count=42 price=19.99

# Strict mode - no coercion
class StrictModel(BaseModel):
    model_config = ConfigDict(strict=True)
    
    count: int
    price: float

try:
    strict = StrictModel(count="42", price="19.99")
except Exception as e:
    print("Strict validation failed:", e)
    # count and price must be actual numbers
```

## Serialization

### Model Serialization

```python
# Example 6: Serialization methods
from pydantic import BaseModel
from typing import List

class User(BaseModel):
    id: int
    username: str
    email: str
    tags: List[str] = []

user = User(id=1, username="john", email="john@example.com", tags=["admin"])

# Dictionary conversion
print(user.model_dump())
# {'id': 1, 'username': 'john', 'email': 'john@example.com', 'tags': ['admin']}

# JSON string conversion
print(user.model_dump_json())
# {"id":1,"username":"john","email":"john@example.com","tags":["admin"]}

# Exclude specific fields
print(user.model_dump(exclude={"email"}))
# {'id': 1, 'username': 'john', 'tags': ['admin']}

# Include only specific fields
print(user.model_dump(include={"id", "username"}))
# {'id': 1, 'username': 'john'}

# Exclude unset fields
partial_user = User(id=2, username="jane")
print(partial_user.model_dump(exclude_unset=True))
# {'id': 2, 'username': 'jane'}
```

### Model Parsing

```python
# Example 7: Parsing data into models
from pydantic import BaseModel
from typing import Optional

class Item(BaseModel):
    name: str
    price: float
    description: Optional[str] = None

# Parse from dictionary
data = {"name": "Laptop", "price": 999.99}
item = Item(**data)

# Parse from JSON string
json_str = '{"name": "Phone", "price": 499.99}'
item = Item.model_validate_json(json_str)

# Parse from dictionary with validation
item = Item.model_validate({"name": "Tablet", "price": 299.99})

print(item)
```

## Error Handling

### Validation Errors

```python
# Example 8: Handling validation errors
from pydantic import BaseModel, ValidationError, Field
from typing import List

class User(BaseModel):
    username: str = Field(..., min_length=3, max_length=20)
    email: str
    age: int = Field(..., ge=13, le=120)
    tags: List[str] = []

# Validation error handling
try:
    user = User(
        username="ab",  # Too short
        email="not-an-email",
        age=10,  # Below minimum
        tags="not-a-list"  # Wrong type
    )
except ValidationError as e:
    print("Validation failed!")
    
    # Get all errors
    for error in e.errors():
        print(f"Field: {error['loc']}")
        print(f"Message: {error['msg']}")
        print(f"Type: {error['type']}")
        print("---")
    
    # Get JSON-formatted errors
    print(e.json())
```

## Best Practices

### Model Organization

```python
# Example 9: Well-organized models
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

# Base model with common fields
class TimestampMixin(BaseModel):
    created_at: datetime = Field(default_factory=datetime.now)
    updated_at: Optional[datetime] = None

# Base model for items
class ItemBase(BaseModel):
    """Common item fields"""
    name: str = Field(..., min_length=1, max_length=100)
    description: Optional[str] = Field(None, max_length=500)
    price: float = Field(..., gt=0)
    is_active: bool = True

# Create model (input)
class ItemCreate(ItemBase):
    """Fields for creating items"""
    tags: List[str] = Field([], max_length=10)

# Update model (partial input)
class ItemUpdate(BaseModel):
    """All fields optional for updates"""
    name: Optional[str] = Field(None, min_length=1, max_length=100)
    description: Optional[str] = None
    price: Optional[float] = Field(None, gt=0)
    is_active: Optional[bool] = None

# Response model (output)
class Item(ItemBase, TimestampMixin):
    """Complete item with ID"""
    id: int
    tags: List[str] = []

    class Config:
        from_attributes = True  # Enable ORM mode
```

## Summary

| Feature | Description | Example |
|---------|-------------|---------|
| Type Validation | Automatic type checking | `age: int` |
| Default Values | Optional fields with defaults | `active: bool = True` |
| Field Constraints | Validation rules | `Field(..., ge=1)` |
| Serialization | Convert to dict/JSON | `model.model_dump()` |
| Parsing | Create from data | `Model(**data)` |

## Next Steps

Continue learning about:
- [Basic Types](./02_basic_types.md) - Detailed type usage
- [Validation Rules](./03_validation_rules.md) - Field validation
- [Advanced Validation](./04_advanced_validation.md) - Custom validators
