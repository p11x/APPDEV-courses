# Test Database Setup

## Overview

Test databases provide isolated environments for running tests without affecting production data.

## Setup

### SQLite Test Database

```python
# Example 1: In-memory SQLite for testing
import pytest
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.pool import StaticPool
from app.database import Base

TEST_DATABASE_URL = "sqlite:///:memory:"

engine = create_engine(
    TEST_DATABASE_URL,
    connect_args={"check_same_thread": False},
    poolclass=StaticPool
)

TestSession = sessionmaker(bind=engine)

@pytest.fixture(scope="session")
def setup_database():
    """Create tables for test session"""
    Base.metadata.create_all(engine)
    yield
    Base.metadata.drop_all(engine)

@pytest.fixture
def db_session(setup_database):
    """Provide test database session"""
    connection = engine.connect()
    transaction = connection.begin()
    session = TestSession(bind=connection)

    yield session

    session.close()
    transaction.rollback()
    connection.close()
```

### PostgreSQL Test Database

```python
# Example 2: PostgreSQL test database
TEST_DATABASE_URL = "postgresql://test:test@localhost:5432/test_db"

@pytest.fixture(scope="session")
def test_engine():
    """Create test engine"""
    engine = create_engine(TEST_DATABASE_URL)
    Base.metadata.create_all(engine)
    yield engine
    Base.metadata.drop_all(engine)
    engine.dispose()
```

## Summary

Test databases enable isolated, repeatable tests.

## Next Steps

Continue learning about:
- [Fixtures and Factories](./03_fixtures_and_factories.md)
- [Transaction Testing](./04_transaction_testing.md)
