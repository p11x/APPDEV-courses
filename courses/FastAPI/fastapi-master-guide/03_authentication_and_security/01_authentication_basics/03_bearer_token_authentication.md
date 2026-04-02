# Bearer Token Authentication

## Overview

Bearer tokens are the most common authentication method for modern APIs. FastAPI provides built-in support for Bearer token authentication through HTTPBearer security scheme.

## Basic Bearer Token

### Simple Bearer Token

```python
# Example 1: Basic Bearer token authentication
from fastapi import FastAPI, Depends, HTTPException, status
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials

app = FastAPI()

# Define Bearer token scheme
security = HTTPBearer()

async def verify_token(
    credentials: HTTPAuthorizationCredentials = Depends(security)
):
    """
    Verify Bearer token from Authorization header.
    Authorization: Bearer <token>
    """
    token = credentials.credentials

    # Validate token (simplified)
    if token != "valid-token":
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Invalid or expired token",
            headers={"WWW-Authenticate": "Bearer"},
        )

    return {"user_id": 1, "username": "john"}

@app.get("/protected/")
async def protected_endpoint(user: dict = Depends(verify_token)):
    """
    Protected endpoint requiring Bearer token.
    Header: Authorization: Bearer valid-token
    """
    return {"message": "Access granted", "user": user}

@app.get("/public/")
async def public_endpoint():
    """No authentication required"""
    return {"message": "Public endpoint"}
```

### Optional Bearer Token

```python
# Example 2: Optional Bearer token
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from typing import Optional

app = FastAPI()

# auto_error=False makes token optional
security = HTTPBearer(auto_error=False)

async def get_current_user(
    credentials: Optional[HTTPAuthorizationCredentials] = Depends(security)
):
    """
    Get current user from Bearer token.
    Returns None if no token provided.
    """
    if credentials is None:
        return None

    token = credentials.credentials

    # Validate token
    if token != "valid-token":
        raise HTTPException(401, "Invalid token")

    return {"user_id": 1, "username": "john"}

@app.get("/items/")
async def list_items(user: Optional[dict] = Depends(get_current_user)):
    """
    Public endpoint with optional authentication.
    Provides enhanced response for authenticated users.
    """
    if user:
        return {
            "items": ["item1", "item2"],
            "user": user["username"],
            "personalized": True
        }

    return {
        "items": ["item1", "item2"],
        "personalized": False
    }
```

## JWT Bearer Tokens

### JWT Implementation

```python
# Example 3: JWT Bearer token
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from jose import JWTError, jwt
from datetime import datetime, timedelta
from pydantic import BaseModel

app = FastAPI()

SECRET_KEY = "your-secret-key-here"
ALGORITHM = "HS256"
ACCESS_TOKEN_EXPIRE_MINUTES = 30

security = HTTPBearer()

class TokenData(BaseModel):
    user_id: int
    username: str
    exp: datetime

def create_access_token(data: dict) -> str:
    """
    Create JWT access token.
    """
    to_encode = data.copy()
    expire = datetime.utcnow() + timedelta(minutes=ACCESS_TOKEN_EXPIRE_MINUTES)
    to_encode.update({"exp": expire})

    return jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)

async def verify_jwt_token(
    credentials: HTTPAuthorizationCredentials = Depends(security)
) -> TokenData:
    """
    Verify JWT Bearer token.
    """
    token = credentials.credentials

    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])

        user_id: int = payload.get("user_id")
        username: str = payload.get("username")

        if user_id is None:
            raise HTTPException(401, "Invalid token")

        return TokenData(
            user_id=user_id,
            username=username,
            exp=datetime.fromtimestamp(payload.get("exp"))
        )

    except JWTError:
        raise HTTPException(401, "Invalid or expired token")

@app.post("/login/")
async def login(username: str, password: str):
    """
    Login endpoint - returns JWT token.
    """
    # Verify credentials (simplified)
    if username == "john" and password == "secret":
        token = create_access_token(
            data={"user_id": 1, "username": username}
        )
        return {
            "access_token": token,
            "token_type": "bearer",
            "expires_in": ACCESS_TOKEN_EXPIRE_MINUTES * 60
        }

    raise HTTPException(401, "Invalid credentials")

@app.get("/protected/")
async def protected(token: TokenData = Depends(verify_jwt_token)):
    """Protected endpoint with JWT verification"""
    return {"user": token.username, "user_id": token.user_id}
```

## Token Refresh

### Refresh Token Flow

```python
# Example 4: Token refresh mechanism
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from jose import JWTError, jwt
from datetime import datetime, timedelta
import secrets

app = FastAPI()

ACCESS_TOKEN_EXPIRE = timedelta(minutes=15)
REFRESH_TOKEN_EXPIRE = timedelta(days=7)

# Token storage (use database in production)
refresh_tokens: dict[str, dict] = {}

def create_access_token(user_id: int) -> str:
    """Create short-lived access token"""
    expire = datetime.utcnow() + ACCESS_TOKEN_EXPIRE
    return jwt.encode(
        {"user_id": user_id, "exp": expire, "type": "access"},
        SECRET_KEY,
        algorithm=ALGORITHM
    )

def create_refresh_token(user_id: int) -> str:
    """Create long-lived refresh token"""
    token = secrets.token_urlsafe(32)
    refresh_tokens[token] = {
        "user_id": user_id,
        "expires_at": datetime.utcnow() + REFRESH_TOKEN_EXPIRE
    }
    return token

@app.post("/login/")
async def login(username: str, password: str):
    """Login returns both access and refresh tokens"""
    user_id = 1  # Verify credentials

    return {
        "access_token": create_access_token(user_id),
        "refresh_token": create_refresh_token(user_id),
        "token_type": "bearer"
    }

@app.post("/refresh/")
async def refresh_token(refresh_token: str):
    """
    Exchange refresh token for new access token.
    """
    if refresh_token not in refresh_tokens:
        raise HTTPException(401, "Invalid refresh token")

    token_data = refresh_tokens[refresh_token]

    if datetime.utcnow() > token_data["expires_at"]:
        del refresh_tokens[refresh_token]
        raise HTTPException(401, "Refresh token expired")

    return {
        "access_token": create_access_token(token_data["user_id"]),
        "token_type": "bearer"
    }

@app.post("/logout/")
async def logout(refresh_token: str):
    """Invalidate refresh token"""
    if refresh_token in refresh_tokens:
        del refresh_tokens[refresh_token]
    return {"message": "Logged out"}
```

## Best Practices

### Security Guidelines

```python
# Example 5: Bearer token best practices
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import HTTPBearer
from jose import jwt
from datetime import datetime, timedelta

app = FastAPI()

# 1. Use HTTPS only (configure at server level)

# 2. Short-lived access tokens
ACCESS_TOKEN_EXPIRE = timedelta(minutes=15)

# 3. Implement token blacklisting
blacklisted_tokens: set[str] = set()

def is_token_blacklisted(token: str) -> bool:
    return token in blacklisted_tokens

# 4. Validate token claims
async def verify_token_secure(credentials=Depends(HTTPBearer())):
    """Secure token verification"""
    token = credentials.credentials

    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])

        # Check expiration (automatic with jose)
        # Check issuer
        if payload.get("iss") != "your-api":
            raise HTTPException(401, "Invalid token issuer")

        # Check audience
        if payload.get("aud") != "your-api":
            raise HTTPException(401, "Invalid token audience")

        # Check blacklist
        if is_token_blacklisted(token):
            raise HTTPException(401, "Token has been revoked")

        return payload

    except jwt.JWTError:
        raise HTTPException(401, "Invalid token")

# 5. Log authentication events
@app.middleware("http")
async def log_auth_events(request, call_next):
    """Log authentication attempts"""
    if "Authorization" in request.headers:
        auth = request.headers["Authorization"]
        if auth.startswith("Bearer "):
            # Log auth attempt (without logging the token)
            pass

    return await call_next(request)
```

## Summary

| Feature | Description | Example |
|---------|-------------|---------|
| Basic Bearer | Simple token | `HTTPBearer()` |
| Optional Bearer | May not be present | `HTTPBearer(auto_error=False)` |
| JWT Bearer | Encoded claims | `jwt.encode()` / `jwt.decode()` |
| Refresh Token | Long-lived token | Separate refresh flow |
| Blacklist | Revoked tokens | Token invalidation |

## Next Steps

Continue learning about:
- [OAuth2 Flow Types](./04_oauth2_flow_types.md) - OAuth2 patterns
- [JWT Token Implementation](./05_jwt_token_implementation.md) - JWT details
- [Create Access Token](../03_security_dependencies/01_create_access_token.md) - Token creation
