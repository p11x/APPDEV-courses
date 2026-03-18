<!-- FILE: 17_file_storage_and_cloud/03_s3_advanced/01_presigned_urls.md -->

## Overview

Generate presigned URLs for temporary, secure access to private S3 files.

## Prerequisites

- Completed basic S3 upload guide
- Understanding of S3 bucket permissions

## Core Concepts

Presigned URLs grant temporary access to a private S3 object without exposing your AWS credentials. They're useful for serving private files or allowing uploads directly to S3.

## Code Walkthrough

```python
# s3_presigned.py
import boto3
from flask import current_app
from datetime import timedelta

def get_s3_client():
    """Get S3 client."""
    return boto3.client(
        's3',
        aws_access_key_id=current_app.config['AWS_ACCESS_KEY_ID'],
        aws_secret_access_key=current_app.config['AWS_SECRET_ACCESS_KEY'],
        region_name=current_app.config['AWS_REGION']
    )

def generate_presigned_upload_url(filename: str, content_type: str, expires: int = 3600) -> dict:
    """
    Generate presigned URL for direct upload to S3.
    
    This allows clients to upload directly to S3 without going through your server.
    """
    s3 = get_s3_client()
    bucket = current_app.config['AWS_S3_BUCKET']
    
    # Generate unique key
    import uuid
    ext = filename.rsplit('.', 1)[1].lower() if '.' in filename else ''
    s3_key = f"uploads/{uuid.uuid4()}.{ext}"
    
    # Generate presigned URL for PUT (upload)
    presigned_url = s3.generate_presigned_url(
        'put_object',
        Params={
            'Bucket': bucket,
            'Key': s3_key,
            'ContentType': content_type
        },
        ExpiresIn=expires  # seconds
    )
    
    return {
        'upload_url': presigned_url,
        'key': s3_key,
        'expires_in': expires
    }

def generate_presigned_download_url(s3_key: str, filename: str = None, expires: int = 3600) -> str:
    """Generate presigned URL for downloading a private file."""
    s3 = get_s3_client()
    bucket = current_app.config['AWS_S3_BUCKET']
    
    params = {
        'Bucket': bucket,
        'Key': s3_key
    }
    
    # Set Content-Disposition for download
    if filename:
        params['ResponseContentDisposition'] = f'attachment; filename="{filename}"'
    
    return s3.generate_presigned_url(
        'get_object',
        Params=params,
        ExpiresIn=expires
    )

# Flask route for upload
@app.route('/get-upload-url', methods=['POST'])
def get_upload_url():
    data = request.get_json()
    result = generate_presigned_upload_url(
        data['filename'],
        data['content_type']
    )
    return jsonify(result)

# Flask route for download
@app.route('/download/<path:s3_key>')
@login_required
def download_file(s3_key):
    url = generate_presigned_download_url(s3_key)
    return redirect(url)
```

### Line-by-Line Breakdown

- `generate_presigned_url()` creates a temporary URL valid for a limited time
- `put_object` action allows uploading
- `get_object` action allows downloading
- `ExpiresIn` sets URL validity in seconds

> **⚡ Performance Note:** Direct-to-S3 uploads offload traffic from your server, improving scalability.

## Common Mistakes

- ❌ Making S3 public for file access
- ✅ Use presigned URLs for private files

- ❌ Presigned URLs that never expire
- ✅ Set reasonable expiration times (15-60 minutes)

## Quick Reference

| Action | Use Case |
|--------|----------|
| `put_object` | Direct upload to S3 |
| `get_object` | Direct download from S3 |
| `delete_object` | Temporary delete access |

## Next Steps

Continue to [02_s3_access_control.md](./02_s3_access_control.md) to learn about IAM policies.
