# Async Query Execution

## Overview

Async queries enable non-blocking database operations in FastAPI applications.

## Async Query Patterns

### Basic Async Queries

```python
# Example 1: Async query patterns
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select
from app.models import User

class AsyncUserRepository:
    """Async user repository"""

    def __init__(self, session: AsyncSession):
        self.session = session

    async def get_by_id(self, user_id: int) -> User | None:
        """Get user by ID asynchronously"""
        result = await self.session.execute(
            select(User).where(User.id == user_id)
        )
        return result.scalar_one_or_none()

    async def get_by_username(self, username: str) -> User | None:
        """Get user by username"""
        result = await self.session.execute(
            select(User).where(User.username == username)
        )
        return result.scalar_one_or_none()

    async def list_all(self, skip: int = 0, limit: int = 100) -> list[User]:
        """List users with pagination"""
        result = await self.session.execute(
            select(User).offset(skip).limit(limit)
        )
        return list(result.scalars().all())

    async def create(self, username: str, email: str) -> User:
        """Create user asynchronously"""
        user = User(username=username, email=email)
        self.session.add(user)
        await self.session.commit()
        await self.session.refresh(user)
        return user
```

### Filtering and Sorting

```python
# Example 2: Async filtering
from sqlalchemy import and_, or_, desc

async def search_users(
    session: AsyncSession,
    username: str = None,
    is_active: bool = None
) -> list[User]:
    """Search users with async query"""
    query = select(User)

    if username:
        query = query.where(User.username.ilike(f"%{username}%"))
    if is_active is not None:
        query = query.where(User.is_active == is_active)

    query = query.order_by(desc(User.created_at))

    result = await session.execute(query)
    return list(result.scalars().all())
```

## Summary

Async queries enable non-blocking database access in FastAPI.

## Next Steps

Continue learning about:
- [Async Relationship Loading](./05_async_relationship_loading.md)
- [Async Transaction Management](./07_async_transaction_management.md)
