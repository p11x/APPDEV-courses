# Pytest Basics

## Overview

Pytest is the standard testing framework for Python and FastAPI. This guide covers pytest fundamentals for effective testing.

## Setup and Configuration

### Basic Configuration

```python
# Example 1: Pytest configuration (pyproject.toml)
"""
[tool.pytest.ini_options]
testpaths = ["tests"]
python_files = ["test_*.py"]
python_functions = ["test_*"]
python_classes = ["Test*"]
asyncio_mode = "auto"
addopts = [
    "-v",
    "--tb=short",
    "--strict-markers",
    "--disable-warnings"
]
markers = [
    "slow: marks tests as slow",
    "integration: marks integration tests",
    "unit: marks unit tests"
]
"""

# Directory structure
"""
app/
├── __init__.py
├── main.py
├── models/
├── routers/
├── services/
└── utils/
tests/
├── __init__.py
├── conftest.py          # Shared fixtures
├── unit/
│   ├── __init__.py
│   ├── test_models.py
│   ├── test_services.py
│   └── test_utils.py
├── integration/
│   ├── __init__.py
│   ├── test_routers.py
│   └── test_database.py
└── e2e/
    ├── __init__.py
    └── test_flows.py
"""
```

## Basic Test Structure

### Simple Tests

```python
# Example 2: Basic test patterns

# Simple function test
def add(a: int, b: int) -> int:
    return a + b

def test_add():
    """Basic test"""
    assert add(1, 2) == 3
    assert add(-1, 1) == 0
    assert add(0, 0) == 0

# Test with setup
class TestCalculator:
    """Group related tests in class"""

    def setup_method(self):
        """Runs before each test"""
        self.calculator = Calculator()

    def test_add(self):
        assert self.calculator.add(2, 3) == 5

    def test_subtract(self):
        assert self.calculator.subtract(5, 3) == 2

    def test_multiply(self):
        assert self.calculator.multiply(2, 3) == 6

    def test_divide(self):
        assert self.calculator.divide(6, 2) == 3

    def test_divide_by_zero(self):
        with pytest.raises(ZeroDivisionError):
            self.calculator.divide(1, 0)
```

## Test Assertions

### Assertion Patterns

```python
# Example 3: Common assertions

def test_equality():
    """Equality assertions"""
    assert 1 + 1 == 2
    assert "hello" == "hello"
    assert [1, 2, 3] == [1, 2, 3]

def test_comparison():
    """Comparison assertions"""
    assert 5 > 3
    assert 3 >= 3
    assert 1 < 2

def test_membership():
    """Membership assertions"""
    assert 1 in [1, 2, 3]
    assert "hello" in "hello world"
    assert "key" in {"key": "value"}

def test_identity():
    """Identity assertions"""
    assert None is None
    assert True is True
    assert [] is not None

def test_boolean():
    """Boolean assertions"""
    assert True
    assert not False
    assert bool([1, 2, 3])

def test_approximate():
    """Approximate equality for floats"""
    assert 0.1 + 0.2 == pytest.approx(0.3)
    assert 1.0001 == pytest.approx(1.0, abs=0.001)

def test_exceptions():
    """Exception assertions"""
    with pytest.raises(ValueError):
        int("not a number")

    with pytest.raises(KeyError) as exc_info:
        {}["missing"]
    assert "missing" in str(exc_info.value)

def test_collections():
    """Collection assertions"""
    assert len([1, 2, 3]) == 3
    assert all([True, True, True])
    assert any([False, True, False])

def test_strings():
    """String assertions"""
    assert "hello".startswith("he")
    assert "hello".endswith("lo")
    assert "hello".upper() == "HELLO"
```

## FastAPI Testing

### TestClient Setup

```python
# Example 4: FastAPI with TestClient
from fastapi import FastAPI, Depends
from fastapi.testclient import TestClient
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

app = FastAPI()

@app.get("/")
def root():
    return {"message": "Hello World"}

@app.get("/items/{item_id}")
def read_item(item_id: int, q: str = None):
    return {"item_id": item_id, "q": q}

# Basic test client usage
client = TestClient(app)

def test_read_root():
    response = client.get("/")
    assert response.status_code == 200
    assert response.json() == {"message": "Hello World"}

def test_read_item():
    response = client.get("/items/42")
    assert response.status_code == 200
    assert response.json() == {"item_id": 42, "q": None}

def test_read_item_with_query():
    response = client.get("/items/42?q=search")
    assert response.status_code == 200
    assert response.json() == {"item_id": 42, "q": "search"}

def test_read_item_not_found():
    response = client.get("/items/999")
    # Assuming endpoint returns 404 for non-existent items
    # assert response.status_code == 404
```

## Markers

### Test Markers

```python
# Example 5: Using pytest markers
import pytest

@pytest.mark.slow
def test_slow_operation():
    """Marked as slow - can be skipped with -m 'not slow'"""
    import time
    time.sleep(2)
    assert True

@pytest.mark.integration
def test_database_integration():
    """Marked as integration test"""
    # Database test code
    pass

@pytest.mark.skip(reason="Not implemented yet")
def test_future_feature():
    """Skip this test"""
    pass

@pytest.mark.skipif(
    sys.platform == "win32",
    reason="Does not work on Windows"
)
def test_unix_only():
    """Conditionally skip"""
    pass

@pytest.mark.parametrize("input,expected", [
    (1, 2),
    (2, 4),
    (3, 6),
])
def test_multiply_by_two(input, expected):
    """Parametrized test"""
    assert input * 2 == expected

# Running specific tests
# pytest -m slow              # Run only slow tests
# pytest -m "not slow"        # Skip slow tests
# pytest -m "unit or integration"  # Run both
```

## Fixtures

### Basic Fixtures

```python
# Example 6: Pytest fixtures
import pytest
from fastapi.testclient import TestClient

@pytest.fixture
def sample_data():
    """Provide test data"""
    return {
        "username": "testuser",
        "email": "test@example.com",
        "password": "testpassword123"
    }

@pytest.fixture
def client():
    """Provide test client"""
    from app.main import app
    return TestClient(app)

@pytest.fixture
def authenticated_client(client, sample_data):
    """Provide authenticated client"""
    # Register user
    client.post("/auth/register", json=sample_data)

    # Login
    response = client.post("/auth/login", json={
        "username": sample_data["username"],
        "password": sample_data["password"]
    })
    token = response.json()["access_token"]

    # Set auth header
    client.headers["Authorization"] = f"Bearer {token}"
    return client

@pytest.fixture(autouse=True)
def reset_database():
    """Runs before and after each test"""
    # Setup
    Base.metadata.create_all(engine)
    yield  # Test runs here
    # Teardown
    Base.metadata.drop_all(engine)

# Using fixtures
def test_public_endpoint(client):
    response = client.get("/public/")
    assert response.status_code == 200

def test_protected_endpoint(authenticated_client):
    response = authenticated_client.get("/protected/")
    assert response.status_code == 200
```

## Best Practices

### Testing Guidelines

```python
# Example 7: Pytest best practices
"""
Pytest Best Practices:

1. Use descriptive test names
   - test_<action>_<condition>_<result>
   - test_create_user_with_invalid_email_returns_422

2. One concept per test
   - Don't test multiple things
   - Keep tests focused

3. Use fixtures for setup
   - Share common setup code
   - Make tests DRY

4. Use parametrize for variations
   - Test multiple inputs
   - Reduce code duplication

5. Mark slow tests
   - @pytest.mark.slow
   - Run fast tests frequently

6. Clean up after tests
   - Use fixtures with yield
   - Don't leave test data

7. Mock external dependencies
   - Don't hit real APIs
   - Control test environment
"""

# Well-structured test
class TestUserRegistration:
    """User registration tests"""

    @pytest.fixture
    def valid_user_data(self):
        return {
            "username": "newuser",
            "email": "new@example.com",
            "password": "SecurePass123!"
        }

    def test_register_with_valid_data_succeeds(
        self, client, valid_user_data
    ):
        """Happy path test"""
        response = client.post("/auth/register", json=valid_user_data)

        assert response.status_code == 201
        assert response.json()["username"] == valid_user_data["username"]

    @pytest.mark.parametrize("invalid_email", [
        "notanemail",
        "@nodomain.com",
        "missing@.com",
        "",
    ])
    def test_register_with_invalid_email_fails(
        self, client, valid_user_data, invalid_email
    ):
        """Test various invalid emails"""
        valid_user_data["email"] = invalid_email
        response = client.post("/auth/register", json=valid_user_data)

        assert response.status_code == 422

    def test_register_with_duplicate_username_fails(
        self, client, valid_user_data
    ):
        """Test duplicate handling"""
        # First registration
        client.post("/auth/register", json=valid_user_data)

        # Duplicate registration
        response = client.post("/auth/register", json=valid_user_data)

        assert response.status_code == 400
        assert "already exists" in response.json()["detail"].lower()
```

## Summary

| Feature | Purpose | Example |
|---------|---------|---------|
| Fixtures | Setup/teardown | `@pytest.fixture` |
| Markers | Categorize tests | `@pytest.mark.slow` |
| Parametrize | Multiple inputs | `@pytest.mark.parametrize` |
| Assertions | Verify results | `assert x == y` |
| Exceptions | Error testing | `pytest.raises()` |

## Next Steps

Continue learning about:
- [Test Fixtures](./03_test_fixtures.md) - Advanced fixtures
- [Mock Objects](./04_mock_objects.md) - Mocking dependencies
- [Test Coverage](./06_test_coverage.md) - Measuring coverage
