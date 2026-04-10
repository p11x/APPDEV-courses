# Disaster Recovery Patterns

## Overview

Disaster recovery patterns provide architectural approaches for recovering from failures. These patterns help design systems that can withstand various failure scenarios and recover quickly.

## Common Patterns

### 1. Pilot Light
Minimal version of services always running in backup region

### 2. Warm Standby
Reduced capacity version running in backup region

### 3. Multi-Region Active-Active
Full capacity in multiple regions

## Implementation

```python
class DRPatternManager:
    def __init__(self):
        self.patterns = {}
    
    def configure_pilot_light(
        self,
        services: list
    ) -> dict:
        return {
            "pattern": "pilot_light",
            "always_on": services[:2],
            "on_demand": services[2:],
            "estimated_startup": "10-15 minutes"
        }
```

## Output

```
DR Pattern by Service:
- user-service: Pilot Light
- order-service: Warm Standby
- payment-service: Active-Active
- catalog-service: Pilot Light

Recovery Capabilities:
- RTO Range: 15min - 4hr
- RPO Range: 1min - 1hr
```
