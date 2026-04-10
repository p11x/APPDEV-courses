---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud SQL
Purpose: Understanding GCP Cloud SQL managed database service
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Cloud_SQL.md, 03_Practical_Cloud_SQL.md
UseCase: Managed relational databases on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Cloud SQL provides fully managed MySQL, PostgreSQL, and SQL Server databases. Understanding managed databases simplifies operations.

## 📖 WHAT

### Cloud SQL Features

- **High Availability**: Multi-zone failover
- **Read Replicas**: For read scaling
- **Automated Backups**: Point-in-time recovery
- **Private IP**: Secure networking
- **SSL/TLS**: Encryption in transit

## 🔧 HOW

### Example: Create Instance

```bash
# Create MySQL instance
gcloud sql instances create my-instance \
    --zone=us-central1-a \
    --database-version=MYSQL_8_0 \
    --tier=db-f1-micro \
    --root-password=SecurePass123

# Create database
gcloud sql databases create mydb --instance=my-instance
```

## ✅ EXAM TIPS

- Fully managed RDBMS
- Supports MySQL, PostgreSQL, SQL Server
- High availability in multi-zone