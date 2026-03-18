<!-- FILE: 17_file_storage_and_cloud/01_local_file_handling/03_serving_uploaded_files.md -->

## Overview

Serve uploaded files securely with proper headers and access controls.

## Prerequisites

- Completed file upload guide
- Understanding of Flask responses

## Core Concepts

When serving files, use `send_from_directory()` which prevents directory traversal attacks. Also set proper Content-Disposition headers to prevent execution of malicious files.

## Code Walkthrough

```python
# app.py
from flask import Flask, send_from_directory, abort, make_response
import os

app = Flask(__name__)
UPLOAD_FOLDER = 'uploads'

@app.route('/uploads/<path:filename>')
def serve_upload(filename):
    """Serve uploaded files with security measures."""
    # send_from_directory prevents directory traversal
    response = make_response(
        send_from_directory(UPLOAD_FOLDER, filename)
    )
    
    # Set Content-Disposition to prevent execution
    # inline = show in browser, attachment = force download
    response.headers['Content-Disposition'] = f'inline; filename="{filename}"'
    
    return response

@app.route('/downloads/<path:filename>')
def serve_download(filename):
    """Force download with proper filename."""
    # Get the original filename from database if stored
    original_name = get_original_filename(filename)  # Your DB lookup
    
    return send_from_directory(
        UPLOAD_FOLDER, 
        filename,
        as_attachment=True,
        download_name=original_name  # Show user-friendly name
    )

# For protected files (require authentication)
@app.route('/protected-files/<path:filename>')
@login_required  # Your auth decorator
def serve_protected_file(filename):
    """Serve files only to authenticated users."""
    # Additional check: does user own this file?
    if not user_owns_file(current_user.id, filename):
        abort(403)
    
    return send_from_directory(UPLOAD_FOLDER, filename)
```

### Line-by-Line Breakdown

- `send_from_directory()` ensures files are only served from the upload directory
- `Content-Disposition: inline` shows files in browser when possible
- `Content-Disposition: attachment` forces download
- `download_name` allows showing user-friendly names while serving secure filenames

## Common Mistakes

- ❌ Using `send_file()` with user-controlled paths
- ✅ Using `send_from_directory()` which validates paths

- ❌ Not setting Content-Disposition
- ✅ Setting headers to prevent MIME-sniffing attacks

> **🔒 Security Note:** Always validate user permissions before serving files.

## Quick Reference

| Function | Purpose |
|----------|---------|
| `send_from_directory()` | Serve files safely from a directory |
| `Content-Disposition` | Control how browser handles file |
| `as_attachment` | Force download vs inline display |

## Next Steps

Continue to [02_aws_s3_basics/01_what_is_s3.md](../../18_rate_limiting_and_security/01_security_fundamentals/01_owasp_top_10_for_flask.md) to learn about cloud storage.
