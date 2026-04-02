# Audit Trails

## Overview

Audit trails track all system activities for compliance and debugging.

## Implementation

### Comprehensive Audit Logging

```python
# Example 1: Audit trail system
from fastapi import FastAPI, Request, Depends
from pydantic import BaseModel
from datetime import datetime
from typing import Optional
import json

app = FastAPI()

class AuditEntry(BaseModel):
    timestamp: datetime
    user_id: Optional[int]
    action: str
    resource: str
    resource_id: Optional[str]
    details: Dict
    ip_address: str
    user_agent: str

audit_log: List[AuditEntry] = []

class AuditLogger:
    """Audit logging service"""

    async def log(self, entry: AuditEntry):
        """Log audit entry"""
        audit_log.append(entry)
        # Also persist to database
        await save_audit_entry(entry)

    async def get_user_actions(
        self,
        user_id: int,
        start_date: datetime,
        end_date: datetime
    ):
        """Get user actions in date range"""
        return [
            e for e in audit_log
            if e.user_id == user_id
            and start_date <= e.timestamp <= end_date
        ]

audit_logger = AuditLogger()

@app.middleware("http")
async def audit_middleware(request: Request, call_next):
    """Audit all requests"""
    start_time = datetime.utcnow()

    response = await call_next(request)

    # Log non-GET requests
    if request.method != "GET":
        user = get_user_from_request(request)

        entry = AuditEntry(
            timestamp=start_time,
            user_id=user.id if user else None,
            action=request.method,
            resource=request.url.path,
            resource_id=None,
            details={
                "query_params": dict(request.query_params),
                "status_code": response.status_code
            },
            ip_address=request.client.host,
            user_agent=request.headers.get("user-agent", "")
        )

        await audit_logger.log(entry)

    return response

@app.get("/audit/logs")
async def get_audit_logs(
    user_id: Optional[int] = None,
    start_date: datetime = None,
    end_date: datetime = None
):
    """Query audit logs"""
    filtered = audit_log

    if user_id:
        filtered = [e for e in filtered if e.user_id == user_id]

    if start_date:
        filtered = [e for e in filtered if e.timestamp >= start_date]

    if end_date:
        filtered = [e for e in filtered if e.timestamp <= end_date]

    return {"logs": filtered, "total": len(filtered)}
```

## Summary

Audit trails provide compliance and debugging capabilities.

## Next Steps

Continue learning about:
- [Security Operations](./08_security_operations.md)
- [Compliance Automation](./05_compliance_automation.md)
