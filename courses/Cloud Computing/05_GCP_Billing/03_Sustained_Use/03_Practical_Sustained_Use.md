---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: Sustained Use
Purpose: Hands-on exercises for Sustained Use optimization
Difficulty: advanced
Prerequisites: 01_Basic_Sustained_Use.md, 02_Advanced_Sustained_Use.md
RelatedFiles: 01_Basic_Sustained_Use.md, 02_Advanced_Sustained_Use.md
UseCase: Maximizing Sustained Use savings, workload optimization
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Sustained Use is essential for maximizing automatic discounts and optimizing compute costs on GCP.

### Lab Goals

- Analyze usage patterns
- Optimize sustained use
- Verify discounts

## 📖 WHAT

### Exercise Overview

1. **Usage Analysis**: Understand usage patterns
2. **Optimization**: Baseline + burst pattern
3. **Verification**: Monitor discount application

## 🔧 HOW

### Exercise 1: Analyze Sustained Use

```bash
#!/bin/bash
# Analyze Sustained Use patterns

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Query daily usage
bq query --use_legacy_sql=false \
    "SELECT
        DATE(usage_start_time) as date,
        service.description as service,
        ROUND(SUM(usage.amount), 2) as usage_hours,
        ROUND(SUM(cost), 2) as cost
     FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
       AND service.description = 'Compute Engine'
     GROUP BY DATE(usage_start_time), service.description
     ORDER BY date"

# Calculate sustained use potential
bq query --use_legacy_sql=false \
    "WITH daily_usage AS (
        SELECT
            DATE(usage_start_time) as date,
            SUM(usage.amount) as total_hours
        FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`
        WHERE service.description = 'Compute Engine'
          AND usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
        GROUP BY DATE(usage_start_time)
    )
    SELECT
        date,
        total_hours,
        CASE
            WHEN total_hours >= 600 THEN 'Tier 4 (100%) - 57% discount'
            WHEN total_hours >= 450 THEN 'Tier 3 (75%) - 47% discount'
            WHEN total_hours >= 300 THEN 'Tier 2 (50%) - 37% discount'
            WHEN total_hours >= 150 THEN 'Tier 1 (25%) - 20% discount'
            ELSE 'No Sustained Use discount'
        END as sustained_use_tier
    FROM daily_usage
    ORDER BY date"

# Get actual sustained use credits
bq query --use_legacy_sql=false \
    "SELECT
        credit_type,
        SUM(amount) as total_credits
     FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`,
       UNNEST(credits) as credit
     WHERE credit.credit_type = 'SUSTAINED_USE_DISCOUNT'
       AND usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
     GROUP BY credit_type"

echo "Usage analysis complete!"
```

### Exercise 2: Optimize Workloads

```bash
#!/bin/bash
# Optimize workloads for sustained use

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create baseline template (24/7 for sustained use)
gcloud compute instance-templates create baseline-template \
    --machine-type=n1-standard-4 \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --metadata=startup-script='#!/bin/bash
apt-get update && apt-get install -y nginx'

# Create baseline MIG (sustained use - always running)
gcloud compute instance-groups managed create baseline-mig \
    --template=baseline-template \
    --size=3 \
    --region=us-central1

# Create preemptible template (burst - lower cost)
gcloud compute instance-templates create burst-template \
    --machine-type=n1-standard-4 \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --preemptible

# Create burst MIG
gcloud compute instance-groups managed create burst-mig \
    --template=burst-template \
    --size=5 \
    --region=us-central1

# Configure autoscaling for burst
gcloud compute instance-groups managed set-autoscaling burst-mig \
    --region=us-central1 \
    --mode=on \
    --min-num-replicas=0 \
    --max-num-replicas=20 \
    --cpu-utilization-threshold=70

# Total capacity: baseline (100%) + burst (variable)
echo "Baseline MIG: 3 instances (24/7) - Sustained Use discount"
echo "Burst MIG: 0-20 instances (autoscaled) - Preemptible discount"
```

### Exercise 3: Verify Discounts

```bash
#!/bin/bash
# Verify Sustained Use discount application

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Get discount breakdown
bq query --use_legacy_sql=false \
    "SELECT
        project.name as project_name,
        ROUND(SUM(cost), 2) as total_cost,
        ROUND(SUM((
            SELECT SUM(amount) FROM UNNEST(credits) WHERE credit_type = 'SUSTAINED_USE_DISCOUNT'
        )), 2) as sustained_discount,
        ROUND(SUM((
            SELECT SUM(amount) FROM UNNEST(credits) WHERE credit_type = 'COMMITTED_USE_DISCOUNT'
        )), 2) as committed_discount,
        ROUND(SUM((
            SELECT SUM(amount) FROM UNNEST(credits) WHERE credit_type = 'PREEMPTIBLE_DISCOUNT'
        )), 2) as preemptible_discount,
        ROUND(SUM((
            SELECT SUM(amount) FROM UNNEST(credits)
        )), 2) as total_discounts,
        ROUND(SUM((
            SELECT SUM(amount) FROM UNNEST(credits)
        )) / SUM(cost) * 100, 1) as savings_percentage
     FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`,
       UNNEST(credits) as credit
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
     GROUP BY project.name"

# Check discount by day
bq query --use_legacy_sql=false \
    "SELECT
        DATE(usage_start_time) as date,
        ROUND(SUM(cost), 2) as daily_cost,
        ROUND(SUM((
            SELECT SUM(amount) FROM UNNEST(credits) WHERE credit_type = 'SUSTAINED_USE_DISCOUNT'
        )), 2) as sustained_discount,
        ROUND(SUM(usage.amount), 2) as usage_hours
     FROM \`$PROJECT_ID.billing_export.gcp_billing_export_v1_XXXXXX\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 7 DAY)
     GROUP BY DATE(usage_start_time)
     ORDER BY date"

# Calculate potential additional savings
echo "Comparing actual vs potential sustained use:"
echo "- Baseline instances (24/7): Full sustained use discount"
echo "- Autoscaled instances: Partial sustained use + preemptible"

echo "Discount verification complete!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| No discount | Increase usage above 25% |
| Low discount | Check usage patterns |
| Not applied | Verify Compute Engine usage |

### Validation

```bash
# Check credits in billing export
bq query --use_legacy_sql=false \
    "SELECT * FROM \`project.billing_export.credits\` LIMIT 10"

# Check instance uptime
gcloud compute instances list --format="table(name,status)"
```

## 🌐 COMPATIBILITY

### Integration

- Committed Use
- Preemptible VMs
- Budget Alerts

## 🔗 CROSS-REFERENCES

### Related Labs

- Committed Use
- Preemptible
- Budget Alerts

### Next Steps

- Combine with CUDs
- Monitor discount tiers
- Set up alerts

## ✅ EXAM TIPS

- Monitor usage above 25% threshold
- Maintain steady usage for maximum discount
- Combine with preemptible for bursts
- Review discount application in billing
