---
title: "Library Performance"
difficulty: 3
category: "Advanced Development"
subcategory: "Component Libraries"
prerequisites:
  - Webpack Bundle Analyzer
  - Performance Budgets
  - Lazy Loading Patterns
---

## Overview

Performance optimization for a Bootstrap component library focuses on bundle analysis, lazy loading components on demand, and enforcing performance budgets in CI. The goal is to minimize the CSS and JavaScript payload delivered to users while maintaining full component functionality.

Bundle analysis identifies heavy dependencies, unused CSS rules, and JavaScript that can be deferred. Lazy loading uses dynamic imports to load component code only when the component is actually used on the page. Performance budgets set hard limits on bundle size growth that CI enforces on every pull request.

## Basic Implementation

```js
// webpack.config.js with bundle analysis
const { BundleAnalyzerPlugin } = require('webpack-bundle-analyzer');
const CompressionPlugin = require('compression-webpack-plugin');

module.exports = {
  entry: './src/index.js',
  output: {
    filename: '[name].[contenthash].js',
    chunkFilename: '[name].[contenthash].chunk.js'
  },
  plugins: [
    new BundleAnalyzerPlugin({
      analyzerMode: 'static',
      reportFilename: 'bundle-report.html',
      openAnalyzer: false
    }),
    new CompressionPlugin({
      algorithm: 'gzip',
      test: /\.(js|css|svg)$/,
      threshold: 10240
    })
  ],
  optimization: {
    splitChunks: {
      chunks: 'all',
      cacheGroups: {
        bootstrap: {
          test: /[\\/]node_modules[\\/]bootstrap[\\/]/,
          name: 'bootstrap',
          priority: 10
        },
        components: {
          test: /[\\/]src[\\/]components[\\/]/,
          name: 'components',
          minChunks: 1
        }
      }
    }
  }
};
```

```js
// Lazy-loaded components
// src/index.js
export { default as Card } from './components/Card';

// Lazy exports - loaded on demand
export const Modal = () => import('./components/Modal');
export const DataTable = () => import('./components/DataTable');
export const Tooltip = () => import('./components/Tooltip');

// Lazy init with Intersection Observer
export function lazyInit(selector, importFn) {
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        importFn().then(module => {
          module.default.getOrCreateInstance(entry.target);
        });
        observer.unobserve(entry.target);
      }
    });
  }, { rootMargin: '100px' });

  document.querySelectorAll(selector).forEach(el => observer.observe(el));
}
```

```json
// performance-budget.js
module.exports = [
  {
    name: 'CSS',
    test: /\.css$/,
    maxSize: '50kb',
    warnOnExceed: true
  },
  {
    name: 'JS',
    test: /\.js$/,
    maxSize: '100kb',
    warnOnExceed: false
  }
];
```

## Advanced Variations

```js
// CI performance budget enforcement
// scripts/check-bundle-size.js
const fs = require('fs');
const path = require('path');
const gzipSize = require('gzip-size');

const BUDGETS = {
  'dist/css/components.min.css': { max: 50 * 1024, label: 'CSS' },
  'dist/js/index.min.js': { max: 80 * 1024, label: 'JS Core' },
  'dist/js/components.min.js': { max: 120 * 1024, label: 'JS Components' }
};

let failed = false;

Object.entries(BUDGETS).forEach(([file, budget]) => {
  if (!fs.existsSync(file)) {
    console.warn(`File not found: ${file}`);
    return;
  }

  const content = fs.readFileSync(file);
  const rawSize = content.length;
  const gzSize = gzipSize.sync(content);

  console.log(`${budget.label}: ${(rawSize / 1024).toFixed(1)}KB raw, ${(gzSize / 1024).toFixed(1)}KB gzipped (budget: ${(budget.max / 1024).toFixed(0)}KB)`);

  if (gzSize > budget.max) {
    console.error(`BUDGET EXCEEDED: ${budget.label} is ${(gzSize - budget.max) / 1024}KB over budget`);
    failed = true;
  }
});

if (failed) {
  console.error('\nBundle size budget exceeded. See details above.');
  process.exit(1);
}

console.log('\nAll bundles within budget.');
```

## Best Practices

1. **Analyze before optimizing** - Use bundle analyzer to find actual bottlenecks, not assumed ones.
2. **Set performance budgets** - Define max CSS, JS, and total bundle sizes in CI.
3. **Lazy load heavy components** - Modal, DataTable, and Tooltip are good candidates for dynamic import.
4. **Split vendor and app code** - Separate Bootstrap from your components for better caching.
5. **Use Brotli compression** - Brotli achieves 15-25% better compression than gzip for text assets.
6. **Tree-shake CSS** - Use PurgeCSS or Lightning CSS to remove unused rules from production builds.
7. **Defer non-critical JS** - Load component JS after the initial page render.
8. **Monitor in production** - Track real-world bundle sizes with tools like bundlesize or size-limit.
9. **Preload critical resources** - Use `<link rel="preload">` for above-the-fold CSS.
10. **Audit third-party dependencies** - Each npm dependency adds to bundle size; justify every one.

## Common Pitfalls

1. **Importing all of Bootstrap** - `import 'bootstrap'` pulls in every plugin; import only what's needed.
2. **No code splitting** - Single monolithic bundle forces users to download everything.
3. **Ignoring gzip/brotli** - Reporting only raw sizes misrepresents actual transfer size.
4. **CSS duplication** - Importing the same SCSS file in multiple entry points duplicates rules.
5. **Missing performance CI** - Without budget enforcement, bundle size silently grows over time.

## Accessibility Considerations

Lazy loading must not break accessibility. When a component loads asynchronously, ensure focus management and ARIA announcements account for the loading delay.

```html
<!-- Accessible lazy loading -->
<div data-lazy-component="DataTable"
     aria-busy="true"
     aria-label="Loading data table...">
  <div class="spinner-border" role="status">
    <span class="visually-hidden">Loading...</span>
  </div>
</div>
```

## Responsive Behavior

Performance budgets should account for mobile-first delivery. Mobile users have slower connections and less powerful devices, making bundle size more critical.

```js
// Conditional loading based on viewport
if (window.innerWidth >= 768) {
  // Load heavy desktop components
  import('./components/DataTable');
} else {
  // Use lightweight mobile list
  import('./components/MobileList');
}
```
