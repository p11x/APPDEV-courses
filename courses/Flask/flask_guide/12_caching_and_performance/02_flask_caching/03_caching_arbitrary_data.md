<!-- FILE: 12_caching_and_performance/02_flask_caching/03_caching_arbitrary_data.md -->

## Overview

Beyond view caching, Flask-Caching can cache arbitrary data like function results, database queries, or API responses. This file shows you how to use the cache directly in your code.

## Code Walkthrough

```python
# arbitrary_caching.py
from flask import Flask
from flask_caching import Cache

app = Flask(__name__)
app.config["CACHE_TYPE"] = "SimpleMemoryCache"
cache = Cache(app)

# ============================================
# Cache arbitrary data
# ============================================

@cache.cached(timeout=60, key_prefix="user_count")
def get_user_count():
    """Cache function result"""
    return {"count": 100}

# Manual cache operations
def get_user(user_id):
    """Manually cache database result"""
    key = f"user:{user_id}"
    
    # Try cache first
    user = cache.get(key)
    if user is None:
        # Fetch from database
        user = fetch_from_db(user_id)
        # Store in cache
        cache.set(key, user, timeout=300)
    
    return user

def fetch_from_db(user_id):
    return {"id": user_id, "name": f"User {user_id}"}

# ============================================
# Cache decorators on functions
# ============================================

import hashlib

def make_cache_key(*args, **kwargs):
    """Custom cache key based on arguments"""
    key = str(args) + str(sorted(kwargs.items()))
    return hashlib.md5(key.encode()).hexdigest()

@cache.cached(key_prefix="expensive_calc", timeout=300)
def expensive_calculation(a, b):
    """Cache expensive computation"""
    return a * b + a ** b

if __name__ == "__main__":
    app.run(port=5000)
```

## Next Steps

Continue to [03_redis_caching/01_redis_overview.md](../03_redis_caching/01_redis_overview.md)
