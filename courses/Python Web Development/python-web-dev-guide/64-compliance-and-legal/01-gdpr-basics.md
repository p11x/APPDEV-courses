# GDPR Basics

## What You'll Learn

- Understanding GDPR
- Data subject rights
- Compliance requirements

## Prerequisites

- Basic web development knowledge

## What Is GDPR

The General Data Protection Regulation is an EU regulation on data protection and privacy. It applies to any organization handling EU residents' data.

Think of GDPR like a privacy bill of rights - it gives people control over their personal data.

## Key Principles

1. **Lawfulness** - Process data legally
2. **Purpose Limitation** - Use data for specific purposes
3. **Data Minimization** - Only collect necessary data
4. **Accuracy** - Keep data accurate
5. **Storage Limitation** - Don't keep data forever
6. **Integrity** - Protect data security

## Data Subject Rights

| Right | Description |
|-------|-------------|
| Access | View their data |
| Rectification | Correct inaccurate data |
| Erasure | Request deletion ("right to be forgotten") |
| Restriction | Limit processing |
| Portability | Get data in machine-readable format |
| Object | Object to processing |

## Implementing in Python

```python
from dataclasses import dataclass
from datetime import datetime

@dataclass
class User:
    id: int
    name: str
    email: str
    consent_date: datetime | None
    data: dict

class GDPRCompliance:
    def __init__(self):
        self.users: dict[int, User] = {}
    
    def get_user_data(self, user_id: int) -> dict:
        """Right to access - provide all user data."""
        user = self.users.get(user_id)
        if not user:
            raise ValueError("User not found")
        
        return {
            "id": user.id,
            "name": user.name,
            "email": user.email,
            "data": user.data,
            "consent_date": user.consent_date.isoformat() if user.consent_date else None
        }
    
    def delete_user_data(self, user_id: int) -> None:
        """Right to erasure - delete all user data."""
        if user_id in self.users:
            del self.users[user_id]
    
    def export_user_data(self, user_id: int) -> str:
        """Right to portability - export as JSON."""
        import json
        data = self.get_user_data(user_id)
        return json.dumps(data, indent=2)
```

## Consent Management

```python
from dataclasses import dataclass
from datetime import datetime

@dataclass
class Consent:
    user_id: int
    purpose: str
    granted: bool
    timestamp: datetime

class ConsentManager:
    def __init__(self):
        self.consents: list[Consent] = []
    
    def record_consent(self, user_id: int, purpose: str, granted: bool) -> None:
        """Record user consent."""
        consent = Consent(
            user_id=user_id,
            purpose=purpose,
            granted=granted,
            timestamp=datetime.now()
        )
        self.consents.append(consent)
    
    def has_consent(self, user_id: int, purpose: str) -> bool:
        """Check if user has given consent."""
        for consent in reversed(self.consents):
            if consent.user_id == user_id and consent.purpose == purpose:
                return consent.granted
        return False
```

## Summary

- GDPR protects EU residents' data
- Users have rights over their data
- Need consent and data protection

## Next Steps

Continue to `02-data-protection.md`.
