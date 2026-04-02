---
title: "Real User Monitoring for Bootstrap Sites"
description: "Setting up RUM for Bootstrap sites, tracking real-world performance, and analyzing performance data"
difficulty: 3
tags: ["performance", "rum", "monitoring", "analytics", "bootstrap"]
prerequisites: ["04_07_02_Core_Web_Vitals", "04_07_07_Performance_Budgets"]
---

## Overview

Real User Monitoring (RUM) captures performance data from actual visitors interacting with your Bootstrap site. Unlike lab tools like Lighthouse that test under controlled conditions, RUM reveals how your site performs across real devices, networks, and geographic locations. This data exposes performance regressions that synthetic tests miss.

RUM tracks Core Web Vitals (LCP, INP, CLS), page load timings, resource loading, and custom Bootstrap component metrics from production traffic. Analyzing this data by device type, connection speed, and page path reveals which Bootstrap components perform well and which need optimization.

## Basic Implementation

```html
<!-- Basic RUM setup with web-vitals library -->
<head>
  <link rel="preconnect" href="https://cdn.jsdelivr.net">
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
  <!-- Bootstrap content -->
  <div class="container py-5">
    <h1>RUM Demo</h1>
    <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#demoModal">
      Open Modal
    </button>
  </div>

  <script type="module">
    import { onLCP, onINP, onCLS } from 'https://cdn.jsdelivr.net/npm/web-vitals@3/dist/web-vitals.js';

    function sendMetric(metric) {
      const payload = JSON.stringify({
        name: metric.name,
        value: metric.value,
        rating: metric.rating,
        delta: metric.delta,
        id: metric.id,
        page: location.pathname,
        connection: navigator.connection?.effectiveType || 'unknown',
        deviceMemory: navigator.deviceMemory || 'unknown',
        timestamp: Date.now()
      });

      // Use sendBeacon for reliable delivery on page unload
      if (navigator.sendBeacon) {
        navigator.sendBeacon('/api/rum', payload);
      }
    }

    onLCP(sendMetric);
    onINP(sendMetric);
    onCLS(sendMetric);
  </script>
</body>
```

```js
// Express.js RUM collection endpoint
const express = require('express');
const app = express();

app.use(express.json({ limit: '10kb' }));

app.post('/api/rum', (req, res) => {
  const metric = req.body;

  // Validate and store metric
  console.log(`[${metric.name}] ${metric.value.toFixed(2)} (${metric.rating})`);
  console.log(`  Page: ${metric.page}`);
  console.log(`  Connection: ${metric.connection}`);

  // In production: write to database or analytics service
  // db.collection('rum_metrics').insertOne(metric);

  res.status(204).end();
});

app.listen(3001);
```

## Advanced Variations

```js
// Comprehensive RUM tracker with Bootstrap-specific metrics
class BootstrapRUM {
  constructor(endpoint) {
    this.endpoint = endpoint;
    this.metrics = [];
    this.init();
  }

  init() {
    this.trackCoreWebVitals();
    this.trackBootstrapComponents();
    this.trackResourceTiming();
    this.trackNavigationTiming();
  }

  trackCoreWebVitals() {
    import('https://cdn.jsdelivr.net/npm/web-vitals@3/dist/web-vitals.js')
      .then(({ onLCP, onINP, onCLS }) => {
        const send = (metric) => this.report(metric);
        onLCP(send);
        onINP(send);
        onCLS(send);
      });
  }

  trackBootstrapComponents() {
    // Track modal open/close timing
    document.addEventListener('show.bs.modal', () => {
      performance.mark('bs-modal-show');
    });

    document.addEventListener('shown.bs.modal', () => {
      performance.mark('bs-modal-shown');
      performance.measure('bs-modal-open', 'bs-modal-show', 'bs-modal-shown');
      const measure = performance.getEntriesByName('bs-modal-open').pop();
      this.report({
        name: 'bootstrap-modal-open',
        value: measure.duration,
        rating: measure.duration > 100 ? 'poor' : 'good'
      });
    });

    // Track carousel slide transitions
    document.addEventListener('slid.bs.carousel', (e) => {
      this.report({
        name: 'bootstrap-carousel-slide',
        value: e.direction || 'unknown',
        rating: 'info'
      });
    });
  }

  trackResourceTiming() {
    const observer = new PerformanceObserver((list) => {
      for (const entry of list.getEntries()) {
        if (entry.name.includes('bootstrap')) {
          this.report({
            name: 'bootstrap-resource-load',
            value: entry.duration,
            resource: entry.name.split('/').pop(),
            size: entry.transferSize
          });
        }
      }
    });
    observer.observe({ entryTypes: ['resource'] });
  }

  trackNavigationTiming() {
    window.addEventListener('load', () => {
      const nav = performance.getEntriesByType('navigation')[0];
      this.report({
        name: 'page-load',
        value: nav.loadEventEnd,
        domContentLoaded: nav.domContentLoadedEventEnd,
        ttfb: nav.responseStart
      });
    });
  }

  report(metric) {
    const enriched = {
      ...metric,
      page: location.pathname,
      viewport: `${window.innerWidth}x${window.innerHeight}`,
      connection: navigator.connection?.effectiveType || 'unknown',
      deviceMemory: navigator.deviceMemory || 'unknown',
      timestamp: Date.now(),
      userAgent: navigator.userAgent.slice(0, 100)
    };

    this.metrics.push(enriched);

    // Batch send every 10 seconds
    if (this.metrics.length >= 10) {
      this.flush();
    }
  }

  flush() {
    if (this.metrics.length === 0) return;

    const payload = JSON.stringify(this.metrics);
    navigator.sendBeacon(this.endpoint, payload);
    this.metrics = [];
  }
}

const rum = new BootstrapRUM('/api/rum');
```

## Best Practices

1. Use `navigator.sendBeacon()` for reliable metric delivery on page unload
2. Sample RUM data — collect from 10-20% of users to reduce storage costs
3. Segment RUM data by connection type (4G, 3G, 2G) to understand offline impact
4. Track Bootstrap-specific metrics like modal open time and carousel slide duration
5. Include viewport dimensions to correlate performance with responsive breakpoints
6. Set up alerting when P75 Core Web Vitals exceed "Good" thresholds
7. Use real device data to prioritize which Bootstrap components need optimization
8. Compare RUM data against lab measurements to validate synthetic test accuracy
9. Anonymize user data and comply with privacy regulations in RUM collection
10. Store RUM data with timestamps for trend analysis over time

## Common Pitfalls

1. **Blocking page load on RUM script** — Loading RUM synchronously delays Bootstrap rendering; always use `defer` or dynamic import
2. **Collecting too many metrics** — Excessive RUM data increases storage costs and analysis complexity; focus on actionable metrics
3. **Ignoring geographic variance** — CDN performance varies by region; segment RUM data by location
4. **No sampling strategy** — Collecting 100% of user data creates massive storage overhead; sample 10-20%
5. **Missing connection type data** — Without `navigator.connection`, you cannot correlate Bootstrap performance with network conditions
6. **Not correlating with deployments** — RUM data without deployment timestamps makes it impossible to identify regression-causing releases

## Accessibility Considerations

RUM can track assistive technology usage patterns. Monitor INP for keyboard-only users, since Bootstrap modal focus-trap and dropdown keyboard navigation may have different latency profiles than mouse interactions. Ensure RUM scripts do not interfere with screen reader announcements or focus management.

## Responsive Behavior

Segment RUM data by viewport width to understand Bootstrap's responsive performance. Track LCP separately for mobile (≤768px) and desktop (>768px) since Bootstrap's grid recalculation at breakpoints affects rendering differently. Monitor CLS on mobile where stacked columns and collapsing navbars cause more layout shifts.
