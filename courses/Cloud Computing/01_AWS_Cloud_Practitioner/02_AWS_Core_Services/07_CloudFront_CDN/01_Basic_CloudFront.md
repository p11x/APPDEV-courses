---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: CloudFront CDN
Purpose: Understanding CloudFront content delivery network, distributions, and caching
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_CloudFront.md, 03_Practical_CloudFront.md
UseCase: Accelerating content delivery globally
CertificationExam: AWS Certified Cloud Practitioner - Domain 3
LastUpdated: 2025
---

## WHY

CloudFront is AWS's content delivery network (CDN) that delivers content with low latency and high data transfer speeds. Understanding CloudFront is essential for building performant global applications.

### Why CloudFront Matters

- **Performance**: Reduced latency for global users (50-80% improvement)
- **Availability**: 450+ edge locations worldwide
- **Security**: DDoS protection included
- **Cost**: Reduces origin load; pay for transfer used

### Industry Statistics

- Processes exabytes of data monthly
- 99.99% availability SLA
- 450+ edge locations in 90+ cities

## WHAT

### CloudFront Core Concepts

**Distribution**: Collection of content from origin servers.

**Origin**: Source of content (S3 bucket, EC2, custom HTTP server).

**Edge Location**: Regional cache server.

**Cache Behavior**: Rules for how CloudFront handles requests.

### Distribution Flow

```
            CLOUDFRONT FLOW
            ==============

  User in Tokyo ──────► Edge Location Tokyo
                             │
                             ▼
  Request ──────► Edge Location Singapore
                             │
                             ▼ (cache miss)
                       Origin Server (US)
                             │
                             ▼
                       Cache at Edge Tokyo
                             │
                             ▼
              Response to User (next time)
```

## HOW

### Example 1: Create S3-backed Distribution

```bash
# Create CloudFront distribution with S3 origin
aws cloudfront create-distribution \
    --origin-domain-name my-bucket.s3.amazonaws.com \
    --default-root-object index.html \
    --enabled true \
    --comment "My CloudFront Distribution" \
    --viewer-certificate '{
        "CloudFrontDefaultCertificate": true
    }' \
    --default-cache-behavior '{
        "TargetOriginId": "S3-my-bucket",
        "ViewerProtocolPolicy": "redirect-to-https",
        "AllowedMethods": {
            "Items": ["GET", "HEAD"],
            "CachedMethods": ["GET", "HEAD"],
            "Quantity": 2
        },
        "Compress": true,
        "TTL": 86400
    }'

# Get distribution ID from output
```

### Example 2: Custom Origin Distribution

```bash
# Create distribution with custom HTTP origin
aws cloudfront create-distribution \
    --origin-domain-name my ALB.us-east-1.elb.amazonaws.com \
    --origin-path "" \
    --enabled true \
    --default-cache-behavior '{
        "TargetOriginId": "custom-origin",
        "ViewerProtocolPolicy": "allow-all",
        "AllowedMethods": {
            "Items": ["GET", "HEAD", "OPTIONS"],
            "CachedMethods": ["GET", "HEAD"],
            "Quantity": 2
        },
        "MinTTL": 10,
        "DefaultTTL": 3600,
        "MaxTTL": 86400
    }'
```

### Example 3: Invalidation

```bash
# Create invalidation to clear cache
aws cloudfront create-invalidation \
    --distribution-id E1234567890ABC \
    --paths "/index.html" "/*.js" "/*.css"

# Or invalidate all
aws cloudfront create-invalidation \
    --distribution-id E1234567890ABC \
    --paths "/*"
```

## PRICING

### CloudFront Pricing (per GB transferred out)

| Transfer Tier | Price (USD) |
|--------------|------------|
| First 10TB | $0.085 |
| Next 40TB | $0.080 |
| Next 100TB | $0.070 |
| Over 150TB | $0.050 |

## CROSS-REFERENCES

### Related Services

- S3: Common origin
- Lambda@Edge: Edge computing
- Route 53: DNS routing

### Prerequisites

- Basic cloud concepts
- S3 basics