# WebSockets in FastAPI

## What You'll Learn
- WebSocket fundamentals and how they differ from HTTP
- Implementing WebSocket endpoints in FastAPI
- Bidirectional real-time communication
- Connection management and broadcasting
- Authentication for WebSocket connections

## Prerequisites
- Completed `04-response-streaming.md` — Streaming responses
- Understanding of async/await in Python 3.11+
- Basic JavaScript for frontend WebSocket usage

## HTTP vs WebSockets

**HTTP (Request-Response):**
```
Client ──Request──▶ Server
Client ◀─Response── Server
       (Connection closed)
```

**WebSocket (Bidirectional):**
```
Client ──Connect──▶ Server
Client ◀─Ack──────── Server
       (Connection stays open!)
Client ──Message───▶ Server
Client ◀─Message──── Server
       (Both can send anytime)
```

Think of HTTP as phone calls where each call is separate. WebSocket is a walkie-talkie — once connected, both parties can talk freely.

## Basic WebSocket Endpoint

```python
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
from typing import list

app = FastAPI()

@app.websocket("/ws")
async def websocket_endpoint(websocket: WebSocket):
    """Basic WebSocket echo server."""
    # Accept the connection
    await websocket.accept()
    
    try:
        while True:
            # Receive message from client
            data = await websocket.receive_text()
            
            # Send message back to client
            await websocket.send_text(f"Echo: {data}")
            
    except WebSocketDisconnect:
        # Client disconnected
        print("Client disconnected")
```

🔍 **Line-by-Line Breakdown:**
1. `@app.websocket("/ws")` — Registers a WebSocket endpoint instead of HTTP
2. `websocket: WebSocket` — WebSocket connection object
3. `await websocket.accept()` — Accepts and establishes WebSocket connection
4. `await websocket.receive_text()` — Waits for message from client (blocking call)
5. `await websocket.send_text(...)` — Sends message back to client
6. `WebSocketDisconnect` — Raised when client closes connection

## Connection Manager Pattern

Manage multiple WebSocket connections:

```python
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
from typing import Dict

class ConnectionManager:
    """Manages active WebSocket connections."""
    
    def __init__(self):
        # Map client_id to WebSocket connection
        self.active_connections: Dict[str, WebSocket] = {}
    
    async def connect(self, client_id: str, websocket: WebSocket):
        """Register new WebSocket connection."""
        await websocket.accept()
        self.active_connections[client_id] = websocket
    
    def disconnect(self, client_id: str):
        """Remove WebSocket connection."""
        self.active_connections.pop(client_id, None)
    
    async def send_personal_message(self, message: str, client_id: str):
        """Send message to specific client."""
        if client_id in self.active_connections:
            await self.active_connections[client_id].send_text(message)
    
    async def broadcast(self, message: str):
        """Send message to all connected clients."""
        for connection in self.active_connections.values():
            await connection.send_text(message)

# Global manager instance
manager = ConnectionManager()

@app.websocket("/ws/{client_id}")
async def websocket_chat(websocket: WebSocket, client_id: str):
    """Chat WebSocket endpoint with connection management."""
    await manager.connect(client_id, websocket)
    
    try:
        while True:
            data = await websocket.receive_text()
            # Broadcast to everyone including sender
            await manager.broadcast(f"Client {client_id}: {data}")
            
    except WebSocketDisconnect:
        manager.disconnect(client_id)
        await manager.broadcast(f"Client {client_id} left the chat")
```

## WebSocket with JSON Data

```python
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
import json
from pydantic import BaseModel
from typing import Optional

class Message(BaseModel):
    """Structured message format."""
    type: str
    payload: dict
    sender: Optional[str] = None

@app.websocket("/ws/chat/{room_id}")
async def chat_room(websocket: WebSocket, room_id: str):
    """WebSocket chat room with JSON messaging."""
    await websocket.accept()
    
    # Join room
    await join_room(room_id, websocket)
    
    try:
        while True:
            # Receive JSON text
            raw_data = await websocket.receive_text()
            
            # Parse JSON
            try:
                message = json.loads(raw_data)
            except json.JSONDecodeError:
                await websocket.send_json({
                    "error": "Invalid JSON"
                })
                continue
            
            # Handle different message types
            match message.get("type"):
                case "chat":
                    content = message.get("payload", {}).get("content", "")
                    await broadcast_to_room(
                        room_id, 
                        {"type": "chat", "content": content}
                    )
                case "typing":
                    # Just notify others
                    await broadcast_to_room(
                        room_id,
                        {"type": "typing", "user": message.get("sender")}
                    )
                case _:
                    await websocket.send_json({
                        "error": f"Unknown message type: {message.get('type')}"
                    })
                    
    except WebSocketDisconnect:
        await leave_room(room_id, websocket)

# Room management helpers (simplified)
rooms: dict[str, set[WebSocket]] = {}

async def join_room(room_id: str, websocket: WebSocket):
    if room_id not in rooms:
        rooms[room_id] = set()
    rooms[room_id].add(websocket)

async def leave_room(room_id: str, websocket: WebSocket):
    if room_id in rooms:
        rooms[room_id].discard(websocket)

async def broadcast_to_room(room_id: str, message: dict):
    if room_id in rooms:
        for ws in rooms[room_id]:
            await ws.send_json(message)
```

## WebSocket Authentication

```python
from fastapi import FastAPI, WebSocket, HTTPException, Depends
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
import jwt

security = HTTPBearer()

async def get_current_user(
    websocket: WebSocket,
    credentials: HTTPAuthorizationCredentials = Depends(security)
) -> dict:
    """Authenticate WebSocket connection using JWT."""
    try:
        token = credentials.credentials
        payload = jwt.decode(token, "secret", algorithms=["HS256"])
        return payload
    except jwt.PyJWTError:
        raise HTTPException(status_code=401, detail="Invalid token")

@app.websocket("/ws/protected")
async def protected_websocket(
    websocket: WebSocket,
    user: dict = Depends(get_current_user)
):
    """WebSocket endpoint that requires authentication."""
    await websocket.accept()
    
    try:
        await websocket.send_json({
            "type": "welcome",
            "message": f"Welcome, {user['sub']}!"
        })
        
        while True:
            data = await websocket.receive_text()
            await websocket.send_json({
                "type": "echo",
                "message": data
            })
            
    except WebSocketDisconnect:
        print(f"User {user['sub']} disconnected")
```

## Client-Side JavaScript

```javascript
// Connect to WebSocket
const ws = new WebSocket("ws://localhost:8000/ws/chat/general");

// Handle connection open
ws.onopen = () => {
    console.log("Connected to WebSocket!");
    
    // Send a message
    ws.send(JSON.stringify({
        type: "chat",
        payload: { content: "Hello everyone!" },
        sender: "user123"
    }));
};

// Handle incoming messages
ws.onmessage = (event) => {
    const data = JSON.parse(event.data);
    
    switch (data.type) {
        case "chat":
            console.log("Chat:", data.content);
            break;
        case "typing":
            console.log("User is typing...");
            break;
        case "welcome":
            console.log("Server:", data.message);
            break;
    }
};

// Handle errors
ws.onerror = (error) => {
    console.error("WebSocket error:", error);
};

// Handle disconnection
ws.onclose = () => {
    console.log("Disconnected from WebSocket");
};

// Send message function
function sendMessage(content) {
    ws.send(JSON.stringify({
        type: "chat",
        payload: { content }
    }));
}

// Clean up on page unload
window.addEventListener("beforeunload", () => {
    ws.close();
});
```

## Production Considerations

- **Connection limits**: Set max connections to prevent resource exhaustion
- **Heartbeat/ping-pong**: Implement keep-alive to detect dead connections
- **Reconnection logic**: Client should handle reconnection on disconnect
- **SSL/TLS**: Always use wss:// (WebSocket Secure) in production

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not handling disconnection

**Wrong:**
```python
@app.websocket("/ws")
async def ws_endpoint(ws: WebSocket):
    await ws.accept()
    while True:
        data = await ws.receive_text()
        # No exception handling!
```

**Why it fails:** App crashes when client disconnects.

**Fix:**
```python
@app.websocket("/ws")
async def ws_endpoint(ws: WebSocket):
    await ws.accept()
    try:
        while True:
            data = await ws.receive_text()
    except WebSocketDisconnect:
        print("Client disconnected")
```

### ❌ Mistake 2: Blocking in WebSocket handler

**Wrong:**
```python
import time

@app.websocket("/ws")
async def ws_endpoint(ws: WebSocket):
    await ws.accept()
    while True:
        time.sleep(10)  # BLOCKS event loop!
        await ws.send_text("ping")
```

**Why it fails:** Blocks all other async operations in the app.

**Fix:**
```python
import asyncio

@app.websocket("/ws")
async def ws_endpoint(ws: WebSocket):
    await ws.accept()
    while True:
        await asyncio.sleep(10)  # Non-blocking
        await ws.send_text("ping")
```

### ❌ Mistake 3: Not using proper close codes

**Wrong:**
```python
# Just let connection close without proper code
await websocket.close()
```

**Why it fails:** No clear reason for disconnection.

**Fix:**
```python
# Use appropriate close code
# 1000 = normal closure
# 1001 = going away
# 1002 = protocol error
# 1010 = unexpected condition
await websocket.close(code=1000)
```

## Summary

- WebSockets provide bidirectional, persistent connections
- Use `WebSocket.receive_text()` and `WebSocket.send_text()` for messaging
- Handle `WebSocketDisconnect` exception to detect closed connections
- Implement connection managers for multi-client scenarios
- Always authenticate WebSocket connections in production

## Next Steps

→ Continue to `06-background-tasks.md` to learn about running background tasks in FastAPI.
