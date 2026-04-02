# Form Data Advanced

## Overview

Advanced form data handling covers complex scenarios including dynamic forms, form validation, file processing, and integration with databases.

## Complex Form Handling

### Dynamic Form Fields

```python
# Example 1: Dynamic form processing
from fastapi import FastAPI, Form, HTTPException
from typing import Dict, Any
import json

app = FastAPI()

@app.post("/submit/dynamic/")
async def dynamic_form(
    form_data: str = Form(...)
):
    """
    Accept JSON-encoded form data for dynamic fields.
    """
    try:
        data = json.loads(form_data)
    except json.JSONDecodeError:
        raise HTTPException(400, "Invalid JSON")

    return {"received": data}
```

### Form with Lists

```python
# Example 2: Form data with lists
from fastapi import FastAPI, Form
from typing import List

app = FastAPI()

@app.post("/submit/tags/")
async def submit_with_tags(
    title: str = Form(...),
    tags: List[str] = Form(...)
):
    """
    Accept multiple values for same field name.
    """
    return {
        "title": title,
        "tags": tags,
        "tag_count": len(tags)
    }
```

## Form Validation Patterns

### Custom Form Validation

```python
# Example 3: Advanced form validation
from fastapi import FastAPI, Form, HTTPException
from pydantic import BaseModel, EmailStr, field_validator
from typing import Optional

app = FastAPI()

class RegistrationForm(BaseModel):
    username: str
    email: str
    password: str
    confirm_password: str

    @field_validator('username')
    @classmethod
    def validate_username(cls, v):
        if not v.isalnum():
            raise ValueError('Username must be alphanumeric')
        return v

    @field_validator('confirm_password')
    @classmethod
    def passwords_match(cls, v, info):
        if 'password' in info.data and v != info.data['password']:
            raise ValueError('Passwords do not match')
        return v

@app.post("/register/advanced/")
async def advanced_register(
    username: str = Form(...),
    email: str = Form(...),
    password: str = Form(..., min_length=8),
    confirm_password: str = Form(...)
):
    """
    Advanced form validation with cross-field checks.
    """
    form = RegistrationForm(
        username=username,
        email=email,
        password=password,
        confirm_password=confirm_password
    )

    return {"username": form.username, "email": form.email}
```

## Form Processing Patterns

### CSV Upload Processing

```python
# Example 4: CSV file processing
from fastapi import FastAPI, File, UploadFile, HTTPException
import csv
import io

app = FastAPI()

@app.post("/upload/csv/")
async def upload_csv(file: UploadFile = File(...)):
    """
    Process CSV file upload.
    """
    if not file.filename.endswith('.csv'):
        raise HTTPException(400, "File must be CSV")

    content = await file.read()
    text = content.decode('utf-8')

    reader = csv.DictReader(io.StringIO(text))
    rows = list(reader)

    return {
        "filename": file.filename,
        "rows": len(rows),
        "columns": reader.fieldnames,
        "preview": rows[:5]
    }
```

### Image Processing

```python
# Example 5: Image upload processing
from fastapi import FastAPI, File, UploadFile, HTTPException
from PIL import Image
import io

app = FastAPI()

@app.post("/upload/image/")
async def upload_image(file: UploadFile = File(...)):
    """
    Process and validate image uploads.
    """
    if not file.content_type.startswith('image/'):
        raise HTTPException(400, "File must be an image")

    content = await file.read()

    try:
        image = Image.open(io.BytesIO(content))
        width, height = image.size
    except Exception:
        raise HTTPException(400, "Invalid image file")

    return {
        "filename": file.filename,
        "format": image.format,
        "size": {"width": width, "height": height}
    }
```

## Form Data with Database

### Save Form to Database

```python
# Example 6: Form data persistence
from fastapi import FastAPI, Form, Depends
from sqlalchemy.orm import Session
from datetime import datetime

app = FastAPI()

# Simulated database
class UserDB:
    def __init__(self):
        self.users = []
        self.id_counter = 0

    def add(self, user_data: dict):
        self.id_counter += 1
        user_data['id'] = self.id_counter
        user_data['created_at'] = datetime.now()
        self.users.append(user_data)
        return user_data

db = UserDB()

@app.post("/users/create/")
async def create_user_form(
    username: str = Form(...),
    email: str = Form(...),
    bio: str = Form("")
):
    """
    Save form data to database.
    """
    user = db.add({
        "username": username,
        "email": email,
        "bio": bio
    })

    return user
```

## Best Practices

### Form Handling Guidelines

```python
# Example 7: Best practices
from fastapi import FastAPI, Form, File, UploadFile
from typing import Optional

app = FastAPI()

@app.post("/submit/best-practice/")
async def best_practice_form(
    # Required fields first
    name: str = Form(..., min_length=1, max_length=100),
    email: str = Form(...),
    # Optional fields
    phone: Optional[str] = Form(None),
    # File with validation
    document: Optional[UploadFile] = File(None)
):
    """
    Best practices:
    1. Validate all inputs
    2. Handle optional fields
    3. Provide clear error messages
    4. Sanitize file uploads
    """
    result = {"name": name, "email": email}

    if phone:
        result["phone"] = phone

    if document:
        # Validate file type
        allowed = {"application/pdf", "image/jpeg", "image/png"}
        if document.content_type not in allowed:
            return {"error": "Invalid file type"}

        content = await document.read()
        result["document"] = {
            "filename": document.filename,
            "size": len(content)
        }

    return result
```

## Summary

| Feature | Usage | Example |
|---------|-------|---------|
| Dynamic forms | JSON in form | `form_data: str = Form(...)` |
| Lists | Multiple values | `tags: List[str] = Form(...)` |
| File processing | UploadFile | CSV, image processing |
| Validation | Cross-field | Password confirmation |

## Next Steps

Continue learning about:
- [Responses](../04_responses_and_status_codes/01_default_responses.md) - Response types
- [Custom Status Codes](../04_responses_and_status_codes/02_custom_status_codes.md) - HTTP status
- [Response Models](../04_responses_and_status_codes/03_response_models.md) - Output validation
