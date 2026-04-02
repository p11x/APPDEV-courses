# Streaming Requests

## Overview

Streaming requests handle large data transfers efficiently without loading everything into memory. FastAPI supports request body streaming for processing large payloads.

## Basic Streaming

### Reading Request Stream

```python
# Example 1: Streaming request body
from fastapi import FastAPI, Request
from fastapi.responses import StreamingResponse
import io

app = FastAPI()

@app.post("/upload/stream/")
async def stream_upload(request: Request):
    """
    Read request body as stream.
    Useful for large files or real-time data.
    """
    total_bytes = 0

    async for chunk in request.stream():
        total_bytes += len(chunk)
        # Process chunk here

    return {"total_bytes": total_bytes}
```

### Chunked Processing

```python
# Example 2: Process data in chunks
from fastapi import FastAPI, Request
import aiofiles

app = FastAPI()

@app.post("/upload/chunked/")
async def chunked_upload(request: Request):
    """
    Process incoming data in chunks.
    Memory efficient for large uploads.
    """
    chunk_count = 0
    file_path = "uploads/streamed_file.bin"

    async with aiofiles.open(file_path, "wb") as f:
        async for chunk in request.stream():
            await f.write(chunk)
            chunk_count += 1

    return {
        "message": "Upload complete",
        "chunks_processed": chunk_count
    }
```

## Streaming with UploadFile

### Efficient File Streaming

```python
# Example 3: UploadFile streaming
from fastapi import FastAPI, File, UploadFile

app = FastAPI()

@app.post("/upload/efficient/")
async def efficient_upload(file: UploadFile = File(...)):
    """
    UploadFile streams automatically.
    Doesn't load entire file into memory.
    """
    chunk_size = 1024 * 1024  # 1MB
    total_size = 0

    while chunk := await file.read(chunk_size):
        total_size += len(chunk)
        # Process chunk here

    return {
        "filename": file.filename,
        "total_size": total_size
    }
```

## Progress Tracking

### Upload Progress

```python
# Example 4: Track upload progress
from fastapi import FastAPI, Request
from typing import Dict
import asyncio

app = FastAPI()

# Store upload progress
uploads: Dict[str, dict] = {}

@app.post("/upload/{upload_id}")
async def tracked_upload(upload_id: str, request: Request):
    """
    Track upload progress by ID.
    """
    uploads[upload_id] = {"status": "uploading", "bytes": 0}

    async for chunk in request.stream():
        uploads[upload_id]["bytes"] += len(chunk)
        # Simulate processing
        await asyncio.sleep(0.001)

    uploads[upload_id]["status"] = "complete"
    return {"upload_id": upload_id, "status": "complete"}

@app.get("/upload/{upload_id}/status")
async def get_upload_status(upload_id: str):
    """Check upload progress"""
    if upload_id not in uploads:
        return {"error": "Upload not found"}
    return uploads[upload_id]
```

## Best Practices

### Streaming Guidelines

```python
# Example 5: Best practices for streaming
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse
import aiofiles

app = FastAPI()

MAX_SIZE = 100 * 1024 * 1024  # 100MB

@app.post("/upload/safe-stream/")
async def safe_stream(request: Request):
    """
    Best practices:
    1. Set size limits
    2. Handle errors gracefully
    3. Clean up resources
    """
    total_bytes = 0
    file_path = "uploads/safe_stream.bin"

    try:
        async with aiofiles.open(file_path, "wb") as f:
            async for chunk in request.stream():
                total_bytes += len(chunk)

                if total_bytes > MAX_SIZE:
                    return JSONResponse(
                        status_code=413,
                        content={"error": "File too large"}
                    )

                await f.write(chunk)

        return {"size": total_bytes, "status": "complete"}

    except Exception as e:
        return JSONResponse(
            status_code=500,
            content={"error": str(e)}
        )
```

## Summary

| Feature | Use Case | Example |
|---------|----------|---------|
| `request.stream()` | Manual streaming | `async for chunk in request.stream()` |
| `UploadFile` | File uploads | Automatic streaming |
| Chunk processing | Large files | Process in 1MB chunks |
| Progress tracking | UX improvement | Track bytes received |

## Next Steps

Continue learning about:
- [Form Data Advanced](./05_form_data_advanced.md) - Complex forms
- [Responses](../04_responses_and_status_codes/01_default_responses.md) - Response types
- [Custom Responses](../04_responses_and_status_codes/06_custom_responses.md) - Custom formats
