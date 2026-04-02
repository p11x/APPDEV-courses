---
title: "SASS Compilation with Bootstrap 5"
module: "Development Workflow"
difficulty: 2
estimated_time: 30 min
prerequisites:
  - NPM setup (05_02_01)
  - Basic Sass/SCSS syntax knowledge
tags:
  - sass
  - scss
  - compilation
  - customization
---

# SASS Compilation with Bootstrap 5

## Overview

Bootstrap 5 is authored in Sass (SCSS syntax), giving developers granular control over which components, utilities, and variables are compiled. Using Sass instead of pre-compiled CSS enables you to override default variables before they cascade, exclude unused components entirely, and extend Bootstrap's design system with custom themes. This module covers installing the Sass compiler, structuring a custom variables file, controlling import order, and running watch mode for continuous compilation during development.

## Basic Implementation

Install the Dart Sass compiler alongside Bootstrap:

```bash
npm install bootstrap @popperjs/core sass --save-dev
```

Create a custom entry SCSS file that imports Bootstrap after your variable overrides:

```json
{
  "scripts": {
    "sass:build": "sass src/scss/main.scss dist/css/main.css",
    "sass:watch": "sass --watch src/scss/main.scss:dist/css/main.css",
    "sass:prod": "sass src/scss/main.scss:dist/css/main.css --style compressed --no-source-map"
  },
  "devDependencies": {
    "sass": "^1.77.0",
    "bootstrap": "^5.3.3"
  }
}
```

In `src/scss/main.scss`, override variables before importing Bootstrap:

```html
<!-- Reference compiled output -->
<link rel="stylesheet" href="dist/css/main.css">
```

## Advanced Variations

Structure your SCSS to override Bootstrap defaults selectively. Create a dedicated variables file that must be imported before Bootstrap's own variables:

```bash
mkdir -p src/scss && touch src/scss/_variables.scss src/scss/main.scss
```

In `_variables.scss`, override only the values you need — Bootstrap uses `!default` flags, so your values take precedence:

```json
{
  "directories": {
    "scss": "src/scss",
    "compiled": "dist/css",
    "bootstrap": "node_modules/bootstrap/scss"
  },
  "watchPaths": [
    "src/scss/**/*.scss"
  ]
}
```

The import order in `main.scss` is critical. Variables must come first, then Bootstrap's functions, mixins, variables, and finally the components:

```html
<!-- Your compiled main.css includes only the components you imported -->
<link rel="stylesheet" href="dist/css/main.css">
```

Use `@use` instead of `@import` for modern Sass (Dart Sass 2.0+) to avoid global namespace pollution. Bootstrap 5.3+ supports both patterns, but `@use` is recommended for new projects.

## Best Practices

1. **Always override variables before importing Bootstrap** — Sass `!default` means the first declaration wins; importing Bootstrap first locks in defaults.
2. **Create a dedicated `_variables.scss` partial** — keep all overrides in one file for easy review and maintenance.
3. **Use `@use` over `@import`** in new projects to leverage Sass module namespaces and avoid duplicate loading.
4. **Import only the Bootstrap components you need** — instead of `@import "bootstrap"`, import individual partials like `@import "bootstrap/scss/buttons"`.
5. **Enable source maps in development** (`--source-map`) for accurate browser DevTools debugging.
6. **Disable source maps in production** (`--no-source-map`) to avoid shipping internal file structure.
7. **Use `sass --watch` during development** to auto-recompile on file save.
8. **Pin your Sass compiler version** — Dart Sass and Node Sass have behavioral differences; use Dart Sass exclusively.
9. **Organize partials with underscore prefix** (`_variables.scss`) to signal they are import-only files.
10. **Test compiled output size** — track CSS file size to ensure selective imports are working.
11. **Version-control your SCSS source, not compiled CSS** — regenerate CSS in CI rather than committing built artifacts.

## Common Pitfalls

1. **Importing Bootstrap before overriding variables** — the `!default` flag means your overrides are ignored if Bootstrap's variables are already defined.
2. **Using Node Sass instead of Dart Sass** — Node Sass is deprecated and lacks features Bootstrap 5.3+ requires, such as the `@use` module system.
3. **Missing the `_` prefix on partials** — Sass treats files without `_` as compilation entry points, not importable partials.
4. **Circular imports** — importing a partial that imports its parent causes infinite loops. Structure your SCSS hierarchy as a tree.
5. **Forgetting to compile after changes** — unlike CSS, SCSS changes require a build step. Always run watch mode or rebuild before testing.
6. **Wrong import paths** — Bootstrap's SCSS entry is at `node_modules/bootstrap/scss/bootstrap`, not `node_modules/bootstrap/scss/_bootstrap`.
7. **Overriding variables that don't exist** — custom variable names that don't match Bootstrap's `$variable-name` convention are silently ignored.

## Accessibility Considerations

Bootstrap's Sass source includes accessibility-related variables such as `$focus-ring-*` for focus indicator styling. When overriding color variables, ensure sufficient contrast ratios are maintained. Custom themes should preserve Bootstrap's focus-visible styles rather than removing them. Compile and test with the `:focus-visible` pseudo-class to confirm keyboard navigation indicators remain visible.

## Responsive Behavior

Bootstrap's responsive breakpoints and grid mixins are defined in SCSS variables (`$grid-breakpoints`). When customizing via Sass, you can add or modify breakpoints by overriding `$grid-breakpoints` before importing Bootstrap. The responsive utilities (`d-md-none`, `col-lg-4`) are generated automatically from this map. Removing unused breakpoints from the map reduces compiled CSS size and prevents unintended responsive behavior at non-standard viewport widths.
