# Honeycomb vs Datadog

## What You'll Learn

- Detailed comparison of Honeycomb and Datadog
- When to choose each
- How to use both together

## Comparison

| Feature | Honeycomb | Datadog |
|---------|-----------|---------|
| Focus | Events + Traces | Metrics + APM + Logs |
| Data model | Events (high cardinality) | Metrics (low cardinality) |
| Query power | Unlimited dimensions | Label-based |
| Debugging | BubbleUp (auto-anomaly) | Manual correlation |
| Pricing | Per event | Per host + ingest |
| Best for | Debugging complex issues | Infrastructure monitoring |
| Log support | Via OpenTelemetry | Native |
| Dashboard | Query-focused | Widget-based |

## When to Choose Honeycomb

- **Debugging complex distributed systems** — BubbleUp finds root causes
- **High-cardinality data** — millions of unique user IDs, endpoints, etc.
- **Event-first culture** — teams that think in traces, not metrics
- **Cost-sensitive** — 20M events/month free tier

## When to Choose Datadog

- **Infrastructure monitoring** — hosts, containers, cloud resources
- **All-in-one** — APM + logs + metrics + security in one platform
- **Large team** — extensive RBAC and collaboration features
- **Existing investment** — already using Datadog for infrastructure

## Using Both Together

```ts
// Send to both via OpenTelemetry
import { NodeSDK } from '@opentelemetry/sdk-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { CompositeSpanProcessor } from '@opentelemetry/sdk-trace-base';

const sdk = new NodeSDK({
  // Send traces to both Honeycomb and Datadog
  spanProcessors: [
    new SimpleSpanProcessor(new OTLPTraceExporter({
      url: 'https://api.honeycomb.io/v1/traces',
      headers: { 'x-honeycomb-team': process.env.HONEYCOMB_API_KEY },
    })),
    new SimpleSpanProcessor(new OTLPTraceExporter({
      url: 'http://localhost:4318/v1/traces',  // Datadog agent
    })),
  ],
});
```

## Next Steps

This concludes Chapter 33. Return to the [guide index](../../index.html).
