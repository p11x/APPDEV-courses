# OAuth2 with FastAPI

## What You'll Learn
- Implementing OAuth2 in FastAPI
- Using OAuth2PasswordBearer and OAuth2PasswordRequestForm
- OAuth2 with Google and GitHub
- Scope-based permissions
- Token refresh implementation

## Prerequisites
- Completed `01-oauth2-deep-dive.md` — OAuth2 fundamentals
- Understanding of FastAPI dependency injection
- Basic JWT knowledge

## FastAPI's OAuth2 Support

FastAPI provides built-in OAuth2 support with `OAuth2PasswordBearer` and dependency injection:

```python
from fastapi import FastAPI, Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer, OAuth2PasswordRequestForm
from pydantic import BaseModel
from datetime import datetime, timedelta
import jwt

app = FastAPI()

# OAuth2 scheme - extracts Bearer token from Authorization header
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

# Secret key for JWT signing
SECRET_KEY = "your-secret-key-change-in-production"
ALGORITHM = "HS256"

# Models
class Token(BaseModel):
    access_token: str
    token_type: str

class TokenData(BaseModel):
    username: str | None = None
```

🔍 **Line-by-Line Breakdown:**
1. `OAuth2PasswordBearer` — Security scheme that extracts token from header
2. `tokenUrl="token"` — URL endpoint that issues tokens
3. `oauth2_scheme(token)` — Dependency that returns the token string

## Implementing Password Authentication

```python
# In-memory user database (use real DB in production)
users_db = {
    "johndoe": {
        "username": "johndoe",
        "full_name": "John Doe",
        "hashed_password": "fakehashedsecret",
        "disabled": False,
    }
}

def verify_password(plain_password: str, hashed_password: str) -> bool:
    """Verify password against hash."""
    # In production, use passlib:
    # return pwd_context.verify(plain_password, hashed_password)
    return plain_password == hashed_password

def get_user(username: str) -> dict | None:
    """Get user from database."""
    return users_db.get(username)

def authenticate_user(username: str, password: str) -> bool:
    """Authenticate user credentials."""
    user = get_user(username)
    if not user:
        return False
    if not verify_password(password, user["hashed_password"]):
        return False
    return True

def create_access_token(data: dict, expires_delta: timedelta = None) -> str:
    """Create JWT access token."""
    to_encode = data.copy()
    
    if expires_delta:
        expire = datetime.utcnow() + expires_delta
    else:
        expire = datetime.utcnow() + timedelta(minutes=15)
    
    to_encode.update({"exp": expire})
    encoded_jwt = jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)
    return encoded_jwt

# Token endpoint
@app.post("/token", response_model=Token)
async def login(form_data: OAuth2PasswordRequestForm = Depends()) -> dict:
    """OAuth2 compatible token endpoint."""
    
    user = get_user(form_data.username)
    
    if not user:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Incorrect username or password",
            headers={"WWW-Authenticate": "Bearer"},
        )
    
    if not authenticate_user(form_data.username, form_data.password):
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Incorrect username or password",
            headers={"WWW-Authenticate": "Bearer"},
        )
    
    # Create token
    access_token_expires = timedelta(minutes=30)
    access_token = create_access_token(
        data={"sub": user["username"]}, 
        expires_delta=access_token_expires
    )
    
    return {"access_token": access_token, "token_type": "bearer"}
```

## Protected Routes

```python
# Dependency to get current user from token
async def get_current_user(token: str = Depends(oauth2_scheme)) -> dict:
    """Validate token and return user."""
    
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="Could not validate credentials",
        headers={"WWW-Authenticate": "Bearer"},
    )
    
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
        username: str = payload.get("sub")
        
        if username is None:
            raise credentials_exception
            
    except jwt.ExpiredSignatureError:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Token has expired",
            headers={"WWW-Authenticate": "Bearer"},
        )
    except jwt.JWTError:
        raise credentials_exception
    
    user = get_user(username)
    if user is None:
        raise credentials_exception
    
    return user

# Protected endpoint
@app.get("/users/me")
async def read_users_me(current_user: dict = Depends(get_current_user)) -> dict:
    """Get current user info."""
    return current_user

@app.get("/items")
async def read_items(current_user: dict = Depends(get_current_user)) -> list:
    """Protected endpoint - requires valid token."""
    return [
        {"item_id": 1, "owner": current_user["username"]},
        {"item_id": 2, "owner": current_user["username"]},
    ]
```

## OAuth2 with Google

```bash
pip install httpx authlib
```

```python
# oauth.py
from fastapi import FastAPI, Depends, HTTPException, status
from fastapi.security import OAuth2
from authlib.integrations.httpx_client import OAuth2Client
from pydantic import BaseModel

app = FastAPI()

# Google OAuth2 settings
GOOGLE_CLIENT_ID = "your-google-client-id"
GOOGLE_CLIENT_SECRET = "your-google-client-secret"
GOOGLE_REDIRECT_URI = "http://localhost:8000/auth/google/callback"

# OAuth2 scheme for Google
oauth2_scheme = OAuth2()

GOOGLE_AUTH_URL = "https://accounts.google.com/o/oauth2/v2/auth"
GOOGLE_TOKEN_URL = "https://oauth2.googleapis.com/token"
GOOGLE_USERINFO_URL = "https://www.googleapis.com/oauth2/v2/userinfo"

@app.get("/auth/google")
async def login_google():
    """Redirect to Google OAuth2."""
    import secrets
    
    state = secrets.token_urlsafe(32)
    
    auth_url = (
        f"{GOOGLE_AUTH_URL}?"
        f"client_id={GOOGLE_CLIENT_ID}&"
        f"redirect_uri={GOOGLE_REDIRECT_URI}&"
        f"response_type=code&"
        f"scope=openid%20email%20profile&"
        f"state={state}"
    )
    
    return {"authorization_url": auth_url}

@app.get("/auth/google/callback")
async def google_callback(code: str, state: str):
    """Exchange code for token."""
    
    # Exchange code for token
    async with OAuth2Client() as client:
        token_response = await client.post(
            GOOGLE_TOKEN_URL,
            params={
                "code": code,
                "client_id": GOOGLE_CLIENT_ID,
                "client_secret": GOOGLE_CLIENT_SECRET,
                "redirect_uri": GOOGLE_REDIRECT_URI,
                "grant_type": "authorization_code",
            },
        )
    
    tokens = token_response.json()
    access_token = tokens["access_token"]
    
    # Get user info
    async with OAuth2Client() as client:
        userinfo_response = await client.get(
            GOOGLE_USERINFO_URL,
            token=access_token,
        )
    
    userinfo = userinfo_response.json()
    
    return {
        "access_token": access_token,
        "user": userinfo,
    }
```

## Scope-Based Permissions

```python
from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer, SecurityScopes
from pydantic import BaseModel

oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token", scopes={"me": "Read user info", "items": "Read items"})

class User(BaseModel):
    username: str
    email: str | None = None
    full_name: str | None = None
    scopes: list[str] = []

async def get_current_user(
    security_scopes: SecurityScopes,
    token: str = Depends(oauth2_scheme)
) -> User:
    """Validate token and check required scopes."""
    
    if security_scopes.scopes:
        authenticate_value = f'Bearer scope="{security_scopes.scope_str}"'
    else:
        authenticate_value = "Bearer"
    
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="Could not validate credentials",
        headers={"WWW-Authenticate": authenticate_value},
    )
    
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
        username: str = payload.get("sub")
        
        if username is None:
            raise credentials_exception
        
        # Get scopes from token
        token_scopes = payload.get("scopes", [])
        
    except jwt.JWTError:
        raise credentials_exception
    
    # Verify required scopes
    for scope in security_scopes.scopes:
        if scope not in token_scopes:
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN,
                detail=f"Not enough permissions. Required: {scope}",
            )
    
    return User(username=username, scopes=token_scopes)

# Endpoints with scope requirements
@app.get("/users/me", response_model=User)
async def read_users_me(current_user: User = Security(get_current_user)) -> User:
    """Get current user - requires 'me' scope."""
    return current_user

@app.get("/items")
async def read_items(current_user: User = Security(get_current_user)) -> list:
    """Read items - requires 'items' scope."""
    return [{"item_id": 1, "owner": current_user.username}]

@app.get("/admin/users")
async def read_system_users(
    current_user: User = Security(get_current_user)
) -> list:
    """Admin endpoint - requires 'admin' scope."""
    if "admin" not in current_user.scopes:
        raise HTTPException(status_code=403, detail="Admin access required")
    
    return [{"username": "admin"}]
```

## Token Refresh

```python
from fastapi import Depends, HTTPException, status
from datetime import datetime, timedelta

# Store refresh tokens (use Redis in production)
refresh_tokens_db: dict[str, dict] = {}

@app.post("/token/refresh")
async def refresh_access_token(refresh_token: str) -> dict:
    """Get new access token using refresh token."""
    
    if refresh_token not in refresh_tokens_db:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Invalid refresh token",
        )
    
    # Get stored token data
    token_data = refresh_tokens_db[refresh_token]
    
    # Create new access token
    access_token_expires = timedelta(minutes=30)
    new_access_token = create_access_token(
        data={"sub": token_data["username"], "scopes": token_data["scopes"]},
        expires_delta=access_token_expires
    )
    
    return {
        "access_token": new_access_token,
        "token_type": "bearer"
    }

# Modified login to include refresh token
@app.post("/token", response_model=Token)
async def login(form_data: OAuth2PasswordRequestForm = Depends()):
    """Login and return both access and refresh tokens."""
    
    user = authenticate_user(form_data.username, form_data.password)
    
    if not user:
        raise HTTPException(status_code=401, detail="Invalid credentials")
    
    # Create tokens
    access_token = create_access_token(
        data={"sub": user["username"], "scopes": ["me", "items"]}
    )
    
    refresh_token = secrets.token_urlsafe(32)
    
    # Store refresh token
    refresh_tokens_db[refresh_token] = {
        "username": user["username"],
        "scopes": ["me", "items"]
    }
    
    return {
        "access_token": access_token,
        "refresh_token": refresh_token,
        "token_type": "bearer"
    }
```

## Production Considerations

- **HTTPS**: Always use HTTPS in production
- **Secure storage**: Store secrets in environment variables
- **Token expiration**: Short-lived access tokens (15-60 min)
- **Refresh tokens**: Use rotation (invalidate old on new)
- **Scope validation**: Verify scopes on every request

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Hardcoding secrets

**Wrong:**
```python
SECRET_KEY = "my-secret-key"  # Hardcoded!
```

**Why it fails:** Anyone can decode tokens.

**Fix:**
```python
import os
SECRET_KEY = os.environ.get("SECRET_KEY")  # From environment
```

### ❌ Mistake 2: Not handling token expiration

**Wrong:**
```python
def get_user(token):
    payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
    return payload  # Doesn't handle expired tokens!
```

**Why it fails:** Expired tokens cause unhandled exceptions.

**Fix:**
```python
def get_user(token):
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
    except jwt.ExpiredSignatureError:
        raise HTTPException(401, "Token expired")
```

### ❌ Mistake 3: Not checking scopes

**Wrong:**
```python
@app.get("/admin")
async def admin_panel():
    return {"admin": True}  # Anyone can access!
```

**Why it fails:** No permission check.

**Fix:**
```python
@app.get("/admin")
async def admin_panel(user: User = Security(get_current_user, scopes=["admin"])):
    if "admin" not in user.scopes:
        raise HTTPException(403, "Admin only")
    return {"admin": True}
```

## Summary

- FastAPI provides built-in OAuth2 support with `OAuth2PasswordBearer`
- Use `OAuth2PasswordRequestForm` for standard token endpoint
- Implement scope-based permissions for fine-grained access control
- Always use environment variables for secrets
- Implement token refresh for better UX

## Next Steps

→ Continue to `03-rbac-with-fastapi.md` to implement Role-Based Access Control.
