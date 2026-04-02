---
title: Standalone Grid CSS
category: Layout System
difficulty: 3
time: 30 min
tags: bootstrap5, grid, standalone, custom-build, sass, tree-shaking
---

## Overview

Bootstrap 5's grid system can be used independently of the full framework. By importing only the grid Sass files, you get the container, row, and column classes without buttons, cards, modals, or any other component CSS. This standalone approach reduces bundle size, avoids style conflicts with existing codebases, and lets you layer Bootstrap's proven 12-column flexbox grid under a custom design system. Whether you need a minimal grid for a legacy project or a foundation for a fully custom UI, extracting the grid-only build gives you production-grade layout infrastructure at a fraction of the full framework's weight.

## Basic Implementation

The simplest way to use the standalone grid is importing Bootstrap's grid-specific Sass partials instead of the entire `bootstrap.scss`.

```scss
// Import only the grid from Bootstrap's Sass source
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/mixins";
@import "bootstrap/scss/containers";
@import "bootstrap/scss/grid";
```

If you only need the grid CSS file for a static HTML project, Bootstrap ships a pre-built `bootstrap-grid.min.css` that includes containers, rows, columns, and all responsive utilities.

```html
<!-- Link only the grid CSS from CDN -->
<link
  href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap-grid.min.css"
  rel="stylesheet"
>
```

You can further reduce the import chain by cherry-picking only the variables and mixins required by the grid.

```scss
// Minimal Sass imports for grid-only
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/maps";
@import "bootstrap/scss/mixins";
@import "bootstrap/scss/utilities/api";
@import "bootstrap/scss/containers";
@import "bootstrap/scss/grid";
@import "bootstrap/scss/utilities";
```

## Advanced Variations

Override Bootstrap's default grid variables before importing the grid partials to customize column count, breakpoints, and gutter width.

```scss
// Custom grid configuration
$grid-columns: 16;
$grid-gutter-width: 24px;
$grid-breakpoints: (
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 1024px,
  xl: 1280px,
  xxl: 1536px,
);
$container-max-widths: (
  sm: 640px,
  md: 840px,
  lg: 1080px,
  xl: 1280px,
  xxl: 1440px,
);

@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/mixins";
@import "bootstrap/scss/containers";
@import "bootstrap/scss/grid";
```

You can generate a grid-only CSS output using Bootstrap's Sass source and a build tool like Vite, webpack, or the Sass CLI.

```bash
# Compile standalone grid with Sass CLI
sass --no-source-map \
  --style compressed \
  grid-only.scss:dist/grid.min.css
```

For projects using CSS modules or scoped styles, namespace the grid by wrapping imports in a parent selector.

```scss
// Scoped grid under a parent class
.grid-scope {
  @import "bootstrap/scss/functions";
  @import "bootstrap/scss/variables";
  @import "bootstrap/scss/mixins";
  @import "bootstrap/scss/containers";
  @import "bootstrap/scss/grid";
}
```

Tree-shaking with bundlers can further eliminate unused grid utilities. Import only the specific grid mixins you need instead of the full grid partial.

```scss
// Import only required mixins
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/mixins";

.custom-grid {
  @include make-row();

  > .custom-col {
    @include make-col-ready();
  }
}

@for $i from 1 through 12 {
  .custom-col-#{$i} {
    @include make-col($i, 12);
  }
}
```

## Best Practices

1. Always import `_functions.scss`, `_variables.scss`, and `_mixins.scss` before `_grid.scss` — the grid depends on these files.
2. Use `bootstrap-grid.min.css` from the CDN for quick prototyping without a Sass build pipeline.
3. Override `$grid-columns`, `$grid-breakpoints`, and `$grid-gutter-width` before importing Bootstrap partials to apply custom values.
4. Exclude `_reboot.scss` and `_root.scss` when building a standalone grid to avoid resetting styles on existing page elements.
5. Test the standalone grid in your project's full CSS context to catch specificity conflicts with existing styles.
6. Use `--no-source-map` in production builds to avoid shipping Sass source maps.
7. Document which Bootstrap version's grid source you are using, since mixin signatures can change between major versions.
8. Pair the standalone grid with your own utility classes rather than importing Bootstrap's full utility system.
9. Audit the compiled CSS output to confirm only grid-related rules are present — no component styles should leak in.
10. Use the standalone grid as a layout layer, keeping all visual styling (colors, typography, spacing beyond gutters) in separate stylesheets.

## Common Pitfalls

1. **Import order errors**: Importing `_grid.scss` before `_functions.scss` causes Sass compilation failures because the grid references undefined functions.
2. **Missing variables**: Customizing `$grid-columns` after importing `_variables.scss` has no effect — override variables before the import.
3. **Full framework import by accident**: Importing `bootstrap.scss` instead of individual partials defeats the purpose of a standalone grid build.
4. **CDN grid file missing utilities**: `bootstrap-grid.min.css` does not include display, flex, or spacing utilities — you need to provide your own or import additional Bootstrap partials.
5. **Version mismatch**: Using grid Sass source from Bootstrap 5.2 with variables from 5.3 can produce unexpected CSS due to changed variable names or default values.
6. **Scoping side effects**: Wrapping grid imports in a parent selector increases specificity, which can make overriding grid styles harder.
7. **Forgetting container classes**: The standalone grid import does not include `container` unless `_containers.scss` is also imported.

## Accessibility Considerations

The standalone grid provides the same semantic HTML structure as the full framework — `container`, `row`, and `col-*` classes do not add ARIA attributes or alter accessibility behavior. Ensure that semantic landmark elements (`<header>`, `<main>`, `<nav>`, `<footer>`) are used inside grid containers to maintain page structure for assistive technology. The standalone grid does not include Bootstrap's visually-hidden utility, so create your own `.sr-only` class if needed for screen-reader-only content.

## Responsive Behavior

The standalone grid includes all responsive column classes (`col-{breakpoint}-{n}`) and responsive container behavior by default. Breakpoints are defined by the `$grid-breakpoints` map, which can be overridden before import to match your project's design system. The `bootstrap-grid.min.css` file includes every breakpoint from Bootstrap's defaults (sm, md, lg, xl, xxl). When customizing breakpoints, ensure that `make-container` and the responsive column loops reference the same map to keep container widths and column calculations synchronized.
