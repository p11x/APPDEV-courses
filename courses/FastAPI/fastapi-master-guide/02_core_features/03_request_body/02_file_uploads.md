# File Uploads

## Overview

FastAPI provides robust file upload handling for single files, multiple files, and files with metadata. This guide covers all file upload patterns.

## Single File Upload

### Basic File Upload

```python
# Example 1: Single file upload
from fastapi import FastAPI, File, UploadFile
import shutil
from pathlib import Path

app = FastAPI()

# Create uploads directory
UPLOAD_DIR = Path("uploads")
UPLOAD_DIR.mkdir(exist_ok=True)

@app.post("/upload/")
async def upload_file(file: UploadFile = File(...)):
    """
    UploadFile provides:
    - filename: Original filename
    - content_type: MIME type
    - file: File-like object
    - read(): Read file contents
    """
    # Read file metadata
    content = await file.read()

    # Save file
    file_path = UPLOAD_DIR / file.filename
    with open(file_path, "wb") as buffer:
        buffer.write(content)

    return {
        "filename": file.filename,
        "content_type": file.content_type,
        "size": len(content)
    }
```

### File Validation

```python
# Example 2: File type validation
from fastapi import FastAPI, File, UploadFile, HTTPException

app = FastAPI()

ALLOWED_TYPES = {"image/jpeg", "image/png", "image/gif"}
MAX_SIZE = 5 * 1024 * 1024  # 5MB

@app.post("/upload/image/")
async def upload_image(file: UploadFile = File(...)):
    """
    Validate file type and size.
    """
    # Check content type
    if file.content_type not in ALLOWED_TYPES:
        raise HTTPException(
            400,
            f"File type {file.content_type} not allowed"
        )

    # Check file size
    content = await file.read()
    if len(content) > MAX_SIZE:
        raise HTTPException(400, "File too large (max 5MB)")

    return {
        "filename": file.filename,
        "size": len(content),
        "content_type": file.content_type
    }
```

## Multiple File Upload

### Upload Multiple Files

```python
# Example 3: Multiple file upload
from fastapi import FastAPI, File, UploadFile
from typing import List

app = FastAPI()

@app.post("/upload/multiple/")
async def upload_multiple(files: List[UploadFile] = File(...)):
    """
    Upload multiple files at once.
    """
    results = []
    for file in files:
        content = await file.read()
        results.append({
            "filename": file.filename,
            "size": len(content)
        })

    return {
        "count": len(files),
        "files": results
    }
```

## File with Metadata

### Upload File with Form Data

```python
# Example 4: File with additional metadata
from fastapi import FastAPI, File, UploadFile, Form
from typing import Optional

app = FastAPI()

@app.post("/upload/document/")
async def upload_document(
    file: UploadFile = File(...),
    title: str = Form(...),
    description: Optional[str] = Form(None),
    tags: str = Form("")  # Comma-separated tags
):
    """
    Combine file upload with form fields.
    Uses multipart/form-data.
    """
    content = await file.read()

    return {
        "title": title,
        "description": description,
        "tags": [t.strip() for t in tags.split(",") if t.strip()],
        "file": {
            "filename": file.filename,
            "size": len(content)
        }
    }
```

## Streaming Large Files

### Chunked Upload

```python
# Example 5: Streaming large files
from fastapi import FastAPI, File, UploadFile
import aiofiles

app = FastAPI()

@app.post("/upload/large/")
async def upload_large_file(file: UploadFile = File(...)):
    """
    Stream large files without loading entirely into memory.
    """
    file_path = f"uploads/{file.filename}"

    async with aiofiles.open(file_path, "wb") as buffer:
        while chunk := await file.read(1024 * 1024):  # 1MB chunks
            await buffer.write(chunk)

    return {"filename": file.filename}
```

## Download Files

### File Download Endpoint

```python
# Example 6: File download
from fastapi import FastAPI, HTTPException
from fastapi.responses import FileResponse
from pathlib import Path

app = FastAPI()

@app.get("/download/{filename}")
async def download_file(filename: str):
    """
    Download a file by name.
    """
    file_path = Path("uploads") / filename

    if not file_path.exists():
        raise HTTPException(404, "File not found")

    return FileResponse(
        path=file_path,
        filename=filename,
        media_type="application/octet-stream"
    )
```

## Best Practices

### File Upload Guidelines

```python
# Example 7: Best practices
from fastapi import FastAPI, File, UploadFile, HTTPException
from pathlib import Path
import uuid

app = FastAPI()

UPLOAD_DIR = Path("uploads")
UPLOAD_DIR.mkdir(exist_ok=True)

# Allowed extensions
ALLOWED_EXTENSIONS = {".jpg", ".jpeg", ".png", ".pdf", ".docx"}
MAX_FILE_SIZE = 10 * 1024 * 1024  # 10MB

@app.post("/upload/safe/")
async def safe_upload(file: UploadFile = File(...)):
    """
    Best practices for file uploads:
    1. Validate file extension
    2. Check file size
    3. Sanitize filename
    4. Use unique filenames
    """
    # Validate extension
    ext = Path(file.filename).suffix.lower()
    if ext not in ALLOWED_EXTENSIONS:
        raise HTTPException(400, f"Extension {ext} not allowed")

    # Read and check size
    content = await file.read()
    if len(content) > MAX_FILE_SIZE:
        raise HTTPException(400, "File too large")

    # Generate unique filename
    safe_filename = f"{uuid.uuid4()}{ext}"
    file_path = UPLOAD_DIR / safe_filename

    # Save file
    with open(file_path, "wb") as buffer:
        buffer.write(content)

    return {
        "original_name": file.filename,
        "saved_as": safe_filename,
        "size": len(content)
    }
```

## Summary

| Feature | Usage | Example |
|---------|-------|---------|
| Single file | `UploadFile` | `file: UploadFile = File(...)` |
| Multiple files | `List[UploadFile]` | `files: List[UploadFile]` |
| With metadata | `Form(...)` | `title: str = Form(...)` |
| Streaming | Chunked reading | `await file.read(1024)` |
| Download | `FileResponse` | `FileResponse(path)` |

## Next Steps

Continue learning about:
- [Multipart Form Data](./03_multipart_form_data.md) - Form handling
- [Streaming Requests](./04_streaming_requests.md) - Large data
- [Responses](../04_responses_and_status_codes/01_default_responses.md) - Response types
