# Service Layer Testing

## Overview

Testing the service layer ensures business logic works correctly independent of API endpoints.

## Service Testing

### Testing Services

```python
# Example 1: Service layer tests
import pytest
from unittest.mock import Mock, AsyncMock

class UserService:
    def __init__(self, db, email_service):
        self.db = db
        self.email_service = email_service

    async def create_user(self, username: str, email: str):
        if self.db.query(User).filter_by(email=email).first():
            raise ValueError("Email already exists")

        user = User(username=username, email=email)
        self.db.add(user)
        self.db.commit()

        await self.email_service.send_welcome(email)
        return user

@pytest.fixture
def mock_db():
    db = Mock()
    db.query.return_value.filter_by.return_value.first.return_value = None
    return db

@pytest.fixture
def mock_email():
    return AsyncMock()

@pytest.fixture
def user_service(mock_db, mock_email):
    return UserService(mock_db, mock_email)

@pytest.mark.asyncio
async def test_create_user(user_service, mock_db, mock_email):
    """Test user creation"""
    user = await user_service.create_user("test", "test@example.com")

    mock_db.add.assert_called_once()
    mock_db.commit.assert_called_once()
    mock_email.send_welcome.assert_called_once_with("test@example.com")

@pytest.mark.asyncio
async def test_create_duplicate_user(user_service, mock_db):
    """Test duplicate user error"""
    mock_db.query.return_value.filter_by.return_value.first.return_value = User()

    with pytest.raises(ValueError):
        await user_service.create_user("test", "test@example.com")
```

## Summary

Service layer testing isolates business logic from infrastructure.

## Next Steps

Continue learning about:
- [External Service Integration](./04_external_service_integration.md)
- [API Integration Testing](./05_api_integration_testing.md)
