# Async Database Access

## What You'll Learn
- Using async databases with Python
- SQLAlchemy async support
- Connecting to databases asynchronously

## Prerequisites
- Completed SQLAlchemy ORM
- Understanding async/await

## Installing Async Support

```bash
pip install sqlalchemy[asyncio] asyncpg aiosqlite
```

## Async SQLAlchemy

```python
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.orm import sessionmaker

# For SQLite
engine = create_async_engine("sqlite+aiosqlite:///db.sqlite")

# For PostgreSQL
engine = create_async_engine("postgresql+asyncpg://user:pass@host/db")

# Create session
AsyncSessionLocal = sessionmaker(
    engine, class_=AsyncSession, expire_on_commit=False
)

# Dependency for FastAPI
async def get_db():
    async with AsyncSessionLocal() as session:
        yield session

# Async queries
async def get_users(session: AsyncSession):
    result = await session.execute(select(User))
    return result.scalars().all()

async def create_user(session: AsyncSession, user: UserCreate):
    session.add(user)
    await session.commit()
    await session.refresh(user)
    return user
```

## Summary
- Use `create_async_engine()` for async database connections
- Use `AsyncSession` for async queries
- Use `await session.execute()` for queries

## Next Steps
→ Continue to `../06-frontend-integration/01-serving-static-files.md`
