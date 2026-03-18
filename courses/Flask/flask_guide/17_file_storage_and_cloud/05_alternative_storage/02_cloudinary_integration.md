<!-- FILE: 17_file_storage_and_cloud/05_alternative_storage/02_cloudinary_integration.md -->

## Overview

Use Cloudinary for automatic image optimization, transformation, and CDN delivery.

## Prerequisites

- Cloudinary account (free tier available)
- Understanding of image handling

## Core Concepts

Cloudinary is a cloud-based image management service. It handles uploads, storage, transformations, and CDN delivery. Key feature: on-the-fly image transformations via URL parameters.

## Code Walkthrough

### Installation

```bash
pip install cloudinary
```

### Configuration

```python
# config.py
import cloudinary

cloudinary.config(
    cloud_name=os.environ.get('CLOUDINARY_CLOUD_NAME'),
    api_key=os.environ.get('CLOUDINARY_API_KEY'),
    api_secret=os.environ.get('CLOUDINARY_API_SECRET'),
    secure=True
)

class Config:
    CLOUDINARY_CLOUD_NAME = os.environ.get('CLOUDINARY_CLOUD_NAME')
```

### Upload to Cloudinary

```python
# cloudinary_utils.py
import cloudinary
import cloudinary.uploader
from cloudinary.utils import cloudinary_url

def upload_image(file_data, public_id: str = None, folder: str = 'flask_app') -> dict:
    """Upload image to Cloudinary."""
    result = cloudinary.uploader.upload(
        file_data,
        public_id=public_id,
        folder=folder,
        transformation=[
            {'width': 1200, 'height': 1200, 'crop': 'limit'},
            {'quality': 'auto', 'fetch_format': 'auto'}
        ]
    )
    
    return {
        'public_id': result['public_id'],
        'url': result['secure_url'],
        'width': result['width'],
        'height': result['height'],
        'format': result['format']
    }

def generate_transformed_url(public_id: str, **transformations) -> str:
    """Generate URL with transformations."""
    url, _ = cloudinary_url(
        public_id,
        transformation=transformations
    )
    return url

# Example transformations
def get_thumbnail_url(public_id: str, width: int = 150, height: int = 150) -> str:
    """Get thumbnail URL."""
    return generate_transformed_url(
        public_id,
        width=width,
        height=height,
        crop='fill',
        gravity='auto'
    )

def get_optimized_url(public_id: str) -> str:
    """Get auto-optimized URL."""
    return generate_transformed_url(
        public_id,
        fetch_format='auto',
        quality='auto'
    )
```

### Flask Routes

```python
# app.py
from flask import Flask, request, jsonify

@app.route('/upload', methods=['POST'])
def upload():
    if 'image' not in request.files:
        return jsonify({'error': 'No image'}), 400
    
    file = request.files['image']
    file_data = file.read()
    
    result = upload_image(file_data, folder='user_uploads')
    
    return jsonify({
        'public_id': result['public_id'],
        'url': result['url'],
        'thumbnail': get_thumbnail_url(result['public_id'])
    }), 201

@app.route('/transform/<public_id>')
def transform_image(public_id):
    """Example: different transformation URLs."""
    return jsonify({
        'thumbnail': get_thumbnail_url(public_id, 100, 100),
        'medium': get_thumbnail_url(public_id, 300, 300),
        'large': get_thumbnail_url(public_id, 600, 600),
        'optimized': get_optimized_url(public_id)
    })
```

### Line-by-Line Breakdown

- Cloudinary automatically optimizes on upload
- URL transformations are applied on-the-fly
- `crop='fill'` with `gravity='auto'` focuses on important content

> **⚡ Performance Note:** Cloudinary's free tier includes 25GB bandwidth, sufficient for small apps.

## Common Mistakes

- ❌ Not using auto-optimization
- ✅ Use `quality='auto'` and `fetch_format='auto'`

- ❌ Hardcoding transformation values
- ✅ Generate URLs dynamically based on needs

## Quick Reference

| Transformation | Parameter |
|---------------|-----------|
| Resize | width, height, crop |
| Crop to fill | crop='fill', gravity='auto' |
| Quality | quality='auto' |
| Format | fetch_format='auto' |

## Next Steps

Continue to [03_choosing_a_storage_provider.md](./03_choosing_a_storage_provider.md) to compare options.
