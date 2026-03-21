# Distributed Tracing with OpenTelemetry

## 📌 What You'll Learn

- What distributed tracing is and why it matters in microservices
- How to set up OpenTelemetry in an Express application
- Creating custom trace spans to measure operations
- Exporting traces to backends like Jaeger, Zipkin, or commercial APMs

## 🧠 Concept Explained (Plain English)

In a monolithic application, a request enters your code, you do some stuff, and it returns. You can measure how long it took and log what happened. Easy.

In a microservices architecture, a single user request might:
1. Hit your Express API
2. Which calls your database
3. Which calls a cache
4. Which calls a payment service
5. Which calls a fraud detection service
6. And returns

When something is slow or fails, how do you know where? You could check logs on each service and try to match timestamps — but that's painful and error-prone.

**Distributed tracing** solves this by giving every request a unique ID (called a trace ID) that propagates through every service. Each operation gets a "span" — a record of how long it took and what happened. Together, spans form a complete picture of the request's journey.

**OpenTelemetry** (OTel) is an open-source standard for collecting telemetry data (traces, metrics, logs). It's vendor-neutral — you can send data to Jaeger, Zipkin, Datadog, New Relic, or others without changing your code.

A **span** is a single operation within a trace — like "query database" or "call payment API". Spans have:
- A name (what operation)
- Start and end times (duration)
- Attributes (custom metadata)
- Events (things that happened during the span)
- Status (ok, error)

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import { 
  NodeSDK, 
  trace, 
  context, 
  SpanStatusCode 
} from '@opentelemetry/sdk-node';
import { 
  HttpInstrumentation 
} from '@opentelemetry/instrumentation-express';
import { 
  JaegerExporter 
} from '@opentelemetry/exporter-jaeger';
import { 
  ZoneContextManager 
} from '@opentelemetry/context-zone';
import { 
  trace, 
  SpanKind, 
  StatusCode 
} from '@opentelemetry/api';

// Initialize OpenTelemetry SDK
const sdk = new NodeSDK({
  // Automatically instrument Express and HTTP requests
  instrumentations: [
    new HttpInstrumentation()
  ],
  
  // Export to Jaeger (or use OTLP for other backends)
  traceExporter: new JaegerExporter({
    endpoint: process.env.JAEGER_ENDPOINT || 'http://localhost:14268/api/traces'
  }),
  
  // Service name shows up in tracing UI
  serviceName: process.env.OTEL_SERVICE_NAME || 'my-express-app',
  
  // Context manager for async propagation
  contextManager: new ZoneContextManager()
});

// Start the SDK
sdk.start().catch(err => console.error('Failed to start SDK:', err));

// Graceful shutdown
process.on('SIGTERM', () => {
  sdk.shutdown().catch(console.error);
});

const app = express();

// Get the tracer
const tracer = trace.getTracer('my-express-app');


// ============================================
// Basic tracing middleware
// ============================================
app.use((req, res, next) => {
  // Continue existing trace from header if present
  const span = tracer.startSpan(req.path, {
    kind: SpanKind.SERVER,
    attributes: {
      'http.method': req.method,
      'http.url': req.url,
      'http.route': req.route?.path || req.path,
      'http.host': req.headers.host,
      'user_agent': req.headers['user-agent']
    }
  });
  
  // Add trace ID to response header for debugging
  res.setHeader('X-Trace-ID', span.spanContext().traceId);
  
  // End span when response finishes
  res.on('finish', () => {
    span.setAttribute('http.status_code', res.statusCode);
    span.setStatus({
      code: res.statusCode >= 400 ? StatusCode.ERROR : StatusCode.OK,
      message: res.statusCode >= 400 ? 'Request failed' : undefined
    });
    span.end();
  });
  
  next();
});


// ============================================
// Manual spans for custom operations
// ============================================
async function fetchUserFromDatabase(userId) {
  // Create a child span for database operation
  return tracer.startActiveSpan('database.query', async (span) => {
    try {
      span.setAttribute('db.system', 'postgresql');
      span.setAttribute('db.statement', `SELECT * FROM users WHERE id = ${userId}`);
      
      // Simulate DB query
      await new Promise(resolve => setTimeout(resolve, 50));
      
      const user = { id: userId, name: 'John Doe', email: 'john@example.com' };
      
      span.setAttribute('db.result', 'success');
      return user;
    } catch (error) {
      span.setStatus({
        code: StatusCode.ERROR,
        message: error.message
      });
      span.recordException(error);
      throw error;
    } finally {
      span.end();
    }
  });
}

async function callExternalPaymentService(amount) {
  // Create span for external API call
  return tracer.startActiveSpan('http.payment_service', async (span) => {
    try {
      span.setAttribute('http.method', 'POST');
      span.setAttribute('http.url', 'https://api.payments.example.com/charge');
      span.setAttribute('http.host', 'api.payments.example.com');
      
      // Simulate external API call
      await new Promise(resolve => setTimeout(resolve, 100));
      
      span.setAttribute('http.status_code', 200);
      return { transactionId: 'txn-123', status: 'success' };
    } catch (error) {
      span.setStatus({
        code: StatusCode.ERROR,
        message: error.message
      });
      throw error;
    } finally {
      span.end();
    }
  });
}


// ============================================
// Example routes demonstrating tracing
// ============================================
app.get('/api/users/:id', async (req, res) => {
  const { id } = req.params;
  
  // Start custom span for this operation
  const span = tracer.startSpan('getUser', {
    attributes: {
      'user.id': id,
      'operation.type': 'read'
    }
  });
  
  try {
    const user = await fetchUserFromDatabase(id);
    
    span.setStatus({ code: StatusCode.OK });
    res.json(user);
  } catch (error) {
    span.setStatus({
      code: StatusCode.ERROR,
      message: error.message
    });
    res.status(500).json({ error: 'Failed to fetch user' });
  } finally {
    span.end();
  }
});

app.post('/api/checkout', async (req, res) => {
  const { amount, userId } = req.body || {};
  
  const span = tracer.startSpan('checkout', {
    attributes: {
      'cart.amount': amount,
      'user.id': userId
    }
  });
  
  try {
    // This will create child spans automatically
    const user = await fetchUserFromDatabase(userId);
    
    // This also creates a child span
    const payment = await callExternalPaymentService(amount);
    
    span.setAttribute('payment.transaction_id', payment.transactionId);
    span.setStatus({ code: StatusCode.OK });
    
    res.json({ orderId: 'order-123', status: 'success' });
  } catch (error) {
    span.setStatus({
      code: StatusCode.ERROR,
      message: error.message
    });
    res.status(500).json({ error: 'Checkout failed' });
  } finally {
    span.end();
  }
});


// ============================================
// Adding custom attributes and events
// ============================================
app.get('/api/search', async (req, res) => {
  const { query } = req.query;
  
  const span = tracer.startSpan('searchProducts');
  span.setAttribute('search.query', query);
  span.addEvent('starting search');
  
  try {
    // Simulate search
    await new Promise(resolve => setTimeout(resolve, 30));
    
    span.addEvent('search completed', { 
      results_count: 42 
    });
    span.setAttribute('results.count', 42);
    span.setStatus({ code: StatusCode.OK });
    
    res.json({ results: [] });
  } catch (error) {
    span.setStatus({ code: StatusCode.ERROR, message: error.message });
    res.status(500).json({ error: 'Search failed' });
  } finally {
    span.end();
  }
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 6-8 | `@opentelemetry/sdk-node` imports | Core OpenTelemetry SDK for Node.js |
| 10-11 | `HttpInstrumentation` | Auto-instrumentation for Express/HTTP |
| 13-14 | `JaegerExporter` | Exports traces to Jaeger backend |
| 19-35 | `new NodeSDK({})` | Configures the telemetry SDK |
| 22 | `instrumentations` | Enables auto-instrumentation for Express |
| 28 | `serviceName` | Identifies this service in traces |
| 43 | `tracer.startSpan()` | Creates a new span manually |
| 56-64 | Response middleware | Ends span when response finishes |
| 60 | `span.setAttribute()` | Adds metadata to span |
| 73-95 | `fetchUserFromDatabase()` | Database operation with child span |
| 78 | `SpanKind.CLIENT` | Marks this as an outbound call |
| 98-120 | `callExternalPaymentService()` | External API call span |
| 137-156 | `/api/users/:id` route | Route with manual span |
| 162-186 | `/api/checkout` route | Complex operation with multiple child spans |
| 193-219 | `/api/search` route | Demonstrates span events |

## ⚠️ Common Mistakes

### 1. Not propagating trace context

**What it is**: Spans don't connect when calling other services.

**Why it happens**: Not extracting trace ID from incoming requests or not passing it to outgoing requests.

**How to fix it**: Use OpenTelemetry's HTTP instrumentation which automatically propagates context. For manual calls, use `context.with()` to propagate the active span.

### 2. Creating too many spans

**What it is**: Every tiny operation gets its own span, overwhelming the tracing system.

**Why it happens**: Over-enthusiastic instrumentation.

**How to fix it**: Only create spans for significant operations — database queries, external API calls, expensive computations. Don't span every function call.

### 3. Forgetting to end spans

**What it is**: Spans that never end, showing incorrect durations or never appearing.

**Why it happens**: Missing `span.end()` in all code paths, especially error cases.

**How to fix it**: Use `try/finally` or `startActiveSpan` (which automatically ends) to ensure spans always complete.

## ✅ Quick Recap

- Distributed tracing tracks requests across service boundaries using trace IDs
- OpenTelemetry is the open standard for collecting traces, metrics, and logs
- Spans represent individual operations with timing and metadata
- Auto-instrumentation captures HTTP requests automatically
- Child spans automatically inherit parent context
- Export traces to Jaeger, Zipkin, or commercial backends

## 🔗 What's Next

Now that you can trace requests, learn about [APM Integration](./03_apm-integration.md) to connect your Express app to commercial monitoring platforms.
