# HTTP Status Code Testing

## Overview

Testing HTTP status codes ensures APIs return appropriate responses for different scenarios.

## Status Code Tests

### Success Codes

```python
# Example 1: Testing success status codes
import pytest
from fastapi.testclient import TestClient

class TestSuccessCodes:
    """Test successful HTTP responses"""

    def test_200_ok(self, client: TestClient):
        """Test 200 OK for successful GET"""
        response = client.get("/items/")
        assert response.status_code == 200

    def test_201_created(self, client: TestClient):
        """Test 201 Created for successful POST"""
        response = client.post("/items/", json={
            "name": "New Item",
            "price": 10.0
        })
        assert response.status_code == 201

    def test_204_no_content(self, client: TestClient):
        """Test 204 No Content for successful DELETE"""
        response = client.delete("/items/1")
        assert response.status_code == 204
```

### Client Error Codes

```python
# Example 2: Testing error status codes
class TestClientErrors:
    """Test client error responses"""

    def test_400_bad_request(self, client: TestClient):
        """Test 400 for invalid input"""
        response = client.post("/items/", json={
            "name": "",  # Invalid
            "price": -10  # Invalid
        })
        assert response.status_code == 400

    def test_401_unauthorized(self, client: TestClient):
        """Test 401 for missing auth"""
        response = client.get("/protected/")
        assert response.status_code == 401

    def test_403_forbidden(self, client: TestClient, auth_client):
        """Test 403 for insufficient permissions"""
        response = auth_client.get("/admin/")
        assert response.status_code == 403

    def test_404_not_found(self, client: TestClient):
        """Test 404 for missing resource"""
        response = client.get("/items/99999")
        assert response.status_code == 404

    def test_422_validation_error(self, client: TestClient):
        """Test 422 for validation failures"""
        response = client.post("/items/", json={})
        assert response.status_code == 422
```

## Summary

Comprehensive status code testing ensures proper API behavior.

## Next Steps

Continue learning about:
- [Request/Response Validation](./03_request_response_validation.md)
- [API Authentication Testing](./04_api_authentication_testing.md)
