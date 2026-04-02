# Redis Integration

## Overview

Redis is an in-memory data store used for caching, sessions, and real-time features in FastAPI applications.

## Setup and Configuration

### Basic Redis Connection

```python
# Example 1: Redis connection setup
import redis.asyncio as redis
from fastapi import FastAPI

app = FastAPI()

# Async Redis connection
redis_client = redis.Redis(
    host='localhost',
    port=6379,
    db=0,
    decode_responses=True,  # Decode bytes to strings
    max_connections=20
)

@app.on_event("startup")
async def startup():
    """Test Redis connection on startup"""
    await redis_client.ping()
    print("Redis connected")

@app.on_event("shutdown")
async def shutdown():
    """Close Redis connection on shutdown"""
    await redis_client.close()
```

## Basic Operations

### Key-Value Operations

```python
# Example 2: Basic Redis operations
import json
from typing import Optional, Any

class RedisCache:
    """Redis cache operations"""

    def __init__(self, client: redis.Redis):
        self.client = client

    async def get(self, key: str) -> Optional[Any]:
        """Get cached value"""
        value = await self.client.get(key)
        if value:
            return json.loads(value)
        return None

    async def set(
        self,
        key: str,
        value: Any,
        expire: int = 3600
    ):
        """Set cached value with expiration"""
        await self.client.setex(
            key,
            expire,
            json.dumps(value)
        )

    async def delete(self, key: str):
        """Delete cached value"""
        await self.client.delete(key)

    async def exists(self, key: str) -> bool:
        """Check if key exists"""
        return await self.client.exists(key) > 0

    async def increment(self, key: str, amount: int = 1) -> int:
        """Increment counter"""
        return await self.client.incrby(key, amount)

    async def get_or_set(
        self,
        key: str,
        factory,
        expire: int = 3600
    ):
        """Get from cache or compute and cache"""
        value = await self.get(key)
        if value is None:
            value = await factory()
            await self.set(key, value, expire)
        return value

cache = RedisCache(redis_client)
```

## Caching Patterns

### Cache-Aside Pattern

```python
# Example 3: Cache-aside pattern
from fastapi import FastAPI, HTTPException, Depends

app = FastAPI()

@app.get("/users/{user_id}")
async def get_user(user_id: int, db: Session = Depends(get_db)):
    """Get user with caching"""
    cache_key = f"user:{user_id}"

    # Try cache first
    cached = await cache.get(cache_key)
    if cached:
        return cached

    # Cache miss - query database
    user = db.query(User).filter(User.id == user_id).first()
    if not user:
        raise HTTPException(404, "User not found")

    # Cache result
    user_data = {"id": user.id, "username": user.username, "email": user.email}
    await cache.set(cache_key, user_data, expire=300)

    return user_data

@app.patch("/users/{user_id}")
async def update_user(user_id: int, data: dict, db: Session = Depends(get_db)):
    """Update user and invalidate cache"""
    user = db.query(User).filter(User.id == user_id).first()
    if not user:
        raise HTTPException(404, "User not found")

    # Update database
    for key, value in data.items():
        setattr(user, key, value)
    db.commit()

    # Invalidate cache
    await cache.delete(f"user:{user_id}")

    return {"message": "User updated"}
```

### Write-Through Pattern

```python
# Example 4: Write-through caching
async def create_user_write_through(
    username: str,
    email: str,
    db: Session
):
    """Create user with write-through caching"""
    # Create in database
    user = User(username=username, email=email)
    db.add(user)
    db.commit()
    db.refresh(user)

    # Immediately cache
    cache_key = f"user:{user.id}"
    await cache.set(cache_key, {
        "id": user.id,
        "username": user.username,
        "email": user.email
    }, expire=3600)

    return user
```

## Session Storage

### Redis Session Management

```python
# Example 5: Session storage with Redis
import uuid
from datetime import datetime, timedelta

class SessionManager:
    """Redis-based session management"""

    def __init__(self, client: redis.Redis, expire: int = 3600):
        self.client = client
        self.expire = expire

    async def create_session(self, user_id: int) -> str:
        """Create new session"""
        session_id = str(uuid.uuid4())
        session_data = {
            "user_id": user_id,
            "created_at": datetime.utcnow().isoformat()
        }

        await self.client.setex(
            f"session:{session_id}",
            self.expire,
            json.dumps(session_data)
        )

        return session_id

    async def get_session(self, session_id: str) -> Optional[dict]:
        """Get session data"""
        data = await self.client.get(f"session:{session_id}")
        return json.loads(data) if data else None

    async def delete_session(self, session_id: str):
        """Delete session (logout)"""
        await self.client.delete(f"session:{session_id}")

    async def refresh_session(self, session_id: str):
        """Refresh session expiration"""
        await self.client.expire(f"session:{session_id}", self.expire)

sessions = SessionManager(redis_client)
```

## Rate Limiting

### Redis Rate Limiting

```python
# Example 6: Rate limiting with Redis
class RateLimiter:
    """Redis-based rate limiter"""

    def __init__(self, client: redis.Redis):
        self.client = client

    async def is_rate_limited(
        self,
        key: str,
        max_requests: int,
        window_seconds: int
    ) -> tuple[bool, dict]:
        """Check if rate limited"""
        current = await self.client.get(f"ratelimit:{key}")

        if current is None:
            await self.client.setex(
                f"ratelimit:{key}",
                window_seconds,
                1
            )
            return False, {"remaining": max_requests - 1}

        current = int(current)
        if current >= max_requests:
            ttl = await self.client.ttl(f"ratelimit:{key}")
            return True, {"remaining": 0, "retry_after": ttl}

        await self.client.incr(f"ratelimit:{key}")
        return False, {"remaining": max_requests - current - 1}

limiter = RateLimiter(redis_client)

@app.middleware("http")
async def rate_limit_middleware(request, call_next):
    """Rate limit by IP"""
    client_ip = request.client.host
    is_limited, info = await limiter.is_rate_limited(
        f"ip:{client_ip}",
        max_requests=100,
        window_seconds=60
    )

    if is_limited:
        return JSONResponse(
            status_code=429,
            content={"error": "Rate limit exceeded"},
            headers={"Retry-After": str(info["retry_after"])}
        )

    response = await call_next(request)
    response.headers["X-RateLimit-Remaining"] = str(info["remaining"])
    return response
```

## Best Practices

### Redis Guidelines

```python
# Example 7: Redis best practices
"""
Redis Best Practices:

1. Set appropriate expiration times
   - Cache: 5-60 minutes
   - Sessions: 1-24 hours
   - Rate limits: 1-60 seconds

2. Use key namespacing
   - user:123
   - session:abc
   - ratelimit:ip:1.2.3.4

3. Handle connection failures
   - Implement retry logic
   - Fallback to database

4. Monitor memory usage
   - Redis is in-memory
   - Set maxmemory policy

5. Use pipelines for batch operations
   - Reduce network round trips
   - Atomic operations
"""

# Key naming convention
KEY_PATTERNS = {
    "user": "user:{id}",
    "session": "session:{id}",
    "cache": "cache:{resource}:{id}",
    "ratelimit": "ratelimit:{type}:{key}"
}
```

## Summary

| Operation | Command | Use Case |
|-----------|---------|----------|
| Get | `GET key` | Retrieve cached data |
| Set | `SET key value` | Cache data |
| SetEX | `SETEX key seconds value` | Cache with expiration |
| Delete | `DEL key` | Invalidate cache |
| Increment | `INCR key` | Counters |
| TTL | `TTL key` | Check expiration |

## Next Steps

Continue learning about:
- [Elasticsearch Integration](./03_elasticsearch_integration.md) - Full-text search
- [Caching Strategies](../08_database_performance/04_caching_strategies.md) - Advanced caching
