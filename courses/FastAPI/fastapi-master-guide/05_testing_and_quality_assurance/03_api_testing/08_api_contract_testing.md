# API Contract Testing

## Overview

Contract testing ensures API producers and consumers agree on the API interface.

## Contract Testing with Schemathesis

### Setup

```python
# Example 1: Schemathesis contract testing
# pip install schemathesis

import schemathesis

# Load OpenAPI schema
schema = schemathesis.from_uri("http://localhost:8000/openapi.json")

@schema.parametrize()
def test_api_contract(case):
    """Test API against OpenAPI schema"""
    response = case.call_and_validate()
    assert response.status_code < 500
```

### Pytest Integration

```python
# Example 2: Contract tests with pytest
import pytest
from hypothesis import settings, Phase

@pytest.fixture
def api_schema():
    return schemathesis.from_uri("http://localhost:8000/openapi.json")

@api_schema.parametrize()
@settings(max_examples=100, phases=[Phase.generate])
def test_contract(case):
    """Contract test with Hypothesis"""
    response = case.call()
    case.validate_response(response)
```

## Manual Contract Tests

```python
# Example 3: Manual contract verification
def test_response_schema_compliance(client):
    """Test response matches expected schema"""
    response = client.get("/items/1")
    data = response.json()

    # Verify required fields
    assert "id" in data
    assert "name" in data
    assert "price" in data

    # Verify types
    assert isinstance(data["id"], int)
    assert isinstance(data["name"], str)
    assert isinstance(data["price"], (int, float))
```

## Summary

Contract testing ensures API compatibility between services.

## Next Steps

Continue learning about:
- [API Security Testing](./11_api_security_testing.md)
- [API Performance Testing](./09_api_performance_testing.md)
