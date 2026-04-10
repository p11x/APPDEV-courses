---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Functions
Purpose: Hands-on exercises for Cloud Functions deployment and management
Difficulty: advanced
Prerequisites: 01_Basic_Cloud_Functions.md, 02_Advanced_Cloud_Functions.md
RelatedFiles: 01_Basic_Cloud_Functions.md, 02_Advanced_Cloud_Functions.md
UseCase: Production serverless applications, event-driven workflows
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Cloud Functions is essential for building production serverless applications and event-driven architectures.

### Lab Goals

- Deploy production functions
- Configure event triggers
- Implement VPC access
- Manage costs

## 📖 WHAT

### Exercise Overview

1. **Gen 2 Deployment**: High-performance functions
2. **Event Triggers**: Storage, Pub/Sub, Firestore
3. **VPC Integration**: Private resource access

## 🔧 HOW

### Exercise 1: Deploy Production Gen 2 Function

```bash
#!/bin/bash
# Deploy production-ready Gen 2 function

PROJECT_ID="my-project-id"
FUNCTION_NAME="production-function"

gcloud config set project $PROJECT_ID

# Create function directory
mkdir -p functions/$FUNCTION_NAME
cd functions/$FUNCTION_NAME

# Create function code
cat > index.js << 'EOF'
exports.handler = (req, res) => {
  const start = Date.now();
  // Process request
  const result = {
    message: 'Processing complete',
    timestamp: new Date().toISOString(),
    processingTime: Date.now() - start
  };
  res.json(result);
};
EOF

cat > package.json << 'EOF'
{
  "name": "production-function",
  "version": "1.0.0",
  "main": "index.js",
  "engines": {"node": "18"}
}
EOF

# Deploy Gen 2 function
gcloud functions deploy $FUNCTION_NAME \
    --gen2 \
    --runtime=nodejs18 \
    --region=us-central1 \
    --source=. \
    --entry-point=handler \
    --memory=512MB \
    --timeout=120s \
    --max-instances=50 \
    --min-instances=2 \
    --concurrency=80 \
    --cpu=2 \
    --service-account=$FUNCTION_NAME@$PROJECT_ID.iam.gserviceaccount.com

# Configure environment variables
gcloud functions deploy $FUNCTION_NAME \
    --update-env-vars="ENV=production,LOG_LEVEL=info"

# Test function
gcloud functions invoke $FUNCTION_NAME \
    --region=us-central1 \
    --data='{"test":"data"}'

echo "Production function deployed!"
```

### Exercise 2: Event-Driven Functions

```bash
#!/bin/bash
# Deploy event-triggered functions

PROJECT_ID="my-project-id"

# Create bucket for storage trigger
gsutil mb -l us-central1 gs://my-trigger-bucket

# Storage trigger function
mkdir -p functions/storage-trigger
cat > functions/storage-trigger/index.js << 'EOF'
exports.handler = (file, context) => {
  console.log('File:', file.bucket, file.name);
  console.log('Event:', context.eventId);
};
EOF

cat > functions/storage-trigger/package.json << 'EOF'
{"name":"storage-trigger","version":"1.0.0","main":"index.js","engines":{"node":"18"}}
EOF

gcloud functions deploy storage-trigger \
    --runtime=nodejs18 \
    --region=us-central1 \
    --source=functions/storage-trigger \
    --entry-point=handler \
    --trigger-resource=my-trigger-bucket \
    --trigger-event=google.storage.object.finalize

# Pub/Sub trigger function
mkdir -p functions/pubsub-trigger
cat > functions/pubsub-trigger/index.js << 'EOF'
exports.handler = (message, context) => {
  const data = Buffer.from(message.data, 'base64').toString();
  console.log('Message:', data);
};
EOF

gcloud functions deploy pubsub-trigger \
    --runtime=nodejs18 \
    --region=us-central1 \
    --source=functions/pubsub-trigger \
    --entry-point=handler \
    --trigger-topic=my-topic

# Test Pub/Sub trigger
gcloud pubsub topics publish my-topic --message='{"test":"data"}'

echo "Event-driven functions deployed!"
```

### Exercise 3: VPC Integration

```bash
#!/bin/bash
# Deploy function with VPC access

PROJECT_ID="my-project-id"

# Create VPC connector
gcloud compute networks create function-vpc --subnet-mode=custom

gcloud compute networks subnets create function-subnet \
    --network=function-vpc \
    --region=us-central1 \
    --range=10.10.0.0/24

gcloud functions vpc-connectors create function-connector \
    --region=us-central1 \
    --network=function-vpc \
    --min-instances=2 \
    --max-instances=10

# Deploy function with VPC connector
gcloud functions deploy vpc-function \
    --gen2 \
    --runtime=nodejs18 \
    --region=us-central1 \
    --source=. \
    --entry-point=handler \
    --vpc-connector=function-connector \
    --egress-settings=all

# Test VPC access to Cloud SQL
gcloud sql instances list --filter="labels.type=internal"

# Clean up VPC connector
# gcloud functions vpc-connectors delete function-connector --region=us-central1

echo "VPC integration configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Cold starts | Set min-instances |
| Timeout errors | Increase timeout |
| VPC access | Check connector |

### Validation

```bash
# Check function status
gcloud functions describe my-function --region=us-central1

# View logs
gcloud functions logs read my-function --region=us-central1

# Check invocations
gcloud functions get-metrics my-function --region=us-central1
```

## 🌐 COMPATIBILITY

### Integration

- Cloud SQL: Via VPC connector
- Cloud Storage: Event triggers
- Pub/Sub: Message triggers

## 🔗 CROSS-REFERENCES

### Related Labs

- VPC Networking
- Cloud Storage
- Pub/Sub

### Next Steps

- Set up monitoring
- Configure alerting
- Implement CI/CD

## ✅ EXAM TIPS

- Know Gen 2 features
- Practice VPC connector setup
- Understand trigger types
- Monitor function metrics
