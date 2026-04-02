# Advanced Validation

## Overview

Advanced validation techniques in FastAPI handle complex business logic, cross-field validation, and custom error handling beyond basic field constraints.

## Complex Validators

### Cross-Field Validation

```python
# Example 1: Cross-field validation
from fastapi import FastAPI
from pydantic import BaseModel, model_validator, field_validator
from datetime import date, datetime
from typing import Optional

app = FastAPI()

class Registration(BaseModel):
    username: str
    email: str
    password: str
    password_confirm: str
    birth_date: date
    terms_accepted: bool

    @model_validator(mode='after')
    def validate_passwords_match(self) -> 'Registration':
        """Ensure passwords match"""
        if self.password != self.password_confirm:
            raise ValueError('Passwords do not match')
        return self

    @model_validator(mode='after')
    def validate_age(self) -> 'Registration':
        """Ensure user is at least 13 years old"""
        today = date.today()
        age = today.year - self.birth_date.year
        if age < 13:
            raise ValueError('Must be at least 13 years old')
        return self

    @model_validator(mode='after')
    def validate_terms(self) -> 'Registration':
        """Ensure terms are accepted"""
        if not self.terms_accepted:
            raise ValueError('Must accept terms and conditions')
        return self

@app.post("/register/")
async def register(user: Registration):
    """Multiple model validators run in sequence"""
    return {"username": user.username, "message": "Registered successfully"}
```

### Dependent Field Validation

```python
# Example 2: Field dependencies
from fastapi import FastAPI
from pydantic import BaseModel, field_validator, model_validator
from typing import Optional

app = FastAPI()

class ShippingAddress(BaseModel):
    street: str
    city: str
    state: Optional[str] = None
    country: str
    postal_code: str

    @field_validator('state')
    @classmethod
    def validate_state_for_us(cls, v: Optional[str], info) -> Optional[str]:
        """State required for US addresses"""
        country = info.data.get('country', '')
        if country.upper() == 'US' and not v:
            raise ValueError('State is required for US addresses')
        return v

    @field_validator('postal_code')
    @classmethod
    def validate_postal_code(cls, v: str, info) -> str:
        """Validate postal code format by country"""
        country = info.data.get('country', '').upper()
        
        if country == 'US' and not v.replace('-', '').isdigit():
            raise ValueError('US postal code must be numeric')
        if country == 'US' and len(v.split('-')[0]) != 5:
            raise ValueError('US ZIP code must be 5 digits')
        
        return v

@app.post("/address/")
async def create_address(address: ShippingAddress):
    return address.model_dump()
```

## Computed Fields

### Dynamic Field Values

```python
# Example 3: Computed fields
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
        """Calculate subtotal"""
        return sum(self.items)

    @computed_field
    @property
    def tax(self) -> float:
        """Calculate tax"""
        return self.subtotal * self.tax_rate

    @computed_field
    @property
    def total(self) -> float:
        """Calculate total with tax and discount"""
        return self.subtotal + self.tax - self.discount

@app.post("/orders/")
async def create_order(order: Order):
    """
    Computed fields are included in response.
    Not part of input validation.
    """
    return order.model_dump()

# Response includes computed fields:
# {
#     "items": [10.0, 20.0],
#     "tax_rate": 0.1,
#     "discount": 0,
#     "subtotal": 30.0,
#     "tax": 3.0,
#     "total": 33.0
# }
```

## Context-Dependent Validation

### Validation with Context

```python
# Example 4: Context-aware validation
from fastapi import FastAPI, Depends
from pydantic import BaseModel, field_validator, ValidationInfo
from typing import List, Set

app = FastAPI()

# Simulated database of taken usernames
TAKEN_USERNAMES: Set[str] = {"admin", "root", "system"}

class UserCreate(BaseModel):
    username: str
    email: str

    @field_validator('username')
    @classmethod
    def username_not_taken(cls, v: str, info: ValidationInfo) -> str:
        """Check username availability"""
        if v.lower() in TAKEN_USERNAMES:
            raise ValueError('Username is already taken')
        return v.lower()

@app.post("/users/")
async def create_user(user: UserCreate):
    return {"username": user.username}
```

## Conditional Validation

### Type-Based Validation

```python
# Example 5: Conditional validation with discriminated unions
from fastapi import FastAPI
from pydantic import BaseModel, Field, model_validator
from typing import Literal, Union
from typing_extensions import Annotated, Discriminator

app = FastAPI()

class CreditCard(BaseModel):
    type: Literal["credit_card"]
    card_number: str
    expiry: str
    cvv: str

    @field_validator('card_number')
    @classmethod
    def validate_card(cls, v: str) -> str:
        digits = ''.join(filter(str.isdigit, v))
        if len(digits) != 16:
            raise ValueError('Card number must be 16 digits')
        return v

class PayPal(BaseModel):
    type: Literal["paypal"]
    email: str

class BankTransfer(BaseModel):
    type: Literal["bank_transfer"]
    account_number: str
    routing_number: str

# Union type
PaymentMethod = Annotated[
    Union[CreditCard, PayPal, BankTransfer],
    Discriminator('type')
]

class Order(BaseModel):
    items: List[str]
    payment: PaymentMethod

@app.post("/orders/")
async def create_order(order: Order):
    """
    Discriminated unions validate based on 'type' field.
    Each variant has its own validation rules.
    """
    return {
        "items": order.items,
        "payment_type": order.payment.type
    }
```

## Async Validators

### Database Validation

```python
# Example 6: Async validation (conceptual)
from fastapi import FastAPI
from pydantic import BaseModel, field_validator
import asyncio

app = FastAPI()

# Simulated async database check
async def check_email_exists(email: str) -> bool:
    """Async database check"""
    await asyncio.sleep(0.1)  # Simulate DB query
    return email in ["taken@example.com", "admin@example.com"]

class UserCreate(BaseModel):
    email: str
    username: str

    # Note: Pydantic v2 validators are sync by default
    # For async validation, use FastAPI dependencies

@app.post("/users/")
async def create_user(user: UserCreate):
    """
    For async validation, validate in endpoint or dependency.
    Pydantic validators are synchronous.
    """
    # Async validation in endpoint
    if await check_email_exists(user.email):
        return {"error": "Email already registered"}
    
    return {"email": user.username, "message": "Created"}
```

## Custom Error Formatting

### Structured Errors

```python
# Example 7: Custom error responses
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, field_validator, ValidationError
from typing import List

app = FastAPI()

class Item(BaseModel):
    name: str
    price: float
    quantity: int

    @field_validator('price')
    @classmethod
    def validate_price(cls, v: float) -> float:
        if v <= 0:
            raise ValueError('Price must be positive')
        if v > 1000000:
            raise ValueError('Price too high')
        return round(v, 2)

@app.post("/items/")
async def create_item(item: Item):
    try:
        return item.model_dump()
    except ValidationError as e:
        # Custom error formatting
        errors = []
        for error in e.errors():
            errors.append({
                "field": ".".join(str(loc) for loc in error["loc"]),
                "message": error["msg"],
                "type": error["type"]
            })
        raise HTTPException(status_code=422, detail=errors)
```

## Summary

| Technique | Use Case | Example |
|-----------|----------|---------|
| `model_validator` | Cross-field validation | Password confirmation |
| `@computed_field` | Derived values | Order total |
| `Discriminator` | Type-based validation | Payment methods |
| Context validation | External checks | Username availability |

## Next Steps

Continue learning about:
- [Nested Models](./05_nested_models.md) - Model composition
- [Enums and Constants](./06_enums_and_constants.md) - Fixed values
- [Request Body](../03_request_body/01_basic_request_body.md) - Body handling
