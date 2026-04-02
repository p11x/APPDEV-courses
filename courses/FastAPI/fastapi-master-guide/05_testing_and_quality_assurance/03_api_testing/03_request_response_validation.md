# Request Response Validation

## Overview

Testing request and response validation ensures APIs properly handle data formats.

## Validation Tests

### Request Validation

```python
# Example 1: Request validation tests
import pytest
from fastapi.testclient import TestClient

class TestRequestValidation:
    """Test request validation"""

    def test_missing_required_field(self, client: TestClient):
        """Test missing required field"""
        response = client.post("/users/", json={
            "username": "test"
            # Missing email
        })
        assert response.status_code == 422
        errors = response.json()["detail"]
        assert any("email" in str(e) for e in errors)

    def test_invalid_email_format(self, client: TestClient):
        """Test invalid email"""
        response = client.post("/users/", json={
            "username": "test",
            "email": "not-an-email"
        })
        assert response.status_code == 422

    def test_field_too_short(self, client: TestClient):
        """Test minimum length validation"""
        response = client.post("/users/", json={
            "username": "ab",  # Too short
            "email": "test@example.com"
        })
        assert response.status_code == 422

    def test_field_too_long(self, client: TestClient):
        """Test maximum length validation"""
        response = client.post("/users/", json={
            "username": "a" * 100,  # Too long
            "email": "test@example.com"
        })
        assert response.status_code == 422
```

### Response Validation

```python
# Example 2: Response validation tests
class TestResponseValidation:
    """Test response format"""

    def test_response_schema(self, client: TestClient):
        """Test response matches schema"""
        response = client.get("/users/1")
        data = response.json()

        assert "id" in data
        assert "username" in data
        assert "email" in data
        assert isinstance(data["id"], int)

    def test_no_sensitive_data_in_response(self, client: TestClient):
        """Test sensitive data excluded"""
        response = client.get("/users/1")
        data = response.json()

        assert "password" not in data
        assert "hashed_password" not in data
```

## Summary

Validation tests ensure data integrity and security.

## Next Steps

Continue learning about:
- [HTTP Status Codes](./02_http_status_codes.md)
- [API Authentication Testing](./04_api_authentication_testing.md)
