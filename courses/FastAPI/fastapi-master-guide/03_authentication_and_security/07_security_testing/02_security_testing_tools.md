# Security Testing Tools

## Overview

Security testing tools help identify vulnerabilities in FastAPI applications before deployment.

## Tools Overview

### Common Security Testing Tools

```python
# Example 1: Bandit - Python security linter
# Install: pip install bandit
# Run: bandit -r app/

# Configuration (.bandit)
"""
skips: ['B101']  # Skip assert warnings
exclude_dirs: ['tests']
"""

# Example 2: Safety - Dependency vulnerability checker
# Install: pip install safety
# Run: safety check

# Example 3: OWASP ZAP - Dynamic security testing
# Can be automated with Python
import subprocess

def run_zap_scan(target_url: str):
    """Run OWASP ZAP scan"""
    subprocess.run([
        "zap-cli", "quick-scan",
        "--self-contained",
        "--start-options", "-config api.disablekey=true",
        target_url
    ])
```

### FastAPI-Specific Security Testing

```python
# Example 4: FastAPI security test suite
import pytest
from fastapi.testclient import TestClient

class TestSecurityHeaders:
    """Test security headers"""

    def test_x_content_type_options(self, client: TestClient):
        response = client.get("/")
        assert response.headers.get("x-content-type-options") == "nosniff"

    def test_x_frame_options(self, client: TestClient):
        response = client.get("/")
        assert response.headers.get("x-frame-options") in ["DENY", "SAMEORIGIN"]

    def test_content_security_policy(self, client: TestClient):
        response = client.get("/")
        assert "content-security-policy" in response.headers

class TestSQLInjection:
    """Test SQL injection vulnerabilities"""

    @pytest.mark.parametrize("payload", [
        "' OR '1'='1",
        "'; DROP TABLE users; --",
        "1' UNION SELECT * FROM users --"
    ])
    def test_sql_injection_prevention(self, client: TestClient, payload: str):
        response = client.get(f"/users/search?q={payload}")
        assert response.status_code in [200, 400, 422]
        assert "error" not in response.text.lower() or response.status_code != 500

class TestXSS:
    """Test XSS vulnerabilities"""

    @pytest.mark.parametrize("payload", [
        "<script>alert('xss')</script>",
        "<img src=x onerror=alert('xss')>"
    ])
    def test_xss_prevention(self, client: TestClient, payload: str):
        response = client.post("/items/", json={"name": payload})
        if response.status_code == 201:
            assert "<script>" not in response.json()["name"]
```

## Best Practices

1. Run security tests in CI/CD
2. Use multiple scanning tools
3. Regular dependency audits
4. Monitor for new vulnerabilities

## Summary

Combining static and dynamic security testing provides comprehensive vulnerability detection.

## Next Steps

Continue learning about:
- [Penetration Testing](./03_penetration_testing_checklist.md)
- [Security Audit](./06_security_audit_practices.md)
