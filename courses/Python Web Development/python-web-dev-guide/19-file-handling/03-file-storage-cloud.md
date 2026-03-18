# Cloud File Storage

## What You'll Learn
- Using AWS S3
- Using Google Cloud Storage
- Azure Blob Storage

## Prerequisites
- Completed file downloads

## AWS S3

```bash
pip install boto3
```

```python
import boto3
from fastapi import UploadFile
import uuid

s3 = boto3.client(
    's3',
    aws_access_key_id='YOUR_KEY',
    aws_secret_access_key='YOUR_SECRET'
)

BUCKET_NAME = 'my-bucket'

async def upload_to_s3(file: UploadFile, filename: str) -> str:
    """Upload file to S3"""
    key = f"uploads/{uuid.uuid4()}-{filename}"
    
    content = await file.read()
    
    s3.put_object(
        Bucket=BUCKET_NAME,
        Key=key,
        Body=content,
        ContentType=file.content_type
    )
    
    return f"https://{BUCKET_NAME}.s3.amazonaws.com/{key}"

@app.post("/upload-s3")
async def upload_s3(file: UploadFile = File(...)):
    url = await upload_to_s3(file, file.filename)
    return {"url": url}
```

## Google Cloud Storage

```bash
pip install google-cloud-storage
```

```python
from google.cloud import storage

gcs = storage.Client()
bucket = gcs.bucket('my-bucket')

async def upload_to_gcs(file: UploadFile, filename: str) -> str:
    """Upload to Google Cloud Storage"""
    blob = bucket.blob(f"uploads/{uuid.uuid4()}-{filename}")
    
    content = await file.read()
    blob.upload_from_string(content, content_type=file.content_type)
    
    return blob.public_url
```

## Summary
- Use cloud storage for production
- S3 is most popular
- Handle credentials securely

## Next Steps
→ Continue to `04-working-with-csv-excel.md`
