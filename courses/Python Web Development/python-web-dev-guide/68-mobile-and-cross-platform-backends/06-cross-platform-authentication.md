# Cross-Platform Authentication

## What You'll Learn

- Unified authentication across web and mobile
- OAuth implementation for multiple platforms
- Token management strategies
- Session handling

## Prerequisites

- Completed `05-flutter-backend-integration.md`

## Introduction

When building for multiple platforms (web, iOS, Android), you need a consistent authentication strategy. This guide covers implementing authentication that works across all platforms.

## Unified Authentication System

Create a single authentication system for all platforms:

```python
from fastapi import FastAPI, HTTPException, Header
from fastapi.security import OAuth2PasswordBearer, OAuth2PasswordRequestForm
from pydantic import BaseModel, EmailStr
from typing import Optional
from datetime import datetime, timedelta
import jwt
import hashlib
import secrets
import os


app = FastAPI()

# ============ Configuration ============
SECRET_KEY = os.environ.get("SECRET_KEY", "your-secret-key")
ALGORITHM = "HS256"
ACCESS_TOKEN_EXPIRE_MINUTES = 30
REFRESH_TOKEN_EXPIRE_DAYS = 7


# ============ Models ============

class User(BaseModel):
    id: str
    email: str
    name: str
    password_hash: str
    created_at: datetime
    platform: Optional[str] = None  # web, ios, android


class Token(BaseModel):
    access_token: str
    refresh_token: str
    token_type: str = "bearer"


class TokenData(BaseModel):
    user_id: Optional[str] = None


class OAuthCallback(BaseModel):
    """OAuth callback data."""
    code: str
    redirect_uri: str
    platform: str  # ios, android, web


# ============ Password Hashing ============

def hash_password(password: str) -> str:
    """Hash password with SHA-256."""
    salt = secrets.token_hex(16)
    hash_obj = hashlib.pbkdf2_hmac(
        "sha256",
        password.encode(),
        salt.encode(),
        100000,
    )
    return f"{salt}${hash_obj.hex()}"


def verify_password(password: str, password_hash: str) -> bool:
    """Verify password against hash."""
    try:
        salt, stored_hash = password_hash.split("$")
        hash_obj = hashlib.pbkdf2_hmac(
            "sha256",
            password.encode(),
            salt.encode(),
            100000,
        )
        return hash_obj.hex() == stored_hash
    except:
        return False


# ============ Token Management ============

def create_access_token(data: dict, expires_delta: Optional[timedelta] = None) -> str:
    """Create JWT access token."""
    to_encode = data.copy()
    
    if expires_delta:
        expire = datetime.utcnow() + expires_delta
    else:
        expire = datetime.utcnow() + timedelta(minutes=ACCESS_TOKEN_EXPIRE_MINUTES)
    
    to_encode.update({"exp": expire, "type": "access"})
    
    encoded_jwt = jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)
    return encoded_jwt


def create_refresh_token(data: dict) -> str:
    """Create JWT refresh token."""
    to_encode = data.copy()
    expire = datetime.utcnow() + timedelta(days=REFRESH_TOKEN_EXPIRE_DAYS)
    to_encode.update({"exp": expire, "type": "refresh"})
    
    encoded_jwt = jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)
    return encoded_jwt


def verify_token(token: str) -> Optional[TokenData]:
    """Verify and decode JWT token."""
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
        user_id: str = payload.get("sub")
        
        if user_id is None:
            return None
        
        return TokenData(user_id=user_id)
    except:
        return None


# ============ User Database ============
# In production, use a real database
users_db: dict[str, User] = {}


def get_user_by_email(email: str) -> Optional[User]:
    """Get user by email."""
    for user in users_db.values():
        if user.email == email:
            return user
    return None


def get_user_by_id(user_id: str) -> Optional[User]:
    """Get user by ID."""
    return users_db.get(user_id)


# ============ Authentication Endpoints ============

@app.post("/api/auth/register", response_model=Token)
async def register(email: str, password: str, name: str, platform: str = "web") -> Token:
    """Register a new user."""
    # Check if user exists
    if get_user_by_email(email):
        raise HTTPException(status_code=400, detail="Email already registered")
    
    # Create user
    user_id = secrets.token_urlsafe(16)
    user = User(
        id=user_id,
        email=email,
        name=name,
        password_hash=hash_password(password),
        created_at=datetime.now(),
        platform=platform,
    )
    
    users_db[user_id] = user
    
    # Generate tokens
    access_token = create_access_token({"sub": user_id})
    refresh_token = create_refresh_token({"sub": user_id})
    
    return Token(
        access_token=access_token,
        refresh_token=refresh_token,
    )


@app.post("/api/auth/login", response_model=Token)
async def login(
    form_data: OAuth2PasswordRequestForm = None,
    email: Optional[str] = None,
    password: Optional[str] = None,
) -> Token:
    """Login with email/password."""
    # Support both form data and JSON
    if form_data:
        login_email = form_data.username
        login_password = form_data.password
    else:
        if not email or not password:
            raise HTTPException(status_code=400, detail="Missing credentials")
        login_email = email
        login_password = password
    
    user = get_user_by_email(login_email)
    
    if not user or not verify_password(login_password, user.password_hash):
        raise HTTPException(
            status_code=401,
            detail="Incorrect email or password",
        )
    
    access_token = create_access_token({"sub": user.id})
    refresh_token = create_refresh_token({"sub": user.id})
    
    return Token(
        access_token=access_token,
        refresh_token=refresh_token,
    )


@app.post("/api/auth/refresh", response_model=Token)
async def refresh_access_token(refresh_token: str) -> Token:
    """Refresh access token using refresh token."""
    token_data = verify_token(refresh_token)
    
    if not token_data or not token_data.user_id:
        raise HTTPException(status_code=401, detail="Invalid refresh token")
    
    user = get_user_by_id(token_data.user_id)
    
    if not user:
        raise HTTPException(status_code=401, detail="User not found")
    
    new_access_token = create_access_token({"sub": user.id})
    new_refresh_token = create_refresh_token({"sub": user.id})
    
    return Token(
        access_token=new_access_token,
        refresh_token=new_refresh_token,
    )


@app.get("/api/auth/me")
async def get_current_user(authorization: str = Header(None)) -> dict:
    """Get current authenticated user."""
    if not authorization or not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Not authenticated")
    
    token = authorization[7:]
    token_data = verify_token(token)
    
    if not token_data or not token_data.user_id:
        raise HTTPException(status_code=401, detail="Invalid token")
    
    user = get_user_by_id(token_data.user_id)
    
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    
    return {
        "id": user.id,
        "email": user.email,
        "name": user.name,
    }


@app.post("/api/auth/logout")
async def logout() -> dict:
    """Logout user (client should discard tokens)."""
    return {"success": True}
```

## OAuth 2.0 for Mobile

Implement OAuth for mobile platforms:

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel


app = FastAPI()


# ============ OAuth State Management ============

oauth_states: dict[str, dict] = {}


def create_oauth_state(platform: str, redirect_uri: str) -> str:
    """Create OAuth state for CSRF protection."""
    import secrets
    state = secrets.token_urlsafe(32)
    
    oauth_states[state] = {
        "platform": platform,
        "redirect_uri": redirect_uri,
        "created_at": datetime.now(),
    }
    
    return state


@app.get("/api/auth/oauth/google")
async def google_oauth_redirect(
    redirect_uri: str = "com.yourapp://oauth/callback",
    platform: str = "ios",
) -> dict:
    """Get Google OAuth URL for mobile."""
    state = create_oauth_state(platform, redirect_uri)
    
    # In production, use actual Google OAuth URL
    google_auth_url = (
        "https://accounts.google.com/o/oauth2/v2/auth?"
        f"client_id=YOUR_CLIENT_ID&"
        f"redirect_uri={redirect_uri}&"
        f"response_type=code&"
        f"scope=email%20profile&"
        f"state={state}"
    )
    
    return {
        "auth_url": google_auth_url,
        "state": state,
    }


@app.post("/api/auth/oauth/google/callback")
async def google_oauth_callback(code: str, state: str) -> Token:
    """Handle Google OAuth callback."""
    # Verify state
    if state not in oauth_states:
        raise HTTPException(status_code=400, detail="Invalid state")
    
    oauth_info = oauth_states.pop(state)
    
    # In production:
    # 1. Exchange code for access token with Google
    # 2. Get user info from Google
    # 3. Create or update user in database
    
    # Demo: create a token for demo user
    import secrets
    user_id = secrets.token_urlsafe(16)
    
    access_token = create_access_token({"sub": user_id, "oauth": "google"})
    refresh_token = create_refresh_token({"sub": user_id})
    
    return Token(
        access_token=access_token,
        refresh_token=refresh_token,
    )
```

## Platform-Specific Token Storage

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel


app = FastAPI()


class DeviceRegistration(BaseModel):
    """Register a device for push notifications."""
    user_id: str
    device_token: str
    platform: str  # ios, android
    app_version: str


# Device tokens storage
device_tokens: dict[str, list[dict]] = {}


@app.post("/api/devices/register")
async def register_device(registration: DeviceRegistration) -> dict:
    """Register device for push notifications."""
    
    if registration.user_id not in device_tokens:
        device_tokens[registration.user_id] = []
    
    # Check if token already exists
    for device in device_tokens[registration.user_id]:
        if device["token"] == registration.device_token:
            device["app_version"] = registration.app_version
            device["last_active"] = datetime.now()
            return {"success": True}
    
    # Add new device
    device_tokens[registration.user_id].append({
        "token": registration.device_token,
        "platform": registration.platform,
        "app_version": registration.app_version,
        "registered_at": datetime.now(),
        "last_active": datetime.now(),
    })
    
    return {"success": True}


@app.delete("/api/devices/{device_token}")
async def unregister_device(device_token: str, user_id: str) -> dict:
    """Unregister a device."""
    if user_id in device_tokens:
        device_tokens[user_id] = [
            d for d in device_tokens[user_id]
            if d["token"] != device_token
        ]
    
    return {"success": True}


@app.get("/api/devices")
async def get_devices(user_id: str) -> list[dict]:
    """Get all registered devices for a user."""
    return device_tokens.get(user_id, [])
```

## Summary

- Use JWT for authentication across all platforms
- Implement refresh tokens for seamless user experience
- Use OAuth 2.0 for social login
- Store device tokens for push notifications
- Implement proper token invalidation on logout

## Next Steps

→ Continue to `07-mobile-app-analytics-backend.md` to learn about analytics for mobile apps.
