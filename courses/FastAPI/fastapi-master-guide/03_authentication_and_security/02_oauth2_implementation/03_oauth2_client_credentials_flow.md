# OAuth2 Client Credentials Flow

## Overview

Client Credentials flow is used for service-to-service authentication without user involvement. It's ideal for machine-to-machine communication.

## Implementation

```python
# Example 1: Client Credentials flow
from fastapi import FastAPI, HTTPException, Depends
from fastapi.security import OAuth2PasswordBearer
from jose import jwt
from pydantic import BaseModel
from datetime import datetime, timedelta

app = FastAPI()

SECRET_KEY = "your-secret-key"

class ClientCredentials(BaseModel):
    client_id: str
    client_secret: str

# Valid clients
VALID_CLIENTS = {
    "service-a": {"secret": "secret-a", "scopes": ["read", "write"]},
    "service-b": {"secret": "secret-b", "scopes": ["read"]},
}

@app.post("/token")
async def get_token(credentials: ClientCredentials):
    """Client credentials token endpoint"""
    client = VALID_CLIENTS.get(credentials.client_id)

    if not client or client["secret"] != credentials.client_secret:
        raise HTTPException(401, "Invalid client credentials")

    token = jwt.encode(
        {
            "client_id": credentials.client_id,
            "scopes": client["scopes"],
            "exp": datetime.utcnow() + timedelta(hours=1)
        },
        SECRET_KEY,
        algorithm="HS256"
    )

    return {
        "access_token": token,
        "token_type": "bearer",
        "expires_in": 3600
    }

oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

@app.get("/protected")
async def protected(token: str = Depends(oauth2_scheme)):
    """Protected endpoint for service-to-service calls"""
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=["HS256"])
        return {"client_id": payload["client_id"], "scopes": payload["scopes"]}
    except jwt.JWTError:
        raise HTTPException(401, "Invalid token")
```

## Summary

Client Credentials flow is perfect for backend services that need to authenticate without user interaction.

## Next Steps

Continue learning about:
- [Device Code Flow](./04_oauth2_device_code_flow.md)
- [Token Validation](./06_oauth2_token_validation.md)
