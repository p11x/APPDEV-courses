---
title: "Performance Code Review for Bootstrap"
module: "Code Review"
difficulty: 2
estimated_time: 20
tags: ["performance", "CSS-optimization", "bundle-size", "rendering"]
prerequisites: ["Bootstrap customization", "build tools basics"]
---

## Overview

Performance code review for Bootstrap projects focuses on CSS bundle size, unused class elimination, render-blocking resources, and runtime efficiency. Bootstrap ships a large CSS file by default, and adding custom styles on top can bloat the final bundle. This guide covers techniques to audit, measure, and optimize Bootstrap's impact on page load and rendering performance.

## Basic Implementation

**CSS Bundle Size Audit**

Measure your compiled CSS size and identify the largest contributors. Use build tools to analyze the bundle.

```bash
# Analyze CSS bundle size
npx purgecss --css dist/css/app.css --content "src/**/*.html" --output dist/css/purged/

# Check compressed size
gzip -c dist/css/app.css | wc -c
```

**Import Only Needed Bootstrap Modules**

Instead of importing the full Bootstrap CSS, import only the components you use.

```scss
// BAD: Imports everything
@import "bootstrap/scss/bootstrap";

// GOOD: Import only what you need
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/mixins";
@import "bootstrap/scss/root";
@import "bootstrap/scss/reboot";
@import "bootstrap/scss/grid";
@import "bootstrap/scss/buttons";
@import "bootstrap/scss/forms";
@import "bootstrap/scss/utilities/api";
```

**JavaScript Import Optimization**

Tree-shake Bootstrap JS by importing individual plugins instead of the full bundle.

```javascript
// BAD: Imports all plugins
import * as bootstrap from 'bootstrap';

// GOOD: Import only needed plugins
import { Modal, Tooltip } from 'bootstrap';

document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(el => {
  new Tooltip(el);
});
```

## Advanced Variations

**PurgeCSS Integration**

Remove unused Bootstrap classes from the production build automatically.

```javascript
// postcss.config.js
module.exports = {
  plugins: [
    require('@fullhuman/postcss-purgecss')({
      content: ['./src/**/*.html', './src/**/*.js'],
      safelist: {
        standard: [/^modal/, /^tooltip/, /^dropdown/],
        deep: [/^carousel/],
        greedy: [/^bs-/]
      },
      defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || []
    })
  ]
};
```

**Critical CSS Extraction**

Inline critical above-the-fold CSS and defer the rest for faster first paint.

```html
<head>
  <style>
    /* Critical Bootstrap styles inlined */
    .container { width: 100%; margin-inline: auto; padding-inline: .75rem; }
    .row { display: flex; flex-wrap: wrap; }
    .navbar { position: relative; display: flex; flex-wrap: wrap; }
  </style>
  <link rel="preload" href="/css/bootstrap.min.css" as="style" onload="this.onload=null;this.rel='stylesheet'">
  <noscript><link rel="stylesheet" href="/css/bootstrap.min.css"></noscript>
</head>
```

**Lazy Loading Bootstrap Components**

Defer loading of non-critical Bootstrap JavaScript plugins until they are needed.

```javascript
// Lazy load modal plugin only when needed
document.querySelector('[data-bs-target="#myModal"]').addEventListener('click', async () => {
  const { Modal } = await import('bootstrap');
  const modal = new Modal(document.getElementById('myModal'));
  modal.show();
});
```

## Best Practices

1. **Import only required Bootstrap SCSS partials** - never import the full bundle in production
2. **Use PurgeCSS or similar tools** to remove unused classes from production builds
3. **Tree-shake Bootstrap JS** by importing individual plugins
4. **Enable gzip/brotli compression** on your server for CSS and JS assets
5. **Minify CSS in production** using cssnano or your build tool's minifier
6. **Use `font-display: swap`** for web fonts to prevent invisible text during loading
7. **Defer non-critical CSS** using `preload` with `onload` pattern
8. **Audit with Lighthouse** - target a performance score above 90
9. **Avoid inline styles that duplicate Bootstrap utilities** - use the classes instead
10. **Use CSS custom properties** for theming to reduce compiled CSS size
11. **Limit custom CSS specificity** - high specificity increases CSS complexity
12. **Monitor cumulative layout shift (CLS)** caused by late-loading styles

## Common Pitfalls

1. **Importing the full Bootstrap CSS** when only grid and buttons are used
2. **Including all Bootstrap JS plugins** when only Modal and Tooltip are needed
3. **Not configuring PurgeCSS safelist** - dynamic classes get stripped in production
4. **Forgetting to compress assets** - uncompressed CSS is 5-10x larger than gzipped
5. **Loading Bootstrap CSS synchronously in `<head>`** - blocks first paint
6. **Using `@import` in CSS** instead of build-tool concatenation - creates waterfall requests
7. **Not versioning asset filenames** - browsers cache old versions after deployments
8. **Duplicating Bootstrap variables** in custom SCSS instead of overriding them
9. **Loading unused icon libraries** alongside Bootstrap Icons
10. **Ignoring bundle analysis** - assuming size is fine without measuring

## Accessibility Considerations

Performance impacts accessibility. Slow-loading pages are harder to use for people with cognitive disabilities. Ensure that deferred CSS does not cause a flash of unstyled content (FOUC) that confuses screen reader users. Verify that lazy-loaded components still function correctly with keyboard navigation and that focus management works even when scripts load asynchronously.

## Responsive Behavior

Optimize responsive assets separately. Serve smaller images on mobile using `srcset` and `<picture>`. Avoid loading desktop-only CSS and JS on mobile devices. Use Bootstrap's responsive display utilities (`d-none d-md-block`) in combination with code splitting to reduce mobile payload. Test performance metrics specifically on mobile network conditions (3G throttling) to ensure acceptable load times.
