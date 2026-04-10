---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Storage
Purpose: Understanding GCP Cloud Storage object storage
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Cloud_Storage.md, 03_Practical_Cloud_Storage.md
UseCase: Storing objects on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Cloud Storage provides object storage with global edge-caching. Understanding storage options is essential for GCP deployments.

## 📖 WHAT

### Storage Classes

| Class | Use Case | Availability |
|-------|----------|---------------|
| Standard | Hot data | 99.95% |
| Nearline | Monthly access | 99.9% |
| Coldline | Quarterly access | 99.9% |
| Archive | Yearly access | 99.9% |

## 🔧 HOW

### Example: Create Bucket

```bash
# Create bucket
gsutil mb -l us-central1 gs://my-bucket

# Upload object
gsutil cp myfile.txt gs://my-bucket/

# Set lifecycle policy
gsutil lifecycle set policy.json gs://my-bucket
```

## ✅ EXAM TIPS

- Nearline = <30 day access
- Coldline = <90 day access
- Archive = <365 day access