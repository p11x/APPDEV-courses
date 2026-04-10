---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: Committed Use
Purpose: Hands-on exercises for Committed Use implementation
Difficulty: advanced
Prerequisites: 01_Basic_Committed_Use.md, 02_Advanced_Committed_Use.md
RelatedFiles: 01_Basic_Committed_Use.md, 02_Advanced_Committed_Use.md
UseCase: Enterprise committed use implementation, cost optimization
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Committed Use is essential for implementing cost-effective GCP deployments with predictable workloads.

### Lab Goals

- Analyze workload patterns
- Purchase commitments
- Monitor savings

## 📖 WHAT

### Exercise Overview

1. **Workload Analysis**: Baseline usage
2. **Commitment Purchase**: CUD implementation
3. **Verification**: Savings monitoring

## 🔧 HOW

### Exercise 1: Analyze Workloads for Commitment

```bash
#!/bin/bash
# Analyze workloads to determine commitment needs

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Query compute usage
bq query --use_legacy_sql=false \
    "SELECT
        service.description as service,
        usage.unit as unit,
        ROUND(SUM(usage.amount), 2) as total_usage,
        ROUND(AVG(usage.amount), 2) as avg_daily_usage,
        ROUND(MAX(usage.amount), 2) as peak_usage
     FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
       AND service.description = 'Compute Engine'
     GROUP BY service.description, usage.unit
     ORDER BY total_usage DESC"

# Analyze by machine type
bq query --use_legacy_sql=false \
    "SELECT
        resource.location.location,
        usage.unit,
        ROUND(SUM(cost), 2) as monthly_cost
     FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
     GROUP BY resource.location.location, usage.unit
     ORDER BY monthly_cost DESC"

# Identify baseline vs burst
bq query --use_legacy_sql=false \
    "SELECT
        DATE(usage_start_time) as date,
        COUNT(*) as hours_used,
        ROUND(AVG(usage.amount), 2) as avg_hourly_usage
     FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
     GROUP BY DATE(usage_start_time)
     ORDER BY date"

echo "Workload analysis complete!"
```

### Exercise 2: Purchase Committed Use

```bash
#!/bin/bash
# Purchase Committed Use discounts

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# View available commitments
gcloud compute resource-commitments list-available \
    --region=us-central1

# Purchase 1-year CPU commitment
gcloud compute commitments create cpu-commit-1yr \
    --region=us-central1 \
    --cores=16 \
    --memory=64GB \
    --plan=1-year

# Purchase 3-year memory CUD
gcloud compute commitments create mem-commit-3yr \
    --region=us-central1 \
    --memory=128GB \
    --plan=3-year

# For GPU workloads
gcloud compute commitments create gpu-commit \
    --region=us-central1 \
    --accelerator-type=nvidia-tesla-v100 \
    --accelerator-count=4 \
    --plan=3-year

# List all commitments
gcloud compute commitments list

# Verify commitment details
gcloud compute commitments describe cpu-commit-1yr \
    --region=us-central1

echo "Committed Use purchased!"
```

### Exercise 3: Monitor and Verify Savings

```bash
#!/bin/bash
# Monitor Committed Use savings

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# View commitments
gcloud compute commitments list

# Get cost breakdown by resource
bq query --use_legacy_sql=false \
    "SELECT
        project.id as project_id,
        project.name as project_name,
        ROUND(SUM(cost), 2) as total_cost,
        ROUND(SUM(ifnull((SELECT SUM(amount) from unnest(credits) where credit_type = 'COMMITTED_USE_DISCOUNT'), 0)), 2) as cud_savings,
        ROUND(SUM(ifnull((SELECT SUM(amount) from unnest(credits) where credit_type = 'SUSTAINED_USE_DISCOUNT'), 0)), 2) as sustained_savings
     FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
     GROUP BY project.id, project.name"

# Calculate savings percentage
bq query --use_legacy_sql=false \
    "SELECT
        ROUND(SUM(cost), 2) as total_spend,
        ROUND(SUM(ifnull((SELECT SUM(amount) from unnest(credits) where credit_type = 'COMMITTED_USE_DISCOUNT'), 0)), 2) as cud_savings,
        ROUND(SUM(ifnull((SELECT SUM(amount) from unnest(credits) where credit_type = 'COMMITTED_USE_DISCOUNT'), 0)) / SUM(cost) * 100, 1) as savings_percentage
     FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)"

# Check commitment utilization
gcloud compute commitments describe my-commitment \
    --region=us-central1 \
    --format="table(name, state, reservation, utilization)"

echo "Savings monitoring complete!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Not getting savings | Verify commitment matches usage |
| High costs | Right-size commitments |
| Quota | Request increase |

### Validation

```bash
# List commitments
gcloud compute commitments list

# Check savings
bq query --use_legacy_sql=false \
    "SELECT * FROM \`project.billing_export.credits\`"
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Billing
- Sustained Use
- Preemptible

## 🔗 CROSS-REFERENCES

### Related Labs

- Sustained Use
- Preemptible
- Budget Alerts

### Next Steps

- Set up billing alerts
- Configure cost exports
- Implement labels

## ✅ EXAM TIPS

- Analyze baseline usage first
- Match commitment to baseline
- 3-year for maximum savings
- Monitor utilization
