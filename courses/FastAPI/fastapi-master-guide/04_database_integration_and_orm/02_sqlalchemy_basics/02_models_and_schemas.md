# Models and Schemas

## Overview

Proper separation of models (database) and schemas (API) is essential for clean FastAPI applications.

## SQLAlchemy Models

### Model Definition

```python
# Example 1: SQLAlchemy models
from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Text
from sqlalchemy.orm import relationship, Mapped, mapped_column, DeclarativeBase
from datetime import datetime
from typing import Optional, List

class Base(DeclarativeBase):
    """Base class for all models"""
    pass

class TimestampMixin:
    """Mixin for timestamps"""
    created_at: Mapped[datetime] = mapped_column(default=datetime.utcnow)
    updated_at: Mapped[Optional[datetime]] = mapped_column(onupdate=datetime.utcnow)

class User(Base, TimestampMixin):
    """User model"""
    __tablename__ = "users"

    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(50), unique=True, index=True)
    email: Mapped[str] = mapped_column(String(100), unique=True, index=True)
    hashed_password: Mapped[str] = mapped_column(String(128))
    is_active: Mapped[bool] = mapped_column(default=True)
    is_superuser: Mapped[bool] = mapped_column(default=False)

    # Relationships
    posts: Mapped[List["Post"]] = relationship(back_populates="author")

class Post(Base, TimestampMixin):
    """Post model"""
    __tablename__ = "posts"

    id: Mapped[int] = mapped_column(primary_key=True)
    title: Mapped[str] = mapped_column(String(200))
    content: Mapped[str] = mapped_column(Text)
    published: Mapped[bool] = mapped_column(default=False)
    author_id: Mapped[int] = mapped_column(ForeignKey("users.id"))

    # Relationships
    author: Mapped["User"] = relationship(back_populates="posts")
    comments: Mapped[List["Comment"]] = relationship(back_populates="post")

class Comment(Base, TimestampMixin):
    """Comment model"""
    __tablename__ = "comments"

    id: Mapped[int] = mapped_column(primary_key=True)
    content: Mapped[str] = mapped_column(Text)
    post_id: Mapped[int] = mapped_column(ForeignKey("posts.id"))
    author_id: Mapped[int] = mapped_column(ForeignKey("users.id"))

    post: Mapped["Post"] = relationship(back_populates="comments")
```

## Pydantic Schemas

### API Schemas

```python
# Example 2: Pydantic schemas
from pydantic import BaseModel, EmailStr, Field
from typing import Optional
from datetime import datetime

# Base schemas
class UserBase(BaseModel):
    username: str = Field(..., min_length=3, max_length=50)
    email: EmailStr

class UserCreate(UserBase):
    password: str = Field(..., min_length=12)

class UserUpdate(BaseModel):
    username: Optional[str] = Field(None, min_length=3, max_length=50)
    email: Optional[EmailStr] = None
    is_active: Optional[bool] = None

class UserResponse(UserBase):
    id: int
    is_active: bool
    created_at: datetime

    class Config:
        from_attributes = True  # Enable ORM mode

# Post schemas
class PostBase(BaseModel):
    title: str = Field(..., min_length=1, max_length=200)
    content: str
    published: bool = False

class PostCreate(PostBase):
    pass

class PostResponse(PostBase):
    id: int
    author_id: int
    created_at: datetime
    author: UserResponse

    class Config:
        from_attributes = True

# Nested response
class UserWithPosts(UserResponse):
    posts: list[PostResponse] = []
```

## Schema Validation

### Advanced Validation

```python
# Example 3: Schema validation
from pydantic import validator, model_validator

class UserRegistration(BaseModel):
    username: str = Field(..., min_length=3)
    email: EmailStr
    password: str = Field(..., min_length=12)
    password_confirm: str

    @validator('username')
    def validate_username(cls, v):
        if not v.isalnum():
            raise ValueError('Username must be alphanumeric')
        return v.lower()

    @validator('password')
    def validate_password(cls, v):
        if not any(c.isupper() for c in v):
            raise ValueError('Password must contain uppercase')
        if not any(c.isdigit() for c in v):
            raise ValueError('Password must contain digit')
        return v

    @model_validator(mode='after')
    def passwords_match(self):
        if self.password != self.password_confirm:
            raise ValueError('Passwords do not match')
        return self
```

## Usage with FastAPI

### Complete Example

```python
# Example 4: Using models and schemas
from fastapi import FastAPI, Depends, HTTPException

app = FastAPI()

@app.post("/users/", response_model=UserResponse, status_code=201)
async def create_user(
    user_data: UserCreate,
    db: Session = Depends(get_db)
):
    """Create user with validation"""
    # Check existence
    if db.query(User).filter(User.username == user_data.username).first():
        raise HTTPException(400, "Username taken")

    # Create user
    user = User(
        username=user_data.username,
        email=user_data.email,
        hashed_password=hash_password(user_data.password)
    )
    db.add(user)
    db.commit()
    db.refresh(user)

    return user  # Auto-converts to UserResponse

@app.get("/users/{user_id}", response_model=UserResponse)
async def get_user(user_id: int, db: Session = Depends(get_db)):
    user = db.query(User).filter(User.id == user_id).first()
    if not user:
        raise HTTPException(404, "User not found")
    return user
```

## Summary

| Component | Purpose | Location |
|-----------|---------|----------|
| Model | Database table | `models/` |
| Schema | API validation | `schemas/` |
| Base | Common fields | `models/base.py` |
| Mixin | Reusable fields | `models/mixins.py` |

## Next Steps

Continue learning about:
- [Basic Queries](./03_basic_queries.md) - Query operations
- [Relationship Mapping](./04_relationship_mapping.md) - Relationships
