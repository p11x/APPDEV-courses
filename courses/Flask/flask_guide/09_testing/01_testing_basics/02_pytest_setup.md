<!-- FILE: 09_testing/01_testing_basics/02_pytest_setup.md -->

## Overview

**pytest** is a popular testing framework for Python. This file covers installing pytest, configuring it for Flask, and writing basic test fixtures.

## Prerequisites

- Basic Flask application
- Understanding of testing concepts

## Core Concepts

### Installation

```bash
pip install pytest
```

### Test Discovery

pytest automatically discovers tests in files named `test_*.py` or `*_test.py`.

## Code Walkthrough

### Basic pytest Configuration

```bash
# Install pytest
pip install pytest
```

### conftest.py for Shared Fixtures

```python
# tests/conftest.py
import pytest
from app import app, db

@pytest.fixture
def client():
    """Create a test client for the app."""
    app.config['TESTING'] = True
    app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///:memory:'
    
    with app.test_client() as client:
        with app.app_context():
            db.create_all()
            yield client
            db.drop_all()
```

### Writing Tests

```python
# tests/test_routes.py
def test_home_page(client):
    """Test the home page."""
    response = client.get('/')
    assert response.status_code == 200
    assert b'Welcome' in response.data

def test_create_user(client):
    """Test creating a user."""
    response = client.post('/users', json={'name': 'Alice'})
    assert response.status_code == 201
    assert b'Alice' in response.data
```

### Running Tests

```bash
# Run all tests
pytest

# Run tests in a specific file
pytest tests/test_routes.py

# Run tests with verbose output
pytest -v
```

## Next Steps

Now you can set up pytest. Continue to [03_flask_test_client.md](03_flask_test_client.md) to learn about Flask's test client.