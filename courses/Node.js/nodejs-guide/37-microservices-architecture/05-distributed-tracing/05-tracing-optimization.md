# Tracing Optimization

## What You'll Learn

- How to reduce tracing overhead
- How to implement sampling
- How to filter sensitive data
- How to optimize trace storage

## Sampling

```ts
// Sample 10% of traces
const sdk = new NodeSDK({
  sampler: new TraceIdRatioBasedSampler(0.1),
});

// Or sample based on attributes
const sampler = {
  shouldSample: (context, traceId, spanName, spanKind, attributes) => {
    // Always trace errors
    if (attributes['error']) return { decision: 1 };
    // Sample health checks at 1%
    if (spanName === '/healthz') return { decision: Math.random() < 0.01 ? 1 : 0 };
    // Sample everything else at 10%
    return { decision: Math.random() < 0.1 ? 1 : 0 };
  },
};
```

## Next Steps

For circuit breaker, continue to [Circuit Breaker Setup](../06-circuit-breaker/01-circuit-breaker-setup.md).
