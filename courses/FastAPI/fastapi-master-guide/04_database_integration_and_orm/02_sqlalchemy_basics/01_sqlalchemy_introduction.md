# SQLAlchemy Introduction

## Overview

SQLAlchemy is the most popular Python SQL toolkit and ORM. This guide covers SQLAlchemy 2.0 with FastAPI integration.

## SQLAlchemy Core vs ORM

### Understanding the Layers

```python
# Example 1: SQLAlchemy layers
"""
SQLAlchemy Architecture:

1. Core (Low-level)
   - SQL expression language
   - Schema definition
   - Connection management

2. ORM (High-level)
   - Object-relational mapping
   - Relationship management
   - Session management

FastAPI works best with SQLAlchemy ORM + Core when needed.
"""

from sqlalchemy import create_engine, MetaData, Table, Column, Integer, String
from sqlalchemy.orm import DeclarativeBase, Session, Mapped, mapped_column
from typing import Optional

# SQLAlchemy 2.0 style declarative base
class Base(DeclarativeBase):
    """Base class for all models"""
    pass

# Modern model definition (SQLAlchemy 2.0)
class User(Base):
    __tablename__ = "users"

    # Using Mapped types for better IDE support
    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(50), unique=True)
    email: Mapped[str] = mapped_column(String(100), unique=True)
    full_name: Mapped[Optional[str]] = mapped_column(String(100))
    is_active: Mapped[bool] = mapped_column(default=True)

    def __repr__(self) -> str:
        return f"<User(id={self.id}, username='{self.username}')>"

class Post(Base):
    __tablename__ = "posts"

    id: Mapped[int] = mapped_column(primary_key=True)
    title: Mapped[str] = mapped_column(String(200))
    content: Mapped[str] = mapped_column()
    author_id: Mapped[int] = mapped_column(ForeignKey("users.id"))
    published: Mapped[bool] = mapped_column(default=False)
```

## Engine Configuration

### Database Connection

```python
# Example 2: Engine setup with FastAPI
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.pool import QueuePool

# Database URL
DATABASE_URL = "postgresql://user:password@localhost/fastapi_db"

# Create engine with connection pooling
engine = create_engine(
    DATABASE_URL,
    poolclass=QueuePool,
    pool_size=5,           # Number of connections to maintain
    max_overflow=10,       # Additional connections when pool is full
    pool_timeout=30,       # Timeout for getting connection
    pool_recycle=1800,     # Recycle connections after 30 minutes
    echo=False             # Set True to log SQL queries
)

# Session factory
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Dependency for FastAPI
def get_db():
    """Database session dependency"""
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# FastAPI integration
from fastapi import FastAPI, Depends

app = FastAPI()

@app.get("/users/")
def list_users(db: Session = Depends(get_db)):
    users = db.query(User).all()
    return users
```

## Model Definition Patterns

### Common Patterns

```python
# Example 3: Model definition patterns
from sqlalchemy import String, Boolean, DateTime, func, Text
from sqlalchemy.orm import Mapped, mapped_column, DeclarativeBase
from datetime import datetime
from typing import Optional

class Base(DeclarativeBase):
    pass

class TimestampMixin:
    """Mixin for created/updated timestamps"""
    created_at: Mapped[datetime] = mapped_column(
        DateTime(timezone=True),
        server_default=func.now()
    )
    updated_at: Mapped[Optional[datetime]] = mapped_column(
        DateTime(timezone=True),
        onupdate=func.now()
    )

class SoftDeleteMixin:
    """Mixin for soft delete"""
    is_deleted: Mapped[bool] = mapped_column(default=False)
    deleted_at: Mapped[Optional[datetime]] = mapped_column(DateTime(timezone=True))

class User(Base, TimestampMixin, SoftDeleteMixin):
    """User model with mixins"""
    __tablename__ = "users"

    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(50), unique=True, index=True)
    email: Mapped[str] = mapped_column(String(100), unique=True, index=True)
    hashed_password: Mapped[str] = mapped_column(String(128))
    bio: Mapped[Optional[str]] = mapped_column(Text)
    is_active: Mapped[bool] = mapped_column(default=True)

    # Table arguments for indexes
    __table_args__ = (
        Index('idx_username_email', 'username', 'email'),
    )
```

## Session Management

### FastAPI Integration

```python
# Example 4: Complete session management
from contextlib import asynccontextmanager
from fastapi import FastAPI, Depends
from sqlalchemy.orm import Session

# Session dependency with type hints
def get_db() -> Session:
    """Get database session"""
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# For dependency injection in path operations
async def get_db_session(db: Session = Depends(get_db)):
    return db

# CRUD operations with session
class UserRepository:
    """User repository with session management"""

    def __init__(self, db: Session):
        self.db = db

    def create(self, username: str, email: str, password: str) -> User:
        """Create new user"""
        user = User(
            username=username,
            email=email,
            hashed_password=hash_password(password)
        )
        self.db.add(user)
        self.db.commit()
        self.db.refresh(user)
        return user

    def get_by_id(self, user_id: int) -> Optional[User]:
        """Get user by ID"""
        return self.db.query(User).filter(User.id == user_id).first()

    def get_by_username(self, username: str) -> Optional[User]:
        """Get user by username"""
        return self.db.query(User).filter(User.username == username).first()

    def update(self, user_id: int, **kwargs) -> Optional[User]:
        """Update user"""
        user = self.get_by_id(user_id)
        if user:
            for key, value in kwargs.items():
                setattr(user, key, value)
            self.db.commit()
            self.db.refresh(user)
        return user

    def delete(self, user_id: int) -> bool:
        """Delete user (soft delete)"""
        user = self.get_by_id(user_id)
        if user:
            user.is_deleted = True
            user.deleted_at = datetime.utcnow()
            self.db.commit()
            return True
        return False

# FastAPI endpoints
@app.post("/users/")
def create_user(
    username: str,
    email: str,
    password: str,
    db: Session = Depends(get_db)
):
    repo = UserRepository(db)
    return repo.create(username, email, password)

@app.get("/users/{user_id}")
def get_user(user_id: int, db: Session = Depends(get_db)):
    repo = UserRepository(db)
    user = repo.get_by_id(user_id)
    if not user:
        raise HTTPException(404, "User not found")
    return user
```

## Best Practices

### SQLAlchemy Guidelines

```python
# Example 5: Best practices
"""
SQLAlchemy Best Practices:

1. Use SQLAlchemy 2.0 syntax (Mapped types)
2. Always use sessions as context managers
3. Use async sessions for async endpoints
4. Define models in separate files
5. Use mixins for common fields
6. Index frequently queried columns
7. Use connection pooling
8. Never expose session to business logic
"""

# Project structure recommendation:
"""
app/
├── database/
│   ├── __init__.py
│   ├── base.py          # Base class
│   ├── session.py       # Session management
│   └── engine.py        # Engine configuration
├── models/
│   ├── __init__.py
│   ├── user.py
│   ├── post.py
│   └── mixins.py
├── schemas/
│   ├── __init__.py
│   ├── user.py
│   └── post.py
└── repositories/
    ├── __init__.py
    ├── user.py
    └── post.py
"""
```

## Summary

| Component | Purpose | Example |
|-----------|---------|---------|
| Base | Model foundation | `class Base(DeclarativeBase)` |
| Engine | Database connection | `create_engine(url)` |
| Session | Transaction scope | `SessionLocal()` |
| Model | Table definition | `class User(Base)` |
| Relationship | Link tables | `relationship()` |

## Next Steps

Continue learning about:
- [Models and Schemas](./02_models_and_schemas.md) - Model definition
- [Basic Queries](./03_basic_queries.md) - Query operations
- [Relationship Mapping](./04_relationship_mapping.md) - Relationships
