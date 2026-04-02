# Sub Dependencies

## Overview

Sub-dependencies allow one dependency to depend on another, creating a dependency tree. FastAPI resolves these automatically, enabling modular and reusable code.

## Dependency Chains

### Basic Sub-Dependency

```python
# Example 1: Dependency chain
from fastapi import FastAPI, Depends, HTTPException, Header
from typing import Optional

app = FastAPI()

def get_token(authorization: str = Header(...)):
    """Extract token from header"""
    if not authorization.startswith("Bearer "):
        raise HTTPException(401, "Invalid authorization")
    return authorization.split(" ")[1]

def get_current_user(token: str = Depends(get_token)):
    """Get user from token (depends on get_token)"""
    if token != "valid-token":
        raise HTTPException(401, "Invalid token")
    return {"user_id": 1, "username": "john"}

def get_user_permissions(user: dict = Depends(get_current_user)):
    """Get user permissions (depends on get_current_user)"""
    return ["read", "write"]

@app.get("/items/")
async def list_items(
    # Chain: get_token → get_current_user → get_user_permissions
    permissions: list = Depends(get_user_permissions)
):
    return {"permissions": permissions}
```

## Nested Dependencies

### Multi-Level Dependencies

```python
# Example 2: Multi-level dependency tree
from fastapi import FastAPI, Depends

app = FastAPI()

# Level 1: Database connection
def get_db():
    """Database connection"""
    db = {"connected": True}
    try:
        yield db
    finally:
        db["connected"] = False

# Level 2: Repository (depends on DB)
class ItemRepository:
    def __init__(self, db: dict = Depends(get_db)):
        self.db = db

    def get_all(self):
        return [{"id": 1, "name": "Item 1"}]

    def get_by_id(self, item_id: int):
        return {"id": item_id, "name": f"Item {item_id}"}

def get_item_repo(repo: ItemRepository = Depends()):
    return repo

# Level 3: Service (depends on Repository)
class ItemService:
    def __init__(self, repo: ItemRepository = Depends(get_item_repo)):
        self.repo = repo

    def list_items(self):
        return self.repo.get_all()

    def get_item(self, item_id: int):
        return self.repo.get_by_id(item_id)

def get_item_service(service: ItemService = Depends()):
    return service

@app.get("/items/")
async def list_items(service: ItemService = Depends(get_item_service)):
    """Dependency tree: DB → Repository → Service"""
    return {"items": service.list_items()}
```

## Shared Sub-Dependencies

### Caching Dependencies

```python
# Example 3: Shared dependency instances
from fastapi import FastAPI, Depends

app = FastAPI()

call_count = {"db": 0, "user": 0}

def get_db():
    """Database dependency (cached within request)"""
    call_count["db"] += 1
    return {"connection": f"conn_{call_count['db']}"}

def get_current_user(db: dict = Depends(get_db)):
    """User dependency depends on DB"""
    call_count["user"] += 1
    return {"user_id": 1, "db": db}

def get_user_items(user: dict = Depends(get_current_user)):
    """Items depend on user, which depends on DB"""
    return {"user": user, "items": []}

@app.get("/test/")
async def test(
    user: dict = Depends(get_current_user),
    items: dict = Depends(get_user_items)
):
    """
    DB is called only once (cached).
    Both user and items get same DB instance.
    """
    return {
        "calls": call_count,
        "user": user,
        "items": items
    }
```

## Best Practices

### Sub-Dependency Guidelines

```python
# Example 4: Best practices
from fastapi import FastAPI, Depends

app = FastAPI()

# GOOD: Clear dependency hierarchy
def get_config():
    """Configuration (base dependency)"""
    return {"db_url": "sqlite:///:memory:"}

def get_db(config: dict = Depends(get_config)):
    """Database (depends on config)"""
    return {"connected": True, "url": config["db_url"]}

def get_repository(db: dict = Depends(get_db)):
    """Repository (depends on DB)"""
    return {"db": db}

# GOOD: Keep dependency trees shallow
# BAD: Deep nesting (5+ levels)
```

## Summary

| Concept | Description | Example |
|---------|-------------|---------|
| Chain | A → B → C | Token → User → Permissions |
| Tree | Multiple paths | Service ← Repo ← DB |
| Caching | Reuse instances | Same DB in multiple deps |

## Next Steps

Continue learning about:
- [Conditional Dependencies](./04_conditional_dependencies.md) - Dynamic deps
- [Dependency Testing](./05_dependency_testing.md) - Testing deps
- [Middleware](../06_middleware/01_middleware_overview.md) - Request processing
