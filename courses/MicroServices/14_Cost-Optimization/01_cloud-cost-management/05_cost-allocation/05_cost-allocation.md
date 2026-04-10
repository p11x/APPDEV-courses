# Cost Allocation Strategy

## Overview

Cost allocation enables organizations to distribute cloud spending across teams, services, or projects. This is critical for chargeback/showback models and for identifying cost optimization opportunities.

## Allocation Methods

### 1. Tag-Based Allocation

Allocate costs based on resource tags:
```python
def allocate_by_tags(costs: dict, tags: dict) -> dict:
    allocations = {}
    for resource, cost in costs.items():
        service = tags.get(resource, {}).get("service", "unknown")
        allocations[service] = allocations.get(service, 0) + cost
    return allocations
```

### 2. Usage-Based Allocation

Allocate based on actual usage metrics:
```python
def allocate_by_usage(usage: dict, total_cost: float) -> dict:
    total_usage = sum(usage.values())
    return {
        k: (v / total_usage) * total_cost 
        for k, v in usage.items()
    }
```

## Output

```
Cost Allocation by Service:
- order-service: $3,200 (25%)
- user-service: $2,800 (22%)
- payment-service: $2,400 (19%)
- catalog-service: $2,100 (17%)
- Other: $2,150 (17%)
```
