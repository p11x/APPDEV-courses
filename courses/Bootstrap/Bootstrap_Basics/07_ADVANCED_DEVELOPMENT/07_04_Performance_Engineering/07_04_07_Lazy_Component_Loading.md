---
title: "Lazy Component Loading"
difficulty: 3
category: "Advanced Development"
subcategory: "Performance Engineering"
prerequisites:
  - Dynamic Imports
  - Intersection Observer
  - Bootstrap 5 Plugin System
---

## Overview

Lazy component loading uses dynamic imports to load Bootstrap plugins and custom components only when they are needed on the page. Instead of importing all of Bootstrap's JavaScript at page load, components like modals, tooltips, and carousels load on demand when their DOM elements become visible or when the user interacts with them. This reduces the initial JavaScript payload and speeds up Time to Interactive.

The strategy uses Intersection Observer for visibility-based loading, event delegation for interaction-based loading, and route-based code splitting for page-specific components. Each approach serves different use cases: Intersection Observer for below-the-fold components, event delegation for triggered components (modals, dropdowns), and route splitting for entire page modules.

## Basic Implementation

```js
// Lazy Bootstrap plugin loading
// Lazy load Modal only when modal trigger is clicked
document.addEventListener('click', async (e) => {
  const trigger = e.target.closest('[data-bs-toggle="modal"]');
  if (trigger) {
    e.preventDefault();
    const { Modal } = await import('bootstrap/js/dist/modal');
    const target = document.querySelector(trigger.dataset.bsTarget);
    const modal = new Modal(target);
    modal.show();
  }
});

// Lazy load Tooltip on hover
document.addEventListener('mouseenter', async (e) => {
  const trigger = e.target.closest('[data-bs-toggle="tooltip"]');
  if (trigger && !bootstrap.Tooltip.getInstance(trigger)) {
    const { Tooltip } = await import('bootstrap/js/dist/tooltip');
    new Tooltip(trigger).show();
  }
}, true);
```

```js
// Intersection Observer-based lazy loading
function lazyLoadComponent(selector, importFn) {
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(async (entry) => {
      if (entry.isIntersecting) {
        observer.unobserve(entry.target);
        const module = await importFn();
        if (module.default) {
          module.default.getOrCreateInstance(entry.target);
        }
      }
    });
  }, { rootMargin: '200px' });

  document.querySelectorAll(selector).forEach(el => observer.observe(el));
}

// Usage
lazyLoadComponent('[data-lazy="carousel"]', () => import('bootstrap/js/dist/carousel'));
lazyLoadComponent('[data-lazy="collapse"]', () => import('bootstrap/js/dist/collapse'));
```

```js
// Component loader with caching
class LazyBootstrap {
  static _cache = new Map();

  static async load(pluginName) {
    if (this._cache.has(pluginName)) {
      return this._cache.get(pluginName);
    }

    const moduleMap = {
      modal: () => import('bootstrap/js/dist/modal'),
      tooltip: () => import('bootstrap/js/dist/tooltip'),
      popover: () => import('bootstrap/js/dist/popover'),
      dropdown: () => import('bootstrap/js/dist/dropdown'),
      collapse: () => import('bootstrap/js/dist/collapse'),
      carousel: () => import('bootstrap/js/dist/carousel'),
      offcanvas: () => import('bootstrap/js/dist/offcanvas')
    };

    const loader = moduleMap[pluginName];
    if (!loader) throw new Error(`Unknown plugin: ${pluginName}`);

    const module = await loader();
    this._cache.set(pluginName, module);
    return module;
  }

  static async init(selector, pluginName) {
    const module = await this.load(pluginName);
    const PluginClass = module.default || module[pluginName.charAt(0).toUpperCase() + pluginName.slice(1)];

    document.querySelectorAll(selector).forEach(el => {
      PluginClass.getOrCreateInstance(el);
    });
  }
}
```

## Advanced Variations

```js
// Route-based code splitting
// router.js
const routes = {
  '/': () => import('./pages/Home'),
  '/dashboard': () => import('./pages/Dashboard'),
  '/settings': () => import('./pages/Settings'),
  '/products': () => import('./pages/Products')
};

async function navigate(path) {
  const loader = routes[path];
  if (!loader) return;

  const { default: page } = await loader();
  const app = document.getElementById('app');
  app.innerHTML = page.render();

  // Initialize Bootstrap components for this page
  page.init?.(app);
}

// Each page module lazily loads its own Bootstrap components
// pages/Dashboard.js
export default {
  render: () => `<div class="dashboard">
    <button data-bs-toggle="modal" data-bs-target="#alertModal">Configure Alerts</button>
    <div id="alertModal" class="modal fade">...</div>
  </div>`,
  init: async (container) => {
    const { Modal } = await import('bootstrap/js/dist/modal');
    container.querySelectorAll('.modal').forEach(el => new Modal(el));
  }
};
```

## Best Practices

1. **Lazy load below-the-fold components** - Use Intersection Observer for components not in initial viewport.
2. **Cache loaded modules** - Avoid re-importing the same plugin multiple times.
3. **Show loading states** - Display spinners or skeletons while plugins load.
4. **Preload on interaction** - Start loading when user hovers over a trigger, not after click.
5. **Use event delegation** - One listener handles all lazy-loaded triggers.
6. **Bundle split by route** - Each page loads only its required Bootstrap plugins.
7. **Set rootMargin for pre-loading** - `rootMargin: '200px'` loads components before they scroll into view.
8. **Handle loading failures** - Gracefully degrade if a dynamic import fails.
9. **Measure lazy load savings** - Track initial bundle size reduction from lazy loading.
10. **Document lazy components** - Clearly mark which components are lazy-loaded.

## Common Pitfalls

1. **Lazy loading above-the-fold** - Components visible at page load should be in the initial bundle.
2. **No loading indicator** - Users click buttons that do nothing while imports load.
3. **Race conditions** - Multiple rapid clicks trigger multiple imports.
4. **Missing error handling** - Network failure on import leaves components broken.
5. **Over-granular splitting** - Each tiny component as a separate chunk creates too many requests.

## Accessibility Considerations

Lazy-loaded components must announce loading state to screen readers. Use `aria-busy="true"` during loading.

```html
<button data-bs-toggle="modal" data-bs-target="#confirm"
        aria-busy="false" aria-label="Open confirmation dialog">
  Delete
</button>
```

## Responsive Behavior

Consider pre-loading desktop-only components (DataTable) only on wider viewports, and mobile-only components (bottom sheet) only on narrow viewports.

```js
if (window.matchMedia('(min-width: 768px)').matches) {
  LazyBootstrap.init('[data-component="datatable"]', 'datatable');
}
```
