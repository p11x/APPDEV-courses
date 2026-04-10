---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: Secret Manager
Purpose: Understanding GCP Secret Manager for secure credential storage
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Secret_Manager.md, 03_Practical_Secret_Manager.md
UseCase: Securely storing API keys, passwords, and secrets
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Secret Manager provides secure storage for sensitive data like API keys and passwords. Understanding Secret Manager helps build secure applications.

## 📖 WHAT

### Secret Manager Features

- **Versioning**: Keep secret history
- **Labels**: Organize secrets
- **IAM Integration**: Fine-grained access
- **Audit Logging**: Track access
- **Rotation**: Automated rotation support

## 🔧 HOW

### Example: Manage Secrets

```bash
# Create secret
gcloud secrets create my-secret \
    --replication-policy=automatic \
    --data-file=secret.txt

# Get secret
gcloud secrets versions access latest \
    --secret=my-secret

# List secrets
gcloud secrets list
```

## ✅ EXAM TIPS

- Store API keys, passwords, certificates
- Versions for rotation
- IAM controls access