# Sentry Integration

## What You'll Learn
- Error tracking setup
- Performance monitoring
- Alert configuration

## Prerequisites
- App deployed

## Do I Need This Right Now?
Error monitoring is essential for production.

## Sentry Setup

This references **Section 20 (Error Monitoring)** — instrumentation.ts setup.

```typescript
// instrumentation.ts
import * as Sentry from '@sentry/nextjs';

Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
  tracesSampleRate: process.env.NODE_ENV === 'production' ? 0.1 : 1.0,
  replaysSessionSampleRate: 0.1,
  replaysOnErrorSampleRate: 1.0,
});
```

## Summary
- Install @sentry/nextjs
- Configure instrumentation.ts
- Set up alerts in Sentry dashboard

## Next Steps
- [deploying-to-vercel.md](./deploying-to-vercel.md) — Deploy to Vercel
