<!-- FILE: 12_caching_and_performance/01_caching_concepts/01_what_is_caching.md -->

## Overview

Caching is a technique for storing frequently accessed data in a fast storage layer so that future requests can be served more quickly. This file explains the fundamentals of caching, why it matters for web applications, and the basic concepts you need to understand before implementing caching in Flask.

## Prerequisites

- Basic Flask knowledge
- Understanding of HTTP requests and responses
- Familiarity with databases

## Core Concepts

### What is Caching?

Caching is like keeping a copy of something in a convenient location so you don't have to fetch it again. Think of it like:

- **Browser cache**: Saving web pages so they load faster
- **DNS cache**: Storing domain name lookups
- **CPU cache**: Fast memory that stores frequently used data

In web applications, caching helps avoid:
- Expensive database queries
- Slow API calls to external services
- Repeated computation of the same data
- Re-rendering the same templates

### How Caching Works

```
                    ┌─────────────────────────────────────────┐
                    │           WITHOUT CACHING               │
                    │                                         │
  Request ────────►│  Database ──► Process ──► Response     │
                    │    (slow)     (slow)      (fast)      │
                    │                                         │
                    └─────────────────────────────────────────┘

                    ┌─────────────────────────────────────────┐
                    │           WITH CACHING                   │
                    │                                         │
  Request ────────►│  Cache HIT? ──► Process ──► Response   │
                    │      │              │                    │
                    │   YES│              │NO                  │
                    │      ▼              ▼                    │
                    │  Cache ◄──► Database ──► Process       │
                    │  (fast)       (slow)     (slow)        │
                    │                                         │
                    └─────────────────────────────────────────┘
```

### Cache Hit vs Cache Miss

| Term | Description |
|------|-------------|
| **Cache Hit** | Data found in cache → return immediately (fast) |
| **Cache Miss** | Data not in cache → fetch from source, store in cache, return |
| **Cache Miss Rate** | Percentage of requests that miss the cache (lower is better) |

### Types of Caching in Web Applications

| Level | Description | Examples |
|-------|-------------|----------|
| **Browser** | Client-side caching | HTTP headers, Service Workers |
| **CDN** | Edge caching | CloudFlare, CloudFront |
| **Application** | In-memory caching | Redis, Memcached |
| **Database** | Query caching | SQLAlchemy, PostgreSQL |

### Key Caching Terminology

| Term | Definition |
|------|------------|
| **TTL (Time To Live)** | How long data stays in cache before expiring |
| **Cache Key** | Unique identifier for cached data |
| **Cache Invalidation** | Removing data from cache (tricky!) |
| **Cache Eviction** | Automatic removal when cache is full |
| **Warm Cache** | Pre-populated cache |
| **Cold Cache** | Empty cache (all requests miss initially) |

## Code Walkthrough

### Simple Caching Example

Let's see caching in action with a basic Flask app:

```python
# without_caching.py - SLOW version
from flask import Flask, jsonify
import time

app = Flask(__name__)

# Simulated database
def get_user_from_db(user_id):
    """Simulate a slow database query"""
    time.sleep(1)  # Takes 1 second!
    return {"id": user_id, "name": f"User {user_id}"}

@app.route("/users/<int:user_id>")
def get_user(user_id):
    """
    This endpoint is slow because:
    - Every request hits the database
    - No caching at all
    """
    user = get_user_from_db(user_id)
    return jsonify(user)

# Benchmark: 10 requests = ~10 seconds

if __name__ == "__main__":
    app.run(port=5000)
```

```python
# with_caching.py - FAST version with caching
from flask import Flask, jsonify
import time
from functools import lru_cache

app = Flask(__name__)

# Simulated database
def get_user_from_db(user_id):
    """Simulate a slow database query"""
    time.sleep(1)  # Takes 1 second!
    return {"id": user_id, "name": f"User {user_id}"}

# Cache: Python's built-in LRU cache
# @lru_cache stores up to 128 results in memory
# TTL not built-in, but simpler for demonstration
@lru_cache(maxsize=128)
def get_user_cached(user_id):
    """Cached version of database query"""
    return get_user_from_db(user_id)

@app.route("/users/<int:user_id>")
def get_user(user_id):
    """
    This endpoint is fast because:
    - First request fetches from DB
    - Subsequent requests return cached result
    """
    user = get_user_cached(user_id)
    return jsonify(user)

# Benchmark: First request ~1 second, rest ~0.001 seconds

if __name__ == "__main__":
    app.run(port=5000)
```

### Benchmark Results

```bash
# Test WITHOUT caching
time for i in {1..10}; do
  curl -s http://localhost:5000/users/1 > /dev/null
done

# Real: ~10 seconds

# Test WITH caching
time for i in {1..10}; do
  curl -s http://localhost:5000/users/1 > /dev/null
done

# Real: ~1 second (first request) + 9 * ~0ms = ~1 second total
```

### Cache Key Design

The cache key should uniquely identify the data. Good vs bad examples:

```python
# BAD: Too generic - can't distinguish different users
cache_key = "user_data"

# GOOD: Specific enough
cache_key = f"user:{user_id}"

# BETTER: Include version, language, etc.
cache_key = f"user:{user_id}:v2:en"

# For complex queries: hash the query
import hashlib

def get_query_cache_key(query, params):
    """Create cache key from query"""
    raw = f"{query}:{params}"
    return f"query:{hashlib.md5(raw.encode()).hexdigest()}"
```

### TTL (Time To Live) Examples

```python
# Different data needs different TTLs

# User profiles: 5-15 minutes (updates infrequent)
ttl_user = 600  # 10 minutes

# Session data: 1-24 hours
ttl_session = 3600  # 1 hour

# Product catalog: 15-60 minutes (updates occasional)
ttl_products = 1800  # 30 minutes

# News feed: 1-5 minutes (updates frequently)
ttl_news = 60  # 1 minute

# Static config: 24 hours or more
ttl_config = 86400  # 24 hours
```

## Common Mistakes

### ❌ Caching too aggressively

```python
# WRONG: Caching everything forever
@lru_cache()
def get_user(user_id):
    # If user data changes, users see stale data!
    return db.query(User).get(user_id)
```

### ✅ Use appropriate TTL

```python
# CORRECT: Cache with reasonable TTL
@lru_cache(maxsize=1000)
def get_user(user_id):
    # Check cache first (handled by lru_cache)
    return db.query(User).get(user_id)

# Clear cache when user updates their profile
@app.route("/users/<int:user_id>", methods=["PUT"])
def update_user(user_id):
    get_user.cache_clear()  # Clear specific cache
    # Or use a smarter cache with TTL
```

### ❌ Not handling cache invalidation

```python
# PROBLEM: Data updates but cache doesn't
user = get_user(user_id)  # Returns cached
user.name = "New Name"
db.commit()
# Next call still returns old cached value!
```

### ✅ Invalidate on updates

```python
# SOLUTION: Clear cache on updates
@lru_cache(maxsize=128)
def get_user(user_id):
    return db.query(User).get(user_id)

@app.route("/users/<int:user_id>", methods=["PUT"])
def update_user(user_id):
    user = db.query(User).get(user_id)
    user.name = request.json["name"]
    db.commit()
    
    # Important: Clear cache!
    get_user.cache_clear()
    
    return jsonify({"status": "updated"})
```

### ❌ Cache key collisions

```python
# WRONG: Same key for different data
def get_user(user_id):
    cache_key = "user"  # All users share same key!
    return cache.get(cache_key) or fetch_and_cache()

# Users 1, 2, 3 all get User 1's data!
```

### ✅ Unique cache keys

```python
# CORRECT: Unique keys per user
def get_user(user_id):
    cache_key = f"user:{user_id}"  # Unique per user
    return cache.get(cache_key) or fetch_and_cache(cache_key)
```

## Quick Reference

| Cache Type | Speed | Persistence | Use Case |
|------------|-------|-------------|----------|
| Memory (lru_cache) | Fastest | Lost on restart | Per-process caching |
| Redis | Fast | Persistent | Distributed caching |
| Memcached | Fast | Persistent | Simple caching |
| CDN | Varies | Configurable | Static assets |

**Good caching candidates:**
- Database queries (especially complex joins)
- External API calls
- Computed values
- Template rendering

**Bad caching candidates:**
- Frequently changing data (real-time)
- User-specific sensitive data
- Very fast operations (cache overhead > compute time)

## Next Steps

Continue to [02_cache_strategies.md](02_cache_strategies.md) to learn about different caching strategies like cache-aside, write-through, and read-through.