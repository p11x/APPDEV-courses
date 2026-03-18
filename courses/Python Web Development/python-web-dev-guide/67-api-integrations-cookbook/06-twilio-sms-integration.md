# Twilio SMS Integration

## What You'll Learn

- How to send SMS messages using Twilio
- How to receive incoming SMS messages
- How to implement two-factor authentication
- How to handle WhatsApp messages

## Prerequisites

- Completed `05-aws-s3-file-upload.md`
- A Twilio account

## Introduction

Twilio is a cloud communications platform that enables you to send SMS, make phone calls, and more. This guide covers SMS integration for notifications, verification, and two-factor authentication.

## Setting Up Twilio

Install the Twilio Python library:

```bash
pip install twilio
```

Configure your Twilio client:

```python
import os
from dataclasses import dataclass
from typing import Optional
from twilio.rest import Client
from twilio.base.exceptions import TwilioRestException


@dataclass
class TwilioConfig:
    """Configuration for Twilio access."""
    account_sid: str
    auth_token: str
    from_number: str  # Your Twilio phone number


class TwilioClient:
    """Client for sending SMS via Twilio."""
    
    def __init__(self, config: TwilioConfig) -> None:
        self.config = config
        self.client = Client(config.account_sid, config.auth_token)
    
    def send_sms(
        self,
        to_number: str,
        message: str,
    ) -> dict:
        """Send an SMS message."""
        # Ensure number is in E.164 format
        to_number = self._format_number(to_number)
        
        twilio_message = self.client.messages.create(
            body=message,
            from_=self.config.from_number,
            to=to_number,
        )
        
        return {
            "sid": twilio_message.sid,
            "status": twilio_message.status,
            "to": twilio_message.to,
            "from": twilio_message.from_,
        }
    
    def send_verification_code(
        self,
        to_number: str,
        code: str,
        service_sid: Optional[str] = None,
    ) -> dict:
        """Send a verification code via SMS."""
        if service_sid:
            # Use Twilio Verify API
            verification = self.client.verify.v2.services(
                service_sid
            ).verifications.create(
                to=self._format_number(to_number),
                channel="sms",
            )
            return {
                "status": verification.status,
                "to": verification.to,
            }
        else:
            # Send code manually
            return self.send_sms(
                to_number,
                f"Your verification code is: {code}. Valid for 10 minutes.",
            )
    
    def check_verification_code(
        self,
        to_number: str,
        code: str,
        service_sid: str,
    ) -> bool:
        """Verify a code using Twilio Verify API."""
        verification_check = self.client.verify.v2.services(
            service_sid
        ).verification_checks.create(
            to=self._format_number(to_number),
            code=code,
        )
        return verification_check.status == "approved"
    
    def _format_number(self, number: str) -> str:
        """Format phone number to E.164 format."""
        # Remove all non-digit characters
        digits = "".join(c for c in number if c.isdigit())
        
        # Add country code if not present (assuming US)
        if len(digits) == 10:
            digits = "1" + digits
        
        return f"+{digits}"


# Example usage
def main() -> None:
    config = TwilioConfig(
        account_sid=os.environ["TWILIO_ACCOUNT_SID"],
        auth_token=os.environ["TWILIO_AUTH_TOKEN"],
        from_number=os.environ["TWILIO_PHONE_NUMBER"],
    )
    
    client = TwilioClient(config)
    
    # Send an SMS
    result = client.send_sms(
        to_number="+1234567890",
        message="Hello from your app!",
    )
    print(f"SMS sent: {result['sid']}")
    
    # Send verification code
    result = client.send_verification_code(
        to_number="+1234567890",
        code="123456",
    )
    print(f"Verification sent: {result['status']}")


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `pip install twilio` — Installs the official Twilio Python SDK.
2. `from twilio.rest import Client` — The main Twilio client class.
3. `TwilioConfig` — Dataclass holding your Twilio credentials and phone number.
4. `self.client.messages.create()` — Creates and sends an SMS message. Parameters are `body` (message text), `from_` (your Twilio number), and `to` (recipient's number).
5. `twilio_message.sid` — The unique identifier for the message (starts with "SM").
6. `send_verification_code()` — Sends a verification code. Can use Twilio's Verify API or manual SMS.
7. `check_verification_code()` — Verifies a code entered by the user against Twilio's Verify API.
8. `_format_number()` — Ensures phone numbers are in E.164 format (+[country][number]).

## Two-Factor Authentication Implementation

Here's a complete 2FA implementation:

```python
import random
import string
from datetime import datetime, timedelta
from typing import Optional
from fastapi import FastAPI, HTTPException, Request
from pydantic import BaseModel
from twilio.rest import Client


app = FastAPI()

# Twilio setup
twilio_client = Client(
    os.environ["TWILIO_ACCOUNT_SID"],
    os.environ["TWILIO_AUTH_TOKEN"],
)
VERIFY_SERVICE_SID = os.environ["TWILIO_VERIFY_SERVICE_SID"]


class SendCodeRequest(BaseModel):
    """Request to send verification code."""
    phone_number: str


class VerifyCodeRequest(BaseModel):
    """Request to verify code."""
    phone_number: str
    code: str


# In-memory store for demo (use Redis in production)
verification_codes: dict[str, tuple[str, datetime]] = {}


def generate_code(length: int = 6) -> str:
    """Generate a random numeric code."""
    return "".join(random.choices(string.digits, k=length))


@app.post("/api/auth/send-code")
async def send_verification_code(request: SendCodeRequest) -> dict:
    """Send a verification code to the user's phone."""
    
    # Format and validate phone number
    phone = request.phone_number
    if not phone.startswith("+"):
        raise HTTPException(
            status_code=400,
            detail="Phone number must be in E.164 format (e.g., +1234567890)"
        )
    
    # Use Twilio Verify API for production
    try:
        verification = twilio_client.verify.v2.services(
            VERIFY_SERVICE_SID
        ).verifications.create(
            to=phone,
            channel="sms",
        )
        
        return {
            "success": True,
            "status": verification.status,
            "message": "Verification code sent",
        }
    except Exception as e:
        raise HTTPException(
            status_code=500,
            detail=f"Failed to send code: {str(e)}"
        )


@app.post("/api/auth/verify-code")
async def verify_code(request: VerifyCodeRequest) -> dict:
    """Verify the code entered by the user."""
    
    try:
        verification_check = twilio_client.verify.v2.services(
            VERIFY_SERVICE_SID
        ).verification_checks.create(
            to=request.phone_number,
            code=request.code,
        )
        
        if verification_check.status == "approved":
            return {
                "success": True,
                "verified": True,
                "message": "Phone number verified successfully",
            }
        else:
            return {
                "success": True,
                "verified": False,
                "message": "Invalid or expired code",
            }
    except Exception as e:
        raise HTTPException(
            status_code=500,
            detail=f"Verification failed: {str(e)}"
        )


# Alternative: Manual code verification (without Verify API)
@app.post("/api/auth/send-code-manual")
async def send_code_manual(request: SendCodeRequest) -> dict:
    """Send code using manual implementation."""
    phone = request.phone_number
    
    # Generate code
    code = generate_code()
    
    # Store code with expiry (10 minutes)
    expiry = datetime.now() + timedelta(minutes=10)
    verification_codes[phone] = (code, expiry)
    
    # Send SMS
    twilio_client.messages.create(
        body=f"Your verification code is: {code}. Valid for 10 minutes.",
        from_=os.environ["TWILIO_PHONE_NUMBER"],
        to=phone,
    )
    
    return {
        "success": True,
        "message": "Verification code sent",
    }


@app.post("/api/auth/verify-code-manual")
async def verify_code_manual(request: VerifyCodeRequest) -> dict:
    """Verify code using manual implementation."""
    phone = request.phone_number
    
    if phone not in verification_codes:
        return {
            "success": True,
            "verified": False,
            "message": "No code found. Request a new code.",
        }
    
    code, expiry = verification_codes[phone]
    
    # Check if expired
    if datetime.now() > expiry:
        del verification_codes[phone]
        return {
            "success": True,
            "verified": False,
            "message": "Code expired. Request a new code.",
        }
    
    # Verify code
    if request.code == code:
        del verification_codes[phone]  # Remove after successful verification
        return {
            "success": True,
            "verified": True,
            "message": "Phone number verified",
        }
    
    return {
        "success": True,
        "verified": False,
        "message": "Invalid code",
    }
```

## WhatsApp Integration

Twilio also supports WhatsApp:

```python
def send_whatsapp(
    self,
    to_number: str,
    message: str,
) -> dict:
    """Send a WhatsApp message."""
    # Format number for WhatsApp
    to_whatsapp = f"whatsapp:{self._format_number(to_number)}"
    from_whatsapp = f"whatsapp:{self.config.from_number}"
    
    twilio_message = self.client.messages.create(
        body=message,
        from_=from_whatsapp,
        to=to_whatsapp,
    )
    
    return {
        "sid": twilio_message.sid,
        "status": twilio_message.status,
    }


def send_whatsapp_template(
    self,
    to_number: str,
    template_name: str,
    parameters: list[str],
) -> dict:
    """Send a WhatsApp template message."""
    # Format components for template
    components = [
        {
            "type": "body",
            "parameters": [
                {"type": "text", "parameter": param}
                for param in parameters
            ]
        }
    ]
    
    twilio_message = self.client.messages.create(
        from_=f"whatsapp:{self.config.from_number}",
        to=f"whatsapp:{self._format_number(to_number)}",
        content_sid="HX...",
        content_variables='{"1":"value1"}',
    )
    
    return {"sid": twilio_message.sid}
```

## Receiving Incoming Messages

Set up a webhook to receive SMS:

```python
from fastapi import Request
from fastapi.responses import PlainTextResponse


@app.post("/api/webhooks/sms")
async def receive_sms(request: Request) -> PlainTextResponse:
    """Handle incoming SMS messages."""
    form = await request.form()
    
    from_number = form.get("From")
    message_body = form.get("Body")
    message_sid = form.get("MessageSid")
    
    print(f"Received SMS from {from_number}: {message_body}")
    
    # Process the message
    response_message = process_incoming_sms(message_body)
    
    # Send a reply
    if response_message:
        twilio_client.messages.create(
            body=response_message,
            from_=os.environ["TWILIO_PHONE_NUMBER"],
            to=from_number,
        )
    
    # Return empty TwiML to acknowledge
    return PlainTextResponse("")


def process_incoming_sms(body: str) -> Optional[str]:
    """Process incoming SMS and return response."""
    body = body.strip().upper()
    
    match body:
        case "STOP":
            return "You have been unsubscribed."
        case "HELP":
            return "Reply with STOP to unsubscribe."
        case "STATUS":
            return "Your account is active."
        case _:
            return "Thank you for your message. Reply HELP for assistance."
```

## Message Status Callbacks

Track message delivery status:

```python
@app.post("/api/webhooks/message-status")
async def message_status_webhook(request: Request) -> PlainTextResponse:
    """Handle message status callbacks."""
    form = await request.form()
    
    message_sid = form.get("MessageSid")
    message_status = form.get("MessageStatus")
    error_code = form.get("ErrorCode")
    error_message = form.get("ErrorMessage")
    
    # Log or store the status
    print(f"Message {message_sid} status: {message_status}")
    
    if error_code:
        print(f"Error {error_code}: {error_message}")
    
    return PlainTextResponse("")


def get_message_status(self, message_sid: str) -> dict:
    """Get the status of a sent message."""
    message = self.client.messages(message_sid).fetch()
    
    return {
        "sid": message.sid,
        "status": message.status,
        "error_code": message.error_code,
        "error_message": message.error_message,
    }
```

## Summary

- Twilio provides SMS, voice, and WhatsApp APIs
- Use the Python SDK for easy integration
- Format phone numbers to E.164 format (+[country][number])
- Use Twilio Verify API for 2FA — it handles rate limiting and spam protection
- Set up webhooks to receive incoming messages
- Track delivery status through status callbacks

## Next Steps

→ Continue to `07-discord-webhook-integration.md` to learn about Discord webhooks.
