# File Downloads

## What You'll Learn
- Serving files for download
- Streaming large files
- Secure file serving

## Prerequisites
- Completed file upload basics

## Basic Download

```python
from fastapi import FastAPI
from fastapi.responses import FileResponse
from pathlib import Path

app = FastAPI()

FILES_DIR = Path("files")

@app.get("/download/{filename}")
async def download_file(filename: str):
    """Download a file"""
    filepath = FILES_DIR / filename
    
    if not filepath.exists():
        return {"error": "File not found"}, 404
    
    return FileResponse(
        path=filepath,
        filename=filename,
        media_type="application/octet-stream"
    )
```

## Streaming Response

```python
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
import asyncio

@app.get("/stream/{filename}")
async def stream_file(filename: str):
    """Stream large file"""
    async def file_iterator():
        filepath = FILES_DIR / filename
        with filepath.open("rb") as f:
            while chunk := f.read(8192):
                yield chunk
                await asyncio.sleep(0)  # Allow other tasks
    
    return StreamingResponse(
        file_iterator(),
        media_type="application/octet-stream",
        headers={"Content-Disposition": f"attachment; filename={filename}"}
    )
```

## Summary
- Use FileResponse for simple downloads
- Use StreamingResponse for large files

## Next Steps
→ Continue to `03-file-storage-cloud.md`
