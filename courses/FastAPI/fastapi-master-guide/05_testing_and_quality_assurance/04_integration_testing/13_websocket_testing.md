# WebSocket Testing

## Overview

Testing WebSocket connections ensures real-time features work correctly.

## WebSocket Tests

### Basic WebSocket Testing

```python
# Example 1: WebSocket test with TestClient
from fastapi.testclient import TestClient

def test_websocket_connection(client: TestClient):
    """Test WebSocket connection"""
    with client.websocket_connect("/ws") as websocket:
        websocket.send_text("hello")
        data = websocket.receive_text()
        assert data == "Echo: hello"

def test_websocket_broadcast(client: TestClient):
    """Test WebSocket broadcast"""
    with client.websocket_connect("/ws") as ws1:
        with client.websocket_connect("/ws") as ws2:
            ws1.send_text("broadcast")
            # Both should receive
            assert ws1.receive_text()
            assert ws2.receive_text()
```

### Testing WebSocket with Auth

```python
# Example 2: Authenticated WebSocket
def test_authenticated_websocket(client: TestClient, auth_token):
    """Test WebSocket with authentication"""
    with client.websocket_connect(f"/ws?token={auth_token}") as websocket:
        websocket.send_text("authenticated message")
        response = websocket.receive_text()
        assert response is not None
```

## Summary

WebSocket testing ensures real-time features work correctly.

## Next Steps

Continue learning about:
- [Background Task Testing](./09_background_task_testing.md)
- [Integration Testing](./08_integration_testing.md)
