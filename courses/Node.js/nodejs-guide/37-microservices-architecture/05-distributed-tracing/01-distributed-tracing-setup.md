# Distributed Tracing Setup

## What You'll Learn

- How distributed tracing works across services
- How trace context propagates
- How to set up tracing with OpenTelemetry
- How to correlate traces across services

## How It Works

```
Service A                    Service B                    Service C
    │                           │                           │
    │── trace-id: abc ────────→│                           │
    │   span-id: 001           │                           │
    │                           │── trace-id: abc ────────→│
    │                           │   span-id: 002           │
    │                           │   parent-span-id: 001    │
    │                           │                           │── trace-id: abc
    │                           │                           │   span-id: 003
    │                           │←──────────────────────────│   parent-span-id: 002
    │←──────────────────────────│                           │
    │                           │                           │
```

All services share the same `trace-id`. Spans are linked by `parent-span-id`.

## Setup with OpenTelemetry

```ts
// tracing.ts — Load before everything else

import { NodeSDK } from '@opentelemetry/sdk-node';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';

const sdk = new NodeSDK({
  traceExporter: new OTLPTraceExporter({
    url: process.env.OTEL_EXPORTER_OTLP_ENDPOINT || 'http://localhost:4318/v1/traces',
  }),
  serviceName: process.env.SERVICE_NAME || 'unknown-service',
  instrumentations: [
    getNodeAutoInstrumentations({
      '@opentelemetry/instrumentation-http': { enabled: true },
      '@opentelemetry/instrumentation-express': { enabled: true },
      '@opentelemetry/instrumentation-pg': { enabled: true },
    }),
  ],
});

sdk.start();

process.on('SIGTERM', async () => {
  await sdk.shutdown();
});
```

## Manual Span Creation

```ts
import { trace } from '@opentelemetry/api';

const tracer = trace.getTracer('user-service');

async function createUser(data: { name: string; email: string }) {
  return tracer.startActiveSpan('createUser', async (span) => {
    span.setAttribute('user.name', data.name);

    try {
      const user = await db.users.create(data);
      span.setAttribute('user.id', user.id);
      span.setStatus({ code: 1 });  // OK
      return user;
    } catch (err) {
      span.setStatus({ code: 2, message: err.message });  // ERROR
      span.recordException(err);
      throw err;
    } finally {
      span.end();
    }
  });
}
```

## Next Steps

For Jaeger, continue to [Jaeger Setup](./02-jaeger-setup.md).
