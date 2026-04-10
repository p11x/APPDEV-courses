---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: BigQuery
Purpose: Understanding GCP BigQuery serverless data warehouse
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_BigQuery.md, 03_Practical_BigQuery.md
UseCase: Data analytics and warehousing on GCP
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

BigQuery is Google's serverless, highly scalable data warehouse. Understanding BigQuery is essential for data analytics workloads.

## 📖 WHAT

### BigQuery Features

- **Serverless**: No infrastructure management
- **Petabyte Scale**: Query petabytes of data
- **SQL**: Standard SQL queries
- **Real-time**: Streaming inserts
- **ML Integration**: Built-in ML

## 🔧 HOW

### Example: Query Data

```bash
# Create dataset
bq mk mydataset

# Create table from CSV
bq load mydataset.mytable gs://my-bucket/data.csv schema.json

# Run query
bq query --use_legacy_sql=false \
    "SELECT * FROM mydataset.mytable LIMIT 10"
```

## ✅ EXAM TIPS

- Serverless data warehouse
- Pay for query processing only
- Streaming inserts charged per GB