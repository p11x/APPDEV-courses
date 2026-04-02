# Refresh Token Implementation

## Overview

Refresh tokens allow obtaining new access tokens without re-authentication. They are long-lived and enable seamless session management.

## Implementation

### Refresh Token Flow

```python
# Example 1: Complete refresh token implementation
from fastapi import FastAPI, HTTPException, Depends
from jose import jwt, JWTError
from datetime import datetime, timedelta
from pydantic import BaseModel
import secrets

app = FastAPI()

SECRET_KEY = "your-secret-key"
ACCESS_TOKEN_EXPIRE = timedelta(minutes=15)
REFRESH_TOKEN_EXPIRE = timedelta(days=7)

class TokenPair(BaseModel):
    access_token: str
    refresh_token: str
    token_type: str = "bearer"

class RefreshRequest(BaseModel):
    refresh_token: str

# Token storage (use database in production)
refresh_tokens: dict[str, dict] = {}

def create_token_pair(user_id: int) -> TokenPair:
    """Create access and refresh token pair"""
    access_token = jwt.encode(
        {
            "sub": str(user_id),
            "type": "access",
            "exp": datetime.utcnow() + ACCESS_TOKEN_EXPIRE
        },
        SECRET_KEY,
        algorithm="HS256"
    )

    refresh_token = secrets.token_urlsafe(32)
    refresh_tokens[refresh_token] = {
        "user_id": user_id,
        "expires": datetime.utcnow() + REFRESH_TOKEN_EXPIRE
    }

    return TokenPair(
        access_token=access_token,
        refresh_token=refresh_token
    )

@app.post("/login")
async def login(username: str, password: str):
    """Login returns token pair"""
    user_id = 1  # Verify credentials
    return create_token_pair(user_id)

@app.post("/refresh")
async def refresh(request: RefreshRequest):
    """Exchange refresh token for new token pair"""
    token_data = refresh_tokens.get(request.refresh_token)

    if not token_data:
        raise HTTPException(401, "Invalid refresh token")

    if datetime.utcnow() > token_data["expires"]:
        del refresh_tokens[request.refresh_token]
        raise HTTPException(401, "Refresh token expired")

    # Revoke old refresh token (rotation)
    del refresh_tokens[request.refresh_token]

    return create_token_pair(token_data["user_id"])

@app.post("/logout")
async def logout(refresh_token: str):
    """Revoke refresh token"""
    if refresh_token in refresh_tokens:
        del refresh_tokens[refresh_token]
    return {"message": "Logged out"}
```

## Best Practices

1. Use short-lived access tokens (15-30 minutes)
2. Implement refresh token rotation
3. Store refresh tokens securely
4. Allow token revocation

## Next Steps

Continue learning about:
- [Password Hashing](./03_password_hashing.md)
- [User Authentication](./04_user_authentication.md)
