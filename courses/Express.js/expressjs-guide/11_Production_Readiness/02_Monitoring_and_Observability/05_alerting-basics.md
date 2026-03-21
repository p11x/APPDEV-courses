# Alerting Basics

## 📌 What You'll Learn

- How to create effective alerts that catch real problems
- Understanding SLOs vs SLAs and how to use them
- Avoiding alert fatigue through smart alerting strategies
- Overview of alerting tools like PagerDuty and Opsgenie

## 🧠 Concept Explained (Plain English)

Alerting is about notifying the right people at the right time when something needs attention. Too many alerts and people start ignoring them (alert fatigue). Too few alerts and real problems go unnoticed.

The key insight is that **not every error should be an alert**. An alert should only fire when:
1. Something is broken that needs immediate human attention
2. A human can actually do something about it

**What to alert on:**
- Error rates exceeding a threshold (e.g., > 1% of requests failing)
- Latency degradation (e.g., p99 > 2 seconds)
- System resource exhaustion (memory, CPU, disk)
- Dependencies being down

**What NOT to alert on:**
- Every single error (you'll get thousands of alerts)
- Recovered issues (self-healing doesn't need human notification)
- Expected failures (like a bad user input)

**SLA vs SLO vs SLI** — these get confusing:

- **SLI** (Service Level Indicator): A quantitative measure of some aspect of the service. Example: "99.5% of requests succeed"
- **SLO** (Service Level Objective): A target value for your SLI. Example: "We want 99.5% of requests to succeed"
- **SLA** (Service Level Agreement): A contract with customers. Example: "If we go below 99.5%, they get a refund"

Think of it as: SLI is what you measure, SLO is what you want, SLA is what you promise.

**Alert severity levels:**
- **Critical (P1)**: Service down, needs immediate response
- **High (P2)**: Significant degradation, needs response within 1 hour
- **Medium (P3)**: Minor issues, needs response within 4 hours
- **Low (P4)**: Informational, no immediate response needed

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import client from 'prom-client';

// Create registry and metrics
const register = new client.Registry();
client.collectDefaultMetrics({ register });

// ============================================
// Application Metrics
// ============================================

const httpRequestsTotal = new client.Counter({
  name: 'http_requests_total',
  help: 'Total HTTP requests',
  labelNames: ['method', 'status', 'route'],
  registers: [register]
});

const httpRequestDuration = new client.Histogram({
  name: 'http_request_duration_seconds',
  help: 'Request duration',
  labelNames: ['method', 'status', 'route'],
  buckets: [0.01, 0.05, 0.1, 0.25, 0.5, 1, 2.5, 5],
  registers: [register]
});

const activeConnections = new client.Gauge({
  name: 'active_connections',
  help: 'Active connections',
  registers: [register]
});

// Business metrics
const ordersTotal = new client.Counter({
  name: 'orders_total',
  help: 'Total orders',
  labelNames: ['status'],
  registers: [register]
});

const databaseConnections = new client.Gauge({
  name: 'database_connections_active',
  help: 'Active database connections',
  registers: [register]
});


const app = express();

// Track connections
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
// Simulated Database Metrics (in production, these come from DB)
// ============================================
setInterval(() => {
  // Simulate connection pool usage (0-100)
  const usage = Math.floor(Math.random() * 100);
  databaseConnections.set(usage);
}, 5000);


// ============================================
// Request Middleware
// ============================================
app.use((req, res, next) => {
  const start = Date.now();
  const route = req.route?.path || req.path.replace(/\/\d+/g, '/:id');
  
  res.on('finish', () => {
    const duration = (Date.now() - start) / 1000;
    httpRequestsTotal.inc({ method: req.method, status: res.statusCode, route });
    httpRequestDuration.observe({ method: req.method, status: res.statusCode, route }, duration);
  });
  
  next();
});


// ============================================
// Metrics Endpoint
// ============================================
app.get('/metrics', async (req, res) => {
  res.set('Content-Type', register.contentType);
  res.end(await register.metrics());
});


// ============================================
// Alert Configuration Examples
// ============================================

/*
 * These are example Prometheus alert rules that would be configured
 * in your Prometheus/Alerta configuration, NOT in the Express app.
 * 
 * But understanding them helps you know what metrics to expose!
 */

// ALERTING RULES (would be in prometheus/rules.yml):
/*
groups:
  - name: express-alerts
    interval: 30s
    rules:
      # High error rate - Critical
      - alert: HighErrorRate
        expr: |
          sum(rate(http_requests_total{status=~"5.."}[5m])) 
          / sum(rate(http_requests_total[5m])) > 0.01
        for: 2m
        labels:
          severity: critical
        annotations:
          summary: "High error rate detected"
          description: "Error rate is {{ $value | humanizePercentage }} (threshold: 1%)"
      
      # High latency - Warning
      - alert: HighLatency
        expr: |
          histogram_quantile(0.99, 
            sum(rate(http_request_duration_seconds_bucket[5m])) by (le)
          ) > 2
        for: 5m
        labels:
          severity: warning
        annotations:
          summary: "High request latency"
          description: "p99 latency is {{ $value }}s (threshold: 2s)"
      
      # Database connection pool exhausted - Critical
      - alert: DatabaseConnectionsHigh
        expr: database_connections_active > 90
        for: 1m
        labels:
          severity: critical
        annotations:
          summary: "Database connection pool nearly full"
          description: "Using {{ $value }} connections"
      
      # No orders in 10 minutes - Warning
      - alert: NoOrdersReceived
        expr: |
          sum(rate(orders_total[10m])) == 0
        for: 10m
        labels:
          severity: warning
        annotations:
          summary: "No orders received in 10 minutes"
          description: "Possible payment or ordering service issue"
      
      # Active connections too high - Warning
      - alert: HighActiveConnections
        expr: active_connections > 1000
        for: 2m
        labels:
          severity: warning
        annotations:
          summary: "High number of active connections"
          description: "{{ $value }} active connections"
      
      # Instance down - Critical
      - alert: InstanceDown
        expr: up{job="express-app"} == 0
        for: 1m
        labels:
          severity: critical
        annotations:
          summary: "Instance is down"
          description: "Express app instance has been down for 1 minute"
*/


// ============================================
// Example Routes
// ============================================

app.get('/api/health', (req, res) => {
  res.json({ status: 'ok' });
});

app.post('/api/orders', async (req, res) => {
  const { total } = req.body || {};
  
  try {
    // Simulate order processing
    await new Promise(resolve => setTimeout(resolve, 100));
    
    ordersTotal.inc({ status: 'success' });
    res.status(201).json({ orderId: 'order-123' });
  } catch (error) {
    ordersTotal.inc({ status: 'failed' });
    res.status(500).json({ error: 'Order failed' });
  }
});

app.get('/api/users/:id', async (req, res) => {
  await new Promise(resolve => setTimeout(resolve, 50));
  res.json({ id: req.params.id, name: 'John' });
});


// ============================================
// Sample Alert Payload (for integration with alerting tools)
// ============================================

/*
 * Example webhook payload that would be sent to PagerDuty/Opsgenie
 * when an alert triggers:
 * 
 {
   "routing_key": "...",
   "event_action": "trigger",
   "payload": {
     "summary": "High Error Rate - My Express App",
     "severity": "critical",
     "source": "prometheus",
     "timestamp": "2024-01-15T10:30:00Z",
     "labels": {
       "service": "my-express-app",
       "environment": "production"
     },
     "annotations": {
       "description": "Error rate is 2.5% (threshold: 1%)"
     }
   }
 }
 */


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log(`Metrics at http://localhost:${PORT}/metrics`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 9-14 | Registry setup | Creates Prometheus metrics registry |
| 20-25 | `httpRequestsTotal` counter | Counts all HTTP requests |
| 30-34 | `httpRequestDuration` histogram | Measures request latency |
| 40-43 | `databaseConnections` gauge | Tracks DB pool usage |
| 68-82 | Request middleware | Records metrics per request |
| 95-102 | `/metrics` endpoint | Prometheus scrapes this |
| 109-120 | `HighErrorRate` alert | Triggers when >1% errors for 2+ minutes |
| 123-132 | `HighLatency` alert | Triggers when p99 > 2 seconds |
| 135-143 | `DatabaseConnectionsHigh` alert | Triggers when DB pool > 90% |
| 146-153 | `NoOrdersReceived` alert | Business alert for 10 min of no orders |
| 156-163 | `HighActiveConnections` alert | Connection limit warning |
| 166-174 | `InstanceDown` alert | Basic uptime check |

## ⚠️ Common Mistakes

### 1. Alerting on every error

**What it is**: Creating alerts that fire for every 500 error.

**Why it happens**: Wanting to catch all problems, but this creates alert storms.

**How to fix it**: Use rates over time windows (e.g., "error rate > 1% over 5 minutes") rather than counting individual errors.

### 2. No clear ownership

**What it is**: Alerts fire but nobody knows who's responsible.

**Why it happens**: Not assigning escalation policies or teams.

**How to fix it**: Integrate with PagerDuty/Opsgenie and define on-call schedules. Use tags to route to correct team.

### 3. Alert windows too short

**What it is**: Alerts fire for temporary blips that resolve themselves.

**Why it happens**: Using `for: 0s` or very short windows.

**How to fix it**: Use `for: 2m` or longer to ensure the issue persists before alerting. Set reasonable `evaluation_interval`.

## ✅ Quick Recap

- Alerts should only fire for actionable issues requiring human intervention
- Use SLI (what you measure), SLO (what you want), and SLA (what you promise)
- Alert on rates over time windows, not individual events
- Use severity levels (critical, warning, info) appropriately
- Integrate with PagerDuty, Opsgenie, or similar for escalation management
- Include "for" duration to prevent alerting on temporary blips

## 🔗 What's Next

Now let's move to the Resilience section. Learn how to handle [Graceful Shutdown](./03_Resilience/01_graceful-shutdown.md) to handle process termination cleanly.
