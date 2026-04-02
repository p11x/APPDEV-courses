# Sentry Setup

## What You'll Learn

- What Sentry is and why error monitoring matters
- How to install and configure @sentry/node
- How to capture exceptions and messages
- How to integrate Sentry with Express
- How to add context (user, tags, breadcrumbs)

## What Is Sentry?

Sentry is an error monitoring service. When your application throws an error in production, Sentry captures it with:
- Full stack trace
- Request details (URL, method, headers)
- User context (who was affected)
- Breadcrumbs (events leading up to the error)

## Project Setup

```bash
npm install @sentry/node
```

Create a Sentry account at [sentry.io](https://sentry.io) and get your DSN (Data Source Name).

## Basic Setup

```js
// app.js — Express app with Sentry error monitoring

import express from 'express';
import * as Sentry from '@sentry/node';

// Initialize Sentry BEFORE creating the Express app
// The DSN identifies your Sentry project
Sentry.init({
  dsn: process.env.SENTRY_DSN || 'https://your-dsn@sentry.io/123456',

  // Performance monitoring — sample 10% of requests for tracing
  tracesSampleRate: 0.1,

  // Environment tag — filter errors by environment
  environment: process.env.NODE_ENV || 'development',

  // Release tag — track which version has the error
  release: process.env.APP_VERSION || '1.0.0',

  // Before sending — filter or modify events
  beforeSend(event) {
    // Do not send events in development
    if (process.env.NODE_ENV === 'development') return null;
    return event;
  },
});

const app = express();

// Sentry request handler — must be the FIRST middleware
// Captures request details (URL, method, headers) for every error
Sentry.setupExpressErrorHandler(app);

// Your routes
app.get('/', (req, res) => {
  res.json({ message: 'Hello' });
});

// This route throws an error — Sentry captures it automatically
app.get('/error', (req, res) => {
  throw new Error('Something went wrong!');
});

// Async errors are also captured
app.get('/async-error', async (req, res) => {
  const data = await fetchFromDB();  // Throws
  res.json(data);
});

async function fetchFromDB() {
  throw new Error('Database connection timeout');
}

// Manual error capture
app.get('/manual', (req, res) => {
  try {
    riskyOperation();
  } catch (err) {
    // Capture the error manually with context
    Sentry.captureException(err, {
      tags: { feature: 'manual-test' },
      extra: { requestBody: req.body },
    });
    res.status(500).json({ error: 'Operation failed' });
  }
});

// Capture messages (not errors)
app.get('/health', (req, res) => {
  const memoryUsage = process.memoryUsage().heapUsed / 1024 / 1024;

  if (memoryUsage > 500) {
    // Alert: high memory usage
    Sentry.captureMessage(`High memory usage: ${memoryUsage.toFixed(1)}MB`, 'warning');
  }

  res.json({ status: 'ok', memory: `${memoryUsage.toFixed(1)}MB` });
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Adding Context

```js
// context.js — Add user and request context to Sentry events

import express from 'express';
import * as Sentry from '@sentry/node';

Sentry.init({ dsn: process.env.SENTRY_DSN });

const app = express();
app.use(express.json());

// Middleware: set user context
app.use((req, res, next) => {
  // After authentication, set the user on the Sentry scope
  // This info appears in every Sentry event for this request
  if (req.user) {
    Sentry.setUser({
      id: req.user.id,
      email: req.user.email,  // PII — consider if you want to send this
      username: req.user.name,
    });
  }

  // Set custom tags — useful for filtering in Sentry dashboard
  Sentry.setTag('endpoint', req.path);
  Sentry.setTag('method', req.method);

  // Set custom context — arbitrary key-value data
  Sentry.setContext('request', {
    ip: req.ip,
    userAgent: req.headers['user-agent'],
  });

  next();
});

// Add breadcrumbs — Sentry shows these in the error timeline
app.get('/process', async (req, res) => {
  // Breadcrumbs are events that lead up to an error
  Sentry.addBreadcrumb({
    category: 'auth',
    message: 'User authenticated',
    level: 'info',
  });

  await step1();
  Sentry.addBreadcrumb({ category: 'process', message: 'Step 1 complete' });

  await step2();
  Sentry.addBreadcrumb({ category: 'process', message: 'Step 2 complete' });

  // If step3 throws, Sentry shows all breadcrumbs above
  await step3();

  res.json({ done: true });
});

async function step1() { /* ... */ }
async function step2() { /* ... */ }
async function step3() {
  throw new Error('Step 3 failed');
}

Sentry.setupExpressErrorHandler(app);
app.listen(3000);
```

## How It Works

### The Sentry Flow

```
Error thrown in your code
    │
    ▼
Sentry catches it (automatic or manual captureException)
    │
    ▼
Sentry attaches context:
  - Stack trace
  - Request URL, method, headers
  - User info
  - Breadcrumbs
  - Tags and extra data
    │
    ▼
Sentry sends to sentry.io (async, non-blocking)
    │
    ▼
You see it in the Sentry dashboard with all context
```

### Scopes

Sentry uses **scopes** to attach context:
- `Sentry.setUser()` — who was affected
- `Sentry.setTag()` — filterable key-value pairs
- `Sentry.setContext()` — arbitrary structured data
- `Sentry.addBreadcrumb()` — timeline of events

## Common Mistakes

### Mistake 1: Not Initializing Sentry First

```js
// WRONG — Sentry must be initialized before other imports that might throw
import express from 'express';
const app = express();
Sentry.init({ dsn: '...' });  // Too late — some errors already missed

// CORRECT — Sentry.init() first
Sentry.init({ dsn: '...' });
import express from 'express';
```

### Mistake 2: Catching Errors Before Sentry Sees Them

```js
// WRONG — the catch block prevents Sentry from seeing the error
try {
  await riskyOperation();
} catch (err) {
  console.log(err);  // Sentry never sees this
}

// CORRECT — let Sentry's error handler catch it, or capture manually
try {
  await riskyOperation();
} catch (err) {
  Sentry.captureException(err);
  console.error(err);
}
```

### Mistake 3: Sending PII Without Consent

```js
// WRONG — sending email addresses without user consent
Sentry.setUser({ email: 'alice@example.com' });

// CORRECT — use IDs instead of PII, or check consent
Sentry.setUser({ id: 'user-123' });  // Safe — no PII
```

## Try It Yourself

### Exercise 1: Capture an Error

Create a route that throws an error. Trigger it, then check the Sentry dashboard for the error with full stack trace.

### Exercise 2: Add User Context

Simulate a logged-in user. Set user context in middleware. Trigger an error and verify the Sentry event shows the user info.

### Exercise 3: Breadcrumbs

Add 3 breadcrumbs before an error. Verify the Sentry event shows them in order.

## Next Steps

You have error monitoring. For health checks and readiness probes, continue to [Health Checks](./02-health-checks.md).
