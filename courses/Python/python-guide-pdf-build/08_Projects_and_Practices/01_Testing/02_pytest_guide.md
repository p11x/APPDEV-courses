# pytest Guide

## What You'll Learn

- pytest basics
- Fixtures
- parametrize
- tmp_path, monkeypatch

## Prerequisites

- Read [01_unittest_basics.md](./01_unittest_basics.md) first

## Basic pytest

```python
# test_example.py
def test_add():
    assert 1 + 1 == 2
```

## Fixtures

```python
import pytest

@pytest.fixture
def data():
    return {"key": "value"}

def test_something(data):
    assert data["key"] == "value"
```

## parametrize

```python
@pytest.mark.parametrize("input,expected", [
    (1, 2),
    (2, 4),
    (3, 6),
])
def test_double(input, expected):
    assert input * 2 == expected
```

## Summary

- **pytest**: Popular testing framework
- **Fixtures**: Reusable test setup
- **parametrize**: Multiple test cases

## Next Steps

Continue to **[03_tdd_workflow.md](./03_tdd_workflow.md)**
