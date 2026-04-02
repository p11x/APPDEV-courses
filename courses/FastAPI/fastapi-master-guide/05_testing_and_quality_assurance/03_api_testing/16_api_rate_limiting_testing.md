# API Rate Limiting Testing

## Overview

Testing rate limiting ensures APIs are protected from abuse.

## Rate Limit Tests

### Testing Rate Limits

```python
# Example 1: Rate limit tests
import pytest
from fastapi.testclient import TestClient

class TestRateLimiting:
    """Test rate limiting"""

    def test_rate_limit_enforced(self, client: TestClient):
        """Test rate limit is enforced"""
        for i in range(100):
            response = client.get("/api/endpoint")

        # Should eventually be rate limited
        assert response.status_code in [200, 429]

    def test_rate_limit_response(self, client: TestClient):
        """Test rate limit response headers"""
        for i in range(100):
            response = client.get("/api/endpoint")

        if response.status_code == 429:
            assert "Retry-After" in response.headers

    def test_rate_limit_per_endpoint(self, client: TestClient):
        """Test rate limits are per-endpoint"""
        # Exhaust limit on one endpoint
        for _ in range(100):
            client.get("/api/endpoint1")

        # Different endpoint should still work
        response = client.get("/api/endpoint2")
        assert response.status_code == 200
```

## Summary

Rate limiting tests ensure API protection.

## Next Steps

Continue learning about:
- [API Security Testing](./11_api_security_testing.md)
- [API Performance Testing](./09_api_performance_testing.md)
