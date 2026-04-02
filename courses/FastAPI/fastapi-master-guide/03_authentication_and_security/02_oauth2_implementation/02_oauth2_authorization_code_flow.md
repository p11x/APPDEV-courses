# OAuth2 Authorization Code Flow

## Overview

The Authorization Code flow is the most secure OAuth2 flow for web applications. It redirects users to the authorization server and exchanges an authorization code for tokens.

## Implementation

### Authorization Code Flow

```python
# Example 1: Complete Authorization Code flow
from fastapi import FastAPI, HTTPException, Query, Request
from fastapi.responses import RedirectResponse, HTMLResponse
import httpx
import secrets
from urllib.parse import urlencode

app = FastAPI()

# OAuth2 Configuration
CLIENT_ID = "your-client-id"
CLIENT_SECRET = "your-client-secret"
AUTH_URL = "https://provider.com/authorize"
TOKEN_URL = "https://provider.com/token"
REDIRECT_URI = "http://localhost:8000/callback"

# Store state for CSRF protection
oauth_states: dict = {}

@app.get("/login")
async def login():
    """Initiate OAuth2 authorization code flow"""
    state = secrets.token_urlsafe(32)
    oauth_states[state] = True

    params = {
        "client_id": CLIENT_ID,
        "redirect_uri": REDIRECT_URI,
        "response_type": "code",
        "scope": "openid profile email",
        "state": state,
        "access_type": "offline"  # Get refresh token
    }

    auth_url = f"{AUTH_URL}?{urlencode(params)}"
    return RedirectResponse(url=auth_url)

@app.get("/callback")
async def callback(code: str = Query(...), state: str = Query(...)):
    """Handle OAuth2 callback"""
    # Verify state
    if state not in oauth_states:
        raise HTTPException(400, "Invalid state")
    del oauth_states[state]

    # Exchange code for token
    async with httpx.AsyncClient() as client:
        response = await client.post(TOKEN_URL, data={
            "grant_type": "authorization_code",
            "code": code,
            "redirect_uri": REDIRECT_URI,
            "client_id": CLIENT_ID,
            "client_secret": CLIENT_SECRET
        })

    if response.status_code != 200:
        raise HTTPException(400, "Token exchange failed")

    token_data = response.json()

    return {
        "access_token": token_data["access_token"],
        "token_type": token_data["token_type"],
        "expires_in": token_data.get("expires_in")
    }
```

## Best Practices

1. Always validate state parameter
2. Use HTTPS in production
3. Store tokens securely
4. Implement token refresh

## Summary

Authorization Code flow provides the best security for web applications by keeping tokens server-side.

## Next Steps

Continue learning about:
- [Client Credentials Flow](./03_oauth2_client_credentials_flow.md)
- [Token Validation](./06_oauth2_token_validation.md)
