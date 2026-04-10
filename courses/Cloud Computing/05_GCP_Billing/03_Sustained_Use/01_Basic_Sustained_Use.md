---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: Sustained Use
Purpose: Understanding GCP Sustained Use discounts
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Pricing.md
RelatedFiles: 02_Advanced_Sustained_Use.md, 03_Practical_Sustained_Use.md
UseCase: Automatic cost savings for sustained compute usage
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Sustained Use provides automatic discounts for consistent compute usage. Understanding this helps leverage automatic savings without commitment.

## 📖 WHAT

### How Sustained Use Works

- **Threshold**: >25% of month usage
- **Discount**: Up to 57% automatically
- **No Commitment**: Pay-as-you-go rates
- **Automatic**: Applied automatically

### Discount Tiers

| Usage | Discount |
|-------|----------|
| 25-50% | 20-37% |
| 50-75% | 37-47% |
| 75-100% | 47-57% |

## 🔧 HOW

### Example: Check Sustained Use

```bash
# Check Compute Engine usage
gcloud compute instances list

# View sustained use in billing
# Billing dashboard shows sustained use credits

# Analyze usage patterns
bq query --use_legacy_sql=false \
    "SELECT usage.unit, SUM(usage.amount) as hours 
     FROM billing export 
     WHERE service='Compute Engine'"
```

## ✅ EXAM TIPS

- Automatic discount for 25%+ usage
- Up to 57% discount
- No commitment required
- More usage = higher discount
