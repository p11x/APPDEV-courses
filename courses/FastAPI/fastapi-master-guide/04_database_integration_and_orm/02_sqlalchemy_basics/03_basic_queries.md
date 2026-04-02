# Basic Queries

## Overview

SQLAlchemy provides powerful query capabilities for FastAPI applications. This guide covers essential query patterns.

## Query Operations

### Basic CRUD Queries

```python
# Example 1: SQLAlchemy query patterns
from sqlalchemy.orm import Session
from sqlalchemy import select, update, delete
from app.models import User

class UserRepository:
    """User repository with common queries"""

    def __init__(self, db: Session):
        self.db = db

    def create(self, username: str, email: str) -> User:
        """Create a new user"""
        user = User(username=username, email=email)
        self.db.add(user)
        self.db.commit()
        self.db.refresh(user)
        return user

    def get_by_id(self, user_id: int) -> User | None:
        """Get user by ID"""
        return self.db.query(User).filter(User.id == user_id).first()

    def get_by_username(self, username: str) -> User | None:
        """Get user by username"""
        return self.db.query(User).filter(User.username == username).first()

    def list_all(self, skip: int = 0, limit: int = 100) -> list[User]:
        """List users with pagination"""
        return self.db.query(User).offset(skip).limit(limit).all()

    def update(self, user_id: int, **kwargs) -> User | None:
        """Update user fields"""
        user = self.get_by_id(user_id)
        if user:
            for key, value in kwargs.items():
                setattr(user, key, value)
            self.db.commit()
            self.db.refresh(user)
        return user

    def delete(self, user_id: int) -> bool:
        """Delete user"""
        user = self.get_by_id(user_id)
        if user:
            self.db.delete(user)
            self.db.commit()
            return True
        return False
```

### Filtering and Sorting

```python
# Example 2: Advanced filtering
from sqlalchemy import and_, or_, desc, asc

def search_users(
    db: Session,
    username: str = None,
    email: str = None,
    is_active: bool = None,
    sort_by: str = "created_at",
    sort_order: str = "desc"
) -> list[User]:
    """Search users with filters"""
    query = db.query(User)

    # Apply filters
    if username:
        query = query.filter(User.username.ilike(f"%{username}%"))
    if email:
        query = query.filter(User.email == email)
    if is_active is not None:
        query = query.filter(User.is_active == is_active)

    # Apply sorting
    column = getattr(User, sort_by)
    if sort_order == "desc":
        query = query.order_by(desc(column))
    else:
        query = query.order_by(asc(column))

    return query.all()
```

## Summary

SQLAlchemy provides intuitive query APIs for database operations.

## Next Steps

Continue learning about:
- [Complex Queries](../04_database_operations/02_complex_queries.md)
- [Relationship Mapping](./04_relationship_mapping.md)
