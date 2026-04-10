# Reserved Instances for Cost Optimization

## Overview

Reserved Instances (RIs) allow you to commit to compute capacity for 1-3 years in exchange for significant discounts (typically 40-60% off on-demand pricing). For microservices with predictable workloads, RIs can substantially reduce cloud spending.

## Strategy

### When to Use Reserved Instances

- **Stable workloads**: Production services with consistent usage
- **Long-running services**: Services that run 24/7
- **Predictable capacity**: Known baseline capacity

### Instance Sizing

- Analyze 30+ days of usage
- Size for base load, not peak
- Use partial reservations for flexibility

## Implementation

```python
class ReservedInstanceManager:
    def __init__(self):
        self.reservations = []
    
    def calculate_savings(self, on_demand_cost: float, ri_coverage: float) -> dict:
        savings = on_demand_cost * ri_coverage * 0.5  # 50% typical savings
        return {
            "monthly_savings": savings,
            "annual_savings": savings * 12
        }
```

## Output

```
RI Coverage: 70%
Monthly Savings: $4,200
Annual Savings: $50,400
```
