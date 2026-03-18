<!-- FILE: 18_rate_limiting_and_security/02_rate_limiting/02_flask_limiter_setup.md -->

## Overview

Set up Flask-Limiter for production-ready rate limiting.

## Prerequisites

- Flask application
- Understanding of rate limiting concepts

## Core Concepts

Flask-Limiter is the standard rate limiting library for Flask. It supports multiple storage backends and flexible configuration.

## Code Walkthrough

### Installation

```bash
pip install Flask-Limiter
```

### Basic Setup

```python
# app.py
from flask import Flask
from flask_limiter import Limiter
from flask_limiter.util import get_remote_address

app = Flask(__name__)

# Initialize Limiter
limiter = Limiter(
    app=app,
    key_func=get_remote_address,  # Use IP as default key
    default_limits=["200 per day", "50 per hour"],
    storage_uri="memory://"  # In-memory (use Redis in production)
)

# Your routes...
@app.route('/api/data')
def get_data():
    return {'data': 'some data'}
```

### Configuration Options

```python
# config.py
class Config:
    # Rate limit storage
    RATELIMIT_ENABLED = True
    RATELIMIT_STORAGE_URI = "redis://localhost:6379"  # Use Redis in production
    
    # Default limits (applied to all routes)
    RATELIMIT_DEFAULT = "100 per minute"
    
    # Strategy: 'fixed-window', 'sliding-window', 'moving-window'
    RATELIMIT_STRATEGY = "fixed-window"
    
    # Headers to send
    RATELIMIT_HEADERS_ENABLED = True
```

### Applying Rate Limits

```python
# Different ways to apply limits

# 1. Decorator on specific route
@app.route('/login', methods=['POST'])
@limiter.limit("5 per minute")
def login():
    # Handle login
    return {'message': 'Logged in'}

# 2. Limit by IP with custom key
@app.route('/api/users')
@limiter.limit("10 per minute", key_func=lambda: request.headers.get('X-API-Key'))
def get_users():
    return {'users': []}

# 3. Limit by user (after authentication)
@app.route('/dashboard')
@limiter.limit("100 per hour", key_func=lambda: current_user.id if current_user.is_authenticated else request.remote_addr)
def dashboard():
    return {'data': 'dashboard'}

# 4. Multiple limits (most restrictive applies)
@app.route('/upload', methods=['POST'])
@limiter.limit("10 per hour")
@limiter.limit("3 per minute")
def upload():
    return {'message': 'Uploaded'}

# 5. Using config limits
@app.route('/search')
@limiter.limit("config:RATE_LIMIT_SEARCH")
def search():
    return {'results': []}
```

### Rate Limit Exceeded Handler

```python
# Custom error handler
@app.errorhandler(429)
def ratelimit_handler(e):
    return {
        'error': 'Rate limit exceeded',
        'message': f'Try again in {e.description} seconds'
    }, 429

# Or with more detail
@app.after_request
def add_rate_limit_headers(response):
    # Headers are added automatically, but you can customize
    if hasattr(g, 'ratelimit_remaining'):
        response.headers['X-RateLimit-Remaining'] = str(g.ratelimit_remaining)
    return response
```

### Line-by-Line Breakdown

- `key_func` determines how clients are identified (IP, user ID, API key)
- `default_limits` applies to all routes if not overridden
- Decorators apply specific limits to individual routes

> **⚡ Performance Note:** For production, use Redis storage to share limits across multiple servers.

## Common Mistakes

- ❌ Not having rate limiting in production
- ✅ Always enable rate limiting

- ❌ Using in-memory storage with multiple servers
- ✅ Use Redis for distributed deployments

- ❌ Too restrictive limits causing false positives
- ✅ Start with generous limits, adjust based on traffic

## Quick Reference

| Configuration | Purpose |
|---------------|---------|
| `RATELIMIT_STORAGE_URI` | Storage backend |
| `key_func` | Client identification |
| `@limiter.limit()` | Per-route limits |
| `default_limits` | Global limits |

## Next Steps

Continue to [03_per_user_and_per_ip_limits.md](./03_per_user_and_per_ip_limits.md) to learn about different rate limit strategies.
