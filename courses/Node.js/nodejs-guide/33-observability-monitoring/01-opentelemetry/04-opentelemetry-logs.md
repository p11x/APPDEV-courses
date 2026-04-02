# OpenTelemetry Logs

## What You'll Learn

- How OpenTelemetry handles logs
- How to correlate logs with traces
- How to use the OpenTelemetry Log SDK
- How logs integrate with traces and metrics

## Log Correlation

OpenTelemetry can automatically correlate logs with traces — each log entry includes the trace ID and span ID, allowing you to jump from a log line to the full trace.

```
Log Entry:
{
  "level": "error",
  "message": "Database query failed",
  "trace_id": "abc123...",      ← Links to the trace
  "span_id": "def456...",       ← Links to the span
  "timestamp": "2024-01-15T10:30:00Z"
}
```

## Setup with Pino

```bash
npm install @opentelemetry/api pino pino-opentelemetry-transport
```

```ts
// logger.ts — Pino with OpenTelemetry correlation

import pino from 'pino';
import { trace } from '@opentelemetry/api';

const logger = pino({
  level: process.env.LOG_LEVEL || 'info',
  transport: {
    targets: [
      // Console output
      {
        target: 'pino-pretty',
        options: { colorize: true },
        level: 'info',
      },
      // OTLP exporter (sends logs to OpenTelemetry collector)
      {
        target: 'pino-opentelemetry-transport',
        options: {
          resourceAttributes: {
            'service.name': 'my-api',
          },
        },
        level: 'info',
      },
    ],
  },
});

// Helper: create a child logger with trace context
export function getLogger() {
  const span = trace.getActiveSpan();
  if (span) {
    const spanContext = span.spanContext();
    return logger.child({
      traceId: spanContext.traceId,
      spanId: spanContext.spanId,
    });
  }
  return logger;
}
```

## Using in Code

```ts
// routes/users.ts

import { getLogger } from '../logger.js';

app.get('/users/:id', async (req, res) => {
  const log = getLogger();  // Automatically includes trace context

  log.info({ userId: req.params.id }, 'Fetching user');

  try {
    const user = await db.findUser(req.params.id);

    if (!user) {
      log.warn({ userId: req.params.id }, 'User not found');
      return res.status(404).json({ error: 'Not found' });
    }

    log.info({ userId: user.id }, 'User fetched');
    res.json(user);
  } catch (err) {
    log.error({ err, userId: req.params.id }, 'Failed to fetch user');
    res.status(500).json({ error: 'Internal error' });
  }
});
```

## Log Levels

| Level | Use Case |
|-------|----------|
| `trace` | Very detailed debugging |
| `debug` | Development debugging |
| `info` | Normal operations |
| `warn` | Something unexpected |
| `error` | Operation failed |
| `fatal` | Application cannot continue |

## Common Mistakes

### Mistake 1: No Trace Correlation

```ts
// WRONG — logs not linked to traces
logger.info('Request received');  // No trace ID!

// CORRECT — include trace context
const log = getLogger();
log.info('Request received');  // Includes traceId and spanId
```

### Mistake 2: Logging Sensitive Data

```ts
// WRONG — passwords and tokens in logs
logger.info({ password: 'secret', token: 'abc123' }, 'Login');

// CORRECT — use redaction
logger.info({ userId: 'user-123' }, 'Login successful');
```

## Next Steps

For Grafana & Loki, continue to [Grafana Setup](../02-grafana-loki/01-grafana-setup.md).
