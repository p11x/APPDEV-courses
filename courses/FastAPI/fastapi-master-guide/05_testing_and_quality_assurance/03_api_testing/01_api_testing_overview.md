# API Testing Overview

## Overview

API testing verifies that your FastAPI endpoints work correctly, handle errors properly, and meet performance requirements.

## Test Setup

### Basic API Test Configuration

```python
# Example 1: API test setup
import pytest
from fastapi.testclient import TestClient
from httpx import AsyncClient, ASGITransport
from app.main import app

# Sync test client
@pytest.fixture
def client():
    """Provide sync test client"""
    return TestClient(app)

# Async test client
@pytest.fixture
async def async_client():
    """Provide async test client"""
    transport = ASGITransport(app=app)
    async with AsyncClient(transport=transport, base_url="http://test") as client:
        yield client

# Authenticated client
@pytest.fixture
def auth_client(client):
    """Client with authentication"""
    response = client.post("/auth/login", json={
        "username": "testuser",
        "password": "testpass123"
    })
    token = response.json()["access_token"]
    client.headers["Authorization"] = f"Bearer {token}"
    return client
```

## Testing API Endpoints

### CRUD Testing

```python
# Example 2: Testing CRUD operations
class TestUserAPI:
    """Test user API endpoints"""

    def test_create_user(self, client):
        """Test POST /users/"""
        response = client.post("/users/", json={
            "username": "newuser",
            "email": "new@example.com",
            "password": "SecurePass123!"
        })

        assert response.status_code == 201
        data = response.json()
        assert data["username"] == "newuser"
        assert "id" in data
        assert "password" not in data  # Password not returned

    def test_get_user(self, client, test_user):
        """Test GET /users/{id}"""
        response = client.get(f"/users/{test_user.id}")

        assert response.status_code == 200
        assert response.json()["username"] == test_user.username

    def test_list_users(self, client):
        """Test GET /users/"""
        response = client.get("/users/")

        assert response.status_code == 200
        assert isinstance(response.json(), list)

    def test_update_user(self, client, test_user):
        """Test PATCH /users/{id}"""
        response = client.patch(
            f"/users/{test_user.id}",
            json={"username": "updated"}
        )

        assert response.status_code == 200
        assert response.json()["username"] == "updated"

    def test_delete_user(self, client, test_user):
        """Test DELETE /users/{id}"""
        response = client.delete(f"/users/{test_user.id}")

        assert response.status_code == 204

        # Verify deletion
        response = client.get(f"/users/{test_user.id}")
        assert response.status_code == 404
```

### Request Validation Testing

```python
# Example 3: Testing input validation
class TestInputValidation:
    """Test request validation"""

    def test_missing_required_field(self, client):
        """Test missing required field"""
        response = client.post("/users/", json={
            "username": "test"
            # Missing email and password
        })

        assert response.status_code == 422
        errors = response.json()["detail"]
        assert any(e["loc"][-1] == "email" for e in errors)

    def test_invalid_email_format(self, client):
        """Test invalid email"""
        response = client.post("/users/", json={
            "username": "test",
            "email": "not-an-email",
            "password": "SecurePass123!"
        })

        assert response.status_code == 422

    def test_password_too_short(self, client):
        """Test password length validation"""
        response = client.post("/users/", json={
            "username": "test",
            "email": "test@example.com",
            "password": "short"
        })

        assert response.status_code == 422

    def test_username_too_long(self, client):
        """Test username length validation"""
        response = client.post("/users/", json={
            "username": "a" * 100,  # Too long
            "email": "test@example.com",
            "password": "SecurePass123!"
        })

        assert response.status_code == 422

    @pytest.mark.parametrize("invalid_data", [
        {"username": "", "email": "test@example.com", "password": "SecurePass123!"},
        {"username": "test", "email": "", "password": "SecurePass123!"},
        {"username": "test", "email": "test@example.com", "password": ""},
    ])
    def test_empty_required_fields(self, client, invalid_data):
        """Test empty required fields"""
        response = client.post("/users/", json=invalid_data)
        assert response.status_code == 422
```

### Response Validation

```python
# Example 4: Testing response structure
class TestResponseStructure:
    """Test response format and structure"""

    def test_user_response_schema(self, client, test_user):
        """Verify response matches expected schema"""
        response = client.get(f"/users/{test_user.id}")
        data = response.json()

        # Check required fields
        assert "id" in data
        assert "username" in data
        assert "email" in data
        assert "created_at" in data

        # Check types
        assert isinstance(data["id"], int)
        assert isinstance(data["username"], str)
        assert isinstance(data["email"], str)

        # Check no sensitive data
        assert "password" not in data
        assert "hashed_password" not in data

    def test_list_response_structure(self, client):
        """Test list endpoint structure"""
        response = client.get("/users/")
        data = response.json()

        assert isinstance(data, list)
        if len(data) > 0:
            assert "id" in data[0]
            assert "username" in data[0]

    def test_error_response_structure(self, client):
        """Test error response format"""
        response = client.get("/users/99999")

        assert response.status_code == 404
        data = response.json()
        assert "detail" in data

    def test_pagination_response(self, client):
        """Test paginated response structure"""
        response = client.get("/users/?skip=0&limit=10")

        assert response.status_code == 200
        # Depending on implementation, might have:
        # data = response.json()
        # assert "items" in data
        # assert "total" in data
        # assert "page" in data
```

## Error Handling Tests

### HTTP Status Code Testing

```python
# Example 5: Testing HTTP status codes
class TestStatusCodes:
    """Test correct HTTP status codes"""

    def test_200_ok(self, client, test_user):
        """Test successful GET"""
        response = client.get(f"/users/{test_user.id}")
        assert response.status_code == 200

    def test_201_created(self, client):
        """Test successful POST"""
        response = client.post("/users/", json={
            "username": "newuser",
            "email": "new@example.com",
            "password": "SecurePass123!"
        })
        assert response.status_code == 201

    def test_204_no_content(self, client, test_user):
        """Test successful DELETE"""
        response = client.delete(f"/users/{test_user.id}")
        assert response.status_code == 204

    def test_400_bad_request(self, client):
        """Test bad request"""
        response = client.post("/users/", json={
            "username": "test",
            "email": "existing@example.com",  # Assume exists
            "password": "SecurePass123!"
        })
        # If email already exists
        # assert response.status_code == 400

    def test_401_unauthorized(self, client):
        """Test missing authentication"""
        response = client.get("/users/me")
        assert response.status_code == 401

    def test_403_forbidden(self, client, auth_client):
        """Test insufficient permissions"""
        response = auth_client.delete("/admin/users/1")
        assert response.status_code == 403

    def test_404_not_found(self, client):
        """Test resource not found"""
        response = client.get("/users/99999")
        assert response.status_code == 404

    def test_409_conflict(self, client, test_user):
        """Test duplicate resource"""
        response = client.post("/users/", json={
            "username": test_user.username,  # Duplicate
            "email": "another@example.com",
            "password": "SecurePass123!"
        })
        assert response.status_code == 409

    def test_422_validation_error(self, client):
        """Test validation error"""
        response = client.post("/users/", json={})
        assert response.status_code == 422
```

## Query Parameter Testing

### Filtering and Sorting

```python
# Example 6: Testing query parameters
class TestQueryParameters:
    """Test query parameter handling"""

    def test_filter_by_status(self, client):
        """Test filtering"""
        response = client.get("/users/?is_active=true")
        assert response.status_code == 200

        for user in response.json():
            assert user["is_active"] is True

    def test_search(self, client):
        """Test search parameter"""
        response = client.get("/users/?search=john")
        assert response.status_code == 200

    def test_pagination(self, client):
        """Test pagination parameters"""
        response = client.get("/users/?skip=0&limit=5")
        assert response.status_code == 200
        assert len(response.json()) <= 5

    def test_sorting(self, client):
        """Test sort parameter"""
        response = client.get("/users/?sort_by=created_at&order=desc")
        assert response.status_code == 200

    @pytest.mark.parametrize("invalid_param", [
        "?skip=-1",
        "?limit=0",
        "?limit=10000",
    ])
    def test_invalid_pagination(self, client, invalid_param):
        """Test invalid pagination"""
        response = client.get(f"/users/{invalid_param}")
        assert response.status_code == 422
```

## Content Type Testing

### Request/Response Headers

```python
# Example 7: Testing headers and content types
class TestHeaders:
    """Test HTTP headers"""

    def test_json_content_type(self, client):
        """Test JSON response content type"""
        response = client.get("/users/")
        assert response.headers["content-type"] == "application/json"

    def test_custom_headers(self, client):
        """Test custom response headers"""
        response = client.get("/users/")
        # If you add custom headers
        # assert "X-Total-Count" in response.headers

    def test_cors_headers(self, client):
        """Test CORS headers"""
        response = client.options(
            "/users/",
            headers={"Origin": "http://localhost:3000"}
        )
        # assert "access-control-allow-origin" in response.headers

    def test_accept_header(self, client):
        """Test Accept header handling"""
        response = client.get(
            "/users/",
            headers={"Accept": "application/json"}
        )
        assert response.status_code == 200
```

## Best Practices

### API Testing Guidelines

```python
# Example 8: API testing best practices
"""
API Testing Best Practices:

1. Test all CRUD operations
2. Test input validation thoroughly
3. Test authentication and authorization
4. Test error responses
5. Verify response schemas
6. Test query parameters
7. Test edge cases
8. Use meaningful test data
9. Clean up test data
10. Test performance for critical paths
"""

# Comprehensive endpoint test
class TestComprehensiveEndpoint:
    """Complete test coverage for an endpoint"""

    def test_success_path(self, client, auth_client):
        """Test happy path"""
        pass

    def test_unauthorized_access(self, client):
        """Test without auth"""
        pass

    def test_forbidden_access(self, client, auth_client):
        """Test insufficient permissions"""
        pass

    def test_invalid_input(self, client, auth_client):
        """Test validation"""
        pass

    def test_not_found(self, client, auth_client):
        """Test missing resource"""
        pass

    def test_duplicate(self, client, auth_client):
        """Test conflict handling"""
        pass
```

## Summary

| Test Area | What to Verify | Example |
|-----------|----------------|---------|
| CRUD | Create, Read, Update, Delete | All HTTP methods |
| Validation | Input constraints | 422 errors |
| Auth | Access control | 401/403 errors |
| Errors | Error responses | Status codes |
| Response | Schema, format | JSON structure |

## Next Steps

Continue learning about:
- [HTTP Status Codes](./02_http_status_codes.md) - Status code testing
- [API Authentication Testing](./04_api_authentication_testing.md) - Auth testing
- [Integration Testing](../04_integration_testing/01_integration_testing_setup.md)
