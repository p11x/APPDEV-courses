# Dependency Injection

## What You'll Learn
- What dependency injection is
- Creating dependencies
- Using dependencies in routes
- Dependency parameters
- Overriding dependencies
- Practical use cases

## Prerequisites
- Completed FastAPI Request Body section

## What Is Dependency Injection?

**Dependency injection** is a design pattern where a function or class receives its dependencies from external sources rather than creating them itself.

In FastAPI, dependencies are functions that run before your route handlers. They're perfect for:

- Database connections
- Authentication
- Request validation
- Shared logic

Think of dependencies like having a assistant prepare everything before you start working.

## Basic Dependencies

```python
from fastapi import FastAPI, Depends

app = FastAPI()

# A simple dependency
def get_database():
    """Simulate a database connection."""
    return {"connection": "database_connected"}

@app.get("/items")
def read_items(db: dict = Depends(get_database)) -> dict:
    """Read items using the database dependency."""
    return {"items": ["item1", "item2"], "db": db}

@app.get("/users")
def read_users(db: dict = Depends(get_database)) -> dict:
    """Read users using the same dependency."""
    return {"users": ["user1", "user2"], "db": db}
```

🔍 **Dependency Breakdown:**

1. `def get_database()` — The dependency function (returns something)
2. `db: dict = Depends(get_database)` — FastAPI calls the dependency and injects the result
3. Multiple routes can use the same dependency

## Dependencies with Parameters

Dependencies can accept parameters:

```python
from fastapi import FastAPI, Depends
from typing import Annotated

app = FastAPI()

def get_query_param(q: str | None = None) -> str:
    """Dependency that extracts a query parameter."""
    if q:
        return f"Query: {q}"
    return "No query provided"

@app.get("/search")
def search(q: Annotated[str, Depends(get_query_param)]) -> dict[str, str]:
    return {"message": q}

# Shorter syntax (automatic)
@app.get("/search2")
def search2(q: str | None = None) -> dict[str, str]:
    message = f"Query: {q}" if q else "No query provided"
    return {"message": message}
```

## Practical Example: Database Session

```python
from fastapi import FastAPI, Depends
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, Session
from typing import Generator

# Database setup (simplified)
DATABASE_URL = "sqlite:///./test.db"
engine = create_engine(DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Dependency to get database session
def get_db() -> Generator[Session, None, None]:
    """Get database session for each request."""
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# Using in routes
@app.get("/users")
def get_users(db: Session = Depends(get_db)) -> dict:
    # In a real app, you'd query: db.query(User).all()
    return {"users": ["Alice", "Bob"]}
```

## Classes as Dependencies

Classes can be dependencies — FastAPI calls them as functions:

```python
from fastapi import FastAPI, Depends

app = FastAPI()

class DatabaseConnection:
    """Database connection class."""
    def __init__(self):
        self.connected = True
    
    def query(self, table: str) -> list:
        return [f"{table}_item1", f"{table}_item2"]

def get_database() -> DatabaseConnection:
    """Dependency that returns a database instance."""
    return DatabaseConnection()

@app.get("/items")
def read_items(db: DatabaseConnection = Depends(get_database)) -> dict:
    return {"items": db.query("items")}

@app.get("/users")
def read_users(db: DatabaseConnection = Depends(get_database)) -> dict:
    return {"users": db.query("users")}
```

## Dependency with Return Value

Dependencies can return values used in route handlers:

```python
from fastapi import FastAPI, Depends, HTTPException
from typing import Annotated

app = FastAPI()

# Simulated user database
FAKE_USERS = {
    1: {"id": 1, "name": "Alice", "email": "alice@example.com"},
    2: {"id": 2, "name": "Bob", "email": "bob@example.com"}
}

def get_current_user(user_id: int) -> dict:
    """Get current user or raise 401."""
    if user_id not in FAKE_USERS:
        raise HTTPException(status_code=401, detail="Not authenticated")
    return FAKE_USERS[user_id]

@app.get("/profile")
def get_profile(user: dict = Depends(get_current_user)) -> dict:
    """Get current user's profile."""
    return user

# With query parameter
@app.get("/users/{user_id}")
def get_user(user_id: int, current_user: dict = Depends(get_current_user)) -> dict:
    """Get a user (must be authenticated)."""
    return FAKE_USERS.get(user_id, {"error": "Not found"})
```

## Dependency Chains

Dependencies can depend on other dependencies:

```python
from fastapi import FastAPI, Depends

app = FastAPI()

def get_database():
    """Database dependency."""
    return {"db": "connected"}

def get_repository(db: dict = Depends(get_database)):
    """Repository depends on database."""
    class Repository:
        def find_all(self):
            return ["item1", "item2"]
    return Repository()

def get_service(repo: Repository = Depends(get_repository)):
    """Service depends on repository."""
    class Service:
        def get_items(self):
            return repo.find_all()
    return Service()

@app.get("/items")
def get_items(service: Service = Depends(get_service)) -> dict:
    return {"items": service.get_items()}
```

## Practical Example: Pagination

```python
from fastapi import FastAPI, Depends, Query
from typing import Annotated

app = FastAPI()

ITEMS = [{"name": f"Item {i}"} for i in range(100)]

class PaginationParams:
    """Pagination dependency."""
    def __init__(self, skip: int = Query(0, ge=0), limit: int = Query(10, ge=1, le=100)):
        self.skip = skip
        self.limit = limit

@app.get("/items")
def get_items(pagination: PaginationParams = Depends()) -> dict:
    start = pagination.skip
    end = start + pagination.limit
    return {
        "items": ITEMS[start:end],
        "total": len(ITEMS),
        "skip": pagination.skip,
        "limit": pagination.limit
    }
```

## Overriding Dependencies

You can override dependencies for testing:

```python
from fastapi import FastAPI, Depends

app = FastAPI()

def get_expensive_service():
    """Production dependency."""
    return {"service": "expensive"}

@app.get("/")
def root(service: dict = Depends(get_expensive_service)) -> dict:
    return service

# Testing - override the dependency
@app.get("/")
def root(service: dict = Depends(lambda: {"service": "fake"})) -> dict:
    return service
```

## Using Annotated for Cleaner Code

```python
from fastapi import FastAPI, Depends
from typing import Annotated

app = FastAPI()

# Without Annotated (verbose)
def get_db_verbose():
    return "database"

@app.get("/items1")
def items1(db: str = Depends(get_db_verbose)) -> dict:
    return {"db": db}

# With Annotated (cleaner)
def get_db() -> str:
    return "database"

@app.get("/items2")
def items2(db: Annotated[str, Depends(get_db)]) -> dict:
    return {"db": db}

# With default dependency
def get_default_settings() -> dict:
    return {"theme": "dark"}

@app.get("/settings")
def get_settings(
    settings: dict = Depends(get_default_settings)
) -> dict:
    return settings
```

## Summary
- **Dependencies** are functions that run before route handlers
- Use `Depends()` to inject dependencies into routes
- Dependencies can be simple functions or classes
- Dependencies can accept parameters and return values
- Dependencies can chain (depend on other dependencies)
- Use `Annotated[Type, Depends(...)]` for cleaner syntax

## Next Steps
→ Continue to `05-authentication-and-jwt.md` to learn about JWT authentication in FastAPI.
