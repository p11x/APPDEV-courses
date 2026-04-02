---
title: "CSS Coverage Analysis for Bootstrap"
description: "Using Chrome DevTools Coverage tab and PurgeCSS to remove unused Bootstrap styles"
difficulty: 2
tags: ["performance", "css", "coverage", "purgecss", "bootstrap"]
prerequisites: ["04_07_01_Lighthouse_Audit"]
---

## Overview

Bootstrap ships approximately 200KB of CSS covering every component and utility class. Most pages use only 20-30% of this CSS, meaning the browser downloads, parses, and stores styles it never renders. CSS coverage analysis identifies unused rules so you can eliminate them, reducing bundle size and improving parse time.

Chrome DevTools provides a built-in Coverage tab that visually maps used versus unused bytes. Combined with build tools like PurgeCSS, you can automate the removal of dead CSS while preserving every Bootstrap class your templates actually reference.

## Basic Implementation

```html
<!-- Page using only grid, buttons, and cards -->
<!DOCTYPE html>
<html lang="en">
<head>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
  <div class="container">
    <div class="row">
      <div class="col-md-6">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Coverage Demo</h5>
            <p class="card-text">This page only uses grid, card, and button classes.</p>
            <a href="#" class="btn btn-primary">Action</a>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Second Card</h5>
            <p class="card-text">Unused modal, carousel, accordion styles still load.</p>
            <button class="btn btn-outline-secondary">Cancel</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</body>
</html>
```

```bash
# Open Chrome DevTools Coverage panel
# 1. Open DevTools (F12)
# 2. Click the >> menu icon
# 3. Select "Coverage"
# 4. Click the reload button to capture initial load coverage
# 5. Click "Coverage" entry for bootstrap.min.css
# 6. Red = unused, Blue = used bytes

# Typical result: ~75% unused CSS on a simple Bootstrap page
```

```js
// PurgeCSS configuration for a static HTML project
// purgecss.config.js
module.exports = {
  content: [
    './src/**/*.html',
    './src/**/*.js'
  ],
  css: ['./node_modules/bootstrap/dist/css/bootstrap.min.css'],
  output: './dist/css/',
  // Safelist Bootstrap classes that are added dynamically
  safelist: {
    standard: [
      /^modal/,
      /^show/,
      /^active/,
      /^collaps/,
      /^fade/,
      /^offcanvas/
    ],
    deep: [/tooltip/, /popover/],
    greedy: [/^bs-/]
  }
};
```

## Advanced Variations

```js
// Webpack + PurgeCSS integration
const PurgeCSSPlugin = require('purgecss-webpack-plugin');
const glob = require('glob-all');
const path = require('path');

module.exports = {
  plugins: [
    new PurgeCSSPlugin({
      paths: glob.sync([
        path.join(__dirname, 'src/**/*.html'),
        path.join(__dirname, 'src/**/*.js'),
        path.join(__dirname, 'src/**/*.jsx')
      ]),
      safelist: {
        standard: [/^show$/, /^active$/, /^collaps/, /^fade/],
        deep: [/tooltip/, /popover/, /carousel/],
        greedy: [/^bs-/, /^data-bs-/]
      },
      // Reject selectors not found in content files
      rejected: true,
      // Print removed selectors for debugging
      stdout: true
    })
  ]
};
```

```js
// Programmatic coverage extraction via Chrome DevTools Protocol
const puppeteer = require('puppeteer');

async function analyzeCSSCoverage(url) {
  const browser = await puppeteer.launch();
  const page = await browser.newPage();

  await Promise.all([
    page.coverage.startCSSCoverage(),
    page.goto(url, { waitUntil: 'networkidle0' })
  ]);

  const coverage = await page.coverage.stopCSSCoverage();

  let totalBytes = 0;
  let usedBytes = 0;

  for (const entry of coverage) {
    totalBytes += entry.text.length;
    for (const range of entry.ranges) {
      usedBytes += range.end - range.start;
    }

    console.log(`${entry.url}: ${((usedBytes / totalBytes) * 100).toFixed(1)}% used`);
  }

  console.log(`\nTotal: ${totalBytes} bytes, Used: ${usedBytes} bytes`);
  console.log(`Wasted: ${((1 - usedBytes / totalBytes) * 100).toFixed(1)}%`);

  await browser.close();
}

analyzeCSSCoverage('http://localhost:3000');
```

## Best Practices

1. Run coverage analysis on every significant page template, not just the homepage
2. Safelist Bootstrap classes added by JavaScript plugins (`show`, `active`, `fade`, `collapsing`)
3. Include all HTML templates and JS files in PurgeCSS content paths
4. Use `rejected: true` in PurgeCSS to review what gets removed before deploying
5. Keep the full Bootstrap source in development; only purge in production builds
6. Re-run coverage analysis after adding new Bootstrap components or pages
7. Separate vendor CSS from application CSS for granular coverage reporting
8. Document which Bootstrap modules you intentionally include in your build
9. Test purged CSS across all pages — a class missing from one template breaks that page
10. Automate coverage thresholds in CI to fail builds if unused CSS exceeds 40%

## Common Pitfalls

1. **Not safelisting dynamic classes** — Bootstrap adds `.show`, `.active`, `.fade` via JavaScript; PurgeCSS removes them if not safelisted, breaking modals and dropdowns
2. **Running coverage only on the homepage** — Inner pages often use different components; missing classes cause broken layouts
3. **Ignoring pseudo-class selectors** — `:hover`, `:focus`, `:disabled` states aren't captured in static coverage and may be incorrectly removed
4. **Purging tooltip/popover content** — Bootstrap's `data-bs-title` attribute generates DOM at runtime; those selectors must be safelisted
5. **Forgetting responsive variants** — `.d-none .d-md-block` may not appear in initial HTML but are critical for responsive behavior
6. **Over-purging utility classes** — Aggressive removal of spacing and display utilities causes layout regressions on edge-case content

## Accessibility Considerations

Removing unused CSS does not affect accessibility when done correctly. However, ensure PurgeCSS does not strip `.visually-hidden`, `.sr-only` utility classes, or focus-visible styling that screen readers and keyboard users depend on. Always test purged output with a screen reader and keyboard-only navigation.

## Responsive Behavior

Bootstrap's responsive utility classes (`d-sm-none`, `col-lg-4`, `text-md-center`) may appear unused in desktop viewport coverage because they only activate at specific breakpoints. Run coverage analysis at multiple viewport widths (mobile, tablet, desktop) or ensure PurgeCSS scans all responsive variants through comprehensive template analysis.
