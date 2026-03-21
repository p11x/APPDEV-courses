# Introduction to Logging

## 📌 What You'll Learn

- What logging is and why it matters in production applications
- The limitations of using `console.log()` in production
- Different log levels and when to use each
- How structured logging improves debugging and monitoring

## 🧠 Concept Explained (Plain English)

Logging is the practice of recording events, errors, and information about your application's behavior during runtime. Think of it as your application's diary — it tells you what happened, when it happened, and (sometimes) why it happened. When something goes wrong in production, your logs are often the first place you'll look to understand what went wrong.

You might be thinking, "I already use `console.log()` — isn't that enough?" In development, yes, `console.log()` works fine for quick debugging. But in production, it falls short in several important ways:

1. **No severity levels**: `console.log()` treats all messages equally. When you're troubleshooting, you need to quickly filter for errors without wading through thousands of informational messages.

2. **No timestamps by default**: Without explicit timestamp handling, you have no way to know the sequence of events or how long operations took.

3. **No structured format**: A string like "User login failed" doesn't tell you which user, from what IP address, or any other contextual information that would help diagnose issues.

4. **No rotation**: In production, log files can grow infinitely, eventually filling up your disk and crashing your server.

5. **No integration with monitoring tools**: Modern observability platforms (like Datadog, New Relic, or Prometheus) expect machine-readable formats like JSON, not plain text.

**Log levels** are categories that indicate the severity of a message. The standard levels, from least to most severe, are:

- **trace**: Detailed diagnostic information, typically only useful during development
- **debug**: Information useful for debugging, like variable values or flow through code
- **info**: General informational messages about application progress
- **warn**: Warning messages about potential problems that don't stop the application
- **error**: Error messages about things that went wrong
- **fatal**: Critical errors that usually cause the application to crash

In Express applications, you'll typically use a logging library that integrates with the request/response cycle, automatically capturing information about each HTTP request.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();

// Simple custom logger demonstrating log levels
const logLevels = {
  trace: 0,
  debug: 1,
  info: 2,
  warn: 3,
  error: 4,
  fatal: 5,
};

function logger(level, message, meta = {}) {
  // Only log if the level is >= current threshold (info = 2)
  if (logLevels[level] >= logLevels.info) {
    const logEntry = {
      timestamp: new Date().toISOString(),
      level,
      message,
      ...meta,
    };
    // In production, you'd use a proper logging library
    console.log(JSON.stringify(logEntry));
  }
}

// Middleware to log all requests
app.use((req, res, next) => {
  const start = Date.now();
  
  res.on('finish', () => {
    const duration = Date.now() - start;
    logger('info', 'HTTP request completed', {
      method: req.method,
      path: req.path,
      status: res.statusCode,
      duration_ms: duration,
    });
  });
  
  next();
});

// Example route demonstrating different log levels
app.get('/api/users/:id', (req, res) => {
  const userId = req.params.id;
  
  logger('debug', 'Fetching user', { userId });
  
  // Simulate finding a user
  const user = { id: userId, name: 'John Doe' };
  
  if (!user) {
    logger('warn', 'User not found', { userId });
    return res.status(404).json({ error: 'User not found' });
  }
  
  logger('info', 'User found', { userId });
  res.json(user);
});

app.post('/api/login', (req, res) => {
  const { email, password } = req.body || {};
  
  // In real code, validate credentials properly
  if (password !== 'secret') {
    logger('warn', 'Failed login attempt', { 
      email, 
      ip: req.ip 
    });
    return res.status(401).json({ error: 'Invalid credentials' });
  }
  
  logger('info', 'Successful login', { email });
  res.json({ token: 'fake-jwt-token' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  logger('info', 'Server started', { port: PORT });
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 4-10 | `logLevels` object | Defines numeric severity for each log level |
| 12-21 | `logger()` function | Custom logging function that formats messages as JSON with timestamp |
| 25 | `app.use((req, res, next) => {})` | Registers middleware that runs on every HTTP request |
| 27 | `const start = Date.now()` | Records when the request started |
| 29 | `res.on('finish', ...)` | Event listener fires when response is complete |
| 30 | `const duration = Date.now() - start` | Calculates how long the request took |
| 31-38 | `logger('info', ...)` | Logs request details including method, path, status, and duration |
| 42 | `next()` | Passes control to the next middleware/route handler |
| 46 | `logger('debug', ...)` | Logs debug information about fetching a user |
| 64-66 | `logger('warn', ...)` | Logs warning when user is not found |
| 73-76 | `logger('warn', ...)` | Logs warning for failed login attempts |
| 86-88 | `logger('info', ...)` | Logs server startup |

## ⚠️ Common Mistakes

### 1. Logging sensitive data

**What it is**: Recording passwords, credit card numbers, API keys, or personal identifiable information (PII) in logs.

**Why it happens**: Developers often want maximum context when debugging and don't think about what's actually in the data they're logging.

**How to fix it**: Create sanitization functions that strip sensitive fields before logging, or use dedicated logging libraries that automatically mask known sensitive fields. Never log the full request body without validation.

### 2. Using the wrong log level

**What it is**: Marking everything as `info` or using `error` for non-critical issues.

**Why it happens**: Without clear guidelines, developers default to the most convenient level or the most alarming level.

**How to fix it**: Establish team conventions. Errors are for things that need immediate attention. Warnings are for things that might cause problems later. Info is for normal operations. Debug is only for development.

### 3. Synchronous logging blocking the event loop

**What it is**: Using file I/O or network calls for logging synchronously, which blocks the Node.js event loop.

**Why it happens**: Simple `console.log()` is synchronous, but most production logging involves file or network operations that can slow down your application.

**How to fix it**: Use async logging libraries that write to buffers and flush in the background. Libraries like Pino are designed to be extremely fast and non-blocking.

## ✅ Quick Recap

- Logging records application events for debugging, monitoring, and compliance
- `console.log()` is insufficient for production — it lacks severity levels, timestamps, and structured formats
- Log levels (trace, debug, info, warn, error, fatal) help filter messages by severity
- Structured logging uses JSON format, making logs machine-parseable for monitoring tools
- Middleware patterns in Express make it easy to automatically log every HTTP request

## 🔗 What's Next

Now that you understand the basics of logging, learn how to implement structured logging with Pino, one of the fastest JSON loggers for Node.js, in [Structured Logging with Pino](./02_structured-logging-with-pino.md).
