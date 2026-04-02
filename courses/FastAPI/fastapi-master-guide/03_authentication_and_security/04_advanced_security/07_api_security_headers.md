# API Security Headers

## Overview

Security headers protect against common web vulnerabilities. This guide covers essential headers for FastAPI APIs.

## Essential Security Headers

### Security Headers Middleware

```python
# Example 1: Complete security headers
from fastapi import FastAPI, Request
from fastapi.responses import Response

app = FastAPI()

@app.middleware("http")
async def security_headers(request: Request, call_next):
    """
    Add comprehensive security headers to all responses.
    """
    response = await call_next(request)

    # Prevent MIME type sniffing
    response.headers["X-Content-Type-Options"] = "nosniff"

    # Prevent clickjacking
    response.headers["X-Frame-Options"] = "DENY"

    # XSS protection (legacy browsers)
    response.headers["X-XSS-Protection"] = "1; mode=block"

    # Content Security Policy
    response.headers["Content-Security-Policy"] = (
        "default-src 'self'; "
        "script-src 'self'; "
        "style-src 'self' 'unsafe-inline'; "
        "img-src 'self' data: https:; "
        "font-src 'self'; "
        "connect-src 'self'; "
        "frame-ancestors 'none'; "
        "form-action 'self'; "
        "base-uri 'self';"
    )

    # Strict Transport Security (HTTPS only)
    response.headers["Strict-Transport-Security"] = (
        "max-age=31536000; includeSubDomains; preload"
    )

    # Referrer Policy
    response.headers["Referrer-Policy"] = "strict-origin-when-cross-origin"

    # Permissions Policy (Feature Policy)
    response.headers["Permissions-Policy"] = (
        "camera=(), "
        "microphone=(), "
        "geolocation=(), "
        "payment=()"
    )

    # Cache control for sensitive data
    if request.url.path.startswith("/api/"):
        response.headers["Cache-Control"] = "no-store, no-cache, must-revalidate"
        response.headers["Pragma"] = "no-cache"

    return response

@app.get("/api/data")
async def get_data():
    """API endpoint with security headers"""
    return {"data": "sensitive"}
```

### Header Descriptions

```python
# Example 2: Security header explanations
"""
Security Headers Explained:

1. X-Content-Type-Options: nosniff
   - Prevents browser from MIME-sniffing
   - Blocks style/script if Content-Type doesn't match

2. X-Frame-Options: DENY
   - Prevents page from being embedded in iframes
   - Stops clickjacking attacks

3. X-XSS-Protection: 1; mode=block
   - Enables browser XSS filter
   - Blocks page if XSS detected

4. Content-Security-Policy
   - Controls resource loading
   - Prevents inline scripts
   - Restricts script sources

5. Strict-Transport-Security
   - Forces HTTPS connections
   - Prevents protocol downgrade attacks

6. Referrer-Policy
   - Controls referrer information
   - Prevents information leakage

7. Permissions-Policy
   - Restricts browser features
   - Camera, microphone, geolocation
"""

from fastapi import FastAPI

app = FastAPI()

# Headers vary by endpoint type
def get_headers_for_endpoint(endpoint_type: str) -> dict:
    """Get appropriate headers for endpoint type"""

    common = {
        "X-Content-Type-Options": "nosniff",
        "X-Frame-Options": "DENY"
    }

    if endpoint_type == "api":
        return {
            **common,
            "Cache-Control": "no-store",
            "Content-Type": "application/json"
        }

    if endpoint_type == "static":
        return {
            **common,
            "Cache-Control": "public, max-age=31536000"
        }

    return common
```

## CORS Configuration

### CORS Security

```python
# Example 3: Secure CORS configuration
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

# Production CORS configuration
app.add_middleware(
    CORSMiddleware,
    # Specific origins only
    allow_origins=[
        "https://yourdomain.com",
        "https://app.yourdomain.com"
    ],

    # Allow credentials (cookies)
    allow_credentials=True,

    # Specific methods
    allow_methods=["GET", "POST", "PUT", "DELETE", "PATCH"],

    # Specific headers
    allow_headers=[
        "Authorization",
        "Content-Type",
        "X-Requested-With",
        "X-CSRF-Token"
    ],

    # Headers exposed to client
    expose_headers=[
        "X-Total-Count",
        "X-RateLimit-Remaining"
    ],

    # Cache preflight for 1 hour
    max_age=3600
)

# NEVER do this in production:
# allow_origins=["*"]  # Allows any origin!
```

## Security Header Testing

### Testing Headers

```python
# Example 4: Test security headers
from fastapi import FastAPI
from fastapi.testclient import TestClient

app = FastAPI()
client = TestClient(app)

def test_security_headers():
    """Verify all security headers are present"""
    response = client.get("/api/data")

    headers = response.headers

    assert headers.get("X-Content-Type-Options") == "nosniff"
    assert headers.get("X-Frame-Options") == "DENY"
    assert "Content-Security-Policy" in headers
    assert "Strict-Transport-Security" in headers

def test_cors_headers():
    """Verify CORS is properly configured"""
    response = client.options(
        "/api/data",
        headers={
            "Origin": "https://malicious.com",
            "Access-Control-Request-Method": "POST"
        }
    )

    # Should not allow malicious origin
    assert response.headers.get("Access-Control-Allow-Origin") != "https://malicious.com"
```

## Best Practices

### Security Headers Checklist

```python
# Example 5: Security headers checklist
"""
Security Headers Checklist:

✓ X-Content-Type-Options: nosniff
✓ X-Frame-Options: DENY (or SAMEORIGIN)
✓ Content-Security-Policy (restrictive)
✓ Strict-Transport-Security (HTTPS)
✓ Referrer-Policy
✓ Permissions-Policy
✓ Cache-Control (for sensitive data)
✓ CORS configured properly
"""

from fastapi import FastAPI

app = FastAPI()

# Implement all headers via middleware (see Example 1)
```

## Summary

| Header | Purpose | Recommended Value |
|--------|---------|-------------------|
| X-Content-Type-Options | Prevent MIME sniff | `nosniff` |
| X-Frame-Options | Prevent clickjacking | `DENY` |
| CSP | Control resources | Restrictive policy |
| HSTS | Force HTTPS | `max-age=31536000` |
| Referrer-Policy | Control referrer | `strict-origin-when-cross-origin` |
| Permissions-Policy | Restrict features | Disable unused features |

## Next Steps

Continue learning about:
- [Security Testing](../07_security_testing/01_authentication_testing.md) - Testing security
- [Security Audit](../07_security_testing/06_security_audit_practices.md) - Audit practices
- [CORS Middleware](../../02_core_features/06_middleware/02_cors_middleware.md) - CORS details
