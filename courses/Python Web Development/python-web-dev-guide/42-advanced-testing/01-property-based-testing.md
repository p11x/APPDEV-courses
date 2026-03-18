# Property-Based Testing

## What You'll Learn
- Property-based testing fundamentals
- Using Hypothesis library
- Writing property tests
- Strategies and customization

## Prerequisites
- Basic pytest knowledge

## What Is Property-Based Testing?

Instead of testing specific inputs, property-based testing tests that properties hold for ALL inputs:

```python
# Traditional: Test specific cases
assert reverse("hello") == "olleh"
assert reverse("") == ""

# Property-based: Test properties
# reverse(reverse(x)) == x for any string
```

## Installation

```bash
pip install hypothesis
```

## Basic Example

```python
from hypothesis import given, strategies as st

@given(st.lists(st.integers()))
def test_reverse_preserves_length(lst):
    """Reversing a list preserves its length."""
    original = lst.copy()
    lst.reverse()
    lst.reverse()
    assert lst == original

@given(st.strings())
def test_string_reverse(s):
    """Reversing twice returns original."""
    assert s[::-1][::-1] == s
```

## Custom Strategies

```python
@given(
    st.tuples(
        st.text(min_size=1),
        st.integers(min_value=0, max_value=100)
    )
)
def test_custom_strategy(data):
    """Test with custom strategy."""
    name, age = data
    assert len(name) >= 1
    assert 0 <= age <= 100
```

## Integration with FastAPI

```python
from hypothesis import given, settings
import hypothesis

# Test FastAPI endpoints
from fastapi.testclient import TestClient
from myapp import app

client = TestClient(app)

@given(st.integers(min_value=1))
@settings(max_examples=10)
def test_get_user(user_id):
    """Test user endpoint with random IDs."""
    response = client.get(f"/users/{user_id}")
    # Should either succeed or return 404
    assert response.status_code in [200, 404]
```

## Summary

- Property-based testing verifies properties hold for all inputs
- Use Hypothesis library for Python
- Define strategies for data generation
- Test properties like reversibility, commutativity, etc.
