# Advanced Dependency Injection in FastAPI

## What You'll Learn
- Complex dependency graphs
- Dependencies with dependencies
- Caching dependencies
- Conditional dependencies
- Generator-based dependencies for resource management

## Prerequisites
- Completed `06-background-tasks.md` — Background tasks
- Understanding of FastAPI's basic dependency injection system
- Understanding of async/await in Python 3.11+

## Recap: Basic Dependency Injection

FastAPI's dependency injection automatically passes required values to endpoints:

```python
from fastapi import Depends

async def get_current_user(token: str = Depends(get_token)) -> User:
    """Dependency injected into endpoint."""
    return User(id=1, name="John")

@app.get("/profile")
async def profile(user: User = Depends(get_current_user)):
    return {"name": user.name}
```

## Dependencies with Dependencies

Dependencies can depend on other dependencies:

```python
from fastapi import FastAPI, Depends, HTTPException
from pydantic import BaseModel
import jwt
from datetime import datetime

app = FastAPI()

# Models
class TokenData(BaseModel):
    user_id: int
    exp: datetime

class User(BaseModel):
    id: int
    username: str
    email: str

# ─────────────────────────────────────────────
# Level 1: Simple dependency
# ─────────────────────────────────────────────
def get_token_from_header(authorization: str = None) -> str:
    """Extract token from Authorization header."""
    if not authorization:
        raise HTTPException(status_code=401, detail="Missing authorization header")
    
    if not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Invalid authorization format")
    
    return authorization.replace("Bearer ", "")

# ─────────────────────────────────────────────
# Level 2: Depends on Level 1
# ─────────────────────────────────────────────
async def get_current_user(token: str = Depends(get_token_from_header)) -> User:
    """Verify token and return user - depends on get_token_from_header."""
    try:
        payload = jwt.decode(token, "secret", algorithms=["HS256"])
        user_id = payload.get("sub")
        
        # Fetch user from database
        user = await db.users.get(user_id)
        if not user:
            raise HTTPException(status_code=401, detail="User not found")
        
        return user
        
    except jwt.PyJWTError:
        raise HTTPException(status_code=401, detail="Invalid token")

# ─────────────────────────────────────────────
# Level 3: Depends on Level 2
# ─────────────────────────────────────────────
async def get_current_user_with_premium(
    user: User = Depends(get_current_user)
) -> dict:
    """Get user with premium status - depends on get_current_user."""
    # Check premium status
    premium = await db.premium.get(user.id)
    
    return {
        "user": user,
        "is_premium": premium is not None,
        "premium_expires": premium.expires_at if premium else None
    }

# ─────────────────────────────────────────────
# Endpoint using Level 3
# ─────────────────────────────────────────────
@app.get("/dashboard")
async def dashboard(
    user_data: dict = Depends(get_current_user_with_premium)
):
    """Dashboard endpoint uses full dependency chain."""
    return user_data
```

🔍 **Line-by-Line Breakdown:**
1. `get_token_from_header` — Level 1: Extracts token from request
2. `get_current_user` — Level 2: Depends on Level 1 via `Depends(get_token_from_header)`
3. `get_current_user_with_premium` — Level 3: Depends on Level 2 via `Depends(get_current_user)`
4. Chain automatically resolves: endpoint → L3 → L2 → L1

## Generator-Based Dependencies

Use generators for setup and teardown (like opening/closing connections):

```python
from fastapi import FastAPI, Depends
from contextlib import asynccontextmanager
from typing import AsyncGenerator
import asyncio
import asyncpg

# Global pool
pool: asyncpg.Pool | None = None

@asynccontextmanager
async def get_db_pool() -> AsyncGenerator[asyncpg.Pool, None]:
    """Dependency that provides database pool."""
    global pool
    
    # Startup: Create pool
    pool = await asyncpg.create_pool(
        host="localhost",
        port=5432,
        user="user",
        password="pass",
        database="mydb",
        min_size=5,
        max_size=20
    )
    
    print("📦 Database pool created")
    
    yield pool  # Pool available to endpoints
    
    # Shutdown: Close pool
    await pool.close()
    print("📦 Database pool closed")

# Use in endpoints
@app.get("/users")
async def list_users(pool: asyncpg.Pool = Depends(get_db_pool)):
    """List users using pooled connection."""
    async with pool.acquire() as conn:
        return await conn.fetch("SELECT * FROM users")

# Alternative: Dependency that handles connection per-request
async def get_db() -> AsyncGenerator[asyncpg.Pool, None]:
    """Get database pool for a single request."""
    async with pool.acquire() as conn:
        yield conn

@app.get("/users/{user_id}")
async def get_user(conn: asyncpg.Connection = Depends(get_db)):
    """Get single user with per-request connection."""
    return await conn.fetchrow("SELECT * FROM users WHERE id = $1", user_id)
```

## Caching Dependencies

Cache expensive dependencies to avoid repeated computation:

```python
from fastapi import Depends
from functools import lru_cache
import time

# Option 1: Use @lru_cache for sync functions
@lru_cache()
def get_app_settings():
    """Cached settings - computed once."""
    # Expensive operation
    time.sleep(2)  # Simulate slow load
    return {"max_items": 100, "theme": "dark"}

# Option 2: Use dependency caching with FastAPI
async def get_expensive_data():
    """This runs once per request by default."""
    # FastAPI caches dependencies per-request automatically!
    await asyncio.sleep(1)
    return {"computed": time.time()}

@app.get("/cached")
async def cached_endpoint(
    data: dict = Depends(get_expensive_data)
):
    """Same request gets same cached data."""
    return data  # Within same request, same timestamp

# Option 3: Use cache for cross-request caching
from datetime import datetime, timedelta

cache: dict[str, tuple[datetime, dict]] = {}

async def get_cached_user(user_id: int) -> dict:
    """Cache user data for 5 minutes."""
    cache_key = f"user:{user_id}"
    
    # Check cache
    if cache_key in cache:
        cached_time, cached_data = cache[cache_key]
        if datetime.utcnow() - cached_time < timedelta(minutes=5):
            return cached_data
    
    # Fetch from database
    data = await db.users.get(user_id)
    
    # Store in cache
    cache[cache_key] = (datetime.utcnow(), data)
    
    return data
```

## Conditional Dependencies

Use different dependencies based on conditions:

```python
from fastapi import FastAPI, Depends
from enum import Enum

class UserRole(str, Enum):
    ADMIN = "admin"
    USER = "user"
    GUEST = "guest"

def get_role_required(required_role: UserRole):
    """Factory function that creates dependency for specific role."""
    def check_role(current_user: User = Depends(get_current_user)) -> User:
        if current_user.role != required_role and current_user.role != UserRole.ADMIN:
            raise HTTPException(
                status_code=403,
                detail=f"Requires {required_role} role"
            )
        return current_user
    return check_role

# Different endpoints need different roles
@app.get("/profile")
async def profile(user: User = Depends(get_role_required(UserRole.USER))):
    """Any logged-in user"""
    return user

@app.get("/admin/users")
async def admin_users(
    admin: User = Depends(get_role_required(UserRole.ADMIN))
):
    """Only admins"""
    return {"users": []}

@app.get("/billing")
async def billing(
    staff: User = Depends(get_role_required(UserRole.ADMIN))
):
    """Only admins (or elevated staff)"""
    return {"invoices": []}
```

## Optional Dependencies

Dependencies that may or may not be present:

```python
from fastapi import FastAPI, Depends
from typing import Optional

async def get_current_user_optional(
    request: Request
) -> Optional[User]:
    """Return user if authenticated, None otherwise."""
    auth = request.headers.get("Authorization")
    if not auth:
        return None
    
    # Verify and return user
    ...

# Using Optional with Depends
@app.get("/dashboard")
async def dashboard(
    user: Optional[User] = Depends(get_current_user_optional)
):
    """Dashboard works for both authenticated and anonymous users."""
    if user:
        return {"view": "user_dashboard", "user": user.name}
    return {"view": "public_dashboard"}
```

## Using Dependencies Outside Endpoints

Sometimes you need dependencies in other places:

```python
from fastapi import FastAPI
from fastapi.testclient import TestClient

app = FastAPI()

# In testing, you can manually call dependencies
def override_get_current_user():
    """Override dependency for testing."""
    return User(id=1, username="testuser", role=UserRole.ADMIN)

app.dependency_overrides[get_current_user] = override_get_current_user

# Test client automatically uses override
client = TestClient(app)

def test_protected_endpoint():
    response = client.get("/profile")
    assert response.status_code == 200
```

## Production Considerations

- **Dependency order**: Dependencies resolve in order — put simpler ones first
- **Database connections**: Use generator dependencies for connection pooling
- **Caching**: Cache expensive operations, but invalidate on data changes
- **Testing**: Use `dependency_overrides` for clean testing

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Circular dependencies

**Wrong:**
```python
async def get_a():
    b = Depends(get_b)
    ...

async def get_b():
    a = Depends(get_a)
    ...
```

**Why it fails:** FastAPI can't resolve infinite loop.

**Fix:** Restructure to break the cycle:
```python
async def get_common():
    ...

async def get_a(common=Depends(get_common)):
    ...

async def get_b(common=Depends(get_common)):
    ...
```

### ❌ Mistake 2: Not cleaning up resources

**Wrong:**
```python
async def get_client():
    client = create_client()
    return client  # Never closed!
```

**Why it fails:** Connections/leaks accumulate.

**Fix:** Use generator:
```python
async def get_client():
    client = create_client()
    try:
        yield client
    finally:
        client.close()
```

### ❌ Mistake 3: Slow dependencies on every request

**Wrong:**
```python
async def get_settings():
    # Runs EVERY request!
    return await slow_database_query()
```

**Why it fails:** Performance impact on every request.

**Fix:** Cache or use application state:
```python
@app.on_event("startup")
async def load_settings():
    app.state.settings = await slow_database_query()

def get_settings():
    return app.state.settings
```

## Summary

- Dependencies can depend on other dependencies (chains)
- Use generators (`yield`) for setup/teardown
- Cache expensive dependencies with `@lru_cache` or manual caching
- Create factory functions for conditional dependencies
- Use `dependency_overrides` for testing

## Next Steps

This completes the Advanced FastAPI Patterns folder. Continue to `34-advanced-django-patterns/01-class-based-views.md` to explore advanced Django patterns.
