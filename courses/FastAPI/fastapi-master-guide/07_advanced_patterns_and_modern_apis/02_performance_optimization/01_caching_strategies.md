# Caching Strategies

## Overview

Caching is essential for FastAPI application performance. This guide covers multiple caching strategies and their implementations.

## Cache Layers

### Multi-Level Caching

```python
# Example 1: Multi-level cache architecture
from fastapi import FastAPI, Depends
from functools import lru_cache
import redis.asyncio as redis
import json
from typing import Optional, Any
from datetime import datetime, timedelta

app = FastAPI()

# Level 1: In-memory cache (fastest)
memory_cache: dict = {}

# Level 2: Redis cache (shared across instances)
redis_client = redis.Redis(host="localhost", port=6379, decode_responses=True)

# Level 3: Database (source of truth)
# Already covered in database section

class MultiLevelCache:
    """Multi-level cache implementation"""

    def __init__(self):
        self.memory_ttl = 60  # 1 minute
        self.redis_ttl = 300  # 5 minutes

    async def get(self, key: str) -> Optional[Any]:
        """Get from cache, checking each level"""

        # Check memory cache
        if key in memory_cache:
            entry = memory_cache[key]
            if entry["expires"] > datetime.utcnow():
                return entry["value"]
            del memory_cache[key]

        # Check Redis cache
        redis_value = await redis_client.get(key)
        if redis_value:
            value = json.loads(redis_value)
            # Populate memory cache
            memory_cache[key] = {
                "value": value,
                "expires": datetime.utcnow() + timedelta(seconds=self.memory_ttl)
            }
            return value

        return None

    async def set(self, key: str, value: Any):
        """Set in all cache levels"""
        # Memory cache
        memory_cache[key] = {
            "value": value,
            "expires": datetime.utcnow() + timedelta(seconds=self.memory_ttl)
        }

        # Redis cache
        await redis_client.setex(
            key,
            self.redis_ttl,
            json.dumps(value)
        )

    async def delete(self, key: str):
        """Delete from all cache levels"""
        memory_cache.pop(key, None)
        await redis_client.delete(key)

cache = MultiLevelCache()
```

## Response Caching

### HTTP Response Caching

```python
# Example 2: Response caching with FastAPI
from fastapi import FastAPI, Request, Response
from fastapi.responses import JSONResponse
from datetime import datetime
import hashlib
import json

app = FastAPI()

class ResponseCache:
    """Cache HTTP responses"""

    def __init__(self, default_ttl: int = 60):
        self.cache = {}
        self.default_ttl = default_ttl

    def _make_key(self, method: str, path: str, query: str) -> str:
        """Generate cache key from request"""
        key_str = f"{method}:{path}:{query}"
        return hashlib.md5(key_str.encode()).hexdigest()

    def get(self, request: Request) -> Optional[Response]:
        """Get cached response"""
        key = self._make_key(
            request.method,
            request.url.path,
            str(request.query_params)
        )

        if key in self.cache:
            entry = self.cache[key]
            if entry["expires"] > datetime.utcnow():
                return entry["response"]
            del self.cache[key]

        return None

    def set(self, request: Request, response: Response, ttl: int = None):
        """Cache response"""
        key = self._make_key(
            request.method,
            request.url.path,
            str(request.query_params)
        )

        self.cache[key] = {
            "response": response,
            "expires": datetime.utcnow() + timedelta(seconds=ttl or self.default_ttl)
        }

response_cache = ResponseCache()

def cache_response(ttl: int = 60):
    """Decorator to cache endpoint responses"""
    def decorator(func):
        async def wrapper(request: Request, *args, **kwargs):
            # Check cache
            cached = response_cache.get(request)
            if cached:
                return cached

            # Execute handler
            response = await func(request, *args, **kwargs)

            # Cache response
            response_cache.set(request, response, ttl)

            return response
        return wrapper
    return decorator

@app.get("/items/")
@cache_response(ttl=300)
async def list_items(request: Request):
    """Cached endpoint"""
    # Expensive database query
    items = await fetch_items_from_db()
    return {"items": items}
```

## Cache Invalidation

### Invalidation Strategies

```python
# Example 3: Cache invalidation patterns
from fastapi import FastAPI
import redis.asyncio as redis

app = FastAPI()
redis_client = redis.Redis(host="localhost", port=6379)

class CacheInvalidator:
    """Handle cache invalidation"""

    def __init__(self):
        self.redis = redis_client
        self.tag_keys: dict = {}  # tag -> set of keys

    async def set_with_tags(self, key: str, value: Any, tags: list[str]):
        """Set value with associated tags"""
        # Store value
        await self.redis.set(key, json.dumps(value))

        # Associate with tags
        for tag in tags:
            await self.redis.sadd(f"tag:{tag}", key)

    async def invalidate_by_tag(self, tag: str):
        """Invalidate all keys with tag"""
        keys = await self.redis.smembers(f"tag:{tag}")
        if keys:
            await self.redis.delete(*keys)
            await self.redis.delete(f"tag:{tag}")

    async def invalidate_pattern(self, pattern: str):
        """Invalidate keys matching pattern"""
        keys = []
        async for key in self.redis.scan_iter(match=pattern):
            keys.append(key)
        if keys:
            await self.redis.delete(*keys)

invalidator = CacheInvalidator()

# Cache-aside pattern with invalidation
@app.get("/users/{user_id}")
async def get_user(user_id: int):
    """Get user with cache-aside"""
    cache_key = f"user:{user_id}"

    # Try cache
    cached = await redis_client.get(cache_key)
    if cached:
        return json.loads(cached)

    # Fetch from DB
    user = await fetch_user_from_db(user_id)

    # Cache with TTL
    await redis_client.setex(cache_key, 300, json.dumps(user))

    return user

@app.put("/users/{user_id}")
async def update_user(user_id: int, data: UserUpdate):
    """Update user and invalidate cache"""
    # Update in DB
    updated = await update_user_in_db(user_id, data)

    # Invalidate cache
    await redis_client.delete(f"user:{user_id}")

    return updated
```

## Cache-Aside Pattern

### Read/Write Through

```python
# Example 4: Cache patterns implementation
from typing import Optional, Callable
import asyncio

class CacheAside:
    """Cache-aside pattern implementation"""

    def __init__(self, cache, db_fetcher: Callable, ttl: int = 300):
        self.cache = cache
        self.db_fetcher = db_fetcher
        self.ttl = ttl

    async def get(self, key: str) -> Optional[Any]:
        """Get with cache-aside"""
        # Try cache
        cached = await self.cache.get(key)
        if cached:
            return cached

        # Fetch from DB
        value = await self.db_fetcher(key)
        if value:
            await self.cache.set(key, value, self.ttl)

        return value

    async def set(self, key: str, value: Any):
        """Set in both cache and DB"""
        # Update DB
        await self.db_updater(key, value)
        # Update cache
        await self.cache.set(key, value, self.ttl)

    async def delete(self, key: str):
        """Delete from both"""
        await self.db_deleter(key)
        await self.cache.delete(key)


class WriteThrough:
    """Write-through cache pattern"""

    def __init__(self, cache, db):
        self.cache = cache
        self.db = db

    async def write(self, key: str, value: Any):
        """Write to both cache and DB synchronously"""
        # Write to DB first
        await self.db.save(key, value)
        # Then update cache
        await self.cache.set(key, value)
        return value


class WriteBehind:
    """Write-behind (write-back) cache pattern"""

    def __init__(self, cache, db):
        self.cache = cache
        self.db = db
        self.pending_writes = []
        self.batch_size = 100

    async def write(self, key: str, value: Any):
        """Write to cache immediately, DB asynchronously"""
        # Update cache immediately
        await self.cache.set(key, value)

        # Queue DB write
        self.pending_writes.append((key, value))

        # Batch write if threshold reached
        if len(self.pending_writes) >= self.batch_size:
            await self._flush()

    async def _flush(self):
        """Flush pending writes to DB"""
        if self.pending_writes:
            await self.db.batch_save(self.pending_writes)
            self.pending_writes.clear()
```

## Redis Caching

### Redis Implementation

```python
# Example 5: Redis cache implementation
from fastapi import FastAPI
import redis.asyncio as redis
import json
from typing import Any, Optional
from datetime import timedelta

app = FastAPI()

class RedisCache:
    """Redis cache wrapper"""

    def __init__(self, url: str = "redis://localhost:6379"):
        self.redis = redis.from_url(url)

    async def get(self, key: str) -> Optional[Any]:
        value = await self.redis.get(key)
        return json.loads(value) if value else None

    async def set(
        self,
        key: str,
        value: Any,
        expire: int = 300
    ):
        await self.redis.setex(key, expire, json.dumps(value))

    async def delete(self, key: str):
        await self.redis.delete(key)

    async def exists(self, key: str) -> bool:
        return await self.redis.exists(key) > 0

    async def increment(self, key: str) -> int:
        return await self.redis.incr(key)

    async def get_or_set(
        self,
        key: str,
        factory: Callable,
        expire: int = 300
    ):
        """Get from cache or compute and set"""
        value = await self.get(key)
        if value is None:
            value = await factory()
            await self.set(key, value, expire)
        return value

    async def invalidate_pattern(self, pattern: str):
        """Invalidate all keys matching pattern"""
        async for key in self.redis.scan_iter(match=pattern):
            await self.redis.delete(key)

cache = RedisCache()

# Cache decorator
def cached(expire: int = 300, key_prefix: str = ""):
    """Cache decorator for FastAPI endpoints"""
    def decorator(func):
        async def wrapper(*args, **kwargs):
            # Generate cache key
            cache_key = f"{key_prefix}:{func.__name__}:{hash(str(kwargs))}"

            # Try cache
            result = await cache.get(cache_key)
            if result is not None:
                return result

            # Execute function
            result = await func(*args, **kwargs)

            # Cache result
            await cache.set(cache_key, result, expire)

            return result
        return wrapper
    return decorator

@app.get("/items/{item_id}")
@cached(expire=300, key_prefix="items")
async def get_item(item_id: int):
    """Cached endpoint"""
    return await fetch_item_from_db(item_id)
```

## Best Practices

### Caching Guidelines

```python
# Example 6: Caching best practices
"""
Caching Best Practices:

1. Cache Key Design
   - Include all relevant parameters
   - Use consistent naming
   - Avoid overly long keys

2. TTL Strategy
   - Short TTL for frequently changing data
   - Long TTL for static data
   - Consider data volatility

3. Cache Invalidation
   - Invalidate on writes
   - Use event-driven invalidation
   - Consider eventual consistency

4. Memory Management
   - Set size limits
   - Use eviction policies (LRU)
   - Monitor memory usage

5. Error Handling
   - Cache failures shouldn't break app
   - Fallback to database
   - Log cache errors
"""

# Smart cache key generation
def make_cache_key(*args, **kwargs) -> str:
    """Generate consistent cache key"""
    parts = [str(arg) for arg in args]
    parts.extend(f"{k}:{v}" for k, v in sorted(kwargs.items()))
    return hashlib.md5(":".join(parts).encode()).hexdigest()
```

## Summary

| Strategy | Use Case | Trade-off |
|----------|----------|-----------|
| Cache-aside | Read-heavy | Cache miss penalty |
| Write-through | Consistency | Write latency |
| Write-behind | Write-heavy | Complexity |
| Read-through | Simple reads | Miss penalty |

## Next Steps

Continue learning about:
- [Database Optimization](./04_database_optimization.md) - Query optimization
- [Response Caching](./03_response_caching.md) - HTTP caching
- [Connection Pooling](./07_connection_pooling.md) - Pool management
