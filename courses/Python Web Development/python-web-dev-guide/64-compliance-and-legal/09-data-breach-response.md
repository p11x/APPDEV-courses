# Data Breach Response

## What You'll Learn

- Breach detection and reporting
- Incident response
- Notification requirements

## Prerequisites

- Completed `08-legal-requirements.md`

## Types of Breaches

- **Confidentiality Breach** - Unauthorized access to data
- **Integrity Breach** - Data modified without permission
- **Availability Breach** - Data made unavailable

## Breach Response Plan

```python
from dataclasses import dataclass
from datetime import datetime
from enum import Enum

class BreachSeverity(Enum):
    LOW = "low"
    MEDIUM = "medium"
    HIGH = "high"
    CRITICAL = "critical"

@dataclass
class DataBreach:
    id: str
    discovered_at: datetime
    severity: BreachSeverity
    description: str
    data_affected: list[str]
    users_affected: int
    reported_at: datetime | None = None

class BreachResponseManager:
    def __init__(self):
        self.breaches: list[DataBreach] = []
    
    def report_breach(self, breach: DataBreach) -> None:
        """Report a data breach."""
        self.breaches.append(breach)
    
    def notify_authorities(self, breach: DataBreach) -> None:
        """Notify relevant authorities within 72 hours."""
        # GDPR requires notification within 72 hours
        breach.reported_at = datetime.now()
        # In practice: notify GDPR supervisory authority
```

## Steps to Take

1. **Detect** - Identify the breach
2. **Contain** - Limit the damage
3. **Assess** - Determine severity and scope
4. **Notify** - Inform authorities and users
5. **Remediate** - Fix the vulnerability
6. **Document** - Record the incident

## Summary

- Have breach response plan
- Notify authorities within required timeframe
- Document everything

## Next Steps

Continue to `10-compliance-checklist.md`.
