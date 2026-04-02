# Create Access Token

## Overview

Access tokens are short-lived credentials that authorize API requests. This guide covers secure token creation and management.

## JWT Access Token Creation

### Basic Token Creation

```python
# Example 1: Create JWT access token
from fastapi import FastAPI
from jose import jwt
from datetime import datetime, timedelta
import uuid

app = FastAPI()

SECRET_KEY = "your-secret-key-change-in-production"
ALGORITHM = "HS256"
ACCESS_TOKEN_EXPIRE_MINUTES = 30

def create_access_token(
    data: dict,
    expires_delta: timedelta | None = None
) -> str:
    """
    Create JWT access token.

    Args:
        data: Payload data (user info)
        expires_delta: Custom expiration time

    Returns:
        Encoded JWT token string
    """
    # Copy data to avoid mutation
    to_encode = data.copy()

    # Set expiration
    expire = datetime.utcnow() + (
        expires_delta or timedelta(minutes=ACCESS_TOKEN_EXPIRE_MINUTES)
    )

    # Add standard claims
    to_encode.update({
        "exp": expire,           # Expiration time
        "iat": datetime.utcnow(),  # Issued at
        "jti": str(uuid.uuid4()),  # Unique token ID
        "type": "access"         # Token type
    })

    # Encode token
    encoded_jwt = jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)

    return encoded_jwt

@app.post("/login")
async def login(username: str, password: str):
    """
    Login endpoint returning access token.
    """
    # Verify credentials (simplified)
    user_id = 1

    # Create token
    token_data = {
        "sub": str(user_id),  # Subject (user ID)
        "username": username,
        "scopes": ["read", "write"]
    }

    access_token = create_access_token(data=token_data)

    return {
        "access_token": access_token,
        "token_type": "bearer",
        "expires_in": ACCESS_TOKEN_EXPIRE_MINUTES * 60
    }
```

### Token with Scopes

```python
# Example 2: Token with scopes/permissions
from fastapi import FastAPI
from jose import jwt
from datetime import datetime, timedelta
from typing import List

app = FastAPI()

def create_scoped_token(
    user_id: int,
    scopes: List[str],
    expires_delta: timedelta | None = None
) -> str:
    """
    Create token with specific scopes.
    """
    payload = {
        "sub": str(user_id),
        "scopes": scopes,
        "exp": datetime.utcnow() + (expires_delta or timedelta(minutes=30)),
        "iat": datetime.utcnow(),
        "type": "access"
    }

    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)

@app.post("/login")
async def login(username: str, password: str):
    """
    Login with role-based scopes.
    """
    # Get user permissions from database
    user_permissions = get_user_permissions(username)

    token = create_scoped_token(
        user_id=1,
        scopes=user_permissions
    )

    return {
        "access_token": token,
        "token_type": "bearer",
        "scopes": user_permissions
    }
```

## Token Validation

### Verifying Access Tokens

```python
# Example 3: Verify access token
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from jose import jwt, JWTError, ExpiredSignatureError

app = FastAPI()

security = HTTPBearer()

def verify_access_token(token: str) -> dict:
    """
    Verify and decode access token.

    Returns:
        Decoded payload if valid

    Raises:
        HTTPException if token is invalid
    """
    try:
        payload = jwt.decode(
            token,
            SECRET_KEY,
            algorithms=[ALGORITHM]
        )

        # Verify token type
        if payload.get("type") != "access":
            raise HTTPException(401, "Invalid token type")

        return payload

    except ExpiredSignatureError:
        raise HTTPException(401, "Token has expired")
    except JWTError:
        raise HTTPException(401, "Invalid token")

async def get_current_user(
    credentials: HTTPAuthorizationCredentials = Depends(security)
) -> dict:
    """
    Dependency to get current user from token.
    """
    payload = verify_access_token(credentials.credentials)

    # Get user from database
    user_id = payload.get("sub")
    user = get_user_by_id(user_id)

    if not user:
        raise HTTPException(401, "User not found")

    return user

@app.get("/protected")
async def protected_route(user: dict = Depends(get_current_user)):
    """Protected endpoint"""
    return {"user": user}
```

## Best Practices

### Token Security

```python
# Example 4: Token security best practices
"""
Access Token Best Practices:

1. Short expiration (15-30 minutes)
2. Include only necessary claims
3. Use strong secret keys
4. Implement token refresh
5. Support token revocation
6. Log token usage
"""

from fastapi import FastAPI
from jose import jwt
from datetime import datetime, timedelta
import secrets

app = FastAPI()

# 1. Short-lived tokens
ACCESS_TOKEN_EXPIRE = timedelta(minutes=15)

# 2. Minimal claims
def create_secure_token(user_id: int) -> str:
    """Create minimal, secure token"""
    return jwt.encode(
        {
            "sub": str(user_id),
            "exp": datetime.utcnow() + ACCESS_TOKEN_EXPIRE,
            "type": "access"
        },
        SECRET_KEY,
        algorithm=ALGORITHM
    )

# 3. Strong secret key generation
def generate_secret_key() -> str:
    """Generate cryptographically secure key"""
    return secrets.token_urlsafe(32)

# 4. Token revocation
revoked_tokens: set[str] = set()

def is_token_revoked(token: str) -> bool:
    """Check if token is revoked"""
    return token in revoked_tokens

def revoke_token(token: str):
    """Revoke a token"""
    revoked_tokens.add(token)
```

## Summary

| Feature | Implementation | Notes |
|---------|----------------|-------|
| Create token | `jwt.encode()` | Include claims |
| Verify token | `jwt.decode()` | Check signature |
| Expiration | `exp` claim | Short-lived |
| Scopes | `scopes` claim | Fine-grained access |

## Next Steps

Continue learning about:
- [Refresh Token](./02_create_refresh_token.md) - Token refresh
- [Password Hashing](./03_password_hashing.md) - Secure passwords
- [Token Expiration](./06_token_expiration_management.md) - Token lifecycle
