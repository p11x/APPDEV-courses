<!-- FILE: 18_rate_limiting_and_security/05_https_and_tls/03_forcing_https_in_flask.md -->

## Overview

Enforce HTTPS in Flask applications using Flask-Talisman and configuration settings.

## Prerequisites

- SSL certificate installed
- Understanding of HTTPS

## Core Concepts

Forcing HTTPS ensures all traffic uses encrypted connections. This is critical for protecting sensitive data like passwords and session tokens.

## Code Walkthrough

### Method 1: Flask-Talisman

```bash
pip install flask-talisman
```

```python
# app.py
from flask import Flask
from flask_talisman import Talisman

app = Flask(__name__)

# Force HTTPS on all routes
talisman = Talisman(
    app,
    force_https=True,  # Redirect HTTP to HTTPS
    force_https_permanent=True,  # Use 301 (permanent) instead of 302
)

@app.route('/')
def index():
    return 'Welcome!'
```

### Method 2: Manual HTTPS Enforcement

```python
# app.py
from flask import Flask, request, redirect

app = Flask(__name__)

@app.before_request
def require_https():
    """Redirect all HTTP requests to HTTPS."""
    if not request.is_secure:
        url = request.url.replace('http://', 'https://', 1)
        return redirect(url, code=301)
```

### Method 3: HSTS Header (Recommended)

```python
# app.py
from flask import Flask

app = Flask(__name__)

@app.after_request
def add_hsts_header(response):
    """Add HSTS header to force HTTPS in browser."""
    response.headers['Strict-Transport-Security'] = (
        'max-age=31536000; includeSubDomains; preload'
    )
    return response
```

### Method 4: Combined Approach

```python
# app.py - Production-ready configuration
from flask import Flask, request, redirect
from flask_talisman import Talisman

app = Flask(__name__)

# Talisman handles most HTTPS enforcement
talisman = Talisman(
    app,
    force_https=True,
    strict_transport_security='max-age=31536000; includeSubDomains; preload',
    x_frame_options='DENY',
)

# Additional security headers
@app.after_request
def add_security_headers(response):
    # Content Security Policy
    response.headers['Content-Security-Policy'] = (
        "default-src 'self'; "
        "script-src 'self'; "
        "style-src 'self' 'unsafe-inline'"
    )
    
    # Prevent clickjacking
    response.headers['X-Frame-Options'] = 'DENY'
    
    # Prevent MIME sniffing
    response.headers['X-Content-Type-Options'] = 'nosniff'
    
    return response
```

### Production Configuration

```python
# config.py
import os

class ProductionConfig:
    """Production configuration with HTTPS enforcement."""
    
    # Force HTTPS in production
    SESSION_COOKIE_SECURE = True  # Cookie only sent over HTTPS
    SESSION_COOKIE_HTTPONLY = True  # No JavaScript access
    SESSION_COOKIE_SAMESITE = 'Lax'  # CSRF protection
    
    # HSTS - browser remembers to use HTTPS
    # This header makes browsers automatically use HTTPS for future visits
    HSTS_MAX_AGE = 31536000  # 1 year

# app.py
app.config.from_object('config.ProductionConfig')
```

### Testing HTTPS Enforcement

```bash
# Test HTTP redirect
curl -I http://yourdomain.com
# Should return: HTTP/1.1 301 Moved Permanently
# Location: https://yourdomain.com/

# Test HSTS header
curl -I https://yourdomain.com
# Should include: Strict-Transport-Security: max-age=31536000

# Test certificate
curl -I -k https://yourdomain.com
# Should show SSL certificate info
```

> **🔒 Security Note:** Always test HTTPS enforcement before deploying. Make sure all static assets load over HTTPS.

## Common Mistakes

- ❌ Not setting SESSION_COOKIE_SECURE
- ✅ Set to True in production

- ❌ HSTS max-age too short initially
- ✅ Start with short duration, increase over time

- ❌ Forgetting mixed content warnings
- ✅ Ensure all resources load over HTTPS

## Quick Reference

| Setting | Purpose |
|---------|---------|
| `force_https=True` | Redirect HTTP to HTTPS |
| `SESSION_COOKIE_SECURE` | Cookie only over HTTPS |
| `Strict-Transport-Security` | HSTS header |
| `301` vs `302` | Permanent vs temporary redirect |

## Next Steps

This completes the Flask Web Application Development Guide. You now have comprehensive knowledge of:

- Flask fundamentals and routing
- Templates and forms
- Databases and migrations
- Authentication and authorization
- REST APIs
- Testing and deployment
- Async and background tasks
- Caching and performance
- WebSockets and real-time features
- Email and notifications
- Admin panels
- GraphQL APIs
- Cloud storage
- Security best practices

Congratulations on completing the guide!
