# Server-Sent Events (SSE)

## What You'll Learn
- SSE fundamentals and protocol
- Implementing SSE in FastAPI
- Connection management
- Browser EventSource API
- Use cases and limitations

## Prerequisites
- Completed `01-websockets-deep-dive.md` — WebSocket basics
- Understanding of HTTP protocol

## What Is SSE?

Server-Sent Events is a server-to-client **one-way** communication over HTTP:

```
WebSocket:     Client ◀──────────▶ Server
                  (Bidirectional)

SSE:           Client ◀─────────── Server
                  (Server to client only)
```

### SSE vs WebSocket

| Feature | SSE | WebSocket |
|---------|-----|-----------|
| Direction | Server → Client | Bidirectional |
| Browser support | Native (EventSource) | Universal |
| Auto-reconnect | Built-in | Manual |
| Binary data | Limited | Full support |
| HTTP/2 | Multiplexed | Single connection |
| Use case | Updates, notifications | Chat, games |

## SSE Protocol

```
HTTP/1.1 200 OK
Content-Type: text/event-stream
Cache-Control: no-cache
Connection: keep-alive

data: {"message": "first"}

data: {"message": "second"}

event: custom
data: {"type": "custom", "content": "third"}

: This is a comment

data: {"message": "fourth"}
```

### Event Format

```
event: <event-type>
data: <JSON payload>

(separated by double newline)
```

## Implementing SSE in FastAPI

```python
from fastapi import FastAPI, Request
from fastapi.responses import StreamingResponse
import asyncio
import json
import time
from typing import AsyncGenerator

app = FastAPI()

async def event_generator() -> AsyncGenerator[str, None]:
    """Generate SSE events."""
    
    while True:
        # Check for client disconnect
        # (in production, track client state)
        
        # Create event
        event_data = {
            "timestamp": time.time(),
            "message": f"Event at {time.strftime('%H:%M:%S')}"
        }
        
        # Format as SSE
        yield f"data: {json.dumps(event_data)}\n\n"
        
        # Wait before next event
        await asyncio.sleep(2)

@app.get("/events")
async def sse_events(request: Request):
    """SSE endpoint."""
    
    return StreamingResponse(
        event_generator(),
        media_type="text/event-stream",
        headers={
            "Cache-Control": "no-cache",
            "Connection": "keep-alive",
            "X-Accel-Buffering": "no"  # Disable nginx buffering
        }
    )
```

🔍 **Line-by-Line Breakdown:**
1. `AsyncGenerator[str, None]` — Type hint for async generator
2. `StreamingResponse` — Streams response instead of waiting for complete
3. `text/event-stream` — Required media type for SSE
4. `json.dumps()` — Serialize event data
5. `\n\n` — Required double newline to send event

## Client-Side EventSource

### JavaScript Implementation

```javascript
// Connect to SSE endpoint
const eventSource = new EventSource("/events");

// Listen for default messages
eventSource.onmessage = (event) => {
    const data = JSON.parse(event.data);
    console.log("Message:", data);
    
    // Update UI
    document.getElementById("messages").innerHTML += `
        <p>${data.message}</p>
    `;
};

// Listen for custom events
eventSource.addEventListener("notification", (event) => {
    const data = JSON.parse(event.data);
    showNotification(data.title, data.body);
});

// Handle connection open
eventSource.onopen = () => {
    console.log("Connected to SSE!");
};

// Handle errors
eventSource.onerror = (error) => {
    console.error("SSE Error:", error);
    
    if (eventSource.readyState === EventSource.CLOSED) {
        console.log("Connection closed");
    }
};

// Clean up on page leave
window.addEventListener("beforeunload", () => {
    eventSource.close();
});
```

### HTML Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>SSE Demo</title>
    <style>
        #messages { 
            border: 1px solid #ccc; 
            padding: 10px; 
            height: 300px; 
            overflow-y: auto; 
        }
    </style>
</head>
<body>
    <h1>Server-Sent Events Demo</h1>
    
    <div id="messages"></div>
    
    <button id="connect">Connect</button>
    <button id="disconnect">Disconnect</button>
    
    <script>
        let eventSource = null;
        
        document.getElementById("connect").onclick = () => {
            if (eventSource) return;
            
            eventSource = new EventSource("/events");
            
            eventSource.onmessage = (event) => {
                const data = JSON.parse(event.data);
                document.getElementById("messages").innerHTML += 
                    `<p>${data.timestamp}: ${data.message}</p>`;
            };
        };
        
        document.getElementById("disconnect").onclick = () => {
            if (eventSource) {
                eventSource.close();
                eventSource = null;
            }
        };
    </script>
</body>
</html>
```

## Real-World Examples

### 1. Live Notifications

```python
from fastapi import FastAPI, Request
from fastapi.responses import StreamingResponse
import asyncio
import json
import uuid
from collections import defaultdict
from typing import Dict, Set

class NotificationManager:
    """Manage SSE notifications."""
    
    def __init__(self):
        # request -> queue
        self.queues: Dict[Request, asyncio.Queue] = {}
    
    async def subscribe(self, request: Request) -> AsyncGenerator[str, None]:
        """Subscribe to notifications."""
        
        queue = asyncio.Queue()
        self.queues[request] = queue
        
        try:
            while True:
                # Wait for notification
                message = await queue.get()
                
                # Send as SSE
                yield f"data: {json.dumps(message)}\n\n"
                
        except asyncio.CancelledError:
            pass
        finally:
            del self.queues[request]
    
    async def notify(self, event_type: str, data: dict) -> None:
        """Send notification to all subscribers."""
        
        message = {"type": event_type, "data": data}
        
        # Send to all queues
        for queue in self.queues.values():
            await queue.put(message)

notifications = NotificationManager()

@app.get("/notifications")
async def notifications_endpoint(request: Request):
    """SSE endpoint for notifications."""
    
    return StreamingResponse(
        notifications.subscribe(request),
        media_type="text/event-stream"
    )

# Trigger notifications
@app.post("/trigger/{event_type}")
async def trigger_event(event_type: str, data: dict):
    """Trigger a notification event."""
    await notifications.notify(event_type, data)
    return {"sent": True}
```

### 2. Progress Updates

```python
@app.get("/progress/{task_id}")
async def task_progress(task_id: str):
    """Stream task progress."""
    
    async def progress_generator():
        # Simulate long-running task
        for i in range(1, 11):
            await asyncio.sleep(1)  # Simulate work
            
            # Send progress update
            progress_data = {
                "task_id": task_id,
                "progress": i * 10,
                "status": "processing" if i < 10 else "complete"
            }
            
            yield f"data: {json.dumps(progress_data)}\n\n"
    
    return StreamingResponse(
        progress_generator(),
        media_type="text/event-stream"
    )
```

### 3. Custom Events

```python
async def custom_events():
    """Send different event types."""
    
    # Default event
    yield "data: {\"message\": \"default event\"}\n\n"
    
    # Custom event named "custom"
    yield "event: custom\ndata: {\"message\": \"custom event\"}\n\n"
    
    # Custom event named "alert"
    yield "event: alert\ndata: {\"level\": \"warning\", \"message\": \"Alert!\"}\n\n"

@app.get("/custom-events")
async def custom_events_endpoint():
    """Endpoint with custom event types."""
    
    return StreamingResponse(
        custom_events(),
        media_type="text/event-stream"
    )
```

## SSE with Authentication

```python
from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import OAuth2PasswordBearer
from fastapi.responses import StreamingResponse

oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

async def get_current_user(token: str = Depends(oauth2_scheme)) -> dict:
    """Authenticate user."""
    # In production, decode JWT
    if not token:
        raise HTTPException(status_code=401, detail="Not authenticated")
    return {"username": "user"}

@app.get("/secure-events")
async def secure_events(
    request: Request,
    user: dict = Depends(get_current_user)
):
    """SSE endpoint requiring authentication."""
    
    async def secure_generator():
        try:
            while True:
                await asyncio.sleep(2)
                
                message = {
                    "user": user["username"],
                    "time": str(time.time())
                }
                
                yield f"data: {json.dumps(message)}\n\n"
                
        except asyncio.CancelledError:
            print(f"Client disconnected: {user['username']}")
    
    return StreamingResponse(
        secure_generator(),
        media_type="text/event-stream"
    )
```

## Production Considerations

- **nginx buffering**: Disable with `X-Accel-Buffering: no`
- **Connection limits**: Configure max connections per IP
- **Reconnection**: Browsers auto-reconnect, but you may need to send last event ID
- **Compression**: SSE can be compressed with gzip
- **Load balancing**: Use sticky sessions

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Missing double newline

**Wrong:**
```python
yield f"data: {json.dumps(data)}\n"  # Single newline!
```

**Why it fails:** Event isn't sent until double newline.

**Fix:**
```python
yield f"data: {json.dumps(data)}\n\n"  # Double newline!
```

### ❌ Mistake 2: Wrong content type

**Wrong:**
```python
return StreamingResponse(generator(), media_type="application/json")
```

**Why it fails:** Browser won't recognize as SSE.

**Fix:**
```python
return StreamingResponse(generator(), media_type="text/event-stream")
```

### ❌ Mistake 3: No disconnect handling

**Wrong:**
```python
async def generator():
    while True:
        await asyncio.sleep(1)
        yield f"data: hi\n\n"  # Infinite loop, never exits!
```

**Why it fails:** Connection closes but generator keeps running forever.

**Fix:**
```python
async def generator(request: Request):
    try:
        while True:
            await asyncio.sleep(1)
            
            # Check if client disconnected
            if await request.is_disconnected():
                break
                
            yield f"data: hi\n\n"
    except asyncio.CancelledError:
        print("Client disconnected")
```

## Summary

- SSE provides server-to-client streaming over HTTP
- Use `text/event-stream` content type
- Separate events with double newline (`\n\n`)
- Use `event:` prefix for custom event types
- Use EventSource API in browsers for easy integration
- Best for: notifications, updates, live feeds

## Next Steps

→ Continue to `03-websockets-with-redis.md` to learn about scaling WebSockets with Redis.
