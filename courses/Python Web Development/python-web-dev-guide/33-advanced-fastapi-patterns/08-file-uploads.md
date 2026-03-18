# File Uploads in FastAPI

## What You'll Learn
- Handling file uploads with FastAPI
- Single and multiple file uploads
- File type validation and size limits
- Streaming uploads for large files
- Storing files to disk and cloud storage

## Prerequisites
- Completed `07-advanced-dependency-injection.md` — Dependency injection patterns
- Understanding of async/await in Python 3.11+

## Basic File Upload

FastAPI provides `UploadFile` for handling file uploads:

```python
from fastapi import FastAPI, File, UploadFile
from fastapi.responses import JSONResponse
import shutil
from pathlib import Path

app = FastAPI()

# Create uploads directory
UPLOAD_DIR = Path("uploads")
UPLOAD_DIR.mkdir(exist_ok=True)

@app.post("/upload")
async def upload_file(file: UploadFile = File(...)):
    """Basic single file upload endpoint."""
    
    # file.filename: Original filename from client
    # file.content_type: MIME type (e.g., "image/png")
    # file.file: File-like object to read from
    
    # Save file to disk
    file_path = UPLOAD_DIR / file.filename
    
    with file_path.open("wb") as buffer:
        # Copy uploaded file to buffer
        shutil.copyfileobj(file.file, buffer)
    
    return {
        "filename": file.filename,
        "content_type": file.content_type,
        "size": file_path.stat().st_size
    }
```

🔍 **Line-by-Line Breakdown:**
1. `file: UploadFile = File(...)` — FastAPI automatically parses multipart form data
2. `file.filename` — Original name of the uploaded file
3. `file.content_type` — MIME type (e.g., "image/png", "application/pdf")
4. `file.file` — File-like object containing the binary data
5. `shutil.copyfileobj()` — Efficiently copies data between file objects

## UploadFile API

`UploadFile` provides several useful methods:

```python
from fastapi import UploadFile
import aiofiles

async def process_upload(file: UploadFile) -> dict:
    """Process uploaded file using UploadFile methods."""
    
    # Read entire file content
    content = await file.read()
    await file.seek(0)  # Reset file pointer after reading
    
    # Get file info
    filename = file.filename
    content_type = file.content_type
    
    return {
        "filename": filename,
        "size": len(content),
        "type": content_type
    }

# Async context manager (recommended)
async def save_upload(file: UploadFile) -> Path:
    """Save upload file using async context manager."""
    file_path = UPLOAD_DIR / file.filename
    
    async with aiofiles.open(file_path, "wb") as f:
        await f.write(await file.read())
    
    return file_path
```

## Multiple File Uploads

```python
from fastapi import FastAPI, File, UploadFile
from typing import List

@app.post("/upload/multiple")
async def upload_multiple_files(
    files: List[UploadFile] = File(...)
):
    """Upload multiple files at once."""
    
    results = []
    
    for file in files:
        # Process each file
        content = await file.read()
        
        results.append({
            "filename": file.filename,
            "size": len(content),
            "content_type": file.content_type
        })
    
    return {
        "uploaded": len(files),
        "files": results
    }
```

**Request format (HTML form):**
```html
<form action="/upload/multiple" method="post" enctype="multipart/form-data">
    <input type="file" name="files" multiple>
    <button type="submit">Upload</button>
</form>
```

## File Validation

### Size Limits

```python
from fastapi import FastAPI, File, UploadFile, HTTPException
from fastapi.responses import JSONResponse

MAX_FILE_SIZE = 10 * 1024 * 1024  # 10 MB

async def validate_file_size(file: UploadFile) -> UploadFile:
    """Validate file size before processing."""
    
    # Read first chunk to check size
    chunk = await file.read(1024 * 1024)  # Read 1MB
    await file.seek(0)  # Reset
    
    if len(chunk) > MAX_FILE_SIZE:
        raise HTTPException(
            status_code=413,
            detail=f"File too large. Max size: {MAX_FILE_SIZE} bytes"
        )
    
    return file

@app.post("/upload/validated")
async def upload_with_validation(
    file: UploadFile = File(..., depends=validate_file_size)
):
    """Upload with size validation."""
    return {"filename": file.filename}
```

### Content Type Validation

```python
from fastapi import FastAPI, File, UploadFile, HTTPException
from typing import List

ALLOWED_IMAGE_TYPES = {"image/jpeg", "image/png", "image/gif", "image/webp"}
ALLOWED_DOCUMENT_TYPES = {"application/pdf"}

async def validate_image(file: UploadFile) -> UploadFile:
    """Validate that uploaded file is an image."""
    if file.content_type not in ALLOWED_IMAGE_TYPES:
        raise HTTPException(
            status_code=400,
            detail=f"Invalid file type. Allowed: {ALLOWED_IMAGE_TYPES}"
        )
    return file

@app.post("/upload/image")
async def upload_image(
    file: UploadFile = File(..., depends=validate_image)
):
    """Upload image with validation."""
    return {"filename": file.filename, "type": file.content_type}

@app.post("/upload/document")
async def upload_document(
    file: UploadFile = File(...)
):
    """Upload document - no validation, just store."""
    # Validate manually if needed
    if file.content_type not in ALLOWED_DOCUMENT_TYPES:
        raise HTTPException(status_code=400, detail="Only PDF allowed")
    
    return {"filename": file.filename, "type": file.content_type}
```

## Streaming Large Files

For very large files, stream directly to disk without loading into memory:

```python
from fastapi import FastAPI, UploadFile, File
from pathlib import Path
import aiofiles

app = FastAPI()

UPLOAD_DIR = Path("uploads")
UPLOAD_DIR.mkdir(exist_ok=True)

@app.post("/upload/stream")
async def stream_upload(file: UploadFile = File(...)):
    """Stream large file directly to disk - memory efficient."""
    
    file_path = UPLOAD_DIR / file.filename
    total_bytes = 0
    
    # Stream in chunks (8KB each)
    async with aiofiles.open(file_path, "wb") as f:
        while chunk := await file.read(8192):
            await f.write(chunk)
            total_bytes += len(chunk)
    
    return {
        "filename": file.filename,
        "size": total_bytes,
        "saved_to": str(file_path)
    }
```

## Storing to Cloud Storage

```python
from fastapi import FastAPI, UploadFile, File
import boto3
from botocore.config import Config

app = FastAPI()

# Configure S3 client
s3_client = boto3.client(
    "s3",
    aws_access_key_id="your-key",
    aws_secret_access_key="your-secret",
    region_name="us-east-1",
    config=Config(signature_version="s3v4")
)

BUCKET_NAME = "your-bucket"

@app.post("/upload/s3")
async def upload_to_s3(file: UploadFile = File(...)):
    """Upload file directly to S3."""
    
    # Read file content
    content = await file.read()
    
    # Upload to S3
    s3_client.put_object(
        Bucket=BUCKET_NAME,
        Key=file.filename,
        Body=content,
        ContentType=file.content_type
    )
    
    # Generate presigned URL for access
    url = s3_client.generate_presigned_url(
        "get_object",
        Params={"Bucket": BUCKET_NAME, "Key": file.filename},
        ExpiresIn=3600
    )
    
    return {
        "filename": file.filename,
        "url": url
    }
```

## File Download Endpoint

```python
from fastapi import FastAPI, UploadFile, File
from fastapi.responses import FileResponse

@app.post("/upload/with-download")
async def upload_and_return(file: UploadFile = File(...)):
    """Upload file and return it for download."""
    
    # Save file
    file_path = UPLOAD_DIR / file.filename
    async with aiofiles.open(file_path, "wb") as f:
        await f.write(await file.read())
    
    # Return file as download
    return FileResponse(
        path=file_path,
        filename=f"processed_{file.filename}",
        media_type=file.content_type
    )
```

## Progress Tracking

```python
from fastapi import FastAPI, UploadFile, File
from fastapi.responses import StreamingResponse
import asyncio

@app.post("/upload/progress")
async def upload_with_progress(file: UploadFile = File(...)):
    """Upload with progress tracking via streaming."""
    
    file_path = UPLOAD_DIR / file.filename
    total_size = 0
    CHUNK_SIZE = 8192
    
    async def upload_generator():
        nonlocal total_size
        
        async with aiofiles.open(file_path, "wb") as f:
            while chunk := await file.read(CHUNK_SIZE):
                await f.write(chunk)
                total_size += len(chunk)
                
                # Yield progress update
                yield f"data: {total_size}\n\n".encode()
    
    return StreamingResponse(
        upload_generator(),
        media_type="text/event-stream",
        headers={
            "X-Content-Type-Options": "nosniff"
        }
    )
```

## Production Considerations

- **File size limits**: Always set limits to prevent disk exhaustion
- **Filename security**: Sanitize filenames to prevent directory traversal attacks
- **Temp cleanup**: Periodically clean up incomplete uploads
- **Virus scanning**: Scan uploaded files before processing
- **Storage costs**: Consider S3/Cloud storage for large files

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Loading large files into memory

**Wrong:**
```python
content = await file.read()  # Loads ENTIRE file into memory!
```

**Why it fails:** Large files cause memory exhaustion.

**Fix:** Stream in chunks:
```python
async with aiofiles.open(path, "wb") as f:
    while chunk := await file.read(8192):
        await f.write(chunk)
```

### ❌ Mistake 2: Not sanitizing filenames

**Wrong:**
```python
file_path = UPLOAD_DIR / file.filename  # Could be "../../etc/passwd"!
```

**Why it fails:** Directory traversal vulnerability.

**Fix:** Sanitize:
```python
import re

safe_name = re.sub(r"[^\w\-.]", "_", file.filename)
safe_name = safe_name[:255]  # Limit length
file_path = UPLOAD_DIR / safe_name
```

### ❌ Mistake 3: Not closing uploaded files

**Wrong:**
```python
data = await file.read()
# Forgot to close?
```

**Why it fails:** Resource leak.

**Fix:** Use context manager or rely on FastAPI to handle:
```python
# FastAPI handles cleanup automatically after request
async with aiofiles.open(path, "wb") as f:
    await f.write(await file.read())
```

## Summary

- Use `UploadFile` for file uploads in FastAPI
- `UploadFile` provides async read, seek, and write methods
- Validate file size and content type before processing
- Stream large files in chunks to avoid memory issues
- Use cloud storage (S3) for production file handling

## Next Steps

This completes the Advanced FastAPI Patterns folder. Continue to `34-advanced-django-patterns/01-class-based-views.md` to explore advanced Django patterns.
