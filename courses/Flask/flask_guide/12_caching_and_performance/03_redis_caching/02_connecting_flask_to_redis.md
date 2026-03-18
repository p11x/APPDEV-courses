<!-- FILE: 12_caching_and_performance/03_redis_caching/02_connecting_flask_to_redis.md -->

## Overview

This file shows how to connect Flask to Redis using different approaches.

## Code Walkthrough

```python
# flask_redis.py
from flask import Flask
from flask_caching import Cache
import redis
import os

app = Flask(__name__)

# ============================================
# Method 1: Flask-Caching with Redis
# ============================================
app.config["CACHE_TYPE"] = "RedisCache"
app.config["CACHE_REDIS_HOST"] = os.environ.get("REDIS_HOST", "localhost")
app.config["CACHE_REDIS_PORT"] = 6379
app.config["CACHE_REDIS_DB"] = 0

cache = Cache(app)

# ============================================
# Method 2: Direct Redis connection
# ============================================
redis_client = redis.Redis(
    host="localhost",
    port=6379,
    db=0,
    decode_responses=True
)

# ============================================
# Method 3: Redis with connection pooling
# ============================================
pool = redis.ConnectionPool(
    host="localhost",
    port=6379,
    db=0,
    max_connections=10
)
redis_from_pool = redis.Redis(connection_pool=pool)

if __name__ == "__main__":
    app.run(port=5000)
```

## Next Steps

Continue to [03_redis_cache_patterns.md](03_redis_cache_patterns.md)
