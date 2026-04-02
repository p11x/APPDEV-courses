# Distributed Tracing Patterns

## What You'll Learn

- How to trace across service boundaries
- How to add custom spans
- How to correlate logs with traces
- How to set up trace-based alerts

## Cross-Service Tracing

```ts
// Service A → Service B
// OpenTelemetry auto-propagates trace context

// Service A
const response = await fetch('http://service-b/api/data', {
  headers: propagation.inject(context.active(), {}),
});

// Service B (auto-instrumented)
app.get('/api/data', async (req, res) => {
  // Trace context automatically extracted from headers
  const result = await db.query('SELECT * FROM data');
  res.json(result);
});
```

## Log Correlation

```ts
import { trace } from '@opentelemetry/api';
import pino from 'pino';

const logger = pino();

function getLogger() {
  const span = trace.getActiveSpan();
  const ctx = span?.spanContext();
  return logger.child({
    traceId: ctx?.traceId,
    spanId: ctx?.spanId,
  });
}
```

## Next Steps

For optimization, continue to [Tracing Optimization](./05-tracing-optimization.md).
