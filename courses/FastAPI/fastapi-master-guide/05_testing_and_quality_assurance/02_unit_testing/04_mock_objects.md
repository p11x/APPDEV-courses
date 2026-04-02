# Mock Objects

## Overview

Mocking replaces real dependencies with controlled test doubles, enabling isolated unit testing. Python's `unittest.mock` and `pytest-mock` provide comprehensive mocking capabilities.

## Basic Mocking

### Simple Mocks

```python
# Example 1: Basic mock usage
from unittest.mock import Mock, MagicMock, patch

# Mock object creation
def test_mock_basics():
    """Basic mock operations"""
    mock = Mock()

    # Call mock
    mock.method()
    mock.method.assert_called_once()

    # Mock with return value
    mock = Mock(return_value=42)
    assert mock() == 42

    # Mock with side effects
    mock = Mock(side_effect=[1, 2, 3])
    assert mock() == 1
    assert mock() == 2
    assert mock() == 3

    # Mock raising exception
    mock = Mock(side_effect=ValueError("Error"))
    with pytest.raises(ValueError):
        mock()

# Mocking class methods
class UserService:
    def get_user(self, user_id: int):
        # Would normally call database
        pass

def test_mock_class():
    """Mock class methods"""
    service = Mock(spec=UserService)
    service.get_user.return_value = {"id": 1, "name": "John"}

    result = service.get_user(1)
    assert result["name"] == "John"
    service.get_user.assert_called_once_with(1)
```

### Patching

```python
# Example 2: Patching dependencies
from unittest.mock import patch

# Module to test
def get_external_data():
    import requests
    response = requests.get("https://api.example.com/data")
    return response.json()

# Test with patch
@patch("requests.get")
def test_get_external_data(mock_get):
    """Patch external HTTP call"""
    # Configure mock
    mock_get.return_value.json.return_value = {"key": "value"}

    # Call function
    result = get_external_data()

    # Verify
    assert result == {"key": "value"}
    mock_get.assert_called_once_with("https://api.example.com/data")

# Patch as context manager
def test_with_context_manager():
    """Use patch as context manager"""
    with patch("requests.get") as mock_get:
        mock_get.return_value.json.return_value = {"data": "test"}

        result = get_external_data()
        assert result == {"data": "test"}

# Patching class
class DatabaseService:
    def query(self, sql):
        pass

@patch.object(DatabaseService, "query")
def test_with_object_patch(mock_query):
    """Patch specific object method"""
    mock_query.return_value = [{"id": 1}]

    service = DatabaseService()
    result = service.query("SELECT * FROM users")

    assert result == [{"id": 1}]
```

## FastAPI Mocking

### Mocking Dependencies

```python
# Example 3: Mocking FastAPI dependencies
from fastapi import FastAPI, Depends
from fastapi.testclient import TestClient
from unittest.mock import Mock, patch

app = FastAPI()

# Dependency to mock
def get_current_user():
    # Would normally verify JWT
    raise NotImplementedError

def get_db():
    # Would normally provide database session
    raise NotImplementedError

@app.get("/users/me")
def read_users_me(user=Depends(get_current_user)):
    return user

# Mocking dependencies in tests
def test_read_users_me():
    """Test with mocked dependency"""
    mock_user = {"id": 1, "username": "testuser"}

    app.dependency_overrides[get_current_user] = lambda: mock_user

    client = TestClient(app)
    response = client.get("/users/me")

    assert response.status_code == 200
    assert response.json() == mock_user

    app.dependency_overrides.clear()

# Mocking service layer
@patch("app.services.user_service.UserService.get_user")
def test_get_user_endpoint(mock_get_user):
    """Mock service layer"""
    mock_get_user.return_value = UserResponse(
        id=1,
        username="testuser",
        email="test@example.com"
    )

    client = TestClient(app)
    response = client.get("/users/1")

    assert response.status_code == 200
    assert response.json()["username"] == "testuser"
```

## Async Mocking

### AsyncMock

```python
# Example 4: Mocking async functions
from unittest.mock import AsyncMock
import pytest

# Async function to mock
async def fetch_data(url: str) -> dict:
    import httpx
    async with httpx.AsyncClient() as client:
        response = await client.get(url)
        return response.json()

# Mocking async
@pytest.mark.asyncio
@patch("httpx.AsyncClient.get")
async def test_fetch_data(mock_get):
    """Mock async HTTP call"""
    mock_response = Mock()
    mock_response.json.return_value = {"status": "ok"}
    mock_get.return_value = mock_response

    result = await fetch_data("https://api.example.com")

    assert result == {"status": "ok"}
    mock_get.assert_called_once()

# AsyncMock directly
@pytest.mark.asyncio
async def test_async_service():
    """Test with AsyncMock"""
    mock_service = AsyncMock()
    mock_service.fetch_user.return_value = {
        "id": 1,
        "name": "John"
    }

    result = await mock_service.fetch_user(1)

    assert result["name"] == "John"
    mock_service.fetch_user.assert_awaited_once_with(1)
```

## Mock Assertions

### Verification Methods

```python
# Example 5: Mock assertions
from unittest.mock import Mock, call

def test_mock_assertions():
    """Various mock assertions"""
    mock = Mock()

    # Call mock
    mock.method()
    mock.method("arg1")
    mock.method(arg2="value")

    # Assertions
    mock.method.assert_called()
    mock.method.assert_called_once()
    mock.method.assert_called_with(arg2="value")
    mock.method.assert_any_call(arg2="value")

    # Call count
    assert mock.method.call_count == 3

    # Call args
    assert mock.method.call_args_list == [
        call(),
        call("arg1"),
        call(arg2="value")
    ]

    # Reset mock
    mock.reset_mock()
    assert mock.method.call_count == 0

# Assert not called
def test_mock_not_called():
    """Verify mock was not called"""
    mock = Mock()
    mock.method.assert_not_called()

# Assert called with specific args
def test_mock_specific_args():
    """Verify specific arguments"""
    mock = Mock()
    mock.method("arg1", kwarg1="value1")

    mock.method.assert_called_with("arg1", kwarg1="value1")
```

## Mock Patterns

### Common Patterns

```python
# Example 6: Common mock patterns

# Pattern 1: Mock external API
@patch("app.external.api_client.APIClient.fetch")
def test_with_external_api(mock_fetch):
    """Mock external API call"""
    mock_fetch.return_value = {"data": "external"}

    service = DataService()
    result = service.get_data()

    assert result.source == "external"
    mock_fetch.assert_called_once()

# Pattern 2: Mock database
@pytest.fixture
def mock_db():
    """Mock database session"""
    db = Mock()
    db.query.return_value.filter.return_value.first.return_value = None
    db.add = Mock()
    db.commit = Mock()
    return db

def test_create_user(mock_db):
    """Test with mocked database"""
    service = UserService(mock_db)
    user = service.create_user({"username": "test"})

    mock_db.add.assert_called_once()
    mock_db.commit.assert_called_once()

# Pattern 3: Mock time/datetime
@patch("app.services.notification.datetime")
def test_scheduled_notification(mock_datetime):
    """Mock datetime for time-dependent tests"""
    from datetime import datetime

    mock_datetime.now.return_value = datetime(2024, 1, 1, 12, 0)

    service = NotificationService()
    notification = service.create_scheduled()

    assert notification.scheduled_for == datetime(2024, 1, 1, 12, 0)

# Pattern 4: Mock file system
@patch("builtins.open")
@patch("os.path.exists")
def test_read_config(mock_exists, mock_open):
    """Mock file operations"""
    mock_exists.return_value = True
    mock_open.return_value.__enter__().read.return_value = '{"key": "value"}'

    config = load_config("config.json")

    assert config["key"] == "value"
```

## Best Practices

### Mocking Guidelines

```python
# Example 7: Mocking best practices
"""
Mocking Best Practices:

1. Mock at the right level
   - Mock external dependencies
   - Don't mock what you're testing

2. Use spec for type safety
   - Mock(spec=RealClass)
   - Catches method name typos

3. Verify important interactions
   - Assert critical calls were made
   - Don't over-specify

4. Keep mocks simple
   - Complex mocks = complex tests
   - Prefer real objects when possible

5. Document mock behavior
   - Explain what mock simulates
   - Why it's mocked

6. Use fixtures for common mocks
   - Share mock setup
   - Keep tests DRY
"""

# Good: Mock external service
@patch("app.services.email.send_email")
def test_user_registration(mock_send):
    """Mock external email service"""
    mock_send.return_value = True

    response = client.post("/register", json={...})

    assert response.status_code == 201
    mock_send.assert_called_once()

# Bad: Mocking internal logic
@patch("app.services.user.validate_password")
def test_bad_example(mock_validate):
    """Don't mock what you're testing"""
    # This tests nothing - just mocks everything!
    pass
```

## Summary

| Mock Type | Purpose | Example |
|-----------|---------|---------|
| Mock | Generic double | `Mock()` |
| MagicMock | With magic methods | `MagicMock()` |
| AsyncMock | Async functions | `AsyncMock()` |
| patch | Replace objects | `@patch("module.func")` |
| patch.object | Patch specific object | `@patch.object(Cls, "method")` |

## Next Steps

Continue learning about:
- [Test Spies](./11_test_spies.md) - Recording calls
- [Test Stubs](./12_test_stubs.md) - Providing responses
- [Test Doubles](./13_test_doubles.md) - Double types
