# Performance Monitoring

## What You'll Learn
- Enable performance monitoring
- Track slow transactions
- Find performance bottlenecks

## Prerequisites
- Sentry installed with tracing configured

## Do I Need This Right Now?
Performance monitoring helps you find slow parts of your app. If users complain about slowness, this is how you find the problem.

## Concept Explained Simply

Performance monitoring is like a speed camera on a highway. It records how fast each request travels and where it slows down. This helps you find the bottlenecks causing delays.

## Enabling Performance Monitoring

```typescript
// instrumentation.ts
import * as Sentry from '@sentry/nextjs';

Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
  
  // Enable tracing
  tracesSampleRate: 1.0, // 100% in development, lower in production
  
  // Or use a percentage
  // tracesSampleRate: process.env.NODE_ENV === 'production' ? 0.1 : 1.0,
});
```

## Viewing Performance Data

1. Go to Sentry dashboard
2. Click "Performance" in sidebar
3. View:
   - **Throughput** - requests per second
   - **Latency** - how long requests take
   - **Apdex** - user satisfaction score
   - **Failure Rate** - error percentage

## Tracing Slow Transactions

### Server Component Tracing

```typescript
// app/slow-page/page.tsx
import * as Sentry from '@sentry/nextjs';
import { SpanStatus } from '@sentry/utils';

export default async function SlowPage() {
  // Create a transaction
  const transaction = Sentry.startInactiveSpan({
    name: 'slow-page-load',
    op: 'pageload',
  });
  
  try {
    // This span is automatically traced
    const user = await getUser();
    
    const profile = await getProfile(user.id);
    
    const notifications = await getNotifications(user.id);
    
    transaction.setStatus(SpanStatus.Ok);
    
    return (
      <div>
        <h1>{profile.name}</h1>
        <p>{notifications.length} notifications</p>
      </div>
    );
  } catch (error) {
    transaction.setStatus(SpanStatus.InternalError);
    throw error;
  } finally {
    transaction.end();
  }
}
```

### Custom Spans

```typescript
// app/api/report/route.ts
import * as Sentry from '@sentry/nextjs';

export async function GET() {
  const transaction = Sentry.startInactiveSpan({
    name: 'generate-report',
    op: 'task',
  });
  
  try {
    // Step 1: Fetch data
    const dataSpan = transaction.startChild({
      op: 'fetch-data',
      description: 'Get data from database',
    });
    
    const data = await fetchDataFromDB();
    
    dataSpan.end();
    
    // Step 2: Process data
    const processSpan = transaction.startChild({
      op: 'process-data',
      description: 'Transform and aggregate',
    });
    
    const processed = processData(data);
    
    processSpan.end();
    
    // Step 3: Generate PDF
    const pdfSpan = transaction.startChild({
      op: 'generate-pdf',
    });
    
    const pdf = await generatePDF(processed);
    
    pdfSpan.end();
    
    transaction.setStatus(SpanStatus.Ok);
    
    return Response.json({ pdf });
  } catch (error) {
    transaction.setStatus(SpanStatus.InternalError);
    throw error;
  } finally {
    transaction.end();
  }
}
```

## Web Vitals

Sentry automatically tracks Core Web Vitals:

```typescript
// Layout shift is tracked automatically
// Largest Contentful Paint (LCP) is tracked automatically
// First Input Delay (FID) is tracked automatically
// Interaction to Next Paint (INP) is tracked automatically
```

### Custom Web Vitals

```typescript
// components/WebVitals.tsx
'use client';

import { useReportWebVitals } from 'next/web-vitals';

export function WebVitalsReporter() {
  useReportWebVitals((metric) => {
    // Send to Sentry
    console.log(metric);
    
    // metric.name - metric name (LCP, FID, CLS, etc.)
    // metric.value - value in milliseconds
    // metric.id - unique for this page load
    
    if (metric.name === 'CLS') {
      Sentry.captureMessage(`CLS: ${metric.value}`, 'info');
    }
  });
  
  return null;
}
```

## Finding Slow Pages

1. **In Sentry Performance:**
   - Click "Page Loads"
   - Sort by "Slowest"
   - Look for high p75/p95/p99

2. **Common Issues:**
   - Large API responses
   - Slow database queries
   - External API calls
   - Missing loading states

## Common Mistakes

### Mistake #1: Too High Sample Rate
```typescript
// Wrong: Captures 100% in production!
tracesSampleRate: 1.0;
```

```typescript
// Correct: Lower rate in production
tracesSampleRate: process.env.NODE_ENV === 'production' ? 0.1 : 1.0;
```

### Mistake #2: Not Setting Status
```typescript
// Wrong: No status means "unknown"
transaction.end();
```

```typescript
// Correct: Set appropriate status
transaction.setStatus(SpanStatus.Ok);
transaction.end();

// Or on error:
transaction.setStatus(SpanStatus.InternalError);
```

### Mistake #3: Missing Child Spans
```typescript
// Wrong: Can't see what was slow
async function getData() {
  const a = await fetchA();
  const b = await fetchB();
  return { a, b };
}
```

```typescript
// Correct: Add spans to see details
async function getData() {
  const span = Sentry.startInactiveSpan({ name: 'get-data' });
  
  const a = span.startChild({ op: 'fetch', description: 'fetchA' });
  const resultA = await fetchA();
  a.end();
  
  const b = span.startChild({ op: 'fetch', description: 'fetchB' });
  const resultB = await fetchB();
  b.end();
  
  span.end();
  
  return { a: resultA, b: resultB };
}
```

## Summary
- Enable with `tracesSampleRate` in Sentry.init
- View in Performance section of Sentry dashboard
- Use `startInactiveSpan` for custom tracing
- Add child spans for detailed breakdowns
- Web Vitals tracked automatically
- Set appropriate span status (Ok, InternalError, etc.)

## Next Steps
- [custom-breadcrumbs.md](./custom-breadcrumbs.md) — Adding custom breadcrumbs
