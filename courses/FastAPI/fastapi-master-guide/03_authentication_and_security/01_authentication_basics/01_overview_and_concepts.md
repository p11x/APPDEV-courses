# Authentication & Authorization Overview

## Core Concepts

### Authentication vs Authorization

```python
# Example 1: Understanding the difference
from fastapi import FastAPI, Depends, HTTPException, status

app = FastAPI()

# AUTHENTICATION: "Who are you?"
# Verifies the identity of a user or client
def authenticate_user(username: str, password: str):
    """
    Authentication verifies credentials.
    Returns user object if valid, None otherwise.
    """
    user = get_user_from_db(username)
    if not user or not verify_password(password, user.hashed_password):
        return None
    return user

# AUTHORIZATION: "What can you do?"
# Determines what an authenticated user is allowed to do
def authorize_user(user: dict, required_permission: str):
    """
    Authorization checks permissions.
    Happens AFTER authentication.
    """
    if required_permission not in user.get("permissions", []):
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="Insufficient permissions"
        )
    return True

# Both work together in API endpoints
@app.get("/admin/users")
async def admin_users(user: dict = Depends(authenticate)):
    """
    1. Authentication: Verify who is making request
    2. Authorization: Check if they can access this resource
    """
    # Step 1: Authentication happens via dependency
    # Step 2: Authorization check
    authorize_user(user, "admin:read")
    return {"users": []}
```

## Security Layers in FastAPI

### Defense in Depth

```python
# Example 2: Multiple security layers
from fastapi import FastAPI, Request, Depends
from fastapi.middleware.cors import CORSMiddleware
from fastapi.security import HTTPBearer
import time

app = FastAPI()

# Layer 1: HTTPS/TLS (configured at server level)
# Ensures encrypted communication

# Layer 2: CORS Middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["https://trusted-domain.com"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Layer 3: Rate Limiting
request_counts = {}

@app.middleware("http")
async def rate_limit_middleware(request: Request, call_next):
    """
    Rate limiting prevents abuse.
    Layer 3 of security defense.
    """
    client_ip = request.client.host
    current_time = time.time()

    # Clean old entries
    if client_ip in request_counts:
        request_counts[client_ip] = [
            t for t in request_counts[client_ip]
            if current_time - t < 60
        ]

    # Check limit (100 requests per minute)
    if len(request_counts.get(client_ip, [])) >= 100:
        from fastapi.responses import JSONResponse
        return JSONResponse(
            status_code=429,
            content={"error": "Rate limit exceeded"}
        )

    # Record request
    request_counts.setdefault(client_ip, []).append(current_time)

    return await call_next(request)

# Layer 4: Security Headers
@app.middleware("http")
async def security_headers(request: Request, call_next):
    """
    Security headers protect against common attacks.
    Layer 4 of security defense.
    """
    response = await call_next(request)

    response.headers["X-Content-Type-Options"] = "nosniff"
    response.headers["X-Frame-Options"] = "DENY"
    response.headers["X-XSS-Protection"] = "1; mode=block"
    response.headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains"
    response.headers["Content-Security-Policy"] = "default-src 'self'"

    return response

# Layer 5: Authentication
security = HTTPBearer()

async def verify_token(token: str = Depends(security)):
    """
    Token verification.
    Layer 5 of security defense.
    """
    # Verify JWT token
    return {"user_id": 1}

# Layer 6: Authorization
def require_permission(permission: str):
    """
    Permission checking.
    Layer 6 of security defense.
    """
    def checker(user: dict = Depends(verify_token)):
        if permission not in user.get("permissions", []):
            raise HTTPException(403, "Forbidden")
        return user
    return checker

# Layer 7: Input Validation (automatic with Pydantic)
from pydantic import BaseModel, Field

class ItemCreate(BaseModel):
    name: str = Field(..., min_length=1, max_length=100)
    price: float = Field(..., gt=0, le=1000000)

@app.post("/items/")
async def create_item(
    item: ItemCreate,  # Automatic validation
    user: dict = Depends(require_permission("items:create"))
):
    """
    All layers combined:
    1. HTTPS encryption
    2. CORS restrictions
    3. Rate limiting
    4. Security headers
    5. Authentication
    6. Authorization
    7. Input validation
    """
    return {"item": item.model_dump(), "created_by": user["user_id"]}
```

## Threat Modeling

### Common Threats and Mitigations

```python
# Example 3: Threat model for API
"""
OWASP Top 10 API Security Risks:

1. Broken Object Level Authorization (BOLA)
   - Mitigation: Check ownership on every request

2. Broken Authentication
   - Mitigation: Strong passwords, MFA, token security

3. Excessive Data Exposure
   - Mitigation: Response filtering, minimal data

4. Lack of Resources & Rate Limiting
   - Mitigation: Rate limiting, pagination limits

5. Broken Function Level Authorization
   - Mitigation: Role-based access control

6. Mass Assignment
   - Mitigation: Whitelist allowed fields

7. Security Misconfiguration
   - Mitigation: Security headers, minimal error info

8. Injection
   - Mitigation: Parameterized queries, input validation

9. Improper Assets Management
   - Mitigation: Document all endpoints

10. Insufficient Logging & Monitoring
    - Mitigation: Comprehensive logging
"""

from fastapi import FastAPI, Depends, HTTPException
from pydantic import BaseModel, Field
from typing import Optional

app = FastAPI()

# Mitigation 1: Object Level Authorization
@app.get("/items/{item_id}")
async def get_item(
    item_id: int,
    current_user: dict = Depends(get_current_user)
):
    """
    Always verify user has access to specific resource.
    """
    item = get_item_from_db(item_id)

    if not item:
        raise HTTPException(404, "Item not found")

    # Check ownership
    if item.owner_id != current_user["id"]:
        raise HTTPException(403, "Not authorized to access this item")

    return item

# Mitigation 2: Strong Authentication
def verify_password_strength(password: str) -> bool:
    """
    Enforce strong password requirements.
    """
    if len(password) < 12:
        return False
    if not any(c.isupper() for c in password):
        return False
    if not any(c.islower() for c in password):
        return False
    if not any(c.isdigit() for c in password):
        return False
    if not any(c in "!@#$%^&*()_+-=" for c in password):
        return False
    return True

# Mitigation 3: Minimal Data Exposure
class UserPublic(BaseModel):
    """Only expose safe fields"""
    id: int
    username: str
    # No password_hash, no email, no sensitive data

# Mitigation 6: Prevent Mass Assignment
class ItemUpdate(BaseModel):
    """Only allow updating specific fields"""
    name: Optional[str] = None
    description: Optional[str] = None
    # owner_id is NOT included - cannot be changed by user

@app.patch("/items/{item_id}")
async def update_item(
    item_id: int,
    item_data: ItemUpdate,  # Only whitelisted fields
    current_user: dict = Depends(get_current_user)
):
    """Mass assignment prevention"""
    return {"updated": item_id}
```

## Authentication Patterns

### Common Patterns Overview

```python
# Example 4: Authentication pattern comparison
"""
Pattern Comparison:

1. API Keys
   - Simple, stateless
   - Good for: Service-to-service, simple APIs
   - Risk: Key leakage

2. Bearer Tokens (JWT)
   - Stateless, scalable
   - Good for: Modern web/mobile apps
   - Risk: Token theft, expiration handling

3. OAuth2
   - Delegated authorization
   - Good for: Third-party integrations
   - Risk: Complexity, misconfiguration

4. Session-based
   - Stateful, server-side
   - Good for: Traditional web apps
   - Risk: Session hijacking

5. OpenID Connect
   - Identity layer on OAuth2
   - Good for: SSO, social login
   - Risk: Provider dependency
"""

from fastapi import FastAPI, Depends, Header, HTTPException
from fastapi.security import APIKeyHeader, HTTPBearer, OAuth2PasswordBearer

app = FastAPI()

# Pattern 1: API Key
api_key_header = APIKeyHeader(name="X-API-Key")

async def verify_api_key(api_key: str = Depends(api_key_header)):
    if api_key not in VALID_API_KEYS:
        raise HTTPException(403, "Invalid API key")
    return api_key

# Pattern 2: Bearer Token (JWT)
bearer_scheme = HTTPBearer()

async def verify_bearer(token = Depends(bearer_scheme)):
    # Decode and verify JWT
    return decode_jwt(token.credentials)

# Pattern 3: OAuth2 Password Flow
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

async def get_current_user(token: str = Depends(oauth2_scheme)):
    # Verify token and return user
    return verify_token(token)

# Pattern 4: Session-based (cookie)
@app.get("/items/")
async def list_items(session_id: str = Cookie(None)):
    if not session_id or not validate_session(session_id):
        raise HTTPException(401, "Invalid session")
    return {"items": []}
```

## Best Practices

### Security Guidelines

```python
# Example 5: Security best practices checklist
from fastapi import FastAPI, Depends, HTTPException
from datetime import datetime, timedelta
import secrets

app = FastAPI()

# 1. Always use HTTPS in production
# Configure at reverse proxy level (nginx, etc.)

# 2. Use secure token generation
def generate_secure_token() -> str:
    """
    Generate cryptographically secure tokens.
    Never use predictable values.
    """
    return secrets.token_urlsafe(32)

# 3. Implement proper token expiration
TOKEN_EXPIRY = timedelta(minutes=30)
REFRESH_TOKEN_EXPIRY = timedelta(days=7)

# 4. Store passwords securely (hashed)
def hash_password(password: str) -> str:
    """Use bcrypt or argon2 for password hashing"""
    from passlib.context import CryptContext
    pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
    return pwd_context.hash(password)

# 5. Validate all inputs
from pydantic import BaseModel, Field, EmailStr

class UserCreate(BaseModel):
    username: str = Field(..., min_length=3, max_length=50)
    email: EmailStr
    password: str = Field(..., min_length=12)

# 6. Implement proper error handling
@app.exception_handler(HTTPException)
async def security_exception_handler(request, exc):
    """
    Don't leak sensitive information in errors.
    """
    if exc.status_code == 401:
        return {"error": "Authentication required"}
    if exc.status_code == 403:
        return {"error": "Access denied"}
    # Don't expose internal details
    return {"error": "An error occurred"}

# 7. Log security events
import logging
security_logger = logging.getLogger("security")

def log_security_event(event: str, user_id: int = None, details: dict = None):
    """Log security-relevant events"""
    security_logger.info(
        f"Security event: {event}",
        extra={
            "user_id": user_id,
            "timestamp": datetime.utcnow().isoformat(),
            "details": details
        }
    )

# 8. Implement account lockout
LOGIN_ATTEMPTS = {}
MAX_ATTEMPTS = 5
LOCKOUT_DURATION = timedelta(minutes=15)

def check_account_lockout(username: str) -> bool:
    """Check if account is locked"""
    if username in LOGIN_ATTEMPTS:
        attempts, locked_until = LOGIN_ATTEMPTS[username]
        if locked_until and datetime.utcnow() < locked_until:
            return True
    return False

def record_failed_login(username: str):
    """Record failed login attempt"""
    attempts, _ = LOGIN_ATTEMPTS.get(username, (0, None))
    attempts += 1

    if attempts >= MAX_ATTEMPTS:
        locked_until = datetime.utcnow() + LOCKOUT_DURATION
        LOGIN_ATTEMPTS[username] = (attempts, locked_until)
        log_security_event("account_locked", details={"username": username})
    else:
        LOGIN_ATTEMPTS[username] = (attempts, None)
```

## Summary

| Concept | Description | Implementation |
|---------|-------------|----------------|
| Authentication | Verify identity | Tokens, passwords, biometrics |
| Authorization | Check permissions | RBAC, ABAC, policies |
| Defense in Depth | Multiple layers | Headers, rate limiting, validation |
| Threat Modeling | Identify risks | OWASP Top 10, risk assessment |
| Best Practices | Security guidelines | HTTPS, hashing, logging |

## Next Steps

Continue learning about:
- [API Keys Authentication](./02_api_keys_authentication.md) - Simple key-based auth
- [Bearer Token Authentication](./03_bearer_token_authentication.md) - JWT tokens
- [OAuth2 Flow Types](./04_oauth2_flow_types.md) - OAuth2 patterns
