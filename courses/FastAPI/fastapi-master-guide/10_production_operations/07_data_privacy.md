# Data Privacy

## Overview

Data privacy implementation ensures compliance with privacy regulations.

## Privacy Implementation

### Data Anonymization

```python
# Example 1: Data anonymization
from fastapi import FastAPI
from pydantic import BaseModel
import hashlib

app = FastAPI()

class PrivacyManager:
    """Manage data privacy"""

    @staticmethod
    def anonymize_email(email: str) -> str:
        """Anonymize email address"""
        local, domain = email.split("@")
        masked_local = local[0] + "*" * (len(local) - 2) + local[-1]
        return f"{masked_local}@{domain}"

    @staticmethod
    def anonymize_phone(phone: str) -> str:
        """Anonymize phone number"""
        return "*" * (len(phone) - 4) + phone[-4:]

    @staticmethod
    def hash_identifier(identifier: str) -> str:
        """One-way hash for identifiers"""
        return hashlib.sha256(identifier.encode()).hexdigest()

privacy = PrivacyManager()

@app.get("/users/{user_id}/profile")
async def get_user_profile(user_id: int):
    """Get user profile with privacy protection"""
    user = await get_user_from_db(user_id)

    return {
        "id": user.id,
        "username": user.username,
        "email": privacy.anonymize_email(user.email),
        "phone": privacy.anonymize_phone(user.phone)
    }

# Consent management
@app.post("/users/{user_id}/consent")
async def update_consent(user_id: int, consent: Dict):
    """Update user consent preferences"""
    await save_consent(user_id, consent)
    return {"updated": True}

@app.get("/users/{user_id}/data-export")
async def export_data(user_id: int):
    """Export all user data (GDPR right to access)"""
    data = await collect_all_user_data(user_id)
    return data

@app.delete("/users/{user_id}/data")
async def delete_data(user_id: int, confirm: bool = False):
    """Delete all user data (GDPR right to erasure)"""
    if not confirm:
        raise HTTPException(400, "Confirmation required")

    await delete_all_user_data(user_id)
    return {"deleted": True}
```

## Summary

Data privacy is essential for regulatory compliance.

## Next Steps

Continue learning about:
- [Security Operations](./08_security_operations.md)
- [Compliance Automation](./05_compliance_automation.md)
