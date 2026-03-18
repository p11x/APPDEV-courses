# RESTful API for Mobile Apps

## What You'll Learn

- How to design APIs optimized for mobile applications
- How to handle mobile-specific concerns like offline support
- How to implement pagination and filtering
- How to optimize for slow/unreliable networks

## Prerequisites

- Completed the FastAPI section
- Basic understanding of mobile app development

## Introduction

Mobile apps have different requirements than web applications. They need to handle slow networks, work offline, minimize data usage, and provide a smooth user experience. This guide covers designing RESTful APIs that serve mobile applications effectively.

## API Design Principles for Mobile

### Version Your API

Always version your API to allow for future changes:

```python
from fastapi import FastAPI, APIRouter

app = FastAPI(title="Mobile API")

# Version 1
v1_router = APIRouter(prefix="/v1")


@v1_router.get("/users/{user_id}")
async def get_user(user_id: int):
    """Get user profile."""
    return {
        "id": user_id,
        "name": "John Doe",
        "email": "john@example.com",
    }


app.include_router(v1_router)
```

🔍 **Line-by-Line Breakdown:**

1. `APIRouter(prefix="/v1")` — Creates a router with version prefix. This allows maintaining multiple API versions simultaneously.
2. `@v1_router.get("/users/{user_id}")` — Route for fetching user data. The `{user_id}` is a path parameter.
3. Returns a dictionary which FastAPI automatically converts to JSON.

### Consistent Response Format

Use a consistent response wrapper:

```python
from pydantic import BaseModel
from typing import TypeVar, Generic, Optional
from enum import Enum


class ResponseStatus(str, Enum):
    """API response status codes."""
    SUCCESS = "success"
    ERROR = "error"
    PARTIAL = "partial"


T = TypeVar("T")


class ApiResponse(BaseModel, Generic[T]):
    """Standard API response wrapper."""
    status: ResponseStatus
    data: Optional[T] = None
    error: Optional[str] = None
    meta: Optional[dict] = None
    
    class Config:
        use_enum_values = True


class PaginatedResponse(BaseModel, Generic[T]):
    """Paginated response wrapper."""
    status: ResponseStatus = ResponseStatus.SUCCESS
    data: list[T]
    pagination: dict
    
    @classmethod
    def create(
        cls,
        items: list[T],
        page: int,
        per_page: int,
        total: int,
    ) -> "PaginatedResponse[T]":
        """Create a paginated response."""
        return cls(
            data=items,
            pagination={
                "page": page,
                "per_page": per_page,
                "total": total,
                "total_pages": (total + per_page - 1) // per_page,
                "has_next": page * per_page < total,
                "has_prev": page > 1,
            },
        )


# Usage in endpoint
from dataclasses import dataclass


@dataclass
class User:
    id: int
    name: str
    email: str


@app.get("/v1/users", response_model=PaginatedResponse[User])
async def get_users(page: int = 1, per_page: int = 20):
    """Get paginated list of users."""
    # In real app, fetch from database
    users = [
        User(id=i, name=f"User {i}", email=f"user{i}@example.com")
        for i in range(1, 101)
    ]
    
    # Paginate
    start = (page - 1) * per_page
    end = start + per_page
    paginated_users = users[start:end]
    
    return PaginatedResponse.create(
        items=paginated_users,
        page=page,
        per_page=per_page,
        total=len(users),
    )
```

## Offline Support

Design your API to support offline-first mobile apps:

```python
from datetime import datetime
from typing import Optional


@dataclass
class SyncableItem:
    """An item that can be synced with mobile app."""
    id: int
    name: str
    updated_at: datetime
    deleted: bool = False


class SyncManager:
    """Manages synchronization between server and mobile app."""
    
    def __init__(self) -> None:
        self.last_sync: Optional[datetime] = None
    
    def get_changes(
        self,
        entity_type: str,
        since: Optional[datetime],
    ) -> dict:
        """Get all changes since the given timestamp."""
        # In real app, query database for changes
        # This is a simplified example
        changes = {
            "created": [],
            "updated": [],
            "deleted": [],
        }
        
        return {
            "changes": changes,
            "server_time": datetime.utcnow(),
        }


sync_manager = SyncManager()


@app.get("/v1/sync/{entity_type}")
async def sync_entity(
    entity_type: str,
    since: Optional[datetime] = None,
) -> dict:
    """Get all changes since last sync."""
    changes = sync_manager.get_changes(entity_type, since)
    return ApiResponse(data=changes)
```

## Field Selection

Allow mobile apps to request only needed fields:

```python
from typing import Optional


class User(BaseModel):
    id: int
    name: str
    email: str
    phone: Optional[str] = None
    address: Optional[str] = None
    bio: Optional[str] = None
    avatar_url: Optional[str] = None
    created_at: datetime
    updated_at: datetime


@app.get("/v1/users/{user_id}")
async def get_user(
    user_id: int,
    fields: Optional[str] = None,
) -> ApiResponse[User]:
    """Get user with optional field selection."""
    # Fetch full user
    user = User(
        id=user_id,
        name="John Doe",
        email="john@example.com",
        phone="+1234567890",
        address="123 Main St",
        bio="Hello world",
        avatar_url="https://example.com/avatar.jpg",
        created_at=datetime.now(),
        updated_at=datetime.now(),
    )
    
    # Filter fields if requested
    if fields:
        field_list = [f.strip() for f in fields.split(",")]
        user_dict = user.model_dump(include=field_list)
        return ApiResponse(data=User(**user_dict))
    
    return ApiResponse(data=user)
```

## Compression and Optimization

Reduce bandwidth usage:

```python
from fastapi import FastAPI
from fastapi.middleware.gzip import GZipMiddleware


app = FastAPI()

# Enable GZIP compression
app.add_middleware(GZipMiddleware, minimum_size=1000)


# Use minimal responses for mobile
class MinimalUser(BaseModel):
    """Minimal user info for lists."""
    id: int
    name: str
    avatar_url: Optional[str] = None


class FullUser(BaseModel):
    """Full user profile."""
    id: int
    name: str
    email: str
    phone: Optional[str] = None
    address: Optional[str] = None
    bio: Optional[str] = None
    avatar_url: Optional[str] = None


@app.get("/v1/users/{user_id}/minimal", response_model=MinimalUser)
async def get_user_minimal(user_id: int) -> MinimalUser:
    """Get minimal user info for mobile lists."""
    return MinimalUser(
        id=user_id,
        name="John Doe",
        avatar_url="https://example.com/avatar.jpg",
    )


@app.get("/v1/users/{user_id}/full", response_model=FullUser)
async def get_user_full(user_id: int) -> FullUser:
    """Get full user profile."""
    return FullUser(
        id=user_id,
        name="John Doe",
        email="john@example.com",
        phone="+1234567890",
        address="123 Main St",
        bio="Hello world",
        avatar_url="https://example.com/avatar.jpg",
    )
```

## Batch Operations

Reduce network requests with batch operations:

```python
from typing import List


@app.post("/v1/users/batch")
async def get_users_batch(user_ids: List[int]) -> ApiResponse[List[User]]:
    """Get multiple users in a single request."""
    # In real app, fetch from database
    users = [
        User(
            id=uid,
            name=f"User {uid}",
            email=f"user{uid}@example.com",
            created_at=datetime.now(),
            updated_at=datetime.now(),
        )
        for uid in user_ids
    ]
    
    return ApiResponse(data=users)


@app.put("/v1/items/batch")
async def update_items_batch(items: List[dict]) -> ApiResponse[List[dict]]:
    """Update multiple items in a single request."""
    results = []
    
    for item in items:
        results.append({
            "id": item.get("id"),
            "status": "updated",
        })
    
    return ApiResponse(data=results)
```

## Summary

- Always version your API to allow future changes
- Use consistent response wrappers for easy parsing
- Implement pagination for large datasets
- Support field selection to reduce bandwidth
- Use compression middleware
- Implement batch operations to reduce network requests
- Design for offline-first mobile apps

## Next Steps

→ Continue to `02-building-an-android-backend.md` to learn about Android-specific backend considerations.
