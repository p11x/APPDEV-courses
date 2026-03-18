# Caching with FastAPI

## What You'll Learn
- Implementing caching in FastAPI applications
- Using python-cache with FastAPI
- HTTP cache headers
- Async Redis caching

## Prerequisites
- Completed FastAPI folder and caching concepts

## Installing Dependencies

```bash
pip install fastapi-cache2 aiocache
```

## Basic In-Memory Cache

```python
from fastapi import FastAPI
from fastapi_cache import FastAPICache
from fastapi_cache.backends.inmemory import InMemoryBackend

app = FastAPI()

@app.on_event("startup")
async def startup():
    FastAPICache.init(InMemoryBackend())
```

## Using Cache with Routes

```python
from fastapi import FastAPI
from fastapi_cache import FastAPICache
from fastapi_cache.backends.inmemory import InMemoryBackend
from fastapi_cache.decorator import cache

app = FastAPI()

@app.on_event("startup")
async def startup():
    FastAPICache.init(InMemoryBackend())

@app.get("/users/{user_id}")
@cache(expire=300)  # Cache for 5 minutes
async def get_user(user_id: int) -> dict:
    # Simulate database call
    await asyncio.sleep(0.1)
    return {"id": user_id, "name": f"User {user_id}"}

import asyncio
```

🔍 **Line-by-Line Breakdown:**
1. `@cache(expire=300)` — Decorator caches the function response for 300 seconds
2. `async def` — FastAPI cache works with both sync and async functions
3. The cache key is automatically generated from the function name and parameters

## Using Redis Backend

```python
from fastapi import FastAPI
from fastapi_cache import FastAPICache
from fastapi_cache.backends.redis import RedisBackend
from redis import asyncio as aioredis

app = FastAPI()

@app.on_event("startup")
async def startup():
    redis = aioredis.from_url("redis://localhost", encoding="utf8")
    FastAPICache.init(RedisBackend(redis), prefix="fastapi-cache")

@app.get("/products/{product_id}")
@cache(expire=3600)
async def get_product(product_id: int) -> dict:
    return {"id": product_id, "name": f"Product {product_id}"}
```

## Manual Cache Operations

```python
from fastapi import APIRouter
from fastapi_cache import FastAPICache

router = APIRouter()

@router.post("/products/invalidate/{product_id}")
async def invalidate_product(product_id: int) -> dict:
    await FastAPICache.clear(namespace="products")
    return {"message": f"Product {product_id} cache invalidated"}

@router.get("/search")
async def search_products(q: str) -> dict:
    cache_key = f"search:{q}"
    result = await FastAPICache.get(cache_key)
    
    if result:
        return {"data": result, "cached": True}
    
    results = await perform_search(q)
    await FastAPICache.set(cache_key, results, expire=600)
    return {"data": results, "cached": False}

async def perform_search(q: str) -> list:
    return [{"id": 1, "name": f"Result for {q}"}]
```

## HTTP Cache Headers

```python
from fastapi import APIRouter
from fastapi.responses import JSONResponse
from fastapi_cache import CACHE

router = APIRouter()

@router.get("/static-data")
async def get_static_data() -> JSONResponse:
    data = {"version": "1.0", "features": ["a", "b", "c"]}
    response = JSONResponse(content=data)
    response.headers["Cache-Control"] = "public, max-age=3600"
    response.headers["ETag"] = "v1"
    return response
```

## Summary
- FastAPI-Cache provides decorator-based caching
- Use Redis backend for production applications
- Implement proper cache invalidation strategies
- Consider HTTP cache headers for static content

## Next Steps
→ Continue to `05-cache-invalidation-strategies.md`
