# Multi-Factor Authentication

## Overview

Multi-Factor Authentication (MFA) adds extra security layers beyond passwords. This guide covers implementing TOTP-based MFA in FastAPI.

## TOTP Implementation

### Time-Based One-Time Password

```python
# Example 1: TOTP implementation
from fastapi import FastAPI, HTTPException, Depends
import pyotp
import qrcode
from io import BytesIO
import base64

app = FastAPI()

class MFAManager:
    """Manages MFA setup and verification"""

    def generate_secret(self) -> str:
        """Generate new TOTP secret"""
        return pyotp.random_base32()

    def get_totp(self, secret: str) -> pyotp.TOTP:
        """Get TOTP instance"""
        return pyotp.TOTP(secret)

    def verify_token(self, secret: str, token: str) -> bool:
        """Verify TOTP token"""
        totp = self.get_totp(secret)
        return totp.verify(token)

    def get_provisioning_uri(self, secret: str, username: str) -> str:
        """Get QR code URI for authenticator apps"""
        totp = self.get_totp(secret)
        return totp.provisioning_uri(
            name=username,
            issuer_name="FastAPI App"
        )

    def generate_qr_code(self, uri: str) -> str:
        """Generate QR code as base64 image"""
        qr = qrcode.QRCode(version=1, box_size=10, border=5)
        qr.add_data(uri)
        qr.make(fit=True)

        img = qr.make_image(fill_color="black", back_color="white")

        buffer = BytesIO()
        img.save(buffer, format="PNG")
        return base64.b64encode(buffer.getvalue()).decode()

mfa = MFAManager()

# User MFA storage (use database in production)
user_mfa: dict[int, dict] = {}

@app.post("/mfa/setup")
async def setup_mfa(user: dict = Depends(get_current_user)):
    """
    Setup MFA for user.
    Returns QR code for authenticator app.
    """
    secret = mfa.generate_secret()

    # Store secret temporarily (confirm before activating)
    user_mfa[user["id"]] = {
        "secret": secret,
        "confirmed": False
    }

    # Generate QR code
    uri = mfa.get_provisioning_uri(secret, user["username"])
    qr_code = mfa.generate_qr_code(uri)

    return {
        "secret": secret,
        "qr_code": f"data:image/png;base64,{qr_code}",
        "message": "Scan QR code with authenticator app"
    }

@app.post("/mfa/confirm")
async def confirm_mfa(token: str, user: dict = Depends(get_current_user)):
    """
    Confirm MFA setup by verifying first token.
    """
    if user["id"] not in user_mfa:
        raise HTTPException(400, "MFA not initiated")

    mfa_data = user_mfa[user["id"]]

    if not mfa.verify_token(mfa_data["secret"], token):
        raise HTTPException(400, "Invalid token")

    # Mark MFA as confirmed
    mfa_data["confirmed"] = True

    return {"message": "MFA enabled successfully"}

@app.post("/mfa/verify")
async def verify_mfa(token: str, user: dict = Depends(get_current_user)):
    """
    Verify MFA token during login.
    """
    if user["id"] not in user_mfa:
        raise HTTPException(400, "MFA not enabled")

    mfa_data = user_mfa[user["id"]]

    if not mfa_data.get("confirmed"):
        raise HTTPException(400, "MFA not confirmed")

    if not mfa.verify_token(mfa_data["secret"], token):
        raise HTTPException(401, "Invalid MFA token")

    return {"verified": True}
```

## Login Flow with MFA

### Two-Step Login

```python
# Example 2: Login with MFA
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel

app = FastAPI()

class LoginRequest(BaseModel):
    username: str
    password: str

class MFARequest(BaseModel):
    login_token: str  # Temporary token from step 1
    mfa_token: str    # 6-digit code from authenticator

@app.post("/login")
async def login(request: LoginRequest):
    """
    Step 1: Verify credentials.
    Returns temporary token if MFA required.
    """
    # Verify username/password
    user = authenticate_user(request.username, request.password)
    if not user:
        raise HTTPException(401, "Invalid credentials")

    # Check if MFA is enabled
    if user_has_mfa(user["id"]):
        # Create temporary token (expires in 5 minutes)
        temp_token = create_temp_token(user["id"])

        return {
            "requires_mfa": True,
            "login_token": temp_token,
            "message": "Enter MFA code"
        }

    # No MFA - return access token directly
    access_token = create_access_token(user["id"])
    return {"access_token": access_token, "token_type": "bearer"}

@app.post("/login/verify-mfa")
async def verify_mfa_login(request: MFARequest):
    """
    Step 2: Verify MFA token.
    Returns access token.
    """
    # Verify temporary token
    user_id = verify_temp_token(request.login_token)
    if not user_id:
        raise HTTPException(401, "Invalid or expired login token")

    # Verify MFA token
    if not verify_user_mfa(user_id, request.mfa_token):
        raise HTTPException(401, "Invalid MFA token")

    # Create access token
    access_token = create_access_token(user_id)

    return {"access_token": access_token, "token_type": "bearer"}
```

## Backup Codes

### Recovery Codes

```python
# Example 3: Backup codes for MFA
import secrets

class BackupCodeManager:
    """Manage MFA backup codes"""

    def generate_codes(self, count: int = 10) -> list[str]:
        """Generate backup codes"""
        return [secrets.token_hex(4).upper() for _ in range(count)]

    def verify_code(self, stored_codes: list[str], code: str) -> bool:
        """Verify and consume backup code"""
        code = code.upper().strip()

        if code in stored_codes:
            stored_codes.remove(code)  # Single use
            return True

        return False

backup_manager = BackupCodeManager()

@app.post("/mfa/backup-codes")
async def generate_backup_codes(user: dict = Depends(get_current_user)):
    """
    Generate backup codes for MFA recovery.
    """
    codes = backup_manager.generate_codes()

    # Store hashed codes in database
    store_backup_codes(user["id"], codes)

    return {
        "codes": codes,
        "message": "Save these codes securely. Each can be used once."
    }
```

## Best Practices

### MFA Security Guidelines

```python
# Example 4: MFA best practices
"""
MFA Best Practices:

1. Always offer MFA but don't force it (for most apps)
2. Provide backup codes for recovery
3. Allow multiple authenticator devices
4. Implement rate limiting on MFA attempts
5. Log MFA events for security audit
6. Use time-limited temporary tokens
7. Allow disabling MFA with proper verification
"""

from fastapi import FastAPI

app = FastAPI()

# Rate limiting for MFA attempts
mfa_attempts: dict[int, list[float]] = {}

def check_mfa_rate_limit(user_id: int) -> bool:
    """Check if user has exceeded MFA attempt limit"""
    import time

    now = time.time()
    attempts = mfa_attempts.get(user_id, [])

    # Remove old attempts (last 15 minutes)
    attempts = [t for t in attempts if now - t < 900]

    if len(attempts) >= 5:  # 5 attempts per 15 minutes
        return False

    attempts.append(now)
    mfa_attempts[user_id] = attempts

    return True
```

## Summary

| Feature | Implementation | Purpose |
|---------|----------------|---------|
| TOTP | `pyotp` library | Time-based codes |
| QR Code | `qrcode` library | Easy setup |
| Backup Codes | `secrets` module | Recovery option |
| Rate Limiting | Custom logic | Brute force protection |

## Next Steps

Continue learning about:
- [SSO Implementation](./06_sso_implementation.md) - Single sign-on
- [Authentication Testing](../07_security_testing/01_authentication_testing.md) - Testing MFA
- [Security Audit](../07_security_testing/06_security_audit_practices.md) - Audit practices
