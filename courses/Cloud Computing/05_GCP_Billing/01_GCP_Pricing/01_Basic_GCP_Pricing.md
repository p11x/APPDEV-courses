---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: GCP Pricing
Purpose: Understanding GCP pricing models and cost components
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_GCP_Pricing.md, 03_Practical_GCP_Pricing.md
UseCase: Understanding GCP costs, pricing models, and billing
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

GCP pricing varies by service and usage. Understanding pricing helps estimate costs and optimize spending on GCP.

## 📖 WHAT

### Pricing Models

| Model | Description | Services |
|-------|-------------|----------|
| Pay-as-you-go | Per-second billing | Most services |
| Committed | 1-3 year commitment | Compute, memory |
| Sustained use | Automatic discount | Compute Engine |
| Preemptible | 60-91% discount | Compute Engine |

### Cost Components

- **Compute**: vCPU, memory, GPU
- **Storage**: GB stored, operations
- **Network**: Egress, ingress
- **API Calls**: Per 1M calls

## 🔧 HOW

### Example: Estimate Costs

```bash
# Use pricing calculator
# https://cloud.google.com/products/calculator

# View current billing
gcloud billing projects link my-project --billing-account=XXXXX

# List billing accounts
gcloud billing accounts list

# Get cost breakdown
# Use Cloud Billing API
```

## ✅ EXAM TIPS

- Per-second billing for most services
- Committed use for predictable usage
- Sustained use automatic discount
- Network egress is primary cost
