---
Category: AWS Cloud Practitioner
Subcategory: AWS Billing
Concept: Cost Management
Purpose: Understanding AWS cost management tools including Cost Explorer, Budgets, and CUR
Difficulty: intermediate
Prerequisites: 01_Basic_Pricing_Models.md
RelatedFiles: 02_Advanced_Cost_Management.md, 03_Practical_Cost_Management.md
UseCase: Tracking, analyzing, and optimizing cloud costs
CertificationExam: AWS Cloud Practitioner
LastUpdated: 2025
---

## 💡 WHY

AWS cost management tools enable organizations to track spending, analyze cost drivers, and optimize cloud expenses. Understanding these tools is essential for controlling cloud costs.

### Why Cost Management Matters

- **Visibility**: Know where money is spent
- **Control**: Set limits and alerts
- **Optimization**: Identify savings opportunities
- **Accountability**: Chargeback to teams

### Key Tools

- **Cost Explorer**: Visualize spending trends
- **Budgets**: Set spending limits with alerts
- **Cost and Usage Report (CUR)**: Detailed data
- **Savings Plans**: Commitment discounts

## 📖 WHAT

### Cost Management Components

**Cost Explorer**: Interactive charts and filters

**Budgets**: Threshold alerts and forecasts

**Cost Categories**: Group by tag, service, account

**Reserved Instance Reporting**: Coverage analysis

### Architecture Diagram

```
Cost Management Architecture
==============================

AWS Services ──────► Cost Data ◄─────► Analysis
                            │
                    ┌───────┴───────┐
                    │               │
              ┌─────┴─────┐    ┌────┴─────┐
              │ Cost      │    │ Budgets  │
              │ Explorer  │    │ Alerts   │
              └───────────┘    └──────────┘
```

## 🔧 HOW

### Example 1: Cost Explorer

```bash
# Get current month costs
aws ce get-cost-and-usage \
    --time-period Start=2024-01-01,End=2024-01-31 \
    --granularity MONTHLY \
    --metrics UnblendedCost \
    --group-by '[
        {"Type": "DIMENSION", "Key": "SERVICE"}
    ]'

# Get EC2 costs
aws ce get-cost-and-usage \
    --time-period Start=2024-01-01,End=2024-01-31 \
    --metrics UnblendedCost \
    --filter '{
        "Dimensions": {
            "Key": "SERVICE",
            "Values": ["Amazon EC2"]
        }
    }'

# Get cost forecast
aws ce get-cost-forecast \
    --metric UNBLENDED_COST \
    --granularity MONTHLY \
    --start-time 2024-02-01 \
    --end-time 2024-03-31
```

### Example 2: Budget Setup

```bash
# Create budget
aws budgets create-budget \
    --account-id 123456789012 \
    --budget '{
        "BudgetName": "monthly-spend",
        "BudgetType": COST",
        "TimeUnit": "MONTHLY",
        "BudgetLimit": {
            "Amount": "1000.0",
            "Unit": "USD"
        },
        "CostFilters": {
            "Key": "SERVICE",
            "Values": ["Amazon EC2"]
        }
    }'

# Add alert
aws budgets create-notification \
    --account-id 123456789012 \
    --budget-name monthly-spend \
    --notification '{
        "NotificationType": "ACTUAL",
        "Threshold": 80.0,
        "ThresholdType": "PERCENTAGE",
        "ComparisonOperator": "GREATER_THAN"
    }'
```

### Example 3: Cost Categories

```bash
# Create cost category
aws ce create-cost-category-definition \
    --name "Environment" \
    --rules '[{
        "Value": "Production",
        "Rule": {
            "CostCategories": {
                "Key": "Environment",
                "MatchOptions": ["CONTAINS"],
                "Values": ["prod", "production"]
            }
        }
    }, {
        "Value": "Development",
        "Rule": {
            "CostCategories": {
                "Key": "Environment",
                "MatchOptions": ["CONTAINS"],
                "Values": ["dev", "development"]
            }
        }
    }]'

# Get cost by category
aws ce get-cost-and-usage \
    --time-period Start=2024-01-01,End=2024-01-31 \
    --metrics UnblendedCost \
    --group-by '[
        {"Type": "COST_CATEGORY", "Key": "Environment"}
    ]'
```

### Example 4: CUR and Analytics

```bash
# Create CUR delivery
aws cur put-delivery-preferences \
    --delivery-preferences '{
        "Format": "TEXT_OR_CSV",
        "Compression": "ZIP",
        "AdditionalSchemaElements": ["RESOURCES"],
        "S3Destination": {
            "Bucket": "my-cur-bucket",
            "Prefix": "cur-reports/",
            "Region": "us-east-1"
        }
    }'

# Use Athena to query CUR (via console or SQL)
# Example query:
# SELECT line_item_product_code, 
#        sum(line_item_unblended_cost) as cost
# FROM aws_cur_data
# WHERE year = '2024' AND month = '01'
# GROUP BY line_item_product_code
```

## ⚠️ COMMON ISSUES

### 1. Missing Cost Data

**Problem**: Costs not appearing in Explorer.

**Solution**: Wait 24-48 hours, check CUR delivery.

### 2. Budget Not Triggering Alerts

**Problem**: No notification received.

**Solution**: Verify SNS topic subscription, check alert threshold.

### 3. High Costs Unexpected

**Problem**: Spending higher than expected.

**Solution**: Use Cost Anomaly Detection, set up alerts.

### 4. Savings Plan Not Showing Savings

**Problem**: Not seeing savings.

**Solution**: Check coverage report, understand discount application.

## 🏃 PERFORMANCE

### Cost Allocation Tags

- Enable tags for detailed tracking
- Tags propagate to supported services
- Cost Categories use tags for grouping

## 🌐 COMPATIBILITY

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Cost Explorer | Yes | Cost Analysis | Billing |
| Budgets | Yes | Budgets | Budget Alerts |
| Tags | Yes | Tags | Labels |

## 🔗 CROSS-REFERENCES

**Related**: Pricing Models, Organizations, Savings Plans

**Prerequisite**: Pricing fundamentals

**Next**: Reserved Instance optimization

## ✅ EXAM TIPS

- Cost Explorer shows last 12 months
- Budgets can track Cost, Usage, RI
- Tags must be enabled for cost allocation
- Cost Anomaly Detection uses ML