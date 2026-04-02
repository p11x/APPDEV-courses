# Honeycomb Setup

## What You'll Learn

- What Honeycomb is and how it differs from other observability tools
- How to set up Honeycomb for Node.js
- How Honeycomb's event-based model works
- How to send traces and events to Honeycomb

## What Is Honeycomb?

Honeycomb is an **observability platform** built for debugging distributed systems. Unlike metrics-first tools, Honeycomb is **event-first** — every request is a rich event with all its context.

| Feature | Datadog | New Relic | Honeycomb |
|---------|---------|-----------|-----------|
| Focus | Metrics + APM | All-in-one | Events + Traces |
| Query | PromQL + custom | NRQL | Honeycomb Query Language |
| Free tier | 14-day trial | 100GB/month | 20M events/month |
| Best for | Infrastructure | General | Debugging complex systems |

## Setup

```bash
npm install @honeycombio/opentelemetry-node
```

```ts
// tracing.ts — Honeycomb OpenTelemetry setup

import { HoneycombSDK } from '@honeycombio/opentelemetry-node';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';

const sdk = new HoneycombSDK({
  apiKey: process.env.HONEYCOMB_API_KEY,
  serviceName: 'my-api',
  instrumentations: [getNodeAutoInstrumentations()],
});

sdk.start();

process.on('SIGTERM', async () => {
  await sdk.shutdown();
});
```

## Environment Variables

```bash
# .env
HONEYCOMB_API_KEY=your-api-key
HONEYCOMB_DATASET=my-api
OTEL_SERVICE_NAME=my-api
```

## Sending Custom Events

```ts
import { trace } from '@opentelemetry/api';

const tracer = trace.getTracer('my-api');

app.post('/orders', async (req, res) => {
  const span = tracer.startSpan('create-order');

  span.setAttribute('order.total', req.body.total);
  span.setAttribute('order.items', req.body.items.length);
  span.setAttribute('user.id', req.user.id);

  const order = await createOrder(req.body);

  span.setAttribute('order.id', order.id);
  span.setStatus({ code: 1 });
  span.end();

  res.json(order);
});
```

## Next Steps

For distributed tracing, continue to [Distributed Tracing](./02-distributed-tracing.md).
