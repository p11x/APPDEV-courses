---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Spanner
Purpose: Understanding GCP Cloud Spanner globally distributed database
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Cloud_Spanner.md, 03_Practical_Cloud_Spanner.md
UseCase: Globally distributed relational databases on GCP
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Cloud Spanner is Google's globally distributed relational database that provides unlimited scaling with strong consistency. Understanding Spanner is essential for global applications.

## 📖 WHAT

### Spanner Features

- **Global Distribution**: Multi-region, multi-continental
- **Horizontal Scaling**: Scales horizontally
- **ACID Transactions**: Strong consistency
- **SQL Support**: ANSI SQL queries
- **99.999% SLA**: High availability

## 🔧 HOW

### Example: Create Instance

```bash
# Create Spanner instance
gcloud spanner instances create my-instance \
    --config=regional-us-central1 \
    --description="My Instance" \
    --nodes=3

# Create database
gcloud spanner databases create my-database \
    --instance=my-instance

# Create table
gcloud spanner databases ddl update my-database \
    --instance=my-instance \
    --ddl='CREATE TABLE Singers (
        SingerId INT64 NOT NULL,
        FirstName STRING(1024),
        LastName STRING(1024),
        SingerInfo STRING(1024)
    ) PRIMARY KEY (SingerId)'
```

## ✅ EXAM TIPS

- Globally distributed database
- Strong consistency
- Horizontal scaling
- 99.999% availability
