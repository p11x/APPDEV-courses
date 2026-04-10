---
Category: Google Cloud Platform
Subcategory: GCP Data AI
Concept: Dataproc
Purpose: Hands-on exercises for Dataproc cluster management and job submission
Difficulty: advanced
Prerequisites: 01_Basic_Dataproc.md, 02_Advanced_Dataproc.md
RelatedFiles: 01_Basic_Dataproc.md, 02_Advanced_Dataproc.md
UseCase: Big data processing, Spark jobs, Hadoop workflows
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Dataproc is essential for managing big data clusters, running Spark jobs, and processing large-scale data workloads.

### Lab Goals

- Create Dataproc clusters
- Submit Spark jobs
- Configure auto-scaling

## 📖 WHAT

### Exercise Overview

1. **Cluster Creation**: Standard and HA clusters
2. **Job Submission**: Spark and Hadoop jobs
3. **Cluster Management**: Auto-scaling and monitoring

## 🔧 HOW

### Exercise 1: Create and Configure Cluster

```bash
#!/bin/bash
# Create and configure Dataproc cluster

PROJECT_ID="my-project-id"
CLUSTER_NAME="production-cluster"

gcloud config set project $PROJECT_ID

# Create cluster with custom configuration
gcloud dataproc clusters create $CLUSTER_NAME \
    --region=us-central1 \
    --zone=us-central1-a \
    --num-masters=1 \
    --num-workers=4 \
    --num-preemptible-workers=8 \
    --machine-type=n1-standard-8 \
    --preemptible-machine-type=n1-standard-4 \
    --master-boot-disk-size=100GB \
    --worker-boot-disk-size=100GB \
    --image-version=2.1-debian11 \
    --properties="spark:spark.driver.memory=4g,spark:spark.executor.memory=4g" \
    --metadata 'pip-packages=pandas,numpy' \
    --enable-component-gateway

# List clusters
gcloud dataproc clusters list --region=us-central1

# Describe cluster
gcloud dataproc clusters describe $CLUSTER_NAME --region=us-central1

echo "Cluster created successfully!"
```

### Exercise 2: Submit Spark Jobs

```bash
#!/bin/bash
# Submit Spark jobs to Dataproc

PROJECT_ID="my-project-id"
CLUSTER_NAME="production-cluster"

gcloud config set project $PROJECT_ID

# Submit PySpark job
gcloud dataproc jobs submit pyspark \
    --cluster=$CLUSTER_NAME \
    --region=us-central1 \
    --jars=gs://spark-lib/spark-examples_2.12-3.3.0.jar \
    main.py

# Create main.py
cat > main.py << 'EOF'
from pyspark.sql import SparkSession
import sys

def main():
    spark = SparkSession.builder.appName("WordCount").getOrCreate()
    
    # Read input
    lines = spark.read.text(sys.argv[1]).rdd.map(lambda r: r[0])
    
    # Count words
    counts = lines.flatMap(lambda x: x.split()) \
        .map(lambda x: (x, 1)) \
        .reduceByKey(lambda a, b: a + b)
    
    # Save output
    counts.saveAsTextFile(sys.argv[2])
    
    spark.stop()

if __name__ == "__main__":
    main()
EOF

# Submit with custom arguments
gcloud dataproc jobs submit pyspark \
    --cluster=$CLUSTER_NAME \
    --region=us-central1 \
    --properties="spark.driver.cores=2,spark.executor.cores=2" \
    main.py \
    -- gs://my-bucket/input/ gs://my-bucket/output/

# Submit Spark Scala job
gcloud dataproc jobs submit spark \
    --cluster=$CLUSTER_NAME \
    --region=us-central1 \
    --class=org.apache.spark.examples.SparkPi \
    --jars=gs://spark-lib/spark-examples_2.12-3.3.0.jar \
    -- 1000

echo "Spark jobs submitted!"
```

### Exercise 3: Configure Auto-scaling

```bash
#!/bin/bash
# Configure auto-scaling for Dataproc

PROJECT_ID="my-project-id"
CLUSTER_NAME="production-cluster"

gcloud config set project $PROJECT_ID

# Create autoscaling policy
gcloud dataproc autoscaling-policies create scale-policy \
    --region=us-central1 \
    --worker-config minInstances=2,maxInstances=20,numInstances=4 \
    --preemptible-config maxInstances=30,numInstances=10 \
    --basic-algo yq \
    --cooldown-period=300s \
    --scale-up-factor=1.0 \
    --scale-down-factor=0.5

# Apply policy to cluster
gcloud dataproc clusters update $CLUSTER_NAME \
    --region=us-central1 \
    --autoscaling-policy=projects/$PROJECT_ID/regions/us-central1/autoscalingPolicies/scale-policy

# Update policy
gcloud dataproc autoscaling-policies update scale-policy \
    --region=us-central1 \
    --worker-config minInstances=3,maxInstances=25,numInstances=5 \
    --preemptible-config maxInstances=40,numInstances=15

# List policies
gcloud dataproc autoscaling-policies list --region=us-central1

# Describe policy
gcloud dataproc autoscaling-policies describe scale-policy --region=us-central1

echo "Auto-scaling configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Job fails | Check driver logs |
| Slow performance | Enable auto-scaling |
| Cluster issues | Check quota |

### Validation

```bash
# Check cluster status
gcloud dataproc clusters describe my-cluster --region=us-central1

# List jobs
gcloud dataproc jobs list --region=us-central1 --cluster=my-cluster
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Storage: Data source
- BigQuery: Data export
- Cloud Logging: Job logs

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud Storage
- BigQuery
- Dataflow

### Next Steps

- Set up monitoring
- Configure alerts
- Implement CI/CD

## ✅ EXAM TIPS

- Practice cluster creation commands
- Know job submission options
- Understand auto-scaling policies
- Remember preemptible workers
