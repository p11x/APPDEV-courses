---
Category: AWS Cloud Practitioner
Subcategory: AWS Security
Concept: Shared Responsibility
Purpose: Hands-on labs implementing shared responsibility controls
Difficulty: intermediate
Prerequisites: 01_Basic_Shared_Responsibility.md, 02_Advanced_Shared_Responsibility.md
RelatedFiles: 01_Basic_Shared_Responsibility.md, 02_Advanced_Shared_Responsibility.md
UseCase: Security control implementation
CertificationExam: AWS Security Specialty
LastUpdated: 2025
---

## WHY

Hands-on implementation of shared responsibility controls demonstrates practical security.

## WHAT

### Lab: Security Control Implementation

Deploy comprehensive security controls.

## HOW

### Module 1: VPC Security

```bash
# Create VPC with private subnets
aws ec2 create-vpc --cidr-block 10.0.0.0/16

# Create security groups
aws ec2 create-security-group \
    --group-name web-sg \
    --vpc-id vpc-123 \
    --description "Web security group"

# Restrictive rules
aws ec2 authorize-security-group-ingress \
    --group-id sg-123 \
    --protocol tcp \
    --port 443 \
    --cidr 10.0.0.0/16
```

### Module 2: Encryption Implementation

```bash
# Create KMS key
KEY_ID=$(aws kms create-key \
    --description "Data encryption key" \
    --query KeyMetadata.KeyId)

# Enable encryption
aws s3api put-bucket-encryption \
    --bucket my-bucket \
    --server-side-encryption-configuration '{
        "Rules": [{
            "ApplyServerSideEncryptionByDefault": {
                "SSEAlgorithm": "aws:kms",
                "KMSKeyId": "'$KEY_ID'"
            }
        }]
    }'
```

### Module 3: Access Control

```bash
# Create IAM policy with least privilege
aws iam create-policy \
    --policy-name restrictive-policy \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": ["s3:GetObject"],
            "Resource": "arn:aws:s3:::my-bucket/*"
        }]
    }'
```

## VERIFICATION

```bash
# Check encryption
aws s3api get-bucket-encryption --bucket my-bucket

# Check IAM
aws iam list-policies --scope Local
```

## CLEANUP

```bash
# Delete resources created
```

## CROSS-REFERENCES

### Prerequisites

- IAM, VPC basics