---
Category: Google Cloud Platform
Subcategory: GCP Data AI
Concept: Vertex AI
Purpose: Hands-on exercises for Vertex AI model training and deployment
Difficulty: advanced
Prerequisites: 01_Basic_Vertex_AI.md, 02_Advanced_Vertex_AI.md
RelatedFiles: 01_Basic_Vertex_AI.md, 02_Advanced_Vertex_AI.md
UseCase: Production ML workflows, model deployment, AutoML
CertificationExam: GCP Data Engineer / Machine Learning Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Vertex AI is essential for building production ML systems, deploying models, and implementing MLOps practices.

### Lab Goals

- Train AutoML models
- Deploy custom models
- Build ML pipelines

## 📖 WHAT

### Exercise Overview

1. **AutoML Training**: No-code model building
2. **Custom Training**: GPU-accelerated training
3. **Model Deployment**: Online and batch predictions

## 🔧 HOW

### Exercise 1: Train AutoML Model

```bash
#!/bin/bash
# Train AutoML model

PROJECT_ID="my-project-id"
DATASET_NAME="housing_data"

gcloud config set project $PROJECT_ID

# Create dataset
gcloud aiplatform datasets create $DATASET_NAME \
    --display-name=$DATASET_NAME \
    --dataset-type=tabular \
    --region=us-central1

# Import training data
gcloud aiplatform datasets import $DATASET_NAME \
    --gcs-uris=gs://my-bucket/train.csv \
    --region=us-central1

# Train AutoML model
gcloud aiplatform models train auto-housing \
    --dataset=$DATASET_NAME \
    --display-name=auto-housing-model \
    --training-target=price \
    --optimization-objective=maximize-r2 \
    --region=us-central1

# Create endpoint
gcloud aiplatform endpoints create housing-endpoint \
    --display-name=housing-endpoint \
    --region=us-central1

# Deploy model
gcloud aiplatform models deploy auto-housing \
    --endpoint=housing-endpoint \
    --machine-type=n1-standard-4

# Make prediction
gcloud aiplatform predict housing-endpoint \
    --region=us-central1 \
    --instance='{"area":1500,"rooms":3,"age":10}'

echo "AutoML model trained and deployed!"
```

### Exercise 2: Custom Training

```bash
#!/bin/bash
# Custom model training

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create training job
gcloud aiplatform custom-jobs create \
    --display-name=custom-trainer \
    --spec='{
        "workerPoolSpecs": [{
            "machineSpec": {"machineType": "n1-standard-8", "gpuType": "nvidia-tesla-v100"},
            "replicaCount": "1",
            "containerSpec": {
                "imageUri": "gcr.io/my-project/trainer:latest",
                "args": ["--epochs", "100", "--batch-size", "32"]
            }
        }]
    }' \
    --region=us-central1

# Hyperparameter tuning
gcloud aiplatform hyperparameter-tuning-jobs create \
    --display-name=hp-tuning \
    --spec='{
        "workerPoolSpecs": [{
            "machineSpec": {"machineType": "n1-standard-4"},
            "replicaCount": "1",
            "containerSpec": {"imageUri": "gcr.io/my-project/trainer:latest"}
        }],
        "parameterSpecs": [
            {"parameterId": "learning_rate", "doubleValueSpec": {"min": 0.001, "max": 0.1}},
            {"parameterId": "batch_size", "discreteValueSpec": {"values": [16, 32, 64]}}
        ],
        "maxTrialCount": 9,
        "trialJobSpec": {"workerPoolSpecs": [{"replicaCount": "1"}]}
    }' \
    --region=us-central1

echo "Custom training configured!"
```

### Exercise 3: ML Pipeline

```bash
#!/bin/bash
# Create ML pipeline

PROJECT_ID="my-project-id"

# Define pipeline components
cat > components/preprocess.yaml << 'EOF'
name: preprocess
implementation:
  container:
    image: gcr.io/my-project/preprocess:latest
    command: ["python", "preprocess.py"]
    args: ["--input", {"inputValue": "input_data"}, {"outputValue": "output_data"}]
EOF

cat > components/train.yaml << 'EOF'
name: train
implementation:
  container:
    image: gcr.io/my-project/train:latest
    command: ["python", "train.py"]
    args: [{"inputValue": "train_data"}, {"outputValue": "model"}]
EOF

# Create pipeline YAML
cat > pipeline.yaml << 'EOF'
pipelineSpec:
  schemaVersion: 2.1.0
  components:
    preprocess:
      executorLabel: preprocess
    train:
      executorLabel: train
  root:
    dag:
      tasks:
        preprocess:
          component: preprocess
        train:
          component: train
          dependencies: [preprocess]
          inputs:
            train_data: preprocess.outputs.output_data
EOF

# Compile pipeline
python -m kfp.v2.compiler compile pipeline.yaml --packaged-pipeline-file=pipeline.json

# Create pipeline job
gcloud aiplatform pipeline-jobs create \
    --pipeline-definition=pipeline.json \
    --display-name=ml-pipeline \
    --region=us-central1

echo "ML pipeline created!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Training fails | Check data format |
| GPU unavailable | Check quota |
| Model poor | Tune hyperparameters |

### Validation

```bash
# Check training status
gcloud aiplatform custom-jobs describe custom-job --region=us-central1

# List models
gcloud aiplatform models list --region=us-central1
```

## 🌐 COMPATIBILITY

### Integration

- BigQuery (data)
- Cloud Storage (models)
- Cloud Logging (monitoring)

## 🔗 CROSS-REFERENCES

### Related Labs

- BigQuery ML
- Dataflow
- Cloud Storage

### Next Steps

- Implement CI/CD for ML
- Set up monitoring
- Configure alerting

## ✅ EXAM TIPS

- Know AutoML vs custom training
- Practice model deployment
- Understand pipeline components
- Monitor training jobs
