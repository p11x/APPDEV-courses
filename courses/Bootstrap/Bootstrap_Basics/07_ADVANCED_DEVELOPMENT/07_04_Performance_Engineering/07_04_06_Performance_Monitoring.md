---
title: "Performance Monitoring"
difficulty: 2
category: "Advanced Development"
subcategory: "Performance Engineering"
prerequisites:
  - Core Web Vitals
  - Performance Observer API
  - RUM (Real User Monitoring)
---

## Overview

Performance monitoring tracks real user metrics (Core Web Vitals) for Bootstrap-based applications in production. The three Core Web Vitals - Largest Contentful Paint (LCP), Interaction to Next Paint (INP), and Cumulative Layout Shift (CLS) - directly impact user experience and search engine rankings. Monitoring these metrics with the Performance Observer API and sending them to an analytics backend provides actionable data for optimization.

Bootstrap components influence Core Web Vitals: large hero images affect LCP, interactive modals and dropdowns affect INP, and lazy-loaded content without reserved space causes CLS. Understanding these relationships helps prioritize optimization efforts.

## Basic Implementation

```js
// Core Web Vitals monitoring
// lib/performance.js
function reportMetric(name, value, rating) {
  // Send to analytics endpoint
  navigator.sendBeacon('/api/metrics', JSON.stringify({
    name,
    value,
    rating,
    url: window.location.href,
    timestamp: Date.now()
  }));
}

function observeLCP() {
  const observer = new PerformanceObserver((list) => {
    const entries = list.getEntries();
    const lastEntry = entries[entries.length - 1];

    const rating = lastEntry.startTime <= 2500 ? 'good'
      : lastEntry.startTime <= 4000 ? 'needs-improvement'
      : 'poor';

    reportMetric('LCP', lastEntry.startTime, rating);
  });
  observer.observe({ type: 'largest-contentful-paint', buffered: true });
}

function observeCLS() {
  let clsValue = 0;
  const observer = new PerformanceObserver((list) => {
    for (const entry of list.getEntries()) {
      if (!entry.hadRecentInput) {
        clsValue += entry.value;
      }
    }
  });
  observer.observe({ type: 'layout-shift', buffered: true });

  // Report on page hide
  document.addEventListener('visibilitychange', () => {
    if (document.visibilityState === 'hidden') {
      const rating = clsValue <= 0.1 ? 'good'
        : clsValue <= 0.25 ? 'needs-improvement'
        : 'poor';
      reportMetric('CLS', clsValue, rating);
    }
  });
}

function observeINP() {
  const observer = new PerformanceObserver((list) => {
    for (const entry of list.getEntries()) {
      if (entry.interactionId) {
        const inp = entry.duration;
        const rating = inp <= 200 ? 'good'
          : inp <= 500 ? 'needs-improvement'
          : 'poor';
        reportMetric('INP', inp, rating);
      }
    }
  });
  observer.observe({ type: 'event', buffered: true, durationThreshold: 16 });
}

// Initialize monitoring
observeLCP();
observeCLS();
observeINP();
```

```js
// Performance dashboard data aggregation
// scripts/perf-report.js
function generateDashboard(metrics) {
  const report = {
    lcp: { p50: 0, p75: 0, p95: 0 },
    cls: { p50: 0, p75: 0, p95: 0 },
    inp: { p50: 0, p75: 0, p95: 0 }
  };

  ['lcp', 'cls', 'inp'].forEach(metric => {
    const values = metrics
      .filter(m => m.name === metric.toUpperCase())
      .map(m => m.value)
      .sort((a, b) => a - b);

    if (values.length) {
      report[metric].p50 = values[Math.floor(values.length * 0.5)];
      report[metric].p75 = values[Math.floor(values.length * 0.75)];
      report[metric].p95 = values[Math.floor(values.length * 0.95)];
    }
  });

  return report;
}
```

## Best Practices

1. **Track all three Core Web Vitals** - LCP, INP, and CLS each measure different aspects of user experience.
2. **Report on page hide** - Use `visibilitychange` event to capture complete metric values.
3. **Use sendBeacon** - Reliable reporting even during page unload.
4. **Aggregate percentiles** - P75 is the standard threshold; P95 catches edge cases.
5. **Monitor in production** - Lab testing doesn't capture real user conditions.
6. **Set performance budgets** - Alert when metrics exceed thresholds.
7. **Track by page type** - Dashboard pages have different expectations than landing pages.
8. **Include device/network data** - Segment metrics by connection type and device.
9. **Correlate with deployments** - Track metric changes after each release.
10. **Create visual dashboards** - Make metrics visible to the entire team.

## Common Pitfalls

1. **Sampling too aggressively** - Missing performance regressions between releases.
2. **Ignoring INP** - FID is deprecated; INP is the current interactivity metric.
3. **Not reporting on hide** - Metrics are incomplete if reported before page interaction ends.
4. **Lab-only testing** - Lighthouse scores don't reflect real-world performance.
5. **No alerting** - Metrics collected but no one reviews them.

## Accessibility Considerations

Monitor accessibility-related performance: screen reader announcement delay, focus management timing, and keyboard navigation latency.

## Responsive Behavior

Track metrics segmented by viewport size. Mobile performance is typically worse and requires more aggressive optimization.

```js
function reportMetric(name, value) {
  const viewport = window.innerWidth < 768 ? 'mobile' : 'desktop';
  navigator.sendBeacon('/api/metrics', JSON.stringify({
    name, value, viewport
  }));
}
```
