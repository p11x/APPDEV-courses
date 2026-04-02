# Test Naming Conventions

## Overview

Consistent test names improve readability and help identify failures quickly.

## Naming Patterns

### Standard Patterns

```python
# Example 1: Test naming patterns
"""
Pattern: test_<unit>_<condition>_<expected>

Good Examples:
- test_create_user_with_valid_data_returns_201
- test_login_with_wrong_password_returns_401
- test_get_item_not_found_returns_404
- test_update_user_without_auth_returns_401

Bad Examples:
- test_user
- test_create
- test_error
"""

# Good test names
def test_create_user_with_valid_data_succeeds(client):
    """Test successful user creation"""
    pass

def test_create_user_with_duplicate_email_fails(client):
    """Test duplicate email rejection"""
    pass

def test_create_user_with_invalid_email_returns_422(client):
    """Test email validation"""
    pass
```

### Class Organization

```python
# Example 2: Test class naming
class TestUserCreation:
    """Tests for user creation"""

    def test_with_valid_data_succeeds(self):
        pass

    def test_with_duplicate_username_fails(self):
        pass

class TestUserAuthentication:
    """Tests for user authentication"""

    def test_login_with_valid_credentials_succeeds(self):
        pass

    def test_login_with_invalid_password_fails(self):
        pass
```

## Summary

Consistent naming makes tests easier to understand and maintain.

## Next Steps

Continue learning about:
- [Test Organization](./07_test_organization.md)
- [Test Documentation](./04_test_documentation.md)
