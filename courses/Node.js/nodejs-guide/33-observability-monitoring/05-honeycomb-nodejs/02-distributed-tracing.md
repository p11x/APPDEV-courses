# Honeycomb Distributed Tracing

## What You'll Learn

- How distributed tracing works in Honeycomb
- How to trace across multiple services
- How to analyze traces in Honeycomb
- How to find performance bottlenecks

## Trace Propagation

Honeycomb uses W3C Trace Context for propagation:

```ts
// Service A → Service B
const headers = {};
propagation.inject(context.active(), headers);

const response = await fetch('http://service-b/api/data', {
  headers,  // Trace context flows automatically
});
```

## Analyzing Traces in Honeycomb

1. Go to Traces → find a trace
2. View the waterfall chart
3. Click on spans to see attributes
4. Use "BubbleUp" to find anomalies

## Honeycomb Query Language

```
# Slow requests
HEATMAP(duration_ms) WHERE http.status_code >= 200

# Error rate by endpoint
COUNT WHERE error = true GROUP BY http.route

# P99 latency
P99(duration_ms) GROUP BY http.route

# Outlier detection
HEATMAP(duration_ms) WHERE http.route = "/api/users"
```

## Next Steps

For events monitoring, continue to [Events Monitoring](./03-events-monitoring.md).
