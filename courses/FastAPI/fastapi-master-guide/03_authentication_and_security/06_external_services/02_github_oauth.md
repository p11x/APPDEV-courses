# GitHub OAuth

## Overview

GitHub OAuth enables "Sign in with GitHub" functionality in FastAPI applications.

## Implementation

```python
# Example 1: GitHub OAuth integration
from fastapi import FastAPI, HTTPException
from fastapi.responses import RedirectResponse
import httpx
from jose import jwt
from datetime import datetime, timedelta

app = FastAPI()

# GitHub OAuth configuration
GITHUB_CLIENT_ID = "your-github-client-id"
GITHUB_CLIENT_SECRET = "your-github-client-secret"
GITHUB_AUTHORIZE_URL = "https://github.com/login/oauth/authorize"
GITHUB_TOKEN_URL = "https://github.com/login/oauth/access_token"
GITHUB_USER_URL = "https://api.github.com/user"
REDIRECT_URI = "http://localhost:8000/auth/github/callback"

SECRET_KEY = "your-secret-key"

@app.get("/auth/github/login")
async def github_login():
    """Initiate GitHub OAuth flow"""
    url = (
        f"{GITHUB_AUTHORIZE_URL}"
        f"?client_id={GITHUB_CLIENT_ID}"
        f"&redirect_uri={REDIRECT_URI}"
        f"&scope=user:email"
    )
    return RedirectResponse(url=url)

@app.get("/auth/github/callback")
async def github_callback(code: str):
    """Handle GitHub OAuth callback"""
    async with httpx.AsyncClient() as client:
        # Exchange code for token
        token_response = await client.post(
            GITHUB_TOKEN_URL,
            data={
                "client_id": GITHUB_CLIENT_ID,
                "client_secret": GITHUB_CLIENT_SECRET,
                "code": code,
                "redirect_uri": REDIRECT_URI
            },
            headers={"Accept": "application/json"}
        )

        token_data = token_response.json()
        access_token = token_data.get("access_token")

        if not access_token:
            raise HTTPException(400, "Failed to get access token")

        # Get user info
        user_response = await client.get(
            GITHUB_USER_URL,
            headers={"Authorization": f"Bearer {access_token}"}
        )

        github_user = user_response.json()

    # Create app token
    app_token = jwt.encode(
        {
            "sub": str(github_user["id"]),
            "username": github_user["login"],
            "email": github_user.get("email"),
            "provider": "github",
            "exp": datetime.utcnow() + timedelta(days=7)
        },
        SECRET_KEY,
        algorithm="HS256"
    )

    return {
        "access_token": app_token,
        "token_type": "bearer",
        "user": {
            "id": github_user["id"],
            "username": github_user["login"],
            "avatar": github_user["avatar_url"]
        }
    }
```

## Summary

GitHub OAuth provides simple authentication for developer-focused applications.

## Next Steps

Continue learning about:
- [Microsoft OAuth](./03_microsoft_oauth.md)
- [OpenID Connect](./05_openid_connect.md)
