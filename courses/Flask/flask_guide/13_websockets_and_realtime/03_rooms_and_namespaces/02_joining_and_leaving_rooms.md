<!-- FILE: 13_websockets_and_realtime/03_rooms_and_namespaces/02_joining_and_leaving_rooms.md -->

## Overview

This file covers joining and leaving rooms in Flask-SocketIO.

## Code Walkthrough

```python
# room_handlers.py
from flask_socketio import join_room, leave_room, emit, socketio

@socketio.on("join_room")
def handle_join(data):
    room = data["room"]
    username = data["username"]
    join_room(room)
    emit("status", f"{username} joined", room=room)

@socketio.on("leave_room")
def handle_leave(data):
    room = data["room"]
    username = data["username"]
    leave_room(room)
    emit("status", f"{username} left", room=room)
```
