# Caching with Flask

## What You'll Learn
- Implementing caching in Flask applications
- Using Flask-Caching with Redis
- Decorator-based caching

## Prerequisites
- Completed Flask folder and Redis fundamentals

## Installing Flask-Caching

```bash
pip install flask-caching redis
```

## Basic Cache Setup

```python
from flask import Flask
from flask_caching import Cache

app = Flask(__name__)
cache = Cache(app, config={
    'CACHE_TYPE': 'RedisCache',
    'CACHE_REDIS_HOST': 'localhost',
    'CACHE_REDIS_PORT': 6379,
    'CACHE_DEFAULT_TIMEOUT': 300
})
```

## Using Cache Decorator

```python
from flask import Flask
from flask_caching import Cache

app = Flask(__name__)
cache = Cache(app, config={'CACHE_TYPE': 'SimpleCache'})

@app.route('/users/<int:user_id>')
@cache.cached(timeout=300, key_prefix='user_')
def get_user(user_id: int) -> dict:
    # Expensive database query
    user = fetch_user_from_db(user_id)
    return {'id': user_id, 'name': user.name}

def fetch_user_from_db(user_id: int) -> object:
    # Simulate database call
    return type('User', (), {'name': f'User {user_id}'})()
```

🔍 **Line-by-Line Breakdown:**
1. `from flask_caching import Cache` — Flask-Caching provides caching integration
2. `cache = Cache(app, config={...})` — Initialize cache with Redis backend
3. `@cache.cached(timeout=300, key_prefix='user_')` — Cache the function result for 5 minutes
4. The key is automatically generated from the URL and parameters

## Manual Cache Operations

```python
@app.route('/products')
def get_products() -> dict:
    # Try to get from cache first
    products = cache.get('all_products')
    
    if products is None:
        products = fetch_all_products()
        cache.set('all_products', products, timeout=3600)
    
    return {'products': products}

@app.route('/product/<int:product_id>/invalidate')
def invalidate_product(product_id: int) -> dict:
    cache.delete(f'product_{product_id}')
    cache.delete('all_products')  # Invalidate list too
    return {'message': 'Cache invalidated'}
```

## Summary
- Flask-Caching provides simple decorator-based caching
- Use Redis for production caching
- Implement cache invalidation for data updates

## Next Steps
→ Continue to `04-caching-with-fastapi.md`
