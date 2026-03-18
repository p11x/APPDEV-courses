# Cache Invalidation Strategies

## What You'll Learn
- Understanding cache invalidation patterns
- Implementing TTL-based expiration
- Event-driven cache invalidation
- Handling cache stampedes

## Prerequisites
- Completed caching with Flask and FastAPI

## Why Invalidation Matters

Cache invalidation is one of the hardest problems in computer science. Stale data can cause:
- Users seeing outdated information
- Security vulnerabilities
- Business logic errors

## Time-Based Invalidation (TTL)

```python
from fastapi import FastAPI
from fastapi_cache import FastAPICache
from fastapi_cache.backends.inmemory import InMemoryBackend

app = FastAPI()

@app.on_event("startup")
async def startup():
    FastAPICache.init(InMemoryBackend())

@app.get("/products/{product_id}")
@cache(expire=300)  # Auto-expire after 5 minutes
async def get_product(product_id: int) -> dict:
    return {"id": product_id, "name": f"Product {product_id}"}
```

🔍 **Line-by-Line Breakdown:**
1. `expire=300` — Time-To-Live in seconds
2. After 5 minutes, cache automatically expires
3. Next request fetches fresh data

## Manual Invalidation

```python
from fastapi import APIRouter, FastAPI
from fastapi_cache import FastAPICache
from fastapi_cache.backends.inmemory import InMemoryBackend

router = APIRouter()

@router.post("/product/{product_id}/invalidate")
async def invalidate_product(product_id: int) -> dict:
    # Clear all cached data
    await FastAPICache.clear()
    return {"message": "Cache cleared"}

@router.post("/products/invalidate-all")
async def invalidate_all_products() -> dict:
    # Clear with namespace
    await FastAPICache.clear(namespace="products")
    return {"message": "Products cache cleared"}
```

## Write-Through Strategy

```python
from fastapi import FastAPI
from fastapi_cache import FastAPICache
import json

app = FastAPI()

# Product cache storage (simulated)
product_cache: dict = {}

async def get_product_cached(product_id: int) -> dict:
    """Get product with cache-aside pattern"""
    cache_key = f"product:{product_id}"
    
    # Check cache first
    cached = await FastAPICache.get(cache_key)
    if cached:
        return cached
    
    # Fetch from database
    product = await fetch_product_from_db(product_id)
    
    # Update cache
    await FastAPICache.set(cache_key, product, expire=3600)
    return product

async def update_product(product_id: int, data: dict) -> dict:
    """Update product and invalidate cache"""
    # Write to database first
    product = await update_product_in_db(product_id, data)
    
    # Invalidate cache immediately
    cache_key = f"product:{product_id}"
    await FastAPICache.clear(namespace=cache_key)
    
    # Optionally set new cache
    await FastAPICache.set(cache_key, product, expire=3600)
    
    return product

async def fetch_product_from_db(product_id: int) -> dict:
    return {"id": product_id, "name": f"Product {product_id}"}

async def update_product_in_db(product_id: int, data: dict) -> dict:
    return {"id": product_id, **data}
```

## Cache Stampede Prevention

```python
import asyncio
import hashlib
from fastapi import FastAPI
from fastapi_cache import FastAPICache

app = FastAPI()

async def get_with_lock(key: str, fetch_func, expire: int = 300) -> dict:
    """
    Prevent cache stampede using locking
    """
    # Try to get from cache
    cached = await FastAPICache.get(key)
    if cached:
        return cached
    
    # Create a lock key
    lock_key = f"lock:{hashlib.md5(key.encode()).hexdigest()}"
    
    # Try to acquire lock (simplified - use Redis SETNX in production)
    lock_acquired = await FastAPICache.get(lock_key)
    
    if lock_acquired:
        # Wait and retry
        await asyncio.sleep(0.1)
        return await get_with_lock(key, fetch_func, expire)
    
    # Fetch data
    data = await fetch_func()
    
    # Set cache and release lock
    await FastAPICache.set(key, data, expire=expire)
    await FastAPICache.set(lock_key, "1", expire=10)
    
    return data

async def fetch_user_data(user_id: int) -> dict:
    await asyncio.sleep(0.5)  # Simulate slow DB
    return {"id": user_id, "name": f"User {user_id}"}

@app.get("/users/{user_id}")
async def get_user(user_id: int) -> dict:
    cache_key = f"user:{user_id}"
    return await get_with_lock(cache_key, lambda: fetch_user_data(user_id))
```

## Summary
- Use TTL for automatic expiration
- Invalidate cache on data updates
- Implement write-through for critical data
- Prevent cache stampedes with locking

## Next Steps
→ Move to `16-task-queues/` folder
