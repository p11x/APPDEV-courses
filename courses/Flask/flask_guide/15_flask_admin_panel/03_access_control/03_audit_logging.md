<!-- FILE: 15_flask_admin_panel/03_access_control/03_audit_logging.md -->

## Overview

Implement audit logging for admin actions.

## Code Walkthrough

```python
# audit_log.py
from datetime import datetime

class AuditLog(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer)
    action = db.Column(db.String(50))
    model = db.Column(db.String(50))
    timestamp = db.Column(db.DateTime, default=datetime.utcnow)

def log_action(action, model_name, user_id):
    log = AuditLog(user_id=user_id, action=action, model=model_name)
    db.session.add(log)
    db.session.commit()
```
