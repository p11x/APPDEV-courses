<!-- FILE: 09_testing/02_unit_tests/01_testing_routes.md -->

## Overview

Testing routes ensures that your application responds correctly to HTTP requests. This file covers testing different route types: GET, POST, dynamic routes, and error handling.

## Prerequisites

- Flask test client knowledge
- Basic testing setup

## Code Walkthrough

### Testing Different Route Types

```python
# tests/test_routes.py
import pytest
import json
from app import app, db
from models import User, Post

@pytest.fixture
def client():
    app.config['TESTING'] = True
    app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///:memory:'
    with app.test_client() as client:
        with app.app_context():
            db.create_all()
            yield client
            db.drop_all()

def test_home_route(client):
    """Test the home page route."""
    response = client.get('/')
    assert response.status_code == 200
    assert b'Welcome' in response.data

def test_about_route(client):
    """Test the about page route."""
    response = client.get('/about')
    assert response.status_code == 200
    assert b'About' in response.data

def test_dynamic_route(client):
    """Test dynamic route with parameter."""
    response = client.get('/user/alice')
    assert response.status_code == 200
    assert b'alice' in response.data

def test_dynamic_route_not_found(client):
    """Test dynamic route with invalid parameter."""
    response = client.get('/user/nonexistent')
    # Depending on implementation, might return 404 or empty
    assert response.status_code in [200, 404]

def test_post_route(client):
    """Test POST route for creating data."""
    response = client.post('/users', 
        json={'username': 'testuser', 'email': 'test@example.com'})
    assert response.status_code == 201
    data = json.loads(response.data)
    assert data['username'] == 'testuser'

def test_put_route(client):
    """Test PUT route for updating data."""
    # First create a user
    post_resp = client.post('/users', 
        json={'username': 'testuser', 'email': 'test@example.com'})
    user_id = json.loads(post_resp.data)['id']
    
    # Then update it
    put_resp = client.put(f'/users/{user_id}', 
        json={'username': 'updateduser'})
    assert put_resp.status_code == 200
    data = json.loads(put_resp.data)
    assert data['username'] == 'updateduser'

def test_delete_route(client):
    """Test DELETE route for removing data."""
    # First create a user
    post_resp = client.post('/users', 
        json={'username': 'testuser', 'email': 'test@example.com'})
    user_id = json.loads(post_resp.data)['id']
    
    # Then delete it
    delete_resp = client.delete(f'/users/{user_id}')
    assert delete_resp.status_code == 204
    
    # Verify it's gone
    get_resp = client.get(f'/users/{user_id}')
    assert get_resp.status_code == 404

def test_404_error(client):
    """Test 404 error handling."""
    response = client.get('/nonexistent-page')
    assert response.status_code == 404
    assert b'Not Found' in response.data or b'404' in response.data

def test_redirect(client):
    """Test redirect functionality."""
    response = client.get('/old-url')
    # Should redirect to new location
    assert response.status_code in [200, 301, 302]
    if response.status_code in [301, 302]:
        assert '/new-url' in response.headers.get('Location', '')
```

### Testing with Database

```python
def test_user_creation_db(client):
    """Test that user is actually saved to database."""
    response = client.post('/users', 
        json={'username': 'dbuser', 'email': 'db@example.com'})
    assert response.status_code == 201
    
    # Check database directly
    with app.app_context():
        user = User.query.filter_by(username='dbuser').first()
        assert user is not None
        assert user.email == 'db@example.com'
```

## Common Mistakes

❌ **Not setting up test database**
```python
# WRONG — Tests might affect production data
# Always use separate test database
```

✅ **Correct — Use in-memory SQLite for tests**
```python
# CORRECT
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///:memory:'
```

❌ **Not cleaning up between tests**
```python
# WRONG — Data persists between tests
# Always use fixtures to reset state
```

✅ **Correct — Use fixtures for setup/teardown**
```python
# CORRECT
@pytest.fixture
def client():
    # Setup
    yield client
    # Teardown
```

## Quick Reference

| Method | Description |
|--------|-------------|
| `client.get('/')` | GET request |
| `client.post('/')` | POST request |
| `client.put('/')` | PUT request |
| `client.delete('/')` | DELETE request |
| `follow_redirects=True` | Follow redirects |
| `json={'key': 'value'}` | Send JSON data |

## Next Steps

Now you can test routes. Continue to [02_testing_models.md](02_testing_models.md) to learn about testing database models.