<!-- FILE: 09_testing/01_testing_basics/03_flask_test_client.md -->

## Overview

Flask provides a **test client** that simulates requests to your application without running a live server. This is essential for testing routes, views, and API endpoints in isolation.

## Core Concepts

### Test Client

The test client is a wrapper around the Werkzeug test client that allows you to:
- Simulate GET, POST, PUT, DELETE, etc. requests
- Access response data, status codes, and headers
- Test authentication and sessions
- Test file uploads

## Code Walkthrough

### Basic Test Client Usage

```python
# tests/test_basic.py
import pytest
from app import app

@pytest.fixture
def client():
    app.config['TESTING'] = True
    app.config['SECRET_KEY'] = 'test-secret'
    with app.test_client() as client:
        yield client

def test_home_page(client):
    """Test the home page returns 200 OK."""
    response = client.get('/')
    assert response.status_code == 200
    assert b'Welcome' in response.data

def test_about_page(client):
    """Test the about page."""
    response = client.get('/about')
    assert response.status_code == 200
    assert b'About' in response.data
```

### Testing with Sessions

```python
def test_login(client):
    """Test login functionality."""
    # Get login page
    response = client.get('/login')
    assert response.status_code == 200
    
    # Submit login form
    response = client.post('/login', data={
        'username': 'alice',
        'password': 'secret'
    }, follow_redirects=True)
    
    assert response.status_code == 200
    assert b'Logged in' in response.data
```

### Testing API Endpoints

```python
def test_create_user(client):
    """Test creating a user via API."""
    response = client.post('/api/users', 
        json={'name': 'Alice', 'email': 'alice@example.com'})
    assert response.status_code == 201
    data = response.get_json()
    assert data['name'] == 'Alice'
    assert data['email'] == 'alice@example.com'
```

### Testing File Uploads

```python
def test_upload_file(client):
    """Test file upload endpoint."""
    data = {
        'file': (io.BytesIO(b'test file content'), 'test.txt')
    }
    response = client.post('/upload', data=data, content_type='multipart/form-data')
    assert response.status_code == 200
    assert b'uploaded' in response.data
```

## Next Steps

Now you can use the test client. Continue to [01_testing_routes.md](../02_unit_tests/01_testing_routes.md) to learn about unit testing routes.