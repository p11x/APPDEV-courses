---
Category: Azure Certification
Subcategory: Azure Pricing
Concept: Azure Reservations
Purpose: Commit to Azure resources for cost savings
Difficulty: beginner
Prerequisites: None
RelatedFiles: 02_Advanced_Azure_Reservations.md, 03_Practical_Azure_Reservations.md
UseCase: Predictable workloads, cost optimization
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure Reservations provide significant cost savings (20-40%) by committing to use Azure resources for 1 or 3 years. They're ideal for predictable, steady-state workloads.

## 📖 WHAT

### Reservation Types

| Type | Scope | Savings |
|------|-------|---------|
| Virtual Machines | Compute | 20-40% |
| SQL Database | Database | 20-40% |
| Cosmos DB | Throughput | 20-30% |
| Synapse Analytics | Commit | 20-30% |

### Reservation Concepts

- **Term**: 1 year or 3 years
- **Scope**: Single subscription or shared
- **Instance Size Flexibility**: Automatic discount
- **Coverage**: Applies to matching resources

### Pricing Model

```
Pay-as-you-go: $X/hour
Reserved: $Y/hour (Y < X)
Savings: (X-Y)/X = 20-40%
```

## 🔧 HOW

### Example 1: Purchase Reservation

```bash
# Purchase VM reservation
az reservation catalog-entry show \
    --offer-id 'MS-AZR-0003P' \
    --arm-sku-name Standard_D2s_v3 \
    --location eastus \
    --part-number 9UG-38936

# Purchase via portal or API
# Recommend: Analyze usage first
```

### Example 2: View Reservations

```bash
# List all reservations
az reservation list \
    --output table

# Show reservation details
az reservation show \
    --reservation-order-id xxx \
    --resource-group myrg
```

### Example 3: Check Utilization

```bash
# View utilization
az reservation utilization list \
    --reservation-order-id xxx \
    --resource-group myrg

# Optimize based on utilization
# Consider exchange or cancellation
```

## ⚠️ COMMON ISSUES

- **Unused reservations**: Waste of money
- **Wrong size**: Not all covered
- **Partial coverage**: Requires monitoring

## 🏃 PERFORMANCE

- Instant discount application

## 🌐 COMPATIBILITY

- Most compute resources
- Not all regions

## 🔗 CROSS-REFERENCES

- **Cost Management**: Savings tracking
- **Advisor**: Recommendations

## ✅ EXAM TIPS

- Analyze before purchasing
- Start with 1-year term
- Use shared scope