---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: Cloud KMS
Purpose: Advanced understanding of Cloud KMS features and configurations
Difficulty: intermediate
Prerequisites: 01_Basic_Cloud_KMS.md
RelatedFiles: 01_Basic_Cloud_KMS.md, 03_Practical_Cloud_KMS.md
UseCase: Enterprise key management, encryption at scale
CertificationExam: GCP Professional Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced KMS knowledge enables implementing enterprise key management with hardware security, automated rotation, and integration with other GCP services.

### Why Advanced KMS

- **HSM Integration**: Hardware security module
- **Key Rotation**: Automatic key rotation
- **IAM Integration**: Fine-grained access
- **Cloud EKM**: External key management

## 📖 WHAT

### Key Rotation Strategies

**Automatic Rotation**:
- 90-day default interval
- Version management
- Decrypt with old versions

**Manual Rotation**:
- Custom rotation schedules
- Immediate rotation
- Version migration

### External Key Management (EKM)

- Keys stored externally
- Google as API gateway
- Customer control over keys
- Use cases: regulatory requirements

## 🔧 HOW

### Example 1: HSM Key Configuration

```bash
# Create keyring
gcloud kms keyrings create hsm-keyring \
    --location=global

# Create HSM-backed key
gcloud kms keys create hsm-key \
    --keyring=hsm-keyring \
    --location=global \
    --purpose=encryption \
    --protection-level=HSM \
    --rotation-period=90d \
    --next-rotation-time=2025-02-01T00:00:00Z

# Verify protection level
gcloud kms keys describe hsm-key \
    --keyring=hsm-keyring \
    --location=global
```

### Example 2: Key Rotation

```bash
# Enable automatic rotation
gcloud kms keys update rotation-key \
    --keyring=my-keyring \
    --location=global \
    --rotation-period=90d \
    --next-rotation-time=2025-02-01T00:00:00Z

# Trigger manual rotation
gcloud kms keys rotate rotation-key \
    --keyring=my-keyring \
    --location=global

# List key versions
gcloud kms keys versions list \
    --key=rotation-key \
    --keyring=my-keyring \
    --location=global

# Disable a key version
gcloud kms keys versions disable key-version-id \
    --key=my-key \
    --keyring=my-keyring \
    --location=global
```

### Example 3: IAM Policies

```bash
# Grant encrypt permission
gcloud kms keyrings add-iam-policy-binding my-keyring \
    --location=global \
    --member=user:admin@example.com \
    --role=roles/cloudkms.cryptoKeyEncrypter

# Grant decrypt permission
gcloud kms keyrings add-iam-policy-binding my-keyring \
    --location=global \
    --member=serviceAccount:app@project.iam.gserviceaccount.com \
    --role=roles/cloudkms.cryptoKeyDecrypter

# Create key with specific policy
gcloud kms keys create policy-key \
    --keyring=my-keyring \
    --location=global \
    --purpose=encryption \
    --labels=team=security

# Use conditional IAM
gcloud kms keyrings add-iam-policy-binding my-keyring \
    --location=global \
    --member=user:dev@example.com \
    --role=roles/cloudkms.cryptoKeyEncrypter \
    --condition="expression=resource.name.endsWith('dev-key'),title=Dev Access"
```

## ⚠️ COMMON ISSUES

### Troubleshooting KMS Issues

| Issue | Solution |
|-------|----------|
| Permission denied | Check IAM roles |
| Key unavailable | Check key state |
| HSM quota | Request increase |

### Best Practices

- Use HSM for sensitive data
- Enable automatic rotation
- Use separate keyrings by environment

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP KMS | AWS KMS | Azure Key Vault |
|---------|---------|---------|-----------------|
| HSM | Yes | Yes | Yes |
| Auto-rotation | Yes | Yes | Yes |
| Cloud EKM | Yes | No | No |
| Key versioning | Yes | Yes | Yes |

## 🔗 CROSS-REFERENCES

### Related Topics

- Cloud Storage (encryption)
- Cloud SQL (encryption)
- Cloud DLP (data protection)

### Study Resources

- Cloud KMS documentation
- Encryption best practices

## ✅ EXAM TIPS

- HSM for hardware security
- Automatic rotation every 90 days
- Use separate keyrings by environment
- Cloud EKM for external key control
