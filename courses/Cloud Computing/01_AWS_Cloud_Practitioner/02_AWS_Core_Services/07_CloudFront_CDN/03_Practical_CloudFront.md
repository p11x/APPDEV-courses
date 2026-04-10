---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: CloudFront CDN
Purpose: Practical CloudFront implementation labs including static website delivery and signed content
Difficulty: intermediate
Prerequisites: 01_Basic_CloudFront.md, 02_Advanced_CloudFront.md
RelatedFiles: 01_Basic_CloudFront.md, 02_Advanced_CloudFront.md
UseCase: Production content delivery
CertificationExam: AWS Solutions Architect
LastUpdated: 2025
---

## WHY

Hands-on CloudFront labs provide practical content delivery experience.

## WHAT

### Lab: Secure Static Website

Deploy static website with global CDN and signed access.

## HOW

### Module 1: S3 Setup

```bash
# Create origin bucket
aws s3 mb s3://my-website-origin

# Upload content
aws s3 sync ./public s3://my-website-origin/
```

### Module 2: CloudFront Distribution

```bash
# Create distribution
aws cloudfront create-distribution \
    --origin-domain-name my-website-origin.s3.amazonaws.com \
    --default-root-object index.html \
    --viewer-certificate '{
        "CloudFrontDefaultCertificate": true
    }'
```

### Module 3: Configure Security

```bash
# Enable field-level encryption
aws cloudfront update-distribution \
    --id distribution-id \
    --distribution-config '{
        "OriginDomains": ["my-website-origin.s3.amazonaws.com"],
        "DefaultRootObject": "index.html",
        "DefaultCacheBehavior": {
            "ViewerProtocolPolicy": "redirect-to-https",
            "MinTTL": 3600
        }
    }'
```

## VERIFICATION

```bash
# Check distribution status
aws cloudfront get-distribution --id distribution-id
```

## CLEANUP

```bash
# Delete distribution
aws cloudfront delete-distribution --id distribution-id --if-match etag-value
aws s3 rb s3://my-website-origin --force
```

## CROSS-REFERENCES

### Prerequisites

- S3 and CloudFront basics