# Connection Pooling

## What You'll Learn
- Database connection pooling
- HTTP connection pooling
- Redis connection pooling

## Prerequisites
- Completed async optimization

## Database Pooling

```python
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.orm import sessionmaker

engine = create_async_engine(
    "postgresql+asyncpg://user:pass@localhost/db",
    pool_size=20,          # Connections to keep open
    max_overflow=10,       # Additional connections when needed
    pool_pre_ping=True     # Verify connections
)

async_session = sessionmaker(
    engine, class_=AsyncSession, expire_on_commit=False
)
```

## HTTP Client Pooling

```python
import httpx

# Reuse client for connection pooling
client = httpx.AsyncClient(
    limits=httpx.Limits(
        max_keepalive_connections=20,
        max_connections=100
    )
)

async def fetch_data():
    response = await client.get("https://api.example.com")
    return response.json()
```

## Redis Pooling

```python
import redis.asyncio as aioredis

pool = aioredis.ConnectionPool(
    host='localhost',
    port=6379,
    max_connections=50
)

async def get_redis():
    return aioredis.Redis(connection_pool=pool)
```

## Summary
- Use connection pools for efficiency
- Configure appropriate pool sizes
- Monitor pool usage

## Next Steps
→ Move to `25-microservices/`
