# Basic Types

## Overview

FastAPI and Pydantic support a wide range of Python types for automatic validation and serialization. Understanding these types is essential for building robust APIs.

## String Types

### String Validation

```python
# Example 1: String types and validation
from fastapi import FastAPI
from pydantic import BaseModel, Field, EmailStr, HttpUrl, constr

app = FastAPI()

class User(BaseModel):
    # Basic string
    name: str

    # Email validation (requires: pip install email-validator)
    email: EmailStr

    # URL validation
    website: HttpUrl

    # Constrained string
    username: constr(min_length=3, max_length=20, pattern=r'^[a-zA-Z0-9_]+$')

    # String with length constraints
    bio: str = Field(None, max_length=500)

@app.post("/users/")
async def create_user(user: User):
    """
    String types provide automatic validation:
    - EmailStr: Validates email format
    - HttpUrl: Validates URL format
    - constr: Custom string constraints
    """
    return user.model_dump()
```

## Numeric Types

### Integer and Float Validation

```python
# Example 2: Numeric types
from fastapi import FastAPI
from pydantic import BaseModel, Field, conint, confloat
from decimal import Decimal

app = FastAPI()

class Product(BaseModel):
    # Basic integers
    quantity: int
    rating: int = Field(..., ge=1, le=5)

    # Constrained integer (must be positive)
    stock: conint(ge=0)

    # Basic floats
    price: float
    discount: float = Field(0.0, ge=0.0, le=1.0)

    # Constrained float
    weight: confloat(gt=0)

    # Decimal for precise values
    amount: Decimal = Field(..., decimal_places=2)

@app.post("/products/")
async def create_product(product: Product):
    """
    Numeric types ensure:
    - Correct data types
    - Value range constraints
    - Precision (for Decimal)
    """
    return product.model_dump()
```

## Boolean Types

### Boolean Handling

```python
# Example 3: Boolean types
from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI()

class Settings(BaseModel):
    # Boolean field
    dark_mode: bool

    # Boolean with default
    notifications: bool = True

    # Optional boolean
    sound_enabled: bool | None = None

@app.post("/settings/")
async def update_settings(settings: Settings):
    """
    Boolean values accept:
    - true, True, 1, on, yes → True
    - false, False, 0, off, no → False
    """
    return settings.model_dump()
```

## Date and Time Types

### Temporal Types

```python
# Example 4: Date and time types
from fastapi import FastAPI
from pydantic import BaseModel
from datetime import datetime, date, time, timedelta

app = FastAPI()

class Event(BaseModel):
    # Date only
    event_date: date

    # Time only
    start_time: time
    end_time: time

    # DateTime
    created_at: datetime

    # Duration
    duration: timedelta

    # Optional dates
    deadline: date | None = None

@app.post("/events/")
async def create_event(event: Event):
    """
    Date/time types:
    - date: YYYY-MM-DD
    - time: HH:MM:SS
    - datetime: ISO 8601 format
    - timedelta: Duration (seconds or parsed)
    """
    return {
        "event_date": event.event_date.isoformat(),
        "start_time": event.start_time.isoformat(),
        "duration_seconds": event.duration.total_seconds()
    }
```

## Collection Types

### Lists and Tuples

```python
# Example 5: Collection types
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import List, Set, Tuple, FrozenSet

app = FastAPI()

class DataModel(BaseModel):
    # List of items
    tags: List[str]
    scores: List[int] = []

    # Set (unique values)
    unique_ids: Set[int]

    # Frozen set (immutable)
    categories: FrozenSet[str]

    # Tuple (fixed length)
    coordinates: Tuple[float, float]

    # Variable length tuple
    values: Tuple[int, ...]

    # List with constraints
    limited_list: List[str] = Field(..., min_length=1, max_length=10)

@app.post("/data/")
async def create_data(data: DataModel):
    """
    Collection types validate:
    - Element types
    - Uniqueness (for sets)
    - Length (with Field constraints)
    """
    return {
        "tags": data.tags,
        "unique_count": len(data.unique_ids),
        "coordinates": data.coordinates
    }
```

### Dictionaries

```python
# Example 6: Dictionary types
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Dict, Any

app = FastAPI()

class Config(BaseModel):
    # Typed dictionary
    settings: Dict[str, int]

    # Dictionary with any values
    metadata: Dict[str, Any]

    # Nested dictionary
    nested: Dict[str, Dict[str, str]]

@app.post("/config/")
async def update_config(config: Config):
    """
    Dictionary types validate:
    - Key types (usually str)
    - Value types
    """
    return config.model_dump()
```

## Enum Types

### Using Enums

```python
# Example 7: Enum types
from fastapi import FastAPI
from pydantic import BaseModel
from enum import Enum

app = FastAPI()

class Status(str, Enum):
    """String enum for status values"""
    ACTIVE = "active"
    INACTIVE = "inactive"
    PENDING = "pending"

class Priority(int, Enum):
    """Integer enum for priority levels"""
    LOW = 1
    MEDIUM = 2
    HIGH = 3

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
```

## Optional and Union Types

### Optional Fields

```python
# Example 8: Optional and union types
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Optional, Union

app = FastAPI()

class UserProfile(BaseModel):
    # Required field
    username: str

    # Optional with None default
    bio: Optional[str] = None

    # Optional with value default
    display_name: str | None = None  # Python 3.10+ syntax

    # Union type (multiple possible types)
    age: Union[int, str]  # Accepts int or str

    # Union with None (same as Optional)
    nickname: str | None = None

@app.post("/profile/")
async def update_profile(profile: UserProfile):
    """
    Optional types:
    - Can be None
    - Excluded from required validation
    """
    return profile.model_dump()
```

## Literal Types

### Literal Values

```python
# Example 9: Literal types
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Literal

app = FastAPI()

class Request(BaseModel):
    # Literal restricts to exact values
    method: Literal["GET", "POST", "PUT", "DELETE"]

    # Single literal (constant)
    version: Literal["1.0"] = "1.0"

    # Multiple literal values
    format: Literal["json", "xml", "csv"] = "json"

@app.post("/request/")
async def make_request(request: Request):
    """
    Literal types:
    - Restrict to specific values
    - Useful for discriminators
    """
    return request.model_dump()
```

## Best Practices

### Type Selection Guidelines

```python
# Example 10: Best practices for types
from fastapi import FastAPI
from pydantic import BaseModel, Field, EmailStr, HttpUrl
from typing import List, Optional
from datetime import datetime
from uuid import UUID, uuid4
from decimal import Decimal

app = FastAPI()

class BestPracticeModel(BaseModel):
    # Use UUID for public IDs (prevents enumeration)
    id: UUID = Field(default_factory=uuid4)

    # Use EmailStr for emails
    email: EmailStr

    # Use HttpUrl for URLs
    website: HttpUrl

    # Use Decimal for money (avoids floating point issues)
    price: Decimal = Field(..., decimal_places=2)

    # Use datetime for timestamps
    created_at: datetime = Field(default_factory=datetime.now)

    # Use Field for constraints
    name: str = Field(..., min_length=1, max_length=100)

    # Use Optional for truly optional fields
    description: Optional[str] = None

    # Use List with constraints
    tags: List[str] = Field(default_factory=list, max_length=10)

@app.post("/items/")
async def create_item(item: BestPracticeModel):
    """
    Best practices:
    - Use specific types (EmailStr, HttpUrl, UUID)
    - Use Decimal for money
    - Use Field for constraints
    - Use Optional for nullable fields
    """
    return item.model_dump()
```

## Summary

| Type | Use Case | Example |
|------|----------|---------|
| `str` | Text data | `name: str` |
| `int` | Whole numbers | `count: int` |
| `float` | Decimal numbers | `price: float` |
| `bool` | True/False | `active: bool` |
| `datetime` | Timestamps | `created_at: datetime` |
| `List[T]` | Collections | `tags: List[str]` |
| `Dict[K, V]` | Key-value pairs | `data: Dict[str, int]` |
| `Enum` | Fixed values | `status: Status` |

## Next Steps

Continue learning about:
- [Validation Rules](./03_validation_rules.md) - Field constraints
- [Advanced Validation](./04_advanced_validation.md) - Custom validators
- [Nested Models](./05_nested_models.md) - Complex structures
