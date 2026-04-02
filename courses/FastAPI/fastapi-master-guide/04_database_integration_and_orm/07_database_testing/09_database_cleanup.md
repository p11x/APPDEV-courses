# Database Cleanup

## Overview

Proper test cleanup ensures tests don't interfere with each other.

## Cleanup Strategies

### Transaction Rollback

```python
# Example 1: Transaction-based cleanup
import pytest
from sqlalchemy.orm import Session

@pytest.fixture
def db_session():
    """Session that rolls back after test"""
    connection = engine.connect()
    transaction = connection.begin()
    session = Session(bind=connection)

    yield session

    session.close()
    transaction.rollback()
    connection.close()
```

### Table Truncation

```python
# Example 2: Table cleanup
@pytest.fixture(autouse=True)
def cleanup_tables(db_session):
    """Clean up tables after each test"""
    yield

    # Delete all data
    for table in reversed(Base.metadata.sorted_tables):
        db_session.execute(table.delete())
    db_session.commit()
```

### Factory Cleanup

```python
# Example 3: Factory cleanup
@pytest.fixture
def user_factory(db_session):
    """Factory with cleanup tracking"""
    created = []

    def create(**kwargs):
        user = User(**kwargs)
        db_session.add(user)
        db_session.commit()
        created.append(user)
        return user

    yield create

    # Cleanup created items
    for item in created:
        db_session.delete(item)
    db_session.commit()
```

## Summary

Test cleanup ensures test isolation.

## Next Steps

Continue learning about:
- [Integration Testing](./08_integration_testing.md)
- [Transaction Testing](./04_transaction_testing.md)
