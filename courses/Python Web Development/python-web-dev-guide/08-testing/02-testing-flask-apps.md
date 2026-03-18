# Testing Flask Apps

## What You'll Learn
- Testing Flask applications
- Using Flask test client

## Prerequisites
- Completed Flask basics

## Flask Test Client

```python
import pytest
from app import app

@pytest.fixture
def client():
    app.config['TESTING'] = True
    with app.test_client() as client:
        yield client

def test_home(client):
    response = client.get('/')
    assert response.status_code == 200

def test_create_user(client):
    response = client.post('/users', json={
        'username': 'test',
        'email': 'test@example.com'
    })
    assert response.status_code == 201
```
