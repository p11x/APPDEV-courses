---
Category: AWS Cloud Practitioner
Subcategory: AWS Security
Concept: Encryption
Purpose: Understanding AWS encryption services including KMS, CloudHSM, and encryption at rest/in transit
Difficulty: intermediate
Prerequisites: 01_Basic_IAM.md, 01_Basic_Shared_Responsibility.md
RelatedFiles: 02_Advanced_Encryption.md, 03_Practical_Encryption.md
UseCase: Implementing encryption for data protection
CertificationExam: AWS Security Specialty
LastUpdated: 2025
---

## 💡 WHY

Encryption is fundamental to data security in the cloud. AWS provides comprehensive encryption services to protect data at rest and in transit.

### Why Encryption Matters

- **Compliance**: Required by regulations (PCI-DSS, HIPAA, GDPR)
- **Data Protection**: Protect sensitive information
- **Defense in Depth**: Layer of security
- **Trust**: Customer confidence

### Types of Encryption

- **At Rest**: Data stored on disks, S3, databases
- **In Transit**: Data moving over network (TLS/SSL)

## 📖 WHAT

### AWS Encryption Services

**KMS (Key Management Service)**: Managed key service

**CloudHSM**: Dedicated hardware security module

**ACM (Certificate Manager)**: TLS certificates

**Secrets Manager**: Encrypted credential storage

### Key Types

| Type | Description |
|------|-------------|
| AWS Managed | Created by AWS services |
| Customer Managed | Created by you |
| AWS Owned | Cross-account keys |
| Symmetric | Same key for encrypt/decrypt |
| Asymmetric | Public/private key pair |

### Encryption Methods

| Service | Encryption |
|---------|------------|
| S3 | AES-256, AWS-KMS |
| EBS | AES-256 |
| RDS | AES-256, TDE |
| Lambda | AWS-KMS |
| DynamoDB | AWS-KMS |

## 🔧 HOW

### Example 1: KMS Key Management

```bash
# Create customer managed key
KEY_ID=$(aws kms create-key \
    --description "My encryption key" \
    --key-usage ENCRYPT_DECRYPT \
    --origin AWS_KMS \
    --query 'KeyMetadata.KeyId' \
    --output text)
echo "Key ID: $KEY_ID"

# Create alias
aws kms create-alias \
    --alias-name alias/my-key \
    --target-key-id $KEY_ID

# Encrypt data
aws kms encrypt \
    --key-id alias/my-key \
    --plaintext "Sensitive data" \
    --output text --query CiphertextBlob | base64 -d > encrypted.bin

# Decrypt data
aws kms decrypt \
    --ciphertext-blob fileb://encrypted.bin \
    --output text --query Plaintext | base64 -d

# Enable key rotation
aws kms enable-key-rotation \
    --key-id $KEY_ID
```

### Example 2: S3 Encryption

```bash
# Create bucket with default encryption (SSE-S3)
aws s3api create-bucket --bucket encrypted-bucket
aws s3api put-bucket-encryption \
    --bucket encrypted-bucket \
    --server-side-encryption-configuration '{
        "Rules": [{
            "ApplyServerSideEncryptionByDefault": {
                "SSEAlgorithm": "AES256"
            }
        }]
    }'

# Create bucket with KMS encryption
aws s3api put-bucket-encryption \
    --bucket kms-bucket \
    --server-side-encryption-configuration '{
        "Rules": [{
            "ApplyServerSideEncryptionByDefault": {
                "SSEAlgorithm": "aws:kms",
                "KMSKeyId": "alias/my-key"
            }
        }]
    }'

# Encrypt object upload
aws s3 cp secret.txt s3://kms-bucket/secret.txt \
    --sse aws:kms \
    --sse-kms-key-id alias/my-key
```

### Example 3: EBS Encryption

```bash
# Create encrypted EBS volume
aws ec2 create-volume \
    --size 100 \
    --availability-zone us-east-1a \
    --encrypted \
    --volume-type gp3

# Enable encryption by default
aws ec2 enable-ebs-encryption-by-default \
    --region us-east-1

# Check encryption status
aws ec2 get-ebs-encryption-by-default \
    --region us-east-1
```

### Example 4: RDS Encryption

```bash
# Create encrypted RDS instance
aws rds create-db-instance \
    --db-instance-identifier encrypted-db \
    --db-instance-class db.t3.medium \
    --engine mysql \
    --allocated-storage 100 \
    --master-username admin \
    --master-user-password 'SecurePass123!' \
    --storage-encrypted

# Enable encryption on existing instance (create snapshot first)
aws rds create-db-snapshot \
    --db-snapshot-identifier pre-encrypt-snap \
    --db-instance-identifier my-db

# Copy snapshot with encryption
aws rds copy-db-snapshot \
    --source-db-snapshot-identifier pre-encrypt-snap \
    --target-db-snapshot-identifier encrypted-snap \
    --storage-encrypted \
    --kms-key-id alias/my-key
```

## ⚠️ COMMON ISSUES

### 1. Key Access Denied

**Problem**: Cannot encrypt/decrypt with key.

**Solution**: Check IAM policy grants kms:Encrypt, kms:Decrypt.

### 2. Cross-Account Encryption

**Problem**: Cannot share encrypted resources.

**Solution**: Grant cross-account key usage in key policy.

### 3. Encryption Not Enabled

**Problem**: Data stored unencrypted.

**Solution**: Enable encryption at creation, use default encryption.

### 4. Lost Key

**Problem**: KMS key deleted, data unrecoverable.

**Solution**: Use key rotation, maintain backups, don't delete keys without backup.

## 🏃 PERFORMANCE

### Key Limits

| Resource | Limit |
|----------|-------|
| Keys per region | 10,000 |
| Requests/sec | 10,000 |
| Key size | 256-bit |

## 🌐 COMPATIBILITY

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Key Management | KMS | Key Vault | Cloud KMS |
| HSM | CloudHSM | Dedicated HSM | Cloud HSM |
| Object Encryption | S3 encryption | Storage encryption | Storage encryption |

## 🔗 CROSS-REFERENCES

**Related**: IAM, S3, RDS, EC2

**Prerequisites**: IAM basics

**Next**: Secrets Manager for credential management

## ✅ EXAM TIPS

- KMS keys are regional
- Use CMK (customer managed key) for control
- Enable key rotation for added security
- S3 supports SSE-S3, SSE-KMS, SSE-C
- EBS encryption is at host level