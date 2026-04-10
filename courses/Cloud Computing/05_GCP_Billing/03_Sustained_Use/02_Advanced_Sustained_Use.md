---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: Sustained Use
Purpose: Advanced understanding of Sustained Use discounts optimization
Difficulty: intermediate
Prerequisites: 01_Basic_Sustained_Use.md
RelatedFiles: 01_Basic_Sustained_Use.md, 03_Practical_Sustained_Use.md
UseCase: Maximizing Sustained Use savings, combining with other discounts
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced Sustained Use knowledge enables maximizing automatic discounts and combining with other savings mechanisms for optimal cost efficiency.

### Why Advanced Sustained Use

- **Tier Optimization**: Higher usage = higher discount
- **Combine with CUDs**: Layer discounts
- **Preemptible Integration**: Hybrid approaches
- **Workload Scheduling**: Steady-state optimization

## 📖 WHAT

### Discount Tier Details

| Monthly Usage | Effective Discount |
|---------------|---------------------|
| 25% | ~20% |
| 50% | ~37% |
| 75% | ~47% |
| 100% | ~57% |

### Combining Discounts

**Sustained Use + Committed Use**:
- Committed Use applies first
- Sustained Use on remaining

**Sustained Use + Preemptible**:
- Baseline uses sustained
- Burst uses preemptible

## 🔧 HOW

### Example 1: Tier Analysis

```bash
# Analyze current sustained use
gcloud monitoring metrics describe compute.googleapis.com/instance/cpu/utilization

# Get sustained use recommendations
gcloud beta recommender insights list \
    --location=global \
    --recommender=google.compute.sustainedUseRecommender

# Query sustained use credits
bq query --use_legacy_sql=false \
    "SELECT
        credit_type,
        SUM(amount) as total_credits
     FROM \`project.billing_export.credits\`
     WHERE credit_type LIKE '%SUSTAINED%'
     GROUP BY credit_type"
```

### Example 2: Workload Optimization

```bash
# Create baseline instances (24/7)
gcloud compute instance-groups managed create baseline-mig \
    --template=baseline-template \
    --size=3 \
    --region=us-central1

# Create burst instances (preemptible)
gcloud compute instance-groups managed create burst-mig \
    --template=preemptible-template \
    --size=10 \
    --region=us-central1

# Configure autoscaling
gcloud compute instance-groups managed set-autoscaling burst-mig \
    --region=us-central1 \
    --mode=on \
    --min-num-replicas=2 \
    --max-num-replicas=20 \
    --cpu-utilization-threshold=80
```

### Example 3: Cost Analysis

```bash
# Calculate potential savings with sustained use
bq query --use_legacy_sql=false \
    "SELECT
        DATE(usage_start_time) as date,
        SUM(cost) as daily_cost,
        SUM(usage.amount) as daily_usage_hours,
        CASE 
            WHEN SUM(usage.amount) >= 600 THEN ROUND(SUM(cost) * 0.57, 2)
            WHEN SUM(usage.amount) >= 450 THEN ROUND(SUM(cost) * 0.47, 2)
            WHEN SUM(usage.amount) >= 300 THEN ROUND(SUM(cost) * 0.37, 2)
            ELSE ROUND(SUM(cost) * 0.20, 2)
        END as potential_sustained_discount
     FROM \`project.billing_export.compute_usage\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
     GROUP BY DATE(usage_start_time)"
```

## ⚠️ COMMON ISSUES

### Troubleshooting Sustained Use

| Issue | Solution |
|-------|----------|
| No discount | Increase usage above 25% |
| Low discount | Optimize usage patterns |
| Not combining | Check CUD overlap |

### Best Practices

- Maintain consistent usage
- Use for baseline workloads
- Add preemptible for bursts
- Monitor tier levels

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP | AWS | Azure |
|---------|-----|-----|-------|
| Sustained Use | Yes | No | No |
| Auto-discount | Yes | No | No |
| Tiered discount | Yes | No | Limited |

## 🔗 CROSS-REFERENCES

### Related Topics

- Committed Use
- Preemptible VMs
- Budget Alerts

### Study Resources

- GCP Sustained Use documentation
- Cost optimization guides

## ✅ EXAM TIPS

- 25%+ usage threshold
- Up to 57% discount
- Automatic application
- Combine with CUDs
