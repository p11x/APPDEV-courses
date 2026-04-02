# Nested Models

## Overview

Nested models allow building complex data structures by composing Pydantic models. This enables hierarchical validation, reuse, and clear data organization.

## Basic Nesting

### Simple Nested Models

```python
# Example 1: Basic nested models
from fastapi import FastAPI
from pydantic import BaseModel, EmailStr
from typing import List, Optional

app = FastAPI()

class Address(BaseModel):
    """Nested address model"""
    street: str
    city: str
    state: Optional[str] = None
    country: str
    postal_code: str

class ContactInfo(BaseModel):
    """Nested contact model"""
    email: EmailStr
    phone: Optional[str] = None
    address: Address  # Nested model

class User(BaseModel):
    """User with nested contact info"""
    id: int
    username: str
    contact: ContactInfo  # Nested model
    tags: List[str] = []

@app.post("/users/")
async def create_user(user: User):
    """
    Nested models are validated recursively.
    FastAPI generates complete JSON schema.
    """
    return {
        "user_id": user.id,
        "username": user.username,
        "email": user.contact.email,
        "city": user.contact.address.city
    }
```

## Complex Nesting

### Deep Nesting

```python
# Example 2: Deeply nested models
from fastapi import FastAPI
from pydantic import BaseModel
from typing import List, Optional
from datetime import datetime

app = FastAPI()

class GeoLocation(BaseModel):
    latitude: float
    longitude: float

class Address(BaseModel):
    street: str
    city: str
    country: str
    location: Optional[GeoLocation] = None

class Company(BaseModel):
    name: str
    address: Address
    website: Optional[str] = None

class Employment(BaseModel):
    company: Company
    position: str
    start_date: datetime
    end_date: Optional[datetime] = None
    is_current: bool = False

class Person(BaseModel):
    name: str
    email: str
    home_address: Address
    employment_history: List[Employment] = []

@app.post("/persons/")
async def create_person(person: Person):
    """
    Deep nesting creates complex schemas.
    Validation happens at every level.
    """
    return {
        "name": person.name,
        "city": person.home_address.city,
        "jobs": len(person.employment_history)
    }
```

## Lists of Models

### Nested Lists

```python
# Example 3: Lists of nested models
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import List

app = FastAPI()

class Tag(BaseModel):
    name: str
    color: str = "gray"

class Comment(BaseModel):
    author: str
    text: str
    likes: int = 0

class Post(BaseModel):
    title: str
    content: str
    tags: List[Tag] = []  # List of models
    comments: List[Comment] = []
    related_posts: List[int] = []  # List of simple types

@app.post("/posts/")
async def create_post(post: Post):
    """
    Lists of models are validated element by element.
    Empty lists are valid by default.
    """
    return {
        "title": post.title,
        "tag_count": len(post.tags),
        "comment_count": len(post.comments)
    }
```

## Optional Nested Models

### Conditional Nesting

```python
# Example 4: Optional nested models
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Optional
from datetime import datetime

app = FastAPI()

class ShippingInfo(BaseModel):
    address: str
    method: str
    tracking_number: Optional[str] = None

class PaymentInfo(BaseModel):
    method: str
    amount: float
    currency: str = "USD"

class Order(BaseModel):
    order_id: int
    items: list[str]
    payment: PaymentInfo
    shipping: Optional[ShippingInfo] = None  # Optional for digital goods
    completed_at: Optional[datetime] = None

@app.post("/orders/")
async def create_order(order: Order):
    """
    Optional nested models can be None.
    Useful for conditional data requirements.
    """
    result = {
        "order_id": order.order_id,
        "payment_method": order.payment.method
    }
    if order.shipping:
        result["shipping_method"] = order.shipping.method
    
    return result
```

## Model Inheritance

### Extending Models

```python
# Example 5: Model inheritance
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Optional
from datetime import datetime

app = FastAPI()

# Base model
class TimestampMixin(BaseModel):
    created_at: datetime = datetime.now()
    updated_at: Optional[datetime] = None

# Base item model
class ItemBase(BaseModel):
    name: str
    description: Optional[str] = None
    price: float

# Extended models
class ItemCreate(ItemBase):
    """For creating items"""
    pass

class ItemUpdate(BaseModel):
    """For updating items (all optional)"""
    name: Optional[str] = None
    description: Optional[str] = None
    price: Optional[float] = None

class Item(ItemBase, TimestampMixin):
    """Complete item with all fields"""
    id: int
    owner_id: int

@app.post("/items/", response_model=Item)
async def create_item(item: ItemCreate):
    """Inheritance enables DRY model definitions"""
    return Item(
        id=1,
        owner_id=1,
        **item.model_dump()
    )
```

## Serialization Options

### Controlling Output

```python
# Example 6: Nested model serialization
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import List, Optional

app = FastAPI()

class Address(BaseModel):
    street: str
    city: str
    country: str
    postal_code: str

class User(BaseModel):
    id: int
    username: str
    email: str
    address: Address
    password_hash: str = Field(exclude=True)  # Exclude from output

@app.get("/users/{user_id}")
async def get_user(user_id: int) -> User:
    """Exclude specified fields from response"""
    return User(
        id=user_id,
        username="john",
        email="john@example.com",
        address=Address(
            street="123 Main St",
            city="New York",
            country="US",
            postal_code="10001"
        ),
        password_hash="secret_hash"
    )

# Response excludes password_hash
```

## Best Practices

### Organization Guidelines

```python
# Example 7: Best practices for nested models
from fastapi import FastAPI
from pydantic import BaseModel, Field
from typing import List, Optional
from datetime import datetime

app = FastAPI()

# Reusable components
class Address(BaseModel):
    """Reusable address model"""
    street: str = Field(..., max_length=200)
    city: str = Field(..., max_length=100)
    country: str = Field(..., max_length=2)
    postal_code: str = Field(..., max_length=20)

class Contact(BaseModel):
    """Reusable contact model"""
    email: str
    phone: Optional[str] = None

# Composed models
class Company(BaseModel):
    name: str
    address: Address
    contact: Contact

class Employee(BaseModel):
    name: str
    contact: Contact
    company: Company
    start_date: datetime

# API models
class EmployeeCreate(BaseModel):
    """Simplified for API input"""
    name: str
    contact: Contact
    company_id: int  # Reference instead of full model

@app.post("/employees/")
async def create_employee(employee: EmployeeCreate):
    """
    Best practices:
    1. Create reusable components
    2. Keep API models separate from domain models
    3. Use references for existing resources
    """
    return {"name": employee.name, "company_id": employee.company_id}
```

## Summary

| Pattern | Use Case | Example |
|---------|----------|---------|
| Basic nesting | Composition | `User` contains `Address` |
| Lists | Collections | `List[Tag]` |
| Optional | Conditional data | `Optional[ShippingInfo]` |
| Inheritance | Reuse | `Item` extends `ItemBase` |
| Mixins | Shared fields | `TimestampMixin` |

## Next Steps

Continue learning about:
- [Enums and Constants](./06_enums_and_constants.md) - Fixed values
- [Request Body](../03_request_body/01_basic_request_body.md) - Body handling
- [Responses](../04_responses_and_status_codes/01_default_responses.md) - Response formatting
