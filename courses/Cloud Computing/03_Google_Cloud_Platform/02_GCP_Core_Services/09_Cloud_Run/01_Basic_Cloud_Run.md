---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Run
Purpose: Understanding GCP Cloud Run serverless containers
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Cloud_Run.md, 03_Practical_Cloud_Run.md
UseCase: Serverless container deployment on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Cloud Run provides a fully managed serverless platform for running containers. Understanding Cloud Run enables rapid container deployment without infrastructure management.

## 📖 WHAT

### Cloud Run Features

- **Fully Managed**: No infrastructure management
- **Stateless**: Containers with no local state
- **Auto-scaling**: Zero to many instances
- **HTTPS**: Built-in load balancing
- **Container Runtime**: Any language/framework

## 🔧 HOW

### Example: Deploy Container

```bash
# Deploy to Cloud Run
gcloud run deploy my-service \
    --image=gcr.io/my-project/my-image \
    --region=us-central1 \
    --allow-unauthenticated

# List services
gcloud run services list

# Describe service
gcloud run services describe my-service --region=us-central1
```

## ✅ EXAM TIPS

- Fully managed serverless
- Auto-scales to zero
- Pay per request
- Any container image
