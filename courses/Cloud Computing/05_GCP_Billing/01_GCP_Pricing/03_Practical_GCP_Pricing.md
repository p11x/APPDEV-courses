---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: GCP Pricing
Purpose: Hands-on exercises for GCP pricing and cost management
Difficulty: advanced
Prerequisites: 01_Basic_GCP_Pricing.md, 02_Advanced_GCP_Pricing.md
RelatedFiles: 01_Basic_GCP_Pricing.md, 02_Advanced_GCP_Pricing.md
UseCase: Cost estimation, pricing optimization, budget management
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with GCP pricing is essential for estimating costs, optimizing spending, and implementing cost-effective architectures.

### Lab Goals

- Estimate costs
- Optimize with sustained use
- Use committed use discounts

## 📖 WHAT

### Exercise Overview

1. **Cost Analysis**: Usage and billing
2. **Optimization**: Sustained and committed use
3. **Preemptible**: Cost savings

## 🔧 HOW

### Exercise 1: Analyze Costs

```bash
#!/bin/bash
# Analyze GCP costs

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Link billing account
gcloud billing projects link $PROJECT_ID \
    --billing-account=XXXXXX-XXXXXX-XXXXXX

# Export billing data to BigQuery
gcloud billing budgets create cost-budget \
    --billing-account=XXXXXX-XXXXXX-XXXXXX \
    --display-name="Monthly Budget" \
    --amount=10000USD \
    --threshold-rules="threshold=0.8, spendBasis=CURRENT_SPEND"

# Create budget with alert
gcloud billing budgets create project-budget \
    --billing-account=XXXXXX-XXXXXX-XXXXXX \
    --display-name="Project Budget" \
    --amount=5000USD \
    --filter-projects=projects/$PROJECT_ID \
    --threshold-rules="threshold=0.5, spendBasis=CURRENT_SPEND" \
    --threshold-rules="threshold=0.8, spendBasis=FORECASTED_SPEND"

# Query billing data (after enabling export)
bq query --use_legacy_sql=false \
    "SELECT
        service.description,
        SUM(cost) as total_cost
     FROM \`my-project.billing_export.billing_data\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
     GROUP BY service.description
     ORDER BY total_cost DESC"

# Check current spend
bq query --use_legacy_sql=false \
    "SELECT
        SUM(cost) as current_spend
     FROM \`my-project.billing_export.billing_data\`
     WHERE usage_start_time >= DATE_TRUNC(CURRENT_DATE(), MONTH)"

echo "Cost analysis complete!"
```

### Exercise 2: Optimize with Sustained Use

```bash
#!/bin/bash
# Optimize sustained use

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Check current usage
gcloud compute instances list --format="table(name,zone,machineType,status)"

# View recommendations
gcloud beta recommender suggestions list \
    --location=us-central1 \
    --recommender=google.compute.sustainedUseRecommender

# Get current utilization
gcloud monitoring metrics list --filter="metric.type=compute.googleapis.com/instance/cpu/utilization"

# Create baseline instance (for sustained use)
gcloud compute instance-groups managed create baseline-mig \
    --template=baseline-template \
    --size=3 \
    --region=us-central1

# Add preemptible for peak
gcloud compute instance-groups managed create peak-mig \
    --template=preemptible-template \
    --size=10 \
    --region=us-central1

# Configure autoscaling based on utilization
gcloud compute instance-groups managed set-autoscaling peak-mig \
    --region=us-central1 \
    --mode=on \
    --min-num-replicas=2 \
    --max-num-replicas=20 \
    --cpu-utilization-threshold=70

echo "Sustained use optimization complete!"
```

### Exercise 3: Implement Committed Use

```bash
#!/bin/bash
# Implement committed use discounts

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# View available commitments
gcloud compute resource-commitments list-available \
    --region=us-central1

# Purchase committed use (Normal - 1 year)
gcloud compute commitments create annual-commitment \
    --region=us-central1 \
    --cores=16 \
    --memory=64GB \
    --plan=1-year

# Purchase committed use (3 year)
gcloud compute commitments create three-year-commitment \
    --region=us-central1 \
    --cores=32 \
    --memory=128GB \
    --plan=3-year

# View commitments
gcloud compute commitments list

# Describe specific commitment
gcloud compute commitments describe annual-commitment \
    --region=us-central1

# Update commitment
gcloud compute commitments update three-year-commitment \
    --region=us-central1 \
    --add-reservations="count=4, machineType=n2-standard-8"

# Check CUD recommendations
gcloud beta recommender insights list \
    --location=us-central1 \
    --recommender=google.commitments.CudRecommender

echo "Committed use configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| No savings | Check usage patterns |
| CUD not applied | Verify commitment |
| Budget exceeded | Set alerts |

### Validation

```bash
# Check billing status
gcloud billing projects describe $PROJECT_ID

# List budgets
gcloud billing budgets list --billing-account=XXXXXX

# Check commitments
gcloud compute commitments list
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Billing
- Cloud Monitoring
- Recommender

## 🔗 CROSS-REFERENCES

### Related Labs

- Budget Alerts
- Cost Optimization
- Sustained Use

### Next Steps

- Set up billing alerts
- Configure recommender
- Implement labels

## ✅ EXAM TIPS

- Practice cost analysis
- Understand commitment types
- Know sustained use discount
- Monitor with budgets
