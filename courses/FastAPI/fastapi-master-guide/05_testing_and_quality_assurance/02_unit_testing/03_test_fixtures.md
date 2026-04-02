# Test Fixtures

## Overview

Pytest fixtures provide reusable setup and teardown code for tests.

## Basic Fixtures

### Simple Fixtures

```python
# Example 1: Basic fixtures
import pytest
from fastapi.testclient import TestClient

@pytest.fixture
def sample_data():
    """Provide test data"""
    return {
        "username": "testuser",
        "email": "test@example.com"
    }

@pytest.fixture
def client():
    """Provide test client"""
    from app.main import app
    return TestClient(app)

@pytest.fixture
def db_session():
    """Provide test database session"""
    session = TestSession()
    yield session
    session.close()
```

### Fixture Scopes

```python
# Example 2: Fixture scopes
# Function scope (default)
@pytest.fixture
def user(db_session):
    return User(username="test")

# Session scope
@pytest.fixture(scope="session")
def database_engine():
    engine = create_engine("sqlite:///:memory:")
    Base.metadata.create_all(engine)
    yield engine
    engine.dispose()

# Module scope
@pytest.fixture(scope="module")
def module_data():
    return {"shared": "data"}
```

## Using Fixtures

```python
# Example 3: Using fixtures in tests
def test_create_user(client, sample_data):
    """Test using fixtures"""
    response = client.post("/users/", json=sample_data)
    assert response.status_code == 201

def test_get_user(client, db_session):
    """Test with database fixture"""
    user = User(username="test")
    db_session.add(user)
    db_session.commit()

    response = client.get(f"/users/{user.id}")
    assert response.status_code == 200
```

## Summary

Fixtures provide reusable test setup and improve test organization.

## Next Steps

Continue learning about:
- [Mock Objects](./04_mock_objects.md)
- [Test Data Factories](./05_test_data_factories.md)
