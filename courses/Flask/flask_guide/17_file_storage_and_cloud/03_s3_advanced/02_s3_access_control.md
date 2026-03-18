<!-- FILE: 17_file_storage_and_cloud/03_s3_advanced/02_s3_access_control.md -->

## Overview

Control access to S3 resources using IAM policies and bucket policies.

## Prerequisites

- Understanding of AWS IAM
- Completed S3 basics

## Core Concepts

IAM (Identity and Access Management) controls who can access your S3 resources. Follow the principle of least privilege - grant only the minimum permissions needed.

## Core Concepts

### IAM Policy Structure

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "s3:GetObject",
                "s3:PutObject"
            ],
            "Resource": "arn:aws:s3:::your-bucket/*"
        }
    ]
}
```

### Bucket Policy Example

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "PublicReadGetObject",
            "Effect": "Allow",
            "Principal": "*",
            "Action": "s3:GetObject",
            "Resource": "arn:aws:s3:::your-bucket/public/*"
        },
        {
            "Sid": "DenyUnencryptedUploads",
            "Effect": "Deny",
            "Principal": "*",
            "Action": "s3:PutObject",
            "Resource": "arn:aws:s3:::your-bucket/*",
            "Condition": {
                "Bool": {
                    "s3:x-amz-server-side-encryption": "false"
                }
            }
        }
    ]
}
```

### Flask Application IAM Policy

Create an IAM user with this policy for your Flask app:

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "s3:PutObject",
                "s3:GetObject",
                "s3:DeleteObject"
            ],
            "Resource": "arn:aws:s3:::your-bucket/uploads/*"
        },
        {
            "Effect": "Allow",
            "Action": [
                "s3:ListBucket"
            ],
            "Resource": "arn:aws:s3:::your-bucket"
        }
    ]
}
```

### CORS Configuration

For browser-based uploads:

```xml
<!-- CORS configuration for bucket -->
<CORSConfiguration>
    <CORSRule>
        <AllowedOrigin>https://yourdomain.com</AllowedOrigin>
        <AllowedMethod>PUT</AllowedMethod>
        <AllowedMethod>POST</AllowedMethod>
        <AllowedMethod>GET</AllowedMethod>
        <AllowedHeader>*</AllowedHeader>
    </CORSRule>
</CORSConfiguration>
```

## Code Walkthrough

```python
# s3_cors.py - Configure CORS via boto3

def configure_bucket_cors(bucket_name: str, allowed_origins: list):
    """Configure CORS for S3 bucket (for direct browser uploads)."""
    s3 = get_s3_client()
    
    cors_config = {
        'CORSRules': [
            {
                'AllowedOrigins': allowed_origins,
                'AllowedMethods': ['GET', 'PUT', 'POST'],
                'AllowedHeaders': ['*']
            }
        ]
    }
    
    s3.put_bucket_cors(
        Bucket=bucket_name,
        CORSConfiguration=cors_config
    )
```

> **🔒 Security Note:** Never make buckets public unless absolutely necessary. Use presigned URLs for private access.

## Common Mistakes

- ❌ Making entire bucket public
- ✅ Use bucket policies for specific paths only

- ❌ Using root account credentials
- ✅ Create IAM user with limited permissions

- ❌ Not requiring encryption
- ✅ Deny unencrypted uploads with bucket policy

## Quick Reference

| Permission | Description |
|------------|-------------|
| s3:GetObject | Read files |
| s3:PutObject | Upload files |
| s3:DeleteObject | Delete files |
| s3:ListBucket | List bucket contents |

## Next Steps

Continue to [03_serving_media_via_cloudfront.md](./03_serving_media_via_cloudfront.md) to learn about CDN distribution.
