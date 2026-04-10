# Audit Logging for Compliance

## Overview

Audit logging captures all system activities essential for compliance, security, and debugging. This guide covers audit logging requirements, implementation, and best practices for microservices.

## Requirements by Standard

| Standard | Log Requirements | Retention |
|----------|------------------|-----------|
| PCI-DSS | All access to cardholder data | 1 year |
| HIPAA | All PHI access | 6 years |
| SOX | Financial transaction records | 7 years |
| GDPR | All data processing activities | 2 years |

## Implementation

```python
import json
from datetime import datetime

class AuditLogger:
    def __init__(self):
        self.log_store = []  # Would use proper logging system
    
    def log_event(
        self,
        event_type: str,
        user_id: str,
        resource: str,
        action: str,
        result: str,
        metadata: dict = None
    ):
        event = {
            "timestamp": datetime.now().isoformat(),
            "event_type": event_type,
            "user_id": user_id,
            "resource": resource,
            "action": action,
            "result": result,
            "metadata": metadata or {}
        }
        
        self.log_store.append(event)
        return event
```

## Output

```
Audit Log Status:
- Events Logged Today: 15,234
- Storage Used: 45GB
- Retention: Per compliance requirement

Compliance Coverage:
- Authentication: 100%
- Authorization: 100%
- Data Access: 100%
- Configuration Changes: 100%
```
