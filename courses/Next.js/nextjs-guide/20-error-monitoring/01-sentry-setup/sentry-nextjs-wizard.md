# Sentry Next.js Wizard Setup

## What You'll Learn
- Set up Sentry with Next.js automatically
- Configure the Sentry wizard
- Understand what gets installed

## Prerequisites
- A Next.js project
- Sentry account (free tier works)

## Do I Need This Right Now?
The Sentry wizard is the fastest way to get started with error monitoring. If you want to track errors in production, this is essential.

## Concept Explained Simply

The Sentry wizard is like a robot that automatically installs all the sensors (error tracking) in your app. Instead of manually connecting each wire, the wizard does it all for you in one command.

## Complete Code Example

### Step 1: Run the Wizard

```bash
# In your Next.js project directory
npx @sentry/wizard@latest -i nextjs
```

### Step 2: Follow the Prompts

The wizard will ask:
1. Connect your Sentry account (opens browser)
2. Select your Sentry project
3. Choose authentication method (Token or OAuth)

### Step 3: What Gets Installed

After running, you'll see these files created:

```typescript
// instrumentation.ts (or instrumentation.js)
import * as Sentry from '@sentry/nextjs';

Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
  
  // Performance monitoring
  tracesSampleRate: 1.0,
  
  // Session replay
  replaysSessionSampleRate: 0.1,
  replaysOnErrorSampleRate: 1.0,
  
  // Release tracking
  release: process.env.npm_package_version,
  
  // Environment
  environment: process.env.NODE_ENV,
});
```

```typescript
// sentry.client.config.ts
import * as Sentry from '@sentry/nextjs';

Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
  
  // Client-side config
  replaysSessionSampleRate: 0.1,
  replaysOnErrorSampleRate: 1.0,
  
  // Filter events
  beforeSend(event, hint) {
    // Filter out specific errors if needed
    const error = hint.originalException;
    if (error && error.message === 'ResizeObserver error') {
      return null;
    }
    return event;
  },
});
```

```typescript
// sentry.server.config.ts
import * as Sentry from '@sentry/nextjs';

Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
  
  // Server-side config
  tracesSampleRate: 1.0,
});
```

```typescript
// next.config.js or next.config.mjs
const nextConfig = {
  // Sentry automatically adds this:
  // sentry: {
  //   widenClientFileUpload: true,
  //   hideSourceMaps: true,
  //   disableLogger: true,
  // }
};

module.exports = nextConfig;
```

### Step 4: Environment Variables

The wizard adds these to your `.env.local`:

```bash
# Get this from Sentry project settings
NEXT_PUBLIC_SENTRY_DSN=https://xxxxx@xxxxx.ingest.sentry.io/xxxxx
```

## Manual Setup (If Needed)

If the wizard doesn't work, you can set up manually:

```bash
# Install dependencies
npm install @sentry/nextjs
```

```typescript
// Create instrumentation.ts in app directory
import * as Sentry from '@sentry/nextjs';

Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
  
  // Add these options:
  tracesSampleRate: 1.0,
  replaysSessionSampleRate: 0.1,
  replaysOnErrorSampleRate: 1.0,
  
  // Filter out common non-critical errors
  beforeSend(event, hint) {
    const error = hint.originalException as Error;
    
    // Ignore specific errors
    if (error?.message === 'ResizeObserver loop limit exceeded') {
      return null;
    }
    
    return event;
  },
});
```

## Verifying Setup

```typescript
// Test that Sentry is working
// app/api/test-sentry/route.ts
import * as Sentry from '@sentry/nextjs';

export async function GET() {
  // This should appear in Sentry
  Sentry.captureMessage('Test message from Sentry!');
  
  try {
    throw new Error('Test error!');
  } catch (error) {
    Sentry.captureException(error);
  }
  
  return Response.json({ ok: true });
}
```

## Common Mistakes

### Mistake #1: Not Setting DSN
```typescript
// Wrong: No DSN configured
Sentry.init({
  // Missing dsn!
});
```

```typescript
// Correct: Set DSN from env
Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
});
```

### Mistake #2: Too High Sample Rate
```typescript
// Wrong: Captures everything in production - too much data!
tracesSampleRate: 1.0;
```

```typescript
// Correct: Use low rate in production
tracesSampleRate: process.env.NODE_ENV === 'production' ? 0.1 : 1.0;
```

### Mistake #3: Not Hiding Source Maps in Production
```typescript
// Wrong: Source maps exposed in production
// next.config.js
const config = {}; // Missing sentry config
```

```typescript
// Correct: Hide source maps
const config = {
  sentry: {
    hideSourceMaps: true,
  },
};
```

## Summary
- Use `npx @sentry/wizard@latest -i nextjs` for quick setup
- The wizard creates instrumentation.ts and config files
- Add NEXT_PUBLIC_SENTRY_DSN to environment
- Use appropriate sample rates for production
- Hide source maps in production
- Test with a sample error to verify

## Next Steps
- [source-maps.md](./source-maps.md) — Configuring source maps
