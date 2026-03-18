# Cookie Compliance

## What You'll Learn

- Cookie regulations
- Cookie consent
- Implementing cookie banners

## Prerequisites

- Completed `04-security-compliance.md`

## Cookie Categories

| Category | Examples | Consent Required |
|----------|----------|------------------|
| Essential | Session, Security | No |
| Analytics | Google Analytics | Yes |
| Marketing | Ad tracking | Yes |
| Functional | Language preference | Yes |

## Cookie Policy

```python
from dataclasses import dataclass
from enum import Enum

class CookieCategory(Enum):
    ESSENTIAL = "essential"
    ANALYTICS = "analytics"
    MARKETING = "marketing"
    FUNCTIONAL = "functional"

@dataclass
class Cookie:
    name: str
    category: CookieCategory
    purpose: str
    duration_days: int

COOKIES = [
    Cookie("session_id", CookieCategory.ESSENTIAL, "Session management", 1),
    Cookie("_ga", CookieCategory.ANALYTICS, "Google Analytics", 730),
    Cookie("ads", CookieCategory.MARKETING, "Advertising", 365),
]
```

## Consent Manager

```python
from dataclasses import dataclass, field
from datetime import datetime

@dataclass
class ConsentRecord:
    user_id: str
    categories: dict[str, bool]
    timestamp: datetime
    ip_address: str
    user_agent: str

class CookieConsentManager:
    def __init__(self):
        self.consents: dict[str, ConsentRecord] = {}
    
    def record_consent(self, user_id: str, consents: dict[str, bool], 
                      ip: str, user_agent: str) -> None:
        """Record user consent."""
        record = ConsentRecord(
            user_id=user_id,
            categories=consents,
            timestamp=datetime.now(),
            ip_address=ip,
            user_agent=user_agent
        )
        self.consents[user_id] = record
    
    def has_consent(self, user_id: str, category: str) -> bool:
        """Check if user has given consent for category."""
        record = self.consents.get(user_id)
        if not record:
            return False
        return record.categories.get(category, False)
```

## Summary

- Categorize cookies
- Get consent for non-essential cookies
- Record consent

## Next Steps

Continue to `06-terms-and-privacy-policy.md`.
