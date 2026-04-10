---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: GCP Pricing
Purpose: Advanced understanding of GCP pricing models and cost optimization
Difficulty: intermediate
Prerequisites: 01_Basic_GCP_Pricing.md
RelatedFiles: 01_Basic_GCP_Pricing.md, 03_Practical_GCP_Pricing.md
UseCase: Cost estimation, pricing optimization, budget management
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced pricing knowledge enables accurate cost estimation, selecting optimal pricing models, and implementing cost optimization strategies for GCP deployments.

### Why Advanced Pricing

- **Sustained Use**: Automatic discounts
- **Committed Use**: Predictable savings
- **CUDs**: Custom committed use discounts
- **Preemptible**: Maximum savings

## 📖 WHAT

### Sustained Use Discounts

- Automatic for >25% usage in month
- Up to 57% discount on sustained use
- Applies to CPU and memory
- No commitment required

### Committed Use Discounts

| Type | Commitment | Discount | Use Case |
|------|------------|----------|----------|
| Normal | 1-3 years | 57% | Predictable |
| CUD | Specific resources | Up to 70% | Custom |

### Cost Optimization Strategies

- Use sustained use for variable workloads
- Committed use for baseline
- Preemptible for fault-tolerant
- Use committed use for GPUs

## 🔧 HOW

### Example 1: Sustained Use Optimization

```bash
# Check current usage
gcloud monitoring policies list --filter="metric.type=compute.googleapis.com/instance/cpu/utilization"

# Analyze sustained use
# Use Recommender API
gcloud beta recommender insights list \
    --location=global \
    --recommender=google.compute.sustainedUseRecommender

# View sustained use discount
gcloud compute instances list --format="table(name,zone,machineType,status,scheduling)"
```

### Example 2: Committed Use Purchases

```bash
# Purchase committed use
gcloud compute resource- commitments create \
    --cores=8 \
    --memory=32GB \
    --region=us-central1 \
    --plan=3-year

# List available commitments
gcloud compute resource-commitments list-available \
    --region=us-central1

# Check committed use status
gcloud compute commitments list

# Update commitment
gcloud compute commitments update commitment-name \
    --region=us-central1 \
    --add-reservations="count=1,instanceTemplate=template-name"
```

### Example 3: Preemptible Optimization

```bash
# Create preemptible instance template
gcloud compute instance-templates create preemptible-template \
    --machine-type=n1-standard-4 \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --preemptible

# Create managed instance group
gcloud compute instance-groups managed create batch-mig \
    --template=preemptible-template \
    --size=10 \
    --region=us-central1

# Configure autohealing
gcloud compute instance-groups managed set-autoscaling batch-mig \
    --region=us-central1 \
    --mode=on \
    --max-num-replicas=20
```

## ⚠️ COMMON ISSUES

### Troubleshooting Pricing Issues

| Issue | Solution |
|-------|----------|
| Unexpected costs | Review usage logs |
| No sustained use | Check usage patterns |
| CUD not applied | Verify commitment |

### Cost Analysis

- Use Billing Export to BigQuery
- Set up cost alerts
- Review Recommender

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP | AWS | Azure |
|---------|-----|-----|-------|
| Sustained Use | Yes | No | No |
| Committed Use | Yes | Reserved | Reserved |
| Spot/Preemptible | Yes | Spot | Spot |
| Per-second | Yes | No | Limited |

## 🔗 CROSS-REFERENCES

### Related Topics

- Budget Alerts
- Cost Optimization
- Committed Use

### Study Resources

- GCP Pricing documentation
- Cost Calculator

## ✅ EXAM TIPS

- Sustained use = 25%+ usage = 57% discount
- CUDs = up to 70% off
- Preemptible = 60-91% discount
- Network egress is primary cost
