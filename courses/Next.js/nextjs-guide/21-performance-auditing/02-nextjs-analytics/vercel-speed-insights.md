# Vercel Speed Insights

## What You'll Learn
- Set up Vercel Speed Insights
- Understand real user metrics
- Monitor performance over time

## Prerequisites
- Vercel account and deployed app

## Do I Need This Right Now?
Speed Insights shows real user performance data. It's essential for understanding how your app performs for actual users.

## Setting Up Speed Insights

### Automatic (Vercel Deployments)

Speed Insights works automatically on Vercel!

1. Deploy to Vercel
2. Go to Speed Insights in dashboard
3. View real user metrics

### Manual Setup

```bash
# Install package
npm install @vercel/speed-insights
```

```typescript
// app/layout.tsx
import { SpeedInsights } from '@vercel/speed-insights/next';

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>
        {children}
        <SpeedInsights />
      </body>
    </html>
  );
}
```

## Understanding Speed Insights

### Metrics Shown

| Metric | Description |
|--------|-------------|
| LCP | Largest Contentful Paint |
| FID | First Input Delay |
| CLS | Cumulative Layout Shift |
| INP | Interaction to Next Paint |
| TTFB | Time to First Byte |

### Data View

- **Real User Data:** Actual visitors
- **Lab Data:** Synthetic testing
- **Device Breakdown:** Mobile vs Desktop
- **Geographic:** Performance by region

## Common Issues

### No Data Showing

```typescript
// Wrong: SpeedInsights not imported correctly
import { SpeedInsights } from '@vercel/speed-insights';

// Make sure to use the Next.js version
import { SpeedInsights } from '@vercel/speed-insights/next';
```

### Debug Mode

```typescript
// Enable debug mode
<SpeedInsights debug={true} />
```

## Summary
- Speed Insights works automatically on Vercel
- Use @vercel/speed-insights for manual setup
- View real user metrics in dashboard
- Check for mobile vs desktop differences

## Next Steps
- [web-vitals-explained.md](./web-vitals-explained.md) — Web Vitals deep dive
