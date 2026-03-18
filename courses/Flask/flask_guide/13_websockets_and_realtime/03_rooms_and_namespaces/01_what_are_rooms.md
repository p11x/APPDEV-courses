<!-- FILE: 13_websockets_and_realtime/03_rooms_and_namespaces/01_what_are_rooms.md -->

## Overview

Rooms allow you to organize WebSocket connections into groups for targeted messaging.

## Code Walkthrough

```python
# rooms.py
from flask_socketio import join_room, leave_room, emit

@socketio.on("join")
def on_join(data):
    room = data["room"]
    join_room(room)
    emit("message", f"Joined {room}", room=room)

@socketio.on("leave")
def on_leave(data):
    room = data["room"]
    leave_room(room)
    emit("message", f"Left {room}", room=room)

@socketio.on("room_message")
def handle_room_message(data):
    room = data["room"]
    message = data["message"]
    emit("message", message, room=room)
```
