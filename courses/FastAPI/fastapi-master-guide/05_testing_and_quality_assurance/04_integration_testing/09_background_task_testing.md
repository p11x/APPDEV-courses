# Background Task Testing

## Overview

Testing background tasks ensures asynchronous operations execute correctly.

## Testing Background Tasks

### FastAPI Background Tasks

```python
# Example 1: Test background tasks
from fastapi import FastAPI, BackgroundTasks
from fastapi.testclient import TestClient
from unittest.mock import Mock

app = FastAPI()

def send_email(email: str, message: str):
    """Background email sending"""
    pass

@app.post("/register/")
async def register(email: str, background_tasks: BackgroundTasks):
    background_tasks.add_task(send_email, email, "Welcome!")
    return {"message": "Registered"}

# Test
def test_background_task():
    """Test that background task is scheduled"""
    client = TestClient(app)

    with patch('app.main.send_email') as mock_send:
        response = client.post("/register/", params={"email": "test@example.com"})

        assert response.status_code == 200
        # Note: Background tasks run after response
```

### Celery Task Testing

```python
# Example 2: Test Celery tasks
from celery import Celery
from unittest.mock import patch

celery_app = Celery('tasks')

@celery_app.task
def process_order(order_id: int):
    """Process order asynchronously"""
    pass

def test_celery_task():
    """Test Celery task execution"""
    with patch('app.tasks.process_order.delay') as mock_delay:
        process_order.delay(1)
        mock_delay.assert_called_once_with(1)
```

## Summary

Background task testing ensures async operations work correctly.

## Next Steps

Continue learning about:
- [Event System Testing](./10_event_system_testing.md)
- [Integration Testing](./08_integration_testing.md)
