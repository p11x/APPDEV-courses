# APM Integration

## 📌 What You'll Learn

- What Application Performance Monitoring (APM) is and why you need it
- How to integrate Express with commercial APM solutions like Datadog and New Relic
- Key metrics to monitor in your Express application
- Custom instrumentation for business-specific metrics

## 🧠 Concept Explained (Plain English)

APM (Application Performance Monitoring) goes beyond basic metrics. It gives you deep visibility into how your application behaves in production — from response times to error rates to detailed transaction traces.

While OpenTelemetry is the open standard, commercial APM tools like **Datadog** and **New Relic** provide:
- Pre-built dashboards for common frameworks (including Express)
- Automatic error tracking and grouping
- Service maps showing how your services connect
- Alerting and anomaly detection
- Log correlation (linking traces to logs)

The key difference from simple logging is that APM agents are **instrumented** — they automatically understand Express routes, database queries, and HTTP calls without you adding explicit code for each one.

**Key metrics** you'll want to monitor:

- **Throughput**: Requests per second — how much traffic you're handling
- **Latency**: How long requests take (usually p50, p95, p99 percentiles)
- **Error rate**: Percentage of requests returning 5xx errors
- **CPU/Memory**: Basic infrastructure metrics
- **Apdex**: User satisfaction score (response time vs. threshold)

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import tracer from 'dd-trace'; // Datadog tracer
import pkg from 'dd-trace';
const { instrument } = pkg;

const app = express();

// ============================================
// Datadog APM Integration
// ============================================

// Initialize Datadog tracer with options
tracer.init({
  // Service name in Datadog
  service: process.env.DATADOG_SERVICE_NAME || 'my-express-app',
  
  // Environment and version tags
  env: process.env.NODE_ENV || 'development',
  version: process.env.APP_VERSION || '1.0.0',
  
  // Enable specific integrations
  integrations: [
    // Express integration is automatic
    // But can configure explicitly:
    new tracer.ExpressIntegration({
      // Express version
      expressVersion: 5
    })
  ],
  
  // Sample rates (1 = 100%)
  sampleRate: 1,
  
  // Log injection for correlation
  logInjection: true,
  
  // Track HTTP tags
  hooks: {
    // Add custom tags to each request span
    request: (span, req) => {
      span.setTag('user.id', req.user?.id);
      span.setTag('tenant.id', req.headers['x-tenant-id']);
    }
  }
});

// Use the tracer
instrument(tracer);

// After tracer is initialized, you can use it for custom spans
const dd = require('dd-trace');


// ============================================
// Alternative: New Relic Integration
// ============================================
/*
// newrelic module must be loaded before other modules
import newrelic from 'newrelic';

// Or require it at the top of your file
// const newrelic = require('newrelic');

// New Relic automatically instruments Express
// Configure in newrelic.js config file
*/


// ============================================
// Custom instrumentation
// ============================================

// Track custom metrics
const gauge = (name, value, tags = {}) => {
  // Datadog: dd.metrics.increment(), dd.metrics.gauge(), etc.
  if (typeof dd !== 'undefined') {
    dd.metrics.distribution(name, value, tags);
  }
};

// Counters for business events
const increment = (name, tags = {}) => {
  if (typeof dd !== 'undefined') {
    dd.metrics.increment(name, Object.entries(tags).map(([k, v]) => `${k}:${v}`));
  }
};


// ============================================
// Request tracking middleware
// ============================================
app.use((req, res, next) => {
  const start = Date.now();
  const requestId = req.headers['x-request-id'] || Math.random().toString(36).substr(2, 9);
  
  // Add custom tags to request
  if (typeof tracer !== 'undefined') {
    tracer.setUser({
      id: req.user?.id,
      email: req.user?.email,
      ip: req.ip
    });
  }
  
  res.on('finish', () => {
    const duration = Date.now() - start;
    
    // Record custom metrics
    gauge('http.request.duration', duration, {
      method: req.method,
      status: res.statusCode,
      path: req.route?.path || 'unknown'
    });
    
    increment('http.request.count', {
      method: req.method,
      status_class: Math.floor(res.statusCode / 100) + 'xx'
    });
    
    // Track errors
    if (res.statusCode >= 500) {
      increment('http.request.errors', {
        path: req.route?.path || 'unknown'
      });
    }
  });
  
  next();
});


// ============================================
// Example routes with custom metrics
// ============================================

// Database query tracking
app.get('/api/users/:id', async (req, res) => {
  const start = Date.now();
  
  try {
    // Simulate database query
    const queryStart = Date.now();
    const user = { id: req.params.id, name: 'John Doe' };
    const queryDuration = Date.now() - queryStart;
    
    // Record database query time
    gauge('db.query.time', queryDuration, {
      operation: 'select',
      table: 'users'
    });
    
    const totalDuration = Date.now() - start;
    gauge('http.request.duration', totalDuration, {
      method: 'GET',
      status: '200',
      path: '/api/users/:id'
    });
    
    res.json(user);
  } catch (error) {
    increment('http.errors', { type: 'database' });
    res.status(500).json({ error: 'Failed to fetch user' });
  }
});

// Business event tracking
app.post('/api/orders', async (req, res) => {
  const { items, userId } = req.body || {};
  
  try {
    // Simulate order processing
    const order = { id: 'order-123', status: 'processing', total: 99.99 };
    
    // Track business metrics
    increment('orders.created', { 
      channel: 'api',
      items_count: items?.length || 0
    });
    
    gauge('orders.total_value', order.total);
    
    res.status(201).json(order);
  } catch (error) {
    increment('orders.failed');
    res.status(500).json({ error: 'Failed to create order' });
  }
});

// Payment tracking
app.post('/api/payments', async (req, res) => {
  const { amount, currency } = req.body || {};
  
  const start = Date.now();
  
  try {
    // Simulate payment processing
    await new Promise(resolve => setTimeout(resolve, 200));
    
    // Track payment metrics
    increment('payments.success', { 
      currency: currency || 'USD'
    });
    
    gauge('payments.amount', amount || 0, {
      currency: currency || 'USD'
    });
    
    const duration = Date.now() - start;
    gauge('payments.processing_time', duration);
    
    res.json({ status: 'success', transactionId: 'txn-123' });
  } catch (error) {
    increment('payments.failed');
    res.status(500).json({ error: 'Payment failed' });
  }
});

// Cache hit/miss tracking
const cache = new Map();

app.get('/api/products/:id', (req, res) => {
  const productId = req.params.id;
  const start = Date.now();
  
  // Check cache
  if (cache.has(productId)) {
    increment('cache.hits', { key: 'products' });
    const duration = Date.now() - start;
    gauge('cache.get_time', duration, { status: 'hit' });
    
    return res.json(cache.get(productId));
  }
  
  increment('cache.misses', { key: 'products' });
  
  // Simulate database fetch
  const product = { id: productId, name: 'Product', price: 29.99 };
  cache.set(productId, product);
  
  const duration = Date.now() - start;
  gauge('cache.get_time', duration, { status: 'miss' });
  
  res.json(product);
});


// ============================================
// Error tracking middleware
// ============================================
app.use((err, req, res, next) => {
  // Record error in APM
  if (typeof dd !== 'undefined') {
    dd.tracer.trace('error-handler', (span) => {
      span.setTag('error', true);
      span.setTag('error.message', err.message);
      span.setTag('error.stack', err.stack);
    });
  }
  
  increment('http.errors', { type: 'unhandled' });
  
  res.status(500).json({ error: 'Internal server error' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  
  // Report startup
  if (typeof dd !== 'undefined') {
    console.log('Datadog APM initialized');
  }
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 6-7 | `import tracer from 'dd-trace'` | Imports Datadog tracing library |
| 14-36 | `tracer.init({})` | Configures Datadog APM agent |
| 22-26 | `integrations` | Enables framework-specific instrumentation |
| 33-40 | `hooks.request` | Adds custom tags to every request span |
| 52-60 | Custom metric functions | Wrappers for recording custom metrics |
| 67-92 | Request middleware | Tracks every request, records duration |
| 75 | `tracer.setUser()` | Associates request with user for tracking |
| 82-85 | `gauge()` | Records request duration as histogram |
| 88-91 | Error tracking | Increments error counter for 5xx responses |
| 117-133 | Database tracking | Records query time as custom metric |
| 144-157 | Business metrics | Tracks orders with relevant tags |
| 162-185 | Payment metrics | Tracks success/failure rates and amounts |
| 192-219 | Cache metrics | Tracks hit/miss ratios |
| 225-237 | Error handler | Reports errors to APM |

## ⚠️ Common Mistakes

### 1. Not configuring sampling

**What it is**: Sending 100% of traces to APM, causing high costs and performance issues.

**Why it happens**: Default configurations often sample everything.

**How to fix it**: Set appropriate sample rates. For high-traffic apps, sample 10-20% of requests. Use head-based sampling for consistent coverage or tail-based for error-focused tracing.

### 2. Missing environment variables

**What it is**: All environments showing as "development" in APM.

**Why it happens**: Not setting environment tags.

**How to fix it**: Always set `DD_ENV` (Datadog) or `NEW_RELIC_ENV` (New Relic) environment variables.

### 3. Not using custom spans for background jobs

**What it is**: Background jobs and scheduled tasks not appearing in APM.

**Why it happens**: APM auto-instrumentation focuses on HTTP requests.

**How to fix it**: Manually create spans in background job processors using the APM SDK.

## ✅ Quick Recap

- APM tools like Datadog and New Relic provide deep visibility into application performance
- APM agents auto-instrument Express routes, database queries, and HTTP calls
- Use custom metrics for business events (orders, payments, cache hits)
- Track latency percentiles (p50, p95, p99) to understand response time distribution
- Monitor error rates as a key health indicator
- Configure sampling to control costs in high-traffic applications

## 🔗 What's Next

Now that you have APM, learn about exposing metrics via [Prometheus](./04_metrics-with-prometheus.md) for open-source metric collection.
