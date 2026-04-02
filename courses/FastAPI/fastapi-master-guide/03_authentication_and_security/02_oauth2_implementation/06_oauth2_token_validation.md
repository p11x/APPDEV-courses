# OAuth2 Token Validation

## Overview

Token validation ensures tokens are authentic, not expired, and have the required claims for access control.

## Implementation

```python
# Example 1: Comprehensive token validation
from fastapi import FastAPI, Depends, HTTPException, Security
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from jose import JWTError, jwt, ExpiredSignatureError
from datetime import datetime

app = FastAPI()

SECRET_KEY = "your-secret-key"
ALGORITHM = "HS256"

security = HTTPBearer()

class TokenValidator:
    """Comprehensive token validation"""

    def __init__(self, secret_key: str, algorithm: str = "HS256"):
        self.secret_key = secret_key
        self.algorithm = algorithm
        self.revoked_tokens: set = set()

    def validate(self, token: str) -> dict:
        """Validate token and return payload"""
        try:
            payload = jwt.decode(
                token,
                self.secret_key,
                algorithms=[self.algorithm]
            )

            # Check if token is revoked
            jti = payload.get("jti")
            if jti in self.revoked_tokens:
                raise HTTPException(401, "Token has been revoked")

            # Check token type
            if payload.get("type") != "access":
                raise HTTPException(401, "Invalid token type")

            # Check expiration (automatic with jose)
            return payload

        except ExpiredSignatureError:
            raise HTTPException(401, "Token has expired")
        except JWTError as e:
            raise HTTPException(401, f"Invalid token: {str(e)}")

    def revoke(self, token: str):
        """Revoke a token"""
        try:
            payload = jwt.decode(
                token,
                self.secret_key,
                algorithms=[self.algorithm],
                options={"verify_exp": False}
            )
            jti = payload.get("jti")
            if jti:
                self.revoked_tokens.add(jti)
        except JWTError:
            pass

validator = TokenValidator(SECRET_KEY)

async def get_current_user(
    credentials: HTTPAuthorizationCredentials = Security(security)
) -> dict:
    """Dependency for getting authenticated user"""
    payload = validator.validate(credentials.credentials)

    # Get user from database
    user = get_user_by_username(payload.get("sub"))
    if not user:
        raise HTTPException(401, "User not found")

    return user

@app.get("/protected")
async def protected(user: dict = Depends(get_current_user)):
    """Protected endpoint"""
    return {"user": user}

@app.post("/logout")
async def logout(credentials: HTTPAuthorizationCredentials = Security(security)):
    """Revoke token on logout"""
    validator.revoke(credentials.credentials)
    return {"message": "Logged out"}
```

## Summary

Token validation is critical for security. Always validate signature, expiration, and claims.

## Next Steps

Continue learning about:
- [Rate Limiting](../04_advanced_security/01_rate_limiting.md)
- [Security Headers](../04_advanced_security/07_api_security_headers.md)
