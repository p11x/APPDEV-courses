---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: S3 Storage
Purpose: Practical S3 implementation labs including static website hosting, permissions, and backup automation
Difficulty: intermediate
Prerequisites: 01_Basic_S3.md, 02_Advanced_S3.md
RelatedFiles: 01_Basic_S3.md, 02_Advanced_S3.md
UseCase: Production S3 deployment and operations
CertificationExam: AWS SysOps Administrator
LastUpdated: 2025
---

## WHY

Hands-on S3 labs solidify understanding through practical implementation scenarios.

## WHAT

### Lab: Static Website with CDN

Build static website with global content delivery.

## HOW

### Module 1: Website Hosting Setup

```bash
# Create bucket
aws s3 mb s3://my-website

# Configure website hosting
aws s3 website s3://my-website \
    --index-document index.html \
    --error-document error.html

# Upload website files
aws s3 sync ./website s3://my-website/
```

### Module 2: Public Access Configuration

```bash
# Set bucket policy for public read
aws s3api put-bucket-policy \
    --bucket my-website \
    --policy '{
        "Version": "2012-10-17",
        "Statement": [{
            "Sid": "PublicReadGetObject",
            "Effect": "Allow",
            "Principal": "*",
            "Action": "s3:GetObject",
            "Resource": "arn:aws:s3:::my-website/*"
        }]
    }'
```

### Module 3: CloudFront CDN

```bash
# Create CloudFront distribution
aws cloudfront create-distribution \
    --origin-domain-name my-website.s3.amazonaws.com \
    --default-root-object index.html
```

## VERIFICATION

```bash
# Test website
curl http://my-website.s3-website-us-east-1.amazonaws.com

# Check CloudFront
aws cloudfront list-distributions
```

## CLEANUP

```bash
# Delete resources
aws s3 rb s3://my-website --force
```

## CROSS-REFERENCES

### Prerequisites

- S3 basic concepts