# Test Parameterization

## Overview

Parameterized tests run the same test logic with different inputs, reducing code duplication.

## Basic Parameterization

### Using pytest.mark.parametrize

```python
# Example 1: Basic parameterization
import pytest

@pytest.mark.parametrize("input,expected", [
    (1, 2),
    (2, 4),
    (3, 6),
    (0, 0),
    (-1, -2),
])
def test_multiply_by_two(input, expected):
    """Test with multiple inputs"""
    assert input * 2 == expected

@pytest.mark.parametrize("a,b,result", [
    (1, 1, 2),
    (2, 3, 5),
    (10, 20, 30),
    (-1, 1, 0),
])
def test_addition(a, b, result):
    """Test addition with multiple inputs"""
    assert a + b == result
```

## FastAPI Parameterization

```python
# Example 2: API endpoint parameterization
import pytest
from fastapi.testclient import TestClient

@pytest.mark.parametrize("status_code,username,password", [
    (200, "valid_user", "valid_pass"),
    (401, "valid_user", "wrong_pass"),
    (401, "invalid_user", "any_pass"),
    (422, "", "password"),  # Missing username
    (422, "user", ""),      # Missing password
])
def test_login(client: TestClient, status_code, username, password):
    """Test login with various credentials"""
    response = client.post("/login", json={
        "username": username,
        "password": password
    })
    assert response.status_code == status_code

@pytest.mark.parametrize("item_id,expected_status", [
    (1, 200),
    (2, 200),
    (999, 404),
    (0, 422),
    (-1, 422),
])
def test_get_item(client: TestClient, item_id, expected_status):
    """Test get item with various IDs"""
    response = client.get(f"/items/{item_id}")
    assert response.status_code == expected_status
```

## Complex Parameterization

```python
# Example 3: Complex parameter combinations
@pytest.mark.parametrize("price,quantity,expected_total", [
    (10.0, 1, 11.0),    # 10 + 10% tax
    (10.0, 2, 22.0),    # 20 + 10% tax
    (0.0, 1, 0.0),      # Free item
    (100.0, 0, 0.0),    # Zero quantity
])
def test_order_total(price, quantity, expected_total):
    """Test order total calculation"""
    tax_rate = 0.1
    total = price * quantity * (1 + tax_rate)
    assert total == pytest.approx(expected_total)

# Nested parameterization
@pytest.mark.parametrize("x", [1, 2])
@pytest.mark.parametrize("y", [10, 20])
def test_multiply(x, y):
    """Runs 4 tests: (1,10), (1,20), (2,10), (2,20)"""
    assert x * y > 0
```

## Summary

Parameterization reduces test code duplication and improves coverage.

## Next Steps

Continue learning about:
- [Test Assertions](./08_test_assertions.md)
- [Test Debugging](./09_test_debugging.md)
