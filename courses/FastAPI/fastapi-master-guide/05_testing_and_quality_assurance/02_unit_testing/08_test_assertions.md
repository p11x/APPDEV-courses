# Test Assertions

## Overview

Effective assertions verify that code behaves correctly. Pytest provides powerful assertion capabilities.

## Assertion Types

### Basic Assertions

```python
# Example 1: Basic assertions
def test_equality():
    """Equality assertions"""
    assert 1 + 1 == 2
    assert "hello" == "hello"
    assert [1, 2, 3] == [1, 2, 3]

def test_inequality():
    """Inequality assertions"""
    assert 1 != 2
    assert "hello" != "world"

def test_comparison():
    """Comparison assertions"""
    assert 5 > 3
    assert 3 >= 3
    assert 1 < 2
    assert 1 <= 1

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
```

### Exception Assertions

```python
# Example 2: Exception assertions
import pytest

def test_exception_raised():
    """Test that exception is raised"""
    with pytest.raises(ValueError):
        int("not a number")

def test_exception_message():
    """Test exception with specific message"""
    with pytest.raises(ValueError) as exc_info:
        int("invalid")

    assert "invalid literal" in str(exc_info.value)

def test_no_exception():
    """Test no exception is raised"""
    # This should not raise
    result = int("42")
    assert result == 42
```

### Approximate Assertions

```python
# Example 3: Approximate equality
def test_float_approximate():
    """Test approximate float equality"""
    assert 0.1 + 0.2 == pytest.approx(0.3)
    assert 1.0001 == pytest.approx(1.0, abs=0.001)

def test_collection_approximate():
    """Test approximate collection equality"""
    assert [0.1 + 0.2, 0.2 + 0.4] == pytest.approx([0.3, 0.6])
```

## FastAPI Assertions

```python
# Example 4: FastAPI response assertions
from fastapi.testclient import TestClient

def test_response_status(client: TestClient):
    response = client.get("/items/")
    assert response.status_code == 200

def test_response_json(client: TestClient):
    response = client.get("/items/1")
    data = response.json()
    assert "id" in data
    assert data["id"] == 1
    assert isinstance(data["name"], str)

def test_response_headers(client: TestClient):
    response = client.get("/items/")
    assert response.headers["content-type"] == "application/json"
```

## Summary

Pytest assertions are readable and provide detailed failure messages.

## Next Steps

Continue learning about:
- [Test Debugging](./09_test_debugging.md)
- [Test Helpers](./10_test_helpers.md)
