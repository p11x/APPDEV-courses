# Validation Rules

## Overview

Pydantic provides extensive validation capabilities through Field constraints, validators, and custom validation logic. This guide covers all built-in validation options.

## Field Constraints

### Numeric Constraints

```python
# Example 1: Numeric validation rules
from fastapi import FastAPI
from pydantic import BaseModel, Field

app = FastAPI()

class NumericModel(BaseModel):
    # Integer constraints
    age: int = Field(..., ge=0, le=150)      # 0 <= age <= 150
    quantity: int = Field(..., gt=0)          # quantity > 0
    score: int = Field(..., multiple_of=5)   # Must be multiple of 5

    # Float constraints
    price: float = Field(..., gt=0, le=1000000)
    percentage: float = Field(..., ge=0.0, le=100.0)
    rating: float = Field(..., ge=1.0, le=5.0)

    # Decimal precision
    amount: float = Field(..., decimal_places=2)

@app.post("/numeric/")
async def validate_numeric(data: NumericModel):
    """
    Numeric constraints:
    - gt: Greater than
    - ge: Greater than or equal
    - lt: Less than
    - le: Less than or equal
    - multiple_of: Must be multiple of value
    """
    return data.model_dump()
```

### String Constraints

```python
# Example 2: String validation rules
from fastapi import FastAPI
from pydantic import BaseModel, Field

app = FastAPI()

class StringModel(BaseModel):
    # Length constraints
    username: str = Field(..., min_length=3, max_length=20)
    bio: str = Field("", max_length=500)

    # Pattern matching (regex)
    phone: str = Field(..., pattern=r'^\+?1?\d{9,15}$')
    zip_code: str = Field(..., pattern=r'^\d{5}(-\d{4})?$')

    # Strip whitespace
    name: str = Field(..., strip_whitespace=True)

@app.post("/strings/")
async def validate_strings(data: StringModel):
    """
    String constraints:
    - min_length: Minimum characters
    - max_length: Maximum characters
    - pattern: Regex pattern
    - strip_whitespace: Remove leading/trailing spaces
    """
    return data.model_dump()
```

### Collection Constraints

```python
# Example 3: Collection validation rules
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import List, Set, Dict

app = FastAPI()

class CollectionModel(BaseModel):
    # List constraints
    tags: List[str] = Field(
        ...,
        min_length=1,      # At least 1 item
        max_length=10,     # At most 10 items
        unique_items=True  # No duplicates
    )

    # Set constraints
    unique_ids: Set[int] = Field(..., min_length=1)

    # List with item constraints
    scores: List[int] = Field(
        default_factory=list,
        min_length=0,
        max_length=100
    )

@app.post("/collections/")
async def validate_collections(data: CollectionModel):
    """
    Collection constraints:
    - min_length: Minimum items
    - max_length: Maximum items
    - unique_items: No duplicates (for lists)
    """
    return {"tags": data.tags, "count": len(data.tags)}
```

## Custom Validators

### Field Validators

```python
# Example 4: Custom field validation
from fastapi import FastAPI
from pydantic import BaseModel, field_validator, model_validator

app = FastAPI()

class User(BaseModel):
    username: str
    password: str
    password_confirm: str

    @field_validator('username')
    @classmethod
    def validate_username(cls, v: str) -> str:
        """Validate username format"""
        if not v.isalnum():
            raise ValueError('Username must be alphanumeric')
        if len(v) < 3:
            raise ValueError('Username must be at least 3 characters')
        return v.lower()  # Normalize to lowercase

    @field_validator('password')
    @classmethod
    def validate_password(cls, v: str) -> str:
        """Validate password strength"""
        if len(v) < 8:
            raise ValueError('Password must be at least 8 characters')
        if not any(c.isupper() for c in v):
            raise ValueError('Password must contain uppercase letter')
        if not any(c.isdigit() for c in v):
            raise ValueError('Password must contain digit')
        return v

    @model_validator(mode='after')
    def passwords_match(self) -> 'User':
        """Validate password confirmation"""
        if self.password != self.password_confirm:
            raise ValueError('Passwords do not match')
        return self

@app.post("/users/")
async def create_user(user: User):
    """Custom validators run automatically"""
    return {"username": user.username, "message": "User created"}
```

### Model Validators

```python
# Example 5: Model-level validation
from fastapi import FastAPI
from pydantic import BaseModel, model_validator
from datetime import date

app = FastAPI()

class DateRange(BaseModel):
    start_date: date
    end_date: date

    @model_validator(mode='after')
    def validate_date_range(self) -> 'DateRange':
        """Ensure end_date is after start_date"""
        if self.end_date <= self.start_date:
            raise ValueError('end_date must be after start_date')
        return self

class PriceRange(BaseModel):
    min_price: float
    max_price: float

    @model_validator(mode='after')
    def validate_price_range(self) -> 'PriceRange':
        """Ensure max_price >= min_price"""
        if self.max_price < self.min_price:
            raise ValueError('max_price must be >= min_price')
        return self

@app.post("/date-range/")
async def create_range(date_range: DateRange):
    """Model validators check relationships between fields"""
    return {"start": date_range.start_date, "end": date_range.end_date}
```

## Conditional Validation

### Before/After Validators

```python
# Example 6: Conditional validation
from fastapi import FastAPI
from pydantic import BaseModel, field_validator, model_validator

app = FastAPI()

class Product(BaseModel):
    name: str
    price: float
    discount_percent: float = 0
    is_on_sale: bool = False

    @field_validator('discount_percent')
    @classmethod
    def validate_discount(cls, v: float, info) -> float:
        """Discount only valid for items on sale"""
        if v > 0 and not info.data.get('is_on_sale'):
            raise ValueError('Discount only allowed for items on sale')
        return v

    @model_validator(mode='after')
    def validate_sale_price(self) -> 'Product':
        """Ensure discounted price is positive"""
        if self.is_on_sale:
            final_price = self.price * (1 - self.discount_percent / 100)
            if final_price <= 0:
                raise ValueError('Final price must be positive')
        return self

@app.post("/products/")
async def create_product(product: Product):
    return product.model_dump()
```

## Error Messages

### Custom Error Messages

```python
# Example 7: Custom validation messages
from fastapi import FastAPI
from pydantic import BaseModel, Field, field_validator, ValidationError

app = FastAPI()

class StrictModel(BaseModel):
    age: int = Field(
        ...,
        ge=18,
        le=100,
        description="Must be 18-100 years old"
    )

    email: str = Field(
        ...,
        description="Valid email address"
    )

    @field_validator('email')
    @classmethod
    def validate_email(cls, v: str) -> str:
        if '@' not in v:
            raise ValueError('Email must contain @ symbol')
        return v

@app.post("/strict/")
async def strict_validation(data: StrictModel):
    """
    Validation errors return detailed messages:
    {
        "detail": [
            {
                "loc": ["body", "age"],
                "msg": "Input should be greater than or equal to 18",
                "type": "greater_than_equal"
            }
        ]
    }
    """
    return data.model_dump()
```

## Best Practices

### Validation Guidelines

```python
# Example 8: Best practices for validation
from fastapi import FastAPI
from pydantic import BaseModel, Field, field_validator
from typing import Optional, List
from datetime import datetime

app = FastAPI()

class BestPracticeValidation(BaseModel):
    # Use Field for constraints (generates better errors)
    name: str = Field(..., min_length=1, max_length=100)

    # Use specific types when possible
    email: str = Field(..., pattern=r'^[\w\.-]+@[\w\.-]+\.\w+$')

    # Use Optional for nullable fields
    bio: Optional[str] = Field(None, max_length=500)

    # Provide sensible defaults
    is_active: bool = True
    created_at: datetime = Field(default_factory=datetime.now)

    # Use lists with constraints
    tags: List[str] = Field(default_factory=list, max_length=10)

    @field_validator('name')
    @classmethod
    def sanitize_name(cls, v: str) -> str:
        """Sanitize input"""
        return v.strip().title()

@app.post("/items/")
async def create_item(item: BestPracticeValidation):
    """
    Validation best practices:
    1. Use Field for constraints
    2. Use specific types (EmailStr, HttpUrl)
    3. Provide defaults for optional fields
    4. Sanitize input in validators
    5. Keep validation logic simple
    """
    return item.model_dump()
```

## Summary

| Constraint | Type | Example |
|------------|------|---------|
| `gt/ge/lt/le` | Numeric | `Field(ge=0, le=100)` |
| `min_length/max_length` | String/List | `Field(min_length=3)` |
| `pattern` | String | `Field(pattern=r'^\d+$')` |
| `multiple_of` | Numeric | `Field(multiple_of=5)` |
| `unique_items` | List | `Field(unique_items=True)` |

## Next Steps

Continue learning about:
- [Advanced Validation](./04_advanced_validation.md) - Complex validators
- [Nested Models](./05_nested_models.md) - Model composition
- [Request Body](../03_request_body/01_basic_request_body.md) - Body validation
