# CORS and CSRF Protection

## What You'll Learn
- Cross-Origin Resource Sharing (CORS)
- CSRF (Cross-Site Request Forgery) protection
- Implementing security headers
- Best practices for web security

## Prerequisites
- Understanding of HTTP
- Basic web development knowledge

## CORS Overview

CORS controls which domains can access your API:

```
Without CORS:
  evil.com ──✗──▶ api.example.com (blocked!)

With CORS:
  myapp.com ──✓──▶ api.example.com (allowed!)
```

## FastAPI CORS

```python
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

# Configure CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["https://myapp.com"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)
```

## CSRF Protection

CSRF prevents attackers from making authenticated requests:

```python
from fastapi import FastAPI, Request
from fastapi.security import CSRFToken
from fastapi.middleware.csrf import CSRFMiddleware

app = FastAPI()

# Generate CSRF token
@app.get("/csrf-token")
async def get_csrf_token(request: Request):
    csrf = CSRFToken(request)
    return {"csrf_token": csrf}

# Verify on POST
@app.post("/submit")
async def submit(data: dict, request: Request):
    csrf = CSRFToken(request)
    csrf.verify()  # Raises error if invalid
    return {"status": "success"}
```

## Security Headers

```python
from fastapi.middleware.gzip import GZipMiddleware

@app.middleware("http")
async def security_headers(request, call_next):
    response = await call_next(request)
    
    response.headers["X-Content-Type-Options"] = "nosniff"
    response.headers["X-Frame-Options"] = "DENY"
    response.headers["X-XSS-Protection"] = "1; mode=block"
    response.headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains"
    
    return response
```

## Summary

- CORS controls cross-origin access to your API
- CSRF prevents forged requests on authenticated users
- Use security headers to protect against common attacks
- Always validate origins in production
