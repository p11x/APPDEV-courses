# Logging with Winston

## 📌 What You'll Learn

- How to install and configure Winston, a versatile logging library
- Understanding Winston transports and how to use multiple outputs
- How to configure formatting for different environments
- How to implement daily log rotation with file transports

## 🧠 Concept Explained (Plain English)

Winston is like having multiple pens that can write to different places simultaneously. While Pino is optimized for speed and JSON output, Winston is designed for flexibility. You can send logs to multiple destinations (called "transports") at the same time — console, files, HTTP endpoints, databases, and more.

A **transport** in Winston is a storage mechanism for log messages. The most common transports are:

- **Console**: Writes to the terminal (what you see when running in development)
- **File**: Writes to disk, with support for rotation
- **HTTP**: Sends logs to remote services like Loggly or Datadog
- **Stream**: Pipes logs to other streams for custom processing

Winston also supports **log rotation** out of the box. This means instead of one infinitely growing log file, you get new files every day (or when a size limit is reached), and old files can be automatically deleted. This prevents your server from running out of disk space.

The concept of **formats** in Winston allows you to transform log entries before they're written. You can combine multiple formats (like adding timestamps, coloring console output, and converting to JSON) to create the perfect logging setup for your needs.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import winston from 'winston';
import expressWinston from 'express-winston';

const app = express();

// Define custom log format
const { combine, timestamp, printf, colorize, json, metadata } = winston.format;

// Custom format for console output
const consoleFormat = printf(({ level, message, timestamp, ...metadata }) => {
  let msg = `${timestamp} [${level}]: ${message}`;
  if (Object.keys(metadata).length > 0) {
    msg += ` ${JSON.stringify(metadata)}`;
  }
  return msg;
});

// Create Winston logger with multiple transports
const logger = winston.createLogger({
  // Minimum level to log
  level: process.env.LOG_LEVEL || 'info',
  
  // Format: combine multiple formatters
  format: combine(
    timestamp({ format: 'YYYY-MM-DD HH:mm:ss' }),
    metadata(),
    json()
  ),
  
  // Default transports
  transports: [
    // Console transport for development
    new winston.transports.Console({
      format: combine(
        colorize(),
        timestamp({ format: 'YYYY-MM-DD HH:mm:ss' }),
        consoleFormat
      ),
      level: 'debug' // Show everything in console
    }),
    
    // File transport for all logs
    new winston.transports.File({ 
      filename: 'logs/combined.log',
      maxsize: 5242880, // 5MB
      maxFiles: 5
    }),
    
    // Separate file for errors only
    new winston.transports.File({ 
      filename: 'logs/error.log',
      level: 'error',
      maxsize: 5242880,
      maxFiles: 5
    }),
    
    // Daily rotating file transport
    new winston.transports.DailyRotateFile({
      filename: 'logs/app-%DATE%.log',
      datePattern: 'YYYY-MM-DD',
      maxSize: '20m',
      maxFiles: '30', // Keep 30 days of logs
      zippedArchive: true
    })
  ],
  
  // Handle uncaught exceptions
  exceptionHandlers: [
    new winston.transports.File({ filename: 'logs/exceptions.log' })
  ],
  
  // Handle unhandled promise rejections
  rejectionHandlers: [
    new winston.transports.File({ filename: 'logs/rejections.log' })
  ]
});

// Middleware for Express request logging
app.use(expressWinston.logger({
  winstonInstance: logger,
  
  // Use JSON format for API requests
  format: combine(
    timestamp(),
    json()
  ),
  
  // Customize what gets logged
  meta: true,
  
  // Colorize output in console (HTTP requests)
  colorize: process.env.NODE_ENV !== 'production',
  
  // Skip certain routes
  skip: (req) => req.url === '/health' || req.url === '/metrics'
}));

// Example routes demonstrating logging
app.get('/api/products', (req, res) => {
  const products = [
    { id: 1, name: 'Widget', price: 29.99 },
    { id: 2, name: 'Gadget', price: 49.99 }
  ];
  
  // Log at different levels
  logger.info('Products fetched', { 
    count: products.length,
    requestId: req.id 
  });
  
  res.json(products);
});

app.post('/api/checkout', (req, res) => {
  const { cart } = req.body || {};
  
  // Log with metadata
  logger.info('Checkout initiated', {
    cartItems: cart?.length || 0,
    userAgent: req.headers['user-agent'],
    ip: req.ip
  });
  
  // Simulate processing error
  const hasError = Math.random() > 0.8;
  
  if (hasError) {
    logger.error('Payment processing failed', {
      error: 'Payment gateway timeout',
      cart: cart?.length
    });
    return res.status(502).json({ error: 'Payment service unavailable' });
  }
  
  logger.info('Checkout completed', { orderId: 'order-123' });
  res.json({ orderId: 'order-123', status: 'success' });
});

// Error handling middleware with Winston
app.use(expressWinston.errorLogger({
  winstonInstance: logger,
  meta: true,
  msg: 'HTTP Error: {{err.message}}'
}));

// Global error handler
app.use((err, req, res, next) => {
  logger.error('Unhandled error', {
    error: err.message,
    stack: err.stack,
    url: req.url,
    method: req.method
  });
  
  res.status(500).json({ error: 'Internal server error' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  logger.info('Server started', { port: PORT, env: process.env.NODE_ENV });
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `const { combine, timestamp, ... } = winston.format` | Destructures format functions from Winston |
| 10-15 | `consoleFormat` function | Creates custom human-readable log format |
| 19-50 | `winston.createLogger({})` | Creates logger with multiple transports and options |
| 24-27 | `format: combine(...)` | Chains multiple formatters together |
| 30-37 | `transports.Console` | Writes logs to terminal with colors |
| 39-45 | `transports.File` | Writes to combined.log, rotates by size |
| 47-53 | `transports.File` (error) | Separate file for error-level logs only |
| 55-62 | `transports.DailyRotateFile` | Creates new file each day, auto-deletes old ones |
| 65-71 | `exceptionHandlers` | Catches and logs uncaught exceptions |
| 74-77 | `rejectionHandlers` | Catches and logs unhandled promise rejections |
| 82-94 | `expressWinston.logger({})` | Middleware for automatic HTTP request logging |
| 106-112 | `logger.info('Products fetched', {})` | Logs info with additional metadata object |
| 117-127 | `logger.error('Payment failed', {})` | Logs errors with error details |
| 130-136 | `expressWinston.errorLogger({})` | Middleware specifically for error responses |

## ⚠️ Common Mistakes

### 1. Not handling async transport errors

**What it is**: Winston transports can fail silently (e.g., disk full, network issues).

**Why it happens**: By default, errors in transports don't crash the app.

**How to fix it**: Add `handleExceptions: true` to transports and use the global exception handlers. Also consider adding a callback or listening to the 'error' event on transports.

### 2. Creating logger inside route handlers

**What it is**: Re-creating Winston logger instances in each file or function.

**Why this happens**: Developers don't realize Winston loggers should be created once and reused.

**How to fix it**: Create a single logger instance in a dedicated file (like `src/utils/logger.js`) and import it wherever needed.

### 3. Not configuring daily rotate in production

**What it is**: Logs filling up disk space, eventually crashing the server.

**Why this happens**: Developers forget to set up rotation, especially when using basic file transport.

**How to fix it**: Use `winston-daily-rotate-file` package with `maxFiles` set to delete old logs automatically, or integrate with an external log rotation tool.

## ✅ Quick Recap

- Winston provides flexible multi-transport logging beyond console output
- Use `DailyRotateFile` transport for automatic log rotation by date
- Custom formats let you control exactly how logs appear in different environments
- `express-winston` middleware automatically logs HTTP requests and errors
- Winston's exception and rejection handlers catch crashes before they silently disappear

## 🔗 What's Next

Now that you can correlate logs using request IDs, learn how to implement log correlation in [Log Correlation IDs](./04_log-correlation-ids.md).
