<!-- FILE: 09_testing/03_integration_tests/02_testing_auth_flows.md -->

## Overview

Testing authentication flows ensures that your login, logout, and session management work correctly. This file covers testing user registration, login, logout, and protected routes.

## Prerequisites

- Flask application with authentication (Flask-Login or similar)
- Testing setup with test client and database

## Core Concepts

### What to Test in Authentication

- User registration with valid and invalid data
- Login with correct and incorrect credentials
- Session persistence after login
- Access to protected routes
- Logout functionality
- Remember-me functionality (if implemented)

## Code Walkthrough

### Testing User Registration

```python
# tests/test_auth.py
import pytest
from app import app, db
from models import User

@pytest.fixture
def client():
    app.config['TESTING'] = True
    app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///:memory:'
    with app.test_client() as client:
        with app.app_context():
            db.create_all()
            yield client
            db.drop_all()

def test_registration_success(client):
    """Test successful user registration."""
    response = client.post('/register', data={
        'username': 'newuser',
        'email': 'new@example.com',
        'password': 'secure123',
        'confirm_password': 'secure123'
    }, follow_redirects=True)
    
    assert response.status_code == 200
    assert b'Login' in response.data or b'Dashboard' in response.data
    
    # Verify user in database
    with client.application.app_context():
        user = User.query.filter_by(username='newuser').first()
        assert user is not None
        assert user.email == 'new@example.com'
        assert user.check_password('secure123')

def test_registration_duplicate_username(client):
    """Test registration with duplicate username."""
    # Create first user
    with client.application.app_context():
        user = User(username='duplicate', email='dup@example.com')
        user.set_password('password')
        db.session.add(user)
        db.session.commit()
    
    # Try to register with same username
    response = client.post('/register', data={
        'username': 'duplicate',
        'email': 'dup2@example.com',
        'password': 'password',
        'confirm_password': 'password'
    })
    
    assert response.status_code == 200  # Form re-rendered
    assert b'Username already exists' in response.data or b'taken' in response.data

def test_registration_password_mismatch(client):
    """Test registration with mismatched passwords."""
    response = client.post('/register', data={
        'username': 'testuser',
        'email': 'test@example.com',
        'password': 'password123',
        'confirm_password': 'different'
    })
    
    assert response.status_code == 200
    assert b'Passwords must match' in response.data or b'do not match' in response.data
```

### Testing Login and Logout

```python
def test_login_success(client):
    """Test successful login."""
    # Create user first
    with client.application.app_context():
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
    assert b'Logged in' in response.data or b'Welcome' in response.data

def test_login_invalid_credentials(client):
    """Test login with incorrect credentials."""
    # Create user
    with client.application.app_context():
        user = User(username='testuser', email='test@example.com')
        user.set_password('password123')
        db.session.add(user)
        db.session.commit()
    
    # Wrong password
    response = client.post('/login', data={
        'username': 'testuser',
        'password': 'wrong'
    })
    
    assert response.status_code == 200
    assert b'Invalid' in response.data or b'incorrect' in response.data
    
    # Non-existent user
    response = client.post('/login', data={
        'username': 'nonexistent',
        'password': 'password'
    })
    
    assert response.status_code == 200
    assert b'Invalid' in response.data or b'incorrect' in response.data

def test_logout(client):
    """Test logout functionality."""
    # Create and login user
    with client.application.app_context():
        user = User(username='testuser', email='test@example.com')
        user.set_password('password123')
        db.session.add(user)
        db.session.commit()
    
    client.post('/login', data={
        'username': 'testuser',
        'password': 'password123'
    })
    
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

### Testing Protected Routes

```python
def test_protected_route_requires_login(client):
    """Test that protected routes redirect to login."""
    response = client.get('/dashboard')
    # Should redirect to login page
    assert response.status_code in [302, 401, 403]
    if response.status_code == 302:
        assert '/login' in response.headers.get('Location', '')

def test_protected_route_after_login(client):
    """Test accessing protected route after login."""
    # Create user
    with client.application.app_context():
        user = User(username='testuser', email='test@example.com')
        user.set_password('password123')
        db.session.add(user)
        db.session.commit()
    
    # Login
    client.post('/login', data={
        'username': 'testuser',
        'password': 'password123'
    })
    
    # Access protected route
    response = client.get('/dashboard')
    assert response.status_code == 200
    assert b'testuser' in response.data or b'Dashboard' in response.data

def test_session_persistence(client):
    """Test that session persists across requests."""
    # Create user
    with client.application.app_context():
        user = User(username='testuser', email='test@example.com')
        user.set_password('password123')
        db.session.add(user)
        db.session.commit()
    
    # Login
    client.post('/login', data={
        'username': 'testuser',
        'password': 'password123'
    })
    
    # Make multiple requests to protected routes
    for _ in range(3):
        response = client.get('/profile')
        assert response.status_code == 200
        assert b'testuser' in response.data
```

### Testing Remember-Me Functionality (if implemented)

```python
def test_remember_me(client):
    """Test remember-me functionality."""
    # Create user
    with client.application.app_context():
        user = User(username='testuser', email='test@example.com')
        user.set_password('password123')
        db.session.add(user)
        db.session.commit()
    
    # Login with remember me
    response = client.post('/login', data={
        'username': 'testuser',
        'password': 'password123',
        'remember': 'y'  # or 'on' depending on form
    }, follow_redirects=True)
    
    assert response.status_code == 200
    
    # Close the client (simulate browser closing)
    # In a real test, we would create a new client instance
    # but for simplicity, we'll just check that the session cookie is set
    # Note: Testing remember-me fully requires checking cookies, which is more complex
    # This is a simplified version
```

## Common Mistakes

❌ **Not cleaning up database between tests**
```python
# WRONG — Test data persists
# Always use fixtures to reset database
```

✅ **Correct — Use fixtures for clean state**
```python
# CORRECT
@pytest.fixture
def client():
    # Setup
    yield client
    # Teardown happens in fixture
```

❌ **Testing passwords in plain text**
```python
# WRONG — Never test or log plain text passwords in tests
# Always work with hashed versions or use test-specific passwords
```

✅ **Correct — Use test-specific passwords**
```python
# CORRECT
password = 'test-password-123'  # Only used in test environment
```

## Quick Reference

| Test | Description |
|------|-------------|
| Registration | Test user creation with valid/invalid data |
| Login | Test authentication with correct/incorrect credentials |
| Logout | Test session termination |
| Protected routes | Test access control |
| Session persistence | Test login state across requests |

## Next Steps

Now you can test authentication flows. Continue to [03_coverage_reports.md](03_coverage_reports.md) to learn about generating test coverage reports.