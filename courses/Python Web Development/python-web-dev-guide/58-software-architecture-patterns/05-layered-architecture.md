# Layered Architecture

## What You'll Learn

- Classic layered architecture patterns
- Separation of concerns
- Building maintainable applications

## Prerequisites

- Understanding of basic web application structure

## Introduction

Layered architecture (also called n-tier architecture) is one of the most common architectural patterns. It organizes code into logical layers where each layer has specific responsibilities and only communicates with adjacent layers.

Think of a restaurant: the waiter (presentation layer) takes your order, communicates it to the kitchen (business logic layer), which gets ingredients from the storage (data access layer). Each layer has a specific job and talks only to its neighboring layers.

## Common Layer Structure

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │  ← Handles HTTP requests, returns responses
├─────────────────────────────────────────┤
│         Business Logic Layer            │  ← Application rules and workflows
├─────────────────────────────────────────┤
│           Data Access Layer             │  ← Database operations
├─────────────────────────────────────────┤
│             Database                   │  ← Persistent storage
└─────────────────────────────────────────┘
```

## Implementation in Python

```python
from dataclasses import dataclass
from datetime import datetime
from typing import Optional
from abc import ABC, abstractmethod

# ============== Domain Layer ==============
@dataclass
class User:
    id: int
    email: str
    name: str
    created_at: datetime

# ============== Data Access Layer ==============
class UserRepository(ABC):
    """Abstract repository for user data access."""
    
    @abstractmethod
    def find_by_id(self, user_id: int) -> Optional[User]:
        pass
    
    @abstractmethod
    def find_by_email(self, email: str) -> Optional[User]:
        pass
    
    @abstractmethod
    def save(self, user: User) -> User:
        pass
    
    @abstractmethod
    def delete(self, user_id: int) -> bool:
        pass

class InMemoryUserRepository(UserRepository):
    """In-memory implementation for testing/development."""
    
    def __init__(self):
        self._users: dict[int, User] = {}
        self._next_id = 1
    
    def find_by_id(self, user_id: int) -> Optional[User]:
        return self._users.get(user_id)
    
    def find_by_email(self, email: str) -> Optional[User]:
        for user in self._users.values():
            if user.email == email:
                return user
        return None
    
    def save(self, user: User) -> User:
        if user.id == 0:
            user.id = self._next_id
            self._next_id += 1
        self._users[user.id] = user
        return user
    
    def delete(self, user_id: int) -> bool:
        if user_id in self._users:
            del self._users[user_id]
            return True
        return False

# ============== Business Logic Layer ==============
class UserService:
    """Business logic for user management."""
    
    def __init__(self, user_repository: UserRepository):
        self._repository = user_repository
    
    def create_user(self, email: str, name: str) -> User:
        # Business rule: check for duplicate email
        existing = self._repository.find_by_email(email)
        if existing:
            raise ValueError(f"User with email {email} already exists")
        
        # Business rule: validate email format
        if not self._is_valid_email(email):
            raise ValueError("Invalid email format")
        
        # Create and save user
        user = User(
            id=0,  # Will be assigned by repository
            email=email,
            name=name,
            created_at=datetime.now()
        )
        return self._repository.save(user)
    
    def get_user(self, user_id: int) -> Optional[User]:
        return self._repository.find_by_id(user_id)
    
    def update_user(self, user_id: int, name: str) -> Optional[User]:
        user = self._repository.find_by_id(user_id)
        if not user:
            return None
        
        user.name = name
        return self._repository.save(user)
    
    def delete_user(self, user_id: int) -> bool:
        return self._repository.delete(user_id)
    
    def _is_valid_email(self, email: str) -> bool:
        """Validate email format."""
        return "@" in email and "." in email.split("@")[-1]

# ============== Presentation Layer ==============
from fastapi import FastAPI, HTTPException, status
from pydantic import BaseModel, EmailStr

app = FastAPI()

# Request/Response models
class CreateUserRequest(BaseModel):
    email: EmailStr  # Pydantic validates email format
    name: str

class UserResponse(BaseModel):
    id: int
    email: str
    name: str
    created_at: datetime

# Dependency injection
user_repository = InMemoryUserRepository()
user_service = UserService(user_repository)

# Routes
@app.post("/users", response_model=UserResponse, status_code=status.HTTP_201_CREATED)
async def create_user(request: CreateUserRequest) -> UserResponse:
    try:
        user = user_service.create_user(
            email=request.email,
            name=request.name
        )
        return UserResponse(
            id=user.id,
            email=user.email,
            name=user.name,
            created_at=user.created_at
        )
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))

@app.get("/users/{user_id}", response_model=UserResponse)
async def get_user(user_id: int) -> UserResponse:
    user = user_service.get_user(user_id)
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    return UserResponse(
        id=user.id,
        email=user.email,
        name=user.name,
        created_at=user.created_at
    )

@app.delete("/users/{user_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_user(user_id: int) -> None:
    deleted = user_service.delete_user(user_id)
    if not deleted:
        raise HTTPException(status_code=404, detail="User not found")
```

🔍 **Line-by-Line Breakdown:**

1. `from dataclasses import dataclass` — Dataclass for clean domain object definition.
2. `class User:` — Domain entity representing a user.
3. `class UserRepository(ABC):` — Abstract interface for data access. Defines contract without implementation.
4. `@abstractmethod` — Decorator forcing subclasses to implement these methods.
5. `class InMemoryUserRepository(UserRepository):` — Concrete implementation using in-memory storage.
6. `self._users: dict[int, User] = {}` — Dictionary mapping user IDs to User objects.
7. `class UserService:` — Business logic layer. Contains application rules.
8. `def __init__(self, user_repository: UserRepository):` — Dependency injection of repository.
9. `def create_user(self, email: str, name: str) -> User:` — Business logic method.
10. `if existing: raise ValueError(...)` — Business rule: no duplicate emails.
11. `def _is_valid_email(self, email: str) -> bool:` — Private method for validation.
12. `user = User(...)` — Create domain object.
13. `return self._repository.save(user)` — Delegate to data access layer.
14. `class CreateUserRequest(BaseModel):` — Pydantic model for request validation.
15. `EmailStr` — Pydantic type that validates email format automatically.
16. `@app.post("/users")` — FastAPI route (presentation layer).
17. `try/except ValueError` — Catch business logic errors and convert to HTTP responses.

## Benefits of Layered Architecture

1. **Separation of Concerns** - Each layer has a single, well-defined responsibility
2. **Testability** - Each layer can be tested independently with mocks
3. **Maintainability** - Changes in one layer don't affect others
4. **Reusability** - Business logic can be used by different interfaces (API, CLI, etc.)

## Summary

- Layered architecture separates concerns into presentation, business logic, and data access
- Each layer only talks to adjacent layers
- Dependency injection enables loose coupling and testing
- FastAPI with Pydantic provides a modern way to implement this pattern in Python

## Next Steps

Continue to `06-clean-architecture.md` to learn about domain-driven design principles.
