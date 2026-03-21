# Webhooks

## What You'll Learn
- Understanding webhooks
- Handling webhook requests
- Verifying webhook signatures
- Processing different webhook events

## Prerequisites
- Understanding of route handlers
- Basic knowledge of HTTP requests
- Familiarity with headers and signatures

## Concept Explained Simply

Webhooks are like a phone call instead of constantly checking your email. Instead of your app constantly asking "Any new payments?" every few seconds (polling), the payment provider calls YOU when something happens: "Hey, a new payment just came in!"

This is much more efficient — you only do work when there's actually something to do. But since anyone could theoretically send a request pretending to be your payment provider, you need to verify that the request is actually from them (using signatures).

## Complete Code Example

### Stripe Webhook Handler

```typescript
// app/api/webhooks/stripe/route.ts
import { NextResponse } from "next/server";
import { headers } from "next/headers";
import Stripe from "stripe";

const stripe = new Stripe(process.env.STRIPE_SECRET_KEY!, {
  apiVersion: "2024-12-18.acacia",
});

const endpointSecret = process.env.STRIPE_WEBHOOK_SECRET!;

export async function POST(request: Request) {
  const body = await request.text();
  const headersList = await headers();
  const signature = headersList.get("stripe-signature")!;
  
  let event: Stripe.Event;
  
  try {
    // Verify the webhook signature
    event = stripe.webhooks.constructEvent(
      body,
      signature,
      endpointSecret
    );
  } catch (err) {
    console.error("Webhook signature verification failed:", err);
    return NextResponse.json(
      { error: "Invalid signature" },
      { status: 400 }
    );
  }
  
  // Handle the event
  try {
    switch (event.type) {
      case "checkout.session.completed": {
        const session = event.data.object as Stripe.Checkout.Session;
        // Handle successful payment
        await handleSuccessfulPayment(session);
        break;
      }
      
      case "customer.subscription.created": {
        const subscription = event.data.object as Stripe.Subscription;
        await handleNewSubscription(subscription);
        break;
      }
      
      case "customer.subscription.deleted": {
        const subscription = event.data.object as Stripe.Subscription;
        await handleCancelledSubscription(subscription);
        break;
      }
      
      case "invoice.payment_failed": {
        const invoice = event.data.object as Stripe.Invoice;
        await handleFailedPayment(invoice);
        break;
      }
      
      default:
        console.log(`Unhandled event type: ${event.type}`);
    }
    
    return NextResponse.json({ received: true });
    
  } catch (error) {
    console.error("Error processing webhook:", error);
    return NextResponse.json(
      { error: "Webhook handler failed" },
      { status: 500 }
    );
  }
}

// Helper functions (implement your logic)
async function handleSuccessfulPayment(session: Stripe.Checkout.Session) {
  const userId = session.metadata?.userId;
  if (!userId) return;
  
  // Update user to paid status in database
  console.log(`Payment successful for user ${userId}`);
}

async function handleNewSubscription(subscription: Stripe.Subscription) {
  console.log("New subscription:", subscription.id);
}

async function handleCancelledSubscription(subscription: Stripe.Subscription) {
  console.log("Subscription cancelled:", subscription.id);
}

async function handleFailedPayment(invoice: Stripe.Invoice) {
  console.log("Payment failed:", invoice.id);
}
```

### GitHub Webhook Handler

```typescript
// app/api/webhooks/github/route.ts
import { NextResponse } from "next/server";
import { headers } from "next/headers";
import crypto from "crypto";

const webhookSecret = process.env.GITHUB_WEBHOOK_SECRET!;

export async function POST(request: Request) {
  const body = await request.text();
  const headersList = await headers();
  
  const signature = headersList.get("x-hub-signature-256")!;
  const event = headersList.get("x-github-event")!;
  
  // Verify signature
  const expectedSignature = 
    "sha256=" + 
    crypto
      .createHmac("sha256", webhookSecret)
      .update(body)
      .digest("hex");
  
  if (signature !== expectedSignature) {
    return NextResponse.json(
      { error: "Invalid signature" },
      { status: 401 }
    );
  }
  
  const payload = JSON.parse(body);
  
  // Handle different GitHub events
  switch (event) {
    case "push":
      await handlePushEvent(payload);
      break;
    case "pull_request":
      await handlePullRequest(payload);
      break;
    case "issues":
      await handleIssueEvent(payload);
      break;
  }
  
  return NextResponse.json({ received: true });
}

async function handlePushEvent(payload: any) {
  const { repository, commits } = payload;
  console.log(`New push to ${repository.name}: ${commits?.length} commits`);
}

async function handlePullRequest(payload: any) {
  const { action, pull_request } = payload;
  console.log(`PR ${action}: ${pull_request?.title}`);
}

async function handleIssueEvent(payload: any) {
  const { action, issue } = payload;
  console.log(`Issue ${action}: ${issue?.title}`);
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `await request.text()` | Get raw body as text | Needed for signature verification |
| `headersList.get("stripe-signature")` | Get webhook signature | Proves request is from Stripe |
| `stripe.webhooks.constructEvent()` | Verify signature | Throws if invalid |
| `event.type` | Event type identifier | Know what happened |
| `event.data.object` | Event data | Contains the actual data |

## Common Mistakes

### Mistake 1: Not Verifying Signatures

```typescript
// WRONG - Anyone can send fake webhooks!
export async function POST(request: Request) {
  const body = await request.json();
  // Process payment without verification!
  // Hackers can send fake "payment successful" events!
}

// CORRECT - Always verify signature
export async function POST(request: Request) {
  const body = await request.text();
  const signature = request.headers.get("stripe-signature")!;
  
  try {
    const event = stripe.webhooks.constructEvent(body, signature, secret);
    // Now it's safe to process
  } catch (err) {
    return NextResponse.json({ error: "Invalid" }, { status: 400 });
  }
}
```

### Mistake 2: Reading Body as JSON Instead of Text

```typescript
// WRONG - Can't verify signature after parsing as JSON
export async function POST(request: Request) {
  const body = await request.json(); // Body already consumed!
  const signature = request.headers.get("signature");
  // Signature verification will fail!
}

// CORRECT - Read as text first
export async function POST(request: Request) {
  const body = await request.text(); // Get raw text
  const signature = request.headers.get("signature");
  
  // Now verify...
  verify(body, signature);
  
  // Only AFTER verification, parse JSON
  const data = JSON.parse(body);
}
```

### Mistake 3: Not Returning 200 Quickly

```typescript
// WRONG - Taking too long to respond
export async function POST(request: Request) {
  // Doing heavy processing before responding
  await processAllData();
  // If this takes > 30 seconds, provider retries!
  
  return NextResponse.json({ ok: true });
}

// CORRECT - Respond immediately, process in background
export async function POST(request: Request) {
  const data = await request.json();
  
  // Respond right away
  return NextResponse.json({ received: true });
  
  // Process asynchronously
  setTimeout(() => processInBackground(data), 0);
}
```

## Summary

- Webhooks are HTTP requests sent by external services
- Always verify signatures to ensure authenticity
- Read request body as text for signature verification
- Process events asynchronously and respond quickly
- Handle different event types with a switch statement
- Log all received webhooks for debugging
- Return 200 quickly to prevent retries

## Next Steps

- [returning-json.md](../02-request-and-response/returning-json.md) - Proper response formatting
- [sse-with-route-handlers.md](../17-websockets-and-realtime/02-server-sent-events/sse-with-route-handlers.md) - Using SSE for real-time updates
