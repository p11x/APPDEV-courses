---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: Cloud KMS
Purpose: Hands-on exercises for Cloud KMS key management
Difficulty: advanced
Prerequisites: 01_Basic_Cloud_KMS.md, 02_Advanced_Cloud_KMS.md
RelatedFiles: 01_Basic_Cloud_KMS.md, 02_Advanced_Cloud_KMS.md
UseCase: Encryption key management, secure data storage, key rotation
CertificationExam: GCP Professional Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Cloud KMS is essential for implementing encryption, managing key lifecycles, and securing sensitive data on GCP.

### Lab Goals

- Create and manage keys
- Configure key rotation
- Implement encryption

## 📖 WHAT

### Exercise Overview

1. **Key Creation**: KeyRing and CryptoKey
2. **Key Rotation**: Automatic rotation
3. **Encryption**: Data encryption

## 🔧 HOW

### Exercise 1: Create and Manage Keys

```bash
#!/bin/bash
# Create and manage Cloud KMS keys

PROJECT_ID="my-project-id"
KEYRING_NAME="production-keyring"

gcloud config set project $PROJECT_ID

# Create keyring
gcloud kms keyrings create $KEYRING_NAME \
    --location=global

# Create symmetric encryption key
gcloud kms keys create encryption-key \
    --keyring=$KEYRING_NAME \
    --location=global \
    --purpose=encryption \
    --protection-level=software \
    --labels=environment=production,team=security

# Create HSM-backed key for sensitive data
gcloud kms keys create hsm-encryption-key \
    --keyring=$KEYRING_NAME \
    --location=global \
    --purpose=encryption \
    --protection-level=hsm \
    --labels=environment=production,team=security

# Create asymmetric key for signing
gcloud kms keys create signing-key \
    --keyring=$KEYRING_NAME \
    --location=global \
    --purpose=asymmetric-signing \
    --default-algorithm=ec-sign-p256-sha256

# List keys
gcloud kms keys list \
    --keyring=$KEYRING_NAME \
    --location=global

# Describe key
gcloud kms keys describe encryption-key \
    --keyring=$KEYRING_NAME \
    --location=global

echo "Keys created successfully!"
```

### Exercise 2: Configure Key Rotation

```bash
#!/bin/bash
# Configure key rotation

PROJECT_ID="my-project-id"
KEYRING_NAME="production-keyring"

gcloud config set project $PROJECT_ID

# Enable automatic rotation
gcloud kms keys update rotation-key \
    --keyring=$KEYRING_NAME \
    --location=global \
    --rotation-period=90d \
    --next-rotation-time=$(date -d "+90 days" -u +%Y-%m-%dT%H:%M:%SZ)

# Trigger manual rotation
gcloud kms keys rotate rotation-key \
    --keyring=$KEYRING_NAME \
    --location=global

# List key versions
gcloud kms keys versions list \
    --key=rotation-key \
    --keyring=$KEYRING_NAME \
    --location=global

# Disable old version
gcloud kms keys versions disable key-version-id \
    --key=rotation-key \
    --keyring=$KEYRING_NAME \
    --location=global

# Schedule key destruction
gcloud kms keys versions destroy key-version-id \
    --key=rotation-key \
    --keyring=$KEYRING_NAME \
    --location=global

# Restore destroyed key (within 30 days)
gcloud kms keys versions restore key-version-id \
    --key=rotation-key \
    --keyring=$KEYRING_NAME \
    --location=global

echo "Key rotation configured!"
```

### Exercise 3: Encrypt Data

```bash
#!/bin/bash
# Encrypt and decrypt data with Cloud KMS

PROJECT_ID="my-project-id"
KEYRING_NAME="production-keyring"

gcloud config set project $PROJECT_ID

# Encrypt a file
echo "Sensitive data" > secret.txt

gcloud kms encrypt \
    --key=encryption-key \
    --keyring=$KEYRING_NAME \
    --location=global \
    --plaintext-file=secret.txt \
    --ciphertext-file=secret.txt.encrypted

# Base64 encode for storage
base64 secret.txt.encrypted > secret.txt.b64
rm secret.txt.encrypted

# Decrypt data
base64 -d secret.txt.b64 > secret.txt.encrypted
gcloud kms decrypt \
    --key=encryption-key \
    --keyring=$KEYRING_NAME \
    --location=global \
    --ciphertext-file=secret.txt.encrypted \
    --plaintext-file=decrypted.txt

cat decrypted.txt

# Encrypt using Cloud Storage integration
gsutil cp secret.txt gs://my-secure-bucket/
gsutil rewrite -k gs://my-secure-bucket/secret.txt

# Encrypt Cloud SQL data
gcloud sql instances patch my-instance \
    --database-flags=ssl-ca=ca.pem,ssl-cert=server-cert.pem,ssl-key=server-key.pem

echo "Data encryption complete!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Permission denied | Check IAM roles |
| HSM quota exceeded | Request increase |
| Decryption fails | Check key version |

### Validation

```bash
# Check key status
gcloud kms keys describe my-key --keyring=my-keyring --location=global

# Verify encryption
gcloud kms keys versions list --key=my-key --keyring=my-keyring --location=global
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Storage
- Cloud SQL
- BigQuery
- Cloud Run

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud Storage Encryption
- Cloud SQL Encryption
- Cloud IAM

### Next Steps

- Implement envelope encryption
- Set up key rotation alerts
- Configure audit logging

## ✅ EXAM TIPS

- Practice key creation commands
- Configure automatic rotation
- Use HSM for sensitive data
- Manage key versions
