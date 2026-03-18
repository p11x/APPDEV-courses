<!-- FILE: 09_testing/02_unit_tests/02_testing_models.md -->

## Overview

Testing database models ensures that your data layer works correctly. This file covers testing model creation, validation, relationships, and query methods.

## Prerequisites

- Flask test client and database setup
- SQLAlchemy models defined

## Code Walkthrough

### Testing Model Creation

```python
# tests/test_models.py
import pytest
from app import app, db
from models import User, Post, Tag

@pytest.fixture
def client():
    app.config['TESTING'] = True
    app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///:memory:'
    with app.test_client() as client:
        with app.app_context():
            db.create_all()
            yield client
            db.drop_all()

def test_user_creation(client):
    """Test creating a user model."""
    with app.app_context():
        user = User(username='testuser', email='test@example.com')
        user.set_password('secret123')
        db.session.add(user)
        db.session.commit()
        
        # Retrieve from database
        retrieved = User.query.filter_by(username='testuser').first()
        assert retrieved is not None
        assert retrieved.email == 'test@example.com'
        assert retrieved.check_password('secret123')
        assert not retrieved.check_password('wrong')

def test_user_repr(client):
    """Test User model string representation."""
    with app.app_context():
        user = User(username='testuser', email='test@example.com')
        db.session.add(user)
        db.session.commit()
        assert repr(user) == '<User testuser>'

def test_post_creation(client):
    """Test creating a post with relationship."""
    with app.app_context():
        # Create user first
        user = User(username='blogger', email='blogger@example.com')
        user.set_password('secret123')
        db.session.add(user)
        db.session.commit()
        
        # Create post
        post = Post(
            title='My First Post',
            content='This is the content.',
            user_id=user.id
        )
        db.session.add(post)
        db.session.commit()
        
        # Test relationships
        assert post.author == user
        assert user.posts.count() == 1
        assert user.posts.first() == post

def test_model_validation(client):
    """Test model validation and constraints."""
    with app.app_context():
        # Test unique constraint
        user1 = User(username='unique', email='unique@example.com')
        user2 = User(username='unique', email='different@example.com')  # Same username
        
        db.session.add(user1)
        db.session.commit()
        
        db.session.add(user2)
        with pytest.raises(Exception):  # IntegrityError
            db.session.commit()
        
        db.session.rollback()

def test_model_methods(client):
    """Test custom model methods."""
    with app.app_context():
        user = User(username='testuser', email='test@example.com')
        user.set_password('secret123')
        db.session.add(user)
        db.session.commit()
        
        # Test to_dict method if exists
        if hasattr(user, 'to_dict'):
            user_dict = user.to_dict()
            assert user_dict['username'] == 'testuser'
            assert user_dict['email'] == 'test@example.com'
            assert 'password_hash' not in user_dict  # Should not expose hash
```

### Testing Model Queries

```python
def test_user_queries(client):
    """Test querying users."""
    with app.app_context():
        # Create test data
        users = [
            User(username='alice', email='alice@example.com'),
            User(username='bob', email='bob@example.com'),
            User(username='charlie', email='charlie@example.com')
        ]
        for user in users:
            user.set_password('password')
            db.session.add(user)
        db.session.commit()
        
        # Test count
        assert User.query.count() == 3
        
        # Test filter
        alice = User.query.filter_by(username='alice').first()
        assert alice is not None
        assert alice.email == 'alice@example.com'
        
        # Test ordering
        ordered = User.query.order_by(User.username).all()
        assert [u.username for u in ordered] == ['alice', 'bob', 'charlie']
        
        # Test limit
        limited = User.query.limit(2).all()
        assert len(limited) == 2
```

### Testing Model Relationships

```python
def test_post_tag_relationship(client):
    """Test many-to-many relationship between posts and tags."""
    with app.app_context():
        # Create user
        user = User(username='tester', email='tester@example.com')
        user.set_password('password')
        db.session.add(user)
        
        # Create tags
        tag1 = Tag(name='python')
        tag2 = Tag(name='flask')
        db.session.add_all([tag1, tag2])
        
        # Create post
        post = Post(
            title='Flask and Python',
            content='Learning Flask with Python',
            user_id=user.id
        )
        post.tags.append(tag1)
        post.tags.append(tag2)
        db.session.add(post)
        db.session.commit()
        
        # Test relationships
        assert post.tags.count() == 2
        assert tag1 in post.tags
        assert tag2 in post.tags
        assert post in tag1.posts
        assert post in tag2.posts
```

## Common Mistakes

❌ **Not using application context**
```python
# WRONG — Database operations need app context
user = User.query.first()  # Might fail outside app context
```

✅ **Correct — Use app context**
```python
# CORRECT
with app.app_context():
    user = User.query.first()
```

❌ **Not rolling back transactions**
```python
# WRONG — Test data persists between tests
# Always rollback or use fresh database
```

✅ **Correct — Use fixtures for clean state**
```python
# CORRECT
@pytest.fixture
def client():
    # Setup
    yield client
    # Teardown happens automatically with fixture
```

## Quick Reference

| Test Type | Description |
|-----------|-------------|
| Model creation | Test saving and retrieving |
| Validation | Test constraints and errors |
| Relationships | Test foreign keys and joins |
| Queries | Test filtering and ordering |
| Methods | Test custom model methods |

## Next Steps

Now you can test models. Continue to [03_mocking_dependencies.md](03_mocking_dependencies.md) to learn about mocking external dependencies.