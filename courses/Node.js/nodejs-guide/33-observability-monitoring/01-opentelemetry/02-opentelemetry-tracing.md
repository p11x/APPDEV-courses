# OpenTelemetry Tracing

## What You'll Learn

- How to create custom spans
- How to add attributes and events to spans
- How span context propagation works
- How to trace async operations

## Custom Spans

```ts
// services/userService.ts

import { trace } from '@opentelemetry/api';

const tracer = trace.getTracer('user-service');

export async function getUserById(id: string) {
  // Create a custom span for this operation
  return tracer.startActiveSpan('getUserById', async (span) => {
    try {
      // Add attributes (metadata) to the span
      span.setAttribute('user.id', id);
      span.setAttribute('db.system', 'postgresql');

      // Simulate database query
      const user = await db.query('SELECT * FROM users WHERE id = $1', [id]);

      // Add events (milestones within the span)
      span.addEvent('query.completed', {
        'db.rows_affected': user.rowCount,
      });

      if (!user.rows[0]) {
        span.setStatus({ code: 2, message: 'User not found' });  // ERROR
        return null;
      }

      span.setStatus({ code: 1 });  // OK
      return user.rows[0];
    } catch (err) {
      span.setStatus({ code: 2, message: err.message });  // ERROR
      span.recordException(err);
      throw err;
    } finally {
      span.end();  // Always end the span
    }
  });
}
```

## Nested Spans

```ts
// services/orderService.ts

import { trace } from '@opentelemetry/api';

const tracer = trace.getTracer('order-service');

export async function createOrder(userId: string, items: CartItem[]) {
  return tracer.startActiveSpan('createOrder', async (span) => {
    span.setAttribute('user.id', userId);
    span.setAttribute('order.items_count', items.length);

    try {
      // Child span: validate inventory
      const inventoryValid = await tracer.startActiveSpan('validateInventory', async (childSpan) => {
        childSpan.setAttribute('items.count', items.length);
        const result = await inventoryService.check(items);
        childSpan.setAttribute('inventory.valid', result.valid);
        childSpan.end();
        return result.valid;
      });

      if (!inventoryValid) {
        span.setStatus({ code: 2, message: 'Insufficient inventory' });
        throw new Error('Insufficient inventory');
      }

      // Child span: process payment
      const payment = await tracer.startActiveSpan('processPayment', async (childSpan) => {
        childSpan.setAttribute('payment.method', 'credit_card');
        const result = await paymentService.charge(userId, items);
        childSpan.setAttribute('payment.transaction_id', result.transactionId);
        childSpan.end();
        return result;
      });

      // Child span: save order
      const order = await tracer.startActiveSpan('saveOrder', async (childSpan) => {
        const saved = await db.orders.create({ userId, items, paymentId: payment.id });
        childSpan.setAttribute('order.id', saved.id);
        childSpan.end();
        return saved;
      });

      span.setStatus({ code: 1 });  // OK
      return order;
    } catch (err) {
      span.setStatus({ code: 2, message: err.message });
      span.recordException(err);
      throw err;
    } finally {
      span.end();
    }
  });
}
```

## Context Propagation

Context propagation ensures trace IDs flow across service boundaries:

```ts
// Automatically handled by auto-instrumentation
// But for manual HTTP calls:

import { propagation, context } from '@opentelemetry/api';

// Inject context into outgoing HTTP headers
const headers = {};
propagation.inject(context.active(), headers);

const response = await fetch('http://other-service/api/data', {
  headers,  // Trace ID flows to the other service
});

// Extract context from incoming HTTP headers (usually auto-instrumented)
const extractedContext = propagation.extract(context.active(), req.headers);
```

## Span Attributes Reference

| Category | Attributes |
|----------|-----------|
| HTTP | `http.method`, `http.url`, `http.status_code` |
| Database | `db.system`, `db.statement`, `db.name` |
| Messaging | `messaging.system`, `messaging.destination` |
| Custom | `user.id`, `order.total`, any key-value pair |

## Common Mistakes

### Mistake 1: Not Ending Spans

```ts
// WRONG — span never ends, memory leak
tracer.startActiveSpan('myOp', (span) => {
  doWork();
  // Forgot span.end()!
});

// CORRECT — use finally or startActiveSpan with callback
tracer.startActiveSpan('myOp', async (span) => {
  try {
    await doWork();
  } finally {
    span.end();
  }
});
```

### Mistake 2: Sensitive Data in Attributes

```ts
// WRONG — PII in span attributes
span.setAttribute('user.email', 'alice@example.com');
span.setAttribute('user.password', 'secret123');

// CORRECT — use IDs, not PII
span.setAttribute('user.id', 'user-123');
```

## Next Steps

For metrics, continue to [OpenTelemetry Metrics](./03-opentelemetry-metrics.md).
