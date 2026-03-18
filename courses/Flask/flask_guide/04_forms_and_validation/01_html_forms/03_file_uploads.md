<!-- FILE: 04_forms_and_validation/01_html_forms/03_file_uploads.md -->

## Overview

File uploads are a common feature in web applications. Flask handles file uploads through the `request.files` object. This file covers uploading files securely: validating file types, limiting file sizes, saving files safely, and best practices.

## Prerequisites

- Understanding of HTML forms
- Basic Flask request handling
- Familiarity with file operations

## Core Concepts

### Form Encoding for Files

To upload files, the form must use `enctype="multipart/form-data"`:

```html
<form action="/upload" method="POST" enctype="multipart/form-data">
    <input type="file" name="file">
</form>
```

### Accessing Uploaded Files

```python
file = request.files["file"]
filename = file.filename
content = file.read()
```

### Security Considerations

- Always validate file types
- Limit file sizes
- Use secure filenames
- Never execute uploaded files

## Code Walkthrough

### Basic File Upload

```python
# app.py — Basic file upload
from flask import Flask, request, render_template_string
import os

app = Flask(__name__)

# Configure upload folder
UPLOAD_FOLDER = "uploads"
app.config["UPLOAD_FOLDER"] = UPLOAD_FOLDER
app.config["MAX_CONTENT_LENGTH"] = 16 * 1024 * 1024  # 16MB max

# Create upload folder
os.makedirs(UPLOAD_FOLDER, exist_ok=True)

@app.route("/")
def index():
    return render_template_string('''
<!DOCTYPE html>
<html>
<head><title>File Upload</title></head>
<body>
    <h1>Upload a File</h1>
    <form method="POST" enctype="multipart/form-data">
        <input type="file" name="file" required>
        <button type="submit">Upload</button>
    </form>
</body>
</html>
    ''')

@app.route("/", methods=["POST"])
def upload_file():
    # Check if file part exists
    if "file" not in request.files:
        return "No file part", 400
    
    file = request.files["file"]
    
    # Check if file was selected
    if file.filename == "":
        return "No selected file", 400
    
    # Save the file
    file.save(os.path.join(app.config["UPLOAD_FOLDER"], file.filename))
    
    return f"File {file.filename} uploaded successfully!"

if __name__ == "__main__":
    app.run(debug=True)
```

### Secure File Upload

```python
# app.py — Secure file upload
from flask import Flask, request, render_template_string, redirect, url_for
import os
import uuid
from werkzeug.utils import secure_filename

app = Flask(__name__)

UPLOAD_FOLDER = "uploads"
ALLOWED_EXTENSIONS = {"txt", "pdf", "png", "jpg", "jpeg", "gif"}
MAX_CONTENT_LENGTH = 16 * 1024 * 1024  # 16MB

app.config["UPLOAD_FOLDER"] = UPLOAD_FOLDER
app.config["MAX_CONTENT_LENGTH"] = MAX_CONTENT_LENGTH

os.makedirs(UPLOAD_FOLDER, exist_ok=True)

def allowed_file(filename):
    """Check if file extension is allowed."""
    return "." in filename and filename.rsplit(".", 1)[1].lower() in ALLOWED_EXTENSIONS

@app.route("/upload", methods=["GET", "POST"])
def upload():
    if request.method == "POST":
        if "file" not in request.files:
            return "No file part", 400
        
        file = request.files["file"]
        
        if file.filename == "":
            return "No selected file", 400
        
        if file and allowed_file(file.filename):
            # Secure the filename (prevents directory traversal)
            filename = secure_filename(file.filename)
            
            # Generate unique filename to prevent overwrites
            ext = filename.rsplit(".", 1)[1].lower()
            unique_filename = f"{uuid.uuid4().hex}.{ext}"
            
            # Save file
            filepath = os.path.join(app.config["UPLOAD_FOLDER"], unique_filename)
            file.save(filepath)
            
            return f"File uploaded: {unique_filename}"
        
        return "File type not allowed", 400
    
    return render_template_string('''
<!DOCTYPE html>
<html>
<body>
    <h1>Secure Upload</h1>
    <form method="POST" enctype="multipart/form-data">
        <input type="file" name="file" accept=".txt,.pdf,.png,.jpg,.jpeg,.gif">
        <button type="submit">Upload</button>
    </form>
</body>
</html>
    ''')

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `enctype="multipart/form-data"` — Required for file uploads.
- `request.files["file"]` — Gets the uploaded file.
- `secure_filename()` — Removes dangerous characters from filename.
- `uuid.uuid4().hex` — Generates unique ID to prevent overwrites.
- `file.save(filepath)` — Saves file to disk.

## Common Mistakes

❌ **Not using multipart/form-data**
```html
<!-- WRONG — File upload won't work -->
<form method="POST">
    <input type="file" name="file">
</form>
```

✅ **Correct — Use enctype**
```html
<!-- CORRECT -->
<form method="POST" enctype="multipart/form-data">
```

❌ **Not validating file types**
```python
# WRONG — Any file can be uploaded
file.save(filepath)
```

✅ **Correct — Validate file types**
```python
# CORRECT — Check extension
if allowed_file(filename):
    file.save(filepath)
```

❌ **Using original filename**
```python
# WRONG — Could contain malicious paths
file.save(file.filename)
```

✅ **Correct — Use secure_filename**
```python
# CORRECT
filename = secure_filename(file.filename)
```

## Quick Reference

| Task | Code |
|------|------|
| Get file | `request.files["file"]` |
| Save file | `file.save(path)` |
| Check filename | `secure_filename(name)` |
| Validate type | `allowed_file(name)` |

## Next Steps

Now you can handle file uploads. Continue to [01_installing_flask_wtf.md](../../04_forms_and_validation/02_wtforms/01_installing_flask_wtf.md) to learn about Flask-WTF for form handling.