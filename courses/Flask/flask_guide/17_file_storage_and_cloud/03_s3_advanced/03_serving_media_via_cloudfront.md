<!-- FILE: 17_file_storage_and_cloud/03_s3_advanced/03_serving_media_via_cloudfront.md -->

## Overview

Set up CloudFront CDN to serve S3 media files with global low-latency access.

## Prerequisites

- S3 bucket with files
- Basic understanding of CDNs

## Core Concepts

CloudFront is AWS's Content Delivery Network (CDN). It caches your S3 files at edge locations worldwide, reducing latency for global users.

## Core Concepts

### CloudFront Benefits

- **Global distribution**: 600+ edge locations worldwide
- **Reduced latency**: Serve from nearest edge location
- **DDoS protection**: Built-in AWS Shield
- **Cost-effective**: Pay only for what you use
- **SSL/TLS**: Free certificates via ACM

### How It Works

1. User requests a file
2. CloudFront checks if file is cached at edge location
3. If cached, serves immediately
4. If not, fetches from S3 origin, caches, and serves

## Code Walkthrough

### Creating CloudFront Distribution

```python
# cloudfront_setup.py
import boto3

def create_cloudfront_origin_access_identity():
    """Create OAI to restrict CloudFront to S3."""
    cf = boto3.client('cloudfront')
    
    response = cf.create_origin_access_identity(
        CallerReference=f'oai-{__import__("time").time()}',
        Comment='Flask app OAI'
    )
    
    return response['OriginAccessIdentity']['Id']

def create_distribution(bucket_name: str, domain_name: str):
    """Create CloudFront distribution for S3 bucket."""
    cf = boto3.client('cloudfront')
    
    response = cf.create_distribution(
        Origin={
            'DomainName': f'{bucket_name}.s3.amazonaws.com',
            'Id': bucket_name,
            'S3OriginConfig': {
                'OriginAccessIdentity': ''
            }
        },
        CallerReference=f'dist-{__import__("time").time()}',
        Comment=f'Distribution for {domain_name}',
        DefaultRootObject='index.html',
        Enabled=True,
        DefaultCacheBehavior={
            'TargetOriginId': bucket_name,
            'ViewerProtocolPolicy': 'redirect-to-https',
            'AllowedMethods': {
                'Items': ['GET', 'HEAD'],
                'CachedMethods': ['GET', 'HEAD']
            },
            'ForwardedValues': {
                'QueryString': False,
                'Cookies': {'Forward': 'none'}
            },
            'MinTTL': 86400  # 24 hours
        }
    )
    
    return response['Distribution']['DomainName']
```

### Flask Integration

```python
# config.py
class Config:
    # CloudFront configuration
    CLOUDFRONT_DOMAIN = os.environ.get('CLOUDFRONT_DOMAIN')
    # e.g., 'd1234567890.cloudfront.net'

# s3_utils.py - Get CloudFront URL instead of S3 URL
def get_file_url(s3_key: str) -> str:
    """Get CloudFront URL for serving."""
    cf_domain = current_app.config.get('CLOUDFRONT_DOMAIN')
    
    if cf_domain:
        return f"https://{cf_domain}/{s3_key}"
    
    # Fallback to S3 URL
    bucket = current_app.config['AWS_S3_BUCKET']
    region = current_app.config['AWS_REGION']
    return f"https://{bucket}.s3.{region}.amazonaws.com/{s3_key}"

# Usage in Flask route
@app.route('/files/<path:key>')
def get_file(key):
    url = get_file_url(key)
    return redirect(url)
```

### Cache Invalidation

```python
# cache_utils.py
def invalidate_cloudfront_cache(keys: list):
    """Invalidate CloudFront cache for specific files."""
    cf = boto3.client('cloudfront')
    
    response = cf.create_invalidation(
        DistributionId=current_app.config['CLOUDFRONT_DISTRIBUTION_ID'],
        InvalidationBatch={
            'CallerReference': f'invalidation-{__import__("time").time()}',
            'Paths': {
                'Quantity': len(keys),
                'Items': keys
            }
        }
    )
    
    return response['Invalidation']['Id']
```

### Line-by-Line Breakdown

- CloudFront serves as a cache layer in front of S3
- OAI (Origin Access Identity) ensures only CloudFront can access private buckets
- Invalidation removes cached files before TTL expires

> **⚡ Performance Note:** Set appropriate cache headers. Long TTL = better cache hit rate, but slower updates.

## Common Mistakes

- ❌ Not setting cache headers
- ✅ Configure default TTL and cache behaviors

- ❌ Forgetting to invalidate after updates
- ✅ Call invalidation when files change

- ❌ Using S3 URLs in production
- ✅ Use CloudFront for better performance

## Quick Reference

| Component | Purpose |
|-----------|---------|
| Origin | S3 bucket (source of files) |
| Distribution | CloudFront config |
| Edge Location | Caching servers worldwide |
| OAI | Secure origin access |

## Next Steps

Continue to [04_image_processing/01_resizing_images_with_pillow.md](../04_image_processing/01_resizing_images_with_pillow.md) to learn about image processing.
