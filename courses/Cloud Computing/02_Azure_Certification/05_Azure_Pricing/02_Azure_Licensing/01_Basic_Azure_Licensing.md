---
Category: Azure Certification
Subcategory: Azure Pricing
Concept: Azure Licensing
Purpose: Understand Azure licensing models and programs
Difficulty: beginner
Prerequisites: None
RelatedFiles: 02_Advanced_Azure_Licensing.md, 03_Practical_Azure_Licensing.md
UseCase: Select appropriate licensing, understand pricing
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Understanding Azure licensing models is essential for cost optimization and compliance. Different programs offer different pricing, and choosing the right licensing can significantly reduce costs.

## 📖 WHAT

### Licensing Programs

| Program | Model | Best For |
|---------|-------|----------|
| Pay-as-you-go | Hourly | Variable/low usage |
| Reserved Instances | 1-3 year | Predictable steady |
| Azure Savings Plan | Flexible commitment | Variable compute |
| CSP | Partner billing | Small business |
| Enterprise Agreement | Negotiated | Large organizations |

### Subscription Types

| Type | Use | Limits |
|------|-----|--------|
| Free | Trial | 30 days, $200 credit |
| Pay-as-you-go | Ongoing | Per subscription |
| Enterprise | Large org | Customnegotiations |
| Visual Studio | Dev/test | MSDN credits |

### Core Licensing Concepts

- **Per-hour billing**: Based on usage
- **Per-minute billing**: More granular
- **License mobility**: Bring your own
- **Multi-factor**: Security included

## 🔧 HOW

### Example 1: Check Subscription Type

```bash
# Get subscription details
az account show \
    --query '[name, state, account]'

# List all subscriptions
az account list \
    --output table
```

### Example 2: View Pricing

```bash
# Get VM pricing by size
az vm sku list \
    --resource-type virtualMachines \
    --location eastus

# Use pricing calculator
# https://azure.microsoft.com/pricing/calculator/
```

### Example 3: License Mobility

```bash
# License mobility for SQL Server
# Available with:
# - Software Assurance
# - Azure Hybrid Benefit
# No additional licensing needed
```

## ⚠️ COMMON ISSUES

- **Billing currency**: Per region
- **Promo expiry**: Track trial limits
- **License compliance**: Audit for compliance

## 🏃 PERFORMANCE

- No performance impact

## 🌐 COMPATIBILITY

- All services use same licensing
- Some exceptions for CSP

## 🔗 CROSS-REFERENCES

- **Hybrid Benefit**: License savings
- **Reservations**: Commitment savings

## ✅ EXAM TIPS

- Use Hybrid Benefit where possible
- Track reserved instances
- Understand subscription limits