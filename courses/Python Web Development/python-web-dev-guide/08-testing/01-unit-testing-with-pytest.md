# Unit Testing with Pytest

## What You'll Learn
- Writing unit tests
- Using pytest
- Test organization

## Prerequisites
- Basic Python knowledge

## Installing Pytest

```bash
pip install pytest
```

## Writing Tests

```python
# test_math.py

def add(a, b):
    return a + b

def test_add():
    assert add(1, 2) == 3
    assert add(0, 0) == 0
    assert add(-1, 1) == 0

def test_add_strings():
    assert add("hello", " world") == "hello world"
```

## Running Tests

```bash
pytest test_math.py
pytest -v  # Verbose
pytest --collect-only  # Show tests without running
```

## Test Fixtures

```python
import pytest

@pytest.fixture
def user_data():
    return {"name": "Alice", "email": "alice@example.com"}

def test_user(user_data):
    assert user_data["name"] == "Alice"
```

## Summary
- Tests go in `test_*.py` files
- Use `assert` to check conditions
- Use fixtures for setup
- Run with `pytest`
