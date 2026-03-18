# Testing FastAPI Apps

## What You'll Learn
- Testing FastAPI with TestClient
- Testing async endpoints

## Prerequisites
- Completed FastAPI basics

## FastAPI TestClient

```python
from fastapi.testclient import TestClient
from main import app

client = TestClient(app)

def test_read_root():
    response = client.get("/")
    assert response.status_code == 200
    assert response.json() == {"message": "Hello!"}

def test_create_item():
    response = client.post("/items/", json={
        "name": "Test",
        "price": 10.0
    })
    assert response.status_code == 201
    data = response.json()
    assert data["name"] == "Test"
```

## Summary
- Use `TestClient` from FastAPI
- Test routes by making requests
- Check response status and content
