# Mock Database Operations

## Overview

Mocking database operations isolates unit tests from actual database dependencies.

## Mock Strategies

### Mocking SQLAlchemy Sessions

```python
# Example 1: Mock database session
from unittest.mock import Mock, MagicMock
import pytest

@pytest.fixture
def mock_db():
    """Create mock database session"""
    db = MagicMock()

    # Mock query chain
    db.query.return_value.filter.return_value.first.return_value = None
    db.query.return_value.filter.return_value.all.return_value = []
    db.query.return_value.offset.return_value.limit.return_value.all.return_value = []

    # Mock add and commit
    db.add = Mock()
    db.commit = Mock()
    db.refresh = Mock()

    return db

def test_create_user(mock_db):
    """Test user creation with mocked database"""
    service = UserService(mock_db)

    user = service.create_user("testuser", "test@example.com")

    mock_db.add.assert_called_once()
    mock_db.commit.assert_called_once()
    assert user.username == "testuser"
```

### Mocking Repository Pattern

```python
# Example 2: Mock repository
from unittest.mock import AsyncMock

@pytest.fixture
def mock_user_repo():
    """Mock user repository"""
    repo = AsyncMock()
    repo.get_by_id.return_value = User(id=1, username="test")
    repo.get_by_username.return_value = User(id=1, username="test")
    repo.create.return_value = User(id=1, username="newuser")
    return repo

@pytest.mark.asyncio
async def test_get_user(mock_user_repo):
    """Test getting user with mocked repo"""
    service = UserService(mock_user_repo)

    user = await service.get_user(1)

    mock_user_repo.get_by_id.assert_called_once_with(1)
    assert user.username == "test"
```

## Best Practices

1. Mock at the right abstraction level
2. Configure mock return values
3. Assert mock was called correctly
4. Use fixtures for common setups

## Summary

Mocking database operations enables fast, isolated unit tests.

## Next Steps

Continue learning about:
- [Transaction Testing](./04_transaction_testing.md)
- [Test Data Factories](../../02_unit_testing/05_test_data_factories.md)
