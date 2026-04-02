# Microsoft OAuth

## Overview

Microsoft OAuth enables authentication via Microsoft accounts (Outlook, Azure AD) for enterprise applications.

## Implementation

```python
# Example 1: Microsoft OAuth integration
from fastapi import FastAPI, HTTPException
from fastapi.responses import RedirectResponse
import httpx
from jose import jwt
from datetime import datetime, timedelta

app = FastAPI()

# Microsoft OAuth configuration
MS_CLIENT_ID = "your-azure-app-id"
MS_CLIENT_SECRET = "your-azure-secret"
MS_TENANT_ID = "your-tenant-id"  # or "common" for multi-tenant
MS_AUTHORIZE_URL = f"https://login.microsoftonline.com/{MS_TENANT_ID}/oauth2/v2.0/authorize"
MS_TOKEN_URL = f"https://login.microsoftonline.com/{MS_TENANT_ID}/oauth2/v2.0/token"
MS_USER_URL = "https://graph.microsoft.com/v1.0/me"
REDIRECT_URI = "http://localhost:8000/auth/microsoft/callback"

SECRET_KEY = "your-secret-key"

@app.get("/auth/microsoft/login")
async def microsoft_login():
    """Initiate Microsoft OAuth flow"""
    url = (
        f"{MS_AUTHORIZE_URL}"
        f"?client_id={MS_CLIENT_ID}"
        f"&response_type=code"
        f"&redirect_uri={REDIRECT_URI}"
        f"&scope=openid profile email User.Read"
        f"&response_mode=query"
    )
    return RedirectResponse(url=url)

@app.get("/auth/microsoft/callback")
async def microsoft_callback(code: str):
    """Handle Microsoft OAuth callback"""
    async with httpx.AsyncClient() as client:
        # Exchange code for token
        token_response = await client.post(
            MS_TOKEN_URL,
            data={
                "client_id": MS_CLIENT_ID,
                "client_secret": MS_CLIENT_SECRET,
                "code": code,
                "redirect_uri": REDIRECT_URI,
                "grant_type": "authorization_code"
            }
        )

        token_data = token_response.json()
        access_token = token_data.get("access_token")

        if not access_token:
            raise HTTPException(400, "Failed to get access token")

        # Get user info
        user_response = await client.get(
            MS_USER_URL,
            headers={"Authorization": f"Bearer {access_token}"}
        )

        ms_user = user_response.json()

    # Create app token
    app_token = jwt.encode(
        {
            "sub": ms_user["id"],
            "username": ms_user.get("displayName"),
            "email": ms_user.get("mail") or ms_user.get("userPrincipalName"),
            "provider": "microsoft",
            "exp": datetime.utcnow() + timedelta(days=7)
        },
        SECRET_KEY,
        algorithm="HS256"
    )

    return {
        "access_token": app_token,
        "token_type": "bearer",
        "user": {
            "id": ms_user["id"],
            "name": ms_user.get("displayName"),
            "email": ms_user.get("mail")
        }
    }
```

## Summary

Microsoft OAuth is ideal for enterprise applications using Azure AD.

## Next Steps

Continue learning about:
- [OpenID Connect](./05_openid_connect.md)
- [SSO Implementation](./06_sso_implementation.md)
