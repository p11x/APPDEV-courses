---
Category: Azure Certification
Subcategory: Azure Pricing
Concept: Azure Cost Management
Purpose: Monitor, analyze, and optimize Azure spending
Difficulty: beginner
Prerequisites: None
RelatedFiles: 02_Advanced_Azure_Cost_Management.md, 03_Practical_Azure_Cost_Management.md
UseCase: Track spending, budget alerts, cost optimization
CertificationExam: AZ-103 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Azure Cost Management provides transparency into cloud spending, enabling organizations to track costs, set budgets, and identify optimization opportunities to reduce waste.

## 📖 WHAT

### Key Features

| Feature | Purpose | Benefit |
|---------|---------|---------|
| Cost Analysis | View spending by scope | Identify trends |
| Budgets | Set spending limits | Alert before overspend |
| Alerts | Threshold notifications | Proactive management |
| Savings | Optimization recommendations | Cost reduction |

### Cost Dimensions

| Dimension | Description |
|-----------|-------------|
| Resource | Individual costs |
| Service | By Azure service |
| Region | Geographic costs |
| Time | Daily/monthly |

### Views

| View | Use |
|------|-----|
| Cost Analysis | Ad-hoc analysis |
| Budgets | Threshold monitoring |
| Savings Plans | Commitment savings |
| Invoices | Billing details |

## 🔧 HOW

### Example 1: View Cost Analysis

```bash
# Open Cost Analysis in portal
# Navigate to Cost Management + Billing
# Select Cost Management
# View Cost Analysis

# CLI - view costs
az consumption usage list \
    --start-date 2025-01-01 \
    --end-date 2025-01-31
```

### Example 2: Create Budget

```bash
# Create budget
az consumption budget create \
    --budget-name monthly-dev \
    --amount 1000 \
    --category Cost \
    --start-date 2025-01-01 \
    --time-grain Monthly \
    --notifications '[
        {
            "enabled": true,
            "operator": "GreaterThan",
            "threshold": 80,
            "contactEmails": ["team@domain.com"]
        }
    ]' \
    --resource-group dev-rg
```

### Example 3: Configure Alerts

```bash
# Add email notification
az consumption budget create \
    --budget-name prod-alert \
    --amount 5000 \
    --category Cost \
    --start-date 2025-01-01 \
    --time-grain Monthly \
    --notification-threshold percents 80,90,100 \
    --contact-emails admin@domain.com,finance@domain.com \
    --contact-groups '/subscriptions/xxx/resourceGroups/myrg'
```

## ⚠️ COMMON ISSUES

- **Data delay**: 8-24 hours for costs
- **Currency**: Costs in billing currency
- **Inclusions**: Some services excluded

## 🏃 PERFORMANCE

- Real-time cost data
- Historical analysis available

## 🌐 COMPATIBILITY

- All Azure services
- CSP, EA, Pay-as-you-go

## 🔗 CROSS-REFERENCES

- **Reservations**: Commitment savings
- **Azure Advisor**: Recommendations

## ✅ EXAM TIPS

- Set budgets before spending
- Check recommendations regularly
- Use tags for allocation