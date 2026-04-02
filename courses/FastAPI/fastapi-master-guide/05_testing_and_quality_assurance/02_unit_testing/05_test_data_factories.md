# Test Data Factories

## Overview

Test data factories generate consistent, realistic test data for FastAPI tests.

## Factory Implementation

### Using Factory Boy

```python
# Example 1: Factory Boy implementation
import factory
from factory.alchemy import SQLAlchemyModelFactory
from app.models import User, Post

class UserFactory(SQLAlchemyModelFactory):
    """User test factory"""

    class Meta:
        model = User
        sqlalchemy_session_persistence = "commit"

    id = factory.Sequence(lambda n: n + 1)
    username = factory.Sequence(lambda n: f"user{n}")
    email = factory.LazyAttribute(lambda obj: f"{obj.username}@example.com")
    is_active = True

class PostFactory(SQLAlchemyModelFactory):
    """Post test factory"""

    class Meta:
        model = Post
        sqlalchemy_session_persistence = "commit"

    id = factory.Sequence(lambda n: n + 1)
    title = factory.Sequence(lambda n: f"Post {n}")
    content = factory.Faker("paragraph")
    author = factory.SubFactory(UserFactory)
```

### Using Fixtures

```python
# Example 2: Pytest fixtures with factories
import pytest

@pytest.fixture
def user_factory(db_session):
    """User factory fixture"""
    def create_user(**kwargs):
        user = UserFactory(**kwargs)
        db_session.add(user)
        db_session.commit()
        return user
    return create_user

@pytest.fixture
def sample_user(user_factory):
    """Create a sample user"""
    return user_factory(username="testuser", email="test@example.com")

@pytest.fixture
def multiple_users(user_factory):
    """Create multiple users"""
    return [user_factory() for _ in range(5)]
```

## Summary

Test factories reduce boilerplate and ensure consistent test data.

## Next Steps

Continue learning about:
- [Mock Objects](./04_mock_objects.md)
- [Test Coverage](./06_test_coverage.md)
