# OpenTelemetry Setup

## What You'll Learn

- What OpenTelemetry is and why it matters
- How to set up the OpenTelemetry SDK for Node.js
- How to configure exporters
- How OpenTelemetry compares to vendor-specific agents

## What Is OpenTelemetry?

OpenTelemetry (OTel) is a **vendor-neutral observability framework**. It provides a single API for collecting **traces**, **metrics**, and **logs** — and exports them to any backend (Jaeger, Prometheus, Datadog, Grafana, etc.).

```
Your App
  │
  ▼
OpenTelemetry SDK (collects data)
  │
  ├── Traces → Jaeger / Zipkin / Datadog
  ├── Metrics → Prometheus / Grafana
  └── Logs → Loki / Elasticsearch
```

## Setup

```bash
npm install @opentelemetry/sdk-node \
  @opentelemetry/api \
  @opentelemetry/auto-instrumentations-node \
  @opentelemetry/exporter-trace-otlp-http \
  @opentelemetry/exporter-metrics-otlp-http
```

## Basic Configuration

```ts
// tracing.ts — OpenTelemetry setup (import BEFORE your app)

import { NodeSDK } from '@opentelemetry/sdk-node';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-http';
import { PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';

// Configure trace exporter (where traces are sent)
const traceExporter = new OTLPTraceExporter({
  url: process.env.OTEL_EXPORTER_OTLP_ENDPOINT || 'http://localhost:4318/v1/traces',
});

// Configure metric exporter
const metricExporter = new OTLPMetricExporter({
  url: process.env.OTEL_EXPORTER_OTLP_ENDPOINT || 'http://localhost:4318/v1/metrics',
});

const metricReader = new PeriodicExportingMetricReader({
  exporter: metricExporter,
  exportIntervalMillis: 15_000,  // Export metrics every 15 seconds
});

// Initialize the SDK
const sdk = new NodeSDK({
  traceExporter,
  metricReader,
  serviceName: 'my-api',  // Name shown in dashboards
  instrumentations: [
    getNodeAutoInstrumentations(),  // Auto-instrument Express, HTTP, fetch, etc.
  ],
});

// Start collecting telemetry
sdk.start();

console.log('OpenTelemetry initialized');

// Graceful shutdown
process.on('SIGTERM', async () => {
  await sdk.shutdown();
  process.exit(0);
});
```

## Application Entry Point

```ts
// server.ts — Import tracing BEFORE everything else

import './tracing.js';  // Must be first import!

import express from 'express';
import { trace } from '@opentelemetry/api';

const app = express();

app.get('/', (req, res) => {
  res.json({ message: 'Hello with OpenTelemetry!' });
});

app.get('/api/users', async (req, res) => {
  // Auto-instrumented: Express and HTTP calls create spans automatically
  const users = [
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' },
  ];
  res.json(users);
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Environment Variables

```bash
# .env
OTEL_SERVICE_NAME=my-api
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4318
OTEL_EXPORTER_OTLP_HEADERS=x-api-key=secret
OTEL_TRACES_SAMPLER=traceidratio
OTEL_TRACES_SAMPLER_ARG=0.1  # Sample 10% of traces
```

## Auto-Instrumented Libraries

The `getNodeAutoInstrumentations()` package automatically instruments:

| Library | What It Captures |
|---------|-----------------|
| `express` | Routes, middleware, status codes |
| `http` / `https` | Outgoing requests, headers |
| `fetch` | Native fetch calls |
| `pg` | PostgreSQL queries |
| `redis` | Redis commands |
| `mongodb` | MongoDB operations |
| `graphql` | GraphQL resolvers |
| `grpc` | gRPC calls |

## Next Steps

For custom tracing, continue to [OpenTelemetry Tracing](./02-opentelemetry-tracing.md).
