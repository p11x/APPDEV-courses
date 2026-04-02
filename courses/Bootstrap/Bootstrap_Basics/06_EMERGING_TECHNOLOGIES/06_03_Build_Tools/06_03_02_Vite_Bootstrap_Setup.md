---
title: "Vite Bootstrap Setup"
topic: "Build Tools"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Node.js basics", "npm/yarn fundamentals"]
tags: ["vite", "bootstrap", "scss", "hmr", "build-tools"]
---

## Overview

Vite is a modern build tool that leverages native ES modules in development for near-instant server start and lightning-fast Hot Module Replacement (HMR). Integrating Bootstrap 5 with Vite is straightforward because Vite natively supports CSS, SCSS, and ES module imports without complex loader configuration. In development, Vite serves Bootstrap's source files on-demand via ESM, while the production build uses Rollup under the hood for optimized, tree-shaken output.

The key advantage over Webpack is zero-configuration SCSS processing, native PostCSS support, and significantly faster development rebuilds. Vite automatically detects `.scss` files, compiles them with Dart Sass, and injects them via HMR, making the developer experience with Bootstrap smooth and productive.

## Basic Implementation

Initialize a Vite project and install Bootstrap:

```bash
npm create vite@latest my-bootstrap-app -- --template vanilla
cd my-bootstrap-app
npm install bootstrap@5 @popperjs/core
npm install --save-dev sass
```

Create the main entry point `main.js` that imports Bootstrap:

```js
// Import Bootstrap SCSS
import './scss/custom.scss';

// Import Bootstrap JS components
import * as bootstrap from 'bootstrap';

// Example: initialize all tooltips
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
const tooltipList = [...tooltipTriggerList].map(
  el => new bootstrap.Tooltip(el)
);
```

Create `scss/custom.scss` with Bootstrap variable overrides:

```scss
// Override Bootstrap defaults
$primary: #6366f1;
$enable-rounded: true;
$enable-shadows: false;

// Import Bootstrap
@import 'bootstrap/scss/bootstrap';
```

Create `index.html` at the project root (Vite uses root-level HTML):

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Vite Bootstrap App</title>
</head>
<body>
  <div class="container py-5">
    <h1 class="text-primary">Vite + Bootstrap 5</h1>
    <button class="btn btn-primary" data-bs-toggle="tooltip" title="Hello!">
      Hover me
    </button>
  </div>
  <script type="module" src="/main.js"></script>
</body>
</html>
```

Run the dev server:

```bash
npm run dev
```

## Advanced Variations

### Custom Vite Configuration

Create `vite.config.js` to customize the build and dev server behavior:

```js
import { defineConfig } from 'vite';
import { resolve } from 'path';

export default defineConfig(({ mode }) => ({
  root: '.',
  base: '/',
  resolve: {
    alias: {
      '@scss': resolve(__dirname, 'src/scss'),
      '@js': resolve(__dirname, 'src/js'),
      '@assets': resolve(__dirname, 'src/assets'),
    },
  },
  css: {
    devSourcemap: true,
    preprocessorOptions: {
      scss: {
        additionalData: `
          $enable-deprecation-messages: false;
        `,
        silenceDeprecations: ['import', 'global-builtin'],
      },
    },
  },
  server: {
    port: 3000,
    open: true,
    host: true,
  },
  build: {
    outDir: 'dist',
    sourcemap: mode !== 'production',
    cssCodeSplit: true,
    rollupOptions: {
      output: {
        manualChunks: {
          bootstrap: ['bootstrap'],
          popper: ['@popperjs/core'],
        },
        assetFileNames: (assetInfo) => {
          const extType = assetInfo.names?.[0]?.split('.').pop();
          if (/css/i.test(extType)) return 'assets/css/[name]-[hash][extname]';
          if (/png|jpe?g|svg|gif|tiff|bmp|ico|webp/i.test(extType)) {
            return 'assets/images/[name]-[hash][extname]';
          }
          if (/woff2?|eot|ttf|otf/i.test(extType)) {
            return 'assets/fonts/[name]-[hash][extname]';
          }
          return 'assets/[name]-[hash][extname]';
        },
        chunkFileNames: 'assets/js/[name]-[hash].js',
        entryFileNames: 'assets/js/[name]-[hash].js',
      },
    },
  },
}));
```

### Selective Bootstrap JS Imports

Import only the Bootstrap modules you need for smaller production bundles:

```js
import './scss/custom.scss';

// Selective imports — each produces a separate chunk
import Collapse from 'bootstrap/js/dist/collapse';
import Dropdown from 'bootstrap/js/dist/dropdown';
import Modal from 'bootstrap/js/dist/modal';
import Offcanvas from 'bootstrap/js/dist/offcanvas';
import Toast from 'bootstrap/js/dist/toast';

document.addEventListener('DOMContentLoaded', () => {
  // Initialize modals
  const modalEl = document.getElementById('exampleModal');
  if (modalEl) {
    modalEl.addEventListener('show.bs.modal', (event) => {
      console.log('Modal opening');
    });
  }

  // Initialize toasts
  const toastElList = document.querySelectorAll('.toast');
  toastElList.forEach(el => new Toast(el).show());
});
```

### Multi-Page Vite Setup

```js
// vite.config.js — multi-page application
import { defineConfig } from 'vite';
import { resolve } from 'path';

export default defineConfig({
  build: {
    rollupOptions: {
      input: {
        main: resolve(__dirname, 'index.html'),
        dashboard: resolve(__dirname, 'dashboard.html'),
        settings: resolve(__dirname, 'settings.html'),
      },
    },
  },
});
```

## Best Practices

1. **Place `index.html` at the project root**, not inside `src/`. Vite resolves entry points from the root HTML file.
2. **Use `type="module"` in script tags** — Vite serves files as native ES modules in development.
3. **Install `sass` separately** — Vite does not bundle a Sass compiler; you must install it explicitly.
4. **Use SCSS partials** with `@use` instead of `@import` for better namespace management and faster compilation.
5. **Configure `manualChunks`** to separate Bootstrap's JavaScript into a vendor chunk for long-term caching.
6. **Enable `css.devSourcemap`** to debug SCSS source maps during development.
7. **Set `build.cssCodeSplit`** to `true` so CSS is split per route/chunk, reducing initial page weight.
8. **Use Vite aliases** (`resolve.alias`) to avoid brittle relative import paths like `../../../scss/custom.scss`.
9. **Keep Bootstrap variables in a dedicated `_variables.scss` partial** imported before Bootstrap's main SCSS entry.
10. **Run `npm run build` with `--debug` flag** to inspect Rollup chunk analysis when optimizing bundle size.
11. **Use `additionalData`** in `preprocessorOptions` to inject global SCSS mixins/variables without manual imports.

## Common Pitfalls

1. **Placing `index.html` inside `src/`** causes Vite to fail finding the entry point in default configuration.
2. **Forgetting to install `sass`** results in `"Preprocessor dependency sass not found"` errors on SCSS files.
3. **Using CommonJS `require()`** in client code — Vite expects ES module syntax (`import/export`).
4. **Missing `@popperjs/core`** causes Bootstrap's tooltip, popover, and dropdown JavaScript to throw runtime errors.
5. **Not setting `silenceDeprecations`** for Sass produces excessive deprecation warnings from Bootstrap's internal stylesheets.
6. **Configuring PostCSS in `postcss.config.js` instead of `vite.config.js`** can cause plugins to be applied inconsistently between dev and build modes.
7. **HMR not working on SCSS changes** usually indicates a misconfigured `preprocessorOptions` or circular `@import` dependency.

## Accessibility Considerations

Vite's fast HMR allows rapid iteration on accessibility features. When testing Bootstrap components, use the dev server to quickly toggle `aria-*` attributes and verify screen reader behavior. Ensure that Vite's production build does not minify away ARIA attributes — the default HTML minifier preserves them, but custom `build.rollupOptions` plugins should not strip `aria-*` or `role` attributes. Use Bootstrap's built-in `visually-hidden` class for skip links and landmark labels. Vite's source maps in development help trace accessibility issues back to the original SCSS partials where responsive utility classes are defined.

## Responsive Behavior

Bootstrap's responsive grid compiles directly through Vite's SCSS pipeline. The `preprocessorOptions.scss` configuration in `vite.config.js` controls how Sass processes Bootstrap's breakpoint mixins. All responsive utility classes (`col-sm-6`, `d-md-flex`, `fs-lg-3`) are generated during compilation. In production, Vite's Rollup bundler preserves all media queries in the output CSS. The `cssCodeSplit` option ensures that only the CSS relevant to loaded chunks is delivered, though Bootstrap's core responsive utilities are included in the main stylesheet. Use Vite's `build.cssTarget` option set to `es2015` or higher to ensure modern CSS features used by Bootstrap are not unnecessarily downleveled.