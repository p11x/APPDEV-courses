---
title: ES Modules with Bootstrap
category: Emerging Technologies
difficulty: 2
time: 25 min
tags: bootstrap5, esm, es-modules, tree-shaking, dynamic-imports
---

## Overview

ES Modules (ESM) provide a native JavaScript module system for importing only the Bootstrap components you need. Combined with bundlers like Vite, Webpack, or Rollup, ESM imports enable tree-shaking that eliminates unused Bootstrap code from production bundles. Dynamic imports further optimize loading by deferring component code until needed.

## Basic Implementation

Bootstrap 5 ships with ESM-compatible source files. Import individual components instead of the entire library.

```js
// Full import (not recommended for production)
import * as bootstrap from 'bootstrap';

// Selective imports - only load what you use
import { Modal, Tooltip, Dropdown } from 'bootstrap';

// Initialize components
const modalEl = document.getElementById('exampleModal');
const modal = new Modal(modalEl, {
  backdrop: true,
  keyboard: true,
  focus: true
});

// Show the modal programmatically
document.getElementById('openModal').addEventListener('click', () => {
  modal.show();
});

// Initialize tooltips on all elements with data-bs-toggle="tooltip"
document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(el => {
  new Tooltip(el);
});
```

```html
<!-- HTML works alongside ESM imports -->
<button id="openModal" class="btn btn-primary" type="button">Open Modal</button>
<button class="btn btn-secondary" data-bs-toggle="tooltip"
        data-bs-placement="top" title="Tooltip text">Hover me</button>

<div class="modal fade" id="exampleModal" tabindex="-1"
     aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">ESM Modal</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"
                aria-label="Close"></button>
      </div>
      <div class="modal-body">
        This modal was initialized via ES Module imports.
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

Dynamic imports load Bootstrap components on demand, reducing initial bundle size for pages that use components conditionally.

```js
// Dynamic import: load Modal only when user triggers it
async function openModal(modalId) {
  const { Modal } = await import('bootstrap');
  const modalEl = document.getElementById(modalId);
  const modal = Modal.getInstance(modalEl) || new Modal(modalEl);
  modal.show();
}

// Lazy-load all toasts on page
async function initToasts() {
  const { Toast } = await import('bootstrap');
  document.querySelectorAll('.toast').forEach(el => {
    new Toast(el, { autohide: true, delay: 5000 }).show();
  });
}

// Conditional loading based on page content
class BootstrapLoader {
  static async loadComponents() {
    const imports = [];
    const selectors = {
      Modal: '[data-bs-toggle="modal"]',
      Dropdown: '[data-bs-toggle="dropdown"]',
      Tooltip: '[data-bs-toggle="tooltip"]',
      Popover: '[data-bs-toggle="popover"]',
      Collapse: '[data-bs-toggle="collapse"]',
      Tab: '[data-bs-toggle="tab"], [data-bs-toggle="pill"]'
    };

    for (const [component, selector] of Object.entries(selectors)) {
      if (document.querySelector(selector)) {
        imports.push(
          import('bootstrap').then(mod => ({ name: component, Class: mod[component] }))
        );
      }
    }

    const loaded = await Promise.all(imports);
    loaded.forEach(({ name, Class }) => {
      document.querySelectorAll(selectors[name]).forEach(el => {
        if (!Class.getInstance(el)) {
          new Class(el);
        }
      });
    });

    return loaded.map(l => l.name);
  }
}

// Usage
document.addEventListener('DOMContentLoaded', () => {
  BootstrapLoader.loadComponents().then(components => {
    console.log('Loaded Bootstrap components:', components);
  });
});
```

```html
<!-- Vite config for Bootstrap tree-shaking -->
<!-- vite.config.js -->
<script type="module">
// vite.config.js - tree-shake Bootstrap effectively
import { defineConfig } from 'vite';

export default defineConfig({
  build: {
    rollupOptions: {
      treeshake: {
        moduleSideEffects: false
      }
    }
  }
});
</script>
```

## Best Practices

1. Import only the Bootstrap components your page actually uses
2. Use dynamic imports for components that appear after user interaction
3. Initialize components after DOM is ready with `DOMContentLoaded`
4. Use `getInstance()` to check if a component is already initialized before creating a new one
5. Bundle Bootstrap with a bundler that supports tree-shaking (Vite, Rollup, Webpack 5)
6. Set `sideEffects: false` in your bundler config for effective tree-shaking
7. Prefer named imports over namespace imports (`import { Modal }` vs `import * as bootstrap`)
8. Use dynamic `import()` for route-based code splitting with Bootstrap components
9. Cache dynamic imports to avoid redundant network requests
10. Test that tree-shaking actually reduces bundle size with `npx vite build --mode analyze`

## Common Pitfalls

1. **Importing everything** - `import * as bootstrap` bundles all components regardless of usage. Use named imports.
2. **Missing bundler config** - Without proper tree-shaking config, unused code persists. Verify with bundle analyzer.
3. **Dynamic import race conditions** - Multiple simultaneous dynamic imports of the same module create duplicate requests. Cache promises.
4. **SSR incompatibility** - Bootstrap's ESM references `window` and `document`. Use dynamic imports in SSR contexts.
5. **Module resolution errors** - Ensure `package.json` `"type": "module"` or `.mjs` extensions are used consistently.

## Accessibility Considerations

ESM imports do not affect Bootstrap's built-in accessibility. However, dynamically loaded components must be initialized with proper ARIA attributes already present in the HTML. Ensure that dynamically imported components receive correct configuration options (`focus: true` for modals, proper `aria-` attributes in markup) and that focus management works correctly after lazy loading.

## Responsive Behavior

ES module loading is viewport-independent. However, consider lazy-loading mobile-only components (like offcanvas navigation) only when the viewport matches. Use `window.matchMedia` with Bootstrap breakpoint queries to conditionally load responsive components, reducing bundle size on viewports that don't need them.
