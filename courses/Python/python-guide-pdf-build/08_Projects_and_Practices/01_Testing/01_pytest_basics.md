# Pytest Basics

## What You'll Learn

- Writing basic tests with pytest
- Running tests
- Test discovery
- Assertions

## Prerequisites

- Read [07_loguru_logging.md](../../06_Modules_and_Packages/03_Popular_Third_Party/05_loguru_logging.md) first

## Writing Tests

```python
# test_example.py

def add(a: int, b: int) -> int:
    return a + b


def test_add():
    assert add(1, 2) == 3
    assert add(0, 0) == 0
    assert add(-1, 1) == 0


def test_add_strings():
    assert add("hello", "world") == "helloworld"
```

## Running Tests

```bash
# Run all tests
pytest

# Run specific test file
pytest test_example.py

# Run with verbose output
pytest -v

# Run tests matching pattern
pytest -k "test_add"
```

## Annotated Full Example

```python
# test_demo.py
"""Complete pytest demonstration."""

import pytest


def multiply(a: int, b: int) -> int:
    return a * b


def test_multiply_basic():
    assert multiply(3, 4) == 12
    assert multiply(0, 10) == 0
    assert multiply(-2, 5) == -10


def test_multiply_edge_cases():
    assert multiply(1, 1) == 1
    assert multiply(1000, 1000) == 1000000


if __name__ == "__main__":
    pytest.main([__file__, "-v"])
```

## Summary

- Writing basic tests with pytest
- Running tests
- Test discovery

## Next Steps

Continue to **[02_fixtures_and_conftest.md](./02_fixtures_and_conftest.md)**
