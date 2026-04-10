---
Category: Google Cloud Platform
Subcategory: GCP Data AI
Concept: Dataflow
Purpose: Understanding GCP Dataflow for data processing
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Dataflow.md, 03_Practical_Dataflow.md
UseCase: ETL pipelines, stream processing on GCP
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Dataflow provides serverless, fully managed data processing for both batch and streaming data. Understanding Dataflow is essential for building data pipelines on GCP.

## 📖 WHAT

### Dataflow Features

- **Serverless**: No infrastructure management
- **Batch & Streaming**: Unified processing model
- **Auto-scaling**: Automatic resource allocation
- **Exactly-once**: Precise processing semantics
- **Windowing**: Time-based aggregations

## 🔧 HOW

### Example: Create Dataflow Job

```bash
# Create Dataflow job from template
gcloud dataflow jobs run my-job \
    --gcs-location=gs://dataflow-templates/latest/templates/GCS_Text_to_BigQuery \
    --region=us-central1 \
    --parameters inputBucket=gs://my-bucket,outputTable=my-project:dataset.table
```

## ✅ EXAM TIPS

- Serverless data processing
- Supports batch and streaming
- Auto-scales based on work
- Uses Apache Beam SDK
