---
Category: Azure Certification
Subcategory: Azure Pricing
Concept: Azure Cost Management
Purpose: Enterprise cost optimization, chargeback, and governance
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Cost_Management.md
RelatedFiles: 01_Basic_Azure_Cost_Management.md, 03_Practical_Azure_Cost_Management.md
UseCase: Chargeback, showback, cost optimization, enterprise governance
CertificationExam: AZ-305 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced Cost Management features enable enterprise-scale cost governance with chargeback/showback models, sophisticated reporting, integration with FinOps practices, and comprehensive cost optimization automation.

## 📖 WHAT

### Enterprise Features

| Feature | Use Case | Benefit |
|---------|----------|---------|
| Cost Allocation | Chargeback | Business unit attribution |
| Anomaly Detection | Unexpected costs | Early warning |
| Forecast | Planning | Future prediction |
| Commitments | Reservations | Significant savings |

### FinOps Maturity Model

| Level | Practice | Cost Impact |
|-------|----------|-------------|
| Inform | Visibility | Baseline |
| Optimize | Rightsizing | 15-25% |
| Operate | Continuous | 20-30% |

### Cross-Platform Comparison

| Feature | Azure | AWS | GCP |
|---------|-------|-----|-----|
| Cost Explorer | Cost Analysis | Cost Explorer | Billing |
| Budgets | 100/billing account | Budgets | Budgets |
| Alerts | Email/Webhook | SNS | Pub/Sub |
| Anomaly | Yes | Limited | Yes |
| Commitments | Reservations | Savings Plans | CommittedUse |

## 🔧 HOW

### Example 1: Cost Allocation with Tags

```bash
# Create budget by tag
az consumption budget create \
    --budget-name 'team-monthly' \
    --amount 10000 \
    --category Cost \
    --start-date 2025-01-01 \
    --time-grain Monthly \
    --filter-tags '{\"Environment\":[\"Production\"]}' \
    --contact-emails team@domain.com

# Run cost analysis by tag
# Use Cost Analysis > Group by > Tag
# Select tag (e.g., CostCenter)
```

### Example 2: Anomaly Detection

```bash
# Enable cost anomaly
az costmanagement alert create \
    --active \
    --alert-type CostAnomaly \
    --globally-enabled true \
    --operator GreaterThan \
    --threshold 20 \
    --recipients 'admin@domain.com,finance@domain.com'

# View anomalies
az costmanagementanomaly list \
    --scope /subscriptions/xxx
```

### Example 3: Cost Export to Storage

```bash
# Create export
az costmanagement export create \
    --name daily-costs \
    --category Cost \
    --storage-container insights-logs \
    --time-grain Daily \
    --start-date 2025-01-01 \
    --storage-account mystorage \
    --storage-directory costs exports

# Daily CSV export to blob storage
```

## ⚠️ COMMON ISSUES

- **Tag gaps**: Missing tags = unallocated
- **Forecast errors**: Based on patterns
- **Currency conversion**: Use unified currency

## 🏃 PERFORMANCE

| Operation | SLA |
|-----------|-----|
| Cost data | 8-24 hours |
| Exports | Daily |
| Alerts | Near real-time |

## 🌐 COMPATIBILITY

- EA agreements fully supported
- CSP limited features

## 🔗 CROSS-REFERENCES

- **Reservations**: Commitment savings
- **Azure Advisor**: Optimization
- **Azure Lighthouse**: Multi-tenant

## ✅ EXAM TIPS

- Use tags from deployment
- Enable anomaly alerts
- Review forecast regularly