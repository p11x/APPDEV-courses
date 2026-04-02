# API Security Testing

## Overview

Security testing identifies vulnerabilities in FastAPI APIs.

## Security Tests

### Authentication Tests

```python
# Example 1: Security test patterns
import pytest
from fastapi.testclient import TestClient

class TestAPISecurity:
    """API security tests"""

    def test_protected_endpoint_requires_auth(self, client: TestClient):
        """Test auth requirement"""
        response = client.get("/protected/")
        assert response.status_code == 401

    def test_invalid_token_rejected(self, client: TestClient):
        """Test invalid token handling"""
        response = client.get(
            "/protected/",
            headers={"Authorization": "Bearer invalid"}
        )
        assert response.status_code == 401

    def test_expired_token_rejected(self, client: TestClient, expired_token):
        """Test expired token"""
        response = client.get(
            "/protected/",
            headers={"Authorization": f"Bearer {expired_token}"}
        )
        assert response.status_code == 401

    @pytest.mark.parametrize("payload", [
        "' OR '1'='1",
        "<script>alert('xss')</script>",
        "../../../etc/passwd"
    ])
    def test_injection_prevention(self, client: TestClient, payload):
        """Test injection attack prevention"""
        response = client.post("/items/", json={"name": payload})
        assert response.status_code in [200, 400, 422]
```

## Summary

Security testing protects APIs from common vulnerabilities.

## Next Steps

Continue learning about:
- [API Rate Limiting Testing](./16_api_rate_limiting_testing.md)
- [Penetration Testing](../../03_authentication_and_security/07_security_testing/03_penetration_testing_checklist.md)
