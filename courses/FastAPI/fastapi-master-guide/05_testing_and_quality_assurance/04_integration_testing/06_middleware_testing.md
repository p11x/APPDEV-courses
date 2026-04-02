# Middleware Testing

## Overview

Testing middleware ensures request/response processing works correctly.

## Middleware Tests

### Testing Custom Middleware

```python
# Example 1: Testing request middleware
from fastapi import FastAPI, Request
from fastapi.testclient import TestClient

app = FastAPI()

@app.middleware("http")
async def add_request_id(request: Request, call_next):
    """Add request ID to all requests"""
    request_id = request.headers.get("X-Request-ID", "generated-id")
    response = await call_next(request)
    response.headers["X-Request-ID"] = request_id
    return response

def test_middleware_adds_header():
    """Test middleware adds header"""
    client = TestClient(app)
    response = client.get("/", headers={"X-Request-ID": "test-123"})
    assert response.headers["X-Request-ID"] == "test-123"

def test_middleware_generates_id():
    """Test middleware generates ID"""
    client = TestClient(app)
    response = client.get("/")
    assert "X-Request-ID" in response.headers
```

### Testing Auth Middleware

```python
# Example 2: Testing auth middleware
def test_auth_middleware_blocks_unauthenticated():
    """Test auth middleware blocks access"""
    client = TestClient(app)
    response = client.get("/protected/")
    assert response.status_code == 401

def test_auth_middleware_allows_authenticated():
    """Test auth middleware allows access"""
    client = TestClient(app)
    response = client.get(
        "/protected/",
        headers={"Authorization": "Bearer valid-token"}
    )
    assert response.status_code == 200
```

## Summary

Middleware tests ensure request/response processing is correct.

## Next Steps

Continue learning about:
- [Authentication Integration Testing](./07_authentication_integration_testing.md)
- [Background Task Testing](./09_background_task_testing.md)
