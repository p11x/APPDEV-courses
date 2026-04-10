---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: Committed Use
Purpose: Advanced understanding of Committed Use discounts
Difficulty: intermediate
Prerequisites: 01_Basic_Committed_Use.md
RelatedFiles: 01_Basic_Committed_Use.md, 03_Practical_Committed_Use.md
UseCase: Enterprise committed use, resource optimization
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced committed use knowledge enables maximizing savings for enterprise workloads with custom commitments and efficient resource allocation.

### Why Advanced Committed Use

- **Resource-specific CUDs**: Custom discounts
- **Region-specific**: Regional commitments
- **GPU commitments**: AI/ML workloads
- **Memory commitments**: Memory-intensive apps

## 📖 WHAT

### CUD Comparison

| Type | Minimum | Discount | Flexibility |
|------|---------|----------|-------------|
| Normal | 1 core | ~57% | Less |
| CUD-Memory | 2 GB | ~60% | High |
| CUD-Resource | Custom | Up to 70% | Medium |

### Commitment Scenarios

**Baseline Workloads**:
- Core application servers
- Database backends
- API services

**AI/ML Workloads**:
- GPU commitments
- Training jobs
- Inference endpoints

## 🔧 HOW

### Example 1: Resource-Specific CUD

```bash
# Purchase memory-specific CUD
gcloud compute commitments create mem-commitment \
    --region=us-central1 \
    --memory=64GB \
    --plan=3-year

# Purchase GPU CUD
gcloud compute commitments create gpu-commitment \
    --region=us-central1 \
    --accelerator-type=nvidia-tesla-v100 \
    --accelerator-count=2 \
    --plan=3-year

# List available resources
gcloud compute machine-types list --region=us-central1

# Describe specific machine type
gcloud compute machine-types describe n2-standard-16 \
    --zone=us-central1-a
```

### Example 2: Regional Commitment

```bash
# Create regional commitment
gcloud compute commitments create regional-commit \
    --region=us-central1 \
    --cores=16 \
    --memory=64GB \
    --plan=1-year

# List regional commitments
gcloud compute commitments list --region=us-central1

# Verify commitment in specific zone
gcloud compute commitments describe regional-commit \
    --region=us-central1
```

### Example 3: Optimization Strategies

```bash
# Analyze workload patterns
bq query --use_legacy_sql=false \
    "SELECT
        DATE(usage_start_time) as date,
        AVG(usage_amount) as avg_usage,
        MAX(usage_amount) as max_usage
     FROM \`project.billing_export.compute_usage\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
     GROUP BY DATE(usage_start_time)
     ORDER BY date"

# Get CUD recommendations
gcloud recommender insights list \
    --location=us-central1 \
    --recommender=google.commitments.CudRecommender

# Calculate potential savings
# Compare pay-as-you-go vs committed
```

## ⚠️ COMMON ISSUES

### Troubleshooting CUD Issues

| Issue | Solution |
|-------|----------|
| CUD not applying | Check machine type |
| Commitment unused | Right-size commitments |
| Quota issues | Request quota increase |

### Best Practices

- Match commitment to baseline usage
- Use 3-year for stable workloads
- Combine with sustained use

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP | AWS | Azure |
|---------|-----|-----|-------|
| Committed | Yes | Reserved | Reserved |
| CUD | Yes | No | No |
| GPU Commitment | Yes | Yes | Yes |

## 🔗 CROSS-REFERENCES

### Related Topics

- Sustained Use
- Preemptible VMs
- Budget Alerts

### Study Resources

- GCP Committed Use documentation
- Cost optimization best practices

## ✅ EXAM TIPS

- CUDs = custom committed use
- 3-year = maximum discount
- GPU commitments for ML
- Memory CUDs for large workloads
