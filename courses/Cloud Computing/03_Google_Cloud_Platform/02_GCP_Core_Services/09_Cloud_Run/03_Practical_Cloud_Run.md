---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Run
Purpose: Hands-on exercises for Cloud Run deployment and management
Difficulty: advanced
Prerequisites: 01_Basic_Cloud_Run.md, 02_Advanced_Cloud_Run.md
RelatedFiles: 01_Basic_Cloud_Run.md, 02_Advanced_Cloud_Run.md
UseCase: Production serverless deployments, VPC integration, scaling configuration
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Cloud Run is essential for deploying production serverless containers, implementing secure connectivity, and optimizing costs.

### Lab Goals

- Deploy serverless containers
- Configure VPC integration
- Set up advanced scaling

## 📖 WHAT

### Exercise Overview

1. **Container Deployment**: Basic to production
2. **VPC Integration**: Private connectivity
3. **Advanced Scaling**: Performance tuning

## 🔧 HOW

### Exercise 1: Production Container Deployment

```bash
#!/bin/bash
# Deploy production Cloud Run service

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Build and push container
gcloud builds submit --tag gcr.io/$PROJECT/my-app:latest .

# Deploy with production settings
gcloud run deploy my-service \
    --image=gcr.io/$PROJECT/my-app:latest \
    --region=us-central1 \
    --platform=managed \
    --allow-unauthenticated \
    --cpu=2 \
    --memory=1Gi \
    --min-instances=2 \
    --max-instances=100 \
    --concurrency=80 \
    --timeout=300s \
    --service-account=my-sa@$PROJECT_ID.iam.gserviceaccount.com

# Configure environment variables
gcloud run deploy my-service \
    --image=gcr.io/$PROJECT/my-app:latest \
    --region=us-central1 \
    --set-env-vars="ENV=production,LOG_LEVEL=info" \
    --secret-env-vars="API_KEY=secret:api-key:latest"

# Add labels
gcloud run deploy my-service \
    --region=us-central1 \
    --update-labels=team=backend,version=v1

# Test deployment
curl $(gcloud run services describe my-service --region=us-central1 --format="value(status.url)")

echo "Production service deployed!"
```

### Exercise 2: VPC Integration

```bash
#!/bin/bash
# Configure VPC integration for Cloud Run

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create VPC network
gcloud compute networks create run-vpc --subnet-mode=custom

gcloud compute networks subnets create run-subnet \
    --network=run-vpc \
    --region=us-central1 \
    --range=10.10.0.0/24

# Create VPC connector
gcloud run integrations connectors create run-connector \
    --location=us-central1 \
    --network=run-vpc \
    --subnet=run-subnet \
    --min-instances=2 \
    --max-instances=10

# Deploy service with VPC connector
gcloud run deploy private-service \
    --image=gcr.io/$PROJECT/my-app:latest \
    --region=us-central1 \
    --vpc-connector=run-connector \
    --vpc-egress=all-traffic \
    --no-allow-unauthenticated

# Alternative: Route only to internal resources
gcloud run deploy internal-service \
    --image=gcr.io/$PROJECT/my-app:latest \
    --region=us-central1 \
    --vpc-connector=run-connector \
    --vpc-egress=route-only-to-10.0.0.0/8

# Verify connector status
gcloud run integrations connectors describe run-connector \
    --location=us-central1

echo "VPC integration configured!"
```

### Exercise 3: Advanced Scaling Configuration

```bash
#!/bin/bash
# Configure advanced scaling options

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Deploy with CPU allocation (always allocated)
gcloud run deploy cpu-allocated-service \
    --image=gcr.io/$PROJECT/my-app:latest \
    --region=us-central1 \
    --cpu=4 \
    --cpu-allocation=THROUGHPUT_BASED \
    --min-instances=5

# Deploy with session affinity
gcloud run deploy sticky-service \
    --image=gcr.io/$PROJECT/my-app:latest \
    --region=us-central1 \
    --session-affinity

# Deploy with GPU
gcloud run deploy gpu-service \
    --image=gcr.io/$PROJECT/gpu-app:latest \
    --region=us-central1 \
    --cpu=8 \
    --memory=32Gi \
    --gpu=1 \
    --gpu-type=nvidia-tesla-t4

# Configure max instances for cost control
gcloud run deploy cost-controlled-service \
    --image=gcr.io/$PROJECT/my-app:latest \
    --region=us-central1 \
    --max-instances=10 \
    --min-instances=0

# Set startup timeout for slow initialization
gcloud run deploy slow-startup-service \
    --image=gcr.io/$PROJECT/heavy-app:latest \
    --region=us-central1 \
    --startup-timeout=120s

# Get scaling metrics
gcloud run services describe scaling-service \
    --region=us-central1 \
    --format="value(metadata.annotations.autoscaling.knative.dev/maxScale)"

echo "Advanced scaling configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Cold starts | Increase min instances |
| VPC access | Check connector |
| Timeouts | Adjust timeout |

### Validation

```bash
# Check service status
gcloud run services describe my-service --region=us-central1

# View logs
gcloud logs read "resource.type=cloud_run_revision"

# Check metrics
gcloud run services get-annotations my-service --region=us-central1
```

## 🌐 COMPATIBILITY

### Integration

- Cloud IAM: Access control
- Cloud Logging: Logging
- Cloud Monitoring: Metrics

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud Functions
- GKE
- VPC Networking

### Next Steps

- Set up custom domain
- Configure Cloud CDN
- Add Cloud Armor

## ✅ EXAM TIPS

- Practice deployment commands
- Know scaling configuration
- Understand VPC connector
- Monitor performance
