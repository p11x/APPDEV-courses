---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Functions
Purpose: Understanding GCP Cloud Functions serverless compute
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Cloud_Functions.md, 03_Practical_Cloud_Functions.md
UseCase: Event-driven serverless functions on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Cloud Functions provides serverless function execution on GCP. Understanding serverless helps build event-driven applications.

## 📖 WHAT

### Cloud Functions Features

- **Triggers**: HTTP, Cloud Storage, Pub/Sub, Firestore
- **Gen 2**: 2nd gen for better performance
- **Scaling**: Auto-scale to zero
- **Timeout**: 60-540 seconds
- **Memory**: 128MB-4GB

## 🔧 HOW

### Example: Deploy Function

```bash
# Deploy function
gcloud functions deploy my-function \
    --runtime nodejs18 \
    --trigger-http \
    --allow-unauthenticated

# Deploy with trigger
gcloud functions deploy my-triggered-function \
    --runtime nodejs18 \
    --trigger-bucket gs://my-bucket
```

## ✅ EXAM TIPS

- GCP's serverless compute
- Pay per invocation + execution time
- Gen 2 offers better performance