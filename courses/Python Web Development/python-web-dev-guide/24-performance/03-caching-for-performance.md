# Caching for Performance

## What You'll Learn
- Application caching
- HTTP caching
- Redis caching

## Prerequisites
- Completed database optimization

## Application-Level Cache

```python
from functools import lru_cache

@lru_cache(maxsize=100)
def get_config(key: str):
    """Cache configuration with LRU"""
    return fetch_from_db(key)

# Clear cache
get_config.cache_clear()
```

## HTTP Caching

```python
from fastapi import Response
from fastapi.responses import JSONResponse

@app.get("/config")
async def get_config():
    data = fetch_config()
    
    response = JSONResponse(content=data)
    response.headers["Cache-Control"] = "public, max-age=3600"
    response.headers["ETag"] = generate_etag(data)
    
    return response
```

## Redis Cache

```python
import redis
import json

r = redis.Redis()

def cache_get(key: str):
    data = r.get(key)
    return json.loads(data) if data else None

def cache_set(key: str, value: dict, ttl: int = 3600):
    r.setex(key, ttl, json.dumps(value))

@app.get("/products")
async def get_products():
    cache_key = "products:all"
    
    data = cache_get(cache_key)
    if not data:
        data = fetch_products()
        cache_set(cache_key, data)
    
    return data
```

## Summary
- Use LRU cache for config
- Implement HTTP caching
- Use Redis for distributed caching

## Next Steps
→ Continue to `04-async-optimization.md`
