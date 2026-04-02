---
title: "Micro CSS Framework"
difficulty: 3
category: "Advanced Development"
subcategory: "Custom Frameworks"
prerequisites:
  - Bootstrap 5 SCSS Architecture
  - CSS Custom Properties
  - Build Tool Configuration
---

## Overview

Extracting a minimal subset of Bootstrap 5 to create a micro CSS framework involves identifying the most essential utilities, components, and layout primitives, then packaging them into a lightweight bundle. This approach is ideal for projects requiring sub-10KB CSS payloads, embedded applications, or environments where the full Bootstrap footprint is unnecessary.

A micro-framework strategy starts with Bootstrap's modular SCSS architecture. Bootstrap's source is organized into independent partials that can be selectively imported. By auditing your project's actual component usage, you can create a custom subset that includes only the grid system, essential utilities, typography, and the few components your application needs.

The key trade-off is flexibility versus weight. A micro-framework sacrifices Bootstrap's comprehensive component library for significant performance gains. The resulting bundle typically achieves 60-80% size reduction compared to a full Bootstrap build while retaining the most impactful features like the grid, spacing utilities, and responsive breakpoints.

## Basic Implementation

The micro-framework starts by selectively importing only the Bootstrap SCSS partials your project requires.

```scss
// micro-bootstrap.scss - Minimal Bootstrap subset

// 1. Core functions and variables (required)
@import 'bootstrap/scss/functions';
@import 'bootstrap/scss/variables';
@import 'bootstrap/scss/variables-dark';
@import 'bootstrap/scss/maps';
@import 'bootstrap/scss/mixins';
@import 'bootstrap/scss/root';

// 2. Utilities (select only what you need)
@import 'bootstrap/scss/utilities';
@import 'bootstrap/scss/utilities/api';

// Override utility generation to include only essentials
$utilities: () !default;
$utilities: map-merge(
  (
    'display': (
      property: display,
      class: d,
      values: none inline flex grid block
    ),
    'flex': (
      property: flex,
      class: flex,
      values: wrap wrap-reverse nowrap
    ),
    'justify-content': (
      property: justify-content,
      class: justify,
      values: start end center between around
    ),
    'align-items': (
      property: align-items,
      class: align,
      values: start end center baseline stretch
    ),
    'gap': (
      property: gap,
      class: gap,
      values: $spacer
    )
  ),
  $utilities
);
```

```html
<!-- micro-bootstrap.html -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link rel="stylesheet" href="dist/micro-bootstrap.min.css">
  <title>Micro Bootstrap App</title>
</head>
<body>
  <div class="container">
    <div class="row">
      <div class="col-12 col-md-6">
        <h1 class="fs-3 fw-bold">Micro Framework</h1>
        <p class="text-muted">Lightweight Bootstrap subset</p>
      </div>
      <div class="col-12 col-md-6 d-flex align-items-center">
        <button class="btn btn-primary btn-sm">Action</button>
      </div>
    </div>
  </div>
</body>
</html>
```

```js
// build-config.js - PostCSS config for micro framework optimization
const purgeCSS = require('@fullhuman/postcss-purgecss');
const cssnano = require('cssnano');

module.exports = {
  plugins: [
    require('autoprefixer'),
    purgeCSS({
      content: ['./src/**/*.html', './src/**/*.js'],
      safelist: {
        standard: [/^show/, /^modal/, /^collapse/],
        greedy: [/^tooltip/]
      },
      defaultExtractor: (content) => content.match(/[\w-/:]+(?<!:)/g) || []
    }),
    cssnano({
      preset: ['default', {
        discardComments: { removeAll: true },
        normalizeWhitespace: true
      }]
    })
  ]
};
```

## Advanced Variations

Advanced micro-frameworks use CSS custom properties for theming and build-time optimization to achieve maximum compression.

```scss
// Advanced: CSS Custom Properties for micro theming
:root {
  --mb-primary: #{$primary};
  --mb-secondary: #{$secondary};
  --mb-success: #{$success};
  --mb-danger: #{$danger};
  --mb-body-bg: #{$body-bg};
  --mb-body-color: #{$body-color};
  --mb-border-radius: #{$border-radius};
  --mb-spacer: #{$spacer};
  --mb-font-family: #{$font-family-base};
  --mb-line-height: #{$line-height-base};
}

// Micro grid with CSS custom properties
.micro-grid {
  --grid-columns: 12;
  --grid-gap: var(--mb-spacer);
  display: grid;
  grid-template-columns: repeat(var(--grid-columns), 1fr);
  gap: var(--grid-gap);

  @each $breakpoint, $width in $grid-breakpoints {
    @include media-breakpoint-up($breakpoint) {
      @for $i from 1 through 12 {
        > .col-#{$breakpoint}-#{$i} {
          grid-column: span $i;
        }
      }
    }
  }
}

// Essential component: Micro card
.micro-card {
  background: var(--mb-body-bg);
  border: 1px solid var(--bs-border-color);
  border-radius: var(--mb-border-radius);
  padding: var(--mb-spacer);

  &__title {
    font-weight: $font-weight-bold;
    margin-bottom: calc(var(--mb-spacer) * 0.5);
  }

  &__body {
    color: var(--mb-body-color);
    line-height: var(--mb-line-height);
  }
}
```

```js
// build-micro.js - Build script for micro framework
const sass = require('sass');
const fs = require('fs');
const path = require('path');

function buildMicroFramework(options = {}) {
  const {
    includeGrid = true,
    includeUtilities = ['display', 'flex', 'spacing', 'color'],
    includeComponents = ['buttons', 'forms'],
    outputDir = './dist',
    minify = true
  } = options;

  const imports = [];
  imports.push("@import 'bootstrap/scss/functions';");
  imports.push("@import 'bootstrap/scss/variables';");
  imports.push("@import 'bootstrap/scss/mixins';");

  if (includeGrid) {
    imports.push("@import 'bootstrap/scss/containers';");
    imports.push("@import 'bootstrap/scss/grid';");
  }

  includeComponents.forEach(comp => {
    imports.push(`@import 'bootstrap/scss/${comp}';`);
  });

  const scssContent = imports.join('\n');
  const result = sass.compileString(scssContent, {
    loadPaths: [path.resolve('node_modules')]
  });

  const sizeKB = Buffer.byteLength(result.css, 'utf8') / 1024;
  console.log(`Micro framework: ${sizeKB.toFixed(2)} KB`);

  fs.writeFileSync(path.join(outputDir, 'micro-bootstrap.css'), result.css);
  return { css: result.css, size: sizeKB };
}

buildMicroFramework({
  includeGrid: true,
  includeComponents: ['buttons'],
  outputDir: './dist'
});
```

## Best Practices

1. **Audit before cutting** - Use tools like PurgeCSS or Chrome's Coverage tab to identify unused CSS before deciding what to exclude.
2. **Preserve the grid system** - The grid is Bootstrap's most valuable feature; always include it unless you have an alternative layout system.
3. **Maintain breakpoint consistency** - Keep Bootstrap's standard breakpoints (sm, md, lg, xl, xxl) even in a micro framework for team familiarity.
4. **Document included features** - Clearly list which Bootstrap features are available in your micro framework so developers know what's missing.
5. **Use CSS custom properties for theming** - CSS variables allow runtime theming without recompiling SCSS, reducing rebuild overhead.
6. **Automate size tracking** - Include bundle size checks in CI to prevent the micro framework from growing beyond its target.
7. **Keep core mixins** - Bootstrap's `media-breakpoint-up`, `make-container`, and other mixins are valuable for consistent patterns.
8. **Version alongside Bootstrap** - Track which Bootstrap version your micro framework extracts from and update accordingly.
9. **Provide extension points** - Allow teams to add specific Bootstrap modules to the micro framework via configuration files.
10. **Test in target environments** - Validate the micro framework works in the actual browsers and devices your project supports.
11. **Include print styles selectively** - If your app needs print support, include Bootstrap's print utilities as an optional module.
12. **Minify and compress** - Always apply minification and Brotli/gzip compression for maximum delivery performance.

## Common Pitfalls

1. **Removing too much** - Cutting utilities like spacing or display leads to inline styles or custom CSS that defeats the purpose of a framework.
2. **Breaking JavaScript dependencies** - Some Bootstrap JS plugins rely on specific CSS classes; removing them causes runtime errors without console warnings.
3. **Ignoring dark mode** - If you exclude `variables-dark` and color mode CSS, your micro framework won't support Bootstrap 5.3's dark mode.
4. **Hardcoding values** - Replacing Bootstrap variables with raw values makes future customization and theming impossible.
5. **Skipping autoprefixer** - A micro framework still needs vendor prefixes; skipping autoprefixer creates cross-browser issues.
6. **Not testing all breakpoints** - Excluding responsive utilities causes layout failures at unexpected viewport sizes.

## Accessibility Considerations

Even a micro framework must maintain Bootstrap's accessibility foundations. Ensure form controls retain proper label associations, focus states remain visible, and color contrast meets WCAG AA standards. When extracting components, verify that ARIA attributes and semantic HTML patterns are preserved.

```scss
// Include accessible focus styles in micro framework
@import 'bootstrap/scss/reboot';

// Ensure focus-visible is supported
.micro-focus-ring {
  &:focus-visible {
    outline: 2px solid var(--mb-primary);
    outline-offset: 2px;
  }
}

// High contrast mode support
@media (forced-colors: active) {
  .micro-card {
    border: 1px solid CanvasText;
  }

  .btn-primary {
    border: 2px solid ButtonText;
  }
}
```

Never strip the `reboot` module from a micro framework. It provides critical baseline styles including proper focus indicators, reduced motion preferences, and semantic element normalization that are essential for accessibility.

## Responsive Behavior

A micro framework should retain Bootstrap's responsive breakpoint system and provide responsive utility generation. Use the grid system's responsive column classes and selectively include responsive utilities based on project needs.

```scss
// Responsive utilities for micro framework
@each $breakpoint, $min-width in $grid-breakpoints {
  @include media-breakpoint-up($breakpoint) {
    $infix: breakpoint-infix($breakpoint, $grid-breakpoints);

    .d#{$infix}-none { display: none !important; }
    .d#{$infix}-flex { display: flex !important; }
    .d#{$infix}-block { display: block !important; }
    .d#{$infix}-grid { display: grid !important; }

    @for $i from 0 through 5 {
      .gap#{$infix}-#{$i} { gap: calc(#{$spacer} * #{$i}) !important; }
    }
  }
}
```

Define responsive behavior at the component level rather than globally. Components like cards should specify how they reflow at different viewport sizes, with grid column spans adjusting automatically through Bootstrap's responsive grid classes.
