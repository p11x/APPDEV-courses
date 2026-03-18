<!-- FILE: 17_file_storage_and_cloud/05_alternative_storage/01_google_cloud_storage.md -->

## Overview

Use Google Cloud Storage (GCS) as an alternative to AWS S3 for file storage.

## Prerequisites

- Google Cloud Platform account
- Basic understanding of cloud storage

## Core Concepts

Google Cloud Storage is S3's primary competitor. It offers similar functionality with different pricing and regional options. Use the `google-cloud-storage` Python library.

## Code Walkthrough

### Installation

```bash
pip install google-cloud-storage
```

### Configuration

```python
# config.py
class Config:
    GCS_BUCKET = os.environ.get('GCS_BUCKET', 'your-bucket-name')
    GOOGLE_APPLICATION_CREDENTIALS = os.environ.get(
        'GOOGLE_APPLICATION_CREDENTIALS', 
        'path/to/your/service-account.json'
    )
```

### Storage Client

```python
# gcs_utils.py
from google.cloud import storage
from flask import current_app
import uuid

def get_gcs_client():
    """Get Google Cloud Storage client."""
    return storage.Client()

def get_gcs_bucket():
    """Get the configured GCS bucket."""
    client = get_gcs_client()
    return client.bucket(current_app.config['GCS_BUCKET'])

def upload_to_gcs(file_data, filename: str, content_type: str = None) -> dict:
    """Upload file to Google Cloud Storage."""
    bucket = get_gcs_bucket()
    
    # Generate unique blob name
    blob_name = f"uploads/{uuid.uuid4()}_{filename}"
    blob = bucket.blob(blob_name)
    
    # Upload
    blob.upload_from_string(
        file_data,
        content_type=content_type or 'application/octet-stream'
    )
    
    # Make public (or keep private and use signed URLs)
    blob.make_public()
    
    return {
        'url': blob.public_url,
        'blob_name': blob_name,
        'bucket': current_app.config['GCS_BUCKET']
    }

def generate_signed_url(blob_name: str, expiration_minutes: int = 60) -> str:
    """Generate signed URL for private files."""
    bucket = get_gcs_bucket()
    blob = bucket.blob(blob_name)
    
    url = blob.generate_signed_url(
        version="v4",
        expiration=expiration_minutes * 60,
        method="GET"
    )
    
    return url
```

### Flask Integration

```python
# app.py
from flask import Flask, request, jsonify

@app.route('/upload', methods=['POST'])
def upload():
    if 'file' not in request.files:
        return jsonify({'error': 'No file'}), 400
    
    file = request.files['file']
    file_data = file.read()
    
    result = upload_to_gcs(
        file_data, 
        file.filename,
        file.content_type
    )
    
    return jsonify(result), 201

@app.route('/download/<blob_name>')
def download(blob_name):
    url = generate_signed_url(blob_name)
    return redirect(url)
```

### Line-by-Line Breakdown

- `storage.Client()` creates GCS client using credentials
- `bucket.blob()` creates a blob reference (like S3 object)
- `upload_from_string()` uploads data to GCS
- `generate_signed_url()` creates temporary access URLs

> **💡 Tip:** Use Google Cloud CDN (similar to CloudFront) for better global performance.

## Common Mistakes

- ❌ Using bucket names with dots in URL
- ✅ Use properly formatted bucket names

- ❌ Not setting content type
- ✅ Always specify content type for proper handling

## Quick Reference

| GCS Term | S3 Equivalent |
|----------|---------------|
| Bucket | Bucket |
| Blob | Object |
| Project | Account |
| Region | Region |

## Next Steps

Continue to [02_cloudinary_integration.md](./02_cloudinary_integration.md) to learn about Cloudinary.
