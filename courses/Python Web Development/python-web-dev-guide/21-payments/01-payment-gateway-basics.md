# Payment Gateway Basics

## What You'll Learn
- Understanding payment gateways
- Stripe integration
- PayPal integration

## Prerequisites
- Completed email folder

## Popular Payment Gateways

| Gateway | Features | Fees |
|---------|----------|------|
| Stripe | API-first, extensive features | 2.9% + 30¢ |
| PayPal | Wide reach, trusted | 2.9% + 30¢ |
| Square | In-person + online | 2.6% + 10¢ |
| Braintree | PayPal integration | 2.9% + 30¢ |

## Stripe Setup

```bash
pip install stripe
```

```python
import stripe
from fastapi import FastAPI

app = FastAPI()
stripe.api_key = "sk_test_..."

@app.post("/create-payment-intent")
async def create_payment_intent(amount: int, currency: str = "usd"):
    """Create a payment intent"""
    intent = stripe.PaymentIntent.create(
        amount=amount,  # Amount in cents
        currency=currency,
        automatic_payment_methods={"enabled": True}
    )
    
    return {
        "client_secret": intent.client_secret
    }
```

## Client-Side Integration

```javascript
// Frontend code
const stripe = await loadStripe('pk_test_...');

const {error, paymentIntent} = await stripe.confirmCardPayment(
    clientSecret,
    {
        payment_method: {
            card: cardElement
        }
    }
);
```

## Summary
- Use Stripe for most web apps
- Always use test mode in development
- Never log or store card details

## Next Steps
→ Continue to `02-stripe-integration.md`
