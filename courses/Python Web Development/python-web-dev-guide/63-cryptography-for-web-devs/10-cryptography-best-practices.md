# Cryptography Best Practices

## What You'll Learn

- General security principles
- Key management
- Security checklist

## Prerequisites

- Completed `09-common-vulnerabilities.md`

## General Principles

1. **Defense in Depth** - Multiple layers of security
2. **Least Privilege** - Minimal permissions
3. **Fail Securely** - Safe defaults
4. **Don't Roll Your Own** - Use established libraries

## Key Management

```python
# GOOD - Use environment variables for keys
import os

API_KEY = os.environ.get("API_KEY")
SECRET_KEY = os.environ.get("SECRET_KEY")

# NEVER hardcode keys
# BAD:
SECRET_KEY = "my_secret_key"  # Don't do this!

# GOOD - Use key management services
# AWS KMS, Azure Key Vault, HashiCorp Vault
```

## Secure Configuration

```python
# Settings for production
SECURITY_SETTINGS = {
    # Password hashing
    "PASSWORD_SCHEMES": ["bcrypt"],
    "BCRYPT_ROUNDS": 12,
    
    # Session
    "SESSION_COOKIE_SECURE": True,
    "SESSION_COOKIE_HTTPONLY": True,
    "SESSION_COOKIE_SAMESITE": "Lax",
    
    # CORS
    "CORS_ORIGINS": ["https://example.com"],
    "CORS_ALLOW_CREDENTIALS": True,
}
```

## Security Checklist

- [ ] Use HTTPS everywhere
- [ ] Hash passwords with bcrypt
- [ ] Use secure session cookies
- [ ] Implement rate limiting
- [ ] Add security headers
- [ ] Validate all input
- [ ] Use parameterized queries
- [ ] Keep dependencies updated
- [ ] Log security events
- [ ] Regular security audits

## Using Security Libraries

```python
# pip install safety bandit

# Check for vulnerabilities
# pip install safety
# safety check

# Scan for security issues
# pip install bandit
# bandit -r your_app/
```

## Environment Variables

```python
import os
from dataclasses import dataclass

@dataclass
class Config:
    database_url: str
    secret_key: str
    jwt_secret: str
    
    @classmethod
    def from_env(cls) -> "Config":
        return cls(
            database_url=os.environ["DATABASE_URL"],
            secret_key=os.environ["SECRET_KEY"],
            jwt_secret=os.environ["JWT_SECRET"]
        )
```

## Summary

- Follow security best practices
- Use established libraries
- Keep systems updated
- Regular security audits

## Next Steps

This concludes the Cryptography folder. Continue to other topics in your learning journey.
