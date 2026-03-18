# API Security Headers

## What You'll Learn

- Important security headers
- Implementing headers in Flask/FastAPI
- Content Security Policy

## Prerequisites

- Completed `04-data-encryption-at-rest.md`

## Important Headers

| Header | Purpose |
|--------|---------|
| HSTS | Force HTTPS |
| X-Content-Type-Options | Prevent MIME sniffing |
| X-Frame-Options | Prevent clickjacking |
| X-XSS-Protection | XSS filter |
| Content-Security-Policy | Prevent XSS/injection |
| Referrer-Policy | Control referrer info |

## Flask Implementation

```python
from flask import Flask, make_response

app = Flask(__name__)

@app.after_request
def add_security_headers(response: make_response) -> make_response:
    """Add security headers to all responses."""
    
    # HSTS - Force HTTPS for 1 year
    response.headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains"
    
    # Prevent MIME sniffing
    response.headers["X-Content-Type-Options"] = "nosniff"
    
    # Prevent clickjacking
    response.headers["X-Frame-Options"] = "DENY"
    
    # XSS Protection
    response.headers["X-XSS-Protection"] = "1; mode=block"
    
    # Content Security Policy
    response.headers["Content-Security-Policy"] = (
        "default-src 'self'; "
        "script-src 'self' 'unsafe-inline'; "
        "style-src 'self' 'unsafe-inline'; "
        "img-src 'self' data: https:; "
        "font-src 'self';"
    )
    
    # Referrer Policy
    response.headers["Referrer-Policy"] = "strict-origin-when-cross-origin"
    
    return response
```

## FastAPI Implementation

```python
from fastapi import FastAPI, Request
from starlette.middleware.base import BaseHTTPMiddleware
from starlette.responses import Response

app = FastAPI()

class SecurityHeadersMiddleware(BaseHTTPMiddleware):
    async def dispatch(self, request: Request, call_next):
        response = await call_next(request)
        
        response.headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains"
        response.headers["X-Content-Type-Options"] = "nosniff"
        response.headers["X-Frame-Options"] = "DENY"
        response.headers["X-XSS-Protection"] = "1; mode=block"
        response.headers["Referrer-Policy"] = "strict-origin-when-cross-origin"
        
        return response

app.add_middleware(SecurityHeadersMiddleware)
```

## Content Security Policy

```python
# CSP for different scenarios
CSP_DEFAULT = "default-src 'self'"

# Allow inline scripts (not recommended)
CSP_ALLOW_INLINE = "default-src 'self' 'unsafe-inline'"

# Strict CSP
CSP_STRICT = (
    "default-src 'none'; "
    "script-src 'self'; "
    "connect-src 'self'; "
    "img-src 'self' https:; "
    "style-src 'self'; "
    "font-src 'self';"
)

@app.after_request
def add_csp(response):
    response.headers["Content-Security-Policy"] = CSP_STRICT
    return response
```

## Using Talisman

```python
# Using flask-talisman for automatic security headers
from flask_talisman import Talisman

app = Flask(__name__)

Talisman(
    app,
    content_security_policy={
        "default-src": "'self'",
        "script-src": "'self' 'unsafe-inline'",
    }
)
```

## Summary

- Add security headers to protect against common attacks
- Use HSTS to enforce HTTPS
- Implement CSP to prevent XSS

## Next Steps

Continue to `06-secure-api-design.md`.
