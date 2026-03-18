# Real-Time Chat

## What You'll Learn
- WebSocket implementation
- Room management
- Message persistence

## Prerequisites
- Completed SaaS application

## WebSocket Chat

```python
from fastapi import WebSocket

class ConnectionManager:
    def __init__(self):
        self.active_connections: dict[str, list[WebSocket]] = {}
    
    async def connect(self, room: str, websocket: WebSocket):
        await websocket.accept()
        if room not in self.active_connections:
            self.active_connections[room] = []
        self.active_connections[room].append(websocket)
    
    async def broadcast(self, room: str, message: str):
        for connection in self.active_connections.get(room, []):
            await connection.send_text(message)

manager = ConnectionManager()

@app.websocket("/ws/{room}")
async def websocket_endpoint(websocket: WebSocket, room: str):
    await manager.connect(room, websocket)
    try:
        while True:
            data = await websocket.receive_text()
            await manager.broadcast(room, data)
    except:
        manager.disconnect(room, websocket)
```

## Summary
- Use WebSockets for real-time
- Implement rooms
- Persist messages

## Next Steps
→ Continue to `04-api-marketplace.md`
