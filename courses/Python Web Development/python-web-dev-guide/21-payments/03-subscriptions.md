# Subscriptions

## What You'll Learn
- Recurring billing
- Subscription management
- Webhook handling

## Prerequisites
- Completed Stripe integration

## Create Subscription

```python
import stripe
from fastapi import FastAPI

app = FastAPI()
stripe.api_key = "sk_test_..."

# Create product and price first in Stripe dashboard
PRICE_ID = "price_..."

@app.post("/subscriptions")
async def create_subscription(customer_id: str):
    """Create a subscription"""
    subscription = stripe.Subscription.create(
        customer=customer_id,
        items=[{"price": PRICE_ID}],
        payment_behavior="default_incomplete",
        expand=["latest_invoice.payment_intent"]
    )
    
    return {
        "subscription_id": subscription.id,
        "client_secret": subscription.latest_invoice.payment_intent.client_secret
    }

@app.get("/subscriptions/{subscription_id}")
async def get_subscription(subscription_id: str):
    """Get subscription details"""
    subscription = stripe.Subscription.retrieve(subscription_id)
    return {
        "status": subscription.status,
        "current_period_end": subscription.current_period_end
    }

@app.post("/subscriptions/{subscription_id}/cancel")
async def cancel_subscription(subscription_id: str):
    """Cancel subscription"""
    subscription = stripe.Subscription.delete(subscription_id)
    return {"status": subscription.status}
```

## Webhook Handling

```python
@app.post("/webhooks/stripe")
async def handle_subscription_webhook(request: Request):
    """Handle subscription events"""
    payload = await request.body()
    event = stripe.Webhook.construct_event(
        payload,
        request.headers.get("stripe-signature"),
        "whsec_..."
    )
    
    if event["type"] == "customer.subscription.created":
        subscription = event["data"]["object"]
        # Update user to active
    
    elif event["type"] == "customer.subscription.updated":
        subscription = event["data"]["object"]
        # Update subscription status
    
    elif event["type"] == "customer.subscription.deleted":
        subscription = event["data"]["object"]
        # Downgrade user to free
    
    elif event["type"] == "invoice.payment_succeeded":
        invoice = event["data"]["object"]
        # Extend subscription
    
    return {"status": "success"}
```

## Summary
- Use Stripe Billing for subscriptions
- Handle subscription webhooks
- Track subscription status

## Next Steps
→ Continue to `04-payment-security.md`
