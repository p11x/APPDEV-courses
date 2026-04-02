---
title: "Vite Integration with Bootstrap 5"
module: "Development Workflow"
difficulty: 2
estimated_time: 25 min
prerequisites:
  - NPM setup (05_02_01)
  - Basic ES module knowledge
tags:
  - vite
  - hmr
  - bundling
  - sass
---

# Vite Integration with Bootstrap 5

## Overview

Vite is a modern build tool that leverages native ES modules and esbuild for extremely fast development server startup and hot module replacement (HMR). Integrating Bootstrap 5 with Vite is straightforward because Vite has built-in support for CSS and Sass imports — no loader configuration required. During development, Vite serves Bootstrap's files on-demand via ESM, and in production it uses Rollup to produce optimized, tree-shaken bundles. This guide covers project setup, SCSS customization, HMR behavior, and Vite-specific configuration for Bootstrap 5.

## Basic Implementation

Scaffold a Vite project and install Bootstrap:

```bash
npm create vite@latest my-bootstrap-app -- --template vanilla
cd my-bootstrap-app
npm install bootstrap @popperjs/core
npm install sass --save-dev
```

Import Bootstrap in your main JavaScript entry file:

```json
{
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview"
  },
  "devDependencies": {
    "vite": "^5.4.0",
    "sass": "^1.77.0"
  },
  "dependencies": {
    "bootstrap": "^5.3.3",
    "@popperjs/core": "^2.11.8"
  }
}
```

Create your HTML entry point with a module script:

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Bootstrap + Vite</title>
  <link rel="stylesheet" href="/src/scss/main.scss">
</head>
<body>
  <div class="container py-5">
    <h1 class="text-primary">Bootstrap + Vite</h1>
    <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#demoModal">Open Modal</button>
  </div>
  <script type="module" src="/src/main.js"></script>
</body>
</html>
```

## Advanced Variations

For SCSS customization, create a partial that overrides Bootstrap variables before importing:

```bash
mkdir -p src/scss && touch src/scss/_variables.scss src/scss/main.scss
```

Configure `vite.config.js` to set global Sass includes so you don't need relative paths in every SCSS file:

```json
{
  "css": {
    "preprocessorOptions": {
      "scss": {
        "additionalData": "@use \"src/scss/variables\" as *;",
        "api": "modern-compiler"
      }
    }
  }
}
```

For multi-page applications, define multiple entry points in `vite.config.js`:

```html
<!-- Each page can import only the Bootstrap components it needs -->
<link rel="stylesheet" href="/src/scss/home.scss">
<script type="module" src="/src/home.js"></script>
```

## Best Practices

1. **Use `npm create vite`** to scaffold — it generates optimized defaults for ES modules and asset handling.
2. **Install `sass` as a devDependency** — Vite detects it automatically and compiles `.scss` imports without configuration.
3. **Use `type="module"` on script tags** — Vite serves unbundled ESM in development; module scripts are required.
4. **Import Bootstrap SCSS directly** — Vite handles `.scss` imports in JS, so you can `import 'bootstrap/scss/bootstrap'` in your entry JS.
5. **Configure `preprocessorOptions.scss.additionalData`** to inject global variables without manual imports in every file.
6. **Use `vite preview`** to test the production build locally before deployment.
7. **Enable `build.cssCodeSplit: true`** (default) to split CSS into per-route chunks for faster initial loads.
8. **Keep `vite.config.js` minimal** — Vite's defaults are sensible; only configure what you need to change.
9. **Use the `@` alias** (`resolve.alias: { '@': path.resolve('src') }`) for clean import paths.
10. **Leverage HMR** — Vite's HMR for SCSS is near-instant; edit variables and see results without page reload.
11. **Pin Vite and plugin versions** — major Vite releases can change behavior; lock versions in production.

## Common Pitfalls

1. **Missing `sass` dependency** — Vite does not bundle a Sass compiler; forgetting `npm install sass` causes `Preprocessor dependency "sass" not found` errors.
2. **Using `type="text/javascript"` instead of `type="module"`** — Vite's dev server requires ES module scripts; traditional scripts fail.
3. **Importing compiled CSS from `node_modules`** — while `import 'bootstrap/dist/css/bootstrap.min.css'` works, importing SCSS source gives you customization control.
4. **HMR not working on SCSS** — if HMR fails for styles, check that you are importing SCSS files (not compiled CSS) and that the `sass` package is installed.
5. **CORS errors with local fonts** — Bootstrap's icon font references may trigger CORS in dev; configure `server.cors: true` if needed.
6. **`additionalData` injection order** — variables injected via `additionalData` are prepended; if Bootstrap is imported first in the file, overrides still work, but verify the compiled output.
7. **Conflicting with `create-react-app`** — Vite and CRA have different conventions; do not mix their configurations.

## Accessibility Considerations

Vite's build process preserves all Bootstrap accessibility classes and ARIA attributes by default. Unlike Webpack, Vite does not require explicit whitelist configuration for utility classes. When using Vite's CSS code splitting, verify that `.visually-hidden` and other a11y utilities are included in the critical CSS chunk so they load on the initial render. Import Bootstrap's bundle JS to ensure modal focus trapping and dropdown keyboard navigation work in the production build.

## Responsive Behavior

Vite does not modify Bootstrap's responsive behavior. All responsive utilities, grid breakpoints, and container queries function identically in Vite-bundled output. The only consideration is that Vite's CSS code splitting may defer loading of responsive utility CSS to later chunks — ensure critical above-the-fold responsive styles are included in the main CSS bundle by using them in your initial HTML or configuring `build.cssTarget`.
