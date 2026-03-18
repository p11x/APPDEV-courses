# Role-Based Access Control (RBAC) with FastAPI

## What You'll Learn
- RBAC fundamentals and use cases
- Implementing roles and permissions
- Dependency injection for authorization
- Object-level permissions
- Practical examples with code

## Prerequisites
- Completed `02-oauth2-with-fastapi.md` — OAuth2 implementation
- Understanding of FastAPI dependency injection
- Basic Python type hints

## What Is RBAC?

**Role-Based Access Control** assigns permissions to roles, not directly to users:

```
Traditional:     User ───▶ Permissions
                      (User has specific permissions)

RBAC:           User ──▶ Role ──▶ Permissions
                   (User has role, role has permissions)
```

**Benefits:**
- Easier permission management
- Scalable (add role, not user)
- Audit-friendly (see what roles can do)

## Implementing RBAC in FastAPI

### 1. Define Roles and Permissions

```python
from enum import Enum
from typing import Set

class Role(str, Enum):
    """Application roles."""
    ADMIN = "admin"
    MANAGER = "manager"
    USER = "user"
    GUEST = "guest"

class Permission(str, Enum):
    """Available permissions."""
    # User management
    USER_READ = "user:read"
    USER_CREATE = "user:create"
    USER_UPDATE = "user:update"
    USER_DELETE = "user:delete"
    
    # Content management
    CONTENT_READ = "content:read"
    CONTENT_CREATE = "content:create"
    CONTENT_UPDATE = "content:update"
    CONTENT_DELETE = "content:delete"
    
    # Admin
    ADMIN_ACCESS = "admin:access"

# Role to permissions mapping
ROLE_PERMISSIONS: dict[Role, Set[Permission]] = {
    Role.ADMIN: {
        Permission.USER_READ,
        Permission.USER_CREATE,
        Permission.USER_UPDATE,
        Permission.USER_DELETE,
        Permission.CONTENT_READ,
        Permission.CONTENT_CREATE,
        Permission.CONTENT_UPDATE,
        Permission.CONTENT_DELETE,
        Permission.ADMIN_ACCESS,
    },
    Role.MANAGER: {
        Permission.USER_READ,
        Permission.CONTENT_READ,
        Permission.CONTENT_CREATE,
        Permission.CONTENT_UPDATE,
    },
    Role.USER: {
        Permission.CONTENT_READ,
        Permission.CONTENT_CREATE,
    },
    Role.GUEST: {
        Permission.CONTENT_READ,
    },
}
```

🔍 **Line-by-Line Breakdown:**
1. `Role(str, Enum)` — Enumeration of available roles
2. `Permission(str, Enum)` — Enumeration of granular permissions
3. `ROLE_PERMISSIONS` — Mapping of roles to their permissions

### 2. User Model with Roles

```python
from pydantic import BaseModel
from typing import Optional

class User(BaseModel):
    """User model with role."""
    id: int
    username: str
    email: str
    role: Role
    is_active: bool = True

class UserInDB(User):
    """User with password hash."""
    hashed_password: str

# Mock database
users_db: dict[str, UserInDB] = {
    "admin": UserInDB(
        id=1,
        username="admin",
        email="admin@example.com",
        role=Role.ADMIN,
        hashed_password="hashed"
    ),
    "manager": UserInDB(
        id=2,
        username="manager",
        email="manager@example.com",
        role=Role.MANAGER,
        hashed_password="hashed"
    ),
    "user": UserInDB(
        id=3,
        username="user",
        email="user@example.com",
        role=Role.USER,
        hashed_password="hashed"
    ),
}
```

### 3. Authorization Dependencies

```python
from fastapi import Depends, HTTPException, Security, status
from fastapi.security import OAuth2PasswordBearer

oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

# Get user from token
async def get_current_user(token: str = Depends(oauth2_scheme)) -> User:
    """Extract and validate current user from token."""
    # In production, decode JWT and fetch from database
    # This is simplified
    username = token.split(".")[0]  # Fake extraction
    
    if username not in users_db:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Invalid authentication",
        )
    
    return users_db[username]

# Check if user has required role
def require_role(required_role: Role):
    """Dependency factory for role-based access."""
    def role_checker(current_user: User = Depends(get_current_user)) -> User:
        if current_user.role != required_role:
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN,
                detail=f"Role '{required_role.value}' required",
            )
        return current_user
    return role_checker

# Check if user has required permission
def require_permission(required_permission: Permission):
    """Dependency factory for permission-based access."""
    def permission_checker(current_user: User = Depends(get_current_user)) -> User:
        # Get user's permissions from role
        user_permissions = ROLE_PERMISSIONS.get(current_user.role, set())
        
        if required_permission not in user_permissions:
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN,
                detail=f"Permission '{required_permission.value}' required",
            )
        return current_user
    return permission_checker
```

### 4. Using RBAC in Routes

```python
from fastapi import FastAPI, Depends

app = FastAPI()

# Role-based endpoints
@app.get("/admin/dashboard")
async def admin_dashboard(
    user: User = Depends(require_role(Role.ADMIN))
) -> dict:
    """Admin-only dashboard."""
    return {"message": f"Welcome, {user.username}!"}

@app.get("/manager/users")
async def manager_user_list(
    user: User = Depends(require_role(Role.MANAGER))
) -> dict:
    """Manager can view users."""
    return {"message": f"User list accessible by {user.username}"}

# Permission-based endpoints
@app.get("/content")
async def read_content(
    user: User = Depends(require_permission(Permission.CONTENT_READ))
) -> list:
    """Anyone with content:read permission."""
    return [{"id": 1, "title": "Article 1"}]

@app.post("/content")
async def create_content(
    user: User = Depends(require_permission(Permission.CONTENT_CREATE))
) -> dict:
    """Anyone with content:create permission."""
    return {"message": "Content created"}

@app.delete("/content/{content_id}")
async def delete_content(
    content_id: int,
    user: User = Depends(require_permission(Permission.CONTENT_DELETE))
) -> dict:
    """Anyone with content:delete permission."""
    return {"message": f"Content {content_id} deleted"}
```

## Object-Level Permissions

```python
from fastapi import Depends, HTTPException

# Ownership check for object-level permissions
async def get_object_owner(object_id: int) -> int:
    """Get the owner ID of an object (from database)."""
    # Mock: In production, query database
    owners = {1: 1, 2: 2, 3: 3}  # content_id -> owner_id
    return owners.get(object_id, 0)

def require_owner_or_role(required_role: Role):
    """Allow owner or specific role."""
    async def owner_checker(
        object_id: int,
        current_user: User = Depends(get_current_user)
    ) -> User:
        # Allow if user has required role
        if current_user.role == required_role:
            return current_user
        
        # Allow if user owns the object
        owner_id = await get_object_owner(object_id)
        if current_user.id == owner_id:
            return current_user
        
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="Not authorized to access this object"
        )
    return owner_checker

# Usage
@app.put("/content/{content_id}")
async def update_content(
    content_id: int,
    user: User = Depends(require_owner_or_role(Role.ADMIN))
) -> dict:
    """Owner or admin can update."""
    return {"message": f"Content {content_id} updated by {user.username}"}
```

## Role Hierarchy

```python
class RoleHierarchy:
    """Manage role hierarchy."""
    
    # Define which roles include others
    HIERARCHY = {
        Role.ADMIN: [Role.ADMIN, Role.MANAGER, Role.USER, Role.GUEST],
        Role.MANAGER: [Role.MANAGER, Role.USER, Role.GUEST],
        Role.USER: [Role.USER, Role.GUEST],
        Role.GUEST: [Role.GUEST],
    }
    
    @classmethod
    def has_role(cls, user_role: Role, required_role: Role) -> bool:
        """Check if user_role includes required_role."""
        allowed_roles = cls.HIERARCHY.get(user_role, [])
        return required_role in allowed_roles
    
    @classmethod
    def get_effective_permissions(cls, role: Role) -> set[Permission]:
        """Get all permissions for a role including inherited ones."""
        permissions = set()
        
        # Include all roles below this one
        allowed_roles = cls.HIERARCHY.get(role, [])
        
        for r in allowed_roles:
            permissions.update(ROLE_PERMISSIONS.get(r, set()))
        
        return permissions

# Updated dependency
def require_role_hierarchy(required_role: Role):
    """Dependency that uses role hierarchy."""
    def role_checker(current_user: User = Depends(get_current_user)) -> User:
        if not RoleHierarchy.has_role(current_user.role, required_role):
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN,
                detail=f"Role '{required_role.value}' or higher required",
            )
        return current_user
    return role_checker

# Usage
@app.get("/reports")
async def view_reports(
    user: User = Depends(require_role_hierarchy(Role.MANAGER))
) -> dict:
    """Manager and above can view reports."""
    return {"reports": []}
```

## Combining with JWT

```python
from pydantic import BaseModel
from datetime import datetime
import jwt

class TokenData(BaseModel):
    username: str
    role: Role

async def get_current_user_jwt(token: str = Depends(oauth2_scheme)) -> User:
    """Get user from JWT token with role."""
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
        username = payload.get("sub")
        role = payload.get("role", Role.USER)
        
        if username not in users_db:
            raise HTTPException(status_code=401, detail="Invalid token")
        
        user = users_db[username]
        
        # Override role from token if present
        if isinstance(role, str):
            user.role = Role(role)
        
        return user
        
    except jwt.JWTError:
        raise HTTPException(status_code=401, detail="Invalid token")

def create_token_with_role(username: str, role: Role) -> str:
    """Create JWT token including role."""
    payload = {
        "sub": username,
        "role": role.value,
        "exp": datetime.utcnow() + timedelta(hours=1)
    }
    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)
```

## Production Considerations

- **Database storage**: Store roles in database, not hardcoded
- **Audit logging**: Log permission checks for security
- **Caching**: Cache role/permission lookups
- **Scalability**: Use Redis for distributed permission checks

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not checking permissions at all

**Wrong:**
```python
@app.delete("/users/{user_id}")
async def delete_user(user_id: int):
    # No check! Anyone can delete!
    delete_from_db(user_id)
```

**Why it fails:** Security vulnerability.

**Fix:**
```python
@app.delete("/users/{user_id}")
async def delete_user(
    user_id: int,
    user: User = Depends(require_permission(Permission.USER_DELETE))
):
    # Protected!
    delete_from_db(user_id)
```

### ❌ Mistake 2: Checking role instead of permission

**Wrong:**
```python
@app.post("/content")
async def create_content(
    user: User = Depends(require_role(Role.ADMIN))  # Too restrictive!
):
```

**Why it fails:** Only admins can create, but managers and users should too.

**Fix:**
```python
@app.post("/content")
async def create_content(
    user: User = Depends(require_permission(Permission.CONTENT_CREATE))
):
```

### ❌ Mistake 3: Not handling object ownership

**Wrong:**
```python
@app.put("/posts/{post_id}")
async def update_post(
    post_id: int,
    user: User = Depends(require_permission(Permission.CONTENT_UPDATE))
):
    # Any user with permission can edit ANY post!
    update_post(post_id, user)
```

**Why it fails:** Users can edit others' content.

**Fix:**
```python
@app.put("/posts/{post_id}")
async def update_post(
    post_id: int,
    user: User = Depends(require_owner_or_role(Role.ADMIN))
):
    update_post(post_id, user)
```

## Summary

- RBAC assigns permissions to roles, not directly to users
- Define granular permissions for fine-grained access control
- Use dependencies for reusable authorization checks
- Implement object-level permissions for resource ownership
- Use role hierarchy to simplify permission management

## Next Steps

→ Continue to `04-pgp-and-ssh-keys.md` to learn about encryption and key management for authentication.
