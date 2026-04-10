---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: CloudFront CDN
Purpose: Advanced CloudFront configuration including lambda@edge, Field-Level Encryption, and geo-restrictions
Difficulty: advanced
Prerequisites: 01_Basic_CloudFront.md
RelatedFiles: 01_Basic_CloudFront.md, 03_Practical_CloudFront.md
UseCase: Enterprise content delivery and security
CertificationExam: AWS Solutions Architect
LastUpdated: 2025
---

## WHY

Advanced CloudFront features enable security, customization, and optimization.

## WHAT

### Lambda@Edge

Run Lambda at edge locations for request/response customization.

### Field-Level Encryption

Encrypt specific fields in HTTPS requests.

### Geo-Restrictions

Limit content by geographic location.

### Signed URLs

Time-limited access to private content.

## HOW

### Lambda@Edge

```bash
# Create function for edge
aws lambda create-function \
    --function-name edge-function \
    --runtime nodejs18.x \
    --handler index.handler \
    --zip-file fileb://function.zip \
    --publish
```

### Geo Restriction

```bash
# Create distribution with geo restriction
aws cloudfront create-distribution \
    --origin-domain-name my-bucket.s3.amazonaws.com \
    --geo-restriction '{
        "RestrictionType": "whitelist",
        "Quantity": 1,
        "Items": ["US", "CA"]
    }'
```

### Signed URL

```bash
# Create signed URL
aws cloudfront signed-url \
    --url "https://d111111abcdef8.cloudfront.net/private/content.mp4" \
    --key-pair-id KPFXXXXXXX \
    --private-key file://private_key.pem \
    --date-less-than 2025-12-31
```

## CROSS-REFERENCES

### Related Services

- Lambda: Edge computation
- WAF: Web application firewall