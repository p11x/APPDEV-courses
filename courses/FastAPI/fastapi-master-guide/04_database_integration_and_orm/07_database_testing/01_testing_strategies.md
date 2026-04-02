# Database Testing

## Overview

Testing database operations ensures data integrity and application reliability. This guide covers comprehensive database testing strategies for FastAPI.

## Test Database Setup

### Test Configuration

```python
# Example 1: Test database setup
import pytest
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, Session
from sqlalchemy.pool import StaticPool
from fastapi.testclient import TestClient

from app.main import app
from app.database.base import Base
from app.database.session import get_db

# Test database URL (SQLite in-memory for speed)
TEST_DATABASE_URL = "sqlite:///:memory:"

# Create test engine
test_engine = create_engine(
    TEST_DATABASE_URL,
    connect_args={"check_same_thread": False},
    poolclass=StaticPool  # Single connection for SQLite memory
)

# Test session factory
TestSessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=test_engine)

@pytest.fixture(scope="session")
def setup_database():
    """Create all tables for testing"""
    Base.metadata.create_all(bind=test_engine)
    yield
    Base.metadata.drop_all(bind=test_engine)

@pytest.fixture
def db_session():
    """Get test database session"""
    session = TestSessionLocal()
    try:
        yield session
    finally:
        session.rollback()
        session.close()

@pytest.fixture
def client(db_session: Session):
    """Get test client with overridden DB dependency"""
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

## CRUD Testing

### Testing Database Operations

```python
# Example 2: CRUD operation tests
import pytest
from app.models.user import User
from app.schemas.user import UserCreate

class TestUserCRUD:
    """Test user CRUD operations"""

    def test_create_user(self, db_session: Session):
        """Test user creation"""
        user = User(
            username="testuser",
            email="test@example.com",
            hashed_password="hashed_password"
        )
        db_session.add(user)
        db_session.commit()
        db_session.refresh(user)

        assert user.id is not None
        assert user.username == "testuser"
        assert user.email == "test@example.com"

    def test_get_user_by_id(self, db_session: Session):
        """Test getting user by ID"""
        # Create user
        user = User(username="john", email="john@example.com", hashed_password="pass")
        db_session.add(user)
        db_session.commit()

        # Retrieve user
        found = db_session.query(User).filter(User.id == user.id).first()

        assert found is not None
        assert found.username == "john"

    def test_update_user(self, db_session: Session):
        """Test user update"""
        user = User(username="jane", email="jane@example.com", hashed_password="pass")
        db_session.add(user)
        db_session.commit()

        # Update
        user.username = "janedoe"
        db_session.commit()
        db_session.refresh(user)

        assert user.username == "janedoe"

    def test_delete_user(self, db_session: Session):
        """Test user deletion"""
        user = User(username="delete_me", email="delete@example.com", hashed_password="pass")
        db_session.add(user)
        db_session.commit()

        # Delete
        db_session.delete(user)
        db_session.commit()

        # Verify deletion
        found = db_session.query(User).filter(User.username == "delete_me").first()
        assert found is None

    def test_unique_constraint(self, db_session: Session):
        """Test unique constraint violation"""
        user1 = User(username="unique", email="unique1@example.com", hashed_password="pass")
        user2 = User(username="unique", email="unique2@example.com", hashed_password="pass")

        db_session.add(user1)
        db_session.commit()

        db_session.add(user2)
        with pytest.raises(Exception):  # IntegrityError
            db_session.commit()
```

## Transaction Testing

### Testing Transactions

```python
# Example 3: Transaction tests
class TestTransactions:
    """Test database transactions"""

    def test_rollback_on_error(self, db_session: Session):
        """Test transaction rollback"""
        try:
            user = User(username="rollback", email="rollback@example.com", hashed_password="pass")
            db_session.add(user)
            db_session.flush()  # Get ID without committing

            # Simulate error
            raise Exception("Simulated error")

        except Exception:
            db_session.rollback()

        # Verify rollback
        found = db_session.query(User).filter(User.username == "rollback").first()
        assert found is None

    def test_nested_transaction(self, db_session: Session):
        """Test savepoint (nested transaction)"""
        user = User(username="nested", email="nested@example.com", hashed_password="pass")
        db_session.add(user)
        db_session.commit()

        # Nested transaction (savepoint)
        savepoint = db_session.begin_nested()
        try:
            user.username = "changed"
            db_session.flush()
            raise Exception("Error in savepoint")
        except Exception:
            savepoint.rollback()

        # Main transaction should be fine
        db_session.refresh(user)
        assert user.username == "nested"
```

## API Endpoint Testing

### Integration Tests

```python
# Example 4: API endpoint tests
class TestUserEndpoints:
    """Test user API endpoints"""

    def test_create_user_endpoint(self, client: TestClient):
        """Test POST /users/"""
        response = client.post("/users/", json={
            "username": "newuser",
            "email": "new@example.com",
            "password": "securepassword123"
        })

        assert response.status_code == 201
        data = response.json()
        assert data["username"] == "newuser"
        assert "id" in data

    def test_get_user_endpoint(self, client: TestClient, db_session: Session):
        """Test GET /users/{id}"""
        # Create user in DB
        user = User(username="getuser", email="get@example.com", hashed_password="pass")
        db_session.add(user)
        db_session.commit()

        # Test endpoint
        response = client.get(f"/users/{user.id}")

        assert response.status_code == 200
        assert response.json()["username"] == "getuser"

    def test_get_nonexistent_user(self, client: TestClient):
        """Test GET with invalid ID"""
        response = client.get("/users/99999")

        assert response.status_code == 404

    def test_list_users_endpoint(self, client: TestClient, db_session: Session):
        """Test GET /users/"""
        # Create multiple users
        for i in range(5):
            user = User(
                username=f"user{i}",
                email=f"user{i}@example.com",
                hashed_password="pass"
            )
            db_session.add(user)
        db_session.commit()

        # Test endpoint
        response = client.get("/users/")

        assert response.status_code == 200
        data = response.json()
        assert len(data) >= 5
```

## Factory Pattern

### Test Data Factories

```python
# Example 5: Test data factories
import factory
from factory.alchemy import SQLAlchemyModelFactory
from app.models.user import User
from app.models.post import Post

class UserFactory(SQLAlchemyModelFactory):
    """User test factory"""

    class Meta:
        model = User
        sqlalchemy_session_persistence = "commit"

    id = factory.Sequence(lambda n: n)
    username = factory.Sequence(lambda n: f"user{n}")
    email = factory.LazyAttribute(lambda obj: f"{obj.username}@example.com")
    hashed_password = "hashed_password"
    is_active = True

class PostFactory(SQLAlchemyModelFactory):
    """Post test factory"""

    class Meta:
        model = Post
        sqlalchemy_session_persistence = "commit"

    id = factory.Sequence(lambda n: n)
    title = factory.Sequence(lambda n: f"Post {n}")
    content = factory.Faker("paragraph")
    author = factory.SubFactory(UserFactory)

# Usage in tests
def test_with_factories(db_session: Session):
    """Test using factories"""
    user = UserFactory.create(session=db_session)
    posts = PostFactory.create_batch(5, author=user, session=db_session)

    assert len(user.posts) == 5
```

## Async Testing

### Async Database Tests

```python
# Example 6: Async database tests
import pytest
from httpx import AsyncClient
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession

@pytest.fixture
async def async_db():
    """Async test database"""
    engine = create_async_engine("sqlite+aiosqlite:///:memory:")

    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)

    async_session = sessionmaker(engine, class_=AsyncSession)

    async with async_session() as session:
        yield session

    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.drop_all)

@pytest.mark.asyncio
async def test_async_user_creation(async_db: AsyncSession):
    """Test async user creation"""
    user = User(username="asyncuser", email="async@example.com", hashed_password="pass")
    async_db.add(user)
    await async_db.commit()
    await async_db.refresh(user)

    assert user.id is not None

@pytest.mark.asyncio
async def test_async_api_endpoint():
    """Test async API endpoint"""
    async with AsyncClient(app=app, base_url="http://test") as client:
        response = await client.get("/users/")
        assert response.status_code == 200
```

## Best Practices

### Testing Guidelines

```python
# Example 7: Database testing best practices
"""
Database Testing Best Practices:

1. Use in-memory SQLite for unit tests
   - Fast execution
   - No external dependencies

2. Use fixtures for common setup
   - Reusable test data
   - Consistent state

3. Clean up after tests
   - Rollback transactions
   - Drop test data

4. Test both success and failure cases
   - Happy path
   - Error handling

5. Use factories for complex test data
   - Generate realistic data
   - Reduce boilerplate

6. Test transaction behavior
   - Rollback scenarios
   - Concurrent access

7. Mock external services
   - Don't test external APIs
   - Focus on your code
"""
```

## Summary

| Test Type | Database | Speed |
|-----------|----------|-------|
| Unit tests | SQLite memory | Fast |
| Integration | SQLite file | Medium |
| E2E tests | PostgreSQL | Slow |

## Next Steps

Continue learning about:
- [Test Databases](./02_test_databases.md) - Database selection
- [Fixtures and Factories](./03_fixtures_and_factories.md) - Test data
- [Performance Testing](./06_performance_testing.md) - Load testing
