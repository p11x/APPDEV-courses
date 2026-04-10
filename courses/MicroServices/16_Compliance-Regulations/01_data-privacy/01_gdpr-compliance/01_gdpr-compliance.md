# GDPR Compliance for Microservices

## Overview

The General Data Protection Regulation (GDPR) is a comprehensive data privacy regulation that affects any organization processing personal data of EU residents. Microservices architectures introduce unique challenges for GDPR compliance due to distributed data storage and processing. This guide covers GDPR requirements and their implementation in microservices.

GDPR establishes principles for lawful data processing, including lawfulness, fairness, transparency, purpose limitation, data minimization, accuracy, storage limitation, integrity and confidentiality, and accountability. Organizations must demonstrate compliance through appropriate technical and organizational measures.

## Key Requirements

### 1. Data Subject Rights

GDPR grants data subjects specific rights that microservices must support:

- **Right to Access**: Individuals can request copies of their personal data
- **Right to Rectification**: Individuals can correct inaccurate data
- **Right to Erasure**: Individuals can request deletion of their data ("right to be forgotten")
- **Right to Data Portability**: Individuals can request their data in portable format
- **Right to Object**: Individuals can object to processing

### 2. Data Processing Principles

```python
# Example: Data processing compliance implementation
from dataclasses import dataclass
from datetime import datetime
from typing import List, Optional
from enum import Enum

class ProcessingPurpose(Enum):
    CONTRACT = "contract"
    CONSENT = "consent"
    LEGAL_OBLIGATION = "legal_obligation"
    VITAL_INTERESTS = "vital_interests"
    PUBLIC_TASK = "public_task"
    LEGITIMATE_INTERESTS = "legitimate_interests"

@dataclass
class DataProcessingRecord:
    """Record of data processing activity"""
    processing_id: str
    data_categories: List[str]
    purpose: ProcessingPurpose
    legal_basis: str
    retention_period: str
    recipients: List[str]
    safeguards: List[str]

@dataclass
class DataSubjectRequest:
    """Request from a data subject"""
    request_id: str
    request_type: str
    requester_id: str
    request_date: datetime
    status: str
    response_date: Optional[datetime]
```

### 3. Data Protection Implementation

```python
class GDPRComplianceService:
    """GDPR compliance implementation for microservices"""
    
    def __init__(self, data_store):
        self.data_store = data_store
    
    def handle_data_access_request(self, user_id: str) -> dict:
        """Handle right to access request"""
        user_data = self.data_store.get_user_data(user_id)
        
        return {
            "personal_data": user_data,
            "categories": self._identify_data_categories(user_data),
            "processing_purposes": self._get_purposes(user_id),
            "recipients": self._get_recipients(user_id),
            "retention_period": self._get_retention(user_id),
            "source": self._get_data_source(user_id)
        }
    
    def handle_data_deletion_request(self, user_id: str) -> bool:
        """Handle right to erasure request"""
        # Check if deletion is subject to legal obligations
        if self._has_retention_requirement(user_id):
            raise GDPRException(
                "Data retention required by legal obligation"
            )
        
        # Delete all personal data
        self.data_store.delete_user_data(user_id)
        
        # Delete from all services
        for service in self.data_store.get_related_services(user_id):
            service.delete_user_data(user_id)
        
        return True
    
    def handle_data_portability_request(self, user_id: str) -> dict:
        """Handle right to data portability"""
        user_data = self.data_store.get_user_data(user_id)
        
        # Format in machine-readable format
        return {
            "data": user_data,
            "format": "JSON",
            "schema": "user-data-v1"
        }
    
    def implement_privacy_by_design(self) -> dict:
        """Implement privacy by design principles"""
        return {
            "data_minimization": {
                "implemented": True,
                "measures": [
                    "Collect only necessary fields",
                    "Anonymize where possible",
                    "Pseudonymize sensitive data"
                ]
            },
            "purpose_limitation": {
                "implemented": True,
                "measures": [
                    "Document processing purposes",
                    "Enforce purpose in data model",
                    "Review purposes quarterly"
                ]
            },
            "storage_limitation": {
                "implemented": True,
                "measures": [
                    "Automated data expiration",
                    "Retention policies",
                    "Periodic data audits"
                ]
            }
        }
```

## Consent Management

```python
class ConsentManager:
    """Manage user consent for data processing"""
    
    def __init__(self):
        self.consents = {}
    
    def record_consent(
        self,
        user_id: str,
        purpose: str,
        granted: bool
    ) -> bool:
        """Record user consent"""
        consent_record = {
            "user_id": user_id,
            "purpose": purpose,
            "granted": granted,
            "timestamp": datetime.now(),
            "ip_address": self._get_client_ip(),
            "consent_method": "web_form"
        }
        
        self.consents[f"{user_id}:{purpose}"] = consent_record
        return True
    
    def check_consent(self, user_id: str, purpose: str) -> bool:
        """Check if user has granted consent"""
        consent = self.consents.get(f"{user_id}:{purpose}")
        return consent and consent.get("granted", False)
    
    def withdraw_consent(self, user_id: str, purpose: str) -> bool:
        """Withdraw user consent"""
        if f"{user_id}:{purpose}" in self.consents:
            self.consents[f"{user_id}:{purpose}"]["granted"] = False
            self.consents[f"{user_id}:{purpose}"]["withdrawn"] = datetime.now()
            return True
        return False
```

## Output Statement

```
GDPR Compliance Status
=======================
Organization: Example Corp
Last Audit: 2024-01-15

Data Subject Requests:
- Received: 45
- Completed within 30 days: 44
- Average response time: 18 days

Consents:
- Total active consents: 15,230
- Marketing consent rate: 72%
- Withdrawal rate: 3.2%

Breach Notifications:
- Total breaches: 0
- Reported to authority: 0
- Reported to data subjects: 0

Compliance Score: 94%
```