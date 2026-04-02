# Testing Concepts and Types

## Overview

Understanding testing fundamentals is essential for building reliable FastAPI applications. This guide covers the different types of testing and when to use each.

## Testing Types

### The Testing Spectrum

```python
# Example 1: Understanding test types
"""
Testing Pyramid:

        /\
       /E2E\       <- Few, Slow, Expensive
      /------\
     /Integr.\     <- Some, Medium speed
    /----------\
   /   Unit     \  <- Many, Fast, Cheap
  /--------------\

Unit Tests:
- Test individual functions/methods
- No external dependencies
- Very fast (< 1ms per test)
- High coverage needed

Integration Tests:
- Test component interactions
- May use databases, APIs
- Medium speed (10-100ms)
- Critical path coverage

End-to-End Tests:
- Test complete user flows
- Real external services
- Slow (100ms-seconds)
- Happy path coverage
"""

# Unit Test Example
def calculate_discount(price: float, discount_percent: float) -> float:
    """Pure function - easy to unit test"""
    if discount_percent < 0 or discount_percent > 100:
        raise ValueError("Discount must be between 0 and 100")
    return price * (1 - discount_percent / 100)

def test_calculate_discount():
    """Unit test - no dependencies needed"""
    assert calculate_discount(100, 10) == 90.0
    assert calculate_discount(100, 0) == 100.0
    assert calculate_discount(100, 100) == 0.0

    with pytest.raises(ValueError):
        calculate_discount(100, -10)
    with pytest.raises(ValueError):
        calculate_discount(100, 110)

# Integration Test Example
from fastapi.testclient import TestClient
from app.main import app

def test_create_user_endpoint(client: TestClient, db_session):
    """Integration test - tests API + database"""
    response = client.post("/users/", json={
        "username": "testuser",
        "email": "test@example.com",
        "password": "securepassword123"
    })

    assert response.status_code == 201
    data = response.json()
    assert data["username"] == "testuser"

    # Verify in database
    user = db_session.query(User).filter_by(username="testuser").first()
    assert user is not None

# E2E Test Example
import httpx

async def test_user_registration_flow():
    """E2E test - complete user journey"""
    async with httpx.AsyncClient(base_url="http://localhost:8000") as client:
        # 1. Register
        response = await client.post("/auth/register", json={
            "username": "newuser",
            "email": "new@example.com",
            "password": "SecurePass123!"
        })
        assert response.status_code == 201

        # 2. Login
        response = await client.post("/auth/login", json={
            "username": "newuser",
            "password": "SecurePass123!"
        })
        assert response.status_code == 200
        token = response.json()["access_token"]

        # 3. Access protected resource
        response = await client.get(
            "/users/me",
            headers={"Authorization": f"Bearer {token}"}
        )
        assert response.status_code == 200
        assert response.json()["username"] == "newuser"
```

## Test Categories

### Functional vs Non-Functional

```python
# Example 2: Functional vs Non-functional testing

# Functional Tests - "Does it work?"
class TestUserCRUD:
    """Functional tests for user operations"""

    def test_create_user(self, client):
        """Does user creation work?"""
        response = client.post("/users/", json={
            "username": "john",
            "email": "john@example.com",
            "password": "password123"
        })
        assert response.status_code == 201

    def test_get_user(self, client, test_user):
        """Does user retrieval work?"""
        response = client.get(f"/users/{test_user.id}")
        assert response.status_code == 200
        assert response.json()["username"] == test_user.username

    def test_update_user(self, client, test_user):
        """Does user update work?"""
        response = client.patch(
            f"/users/{test_user.id}",
            json={"username": "newname"}
        )
        assert response.status_code == 200

    def test_delete_user(self, client, test_user):
        """Does user deletion work?"""
        response = client.delete(f"/users/{test_user.id}")
        assert response.status_code == 204

# Non-Functional Tests - "How well does it work?"
class TestPerformance:
    """Non-functional performance tests"""

    def test_response_time(self, client):
        """Is response time acceptable?"""
        import time

        start = time.time()
        response = client.get("/users/")
        duration = time.time() - start

        assert response.status_code == 200
        assert duration < 0.5  # Response under 500ms

    def test_concurrent_requests(self, client):
        """Can it handle concurrent load?"""
        import concurrent.futures

        def make_request():
            return client.get("/users/")

        with concurrent.futures.ThreadPoolExecutor(max_workers=10) as executor:
            futures = [executor.submit(make_request) for _ in range(100)]
            results = [f.result() for f in futures]

        success_count = sum(1 for r in results if r.status_code == 200)
        assert success_count >= 95  # 95% success rate
```

## Test Scope

### Narrow vs Broad Tests

```python
# Example 3: Test scope levels

# Narrow scope - Single function
def validate_email(email: str) -> bool:
    """Simple validation function"""
    import re
    pattern = r'^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$'
    return bool(re.match(pattern, email))

def test_validate_email():
    """Narrow test - single function"""
    assert validate_email("user@example.com") is True
    assert validate_email("invalid") is False
    assert validate_email("user@") is False

# Medium scope - Class/Service
class UserService:
    """User service with dependencies"""

    def __init__(self, db_session, email_service):
        self.db = db_session
        self.email = email_service

    async def create_user(self, data: dict) -> User:
        # Validate
        if not validate_email(data["email"]):
            raise ValueError("Invalid email")

        # Check duplicate
        existing = self.db.query(User).filter_by(email=data["email"]).first()
        if existing:
            raise ValueError("Email already exists")

        # Create user
        user = User(**data)
        self.db.add(user)
        self.db.commit()

        # Send welcome email
        await self.email.send_welcome(user.email)

        return user

@pytest.mark.asyncio
async def test_create_user_service():
    """Medium test - service with mocked dependencies"""
    mock_db = Mock()
    mock_email = AsyncMock()

    service = UserService(mock_db, mock_email)

    user = await service.create_user({
        "username": "test",
        "email": "test@example.com"
    })

    mock_db.add.assert_called_once()
    mock_db.commit.assert_called_once()
    mock_email.send_welcome.assert_called_once_with("test@example.com")

# Broad scope - Full API flow
def test_user_registration_flow(client, db_session):
    """Broad test - complete flow through API"""
    # Register
    response = client.post("/auth/register", json={
        "username": "newuser",
        "email": "new@example.com",
        "password": "SecurePass123!"
    })
    assert response.status_code == 201

    # Verify in database
    user = db_session.query(User).filter_by(username="newuser").first()
    assert user is not None

    # Login
    response = client.post("/auth/login", data={
        "username": "newuser",
        "password": "SecurePass123!"
    })
    assert response.status_code == 200
    assert "access_token" in response.json()
```

## Best Practices

### Testing Guidelines

```python
# Example 4: Testing best practices checklist
"""
Testing Best Practices:

1. Follow the Test Pyramid
   - Many unit tests (70-80%)
   - Some integration tests (15-25%)
   - Few E2E tests (5-10%)

2. Test Behavior, Not Implementation
   - Focus on WHAT, not HOW
   - Tests should survive refactoring

3. One Assertion Per Concept
   - Each test should verify one thing
   - Multiple assertions OK if testing same concept

4. Use Descriptive Test Names
   - test_<what>_<when>_<expected>
   - test_create_user_with_invalid_email_returns_422

5. Arrange-Act-Assert Pattern
   - Setup test data
   - Execute action
   - Verify result

6. Independent Tests
   - No test depends on another
   - Tests run in any order

7. Fast Tests
   - Unit tests < 10ms
   - Suite runs in seconds, not minutes
"""

# Good test structure
class TestUserCreation:
    """Well-structured test class"""

    def test_create_user_with_valid_data_returns_201(
        self, client, db_session
    ):
        """Clear name describing scenario and expected outcome"""
        # Arrange
        user_data = {
            "username": "newuser",
            "email": "new@example.com",
            "password": "SecurePass123!"
        }

        # Act
        response = client.post("/users/", json=user_data)

        # Assert
        assert response.status_code == 201
        assert response.json()["username"] == "newuser"

    def test_create_user_with_duplicate_email_returns_400(
        self, client, existing_user
    ):
        """Tests error case"""
        # Arrange
        user_data = {
            "username": "another",
            "email": existing_user.email,  # Duplicate!
            "password": "SecurePass123!"
        }

        # Act
        response = client.post("/users/", json=user_data)

        # Assert
        assert response.status_code == 400
        assert "already exists" in response.json()["detail"].lower()
```

## Summary

| Test Type | Speed | Scope | Coverage |
|-----------|-------|-------|----------|
| Unit | Very Fast | Single function | High |
| Integration | Medium | Components | Medium |
| E2E | Slow | Full flow | Low |

## Next Steps

Continue learning about:
- [Test Pyramid](./02_test_pytest.md) - Balancing test types
- [Testing Philosophy](./03_testing_philosophy.md) - Testing mindset
- [Unit Testing](../02_unit_testing/01_setup_and_configuration.md) - Write unit tests
