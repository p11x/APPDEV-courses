# JWT Token Implementation

## Overview

JSON Web Tokens (JWT) are the industry standard for stateless authentication. This guide covers complete JWT implementation in FastAPI.

## JWT Structure

### Understanding JWT

```python
# Example 1: JWT structure and components
"""
JWT Structure: header.payload.signature

Header:
{
    "alg": "HS256",      // Algorithm
    "typ": "JWT"         // Token type
}

Payload (Claims):
{
    "sub": "1234567890",  // Subject (user ID)
    "name": "John Doe",   // Custom claim
    "iat": 1516239022,    // Issued at
    "exp": 1516242622     // Expiration time
}

Signature:
HMACSHA256(
    base64UrlEncode(header) + "." + base64UrlEncode(payload),
    secret
)
"""

from jose import jwt, JWTError
from datetime import datetime, timedelta
from pydantic import BaseModel

# Configuration
SECRET_KEY = "your-256-bit-secret-key-here-change-in-production"
ALGORITHM = "HS256"
ACCESS_TOKEN_EXPIRE_MINUTES = 30
REFRESH_TOKEN_EXPIRE_DAYS = 7

class TokenPayload(BaseModel):
    """JWT payload structure"""
    sub: str  # Subject (user ID)
    exp: datetime  # Expiration
    iat: datetime  # Issued at
    type: str  # Token type (access/refresh)
    scopes: list[str] = []  # User permissions
```

## Token Creation

### Creating JWT Tokens

```python
# Example 2: Create JWT tokens
from fastapi import FastAPI
from jose import jwt
from datetime import datetime, timedelta
import uuid

app = FastAPI()

def create_access_token(
    user_id: int,
    username: str,
    scopes: list[str] = None
) -> str:
    """
    Create JWT access token with claims.
    """
    now = datetime.utcnow()
    expire = now + timedelta(minutes=ACCESS_TOKEN_EXPIRE_MINUTES)

    payload = {
        # Standard claims
        "sub": str(user_id),           # Subject
        "exp": expire,                  # Expiration
        "iat": now,                     # Issued at
        "jti": str(uuid.uuid4()),      # JWT ID (unique)

        # Custom claims
        "username": username,
        "type": "access",
        "scopes": scopes or []
    }

    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)

def create_refresh_token(user_id: int) -> str:
    """
    Create JWT refresh token.
    Longer-lived token for obtaining new access tokens.
    """
    now = datetime.utcnow()
    expire = now + timedelta(days=REFRESH_TOKEN_EXPIRE_DAYS)

    payload = {
        "sub": str(user_id),
        "exp": expire,
        "iat": now,
        "jti": str(uuid.uuid4()),
        "type": "refresh"
    }

    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)

@app.post("/login")
async def login(username: str, password: str):
    """
    Login endpoint returning JWT tokens.
    """
    # Verify credentials
    user_id = 1  # Get from database

    return {
        "access_token": create_access_token(user_id, username),
        "refresh_token": create_refresh_token(user_id),
        "token_type": "bearer",
        "expires_in": ACCESS_TOKEN_EXPIRE_MINUTES * 60
    }
```

## Token Verification

### Verifying JWT Tokens

```python
# Example 3: Verify JWT tokens
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from jose import JWTError, jwt, ExpiredSignatureError

app = FastAPI()

security = HTTPBearer()

def decode_token(token: str) -> dict:
    """
    Decode and verify JWT token.
    Returns payload if valid, raises exception otherwise.
    """
    try:
        payload = jwt.decode(
            token,
            SECRET_KEY,
            algorithms=[ALGORITHM]
        )
        return payload

    except ExpiredSignatureError:
        raise HTTPException(
            status_code=401,
            detail="Token has expired"
        )
    except JWTError as e:
        raise HTTPException(
            status_code=401,
            detail=f"Invalid token: {str(e)}"
        )

async def get_current_user(
    credentials: HTTPAuthorizationCredentials = Depends(security)
) -> dict:
    """
    Dependency to get current user from JWT token.
    """
    token = credentials.credentials
    payload = decode_token(token)

    # Verify token type
    if payload.get("type") != "access":
        raise HTTPException(401, "Invalid token type")

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

## Token Refresh

### Refresh Token Implementation

```python
# Example 4: Token refresh flow
from fastapi import FastAPI, HTTPException
from jose import jwt, JWTError
from pydantic import BaseModel

app = FastAPI()

# Blacklist for revoked tokens
revoked_tokens: set[str] = set()

class RefreshRequest(BaseModel):
    refresh_token: str

@app.post("/refresh")
async def refresh_token(request: RefreshRequest):
    """
    Refresh access token using refresh token.
    """
    try:
        payload = jwt.decode(
            request.refresh_token,
            SECRET_KEY,
            algorithms=[ALGORITHM]
        )

        # Verify it's a refresh token
        if payload.get("type") != "refresh":
            raise HTTPException(401, "Invalid token type")

        # Check if revoked
        jti = payload.get("jti")
        if jti in revoked_tokens:
            raise HTTPException(401, "Token has been revoked")

        # Get user
        user_id = payload.get("sub")
        user = get_user_by_id(user_id)

        # Create new access token
        access_token = create_access_token(
            user_id=int(user_id),
            username=user["username"]
        )

        # Optionally rotate refresh token
        new_refresh_token = create_refresh_token(int(user_id))

        # Revoke old refresh token
        revoked_tokens.add(jti)

        return {
            "access_token": access_token,
            "refresh_token": new_refresh_token,
            "token_type": "bearer"
        }

    except JWTError:
        raise HTTPException(401, "Invalid refresh token")

@app.post("/logout")
async def logout(user: dict = Depends(get_current_user)):
    """
    Logout - revoke current token.
    """
    # Add token to blacklist
    # In production, use Redis with TTL
    return {"message": "Logged out successfully"}
```

## Advanced JWT Features

### Token with Scopes

```python
# Example 5: JWT with scopes/permissions
from fastapi import FastAPI, Depends, HTTPException, Security
from fastapi.security import SecurityScopes

app = FastAPI()

def create_token_with_scopes(user_id: int, scopes: list[str]) -> str:
    """Create token with specific scopes"""
    payload = {
        "sub": str(user_id),
        "scopes": scopes,
        "exp": datetime.utcnow() + timedelta(minutes=30)
    }
    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)

async def verify_scopes(
    security_scopes: SecurityScopes,
    credentials: HTTPAuthorizationCredentials = Depends(HTTPBearer())
):
    """
    Verify token has required scopes.
    """
    token = credentials.credentials
    payload = decode_token(token)

    token_scopes = payload.get("scopes", [])

    for scope in security_scopes.scopes:
        if scope not in token_scopes:
            raise HTTPException(
                status_code=403,
                detail=f"Missing required scope: {scope}"
            )

    return payload

@app.get("/items/", dependencies=[Security(verify_scopes, scopes=["items:read"])])
async def read_items():
    """Requires items:read scope"""
    return {"items": []}

@app.post("/items/", dependencies=[Security(verify_scopes, scopes=["items:write"])])
async def create_item():
    """Requires items:write scope"""
    return {"created": True}

@app.delete("/items/{id}", dependencies=[Security(verify_scopes, scopes=["items:delete"])])
async def delete_item(id: int):
    """Requires items:delete scope"""
    return {"deleted": id}
```

## Best Practices

### Security Guidelines

```python
# Example 6: JWT best practices
"""
JWT Security Best Practices:

1. Use strong secret keys (256+ bits)
2. Use appropriate algorithm (HS256, RS256)
3. Set reasonable expiration times
4. Include only necessary claims
5. Validate all claims
6. Implement token revocation
7. Use HTTPS only
8. Don't store sensitive data in JWT
"""

from jose import jwt
import secrets

# 1. Generate secure secret key
def generate_secret_key() -> str:
    """Generate 256-bit secret key"""
    return secrets.token_urlsafe(32)

# 2. Token configuration
TOKEN_CONFIG = {
    "access_token": {
        "expire_minutes": 15,  # Short-lived
        "algorithm": "HS256"
    },
    "refresh_token": {
        "expire_days": 7,
        "algorithm": "HS256"
    }
}

# 3. Claim validation
def validate_token_claims(payload: dict) -> bool:
    """Validate all required claims"""
    required_claims = ["sub", "exp", "iat", "type"]

    for claim in required_claims:
        if claim not in payload:
            return False

    # Verify expiration
    if datetime.utcnow() > datetime.fromtimestamp(payload["exp"]):
        return False

    return True
```

## Summary

| Feature | Implementation | Notes |
|---------|----------------|-------|
| Create token | `jwt.encode()` | Include claims |
| Verify token | `jwt.decode()` | Validates signature & exp |
| Refresh token | Separate flow | Longer-lived |
| Scopes | Claims array | Fine-grained access |
| Revocation | Blacklist | Token invalidation |

## Next Steps

Continue learning about:
- [Create Access Token](../03_security_dependencies/01_create_access_token.md) - Detailed token creation
- [Create Refresh Token](../03_security_dependencies/02_create_refresh_token.md) - Refresh implementation
- [Password Hashing](../03_security_dependencies/03_password_hashing.md) - Secure passwords
