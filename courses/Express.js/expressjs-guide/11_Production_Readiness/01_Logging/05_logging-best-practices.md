# Logging Best Practices

## 📌 What You'll Learn

- What information to log and what to avoid
- How to handle personally identifiable information (PII)
- When and how to use log sampling for high-traffic applications
- Building a logging strategy that balances observability with performance

## 🧠 Concept Explained (Plain English)

Good logging is like good journalism — include the facts, the context, and the who/what/when/where, but leave out private information. The goal is to have enough information to debug issues and understand system behavior, without overwhelming your storage, slowing down your app, or violating privacy regulations.

**What to log** generally falls into these categories:

- **Request lifecycle**: Incoming requests, outgoing responses, timing
- **Business events**: User actions, order creation, payments processed
- **Errors and exceptions**: What went wrong, stack traces (in non-production)
- **System state**: Startup, shutdown, configuration changes, resource usage

**What NOT to log** includes anything that could:

- Expose security vulnerabilities (API keys, passwords, tokens)
- Violate privacy laws (GDPR, HIPAA, CCPA) — names, emails, addresses, social security numbers
- Create performance issues — excessive logging slows down your application

**Log sampling** is a technique where you only log a percentage of similar events. If your API gets 10,000 requests per second, logging every single one might be unnecessary. You could sample 1% and still get meaningful data while dramatically reducing storage costs and log volume.

**PII scrubbing** is the process of removing or masking personally identifiable information from logs before they're written. This is critical for compliance and security — even if your database is secure, logs containing PII become a liability.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import { v4 as uuidv4 } from 'uuid';
import crypto from 'crypto';
import pino from 'pino';

const app = express();

// PII fields that should never appear in logs
const PII_FIELDS = [
  'password',
  'passwordHash',
  'creditCard',
  'credit_card',
  'cvv',
  'ssn',
  'social_security',
  'apiKey',
  'api_key',
  'secret',
  'token',
  'accessToken',
  'access_token',
  'refreshToken',
  'refresh_token'
];

// Fields to partially mask
const MASKED_FIELDS = [
  'email',
  'phone',
  'address',
  'ip',
  'userAgent'
];

// Recursively scrub sensitive data from objects
function scrubObject(obj, depth = 0) {
  if (depth > 10) return '[max-depth-reached]';
  if (!obj || typeof obj !== 'object') return obj;
  
  const result = Array.isArray(obj) ? [] : {};
  
  for (const [key, value] of Object.entries(obj)) {
    const lowerKey = key.toLowerCase();
    
    // Completely remove PII fields
    if (PII_FIELDS.some(field => lowerKey.includes(field))) {
      result[key] = '[REDACTED]';
      continue;
    }
    
    // Partially mask other sensitive fields
    if (MASKED_FIELDS.some(field => lowerKey.includes(field))) {
      if (typeof value === 'string' && value.length > 4) {
        result[key] = value.slice(0, 2) + '***' + value.slice(-2);
      } else {
        result[key] = value;
      }
      continue;
    }
    
    // Recursively process nested objects
    if (typeof value === 'object' && value !== null) {
      result[key] = scrubObject(value, depth + 1);
    } else {
      result[key] = value;
    }
  }
  
  return result;
}

// Create logger with custom serializers
const logger = pino({
  level: process.env.LOG_LEVEL || 'info',
  
  // Custom serializer that scrubs sensitive data
  serializers: {
    req(req) {
      return {
        id: req.id,
        method: req.method,
        url: scrubObject(req.url),
        headers: {
          host: req.headers.host,
          content_type: req.headers['content-type'],
          user_agent: req.headers['user-agent']
          // Note: Authorization header is intentionally NOT included
        },
        query: scrubObject(req.query),
        params: scrubObject(req.params)
      };
    },
    res(res) {
      return { statusCode: res.statusCode };
    }
  }
});

// Log sampling configuration
const samplingConfig = {
  // Sample rate: 0-1 (0.1 = 10%)
  healthCheck: 0.01,      // 1% of health checks
  apiRequest: 1.0,        // 100% of API requests
  staticAsset: 0.05       // 5% of static assets
};

// Should this request be logged based on sampling?
function shouldLog(req) {
  const path = req.path;
  
  if (path === '/health' || path === '/ready') {
    return Math.random() < samplingConfig.healthCheck;
  }
  
  if (path.startsWith('/static') || path.endsWith('.js') || path.endsWith('.css')) {
    return Math.random() < samplingConfig.staticAsset;
  }
  
  return Math.random() < samplingConfig.apiRequest;
}

// Request logging with sampling and scrubbing
app.use((req, res, next) => {
  // Only log if sampled
  if (!shouldLog(req)) {
    return next();
  }
  
  const start = Date.now();
  const requestId = req.headers['x-request-id'] || uuidv4();
  
  res.on('finish', () => {
    const duration = Date.now() - start;
    
    // Determine log level based on status code
    let level = 'info';
    if (res.statusCode >= 500) level = 'error';
    else if (res.statusCode >= 400) level = 'warn';
    
    // Always include request ID for correlation
    const logData = {
      requestId,
      method: req.method,
      path: req.path,
      status: res.statusCode,
      duration_ms: duration,
      user_agent: req.headers['user-agent'],
      ip: req.ip
    };
    
    logger[level](logData, `${req.method} ${req.path}`);
  });
  
  next();
});

// Example: Safe logging in a route
app.post('/api/users', (req, res) => {
  const { email, name, password } = req.body || {};
  
  // Log with scrubbed data - safe to log
  logger.info({
    action: 'create_user',
    userData: scrubObject({ email, name })
  }, 'Creating new user');
  
  // The password should NEVER be logged, even scrubObject won't help
  // if you pass it directly. Just don't include it in logs.
  
  // Instead of this (DANGEROUS):
  // logger.info({ password }); 
  
  const user = { id: uuidv4(), email, name };
  res.status(201).json(user);
});

app.post('/api/auth/login', (req, res) => {
  const { email } = req.body || {};
  
  // Log successful login
  logger.info({
    action: 'login_success',
    // Never log passwords! But email is generally okay to log
    email, // Consider hashing this too for extra safety
    ip: req.ip
  }, 'User logged in');
  
  res.json({ token: 'jwt-token-here' });
});

app.get('/api/payments/:orderId', (req, res) => {
  const { orderId } = req.params;
  
  // Don't log full payment details
  logger.info({
    action: 'view_payment',
    orderId
  }, 'Payment viewed');
  
  // NEVER log this:
  // logger.info({ creditCard: '4111-1111-1111-1111' });
  
  res.json({ amount: 99.99, status: 'paid' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  logger.info({ port: PORT }, 'Server started');
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 9-22 | `PII_FIELDS` array | Lists field names that should never appear in logs |
| 25-29 | `MASKED_FIELDS` array | Fields to partially mask (e.g., email → jo***@example.com) |
| 32-57 | `scrubObject()` function | Recursively removes or masks sensitive data |
| 36 | `depth > 10` | Prevents infinite recursion on circular references |
| 41-44 | PII check | Completely removes passwords, tokens, API keys |
| 47-53 | Masked fields | Partially masks emails, phones, IPs |
| 57 | Recursive call | Processes nested objects |
| 71-75 | `samplingConfig` | Defines sampling rates for different request types |
| 78-91 | `shouldLog()` | Determines if request should be logged based on sampling |
| 97-118 | Request middleware | Logs requests with sampling and scrubbing |
| 112 | `res.on('finish')` | Logs after response is complete, includes duration |
| 132-138 | Route example | Safe logging with scrubObject |
| 151-156 | Auth route | Shows correct approach to logging logins |
| 162-169 | Payment route | Demonstrates what NOT to log |

## ⚠️ Common Mistakes

### 1. Logging entire request bodies

**What it is**: Logging `req.body` without sanitization, exposing passwords and sensitive data.

**Why it happens**: It's the easiest way to get full context for debugging.

**How to fix it**: Always use a serializer or scrub function, and explicitly list which fields to log.

### 2. Not implementing log sampling

**What it is**: Logging every single request, creating massive log volumes and costs.

**Why it happens**: Developers don't anticipate high traffic or don't know about sampling.

**How to fix it**: Implement sampling for high-volume, low-value endpoints like health checks and static assets.

### 3. Logging without correlation IDs

**What it is**: Multiple logs for one request that can't be connected.

**Why it happens**: Forgetting to add request ID to every log statement.

**How to fix it**: Use AsyncLocalStorage as covered in the previous lesson to automatically include correlation IDs.

## ✅ Quick Recap

- Never log passwords, tokens, credit card numbers, or other sensitive data
- Implement PII scrubbing to automatically remove or mask personal information
- Use log sampling for high-volume, low-value endpoints to reduce storage costs
- Always include correlation IDs in logs for debugging distributed systems
- Create a consistent logging format that includes timing, status codes, and request IDs

## 🔗 What's Next

With logging covered, move on to monitoring your Express application with [Health Check Endpoints](./02_Monitoring_and_Observability/01_health-check-endpoints.md).
