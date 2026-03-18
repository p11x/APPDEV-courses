# Audit Logging

## What You'll Learn

- What to log
- Audit trail requirements
- Implementing audit logs

## Prerequisites

- Completed `06-terms-and-privacy-policy.md`

## What to Log

- Authentication events (login, logout, failures)
- Data access and modifications
- Administrative actions
- Security events
- Errors and exceptions

## Audit Log Implementation

```python
from dataclasses import dataclass
from datetime import datetime
from enum import Enum

class AuditAction(Enum):
    LOGIN = "login"
    LOGOUT = "logout"
    LOGIN_FAILED = "login_failed"
    DATA_ACCESS = "data_access"
    DATA_CREATE = "data_create"
    DATA_UPDATE = "data_update"
    DATA_DELETE = "data_delete"
    ADMIN_ACTION = "admin_action"

@dataclass
class AuditLogEntry:
    timestamp: datetime
    user_id: str | None
    action: AuditAction
    resource: str
    details: dict
    ip_address: str | None
    user_agent: str | None

class AuditLogger:
    def __init__(self):
        self.logs: list[AuditLogEntry] = []
    
    def log(self, user_id: str | None, action: AuditAction, 
            resource: str, details: dict,
            ip_address: str | None = None,
            user_agent: str | None = None) -> None:
        """Create audit log entry."""
        entry = AuditLogEntry(
            timestamp=datetime.now(),
            user_id=user_id,
            action=action,
            resource=resource,
            details=details,
            ip_address=ip_address,
            user_agent=user_agent
        )
        self.logs.append(entry)
    
    def get_logs(self, user_id: str | None = None,
                  action: AuditAction | None = None) -> list[AuditLogEntry]:
        """Query audit logs."""
        results = self.logs
        
        if user_id:
            results = [l for l in results if l.user_id == user_id]
        
        if action:
            results = [l for l in results if l.action == action]
        
        return results
```

## Summary

- Log important events
- Keep audit trail for compliance
- Secure audit logs

## Next Steps

Continue to `08-legal-requirements.md`.
