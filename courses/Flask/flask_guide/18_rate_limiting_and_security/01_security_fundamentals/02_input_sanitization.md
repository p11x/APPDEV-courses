<!-- FILE: 18_rate_limiting_and_security/01_security_fundamentals/02_input_sanitization.md -->

## Overview

Sanitize user input to prevent injection attacks and data corruption.

## Prerequisites

- Understanding of Flask request handling
- Basic security awareness

## Core Concepts

All user input is potentially malicious. Never trust input directly - always validate and sanitize before using it in queries, HTML, or commands.

## Code Walkthrough

### Basic Input Validation

```python
# validators.py
import re
from urllib.parse import urlparse

def sanitize_string(value: str, max_length: int = 255) -> str:
    """Basic string sanitization."""
    if not isinstance(value, str):
        return ''
    
    # Strip whitespace
    value = value.strip()
    
    # Truncate to max length
    if len(value) > max_length:
        value = value[:max_length]
    
    return value

def validate_email(email: str) -> bool:
    """Validate email format."""
    pattern = r'^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$'
    return bool(re.match(pattern, email))

def validate_username(username: str) -> bool:
    """Validate username format (alphanumeric + underscore)."""
    if not username:
        return False
    return bool(re.match(r'^[a-zA-Z0-9_]{3,20}$', username))

def sanitize_html(dirty_html: str) -> str:
    """Remove dangerous HTML tags (basic sanitization)."""
    # For proper sanitization, use bleach library
    import bleach
    
    allowed_tags = ['b', 'i', 'u', 'p', 'br', 'ul', 'ol', 'li']
    allowed_attrs = {}
    
    return bleach.clean(
        dirty_html,
        tags=allowed_tags,
        attributes=allowed_attrs,
        strip=True
    )

def validate_url(url: str) -> bool:
    """Validate URL is safe (prevent SSRF)."""
    try:
        parsed = urlparse(url)
        # Only allow http and https
        if parsed.scheme not in ('http', 'https'):
            return False
        # Block private IPs
        if parsed.hostname in ('localhost', '127.0.0.1', '0.0.0.0'):
            return False
        return True
    except:
        return False
```

### Using Validators in Flask

```python
# app.py
from flask import Flask, request, jsonify, abort

@app.route('/register', methods=['POST'])
def register():
    data = request.get_json()
    
    # Validate and sanitize username
    username = sanitize_string(data.get('username', ''), 20)
    if not validate_username(username):
        return jsonify({'error': 'Invalid username'}), 400
    
    # Validate email
    email = data.get('email', '').strip().lower()
    if not validate_email(email):
        return jsonify({'error': 'Invalid email'}), 400
    
    # Continue with registration...
    return jsonify({'username': username, 'email': email}), 201
```

### File Upload Validation

```python
# upload validation
ALLOWED_EXTENSIONS = {'png', 'jpg', 'jpeg', 'gif'}
MAX_FILENAME_LENGTH = 255

def validate_uploaded_file(file) -> tuple[bool, str]:
    """Validate uploaded file."""
    # Check filename
    if file.filename == '':
        return False, 'No file selected'
    
    # Check extension
    ext = file.filename.rsplit('.', 1)[-1].lower() if '.' in file.filename else ''
    if ext not in ALLOWED_EXTENSIONS:
        return False, 'File type not allowed'
    
    # Check content type
    if not file.content_type.startswith('image/'):
        return False, 'Invalid content type'
    
    return True, 'Valid'

@app.route('/upload', methods=['POST'])
def upload():
    if 'file' not in request.files:
        return jsonify({'error': 'No file'}), 400
    
    file = request.files['file']
    valid, message = validate_uploaded_file(file)
    
    if not valid:
        return jsonify({'error': message}), 400
    
    # Process upload...
    return jsonify({'message': 'Uploaded'}), 201
```

> **🔒 Security Note:** Always validate input on the server side. Client-side validation is for UX only.

## Common Mistakes

- ❌ Trusting client-side validation only
- ✅ Always validate on server

- ❌ Using user input directly in queries
- ✅ Sanitize and use ORM

- ❌ Allowing any file type for uploads
- ✅ Whitelist allowed extensions

## Quick Reference

| Input Type | Validation |
|------------|------------|
| Email | Regex pattern |
| Username | Whitelist allowed chars |
| URL | Parse and validate scheme |
| File | Check extension + content type |

## Next Steps

Continue to [03_sql_injection_prevention.md](./03_sql_injection_prevention.md) to learn about SQL injection prevention.
