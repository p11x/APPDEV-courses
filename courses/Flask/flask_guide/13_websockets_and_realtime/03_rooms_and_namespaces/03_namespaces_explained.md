<!-- FILE: 13_websockets_and_realtime/03_rooms_and_namespaces/03_namespaces_explained.md -->

## Overview

Namespaces allow you to split your WebSocket connections into separate communication channels.

## Code Walkthrough

```python
# namespaces.py
from flask_socketio import Namespace, emit

class ChatNamespace(Namespace):
    def on_connect(self, connected, environ):
        emit("message", "Connected to chat")
    
    def on_message(self, data):
        emit("message", data, broadcast=True)

class NotificationsNamespace(Namespace):
    def on_connect(self, connected, environ):
        emit("notification", "Connected to notifications")
    
    def on_subscribe(self, data):
        emit("notification", f"Subscribed to {data}")

# Register namespaces
socketio.on_namespace(ChatNamespace("/chat"))
socketio.on_namespace(NotificationsNamespace("/notifications"))
```
