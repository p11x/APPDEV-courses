<!-- FILE: 17_file_storage_and_cloud/04_image_processing/01_resizing_images_with_pillow.md -->

## Overview

Resize images in Flask using the Pillow library.

## Prerequisites

- Python installed
- Understanding of file uploads

## Core Concepts

Pillow is the Python Imaging Library fork. It handles image opening, manipulation, and saving. Resizing images reduces storage and improves page load times.

## Code Walkthrough

### Installation

```bash
pip install Pillow
```

### Basic Image Resizing

```python
# image_utils.py
from PIL import Image
import io
import os

def resize_image(
    image_data: bytes, 
    max_width: int = 800, 
    max_height: int = 800,
    quality: int = 85
) -> bytes:
    """
    Resize an image while maintaining aspect ratio.
    
    Args:
        image_data: Raw image bytes
        max_width: Maximum width in pixels
        max_height: Maximum height in pixels
        quality: JPEG quality (1-100)
    
    Returns:
        Resized image bytes
    """
    # Open image from bytes
    img = Image.open(io.BytesIO(image_data))
    
    # Convert to RGB (required for JPEG)
    if img.mode in ('RGBA', 'P'):
        img = img.convert('RGB')
    
    # Calculate new dimensions maintaining aspect ratio
    width, height = img.size
    aspect_ratio = width / height
    
    if width > max_width or height > max_height:
        if aspect_ratio > 1:
            # Landscape - scale by width
            new_width = max_width
            new_height = int(max_width / aspect_ratio)
        else:
            # Portrait - scale by height
            new_height = max_height
            new_width = int(max_height * aspect_ratio)
        
        # Resize using high-quality downsampling
        img = img.resize((new_width, new_height), Image.Resampling.LANCZOS)
    
    # Save to bytes
    output = io.BytesIO()
    img.save(output, format='JPEG', quality=quality, optimize=True)
    
    return output.getvalue()

# Flask route with image processing
@app.route('/upload-image', methods=['POST'])
def upload_image():
    if 'image' not in request.files:
        return jsonify({'error': 'No image'}), 400
    
    file = request.files['image']
    
    # Read image data
    image_data = file.read()
    
    # Resize image
    resized_data = resize_image(image_data)
    
    # Upload to S3
    result = upload_to_s3(resized_data, file.filename)
    
    return jsonify(result), 201
```

### Line-by-Line Breakdown

- `Image.open()` opens image from bytes
- `img.convert('RGB')` ensures JPEG compatibility
- `Image.Resampling.LANCZOS` provides high-quality resampling
- `img.save()` with `optimize=True` reduces file size

> **⚡ Performance Note:** For high-traffic sites, resize images asynchronously using a task queue.

## Common Mistakes

- ❌ Not converting RGBA to RGB for JPEG
- ✅ Always convert before saving as JPEG

- ❌ Using poor-quality resampling
- ✅ Use LANCZOS for downscaling

- ❌ Not handling animated GIFs
- ✅ Check image format before processing

## Quick Reference

| Function | Purpose |
|----------|---------|
| `Image.open()` | Open image |
| `img.resize()` | Resize image |
| `img.convert()` | Convert color mode |
| `img.save()` | Save image |

## Next Steps

Continue to [02_generating_thumbnails.md](./02_generating_thumbnails.md) to learn about thumbnail generation.
