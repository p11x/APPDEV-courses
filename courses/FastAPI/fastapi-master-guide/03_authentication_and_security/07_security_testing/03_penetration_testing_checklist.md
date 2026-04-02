# Penetration Testing Checklist

## Overview

Penetration testing identifies security vulnerabilities by simulating attacks on FastAPI applications.

## Testing Checklist

### Authentication Tests

```python
# Example 1: Authentication testing
"""
Authentication Test Checklist:

□ Brute force protection
  - Test multiple failed login attempts
  - Verify account lockout
  - Check rate limiting

□ Password security
  - Test weak passwords
  - Verify password hashing
  - Check password reset flow

□ Token security
  - Test token expiration
  - Verify token invalidation on logout
  - Check for token leakage

□ Session management
  - Test session timeout
  - Verify secure cookie flags
  - Check session fixation
"""

# Brute force test
def test_brute_force_protection(client):
    for i in range(10):
        response = client.post("/login", json={
            "username": "admin",
            "password": f"wrong_{i}"
        })

    # Should be rate limited
    assert response.status_code == 429
```

### Input Validation Tests

```python
# Example 2: Input validation testing
"""
Input Validation Checklist:

□ SQL Injection
  - Test with: ' OR '1'='1
  - Test with: ; DROP TABLE users; --
  - Test with: UNION SELECT

□ XSS
  - Test with: <script>alert('xss')</script>
  - Test with: <img src=x onerror=alert('xss')>
  - Test in all input fields

□ Command Injection
  - Test with: ; ls -la
  - Test with: | cat /etc/passwd

□ Path Traversal
  - Test with: ../../../etc/passwd
  - Test with: ..\\..\\windows\\system32
"""
```

### API Security Tests

```python
# Example 3: API security testing
"""
API Security Checklist:

□ Authorization
  - Test horizontal privilege escalation
  - Test vertical privilege escalation
  - Test IDOR vulnerabilities

□ Rate Limiting
  - Test rate limit enforcement
  - Test rate limit bypass
  - Test different endpoints

□ Error Handling
  - Test for information leakage
  - Test error message content
  - Test stack trace exposure
"""
```

## Summary

Regular penetration testing helps identify and fix security vulnerabilities.

## Next Steps

Continue learning about:
- [Security Audit Practices](./06_security_audit_practices.md)
- [Security Testing Tools](./02_security_testing_tools.md)
