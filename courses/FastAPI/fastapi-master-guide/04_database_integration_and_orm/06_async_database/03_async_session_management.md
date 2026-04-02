# Async Session Management

## Overview

Async sessions enable non-blocking database operations in FastAPI.

## Session Configuration

### Async Session Setup

```python
# Example 1: Async session configuration
from sqlalchemy.ext.asyncio import (
    create_async_engine,
    AsyncSession,
    async_sessionmaker
)

# Async engine
async_engine = create_async_engine(
    "postgresql+asyncpg://user:pass@localhost/db",
    pool_size=20,
    max_overflow=10
)

# Async session factory
AsyncSessionLocal = async_sessionmaker(
    async_engine,
    class_=AsyncSession,
    expire_on_commit=False
)

# Session dependency
async def get_async_db():
    async with AsyncSessionLocal() as session:
        try:
            yield session
        finally:
            await session.close()
```

### FastAPI Integration

```python
# Example 2: Using async session
from fastapi import FastAPI, Depends

app = FastAPI()

@app.get("/users/")
async def list_users(db: AsyncSession = Depends(get_async_db)):
    """List users with async session"""
    result = await db.execute(select(User))
    return result.scalars().all()

@app.post("/users/")
async def create_user(
    user: UserCreate,
    db: AsyncSession = Depends(get_async_db)
):
    """Create user with async session"""
    db_user = User(**user.dict())
    db.add(db_user)
    await db.commit()
    await db.refresh(db_user)
    return db_user
```

## Summary

Async sessions enable non-blocking database operations.

## Next Steps

Continue learning about:
- [Async Query Execution](./04_async_query_execution.md)
- [Async Transaction Management](./07_async_transaction_management.md)
