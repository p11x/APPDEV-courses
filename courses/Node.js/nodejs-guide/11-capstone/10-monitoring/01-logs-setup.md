# Logging Setup

## What You'll Learn

- Structured logging with Pino
- Request logging middleware
- Log levels and filtering

## Pino Setup

```js
// logger.js
import pino from 'pino';

export const logger = pino({
  level: process.env.LOG_LEVEL || 'info',
  transport: process.env.NODE_ENV === 'development'
    ? { target: 'pino-pretty', options: { colorize: true } }
    : undefined,
});
```

## Request Logging

```js
import pinoHttp from 'pino-http';

app.use(pinoHttp({
  logger,
  autoLogging: true,
  redact: ['req.headers.authorization'],
}));
```

## Next Steps

For error tracking, continue to [Error Tracking](./02-error-tracking.md).
