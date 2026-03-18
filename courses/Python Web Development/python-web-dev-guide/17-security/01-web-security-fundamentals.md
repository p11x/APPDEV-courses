# Web Security Fundamentals

## What You'll Learn
- Understanding common web vulnerabilities
- Security best practices for Python web apps
- HTTPS and SSL/TLS
- Security headers

## Prerequisites
- Completed task queues folder

## Common Web Vulnerabilities

| Vulnerability | Description | Prevention |
|--------------|-------------|------------|
| XSS | Cross-site scripting | Escape output |
| CSRF | Cross-site request forgery | Use CSRF tokens |
| SQL Injection | Database injection | Use parameterized queries |
| Password Leaks | Exposed passwords | Hash passwords |
| XXE | XML external entity | Disable XML parsing |

## Security Headers

```python
from fastapi import FastAPI
from fastapi.responses import Response

app = FastAPI()

@app.middleware("http")
async def add_security_headers(response: Response):
    """Add security headers to all responses"""
    response.headers["X-Content-Type-Options"] = "nosniff"
    response.headers["X-Frame-Options"] = "DENY"
    response.headers["X-XSS-Protection"] = "1; mode=block"
    response.headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains"
    response.headers["Content-Security-Policy"] = "default-src 'self'"
    return response
```

🔍 **Line-by-Line Breakdown:**
1. `X-Content-Type-Options: nosniff` — Prevents browser from guessing content type
2. `X-Frame-Options: DENY` — Prevents clickjacking (embedding in iframe)
3. `X-XSS-Protection` — Enables browser's XSS filter
4. `Strict-Transport-Security` — Forces HTTPS connections
5. `Content-Security-Policy` — Controls resource loading

## HTTPS Setup

```bash
# Using Let's Encrypt (production)
pip install certbot
certbot certonly --webroot -w /var/www/html -d example.com
```

```python
# FastAPI with HTTPS
import uvicorn

# Run with SSL
uvicorn.run(
    "main:app",
    host="0.0.0.0",
    port=443,
    ssl_keyfile="/path/to/privkey.pem",
    ssl_certfile="/path/to/fullchain.pem"
)
```

## Input Validation

```python
from pydantic import BaseModel, validator
from fastapi import FastAPI, HTTPException

app = FastAPI()

class UserCreate(BaseModel):
    username: str
    email: str
    password: str
    
    @validator('username')
    def validate_username(cls, v):
        if len(v) < 3:
            raise ValueError('Username must be at least 3 characters')
        if not v.isalnum():
            raise ValueError('Username must be alphanumeric')
        return v.lower()
    
    @validator('password')
    def validate_password(cls, v):
        if len(v) < 8:
            raise ValueError('Password must be at least 8 characters')
        return v

@app.post("/users")
async def create_user(user: UserCreate):
    return {"username": user.username, "email": user.email}
```

## Rate Limiting

```python
from fastapi import FastAPI, Request
from fastapi.middlewareiddleware import Middleware
import time
from collections import defaultdict

app = FastAPI()

# Simple rate limiter
request_counts: dict = defaultdict(list)

@app.middleware("http")
async def rate_limit_middleware(request: Request, call_next):
    client_ip = request.client.host
    now = time.time()
    
    # Clean old requests
    request_counts[client_ip] = [
        t for t in request_counts[client_ip] if now - t < 60
    ]
    
    # Check limit (100 requests per minute)
    if len(request_counts[client_ip]) >= 100:
        return Response(
            content="Rate limit exceeded",
            status_code=429
        )
    
    request_counts[client_ip].append(now)
    return await call_next(request)
```

## Summary
- Always use HTTPS in production
- Add security headers to responses
- Validate and sanitize all input
- Implement rate limiting
- Use parameterized queries for database

## Next Steps
→ Continue to `02-xss-and-csrf-protection.md`
