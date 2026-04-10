---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud IAM
Purpose: Understanding GCP Identity and Access Management
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Cloud_IAM.md, 03_Practical_Cloud_IAM.md
UseCase: Managing access on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

GCP IAM provides fine-grained access control across all GCP services. Understanding IAM is essential for GCP security.

## 📖 WHAT

### IAM Components

- **Principal**: User, group, service account
- **Role**: Collection of permissions
- **Policy**: Binds principals to roles

### Role Types

| Type | Description |
|------|-------------|
| Primitive | Owner, Editor, Viewer |
| Predefined | GCP-managed roles |
| Custom | User-defined roles |

## 🔧 HOW

### Example: Grant Access

```bash
# Add member to project
gcloud projects add-iam-policy-binding my-project \
    --member=user:user@example.com \
    --role=roles/viewer

# Create service account
gcloud iam service-accounts create my-sa \
    --display-name "My Service Account"

# Grant role to service account
gcloud projects add-iam-policy-binding my-project \
    --member=serviceAccount:my-sa@my-project.iam.gserviceaccount.com \
    --role=roles/storage.objectViewer
```

## ✅ EXAM TIPS

- Principle = who
- Role = what they can do
- Least privilege = use custom roles