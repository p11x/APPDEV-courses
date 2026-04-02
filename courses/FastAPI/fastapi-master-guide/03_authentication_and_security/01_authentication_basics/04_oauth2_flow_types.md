# OAuth2 Flow Types

## Overview

OAuth2 is an authorization framework enabling secure delegated access. FastAPI provides built-in support for various OAuth2 flows.

## OAuth2 Password Flow

### Basic Password Flow

```python
# Example 1: OAuth2 Password Flow
from fastapi import FastAPI, Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer, OAuth2PasswordRequestForm
from jose import JWTError, jwt
from datetime import datetime, timedelta
from pydantic import BaseModel

app = FastAPI()

SECRET_KEY = "your-secret-key"
ALGORITHM = "HS256"

# OAuth2 scheme
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

class User(BaseModel):
    username: str
    email: str
    disabled: bool = False

class UserInDB(User):
    hashed_password: str

# Simulated user database
fake_users_db = {
    "john": {
        "username": "john",
        "email": "john@example.com",
        "hashed_password": "$2b$12$EixZaYVK1fsbw1ZfbX3OXePaWxn96p36WQoeG6Lruj3vjPGga31lW",
        "disabled": False
    }
}

def verify_password(plain_password: str, hashed_password: str) -> bool:
    """Verify password against hash"""
    from passlib.context import CryptContext
    pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
    return pwd_context.verify(plain_password, hashed_password)

def get_user(username: str) -> UserInDB:
    """Get user from database"""
    if username in fake_users_db:
        return UserInDB(**fake_users_db[username])
    return None

def authenticate_user(username: str, password: str) -> User:
    """Authenticate user with credentials"""
    user = get_user(username)
    if not user or not verify_password(password, user.hashed_password):
        return None
    return user

def create_access_token(data: dict) -> str:
    """Create JWT access token"""
    to_encode = data.copy()
    expire = datetime.utcnow() + timedelta(minutes=30)
    to_encode.update({"exp": expire})
    return jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)

@app.post("/token")
async def login(form_data: OAuth2PasswordRequestForm = Depends()):
    """
    OAuth2 compatible token login.
    Uses form data with username and password.
    """
    user = authenticate_user(form_data.username, form_data.password)
    if not user:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Incorrect username or password",
            headers={"WWW-Authenticate": "Bearer"},
        )

    access_token = create_access_token(
        data={"sub": user.username}
    )

    return {
        "access_token": access_token,
        "token_type": "bearer"
    }

async def get_current_user(token: str = Depends(oauth2_scheme)) -> User:
    """Get current user from token"""
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
        username: str = payload.get("sub")
        if username is None:
            raise HTTPException(401, "Invalid token")
    except JWTError:
        raise HTTPException(401, "Invalid token")

    user = get_user(username)
    if user is None:
        raise HTTPException(401, "User not found")

    return user

@app.get("/users/me")
async def read_users_me(current_user: User = Depends(get_current_user)):
    """Get current user profile"""
    return current_user
```

## OAuth2 Authorization Code Flow

### Authorization Code Flow

```python
# Example 2: Authorization Code Flow
from fastapi import FastAPI, Depends, HTTPException, Query
from fastapi.responses import RedirectResponse
from jose import jwt
import httpx
import secrets

app = FastAPI()

# OAuth2 configuration
CLIENT_ID = "your-client-id"
CLIENT_SECRET = "your-client-secret"
REDIRECT_URI = "http://localhost:8000/callback"
AUTHORIZATION_URL = "https://provider.com/authorize"
TOKEN_URL = "https://provider.com/token"

# Store state for CSRF protection
oauth_states: dict[str, dict] = {}

@app.get("/login")
async def login():
    """
    Initiate OAuth2 authorization code flow.
    Redirects user to authorization server.
    """
    state = secrets.token_urlsafe(32)
    oauth_states[state] = {"created_at": "now"}

    auth_url = (
        f"{AUTHORIZATION_URL}?"
        f"response_type=code&"
        f"client_id={CLIENT_ID}&"
        f"redirect_uri={REDIRECT_URI}&"
        f"scope=read:user&"
        f"state={state}"
    )

    return RedirectResponse(url=auth_url)

@app.get("/callback")
async def callback(code: str = Query(...), state: str = Query(...)):
    """
    Handle OAuth2 callback.
    Exchange authorization code for access token.
    """
    # Verify state (CSRF protection)
    if state not in oauth_states:
        raise HTTPException(400, "Invalid state parameter")

    del oauth_states[state]

    # Exchange code for token
    async with httpx.AsyncClient() as client:
        response = await client.post(
            TOKEN_URL,
            data={
                "grant_type": "authorization_code",
                "code": code,
                "redirect_uri": REDIRECT_URI,
                "client_id": CLIENT_ID,
                "client_secret": CLIENT_SECRET
            }
        )

    if response.status_code != 200:
        raise HTTPException(400, "Failed to exchange code")

    token_data = response.json()

    return {
        "access_token": token_data["access_token"],
        "token_type": token_data["token_type"]
    }
```

## OAuth2 Client Credentials Flow

### Service-to-Service Auth

```python
# Example 3: Client Credentials Flow
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import OAuth2PasswordBearer
from jose import jwt
from pydantic import BaseModel

app = FastAPI()

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
    """
    Client credentials flow for service-to-service auth.
    No user context - machine authentication.
    """
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
        algorithm=ALGORITHM
    )

    return {
        "access_token": token,
        "token_type": "bearer",
        "expires_in": 3600
    }
```

## OAuth2 Device Code Flow

### Device Authorization

```python
# Example 4: Device Code Flow
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import secrets
from datetime import datetime, timedelta

app = FastAPI()

# Device code storage
device_codes: dict[str, dict] = {}

class DeviceCodeResponse(BaseModel):
    device_code: str
    user_code: str
    verification_uri: str
    expires_in: int
    interval: int

@app.post("/device/code")
async def request_device_code(client_id: str) -> DeviceCodeResponse:
    """
    Initiate device code flow.
    For devices with limited input capabilities.
    """
    device_code = secrets.token_urlsafe(32)
    user_code = secrets.token_urlsafe(8).upper()

    device_codes[device_code] = {
        "user_code": user_code,
        "client_id": client_id,
        "status": "pending",
        "created_at": datetime.utcnow(),
        "expires_at": datetime.utcnow() + timedelta(minutes=10)
    }

    return DeviceCodeResponse(
        device_code=device_code,
        user_code=user_code,
        verification_uri="https://example.com/device",
        expires_in=600,
        interval=5
    )

@app.post("/device/token")
async def poll_device_token(device_code: str, client_id: str):
    """
    Poll for device authorization completion.
    Device calls this repeatedly until approved.
    """
    if device_code not in device_codes:
        raise HTTPException(400, "Invalid device code")

    device = device_codes[device_code]

    if datetime.utcnow() > device["expires_at"]:
        del device_codes[device_code]
        raise HTTPException(400, "Device code expired")

    if device["status"] == "pending":
        raise HTTPException(400, "Authorization pending")

    if device["status"] == "approved":
        token = create_access_token({"client_id": client_id})
        del device_codes[device_code]
        return {"access_token": token, "token_type": "bearer"}

    raise HTTPException(400, "Authorization denied")
```

## Flow Comparison

| Flow | Use Case | Security Level |
|------|----------|----------------|
| Password | First-party apps | Medium |
| Authorization Code | Third-party apps | High |
| Client Credentials | Service-to-service | High |
| Device Code | Limited input devices | High |

## Summary

OAuth2 provides flexible authorization patterns for different application types. Choose the flow based on your security requirements and client capabilities.

## Next Steps

Continue learning about:
- [OAuth2 Password Flow](../02_oauth2_implementation/01_oauth2_password_flow.md) - Detailed implementation
- [JWT Token Implementation](./05_jwt_token_implementation.md) - JWT details
- [OAuth2 Server Config](../02_oauth2_implementation/05_oauth2_server_config.md) - Server setup
