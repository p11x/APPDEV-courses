---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: BigQuery
Purpose: Hands-on exercises for BigQuery analytics and optimization
Difficulty: advanced
Prerequisites: 01_Basic_BigQuery.md, 02_Advanced_BigQuery.md
RelatedFiles: 01_Basic_BigQuery.md, 02_Advanced_BigQuery.md
UseCase: Analytics workflows, ML pipelines, cost optimization
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with BigQuery is essential for building analytics platforms and managing large-scale data processing workflows.

### Lab Goals

- Create optimized table schemas
- Build ML models
- Manage costs effectively

## 📖 WHAT

### Exercise Overview

1. **Schema Design**: Partitioned and clustered tables
2. **ML Workflows**: BigQuery ML models
3. **Cost Management**: Slot reservations

## 🔧 HOW

### Exercise 1: Design Optimized Schema

```bash
#!/bin/bash
# Create optimized BigQuery schema

PROJECT_ID="my-project-id"
DATASET_NAME="analytics"

# Create dataset
bq mk --dataset --location=US $PROJECT_ID:$DATASET_NAME

# Create partitioned table
bq mk --table \
    --schema "timestamp:TIMESTAMP,user_id:STRING,event:STRING,platform:STRING,revenue:FLOAT" \
    --time_partitioning_field=timestamp \
    --time_partitioning_type=DAY \
    $PROJECT_ID:$DATASET_NAME.events

# Create clustered table
bq mk --table \
    --schema "date:DATE,region:STRING,category:STRING,product_id:STRING,quantity:INT64,price:FLOAT" \
    --time_partitioning_field=date \
    --clustering_fields=region,category \
    $PROJECT_ID:$DATASET_NAME.sales

# Insert sample data
bq query --use_legacy_sql=false \
    "INSERT INTO $PROJECT_ID.$DATASET_NAME.events (timestamp, user_id, event, platform, revenue)
     VALUES (CURRENT_TIMESTAMP(), 'user1', 'purchase', 'mobile', 99.99)"

# Test partition pruning
bq query --use_legacy_sql=false \
    "SELECT COUNT(*) as total_events
     FROM $PROJECT_ID.$DATASET_NAME.events
     WHERE DATE(timestamp) = CURRENT_DATE()"

echo "Optimized schema created!"
```

### Exercise 2: Build ML Model

```bash
#!/bin/bash
# Build ML model with BigQuery ML

PROJECT_ID="my-project-id"
DATASET_NAME="analytics"

# Create training data
bq query --use_legacy_sql=false \
    "CREATE OR REPLACE TABLE $PROJECT_ID.$DATASET_NAME.training_data AS
     SELECT * FROM $PROJECT_ID.$DATASET_NAME.events
     WHERE revenue > 0"

# Create classification model
bq query --use_legacy_sql=false \
    "CREATE OR REPLACE MODEL $PROJECT_ID.$DATASET_NAME.user_classifier
     OPTIONS(model_type='LOGISTIC_REG',
            labels=['high_value'],
            optimize_for='classification')
     AS SELECT
        CASE WHEN revenue > 100 THEN TRUE ELSE FALSE END as high_value,
        platform,
        event
     FROM $PROJECT_ID.$DATASET_NAME.training_data"

# Evaluate model
bq query --use_legacy_sql=false \
    "SELECT * FROM ML.EVALUATE(MODEL $PROJECT_ID.$DATASET_NAME.user_classifier)"

# Make predictions
bq query --use_legacy_sql=false \
    "SELECT user_id, predicted_high_value
     FROM ML.PREDICT(MODEL $PROJECT_ID.$DATASET_NAME.user_classifier,
                     (SELECT 'mobile' as platform, 'purchase' as event))"

echo "ML model created and evaluated!"
```

### Exercise 3: Cost Management

```bash
#!/bin/bash
# Manage BigQuery costs

PROJECT_ID="my-project-id"

# Enable query cache
bq query --use_cache --use_legacy_sql=false \
    "SELECT COUNT(*) FROM myproject.mydataset.mytable"

# Create reservation for consistent workloads
bq mk -q --reservation \
    --project_id=$PROJECT_ID \
    --slot_count=100 \
    --name=analytics_reservation

# Run query with reservation
bq query --use_legacy_sql=false \
    --reservation=$PROJECT_ID.analytics_reservation \
    "SELECT * FROM myproject.mydataset.large_table"

# Monitor costs with INFORMATION_SCHEMA
bq query --use_legacy_sql=false \
    "SELECT user_email, total_bytes_processed, total_bytes_billed
     FROM INFORMATION_SCHEMA.JOBS_BY_USER
     WHERE creation_time > TIMESTAMP_SUB(CURRENT_TIMESTAMP(), INTERVAL 7 DAY)
     ORDER BY total_bytes_billed DESC"

# Create daily cost alert
bq query --use_legacy_sql=false \
    "SELECT
       DATE(creation_time) as date,
       SUM(total_bytes_billed) / 1e12 as tb_billed
     FROM INFORMATION_SCHEMA.JOBS_BY_PROJECT
     WHERE creation_time > TIMESTAMP_SUB(CURRENT_TIMESTAMP(), INTERVAL 30 DAY)
     GROUP BY DATE(creation_time)"

echo "Cost management configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Slow queries | Check partitioning |
| High costs | Use query cache |
| Quota errors | Request increase |

### Validation

```bash
# Check table info
bq show myproject:mydataset.mytable

# Check partition info
bq query --use_legacy_sql=false \
    "SELECT * FROM myproject.mydataset.INFORMATION_SCHEMA.PARTITIONS"
```

## 🌐 COMPATIBILITY

### Integration

- Looker Studio: Visualization
- Cloud Storage: Data import/export
- Dataflow: ETL pipelines

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud Storage Import
- Dataflow ETL
- Looker Studio

### Next Steps

- Set up scheduling
- Configure alerts
- Implement data governance

## ✅ EXAM TIPS

- Practice partitioning queries
- Understand clustering benefits
- Know BQML capabilities
- Monitor query costs
