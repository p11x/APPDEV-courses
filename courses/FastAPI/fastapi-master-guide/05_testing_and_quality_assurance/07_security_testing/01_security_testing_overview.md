# Security Testing Overview

## Overview

Security testing identifies vulnerabilities in your FastAPI application before attackers can exploit them. This guide covers common security tests.

## Authentication Testing

### Testing Auth Vulnerabilities

```python
# Example 1: Authentication security tests
import pytest
from fastapi.testclient import TestClient

class TestAuthenticationSecurity:
    """Test authentication security"""

    def test_brute_force_protection(self, client):
        """Test protection against brute force attacks"""
        # Attempt multiple failed logins
        for i in range(10):
            response = client.post("/auth/login", json={
                "username": "admin",
                "password": f"wrong_{i}"
            })

        # Should be rate limited
        response = client.post("/auth/login", json={
            "username": "admin",
            "password": "wrong_final"
        })

        assert response.status_code == 429  # Too Many Requests

    def test_token_expiration(self, client):
        """Test that expired tokens are rejected"""
        from jose import jwt
        from datetime import datetime, timedelta

        # Create expired token
        expired_token = jwt.encode(
            {
                "sub": "1",
                "exp": datetime.utcnow() - timedelta(hours=1)
            },
            SECRET_KEY,
            algorithm="HS256"
        )

        response = client.get(
            "/users/me",
            headers={"Authorization": f"Bearer {expired_token}"}
        )

        assert response.status_code == 401

    def test_invalid_token_rejected(self, client):
        """Test that invalid tokens are rejected"""
        response = client.get(
            "/users/me",
            headers={"Authorization": "Bearer invalid_token_here"}
        )

        assert response.status_code == 401

    def test_missing_token_rejected(self, client):
        """Test that missing tokens are rejected"""
        response = client.get("/users/me")

        assert response.status_code == 401

    def test_password_hashing(self, db_session):
        """Test that passwords are properly hashed"""
        user = User(
            username="hashtest",
            email="hash@example.com",
            hashed_password=hash_password("plaintext123")
        )

        # Password should not be stored in plaintext
        assert user.hashed_password != "plaintext123"
        assert len(user.hashed_password) > 50  # Hash is long

    def test_timing_attack_protection(self, client):
        """Test protection against timing attacks"""
        import time

        # Time for valid username, wrong password
        start = time.time()
        client.post("/auth/login", json={
            "username": "existing_user",
            "password": "wrong"
        })
        time1 = time.time() - start

        # Time for non-existent username
        start = time.time()
        client.post("/auth/login", json={
            "username": "nonexistent_user",
            "password": "wrong"
        })
        time2 = time.time() - start

        # Times should be similar (within 50ms)
        assert abs(time1 - time2) < 0.05
```

## Input Validation Security

### Testing Injection Attacks

```python
# Example 2: Input validation security tests
class TestInputSecurity:
    """Test input validation security"""

    @pytest.mark.parametrize("sql_injection", [
        "'; DROP TABLE users; --",
        "1' OR '1'='1",
        "admin'--",
        "1; SELECT * FROM users",
        "' UNION SELECT * FROM users --"
    ])
    def test_sql_injection_prevention(self, client, sql_injection):
        """Test SQL injection prevention"""
        response = client.get(f"/users/search?q={sql_injection}")

        # Should not cause server error
        assert response.status_code in [200, 400, 422]
        # Should not return unexpected data
        if response.status_code == 200:
            assert len(response.json()) == 0 or "error" not in str(response.json())

    @pytest.mark.parametrize("xss_payload", [
        "<script>alert('xss')</script>",
        "<img src=x onerror=alert('xss')>",
        "javascript:alert('xss')",
        "<svg onload=alert('xss')>"
    ])
    def test_xss_prevention(self, client, xss_payload):
        """Test XSS prevention"""
        response = client.post("/users/", json={
            "username": xss_payload,
            "email": "xss@example.com",
            "password": "SecurePass123!"
        })

        # Should either reject or sanitize
        if response.status_code == 201:
            # If accepted, should be escaped in response
            assert "<script>" not in response.json()["username"]

    @pytest.mark.parametrize("path_traversal", [
        "../../../etc/passwd",
        "..\\..\\windows\\system32",
        "%2e%2e%2f%2e%2e%2fetc%2fpasswd"
    ])
    def test_path_traversal_prevention(self, client, path_traversal):
        """Test path traversal prevention"""
        response = client.get(f"/files/{path_traversal}")

        # Should not access system files
        assert response.status_code in [400, 403, 404]

    def test_command_injection_prevention(self, client):
        """Test command injection prevention"""
        response = client.post("/utils/ping", json={
            "host": "localhost; rm -rf /"
        })

        # Should sanitize or reject
        assert response.status_code in [400, 422]

    def test_oversized_input_rejected(self, client):
        """Test rejection of oversized inputs"""
        large_string = "a" * 10000

        response = client.post("/users/", json={
            "username": large_string,
            "email": "large@example.com",
            "password": "SecurePass123!"
        })

        assert response.status_code == 422
```

## Authorization Testing

### Access Control Tests

```python
# Example 3: Authorization security tests
class TestAuthorizationSecurity:
    """Test authorization security"""

    def test_horizontal_privilege_escalation(self, client, db_session):
        """Test users can't access other users' data"""
        # Create two users
        user1 = create_user(db_session, "user1")
        user2 = create_user(db_session, "user2")

        token1 = get_token_for_user(user1)

        # User1 tries to access user2's private data
        response = client.get(
            f"/users/{user2.id}/private",
            headers={"Authorization": f"Bearer {token1}"}
        )

        assert response.status_code == 403

    def test_vertical_privilege_escalation(self, client, db_session):
        """Test regular users can't access admin endpoints"""
        regular_user = create_user(db_session, "regular", is_admin=False)
        token = get_token_for_user(regular_user)

        response = client.get(
            "/admin/users",
            headers={"Authorization": f"Bearer {token}"}
        )

        assert response.status_code == 403

    def test_idor_prevention(self, client, db_session):
        """Test Insecure Direct Object Reference prevention"""
        user1 = create_user(db_session, "idor_user1")
        user2 = create_user(db_session, "idor_user2")

        token1 = get_token_for_user(user1)

        # User1 tries to modify user2's resource
        response = client.patch(
            f"/users/{user2.id}",
            json={"username": "hacked"},
            headers={"Authorization": f"Bearer {token1}"}
        )

        assert response.status_code == 403

    def test_role_based_access(self, client, db_session):
        """Test role-based access control"""
        editor = create_user(db_session, "editor", role="editor")
        viewer = create_user(db_session, "viewer", role="viewer")

        editor_token = get_token_for_user(editor)
        viewer_token = get_token_for_user(viewer)

        # Editor can create
        response = client.post(
            "/posts/",
            json={"title": "New Post", "content": "Content"},
            headers={"Authorization": f"Bearer {editor_token}"}
        )
        assert response.status_code == 201

        # Viewer cannot create
        response = client.post(
            "/posts/",
            json={"title": "New Post", "content": "Content"},
            headers={"Authorization": f"Bearer {viewer_token}"}
        )
        assert response.status_code == 403
```

## Security Headers Testing

### HTTP Security Headers

```python
# Example 4: Security headers tests
class TestSecurityHeaders:
    """Test security headers"""

    def test_cors_headers(self, client):
        """Test CORS configuration"""
        response = client.options(
            "/users/",
            headers={
                "Origin": "https://malicious-site.com",
                "Access-Control-Request-Method": "POST"
            }
        )

        # Should not allow arbitrary origins
        origin = response.headers.get("access-control-allow-origin")
        assert origin != "https://malicious-site.com"

    def test_content_type_options(self, client):
        """Test X-Content-Type-Options header"""
        response = client.get("/users/")

        assert response.headers.get("x-content-type-options") == "nosniff"

    def test_frame_options(self, client):
        """Test X-Frame-Options header"""
        response = client.get("/users/")

        frame_options = response.headers.get("x-frame-options")
        assert frame_options in ["DENY", "SAMEORIGIN"]

    def test_xss_protection(self, client):
        """Test X-XSS-Protection header"""
        response = client.get("/users/")

        xss_protection = response.headers.get("x-xss-protection")
        assert xss_protection is not None

    def test_strict_transport_security(self, client):
        """Test HSTS header"""
        response = client.get("/users/")

        hsts = response.headers.get("strict-transport-security")
        # Should enforce HTTPS
        assert hsts is None or "max-age" in hsts

    def test_content_security_policy(self, client):
        """Test CSP header"""
        response = client.get("/users/")

        csp = response.headers.get("content-security-policy")
        # Should have restrictive CSP
        if csp:
            assert "default-src" in csp
```

## Rate Limiting Testing

### Rate Limit Tests

```python
# Example 5: Rate limiting tests
class TestRateLimiting:
    """Test rate limiting"""

    def test_rate_limit_enforced(self, client):
        """Test that rate limits are enforced"""
        # Make requests until rate limited
        for i in range(150):  # Assuming 100/min limit
            response = client.get("/api/endpoint")

            if response.status_code == 429:
                # Rate limited
                assert "retry-after" in response.headers
                return

        # Should have been rate limited
        pytest.fail("Rate limiting not enforced")

    def test_rate_limit_per_endpoint(self, client):
        """Test rate limits are per-endpoint"""
        # Exhaust limit on one endpoint
        for _ in range(100):
            client.get("/api/endpoint1")

        # Different endpoint should still work
        response = client.get("/api/endpoint2")
        assert response.status_code == 200

    def test_rate_limit_reset(self, client, mock_time):
        """Test rate limit resets after window"""
        # Exhaust limit
        for _ in range(100):
            client.get("/api/endpoint")

        # Wait for reset (mock time)
        mock_time.advance(seconds=60)

        # Should work again
        response = client.get("/api/endpoint")
        assert response.status_code == 200
```

## Data Protection Testing

### Sensitive Data Tests

```python
# Example 6: Data protection tests
class TestDataProtection:
    """Test sensitive data protection"""

    def test_password_not_exposed(self, client, test_user):
        """Test passwords are never in responses"""
        response = client.get(f"/users/{test_user.id}")

        data = response.json()
        assert "password" not in data
        assert "hashed_password" not in data
        assert "password_hash" not in data

    def test_sensitive_data_masked(self, client, test_user):
        """Test sensitive data is masked"""
        response = client.get(f"/users/{test_user.id}")

        data = response.json()
        if "email" in data:
            # Email might be partially masked
            pass

        if "phone" in data:
            # Phone should be masked
            assert "***" in data["phone"] or len(data["phone"]) < 10

    def test_no_sensitive_data_in_logs(self, client, caplog):
        """Test sensitive data not logged"""
        client.post("/auth/login", json={
            "username": "testuser",
            "password": "SuperSecretPassword123!"
        })

        # Password should not appear in logs
        assert "SuperSecretPassword123!" not in caplog.text

    def test_error_messages_safe(self, client):
        """Test error messages don't leak info"""
        response = client.get("/users/99999")

        error = response.json().get("detail", "")

        # Should not reveal internal details
        assert "database" not in error.lower()
        assert "sql" not in error.lower()
        assert "stack trace" not in error.lower()
```

## Best Practices

### Security Testing Guidelines

```python
# Example 7: Security testing best practices
"""
Security Testing Best Practices:

1. Test authentication thoroughly
   - Token validation
   - Session management
   - Password security

2. Test authorization
   - Role-based access
   - Resource ownership
   - Privilege escalation

3. Test input validation
   - SQL injection
   - XSS
   - Command injection
   - Path traversal

4. Test rate limiting
   - Brute force protection
   - DDoS mitigation

5. Test data protection
   - Sensitive data exposure
   - Logging safety
   - Error message safety

6. Test security headers
   - CORS
   - CSP
   - HSTS

7. Use security scanning tools
   - OWASP ZAP
   - Bandit
   - Safety
"""
```

## Summary

| Security Area | What to Test |
|---------------|--------------|
| Authentication | Token handling, brute force |
| Authorization | Access control, escalation |
| Input Validation | Injection attacks, XSS |
| Rate Limiting | DoS protection |
| Data Protection | Sensitive data exposure |
| Headers | Security headers |

## Next Steps

Continue learning about:
- [API Security Testing](../03_api_testing/11_api_security_testing.md)
- [OWASP Testing](https://owasp.org/www-project-web-security-testing-guide/)
