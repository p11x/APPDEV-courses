<!-- FILE: 13_websockets_and_realtime/02_flask_socketio_basics/01_installing_flask_socketio.md -->

## Overview

Flask-SocketIO adds WebSocket support to Flask using Socket.IO. This file covers installation.

## Installation

```bash
pip install flask-socketio
```

## Code Walkthrough

```python
# app.py
from flask import Flask
from flask_socketio import SocketIO

app = Flask(__name__)
socketio = SocketIO(app)

@socketio.on("message")
def handle_message(data):
    print(f"Received: {data}")
    socketio.send("Hello!")

if __name__ == "__main__":
    socketio.run(app, debug=True)
```
