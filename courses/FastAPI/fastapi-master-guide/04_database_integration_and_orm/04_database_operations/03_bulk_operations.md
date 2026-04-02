# Bulk Operations

## Overview

Bulk operations improve performance when creating, updating, or deleting multiple records at once.

## Bulk Insert

### SQLAlchemy Bulk Insert

```python
# Example 1: Bulk insert patterns
from sqlalchemy import insert
from sqlalchemy.orm import Session

def bulk_insert_users(db: Session, users_data: list[dict]):
    """Bulk insert using SQLAlchemy core"""
    stmt = insert(User).values(users_data)
    db.execute(stmt)
    db.commit()

def bulk_insert_orm(db: Session, users_data: list[dict]):
    """Bulk insert using ORM"""
    users = [User(**data) for data in users_data]
    db.add_all(users)
    db.commit()

# FastAPI endpoint
@app.post("/users/bulk/")
async def create_users_bulk(
    users: list[UserCreate],
    db: Session = Depends(get_db)
):
    """Create multiple users in one operation"""
    users_data = [user.dict() for user in users]
    stmt = insert(User).values(users_data)
    db.execute(stmt)
    db.commit()
    return {"created": len(users)}
```

## Bulk Update

```python
# Example 2: Bulk update
from sqlalchemy import update

def bulk_update_users(db: Session, user_ids: list[int], **kwargs):
    """Bulk update multiple users"""
    stmt = (
        update(User)
        .where(User.id.in_(user_ids))
        .values(**kwargs)
    )
    result = db.execute(stmt)
    db.commit()
    return result.rowcount

@app.patch("/users/bulk/")
async def update_users_bulk(
    user_ids: list[int],
    updates: UserUpdate,
    db: Session = Depends(get_db)
):
    """Update multiple users at once"""
    update_data = updates.dict(exclude_unset=True)
    count = bulk_update_users(db, user_ids, **update_data)
    return {"updated": count}
```

## Bulk Delete

```python
# Example 3: Bulk delete
from sqlalchemy import delete

def bulk_delete_users(db: Session, user_ids: list[int]):
    """Bulk delete users"""
    stmt = delete(User).where(User.id.in_(user_ids))
    result = db.execute(stmt)
    db.commit()
    return result.rowcount

@app.delete("/users/bulk/")
async def delete_users_bulk(
    user_ids: list[int],
    db: Session = Depends(get_db)
):
    """Delete multiple users at once"""
    count = bulk_delete_users(db, user_ids)
    return {"deleted": count}
```

## Summary

Bulk operations significantly improve performance for batch processing.

## Next Steps

Continue learning about:
- [Database Functions](./04_database_functions.md)
- [Batch Processing](./08_batch_processing.md)
