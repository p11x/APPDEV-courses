# Request Logging

## What You'll Learn

- How to log HTTP requests with pino-http
- What correlation IDs are and how they trace requests across services
- How to use AsyncLocalStorage for request-scoped context
- How to add custom fields to every request log
- How to filter sensitive request data

## Project Setup

```bash
npm install express pino pino-http
```

## Basic Request Logging

```js
// server.js — Express with pino-http request logging

import express from 'express';
import pino from 'pino';
import pinoHttp from 'pino-http';

const logger = pino({ level: 'info' });

const app = express();

// pino-http logs every request automatically
app.use(
  pinoHttp({
    logger,  // Use our pino logger instance

    // Custom log message
    customLogLevel(req, res, err) {
      if (res.statusCode >= 500) return 'error';
      if (res.statusCode >= 400) return 'warn';
      return 'info';
    },

    // Custom fields added to every request log
    customProps(req) {
      return {
        // Add request-specific context
        userAgent: req.headers['user-agent'],
        requestId: req.id,  // If using auto-gen IDs
      };
    },

    // Redact sensitive headers from logs
    redact: ['req.headers.authorization', 'req.headers.cookie'],
  })
);

// Routes
app.get('/', (req, res) => {
  // req.log is a child logger with request context
  req.log.info('Handling root route');
  res.json({ message: 'Hello' });
});

app.get('/error', (req, res) => {
  req.log.error('Something went wrong');
  res.status(500).json({ error: 'Internal error' });
});

app.get('/users/:id', (req, res) => {
  req.log.info({ userId: req.params.id }, 'Fetching user');
  res.json({ id: req.params.id, name: 'Alice' });
});

app.listen(3000, () => {
  logger.info('Server on http://localhost:3000');
});
```

### Log Output per Request

```json
{"level":30,"time":"2024-01-15T10:30:00.000Z","req":{"id":1,"method":"GET","url":"/"},"msg":"request completed","responseTime":2,"statusCode":200}
```

## Correlation IDs with AsyncLocalStorage

```js
// correlation.js — Trace requests across async operations

import express from 'express';
import pino from 'pino';
import pinoHttp from 'pino-http';
import { AsyncLocalStorage } from 'node:async_hooks';
import { randomUUID } from 'node:crypto';

// AsyncLocalStorage stores data per async execution context
// Each request gets its own isolated storage
const asyncLocalStorage = new AsyncLocalStorage();

const logger = pino({ level: 'info' });

const app = express();

// Middleware: generate correlation ID and store in AsyncLocalStorage
app.use((req, res, next) => {
  // Use the client's correlation ID or generate a new one
  const correlationId = req.headers['x-correlation-id'] || randomUUID();

  // Store in AsyncLocalStorage — all async code in this request can access it
  const store = { correlationId, userId: null };
  asyncLocalStorage.run(store, () => {
    // Attach to request for downstream use
    req.correlationId = correlationId;
    res.setHeader('x-correlation-id', correlationId);
    next();
  });
});

// pino-http with correlation ID in every log
app.use(
  pinoHttp({
    logger,
    customProps() {
      // Read from AsyncLocalStorage — available in any async context
      const store = asyncLocalStorage.getStore();
      return {
        correlationId: store?.correlationId,
      };
    },
  })
);

// Helper: get the current correlation ID from any async context
function getCorrelationId() {
  return asyncLocalStorage.getStore()?.correlationId;
}

// Helper: create a child logger with correlation ID
function getLogger() {
  const store = asyncLocalStorage.getStore();
  return logger.child({ correlationId: store?.correlationId });
}

// Simulated service layer — correlation ID flows through automatically
async function fetchUser(userId) {
  const log = getLogger();
  log.info({ userId }, 'Querying database');

  // Simulate DB query
  await new Promise((r) => setTimeout(r, 50));

  log.info({ userId }, 'Database query complete');
  return { id: userId, name: 'Alice' };
}

async function sendNotification(userId, message) {
  const log = getLogger();
  log.info({ userId, message }, 'Sending notification');
  await new Promise((r) => setTimeout(r, 30));
}

// Routes
app.get('/users/:id', async (req, res) => {
  const user = await fetchUser(req.params.id);
  await sendNotification(req.params.id, 'Profile viewed');
  res.json(user);
});

app.listen(3000, () => {
  logger.info('Server on http://localhost:3000');
});
```

### Log Output

All log lines for the same request share the same `correlationId`:

```json
{"level":30,"msg":"request completed","correlationId":"abc-123","method":"GET","url":"/users/1"}
{"level":30,"msg":"Querying database","correlationId":"abc-123","userId":"1"}
{"level":30,"msg":"Database query complete","correlationId":"abc-123","userId":"1"}
{"level":30,"msg":"Sending notification","correlationId":"abc-123","userId":"1"}
```

Search for `correlationId=abc-123` to see all operations for that request.

## How It Works

### AsyncLocalStorage

```
Request A ──→ asyncLocalStorage.run({ correlationId: 'A' })
  ├── fetchUser() → asyncLocalStorage.getStore() → { correlationId: 'A' }
  └── sendNotification() → asyncLocalStorage.getStore() → { correlationId: 'A' }

Request B ──→ asyncLocalStorage.run({ correlationId: 'B' })
  ├── fetchUser() → asyncLocalStorage.getStore() → { correlationId: 'B' }
  └── sendNotification() → asyncLocalStorage.getStore() → { correlationId: 'B' }
```

Each request runs in its own context. The store is isolated — no cross-contamination.

### Why Correlation IDs?

In a microservices architecture, a single user request may touch 5 services. A correlation ID ties all logs together:

```
API Gateway → Auth Service → User Service → Database
  (req-abc)     (req-abc)      (req-abc)     (req-abc)
```

Search for `correlationId=req-abc` across all services to trace the entire request path.

## Common Mistakes

### Mistake 1: Creating New Logger Per Request

```js
// WRONG — new logger instance per request (wastes memory)
app.use((req, res, next) => {
  req.log = pino();  // New logger every request
  next();
});

// CORRECT — use pino-http or child loggers
app.use(pinoHttp({ logger }));
```

### Mistake 2: Not Propagating Correlation IDs

```js
// WRONG — downstream services have no context
const response = await fetch('http://other-service/api/data');

// CORRECT — pass the correlation ID in headers
const correlationId = getCorrelationId();
const response = await fetch('http://other-service/api/data', {
  headers: { 'x-correlation-id': correlationId },
});
```

### Mistake 3: Logging Full Request Bodies

```js
// WRONG — request body may contain passwords, tokens, PII
req.log.info({ body: req.body }, 'Received request');

// CORRECT — log only safe fields
req.log.info({ action: req.body.action }, 'Received request');
```

## Try It Yourself

### Exercise 1: Request Timing

Add middleware that logs the request duration in milliseconds. Log a warning if the request takes longer than 1 second.

### Exercise 2: User Context

After authentication, add the user ID to AsyncLocalStorage. Verify it appears in all downstream logs.

### Exercise 3: Log Sampling

Implement log sampling: only log 10% of successful requests (to reduce volume) but log 100% of errors.

## Next Steps

You have request logging with correlation IDs. For production log routing, continue to [Log Transport](./03-log-transport.md).
