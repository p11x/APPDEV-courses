<!-- FILE: 17_file_storage_and_cloud/02_aws_s3_basics/03_uploading_files_to_s3.md -->

## Overview

Upload files from Flask to AWS S3 using boto3.

## Prerequisites

- boto3 installed and configured
- S3 bucket created

## Core Concepts

Uploading to S3 involves reading the file and sending it to S3 with appropriate metadata. Use multipart upload for large files.

## Code Walkthrough

```python
# s3_utils.py
import boto3
import uuid
import os
from flask import current_app
from werkzeug.utils import secure_filename

def get_s3_client():
    """Get S3 client from Flask app config."""
    return boto3.client(
        's3',
        aws_access_key_id=current_app.config['AWS_ACCESS_KEY_ID'],
        aws_secret_access_key=current_app.config['AWS_SECRET_ACCESS_KEY'],
        region_name=current_app.config['AWS_REGION']
    )

def generate_s3_key(filename: str, folder: str = 'uploads') -> str:
    """Generate unique S3 key for file."""
    ext = os.path.splitext(filename)[1].lower() if '.' in filename else ''
    unique_id = str(uuid.uuid4())
    return f"{folder}/{unique_id}{ext}"

def upload_file_to_s3(file, filename: str, folder: str = 'uploads') -> dict:
    """
    Upload a file object to S3.
    
    Args:
        file: File object from request.files
        filename: Original filename
        folder: S3 folder prefix
    
    Returns:
        dict with 'url', 'key', 'bucket'
    """
    s3 = get_s3_client()
    bucket = current_app.config['AWS_S3_BUCKET']
    
    # Generate unique key
    s3_key = generate_s3_key(filename, folder)
    
    # Determine content type
    content_type = file.content_type or 'application/octet-stream'
    
    # Upload file
    s3.upload_fileobj(
        file,
        bucket,
        s3_key,
        ExtraArgs={
            'ContentType': content_type,
            'Metadata': {
                'original-filename': secure_filename(filename)
            }
        }
    )
    
    # Generate URL
    url = f"https://{bucket}.s3.{current_app.config['AWS_REGION']}.amazonaws.com/{s3_key}"
    
    return {
        'url': url,
        'key': s3_key,
        'bucket': bucket,
        'content_type': content_type
    }

def upload_local_file(filepath: str, s3_key: str = None) -> dict:
    """Upload a local file to S3."""
    s3 = get_s3_client()
    bucket = current_app.config['AWS_S3_BUCKET']
    
    if s3_key is None:
        s3_key = generate_s3_key(os.path.basename(filepath))
    
    # Determine content type from extension
    content_type = 'application/octet-stream'
    ext = os.path.splitext(filepath)[1].lower()
    content_types = {
        '.jpg': 'image/jpeg',
        '.jpeg': 'image/jpeg',
        '.png': 'image/png',
        '.gif': 'image/gif',
        '.pdf': 'application/pdf',
        '.txt': 'text/plain',
    }
    content_type = content_types.get(ext, content_type)
    
    s3.upload_file(
        filepath,
        bucket,
        s3_key,
        ExtraArgs={'ContentType': content_type}
    )
    
    return {
        'key': s3_key,
        'bucket': bucket,
        'url': f"https://{bucket}.s3.{current_app.config['AWS_REGION']}.amazonaws.com/{s3_key}"
    }
```

### Flask Route

```python
# app.py
from flask import Flask, request, jsonify

@app.route('/upload', methods=['POST'])
def upload():
    if 'file' not in request.files:
        return jsonify({'error': 'No file'}), 400
    
    file = request.files['file']
    
    if file.filename == '':
        return jsonify({'error': 'No file selected'}), 400
    
    try:
        result = upload_file_to_s3(file, file.filename)
        return jsonify(result), 201
    except Exception as e:
        return jsonify({'error': str(e)}), 500
```

### Line-by-Line Breakdown

- `upload_fileobj()` uploads a file object directly to S3
- `ExtraArgs` allows setting content type and metadata
- `generate_s3_key()` creates unique keys to prevent collisions

> **⚡ Performance Note:** For files larger than 100MB, use multipart upload with `upload_part()`.

## Common Mistakes

- ❌ Not setting ContentType
- ✅ Set correct ContentType for proper browser handling

- ❌ Using original filenames as S3 keys
- ✅ Generate unique keys with UUID

## Quick Reference

| Function | Purpose |
|----------|---------|
| `upload_fileobj()` | Upload file object |
| `upload_file()` | Upload local file path |
| `generate_s3_key()` | Create unique key |

## Next Steps

Continue to [03_s3_advanced/01_presigned_urls.md](../03_s3_advanced/01_presigned_urls.md) to learn about presigned URLs.
