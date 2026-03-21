# Installing Sentry

## What You'll Learn
- Setting up Sentry in Next.js
- Configuration basics
- Testing error capture
- Understanding the dashboard

## Prerequisites
- A Next.js project
- A Sentry account (free tier works)
- Basic understanding of error handling

## Concept Explained Simply

Sentry is like a security camera for your app — it watches for errors and records what went wrong when they happen. Instead of hoping users report bugs, you see exactly what errors occurred, how often, and the details to fix them.

Think of it like flight recorders on airplanes: when something goes wrong, you want as much information as possible to understand and fix the problem.

## Complete Code Example

### Installation

```bash
# Install Sentry
npm install @sentry/nextjs
```

### Configuration

```typescript
// sentry.client.config.ts
import * as Sentry from "@sentry/nextjs";

Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
  
  // Performance monitoring
  tracesSampleRate: 1.0,
  
  // Session replay
  replaysSessionSampleRate: 0.1,
  replaysOnErrorSampleRate: 1.0,
  
  // Environment
  environment: process.env.NODE_ENV,
  
  // Release tracking
  release: process.env.NEXT_PUBLIC_VERCEL_GIT_COMMIT_SHA,
});
```

```typescript
// sentry.server.config.ts
import * as Sentry from "@sentry/nextjs";

Sentry.init({
  dsn: process.env.SENTRY_DSN,
  tracesSampleRate: 1.0,
});
```

```typescript
// sentry.edge.config.ts
import * as Sentry from "@sentry/nextjs";

Sentry.init({
  dsn: process.env.SENTRY_DSN,
  tracesSampleRate: 1.0,
});
```

### Environment Variables

```bash
# .env.local
# Get this from Sentry project settings
NEXT_PUBLIC_SENTRY_DSN=https://xxxxx@xxxxx.ingest.sentry.io/xxxxx
```

### instrument.ts

```typescript
// instrument.ts
import * as Sentry from "@sentry/nextjs";

export async function register() {
  if (process.env.NEXT_RUNTIME === "nodejs") {
    await import("./sentry.server.config");
  } else if (process.env.NEXT_RUNTIME === "edge") {
    await import("./sentry.edge.config");
  }
}
```

## Testing

```typescript
// Test error capture
// app/api/test-error/route.ts
import * as Sentry from "@sentry/nextjs";

export async function GET() {
  try {
    throw new Error("Test error from Sentry!");
  } catch (error) {
    Sentry.captureException(error);
    return Response.json({ error: "Error captured!" });
  }
}
```

## Common Issues

### Missing DSN

```typescript
// Make sure DSN is set - errors will be silently ignored without it!
```

### Not in Next.js

```typescript
// WRONG - Regular import
import Sentry from "@sentry/node";

// CORRECT - Use Next.js integration
import * as Sentry from "@sentry/nextjs";
```

## Summary

- Install with `npm install @sentry/nextjs`
- Configure DSN in environment variables
- Use provided config files for client/server/edge
- Test by triggering an error and checking Sentry dashboard
