# Redis Fundamentals

## What You'll Learn
- Using Redis for caching

## Prerequisites
- Completed caching concepts

```bash
pip install redis
```

```python
import redis

r = redis.Redis(host='localhost', port=6379, db=0)

# Set and get
r.set('key', 'value')
value = r.get('key')

# With expiry
r.setex('key', 3600, 'value')  # 1 hour

# Delete
r.delete('key')
```

## Summary
- Redis is an in-memory data store
- Great for caching and session storage
