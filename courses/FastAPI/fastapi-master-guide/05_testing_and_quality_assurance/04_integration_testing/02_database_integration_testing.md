# Database Integration Testing

## Overview

Database integration tests verify that your FastAPI application correctly interacts with the database.

## Test Setup

### Database Fixtures

```python
# Example 1: Database test setup
import pytest
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy.pool import StaticPool
from app.database import Base, get_db
from app.main import app
from fastapi.testclient import TestClient

# Test database
TEST_DATABASE_URL = "sqlite:///:memory:"
engine = create_engine(
    TEST_DATABASE_URL,
    connect_args={"check_same_thread": False},
    poolclass=StaticPool
)
TestSession = sessionmaker(bind=engine)

@pytest.fixture(scope="session")
def setup_db():
    Base.metadata.create_all(engine)
    yield
    Base.metadata.drop_all(engine)

@pytest.fixture
def db_session(setup_db):
    connection = engine.connect()
    transaction = connection.begin()
    session = TestSession(bind=connection)
    yield session
    session.close()
    transaction.rollback()
    connection.close()

@pytest.fixture
def client(db_session):
    def override_get_db():
        try:
            yield db_session
        finally:
            pass

    app.dependency_overrides[get_db] = override_get_db
    with TestClient(app) as c:
        yield c
    app.dependency_overrides.clear()
```

## CRUD Tests

```python
# Example 2: Database CRUD tests
class TestDatabaseOperations:

    def test_create_user(self, client, db_session):
        response = client.post("/users/", json={
            "username": "newuser",
            "email": "new@example.com"
        })
        assert response.status_code == 201

        # Verify in database
        user = db_session.query(User).filter_by(username="newuser").first()
        assert user is not None

    def test_get_user(self, client, db_session):
        user = User(username="test", email="test@example.com")
        db_session.add(user)
        db_session.commit()

        response = client.get(f"/users/{user.id}")
        assert response.status_code == 200
        assert response.json()["username"] == "test"
```

## Summary

Database integration tests ensure data persistence works correctly.

## Next Steps

Continue learning about:
- [Service Layer Testing](./03_service_layer_testing.md)
- [Transaction Testing](../07_database_testing/04_transaction_testing.md)
