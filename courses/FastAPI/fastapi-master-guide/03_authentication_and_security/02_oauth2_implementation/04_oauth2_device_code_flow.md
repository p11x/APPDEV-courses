# OAuth2 Device Code Flow

## Overview

Device Code flow is designed for devices with limited input capabilities (smart TVs, CLI tools, IoT devices). Users authenticate on a separate device.

## Implementation

```python
# Example 1: Device Code flow
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import secrets
from datetime import datetime, timedelta

app = FastAPI()

# Storage for device codes
device_codes: dict = {}

class DeviceCodeResponse(BaseModel):
    device_code: str
    user_code: str
    verification_uri: str
    expires_in: int
    interval: int

@app.post("/device/code", response_model=DeviceCodeResponse)
async def request_device_code(client_id: str):
    """Request device code for authentication"""
    device_code = secrets.token_urlsafe(32)
    user_code = secrets.token_urlsafe(8).upper()

    device_codes[device_code] = {
        "user_code": user_code,
        "client_id": client_id,
        "status": "pending",
        "created_at": datetime.utcnow(),
        "expires_at": datetime.utcnow() + timedelta(minutes=10)
    }

    return DeviceCodeResponse(
        device_code=device_code,
        user_code=user_code,
        verification_uri="https://example.com/device",
        expires_in=600,
        interval=5
    )

@app.post("/device/token")
async def poll_token(device_code: str):
    """Poll for token after user authorizes"""
    if device_code not in device_codes:
        raise HTTPException(400, "Invalid device code")

    device = device_codes[device_code]

    if datetime.utcnow() > device["expires_at"]:
        del device_codes[device_code]
        raise HTTPException(400, "Device code expired")

    if device["status"] == "pending":
        raise HTTPException(400, "Authorization pending")

    if device["status"] == "approved":
        token = create_token(device["client_id"])
        del device_codes[device_code]
        return {"access_token": token, "token_type": "bearer"}

    raise HTTPException(400, "Authorization denied")
```

## Summary

Device Code flow enables authentication on devices with limited input capabilities.

## Next Steps

Continue learning about:
- [Server Configuration](./05_oauth2_server_config.md)
- [Token Validation](./06_oauth2_token_validation.md)
