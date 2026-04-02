# Datadog Setup

## What You'll Learn

- How to set up Datadog APM for Node.js
- How Datadog's tracing agent works
- How to configure environment and service tags
- How Datadog compares to other observability platforms

## Setup

```bash
npm install dd-trace
```

```ts
// tracing.ts — Import BEFORE your app

import 'dd-trace/init.js';

// Or with configuration:
import tracer from 'dd-trace';

tracer.init({
  service: 'my-api',
  env: process.env.NODE_ENV || 'development',
  version: process.env.APP_VERSION || '1.0.0',
  logInjection: true,       // Inject trace IDs into logs
  runtimeMetrics: true,     // Collect Node.js runtime metrics
  profiling: true,          // Enable continuous profiling
});

export default tracer;
```

## Express Integration

```ts
// server.ts

import './tracing.js';  // Must be first import!
import express from 'express';

const app = express();

// Datadog automatically instruments Express routes
app.get('/api/users', async (req, res) => {
  // Automatic spans for this route
  const users = await db.query('SELECT * FROM users');
  res.json(users);
});
```

## Custom Spans

```ts
import tracer from 'dd-trace';

// Create custom span
const span = tracer.startSpan('process-order');

span.setTag('order.id', orderId);
span.setTag('order.total', total);
span.setTag('user.id', userId);

try {
  await processOrder(orderId);
  span.setTag('status', 'ok');
} catch (err) {
  span.setTag('error', true);
  span.setTag('error.msg', err.message);
  throw err;
} finally {
  span.finish();
}
```

## Environment Variables

```bash
# .env
DD_SERVICE=my-api
DD_ENV=production
DD_VERSION=1.0.0
DD_TRACE_SAMPLE_RATE=0.1
DD_LOGS_INJECTION=true
DD_RUNTIME_METRICS_ENABLED=true
DD_PROFILING_ENABLED=true
```

## Next Steps

For APM monitoring, continue to [APM Monitoring](./02-apm-monitoring.md).
