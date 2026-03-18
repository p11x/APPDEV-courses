<!-- FILE: 17_file_storage_and_cloud/04_image_processing/03_storing_processed_images.md -->

## Overview

Store processed images efficiently with proper organization and metadata.

## Prerequisites

- Completed image resizing and thumbnails guides
- Understanding of S3 storage

## Core Concepts

Store processed images with descriptive keys, track metadata in database, and implement proper lifecycle management.

## Code Walkthrough

```python
# image_storage.py
import uuid
import os
from datetime import datetime, timedelta

class ImageStorageManager:
    def __init__(self, s3_client, bucket: str):
        self.s3 = s3_client
        self.bucket = bucket
    
    def generate_storage_key(
        self, 
        user_id: int, 
        original_filename: str,
        process_type: str = 'original'
    ) -> str:
        """Generate organized S3 key for image storage."""
        # Extract extension
        ext = os.path.splitext(original_filename)[1].lower()
        if not ext:
            ext = '.jpg'
        
        # Create unique identifier
        unique_id = str(uuid.uuid4())
        
        # Organize by user and date
        date_path = datetime.now().strftime('%Y/%m/%d')
        
        # Key format: uploads/{user_id}/2024/01/15/{uuid}_original.jpg
        key = f"uploads/{user_id}/{date_path}/{unique_id}_{process_type}{ext}"
        
        return key
    
    def store_processed_image(
        self,
        user_id: int,
        image_data: bytes,
        original_filename: str,
        metadata: dict
    ) -> dict:
        """Store processed image with metadata."""
        # Generate keys for each version
        original_key = self.generate_storage_key(
            user_id, original_filename, 'original'
        )
        
        # Upload original
        self.s3.put_object(
            Bucket=self.bucket,
            Key=original_key,
            Body=image_data,
            ContentType=metadata.get('content_type', 'image/jpeg'),
            Metadata={
                'user_id': str(user_id),
                'original_filename': original_filename,
                'uploaded_at': datetime.utcnow().isoformat(),
                'width': str(metadata.get('width', '')),
                'height': str(metadata.get('height', '')),
                'file_size': str(len(image_data))
            }
        )
        
        return {
            'key': original_key,
            'bucket': self.bucket,
            'url': f"https://{self.bucket}.s3.amazonaws.com/{original_key}",
            'metadata': metadata
        }
    
    def delete_user_images(self, user_id: int):
        """Delete all images for a user (for account deletion)."""
        # List all objects with user prefix
        paginator = self.s3.get_paginator('list_objects_v2')
        
        user_prefix = f"uploads/{user_id}/"
        
        for page in paginator.paginate(Bucket=self.bucket, Prefix=user_prefix):
            if 'Contents' in page:
                objects_to_delete = [
                    {'Key': obj['Key']} for obj in page['Contents']
                ]
                
                self.s3.delete_objects(
                    Bucket=self.bucket,
                    Delete={'Objects': objects_to_delete}
                )

# Flask route with storage manager
from flask import current_app

@app.route('/upload-photo', methods=['POST'])
@login_required
def upload_photo():
    if 'photo' not in request.files:
        return jsonify({'error': 'No photo'}), 400
    
    file = request.files['photo']
    image_data = file.read()
    
    # Process image (resize, create thumbnails)
    processed = process_image(image_data)
    
    # Store with manager
    manager = ImageStorageManager(get_s3_client(), current_app.config['AWS_S3_BUCKET'])
    
    result = manager.store_processed_image(
        user_id=current_user.id,
        image_data=processed['original'],
        original_filename=file.filename,
        metadata=processed['metadata']
    )
    
    # Save to database
    image = Image(
        user_id=current_user.id,
        s3_key=result['key'],
        url=result['url'],
        width=processed['metadata']['width'],
        height=processed['metadata']['height']
    )
    db.session.add(image)
    db.session.commit()
    
    return jsonify(result), 201
```

### Line-by-Line Breakdown

- Organized key structure: `uploads/{user_id}/{date}/{unique_id}_{type}.{ext}`
- Metadata is stored in S3 object metadata for quick access
- User cleanup removes all files when account is deleted

> **⚡ Performance Note:** Use S3 lifecycle policies to move old images to cheaper storage classes.

## Common Mistakes

- ❌ Not organizing uploads by user
- ✅ Use user-specific prefixes for security and organization

- ❌ Not storing metadata
- ✅ Store in database AND S3 metadata

- ❌ Orphaned images after record deletion
- ✅ Implement cascade deletion

## Quick Reference

| Component | Purpose |
|-----------|---------|
| Key structure | Organized file paths |
| Object metadata | Quick access to image info |
| Lifecycle policies | Cost-effective storage |

## Next Steps

Continue to [05_alternative_storage/01_google_cloud_storage.md](../05_alternative_storage/01_google_cloud_storage.md) to learn about alternative storage options.
