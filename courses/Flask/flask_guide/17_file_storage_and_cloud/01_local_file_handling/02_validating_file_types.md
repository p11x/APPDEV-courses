<!-- FILE: 17_file_storage_and_cloud/01_local_file_handling/02_validating_file_types.md -->

## Overview

Validate file types by checking MIME content, not just extensions.

## Prerequisites

- Completed secure file uploads guide
- Understanding of Flask request objects

## Core Concepts

File extensions can be easily spoofed. Always validate the actual file content (MIME type) server-side using Python's `magic` library or `imghdr` for images.

## Code Walkthrough

```python
# validators.py
import imghdr
import magic

def validate_image_content(file_path: str) -> bool:
    """Validate that an image is actually an image by checking content."""
    # Check with imghdr (simple)
    img_type = imghdr.what(file_path)
    return img_type is not None

def validate_mime_type(file_path: str) -> str:
    """Get the actual MIME type from file content."""
    # Use python-magic for more accurate detection
    mime = magic.Magic(mime=True)
    return mime.from_file(file_path)

ALLOWED_MIME_TYPES = {
    'image/jpeg',
    'image/png',
    'image/gif',
    'application/pdf',
    'text/plain'
}

def validate_file_mime(file_path: str) -> bool:
    """Validate file MIME type against whitelist."""
    mime_type = validate_mime_type(file_path)
    return mime_type in ALLOWED_MIME_TYPES

# Usage in upload route
@app.route('/upload', methods=['POST'])
def upload_file():
    file = request.files['file']
    
    # Save temporarily
    temp_path = os.path.join(app.config['UPLOAD_FOLDER'], 'temp_' + filename)
    file.save(temp_path)
    
    # Validate content
    if not validate_file_mime(temp_path):
        os.remove(temp_path)
        return {'error': 'Invalid file type'}, 400
    
    # If valid, rename to final secure name
    final_name = secure_filename_gen(original_filename)
    final_path = os.path.join(app.config['UPLOAD_FOLDER'], final_name)
    os.rename(temp_path, final_path)
    
    return {'filename': final_name}, 201
```

### Line-by-Line Breakdown

- `imghdr.what()` checks image file headers to verify it's a valid image
- `magic.Magic()` provides more comprehensive MIME type detection
- We validate AFTER saving but BEFORE finalizing the upload
- If validation fails, we clean up the temporary file

## Common Mistakes

- ❌ Trusting the `Content-Type` header from the browser
- ✅ Always checking file content server-side

- ❌ Only checking file extension
- ✅ Using MIME type validation with content inspection

> **⚠️ Warning:** Never trust client-provided MIME types. They can be easily manipulated.

## Quick Reference

| Method | Accuracy | Use Case |
|--------|----------|----------|
| `imghdr.what()` | Medium | Image files only |
| `python-magic` | High | All file types |
| Extension check | Low | Never use alone |

## Next Steps

Continue to [03_serving_uploaded_files.md](../03_serving_uploaded_files.md) to learn about serving uploaded files.
