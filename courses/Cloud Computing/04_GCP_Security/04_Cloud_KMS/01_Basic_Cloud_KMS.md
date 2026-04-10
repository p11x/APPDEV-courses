---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: Cloud KMS
Purpose: Understanding GCP Cloud Key Management Service
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Cloud_KMS.md, 03_Practical_Cloud_KMS.md
UseCase: Encryption key management on GCP
CertificationExam: GCP Professional Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Cloud KMS provides encryption key management for GCP. Understanding KMS is essential for implementing encryption and securing sensitive data.

## 📖 WHAT

### KMS Components

- **KeyRing**: Groups encryption keys
- **CryptoKey**: The actual encryption key
- **Key Version**: Specific key version

### Key Types

| Type | Use Case |
|------|----------|
| Symmetric | AES-256 encryption |
| Asymmetric | RSA, EC keys |
| HSM | Hardware security module |

## 🔧 HOW

### Example: Create Key

```bash
# Create keyring
gcloud kms keyrings create my-keyring \
    --location=global

# Create key
gcloud kms keys create my-key \
    --keyring=my-keyring \
    --location=global \
    --purpose=encryption

# Encrypt data
echo "secret" | gcloud kms encrypt \
    --key=my-key \
    --keyring=my-keyring \
    --location=global \
    --ciphertext-file=encrypted.txt \
    --plaintext-file=-

# Decrypt data
gcloud kms decrypt \
    --key=my-key \
    --keyring=my-keyring \
    --location=global \
    --plaintext-file=decrypted.txt \
    --ciphertext-file=encrypted.txt
```

## ✅ EXAM TIPS

- KeyRing groups keys
- Symmetric keys for encryption
- HSM for hardware-backed security
- Key rotation supported
