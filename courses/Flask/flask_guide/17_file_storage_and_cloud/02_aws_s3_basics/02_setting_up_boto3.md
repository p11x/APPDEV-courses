<!-- FILE: 17_file_storage_and_cloud/02_aws_s3_basics/02_setting_up_boto3.md -->

## Overview

Set up boto3, the AWS SDK for Python, to connect Flask to S3.

## Prerequisites

- Python installed
- AWS account with S3 access
- AWS credentials configured

## Core Concepts

boto3 is the AWS SDK for Python. It provides Pythonic interfaces to AWS services. You'll need to install it and configure credentials.

## Code Walkthrough

### Installation

```bash
pip install boto3
```

### Configuration

There are several ways to configure AWS credentials:

**Option 1: Environment Variables**

```bash
# Set in your environment or .env file
export AWS_ACCESS_KEY_ID="your-access-key"
export AWS_SECRET_ACCESS_KEY="your-secret-key"
export AWS_DEFAULT_REGION="us-east-1"
```

**Option 2: AWS Credentials File**

Create `~/.aws/credentials`:

```ini
[default]
aws_access_key_id = your_access_key
aws_secret_access_key = your_secret_key
region = us-east-1
```

**Option 3: IAM Role (EC2/Lambda)**

When running on AWS infrastructure, use IAM roles instead of credentials.

### Flask Configuration

```python
# config.py
import os

class Config:
    # AWS S3 Configuration
    AWS_S3_BUCKET = os.environ.get('AWS_S3_BUCKET', 'your-bucket-name')
    AWS_REGION = os.environ.get('AWS_REGION', 'us-east-1')
    AWS_ACCESS_KEY_ID = os.environ.get('AWS_ACCESS_KEY_ID')
    AWS_SECRET_ACCESS_KEY = os.environ.get('AWS_SECRET_ACCESS_KEY')
```

### Creating the S3 Client

```python
# s3_client.py
import boto3
from flask import current_app

def get_s3_client():
    """Create an S3 client using Flask app config."""
    return boto3.client(
        's3',
        aws_access_key_id=current_app.config['AWS_ACCESS_KEY_ID'],
        aws_secret_access_key=current_app.config['AWS_SECRET_ACCESS_KEY'],
        region_name=current_app.config['AWS_REGION']
    )

# Using botocore session (recommended for production)
def get_s3_client_session():
    """Create S3 client using boto3 session."""
    session = boto3.Session(
        aws_access_key_id=current_app.config.get('AWS_ACCESS_KEY_ID'),
        aws_secret_access_key=current_app.config.get('AWS_SECRET_ACCESS_KEY'),
        region_name=current_app.config.get('AWS_REGION', 'us-east-1')
    )
    return session.client('s3')
```

### Testing the Connection

```python
# Test connection
import boto3

def test_s3_connection():
    """Verify S3 credentials work."""
    s3 = boto3.client('s3')
    try:
        # List buckets (will raise if credentials invalid)
        s3.list_buckets()
        return True
    except Exception as e:
        print(f"S3 connection failed: {e}")
        return False
```

### Line-by-Line Breakdown

- `boto3.client('s3')` creates an S3 service client
- `aws_access_key_id` and `aws_secret_access_key` authenticate with AWS
- `region_name` specifies which AWS region to use

> **🔒 Security Note:** Never commit AWS credentials to version control. Use environment variables or AWS IAM roles.

## Common Mistakes

- ❌ Hardcoding AWS credentials in source code
- ✅ Using environment variables or IAM roles

- ❌ Using root AWS account credentials
- ✅ Creating IAM user with limited S3 permissions

## Quick Reference

| Method | Description |
|--------|-------------|
| `boto3.client('s3')` | Create S3 client |
| `boto3.Session()` | Create boto3 session |
| `list_buckets()` | Test credentials |

## Next Steps

Continue to [03_uploading_files_to_s3.md](./03_uploading_files_to_s3.md) to learn about uploading files.
