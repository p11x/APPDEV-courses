# OAuth2 Deep Dive

## What You'll Learn
- OAuth2 flow and grant types
- Authorization code flow implementation
- Client credentials flow
- Token refresh mechanisms
- Security best practices

## Prerequisites
- Completed basic authentication — Understanding of JWT, sessions
- Understanding of HTTP requests

## What Is OAuth2?

OAuth2 is an **authorization framework** that lets third-party applications access user resources without sharing passwords. Think of it as a **valet key** for your digital life.

```
Traditional:     App ──▶ User Password ──▶ Resource
OAuth2:          App ──▶ Limited Key (Token) ──▶ Resource
```

## OAuth2 Roles

| Role | Description |
|------|-------------|
| **Resource Owner** | The user who owns the data |
| **Client** | The application requesting access |
| **Authorization Server** | Issues tokens after user consent |
| **Resource Server** | Hosts protected resources (your API) |

## Grant Types

### 1. Authorization Code Flow (Web/Mobile Apps)

Best for: Web apps, mobile apps — most secure

```
┌─────────────────────────────────────────────────────────────────┐
│  Authorization Code Flow                                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  1. User clicks "Login with Google"                             │
│       │                                                         │
│       ▼                                                         │
│  2. App redirects to Authorization Server                      │
│     GET /authorize?client_id=APP&                               │
│              redirect_uri=CALLBACK&                              │
│              response_type=code&                                │
│              scope=read:profile                                 │
│       │                                                         │
│       ▼                                                         │
│  3. User sees consent screen, clicks "Allow"                  │
│       │                                                         │
│       ▼                                                         │
│  4. Auth Server redirects to callback with CODE                │
│     https://myapp.com/callback?code=AUTH_CODE                   │
│       │                                                         │
│       ▼                                                         │
│  5. App exchanges CODE for TOKEN                               │
│     POST /token                                                 │
│       - grant_type=authorization_code                           │
│       - code=AUTH_CODE                                          │
│       - client_id=APP                                           │
│       - client_secret=SECRET                                    │
│       │                                                         │
│       ▼                                                         │
│  6. Auth Server returns TOKEN                                   │
│     { "access_token": "...", "refresh_token": "..." }          │
│       │                                                         │
│       ▼                                                         │
│  7. App uses TOKEN to access API                                │
│     GET /api/user                                               │
│       Header: Authorization: Bearer TOKEN                       │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Implementing Authorization Code Flow

```python
# settings.py
SOCIAL_AUTH_GOOGLE_OAUTH2_KEY = 'your-google-client-id'
SOCIAL_AUTH_GOOGLE_OAUTH2_SECRET = 'your-google-client-secret'

# urls.py
from django.urls import path
from . import views

urlpatterns = [
    path("authorize/", views.authorize, name="authorize"),
    path("callback/", views.oauth_callback, name="oauth_callback"),
    path("token/", views.token, name="token"),
]
```

```python
# views.py
from django.shortcuts import render, redirect
from django.http import JsonResponse, HttpResponse
from django.conf import settings
import secrets
import requests

# Store authorization codes temporarily (use Redis in production)
auth_codes = {}

def authorize(request):
    """Step 1 & 2: Redirect to authorization server."""
    
    client_id = "my_app"
    redirect_uri = "http://localhost:8000/callback/"
    scope = "profile email"
    state = secrets.token_urlsafe(32)  # CSRF protection
    
    # Store state for verification
    request.session["oauth_state"] = state
    
    # Redirect to authorization server (e.g., Google)
    auth_url = (
        f"https://accounts.google.com/o/oauth2/v2/auth?"
        f"client_id={client_id}&"
        f"redirect_uri={redirect_uri}&"
        f"response_type=code&"
        f"scope={scope}&"
        f"state={state}"
    )
    
    return redirect(auth_url)

def oauth_callback(request):
    """Step 3 & 4: Handle authorization code."""
    
    error = request.GET.get("error")
    if error:
        return JsonResponse({"error": error}, status=400)
    
    code = request.GET.get("code")
    state = request.GET.get("state")
    
    # Verify state to prevent CSRF
    if state != request.session.get("oauth_state"):
        return JsonResponse({"error": "Invalid state"}, status=400)
    
    # Step 5: Exchange code for token
    token_response = requests.post("https://oauth2.googleapis.com/token", {
        "code": code,
        "client_id": "my_app",
        "client_secret": "my_secret",
        "redirect_uri": "http://localhost:8000/callback/",
        "grant_type": "authorization_code",
    })
    
    tokens = token_response.json()
    
    # Store tokens
    request.session["access_token"] = tokens["access_token"]
    request.session["refresh_token"] = tokens.get("refresh_token")
    
    return redirect("dashboard")

def token(request):
    """Step 6: API returns access token."""
    
    return JsonResponse({
        "access_token": request.session.get("access_token"),
        "token_type": "Bearer",
    })
```

🔍 **Line-by-Line Breakdown:**
1. `secrets.token_urlsafe(32)` — Generates cryptographically secure random string
2. `state` parameter — Prevents CSRF attacks by verifying the same state
3. `requests.post()` — Exchanges authorization code for access token
4. `tokens["access_token"]` — Used in Authorization header for API calls

### 2. Client Credentials Flow (Server-to-Server)

Best for: Machine-to-machine communication, no user involvement

```python
# Token endpoint for client credentials
def client_credentials_token(request):
    """Get token using client credentials (no user involved)."""
    
    # Verify client credentials
    client_id = request.POST.get("client_id")
    client_secret = request.POST.get("client_secret")
    
    if not verify_client(client_id, client_secret):
        return JsonResponse({"error": "invalid_client"}, status=401)
    
    # Generate token
    access_token = generate_token()
    
    return JsonResponse({
        "access_token": access_token,
        "token_type": "Bearer",
        "expires_in": 3600
    })

# Using the token
def protected_api(request):
    """Access protected resource."""
    
    auth_header = request.headers.get("Authorization", "")
    
    if not auth_header.startswith("Bearer "):
        return JsonResponse({"error": "invalid_token"}, status=401)
    
    token = auth_header[7:]  # Remove "Bearer "
    
    if not validate_token(token):
        return JsonResponse({"error": "invalid_token"}, status=401)
    
    return JsonResponse({"data": "protected content"})
```

### 3. Refresh Token Flow

```python
def refresh_token(request):
    """Get new access token using refresh token."""
    
    refresh_token = request.POST.get("refresh_token")
    
    # Verify refresh token
    if not verify_refresh_token(refresh_token):
        return JsonResponse({"error": "invalid_grant"}, status=400)
    
    # Generate new tokens
    new_access_token = generate_token()
    new_refresh_token = generate_token()  # Rotation
    
    # Invalidate old refresh token
    invalidate_token(refresh_token)
    
    return JsonResponse({
        "access_token": new_access_token,
        "refresh_token": new_refresh_token,
        "token_type": "Bearer",
        "expires_in": 3600
    })
```

## Implementing OAuth2 with Python Libraries

### Using Authlib (Recommended)

```bash
pip install authlib
```

```python
# oauth.py
from authlib.integrations.django_client import DjangoOAuth2Client
from authlib.oauth2.auth import AuthCodeOAuth2Session
from django.conf import settings

# Configuration
GOOGLE_CLIENT_ID = "your-client-id"
GOOGLE_CLIENT_SECRET = "your-client-secret"

def get_google_login_url():
    """Get Google OAuth2 authorization URL."""
    oauth = DjangoOAuth2Client(
        client_id=GOOGLE_CLIENT_ID,
        client_secret=GOOGLE_CLIENT_SECRET,
        redirect_uri="http://localhost:8000/callback/",
    )
    
    return oauth.authorize_redirect(
        "https://accounts.google.com/o/oauth2/v2/auth",
        scope="openid email profile"
    )

def get_google_user(token):
    """Get user info from Google using access token."""
    response = requests.get(
        "https://www.googleapis.com/oauth2/v2/userinfo",
        headers={"Authorization": f"Bearer {token}"}
    )
    return response.json()
```

### Using PyJWT for Token Validation

```bash
pip install pyjwt
```

```python
# token_validation.py
import jwt
from datetime import datetime, timedelta

SECRET_KEY = "your-secret-key"
ALGORITHM = "HS256"

def create_access_token(data: dict, expires_delta: timedelta = None) -> str:
    """Create JWT access token."""
    to_encode = data.copy()
    
    if expires_delta:
        expire = datetime.utcnow() + expires_delta
    else:
        expire = datetime.utcnow() + timedelta(hours=1)
    
    to_encode.update({"exp": expire, "type": "access"})
    
    return jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)

def decode_token(token: str) -> dict:
    """Decode and validate JWT token."""
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
        return payload
    except jwt.ExpiredSignatureError:
        raise ValueError("Token has expired")
    except jwt.InvalidTokenError:
        raise ValueError("Invalid token")

def verify_token(token: str) -> bool:
    """Verify token is valid and not expired."""
    try:
        decode_token(token)
        return True
    except ValueError:
        return False
```

## Scopes

```python
# Define scopes
SCOPES = {
    "read:profile": "Read your profile information",
    "write:profile": "Update your profile",
    "read:email": "Read your email address",
}

# Request specific scopes
def authorize(request):
    scope = "read:profile write:profile read:email"
    # Build authorization URL with scopes
    ...

# Verify scopes in token
def verify_token_with_scopes(token: str, required_scope: str) -> bool:
    payload = decode_token(token)
    token_scopes = payload.get("scope", "").split()
    return required_scope in token_scopes
```

## Production Considerations

- **HTTPS**: OAuth2 requires HTTPS in production
- **State parameter**: Always use and verify state to prevent CSRF
- **Token storage**: Store securely (httpOnly cookies for web, secure storage for mobile)
- **Token expiration**: Use short-lived access tokens (15-60 minutes)
- **Refresh tokens**: Use rotation (new refresh token with each use)

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not using state parameter

**Wrong:**
```python
# No state!
auth_url = f"https://auth.server/authorize?client_id=..."
```

**Why it fails:** Vulnerable to CSRF attacks.

**Fix:**
```python
state = secrets.token_urlsafe(32)
auth_url = f"https://auth.server/authorize?client_id=...&state={state}"
```

### ❌ Mistake 2: Exposing client secret

**Wrong:**
```python
# In frontend JavaScript!
const clientSecret = "my_secret";  // WRONG!
```

**Why it fails:** Anyone can steal the secret.

**Fix:**
```python
# Only use client secret server-side in token exchange
def get_token(code):
    # This happens on your server, not in browser
    response = requests.post(token_url, {
        "client_secret": settings.CLIENT_SECRET,  # Server-side only
        ...
    })
```

### ❌ Mistake 3: Not validating tokens

**Wrong:**
```python
def api_view(request):
    token = request.headers.get("Authorization")
    # Just extract without validation!
    user_id = extract_claim(token)
```

**Why it fails:** Any forged token works!

**Fix:**
```python
def api_view(request):
    token = request.headers.get("Authorization", "")[7:]  # Remove "Bearer "
    
    if not verify_token(token):  # Always verify!
        return JsonResponse({"error": "Invalid"}, status=401)
    
    payload = decode_token(token)
    user_id = payload.get("sub")
```

## Summary

- OAuth2 lets third parties access user data without passwords
- Authorization Code flow is best for user-facing applications
- Client Credentials flow is for server-to-server communication
- Always use state parameter to prevent CSRF
- Implement token refresh for better security

## Next Steps

→ Continue to `02-oauth2-with-fastapi.md` to implement OAuth2 in FastAPI.
