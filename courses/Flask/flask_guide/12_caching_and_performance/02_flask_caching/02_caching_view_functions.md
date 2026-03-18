<!-- FILE: 12_caching_and_performance/02_flask_caching/02_caching_view_functions.md -->

## Overview

Flask-Caching provides decorators to cache entire views or parts of views. This file teaches you how to use the `@cached` and `@cache.cached` decorators, how to generate cache keys based on request parameters, and how to handle cache invalidation.

## Prerequisites

- Flask-Caching installed and configured
- Understanding of Flask routes

## Core Concepts

The `@cached` decorator caches the return value of a view function. Flask-Caching automatically generates a cache key based on the request URL, but you can customize it.

## Code Walkthrough

```python
# view_caching.py
from flask import Flask, jsonify, request
from flask_caching import Cache
import time

app = Flask(__name__)
app.config["CACHE_TYPE"] = "SimpleMemoryCache"
cache = Cache(app)

# ============================================
# Basic View Caching
# ============================================

@app.route("/api/users")
@cache.cached(timeout=60)
def get_users():
    """Cached endpoint - returns same result for 60 seconds"""
    time.sleep(1)  # Simulate slow query
    return jsonify({"users": [{"id": 1, "name": "Alice"}]})

# ============================================
# Cache Key Based on URL
# ============================================

@app.route("/api/users/<int:user_id>")
@cache.cached(timeout=300)
def get_user(user_id):
    """Cached with user_id - separate cache per user"""
    time.sleep(0.5)
    return jsonify({"id": user_id, "name": f"User {user_id}"})

# ============================================
# Custom Cache Key
# ============================================

@app.route("/api/search")
@cache.cached(timeout=300, query_string=True)
def search():
    """Cache based on query string"""
    query = request.args.get("q", "")
    time.sleep(1)
    return jsonify({"query": query, "results": ["result1", "result2"]})

# ============================================
# Manual Cache Operations
# ============================================

@app.route("/api/cache/set", methods=["POST"])
def set_cache():
    """Manually set cache value"""
    key = request.json.get("key")
    value = request.json.get("value")
    timeout = request.json.get("timeout", 300)
    
    cache.set(key, value, timeout=timeout)
    return jsonify({"status": "set", "key": key})

@app.route("/api/cache/get/<key>")
def get_cache(key):
    """Manually get cache value"""
    value = cache.get(key)
    return jsonify({"key": key, "value": value})

@app.route("/api/cache/delete/<key>")
def delete_cache(key):
    """Delete specific cache key"""
    cache.delete(key)
    return jsonify({"status": "deleted", "key": key})

@app.route("/api/cache/clear")
def clear_all_cache():
    """Clear all cache"""
    cache.clear()
    return jsonify({"status": "cleared"})

if __name__ == "__main__":
    app.run(port=5000)
```

## Common Mistakes

```python
# WRONG: Cache key doesn't include parameters
@app.route("/search")
@cache.cached()
def search():
    return search_db(request.args.get("q"))  # Wrong for different queries

# CORRECT: Use query_string=True
@app.route("/search") 
@cache.cached(query_string=True)
def search():
    return search_db(request.args.get("q"))
```

## Next Steps

Continue to [03_caching_arbitrary_data.md](03_caching_arbitrary_data.md)
