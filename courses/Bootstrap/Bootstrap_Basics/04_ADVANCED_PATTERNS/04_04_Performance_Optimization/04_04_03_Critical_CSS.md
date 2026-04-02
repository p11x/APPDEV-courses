---
title: "Critical CSS"
module: "Performance Optimization"
difficulty: 3
duration: "35 minutes"
prerequisites: ["CSS optimization", "Build tools"]
tags: ["critical-css", "performance", "rendering", "optimization"]
---

# Critical CSS

## Overview

Critical CSS extracts the minimal styles needed to render above-the-fold content, inlining them directly in the HTML `<head>`. This eliminates render-blocking CSS requests for the initial viewport, significantly improving First Contentful Paint (FCP) and Largest Contentful Paint (LCP) metrics. Non-critical styles are deferred and loaded asynchronously.

## Basic Implementation

Extract critical CSS using the `critical` npm package:

```bash
npm install --save-dev critical
```

```js
// scripts/extract-critical.js
const critical = require('critical');

critical.generate({
  base: 'dist/',
  src: 'index.html',
  css: ['dist/css/custom.min.css'],
  dimensions: [
    { width: 375, height: 667 },   // Mobile
    { width: 768, height: 1024 },  // Tablet
    { width: 1280, height: 800 }   // Desktop
  ],
  inline: true,
  extract: true
}).then(({ css, html }) => {
  console.log('Critical CSS extracted:', css.length, 'bytes');
});
```

Inline critical CSS and defer the rest:

```html
<head>
  <style id="critical-css">
    /* Inlined critical CSS extracted at build time */
    :root { --bs-primary: #6366f1; }
    body { margin: 0; font-family: Inter, sans-serif; }
    .container { max-width: 1140px; margin: 0 auto; padding: 0 15px; }
    .row { display: flex; flex-wrap: wrap; margin: 0 -15px; }
    .col-md-6 { flex: 0 0 50%; max-width: 50%; padding: 0 15px; }
    .btn-primary { background: var(--bs-primary); color: #fff; }
  </style>

  <!-- Defer non-critical CSS -->
  <link rel="preload" href="/css/custom.min.css" as="style"
        onload="this.onload=null;this.rel='stylesheet'">
  <noscript>
    <link rel="stylesheet" href="/css/custom.min.css">
  </noscript>
</head>
```

## Advanced Variations

Automate critical CSS extraction in build pipeline:

```js
// gulpfile.js - Critical CSS task
const { src, dest } = require('gulp');
const critical = require('critical').stream;

function criticalCss() {
  return src('dist/*.html')
    .pipe(critical({
      base: 'dist/',
      inline: true,
      css: ['dist/css/custom.min.css'],
      dimensions: [
        { width: 320, height: 568 },
        { width: 768, height: 1024 },
        { width: 1440, height: 900 }
      ],
      penthouse: {
        timeout: 60000
      }
    }))
    .on('error', err => console.error(err))
    .pipe(dest('dist'));
}
```

Create a page-specific critical CSS strategy:

```js
// Critical CSS per page type
const pages = [
  { name: 'home', src: 'dist/index.html', css: 'dist/css/home.css' },
  { name: 'product', src: 'dist/product.html', css: 'dist/css/product.css' },
  { name: 'checkout', src: 'dist/checkout.html', css: 'dist/css/checkout.css' }
];

async function extractAllCritical() {
  for (const page of pages) {
    await critical.generate({
      base: 'dist/',
      src: page.src,
      css: [page.css, 'dist/css/bootstrap.min.css'],
      inline: true,
      dimensions: [{ width: 1280, height: 800 }]
    });
  }
}
```

Implement with Vite plugin:

```js
// vite.config.js
import { defineConfig } from 'vite';
import critters from 'critters-webpack-plugin';

export default defineConfig({
  plugins: [
    {
      name: 'critical-css',
      transformIndexHtml: {
        order: 'post',
        handler(html) {
          // Critters integration for inline critical CSS
          return html;
        }
      }
    }
  ]
});
```

## Best Practices

1. Extract critical CSS for each unique page layout
2. Include mobile, tablet, and desktop viewport dimensions
3. Inline critical CSS in the `<head>` before any external stylesheets
4. Use `preload` for deferred stylesheets
5. Include `<noscript>` fallback for non-JS users
6. Re-extract critical CSS when layouts change
7. Automate extraction in CI/CD pipeline
8. Monitor critical CSS size (keep under 14KB for first TCP packet)
9. Test rendering with throttled network connections
10. Include above-the-fold component styles in critical CSS
11. Version critical CSS alongside page templates

## Common Purposes

1. Extracting too much CSS (defeats the purpose)
2. Not re-extracting after component changes
3. Missing viewport dimensions for key breakpoints
4. Breaking JavaScript-dependent components by deferring their CSS
5. Not including fonts in critical rendering path
6. Ignoring server-side rendering implications
7. Forgetting `<noscript>` fallback for CSS
8. Not testing with real device viewport sizes

## Accessibility Considerations

- Ensure critical CSS includes focus indicator styles
- Verify text remains readable during CSS loading
- Test with screen readers during deferred loading
- Include reduced-motion media queries in critical CSS
- Maintain color contrast during progressive enhancement

## Responsive Behavior

- Extract critical CSS for all major viewport sizes
- Test above-the-fold rendering at each breakpoint
- Ensure responsive grid styles are included in critical CSS
- Verify deferred CSS loads before user scrolls
