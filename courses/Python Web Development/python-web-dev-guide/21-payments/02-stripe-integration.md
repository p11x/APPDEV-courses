# Stripe Integration

## What You'll Learn
- Payment intents
- Webhooks
- Customer management

## Prerequisites
- Completed payment gateway basics

## Complete Payment Flow

```python
import stripe
from fastapi import FastAPI, Request, WebSocket
from pydantic import BaseModel

app = FastAPI()
stripe.api_key = "sk_test_..."

class CreatePaymentRequest(BaseModel):
    amount: int
    currency: str = "usd"

@app.post("/create-payment-intent")
async def create_payment_intent(request: CreatePaymentRequest):
    """Create payment intent"""
    intent = stripe.PaymentIntent.create(
        amount=request.amount,
        currency=request.currency,
        automatic_payment_methods={"enabled": True}
    )
    
    return {"client_secret": intent.client_secret}

@app.post("/webhooks/stripe")
async def stripe_webhook(request: Request):
    """Handle Stripe webhooks"""
    payload = await request.body()
    sig_header = request.headers.get("stripe-signature")
    
    try:
        event = stripe.Webhook.construct_event(
            payload, sig_header, "whsec_..."
        )
    except ValueError:
        return {"error": "Invalid payload"}, 400
    except stripe.error.SignatureVerificationError:
        return {"error": "Invalid signature"}, 400
    
    # Handle events
    if event["type"] == "payment_intent.succeeded":
        payment_intent = event["data"]["object"]
        print(f"Payment succeeded: {payment_intent['id']}")
    
    elif event["type"] == "payment_intent.payment_failed":
        payment_intent = event["data"]["object"]
        print(f"Payment failed: {payment_intent['id']}")
    
    return {"status": "success"}
```

## Customer Management

```python
@app.post("/customers")
async def create_customer(email: str, name: str):
    """Create Stripe customer"""
    customer = stripe.Customer.create(
        email=email,
        name=name
    )
    return {"customer_id": customer.id}

@app.get("/customers/{customer_id}")
async def get_customer(customer_id: str):
    """Get customer details"""
    customer = stripe.Customer.retrieve(customer_id)
    return customer
```

## Summary
- Use PaymentIntent for secure payments
- Handle webhooks for payment events
- Store customer IDs for reuse

## Next Steps
→ Continue to `03-subscriptions.md`
