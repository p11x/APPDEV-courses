# Datadog APM Monitoring

## What You'll Learn

- How Datadog APM captures traces
- How to analyze traces in the Datadog UI
- How to set up APM alerts
- How to use trace analytics

## What Datadog APM Captures

| Data | Automatic | Custom |
|------|-----------|--------|
| HTTP requests | Yes | — |
| Database queries | Yes | — |
| Cache operations | Yes | — |
| Business logic | — | Yes (custom spans) |
| Error tracking | Yes | Yes |
| Latency | Yes | Yes |

## Trace Analysis

In Datadog UI → APM → Traces:

1. **Service Map** — visual dependency graph
2. **Traces** — individual request traces with flame graphs
3. **Analytics** — aggregated metrics by endpoint, status, duration
4. **Error Tracking** — grouped errors with stack traces

## Custom Metrics

```ts
import tracer from 'dd-trace';

// Custom metric (not a trace)
tracer.dogstatsd.increment('orders.created', 1, {
  env: 'production',
  region: 'us-east-1',
});

tracer.dogstatsd.histogram('order.value', totalValue, {
  currency: 'usd',
});

tracer.dogstatsd.gauge('queue.size', queueLength);
```

## Next Steps

For dashboard creation, continue to [Dashboard Creation](./03-dashboard-creation.md).
