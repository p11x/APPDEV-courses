# API Security Best Practices

## What You'll Learn
- API authentication patterns
- Rate limiting for APIs
- CORS configuration
- API versioning security

## Prerequisites
- Completed input validation

## API Key Authentication

```python
from fastapi import FastAPI, Header, HTTPException
from typing import Optional

app = FastAPI()

# Simple API key validation
API_KEYS = {
    "key_123": {"name": "Client A", "rate_limit": 100},
    "key_456": {"name": "Client B", "rate_limit": 50},
}

async def verify_api_key(x_api_key: Optional[str] = Header(None)) -> dict:
    """Verify API key from header"""
    if not x_api_key:
        raise HTTPException(status_code=401, detail="API key required")
    
    if x_api_key not in API_KEYS:
        raise HTTPException(status_code=403, detail="Invalid API key")
    
    return API_KEYS[x_api_key]

@app.get("/secure-data")
async def get_secure_data(api_key_info: dict = verify_api_key):
    return {
        "data": "secret information",
        "client": api_key_info["name"]
    }
```

## OAuth2 with FastAPI

```python
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import OAuth2PasswordBearer, OAuth2PasswordRequestForm

app = FastAPI()
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

# Token validation
async def get_current_user(token: str = Depends(oauth2_scheme)) -> dict:
    """Validate JWT token"""
    # In production, decode and verify JWT
    return {"username": "user", "id": 1}

@app.get("/protected")
async def protected_route(user: dict = Depends(get_current_user)):
    return {"message": f"Hello {user['username']}"}

@app.post("/token")
async def login(form_data: OAuth2PasswordRequestForm = Depends()):
    """Login endpoint"""
    if form_data.username == "admin" and form_data.password == "password":
        return {"access_token": "fake_token", "token_type": "bearer"}
    raise HTTPException(status_code=401, detail="Invalid credentials")
```

## CORS Configuration

```python
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["https://example.com"],  # Specific origins
    allow_credentials=True,
    allow_methods=["GET", "POST"],  # Specific methods
    allow_headers=["Authorization", "Content-Type"],
)

# For development (be careful in production!)
# allow_origins=["*"]  # Allow all origins
```

## Rate Limiting for APIs

```python
from fastapi import FastAPI, Request
from fastapi.responses import Response
import time
from collections import defaultdict

app = FastAPI()

# Simple rate limiter
requests: dict = defaultdict(list)

@app.middleware("http")
async def rate_limiter(request: Request, call_next):
    client_id = request.headers.get("X-API-Key", request.client.host)
    now = time.time()
    
    # Clean old requests
    requests[client_id] = [t for t in requests[client_id] if now - t < 60]
    
    if len(requests[client_id]) >= 60:  # 60 requests per minute
        return Response(
            content='{"error": "Rate limit exceeded"}',
            status_code=429,
            media_type="application/json"
        )
    
    requests[client_id].append(now)
    return await call_next(request)
```

## API Versioning

```python
from fastapi import FastAPI, APIRouter

# Version 1
app_v1 = APIRouter(prefix="/v1")

@app_v1.get("/users")
async def get_users_v1():
    return {"version": "v1", "users": []}

# Version 2
app_v2 = APIRouter(prefix="/v2")

@app_v2.get("/users")
async def get_users_v2():
    return {"version": "v2", "users": [], "total": 0}

# Main app
app = FastAPI()
app.include_router(app_v1)
app.include_router(app_v2)
```

## Summary
- Use API keys or OAuth2 for authentication
- Configure CORS carefully
- Implement rate limiting
- Version your APIs

## Next Steps
→ Continue to `07-security-headers-and-https.md`
