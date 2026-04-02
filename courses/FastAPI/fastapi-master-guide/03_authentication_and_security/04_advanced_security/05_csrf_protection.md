# CSRF Protection

## Overview

Cross-Site Request Forgery (CSRF) tricks authenticated users into making unwanted requests. FastAPI applications need CSRF protection for cookie-based authentication.

## Understanding CSRF

### CSRF Attack Example

```python
# Example 1: Understanding CSRF
"""
CSRF Attack Flow:

1. User logs into bank.com (session cookie set)
2. User visits malicious-site.com
3. Malicious site makes request to bank.com/transfer
4. Browser sends session cookie automatically
5. Bank processes transfer (thinking it's legitimate)

Prevention: Require CSRF token that attacker cannot know
"""

from fastapi import FastAPI, Request, HTTPException
from fastapi.responses import JSONResponse
import secrets

app = FastAPI()

# CSRF token storage (use Redis in production)
csrf_tokens: dict[str, str] = {}
```

## CSRF Token Implementation

### Token Generation and Validation

```python
# Example 2: CSRF token implementation
from fastapi import FastAPI, Request, HTTPException, Depends, Cookie
from fastapi.responses import JSONResponse
import secrets
import hmac
import hashlib

app = FastAPI()

SECRET_KEY = "your-csrf-secret-key"

def generate_csrf_token(session_id: str) -> str:
    """
    Generate CSRF token bound to session.
    """
    # Create HMAC-based token
    token = secrets.token_urlsafe(32)
    signature = hmac.new(
        SECRET_KEY.encode(),
        f"{session_id}:{token}".encode(),
        hashlib.sha256
    ).hexdigest()

    return f"{token}.{signature}"

def verify_csrf_token(session_id: str, token: str) -> bool:
    """
    Verify CSRF token is valid.
    """
    try:
        token_value, signature = token.split(".")

        expected_signature = hmac.new(
            SECRET_KEY.encode(),
            f"{session_id}:{token_value}".encode(),
            hashlib.sha256
        ).hexdigest()

        return hmac.compare_digest(signature, expected_signature)
    except ValueError:
        return False

@app.get("/csrf-token")
async def get_csrf_token(request: Request):
    """
    Get CSRF token for form submission.
    """
    session_id = request.cookies.get("session_id", "anonymous")
    csrf_token = generate_csrf_token(session_id)

    return {
        "csrf_token": csrf_token,
        "header_name": "X-CSRF-Token"
    }

async def verify_csrf(request: Request):
    """
    Dependency to verify CSRF token.
    """
    session_id = request.cookies.get("session_id", "anonymous")
    csrf_token = request.headers.get("X-CSRF-Token")

    if not csrf_token:
        raise HTTPException(403, "CSRF token missing")

    if not verify_csrf_token(session_id, csrf_token):
        raise HTTPException(403, "Invalid CSRF token")

@app.post("/transfer", dependencies=[Depends(verify_csrf)])
async def transfer_money(amount: float, to_account: str):
    """
    Protected endpoint requiring CSRF token.
    """
    return {"transferred": amount, "to": to_account}
```

### Double Submit Cookie

```python
# Example 3: Double submit cookie pattern
from fastapi import FastAPI, Request, HTTPException
from fastapi.responses import JSONResponse
import secrets

app = FastAPI()

@app.middleware("http")
async def csrf_double_submit(request: Request, call_next):
    """
    Double submit cookie CSRF protection.
    """
    # Skip for safe methods
    if request.method in ["GET", "HEAD", "OPTIONS"]:
        return await call_next(request)

    # Get cookie token
    cookie_token = request.cookies.get("csrf_token")

    # Get header token
    header_token = request.headers.get("X-CSRF-Token")

    if not cookie_token or not header_token:
        return JSONResponse(
            status_code=403,
            content={"error": "CSRF token required"}
        )

    if cookie_token != header_token:
        return JSONResponse(
            status_code=403,
            content={"error": "CSRF token mismatch"}
        )

    return await call_next(request)

@app.get("/get-token")
async def get_token():
    """Issue CSRF token"""
    token = secrets.token_urlsafe(32)

    response = JSONResponse(content={"message": "Token set"})
    response.set_cookie(
        key="csrf_token",
        value=token,
        httponly=False,  # JavaScript needs to read this
        samesite="strict",
        secure=True
    )

    return response
```

## SameSite Cookies

### Cookie Configuration

```python
# Example 4: SameSite cookie protection
from fastapi import FastAPI, Response

app = FastAPI()

@app.post("/login")
async def login(response: Response, username: str, password: str):
    """
    Set session cookie with SameSite protection.
    """
    # Create session
    session_id = create_session(username)

    response.set_cookie(
        key="session_id",
        value=session_id,
        httponly=True,      # Not accessible via JavaScript
        secure=True,        # Only sent over HTTPS
        samesite="strict",  # Not sent on cross-site requests
        max_age=3600
    )

    # Also set CSRF token (readable by JavaScript)
    csrf_token = generate_csrf_token(session_id)
    response.set_cookie(
        key="csrf_token",
        value=csrf_token,
        httponly=False,     # JavaScript needs to read this
        secure=True,
        samesite="strict"
    )

    return {"message": "Logged in"}

# SameSite values:
# - "strict": Never sent on cross-site requests (most secure)
# - "lax": Sent on top-level navigations (good balance)
# - "none": Always sent (requires Secure=True)
```

## FastAPI CORS + CSRF

### Combined Protection

```python
# Example 5: CORS and CSRF together
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from fastapi.middleware.httpsredirect import HTTPSRedirectMiddleware

app = FastAPI()

# HTTPS redirect (production)
# app.add_middleware(HTTPSRedirectMiddleware)

# CORS configuration
app.add_middleware(
    CORSMiddleware,
    allow_origins=["https://yourdomain.com"],
    allow_credentials=True,  # Required for cookies
    allow_methods=["GET", "POST", "PUT", "DELETE"],
    allow_headers=["Authorization", "Content-Type", "X-CSRF-Token"],
)

# CSRF protection middleware
@app.middleware("http")
async def csrf_protection(request, call_next):
    """Combined CORS + CSRF protection"""
    response = await call_next(request)
    return response
```

## Best Practices

### CSRF Prevention Guidelines

```python
# Example 6: CSRF best practices
"""
CSRF Prevention Best Practices:

1. Use SameSite cookies (strict or lax)
2. Implement CSRF tokens for state-changing requests
3. Verify Origin/Referer headers
4. Use custom request headers (for AJAX)
5. Require re-authentication for sensitive operations
6. Use double-submit cookie pattern
"""

from fastapi import FastAPI, Request, HTTPException

app = FastAPI()

# 1. Verify Origin header
@app.middleware("http")
async def verify_origin(request: Request, call_next):
    """Verify request origin"""
    if request.method not in ["GET", "HEAD", "OPTIONS"]:
        origin = request.headers.get("origin")
        referer = request.headers.get("referer")

        allowed = ["https://yourdomain.com"]

        if origin and origin not in allowed:
            return JSONResponse(
                status_code=403,
                content={"error": "Invalid origin"}
            )

    return await call_next(request)

# 2. Require custom header for AJAX
@app.post("/api/action")
async def api_action(request: Request):
    """
    AJAX requests should include custom header.
    Browsers cannot send custom headers cross-origin without CORS.
    """
    if "X-Requested-With" not in request.headers:
        raise HTTPException(403, "Missing required header")

    return {"success": True}
```

## Summary

| Protection | Method | Use Case |
|------------|--------|----------|
| CSRF Token | Generate/verify token | Form submissions |
| SameSite Cookie | Cookie attribute | All cookies |
| Double Submit | Cookie + header match | AJAX requests |
| Origin Check | Header verification | API endpoints |

## Next Steps

Continue learning about:
- [File Upload Security](./06_file_upload_security.md) - Safe file handling
- [API Security Headers](./07_api_security_headers.md) - Security headers
- [Security Testing](../07_security_testing/01_authentication_testing.md) - Testing security
