# Secure API Design

## What You'll Learn

- API security best practices
- Input validation
- Rate limiting

## Prerequisites

- Completed `05-api-security-headers.md`

## Input Validation

```python
from pydantic import BaseModel, validator, Field
from typing import Optional

class UserCreate(BaseModel):
    username: str = Field(..., min_length=3, max_length=50)
    email: str = Field(..., regex=r"^[\w.-]+@[\w.-]+\.\w+$")
    password: str = Field(..., min_length=8)
    
    @validator("username")
    def username_alphanumeric(cls, v):
        if not v.isalnum():
            raise ValueError("Username must be alphanumeric")
        return v
    
    @validator("password")
    def password_strength(cls, v):
        if not any(c.isupper() for c in v):
            raise ValueError("Password must contain uppercase")
        if not any(c.islower() for c in v):
            raise ValueError("Password must contain lowercase")
        if not any(c.isdigit() for c in v):
            raise ValueError("Password must contain digit")
        return v
```

## SQL Injection Prevention

```python
# BAD - vulnerable to SQL injection
query = f"SELECT * FROM users WHERE email = '{email}'"

# GOOD - use parameterized queries
query = "SELECT * FROM users WHERE email = ?"
cursor.execute(query, (email,))

# Using SQLAlchemy (recommended)
user = session.query(User).filter(User.email == email).first()
```

## Command Injection Prevention

```python
import subprocess
import shlex

# BAD - vulnerable to command injection
command = f"ls {user_input}"
os.system(command)

# GOOD - use shell=False
command = ["ls", user_input]
subprocess.run(command, shell=False)

# If shell=True is needed, use shlex.quote
safe_input = shlex.quote(user_input)
command = f"ls {safe_input}"
```

## Rate Limiting

```python
from fastapi import FastAPI, Request
from fastapi.middleware.limiter import Limiter
from fastapi.responses import JSONResponse

app = FastAPI()
limiter = Limiter(app=app)

@app.post("/api/login")
@limiter.limit("5/minute")
async def login(request: Request):
    # Rate limited endpoint
    return {"message": "Login attempt"}
```

## Request Size Limits

```python
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

app = FastAPI()

@app.middleware("http")
async def limit_request_size(request: Request, call_next):
    # Limit request body to 1MB
    content_length = request.headers.get("content-length")
    if content_length and int(content_length) > 1_000_000:
        return JSONResponse(
            status_code=413,
            content={"error": "Request too large"}
        )
    
    response = await call_next(request)
    return response
```

## Error Handling

```python
from fastapi import FastAPI, HTTPException
import logging

logger = logging.getLogger(__name__)

@app.exception_handler(Exception)
async def global_exception_handler(request: Request, exc: Exception):
    # Log the error (don't expose details)
    logger.error(f"Unhandled exception: {exc}")
    
    return JSONResponse(
        status_code=500,
        content={"error": "Internal server error"}
    )

@app.get("/api/users/{user_id}")
async def get_user(user_id: int):
    user = find_user(user_id)
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    return user
```

## Summary

- Always validate input
- Use parameterized queries
- Implement rate limiting
- Don't expose error details

## Next Steps

Continue to `07-hashing-and-signing.md`.
