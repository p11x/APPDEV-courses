# Error Tracking

## What You'll Learn

- Setting up Sentry for error tracking
- Capturing exceptions with context
- Performance monitoring

## Sentry Setup

```js
// sentry.js
import * as Sentry from '@sentry/node';

Sentry.init({
  dsn: process.env.SENTRY_DSN,
  environment: process.env.NODE_ENV,
  tracesSampleRate: 0.1,
});

// Express integration
Sentry.setupExpressErrorHandler(app);

// Manual capture
try {
  riskyOperation();
} catch (err) {
  Sentry.captureException(err, {
    tags: { feature: 'bookmark-create' },
    extra: { userId: req.user?.id },
  });
}
```

## Next Steps

For performance monitoring, continue to [Performance Monitoring](./03-performance-monitoring.md).
