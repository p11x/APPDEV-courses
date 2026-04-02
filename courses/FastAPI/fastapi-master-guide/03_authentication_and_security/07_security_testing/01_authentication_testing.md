# Authentication Testing

## Overview

Testing authentication ensures security mechanisms work correctly and vulnerabilities are caught early. This guide covers comprehensive auth testing strategies.

## Unit Testing Authentication

### Testing Password Hashing

```python
# Example 1: Test password hashing
import pytest
from passlib.context import CryptContext

pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")

def hash_password(password: str) -> str:
    return pwd_context.hash(password)

def verify_password(plain: str, hashed: str) -> bool:
    return pwd_context.verify(plain, hashed)

class TestPasswordHashing:
    """Test password hashing functionality"""

    def test_hash_password(self):
        """Test password is hashed correctly"""
        password = "SecurePassword123!"
        hashed = hash_password(password)

        # Hash should be different from plain password
        assert hashed != password

        # Hash should start with bcrypt identifier
        assert hashed.startswith("$2b$")

    def test_verify_correct_password(self):
        """Test correct password verification"""
        password = "SecurePassword123!"
        hashed = hash_password(password)

        assert verify_password(password, hashed) is True

    def test_verify_wrong_password(self):
        """Test wrong password fails verification"""
        password = "SecurePassword123!"
        hashed = hash_password(password)

        assert verify_password("WrongPassword!", hashed) is False

    def test_different_hashes_for_same_password(self):
        """Test same password produces different hashes (salt)"""
        password = "SecurePassword123!"
        hash1 = hash_password(password)
        hash2 = hash_password(password)

        assert hash1 != hash2
        assert verify_password(password, hash1)
        assert verify_password(password, hash2)
```

### Testing JWT Tokens

```python
# Example 2: Test JWT functionality
import pytest
from jose import jwt, JWTError
from datetime import datetime, timedelta

SECRET_KEY = "test-secret-key"
ALGORITHM = "HS256"

def create_token(user_id: int, expires_delta: timedelta = None) -> str:
    expire = datetime.utcnow() + (expires_delta or timedelta(minutes=30))
    return jwt.encode(
        {"sub": str(user_id), "exp": expire},
        SECRET_KEY,
        algorithm=ALGORITHM
    )

def verify_token(token: str) -> dict:
    try:
        return jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
    except JWTError:
        return None

class TestJWTTokens:
    """Test JWT token functionality"""

    def test_create_token(self):
        """Test token creation"""
        token = create_token(user_id=1)

        assert token is not None
        assert isinstance(token, str)

    def test_verify_valid_token(self):
        """Test valid token verification"""
        token = create_token(user_id=1)
        payload = verify_token(token)

        assert payload is not None
        assert payload["sub"] == "1"

    def test_verify_expired_token(self):
        """Test expired token fails verification"""
        token = create_token(user_id=1, expires_delta=timedelta(seconds=-1))
        payload = verify_token(token)

        assert payload is None

    def test_verify_invalid_token(self):
        """Test invalid token fails verification"""
        payload = verify_token("invalid.token.here")

        assert payload is None

    def test_verify_tampered_token(self):
        """Test tampered token fails verification"""
        token = create_token(user_id=1)
        tampered = token[:-5] + "xxxxx"

        payload = verify_token(tampered)

        assert payload is None
```

## Integration Testing

### Testing Auth Endpoints

```python
# Example 3: Test auth endpoints with TestClient
import pytest
from fastapi import FastAPI, Depends, HTTPException
from fastapi.testclient import TestClient
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials

app = FastAPI()
security = HTTPBearer()

# Simulated auth
VALID_CREDENTIALS = {"username": "testuser", "password": "TestPass123!"}

def verify_token(credentials: HTTPAuthorizationCredentials = Depends(security)):
    if credentials.credentials != "valid-token":
        raise HTTPException(401, "Invalid token")
    return {"user_id": 1}

@app.post("/login")
async def login(username: str, password: str):
    if username == VALID_CREDENTIALS["username"] and \
       password == VALID_CREDENTIALS["password"]:
        return {"access_token": "valid-token", "token_type": "bearer"}
    raise HTTPException(401, "Invalid credentials")

@app.get("/protected")
async def protected(user: dict = Depends(verify_token)):
    return {"user": user}

client = TestClient(app)

class TestAuthEndpoints:
    """Test authentication endpoints"""

    def test_login_success(self):
        """Test successful login"""
        response = client.post(
            "/login",
            params={
                "username": "testuser",
                "password": "TestPass123!"
            }
        )

        assert response.status_code == 200
        data = response.json()
        assert "access_token" in data
        assert data["token_type"] == "bearer"

    def test_login_wrong_password(self):
        """Test login with wrong password"""
        response = client.post(
            "/login",
            params={
                "username": "testuser",
                "password": "wrongpassword"
            }
        )

        assert response.status_code == 401

    def test_login_wrong_username(self):
        """Test login with wrong username"""
        response = client.post(
            "/login",
            params={
                "username": "wronguser",
                "password": "TestPass123!"
            }
        )

        assert response.status_code == 401

    def test_protected_with_valid_token(self):
        """Test protected endpoint with valid token"""
        response = client.get(
            "/protected",
            headers={"Authorization": "Bearer valid-token"}
        )

        assert response.status_code == 200
        assert response.json()["user"]["user_id"] == 1

    def test_protected_without_token(self):
        """Test protected endpoint without token"""
        response = client.get("/protected")

        assert response.status_code == 401

    def test_protected_with_invalid_token(self):
        """Test protected endpoint with invalid token"""
        response = client.get(
            "/protected",
            headers={"Authorization": "Bearer invalid-token"}
        )

        assert response.status_code == 401
```

## Security Testing

### Testing Vulnerabilities

```python
# Example 4: Security vulnerability tests
import pytest
from fastapi import FastAPI
from fastapi.testclient import TestClient

app = FastAPI()
client = TestClient(app)

class TestSecurityVulnerabilities:
    """Test for common security vulnerabilities"""

    def test_sql_injection_attempt(self):
        """Test SQL injection is prevented"""
        response = client.post(
            "/login",
            params={
                "username": "admin' OR '1'='1",
                "password": "anything"
            }
        )

        # Should fail, not succeed with SQL injection
        assert response.status_code == 401

    def test_brute_force_protection(self):
        """Test brute force protection"""
        for i in range(10):
            response = client.post(
                "/login",
                params={
                    "username": "testuser",
                    "password": f"wrong{i}"
                }
            )

        # After multiple failures, should be rate limited
        assert response.status_code in [401, 429]

    def test_token_theft_protection(self):
        """Test token cannot be reused after logout"""
        # Login
        login_response = client.post(
            "/login",
            params={"username": "testuser", "password": "TestPass123!"}
        )
        token = login_response.json()["access_token"]

        # Logout
        client.post("/logout", headers={"Authorization": f"Bearer {token}"})

        # Try to use token after logout
        response = client.get(
            "/protected",
            headers={"Authorization": f"Bearer {token}"}
        )

        assert response.status_code == 401

    def test_cors_headers(self):
        """Test CORS headers are properly set"""
        response = client.options(
            "/api/items",
            headers={
                "Origin": "https://malicious-site.com",
                "Access-Control-Request-Method": "GET"
            }
        )

        # Should not allow arbitrary origins
        origin = response.headers.get("Access-Control-Allow-Origin")
        assert origin != "https://malicious-site.com"
```

## Test Fixtures

### Reusable Test Setup

```python
# Example 5: Test fixtures for auth
import pytest
from fastapi.testclient import TestClient
from jose import jwt
from datetime import datetime, timedelta

SECRET_KEY = "test-secret"
ALGORITHM = "HS256"

@pytest.fixture
def valid_token() -> str:
    """Create valid JWT token for testing"""
    payload = {
        "sub": "1",
        "exp": datetime.utcnow() + timedelta(hours=1)
    }
    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)

@pytest.fixture
def expired_token() -> str:
    """Create expired JWT token for testing"""
    payload = {
        "sub": "1",
        "exp": datetime.utcnow() - timedelta(hours=1)
    }
    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)

@pytest.fixture
def auth_headers(valid_token: str) -> dict:
    """Create authorization headers"""
    return {"Authorization": f"Bearer {valid_token}"}

@pytest.fixture
def test_user() -> dict:
    """Test user data"""
    return {
        "id": 1,
        "username": "testuser",
        "email": "test@example.com",
        "is_active": True
    }

class TestWithFixtures:
    """Tests using fixtures"""

    def test_protected_endpoint(self, client: TestClient, auth_headers: dict):
        """Test using auth headers fixture"""
        response = client.get("/protected", headers=auth_headers)
        assert response.status_code == 200

    def test_expired_token(self, client: TestClient, expired_token: str):
        """Test expired token fixture"""
        response = client.get(
            "/protected",
            headers={"Authorization": f"Bearer {expired_token}"}
        )
        assert response.status_code == 401
```

## Best Practices

### Testing Guidelines

```python
# Example 6: Testing best practices
"""
Authentication Testing Best Practices:

1. Test both success and failure cases
2. Test edge cases (expired tokens, malformed input)
3. Test rate limiting and brute force protection
4. Test token refresh flow
5. Test logout and token invalidation
6. Test permission checks
7. Use fixtures for common test data
8. Mock external services
9. Test with different user roles
10. Test CORS and security headers
"""

import pytest

class TestAuthBestPractices:
    """Demonstrates testing best practices"""

    # 1. Always test happy path and error cases
    def test_login_success(self):
        """Happy path: successful login"""
        pass

    def test_login_invalid_credentials(self):
        """Error case: wrong credentials"""
        pass

    # 2. Test boundary conditions
    def test_token_exactly_at_expiry(self):
        """Edge case: token at exact expiry time"""
        pass

    # 3. Test security headers
    def test_security_headers_present(self):
        """Verify security headers"""
        pass

    # 4. Test with different roles
    def test_admin_access(self):
        """Test admin role permissions"""
        pass

    def test_viewer_access(self):
        """Test viewer role permissions"""
        pass

    def test_unauthorized_access(self):
        """Test no role permissions"""
        pass
```

## Summary

| Test Type | Coverage | Example |
|-----------|----------|---------|
| Unit | Individual functions | Password hashing, JWT |
| Integration | Endpoints | Login, protected routes |
| Security | Vulnerabilities | SQL injection, XSS |
| Fixtures | Reusable setup | Tokens, test users |

## Next Steps

Continue learning about:
- [Security Testing Tools](./02_security_testing_tools.md) - Testing utilities
- [Penetration Testing](./03_penetration_testing_checklist.md) - Pen testing
- [Security Audit](./06_security_audit_practices.md) - Audit practices
