# Resource Tagging Strategy

## Overview

Resource tagging is essential for cost allocation and governance in microservices architectures. Tags are key-value pairs applied to cloud resources that enable tracking costs by service, environment, team, or any other dimension. Without proper tagging, it's impossible to understand where costs are incurred.

## Tagging Strategy

### Required Tags

| Tag | Description | Example |
|-----|-------------|---------|
| `service` | Microservice name | `order-service` |
| `environment` | Deployment environment | `production`, `staging` |
| `team` | Owner team | `platform-team` |
| `cost-center` | Cost center | `CC-1234` |
| `project` | Project identifier | `migration-q1` |

### Implementation

```python
class ResourceTagger:
    def __init__(self):
        self.required_tags = ["service", "environment", "team"]
    
    def apply_tags(self, resource_id: str, tags: dict) -> bool:
        missing = [t for t in self.required_tags if t not in tags]
        if missing:
            raise ValueError(f"Missing required tags: {missing}")
        # Apply tags to resource
        return True
```

## Output

```
Tagging Compliance: 98%
Resources Tagged: 245/250
Missing Tags: 5 (action required)
```
