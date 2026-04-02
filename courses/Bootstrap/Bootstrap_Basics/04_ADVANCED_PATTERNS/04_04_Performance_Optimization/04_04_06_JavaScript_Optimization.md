---
title: "JavaScript Optimization"
module: "Performance Optimization"
difficulty: 2
duration: "25 minutes"
prerequisites: ["JavaScript modules", "Bootstrap JS"]
tags: ["javascript", "esm", "tree-shaking", "performance"]
---

# JavaScript Optimization

## Overview

Bootstrap's full JavaScript bundle includes all components (modal, tooltip, carousel, etc.) but most projects use only a few. Importing only needed JS plugins, using ESM modules, and applying async/defer loading strategies can reduce JavaScript payloads by 60-80%, improving Time to Interactive (TTI) and reducing main thread blocking.

## Basic Implementation

Import individual Bootstrap JS plugins:

```js
// Import only the plugins you need
import { Modal } from 'bootstrap/js/dist/modal';
import { Tooltip } from 'bootstrap/js/dist/tooltip';
import { Dropdown } from 'bootstrap/js/dist/dropdown';

// Initialize tooltips
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
const tooltipList = [...tooltipTriggerList].map(
  el => new Tooltip(el)
);

// Initialize dropdowns
const dropdownElementList = document.querySelectorAll('.dropdown-toggle');
const dropdownList = [...dropdownElementList].map(
  el => new Dropdown(el)
);
```

Use async/defer script loading:

```html
<!-- Defer: executes after HTML parsing -->
<script src="/js/bootstrap.bundle.min.js" defer></script>

<!-- Async: executes when downloaded, non-blocking -->
<script src="/js/analytics.js" async></script>

<!-- ESM module with dynamic import -->
<script type="module">
  const { Modal } = await import('/js/bootstrap-modal.min.js');
</script>
```

## Advanced Variations

Create a minimal Bootstrap JS bundle:

```js
// bootstrap-custom.js - Only what you need
export { Modal } from 'bootstrap/js/dist/modal';
export { Collapse } from 'bootstrap/js/dist/collapse';
export { Offcanvas } from 'bootstrap/js/dist/offcanvas';

// Auto-initialize components
document.addEventListener('DOMContentLoaded', () => {
  // Initialize modals
  document.querySelectorAll('[data-bs-toggle="modal"]').forEach(el => {
    el.addEventListener('click', (e) => {
      const target = document.querySelector(el.dataset.bsTarget);
      if (target) new Modal(target).show();
    });
  });
});
```

Dynamic import for lazy-loaded components:

```js
// Lazy load modal when needed
document.querySelectorAll('[data-bs-toggle="modal"]').forEach(trigger => {
  trigger.addEventListener('click', async () => {
    const { Modal } = await import('bootstrap/js/dist/modal');
    const target = document.querySelector(trigger.dataset.bsTarget);
    new Modal(target).show();
  }, { once: true });
});

// Lazy load carousel for below-fold content
const carouselObserver = new IntersectionObserver((entries) => {
  entries.forEach(async (entry) => {
    if (entry.isIntersecting) {
      const { Carousel } = await import('bootstrap/js/dist/carousel');
      new Carousel(entry.target);
      carouselObserver.unobserve(entry.target);
    }
  });
});

document.querySelectorAll('.carousel').forEach(el => {
  carouselObserver.observe(el);
});
```

Configure Webpack for optimal Bootstrap JS bundling:

```js
// webpack.config.js
const TerserPlugin = require('terser-webpack-plugin');

module.exports = {
  optimization: {
    usedExports: true,
    sideEffects: false,
    minimizer: [
      new TerserPlugin({
        terserOptions: {
          compress: {
            drop_console: true
          }
        }
      })
    ]
  },
  experiments: {
    outputModule: true
  },
  output: {
    module: true,
    library: { type: 'module' }
  }
};
```

## Best Practices

1. Import individual plugins, not the full bundle
2. Use ESM imports for tree-shaking support
3. Defer non-critical Bootstrap JS
4. Lazy load components not needed on initial render
5. Use `async` for analytics and tracking scripts
6. Initialize Bootstrap components after DOM is ready
7. Bundle only the Bootstrap JS components you use
8. Use dynamic `import()` for route-specific components
9. Remove console.log statements in production
10. Monitor JavaScript bundle size in CI/CD
11. Consider CSS-only alternatives for simple interactions
12. Use event delegation to reduce event listener count

## Common Pitfalls

1. Importing full `bootstrap.bundle.min.js` when only 2 plugins are used
2. Not tree-shaking due to incorrect import syntax
3. Loading scripts in `<head>` without defer/async
4. Initializing components before DOM is ready
5. Missing Popper.js dependency for tooltips/popovers
6. Not testing lazy-loaded components for flash of unstyled content
7. Dynamic imports causing waterfall requests
8. Forgetting `crossorigin` on CDN script tags

## Accessibility Considerations

- Ensure keyboard navigation works with lazy-loaded components
- Test screen reader announcements after dynamic initialization
- Verify focus management in dynamically imported modals
- Maintain ARIA attribute support in minimal bundles

## Responsive Behavior

- Test touch events on mobile with optimized bundles
- Verify swipe gestures work in lazy-loaded carousels
- Ensure offcanvas components work on all viewports
- Test dropdown positioning after dynamic import
