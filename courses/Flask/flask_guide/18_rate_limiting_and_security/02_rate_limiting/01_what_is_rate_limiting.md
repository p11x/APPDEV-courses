<!-- FILE: 18_rate_limiting_and_security/02_rate_limiting/01_what_is_rate_limiting.md -->

## Overview

Understand rate limiting and why it's essential for Flask applications.

## Prerequisites

- Basic Flask knowledge
- Understanding of HTTP requests

## Core Concepts

Rate limiting controls how many requests a client can make in a given time period. It's your first line of defense against abuse, brute force attacks, and DoS attacks.

## Why Rate Limiting Matters

### Attack Prevention

**Brute Force Protection:**
Without rate limiting, attackers can try millions of passwords per second.

```
# Attacker tries 10,000 passwords
# Without rate limiting: succeeds in minutes
# With rate limiting (5/min): takes 28+ hours
```

**DoS Prevention:**
Without rate limiting, a single user can overwhelm your server.

### Real-World Examples

| Scenario | Limit | Purpose |
|----------|-------|---------|
| Login attempts | 5/minute | Prevent brute force |
| API calls | 100/minute | Fair usage |
| File uploads | 10/hour | Prevent storage abuse |
| Password reset | 3/hour | Prevent email spam |

## How Rate Limiting Works

```
Client Request
     ↓
Check rate limit (Redis/in-memory)
     ↓
[Under limit] → Allow → Process request
     ↓
[Over limit] → Deny → Return 429 Too Many Requests
```

### HTTP Response Codes

| Code | Meaning |
|------|---------|
| 200 | Request allowed |
| 429 | Rate limit exceeded |
| 429 + Retry-After | When to try again |

### Rate Limiting Strategies

1. **Fixed Window:** Count requests in each time window
2. **Sliding Window:** More accurate, tracks request timestamps
3. **Token Bucket:** Allows burst traffic up to a limit

## Code Walkthrough

```python
# Simple in-memory rate limiter
from flask import Flask, request, jsonify
from time import time

# In-memory storage (use Redis in production)
request_counts = {}

def check_rate_limit(key: str, limit: int, window: int) -> tuple[bool, int]:
    """
    Check if request is within rate limit.
    
    Returns: (is_allowed, remaining_requests)
    """
    now = time()
    window_start = now - window
    
    # Get or initialize user window
    if key not in request_counts:
        request_counts[key] = []
    
    # Remove old requests outside window
    request_counts[key] = [
        ts for ts in request_counts[key] 
        if ts > window_start
    ]
    
    # Check limit
    if len(request_counts[key]) >= limit:
        return False, 0
    
    # Add current request
    request_counts[key].append(now)
    remaining = limit - len(request_counts[key])
    
    return True, remaining

@app.route('/api/data')
def get_data():
    # Rate limit by IP
    ip = request.remote_addr
    allowed, remaining = check_rate_limit(ip, limit=10, window=60)
    
    if not allowed:
        return jsonify({'error': 'Rate limit exceeded'}), 429
    
    response = jsonify({'data': 'some data'})
    response.headers['X-RateLimit-Limit'] = str(10)
    response.headers['X-RateLimit-Remaining'] = str(remaining)
    return response
```

> **💡 Tip:** Use Flask-Limiter library for production-ready rate limiting.

## Quick Reference

| Concept | Description |
|---------|-------------|
| Limit | Max requests per window |
| Window | Time period (seconds) |
| Key | Identifier (IP, user ID) |
| 429 | HTTP status for rate limited |

## Next Steps

Continue to [02_flask_limiter_setup.md](./02_flask_limiter_setup.md) to learn about Flask-Limiter.
