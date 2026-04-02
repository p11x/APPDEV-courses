# Transaction Testing

## Overview

Testing database transactions ensures data consistency and proper rollback behavior.

## Transaction Tests

### Testing Commits

```python
# Example 1: Test transaction commits
import pytest
from sqlalchemy.orm import Session

def test_user_creation_commits(db_session: Session):
    """Test that user creation commits properly"""
    user = User(username="test", email="test@example.com")
    db_session.add(user)
    db_session.commit()

    # Verify user exists
    found = db_session.query(User).filter_by(username="test").first()
    assert found is not None
    assert found.email == "test@example.com"
```

### Testing Rollbacks

```python
# Example 2: Test transaction rollback
def test_rollback_on_error(db_session: Session):
    """Test rollback when error occurs"""
    user = User(username="rollback_test", email="test@example.com")
    db_session.add(user)

    try:
        # Simulate error
        raise Exception("Test error")
    except Exception:
        db_session.rollback()

    # Verify rollback
    found = db_session.query(User).filter_by(username="rollback_test").first()
    assert found is None

def test_unique_constraint_rollback(db_session: Session):
    """Test rollback on constraint violation"""
    user1 = User(username="unique", email="test1@example.com")
    db_session.add(user1)
    db_session.commit()

    user2 = User(username="unique", email="test2@example.com")
    db_session.add(user2)

    with pytest.raises(Exception):
        db_session.commit()

    db_session.rollback()
```

## Summary

Transaction tests ensure data consistency and proper error handling.

## Next Steps

Continue learning about:
- [Integration Testing](./08_integration_testing.md)
- [Database Cleanup](./09_database_cleanup.md)
