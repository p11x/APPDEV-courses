<!-- FILE: 18_rate_limiting_and_security/02_rate_limiting/03_per_user_and_per_ip_limits.md -->

## Overview

Implement per-user and per-IP rate limiting strategies for Flask applications.

## Prerequisites

- Flask-Limiter installed
- Understanding of authentication

## Core Concepts

Different rate limit strategies protect against different attack vectors. Combining per-IP and per-user limits provides comprehensive protection.

## Code Walkthrough

### Per-IP Rate Limiting

```python
# Per-IP limits - protects against coordinated attacks from single source

@app.route('/api/public')
@limiter.limit("30 per minute", key_func=lambda: request.remote_addr)
def public_api():
    """Public endpoint - rate limited by IP."""
    return {'data': 'public data'}

@app.route('/login', methods=['POST'])
@limiter.limit("5 per minute", key_func=lambda: request.remote_addr)
def login():
    """Login - stricter limit by IP to prevent brute force."""
    return {'message': 'Login endpoint'}
```

### Per-User Rate Limiting

```python
# Per-user limits - protects against account-specific abuse

def get_user_identity():
    """Get identity for authenticated users, fall back to IP."""
    if hasattr(current_user, 'is_authenticated') and current_user.is_authenticated:
        return f"user:{current_user.id}"
    return f"ip:{request.remote_addr}"

@app.route('/api/premium')
@limiter.limit("100 per hour", key_func=get_user_identity)
def premium_api():
    """Premium endpoint - higher limit for authenticated users."""
    return {'premium_data': True}
```

### Combined Per-User and Per-IP

```python
# Multiple limits - both must pass

@app.route('/api/upload', methods=['POST'])
@limiter.limit("10 per hour", key_func=get_user_identity)  # Per user
@limiter.limit("5 per minute", key_func=lambda: request.remote_addr)  # Per IP
def upload_file():
    """Upload endpoint - strict limits to prevent abuse."""
    return {'message': 'File uploaded'}
```

### Role-Based Rate Limits

```python
# Different limits based on user role

def get_dynamic_limit():
    """Determine limit based on user role."""
    if not current_user.is_authenticated:
        return "10 per minute"  # Anonymous users
    
    if current_user.role == 'admin':
        return "1000 per minute"  # Admins get more
    
    if current_user.role == 'premium':
        return "500 per minute"  # Premium users
    
    return "100 per minute"  # Regular users

@app.route('/api/data')
@limiter.limit(get_dynamic_limit)
def get_data():
    """Dynamic rate limit based on user role."""
    return {'data': 'some data'}
```

### Different Endpoints, Different Limits

```python
# Sensitive endpoints get stricter limits

# Login - very strict (prevent brute force)
@app.route('/auth/login', methods=['POST'])
@limiter.limit("5 per minute;10 per hour")
def login():
    return {'token': 'xyz'}

# Password reset - extremely strict
@app.route('/auth/reset', methods=['POST'])
@limiter.limit("3 per hour")
def reset_password():
    return {'message': 'Reset email sent'}

# Registration - strict to prevent spam
@app.route('/auth/register', methods=['POST'])
@limiter.limit("3 per hour;10 per day")
def register():
    return {'message': 'Registered'}

# General API - moderate limits
@app.route('/api/posts')
@limiter.limit("60 per minute")
def get_posts():
    return {'posts': []}

# Search - expensive, stricter limits
@app.route('/api/search')
@limiter.limit("10 per minute")
def search():
    return {'results': []}
```

### Line-by-Line Breakdown

- `request.remote_addr` identifies users by IP address
- `current_user.id` identifies authenticated users
- Combining both provides defense in depth
- Role-based limits reward trusted users

## Common Mistakes

- ❌ Not rate limiting authentication endpoints
- ✅ Always limit login/register/password reset

- ❌ Same limits for all users
- ✅ Use role-based limits

- ❌ Not using per-IP for anonymous users
- ✅ Fall back to IP when not authenticated

## Quick Reference

| Endpoint Type | Recommended Limit |
|---------------|-------------------|
| Login | 5/minute |
| Password Reset | 3/hour |
| Registration | 3/hour |
| General API | 60/minute |
| Search | 10/minute |
| File Upload | 10/hour |

## Next Steps

Continue to [03_security_headers/01_what_are_security_headers.md](../03_security_headers/01_what_are_security_headers.md) to learn about HTTP security headers.
