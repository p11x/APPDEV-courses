# Log Correlation IDs

## 📌 What You'll Learn

- What correlation IDs are and why they matter in distributed systems
- How to generate and attach unique request IDs to every HTTP request
- Using AsyncLocalStorage to propagate IDs across async operations
- How to include correlation IDs in all log entries automatically

## 🧠 Concept Explained (Plain English)

Imagine a user makes a request to your API, which then calls your database, which calls an external payment service, which calls a fraud detection API. When something goes wrong, you need to trace the entire journey. Without correlation IDs, you'd have to match up timestamps across multiple log files — tedious and error-prone.

A **correlation ID** (also called trace ID or request ID) is a unique identifier that follows a request through every step of its journey. Every log entry, every database query, every external API call includes this same ID. When troubleshooting, you can filter all logs by that single ID and see the complete story of what happened.

In Express, correlation IDs typically come from a header like `X-Request-ID` (from a load balancer) or are generated fresh if not provided. The ID needs to be available throughout the entire request lifecycle, including inside asynchronous callbacks — that's where `AsyncLocalStorage` comes in.

**AsyncLocalStorage** is a Node.js feature that creates a "storage" that persists across async operations. Think of it as a global variable that automatically flows through `.then()`, `await`, callbacks, and even setTimeout. This is crucial because in Express, your code makes many async calls, and you don't want to manually pass the correlation ID through every function.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import { v4 as uuidv4 } from 'uuid';
import { AsyncLocalStorage } from 'async_hooks';
import pino from 'pino';

const app = express();

// Create AsyncLocalStorage instance for context propagation
const asyncLocalStorage = new AsyncLocalStorage();

// Create logger - correlation ID will be added automatically
const logger = pino({
  level: process.env.LOG_LEVEL || 'info',
  
  // Add correlation ID to every log entry
  formatters: {
    log(object) {
      const store = asyncLocalStorage.getStore();
      return {
        ...object,
        traceId: store?.traceId || 'no-trace-id',
        timestamp: undefined // Let Pino handle timestamp
      };
    }
  }
});

// Middleware to extract or generate correlation ID
app.use((req, res, next) => {
  // Use existing ID from header if present (from load balancer, etc.)
  // Common headers: X-Request-ID, X-Correlation-ID, X-Trace-ID
  const traceId = 
    req.headers['x-request-id'] || 
    req.headers['x-correlation-id'] || 
    req.headers['x-trace-id'] || 
    uuidv4();
  
  // Store in AsyncLocalStorage
  const store = { traceId, request: { method: req.method, url: req.url } };
  
  asyncLocalStorage.run(store, () => {
    // Attach to request for easy access in routes
    req.traceId = traceId;
    
    // Set response header so client can reference the ID
    res.setHeader('X-Trace-ID', traceId);
    
    // Log the incoming request
    const store = asyncLocalStorage.getStore();
    logger.info({ 
      traceId, 
      method: req.method, 
      url: req.url 
    }, 'Incoming request');
    
    next();
  });
});

// Utility function to get logger with correlation context
function getLogger() {
  return logger;
}

// Simulated database service with correlation ID
async function fetchUserFromDb(userId) {
  const store = asyncLocalStorage.getStore();
  const traceId = store?.traceId;
  
  // This log includes the traceId automatically
  logger.debug({ userId, traceId }, 'Querying database for user');
  
  // Simulate async database call
  await new Promise(resolve => setTimeout(resolve, 10));
  
  logger.debug({ userId, traceId }, 'Database query complete');
  return { id: userId, name: 'John Doe', email: 'john@example.com' };
}

// Simulated external API call with correlation ID propagation
async function callPaymentService(amount) {
  const store = asyncLocalStorage.getStore();
  const traceId = store?.traceId;
  
  logger.info({ amount, traceId }, 'Calling payment service');
  
  // Simulate external API call
  await new Promise(resolve => setTimeout(resolve, 50));
  
  logger.info({ amount, traceId }, 'Payment service response received');
  return { transactionId: 'txn-123', status: 'success' };
}

// Example route demonstrating correlation ID usage
app.get('/api/users/:id', async (req, res) => {
  const { id } = req.params;
  const traceId = req.traceId;
  
  try {
    // Database call - traceId flows automatically via AsyncLocalStorage
    const user = await fetchUserFromDb(id);
    
    logger.info({ userId: id, traceId }, 'User retrieved successfully');
    res.json(user);
  } catch (error) {
    logger.error({ error: error.message, userId: id, traceId }, 'Failed to fetch user');
    res.status(500).json({ error: 'Failed to fetch user' });
  }
});

app.post('/api/purchase', async (req, res) => {
  const { amount } = req.body || {};
  const traceId = req.traceId;
  
  logger.info({ amount, traceId }, 'Processing purchase');
  
  try {
    // Call multiple services - all get the same traceId
    const paymentResult = await callPaymentService(amount);
    
    // This also works with AsyncLocalStorage
    logger.info({ 
      transactionId: paymentResult.transactionId, 
      traceId 
    }, 'Purchase completed');
    
    res.json({ status: 'success', transactionId: paymentResult.transactionId });
  } catch (error) {
    logger.error({ error: error.message, traceId }, 'Purchase failed');
    res.status(500).json({ error: 'Purchase failed' });
  }
});

// Error handler that includes correlation ID
app.use((err, req, res, next) => {
  logger.error({ 
    err, 
    traceId: req.traceId,
    url: req.url,
    method: req.method 
  }, 'Unhandled error');
  
  res.status(500).json({ error: 'Internal server error', traceId: req.traceId });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  logger.info({ port: PORT }, 'Server started');
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 7 | `new AsyncLocalStorage()` | Creates storage that persists across async calls |
| 14-22 | `formatters.log()` | Automatically adds traceId to every log entry |
| 29-30 | `req.headers['x-request-id']` | Checks for existing correlation ID from upstream |
| 32 | `uuidv4()` | Generates new UUID if no header provided |
| 37 | `asyncLocalStorage.run(store, () => {})` | Runs callback with storage context |
| 40 | `req.traceId = traceId` | Attaches ID to request object for easy access |
| 42 | `res.setHeader('X-Trace-ID', traceId)` | Returns ID to client for reference |
| 55-60 | `fetchUserFromDb()` | Database call automatically gets traceId from storage |
| 64-72 | `callPaymentService()` | External API call with correlation context |
| 78-85 | `app.get('/api/users/:id')` | Route using async operations with traceId |
| 99-110 | `app.post('/api/purchase')` | Complex route calling multiple services |

## ⚠️ Common Mistakes

### 1. Not generating a new ID when none is provided

**What it is**: Using an empty or undefined correlation ID, breaking traceability.

**Why it happens**: Assuming upstream systems always provide the header.

**How to fix it**: Always generate a new UUID as a fallback: `req.headers['x-request-id'] || uuidv4()`.

### 2. Forgetting AsyncLocalStorage in async handlers

**What it is**: Logs inside callbacks or Promise.then() lose the correlation ID.

**Why it happens**: Regular variables don't flow through async boundaries, but AsyncLocalStorage does.

**How to fix it**: Always access correlation ID from AsyncLocalStorage: `asyncLocalStorage.getStore()?.traceId`.

### 3. Not passing correlation ID to external services

**What it is**: External API calls don't include the trace ID, breaking end-to-end tracing.

**Why it happens**: Forgetting to add the header when making outbound requests.

**How to fix it**: Create an HTTP client wrapper that automatically adds `X-Request-ID` to all outgoing requests.

## ✅ Quick Recap

- Correlation IDs trace requests across all services and async operations
- Use `X-Request-ID` header if provided, otherwise generate with UUID
- AsyncLocalStorage propagates the ID through all async boundaries automatically
- Include correlation ID in all logs for filtering and debugging
- Return the ID in response headers so clients can reference it

## 🔗 What's Next

Now that you can trace requests end-to-end, learn best practices for what to log in [Logging Best Practices](./05_logging-best-practices.md).
