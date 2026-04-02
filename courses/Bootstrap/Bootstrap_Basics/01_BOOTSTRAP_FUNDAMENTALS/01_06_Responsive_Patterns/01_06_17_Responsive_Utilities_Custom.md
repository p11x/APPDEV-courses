---
title: "Responsive Utilities Custom"
lesson: "01_06_17"
difficulty: "3"
topics: ["custom-utilities", "extending-breakpoints", "scss-mixins", "utility-api"]
estimated_time: "35 minutes"
---

# Responsive Utilities Custom

## Overview

Bootstrap's utility API lets you generate custom responsive utility classes from SCSS maps and mixins. By defining a utility in the `$utilities` map or using Bootstrap's `make-utility()` mixin, you can create classes like `.custom-prop-sm-value` that follow Bootstrap's breakpoint-prefix convention. You can also extend existing breakpoints by adding custom values to utility maps or creating entirely new responsive utilities not provided by the framework. This advanced technique eliminates one-off CSS and keeps your codebase consistent.

## Basic Implementation

### Adding a Custom Utility via SCSS Map

```scss
@import "node_modules/bootstrap/scss/functions";
@import "node_modules/bootstrap/scss/variables";
@import "node_modules/bootstrap/scss/mixins";
@import "node_modules/bootstrap/scss/utilities";
@import "node_modules/bootstrap/scss/utilities/api";

// Add custom utility AFTER Bootstrap's utilities API
$utilities: (
  "opacity": (
    property: opacity,
    class: opacity,
    values: (
      0: 0,
      25: 0.25,
      50: 0.5,
      75: 0.75,
      100: 1,
    )
  )
);
```

This generates: `.opacity-0`, `.opacity-25`, `.opacity-50`, `.opacity-75`, `.opacity-100` and their responsive variants (`.opacity-md-50`, etc.).

### Extending an Existing Utility

```scss
// Before importing Bootstrap, extend the spacer values
$spacer: 1rem;
$spacers: (
  0: 0,
  1: ($spacer * .25),
  2: ($spacer * .5),
  3: $spacer,
  4: ($spacer * 1.5),
  5: ($spacer * 3),
  6: ($spacer * 4),    // new
  7: ($spacer * 5),    // new
);

@import "node_modules/bootstrap/scss/bootstrap";
```

### Custom Breakpoint Extension

```scss
// Add a 1600px breakpoint
$grid-breakpoints: (
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 992px,
  xl: 1200px,
  xxl: 1400px,
  xxxl: 1600px
);

@import "node_modules/bootstrap/scss/bootstrap";
```

All utilities now generate `.d-xxxl-flex`, `.col-xxxl-6`, `.text-xxxl-center`, etc.

## Advanced Variations

### Complete Custom Utility Definition

```scss
@import "node_modules/bootstrap/scss/functions";
@import "node_modules/bootstrap/scss/variables";
@import "node_modules/bootstrap/scss/mixins";

// Define custom utility
$utilities: () !default;
$utilities: map-merge($utilities, (
  "cursor": (
    property: cursor,
    class: cursor,
    values: (
      auto: auto,
      default: default,
      pointer: pointer,
      not-allowed: not-allowed,
      grab: grab,
    )
  )
));

// Import remaining Bootstrap partials manually
@import "node_modules/bootstrap/scss/utilities";
@import "node_modules/bootstrap/scss/utilities/api";
// Import components as needed...
```

### State-Based Custom Utility

```scss
// Utility with pseudo-class states
$utilities: map-merge($utilities, (
  "border-opacity": (
    property: border-color,
    class: border-opacity,
    values: (
      10: rgba(var(--bs-border-color), 0.1),
      25: rgba(var(--bs-border-color), 0.25),
      50: rgba(var(--bs-border-color), 0.5),
      75: rgba(var(--bs-border-color), 0.75),
    ),
    responsive: true,
  )
));
```

### Custom Utility with CSS Variables

```scss
$utilities: map-merge($utilities, (
  "gradient-direction": (
    css-var: true,
    css-variable-name: gradient-direction,
    class: gradient,
    values: (
      to-right: to right,
      to-bottom: to bottom,
      to-top-right: to top right,
    )
  )
));
```

### Regenerating Utilities After Extension

```scss
// After extending $utilities, regenerate the API
@import "node_modules/bootstrap/scss/utilities/api";

// Then import components
@import "node_modules/bootstrap/scss/root";
@import "node_modules/bootstrap/scss/reboot";
@import "node_modules/bootstrap/scss/containers";
@import "node_modules/bootstrap/scss/grid";
```

## Best Practices

1. **Use Bootstrap's utility API for custom utilities** - Generates responsive and state variants automatically.
2. **Merge with existing `$utilities` map** - Use `map-merge()` to extend, not replace.
3. **Set `responsive: true` on custom utilities** - Enables breakpoint prefixes (`.custom-md-value`).
4. **Define custom breakpoints before importing Bootstrap** - All utilities automatically get the new breakpoint.
5. **Keep utility values concise** - Short class names are easier to work with in HTML.
6. **Use CSS variables for themable utilities** - `css-var: true` enables runtime value switching.
7. **Document custom utilities in your project's style guide** - Team members need to know what's available.
8. **Test custom utilities with all breakpoint prefixes** - Ensure `.custom-sm-value` generates correctly.
9. **Avoid creating utilities that duplicate Bootstrap's built-in ones** - Extend existing maps instead.
10. **Use `$utilities` merge at the correct point in your import order** - After variables, before the API import.

## Common Pitfalls

1. **Importing Bootstrap before defining custom `$utilities`** - The API has already processed; your additions are ignored.
2. **Forgetting `responsive: true`** - Custom utilities generate without breakpoint prefixes.
3. **Replacing `$utilities` instead of merging** - All Bootstrap utilities disappear.
4. **Adding too many custom utility values** - CSS file bloat; each value generates a class.
5. **Not updating `$grid-breakpoints` when adding custom breakpoints** - Grid and utilities desync.

## Accessibility Considerations

Custom responsive utilities should not hide essential content without providing alternatives. When creating visibility-related utilities, pair them with `.visually-hidden` alternatives for screen reader access. Custom focus-related utilities must maintain visible focus indicators. Avoid creating utilities that reduce contrast below WCAG minimums or remove focus outlines.

## Responsive Behavior

Custom utilities follow Bootstrap's responsive prefix system automatically when `responsive: true` is set. Adding a new breakpoint to `$grid-breakpoints` propagates to ALL utilities (built-in and custom), generating prefixed classes for the new breakpoint. This means extending breakpoints is the single source of truth for responsive behavior across the entire utility system.
