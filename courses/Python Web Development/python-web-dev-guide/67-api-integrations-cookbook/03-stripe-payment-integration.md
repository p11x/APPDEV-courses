# Stripe Payment Integration

## What You'll Learn

- How to set up Stripe for payments
- How to create payment intents
- How to handle webhooks for payment events
- How to implement subscriptions
- PCI compliance best practices

## Prerequisites

- Completed `02-github-api-integration.md`
- A Stripe account (free at stripe.com)

## Introduction

Stripe is the most popular payment processor for web applications. It provides a complete suite of APIs for handling payments, subscriptions, and financial reporting. This guide covers the essential integrations you'll need for most web applications.

## Setting Up Stripe

First, install the Stripe Python library:

```bash
pip install stripe
```

Then, configure your Stripe client:

```python
import os
import stripe
from dataclasses import dataclass
from typing import Optional
from datetime import datetime


@dataclass
class StripeConfig:
    """Configuration for Stripe API access."""
    secret_key: str
    publishable_key: str
    webhook_secret: str


class StripeClient:
    """Client for Stripe payment operations."""
    
    def __init__(self, config: StripeConfig) -> None:
        stripe.api_key = config.secret_key
        self.config = config
        self.publishable_key = config.publishable_key
    
    def create_customer(
        self,
        email: str,
        name: Optional[str] = None,
        metadata: Optional[dict] = None,
    ) -> stripe.Customer:
        """Create a new customer."""
        customer = stripe.Customer.create(
            email=email,
            name=name,
            metadata=metadata or {},
        )
        return customer
    
    def create_payment_intent(
        self,
        amount: int,  # Amount in cents
        currency: str = "usd",
        customer_id: Optional[str] = None,
        metadata: Optional[dict] = None,
    ) -> stripe.PaymentIntent:
        """Create a payment intent for checkout."""
        intent = stripe.PaymentIntent.create(
            amount=amount,
            currency=currency,
            customer=customer_id,
            metadata=metadata or {},
            automatic_payment_methods={"enabled": True},
        )
        return intent
    
    def retrieve_payment_intent(self, intent_id: str) -> stripe.PaymentIntent:
        """Retrieve a payment intent by ID."""
        return stripe.PaymentIntent.retrieve(intent_id)
    
    def create_refund(
        self,
        payment_intent_id: str,
        amount: Optional[int] = None,
    ) -> stripe.Refund:
        """Create a refund for a payment."""
        refund = stripe.Refund.create(
            payment_intent=payment_intent_id,
            amount=amount,  # Optional: partial refund amount in cents
        )
        return refund


# Example usage
def main() -> None:
    config = StripeConfig(
        secret_key=os.environ["STRIPE_SECRET_KEY"],
        publishable_key=os.environ["STRIPE_PUBLISHABLE_KEY"],
        webhook_secret=os.environ["STRIPE_WEBHOOK_SECRET"],
    )
    
    client = StripeClient(config)
    
    # Create a customer
    customer = client.create_customer(
        email="customer@example.com",
        name="John Doe",
        metadata={"user_id": "12345"},
    )
    print(f"Created customer: {customer.id}")
    
    # Create a payment intent for $29.99
    payment_intent = client.create_payment_intent(
        amount=2999,  # $29.99 in cents
        currency="usd",
        customer_id=customer.id,
        metadata={"order_id": "ORD-12345"},
    )
    print(f"Created payment intent: {payment_intent.client_secret}")
    print(f"Publishable key: {client.publishable_key}")


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `import stripe` — Official Stripe Python library.
2. `stripe.api_key = config.secret_key` — Set the API key globally for all Stripe operations.
3. `create_customer()` — Creates a customer record in Stripe. Customers allow you to track payments, issue refunds, and set up subscriptions. Returns a Stripe Customer object.
4. `create_payment_intent()` — Creates a payment intent, which represents the intent to collect payment. The `amount` is in the smallest currency unit (cents for USD). `automatic_payment_methods` enables Stripe's intelligent payment method selection.
5. `stripe.PaymentIntent` — The core object representing a payment. Contains `client_secret` which you'll need on the frontend.
6. `retrieve_payment_intent()` — Fetches the current state of a payment intent (useful for verifying payment status).
7. `create_refund()` — Issues a refund. Can be full (no amount) or partial (specify amount in cents).

## Frontend Integration with Stripe Elements

Stripe provides client-side libraries for securely collecting card information:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Stripe Checkout</title>
    <script src="https://js.stripe.com/v3/"></script>
    <style>
        .payment-form {
            max-width: 400px;
            margin: 50px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 8px;
        }
        #payment-element {
            margin-bottom: 20px;
        }
        button {
            background: #5469d4;
            color: white;
            border: none;
            padding: 12px 20px;
            border-radius: 4px;
            cursor: pointer;
            width: 100%;
        }
        button:disabled {
            background: #c0c5ea;
            cursor: not-allowed;
        }
    </style>
</head>
<body>
    <div class="payment-form">
        <h2>Complete Your Payment</h2>
        <div id="payment-element">
            <!-- Stripe Elements will insert the card form here -->
        </div>
        <button id="submit-button">Pay $29.99</button>
        <div id="error-message"></div>
    </div>

    <script>
        const stripe = Stripe('{{ publishable_key }}');
        const clientSecret = '{{ client_secret }}';

        // Initialize Stripe Elements
        const elements = stripe.elements({
            clientSecret: clientSecret,
            appearance: {
                theme: 'stripe',
            },
        });

        const paymentElement = elements.create('payment');
        paymentElement.mount('#payment-element');

        // Handle form submission
        const submitButton = document.getElementById('submit-button');
        const errorMessage = document.getElementById('error-message');

        submitButton.addEventListener('click', async () => {
            submitButton.disabled = true;
            submitButton.textContent = 'Processing...';

            const { error } = await stripe.confirmPayment({
                elements: elements,
                confirmParams: {
                    return_url: 'https://yourapp.com/payment/success',
                },
            });

            if (error) {
                errorMessage.textContent = error.message;
                submitButton.disabled = false;
                submitButton.textContent = 'Pay $29.99';
            }
        });
    </script>
</body>
</html>
```

🔍 **Line-by-Line Breakdown:**

1. `<script src="https://js.stripe.com/v3/"></script>` — Loads Stripe's JavaScript library.
2. `Stripe('{{ publishable_key }}')` — Initializes Stripe with your publishable key (safe to expose in frontend).
3. `clientSecret` — The client secret from the payment intent created on your server.
4. `stripe.elements()` — Creates the Stripe Elements mounting point. Pass the `clientSecret` so Elements can connect to the payment intent.
5. `elements.create('payment')` — Creates a payment element that automatically handles various payment methods (card, Apple Pay, Google Pay, etc.).
6. `paymentElement.mount('#payment-element')` — Mounts the payment form to a DOM element.
7. `stripe.confirmPayment()` — Confirms the payment. On success, redirects to `return_url`. On failure, returns an error.

## FastAPI Backend for Stripe Checkout

Here's how to integrate Stripe with FastAPI:

```python
from fastapi import FastAPI, HTTPException, Request
from fastapi.responses import HTMLResponse
from pydantic import BaseModel, Field
from typing import Optional
import stripe


app = FastAPI()

# Configure Stripe
stripe.api_key = os.environ["STRIPE_SECRET_KEY"]
STRIPE_PUBLISHABLE_KEY = os.environ["STRIPE_PUBLISHABLE_KEY"]


class CreatePaymentRequest(BaseModel):
    """Request model for creating a payment."""
    amount: int = Field(..., gt=0, description="Amount in cents")
    currency: str = Field(default="usd", description="Currency code")
    email: Optional[str] = None
    order_id: Optional[str] = None


class PaymentResponse(BaseModel):
    """Response model for payment operations."""
    client_secret: str
    payment_intent_id: str


@app.post("/api/payments/create", response_model=PaymentResponse)
async def create_payment(request: CreatePaymentRequest) -> PaymentResponse:
    """Create a payment intent and return client secret."""
    # Create or get customer
    customer = None
    if request.email:
        customers = stripe.Customer.list(email=request.email, limit=1)
        if customers.data:
            customer = customers.data[0]
        else:
            customer = stripe.Customer.create(email=request.email)
    
    # Create payment intent
    intent = stripe.PaymentIntent.create(
        amount=request.amount,
        currency=request.currency,
        customer=customer.id if customer else None,
        metadata={"order_id": request.order_id} if request.order_id else {},
        automatic_payment_methods={"enabled": True},
    )
    
    return PaymentResponse(
        client_secret=intent.client_secret,
        payment_intent_id=intent.id,
    )


@app.get("/checkout/{payment_intent_id}", response_class=HTMLResponse)
async def checkout_page(payment_intent_id: str) -> str:
    """Render checkout page with Stripe Elements."""
    intent = stripe.PaymentIntent.retrieve(payment_intent_id)
    
    return f"""
    <!DOCTYPE html>
    <html>
    <head>
        <title>Checkout</title>
        <script src="https://js.stripe.com/v3/"></script>
    </head>
    <body>
        <h1>Pay ${intent.amount / 100:.2f}</h1>
        <div id="payment-element"></div>
        <button id="submit">Pay Now</button>
        <script>
            const stripe = Stripe('{STRIPE_PUBLISHABLE_KEY}');
            const elements = stripe.elements({{
                clientSecret: '{intent.client_secret}',
                appearance: {{ theme: 'stripe' }},
            }});
            const paymentElement = elements.create('payment');
            paymentElement.mount('#payment-element');
            
            document.getElementById('submit').addEventListener('click', async () => {{
                await stripe.confirmPayment({{
                    elements,
                    confirmParams: {{ return_url: window.location.origin + '/success' }},
                }});
            }});
        </script>
    </body>
    </html>
    """


@app.post("/api/payments/webhook")
async def stripe_webhook(request: Request) -> dict:
    """Handle Stripe webhook events."""
    payload = await request.body()
    sig_header = request.headers.get("stripe-signature")
    webhook_secret = os.environ["STRIPE_WEBHOOK_SECRET"]
    
    try:
        event = stripe.Webhook.construct_event(
            payload, sig_header, webhook_secret
        )
    except ValueError as e:
        raise HTTPException(status_code=400, detail="Invalid payload")
    except stripe.error.SignatureVerificationError as e:
        raise HTTPException(status_code=400, detail="Invalid signature")
    
    # Handle the event
    match event.type:
        case "payment_intent.succeeded":
            payment_intent = event.data.object
            print(f"Payment succeeded: {payment_intent.id}")
            # TODO: Update order status in database
            
        case "payment_intent.payment_failed":
            payment_intent = event.data.object
            print(f"Payment failed: {payment_intent.id}")
            # TODO: Handle failed payment
            
        case "customer.subscription.created":
            subscription = event.data.object
            print(f"Subscription created: {subscription.id}")
            # TODO: Activate subscription in database
            
        case "customer.subscription.deleted":
            subscription = event.data.object
            print(f"Subscription cancelled: {subscription.id}")
            # TODO: Deactivate subscription
            
        case _:
            print(f"Unhandled event type: {event.type}")
    
    return {"status": "success"}
```

🔍 **Line-by-Line Breakdown:**

1. `from fastapi import FastAPI` — FastAPI for creating the backend API.
2. `CreatePaymentRequest(BaseModel)` — Pydantic model for validating payment creation requests. Uses `Field` for additional validation.
3. `create_payment()` endpoint — Creates a payment intent. First looks up existing customer by email, or creates a new one.
4. `stripe.PaymentIntent.create()` — Creates the actual payment intent with amount, currency, and metadata.
5. `checkout_page()` — Returns HTML with embedded Stripe Elements. Uses f-string to inject the client secret.
6. `stripe_webhook()` — Handles asynchronous events from Stripe (payment success, failure, subscription changes).
7. `stripe.Webhook.construct_event()` — Verifies the webhook signature to ensure it came from Stripe.
8. `match event.type:` — Uses Python 3.10+ match statement to handle different event types cleanly.

## Implementing Subscriptions

Stripe makes subscriptions easy:

```python
def create_subscription(
    self,
    customer_id: str,
    price_id: str,
) -> stripe.Subscription:
    """Create a subscription for a customer."""
    subscription = stripe.Subscription.create(
        customer=customer_id,
        items=[{"price": price_id}],
        payment_behavior="default_incomplete",
        payment_settings={"save_default_payment_method": "on_subscription"},
        expand=["latest_invoice.payment_intent"],
    )
    return subscription


def get_subscription(self, subscription_id: str) -> stripe.Subscription:
    """Retrieve subscription details."""
    return stripe.Subscription.retrieve(subscription_id)


def cancel_subscription(self, subscription_id: str) -> stripe.Subscription:
    """Cancel a subscription at period end."""
    return stripe.Subscription.modify(
        subscription_id,
        cancel_at_period_end=True,
    )


def create_product_and_price(
    self,
    product_name: str,
    price_amount: int,
    currency: str = "usd",
    interval: str = "month",
) -> tuple[str, str]:
    """Create a product and its price."""
    product = stripe.Product.create(name=product_name)
    price = stripe.Price.create(
        product=product.id,
        unit_amount=price_amount,
        currency=currency,
        recurring={"interval": interval},
    )
    return product.id, price.id
```

## PCI Compliance

Stripe handles most PCI compliance for you when using Elements. However, follow these best practices:

1. **Never touch card data directly** — Always use Stripe Elements or Checkout
2. **Use HTTPS** — All payment pages must be served over HTTPS
3. **Store minimal data** — Don't store card numbers; Stripe handles this
4. **Use webhooks** — Verify payment status through webhooks, not client-side redirects

## Summary

- Stripe provides a complete payment solution with Python SDK
- Use Payment Intents for one-time payments
- Stripe Elements handles secure card collection on the frontend
- Webhooks notify your server of payment events asynchronously
- Subscriptions are handled through Stripe's Subscription API
- Always verify webhook signatures for security

## Next Steps

→ Continue to `04-sendgrid-email-integration.md` to learn about sending emails.
