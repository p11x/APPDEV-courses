---
Category: Azure Certification
Subcategory: Azure Pricing
Concept: Azure Reservations
Purpose: Advanced reservation strategies and optimization
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Reservations.md
RelatedFiles: 01_Basic_Azure_Reservations.md, 03_Practical_Azure_Reservations.md
UseCase: Enterprise optimization, reservation management
CertificationExam: AZ-305 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced reservation management enables enterprises to maximize savings through proper sizing, exchange, cancellation strategies, and cross-subscription management.

## 📖 WHAT

### Advanced Features

| Feature | Purpose | Benefit |
|---------|---------|---------|
| Instance Flexibility | Auto-match | Broader coverage |
| Shared Scope | Cross-sub | Full utilization |
| Exchange | Change reservation | Flexibility |
| Cancellation | Early close | Partial refund |

### Reservation Strategy

| Workload Type | Approach | Savings |
|--------------|---------|----------|
| Steady 24/7 | Reserved | 30-40% |
| Business hours | Reserved | 20-30% |
| Variable | Savings Plan | 20-35% |
| Burst | On-demand | 0% (pay for use) |

### Cross-Platform Comparison

| Feature | Azure | AWS | GCP |
|---------|-------|-----|-----|
| Committed Use | Reserved | Savings Plans | Committed |
| Flexibility | Yes | High | Limited |
| Exchange | Yes | Yes | No |
| Scope | Shared | Shared | Project |

## 🔧 HOW

### Example 1: Shared Scope Reservation

```bash
# Purchase with shared scope
az reservation order create \
    --arm-sku-name Standard_D2s_v3 \
    --billing-scope /subscriptions/xxx \
    --display-name 'Shared-VM-Res' \
    --quantity 2 \
    --reservation-order-name 'res-order-001' \
    --start-date 2025-02-01 \
    --term 1Year \
    --billing-plan-id xxx

# Applies to all subscriptions in enrollment
```

### Example 2: Exchange Reservation

```bash
# Exchange for different size
az reservation order exchange post \
    --reservation-order-id xxx \
    --reservations '[
        {
            "reservationId": "res-id-1",
            "quantity": 1
        }
    ]' \
    --target-sku Standard_D4s_v3
```

### Example 3: Reservation to Savings Plan Migration

```bash
# Migration available via portal
# 1. Review savings plan eligibility
# 2. Calculate equivalent
# 3. Migrate while keeping savings
# Note: One-way transition

# View options
az savings-plan show \
    --savings-plan-id xxx
```

## ⚠️ COMMON ISSUES

- **Lock-in**: 1-3 year commitment
- **Refund limits**: Partial only
- **Scope restrictions**: Cannot change scope

## 🏃 PERFORMANCE

| Operation | SLA |
|-----------|-----|
| Purchase | Instant |
| Exchange | 2-3 days |
| Cancellation | 5 days |

## 🌐 COMPATIBILITY

- Enterprise Agreement
- Microsoft Customer Agreement

## 🔗 CROSS-REFERENCES

- **Cost Management**: Savings tracking
- **Savings Plans**: Alternative

## ✅ EXAM TIPS

- Analyze utilization first
- Use shared scope for EA
- Plan refresh cycle