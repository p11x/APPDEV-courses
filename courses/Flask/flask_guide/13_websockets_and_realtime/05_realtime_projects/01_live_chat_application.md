<!-- FILE: 13_websockets_and_realtime/05_realtime_projects/01_live_chat_application.md -->

## Overview

Build a live chat application using Flask-SocketIO.

## Code Walkthrough

```python
# chat_app.py
from flask import Flask, render_template
from flask_socketio import SocketIO, send, emit

app = Flask(__name__)
socketio = SocketIO(app)

@app.route("/")
def index():
    return render_template("chat.html")

@socketio.on("message")
def handle_message(msg):
    print(f"Message: {msg}")
    send(msg, broadcast=True)

if __name__ == "__main__":
    socketio.run(app, debug=True)
```

```html
<!-- templates/chat.html -->
<!DOCTYPE html>
<html>
<head>
    <title>Chat</title>
    <script src="https://cdn.socket.io/4.7.2/socket.io.min.js"></script>
</head>
<body>
    <div id="chat"></div>
    <input id="msg"><button onclick="send()">Send</button>
    <script>
        const socket = io();
        socket.on("message", (msg) => {
            document.getElementById("chat").innerHTML += "<p>" + msg + "</p>";
        });
        function send() {
            socket.emit("message", document.getElementById("msg").value);
        }
    </script>
</body>
</html>
```
