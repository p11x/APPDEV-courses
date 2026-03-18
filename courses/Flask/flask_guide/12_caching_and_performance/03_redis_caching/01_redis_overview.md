<!-- FILE: 12_caching_and_performance/03_redis_caching/01_redis_overview.md -->

## Overview

Redis is the most popular caching backend for Flask applications. This file provides an overview of Redis, why it's excellent for caching, and how to set it up.

## Core Concepts

Redis is an in-memory data store that can be used as a database, cache, and message broker. It's ideal for caching because:
- Extremely fast (in-memory)
- Supports data structures (strings, hashes, lists, sets)
- TTL support for automatic expiration
- Persistent (optional)
- Network-accessible (distributed caching)

## Code Walkthrough

```bash
# Install Redis
# macOS
brew install redis

# Ubuntu/Debian
sudo apt-get install redis-server
```

```python
# redis_basics.py
import redis
import json

# Connect to Redis
r = redis.Redis(host="localhost", port=6379, db=0)

# String operations
r.set("name", "Alice")
r.setex("temp_data", 60, "expires in 60s")  # With TTL
value = r.get("name")

# Hash operations (good for objects)
r.hset("user:1", "name", "Alice")
r.hset("user:1", "email", "alice@example.com")
user = r.hgetall("user:1")

# JSON serialization
def cache_json(key, data, ttl=300):
    r.setex(key, ttl, json.dumps(data))

def get_cached_json(key):
    data = r.get(key)
    return json.loads(data) if data else None
```

## Next Steps

Continue to [02_connecting_flask_to_redis.md](02_connecting_flask_to_redis.md)
