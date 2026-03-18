# Privacy by Design

## What You'll Learn

- Privacy by design principles
- Implementing in practice
- Data minimization

## Prerequisites

- Completed `02-data-protection.md`

## Seven Principles

1. **Proactive** - Prevent privacy breaches
2. **Privacy as Default** - Ensure privacy by default
3. **Privacy Embedded** - Part of system design
4. **Full Functionality** - No trade-offs
5. **End-to-End Security** - Complete lifecycle protection
6. **Visibility and Transparency** - Open practices
7. **Respect for User** - User-centric

## Implementation

```python
from dataclasses import dataclass, field
from datetime import datetime
from typing import Callable

@dataclass
class PrivacyConfig:
    """Privacy by design configuration."""
    collect_minimum: bool = True
    encrypt_data: bool = True
    auto_delete: bool = True
    user_control: bool = True
    transparent: bool = True

class PrivacyManager:
    def __init__(self, config: PrivacyConfig):
        self.config = config
        self.data_store: dict = {}
    
    def collect_data(self, user_id: str, data: dict) -> dict:
        """Collect minimum necessary data."""
        if self.config.collect_minimum:
            # Only collect what's needed
            filtered_data = self._filter_essential_only(data)
        else:
            filtered_data = data
        
        self.data_store[user_id] = {
            "data": filtered_data,
            "collected_at": datetime.now(),
            "purpose": "service_delivery"
        }
        
        return filtered_data
    
    def _filter_essential_only(self, data: dict) -> dict:
        """Filter to essential fields only."""
        essential_fields = {"user_id", "email", "subscription_type"}
        return {k: v for k, v in data.items() if k in essential_fields}
```

## Consent First

```python
class ConsentFirstPrivacy:
    """Privacy with consent as primary requirement."""
    
    def __init__(self):
        self.consents: dict[str, dict] = {}
    
    def can_collect(self, user_id: str, purpose: str) -> bool:
        """Check if user has consented."""
        return self.consents.get(user_id, {}).get(purpose, False)
    
    def record_consent(self, user_id: str, purpose: str, granted: bool) -> None:
        """Record user's consent decision."""
        if user_id not in self.consents:
            self.consents[user_id] = {}
        
        self.consents[user_id][purpose] = granted
```

## Summary

- Build privacy into systems from the start
- Collect minimum data needed
- Get consent before collecting

## Next Steps

Continue to `04-security-compliance.md`.
