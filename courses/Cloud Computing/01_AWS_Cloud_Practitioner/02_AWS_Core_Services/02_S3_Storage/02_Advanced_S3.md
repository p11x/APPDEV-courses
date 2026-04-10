---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: S3 Storage
Purpose: Advanced S3 features including cross-region replication, lifecycle policies, and analytics
Difficulty: advanced
Prerequisites: 01_Basic_S3.md
RelatedFiles: 01_Basic_S3.md, 03_Practical_S3.md
UseCase: Enterprise data management and compliance
CertificationExam: AWS Solutions Architect
LastUpdated: 2025
---

## WHY

Advanced S3 features enable enterprise-grade data management with automation and compliance.

## WHAT

### Cross-Region Replication

Automatic replication to another region for disaster recovery.

### S3 Lifecycle Policies

Automated transition to cheaper storage classes over time.

### S3 Analytics

Analysis of access patterns to optimize storage costs.

### S3 Object Lock

WORM (Write Once, Read Many) for compliance.

## HOW

### Example: Cross-Region Replication

```bash
# Enable versioning on source bucket
aws s3api put-bucket-versioning \
    --bucket source-bucket \
    --versioning-configuration Status=Enabled

# Create IAM role for replication
aws iam create-role --role-name s3-replication-role \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"Service": "s3.amazonaws.com"},
            "Action": "sts:AssumeRole"
        }]
    }'

# Configure replication
aws s3api put-bucket-replication \
    --bucket source-bucket \
    --replication-configuration '{
        "Role": "arn:aws:iam::123456789:role/s3-replication-role",
        "Rules": [{
            "ID": "replicate-all",
            "Status": "Enabled",
            "Destination": {"Bucket": "arn:aws:s3:::dest-bucket"},
            "Priority": 1
        }]
    }'
```

### Example: Lifecycle Policy

```bash
# Configure lifecycle
aws s3api put-bucket-lifecycle-configuration \
    --bucket my-bucket \
    --lifecycle-configuration '{
        "Rules": [{
            "ID": "archive-after-90-days",
            "Status": "Enabled",
            "Transitions": [
                {"Days": 30, "StorageClass": "STANDARD_IA"},
                {"Days": 90, "StorageClass": "GLACIER"},
                {"Days": 365, "StorageClass": "DEEP_ARCHIVE"}
            ],
            "Expiration": {"Days": 3650}
        }]
    }'
```

### S3 Analytics Configuration

```bash
# Enable analytics
aws s3api put-bucket-analytics-configuration \
    --bucket my-bucket \
    --analytics-configuration '{
        "Id": "access-patterns",
        "StorageClassAnalysis": {
            "DataExport": {
                "OutputSchemaVersion": "V_1",
                "Destination": {
                    "Bucket": "arn:aws:s3:::analytics-bucket"
                }
            }
        }
    }'
```

## CROSS-REFERENCES

### Related Services

- CloudTrail: API logging
- Glacier: Archive storage
- Backup: Enterprise backup