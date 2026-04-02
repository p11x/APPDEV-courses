# End-to-End Testing

## Overview

End-to-end (E2E) tests verify complete user workflows from start to finish, simulating real user interactions with your FastAPI application.

## E2E Test Setup

### Configuration

```python
# Example 1: E2E test configuration
import pytest
import httpx
from typing import AsyncGenerator

# E2E test configuration
E2E_BASE_URL = "http://localhost:8000"

@pytest.fixture(scope="session")
def e2e_client():
    """Provide HTTP client for E2E tests"""
    with httpx.Client(base_url=E2E_BASE_URL, timeout=30.0) as client:
        yield client

@pytest.fixture(scope="session")
async def async_e2e_client() -> AsyncGenerator[httpx.AsyncClient, None]:
    """Provide async HTTP client for E2E tests"""
    async with httpx.AsyncClient(base_url=E2E_BASE_URL, timeout=30.0) as client:
        yield client

@pytest.fixture(scope="session")
def e2e_user(e2e_client):
    """Create E2E test user"""
    import uuid
    unique = uuid.uuid4().hex[:8]

    user_data = {
        "username": f"e2e_user_{unique}",
        "email": f"e2e_{unique}@test.com",
        "password": "E2ETestPassword123!"
    }

    # Register
    response = e2e_client.post("/auth/register", json=user_data)
    assert response.status_code == 201

    # Login
    response = e2e_client.post("/auth/login", json={
        "username": user_data["username"],
        "password": user_data["password"]
    })
    token = response.json()["access_token"]

    return {
        **user_data,
        "token": token,
        "headers": {"Authorization": f"Bearer {token}"}
    }
```

## User Flow Tests

### Registration Flow

```python
# Example 2: Complete registration flow
class TestRegistrationFlow:
    """E2E tests for user registration"""

    def test_complete_registration_flow(self, e2e_client):
        """Test full registration process"""
        import uuid
        unique = uuid.uuid4().hex[:8]

        # Step 1: Register
        register_response = e2e_client.post("/auth/register", json={
            "username": f"newuser_{unique}",
            "email": f"new_{unique}@example.com",
            "password": "SecurePass123!"
        })

        assert register_response.status_code == 201
        user_data = register_response.json()
        assert user_data["username"] == f"newuser_{unique}"

        # Step 2: Login
        login_response = e2e_client.post("/auth/login", json={
            "username": f"newuser_{unique}",
            "password": "SecurePass123!"
        })

        assert login_response.status_code == 200
        token = login_response.json()["access_token"]

        # Step 3: Get profile
        profile_response = e2e_client.get(
            "/users/me",
            headers={"Authorization": f"Bearer {token}"}
        )

        assert profile_response.status_code == 200
        assert profile_response.json()["username"] == f"newuser_{unique}"

    def test_registration_validation_flow(self, e2e_client):
        """Test registration with validation errors"""
        # Try invalid email
        response = e2e_client.post("/auth/register", json={
            "username": "testuser",
            "email": "invalid-email",
            "password": "SecurePass123!"
        })
        assert response.status_code == 422

        # Try short password
        response = e2e_client.post("/auth/register", json={
            "username": "testuser",
            "email": "test@example.com",
            "password": "short"
        })
        assert response.status_code == 422
```

### CRUD Flow

```python
# Example 3: CRUD operation flow
class TestCRUDFlow:
    """E2E tests for CRUD operations"""

    def test_item_lifecycle(self, e2e_client, e2e_user):
        """Test complete item lifecycle"""
        headers = e2e_user["headers"]

        # Create
        create_response = e2e_client.post("/items/", json={
            "name": "E2E Test Item",
            "description": "Created during E2E test",
            "price": 29.99
        }, headers=headers)

        assert create_response.status_code == 201
        item_id = create_response.json()["id"]

        # Read
        read_response = e2e_client.get(
            f"/items/{item_id}",
            headers=headers
        )

        assert read_response.status_code == 200
        assert read_response.json()["name"] == "E2E Test Item"

        # Update
        update_response = e2e_client.patch(
            f"/items/{item_id}",
            json={"name": "Updated E2E Item", "price": 39.99},
            headers=headers
        )

        assert update_response.status_code == 200
        assert update_response.json()["name"] == "Updated E2E Item"

        # List (should include our item)
        list_response = e2e_client.get("/items/", headers=headers)

        assert list_response.status_code == 200
        item_ids = [item["id"] for item in list_response.json()]
        assert item_id in item_ids

        # Delete
        delete_response = e2e_client.delete(
            f"/items/{item_id}",
            headers=headers
        )

        assert delete_response.status_code == 204

        # Verify deleted
        verify_response = e2e_client.get(
            f"/items/{item_id}",
            headers=headers
        )

        assert verify_response.status_code == 404
```

## Complex Workflow Tests

### Order Flow

```python
# Example 4: Complex workflow - Order process
class TestOrderFlow:
    """E2E tests for order workflow"""

    def test_complete_order_flow(self, e2e_client, e2e_user):
        """Test complete order process"""
        headers = e2e_user["headers"]

        # Step 1: Browse products
        products_response = e2e_client.get("/products/", headers=headers)
        assert products_response.status_code == 200
        products = products_response.json()
        assert len(products) > 0

        # Step 2: Add to cart
        product_id = products[0]["id"]
        cart_response = e2e_client.post("/cart/items", json={
            "product_id": product_id,
            "quantity": 2
        }, headers=headers)

        assert cart_response.status_code == 201

        # Step 3: View cart
        cart_view = e2e_client.get("/cart/", headers=headers)
        assert cart_view.status_code == 200
        assert len(cart_view.json()["items"]) > 0

        # Step 4: Checkout
        order_response = e2e_client.post("/orders/", json={
            "shipping_address": "123 Test St, Test City, TC 12345",
            "payment_method": "credit_card"
        }, headers=headers)

        assert order_response.status_code == 201
        order_id = order_response.json()["id"]

        # Step 5: Verify order
        order_details = e2e_client.get(
            f"/orders/{order_id}",
            headers=headers
        )

        assert order_details.status_code == 200
        assert order_details.json()["status"] == "pending"

        # Step 6: Track order history
        history = e2e_client.get("/orders/", headers=headers)
        assert history.status_code == 200
        order_ids = [o["id"] for o in history.json()]
        assert order_id in order_ids
```

## Error Recovery Flows

### Failure Scenarios

```python
# Example 5: Error recovery tests
class TestErrorRecovery:
    """E2E tests for error recovery"""

    def test_session_expiration_recovery(self, e2e_client, e2e_user):
        """Test recovery from expired session"""
        headers = e2e_user["headers"]

        # Make authenticated request
        response = e2e_client.get("/users/me", headers=headers)
        assert response.status_code == 200

        # Simulate expired token
        expired_headers = {"Authorization": "Bearer expired_token"}
        response = e2e_client.get("/users/me", headers=expired_headers)
        assert response.status_code == 401

        # Re-login
        login_response = e2e_client.post("/auth/login", json={
            "username": e2e_user["username"],
            "password": e2e_user["password"]
        })

        assert login_response.status_code == 200
        new_token = login_response.json()["access_token"]

        # Continue with new token
        new_headers = {"Authorization": f"Bearer {new_token}"}
        response = e2e_client.get("/users/me", headers=new_headers)
        assert response.status_code == 200

    def test_conflict_resolution(self, e2e_client, e2e_user):
        """Test handling resource conflicts"""
        headers = e2e_user["headers"]

        # Create item
        item1 = e2e_client.post("/items/", json={
            "name": "Unique Item",
            "price": 10.00
        }, headers=headers).json()

        # Try to create duplicate
        response = e2e_client.post("/items/", json={
            "name": "Unique Item",
            "price": 20.00
        }, headers=headers)

        # Should handle conflict
        assert response.status_code in [400, 409]
```

## Performance E2E Tests

### Load Scenarios

```python
# Example 6: Performance E2E tests
class TestPerformanceE2E:
    """E2E performance tests"""

    def test_search_performance(self, e2e_client, e2e_user):
        """Test search endpoint performance"""
        import time

        headers = e2e_user["headers"]

        # Warm up
        e2e_client.get("/items/search?q=test", headers=headers)

        # Measure
        start = time.time()
        for _ in range(10):
            response = e2e_client.get("/items/search?q=test", headers=headers)
            assert response.status_code == 200
        elapsed = time.time() - start

        # Should complete 10 searches in under 5 seconds
        assert elapsed < 5.0

    def test_pagination_performance(self, e2e_client, e2e_user):
        """Test pagination performance"""
        headers = e2e_user["headers"]

        # Test various page sizes
        for limit in [10, 50, 100]:
            import time
            start = time.time()
            response = e2e_client.get(
                f"/items/?skip=0&limit={limit}",
                headers=headers
            )
            elapsed = time.time() - start

            assert response.status_code == 200
            assert elapsed < 2.0  # Under 2 seconds
```

## Best Practices

### E2E Testing Guidelines

```python
# Example 7: E2E testing best practices
"""
E2E Testing Best Practices:

1. Test realistic user journeys
   - Complete workflows
   - Common use cases
   - Error scenarios

2. Use dedicated test environment
   - Separate from development
   - Isolated data
   - Consistent state

3. Handle test data carefully
   - Clean up after tests
   - Use unique identifiers
   - Avoid conflicts

4. Make tests independent
   - No test depends on another
   - Can run in any order
   - Self-contained setup

5. Test failure scenarios
   - Network errors
   - Invalid inputs
   - Edge cases

6. Monitor test duration
   - E2E tests are slow
   - Run selectively
   - Parallelize when possible

7. Use meaningful assertions
   - Verify business outcomes
   - Check data integrity
   - Validate state changes
"""
```

## Summary

| Flow Type | What to Test |
|-----------|--------------|
| Registration | Sign up, validation, login |
| CRUD | Create, read, update, delete |
| Orders | Browse, cart, checkout, track |
| Error Recovery | Session expiry, conflicts |
| Performance | Response times, load |

## Next Steps

Continue learning about:
- [Performance Testing](../06_performance_testing/01_performance_testing_overview.md)
- [API Contract Testing](../03_api_testing/08_api_contract_testing.md)
