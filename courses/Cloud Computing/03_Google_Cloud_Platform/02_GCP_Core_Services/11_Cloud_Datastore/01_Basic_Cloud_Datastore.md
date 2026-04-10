---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Datastore
Purpose: Understanding GCP Cloud Datastore NoSQL database
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Cloud_Datastore.md, 03_Practical_Cloud_Datastore.md
UseCase: NoSQL document database for applications on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Cloud Datastore is a fully managed NoSQL document database. Understanding Datastore is essential for building scalable applications requiring flexible schema.

## 📖 WHAT

### Datastore Features

- **NoSQL**: Document storage
- **Auto-scaling**: Automatic capacity
- **Strong Consistency**: Transactional updates
- **Full-text Search**: Search capabilities
- **Indexes**: Query performance

## 🔧 HOW

### Example: Create Entity

```bash
# Enable Datastore
gcloud datastore databases create --location=us-central1

# Create entity (using gcloud)
gcloud datastore entities insert \
    --kind=Task \
    --properties='{"description": "Buy milk", "done": false}'

# Query entities
gcloud datastore entities query \
    --kind=Task \
    --filter="done = false"
```

## ✅ EXAM TIPS

- Fully managed NoSQL
- Automatic scaling
- Strong consistency
- Entity-Property-Value model
