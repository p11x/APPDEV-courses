# Test Pyramid

## Overview

The test pyramid guides the distribution of different test types for optimal coverage and speed.

## Test Pyramid Structure

```
        /\
       /  \      E2E Tests (Few, Slow)
      /    \     - Full user flows
     /------\    - Real external services
    /        \
   / Integr.  \  Integration Tests (Some)
  /   Tests    \ - Component interactions
 /--------------\ - Database, APIs
/                \
/   Unit Tests    \ Unit Tests (Many, Fast)
/                  \ - Individual functions
/-------------------\ - No external dependencies
```

## Test Distribution

### Unit Tests (70-80%)

```python
# Example 1: Unit tests - fast, isolated
def calculate_discount(price: float, percent: float) -> float:
    return price * (1 - percent / 100)

def test_calculate_discount():
    assert calculate_discount(100, 10) == 90.0
    assert calculate_discount(100, 0) == 100.0
    assert calculate_discount(100, 100) == 0.0
```

### Integration Tests (15-25%)

```python
# Example 2: Integration tests - component interaction
def test_user_service_integration(db_session):
    service = UserService(db_session)
    user = service.create_user("test", "test@example.com")
    assert user.id is not None
```

### E2E Tests (5-10%)

```python
# Example 3: E2E tests - full flows
def test_user_registration_flow(client):
    response = client.post("/register", json={...})
    assert response.status_code == 201
```

## Best Practices

1. Most tests should be unit tests
2. Integration tests cover critical paths
3. E2E tests cover happy paths
4. Fast feedback loops

## Summary

The test pyramid ensures fast, reliable testing with appropriate coverage.

## Next Steps

Continue learning about:
- [Testing Philosophy](./03_testing_philosophy.md)
- [Unit Testing](../02_unit_testing/01_setup_and_configuration.md)
