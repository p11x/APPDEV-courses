# Pino Setup

## What You'll Learn

- Why structured logging matters (JSON logs vs plain text)
- How to set up pino as a fast structured logger
- How to use log levels (trace, debug, info, warn, error, fatal)
- How to add serializers and redact sensitive fields
- How pino compares to console.log

## Why Structured Logging?

`console.log('User logged in:', userId)` produces a plain text line. In production, you need to search millions of log lines — plain text is hard to parse.

**Structured logging** outputs JSON that log aggregators (Datadog, ELK, Splunk) can index and search:

```json
{"level":30,"time":1705312200000,"msg":"User logged in","userId":"abc123","reqId":"req-1"}
```

**Pino** is the fastest Node.js logger. It outputs JSON by default and is designed for production.

## Project Setup

```bash
npm install pino pino-pretty
```

## Basic Logger

```js
// logger.js — Create and configure a pino logger

import pino from 'pino';

// Create a logger instance
// In production, pino outputs JSON to stdout (pipe to a log aggregator)
const logger = pino({
  // Log level — only messages at this level or higher are output
  // Levels: trace (10) < debug (20) < info (30) < warn (40) < error (50) < fatal (60)
  level: process.env.LOG_LEVEL || 'info',

  // Custom base fields — added to every log line
  base: {
    pid: process.pid,     // Process ID (useful in clustered apps)
    hostname: undefined,  // Remove hostname (redundant in containers)
  },

  // Timestamp format
  timestamp: pino.stdTimeFunctions.isoTime,  // ISO 8601: "2024-01-15T10:30:00.000Z"
});

// Log at different levels
logger.trace('Detailed debugging info');   // Not output (below 'info' level)
logger.debug('Debug information');          // Not output
logger.info('Server started on port 3000'); // Output
logger.warn('Disk usage at 85%');          // Output
logger.error('Database query failed');      // Output
logger.fatal('Out of memory — shutting down'); // Output

// Log with structured data
logger.info({
  event: 'user_login',
  userId: 'abc123',
  ip: '192.168.1.1',
  duration: 45,
}, 'User logged in');

// Output (JSON):
// {"level":30,"time":"2024-01-15T10:30:00.000Z","pid":12345,"event":"user_login","userId":"abc123","ip":"192.168.1.1","duration":45,"msg":"User logged in"}
```

## Child Loggers

```js
// child-logger.js — Child loggers add context to every log line

import pino from 'pino';

const logger = pino();

// Create a child logger with fixed fields
// Every log from this child includes { module: 'auth' }
const authLogger = logger.child({ module: 'auth' });

authLogger.info('Login attempt', { userId: 'abc' });
// {"level":30,"module":"auth","msg":"Login attempt","userId":"abc"}

// Nested child loggers — add more context
const requestLogger = authLogger.child({ reqId: 'req-42' });

requestLogger.warn('Invalid password', { userId: 'abc', attempts: 3 });
// {"level":40,"module":"auth","reqId":"req-42","msg":"Invalid password","userId":"abc","attempts":3}
```

## Serializers and Redaction

```js
// serializers.js — Custom serializers and sensitive data redaction

import pino from 'pino';

const logger = pino({
  // Serializers transform objects before logging
  serializers: {
    // Customize how errors are logged (stack traces, codes)
    err: pino.stdSerializers.err,  // Built-in error serializer

    // Custom serializer for user objects
    user(user) {
      return {
        id: user.id,
        name: user.name,
        // Do NOT log email or password
      };
    },

    // Custom serializer for HTTP requests
    req(req) {
      return {
        method: req.method,
        url: req.url,
        headers: {
          host: req.headers?.host,
          // Omit authorization header
        },
      };
    },
  },

  // Redact — remove sensitive fields from ALL log output
  // Uses dot-notation paths to specify what to redact
  redact: [
    'password',
    'token',
    'authorization',
    'req.headers.authorization',
    'user.email',
    '*.secret',     // Redact 'secret' field at any nesting level
  ],
});

// These fields are automatically redacted
logger.info({
  user: { id: '1', name: 'Alice', email: 'alice@example.com', password: 'secret123' },
  token: 'jwt-token-here',
});
// Output: user.email is replaced with '[Redacted]', password is '[Redacted]', token is '[Redacted]'
```

## Development Pretty Printing

```js
// dev-logger.js — Pretty output for development

import pino from 'pino';

// In development, use pino-pretty to format JSON as readable colored text
// In production, output raw JSON for log aggregators
const logger = pino(
  process.env.NODE_ENV === 'production'
    ? {}  // Production: raw JSON
    : {
        // Development: pretty-printed output
        transport: {
          target: 'pino-pretty',
          options: {
            colorize: true,           // Colorize output
            translateTime: 'HH:MM:ss', // Human-readable time
            ignore: 'pid,hostname',    // Hide these fields
            singleLine: false,         // Multi-line for readability
          },
        },
      }
);

logger.info('Server started');
logger.warn('High memory usage', { usage: '85%' });
logger.error(new Error('Something failed'));
```

### Running

```bash
# Development (pretty output)
NODE_ENV=development node dev-logger.js

# Production (JSON output)
NODE_ENV=production node dev-logger.js
```

## How It Works

### Pino vs console.log

| Feature | console.log | pino |
|---------|-------------|------|
| Output format | Plain text | JSON (structured) |
| Performance | Slow (synchronous) | Fast (async, batched) |
| Log levels | Only `console.warn/error` | 6 levels (trace → fatal) |
| Child loggers | No | Yes |
| Redaction | No | Built-in |
| Serialization | Manual | Configurable |

### Log Level Hierarchy

```
trace (10) — extremely detailed, only for deep debugging
debug (20) — development debugging
info (30)  — normal operations (default)
warn (40)  — something unexpected, but app continues
error (50) — something failed
fatal (60) — app cannot continue
```

Setting `level: 'info'` means info, warn, error, and fatal are output; trace and debug are suppressed.

## Common Mistakes

### Mistake 1: Using console.log in Production

```js
// WRONG — unstructured, slow, no level filtering
console.log('User created:', JSON.stringify(user));
console.error('Error:', err.stack);

// CORRECT — structured, fast, filterable
logger.info({ user }, 'User created');
logger.error({ err }, 'Request failed');
```

### Mistake 2: Logging Sensitive Data

```js
// WRONG — password and tokens in logs
logger.info('Login', { user: { email, password }, token });

// CORRECT — use redaction (see serializers.js above)
// Or manually exclude sensitive fields
logger.info('Login', { userId: user.id });
```

### Mistake 3: String Interpolation in Message

```js
// WRONG — string interpolation loses structure
logger.info(`User ${userId} logged in from ${ip}`);
// The message is a flat string — log aggregators cannot search by userId or ip

// CORRECT — pass structured data as the first argument
logger.info({ userId, ip }, 'User logged in');
// Now you can search: userId=abc123 AND ip=192.168.*
```

## Try It Yourself

### Exercise 1: Log Levels

Create a logger with `level: 'warn'`. Try logging at every level. Verify that only warn, error, and fatal appear.

### Exercise 2: Child Loggers

Create a base logger. Add child loggers for 'auth', 'db', and 'api' modules. Log messages from each and verify the `module` field appears.

### Exercise 3: Redaction

Log an object with `password`, `token`, and `user.email` fields. Verify they appear as `[Redacted]` in the output.

## Next Steps

You have structured logging. For request logging with correlation IDs, continue to [Request Logging](./02-request-logging.md).
