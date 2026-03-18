<!-- FILE: 17_file_storage_and_cloud/04_image_processing/02_generating_thumbnails.md -->

## Overview

Generate thumbnails for image previews and galleries.

## Prerequisites

- Pillow installed
- Understanding of image resizing

## Core Concepts

Thumbnails are smaller versions of images used for previews. Generate multiple sizes for different use cases: small (64px), medium (150px), large (300px).

## Code Walkthrough

```python
# thumbnail_utils.py
from PIL import Image
import io
import os

THUMBNAIL_SIZES = {
    'small': (64, 64),
    'medium': (150, 150),
    'large': (300, 300),
    'avatar': (128, 128)
}

def create_thumbnail(
    image_data: bytes, 
    size_name: str = 'medium',
    quality: int = 80
) -> bytes:
    """Create a thumbnail at the specified size."""
    size = THUMBNAIL_SIZES.get(size_name, THUMBNAIL_SIZES['medium'])
    
    img = Image.open(io.BytesIO(image_data))
    
    # Convert to RGB
    if img.mode in ('RGBA', 'P'):
        img = img.convert('RGB')
    
    # Create thumbnail (maintains aspect ratio)
    img.thumbnail(size, Image.Resampling.LANCZOS)
    
    # Save
    output = io.BytesIO()
    img.save(output, format='JPEG', quality=quality, optimize=True)
    
    return output.getvalue()

def create_all_thumbnails(image_data: bytes) -> dict:
    """Create all thumbnail sizes at once."""
    thumbnails = {}
    
    for size_name, size in THUMBNAIL_SIZES.items():
        thumbnails[size_name] = create_thumbnail(image_data, size_name)
    
    return thumbnails

def save_thumbnails_to_s3(image_data: bytes, s3_key: str) -> dict:
    """Save original and all thumbnails to S3."""
    # Get original extension
    ext = os.path.splitext(s3_key)[1]
    base_key = s3_key.replace(ext, '')
    
    results = {}
    
    # Upload original
    results['original'] = upload_to_s3(image_data, s3_key)
    
    # Create and upload thumbnails
    thumbnails = create_all_thumbnails(image_data)
    
    for size_name, thumb_data in thumbnails.items():
        thumb_key = f"{base_key}_thumb_{size_name}.jpg"
        results[size_name] = upload_to_s3(thumb_data, thumb_key)
    
    return results

# Flask route
@app.route('/upload-gallery', methods=['POST'])
def upload_gallery():
    if 'image' not in request.files:
        return jsonify({'error': 'No image'}), 400
    
    file = request.files['image']
    image_data = file.read()
    
    # Save original and thumbnails
    results = save_thumbnails_to_s3(image_data, file.filename)
    
    return jsonify({
        'original': results['original']['url'],
        'small': results['small']['url'],
        'medium': results['medium']['url'],
        'large': results['large']['url']
    }), 201
```

### Line-by-Line Breakdown

- `img.thumbnail()` modifies image in-place, maintaining aspect ratio
- Each size has a specific key suffix for organization
- All thumbnails are uploaded to S3 with descriptive keys

> **💡 Tip:** Use WebP format for smaller file sizes while maintaining quality.

## Common Mistakes

- ❌ Using crop for thumbnails without centering
- ✅ Use `thumbnail()` which maintains aspect ratio

- ❌ Not generating multiple sizes
- ✅ Generate common sizes upfront

- ❌ Storing thumbnails locally in production
- ✅ Use S3 with a CDN

## Quick Reference

| Size | Pixels | Use Case |
|------|--------|----------|
| small | 64x64 | Icons, tiny previews |
| medium | 150x150 | Grid thumbnails |
| large | 300x300 | Detail previews |
| avatar | 128x128 | Profile pictures |

## Next Steps

Continue to [03_storing_processed_images.md](./03_storing_processed_images.md) to learn about storing processed images.
