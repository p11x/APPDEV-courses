# Custom Vitals Reporting

## What You'll Learn
- Report custom performance metrics
- Track specific user interactions
- Send to analytics

## Prerequisites
- Next.js project

## Do I Need This Right Now?
Custom vitals let you track what matters to your specific app.

## Using web-vitals Package

```typescript
// app/components/WebVitals.tsx
'use client';

import { useReportWebVitals } from 'next/web-vitals';

export function WebVitalsReporter() {
  useReportWebVitals((metric) => {
    console.log(metric);
    
    // metric.name: 'CLS', 'FCP', 'FID', 'LCP', 'INP'
    // metric.value: number (ms or score)
    // metric.id: unique identifier
  });
  
  return null;
}
```

## Sending to Google Analytics

```typescript
// app/components/WebVitals.tsx
'use client';

import { useReportWebVitals } from 'next/web-vitals';

export function WebVitalsReporter() {
  useReportWebVitals((metric) => {
    // Send to GA4
    if (typeof window.gtag === 'function') {
      window.gtag('event', metric.name, {
        value: Math.round(metric.value),
        event_category: 'Web Vitals',
        event_label: metric.id,
      });
    }
  });
  
  return null;
}
```

## Summary
- Use next/web-vitals for custom reporting
- Track specific metrics that matter to your app
- Send to analytics for analysis
