# Input Validation and Sanitization

## What You'll Learn
- Input validation with Pydantic
- Custom validators
- Sanitizing user input
- File upload security

## Prerequisites
- Completed secure password handling

## Pydantic Validation

```python
from pydantic import BaseModel, validator, Field, EmailStr
from typing import Optional

class UserRegistration(BaseModel):
    username: str = Field(..., min_length=3, max_length=30)
    email: EmailStr  # Built-in email validation
    password: str = Field(..., min_length=8)
    age: Optional[int] = Field(None, ge=13, le=120)
    
    @validator('username')
    def validate_username(cls, v):
        if not v.isalnum():
            raise ValueError('Username must be alphanumeric')
        return v.lower()
    
    @validator('password')
    def validate_password(cls, v):
        # Check for common passwords
        common = ['password', '12345678', 'qwerty']
        if v.lower() in common:
            raise ValueError('Password too common')
        return v

# Usage
user = UserRegistration(
    username="JohnDoe",
    email="john@example.com",
    password="SecurePass123!",
    age=25
)
print(user.username)  # "johndoe" (lowercased)
```

## Sanitizing HTML Input

```bash
pip install bleach
```

```python
import bleach

def sanitize_html(dirty: str) -> str:
    """Sanitize HTML to prevent XSS"""
    allowed_tags = ['b', 'i', 'u', 'em', 'strong', 'p', 'br']
    allowed_attributes = {}
    
    return bleach.clean(
        dirty,
        tags=allowed_tags,
        attributes=allowed_attributes,
        strip=True
    )

# Usage
dirty = "<script>alert('xss')</script><b>Hello</b>"
clean = sanitize_html(dirty)
print(clean)  # "<b>Hello</b>" (script removed)
```

## File Upload Security

```python
from fastapi import FastAPI, UploadFile, HTTPException
from fastapi.responses import FileResponse
import os
import magic

app = FastAPI()

ALLOWED_EXTENSIONS = {'.png', '.jpg', '.jpeg', '.gif', '.pdf'}
MAX_FILE_SIZE = 5 * 1024 * 1024  # 5MB

async def validate_file(file: UploadFile) -> None:
    """Validate file type and size"""
    # Check file size
    file.file.seek(0, 2)
    size = file.file.tell()
    file.file.seek(0)
    
    if size > MAX_FILE_SIZE:
        raise HTTPException(status_code=400, detail="File too large")
    
    # Check extension
    ext = os.path.splitext(file.filename)[1].lower()
    if ext not in ALLOWED_EXTENSIONS:
        raise HTTPException(status_code=400, detail="File type not allowed")
    
    # Check actual content type
    content = await file.read(2048)
    await file.seek(0)
    
    content_type = magic.from_buffer(content, mime=True)
    allowed_types = ['image/png', 'image/jpeg', 'application/pdf']
    
    if content_type not in allowed_types:
        raise HTTPException(status_code=400, detail="Invalid file content")

@app.post("/upload")
async def upload_file(file: UploadFile):
    await validate_file(file)
    
    # Save file securely
    upload_dir = "/secure/upload/path"
    filepath = os.path.join(upload_dir, file.filename)
    
    with open(filepath, "wb") as f:
        content = await file.read()
        f.write(content)
    
    return {"filename": file.filename, "size": len(content)}
```

## Command Injection Prevention

```python
import subprocess
import shlex

# VULNERABLE
@app.get("/ping")
def ping(host: str):
    # DON'T do this!
    os.system(f"ping -c 1 {host}")

# SAFE
@app.get("/ping")
def ping(host: str):
    # Use shell=False and pass args as list
    result = subprocess.run(
        ["ping", "-c", "1", host],
        capture_output=True,
        text=True,
        shell=False  # Important!
    )
    return {"output": result.stdout}
```

## Summary
- Use Pydantic for robust input validation
- Sanitize HTML to prevent XSS
- Validate file uploads thoroughly
- Prevent command injection

## Next Steps
→ Continue to `06-api-security-best-practices.md`
