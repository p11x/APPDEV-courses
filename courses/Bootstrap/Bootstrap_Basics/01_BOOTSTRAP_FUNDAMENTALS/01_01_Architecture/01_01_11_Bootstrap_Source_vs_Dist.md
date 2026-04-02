---
title: "Bootstrap Source vs Dist"
lesson: "01_01_11"
difficulty: "2"
topics: ["scss", "compiled-css", "source", "dist", "build-process"]
estimated_time: "30 minutes"
---

# Bootstrap Source vs Dist

## Overview

Bootstrap ships with two ways to use its styles: precompiled CSS from the `dist/` directory and raw SCSS source files from the `scss/` directory. The compiled CSS is ready to use immediately with no build step, while the source SCSS requires a Sass compiler but gives you full control over variables, mixins, and which components are included. Choosing between them depends on your customization needs, build toolchain, and development workflow.

The `dist/` folder contains production-ready CSS and JS files optimized for delivery. The `scss/` folder contains modular source files that enable selective inclusion and deep customization through variable overrides before compilation.

## Basic Implementation

### Using Compiled CSS (dist/)

```html
<!-- No build step required -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
```

### Using Source SCSS

```scss
// custom.scss
// Override variables BEFORE importing Bootstrap
$primary: #6f42c1;
$border-radius: 0.5rem;

// Import Bootstrap source
@import "node_modules/bootstrap/scss/bootstrap";
```

Then compile:
```bash
sass custom.scss css/custom.css --style compressed
```

### Importing the Compiled Entry Point

```scss
// This imports the same CSS as bootstrap.min.css
@import "node_modules/bootstrap/scss/bootstrap";
```

## Advanced Variations

### Selective Component Imports from Source

```scss
// Only import what you need
@import "node_modules/bootstrap/scss/functions";
@import "node_modules/bootstrap/scss/variables";
@import "node_modules/bootstrap/scss/variables-dark";
@import "node_modules/bootstrap/scss/maps";
@import "node_modules/bootstrap/scss/mixins";
@import "node_modules/bootstrap/scss/root";
@import "node_modules/bootstrap/scss/reboot";
@import "node_modules/bootstrap/scss/containers";
@import "node_modules/bootstrap/scss/grid";
@import "node_modules/bootstrap/scss/buttons";
// Skip modals, carousels, etc.
```

### Custom Build with Feature Flags

```scss
// Disable features you don't need
$enable-rounded: false;
$enable-shadows: true;
$enable-gradients: true;
$enable-grid-classes: true;
$enable-cssgrid: false;

@import "node_modules/bootstrap/scss/bootstrap";
```

### Development vs Production Workflow

```scss
// _variables-custom.scss (development)
$primary: #0d6efd;
$font-size-base: 1rem;
$spacer: 1rem;

// styles.scss (development)
@import "variables-custom";
@import "node_modules/bootstrap/scss/bootstrap";

// Production compile command
sass styles.scss:dist/styles.css --no-source-map --style compressed
```

### Source Map Configuration

```bash
# Development - with source maps for debugging
sass scss/app.scss css/app.css --source-map

# Production - no source maps, minified
sass scss/app.scss css/app.css --no-source-map --style compressed
```

## Best Practices

1. **Use `dist/` CSS for prototyping and simple projects** - No build step, instant results.
2. **Use `scss/` source for production projects requiring customization** - Full variable control.
3. **Override Bootstrap variables BEFORE importing the SCSS** - Variables must be defined before they are read.
4. **Keep a `_variables-custom.scss` partial** - Separates your overrides from Bootstrap's defaults.
5. **Use `--style compressed` for production** - Smaller file size, no whitespace.
6. **Enable source maps in development only** - Helps debug SCSS line numbers.
7. **Lock Bootstrap's SCSS version in package.json** - SCSS variable names can change between minor versions.
8. **Use `sass` (Dart Sass) not `node-sass`** - Bootstrap 5 requires Dart Sass for `@use`/`@forward` compatibility.
9. **Monitor compiled CSS file size** - Selective imports can reduce output by 60%+.
10. **Test both minified and expanded output** - Ensure no broken selectors in production build.
11. **Version control your compiled CSS** - Some teams prefer committing compiled output for review.
12. **Use watch mode during development** - `sass --watch scss:css` auto-recompiles on save.

## Common Pitfalls

1. **Importing Bootstrap SCSS without `_functions.scss` first** - All variables depend on Bootstrap's built-in functions.
2. **Using `node-sass` instead of Dart Sass** - Bootstrap 5 uses features only available in Dart Sass, causing compilation errors.
3. **Overriding variables after importing Bootstrap** - SCSS variable assignments are first-write-wins; later overrides have no effect.
4. **Forgetting `--source-map` in development** - Browser DevTools show compiled CSS line numbers instead of SCSS source.
5. **Including both compiled CSS and custom SCSS output** - Duplicates all Bootstrap styles, doubling bundle size.
6. **Editing Bootstrap's source files directly in `node_modules/`** - Changes are lost on every `npm install`.

## Accessibility Considerations

Both the compiled `dist/` CSS and source `scss/` produce identical accessibility-related styles. Variables like `$focus-ring-*` and `$form-check-*` control focus indicators and form element sizing. When customizing via source, ensure you do not reduce focus ring visibility or set contrast ratios below WCAG minimums. Always test that disabled and forced-colors mode styles remain functional after SCSS customization.

## Responsive Behavior

The compiled `dist/` CSS includes all breakpoints and responsive utilities by default. When using source SCSS, you can modify `$grid-breakpoints` to add or remove breakpoints, and toggle `$enable-grid-classes` to exclude grid CSS entirely. However, removing breakpoints from the SCSS source means all responsive classes at those breakpoints become undefined, which can break templates that reference them.
