# Dependency Testing

## Overview

Testing dependencies in isolation and with route handlers ensures your API behaves correctly. FastAPI provides built-in support for dependency overrides in tests.

## Testing Dependencies

### Basic Dependency Testing

```python
# Example 1: Testing with dependency overrides
from fastapi import FastAPI, Depends
from fastapi.testclient import TestClient

app = FastAPI()

def get_db():
    """Production database dependency"""
    return {"type": "production", "connected": True}

@app.get("/items/")
async def list_items(db: dict = Depends(get_db)):
    return {"db": db["type"], "items": []}

# Test with mock dependency
def get_test_db():
    """Test database dependency"""
    return {"type": "test", "connected": True}

# Override dependency for testing
app.dependency_overrides[get_db] = get_test_db

client = TestClient(app)

def test_list_items():
    response = client.get("/items/")
    assert response.status_code == 200
    assert response.json()["db"] == "test"

# Clear overrides after tests
app.dependency_overrides = {}
```

### Testing Authentication

```python
# Example 2: Testing authenticated endpoints
from fastapi import FastAPI, Depends, HTTPException, Header
from fastapi.testclient import TestClient
from typing import Optional

app = FastAPI()

def get_current_user(authorization: Optional[str] = Header(None)):
    """Auth dependency"""
    if not authorization:
        raise HTTPException(401, "Not authenticated")
    if authorization != "Bearer valid-token":
        raise HTTPException(401, "Invalid token")
    return {"user_id": 1, "username": "john"}

@app.get("/profile/")
async def get_profile(user: dict = Depends(get_current_user)):
    return {"user": user}

# Test with mock user
def get_mock_user():
    return {"user_id": 999, "username": "testuser"}

app.dependency_overrides[get_current_user] = get_mock_user

client = TestClient(app)

def test_profile():
    response = client.get("/profile/")
    assert response.status_code == 200
    assert response.json()["user"]["username"] == "testuser"

app.dependency_overrides = {}
```

## Testing Patterns

### Database Testing

```python
# Example 3: Database testing pattern
from fastapi import FastAPI, Depends
from fastapi.testclient import TestClient

app = FastAPI()

# Production DB
class Database:
    def __init__(self, url: str):
        self.url = url

    def get_items(self):
        return [{"id": 1, "name": "Real Item"}]

def get_db():
    return Database("postgresql://prod/db")

@app.get("/items/")
async def list_items(db: Database = Depends(get_db)):
    return {"items": db.get_items()}

# Test DB with in-memory data
class TestDatabase:
    def __init__(self):
        self.items = [{"id": 1, "name": "Test Item"}]

    def get_items(self):
        return self.items

def get_test_db():
    return TestDatabase()

# Test
def test_items():
    app.dependency_overrides[get_db] = get_test_db

    client = TestClient(app)
    response = client.get("/items/")

    assert response.status_code == 200
    assert response.json()["items"][0]["name"] == "Test Item"

    app.dependency_overrides = {}
```

## Best Practices

### Testing Guidelines

```python
# Example 4: Best practices
from fastapi import FastAPI, Depends
from fastapi.testclient import TestClient
import pytest

app = FastAPI()

# Use fixtures for dependency management
@pytest.fixture
def override_db():
    """Fixture to manage DB override"""
    original = app.dependency_overrides.copy()

    def mock_db():
        return {"type": "test"}

    app.dependency_overrides[get_db] = mock_db
    yield
    app.dependency_overrides = original

@pytest.fixture
def client():
    return TestClient(app)

def test_with_override(client, override_db):
    response = client.get("/items/")
    assert response.status_code == 200
```

## Summary

| Technique | Usage | Example |
|-----------|-------|---------|
| `dependency_overrides` | Replace deps | `app.dependency_overrides[dep] = mock` |
| Clear overrides | Reset state | `app.dependency_overrides = {}` |
| Fixtures | Manage lifecycle | `@pytest.fixture` |
| Mock classes | Test implementations | `TestDatabase` class |

## Next Steps

Continue learning about:
- [Middleware Overview](../06_middleware/01_middleware_overview.md) - Request processing
- [CORS Middleware](../06_middleware/02_cors_middleware.md) - Cross-origin
- [Custom Middleware](../06_middleware/03_custom_middleware.md) - Custom processing
