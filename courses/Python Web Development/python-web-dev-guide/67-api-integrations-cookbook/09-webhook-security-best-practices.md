# Webhook Security Best Practices

## What You'll Learn

- How to secure your webhook endpoints
- How to verify webhook signatures
- How to handle webhook payloads safely
- Best practices for API key management
- Rate limiting and abuse prevention

## Prerequisites

- Completed `08-slack-integration.md`
- Understanding of HTTP requests

## Introduction

Webhooks are a powerful way to receive data from external services, but they also present security risks. An attacker could send fake webhook requests to your server, impersonating a legitimate service. This guide covers essential security practices for webhook integration.

## Understanding Webhook Security Risks

The main security risks with webhooks are:

1. **Spoofing** — Attackers send fake requests pretending to be a legitimate service
2. **Replay Attacks** — Attackers replay a valid request multiple times
3. **Data Leaks** — Sensitive data in webhooks is intercepted
4. **Denial of Service** — Attackers overwhelm your webhook endpoint

## Signature Verification

The most important security measure is verifying webhook signatures. Each service signs its webhooks with a secret key.

### Stripe Signature Verification

```python
import os
import hmac
import hashlib
import time
from fastapi import FastAPI, Request, HTTPException, Header
from starlette.datastructures import Headers


app = FastAPI()

STRIPE_WEBHOOK_SECRET = os.environ["STRIPE_WEBHOOK_SECRET"]


def verify_stripe_signature(
    payload: bytes,
    signature: str,
    secret: str,
) -> bool:
    """Verify Stripe webhook signature."""
    
    # Compute expected signature
    expected_signature = hmac.new(
        secret.encode("utf-8"),
        payload,
        hashlib.sha256,
    ).hexdigest()
    
    # Compare signatures using constant-time comparison
    return hmac.compare_digest(f"sha256={expected_signature}", signature)


@app.post("/api/webhooks/stripe")
async def handle_stripe_webhook(
    request: Request,
    stripe_signature: str = Header(..., alias="stripe-signature"),
) -> dict:
    """Handle Stripe webhook with signature verification."""
    
    # Get raw body (needed for signature verification)
    body = await request.body()
    
    # Verify signature
    if not verify_stripe_signature(body, stripe_signature, STRIPE_WEBHOOK_SECRET):
        raise HTTPException(
            status_code=400,
            detail="Invalid signature"
        )
    
    # Parse the payload
    import json
    event = json.loads(body)
    
    # Process the event
    event_type = event.get("type")
    
    match event_type:
        case "payment_intent.succeeded":
            # Handle successful payment
            payment = event["data"]["object"]
            print(f"Payment succeeded: {payment['id']}")
            
        case "customer.subscription.created":
            # Handle new subscription
            subscription = event["data"]["object"]
            print(f"New subscription: {subscription['id']}")
            
        case _:
            print(f"Unhandled event type: {event_type}")
    
    return {"status": "success"}


# Alternative: Using stripe library
import stripe


@app.post("/api/webhooks/stripe-library")
async def handle_stripe_webhook_library(
    request: Request,
    stripe_signature: str = Header(..., alias="stripe-signature"),
) -> dict:
    """Handle Stripe webhook using the official library."""
    
    body = await request.body()
    
    try:
        event = stripe.Webhook.construct_event(
            body,
            stripe_signature,
            STRIPE_WEBHOOK_SECRET,
        )
    except ValueError:
        raise HTTPException(status_code=400, detail="Invalid payload")
    except stripe.error.SignatureVerificationError:
        raise HTTPException(status_code=400, detail="Invalid signature")
    
    # Process the event
    print(f"Received event: {event['type']}")
    
    return {"status": "success"}
```

🔍 **Line-by-Line Breakdown:**

1. `verify_stripe_signature()` — Manually verifies the signature by computing the expected HMAC-SHA256 and comparing it with the received signature.
2. `hmac.new(secret.encode(), payload, hashlib.sha256)` — Creates an HMAC signature using SHA256.
3. `hmac.compare_digest()` — Constant-time string comparison to prevent timing attacks. Never use `==` for comparing secrets.
4. `stripe.Webhook.construct_event()` — The official Stripe library handles signature verification for you.
5. `stripe_signature` header — Stripe sends the signature in this header.

### GitHub Webhook Verification

```python
import hmac
import hashlib
from fastapi import FastAPI, Request, HTTPException, Header


app = FastAPI()

GITHUB_WEBHOOK_SECRET = os.environ["GITHUB_WEBHOOK_SECRET"]


def verify_github_signature(
    payload: bytes,
    signature: str,
    secret: str,
) -> bool:
    """Verify GitHub webhook signature."""
    if not signature.startswith("sha256="):
        return False
    
    expected_signature = f"sha256={hmac.new(secret.encode(), payload, hashlib.sha256).hexdigest()}"
    return hmac.compare_digest(expected_signature, signature)


@app.post("/api/webhooks/github")
async def handle_github_webhook(
    request: Request,
    x_hub_signature_256: str = Header(..., alias="x-hub-signature-256"),
) -> dict:
    """Handle GitHub webhook with signature verification."""
    
    body = await request.body()
    
    if not verify_github_signature(body, x_hub_signature_256, GITHUB_WEBHOOK_SECRET):
        raise HTTPException(status_code=401, detail="Invalid signature")
    
    import json
    event = json.loads(body)
    
    # Get event type from header
    event_type = request.headers.get("x-github-event", "unknown")
    
    match event_type:
        case "push":
            # Handle push event
            repo = event.get("repository", {}).get("name")
            commits = event.get("commits", [])
            print(f"Push to {repo}: {len(commits)} commits")
            
        case "pull_request":
            # Handle PR event
            action = event.get("action")
            pr = event.get("pull_request", {})
            print(f"PR {action}: {pr.get('title')}")
            
        case "issues":
            # Handle issue event
            action = event.get("action")
            issue = event.get("issue", {})
            print(f"Issue {action}: {issue.get('title')}")
    
    return {"status": "received"}
```

### Slack Signature Verification

```python
import hmac
import hashlib
import time
from fastapi import FastAPI, Request, HTTPException, Header


app = FastAPI()

SLACK_SIGNING_SECRET = os.environ["SLACK_SIGNING_SECRET"]


def verify_slack_signature(
    body: bytes,
    timestamp: str,
    signature: str,
    secret: str,
) -> bool:
    """Verify Slack webhook signature."""
    
    # Reject requests older than 5 minutes (prevent replay attacks)
    current_time = int(time.time())
    request_time = int(timestamp)
    
    if abs(current_time - request_time) > 300:
        return False
    
    # Create base string
    base_string = f"v0:{timestamp}:{body.decode('utf-8')}"
    
    # Compute signature
    expected_signature = f"v0={hmac.new(secret.encode(), base_string.encode(), hashlib.sha256).hexdigest()}"
    
    return hmac.compare_digest(expected_signature, signature)


@app.post("/api/webhooks/slack")
async def handle_slack_webhook(
    request: Request,
    x_slack_signature: str = Header(..., alias="x-slack-signature"),
    x_slack_request_timestamp: str = Header(..., alias="x-slack-request-timestamp"),
) -> dict:
    """Handle Slack webhook with signature verification."""
    
    body = await request.body()
    body_str = body.decode("utf-8")
    
    if not verify_slack_signature(
        body_str.encode(),
        x_slack_request_timestamp,
        x_slack_signature,
        SLACK_SIGNING_SECRET,
    ):
        raise HTTPException(status_code=401, detail="Invalid signature")
    
    # Process the request
    form = await request.form()
    # ... handle the webhook
    
    return {"status": "ok"}
```

## Secure API Key Management

### Environment Variables

Never hardcode API keys:

```python
# ❌ BAD - Hardcoded key
API_KEY = "sk_live_1234567890"

# ✅ GOOD - Environment variable
import os
API_KEY = os.environ.get("STRIPE_API_KEY")

# ✅ BETTER - With validation
API_KEY = os.environ.get("STRIPE_API_KEY")
if not API_KEY:
    raise ValueError("STRIPE_API_KEY environment variable not set")
```

### Secrets Management Services

For production, use a secrets manager:

```python
# Using AWS Secrets Manager
import boto3
import json


def get_secret(secret_name: str) -> dict:
    """Retrieve secret from AWS Secrets Manager."""
    client = boto3.client("secretsmanager")
    
    response = client.get_secret_value(SecretId=secret_name)
    return json.loads(response["SecretString"])


# Using HashiCorp Vault
import hvac


def get_secret_from_vault(path: str) -> dict:
    """Retrieve secret from HashiCorp Vault."""
    client = hvac.Client()
    return client.secrets.kv.v2.read_secret_version(path=path)["data"]["data"]
```

### Pydantic Settings

Use Pydantic for secure settings:

```python
from pydantic_settings import BaseSettings
from typing import Optional


class Settings(BaseSettings):
    """Application settings with secure defaults."""
    
    # Stripe
    stripe_api_key: str
    stripe_webhook_secret: str
    
    # GitHub
    github_webhook_secret: str
    
    # Slack
    slack_signing_secret: str
    
    # Database
    database_url: str
    
    class Config:
        env_file = ".env"
        case_sensitive = False
        
        # Fields that should not be logged
        sensitive_fields = {"stripe_api_key", "database_url"}


# Usage
settings = Settings()
```

## Rate Limiting

Protect your webhooks from abuse:

```python
from fastapi import FastAPI, HTTPException, Request
from fastapi.security import APIKeyHeader
from collections import defaultdict
from datetime import datetime, timedelta
import asyncio


app = FastAPI()

# Simple in-memory rate limiter (use Redis in production)
request_counts: dict[str, list[datetime]] = defaultdict(list)


async def check_rate_limit(
    client_id: str,
    max_requests: int = 100,
    window_seconds: int = 60,
) -> bool:
    """Check if client has exceeded rate limit."""
    
    now = datetime.now()
    cutoff = now - timedelta(seconds=window_seconds)
    
    # Clean old requests
    request_counts[client_id] = [
        ts for ts in request_counts[client_id]
        if ts > cutoff
    ]
    
    # Check limit
    if len(request_counts[client_id]) >= max_requests:
        return False
    
    # Record this request
    request_counts[client_id].append(now)
    return True


@app.middleware("http")
async def rate_limit_middleware(request: Request, call_next):
    """Apply rate limiting to webhook endpoints."""
    
    if "/webhook/" in request.url.path:
        client_id = request.client.host  # Or use API key
        
        if not await check_rate_limit(client_id):
            raise HTTPException(
                status_code=429,
                detail="Too many requests"
            )
    
    return await call_next(request)
```

## Payload Validation

Always validate webhook payloads:

```python
from pydantic import BaseModel, ValidationError, validator
from typing import Optional
from enum import Enum


class StripeEventType(str, Enum):
    """Valid Stripe event types."""
    PAYMENT_INTENT_SUCCEEDED = "payment_intent.succeeded"
    PAYMENT_INTENT_FAILED = "payment_intent.failed"
    CUSTOMER_SUBSCRIPTION_CREATED = "customer.subscription.created"
    CUSTOMER_SUBSCRIPTION_DELETED = "customer.subscription.deleted"


class StripePaymentIntent(BaseModel):
    """Stripe payment intent model."""
    id: str
    amount: int
    currency: str
    status: str
    customer: Optional[str] = None
    
    @validator("amount")
    def amount_must_be_positive(cls, v):
        if v <= 0:
            raise ValueError("Amount must be positive")
        return v


class StripeWebhookEvent(BaseModel):
    """Validated Stripe webhook event."""
    id: str
    type: StripeEventType
    data: dict  # Will be validated as needed
    
    @validator("id")
    def id_must_start_with_evt(cls, v):
        if not v.startswith("evt_"):
            raise ValueError("Invalid event ID")
        return v


def validate_stripe_event(event: dict) -> StripeWebhookEvent:
    """Validate and parse Stripe webhook event."""
    try:
        return StripeWebhookEvent(**event)
    except ValidationError as e:
        raise ValueError(f"Invalid event: {e}")
```

## Summary

- **Always verify webhook signatures** — This is the most critical security measure
- **Use HMAC with constant-time comparison** — Prevents timing attacks
- **Implement replay protection** — Reject old requests (e.g., older than 5 minutes)
- **Validate all payloads** — Use Pydantic models to ensure data integrity
- **Store secrets securely** — Use environment variables or secrets managers
- **Implement rate limiting** — Prevent abuse and denial of service
- **Log all webhook activity** — Helps with debugging and security audits

## Next Steps

→ Continue to `10-api-integration-checklist.md` for a comprehensive checklist of API integrations.
