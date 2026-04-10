---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Run
Purpose: Advanced understanding of Cloud Run features and configurations
Difficulty: intermediate
Prerequisites: 01_Basic_Cloud_Run.md
RelatedFiles: 01_Basic_Cloud_Run.md, 03_Practical_Cloud_Run.md
UseCase: Production serverless containers, advanced scaling, VPC integration
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced Cloud Run knowledge enables building production serverless applications with VPC integration, custom domains, and advanced scaling configurations.

### Why Advanced Cloud Run

- **VPC Integration**: Secure connectivity
- **Custom Domains**: SSL certificate management
- **Cloud Run for Anthos**: Hybrid/multi-cloud
- **GPU Support**: ML inference
- **Session Affinity**: Stickiness

## 📖 WHAT

### Scaling Configuration

| Setting | Description | Options |
|---------|-------------|---------|
| Min Instances | Always running | 0-1000 |
| Max Instances | Peak capacity | 0-1000 |
| CPU Allocation | Always vs requested | CPU/throughput |
| Concurrency | Requests per instance | 1-1000 |

### Advanced Features

**CPU Allocation**:
- Default: CPU throttled when idle
- Allocated: CPU always available

**Session Affinity**:
- Route same user to same instance
- Cookie-based

## 🔧 HOW

### Example 1: VPC Connector Configuration

```bash
# Create VPC connector
gcloud compute networks create run-vpc --subnet-mode=custom

gcloud compute networks subnets create run-subnet \
    --network=run-vpc \
    --region=us-central1 \
    --range=10.10.0.0/24

gcloud run integrations connectors create run-connector \
    --location=us-central1 \
    --network=run-vpc \
    --subnet=run-subnet \
    --min-instances=2 \
    --max-instances=10

# Deploy with VPC connector
gcloud run deploy my-service \
    --image=gcr.io/my-project/my-image \
    --vpc-connector=run-connector \
    --vpc-egress=all-traffic \
    --region=us-central1
```

### Example 2: Custom Domain and SSL

```bash
# Deploy with custom domain
gcloud run deploy my-service \
    --image=gcr.io/my-project/my-image \
    --domain-map=my-domain=example.com \
    --region=us-central1

# Map custom domain
gcloud run domain-mappings create \
    --service=my-service \
    --domain=api.example.com \
    --region=us-central1

# Provision managed SSL
gcloud run services update my-service \
    --region=us-central1 \
    --image=gcr.io/my-project/my-image

# Verify domain mapping
gcloud run domain-mappings list --region=us-central1
```

### Example 3: Advanced Scaling

```bash
# Deploy with scaling configuration
gcloud run deploy my-service \
    --image=gcr.io/my-project/my-image \
    --region=us-central1 \
    --min-instances=2 \
    --max-instances=100 \
    --cpu=4 \
    --memory=2Gi \
    --concurrency=80 \
    --session-affinity

# Configure cold start
gcloud run deploy optimized-service \
    --image=gcr.io/my-project/my-image \
    --region=us-central1 \
    --min-instances=5 \
    --cpu=allocation \
    --startup-timeout=60s

# Configure timeout
gcloud run deploy long-running \
    --image=gcr.io/my-project/my-image \
    --region=us-central1 \
    --timeout=3600s
```

## ⚠️ COMMON ISSUES

### Troubleshooting Cloud Run Issues

| Issue | Solution |
|-------|----------|
| Cold starts | Increase min instances |
| VPC access | Configure connector |
| Timeouts | Adjust timeout settings |
| Cold start | Use CPU allocation |

### Best Practices

- Set min instances for latency
- Use VPC for private access
- Enable session affinity when needed

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP Cloud Run | AWS ECS Fargate | Azure Container Apps |
|---------|---------------|-----------------|---------------------|
| Serverless | Yes | Yes | Yes |
| Auto-scale to zero | Yes | No | Yes |
| Custom domains | Yes | Yes | Yes |
| VPC integration | Yes | Yes | Yes |

## 🔗 CROSS-REFERENCES

### Related Topics

- Cloud Functions (event-driven)
- GKE (container orchestration)
- Cloud Load Balancing

### Study Resources

- Cloud Run documentation
- Best practices for Cloud Run

## ✅ EXAM TIPS

- Auto-scales to zero by default
- VPC connector for private access
- Min instances prevent cold starts
- Session affinity for user stickiness
