# WebSockets with Redis

## What You'll Learn
- Scaling WebSockets with Redis pub/sub
- Distributed connection management
- Message broadcasting across servers
- Implementing Redis-backed WebSocket server
- Production deployment patterns

## Prerequisites
- Completed `01-websockets-deep-dive.md` — WebSocket basics
- Understanding of Redis
- Python async/await knowledge

## The Scaling Problem

When you run multiple server instances, WebSocket connections are tied to specific servers:

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│  Server 1   │     │  Server 2   │     │  Server 3   │
│  (WS: 100)  │     │  (WS: 95)   │     │  (WS: 105)  │
│             │     │             │     │             │
│   User A    │     │   User B    │     │   User C    │
└─────────────┘     └─────────────┘     └─────────────┘

Problem: 
- User A sends message to User B -> User B is on Server 2!
- Direct WebSocket doesn't work across servers
```

## Redis Pub/Sub Solution

Redis pub/sub enables cross-server communication:

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│  Server 1   │     │  Server 2   │     │  Server 3   │
│             │     │             │     │             │
│  WebSocket  │     │  WebSocket  │     │  WebSocket  │
│    │        │     │    │        │     │    │        │
│    ▼        │     │    ▼        │     │    ▼        │
│  ┌──────┐   │     │  ┌──────┐   │     │  ┌──────┐   │
│  │Redis │   │◄────┴─►│Redis │   │◄────┴─►│Redis │   │
│  │ Pub/ │   │     │  │ Sub  │   │     │  │ Pub  │   │
│  └──────┘   │     │  └──────┘   │     │  └──────┘   │
└─────────────┘     └─────────────┘     └─────────────┘

Solution: 
- Server 1 publishes to Redis channel
- Redis broadcasts to Server 2 and 3
- Each server sends to their connected WebSocket clients
```

## Implementing Redis Pub/Sub

```python
pip install redis aioredis
```

```python
# ws_manager.py
import asyncio
import json
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
from typing import Dict, Set
import aioredis

class RedisWebSocketManager:
    """WebSocket manager with Redis pub/sub."""
    
    def __init__(self, redis_url: str = "redis://localhost:6379"):
        self.redis_url = redis_url
        self.redis: aioredis.Redis | None = None
        self.pubsub: aioredis.client.PubSub | None = None
        
        # Local connections
        self.connections: Dict[str, Set[WebSocket]] = {}
        self.server_id: str = ""  # Unique ID for this server
    
    async def connect(self) -> None:
        """Connect to Redis."""
        self.redis = await aioredis.create_redis_pool(self.redis_url)
        self.pubsub = self.redis.pubsub()
        
        # Start listening to Redis messages
        asyncio.create_task(self._listen_to_redis())
    
    async def disconnect(self) -> None:
        """Disconnect from Redis."""
        if self.pubsub:
            await self.pubsub.unsubscribe("ws:broadcast")
            self.pubsub.close()
        if self.redis:
            self.redis.close()
    
    async def _listen_to_redis(self) -> None:
        """Listen to Redis messages and forward to local connections."""
        
        await self.pubsub.subscribe("ws:broadcast")
        
        async for message in self.pubsub.iter():
            if message.type == "message":
                data = json.loads(message.data)
                
                # Get target channel
                channel = data.get("channel", "global")
                
                # Send to local connections in that channel
                await self._send_to_channel(channel, data["message"])
    
    async def _send_to_channel(self, channel: str, message: str) -> None:
        """Send message to all connections in a channel."""
        
        if channel not in self.connections:
            return
        
        disconnected = set()
        
        for ws in self.connections[channel]:
            try:
                await ws.send_text(message)
            except Exception:
                disconnected.add(ws)
        
        # Clean up
        for ws in disconnected:
            self.connections[channel].discard(ws)
    
    async def connect_websocket(
        self, 
        websocket: WebSocket, 
        channel: str = "global"
    ) -> None:
        """Register a new WebSocket connection."""
        
        await websocket.accept()
        
        if channel not in self.connections:
            self.connections[channel] = set()
        
        self.connections[channel].add(websocket)
    
    async def disconnect_websocket(
        self, 
        websocket: WebSocket, 
        channel: str = "global"
    ) -> None:
        """Remove a WebSocket connection."""
        
        if channel in self.connections:
            self.connections[channel].discard(websocket)
    
    async def publish(self, channel: str, message: str) -> None:
        """Publish message to channel (all servers receive)."""
        
        data = json.dumps({
            "channel": channel,
            "message": message
        })
        
        await self.redis.publish("ws:broadcast", data)

# Global manager instance
ws_manager = RedisWebSocketManager()
```

🔍 **Line-by-Line Breakdown:**
1. `aioredis.create_redis_pool()` — Async Redis connection
2. `pubsub.subscribe()` — Subscribe to Redis channel
3. `iter()` — Async iterator for Redis messages
4. `redis.publish()` — Publish to channel (broadcasts to all subscribers)

## FastAPI Integration

```python
# main.py
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
import json

app = FastAPI()

@app.on_event("startup")
async def startup():
    """Initialize Redis connection on startup."""
    await ws_manager.connect()

@app.on_event("shutdown")
async def shutdown():
    """Close Redis connection on shutdown."""
    await ws_manager.disconnect()

@app.websocket("/ws/{channel}")
async def websocket_endpoint(websocket: WebSocket, channel: str):
    """WebSocket endpoint with Redis pub/sub."""
    
    await ws_manager.connect_websocket(websocket, channel)
    
    try:
        while True:
            # Receive message from client
            data = await websocket.receive_text()
            
            # Parse and broadcast to all servers
            await ws_manager.publish(channel, data)
            
    except WebSocketDisconnect:
        await ws_manager.disconnect_websocket(websocket, channel)

# Send to specific channel from anywhere
@app.post("/broadcast/{channel}")
async def broadcast(channel: str, message: str):
    """HTTP endpoint to broadcast to WebSocket channel."""
    
    await ws_manager.publish(channel, message)
    return {"sent": True}
```

## Channel Management

### User-Specific Channels

```python
@app.websocket("/ws/user/{user_id}")
async def user_websocket(websocket: WebSocket, user_id: str):
    """Personal WebSocket for each user."""
    
    channel = f"user:{user_id}"
    await ws_manager.connect_websocket(websocket, channel)
    
    try:
        while True:
            data = await websocket.receive_text()
            
            # Echo back to user's channel
            await ws_manager.publish(channel, data)
            
    except WebSocketDisconnect:
        await ws_manager.disconnect_websocket(websocket, channel)
```

### Room/Group Channels

```python
class RoomManager:
    """Manage chat rooms."""
    
    def __init__(self, ws_manager: RedisWebSocketManager):
        self.ws_manager = ws_manager
        self.rooms: Dict[str, Set[str]] = {}  # room_id -> user_ids
    
    async def join_room(self, user_id: str, room_id: str) -> None:
        """Add user to room."""
        
        if room_id not in self.rooms:
            self.rooms[room_id] = set()
        
        self.rooms[room_id].add(user_id)
        
        # Notify room
        await self.ws_manager.publish(
            f"room:{room_id}",
            json.dumps({
                "type": "user_joined",
                "user_id": user_id,
                "members": list(self.rooms[room_id])
            })
        )
    
    async def leave_room(self, user_id: str, room_id: str) -> None:
        """Remove user from room."""
        
        if room_id in self.rooms:
            self.rooms[room_id].discard(user_id)
            
            await self.ws_manager.publish(
                f"room:{room_id}",
                json.dumps({
                    "type": "user_left",
                    "user_id": user_id
                })
            )
    
    async def send_to_room(self, room_id: str, message: dict) -> None:
        """Send message to all room members."""
        
        await self.ws_manager.publish(
            f"room:{room_id}",
            json.dumps(message)
        )
```

## Presence System

```python
import time

class PresenceManager:
    """Track online users with Redis."""
    
    KEY_PREFIX = "presence:"
    HEARTBEAT_TTL = 60  # User considered offline after 60s
    
    def __init__(self, redis: aioredis.Redis):
        self.redis = redis
    
    async def user_online(self, user_id: str) -> None:
        """Mark user as online."""
        
        key = f"{self.KEY_PREFIX}{user_id}"
        await self.redis.set(key, str(time.time()), expire=self.HEARTBEAT_TTL)
    
    async def user_offline(self, user_id: str) -> None:
        """Mark user as offline."""
        
        key = f"{self.KEY_PREFIX}{user_id}"
        await self.redis.delete(key)
    
    async def is_online(self, user_id: str) -> bool:
        """Check if user is online."""
        
        key = f"{self.KEY_PREFIX}{user_id}"
        return await self.redis.exists(key)
    
    async def get_online_users(self) -> list[str]:
        """Get all online users."""
        
        keys = await self.redis.keys(f"{self.KEY_PREFIX}*")
        return [key.decode().replace(self.KEY_PREFIX, "") for key in keys]
    
    async def start_heartbeat(self, user_id: str) -> None:
        """Maintain online status with heartbeat."""
        
        while True:
            await asyncio.sleep(30)  # Heartbeat every 30s
            await self.user_online(user_id)

# Usage
presence = PresenceManager(redis)

@app.websocket("/ws/presence/{user_id}")
async def presence_websocket(websocket: WebSocket, user_id: str):
    """WebSocket with presence tracking."""
    
    await websocket.accept()
    
    # Mark as online
    await presence.user_online(user_id)
    
    # Start heartbeat task
    heartbeat = asyncio.create_task(presence.start_heartbeat(user_id))
    
    try:
        while True:
            # Keep connection alive
            await websocket.receive_text()
            
    except WebSocketDisconnect:
        heartbeat.cancel()
        await presence.user_offline(user_id)
```

## Production Deployment

### Docker Compose

```yaml
# docker-compose.yml
version: '3.8'

services:
  app:
    build: .
    ports:
      - "8000:8000"
    environment:
      - REDIS_URL=redis://redis:6379
    depends_on:
      - redis

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"

  # Add more app instances for scaling
  app2:
    build: .
    environment:
      - REDIS_URL=redis://redis:6379
    depends_on:
      - redis
```

### Nginx WebSocket Proxy

```nginx
http {
    upstream websocket {
        server app:8000;
        server app2:8000;  # Load balance across instances
    }
    
    server {
        location /ws/ {
            proxy_pass http://websocket;
            
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection "upgrade";
            
            proxy_read_timeout 86400;
            proxy_send_timeout 86400;
        }
    }
}
```

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not handling Redis disconnect

**Wrong:**
```python
# Assume Redis always works
await redis.publish("channel", message)
```

**Why it fails:** Redis can disconnect, breaking messaging.

**Fix:**
```python
async def safe_publish(channel: str, message: str):
    try:
        await redis.publish(channel, message)
    except Exception as e:
        logger.error(f"Redis publish failed: {e}")
        # Fallback or retry logic
```

### ❌ Mistake 2: Memory leaks on server

**Wrong:**
```python
# Never remove connections!
self.connections[channel].add(ws)
```

**Why it fails:** Closed connections accumulate.

**Fix:**
```python
# Always remove on disconnect
except WebSocketDisconnect:
    self.connections[channel].discard(ws)
```

### ❌ Mistake 3: No message acknowledgment

**Wrong:**
```python
# Fire and forget - no guarantee of delivery
await ws_manager.publish(channel, message)
```

**Why it fails:** Client might not receive.

**Fix:**
```python
# Add acknowledgment pattern
await ws_manager.publish(channel, json.dumps({
    "type": "message",
    "id": generate_uuid(),
    "ack": True  # Client acknowledges receipt
}))
```

## Summary

- Redis pub/sub enables WebSocket scaling across multiple servers
- Publish messages to Redis, all servers receive and forward
- Track presence with Redis keys with TTL
- Use nginx proxy for WebSocket load balancing
- Handle Redis disconnections gracefully

## Next Steps

→ Continue to `04-grpc-introduction.md` to learn about gRPC for high-performance APIs.
