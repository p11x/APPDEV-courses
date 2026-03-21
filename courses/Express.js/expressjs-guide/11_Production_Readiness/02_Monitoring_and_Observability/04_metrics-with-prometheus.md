# Metrics with Prometheus

## 📌 What You'll Learn

- What Prometheus is and how it differs from traditional monitoring
- How to expose Prometheus metrics from an Express application
- Understanding metric types: counters, gauges, and histograms
- Creating custom metrics for business and application monitoring

## 🧠 Concept Explained (Plain English)

Prometheus is an open-source monitoring system that works differently from commercial APM tools. Instead of having an agent that pushes data to a central server, Prometheus **pulls** data from your application via HTTP endpoints.

Here's how it works:
1. Your Express app exposes a `/metrics` endpoint
2. Prometheus server periodically scrapes (pulls) this endpoint every 15 seconds by default
3. Data is stored in a time-series database
4. You query data using PromQL (Prometheus Query Language)
5. Grafana visualizes the data

**Why use Prometheus?** It's open-source, lightweight, and integrates with many systems. It's ideal if you want open standards without vendor lock-in.

The key **metric types**:

- **Counter**: Always increases (like an odometer). Use for: total requests, total errors, items processed. Never decreases.
- **Gauge**: Can go up and down. Use for: current memory usage, number of active connections, temperature.
- **Histogram**: Buckets values into ranges. Use for: request duration, response sizes. Great for calculating percentiles.

**Labels** are like tags that let you slice and dice metrics. Instead of one "request duration" metric, you can have one metric with labels: `method="GET", status="200"`, `method="POST", status="201"`, etc.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import client from 'prom-client';

// Create a Registry - this groups all your metrics
const register = new client.Registry();

// Add default metrics (CPU, memory, event loop, etc.)
client.collectDefaultMetrics({ register });

// ============================================
// Define Custom Metrics
// ============================================

// Counter: total number of HTTP requests
const httpRequestsTotal = new client.Counter({
  name: 'http_requests_total',
  help: 'Total number of HTTP requests',
  labelNames: ['method', 'status', 'route'],
  registers: [register]
});

// Counter: total number of business events
const ordersCreated = new client.Counter({
  name: 'orders_created_total',
  help: 'Total number of orders created',
  labelNames: ['status', 'channel'],
  registers: [register]
});

// Gauge: current active connections
const activeConnections = new client.Gauge({
  name: 'active_connections',
  help: 'Number of currently active connections',
  registers: [register]
});

// Gauge: cache size
const cacheSize = new client.Gauge({
  name: 'cache_size',
  help: 'Number of items in cache',
  labelNames: ['cache_name'],
  registers: [register]
});

// Histogram: request duration in seconds
const httpRequestDuration = new client.Histogram({
  name: 'http_request_duration_seconds',
  help: 'Duration of HTTP requests in seconds',
  labelNames: ['method', 'status', 'route'],
  buckets: [0.01, 0.05, 0.1, 0.25, 0.5, 1, 2.5, 5, 10],
  registers: [register]
});

// Histogram: response size in bytes
const httpResponseSize = new client.Histogram({
  name: 'http_response_size_bytes',
  help: 'Size of HTTP response in bytes',
  labelNames: ['method', 'status'],
  buckets: [100, 500, 1000, 5000, 10000, 50000],
  registers: [register]
});

// Histogram: order value
const orderValue = new client.Histogram({
  name: 'order_value_usd',
  help: 'Value of orders in USD',
  labelNames: ['currency'],
  buckets: [10, 25, 50, 100, 250, 500, 1000, 2500, 5000],
  registers: [register]
});

// Summary: alternative to histogram (calculates percentiles server-side)
const dbQueryDuration = new client.Summary({
  name: 'db_query_duration_seconds',
  help: 'Database query duration in seconds',
  labelNames: ['operation', 'table'],
  percentiles: [0.5, 0.9, 0.95, 0.99],
  registers: [register]
});


const app = express();

// Track active connections
let connections = 0;

app.on('connection', (socket) => {
  connections++;
  activeConnections.inc();
  
  socket.on('close', () => {
    connections--;
    activeConnections.dec();
  });
});


// ============================================
// Metrics middleware
// ============================================
app.use((req, res, next) => {
  const start = Date.now();
  
  // Get route pattern (remove IDs, numbers for consistent grouping)
  const route = req.route?.path || req.path.replace(/\/\d+/g, '/:id');
  
  res.on('finish', () => {
    const duration = (Date.now() - start) / 1000;
    const status = res.statusCode;
    const method = req.method;
    
    // Increment counter
    httpRequestsTotal.inc({ method, status, route });
    
    // Record duration histogram
    httpRequestDuration.observe({ method, status, route }, duration);
    
    // Record response size
    const responseSize = res.get('Content-Length') || 0;
    httpResponseSize.observe({ method, status }, parseInt(responseSize));
  });
  
  next();
});


// ============================================
// Metrics endpoint (Prometheus scrapes this)
// ============================================
app.get('/metrics', async (req, res) => {
  try {
    res.set('Content-Type', register.contentType);
    res.end(await register.metrics());
  } catch (error) {
    res.status(500).end(error.message);
  }
});

// Individual metric endpoints (useful for debugging)
app.get('/metrics/counters', (req, res) => {
  res.set('Content-Type', register.contentType);
  res.end(await register.getSingleMetricAsString('http_requests_total'));
});


// ============================================
// Example routes with custom metrics
// ============================================

// Simple endpoint
app.get('/api/health', (req, res) => {
  res.json({ status: 'ok' });
});

// Database query with custom metric
app.get('/api/users/:id', async (req, res) => {
  const start = Date.now();
  const userId = req.params.id;
  
  try {
    // Simulate database query
    await new Promise(resolve => setTimeout(resolve, 50));
    
    // Record query duration
    dbQueryDuration.observe(
      { operation: 'select', table: 'users' },
      (Date.now() - start) / 1000
    );
    
    res.json({ id: userId, name: 'John Doe' });
  } catch (error) {
    res.status(500).json({ error: 'Database error' });
  }
});

// Order creation with business metrics
app.post('/api/orders', async (req, res) => {
  const { items, total, channel } = req.body || {};
  
  try {
    // Simulate processing
    await new Promise(resolve => setTimeout(resolve, 100));
    
    const orderTotal = total || 99.99;
    const orderStatus = 'completed';
    
    // Increment order counter
    ordersCreated.inc({ status: orderStatus, channel: channel || 'web' });
    
    // Record order value histogram
    orderValue.observe({ currency: 'USD' }, orderTotal);
    
    // Update cache size gauge
    cacheSize.inc({ cache_name: 'orders' });
    
    res.status(201).json({ 
      id: 'order-123', 
      status: orderStatus,
      total: orderTotal 
    });
  } catch (error) {
    ordersCreated.inc({ status: 'failed', channel: channel || 'web' });
    res.status(500).json({ error: 'Order failed' });
  }
});

// Cache tracking
const productCache = new Map();

app.get('/api/products/:id', (req, res) => {
  const productId = req.params.id;
  
  if (productCache.has(productId)) {
    cacheSize.set({ cache_name: 'products' }, productCache.size);
    return res.json(productCache.get(productId));
  }
  
  const product = { id: productId, name: 'Widget', price: 29.99 };
  productCache.set(productId, product);
  cacheSize.set({ cache_name: 'products' }, productCache.size);
  
  res.json(product);
});


// ============================================
// Custom application info
// ============================================
const appInfo = new client.Gauge({
  name: 'app_info',
  help: 'Application information',
  labelNames: ['version', 'name'],
  registers: [register]
});

appInfo.labels({ version: process.env.APP_VERSION || '1.0.0', name: 'my-express-app' }).set(1);


// Error handling middleware
app.use((err, req, res, next) => {
  httpRequestsTotal.inc({ 
    method: req.method, 
    status: 500, 
    route: 'error' 
  });
  
  res.status(500).json({ error: 'Internal server error' });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log(`Metrics available at http://localhost:${PORT}/metrics`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 5 | `new client.Registry()` | Creates a registry to hold all metrics |
| 8 | `collectDefaultMetrics()` | Collects Node.js built-in metrics (CPU, memory, etc.) |
| 15-20 | `new client.Counter()` | Defines counter for total requests |
| 26-30 | `ordersCreated` counter | Business metric for orders |
| 35-39 | `new client.Gauge()` | Gauge for active connections (can go up/down) |
| 45-50 | `cacheSize` gauge | Tracks cache size |
| 55-60 | `new client.Histogram()` | Buckets request durations |
| 64-68 | `httpResponseSize` histogram | Response size distribution |
| 72-78 | `orderValue` histogram | Business metric for order values |
| 82-90 | `new client.Summary()` | Alternative to histogram with server-side percentiles |
| 107-130 | Metrics middleware | Records metrics on every request |
| 110-112 | Route normalization | Converts `/users/123` to `/users/:id` |
| 138-143 | `/metrics` endpoint | Prometheus scrapes this endpoint |
| 171-185 | `/api/users/:id` route | Database query with timing metric |
| 191-210 | Order creation | Records business metrics |
| 222-234 | Cache endpoint | Tracks cache size |

## ⚠️ Common Mistakes

### 1. Using summary instead of histogram

**What it is**: Can't calculate percentiles (p95, p99) from summaries.

**Why it happens**: Summaries calculate percentiles in the app, losing the ability to aggregate.

**How to fix it**: Use histograms, which bucket values and let Prometheus calculate percentiles across all instances.

### 2. Cardinality explosion

**What it is**: Too many unique label combinations causing memory issues.

**Why it happens**: Using high-cardinality values like user IDs or timestamps as labels.

**How to fix it**: Use static or low-cardinality labels. Instead of `user_id: "12345"`, use `user_type: "premium"`.

### 3. Not normalizing route paths

**What it is**: Every unique user ID creates a new time series.

**Why it happens**: Using raw paths like `/users/123`, `/users/456`.

**How to fix it**: Normalize routes to patterns like `/users/:id`.

## ✅ Quick Recap

- Prometheus pulls metrics from `/metrics` endpoint via HTTP
- Counters always increase, gauges go up and down, histograms bucket values
- Use histograms for request duration to calculate percentiles
- Normalize route paths to prevent cardinality explosion
- Add business metrics (orders, payments) alongside technical metrics
- Prometheus integrates with Grafana for visualization

## 🔗 What's Next

Now that you have metrics, learn about [Alerting Basics](./05_alerting-basics.md) to get notified when things go wrong.
