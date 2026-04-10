---
Category: Google Cloud Platform
Subcategory: GCP Data AI
Concept: Dataproc
Purpose: Understanding GCP Dataproc for managed Hadoop and Spark
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Dataproc.md, 03_Practical_Dataproc.md
UseCase: Big data processing, Spark and Hadoop workloads on GCP
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Dataproc provides managed Hadoop, Spark, Hive, and Pig services on GCP. Understanding Dataproc is essential for big data processing workloads.

## 📖 WHAT

### Dataproc Features

- **Managed Clusters**: Hadoop/Spark clusters
- **Pre-emptible**: Cost-effective workers
- **Component Gateway**: Access UIs
- **Initialization Actions**: Custom setup
- **Auto-scaling**: Dynamic cluster sizing

## 🔧 HOW

### Example: Create Cluster

```bash
# Create Dataproc cluster
gcloud dataproc clusters create my-cluster \
    --region=us-central1 \
    --num-workers=3 \
    --machine-type=n1-standard-4

# Submit Spark job
gcloud dataproc jobs submit spark \
    --cluster=my-cluster \
    --region=us-central1 \
    --class=org.apache.spark.examples.SparkPi \
    --jars=gs://spark-lib/\
spark-examples_2.12-3.3.0.jar
```

## ✅ EXAM TIPS

- Managed Hadoop/Spark
- Cost-effective with pre-emptible
- Quick cluster creation (90 seconds)
- Auto-scaling supported
