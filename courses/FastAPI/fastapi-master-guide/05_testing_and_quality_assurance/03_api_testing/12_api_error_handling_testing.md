# API Error Handling Testing

## Overview

Testing error handling ensures APIs return appropriate error responses.

## Error Tests

### HTTP Error Codes

```python
# Example 1: Error response tests
import pytest
from fastapi.testclient import TestClient

class TestErrorHandling:
    """Test error handling"""

    def test_404_not_found(self, client: TestClient):
        """Test 404 for missing resource"""
        response = client.get("/items/99999")
        assert response.status_code == 404
        assert "detail" in response.json()

    def test_400_bad_request(self, client: TestClient):
        """Test 400 for invalid input"""
        response = client.post("/items/", json={
            "price": -10  # Invalid
        })
        assert response.status_code == 400

    def test_422_validation_error(self, client: TestClient):
        """Test 422 for validation errors"""
        response = client.post("/items/", json={})
        assert response.status_code == 422
        assert "detail" in response.json()

    def test_500_internal_error(self, client: TestClient):
        """Test 500 for server errors"""
        response = client.get("/error-test/")
        assert response.status_code == 500
```

### Error Response Format

```python
# Example 2: Error format tests
def test_error_response_format(client: TestClient):
    """Test error response structure"""
    response = client.get("/items/99999")
    error = response.json()

    assert "detail" in error
    assert isinstance(error["detail"], (str, list))
```

## Summary

Error handling tests ensure proper error responses.

## Next Steps

Continue learning about:
- [API Security Testing](./11_api_security_testing.md)
- [API Contract Testing](./08_api_contract_testing.md)
