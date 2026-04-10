---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: Committed Use
Purpose: Understanding GCP Committed Use discounts
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Pricing.md
RelatedFiles: 02_Advanced_Committed_Use.md, 03_Practical_Committed_Use.md
UseCase: Predictable workloads requiring committed use discounts
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Committed Use discounts provide significant savings for predictable workloads. Understanding committed use helps reduce GCP costs for baseline capacity.

## 📖 WHAT

### Commitment Types

| Type | Duration | Typical Discount |
|------|----------|------------------|
| Normal | 1-3 years | Up to 57% |
| CUD | Custom | Up to 70% |

### Applicable Resources

- Compute Engine vCPUs
- Memory (RAM)
- GPUs
- Local SSDs

## 🔧 HOW

### Example: Purchase Commitment

```bash
# View available commitments
gcloud compute resource-commitments list-available

# Purchase 1-year commitment
gcloud compute commitments create my-commitment \
    --region=us-central1 \
    --cores=8 \
    --memory=32GB \
    --plan=1-year

# List commitments
gcloud compute commitments list
```

## ✅ EXAM TIPS

- Up to 57% discount
- 1 or 3 year commitments
- CUDs for specific resources
- Non-cancellable
