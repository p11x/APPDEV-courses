# 📦 Pydantic Models: Validation and Serialization

## 🎯 What You'll Learn

- Using Pydantic for request/response validation
- Field validators and nested models
- Response models for hiding internal fields

---

## Basic Model

```python
from pydantic import BaseModel, Field
from typing import Optional

class User(BaseModel):
    """User model with validation."""
    name: str                           # Required
    email: str                          # Required
    age: int = Field(ge=0, le=150)      # With validation: 0-150
    bio: Optional[str] = None           # Optional, with default None
    
# Usage
user = User(name="Alice", email="alice@example.com", age=30)
print(user.model_dump())  # Convert to dict
print(user.model_dump_json())  # Convert to JSON
```

---

## Validation

```python
from pydantic import field_validator

class User(BaseModel):
    email: str
    
    @field_validator("email")
    @classmethod
    def email_must_have_at(cls, v: str) -> str:
        if "@" not in v:
            raise ValueError("Invalid email")
        return v.lower()  # Normalize

# Try invalid
try:
    User(name="Test", email="invalid")  # Raises ValidationError
except Exception as e:
    print(e)
```

---

## Nested Models

```python
class Address(BaseModel):
    street: str
    city: str
    zip_code: str

class User(BaseModel):
    name: str
    address: Address  # Nested model

# Pydantic handles nested validation!
user = User(
    name="Alice",
    address={"street": "123 Main", "city": "NYC", "zip_code": "10001"}
)
```

---

## Response Models

```python
class UserBase(BaseModel):
    name: str
    email: str

class UserCreate(UserBase):
    password: str  # Only needed for creation

class UserResponse(UserBase):
    """Response without password!"""
    id: int
    
    model_config = {"from_attributes": True}

# In FastAPI
@app.post("/users", response_model=UserResponse)
def create_user(user: UserCreate):
    # Create user in DB...
    return UserResponse(id=1, name=user.name, email=user.email)
```

---

## Settings with Pydantic

```python
from pydantic_settings import BaseSettings

class Settings(BaseModel):
    """App settings from environment."""
    database_url: str
    secret_key: str
    debug: bool = False
    
    model_config = {"env_file": ".env"}

settings = Settings()  # Loads from .env automatically
```

---

## ✅ Summary

- Pydantic provides automatic validation
- Use Field() for constraints
- Response models hide internal fields
- pydantic-settings loads from .env

## 🔗 Further Reading

- [Pydantic Documentation](https://docs.pydantic.dev/)
