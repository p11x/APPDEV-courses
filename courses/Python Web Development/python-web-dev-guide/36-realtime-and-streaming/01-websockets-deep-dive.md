# WebSockets Deep Dive

## What You'll Learn
- WebSocket protocol internals
- Connection lifecycle management
- Reconnection strategies
- Performance optimization
- Production deployment considerations

## Prerequisites
- Completed basic WebSocket tutorials
- Understanding of HTTP protocol
- Python async/await knowledge

## WebSocket Protocol Overview

WebSocket is a **full-duplex** communication protocol over a single TCP connection:

```
HTTP:      Request ──▶ Response ──▶ Close
WebSocket: Connect ──▶ Open ──▶ Data Exchange ──▶ Close
             │                                    │
             └──────── Persistent ───────────────┘
```

### Handshake

```
Client ──────────────────────────────────────────────▶ Server

GET /ws HTTP/1.1
Host: example.com
Upgrade: websocket
Connection: Upgrade
Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==
Sec-WebSocket-Version: 13

                                    Server ──────▶ Client

HTTP/1.1 101 Switching Protocols
Upgrade: websocket
Connection: Upgrade
Sec-WebSocket-Accept: s3pPLMBiTxaQ9kYGzzhZRbK+xOo=
```

## Connection Management

### Connection Pool Pattern

```python
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
from typing import Dict, Set
import asyncio

class ConnectionManager:
    """Manage WebSocket connections."""
    
    def __init__(self):
        # Active connections by user ID
        self.active_connections: Dict[int, Set[WebSocket]] = {}
        # Lock for thread-safe operations
        self._lock = asyncio.Lock()
    
    async def connect(self, user_id: int, websocket: WebSocket) -> None:
        """Add new WebSocket connection."""
        await websocket.accept()
        
        async with self._lock:
            if user_id not in self.active_connections:
                self.active_connections[user_id] = set()
            self.active_connections[user_id].add(websocket)
    
    async def disconnect(self, user_id: int, websocket: WebSocket) -> None:
        """Remove WebSocket connection."""
        
        async with self._lock:
            if user_id in self.active_connections:
                self.active_connections[user_id].discard(websocket)
                
                # Clean up empty sets
                if not self.active_connections[user_id]:
                    del self.active_connections[user_id]
    
    async def send_to_user(self, user_id: int, message: str) -> bool:
        """Send message to specific user."""
        
        async with self._lock:
            connections = self.active_connections.get(user_id, set())
        
        if not connections:
            return False
        
        # Send to all connections for this user
        disconnected = set()
        
        for ws in connections:
            try:
                await ws.send_text(message)
            except Exception:
                disconnected.add(ws)
        
        # Clean up disconnected
        for ws in disconnected:
            await self.disconnect(user_id, ws)
        
        return True
    
    async def broadcast(self, message: str) -> None:
        """Broadcast message to all connected users."""
        
        all_connections = []
        
        async with self._lock:
            for connections in self.active_connections.values():
                all_connections.extend(connections)
        
        for ws in all_connections:
            try:
                await ws.send_text(message)
            except Exception:
                pass  # Log error in production

manager = ConnectionManager()
```

🔍 **Line-by-Line Breakdown:**
1. `Dict[int, Set[WebSocket]]` — Maps user IDs to their WebSocket connections
2. `asyncio.Lock()` — Prevents race conditions when modifying connections
3. `websocket.accept()` — Accepts and upgrades the connection
4. `discard()` — Removes without raising error if not present

### Ping/Pong for Keep-Alive

```python
import asyncio

class PingPongManager:
    """Manage ping/pong for connection health."""
    
    PING_INTERVAL = 30  # seconds
    
    async def handle_connection(self, websocket: WebSocket, user_id: int) -> None:
        """Handle WebSocket with ping/pong."""
        
        try:
            while True:
                # Send ping
                await websocket.send_text("ping")
                
                # Wait for pong (with timeout)
                try:
                    message = await asyncio.wait_for(
                        websocket.receive_text(),
                        timeout=self.PING_INTERVAL
                    )
                    
                    if message != "pong":
                        break  # Invalid response
                        
                except asyncio.TimeoutError:
                    # No response, close connection
                    break
                
                # Wait before next ping
                await asyncio.sleep(self.PING_INTERVAL)
                
        except Exception:
            pass  # Connection closed
        finally:
            await manager.disconnect(user_id, websocket)
```

## Reconnection Strategies

### Client-Side Reconnection

```javascript
class WebSocketClient {
    constructor(url) {
        this.url = url;
        this.reconnectDelay = 1000;
        this.maxReconnectDelay = 30000;
        this.connect();
    }
    
    connect() {
        this.ws = new WebSocket(this.url);
        
        this.ws.onopen = () => {
            console.log("Connected!");
            this.reconnectDelay = 1000; // Reset delay
        };
        
        this.ws.onmessage = (event) => {
            this.handleMessage(event.data);
        };
        
        this.ws.onclose = () => {
            console.log("Disconnected, reconnecting...");
            this.scheduleReconnect();
        };
        
        this.ws.onerror = (error) => {
            console.error("WebSocket error:", error);
        };
    }
    
    scheduleReconnect() {
        setTimeout(() => {
            console.log(`Reconnecting in ${this.reconnectDelay}ms...`);
            this.connect();
            
            // Exponential backoff
            this.reconnectDelay = Math.min(
                this.reconnectDelay * 2,
                this.maxReconnectDelay
            );
        }, this.reconnectDelay);
    }
    
    handleMessage(data) {
        console.log("Received:", data);
    }
}

// Usage
const client = new WebSocketClient("ws://localhost:8000/ws");
```

### Server-Side Heartbeat

```python
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
import asyncio

class HeartbeatWebSocket:
    """WebSocket with heartbeat monitoring."""
    
    def __init__(self, websocket: WebSocket):
        self.ws = websocket
        self.last_pong = asyncio.get_event_loop().time()
        self._running = True
    
    async def send_heartbeat(self) -> None:
        """Send periodic heartbeats."""
        
        while self._running:
            try:
                await asyncio.sleep(30)  # Heartbeat every 30s
                
                # Try to send heartbeat
                await self.ws.send_json({"type": "heartbeat"})
                self.last_pong = asyncio.get_event_loop().time()
                
            except Exception:
                break
    
    async def receive_with_heartbeat(self) -> str:
        """Receive message with heartbeat monitoring."""
        
        while self._running:
            try:
                # Wait for message or heartbeat
                message = await self.ws.receive_text()
                return message
                
            except Exception:
                break
        
        raise WebSocketDisconnect()
```

## Message Patterns

### Request-Response over WebSocket

```python
# Server: Request-Response pattern
class RequestResponseHandler:
    """Handle request-response over WebSocket."""
    
    def __init__(self, websocket: WebSocket):
        self.ws = websocket
        self.pending: dict[str, asyncio.Future] = {}
    
    async def handle_message(self, message: str) -> None:
        """Process incoming message."""
        import json
        
        data = json.loads(message)
        msg_type = data.get("type")
        request_id = data.get("id")
        
        if msg_type == "request":
            # Process request
            response = await self.process_request(data)
            
            # Send response with same ID
            await self.ws.send_json({
                "type": "response",
                "id": request_id,
                "data": response
            })
        
        elif msg_type == "response":
            # Complete pending future
            if request_id in self.pending:
                self.pending[request_id].set_result(data.get("data"))
    
    async def call(self, method: str, params: dict) -> dict:
        """Send request and wait for response."""
        
        import uuid
        
        request_id = str(uuid.uuid4())
        future = asyncio.Future()
        self.pending[request_id] = future
        
        # Send request
        await self.ws.send_json({
            "type": "request",
            "id": request_id,
            "method": method,
            "params": params
        })
        
        # Wait for response
        return await future
    
    async def process_request(self, data: dict) -> dict:
        """Process request and return result."""
        
        method = data.get("method")
        
        match method:
            case "get_user":
                return {"name": "John", "id": 1}
            case "get_items":
                return [{"id": 1}, {"id": 2}]
            case _:
                return {"error": "Unknown method"}
```

### Pub/Sub Pattern

```python
from fastapi import FastAPI, WebSocket
import asyncio
import json
from typing import Dict, Set

class PubSubManager:
    """Pub/Sub over WebSocket."""
    
    def __init__(self):
        # topic -> set of websockets
        self.subscriptions: Dict[str, Set[WebSocket]] = {}
        self.user_subscriptions: Dict[WebSocket, Set[str]] = {}
    
    async def subscribe(self, websocket: WebSocket, topic: str) -> None:
        """Subscribe to a topic."""
        
        if topic not in self.subscriptions:
            self.subscriptions[topic] = set()
        
        self.subscriptions[topic].add(websocket)
        
        if websocket not in self.user_subscriptions:
            self.user_subscriptions[websocket] = set()
        
        self.user_subscriptions[websocket].add(topic)
        
        await websocket.send_json({
            "type": "subscribed",
            "topic": topic
        })
    
    async def unsubscribe(self, websocket: WebSocket, topic: str) -> None:
        """Unsubscribe from topic."""
        
        if topic in self.subscriptions:
            self.subscriptions[topic].discard(websocket)
        
        if websocket in self.user_subscriptions:
            self.user_subscriptions[websocket].discard(topic)
    
    async def publish(self, topic: str, message: dict) -> None:
        """Publish message to topic subscribers."""
        
        if topic not in self.subscriptions:
            return
        
        payload = json.dumps({
            "type": "message",
            "topic": topic,
            "data": message
        })
        
        for ws in self.subscriptions[topic]:
            try:
                await ws.send_text(payload)
            except Exception:
                # Clean up dead connections
                await self.unsubscribe(ws, topic)

# Usage in FastAPI
pubsub = PubSubManager()

@app.websocket("/ws/pubsub")
async def pubsub_websocket(websocket: WebSocket):
    """WebSocket with pub/sub support."""
    
    await websocket.accept()
    
    try:
        while True:
            message = await websocket.receive_text()
            data = json.loads(message)
            
            match data.get("type"):
                case "subscribe":
                    await pubsub.subscribe(websocket, data["topic"])
                
                case "unsubscribe":
                    await pubsub.unsubscribe(websocket, data["topic"])
                
                case "publish":
                    await pubsub.publish(data["topic"], data["message"])
                    
    except WebSocketDisconnect:
        # Clean up subscriptions
        if websocket in pubsub.user_subscriptions:
            for topic in pubsub.user_subscriptions[websocket]:
                await pubsub.unsubscribe(websocket, topic)
```

## Production Considerations

- **SSL/TLS**: Always use wss:// (WebSocket Secure)
- **Load balancing**: Configure sticky sessions or Redis for multi-server
- **Connection limits**: Set max connections per user/IP
- **Timeouts**: Configure appropriate timeouts
- **Monitoring**: Track connection counts and message throughput

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: No error handling

**Wrong:**
```python
async def ws_endpoint(ws: WebSocket):
    await ws.accept()
    while True:
        data = await ws.receive_text()  # No error handling!
        await ws.send_text(f"Echo: {data}")
```

**Why it fails:** Connection errors crash the endpoint.

**Fix:**
```python
async def ws_endpoint(ws: WebSocket):
    await ws.accept()
    try:
        while True:
            data = await ws.receive_text()
            await ws.send_text(f"Echo: {data}")
    except WebSocketDisconnect:
        print("Client disconnected")
```

### ❌ Mistake 2: Not cleaning up on disconnect

**Wrong:**
```python
# Never removes connection from storage!
await manager.connect(user_id, ws)
```

**Why it fails:** Memory leak, stale connections.

**Fix:**
```python
try:
    while True:
        data = await ws.receive_text()
except WebSocketDisconnect:
    await manager.disconnect(user_id, ws)
```

### ❌ Mistake 3: Blocking operations in WebSocket

**Wrong:**
```python
async def handle(ws: WebSocket):
    # Blocking call!
    time.sleep(10)
    await ws.send_text("done")
```

**Why it fails:** Blocks event loop.

**Fix:**
```python
async def handle(ws: WebSocket):
    # Async sleep
    await asyncio.sleep(10)
    await ws.send_text("done")
```

## Summary

- WebSocket provides persistent bidirectional communication
- Implement connection management with connection pools
- Use ping/pong for connection health monitoring
- Handle reconnection with exponential backoff on client
- Use pub/sub patterns for scalable messaging

## Next Steps

→ Continue to `02-server-sent-events.md` to learn about Server-Sent Events for real-time updates.
