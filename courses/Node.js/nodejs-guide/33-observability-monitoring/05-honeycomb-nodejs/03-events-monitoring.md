# Honeycomb Events Monitoring

## What You'll Learn

- How Honeycomb's event model works
- How to add rich context to events
- How to use BubbleUp for anomaly detection
- How to create triggers and alerts

## Event Model

Every trace in Honeycomb is an event with unlimited dimensions:

```ts
const span = tracer.startSpan('process-payment');

// Add as many attributes as needed
span.setAttribute('payment.amount', 99.99);
span.setAttribute('payment.currency', 'USD');
span.setAttribute('payment.method', 'credit_card');
span.setAttribute('user.tier', 'premium');
span.setAttribute('cart.item_count', 3);
span.setAttribute('promo.code', 'SAVE10');
span.setAttribute('region', 'us-east-1');

span.end();
```

## BubbleUp

Honeycomb's BubbleUp automatically highlights dimensions that correlate with problems:

1. Run a query showing slow requests
2. Click "BubbleUp"
3. Honeycomb shows which attributes are different for slow vs fast requests
4. Example: "95% of slow requests have `user.tier=free` and `region=eu-west-1`"

## Triggers

```
Trigger: High Error Rate
Query: COUNT WHERE http.status_code >= 500
Threshold: > 10 in 5 minutes
Action: Send to Slack #alerts
```

## Next Steps

For comparison, continue to [Honeycomb vs Datadog](./04-honeycomb-vs-datadog.md).
