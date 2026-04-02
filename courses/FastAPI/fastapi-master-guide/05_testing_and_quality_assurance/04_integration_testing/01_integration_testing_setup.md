# Integration Testing Setup

## Overview

Integration testing verifies that components work together correctly, including database interactions, external services, and middleware.

## Test Environment Setup

### Database Integration

```python
# Example 1: Integration test database setup
import pytest
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.pool import StaticPool
from fastapi.testclient import TestClient

from app.main import app
from app.database.base import Base
from app.database.session import get_db

# Test database configuration
TEST_DATABASE_URL = "sqlite:///:memory:"

engine = create_engine(
    TEST_DATABASE_URL,
    connect_args={"check_same_thread": False},
    poolclass=StaticPool
)

TestingSessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

@pytest.fixture(scope="session")
def setup_test_db():
    """Create test database tables"""
    Base.metadata.create_all(bind=engine)
    yield
    Base.metadata.drop_all(bind=engine)

@pytest.fixture
def db_session(setup_test_db):
    """
    Provide a database session that rolls back after each test.
    This ensures test isolation.
    """
    connection = engine.connect()
    transaction = connection.begin()
    session = TestingSessionLocal(bind=connection)

    yield session

    session.close()
    transaction.rollback()
    connection.close()

@pytest.fixture
def client(db_session):
    """
    Provide a test client with database session override.
    """
    def override_get_db():
        try:
            yield db_session
        finally:
            pass

    app.dependency_overrides[get_db] = override_get_db

    with TestClient(app) as c:
        yield c

    app.dependency_overrides.clear()
```

### External Service Mocking

```python
# Example 2: Mocking external services for integration tests
import pytest
from unittest.mock import AsyncMock, patch
import httpx

@pytest.fixture
def mock_external_api():
    """Mock external API responses"""
    with patch("httpx.AsyncClient.get") as mock_get:
        mock_get.return_value.status_code = 200
        mock_get.return_value.json.return_value = {
            "external_data": "mocked"
        }
        yield mock_get

@pytest.fixture
def mock_email_service():
    """Mock email sending"""
    with patch("app.services.email.send_email") as mock_send:
        mock_send.return_value = True
        yield mock_send

@pytest.fixture
def mock_payment_gateway():
    """Mock payment processing"""
    with patch("app.services.payment.process_payment") as mock_payment:
        mock_payment.return_value = {
            "status": "success",
            "transaction_id": "test_123"
        }
        yield mock_payment

# Usage
def test_order_with_payment(client, mock_payment_gateway):
    """Test order creation with mocked payment"""
    response = client.post("/orders/", json={
        "items": [{"product_id": 1, "quantity": 2}],
        "payment_method": "credit_card"
    })

    assert response.status_code == 201
    assert response.json()["status"] == "paid"
    mock_payment_gateway.assert_called_once()
```

## Integration Test Patterns

### Service Integration Tests

```python
# Example 3: Testing service integrations
class TestUserServiceIntegration:
    """Integration tests for user service"""

    def test_user_registration_flow(self, client, db_session, mock_email_service):
        """Test complete user registration"""
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
        assert user.email == "new@example.com"

        # Verify welcome email sent
        mock_email_service.assert_called_once_with(
            to="new@example.com",
            subject="Welcome!"
        )

    def test_user_login_flow(self, client, db_session):
        """Test login and token generation"""
        # Create user
        user = User(
            username="loginuser",
            email="login@example.com",
            hashed_password=hash_password("password123")
        )
        db_session.add(user)
        db_session.commit()

        # Login
        response = client.post("/auth/login", data={
            "username": "loginuser",
            "password": "password123"
        })

        assert response.status_code == 200
        data = response.json()
        assert "access_token" in data
        assert data["token_type"] == "bearer"

    def test_user_profile_update(self, client, db_session):
        """Test updating user profile"""
        # Create user
        user = User(
            username="profileuser",
            email="profile@example.com",
            hashed_password=hash_password("password123")
        )
        db_session.add(user)
        db_session.commit()

        # Login
        token = get_auth_token(client, "profileuser", "password123")

        # Update profile
        response = client.patch(
            "/users/me",
            json={"bio": "Updated bio"},
            headers={"Authorization": f"Bearer {token}"}
        )

        assert response.status_code == 200
        assert response.json()["bio"] == "Updated bio"
```

### Transaction Integration Tests

```python
# Example 4: Testing database transactions
class TestTransactionIntegration:
    """Integration tests for transactions"""

    def test_successful_transaction(self, client, db_session):
        """Test successful multi-step transaction"""
        response = client.post("/orders/", json={
            "items": [
                {"product_id": 1, "quantity": 2},
                {"product_id": 2, "quantity": 1}
            ],
            "shipping_address": "123 Test St"
        })

        assert response.status_code == 201

        # Verify order created
        order = db_session.query(Order).first()
        assert order is not None

        # Verify order items
        assert len(order.items) == 2

        # Verify inventory updated
        product1 = db_session.query(Product).get(1)
        assert product1.stock < product1.initial_stock

    def test_transaction_rollback_on_error(self, client, db_session):
        """Test transaction rollback on failure"""
        # Set product out of stock
        product = db_session.query(Product).get(1)
        product.stock = 0
        db_session.commit()

        # Attempt order
        response = client.post("/orders/", json={
            "items": [{"product_id": 1, "quantity": 1}]
        })

        assert response.status_code == 400

        # Verify no order created
        order_count = db_session.query(Order).count()
        assert order_count == 0
```

## Middleware Integration Tests

### Testing Middleware

```python
# Example 5: Middleware integration tests
class TestMiddlewareIntegration:
    """Integration tests for middleware"""

    def test_cors_middleware(self, client):
        """Test CORS headers"""
        response = client.options(
            "/users/",
            headers={
                "Origin": "http://localhost:3000",
                "Access-Control-Request-Method": "POST"
            }
        )

        assert "access-control-allow-origin" in response.headers
        assert "access-control-allow-methods" in response.headers

    def test_rate_limiting_middleware(self, client):
        """Test rate limiting"""
        # Make many requests quickly
        for _ in range(100):
            response = client.get("/users/")

        # Should eventually be rate limited
        # (depends on rate limit configuration)
        # assert response.status_code == 429

    def test_logging_middleware(self, client, caplog):
        """Test request logging"""
        with caplog.at_level("INFO"):
            response = client.get("/users/")

        assert "GET /users/" in caplog.text

    def test_authentication_middleware(self, client):
        """Test auth middleware"""
        # Without token
        response = client.get("/protected/")
        assert response.status_code == 401

        # With token
        response = client.get(
            "/protected/",
            headers={"Authorization": "Bearer valid_token"}
        )
        assert response.status_code == 200
```

## Background Task Integration

### Testing Async Tasks

```python
# Example 6: Background task integration tests
from unittest.mock import patch
import pytest

class TestBackgroundTasks:
    """Integration tests for background tasks"""

    def test_email_background_task(self, client, mock_email_service):
        """Test background email sending"""
        response = client.post("/users/", json={
            "username": "bguser",
            "email": "bg@example.com",
            "password": "SecurePass123!"
        })

        assert response.status_code == 201

        # Background task should have been queued
        # Verify email service was called
        assert mock_email_service.called

    def test_notification_task(self, client, db_session):
        """Test notification background task"""
        # Create user and trigger notification
        user = create_test_user(db_session)

        with patch("app.tasks.send_notification") as mock_notify:
            response = client.post(
                f"/users/{user.id}/notify",
                headers=get_auth_headers(user)
            )

            assert response.status_code == 200
            mock_notify.delay.assert_called_once()
```

## File Upload Integration

### Testing File Operations

```python
# Example 7: File upload integration tests
import io

class TestFileUploadIntegration:
    """Integration tests for file uploads"""

    def test_image_upload(self, client, auth_client):
        """Test image file upload"""
        file_content = io.BytesIO(b"fake image content")

        response = auth_client.post(
            "/upload/image",
            files={"file": ("test.jpg", file_content, "image/jpeg")}
        )

        assert response.status_code == 200
        assert "filename" in response.json()

    def test_invalid_file_type(self, client, auth_client):
        """Test rejecting invalid file type"""
        file_content = io.BytesIO(b"fake content")

        response = auth_client.post(
            "/upload/image",
            files={"file": ("test.exe", file_content, "application/exe")}
        )

        assert response.status_code == 400

    def test_file_too_large(self, client, auth_client):
        """Test file size limit"""
        large_content = io.BytesIO(b"x" * (10 * 1024 * 1024))  # 10MB

        response = auth_client.post(
            "/upload/image",
            files={"file": ("large.jpg", large_content, "image/jpeg")}
        )

        assert response.status_code == 413
```

## Best Practices

### Integration Testing Guidelines

```python
# Example 8: Integration testing best practices
"""
Integration Testing Best Practices:

1. Use test database
   - Separate from production
   - Clean between tests
   - Use transactions for isolation

2. Mock external services
   - Don't hit real APIs
   - Control responses
   - Test error scenarios

3. Test complete flows
   - End-to-end user journeys
   - Multi-step operations
   - Error recovery

4. Verify database state
   - Check created records
   - Verify updates
   - Confirm deletions

5. Test transaction behavior
   - Successful commits
   - Rollback on errors

6. Clean up after tests
   - Remove test data
   - Reset state
   - Close connections
"""
```

## Summary

| Integration Area | What to Test |
|-----------------|--------------|
| Database | CRUD, transactions, constraints |
| External Services | API calls, error handling |
| Middleware | Request/response processing |
| Background Tasks | Async operations |
| File Operations | Upload, download, processing |

## Next Steps

Continue learning about:
- [Database Integration Testing](./02_database_integration_testing.md) - DB tests
- [Service Layer Testing](./03_service_layer_testing.md) - Service tests
- [Performance Testing](../06_performance_testing/01_performance_testing_overview.md)
