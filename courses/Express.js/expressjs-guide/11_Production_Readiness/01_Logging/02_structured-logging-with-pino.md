# Structured Logging with Pino

## 📌 What You'll Learn

- How to install and configure Pino, a high-performance JSON logger
- How to use pino-http middleware for automatic request logging
- How to configure log levels and customize output
- Why Pino is significantly faster than console.log

## 🧠 Concept Explained (Plain English)

Pino is a super fast JSON logger for Node.js. "JSON logger" means instead of writing human-readable text like "Error: User not found", it writes machine-readable JSON like `{"level":30,"time":1617955768092,"msg":"User not found"}`. This matters because:

1. **Machine readability**: Log aggregation tools (Datadog, Splunk, ELK stack) can parse JSON automatically
2. **Consistency**: Every log has the same structure, making queries and filtering reliable
3. **Performance**: Pino is one of the fastest JSON loggers available — it's heavily optimized

The "structured" part refers to the fact that each piece of information (timestamp, level, message, metadata) is a separate field in the JSON object. This allows you to do powerful searches like "give me all logs where level is greater than 50 and the userId field exists."

In Express, you'll typically use `pino-http`, which is middleware that automatically logs every HTTP request with useful information like response time, status code, and request details.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import pino from 'pino';
import pinoHttp from 'pino-http';

const app = express();

// Create Pino logger instance
const logger = pino({
  // Only show warnings and errors in production
  level: process.env.LOG_LEVEL || 'info',
  
  // Custom serializers to control what gets logged
  serializers: {
    req(req) {
      return {
        id: req.id,
        method: req.method,
        url: req.url,
        path: req.path,
        parameters: req.query,
        headers: { 
          // Remove sensitive headers
          host: req.headers.host,
          user_agent: req.headers['user-agent']
        }
      };
    },
    res(res) {
      return {
        statusCode: res.statusCode
      };
    }
  },
  
  // Add extra attributes to every log
  base: {
    service: 'my-express-app',
    environment: process.env.NODE_ENV || 'development'
  },
  
  // Pretty print in development only
  transport: process.env.NODE_ENV !== 'production' 
    ? { target: 'pino-pretty' }
    : undefined
});

// Use pino-http middleware for automatic request logging
// This adds req.log and res.log to each request
app.use(pinoHttp({ 
  logger,
  
  // Custom log level based on status code
  customSuccessMessage: (req, res) => {
    return `${req.method} ${req.url} completed`;
  },
  
  customErrorMessage: (req, res, err) => {
    return `${req.method} ${req.url} failed: ${err.message}`;
  },
  
  // Don't log health check endpoints
  autoLogging: {
    ignore: (req) => req.url === '/health'
  }
}));

// Example route with manual logging
app.get('/api/users/:id', (req, res) => {
  const userId = req.params.id;
  
  // req.log is provided by pino-http
  req.log.debug({ userId }, 'Fetching user from database');
  
  // Simulate database lookup
  const user = { id: userId, name: 'John Doe', email: 'john@example.com' };
  
  if (!user) {
    req.log.warn({ userId }, 'User not found');
    return res.status(404).json({ error: 'User not found' });
  }
  
  req.log.info({ userId, user }, 'User retrieved successfully');
  res.json(user);
});

app.post('/api/orders', (req, res) => {
  const { items, customerId } = req.body || {};
  
  req.log.info({ items: items?.length, customerId }, 'Processing new order');
  
  try {
    // Simulate order processing
    const order = { id: 'order-123', status: 'processing' };
    
    req.log.info({ orderId: order.id }, 'Order created successfully');
    res.status(201).json(order);
  } catch (error) {
    req.log.error({ error: error.message, customerId }, 'Failed to create order');
    res.status(500).json({ error: 'Failed to process order' });
  }
});

// Error handler that logs errors
app.use((err, req, res, next) => {
  req.log.error({ 
    err, 
    url: req.url, 
    method: req.method 
  }, 'Unhandled error');
  
  res.status(500).json({ error: 'Internal server error' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  req.log || logger.info({ port: PORT }, 'Server started');
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 6 | `import pino from 'pino'` | Imports the Pino logger library |
| 7 | `import pinoHttp from 'pino-http'` | Imports the Express middleware for request logging |
| 10-40 | `pino({ ... })` | Configures the Pino logger with level, serializers, and options |
| 12-14 | `level: process.env.LOG_LEVEL` | Sets minimum log level from environment variable |
| 16-30 | `serializers: { req, res }` | Controls what gets logged for requests/responses |
| 32-35 | `base: { service, environment }` | Adds these fields to every log entry |
| 37-39 | `transport` config | Uses pino-pretty for human-readable logs in development |
| 45-54 | `pinoHttp({ logger, ... })` | Configures the middleware with custom messages and filtering |
| 56 | `autoLogging.ignore` | Prevents logging for health check endpoints |
| 66 | `req.log.debug()` | Logs debug message (only shows if LOG_LEVEL is debug) |
| 80 | `req.log.info()` | Logs info message about order creation |
| 84-87 | `req.log.error()` | Logs error with full error object |
| 93-101 | `app.use((err, req, res, next))` | Error-handling middleware that logs unhandled errors |

## ⚠️ Common Mistakes

### 1. Not installing pino-pretty separately

**What it is**: In development, logs appear as unreadable JSON instead of formatted text.

**Why it happens**: `pino-pretty` is a separate package that needs to be installed explicitly.

**How to fix it**: Run `npm install pino-pretty` and configure it in your transport options, or use the CLI: `node app.js | pino-pretty`.

### 2. Logging entire request bodies without sanitization

**What it is**: Passwords, tokens, and sensitive data end up in logs because serializers log everything.

**Why it happens**: Default serializers capture the full request, including bodies and headers.

**How to fix it**: Write custom serializers that explicitly select which fields to include, as shown in the example. Never log `req.body` without validation.

### 3. Not using the logger from req.log in routes

**What it is**: Creating a new logger instance in each route instead of using the one attached to the request.

**Why it happens**: Developers forget that pino-http attaches its own logger to each request.

**How to fix it**: Always use `req.log` (or `req.log.info()`, etc.) in route handlers. This automatically includes the request ID for correlation.

## ✅ Quick Recap

- Pino is a high-performance JSON logger that outputs structured log data
- `pino-http` middleware automatically logs HTTP requests with timing and status
- Serializers control what data gets included in logs — customize them to exclude sensitive info
- Use `pino-pretty` for readable logs in development, plain JSON in production
- Always use `req.log` in route handlers to maintain request correlation

## 🔗 What's Next

Pino is excellent for simple, fast logging. For more complex logging needs like multiple output destinations and custom formatting, learn about Winston in [Logging with Winston](./03_logging-with-winston.md).
