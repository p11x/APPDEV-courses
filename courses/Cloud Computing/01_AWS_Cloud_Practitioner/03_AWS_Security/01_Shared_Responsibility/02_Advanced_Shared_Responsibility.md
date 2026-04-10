---
Category: AWS Cloud Practitioner
Subcategory: AWS Security
Concept: Shared Responsibility
Purpose: Advanced shared responsibility implementation for enterprise compliance
Difficulty: advanced
Prerequisites: 01_Basic_Shared_Responsibility.md
RelatedFiles: 01_Basic_Shared_Responsibility.md, 03_Practical_Shared_Responsibility.md
UseCase: Enterprise compliance and governance
CertificationExam: AWS Security Specialty
LastUpdated: 2025
---

## WHY

Advanced shared responsibility implementations address enterprise compliance requirements.

## WHAT

### Customer Responsibility Areas

**Data Classification**: Data categorization and handling

**Identity Management**: Fine-grained access control

**Encryption Management**: Customer-managed keys

**Application Security**: Code and configuration security

## HOW

### Example: Customer-Managed Keys

```bash
# Create KMS key
aws kms create-key \
    --description "Customer managed key" \
    --key-usage ENCRYPT_DECRYPT \
    --origin AWS_KMS

# Use with S3
aws s3api put-bucket-encryption \
    --bucket my-bucket \
    --server-side-encryption-configuration '{
        "Rules": [{
            "ApplyServerSideEncryptionByDefault": {
                "SSEAlgorithm": "aws:kms",
                "KMSKeyId": "key-id"
            }
        }]
    }'
```

### Example: Security Hub

```bash
# Enable Security Hub
aws securityhub enable-security-hub

# Enable standards
aws securityhub enable-standards \
    --standards-arn "arn:aws:securityhub:us-east-1::standards/aws-foundational-security-best-practices/v/1.0.0"
```

## CROSS-REFERENCES

### Related Services

- KMS: Key management
- Security Hub: Security visibility
- Config: Resource monitoring