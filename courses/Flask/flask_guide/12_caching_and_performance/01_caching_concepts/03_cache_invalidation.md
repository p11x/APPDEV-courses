<!-- FILE: 12_caching_and_performance/01_caching_concepts/03_cache_invalidation.md -->

## Overview

Cache invalidation—removing or updating stale data in your cache—is one of the hardest problems in computer science. This file explains why it's difficult, the different invalidation strategies, and practical patterns for keeping your Flask cache in sync with your database.

## Prerequisites

- Understanding of caching concepts
- Familiarity with Cache-Aside pattern

## Core Concepts

### Why Cache Invalidation is Hard

The fundamental problem: **caches are copies of data, not the authoritative source**. When the source (database) changes, the copy becomes stale. You need to decide:

1. **When** to invalidate (immediate vs delayed)
2. **What** to invalidate (single key vs patterns)
3. **How** to propagate changes (synchronously vs asynchronously)

There's no perfect solution—each approach has trade-offs between complexity, performance, and data freshness.

### Invalidation Strategies

| Strategy | Description | Pros | Cons |
|----------|-------------|------|------|
| **TTL-based** | Let cache expire naturally | Simple | Stale data until TTL |
| **Write-through** | Update cache on writes | Always fresh | Extra write latency |
| **Invalidate on write** | Delete cache entry when data changes | Simple | Cache miss on next read |
| **Event-based** | Listen to DB changes | Near real-time | Complex setup |
| **Versioned keys** | Include version in keys | Clean updates | More memory |

## Code Walkthrough

### TTL-Based Invalidation

The simplest approach—just set a reasonable TTL and accept some staleness:

```python
# ttl_invalidation.py
import redis
import json
import time

redis_client = redis.Redis(host="localhost", port=6379, db=0)

# Different TTLs for different data types
TTL_USER = 600        # 10 minutes - user profiles change occasionally
TTL_PRODUCT = 1800    # 30 minutes - products change less often
TTL_CONFIG = 86400    # 24 hours - config rarely changes
TTL_NEWS = 60         # 1 minute - news changes constantly

def get_user(user_id):
    """Cache user with TTL"""
    cache_key = f"user:{user_id}"
    
    # Try cache
    cached = redis_client.get(cache_key)
    if cached:
        return json.loads(cached)
    
    # Fetch from DB
    user = fetch_user_from_db(user_id)
    
    # Store in cache with TTL
    redis_client.setex(cache_key, TTL_USER, json.dumps(user))
    
    return user

# No explicit invalidation needed!
# Cache naturally expires after TTL
```

### Write-Through Invalidation

Update cache at the same time as database:

```python
# write_through.py
import redis
import json

redis_client = redis.Redis(host="localhost", port=6379, db=0)

def get_user(user_id):
    """Read from cache"""
    cache_key = f"user:{user_id}"
    cached = redis_client.get(cache_key)
    if cached:
        return json.loads(cached)
    return None

def create_user(user_data):
    """Write to both DB and cache"""
    # 1. Write to database
    user = User(**user_data)
    db.session.add(user)
    db.session.commit()
    
    # 2. Write to cache immediately
    cache_key = f"user:{user.id}"
    redis_client.setex(
        cache_key,
        600,  # TTL
        json.dumps(user.to_dict())
    )
    
    return user

def update_user(user_id, user_data):
    """Update both DB and cache"""
    # 1. Update database
    user = db.session.query(User).get(user_id)
    for key, value in user_data.items():
        setattr(user, key, value)
    db.session.commit()
    
    # 2. Update cache
    cache_key = f"user:{user_id}"
    redis_client.setex(
        cache_key,
        600,
        json.dumps(user.to_dict())
    )
    
    return user

def delete_user(user_id):
    """Delete from both DB and cache"""
    # 1. Delete from database
    user = db.session.query(User).get(user_id)
    db.session.delete(user)
    db.session.commit()
    
    # 2. Delete from cache
    cache_key = f"user:{user_id}"
    redis_client.delete(cache_key)
```

### Invalidate on Write

Delete cache entry when data changes:

```python
# invalidate_on_write.py
import redis

redis_client = redis.Redis(host="localhost", port=6379, db=0)

def get_user(user_id):
    """Read from cache (cache-aside)"""
    cache_key = f"user:{user_id}"
    cached = redis_client.get(cache_key)
    if cached:
        return json.loads(cached)
    
    # Cache miss - fetch from DB
    user = db.session.query(User).get(user_id)
    if user:
        redis_client.setex(cache_key, 600, json.dumps(user.to_dict()))
    return user

def update_user(user_id, user_data):
    """Update database, invalidate cache"""
    # 1. Update database
    user = db.session.query(User).get(user_id)
    for key, value in user_data.items():
        setattr(user, key, value)
    db.session.commit()
    
    # 2. Invalidate cache - DELETE it!
    cache_key = f"user:{user_id}"
    redis_client.delete(cache_key)
    
    # Next read will fetch fresh data from DB
    
    return user

def delete_user(user_id):
    """Delete from DB, invalidate cache"""
    user = db.session.query(User).get(user_id)
    db.session.delete(user)
    db.session.commit()
    
    cache_key = f"user:{user_id}"
    redis_client.delete(cache_key)
```

### Versioned Cache Keys

Use version numbers in keys to make invalidation clean:

```python
# versioned_keys.py
import redis

redis_client = redis.Redis(host="localhost", port=6379, db=0)

# Store current version in Redis
def get_version(key_type):
    """Get current version for a key type"""
    version_key = f"version:{key_type}"
    return int(redis_client.get(version_key) or 1)

def increment_version(key_type):
    """Increment version - automatically invalidates all keys"""
    version_key = f"version:{key_type}"
    redis_client.incr(version_key)

def get_user(user_id):
    """Read with versioned key"""
    version = get_version("user")
    cache_key = f"user:v{version}:{user_id}"
    
    cached = redis_client.get(cache_key)
    if cached:
        return json.loads(cached)
    
    user = db.session.query(User).get(user_id)
    if user:
        redis_client.setex(cache_key, 86400, json.dumps(user.to_dict()))
    return user

def update_user(user_id, user_data):
    """Update user and bump version"""
    user = db.session.query(User).get(user_id)
    for key, value in user_data.items():
        setattr(user, key, value)
    db.session.commit()
    
    # Bump version - ALL user cache entries invalidated!
    increment_version("user")
    
    return user
```

### Tag-Based Invalidation

Group related cache entries with tags:

```python
# tagged_invalidation.py
import redis

redis_client = redis.Redis(host="localhost", port=6379, db=0)

def add_to_cache(key, value, ttl=600, tags=None):
    """Add to cache with tags"""
    # Store value
    redis_client.setex(key, ttl, json.dumps(value))
    
    # Add tags
    if tags:
        for tag in tags:
            tag_key = f"tag:{tag}"
            redis_client.sadd(tag_key, key)

def invalidate_tag(tag):
    """Invalidate all cache entries with a tag"""
    tag_key = f"tag:{tag}"
    
    # Get all keys with this tag
    keys = redis_client.smembers(tag_key)
    
    # Delete all keys
    if keys:
        redis_client.delete(*keys)
    
    # Delete the tag set itself
    redis_client.delete(tag_key)

# Example: User has posts, invalidate both when user updates
def get_user_with_posts(user_id):
    """Get user and their posts (cached together)"""
    cache_key = f"user:{user_id}:with_posts"
    cached = redis_client.get(cache_key)
    if cached:
        return json.loads(cached)
    
    user = db.session.query(User).get(user_id)
    posts = db.session.query(Post).filter_by(user_id=user_id).all()
    
    result = {
        "user": user.to_dict(),
        "posts": [p.to_dict() for p in posts]
    }
    
    add_to_cache(cache_key, result, ttl=300, tags=[f"user:{user_id}", "users"])
    return result

def update_user(user_id, user_data):
    """Update user and invalidate related caches"""
    user = db.session.query(User).get(user_id)
    for key, value in user_data.items():
        setattr(user, key, value)
    db.session.commit()
    
    # Invalidate user's caches via tags
    invalidate_tag(f"user:{user_id}")
    invalidate_tag("users")
```

### Event-Based Invalidation (Advanced)

Listen to database changes via triggers:

```python
# event_based.py
# Note: This requires PostgreSQL LISTEN/NOTIFY or similar

from flask import Flask
import redis
import sqlalchemy

app = Flask(__name__)

# Setup Redis for pub/sub
redis_client = redis.Redis(host="localhost", port=6379, db=0)
pubsub = redis_client.pubsub()

# Subscribe to invalidation events
pubsub.subscribe("cache_invalidation")

def handle_invalidation(message):
    """Handle cache invalidation message"""
    if message["type"] == "message":
        data = message["data"].decode()
        action, key = data.split(":", 1)
        
        if action == "delete":
            redis_client.delete(key)
        elif action == "invalidate_tag":
            # Handle tag invalidation
            pass

# Start listener in background
import threading
thread = threading.Thread(target=lambda: [handle_invalidation(m) for m in pubsub.listen()])
thread.daemon = True
thread.start()

# In Flask route, publish invalidation event
def update_user(user_id, user_data):
    user = db.session.query(User).get(user_id)
    # ... update user
    
    # Publish invalidation event
    redis_client.publish("cache_invalidation", f"delete:user:{user_id}")
```

## Flask Integration Example

```python
# app.py - Complete Flask + Redis caching with invalidation
from flask import Flask, jsonify, request
import redis
import json
import time
from functools import wraps

app = Flask(__name__)

# Redis setup
redis_client = redis.Redis(host="localhost", port=6379, db=0)

# Cache decorator
def cached(ttl=300, key_prefix="default"):
    """Cache decorator with invalidation support"""
    def decorator(f):
        @wraps(f)
        def wrapper(*args, **kwargs):
            # Build cache key
            cache_key = f"{key_prefix}:{f.__name__}:{':'.join(map(str, args))}"
            
            # Try cache
            cached_data = redis_client.get(cache_key)
            if cached_data:
                return json.loads(cached_data)
            
            # Execute function
            result = f(*args, **kwargs)
            
            # Cache result
            redis_client.setex(cache_key, ttl, json.dumps(result))
            
            return result
        return wrapper
    return decorator

# Manual invalidation decorator
def invalidate_cache(key_prefix, *args):
    """Invalidate cache for a specific key"""
    cache_key = f"{key_prefix}:*"
    keys = redis_client.keys(cache_key)
    if keys:
        redis_client.delete(*keys)

# ============================================
# Models
# ============================================
class User:
    def __init__(self, id, name, email):
        self.id = id
        self.name = name
        self.email = email
    
    def to_dict(self):
        return {"id": self.id, "name": self.name, "email": self.email}

# Fake database
users_db = {1: User(1, "Alice", "alice@example.com")}

# ============================================
# Routes
# ============================================

@app.route("/users/<int:user_id>")
@cached(ttl=300, key_prefix="user")
def get_user(user_id):
    """Get user - cached for 5 minutes"""
    user = users_db.get(user_id)
    if user:
        return user.to_dict()
    return None

@app.route("/users/<int:user_id>", methods=["PUT"])
def update_user(user_id):
    """Update user - invalidates cache"""
    data = request.get_json()
    user = users_db.get(user_id)
    
    if user:
        user.name = data.get("name", user.name)
        user.email = data.get("email", user.email)
        
        # Invalidate cache!
        cache_key = f"user:get_user:{user_id}"
        redis_client.delete(cache_key)
        
        return jsonify(user.to_dict())
    
    return jsonify({"error": "Not found"}), 404

@app.route("/users", methods=["POST"])
def create_user():
    """Create user"""
    data = request.get_json()
    new_id = max(users_db.keys()) + 1
    user = User(new_id, data["name"], data["email"])
    users_db[new_id] = user
    return jsonify(user.to_dict()), 201

@app.route("/cache/clear", methods=["POST"])
def clear_all_cache():
    """Clear all cache (admin only)"""
    redis_client.flushdb()
    return jsonify({"status": "cleared"})

if __name__ == "__main__":
    app.run(port=5000)
```

## Common Mistakes

### ❌ Forgetting to invalidate on updates

```python
# PROBLEM: Stale data in cache
@app.route("/update-user", methods=["POST"])
def update_user():
    # Update database...
    db.commit()
    # But forgot to invalidate cache!
    return jsonify({"status": "updated"})
```

### ✅ Always invalidate

```python
# CORRECT: Invalidate after updates
@app.route("/update-user", methods=["POST"])
def update_user():
    # Update database...
    db.commit()
    
    # Invalidate cache!
    redis_client.delete(f"user:{user_id}")
    
    return jsonify({"status": "updated"})
```

### ❌ Invalidating too much

```python
# PROBLEM: Clears entire cache on single user update
def update_user(user_id):
    db.commit()
    redis_client.flushdb()  # Clears ALL cache!
```

### ✅ Invalidate precisely

```python
# CORRECT: Only invalidate affected entries
def update_user(user_id):
    db.commit()
    redis_client.delete(f"user:{user_id}")  # Only this user
```

## Quick Reference

| Strategy | When to Use |
|----------|-------------|
| TTL only | Data changes infrequently, some staleness OK |
| Write-through | Data must always be fresh, write latency OK |
| Invalidate on write | Simple, occasional stale data OK |
| Versioned keys | Need atomic updates, multiple related keys |
| Event-based | Real-time requirements, complex invalidation |

**Best practices:**
1. Always invalidate on writes (create, update, delete)
2. Use appropriate TTL for data freshness requirements
3. Monitor cache hit rates
4. Have a fallback when cache is unavailable

## Next Steps

Continue to [02_flask_caching/01_installing_flask_caching.md](../02_flask_caching/01_installing_flask_caching.md) to learn about Flask-Caching extension.