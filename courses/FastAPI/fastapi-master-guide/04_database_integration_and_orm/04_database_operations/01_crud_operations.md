# CRUD Operations

## Overview

CRUD (Create, Read, Update, Delete) operations are the foundation of database interactions. This guide covers comprehensive CRUD patterns for FastAPI.

## Create Operations

### Basic Creation

```python
# Example 1: Create operations with validation
from fastapi import FastAPI, HTTPException, Depends
from sqlalchemy.orm import Session
from pydantic import BaseModel, Field, EmailStr
from typing import Optional
from datetime import datetime

app = FastAPI()

class UserCreate(BaseModel):
    """Schema for creating users"""
    username: str = Field(..., min_length=3, max_length=50)
    email: EmailStr
    password: str = Field(..., min_length=12)

class UserResponse(BaseModel):
    """Schema for user responses"""
    id: int
    username: str
    email: str
    is_active: bool
    created_at: datetime

    class Config:
        from_attributes = True

@app.post("/users/", response_model=UserResponse, status_code=201)
async def create_user(
    user_data: UserCreate,
    db: Session = Depends(get_db)
):
    """Create new user with validation"""
    # Check if username exists
    existing = db.query(User).filter(User.username == user_data.username).first()
    if existing:
        raise HTTPException(400, "Username already exists")

    # Check if email exists
    existing_email = db.query(User).filter(User.email == user_data.email).first()
    if existing_email:
        raise HTTPException(400, "Email already exists")

    # Create user
    user = User(
        username=user_data.username,
        email=user_data.email,
        hashed_password=hash_password(user_data.password)
    )

    db.add(user)
    db.commit()
    db.refresh(user)

    return user

@app.post("/users/bulk/", response_model=list[UserResponse], status_code=201)
async def create_users_bulk(
    users_data: list[UserCreate],
    db: Session = Depends(get_db)
):
    """Create multiple users in one transaction"""
    users = []
    for user_data in users_data:
        user = User(
            username=user_data.username,
            email=user_data.email,
            hashed_password=hash_password(user_data.password)
        )
        users.append(user)

    db.add_all(users)
    db.commit()

    for user in users:
        db.refresh(user)

    return users
```

## Read Operations

### Filtering and Pagination

```python
# Example 2: Read operations with filtering
from fastapi import Query
from sqlalchemy import or_, and_

@app.get("/users/", response_model=list[UserResponse])
async def list_users(
    skip: int = Query(0, ge=0),
    limit: int = Query(20, ge=1, le=100),
    search: Optional[str] = None,
    is_active: Optional[bool] = None,
    db: Session = Depends(get_db)
):
    """List users with filtering and pagination"""
    query = db.query(User)

    # Apply filters
    if search:
        query = query.filter(
            or_(
                User.username.ilike(f"%{search}%"),
                User.email.ilike(f"%{search}%")
            )
        )

    if is_active is not None:
        query = query.filter(User.is_active == is_active)

    # Apply pagination
    users = query.offset(skip).limit(limit).all()

    return users

@app.get("/users/{user_id}", response_model=UserResponse)
async def get_user(user_id: int, db: Session = Depends(get_db)):
    """Get user by ID"""
    user = db.query(User).filter(User.id == user_id).first()
    if not user:
        raise HTTPException(404, "User not found")
    return user

@app.get("/users/search/", response_model=list[UserResponse])
async def search_users(
    q: str = Query(..., min_length=1),
    db: Session = Depends(get_db)
):
    """Search users by username or email"""
    users = db.query(User).filter(
        or_(
            User.username.ilike(f"%{q}%"),
            User.email.ilike(f"%{q}%")
        )
    ).limit(20).all()

    return users
```

## Update Operations

### Partial Updates

```python
# Example 3: Update operations
from fastapi import Patch

class UserUpdate(BaseModel):
    """Schema for updating users - all fields optional"""
    username: Optional[str] = Field(None, min_length=3, max_length=50)
    email: Optional[EmailStr] = None
    is_active: Optional[bool] = None

@app.patch("/users/{user_id}", response_model=UserResponse)
async def update_user(
    user_id: int,
    user_data: UserUpdate,
    db: Session = Depends(get_db)
):
    """Update user - partial updates supported"""
    user = db.query(User).filter(User.id == user_id).first()
    if not user:
        raise HTTPException(404, "User not found")

    # Update only provided fields
    update_data = user_data.model_dump(exclude_unset=True)

    # Validate unique constraints
    if "username" in update_data:
        existing = db.query(User).filter(
            and_(
                User.username == update_data["username"],
                User.id != user_id
            )
        ).first()
        if existing:
            raise HTTPException(400, "Username already taken")

    if "email" in update_data:
        existing = db.query(User).filter(
            and_(
                User.email == update_data["email"],
                User.id != user_id
            )
        ).first()
        if existing:
            raise HTTPException(400, "Email already taken")

    # Apply updates
    for key, value in update_data.items():
        setattr(user, key, value)

    user.updated_at = datetime.utcnow()
    db.commit()
    db.refresh(user)

    return user
```

## Delete Operations

### Soft and Hard Delete

```python
# Example 4: Delete operations
@app.delete("/users/{user_id}", status_code=204)
async def delete_user(
    user_id: int,
    soft: bool = Query(True, description="Soft delete (default)"),
    db: Session = Depends(get_db)
):
    """Delete user - soft or hard delete"""
    user = db.query(User).filter(User.id == user_id).first()
    if not user:
        raise HTTPException(404, "User not found")

    if soft:
        # Soft delete - mark as deleted
        user.is_deleted = True
        user.deleted_at = datetime.utcnow()
        db.commit()
    else:
        # Hard delete - remove from database
        db.delete(user)
        db.commit()

@app.post("/users/{user_id}/restore", response_model=UserResponse)
async def restore_user(user_id: int, db: Session = Depends(get_db)):
    """Restore soft-deleted user"""
    user = db.query(User).filter(
        and_(
            User.id == user_id,
            User.is_deleted == True
        )
    ).first()

    if not user:
        raise HTTPException(404, "User not found or not deleted")

    user.is_deleted = False
    user.deleted_at = None
    db.commit()
    db.refresh(user)

    return user
```

## Bulk Operations

### Efficient Bulk Operations

```python
# Example 5: Bulk operations
from sqlalchemy import update, delete

@app.post("/users/bulk-update/")
async def bulk_update_users(
    user_ids: list[int],
    updates: UserUpdate,
    db: Session = Depends(get_db)
):
    """Bulk update multiple users"""
    update_data = updates.model_dump(exclude_unset=True)

    if not update_data:
        raise HTTPException(400, "No update data provided")

    # Use SQLAlchemy bulk update
    stmt = (
        update(User)
        .where(User.id.in_(user_ids))
        .values(**update_data)
    )

    result = db.execute(stmt)
    db.commit()

    return {"updated_count": result.rowcount}

@app.delete("/users/bulk-delete/")
async def bulk_delete_users(
    user_ids: list[int],
    db: Session = Depends(get_db)
):
    """Bulk delete users"""
    stmt = delete(User).where(User.id.in_(user_ids))
    result = db.execute(stmt)
    db.commit()

    return {"deleted_count": result.rowcount}
```

## Advanced Queries

### Complex Filtering

```python
# Example 6: Advanced query patterns
from sqlalchemy import func, desc, asc

@app.get("/users/stats/")
async def user_stats(db: Session = Depends(get_db)):
    """Get user statistics"""
    total = db.query(func.count(User.id)).scalar()
    active = db.query(func.count(User.id)).filter(User.is_active == True).scalar()
    deleted = db.query(func.count(User.id)).filter(User.is_deleted == True).scalar()

    return {
        "total": total,
        "active": active,
        "inactive": total - active,
        "deleted": deleted
    }

@app.get("/users/recent/", response_model=list[UserResponse])
async def recent_users(
    days: int = Query(7, ge=1, le=365),
    db: Session = Depends(get_db)
):
    """Get recently created users"""
    cutoff = datetime.utcnow() - timedelta(days=days)

    users = (
        db.query(User)
        .filter(User.created_at >= cutoff)
        .order_by(desc(User.created_at))
        .limit(50)
        .all()
    )

    return users
```

## Summary

| Operation | Method | HTTP Status | Example |
|-----------|--------|-------------|---------|
| Create | `db.add()` | 201 | `POST /users/` |
| Read | `db.query()` | 200 | `GET /users/{id}` |
| Update | `setattr()` | 200 | `PATCH /users/{id}` |
| Delete | `db.delete()` | 204 | `DELETE /users/{id}` |

## Next Steps

Continue learning about:
- [Complex Queries](./02_complex_queries.md) - Advanced queries
- [Bulk Operations](./03_bulk_operations.md) - Batch processing
- [Transaction Management](../01_database_concepts/04_transaction_management.md) - Transactions
