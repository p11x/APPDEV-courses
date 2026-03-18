# File Upload Basics

## What You'll Learn
- Handling file uploads
- File validation
- Secure file storage

## Prerequisites
- Completed API design folder

## Basic File Upload

```python
from fastapi import FastAPI, UploadFile, File, HTTPException
from pathlib import Path
import shutil

app = FastAPI()

UPLOAD_DIR = Path("uploads")
UPLOAD_DIR.mkdir(exist_ok=True)

@app.post("/upload")
async def upload_file(file: UploadFile = File(...)):
    """Upload a single file"""
    # Validate file
    if file.size and file.size > 10_000_000:  # 10MB
        raise HTTPException(status_code=400, detail="File too large")
    
    # Save file
    filepath = UPLOAD_DIR / file.filename
    with filepath.open("wb") as buffer:
        shutil.copyfileobj(file.file, buffer)
    
    return {"filename": file.filename, "size": file.size}
```

## Multiple Files

```python
from typing import List

@app.post("/upload-multiple")
async def upload_multiple(files: List[UploadFile] = File(...)):
    """Upload multiple files"""
    results = []
    
    for file in files:
        filepath = UPLOAD_DIR / file.filename
        with filepath.open("wb") as buffer:
            shutil.copyfileobj(file.file, buffer)
        results.append({"filename": file.filename})
    
    return {"uploaded": results}
```

## File Validation

```python
import magic

ALLOWED_TYPES = {
    "image/jpeg": ".jpg",
    "image/png": ".png",
    "application/pdf": ".pdf"
}
MAX_SIZE = 5_000_000  # 5MB

async def validate_file(file: UploadFile) -> None:
    """Validate file type and size"""
    # Check size
    if file.size and file.size > MAX_SIZE:
        raise HTTPException(status_code=400, detail="File too large")
    
    # Read content to check type
    content = await file.read(2048)
    await file.seek(0)
    
    file_type = magic.from_buffer(content, mime=True)
    
    if file_type not in ALLOWED_TYPES:
        raise HTTPException(
            status_code=400, 
            detail=f"File type {file_type} not allowed"
        )

@app.post("/upload-validated")
async def upload_validated(file: UploadFile = File(...)):
    await validate_file(file)
    
    filepath = UPLOAD_DIR / file.filename
    with filepath.open("wb") as buffer:
        shutil.copyfileobj(file.file, buffer)
    
    return {"filename": file.filename}
```

## Summary
- Validate file size and type
- Use secure filenames
- Store in dedicated directory

## Next Steps
→ Continue to `02-file-downloads.md`
