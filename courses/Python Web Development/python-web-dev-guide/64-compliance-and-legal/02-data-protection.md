# Data Protection

## What You'll Learn

- Protecting user data
- Anonymization and pseudonymization
- Data retention policies

## Prerequisites

- Completed `01-gdpr-basics.md`

## Data Classification

```python
from enum import Enum

class DataClassification(Enum):
    PUBLIC = "public"
    INTERNAL = "internal"
    CONFIDENTIAL = "confidential"
    PII = "pii"  # Personally Identifiable Information

# Define classification for fields
USER_DATA_FIELDS = {
    "user_id": DataClassification.INTERNAL,
    "username": DataClassification.PII,
    "email": DataClassification.PII,
    "password_hash": DataClassification.CONFIDENTIAL,
    "created_at": DataClassification.INTERNAL,
}
```

## Anonymization

```python
import hashlib
import secrets

def anonymize_email(email: str) -> str:
    """Anonymize email address."""
    # Hash the email, keep domain for analytics
    local, domain = email.split("@")
    hashed_local = hashlib.sha256(local.encode()).hexdigest()[:8]
    return f"{hashed_local}@{domain}"

def anonymize_data(data: dict) -> dict:
    """Anonymize user data."""
    anonymized = data.copy()
    
    # Remove or hash PII
    if "email" in anonymized:
        anonymized["email"] = anonymize_email(anonymized["email"])
    
    if "name" in anonymized:
        anonymized["name"] = "REDACTED"
    
    if "phone" in anonymized:
        anonymized["phone"] = "REDACTED"
    
    return anonymized
```

## Pseudonymization

```python
from cryptography.fernet import Fernet
import json

class DataPseudonymizer:
    """Replace identifiers with pseudonyms."""
    
    def __init__(self, key: bytes = None):
        self.key = key or Fernet.generate_key()
        self.cipher = Fernet(self.key)
        self.mapping: dict = {}
    
    def pseudonymize(self, user_id: int) -> str:
        """Replace user ID with pseudonym."""
        if user_id not in self.mapping:
            pseudonym = Fernet.generate_key().decode()
            self.mapping[user_id] = pseudonym
        
        return self.mapping[user_id]
    
    def depseudonymize(self, pseudonym: str) -> int | None:
        """Get original user ID from pseudonym."""
        for user_id, pseudo in self.mapping.items():
            if pseudo == pseudonym:
                return user_id
        return None
```

## Data Retention

```python
from datetime import datetime, timedelta
from dataclasses import dataclass

@dataclass
class RetentionPolicy:
    data_type: str
    retention_days: int
    deletion_method: str  # "anonymize" or "delete"

RETENTION_POLICIES = [
    RetentionPolicy("session_logs", 30, "delete"),
    RetentionPolicy("user_activity", 365, "anonymize"),
    RetentionPolicy("transaction_logs", 2555, "delete"),  # 7 years
    RetentionPolicy("marketing_consent", 730, "delete"),
]

class DataRetentionManager:
    def __init__(self):
        self.policies = {p.data_type: p for p in RETENTION_POLICIES}
        self.data: dict = {}
    
    def should_delete(self, data_type: str, created_at: datetime) -> bool:
        """Check if data should be deleted."""
        policy = self.policies.get(data_type)
        if not policy:
            return False
        
        retention_period = timedelta(days=policy.retention_days)
        return datetime.now() - created_at > retention_period
```

## Summary

- Classify data by sensitivity
- Anonymize or pseudonymize data
- Implement retention policies

## Next Steps

Continue to `03-privacy-by-design.md`.
