<!-- FILE: 13_websockets_and_realtime/02_flask_socketio_basics/02_server_side_events.md -->

## Overview

This file covers server-side events in Flask-SocketIO.

## Code Walkthrough

```python
# events.py
from flask import Flask
from flask_socketio import SocketIO, emit

app = Flask(__name__)
socketio = SocketIO(app)

# Event handlers
@socketio.on("connect")
def handle_connect():
    print("Client connected")
    emit("response", {"message": "Welcome!"})

@socketio.on("disconnect")
def handle_disconnect():
    print("Client disconnected")

@socketio.on("my_event")
def handle_my_event(data):
    print(f"Received: {data}")
    emit("my_response", {"message": "Got your message!"})

@socketio.on("broadcast_event")
def handle_broadcast(data):
    # Broadcast to all clients
    socketio.emit("broadcast", data)

if __name__ == "__main__":
    socketio.run(app)
```
