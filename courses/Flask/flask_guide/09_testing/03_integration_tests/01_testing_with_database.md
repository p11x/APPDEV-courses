<!-- FILE: 09_testing/03_integration_tests/01_testing_with_database.md -->

## Overview

Integration tests with databases ensure that your application works correctly with a real database. This file covers setting up a test database, running migrations, and testing full application flows.

## Prerequisites

- Flask application with database
- Testing setup (pytest, test client)

## Core Concepts

### Test Database

Use a separate database for testing (often SQLite in-memory) to avoid affecting development or production data.

### Migrations in Tests

For applications using Flask-Migrate, you may need to run migrations in your test setup.

## Code Walkthrough

### Setting Up Test Database

```python
# tests/conftest.py
import pytest
from app import app, db
from flask import Flask

@pytest.fixture
def app():
    """Create and configure a new app instance for each test."""
    app = Flask(__name__)
    app.config['TESTING'] = True
    app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///:memory:'
    app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
    app.config['SECRET_KEY'] = 'test-secret-key'
    
    with app.app_context():
        db.init_app(app)
        db.create_all()
        yield app
        db.drop_all()

@pytest.fixture
def client(app):
    """A test client for the app."""
    return app.test_client()

@pytest.fixture
def runner(app):
    """A test runner for the app's Click commands."""
    return app.test_cli_runner()
```

### Testing with Migrations

```python
# tests/conftest.py (with migrations)
import pytest
from app import app, db
from flask_migrate import upgrade, init, migrate
import os
import tempfile

@pytest.fixture
def app():
    """Create and configure a new app instance for each test."""
    # Create a temporary directory for migration scripts
    with tempfile.TemporaryDirectory() as tmpdir:
        app = Flask(__name__)
        app.config['TESTING'] = True
        app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///:memory:'
        app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
        app.config['SECRET_KEY'] = 'test-secret-key'
        app.config['MIGRATIONS_DIR'] = os.path.join(tmpdir, 'migrations')
        
        with app.app_context():
            db.init_app(app)
            # Initialize migrations
            init()
            # Create initial migration
            migrate(message='Initial migration')
            # Apply migration
            upgrade()
            yield app
            db.drop_all()
```

### Integration Test Example

```python
# tests/test_integration.py
def test_user_registration_flow(client):
    """Test the complete user registration flow."""
    # Get the registration page
    response = client.get('/register')
    assert response.status_code == 200
    assert b'Register' in response.data
    
    # Submit registration form
    response = client.post('/register', data={
        'username': 'newuser',
        'email': 'new@example.com',
        'password': 'secure123',
        'confirm_password': 'secure123'
    }, follow_redirects=True)
    
    # Should redirect to login or dashboard
    assert response.status_code == 200
    assert b'Login' in response.data or b'Dashboard' in response.data
    
    # Verify user was created in database
    with client.application.app_context():
        from models import User
        user = User.query.filter_by(username='newuser').first()
        assert user is not None
        assert user.email == 'new@example.com'
        assert user.check_password('secure123')

def test_login_logout_flow(client):
    """Test login, accessing protected route, and logout."""
    # First create a user
    with client.application.app_context():
        from models import User
        user = User(username='testuser', email='test@example.com')
        user.set_password('password123')
        db.session.add(user)
        db.session.commit()
    
    # Login
    response = client.post('/login', data={
        'username': 'testuser',
        'password': 'password123'
    }, follow_redirects=True)
    assert response.status_code == 200
    assert b'Dashboard' in response.data or b'Profile' in response.data
    
    # Access protected route
    response = client.get('/profile')
    assert response.status_code == 200
    assert b'testuser' in response.data
    
    # Logout
    response = client.get('/logout', follow_redirects=True)
    assert response.status_code == 200
    assert b'Login' in response.data
    
    # Try to access protected route after logout
    response = client.get('/profile')
    # Should redirect to login or show error
    assert response.status_code in [302, 401, 403]
```

### Testing API Endpoints with Database

```python
def test_api_user_creation(client):
    """Test creating a user via API endpoint."""
    response = client.post('/api/users', 
        json={'username': 'apiuser', 'email': 'api@example.com'})
    assert response.status_code == 201
    data = response.get_json()
    assert data['username'] == 'apiuser'
    assert data['email'] == 'api@example.com'
    
    # Verify in database
    with client.application.app_context():
        from models import User
        user = User.query.filter_by(username='apiuser').first()
        assert user is not None
        assert user.email == 'api@example.com'
```

## Common Mistakes

❌ **Using production database for tests**
```python
# WRONG — Tests might corrupt production data
app.config['SQLALCHEMY_DATABASE_URI'] = 'postgresql://user:pass@localhost/prod'
```

✅ **Correct — Use test database**
```python
# CORRECT
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///:memory:'
```

❌ **Not initializing database in tests**
```python
# WRONG — Database tables don't exist
# Always call db.create_all() in test setup
```

✅ **Correct — Initialize database**
```python
# CORRECT
with app.app_context():
    db.create_all()
```

## Quick Reference

| Fixture | Description |
|---------|-------------|
| `app` | Configured Flask application |
| `client` | Test client for making requests |
| `runner` | Test runner for CLI commands |

## Next Steps

Now you can test with databases. Continue to [02_testing_auth_flows.md](02_testing_auth_flows.md) to learn about testing authentication flows.