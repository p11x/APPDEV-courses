# WebSocket Implementation

## Overview

WebSockets enable real-time bidirectional communication between client and server. FastAPI provides native WebSocket support for building real-time features.

## Basic WebSocket

### Simple WebSocket Endpoint

```python
# Example 1: Basic WebSocket in FastAPI
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
from typing import List

app = FastAPI()

class ConnectionManager:
    """Manage WebSocket connections"""

    def __init__(self):
        self.active_connections: List[WebSocket] = []

    async def connect(self, websocket: WebSocket):
        await websocket.accept()
        self.active_connections.append(websocket)

    def disconnect(self, websocket: WebSocket):
        self.active_connections.remove(websocket)

    async def send_personal_message(self, message: str, websocket: WebSocket):
        await websocket.send_text(message)

    async def broadcast(self, message: str):
        for connection in self.active_connections:
            await connection.send_text(message)

manager = ConnectionManager()

@app.websocket("/ws")
async def websocket_endpoint(websocket: WebSocket):
    await manager.connect(websocket)
    try:
        while True:
            data = await websocket.receive_text()
            await manager.send_personal_message(f"Echo: {data}", websocket)
    except WebSocketDisconnect:
        manager.disconnect(websocket)
```

## Chat Application

### Room-Based Chat

```python
# Example 2: Chat with rooms
from fastapi import FastAPI, WebSocket, WebSocketDisconnect, Depends
from typing import Dict, Set
import json

app = FastAPI()

class RoomManager:
    """Manage chat rooms"""

    def __init__(self):
        # room_id -> set of websockets
        self.rooms: Dict[str, Set[WebSocket]] = {}
        # websocket -> user info
        self.users: Dict[WebSocket, dict] = {}

    async def connect(self, websocket: WebSocket, room_id: str, username: str):
        await websocket.accept()

        if room_id not in self.rooms:
            self.rooms[room_id] = set()

        self.rooms[room_id].add(websocket)
        self.users[websocket] = {"username": username, "room": room_id}

        # Notify room of new user
        await self.broadcast(
            room_id,
            {"type": "system", "message": f"{username} joined the room"}
        )

    def disconnect(self, websocket: WebSocket):
        user = self.users.get(websocket)
        if user:
            room_id = user["room"]
            self.rooms[room_id].discard(websocket)
            del self.users[websocket]

            if not self.rooms[room_id]:
                del self.rooms[room_id]

    async def broadcast(self, room_id: str, message: dict):
        if room_id in self.rooms:
            for connection in self.rooms[room_id]:
                await connection.send_json(message)

    async def send_to_user(self, websocket: WebSocket, message: dict):
        await websocket.send_json(message)

room_manager = RoomManager()

@app.websocket("/ws/{room_id}/{username}")
async def chat_websocket(websocket: WebSocket, room_id: str, username: str):
    await room_manager.connect(websocket, room_id, username)
    try:
        while True:
            data = await websocket.receive_json()
            await room_manager.broadcast(room_id, {
                "type": "message",
                "username": username,
                "message": data["message"],
                "timestamp": data.get("timestamp")
            })
    except WebSocketDisconnect:
        room_manager.disconnect(websocket)
        await room_manager.broadcast(room_id, {
            "type": "system",
            "message": f"{username} left the room"
        })
```

## Real-Time Notifications

### Notification System

```python
# Example 3: Real-time notifications
from fastapi import FastAPI, WebSocket, Depends
from typing import Dict
import asyncio
import json

app = FastAPI()

class NotificationManager:
    """Manage user notifications"""

    def __init__(self):
        # user_id -> list of websockets (multiple devices)
        self.connections: Dict[int, List[WebSocket]] = {}

    async def connect(self, websocket: WebSocket, user_id: int):
        await websocket.accept()

        if user_id not in self.connections:
            self.connections[user_id] = []

        self.connections[user_id].append(websocket)

    def disconnect(self, websocket: WebSocket, user_id: int):
        if user_id in self.connections:
            self.connections[user_id].remove(websocket)
            if not self.connections[user_id]:
                del self.connections[user_id]

    async def notify_user(self, user_id: int, notification: dict):
        """Send notification to specific user"""
        if user_id in self.connections:
            for websocket in self.connections[user_id]:
                await websocket.send_json(notification)

    async def notify_all(self, notification: dict):
        """Broadcast to all connected users"""
        for user_id, connections in self.connections.items():
            for websocket in connections:
                await websocket.send_json(notification)

notification_manager = NotificationManager()

@app.websocket("/ws/notifications/{user_id}")
async def notification_websocket(websocket: WebSocket, user_id: int):
    await notification_manager.connect(websocket, user_id)
    try:
        while True:
            # Keep connection alive, wait for disconnect
            await websocket.receive_text()
    except Exception:
        notification_manager.disconnect(websocket, user_id)

# Trigger notifications from HTTP endpoints
@app.post("/notify/{user_id}")
async def send_notification(user_id: int, message: str):
    """Send notification via HTTP"""
    await notification_manager.notify_user(user_id, {
        "type": "notification",
        "message": message
    })
    return {"status": "sent"}

@app.post("/broadcast")
async def broadcast(message: str):
    """Broadcast to all users"""
    await notification_manager.notify_all({
        "type": "broadcast",
        "message": message
    })
    return {"status": "broadcasted"}
```

## Server-Sent Events (SSE)

### SSE Alternative

```python
# Example 4: Server-Sent Events
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
import asyncio
import json

app = FastAPI()

async def event_generator():
    """Generate SSE events"""
    count = 0
    while True:
        count += 1
        data = {
            "count": count,
            "message": f"Event {count}",
            "timestamp": datetime.utcnow().isoformat()
        }
        yield f"data: {json.dumps(data)}\n\n"
        await asyncio.sleep(1)

@app.get("/events")
async def get_events():
    """SSE endpoint"""
    return StreamingResponse(
        event_generator(),
        media_type="text/event-stream"
    )

# Real-time data stream
@app.get("/stock/{symbol}")
async def stock_updates(symbol: str):
    """Stream stock price updates"""
    async def generate():
        while True:
            price = get_stock_price(symbol)  # Simulated
            yield f"data: {json.dumps({'symbol': symbol, 'price': price})}\n\n"
            await asyncio.sleep(5)

    return StreamingResponse(generate(), media_type="text/event-stream")
```

## Authentication

### Secured WebSockets

```python
# Example 5: Authenticated WebSocket
from fastapi import FastAPI, WebSocket, WebSocketDisconnect, Depends, Query
from jose import JWTError, jwt

app = FastAPI()

SECRET_KEY = "your-secret-key"

async def get_current_user_ws(websocket: WebSocket, token: str = Query(...)):
    """Authenticate WebSocket connection"""
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=["HS256"])
        user_id = payload.get("sub")
        if user_id is None:
            await websocket.close(code=1008)  # Policy violation
            return None
        return {"user_id": user_id}
    except JWTError:
        await websocket.close(code=1008)
        return None

@app.websocket("/ws/protected")
async def protected_websocket(
    websocket: WebSocket,
    token: str = Query(...)
):
    # Authenticate
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=["HS256"])
        user_id = payload.get("sub")
        if not user_id:
            await websocket.close(code=1008)
            return
    except JWTError:
        await websocket.close(code=1008)
        return

    await websocket.accept()

    try:
        while True:
            data = await websocket.receive_text()
            await websocket.send_text(f"User {user_id}: {data}")
    except WebSocketDisconnect:
        pass
```

## Broadcast with Redis

### Scalable WebSockets

```python
# Example 6: Redis-backed WebSocket scaling
from fastapi import FastAPI, WebSocket
import redis.asyncio as redis
import asyncio
import json

app = FastAPI()

# Redis connection
redis_client = redis.Redis(host="localhost", port=6379, decode_messages=True)

class RedisPubSubManager:
    """Manage WebSocket scaling with Redis"""

    def __init__(self):
        self.redis = redis_client

    async def publish(self, channel: str, message: dict):
        """Publish message to Redis channel"""
        await self.redis.publish(channel, json.dumps(message))

    async def subscribe(self, channel: str, websocket: WebSocket):
        """Subscribe websocket to Redis channel"""
        pubsub = self.redis.pubsub()
        await pubsub.subscribe(channel)

        try:
            async for message in pubsub.listen():
                if message["type"] == "message":
                    data = json.loads(message["data"])
                    await websocket.send_json(data)
        finally:
            await pubsub.unsubscribe(channel)

pubsub_manager = RedisPubSubManager()

@app.websocket("/ws/global")
async def global_websocket(websocket: WebSocket):
    await websocket.accept()

    # Start Redis subscriber task
    subscriber_task = asyncio.create_task(
        pubsub_manager.subscribe("global", websocket)
    )

    try:
        while True:
            data = await websocket.receive_json()
            # Publish to Redis for all instances
            await pubsub_manager.publish("global", data)
    except Exception:
        subscriber_task.cancel()

# HTTP endpoint to broadcast
@app.post("/broadcast/redis")
async def broadcast_redis(message: str):
    await pubsub_manager.publish("global", {"message": message})
    return {"status": "sent"}
```

## Best Practices

### WebSocket Guidelines

```python
# Example 7: WebSocket best practices
"""
WebSocket Best Practices:

1. Connection Management
   - Track active connections
   - Handle disconnections gracefully
   - Implement heartbeat/ping-pong

2. Scaling
   - Use Redis pub/sub for multi-instance
   - Sticky sessions if needed
   - Connection pooling

3. Security
   - Authenticate connections
   - Validate all messages
   - Rate limit connections

4. Error Handling
   - Catch WebSocketDisconnect
   - Implement reconnection logic
   - Log connection issues

5. Performance
   - Use binary for large data
   - Compress messages
   - Batch updates when possible
"""

# Heartbeat implementation
import asyncio
from datetime import datetime

class WebSocketWithHeartbeat:
    """WebSocket with automatic heartbeat"""

    def __init__(self, websocket: WebSocket, interval: int = 30):
        self.websocket = websocket
        self.interval = interval
        self.last_pong = datetime.utcnow()

    async def start(self):
        """Start heartbeat loop"""
        while True:
            await asyncio.sleep(self.interval)
            try:
                await self.websocket.send_json({"type": "ping"})
                # Wait for pong (simplified)
            except Exception:
                break

    async def handle_message(self, data: dict):
        """Handle incoming message"""
        if data.get("type") == "pong":
            self.last_pong = datetime.utcnow()
```

## Summary

| Feature | Implementation | Use Case |
|---------|----------------|----------|
| Basic WebSocket | `WebSocket` endpoint | Simple real-time |
| Room Management | Connection manager | Chat applications |
| Notifications | Targeted messages | Alert systems |
| SSE | StreamingResponse | One-way updates |
| Scaling | Redis pub/sub | Multi-instance |

## Next Steps

Continue learning about:
- [Server-Sent Events](./02_server_sent_events.md) - SSE details
- [Caching Strategies](../02_performance_optimization/01_caching_strategies.md)
- [GraphQL Implementation](./09_graphql_implementation.md) - Alternative API
