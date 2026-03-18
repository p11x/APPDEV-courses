<!-- FILE: 17_file_storage_and_cloud/01_local_file_handling/01_secure_file_uploads.md -->

## Overview

Handle file uploads securely in Flask applications.

## Prerequisites

- Basic Flask knowledge
- Understanding of HTTP requests

## Core Concepts

File uploads require careful security considerations. Never trust the filename provided by the client - use secure random filenames and validate file contents.

## Code Walkthrough

```python
# app.py
import os
import uuid
from flask import Flask, request, send_from_directory
from werkzeug.utils import secure_filename

app = Flask(__name__)

# Secure configuration
UPLOAD_FOLDER = os.path.join(os.getcwd(), 'uploads')
ALLOWED_EXTENSIONS = {'txt', 'pdf', 'png', 'jpg', 'jpeg', 'gif'}
MAX_CONTENT_LENGTH = 16 * 1024 * 1024  # 16MB max

app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER
app.config['MAX_CONTENT_LENGTH'] = MAX_CONTENT_LENGTH

def allowed_file(filename: str) -> bool:
    """Check if file extension is allowed."""
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

def secure_filename_gen(filename: str) -> str:
    """Generate a secure unique filename."""
    ext = filename.rsplit('.', 1)[1].lower() if '.' in filename else ''
    unique_name = str(uuid.uuid4())
    return f"{unique_name}.{ext}" if ext else unique_name

@app.route('/upload', methods=['POST'])
def upload_file():
    """Handle file upload securely."""
    if 'file' not in request.files:
        return {'error': 'No file part'}, 400
    
    file = request.files['file']
    
    if file.filename == '':
        return {'error': 'No selected file'}, 400
    
    if file and allowed_file(file.filename):
        # Generate secure filename
        filename = secure_filename_gen(secure_filename(file.filename))
        filepath = os.path.join(app.config['UPLOAD_FOLDER'], filename)
        
        # Ensure upload directory exists
        os.makedirs(app.config['UPLOAD_FOLDER'], exist_ok=True)
        
        # Save file
        file.save(filepath)
        
        return {'filename': filename, 'url': f'/uploads/{filename}'}, 201
    
    return {'error': 'File type not allowed'}, 400

@app.route('/uploads/<filename>')
def uploaded_file(filename):
    """Serve uploaded files."""
    return send_from_directory(app.config['UPLOAD_FOLDER'], filename)
```

### Line-by-Line Breakdown

- `uuid.uuid4()` generates a unique random identifier to prevent filename collisions
- `secure_filename()` sanitizes the original filename, removing dangerous characters
- `ALLOWED_EXTENSIONS` whitelist prevents uploading executable files
- `MAX_CONTENT_LENGTH` limits upload size to prevent DoS attacks

## Common Mistakes

- ❌ Using the original filename from the request
- ✅ Generating a unique secure filename with UUID

- ❌ Allowing all file extensions
- ✅ Using a whitelist of allowed extensions

## Quick Reference

| Function | Purpose |
|----------|---------|
| `secure_filename()` | Sanitize client-provided filename |
| `uuid.uuid4()` | Generate unique identifiers |
| `allowed_file()` | Check extension whitelist |
| `send_from_directory()` | Serve uploaded files safely |

## Next Steps

Continue to [02_validating_file_types.md](../02_validating_file_types.md) to learn about MIME type validation.
