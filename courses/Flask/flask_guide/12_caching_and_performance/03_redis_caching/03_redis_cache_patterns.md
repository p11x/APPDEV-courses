<!-- FILE: 12_caching_and_performance/03_redis_caching/03_redis_cache_patterns.md -->

## Overview

This file covers common Redis caching patterns for Flask applications.

## Code Walkthrough

```python
# redis_patterns.py
import redis
import json
import hashlib
from functools import wraps

redis_client = redis.Redis(host="localhost", port=6379, db=0)

# ============================================
# Pattern 1: Cache-Aside
# ============================================

def cache_aside(key_prefix, ttl=300):
    """Decorator for cache-aside pattern"""
    def decorator(f):
        @wraps(f)
        def wrapper(*args, **kwargs):
            # Build cache key
            key_parts = [key_prefix, f.__name__]
            key_parts.extend(str(a) for a in args)
            key = ":".join(key_parts)
            
            # Try cache
            cached = redis_client.get(key)
            if cached:
                return json.loads(cached)
            
            # Fetch from source
            result = f(*args, **kwargs)
            
            # Store in cache
            redis_client.setex(key, ttl, json.dumps(result))
            return result
        return wrapper
    return decorator

@cache_aside("user", ttl=600)
def get_user(user_id):
    """Get user from database"""
    return {"id": user_id, "name": f"User {user_id}"}

# ============================================
# Pattern 2: Cache with invalidation
# ============================================

def invalidate_cache(key_pattern):
    """Invalidate cache matching pattern"""
    keys = redis_client.keys(key_pattern)
    if keys:
        redis_client.delete(*keys)

def update_user(user_id, data):
    """Update user and invalidate cache"""
    # Update database...
    
    # Invalidate cache
    invalidate_cache(f"user:*{user_id}*")

# ============================================
# Pattern 3: Distributed locking
# ============================================

def acquire_lock(lock_name, timeout=10):
    """Acquire distributed lock"""
    return redis_client.set(lock_name, "1", nx=True, ex=timeout)

def release_lock(lock_name):
    """Release distributed lock"""
    redis_client.delete(lock_name)

def locked_operation(lock_name, operation, *args):
    """Execute operation with lock"""
    if acquire_lock(lock_name):
        try:
            return operation(*args)
        finally:
            release_lock(lock_name)
    return None

# ============================================
# Pattern 4: Rate limiting with Redis
# ============================================

def rate_limit(key, limit=100, window=60):
    """Rate limiting using sliding window"""
    now = redis_client.time()[0]
    redis_client.zremrangebyscore(key, 0, now - window)
    
    count = redis_client.zcard(key)
    if count >= limit:
        return False
    
    redis_client.zadd(key, {str(now): now})
    redis_client.expire(key, window)
    return True

if __name__ == "__main__":
    pass
```

## Next Steps

Continue to [04_database_performance/01_n_plus_one_problem.md](../04_database_performance/01_n_plus_one_problem.md)
