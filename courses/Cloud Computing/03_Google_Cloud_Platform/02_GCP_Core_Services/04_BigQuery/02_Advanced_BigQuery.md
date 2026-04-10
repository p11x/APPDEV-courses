---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: BigQuery
Purpose: Advanced understanding of BigQuery features and query optimization
Difficulty: intermediate
Prerequisites: 01_Basic_BigQuery.md
RelatedFiles: 01_Basic_BigQuery.md, 03_Practical_BigQuery.md
UseCase: Enterprise analytics, ML integration, cost optimization
CertificationExam: GCP Data Engineer / Professional Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced BigQuery knowledge enables building scalable analytics platforms, implementing ML workflows, and optimizing query costs for enterprise workloads.

### Why Advanced BigQuery

- **Partitioning & Clustering**: Query performance optimization
- **BI Engine**: Fast BI queries
- **ML Integration**: Built-in ML capabilities
- **Slot Reservation**: Predictable performance

## 📖 WHAT

### Performance Optimization Features

**Table Partitioning**:
- By time-unit column
- By ingestion time
- Date/timestamp supported

**Table Clustering**:
- Up to 4 columns
- Automatic data organization
- Reduced data scanned

### Cost Management

- On-demand vs flat-rate pricing
- Reserved slots for predictable costs
- Query caching
- Controlled data access

## 🔧 HOW

### Example 1: Partitioned and Clustered Tables

```bash
# Create partitioned table
bq mk --table \
    --schema "timestamp:TIMESTAMP,user_id:STRING,event:STRING,value:FLOAT" \
    --time_partitioning_field=timestamp \
    --time_partitioning_type=DAY \
    myproject:mydataset.partitioned_table

# Create table with clustering
bq mk --table \
    --schema "timestamp:TIMESTAMP,user_id:STRING,event:STRING,region:STRING,value:FLOAT" \
    --time_partitioning_field=timestamp \
    --clustering_fields=event,region \
    myproject:mydataset.clustered_table

# Query with partition pruning
bq query --use_legacy_sql=false \
    "SELECT * FROM myproject.mydataset.partitioned_table
     WHERE timestamp BETWEEN '2025-01-01' AND '2025-01-31'"
```

### Example 2: BigQuery ML

```bash
# Create ML model for forecasting
bq query --use_legacy_sql=false \
    "CREATE OR REPLACE MODEL mydataset.forecast_model
     OPTIONS(model_type='ARIMA_PLUS',
            time_series_timestamp_column='timestamp',
            time_series_data_columns=['value'])
     AS SELECT timestamp, value
     FROM mydataset.time_series_data"

# Make predictions
bq query --use_legacy_sql=false \
    "SELECT * FROM ML.FORECAST(MODEL mydataset.forecast_model,
                               STRUCT(30 AS horizon))"

# Create classification model
bq query --use_legacy_sql=false \
    "CREATE OR REPLACE MODEL mydataset.classifier
     OPTIONS(model_type='LOGISTIC_REG',
            labels=['will_purchase'])
     AS SELECT * FROM mydataset.training_data"
```

### Example 3: Slot Reservation and Cost Management

```bash
# Create reservation
bq mk -q --reservation \
    --project_id=myproject \
    --slot_count=100 \
    --name=analytics_reservation

# Assign reservation to project
bq assign-reservations \
    --project_id=myproject \
    --assignment_id=analytics_assignment \
    --reservation_name=analytics_reservation \
    --job_type=QUERY

# Create flex slot reservation
bq mk -q --reservation \
    --project_id=myproject \
    --slot_count=500 \
    --name=flex_slots \
    --plan=FLEX

# Monitor slot usage
bq query --use_legacy_sql=false \
    "SELECT * FROM INFORMATION_SCHEMA.JOBS_BY_PROJECT
     WHERE creation_time > TIMESTAMP_SUB(CURRENT_TIMESTAMP(), INTERVAL 1 DAY)"
```

## ⚠️ COMMON ISSUES

### Troubleshooting Performance

| Issue | Solution |
|-------|----------|
| Slow queries | Use partitioning, clustering |
| High costs | Enable query cache, limit data |
| Quota exceeded | Request quota increase |

### Cost Optimization Tips

- Use LIMIT to limit query output
- Avoid SELECT *
- Use LIMIT on table metadata queries
- Enable query cache

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP BigQuery | AWS Redshift | Azure Synapse |
|---------|--------------|--------------|---------------|
| Serverless | Yes | Limited | Limited |
| Partitioning | Yes | Yes | Yes |
| ML | Yes (BQML) | SageMaker | Synapse ML |
| Storage | Petabyte scale | Petabyte scale | Petabyte scale |

## 🔗 CROSS-REFERENCES

### Related Topics

- Cloud Storage (data import)
- Dataflow (ETL pipelines)
- Looker (visualization)

### Study Resources

- BigQuery documentation
- Best practices for query optimization

## ✅ EXAM TIPS

- Partitioning reduces scanned data
- Clustering organizes data within partition
- BQML for built-in ML
- Reserved slots for predictable performance
