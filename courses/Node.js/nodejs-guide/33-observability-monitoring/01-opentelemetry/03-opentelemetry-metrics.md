# OpenTelemetry Metrics

## What You'll Learn

- How to create custom metrics with OpenTelemetry
- How to use counters, histograms, and gauges
- How to set up metric exporters
- How metrics differ from traces

## Metric Types

| Type | Use Case | Example |
|------|----------|---------|
| **Counter** | Cumulative count (only goes up) | Total requests, errors |
| **Histogram** | Distribution of values | Request duration, response size |
| **Gauge** | Current value (can go up/down) | Memory usage, active connections |
| **UpDownCounter** | Cumulative that can go up/down | Active requests |

## Custom Metrics

```ts
// metrics.ts — Custom metrics

import { metrics } from '@opentelemetry/api';
import { MeterProvider, PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-http';

// Create meter (groups related metrics)
const meter = metrics.getMeter('my-api');

// Counter: total HTTP requests
export const requestCounter = meter.createCounter('http.requests.total', {
  description: 'Total number of HTTP requests',
  unit: 'requests',
});

// Counter: total errors
export const errorCounter = meter.createCounter('http.errors.total', {
  description: 'Total number of HTTP errors',
  unit: 'errors',
});

// Histogram: request duration
export const requestDuration = meter.createHistogram('http.request.duration', {
  description: 'HTTP request duration in milliseconds',
  unit: 'ms',
});

// Gauge: active connections
export const activeConnections = meter.createUpDownCounter('http.connections.active', {
  description: 'Number of active HTTP connections',
  unit: 'connections',
});

// Custom business metric
export const ordersTotal = meter.createCounter('orders.total', {
  description: 'Total orders placed',
  unit: 'orders',
});

export const orderValue = meter.createHistogram('orders.value', {
  description: 'Order value in dollars',
  unit: 'usd',
});
```

## Using Metrics in Middleware

```ts
// middleware/metrics.ts

import { requestCounter, errorCounter, requestDuration } from './metrics.js';
import { Request, Response, NextFunction } from 'express';

export function metricsMiddleware(req: Request, res: Response, next: NextFunction) {
  const start = performance.now();

  res.on('finish', () => {
    const duration = performance.now() - start;

    // Record metrics with labels (dimensions)
    requestCounter.add(1, {
      method: req.method,
      route: req.route?.path || req.path,
      status: res.statusCode,
    });

    requestDuration.record(duration, {
      method: req.method,
      route: req.route?.path || req.path,
    });

    if (res.statusCode >= 400) {
      errorCounter.add(1, {
        method: req.method,
        route: req.route?.path || req.path,
        status: res.statusCode,
      });
    }
  });

  next();
}
```

## Business Metrics

```ts
// services/orderService.ts

import { ordersTotal, orderValue } from '../metrics.js';

export async function createOrder(userId: string, items: CartItem[]) {
  const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);

  const order = await db.orders.create({ userId, items, total });

  // Record business metrics
  ordersTotal.add(1, { status: 'completed' });
  orderValue.record(total, { currency: 'usd' });

  return order;
}
```

## Next Steps

For logging, continue to [OpenTelemetry Logs](./04-opentelemetry-logs.md).
