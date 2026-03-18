# AWS S3 File Upload

## What You'll Learn

- How to configure AWS credentials for S3 access
- How to upload files to S3 buckets
- How to generate presigned URLs for secure downloads
- How to handle file uploads from web applications
- Best practices for S3 security

## Prerequisites

- Completed `04-sendgrid-email-integration.md`
- An AWS account with S3 access

## Introduction

Amazon S3 (Simple Storage Service) is the most popular cloud storage solution. It's used to store images, videos, documents, and any other file types. This guide covers the essentials of integrating S3 into your Python web applications.

## Setting Up AWS Credentials

First, install the AWS SDK for Python (boto3):

```bash
pip install boto3
```

Configure your AWS credentials:

```python
import os
import boto3
from dataclasses import dataclass
from typing import Optional
from datetime import datetime


@dataclass
class S3Config:
    """Configuration for AWS S3 access."""
    access_key_id: str
    secret_access_key: str
    region_name: str
    bucket_name: str


class S3Client:
    """Client for interacting with AWS S3."""
    
    def __init__(self, config: S3Config) -> None:
        self.config = config
        self.client = boto3.client(
            "s3",
            aws_access_key_id=config.access_key_id,
            aws_secret_access_key=config.secret_access_key,
            region_name=config.region_name,
        )
        self.bucket_name = config.bucket_name
    
    def upload_file(
        self,
        file_path: str,
        key: str,
        content_type: Optional[str] = None,
    ) -> dict:
        """Upload a file to S3."""
        extra_args = {}
        if content_type:
            extra_args["ContentType"] = content_type
        
        self.client.upload_file(
            file_path,
            self.bucket_name,
            key,
            ExtraArgs=extra_args,
        )
        
        return {
            "bucket": self.bucket_name,
            "key": key,
            "url": f"https://{self.bucket_name}.s3.{self.config.region_name}.amazonaws.com/{key}",
        }
    
    def upload_fileobj(
        self,
        file_obj: bytes,
        key: str,
        content_type: Optional[str] = None,
    ) -> dict:
        """Upload file-like object to S3."""
        extra_args = {}
        if content_type:
            extra_args["ContentType"] = content_type
        
        self.client.upload_fileobj(
            file_obj,
            self.bucket_name,
            key,
            ExtraArgs=extra_args,
        )
        
        return {"bucket": self.bucket_name, "key": key}
    
    def download_file(self, key: str, file_path: str) -> None:
        """Download a file from S3."""
        self.client.download_file(self.bucket_name, key, file_path)
    
    def get_object(self, key: str) -> dict:
        """Get an object from S3."""
        response = self.client.get_object(Bucket=self.bucket_name, Key=key)
        return response
    
    def list_objects(self, prefix: str = "") -> list[dict]:
        """List objects in the bucket with optional prefix."""
        response = self.client.list_objects_v2(
            Bucket=self.bucket_name,
            Prefix=prefix,
        )
        return response.get("Contents", [])
    
    def delete_object(self, key: str) -> None:
        """Delete an object from S3."""
        self.client.delete_object(Bucket=self.bucket_name, Key=key)
    
    def generate_presigned_url(
        self,
        key: str,
        expiration: int = 3600,
    ) -> str:
        """Generate a presigned URL for temporary access."""
        url = self.client.generate_presigned_url(
            "get_object",
            Params={
                "Bucket": self.bucket_name,
                "Key": key,
            },
            ExpiresIn=expiration,
        )
        return url
    
    def generate_presigned_post(
        self,
        key: str,
        expiration: int = 3600,
    ) -> dict:
        """Generate presigned URL and fields for direct browser upload."""
        response = self.client.generate_presigned_post(
            Bucket=self.bucket_name,
            Key=key,
            ExpiresIn=expiration,
        )
        return response


# Example usage
def main() -> None:
    config = S3Config(
        access_key_id=os.environ["AWS_ACCESS_KEY_ID"],
        secret_access_key=os.environ["AWS_SECRET_ACCESS_KEY"],
        region_name="us-east-1",
        bucket_name="my-app-bucket",
    )
    
    client = S3Client(config)
    
    # Upload a file
    result = client.upload_file(
        file_path="image.jpg",
        key=f"uploads/{datetime.now().year}/{datetime.now().month}/image.jpg",
        content_type="image/jpeg",
    )
    print(f"Uploaded: {result['url']}")
    
    # Generate a presigned URL (valid for 1 hour)
    url = client.generate_presigned_url("uploads/image.jpg", expiration=3600)
    print(f"Presigned URL: {url}")
    
    # List objects
    objects = client.list_objects("uploads/")
    for obj in objects:
        print(f"- {obj['Key']}: {obj['Size']} bytes")


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `pip install boto3` — AWS SDK for Python, the official library for AWS services.
2. `boto3.client("s3", ...)` — Creates an S3 client with your credentials. The client is the main interface for S3 operations.
3. `upload_file()` — Uploads a local file to S3. The `ExtraArgs` parameter lets you set content type, cache control, etc.
4. `key` — The S3 object key (like a file path). Use prefixes to organize files (e.g., "images/2024/01/photo.jpg").
5. `upload_fileobj()` — Uploads a file-like object (bytes, StringIO, etc.) rather than a file on disk.
6. `generate_presigned_url()` — Creates a temporary URL that allows anyone to download the file without AWS credentials. Great for private files.
7. `generate_presigned_post()` — Creates a presigned URL for uploading files directly from the browser.

## Direct Browser Upload with Presigned URLs

The best practice for handling file uploads is to have the browser upload directly to S3:

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional
import uuid


app = FastAPI()

# S3 client setup
s3_client = boto3.client(
    "s3",
    aws_access_key_id=os.environ["AWS_ACCESS_KEY_ID"],
    aws_secret_access_key=os.environ["AWS_SECRET_ACCESS_KEY"],
    region_name=os.environ["AWS_REGION"],
)
BUCKET_NAME = os.environ["S3_BUCKET_NAME"]


class UploadRequest(BaseModel):
    """Request for presigned upload URL."""
    filename: str
    content_type: str
    max_size_mb: int = 10


class UploadResponse(BaseModel):
    """Response with presigned upload data."""
    upload_url: str
    key: str
    fields: dict


@app.post("/api/upload/presign", response_model=UploadResponse)
async def get_presigned_upload_url(request: UploadRequest) -> UploadResponse:
    """Generate presigned URL for direct browser upload."""
    
    # Validate file type
    allowed_types = [
        "image/jpeg",
        "image/png",
        "image/gif",
        "application/pdf",
    ]
    if request.content_type not in allowed_types:
        raise HTTPException(
            status_code=400,
            detail=f"File type {request.content_type} not allowed"
        )
    
    # Validate file size
    max_size_bytes = request.max_size_mb * 1024 * 1024
    
    # Generate unique key
    file_ext = request.filename.split(".")[-1]
    key = f"uploads/{uuid.uuid4()}.{file_ext}"
    
    # Generate presigned post
    try:
        response = s3_client.generate_presigned_post(
            Bucket=BUCKET_NAME,
            Key=key,
            Fields={
                "Content-Type": request.content_type,
            },
            Conditions=[
                {"Content-Type": request.content_type},
                ["content-length-range", 0, max_size_bytes],
            ],
            ExpiresIn=3600,
        )
        
        return UploadResponse(
            upload_url=response["url"],
            key=key,
            fields=response["fields"],
        )
    except Exception as e:
        raise HTTPException(
            status_code=500,
            detail=f"Failed to generate upload URL: {str(e)}"
        )


@app.post("/api/upload/confirm")
async def confirm_upload(key: str) -> dict:
    """Confirm that a file was uploaded successfully."""
    try:
        response = s3_client.head_object(Bucket=BUCKET_NAME, Key=key)
        return {
            "success": True,
            "size": response["ContentLength"],
            "content_type": response["ContentType"],
        }
    except s3_client.exceptions.NoSuchKey:
        raise HTTPException(status_code=404, detail="File not found")
```

## Frontend JavaScript for Direct Upload

Here's how to use the presigned URL in the browser:

```html
<!DOCTYPE html>
<html>
<head>
    <title>File Upload</title>
    <style>
        .upload-container {
            max-width: 500px;
            margin: 50px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 8px;
        }
        #preview {
            max-width: 100%;
            margin-top: 20px;
            display: none;
        }
    </style>
</head>
<body>
    <div class="upload-container">
        <h2>Upload a File</h2>
        <input type="file" id="file-input" accept="image/*,.pdf">
        <button id="upload-button">Upload</button>
        <div id="status"></div>
        <img id="preview">
    </div>

    <script>
        const fileInput = document.getElementById('file-input');
        const uploadButton = document.getElementById('upload-button');
        const statusDiv = document.getElementById('status');
        const previewImg = document.getElementById('preview');

        uploadButton.addEventListener('click', async () => {
            const file = fileInput.files[0];
            if (!file) {
                statusDiv.textContent = 'Please select a file';
                return;
            }

            // Show preview for images
            if (file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    previewImg.src = e.target.result;
                    previewImg.style.display = 'block';
                };
                reader.readAsDataURL(file);
            }

            statusDiv.textContent = 'Getting upload URL...';

            // Get presigned URL from our backend
            const response = await fetch('/api/upload/presign', {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify({
                    filename: file.name,
                    content_type: file.type,
                }),
            });

            const { upload_url, fields } = await response.json();

            // Upload directly to S3
            statusDiv.textContent = 'Uploading...';
            const formData = new FormData();
            for (const [key, value] of Object.entries(fields)) {
                formData.append(key, value);
            }
            formData.append('file', file);

            const uploadResponse = await fetch(upload_url, {
                method: 'POST',
                body: formData,
            });

            if (uploadResponse.ok) {
                statusDiv.textContent = 'Upload successful!';
            } else {
                statusDiv.textContent = 'Upload failed!';
            }
        });
    </script>
</body>
</html>
```

## S3 Security Best Practices

1. **Use IAM users** — Never use root AWS credentials
2. **Bucket policies** — Restrict access to specific IP ranges
3. **Presigned URLs** — For private files, use presigned URLs instead of making them public
4. **CORS configuration** — Configure CORS on your bucket for browser uploads

```python
def set_bucket_cors(self, allowed_origins: list[str]) -> None:
    """Configure CORS for the bucket to allow browser uploads."""
    cors_configuration = {
        "CORSRules": [
            {
                "AllowedOrigins": allowed_origins,
                "AllowedMethods": ["PUT", "POST", "GET"],
                "AllowedHeaders": ["*"],
            }
        ]
    }
    self.client.put_bucket_cors(
        Bucket=self.bucket_name,
        CORSConfiguration=cors_configuration,
    )


def get_bucket_policy(self) -> dict:
    """Get current bucket policy."""
    response = self.client.get_bucket_policy(Bucket=self.bucket_name)
    return response
```

## Handling Large Files

For large files, use multipart upload:

```python
def upload_large_file(
    self,
    file_path: str,
    key: str,
    chunk_size: int = 5 * 1024 * 1024,  # 5 MB chunks
) -> dict:
    """Upload a large file using multipart upload."""
    
    # Initiate multipart upload
    response = self.client.create_multipart_upload(
        Bucket=self.bucket_name,
        Key=key,
    )
    upload_id = response["UploadId"]
    
    try:
        # Upload parts
        parts = []
        with open(file_path, "rb") as f:
            part_number = 1
            while True:
                chunk = f.read(chunk_size)
                if not chunk:
                    break
                
                # Upload each part
                part_response = self.client.upload_part(
                    Bucket=self.bucket_name,
                    Key=key,
                    UploadId=upload_id,
                    PartNumber=part_number,
                    Body=chunk,
                )
                
                parts.append({
                    "ETag": part_response["ETag"],
                    "PartNumber": part_number,
                })
                
                part_number += 1
        
        # Complete multipart upload
        self.client.complete_multipart_upload(
            Bucket=self.bucket_name,
            Key=key,
            UploadId=upload_id,
            MultipartUpload={"Parts": parts},
        )
        
        return {"bucket": self.bucket_name, "key": key}
    
    except Exception as e:
        # Abort multipart upload on failure
        self.client.abort_multipart_upload(
            Bucket=self.bucket_name,
            Key=key,
            UploadId=upload_id,
        )
        raise e
```

## Summary

- AWS S3 is the standard for cloud file storage
- Use boto3 to interact with S3 from Python
- Upload files using `upload_file()` for local files or `upload_fileobj()` for byte data
- Generate presigned URLs for secure, temporary access to private files
- Use presigned POST for direct browser uploads to avoid server load
- Configure CORS on your bucket for browser-based uploads
- Follow security best practices: use IAM credentials, not root keys

## Next Steps

→ Continue to `06-twilio-sms-integration.md` to learn about SMS messaging.
