# Async SQLAlchemy Basics

## Overview

Async SQLAlchemy enables non-blocking database operations in FastAPI. This is essential for high-performance applications.

## Async Setup

### Engine and Session Configuration

```python
# Example 1: Async SQLAlchemy setup
from sqlalchemy.ext.asyncio import (
    create_async_engine,
    AsyncSession,
    async_sessionmaker
)
from sqlalchemy.orm import DeclarativeBase
from fastapi import FastAPI, Depends

# Async database URL (use asyncpg for PostgreSQL)
DATABASE_URL = "postgresql+asyncpg://user:password@localhost/fastapi_db"

# Create async engine
async_engine = create_async_engine(
    DATABASE_URL,
    echo=False,
    pool_size=5,
    max_overflow=10
)

# Async session factory
AsyncSessionLocal = async_sessionmaker(
    async_engine,
    class_=AsyncSession,
    expire_on_commit=False
)

# Base class
class Base(DeclarativeBase):
    pass

# Async session dependency
async def get_db() -> AsyncSession:
    """Get async database session"""
    async with AsyncSessionLocal() as session:
        try:
            yield session
        finally:
            await session.close()

# FastAPI app
app = FastAPI()

# Create tables on startup
@app.on_event("startup")
async def startup():
    async with async_engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)

@app.on_event("shutdown")
async def shutdown():
    await async_engine.dispose()
```

## Model Definitions

### Async-Compatible Models

```python
# Example 2: Models for async usage
from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.orm import relationship, Mapped, mapped_column
from datetime import datetime
from typing import Optional, List

class User(Base):
    __tablename__ = "users"

    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(50), unique=True, index=True)
    email: Mapped[str] = mapped_column(String(100), unique=True, index=True)
    hashed_password: Mapped[str] = mapped_column(String(128))
    is_active: Mapped[bool] = mapped_column(default=True)
    created_at: Mapped[datetime] = mapped_column(default=datetime.utcnow)

    # Relationships
    posts: Mapped[List["Post"]] = relationship(back_populates="author")

class Post(Base):
    __tablename__ = "posts"

    id: Mapped[int] = mapped_column(primary_key=True)
    title: Mapped[str] = mapped_column(String(200))
    content: Mapped[str] = mapped_column()
    author_id: Mapped[int] = mapped_column(ForeignKey("users.id"))
    published: Mapped[bool] = mapped_column(default=False)
    created_at: Mapped[datetime] = mapped_column(default=datetime.utcnow)

    # Relationships
    author: Mapped["User"] = relationship(back_populates="posts")
```

## Async CRUD Operations

### Complete Async CRUD

```python
# Example 3: Async CRUD operations
from sqlalchemy import select, update, delete
from sqlalchemy.orm import selectinload, joinedload

class AsyncUserRepository:
    """Async user repository"""

    def __init__(self, session: AsyncSession):
        self.session = session

    async def create(self, username: str, email: str, password: str) -> User:
        """Create user asynchronously"""
        user = User(
            username=username,
            email=email,
            hashed_password=hash_password(password)
        )
        self.session.add(user)
        await self.session.commit()
        await self.session.refresh(user)
        return user

    async def get_by_id(self, user_id: int) -> Optional[User]:
        """Get user by ID"""
        result = await self.session.execute(
            select(User).where(User.id == user_id)
        )
        return result.scalar_one_or_none()

    async def get_by_username(self, username: str) -> Optional[User]:
        """Get user by username"""
        result = await self.session.execute(
            select(User).where(User.username == username)
        )
        return result.scalar_one_or_none()

    async def list_users(
        self,
        skip: int = 0,
        limit: int = 20,
        is_active: Optional[bool] = None
    ) -> list[User]:
        """List users with filtering"""
        query = select(User)

        if is_active is not None:
            query = query.where(User.is_active == is_active)

        query = query.offset(skip).limit(limit)

        result = await self.session.execute(query)
        return list(result.scalars().all())

    async def update(self, user_id: int, **kwargs) -> Optional[User]:
        """Update user"""
        user = await self.get_by_id(user_id)
        if not user:
            return None

        for key, value in kwargs.items():
            setattr(user, key, value)

        await self.session.commit()
        await self.session.refresh(user)
        return user

    async def delete(self, user_id: int) -> bool:
        """Delete user"""
        user = await self.get_by_id(user_id)
        if not user:
            return False

        await self.session.delete(user)
        await self.session.commit()
        return True

# FastAPI endpoints with async
@app.post("/users/", response_model=UserResponse)
async def create_user(
    user_data: UserCreate,
    db: AsyncSession = Depends(get_db)
):
    repo = AsyncUserRepository(db)
    user = await repo.create(user_data.username, user_data.email, user_data.password)
    return user

@app.get("/users/", response_model=list[UserResponse])
async def list_users(
    skip: int = 0,
    limit: int = 20,
    db: AsyncSession = Depends(get_db)
):
    repo = AsyncUserRepository(db)
    return await repo.list_users(skip, limit)
```

## Async Relationship Loading

### Eager and Lazy Loading

```python
# Example 4: Async relationship loading
from sqlalchemy.orm import selectinload, joinedload

class AsyncPostRepository:
    """Async post repository with relationships"""

    def __init__(self, session: AsyncSession):
        self.session = session

    async def get_post_with_author(self, post_id: int) -> Optional[Post]:
        """Get post with author (eager loading)"""
        result = await self.session.execute(
            select(Post)
            .options(selectinload(Post.author))
            .where(Post.id == post_id)
        )
        return result.scalar_one_or_none()

    async def get_user_with_posts(self, user_id: int) -> Optional[User]:
        """Get user with all posts (eager loading)"""
        result = await self.session.execute(
            select(User)
            .options(selectinload(User.posts))
            .where(User.id == user_id)
        )
        return result.scalar_one_or_none()

    async def list_posts_with_authors(self, limit: int = 20) -> list[Post]:
        """List posts with authors loaded"""
        result = await self.session.execute(
            select(Post)
            .options(selectinload(Post.author))
            .order_by(Post.created_at.desc())
            .limit(limit)
        )
        return list(result.scalars().all())
```

## Async Transactions

### Transaction Management

```python
# Example 5: Async transactions
from sqlalchemy.ext.asyncio import AsyncSession

async def transfer_points(
    session: AsyncSession,
    from_user_id: int,
    to_user_id: int,
    amount: int
):
    """Transfer points between users (atomic transaction)"""
    async with session.begin():
        # Get users
        from_user = await session.get(User, from_user_id)
        to_user = await session.get(User, to_user_id)

        if not from_user or not to_user:
            raise ValueError("User not found")

        if from_user.points < amount:
            raise ValueError("Insufficient points")

        # Transfer
        from_user.points -= amount
        to_user.points += amount

        # Transaction commits automatically on exit

# Using the transaction
@app.post("/transfer/")
async def transfer(
    from_id: int,
    to_id: int,
    amount: int,
    db: AsyncSession = Depends(get_db)
):
    try:
        await transfer_points(db, from_id, to_id, amount)
        return {"success": True}
    except ValueError as e:
        raise HTTPException(400, str(e))
```

## Performance Benefits

### Why Use Async?

```python
# Example 6: Performance comparison
"""
Sync vs Async Performance:

Sync (blocking):
Request 1 → [DB Query ████████] → Response 1
Request 2 →                  [DB Query ████████] → Response 2
Request 3 →                                      [DB Query ████████] → Response 3
Total time: 24 units

Async (non-blocking):
Request 1 → [DB Query ████████] → Response 1
Request 2 → [DB Query ████████] → Response 2
Request 3 → [DB Query ████████] → Response 3
Total time: 8 units (3x faster!)

Async benefits:
✓ Handle more concurrent requests
✓ Better resource utilization
✓ Lower latency under load
✓ Non-blocking I/O operations
"""

# Benchmark example
import asyncio
import time

async def async_query_simulation():
    """Simulate async database query"""
    await asyncio.sleep(0.1)  # Non-blocking wait
    return {"result": "data"}

@app.get("/benchmark/async/")
async def async_benchmark():
    """Run 10 queries concurrently"""
    start = time.time()

    tasks = [async_query_simulation() for _ in range(10)]
    results = await asyncio.gather(*tasks)

    duration = time.time() - start
    return {
        "queries": len(results),
        "duration": round(duration, 3),
        "note": "~0.1s total (concurrent)"
    }
```

## Best Practices

### Async Guidelines

```python
# Example 7: Async best practices
"""
Async SQLAlchemy Best Practices:

1. Always use AsyncSession, not Session
2. Use await for all database operations
3. Use selectinload for relationships
4. Avoid lazy loading in async context
5. Use async engine with asyncpg
6. Handle connection pool properly
7. Use transactions for atomic operations
"""

# Good: Async pattern
@app.get("/users/{user_id}")
async def get_user(
    user_id: int,
    db: AsyncSession = Depends(get_db)
):
    result = await db.execute(
        select(User).options(selectinload(User.posts))
        .where(User.id == user_id)
    )
    return result.scalar_one_or_none()

# Bad: Don't mix sync and async
# @app.get("/users/{user_id}")
# async def get_user_bad(user_id: int, db: AsyncSession = Depends(get_db)):
#     return db.query(User).filter(User.id == user_id).first()  # Wrong!
```

## Summary

| Component | Async Version | Purpose |
|-----------|---------------|---------|
| Engine | `create_async_engine` | Connection management |
| Session | `AsyncSession` | Transaction scope |
| Query | `select()` | Query building |
| Execute | `await session.execute()` | Query execution |
| Commit | `await session.commit()` | Save changes |

## Next Steps

Continue learning about:
- [Async Connection Pooling](./02_async_connection_pooling.md) - Pool management
- [Async Session Management](./03_async_session_management.md) - Sessions
- [Async Query Execution](./04_async_query_execution.md) - Queries
