# Response Streaming in FastAPI

## What You'll Learn
- Streaming responses for large data
- Server-Sent Events (SSE) for real-time updates
- File streaming with proper headers
- Generator-based responses
- When to use streaming vs regular responses

## Prerequisites
- Completed `03-custom-exception-handlers.md` — Exception handling
- Understanding of async/await in Python 3.11+

## Why Streaming?

Regular responses wait for all data before sending:
```
Client                    Server
   │──── Request ────────▶│
   │                      │
   │         (processing) │
   │                      │
   │◀─── Complete JSON ───│
```

Streaming sends data incrementally:
```
Client                    Server
   │──── Request ────────▶│
   │                      │
   │◀─ First chunk ──────│
   │◀─ Second chunk ────│
   │◀─ Third chunk ─────│
   │         ...         │
```

**Use cases:**
- Large file downloads
- Real-time progress updates
- Live data feeds (SSE)
- Processing large datasets
- Long-running computations

## Basic Streaming Response

```python
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
import asyncio

app = FastAPI()

async def generate_numbers(max_count: int):
    """Generator that yields numbers one at a time."""
    for i in range(max_count):
        yield f"data: {i}\n\n"
        await asyncio.sleep(0.5)

@app.get("/stream-count/{max_count}")
async def stream_count(max_count: int):
    """Stream counting numbers to client."""
    return StreamingResponse(
        generate_numbers(max_count),
        media_type="text/event-stream"  # For SSE
    )
```

🔍 **Line-by-Line Breakdown:**
1. `generate_numbers(max_count)` — An async generator that yields data
2. `yield f"data: {i}\n\n"` — SSE format: "data:" + content + double newline
3. `await asyncio.sleep(0.5)` — Non-blocking delay between chunks
4. `StreamingResponse(...)` — Tells FastAPI to stream the response

## Server-Sent Events (SSE)

SSE is a server-to-client one-way communication channel:

```python
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
import asyncio
import json
from datetime import datetime

app = FastAPI()

async def event_generator():
    """Generate real-time events for client."""
    event_id = 0
    
    while True:
        # Create event data
        event_data = {
            "id": event_id,
            "timestamp": datetime.utcnow().isoformat(),
            "message": f"Event {event_id} processed"
        }
        
        # SSE format: event type + data + double newline
        yield f"event: message\ndata: {json.dumps(event_data)}\n\n"
        
        event_id += 1
        await asyncio.sleep(2)  # Send every 2 seconds

@app.get("/events")
async def sse_events():
    """Endpoint for Server-Sent Events."""
    return StreamingResponse(
        event_generator(),
        media_type="text/event-stream",
        headers={
            "Cache-Control": "no-cache",
            "Connection": "keep-alive"
        }
    )
```

### Frontend JavaScript for SSE

```javascript
// Connect to SSE endpoint
const eventSource = new EventSource("/events");

// Listen for messages
eventSource.addEventListener("message", (event) => {
    const data = JSON.parse(event.data);
    console.log("Received:", data);
    
    // Update UI
    document.getElementById("events").innerHTML += `
        <div>Event ${data.id}: ${data.message}</div>
    `;
});

// Handle connection open
eventSource.onopen = () => {
    console.log("Connected to event stream!");
};

// Handle errors
eventSource.onerror = (error) => {
    console.log("SSE Error:", error);
    eventSource.close();
};

// Clean up on page unload
window.addEventListener("beforeunload", () => {
    eventSource.close();
});
```

## Streaming File Downloads

```python
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
from pathlib import Path
import aiofiles

app = FastAPI()

@app.get("/download/{filename}")
async def download_file(filename: str):
    """Stream file to client with proper headers."""
    file_path = Path(f"./files/{filename}")
    
    if not file_path.exists():
        raise HTTPException(status_code=404, detail="File not found")
    
    async def file_iterator():
        async with aiofiles.open(file_path, "rb") as f:
            while chunk := await f.read(8192):  # 8KB chunks
                yield chunk
    
    return StreamingResponse(
        file_iterator(),
        media_type="application/octet-stream",
        headers={
            "Content-Disposition": f"attachment; filename={filename}",
            "Content-Length": str(file_path.stat().st_size)
        }
    )
```

## Progress Streaming for Long Operations

```python
from fastapi import FastAPI, BackgroundTasks
from fastapi.responses import StreamingResponse
from pydantic import BaseModel

app = FastAPI()

class BatchJobStatus(BaseModel):
    job_id: str
    total: int
    processed: int
    status: str

# In-memory job storage (use Redis in production)
jobs: dict[str, BatchJobStatus] = {}

async def process_batch(job_id: str, items: list[str]):
    """Process batch items and report progress."""
    job = jobs[job_id]
    job.total = len(items)
    
    for i, item in enumerate(items):
        # Simulate processing
        await asyncio.sleep(0.5)
        
        job.processed = i + 1
        job.status = f"Processing {item}..."
    
    job.status = "Complete"

def generate_progress(job_id: str):
    """Stream progress updates."""
    while True:
        job = jobs.get(job_id)
        if not job:
            break
        
        progress = {
            "job_id": job.job_id,
            "processed": job.processed,
            "total": job.total,
            "percentage": (job.processed / job.total * 100) if job.total > 0 else 0,
            "status": job.status
        }
        
        yield f"data: {json.dumps(progress)}\n\n"
        
        if job.status == "Complete":
            break
        
        import time
        time.sleep(0.5)  # Check every 500ms

@app.post("/batch/{job_id}")
async def start_batch(job_id: str, items: list[str]):
    """Start a batch job and return stream endpoint."""
    jobs[job_id] = BatchJobStatus(job_id=job_id, processed=0, total=0, status="Starting")
    
    # Start processing in background
    asyncio.create_task(process_batch(job_id, items))
    
    return {"stream_url": f"/batch/{job_id}/progress"}

@app.get("/batch/{job_id}/progress")
async def batch_progress(job_id: str):
    """Stream batch job progress."""
    return StreamingResponse(
        generate_progress(job_id),
        media_type="text/event-stream"
    )
```

## Streaming JSON Arrays

```python
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
import json
import asyncio

app = FastAPI()

async def generate_large_json():
    """Stream a large JSON array as NDJSON (newline-delimited JSON)."""
    yield "[\n"
    
    for i in range(1000):
        item = {"id": i, "name": f"Item {i}", "value": i * 10}
        
        if i > 0:
            yield ",\n"
        
        yield json.dumps(item)
        await asyncio.sleep(0.01)  # Simulate data fetching
    
    yield "\n]"

@app.get("/large-data")
async def get_large_data():
    """Stream large JSON array."""
    return StreamingResponse(
        generate_large_json(),
        media_type="application/json"
    )
```

## Production Considerations

- **Memory efficiency**: Streaming uses constant memory regardless of data size
- **Timeout handling**: Clients may disconnect; handle disconnection gracefully
- **Content-Length**: Not needed for streaming (chunked transfer encoding)
- **nginx buffering**: Consider nginx settings if reverse proxying

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Blocking the event loop

**Wrong:**
```python
async def slow_generator():
    for i in range(100):
        time.sleep(1)  # BLOCKS event loop!
        yield f"data: {i}\n\n"
```

**Why it fails:** Blocks all other async operations.

**Fix:**
```python
async def fast_generator():
    for i in range(100):
        await asyncio.sleep(1)  # Non-blocking!
        yield f"data: {i}\n\n"
```

### ❌ Mistake 2: Not handling client disconnection

**Wrong:**
```python
async def stream_data():
    for i in range(10000):
        await asyncio.sleep(1)
        yield f"data: {i}\n\n"  # Continues even if client left!
```

**Why it fails:** Wastes resources sending data to disconnected client.

**Fix:**
```python
async def stream_data(request: Request):
    for i in range(10000):
        if await request.is_disconnected():
            break  # Stop when client leaves
        yield f"data: {i}\n\n"
        await asyncio.sleep(1)
```

### ❌ Mistake 3: Wrong media type for SSE

**Wrong:**
```python
return StreamingResponse(generator(), media_type="application/json")
```

**Why it fails:** Browser won't process as event stream.

**Fix:**
```python
return StreamingResponse(generator(), media_type="text/event-stream")
```

## Summary

- StreamingResponse sends data incrementally without waiting for complete response
- Server-Sent Events (SSE) provide server-to-client real-time communication
- Use `async for` or generators with `yield`
- Always use `await asyncio.sleep()` not `time.sleep()`
- Handle client disconnection to avoid wasting resources

## Next Steps

→ Continue to `05-websockets.md` to learn about bidirectional real-time communication with WebSockets.
