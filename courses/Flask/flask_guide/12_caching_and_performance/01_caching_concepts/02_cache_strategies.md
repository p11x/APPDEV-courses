<!-- FILE: 12_caching_and_performance/01_caching_concepts/02_cache_strategies.md -->

## Overview

Different caching strategies suit different use cases. This file explains the most common caching strategies—Cache-Aside, Write-Through, Write-Behind, and Refresh-Ahead—and helps you choose the right one for your Flask application.

## Prerequisites

- Understanding of caching basics
- Familiarity with Flask

## Core Concepts

### Cache-Aside (Lazy Loading)

The most common strategy. The application first checks the cache, and only queries the database if there's a cache miss.

**Flow:**
1. Application checks cache
2. If cache miss → fetch from database
3. Store result in cache
4. Return data

**Pros:**
- Simple to implement
- Cache only contains data that's actually requested
- Resilient to cache failures (falls back to DB)

**Cons:**
- First request is slow (cache miss)
- Stale data possible (until TTL expires)

```python
# Cache-Aside implementation
def get_user(user_id):
    cache_key = f"user:{user_id}"
    
    # 1. Check cache
    user = cache.get(cache_key)
    if user:
        return user  # Cache hit!
    
    # 2. Cache miss - fetch from database
    user = db.query(User).get(user_id)
    
    # 3. Store in cache for next time
    if user:
        cache.set(cache_key, user, ttl=600)
    
    return user
```

### Write-Through

Data is written to both cache and database simultaneously. The cache is always consistent with the database.

**Flow:**
1. Application writes to cache
2. Cache writes to database
3. Return success

**Pros:**
- Data always fresh in cache
- No cache misses on reads after writes
- Simple read pattern

**Cons:**
- Write latency is higher (two writes)
- If cache fails, data might be inconsistent

```python
# Write-Through implementation
def create_user(user_data):
    # 1. Write to database
    user = User(**user_data)
    db.add(user)
    db.commit()
    
    # 2. Write to cache (immediately available)
    cache_key = f"user:{user.id}"
    cache.set(cache_key, user.to_dict(), ttl=600)
    
    return user

def update_user(user_id, user_data):
    # 1. Update database
    user = db.query(User).get(user_id)
    for key, value in user_data.items():
        setattr(user, key, value)
    db.commit()
    
    # 2. Update cache
    cache_key = f"user:{user_id}"
    cache.set(cache_key, user.to_dict(), ttl=600)
    
    return user
```

### Write-Behind (Write-Back)

Data is written to cache first, then asynchronously written to database.

**Flow:**
1. Application writes to cache
2. Return success immediately
3. Background process writes to database

**Pros:**
- Fastest writes (no waiting for DB)
- Better write throughput

**Cons:**
- Risk of data loss if cache fails before DB write
- More complex implementation

```python
# Write-Behind implementation (simplified)
from queue import Queue
import threading

write_queue = Queue()

def write_to_db_worker():
    """Background worker that writes to database"""
    while True:
        model, data = write_queue.get()
        # Write to database
        db.session.add(model(**data))
        db.session.commit()
        write_queue.task_done()

# Start background writer
threading.Thread(target=write_to_db_worker, daemon=True).start()

def create_user(user_data):
    # 1. Write to cache (fast!)
    user_id = generate_id()
    cache_key = f"user:{user_id}"
    cache.set(cache_key, user_data, ttl=600)
    
    # 2. Queue for database write (async)
    write_queue.put((User, user_data))
    
    return {"id": user_id, **user_data}
```

### Refresh-Ahead (Proactive Caching)

The cache automatically refreshes entries before they expire, reducing the chance of cache misses.

**Flow:**
1. Cache tracks when entries were last accessed
2. When TTL is approaching, refresh in background
3. Next request always gets fresh data

**Pros:**
- Reduces cache misses significantly
- Better user experience (no slow requests)

**Cons:**
- More complex to implement
- Wastes resources refreshing unused data

```python
# Refresh-Ahead implementation concept
import time

class RefreshAheadCache:
    def __init__(self, ttl, refresh_threshold=0.8):
        self.cache = {}
        self.ttl = ttl
        self.refresh_threshold = refresh_threshold
    
    def get(self, key):
        if key not in self.cache:
            return None
        
        entry = self.cache[key]
        
        # Check if approaching expiration
        age = time.time() - entry["timestamp"]
        if age > self.ttl * self.refresh_threshold:
            # Refresh in background (simplified)
            self.refresh_async(key)
        
        return entry["value"]
    
    def refresh_async(self, key):
        # In production, use thread pool or task queue
        # This is simplified
        value = fetch_from_db(key)
        self.set(key, value)
```

## Code Walkthrough

### Implementing Cache-Aside in Flask

```python
# app.py
from flask import Flask, jsonify
import redis
import json
import time
from functools import wraps

app = Flask(__name__)

# Redis connection
redis_client = redis.Redis(host="localhost", port=6379, db=0)

# Simulated database
class User:
    def __init__(self, id, name, email):
        self.id = id
        self.name = name
        self.email = email
    
    def to_dict(self):
        return {"id": self.id, "name": self.name, "email": self.email}

# Fake database
users_db = {
    1: User(1, "Alice", "alice@example.com"),
    2: User(2, "Bob", "bob@example.com"),
    3: User(3, "Charlie", "charlie@example.com"),
}

# ============================================
# Cache-Aside Implementation
# ============================================

def cache_aside(ttl=60):
    """
    Decorator for Cache-Aside pattern
    
    Args:
        ttl: Time to live in seconds
    """
    def decorator(f):
        @wraps(f)
        def wrapper(*args, **kwargs):
            # Generate cache key from function name and args
            cache_key = f"{f.__name__}:{':'.join(map(str, args))}"
            
            # 1. Try to get from cache
            cached = redis_client.get(cache_key)
            if cached:
                print(f"Cache HIT for {cache_key}")
                return json.loads(cached)
            
            print(f"Cache MISS for {cache_key}")
            
            # 2. Execute function (fetch from DB)
            result = f(*args, **kwargs)
            
            # 3. Store in cache
            redis_client.setex(
                cache_key,
                ttl,
                json.dumps(result)
            )
            
            return result
        return wrapper
    return decorator

@cache_aside(ttl=60)  # Cache for 60 seconds
def get_user(user_id):
    """Fetch user from database"""
    time.sleep(1)  # Simulate slow DB query
    user = users_db.get(user_id)
    if user:
        return user.to_dict()
    return None

@cache_aside(ttl=300)  # Cache for 5 minutes
def get_all_users():
    """Fetch all users from database"""
    time.sleep(2)  # Simulate slow query
    return [user.to_dict() for user in users_db.values()]

# ============================================
# Cache Invalidation
# ============================================

def invalidate_user_cache(user_id):
    """Invalidate cache for a specific user"""
    cache_key = f"get_user:{user_id}"
    redis_client.delete(cache_key)
    print(f"Invalidated cache for {cache_key}")

def invalidate_all_users_cache():
    """Invalidate all users cache"""
    # Get all keys matching pattern
    keys = redis_client.keys("get_all_users:*")
    if keys:
        redis_client.delete(*keys)
    print(f"Invalidated {len(keys)} cache entries")

# ============================================
# Flask Routes
# ============================================

@app.route("/users/<int:user_id>")
def user_route(user_id):
    user = get_user(user_id)
    if user:
        return jsonify(user)
    return jsonify({"error": "User not found"}), 404

@app.route("/users")
def users_route():
    users = get_all_users()
    return jsonify(users)

@app.route("/users/<int:user_id>", methods=["DELETE"])
def delete_user_route(user_id):
    if user_id in users_db:
        del users_db[user_id]
        invalidate_user_cache(user_id)
        invalidate_all_users_cache()
        return jsonify({"status": "deleted"})
    return jsonify({"error": "User not found"}), 404

if __name__ == "__main__":
    app.run(debug=True, port=5000)
```

### Testing Cache-Aside

```bash
# First request - cache miss (slow)
curl http://localhost:5000/users/1
# Returns user after ~1 second (DB query)

# Second request - cache hit (fast)
curl http://localhost:5000/users/1
# Returns immediately from cache

# Delete user - invalidates cache
curl -X DELETE http://localhost:5000/users/1

# Next request - cache miss again (DB query)
curl http://localhost:5000/users/1
# Slow again until cache refills
```

### Benchmarking Cache-Aside

```python
# benchmark.py
import time
import requests

def benchmark(endpoint, iterations=10):
    """Benchmark an endpoint"""
    times = []
    
    for i in range(iterations):
        start = time.time()
        response = requests.get(f"http://localhost:5000{endpoint}")
        elapsed = time.time() - start
        times.append(elapsed)
    
    avg = sum(times) / len(times)
    print(f"\n{endpoint}:")
    print(f"  Iterations: {iterations}")
    print(f"  Average: {avg:.3f}s")
    print(f"  Min: {min(times):.3f}s")
    print(f"  Max: {max(times):.3f}s")
    
    # First request is typically slow (cache miss)
    print(f"  First request: {times[0]:.3f}s")
    print(f"  Subsequent average: {sum(times[1:])/(len(times)-1):.3f}s")

# Run benchmarks
benchmark("/users/1")
benchmark("/users")
```

**Expected results:**

```
/users/1:
  Iterations: 10
  Average: 0.101s
  First request: 1.002s (cache miss)
  Subsequent average: 0.001s (cache hits!)

/users:
  Iterations: 10
  Average: 0.201s
  First request: 2.003s (cache miss)
  Subsequent average: 0.002s (cache hits!)
```

## Common Mistakes

### ❌ Using same TTL for all data

```python
# WRONG: Everything cached for same time
cache.set("user:1", user, ttl=3600)  # 1 hour
cache.set("news_feed", news, ttl=3600)  # 1 hour - too long!
```

### ✅ Match TTL to data freshness needs

```python
# CORRECT: Different TTLs for different data
cache.set(f"user:{user_id}", user, ttl=600)    # 10 min - profiles change
cache.set("news_feed", news, ttl=60)           # 1 min - news changes fast
cache.set("site_config", config, ttl=86400)   # 24 hours - rarely changes
```

### ❌ Not handling cache failures gracefully

```python
# WRONG: If cache fails, whole request fails
@app.route("/users/<int:user_id>")
def get_user(user_id):
    user = redis.get(f"user:{user_id}")  # If Redis is down, this raises!
    # ...
```

### ✅ Handle cache failures

```python
# CORRECT: Graceful degradation
@app.route("/users/<int:user_id>")
def get_user(user_id):
    try:
        # Try cache first
        cached = redis.get(f"user:{user_id}")
        if cached:
            return json.loads(cached)
    except redis.RedisError:
        # Cache failed, continue to DB
        pass
    
    # Fall back to database
    user = db.query(User).get(user_id)
    return jsonify(user.to_dict())
```

## Quick Reference

| Strategy | Read Speed | Write Speed | Complexity | Data Freshness |
|----------|-----------|-------------|------------|-----------------|
| Cache-Aside | Fast (after first) | Fast | Low | Stale until TTL |
| Write-Through | Fast | Slow | Medium | Always fresh |
| Write-Behind | Fast | Fastest | High | Risk of loss |
| Refresh-Ahead | Fast | Medium | High | Usually fresh |

**Choosing a strategy:**

- **Most applications**: Cache-Aside
- **Write-heavy + read-heavy**: Write-Through
- **Very write-heavy**: Write-Behind (carefully!)
- **Latency-critical reads**: Refresh-Ahead

## Next Steps

Continue to [03_cache_invalidation.md](03_cache_invalidation.md) to learn about one of the hardest problems in caching: keeping your cache in sync with your database.