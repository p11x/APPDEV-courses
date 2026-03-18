# Payment Security

## What You'll Learn
- PCI compliance
- Secure handling
- Fraud prevention

## Prerequisites
- Completed subscriptions

## PCI Compliance Levels

| Level | Annual Transactions | Requirements |
|-------|---------------------|--------------|
| 1 | 6M+ | Annual audit, network scanning |
| 2-4 | <6M | Self-assessment questionnaire |

## Security Best Practices

```python
# DON'T: Never handle raw card data
# BAD:
def process_payment(card_number, cvc, expiry):
    # Never store or process card numbers yourself!
    pass

# DO: Use Stripe Elements
# Frontend handles card securely, sends token to backend

@app.post("/process-payment")
async def process_payment(payment_method_id: str, amount: int):
    """Process payment using payment method ID"""
    # Stripe handles card security
    intent = stripe.PaymentIntent.create(
        payment_method=payment_method_id,
        amount=amount,
        currency="usd",
        confirm=True,
        return_url="https://example.com/success"
    )
    return {"status": intent.status}

# Always verify webhook signatures
def verify_webhook(payload: bytes, signature: str) -> bool:
    """Verify Stripe webhook signature"""
    try:
        stripe.Webhook.construct_event(
            payload, signature, "whsec_..."
        )
        return True
    except:
        return False
```

## Fraud Prevention

```python
# Use Stripe Radar
@app.post("/create-payment-intent")
async def create_intent(amount: int):
    intent = stripe.PaymentIntent.create(
        amount=amount,
        currency="usd",
        automatic_payment_methods={"enabled": True},
        # Enable Radar
        payment_method_options={
            "card": {
                "request_three_d_secure": "automatic"
            }
        }
    )
    return {"client_secret": intent.client_secret}
```

## Summary
- Never handle raw card data
- Use Stripe Elements or Checkout
- Verify webhook signatures
- Enable 3D Secure

## Next Steps
→ Move to `22-search/`
