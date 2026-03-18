<!-- FILE: 13_websockets_and_realtime/02_flask_socketio_basics/03_client_side_socketio.md -->

## Overview

Client-side JavaScript for Socket.IO communication.

## Code Walkthrough

```html
<!-- index.html -->
<!DOCTYPE html>
<html>
<head>
    <title>Socket.IO Demo</title>
    <script src="https://cdn.socket.io/4.7.2/socket.io.min.js"></script>
</head>
<body>
    <h1>Socket.IO Chat</h1>
    <div id="messages"></div>
    <input type="text" id="messageInput">
    <button onclick="sendMessage()">Send</button>
    
    <script>
        const socket = io();
        
        // Connect
        socket.on("connect", () => {
            console.log("Connected!");
        });
        
        // Receive message
        socket.on("message", (data) => {
            const div = document.createElement("div");
            div.textContent = data;
            document.getElementById("messages").appendChild(div);
        });
        
        // Send message
        function sendMessage() {
            const input = document.getElementById("messageInput");
            socket.emit("message", input.value);
            input.value = "";
        }
    </script>
</body>
</html>
```
