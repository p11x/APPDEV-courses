# Cost Analysis for Microservices

## Overview

Understanding and managing costs is crucial for microservices architectures. While microservices offer scalability and agility, they also introduce complexity that can lead to cost overruns if not properly managed. Cost analysis involves tracking, attributing, and optimizing spending across all services and infrastructure components.

Effective cost management requires visibility into resource consumption at multiple levels: individual services, environments (dev, staging, production), and infrastructure components. This visibility enables teams to make informed decisions about optimization opportunities and trade-offs.

This guide covers cost analysis methodologies, tools, and best practices for microservices architectures, helping organizations understand where their money is going and identify optimization opportunities.

## Cost Categories

### 1. Compute Costs

Compute costs include CPU and memory usage for containers, virtual machines, and serverless functions. In Kubernetes environments, this translates to pod CPU and memory requests/limits.

```yaml
# Example: Kubernetes resource requests/limits
resources:
  requests:
    cpu: "500m"
    memory: "512Mi"
  limits:
    cpu: "1000m"
    memory: "1Gi"
```

### 2. Storage Costs

Storage costs encompass database storage, object storage, file storage, and backup storage. Each service typically has its own database, contributing to overall storage costs.

### 3. Network Costs

Network costs include data transfer between services, data transfer to/from external networks, and load balancer costs. In microservices, inter-service communication can generate significant network traffic.

### 4. Managed Service Costs

Managed services like databases, message queues, monitoring, and logging add operational costs. These are often billed based on usage tiers.

## Implementation Example

```python
#!/usr/bin/env python3
"""
Cost Analysis System for Microservices
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional
from datetime import datetime
from enum import Enum
import json


class CostCategory(Enum):
    COMPUTE = "compute"
    STORAGE = "storage"
    NETWORK = "network"
    MANAGED_SERVICES = "managed_services"
    OTHER = "other"


@dataclass
class CostEntry:
    """Represents a cost entry"""
    service_id: str
    category: CostCategory
    resource_id: str
    amount: float
    currency: str = "USD"
    timestamp: datetime = field(default_factory=datetime.now)
    tags: Dict[str, str] = field(default_factory=dict)


@dataclass
class CostSummary:
    """Cost summary for a service or category"""
    service_id: str
    category: CostCategory
    total_cost: float
    cost_change_percent: float
    resources: List[str]


class CostAnalyzer:
    """Analyzes costs across microservices"""
    
    def __init__(self):
        self.cost_entries: List[CostEntry] = []
        self.service_costs: Dict[str, Dict[CostCategory, float]] = {}
    
    def add_cost_entry(
        self,
        service_id: str,
        category: CostCategory,
        resource_id: str,
        amount: float,
        tags: Dict[str, str] = None
    ):
        """Add a cost entry"""
        entry = CostEntry(
            service_id=service_id,
            category=category,
            resource_id=resource_id,
            amount=amount,
            tags=tags or {}
        )
        
        self.cost_entries.append(entry)
        
        # Update service costs
        if service_id not in self.service_costs:
            self.service_costs[service_id] = {}
        
        if category not in self.service_costs[service_id]:
            self.service_costs[service_id][category] = 0.0
        
        self.service_costs[service_id][category] += amount
    
    def get_service_cost(self, service_id: str) -> Dict:
        """Get total cost for a service"""
        if service_id not in self.service_costs:
            return {"total": 0.0, "by_category": {}}
        
        by_category = self.service_costs[service_id]
        total = sum(by_category.values())
        
        return {
            "service_id": service_id,
            "total_cost": total,
            "by_category": {
                cat.value: amount 
                for cat, amount in by_category.items()
            }
        }
    
    def get_top_services(self, limit: int = 10) -> List[Dict]:
        """Get top services by cost"""
        service_totals = []
        
        for service_id, categories in self.service_costs.items():
            total = sum(categories.values())
            service_totals.append({
                "service_id": service_id,
                "total_cost": total
            })
        
        service_totals.sort(key=lambda x: x["total_cost"], reverse=True)
        return service_totals[:limit]
    
    def get_cost_trends(self, service_id: str) -> Dict:
        """Analyze cost trends for a service"""
        # Group by month
        monthly_costs = {}
        
        for entry in self.cost_entries:
            if entry.service_id == service_id:
                month = entry.timestamp.strftime("%Y-%m")
                if month not in monthly_costs:
                    monthly_costs[month] = 0.0
                monthly_costs[month] += entry.amount
        
        # Calculate trend
        months = sorted(monthly_costs.keys())
        if len(months) >= 2:
            last_month = monthly_costs[months[-1]]
            prev_month = monthly_costs[months[-2]]
            change = ((last_month - prev_month) / prev_month) * 100
        else:
            change = 0.0
        
        return {
            "service_id": service_id,
            "monthly_costs": monthly_costs,
            "trend_percent": change
        }
    
    def generate_cost_report(self) -> str:
        """Generate comprehensive cost report"""
        
        # Calculate totals by category
        category_totals = {}
        for entry in self.cost_entries:
            if entry.category not in category_totals:
                category_totals[entry.category] = 0.0
            category_totals[entry.category] += entry.amount
        
        total_cost = sum(category_totals.values())
        
        report = {
            "total_cost": total_cost,
            "by_category": {
                cat.value: amount 
                for cat, amount in category_totals.items()
            },
            "top_services": self.get_top_services(5),
            "services_count": len(self.service_costs)
        }
        
        return json.dumps(report, indent=2)


# Example usage
if __name__ == "__main__":
    analyzer = CostAnalyzer()
    
    # Add cost entries for various services
    analyzer.add_cost_entry(
        "user-service",
        CostCategory.COMPUTE,
        "k8s_pod_user_001",
        150.00,
        {"env": "production", "region": "us-east-1"}
    )
    
    analyzer.add_cost_entry(
        "user-service",
        CostCategory.STORAGE,
        "rds_user_db",
        75.00,
        {"env": "production"}
    )
    
    analyzer.add_cost_entry(
        "order-service",
        CostCategory.COMPUTE,
        "k8s_pod_order_001",
        200.00,
        {"env": "production"}
    )
    
    analyzer.add_cost_entry(
        "order-service",
        CostCategory.NETWORK,
        "lb_order_001",
        50.00
    )
    
    # Generate reports
    print("Service Cost - User Service:")
    print(json.dumps(analyzer.get_service_cost("user-service"), indent=2))
    
    print("\nTop Services:")
    for service in analyzer.get_top_services():
        print(f"  {service['service_id']}: ${service['total_cost']:.2f}")
    
    print("\nFull Report:")
    print(analyzer.generate_cost_report())
```

## Best Practices

1. **Tag Resources Consistently**: Apply tags to all resources for accurate cost attribution.

2. **Track Cost at Service Level**: Attribute costs to individual services to understand spending patterns.

3. **Monitor Trends**: Track cost changes over time to identify anomalies.

4. **Set Budgets**: Establish budgets per service with alerts for exceeding thresholds.

5. **Review Regularly**: Conduct monthly cost reviews with engineering teams.

---

## Output Statement

```
Cost Analysis Report
====================
Total Monthly Cost: $12,450.00

By Category:
- Compute: $7,200.00 (57.8%)
- Storage: $2,800.00 (22.5%)
- Network: $1,500.00 (12.0%)
- Managed Services: $950.00 (7.7%)

Top Services by Cost:
1. order-service: $3,200.00
2. payment-service: $2,800.00
3. user-service: $2,100.00
4. inventory-service: $1,850.00
5. notification-service: $1,200.00

Cost Trends:
- order-service: +5.2% (review needed)
- user-service: -2.1% (improving)
- payment-service: +1.8% (stable)
```