---
title: "Bootstrap Global Settings"
lesson: "01_01_19"
difficulty: "2"
topics: ["enable-variables", "global-config", "feature-flags", "scss-variables"]
estimated_time: "25 minutes"
---

# Bootstrap Global Settings

## Overview

Bootstrap provides a set of global configuration variables that control framework-wide behavior. The `$enable-*` variables act as feature flags, toggling entire CSS output sections on or off during SCSS compilation. Combined with `$spacer`, `$font-size-base`, and `$body-bg` variables, these settings let you tailor Bootstrap's defaults to your project without touching individual component files. Understanding global settings is the foundation for efficient Bootstrap customization.

These variables are defined in `_variables.scss` and must be overridden before importing Bootstrap's main SCSS file. Once compiled, the settings are baked into the output CSS and cannot be changed at runtime.

## Basic Implementation

### Available Feature Flags

```scss
// Override BEFORE importing Bootstrap
$enable-rounded: true;      // border-radius on components
$enable-shadows: false;     // box-shadow on components
$enable-gradients: false;   // gradient backgrounds
$enable-transitions: true;  // transition effects
$enable-reduced-motion: true; // respect prefers-reduced-motion
$enable-grid-classes: true; // .container, .row, .col-*
$enable-cssgrid: false;     // CSS Grid classes
$enable-container-classes: true; // .container-* classes
$enable-caret: true;        // dropdown caret indicators
$enable-button-pointers: true; // cursor: pointer on buttons

@import "node_modules/bootstrap/scss/bootstrap";
```

### Global Layout Settings

```scss
// Base spacing and sizing
$spacer: 1rem;
$spacer-x: $spacer;
$spacer-y: $spacer;

// Body defaults
$body-bg: #ffffff;
$body-color: #212529;
$body-text-align: null;

// Border radius
$border-radius: 0.375rem;
$border-radius-sm: 0.25rem;
$border-radius-lg: 0.5rem;
$border-radius-pill: 50rem;

@import "node_modules/bootstrap/scss/bootstrap";
```

## Advanced Variations

### Minimal Bootstrap Configuration

```scss
// Disable everything non-essential
$enable-rounded: false;
$enable-shadows: false;
$enable-gradients: false;
$enable-transitions: false;
$enable-caret: false;
$enable-button-pointers: false;
$enable-cssgrid: false;

// Reduce base spacing
$spacer: 0.75rem;
$border-radius: 0;

@import "node_modules/bootstrap/scss/bootstrap";
```

### Dark Mode Default Configuration

```scss
// Set dark mode as default
$body-bg: #1a1a2e;
$body-color: #e0e0e0;
$body-secondary-color: #a0a0a0;

// Adjust borders for dark backgrounds
$border-color: #333;

@import "node_modules/bootstrap/scss/bootstrap";
```

### Custom Typography Global Settings

```scss
// Global typography
$font-family-base: 'Inter', system-ui, sans-serif;
$font-size-base: 1rem;
$line-height-base: 1.6;
$headings-font-weight: 700;
$headings-line-height: 1.2;

// Link defaults
$link-color: #0d6efd;
$link-decoration: none;
$link-hover-decoration: underline;

@import "node_modules/bootstrap/scss/bootstrap";
```

### Reading and Inspecting Current Settings

```scss
// After import, you can use Bootstrap's functions to read values
@import "node_modules/bootstrap/scss/functions";
@import "node_modules/bootstrap/scss/variables";

.debug-enable-rounded {
  // Outputs: true or false
  content: "#{$enable-rounded}";
}

.debug-spacer {
  // Outputs: 1rem (or your override)
  content: "#{$spacer}";
}
```

## Best Practices

1. **Override `$enable-*` variables before importing Bootstrap** - SCSS first-write-wins applies.
2. **Disable unused features to reduce CSS output** - `$enable-gradients: false` removes gradient utilities.
3. **Keep `$enable-reduced-motion: true`** - Respects user accessibility preferences.
4. **Use `$enable-transitions: false` for print stylesheets** - Unnecessary in print media.
5. **Set `$body-bg` and `$body-color` for dark mode defaults** - Avoids flash of light theme.
6. **Document your global overrides in a `_config.scss` partial** - Clear single source of truth.
7. **Review `_variables.scss` on each Bootstrap upgrade** - New global variables may be added.
8. **Test with all feature flags enabled** - Ensure your code works with the full Bootstrap suite.
9. **Use `$spacer` as your base unit** - All spacing scales derive from it.
10. **Avoid setting `$enable-grid-classes: false` unless using CSS Grid** - Grid classes are used by most templates.

## Common Pitfalls

1. **Setting feature flags after the Bootstrap import** - Variables are already consumed; overrides have no effect.
2. **Disabling `$enable-grid-classes` without an alternative** - All `.row` and `.col-*` classes disappear from output.
3. **Setting `$spacer` to a pixel value** - Breaks responsive scaling; use `rem` units.
4. **Forgetting `$enable-reduced-motion: true`** - Users with motion sensitivity experience discomfort.
5. **Overriding global variables in individual component files** - Scoping issues; keep globals in a central config file.

## Accessibility Considerations

Global settings directly impact accessibility. `$enable-reduced-motion: true` generates CSS that respects `prefers-reduced-motion: reduce`, disabling animations for users who need it. `$body-color` and `$body-bg` must maintain a 4.5:1 contrast ratio minimum. Setting `$enable-rounded: false` removes visual affordances that help users with cognitive disabilities identify interactive elements. Always test global configuration changes against WCAG 2.1 AA standards.

## Responsive Behavior

Global settings apply uniformly across all breakpoints. `$spacer` produces the same base unit at every screen size, while grid column gutters scale responsively through Bootstrap's `$grid-gutter-width` variable. Global settings like `$enable-container-classes` control whether responsive container variants (`.container-sm`, `.container-md`) are generated, which directly affects how layouts adapt to different viewports.
