# File Upload Security

## Overview

File uploads are a common attack vector. Proper validation and handling are essential for security.

## Secure File Uploads

### File Validation

```python
# Example 1: Secure file upload handling
from fastapi import FastAPI, File, UploadFile, HTTPException
from pathlib import Path
import magic
import hashlib

app = FastAPI()

ALLOWED_EXTENSIONS = {'.jpg', '.jpeg', '.png', '.pdf', '.docx'}
ALLOWED_MIME_TYPES = {
    'image/jpeg', 'image/png',
    'application/pdf',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
}
MAX_FILE_SIZE = 10 * 1024 * 1024  # 10MB

def validate_file_extension(filename: str) -> bool:
    """Validate file extension"""
    ext = Path(filename).suffix.lower()
    return ext in ALLOWED_EXTENSIONS

async def validate_file_content(file: UploadFile) -> bool:
    """Validate file content using magic numbers"""
    content = await file.read(1024)  # Read first 1KB
    await file.seek(0)  # Reset file position

    mime = magic.from_buffer(content, mime=True)
    return mime in ALLOWED_MIME_TYPES

def generate_safe_filename(original: str) -> str:
    """Generate safe filename"""
    import uuid
    ext = Path(original).suffix.lower()
    return f"{uuid.uuid4()}{ext}"

@app.post("/upload/")
async def upload_file(file: UploadFile = File(...)):
    """Secure file upload"""
    
    # Validate extension
    if not validate_file_extension(file.filename):
        raise HTTPException(400, "File type not allowed")
    
    # Validate content
    if not await validate_file_content(file):
        raise HTTPException(400, "File content type not allowed")
    
    # Check size
    content = await file.read()
    if len(content) > MAX_FILE_SIZE:
        raise HTTPException(400, "File too large")
    
    # Generate safe filename
    safe_filename = generate_safe_filename(file.filename)
    
    # Save file
    upload_dir = Path("uploads")
    upload_dir.mkdir(exist_ok=True)
    
    file_path = upload_dir / safe_filename
    file_path.write_bytes(content)
    
    return {
        "filename": safe_filename,
        "size": len(content),
        "content_type": file.content_type
    }
```

## Best Practices

1. Validate file extensions
2. Verify file content (magic numbers)
3. Limit file sizes
4. Use safe filenames
5. Store files outside web root
6. Scan for malware

## Summary

Secure file handling prevents directory traversal, arbitrary code execution, and other attacks.

## Next Steps

Continue learning about:
- [API Security Headers](./07_api_security_headers.md)
- [Security Testing](../07_security_testing/01_authentication_testing.md)
