# API Keys Authentication

## Overview

API keys provide simple authentication for service-to-service communication and client identification. While less secure than OAuth2, they are easy to implement and suitable for many use cases.

## Basic API Key Authentication

### Header-Based API Keys

```python
# Example 1: API key in header
from fastapi import FastAPI, Depends, HTTPException, Security
from fastapi.security import APIKeyHeader
from typing import Optional

app = FastAPI()

# Define API key header
API_KEY_HEADER = APIKeyHeader(name="X-API-Key", auto_error=False)

# Valid API keys (in production, store in database)
VALID_API_KEYS = {
    "key_abc123": {"name": "Client A", "rate_limit": 1000},
    "key_def456": {"name": "Client B", "rate_limit": 500},
}

async def verify_api_key(api_key: Optional[str] = Security(API_KEY_HEADER)):
    """
    Verify API key from X-API-Key header.
    Returns client information if valid.
    """
    if api_key is None:
        raise HTTPException(
            status_code=401,
            detail="API key required. Provide via X-API-Key header."
        )

    if api_key not in VALID_API_KEYS:
        raise HTTPException(
            status_code=403,
            detail="Invalid API key"
        )

    return VALID_API_KEYS[api_key]

@app.get("/items/")
async def list_items(client: dict = Depends(verify_api_key)):
    """
    Protected endpoint requiring API key.
    X-API-Key: key_abc123
    """
    return {
        "items": [],
        "client": client["name"]
    }

@app.get("/public/")
async def public_endpoint():
    """No authentication required"""
    return {"message": "Public endpoint"}
```

### Query Parameter API Keys

```python
# Example 2: API key in query parameter
from fastapi import FastAPI, Depends, HTTPException, Query, Security
from fastapi.security import APIKeyQuery
from typing import Optional

app = FastAPI()

# API key as query parameter
API_KEY_QUERY = APIKeyQuery(name="api_key", auto_error=False)

VALID_API_KEYS = {"secret_key_123": {"name": "Client A"}}

async def verify_query_key(api_key: Optional[str] = Security(API_KEY_QUERY)):
    """
    Verify API key from query parameter.
    URL: /items/?api_key=secret_key_123
    """
    if api_key is None:
        raise HTTPException(401, "API key required")

    if api_key not in VALID_API_KEYS:
        raise HTTPException(403, "Invalid API key")

    return VALID_API_KEYS[api_key]

@app.get("/items/")
async def list_items(client: dict = Depends(verify_query_key)):
    """
    API key via query parameter.
    Note: Less secure than header (logged in URLs).
    """
    return {"items": [], "client": client["name"]}
```

### Cookie-Based API Keys

```python
# Example 3: API key in cookie
from fastapi import FastAPI, Depends, HTTPException, Cookie, Security
from fastapi.security import APIKeyCookie
from typing import Optional

app = FastAPI()

# API key in cookie
API_KEY_COOKIE = APIKeyCookie(name="api_key", auto_error=False)

async def verify_cookie_key(api_key: Optional[str] = Security(API_KEY_COOKIE)):
    """
    Verify API key from cookie.
    Useful for browser-based applications.
    """
    if api_key is None:
        raise HTTPException(401, "API key cookie required")

    if api_key not in VALID_API_KEYS:
        raise HTTPException(403, "Invalid API key")

    return VALID_API_KEYS[api_key]

@app.get("/items/")
async def list_items(client: dict = Depends(verify_cookie_key)):
    return {"items": [], "client": client["name"]}
```

## API Key Management

### Database-Backed API Keys

```python
# Example 4: API key storage and validation
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import APIKeyHeader
from pydantic import BaseModel
from datetime import datetime
from typing import Optional
import hashlib
import secrets

app = FastAPI()

class APIKeyDB(BaseModel):
    """API key database model"""
    key_hash: str  # Store hash, not plain key
    name: str
    client_id: int
    created_at: datetime
    expires_at: Optional[datetime]
    is_active: bool = True
    rate_limit: int = 1000
    permissions: list[str] = []

# Simulated database
api_keys_db: dict[str, APIKeyDB] = {}

def hash_api_key(key: str) -> str:
    """
    Hash API key for secure storage.
    Never store plain text keys.
    """
    return hashlib.sha256(key.encode()).hexdigest()

def generate_api_key() -> tuple[str, str]:
    """
    Generate new API key.
    Returns (plain_key, hashed_key).
    """
    plain_key = f"sk_{secrets.token_urlsafe(32)}"
    hashed_key = hash_api_key(plain_key)
    return plain_key, hashed_key

@app.post("/api-keys/")
async def create_api_key(name: str, client_id: int):
    """
    Create new API key.
    Returns plain key ONCE - store it securely!
    """
    plain_key, hashed_key = generate_api_key()

    api_keys_db[hashed_key] = APIKeyDB(
        key_hash=hashed_key,
        name=name,
        client_id=client_id,
        created_at=datetime.utcnow(),
        expires_at=None,
        rate_limit=1000,
        permissions=["read"]
    )

    return {
        "api_key": plain_key,  # Only shown once!
        "message": "Store this key securely. It cannot be retrieved later."
    }

API_KEY_HEADER = APIKeyHeader(name="X-API-Key", auto_error=False)

async def verify_api_key_db(api_key: Optional[str] = Security(API_KEY_HEADER)):
    """Verify API key against database"""
    if api_key is None:
        raise HTTPException(401, "API key required")

    key_hash = hash_api_key(api_key)

    if key_hash not in api_keys_db:
        raise HTTPException(403, "Invalid API key")

    key_data = api_keys_db[key_hash]

    # Check if active
    if not key_data.is_active:
        raise HTTPException(403, "API key is disabled")

    # Check expiration
    if key_data.expires_at and datetime.utcnow() > key_data.expires_at:
        raise HTTPException(403, "API key has expired")

    return key_data

@app.get("/protected/")
async def protected_endpoint(key_data: APIKeyDB = Depends(verify_api_key_db)):
    return {"client": key_data.name, "permissions": key_data.permissions}
```

## Rate Limiting with API Keys

### Per-Key Rate Limiting

```python
# Example 5: Rate limiting per API key
from fastapi import FastAPI, Depends, HTTPException, Request
from fastapi.security import APIKeyHeader
from collections import defaultdict
import time

app = FastAPI()

# Rate limit storage
rate_limits: dict[str, list[float]] = defaultdict(list)

class RateLimiter:
    """Rate limiter per API key"""

    def __init__(self, requests_per_minute: int = 60):
        self.requests_per_minute = requests_per_minute
        self.window = 60  # seconds

    def check_rate_limit(self, key: str) -> dict:
        """Check if request is within rate limit"""
        current_time = time.time()

        # Clean old requests
        rate_limits[key] = [
            t for t in rate_limits[key]
            if current_time - t < self.window
        ]

        # Check limit
        remaining = self.requests_per_minute - len(rate_limits[key])

        if remaining <= 0:
            return {
                "allowed": False,
                "remaining": 0,
                "reset": int(rate_limits[key][0] + self.window)
            }

        # Record request
        rate_limits[key].append(current_time)

        return {
            "allowed": True,
            "remaining": remaining - 1,
            "reset": int(current_time + self.window)
        }

limiter = RateLimiter(requests_per_minute=100)

@app.get("/items/")
async def list_items(request: Request):
    """Endpoint with rate limiting"""
    api_key = request.headers.get("X-API-Key", "anonymous")

    result = limiter.check_rate_limit(api_key)

    if not result["allowed"]:
        raise HTTPException(
            status_code=429,
            detail="Rate limit exceeded",
            headers={
                "X-RateLimit-Remaining": "0",
                "X-RateLimit-Reset": str(result["reset"]),
                "Retry-After": str(result["reset"] - int(time.time()))
            }
        )

    return {
        "items": [],
        "rate_limit": {
            "remaining": result["remaining"],
            "reset": result["reset"]
        }
    }
```

## Best Practices

### API Key Security Guidelines

```python
# Example 6: Best practices implementation
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import APIKeyHeader
from datetime import datetime, timedelta
import secrets
import hashlib

app = FastAPI()

# 1. Use secure key generation
def generate_secure_api_key() -> str:
    """
    Generate cryptographically secure API key.
    Use secrets module, not random.
    """
    return f"sk_live_{secrets.token_urlsafe(32)}"

# 2. Hash keys before storage
def hash_key(key: str) -> str:
    """
    Hash API key for storage.
    Never store plaintext keys.
    """
    return hashlib.sha256(key.encode()).hexdigest()

# 3. Implement key rotation
class APIKeyManager:
    def __init__(self):
        self.keys = {}
        self.rotation_period = timedelta(days=90)

    def create_key(self, client_id: str) -> str:
        """Create new key with expiration"""
        plain_key = generate_secure_api_key()
        key_hash = hash_key(plain_key)

        self.keys[key_hash] = {
            "client_id": client_id,
            "created_at": datetime.utcnow(),
            "expires_at": datetime.utcnow() + self.rotation_period,
            "last_used": None
        }

        return plain_key

    def rotate_key(self, old_key_hash: str) -> str:
        """Rotate expired or compromised key"""
        if old_key_hash in self.keys:
            old_data = self.keys[old_key_hash]
            self.keys[old_key_hash]["is_active"] = False
            return self.create_key(old_data["client_id"])
        raise ValueError("Key not found")

# 4. Log key usage
import logging
logger = logging.getLogger("api_keys")

def log_key_usage(key_hash: str, endpoint: str, success: bool):
    """Log API key usage for audit"""
    logger.info(
        f"API Key Usage: hash={key_hash[:8]}... "
        f"endpoint={endpoint} success={success}"
    )

# 5. Implement key scoping
API_KEY_PERMISSIONS = {
    "sk_live_abc123": {"read": True, "write": True, "admin": False},
    "sk_live_def456": {"read": True, "write": False, "admin": False},
}

def check_permission(key: str, permission: str) -> bool:
    """Check if API key has specific permission"""
    perms = API_KEY_PERMISSIONS.get(key, {})
    return perms.get(permission, False)

# 6. Never expose keys in responses or logs
@app.get("/items/")
async def list_items():
    # NEVER include API keys in response
    return {"items": []}

# 7. Use HTTPS only
# Configure at reverse proxy level

# 8. Implement key expiration
@app.middleware("http")
async def check_key_expiration(request, call_next):
    """Check for expired keys"""
    response = await call_next(request)
    return response
```

## Summary

| Feature | Implementation | Security Level |
|---------|----------------|----------------|
| Header auth | `APIKeyHeader` | Medium |
| Query auth | `APIKeyQuery` | Low (logged) |
| Cookie auth | `APIKeyCookie` | Medium |
| Key hashing | SHA-256 | High |
| Rate limiting | Per-key limits | High |
| Key rotation | Periodic rotation | High |

## Next Steps

Continue learning about:
- [Bearer Token Authentication](./03_bearer_token_authentication.md) - JWT tokens
- [OAuth2 Flow Types](./04_oauth2_flow_types.md) - OAuth2 patterns
- [JWT Token Implementation](./05_jwt_token_implementation.md) - JWT details
