# Multipart Form Data

## Overview

Multipart form data handles form submissions including files and regular fields. FastAPI provides `Form` for text fields and `File` for uploads.

## Basic Form Data

### Simple Form Submission

```python
# Example 1: Basic form data
from fastapi import FastAPI, Form

app = FastAPI()

@app.post("/login/")
async def login(
    username: str = Form(...),
    password: str = Form(...)
):
    """
    Form data is sent as application/x-www-form-urlencoded
    or multipart/form-data.
    """
    if username == "admin" and password == "secret":
        return {"message": "Login successful"}
    return {"message": "Invalid credentials"}
```

### Form with Validation

```python
# Example 2: Form data validation
from fastapi import FastAPI, Form, HTTPException

app = FastAPI()

@app.post("/register/")
async def register(
    username: str = Form(..., min_length=3, max_length=20),
    email: str = Form(...),
    password: str = Form(..., min_length=8),
    age: int = Form(..., ge=13, le=120)
):
    """
    Form fields support validation like query parameters.
    """
    return {
        "username": username,
        "email": email,
        "message": "Registration successful"
    }
```

## Form with Files

### File and Form Combined

```python
# Example 3: Form with file upload
from fastapi import FastAPI, Form, File, UploadFile
from typing import Optional

app = FastAPI()

@app.post("/upload/profile/")
async def upload_profile(
    name: str = Form(...),
    bio: Optional[str] = Form(None),
    avatar: UploadFile = File(...)
):
    """
    Combines form fields with file upload.
    """
    content = await avatar.read()

    return {
        "name": name,
        "bio": bio,
        "avatar": {
            "filename": avatar.filename,
            "size": len(content)
        }
    }
```

### Multiple Files with Form

```python
# Example 4: Multiple files with form
from fastapi import FastAPI, Form, File, UploadFile
from typing import List

app = FastAPI()

@app.post("/upload/gallery/")
async def upload_gallery(
    title: str = Form(...),
    description: str = Form(""),
    photos: List[UploadFile] = File(...)
):
    """
    Upload multiple files with metadata.
    """
    results = []
    for photo in photos:
        content = await photo.read()
        results.append({
            "filename": photo.filename,
            "size": len(content)
        })

    return {
        "title": title,
        "description": description,
        "photos": results
    }
```

## Form Models

### Pydantic Form Models

```python
# Example 5: Form data with Pydantic
from fastapi import FastAPI, Form, Depends
from pydantic import BaseModel
from typing import Optional

app = FastAPI()

class UserForm(BaseModel):
    username: str
    email: str
    bio: Optional[str] = None

async def get_user_form(
    username: str = Form(...),
    email: str = Form(...),
    bio: Optional[str] = Form(None)
) -> UserForm:
    """Dependency to create model from form data"""
    return UserForm(username=username, email=email, bio=bio)

@app.post("/users/")
async def create_user(user: UserForm = Depends(get_user_form)):
    """
    Use dependency to convert form to Pydantic model.
    """
    return user.model_dump()
```

## Best Practices

### Form Data Guidelines

```python
# Example 6: Best practices
from fastapi import FastAPI, Form, File, UploadFile
from typing import Optional

app = FastAPI()

@app.post("/submit/")
async def submit_form(
    # Required fields first
    name: str = Form(..., min_length=1),
    email: str = Form(...),
    # Optional fields with defaults
    phone: Optional[str] = Form(None),
    # File field
    document: Optional[UploadFile] = File(None)
):
    """
    Best practices:
    1. Mark required fields with ...
    2. Use Optional for nullable fields
    3. Add validation constraints
    4. Document expected format
    """
    result = {"name": name, "email": email}

    if phone:
        result["phone"] = phone
    if document:
        result["document"] = document.filename

    return result
```

## Summary

| Feature | Usage | Example |
|---------|-------|---------|
| Form field | `Form(...)` | `name: str = Form(...)` |
| File upload | `File(...)` | `file: UploadFile = File(...)` |
| Optional | `Form(None)` | `bio: Optional[str] = Form(None)` |
| Validation | Constraints | `age: int = Form(ge=13)` |

## Next Steps

Continue learning about:
- [Streaming Requests](./04_streaming_requests.md) - Large data handling
- [Form Data Advanced](./05_form_data_advanced.md) - Complex forms
- [Responses](../04_responses_and_status_codes/01_default_responses.md) - Response types
