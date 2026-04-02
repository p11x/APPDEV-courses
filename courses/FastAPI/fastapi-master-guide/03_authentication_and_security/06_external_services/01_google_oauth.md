# Google OAuth

## Overview

Google OAuth enables "Sign in with Google" functionality. FastAPI makes it straightforward to implement Google authentication.

## Basic Google OAuth

### OAuth2 Configuration

```python
# Example 1: Google OAuth setup
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import OAuth2AuthorizationCodeBearer
from httpx import AsyncClient
from jose import jwt
from pydantic import BaseModel

app = FastAPI()

# Google OAuth configuration
GOOGLE_CLIENT_ID = "your-client-id.apps.googleusercontent.com"
GOOGLE_CLIENT_SECRET = "your-client-secret"
GOOGLE_REDIRECT_URI = "http://localhost:8000/auth/google/callback"

GOOGLE_AUTH_URL = "https://accounts.google.com/o/oauth2/auth"
GOOGLE_TOKEN_URL = "https://oauth2.googleapis.com/token"
GOOGLE_USERINFO_URL = "https://www.googleapis.com/oauth2/v2/userinfo"

class GoogleUser(BaseModel):
    id: str
    email: str
    name: str
    picture: str | None = None
    verified_email: bool = False

@app.get("/auth/google/login")
async def google_login():
    """
    Initiate Google OAuth flow.
    Returns URL for user to visit.
    """
    from urllib.parse import urlencode

    params = {
        "client_id": GOOGLE_CLIENT_ID,
        "redirect_uri": GOOGLE_REDIRECT_URI,
        "response_type": "code",
        "scope": "openid email profile",
        "access_type": "offline",
        "prompt": "consent"
    }

    auth_url = f"{GOOGLE_AUTH_URL}?{urlencode(params)}"

    return {"auth_url": auth_url}

@app.get("/auth/google/callback")
async def google_callback(code: str):
    """
    Handle Google OAuth callback.
    Exchange code for token and get user info.
    """
    async with AsyncClient() as client:
        # Exchange code for token
        token_response = await client.post(
            GOOGLE_TOKEN_URL,
            data={
                "client_id": GOOGLE_CLIENT_ID,
                "client_secret": GOOGLE_CLIENT_SECRET,
                "code": code,
                "grant_type": "authorization_code",
                "redirect_uri": GOOGLE_REDIRECT_URI
            }
        )

        token_data = token_response.json()

        if "access_token" not in token_data:
            raise HTTPException(400, "Failed to get access token")

        # Get user info
        user_response = await client.get(
            GOOGLE_USERINFO_URL,
            headers={"Authorization": f"Bearer {token_data['access_token']}"}
        )

        user_data = user_response.json()

    # Create or update user in database
    google_user = GoogleUser(**user_data)

    # Create JWT token for your app
    app_token = create_app_token(google_user)

    return {
        "access_token": app_token,
        "user": google_user
    }
```

### Complete Implementation

```python
# Example 2: Complete Google OAuth with JWT
from fastapi import FastAPI, HTTPException, Depends
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from httpx import AsyncClient
from jose import jwt, JWTError
from datetime import datetime, timedelta
from pydantic import BaseModel
import secrets

app = FastAPI()

SECRET_KEY = "your-app-secret-key"
ALGORITHM = "HS256"

class GoogleOAuthConfig(BaseModel):
    client_id: str
    client_secret: str
    redirect_uri: str

config = GoogleOAuthConfig(
    client_id=GOOGLE_CLIENT_ID,
    client_secret=GOOGLE_CLIENT_SECRET,
    redirect_uri=GOOGLE_REDIRECT_URI
)

# Store OAuth states for CSRF protection
oauth_states: dict[str, dict] = {}

async def exchange_code_for_token(code: str) -> dict:
    """Exchange authorization code for tokens"""
    async with AsyncClient() as client:
        response = await client.post(
            GOOGLE_TOKEN_URL,
            data={
                "client_id": config.client_id,
                "client_secret": config.client_secret,
                "code": code,
                "grant_type": "authorization_code",
                "redirect_uri": config.redirect_uri
            }
        )

        if response.status_code != 200:
            raise HTTPException(400, "Token exchange failed")

        return response.json()

async def get_google_userinfo(access_token: str) -> dict:
    """Get user info from Google"""
    async with AsyncClient() as client:
        response = await client.get(
            GOOGLE_USERINFO_URL,
            headers={"Authorization": f"Bearer {access_token}"}
        )

        if response.status_code != 200:
            raise HTTPException(400, "Failed to get user info")

        return response.json()

def create_app_token(user_data: dict) -> str:
    """Create application JWT token"""
    payload = {
        "sub": user_data["id"],
        "email": user_data["email"],
        "name": user_data["name"],
        "provider": "google",
        "exp": datetime.utcnow() + timedelta(days=7)
    }

    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)

@app.get("/auth/google/login")
async def google_login():
    """Generate Google OAuth URL with state"""
    state = secrets.token_urlsafe(32)
    oauth_states[state] = {"created_at": datetime.utcnow().isoformat()}

    from urllib.parse import urlencode
    params = {
        "client_id": config.client_id,
        "redirect_uri": config.redirect_uri,
        "response_type": "code",
        "scope": "openid email profile",
        "state": state,
        "access_type": "offline",
        "prompt": "consent"
    }

    return {
        "url": f"{GOOGLE_AUTH_URL}?{urlencode(params)}",
        "state": state
    }

@app.get("/auth/google/callback")
async def google_callback(code: str, state: str):
    """Handle Google OAuth callback"""
    # Verify state
    if state not in oauth_states:
        raise HTTPException(400, "Invalid state parameter")

    del oauth_states[state]

    # Exchange code
    token_data = await exchange_code_for_token(code)

    # Get user info
    user_data = await get_google_userinfo(token_data["access_token"])

    # Create app token
    app_token = create_app_token(user_data)

    return {
        "access_token": app_token,
        "token_type": "bearer",
        "user": {
            "id": user_data["id"],
            "email": user_data["email"],
            "name": user_data["name"]
        }
    }

async def verify_token(credentials: HTTPAuthorizationCredentials = Depends(HTTPBearer())):
    """Verify app JWT token"""
    try:
        payload = jwt.decode(
            credentials.credentials,
            SECRET_KEY,
            algorithms=[ALGORITHM]
        )
        return payload
    except JWTError:
        raise HTTPException(401, "Invalid token")

@app.get("/profile")
async def get_profile(user: dict = Depends(verify_token)):
    """Get user profile (protected)"""
    return {
        "id": user["sub"],
        "email": user["email"],
        "name": user["name"],
        "provider": user["provider"]
    }
```

## Best Practices

### Security Guidelines

```python
# Example 3: Google OAuth best practices
"""
Google OAuth Best Practices:

1. Always validate state parameter (CSRF protection)
2. Use HTTPS in production
3. Store client secret securely
4. Request minimal scopes
5. Verify ID tokens if using OpenID Connect
6. Handle refresh tokens properly
7. Implement proper error handling
"""

from fastapi import FastAPI
from httpx import AsyncClient
from jose import jwt

app = FastAPI()

# 1. Validate ID token (OpenID Connect)
async def verify_google_id_token(id_token: str) -> dict:
    """
    Verify Google ID token signature.
    More secure than just using access token.
    """
    async with AsyncClient() as client:
        response = await client.get(
            f"https://oauth2.googleapis.com/tokeninfo?id_token={id_token}"
        )

        if response.status_code != 200:
            raise HTTPException(401, "Invalid ID token")

        token_data = response.json()

        # Verify audience
        if token_data.get("aud") != GOOGLE_CLIENT_ID:
            raise HTTPException(401, "Invalid token audience")

        # Verify issuer
        if token_data.get("iss") not in [
            "https://accounts.google.com",
            "accounts.google.com"
        ]:
            raise HTTPException(401, "Invalid token issuer")

        return token_data

# 2. Handle refresh tokens
class GoogleTokenManager:
    def __init__(self):
        self.refresh_tokens: dict[str, str] = {}

    def store_refresh_token(self, user_id: str, refresh_token: str):
        """Store refresh token for user"""
        self.refresh_tokens[user_id] = refresh_token

    async def refresh_access_token(self, user_id: str) -> str:
        """Get new access token using refresh token"""
        refresh_token = self.refresh_tokens.get(user_id)

        if not refresh_token:
            raise HTTPException(401, "No refresh token")

        async with AsyncClient() as client:
            response = await client.post(
                GOOGLE_TOKEN_URL,
                data={
                    "client_id": GOOGLE_CLIENT_ID,
                    "client_secret": GOOGLE_CLIENT_SECRET,
                    "refresh_token": refresh_token,
                    "grant_type": "refresh_token"
                }
            )

            if response.status_code != 200:
                raise HTTPException(401, "Token refresh failed")

            return response.json()["access_token"]
```

## Summary

| Component | Purpose | Implementation |
|-----------|---------|----------------|
| Login URL | Initiate flow | `GET /auth/google/login` |
| Callback | Handle response | `GET /auth/google/callback` |
| Token Exchange | Get access token | POST to Google token URL |
| User Info | Get profile | GET from Google userinfo |
| App Token | Session management | JWT for your app |

## Next Steps

Continue learning about:
- [GitHub OAuth](./02_github_oauth.md) - GitHub authentication
- [Microsoft OAuth](./03_microsoft_oauth.md) - Microsoft authentication
- [OpenID Connect](./05_openid_connect.md) - OIDC protocol
