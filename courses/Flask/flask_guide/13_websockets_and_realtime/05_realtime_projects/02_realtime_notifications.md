<!-- FILE: 13_websockets_and_realtime/05_realtime_projects/02_realtime_notifications.md -->

## Overview

Build a real-time notification system using Flask-SocketIO.

## Code Walkthrough

```python
# notifications.py
from flask import Flask
from flask_socketio import SocketIO, emit

app = Flask(__name__)
socketio = SocketIO(app)

# Store connected users
connected_users = {}

@socketio.on("connect")
def handle_connect():
    print("Client connected")

@socketio.on("register")
def register_user(data):
    user_id = data["user_id"]
    connected_users[user_id] = request.sid
    emit("registered", {"status": "ok"})

def send_notification(user_id, message):
    if user_id in connected_users:
        emit("notification", {"message": message}, 
             room=connected_users[user_id])

if __name__ == "__main__":
    socketio.run(app)
```
