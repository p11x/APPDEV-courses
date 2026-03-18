<!-- FILE: 12_caching_and_performance/02_flask_caching/01_installing_flask_caching.md -->

## Overview

Flask-Caching is the official caching extension for Flask. It provides a unified API for various caching backends and makes it easy to add caching to your Flask applications. This file teaches you how to install and configure Flask-Caching.

## Prerequisites

- Flask installed
- Basic understanding of caching concepts

## Core Concepts

### Flask-Caching Features

- **Multiple backends**: Redis, Memcached, SimpleMemory, FileSystem
- **Unified API**: Same code regardless of backend
- **View caching**: Decorator-based route caching
- **Fragment caching**: Cache specific template parts
- **Cache key generation**: Automatic based on request

### Supported Backends

| Backend | Description | Use Case |
|---------|-------------|----------|
| Redis | In-memory data store | Production, distributed |
| Memcached | In-memory key-value store | Production, simple |
| SimpleMemory | In-memory dict | Development, testing |
| FileSystem | Disk-based | Shared hosting, dev |

## Code Walkthrough

### Installation

```bash
# Install Flask-Caching
pip install Flask-Caching

# Install Redis support
pip install redis

# Install Memcached support (optional)
pip install python-memcached
```

### Basic Configuration

```python
# app.py
from flask import Flask
from flask_caching import Cache

app = Flask(__name__)

# Configuration
app.config["CACHE_TYPE"] = "SimpleMemoryCache"  # For development
# app.config["CACHE_TYPE"] = "RedisCache"       # For production
# app.config["CACHE_REDIS_HOST"] = "localhost"
# app.config["CACHE_REDIS_PORT"] = 6379
# app.config["CACHE_REDIS_DB"] = 0

# Initialize cache
cache = Cache(app)

@app.route("/")
def index():
    return "Hello, World!"

if __name__ == "__main__":
    app.run(debug=True)
```

### Redis Configuration

```python
# production_config.py
import os

class Config:
    # Redis configuration
    CACHE_TYPE = "RedisCache"
    CACHE_REDIS_HOST = os.environ.get("REDIS_HOST", "localhost")
    CACHE_REDIS_PORT = int(os.environ.get("REDIS_PORT", 6379))
    CACHE_REDIS_DB = int(os.environ.get("REDIS_DB", 0))
    CACHE_REDIS_PASSWORD = os.environ.get("REDIS_PASSWORD", None)
    
    # Optional settings
    CACHE_DEFAULT_TIMEOUT = 300  # 5 minutes
    CACHE_KEY_PREFIX = "myapp"   # Prefix for all keys
    CACHE_OPTIONS = {
        "socket_timeout": 5,
        "socket_connect_timeout": 5,
    }
```

### Complete Application Example

```python
# complete_app.py
from flask import Flask, jsonify
from flask_caching import Cache
import time

app = Flask(__name__)

# Configure cache
app.config.update(
    CACHE_TYPE="RedisCache",
    CACHE_REDIS_HOST="localhost",
    CACHE_REDIS_PORT=6379,
    CACHE_REDIS_DB=0,
    CACHE_DEFAULT_TIMEOUT=300,
    CACHE_KEY_PREFIX="flask_caching_demo"
)

# Initialize cache
cache = Cache(app)

# Simulated database
users_db = {
    1: {"id": 1, "name": "Alice", "email": "alice@example.com"},
    2: {"id": 2, "name": "Bob", "email": "bob@example.com"},
    3: {"id": 3, "name": "Charlie", "email": "charlie@example.com"},
}

# ============================================
# Cache Operations
# ============================================

@app.route("/")
def index():
    return jsonify({"message": "Flask Caching Demo"})

@app.route("/users")
@cache.cached(timeout=60, key_prefix="all_users")
def get_users():
    """Get all users - cached for 60 seconds"""
    time.sleep(1)  # Simulate slow DB query
    return jsonify(list(users_db.values()))

@app.route("/users/<int:user_id>")
@cache.cached(timeout=300, key_prefix="user")
def get_user(user_id):
    """Get single user - cached for 5 minutes"""
    time.sleep(0.5)  # Simulate slow DB query
    user = users_db.get(user_id)
    if user:
        return jsonify(user)
    return jsonify({"error": "User not found"}), 404

@app.route("/users/<int:user_id>", methods=["DELETE"])
def delete_user(user_id):
    """Delete user and invalidate cache"""
    if user_id in users_db:
        del users_db[user_id]
        
        # Invalidate caches
        cache.delete("all_users")
        cache.delete(f"user:{user_id}")
        
        return jsonify({"status": "deleted"})
    return jsonify({"error": "Not found"}), 404

@app.route("/cache/clear")
def clear_cache():
    """Clear all cache"""
    cache.clear()
    return jsonify({"status": "cleared"})

@app.route("/cache/stats")
def cache_stats():
    """Get cache statistics"""
    # Note: Not all backends support this
    return jsonify({
        "backend": app.config["CACHE_TYPE"],
        "key_prefix": app.config.get("CACHE_KEY_PREFIX", "")
    })

if __name__ == "__main__":
    app.run(debug=True, port=5000)
```

## Common Mistakes

### ❌ Using SimpleMemoryCache in production

```python
# WRONG: Not suitable for production
app.config["CACHE_TYPE"] = "SimpleMemoryCache"
```

### ✅ Use Redis in production

```python
# CORRECT: Use Redis
app.config["CACHE_TYPE"] = "RedisCache"
app.config["CACHE_REDIS_HOST"] = "redis"
app.config["CACHE_REDIS_PORT"] = 6379
```

## Quick Reference

```bash
pip install Flask-Caching redis
```

```python
app.config["CACHE_TYPE"] = "RedisCache"
cache = Cache(app)
```

## Next Steps

Continue to [02_caching_view_functions.md](02_caching_view_functions.md)
