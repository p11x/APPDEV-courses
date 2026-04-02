# Regulatory Compliance

## Overview

Regulatory compliance ensures FastAPI applications meet legal requirements like GDPR, HIPAA, and PCI-DSS.

## GDPR Compliance

### Data Protection Implementation

```python
# Example 1: GDPR compliance features
from fastapi import FastAPI, HTTPException, Depends
from pydantic import BaseModel
from typing import Optional
from datetime import datetime

app = FastAPI()

class ConsentRecord(BaseModel):
    user_id: int
    consent_type: str
    granted: bool
    timestamp: datetime
    ip_address: str

class DataExportRequest(BaseModel):
    user_id: int
    format: str = "json"

class DataDeletionRequest(BaseModel):
    user_id: int
    confirm: bool

# Consent Management
@app.post("/consent/")
async def record_consent(consent: ConsentRecord):
    """Record user consent (GDPR requirement)"""
    await save_consent(consent)
    return {"status": "recorded"}

@app.get("/consent/{user_id}")
async def get_consent(user_id: int):
    """Get user's consent status"""
    return await get_user_consent(user_id)

# Right to Access (GDPR Article 15)
@app.get("/users/{user_id}/data-export")
async def export_user_data(user_id: int, format: str = "json"):
    """Export all user data (GDPR right to access)"""
    user_data = await collect_all_user_data(user_id)

    if format == "json":
        return user_data
    elif format == "csv":
        return generate_csv(user_data)

# Right to Erasure (GDPR Article 17)
@app.delete("/users/{user_id}/data-deletion")
async def delete_user_data(user_id: int, confirm: bool = False):
    """Delete all user data (GDPR right to erasure)"""
    if not confirm:
        raise HTTPException(
            400,
            "Please confirm data deletion"
        )

    await anonymize_user_data(user_id)
    await delete_user_records(user_id)

    return {"status": "deleted", "user_id": user_id}

# Data Processing Logs
@app.middleware("http")
async def log_data_processing(request: Request, call_next):
    """Log all data processing activities"""
    response = await call_next(request)

    if request.method in ["POST", "PUT", "PATCH", "DELETE"]:
        await log_processing_activity({
            "endpoint": request.url.path,
            "method": request.method,
            "timestamp": datetime.utcnow(),
            "user_id": get_user_id_from_request(request)
        })

    return response
```

## HIPAA Compliance

### Healthcare Data Protection

```python
# Example 2: HIPAA compliance
from cryptography.fernet import Fernet
import hashlib

class PHIProtection:
    """Protected Health Information (PHI) handling"""

    def __init__(self, encryption_key: bytes):
        self.cipher = Fernet(encryption_key)

    def encrypt_phi(self, data: str) -> str:
        """Encrypt PHI data"""
        return self.cipher.encrypt(data.encode()).decode()

    def decrypt_phi(self, encrypted_data: str) -> str:
        """Decrypt PHI data"""
        return self.cipher.decrypt(encrypted_data.encode()).decode()

    def hash_identifier(self, identifier: str) -> str:
        """Hash patient identifier"""
        return hashlib.sha256(identifier.encode()).hexdigest()

phi_protection = PHIProtection(Fernet.generate_key())

class PatientRecord(BaseModel):
    patient_id: str  # Hashed
    encrypted_ssn: str  # Encrypted
    encrypted_medical_record: str  # Encrypted
    created_at: datetime

@app.post("/patients/")
async def create_patient_record(ssn: str, medical_data: str):
    """Create patient record with PHI protection"""
    record = PatientRecord(
        patient_id=phi_protection.hash_identifier(ssn),
        encrypted_ssn=phi_protection.encrypt_phi(ssn),
        encrypted_medical_record=phi_protection.encrypt_phi(medical_data),
        created_at=datetime.utcnow()
    )

    await save_patient_record(record)
    return {"patient_id": record.patient_id}

# Audit logging for HIPAA
@app.middleware("http")
async def hipaa_audit_log(request: Request, call_next):
    """Log all PHI access for HIPAA compliance"""
    user = get_current_user(request)

    await log_phi_access({
        "user_id": user.id,
        "endpoint": request.url.path,
        "timestamp": datetime.utcnow(),
        "action": request.method
    })

    return await call_next(request)
```

## PCI-DSS Compliance

### Payment Data Security

```python
# Example 3: PCI-DSS compliance
from fastapi import FastAPI
from pydantic import BaseModel, Field

app = FastAPI()

class PaymentCard(BaseModel):
    # Never store full card number
    last_four: str = Field(..., pattern=r'^\d{4}$')
    card_type: str
    expiry_month: int = Field(..., ge=1, le=12)
    expiry_year: int = Field(..., ge=2024)

class PaymentRequest(BaseModel):
    amount: float = Field(..., gt=0)
    currency: str = "USD"
    # Token from payment processor, not actual card
    payment_token: str

@app.post("/payments/")
async def process_payment(payment: PaymentRequest):
    """Process payment via payment processor"""
    # Never handle raw card data
    # Use payment processor tokenization
    result = await payment_processor.charge(
        token=payment.payment_token,
        amount=payment.amount,
        currency=payment.currency
    )

    return {
        "transaction_id": result.id,
        "status": result.status
    }

# Mask sensitive data in logs
@app.middleware("http")
async def mask_sensitive_data(request: Request, call_next):
    """Mask sensitive data in logs"""
    response = await call_next(request)

    # Don't log payment endpoints
    if "/payment" in request.url.path:
        return response

    return response
```

## Summary

Regulatory compliance is essential for handling sensitive data.

## Next Steps

Continue learning about:
- [Security Frameworks](./09_security_frameworks.md)
- [Security Certifications](./10_security_certifications.md)
