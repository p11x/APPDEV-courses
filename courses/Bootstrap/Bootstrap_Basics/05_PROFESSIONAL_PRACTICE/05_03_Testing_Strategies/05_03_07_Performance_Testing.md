---
title: "Performance Testing Bootstrap Applications"
slug: "performance-testing"
difficulty: 2
duration: "45 minutes"
prerequisites:
  - "Bootstrap 5 Optimization"
  - "Web Performance Basics"
  - "Lighthouse"
topics:
  - "Testing"
  - "Performance"
  - "Lighthouse CI"
  - "Core Web Vitals"
  - "CSS Metrics"
tools:
  - "Lighthouse CI"
  - "Web Vitals"
  - "Bundle Analyzer"
learning_objectives:
  - "Configure Lighthouse CI for automated performance audits"
  - "Measure Core Web Vitals impact of Bootstrap components"
  - "Analyze Bootstrap CSS bundle size and unused styles"
  - "Set performance budgets for Bootstrap projects"
---

## Overview

Performance testing measures how Bootstrap affects page load speed, rendering performance, and user experience metrics. Bootstrap's full CSS bundle is ~220KB uncompressed; unused styles bloat pages and slow Time to Interactive (TTI). Core Web Vitals (LCP, FID/INP, CLS) directly impact SEO rankings and user satisfaction.

Lighthouse CI automates performance audits on every commit, catching regressions before they reach production. Combined with bundle analysis and CSS purging, you can ensure Bootstrap delivers only the styles your project actually uses.

## Basic Implementation

### Lighthouse CI Setup

```bash
npm install --save-dev @lhci/cli
```

```json
// lighthouserc.json
{
  "ci": {
    "collect": {
      "url": [
        "http://localhost:3000/index.html",
        "http://localhost:3000/components/cards.html"
      ],
      "numberOfRuns": 3,
      "settings": {
        "onlyCategories": ["performance", "accessibility"],
        "throttlingMethod": "simulate"
      }
    },
    "assert": {
      "assertions": {
        "categories:performance": ["error", { "minScore": 0.8 }],
        "first-contentful-paint": ["error", { "maxNumericValue": 2000 }],
        "largest-contentful-paint": ["error", { "maxNumericValue": 3000 }],
        "cumulative-layout-shift": ["error", { "maxNumericValue": 0.1 }]
      }
    },
    "upload": {
      "target": "temporary-public-storage"
    }
  }
}
```

```bash
# Run Lighthouse CI
npx lhci autorun

# Single page audit
npx lhci collect --url=http://localhost:3000 --assert
```

### Measuring Core Web Vitals

```html
<!-- Add to your Bootstrap page -->
<script type="module">
  import { onCLS, onFID, onLCP, onINP } from 'https://unpkg.com/web-vitals@3?module';

  function sendMetric(metric) {
    console.log(metric.name, metric.value, metric.rating);
    // Send to analytics endpoint
    navigator.sendBeacon('/analytics', JSON.stringify(metric));
  }

  onCLS(sendMetric);
  onFID(sendMetric);
  onLCP(sendMetric);
  onINP(sendMetric);
</script>
```

```js
// __tests__/performance.test.js
const { test, expect } = require('@playwright/test');

test('page meets LCP threshold', async ({ page }) => {
  await page.goto('/index.html');

  const lcp = await page.evaluate(() => {
    return new Promise(resolve => {
      new PerformanceObserver(list => {
        const entries = list.getEntries();
        resolve(entries[entries.length - 1].startTime);
      }).observe({ type: 'largest-contentful-paint', buffered: true });
    });
  });

  expect(lcp).toBeLessThan(2500);
});

test('CLS is within acceptable range', async ({ page }) => {
  await page.goto('/index.html');

  const cls = await page.evaluate(() => {
    return new Promise(resolve => {
      let clsValue = 0;
      new PerformanceObserver(list => {
        for (const entry of list.getEntries()) {
          if (!entry.hadRecentInput) clsValue += entry.value;
        }
        resolve(clsValue);
      }).observe({ type: 'layout-shift', buffered: true });

      setTimeout(() => resolve(clsValue), 3000);
    });
  });

  expect(cls).toBeLessThan(0.1);
});
```

## Advanced Variations

### CSS Bundle Analysis

```bash
# Analyze Bootstrap CSS bundle
npm install --save-dev purgecss @fullhuman/postcss-purgecss

# Before optimization
npx purgecss --css node_modules/bootstrap/dist/css/bootstrap.min.css \
  --content src/**/*.html \
  --output dist/css/
```

```js
// postcss.config.js
const purgeCSSPlugin = require('@fullhuman/postcss-purgecss')({
  content: ['./src/**/*.html', './src/**/*.js'],
  defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || [],
  safelist: {
    standard: [/^modal/, /^tooltip/, /^popover/, /^offcanvas/],
    deep: [/^bs-/],
  },
});

module.exports = {
  plugins: [
    require('cssnano')({ preset: 'default' }),
    ...(process.env.NODE_ENV === 'production' ? [purgeCSSPlugin] : []),
  ],
};
```

### Performance Budget Configuration

```json
// .budgets.json
[
  {
    "path": "/*",
    "budgets": [
      {
        "resourceType": "stylesheet",
        "budget": 50
      },
      {
        "resourceType": "script",
        "budget": 150
      },
      {
        "resourceType": "total",
        "budget": 500
      }
    ]
  }
]
```

```js
// lighthouserc.json with budgets
{
  "ci": {
    "assert": {
      "assertions": {
        "resource-summary:stylesheet:size": ["error", { "maxNumericValue": 51200 }],
        "resource-summary:script:size": ["error", { "maxNumericValue": 153600 }],
        "resource-summary:total:size": ["error", { "maxNumericValue": 512000 }],
        "total-byte-weight": ["error", { "maxNumericValue": 500000 }]
      }
    }
  }
}
```

### CSS Performance Metrics

```js
// tests/css-performance.spec.js
const { test, expect } = require('@playwright/test');

test('Bootstrap CSS loads within budget', async ({ page }) => {
  await page.goto('/index.html');

  const cssSize = await page.evaluate(() => {
    const sheets = [...document.styleSheets];
    let totalRules = 0;
    sheets.forEach(sheet => {
      try { totalRules += sheet.cssRules.length; } catch (e) {}
    });
    return totalRules;
  });

  expect(cssSize).toBeLessThan(5000);
});

test('no unused CSS classes in rendered page', async ({ page }) => {
  await page.goto('/index.html');

  const unusedBootstrapClasses = await page.evaluate(() => {
    const usedClasses = new Set();
    document.querySelectorAll('*').forEach(el => {
      el.classList.forEach(cls => usedClasses.add(cls));
    });

    const allBootstrapClasses = [];
    for (const sheet of document.styleSheets) {
      try {
        for (const rule of sheet.cssRules) {
          if (rule.selectorText?.startsWith('.')) {
            allBootstrapClasses.push(rule.selectorText.replace(/^\./, ''));
          }
        }
      } catch (e) {}
    }

    return allBootstrapClasses.filter(cls => !usedClasses.has(cls)).length;
  });

  console.log(`Potentially unused classes: ${unusedBootstrapClasses}`);
});
```

## Best Practices

1. **Set Lighthouse CI thresholds** - Enforce performance score >= 80 and LCP < 2.5s in CI.
2. **Purge unused Bootstrap CSS** - Use PurgeCSS to remove unused utility classes, reducing bundle by 60-80%.
3. **Import only needed Bootstrap modules** - Use `@import 'bootstrap/scss/buttons'` instead of full `bootstrap.min.css`.
4. **Measure with simulated throttling** - Lighthouse's "Simulated Slow 4G" reflects real user conditions.
5. **Track performance over time** - Use Lighthouse CI's `--uploadTarget=temporary-public-storage` to compare PRs.
6. **Minimize Bootstrap JS imports** - Import individual plugins (`bootstrap/js/dist/modal`) instead of the full bundle.
7. **Use `font-display: swap`** - Prevent font-loading render blocking for Bootstrap's web fonts.
8. **Defer non-critical CSS** - Load Bootstrap's print styles or optional components asynchronously.
9. **Test on low-end devices** - Lighthouse simulates mid-range devices; test on actual low-end phones.
10. **Monitor CLS during interactions** - Modals, dropdowns, and toasts can cause layout shifts.
11. **Use CDN caching** - Serve `bootstrap.min.css` from a CDN with long `max-age` headers.
12. **Compress assets** - Enable Brotli or Gzip for Bootstrap CSS/JS in production.

## Common Pitfalls

1. **Importing full Bootstrap when using 10%** - The full CSS bundle includes hundreds of unused rules.
2. **Not minifying production CSS** - `bootstrap.min.css` is minified, but custom overrides may not be.
3. **Ignoring CSS specificity costs** - Overriding Bootstrap with `!important` adds parsing overhead.
4. **Loading Bootstrap JS synchronously** - Render-blocking scripts delay TTI; use `defer` or dynamic import.
5. **Not measuring real user metrics** - Lab scores differ from field data; use `web-vitals` library for real monitoring.
6. **Testing only desktop performance** - Mobile performance is typically 2-3x slower; prioritize mobile testing.
7. **Forgetting third-party scripts** - Analytics, ads, and chat widgets often dominate performance budgets.
8. **Using `@import` in CSS** - Each `@import` creates a blocking request; use `<link>` tags or Sass compilation.
9. **Not setting `font-display`** - Bootstrap's default fonts may block rendering without `font-display: swap`.
10. **Ignoring cumulative script size** - Bootstrap JS + Popper.js + custom scripts can exceed 200KB.

## Accessibility Considerations

Performance and accessibility intersect:

- **Fast LCP improves screen reader experience** - Users with assistive technologies benefit from faster content rendering.
- **CLS affects screen reader focus** - Layout shifts cause screen readers to lose their reading position.
- **Reducing motion for vestibular disorders** - Bootstrap's animations should respect `prefers-reduced-motion`; test performance impact.

```css
/* Reduce motion to improve both accessibility and performance */
@media (prefers-reduced-motion: reduce) {
  *, *::before, *::after {
    animation-duration: 0.01ms !important;
    transition-duration: 0.01ms !important;
    scroll-behavior: auto !important;
  }
}
```

```js
// Test reduced motion compliance
test('respects prefers-reduced-motion', async ({ page }) => {
  await page.emulateMedia({ reducedMotion: 'reduce' });
  await page.goto('/components/modal.html');

  const duration = await page.$eval('.modal', el =>
    getComputedStyle(el).transitionDuration
  );
  expect(parseFloat(duration)).toBeLessThan(0.1);
});
```

## Responsive Behavior

Performance varies across viewports:

```js
// Test performance at different viewport sizes
const viewports = [
  { name: 'mobile', width: 375, height: 667 },
  { name: 'tablet', width: 768, height: 1024 },
  { name: 'desktop', width: 1280, height: 720 },
];

for (const vp of viewports) {
  test(`LCP at ${vp.name}`, async ({ page }) => {
    await page.setViewportSize({ width: vp.width, height: vp.height });
    await page.goto('/index.html');

    const lcp = await page.evaluate(() => {
      return new Promise(resolve => {
        new PerformanceObserver(list => {
          const entries = list.getEntries();
          resolve(entries[entries.length - 1].startTime);
        }).observe({ type: 'largest-contentful-paint', buffered: true });
      });
    });

    const threshold = vp.width < 768 ? 3000 : 2500;
    expect(lcp).toBeLessThan(threshold);
  });
}
```

Mobile viewports typically load fewer visible elements, which can reduce LCP but increase layout shifts from lazy-loaded content. Test both metrics at each breakpoint.
