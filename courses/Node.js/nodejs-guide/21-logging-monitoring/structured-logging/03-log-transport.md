# Log Transport

## What You'll Learn

- How to configure pino transports for different outputs
- How to use pino-pretty for development
- How to write logs to files with rotation
- How to pipe logs to external services
- How to separate dev and prod logging configurations

## Project Setup

```bash
npm install pino pino-pretty pino-roll
```

## Transport Configuration

```js
// transport.js — Configure different log transports per environment

import pino from 'pino';

// Development: pretty-printed to stdout
const devLogger = pino({
  transport: {
    target: 'pino-pretty',
    options: {
      colorize: true,
      translateTime: 'SYS:HH:MM:ss',
      ignore: 'pid,hostname',
    },
  },
});

// Production: JSON to stdout (for container log aggregation)
const prodLogger = pino({
  level: 'info',
  // No transport — raw JSON to stdout
  // Docker/Kubernetes captures stdout and sends to log aggregator
});

// File logging with rotation
const fileLogger = pino({
  transport: {
    targets: [
      // Output to stdout (always)
      {
        target: 'pino/file',
        options: { destination: 1 },  // 1 = stdout
        level: 'info',
      },
      // Also output to a file
      {
        target: 'pino-roll',
        options: {
          file: './logs/app.log',     // Log file path
          frequency: 'daily',          // Rotate daily
          maxFiles: 30,                // Keep 30 days of logs
          compress: true,              // Gzip old log files
        },
        level: 'info',
      },
    ],
  },
});

// Choose logger based on environment
const logger = process.env.NODE_ENV === 'production' ? prodLogger : devLogger;

logger.info('Application started');
logger.warn('Configuration warning', { field: 'port', value: 'abc' });
logger.error(new Error('Connection failed'));
```

## Multi-Target Transport

```js
// multi-transport.js — Send logs to multiple destinations

import pino from 'pino';

const logger = pino({
  transport: {
    targets: [
      // Always: pretty-printed to stdout in dev
      {
        target: 'pino-pretty',
        options: { colorize: true, translateTime: 'HH:MM:ss' },
        level: 'debug',
      },
      // Always: JSON file for archive
      {
        target: 'pino/file',
        options: { destination: './logs/app.json.log' },
        level: 'info',
      },
      // Errors only: separate error log
      {
        target: 'pino/file',
        options: { destination: './logs/error.json.log' },
        level: 'error',
      },
    ],
  },
});

logger.debug('Debug info');        // stdout only
logger.info('Request handled');     // stdout + app.json.log
logger.error('Something failed');   // stdout + app.json.log + error.json.log
```

## Log Rotation with pino-roll

```js
// rotation.js — Automatic log file rotation

import pino from 'pino';

const logger = pino({
  transport: {
    target: 'pino-roll',
    options: {
      file: './logs/app.log',
      frequency: 'daily',      // Rotate every day
      // frequency: 'hourly',  // Or every hour
      // frequency: '10m',     // Or every 10 minutes
      maxFiles: 14,            // Delete logs older than 14 days
      mkdir: true,             // Create directory if it does not exist
      compress: 'gzip',        // Compress rotated files → app.log.2024-01-15.gz
    },
  },
});

// Simulate logging
for (let i = 0; i < 1000; i++) {
  logger.info({ event: 'request', path: `/api/${i % 10}`, duration: Math.random() * 100 });
}

console.log('Logs written to ./logs/app.log');
```

## Production Configuration

```js
// production-logger.js — Full production logging setup

import pino from 'pino';

function createLogger() {
  const isProd = process.env.NODE_ENV === 'production';

  if (isProd) {
    // Production: JSON to stdout, errors to separate file
    return pino({
      level: process.env.LOG_LEVEL || 'info',
      formatters: {
        level(label) {
          return { level: label };  // Use string labels instead of numbers
        },
      },
      timestamp: pino.stdTimeFunctions.isoTime,
      redact: ['req.headers.authorization', 'password', 'token'],
      transport: {
        targets: [
          // stdout for container log aggregation
          {
            target: 'pino/file',
            options: { destination: 1 },
            level: 'info',
          },
          // Error file for post-mortem analysis
          {
            target: 'pino-roll',
            options: {
              file: './logs/error.log',
              frequency: 'daily',
              maxFiles: 30,
              mkdir: true,
            },
            level: 'error',
          },
        ],
      },
    });
  }

  // Development: pretty-printed with colors
  return pino({
    level: 'debug',
    transport: {
      target: 'pino-pretty',
      options: {
        colorize: true,
        translateTime: 'SYS:HH:MM:ss',
        ignore: 'pid,hostname',
      },
    },
  });
}

const logger = createLogger();

export default logger;
```

## How It Works

### Transport Pipeline

```
Application code
    │
    │ logger.info({ data }, 'message')
    │
    ▼
Pino core (serializes to JSON)
    │
    ▼
Transport targets
    ├── stdout (JSON or pretty)
    ├── file (rotated daily)
    └── error file (errors only)
```

### pino-pretty for Development Only

`pino-pretty` is slow — it parses JSON and reformats it. Never use it in production. In production, output raw JSON and let your log aggregator handle formatting.

## Common Mistakes

### Mistake 1: pino-pretty in Production

```js
// WRONG — pino-pretty adds 10x overhead
const logger = pino({ transport: { target: 'pino-pretty' } });

// CORRECT — only use pino-pretty in development
const logger = process.env.NODE_ENV === 'production'
  ? pino()  // Raw JSON
  : pino({ transport: { target: 'pino-pretty' } });
```

### Mistake 2: No Log Rotation

```js
// WRONG — log file grows forever until disk is full
const logger = pino({ destination: './app.log' });

// CORRECT — use pino-roll for rotation
const logger = pino({
  transport: { target: 'pino-roll', options: { file: './app.log', frequency: 'daily', maxFiles: 30 } },
});
```

### Mistake 3: Logging to the Wrong Destination

```js
// WRONG — logging to a file path that does not exist
const logger = pino({ destination: '/var/log/myapp/app.log' });
// Throws if /var/log/myapp/ does not exist

// CORRECT — use mkdir option or ensure the directory exists
const logger = pino({
  transport: {
    target: 'pino-roll',
    options: { file: './logs/app.log', mkdir: true },
  },
});
```

## Try It Yourself

### Exercise 1: Dev vs Prod

Create a logger that uses pino-pretty in development and raw JSON in production. Test with `NODE_ENV=production node app.js`.

### Exercise 2: File Rotation

Write 10,000 log entries. Verify the log file exists and is valid JSON (one object per line).

### Exercise 3: Error-Only File

Configure two transports: all logs to stdout, and only errors to `./logs/error.log`. Generate info, warn, and error logs. Verify the error file contains only errors.

## Next Steps

You have production-grade logging. For error monitoring, continue to [Sentry Setup](../error-monitoring/01-sentry-setup.md).
