# XSS Protection

## Overview

Cross-Site Scripting (XSS) allows attackers to inject malicious scripts into web pages. FastAPI applications must protect against XSS through output encoding, input validation, and security headers.

## Types of XSS

### Understanding XSS Attacks

```python
# Example 1: XSS vulnerability types
"""
XSS Attack Types:

1. Reflected XSS
   - Malicious script in URL parameter
   - Example: /search?q=<script>alert('xss')</script>

2. Stored XSS
   - Malicious script saved in database
   - Displayed to other users

3. DOM-based XSS
   - Client-side JavaScript manipulation
   - No server involvement
"""

from fastapi import FastAPI, Query, HTTPException
from pydantic import BaseModel, validator
import html

app = FastAPI()

# VULNERABLE ENDPOINT - DO NOT USE
# @app.get("/search")
# async def search(q: str = Query(...)):
#     # Returns unsanitized input - VULNERABLE!
#     return {"message": f"Results for: {q}"}

# SAFE ENDPOINT - Escape output
@app.get("/search")
async def search(q: str = Query(...)):
    """
    Safe search - HTML entities escaped.
    """
    safe_q = html.escape(q)
    return {"message": f"Results for: {safe_q}"}
```

## Output Encoding

### HTML Escaping

```python
# Example 2: Output encoding
from fastapi import FastAPI
from fastapi.responses import HTMLResponse
import html

app = FastAPI()

def escape_html(text: str) -> str:
    """
    Escape HTML special characters.
    Prevents XSS in HTML output.
    """
    return html.escape(text)

@app.get("/page/{name}", response_class=HTMLResponse)
async def get_page(name: str):
    """
    Return HTML page with escaped user input.
    """
    safe_name = escape_html(name)

    return f"""
    <html>
        <body>
            <h1>Welcome, {safe_name}!</h1>
            <!-- User input is safely escaped -->
        </body>
    </html>
    """

@app.get("/api/user/{name}")
async def get_user(name: str):
    """
    JSON API - automatic encoding in JSON.
    JSON is inherently safer than HTML.
    """
    return {"name": name}  # Safe - JSON encoding prevents XSS
```

### JSON Response Safety

```python
# Example 3: JSON response safety
from fastapi import FastAPI
from fastapi.responses import JSONResponse
import json

app = FastAPI()

@app.get("/user/{username}")
async def get_user(username: str):
    """
    JSON responses are generally safe from XSS.
    FastAPI automatically handles JSON encoding.
    """
    return {
        "username": username,
        "message": f"Hello, {username}!"
    }

# Custom JSON response with safe encoding
@app.get("/custom/{data}")
async def custom_response(data: str):
    """Custom JSON with explicit encoding"""
    return JSONResponse(
        content={"data": data},
        media_type="application/json"
    )

# Content-Type header prevents browser misinterpretation
@app.middleware("http")
async def set_content_type(request, call_next):
    """Ensure correct Content-Type"""
    response = await call_next(request)

    if not response.headers.get("content-type"):
        response.headers["content-type"] = "application/json"

    return response
```

## Input Validation

### Sanitizing User Input

```python
# Example 4: Input sanitization
from fastapi import FastAPI
from pydantic import BaseModel, validator, Field
import re
import bleach

app = FastAPI()

class UserInput(BaseModel):
    """User input with sanitization"""
    username: str = Field(..., min_length=3, max_length=50)
    bio: str = Field("", max_length=500)

    @validator('username')
    def sanitize_username(cls, v):
        """Remove dangerous characters from username"""
        # Allow only alphanumeric and underscore
        if not re.match(r'^[a-zA-Z0-9_]+$', v):
            raise ValueError('Username contains invalid characters')
        return v

    @validator('bio')
    def sanitize_bio(cls, v):
        """Remove HTML tags from bio"""
        # Strip all HTML tags
        return bleach.clean(v, tags=[], strip=True)

class CommentInput(BaseModel):
    """Comment with safe HTML"""
    content: str = Field(..., max_length=1000)

    @validator('content')
    def sanitize_content(cls, v):
        """Allow only safe HTML tags"""
        allowed_tags = ['b', 'i', 'u', 'em', 'strong', 'p', 'br']
        return bleach.clean(v, tags=allowed_tags, strip=True)

@app.post("/users/")
async def create_user(user: UserInput):
    """Create user with sanitized input"""
    return {
        "username": user.username,
        "bio": user.bio
    }

@app.post("/comments/")
async def create_comment(comment: CommentInput):
    """Create comment with safe HTML"""
    return {"content": comment.content}
```

## Security Headers

### XSS Protection Headers

```python
# Example 5: Security headers for XSS protection
from fastapi import FastAPI, Request
from fastapi.responses import Response

app = FastAPI()

@app.middleware("http")
async def xss_protection_headers(request: Request, call_next):
    """
    Add security headers to prevent XSS.
    """
    response = await call_next(request)

    # X-XSS-Protection - Enable browser XSS filter
    response.headers["X-XSS-Protection"] = "1; mode=block"

    # X-Content-Type-Options - Prevent MIME sniffing
    response.headers["X-Content-Type-Options"] = "nosniff"

    # Content-Security-Policy - Restrict script sources
    response.headers["Content-Security-Policy"] = (
        "default-src 'self'; "
        "script-src 'self' 'unsafe-inline' 'unsafe-eval'; "
        "style-src 'self' 'unsafe-inline'; "
        "img-src 'self' data: https:; "
        "font-src 'self'; "
        "connect-src 'self'; "
        "frame-ancestors 'none';"
    )

    # X-Frame-Options - Prevent clickjacking
    response.headers["X-Frame-Options"] = "DENY"

    # Referrer-Policy - Control referrer information
    response.headers["Referrer-Policy"] = "strict-origin-when-cross-origin"

    # Permissions-Policy - Restrict browser features
    response.headers["Permissions-Policy"] = (
        "camera=(), "
        "microphone=(), "
        "geolocation=()"
    )

    return response

@app.get("/items/")
async def list_items():
    return {"items": []}
```

## Template Security

### Safe Template Rendering

```python
# Example 6: Safe template rendering
from fastapi import FastAPI, Request
from fastapi.responses import HTMLResponse
from fastapi.templating import Jinja2Templates
import html

app = FastAPI()

# Jinja2 auto-escapes by default (safe!)
templates = Jinja2Templates(directory="templates")

@app.get("/page/{name}", response_class=HTMLResponse)
async def get_page(request: Request, name: str):
    """
    Jinja2 auto-escapes variables.
    {{ name }} is automatically HTML-escaped.
    """
    return templates.TemplateResponse(
        "page.html",
        {"request": request, "name": name}
    )

# template.html:
"""
<!DOCTYPE html>
<html>
<body>
    <!-- Auto-escaped - SAFE -->
    <h1>Hello, {{ name }}!</h1>

    <!-- Mark as safe only if you trust the source -->
    <!-- <div>{{ trusted_html | safe }}</div> -->
</body>
</html>
"""
```

## API Security

### Preventing XSS in APIs

```python
# Example 7: API XSS prevention
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, validator
from typing import Any

app = FastAPI()

class ApiResponse(BaseModel):
    """Safe API response model"""
    success: bool
    data: Any
    message: str

    @validator('message')
    def sanitize_message(cls, v):
        """Ensure message is safe"""
        return html.escape(str(v))

@app.get("/api/items/{item_id}")
async def get_item(item_id: int):
    """
    Safe API response.
    JSON responses don't execute as scripts in browsers.
    """
    return ApiResponse(
        success=True,
        data={"id": item_id, "name": "Item"},
        message="Item retrieved successfully"
    )

# Content-Type enforcement
@app.middleware("http")
async def enforce_json_content_type(request, call_next):
    """Force JSON content type for API responses"""
    response = await call_next(request)

    if request.url.path.startswith("/api/"):
        response.headers["content-type"] = "application/json; charset=utf-8"

    return response
```

## Best Practices

### XSS Prevention Checklist

```python
# Example 8: XSS prevention best practices
"""
XSS Prevention Checklist:

1. ✓ Escape all output (HTML, JavaScript, CSS)
2. ✓ Validate and sanitize all input
3. ✓ Use Content-Security-Policy headers
4. ✓ Set X-XSS-Protection header
5. ✓ Use JSON responses (safer than HTML)
6. ✓ Enable auto-escaping in templates
7. ✓ Avoid innerHTML with user data
8. ✓ Use HttpOnly cookies
9. ✓ Implement Content-Type enforcement
10. ✓ Regular security audits
"""

from fastapi import FastAPI

app = FastAPI()

# 1. Always escape output
# 2. Validate input with Pydantic
# 3. Security headers (middleware)
# 4. JSON over HTML when possible
# 5. Jinja2 auto-escaping
```

## Summary

| Protection | Implementation | Effectiveness |
|------------|----------------|---------------|
| HTML escaping | `html.escape()` | High |
| Input validation | Pydantic validators | High |
| Security headers | CSP, X-XSS-Protection | High |
| JSON responses | FastAPI default | High |
| Template auto-escape | Jinja2 | High |

## Next Steps

Continue learning about:
- [CSRF Protection](./05_csrf_protection.md) - Cross-site request forgery
- [File Upload Security](./06_file_upload_security.md) - Safe file handling
- [API Security Headers](./07_api_security_headers.md) - Security headers
