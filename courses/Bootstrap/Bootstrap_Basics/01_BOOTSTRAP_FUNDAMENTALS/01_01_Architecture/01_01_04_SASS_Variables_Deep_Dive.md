---
tags: [bootstrap, sass, variables, customization, theming, design-tokens]
category: Bootstrap Fundamentals
difficulty: 3
estimated_time: 40 minutes
---

# SASS Variables Deep Dive

## Overview

Sass variables are the backbone of Bootstrap 5's customization system. Every color, spacing value, font size, border radius, and component option is defined as a Sass variable in `_variables.scss`, making it the single source of truth for the framework's design tokens.

The `!default` flag is the mechanism that enables customization. When a Sass variable is declared with `!default`, it is only assigned if the variable hasn't been defined yet. This means overriding a variable before importing Bootstrap's `_variables.scss` file effectively replaces the framework's default value.

Bootstrap defines over 1,500 Sass variables organized into logical categories: color system, theme colors, spacing, typography, breakpoints, borders, shadows, z-index, components, and more. Understanding how these variables relate to each other through derived calculations is essential for coherent theming.

The **color map system** uses Sass maps (`$colors`, `$theme-colors`, `$grays`) to organize color values. These maps drive the generation of utility classes (`.text-primary`, `.bg-success`), component variants (`.btn-danger`, `.alert-warning`), and CSS custom properties throughout the framework.

**Custom variable creation** follows Bootstrap's own patterns — defining variables in a dedicated partial with `!default` flags, then importing that partial before Bootstrap's variables to take precedence.

```scss
// The core Sass variables file structure
// bootstrap/scss/_variables.scss (simplified)

// Color maps first
$white:    #fff !default;
$black:    #000 !default;

$grays: (
  "100": $gray-100,
  "200": $gray-200,
  "300": $gray-300,
  "400": $gray-400,
  "500": $gray-500,
  "600": $gray-600,
  "700": $gray-700,
  "800": $gray-800,
  "900": $gray-900
) !default;

// Theme color map - drives most component variants
$theme-colors: (
  "primary":    $primary,
  "secondary":  $secondary,
  "success":    $success,
  "info":       $info,
  "warning":    $warning,
  "danger":     $danger,
  "light":      $light,
  "dark":       $dark
) !default;
```

## Basic Implementation

Customizing Bootstrap starts with overriding variables before importing the framework. The import order is critical: your variable overrides must come after functions but before Bootstrap's `_variables.scss`.

```scss
// _custom-variables.scss
// This file overrides Bootstrap defaults

// ---- Color Overrides ----
$primary:       #6f42c1;
$secondary:     #6c757d;
$success:       #198754;
$info:          #0dcaf0;
$warning:       #ffc107;
$danger:        #dc3545;

// ---- Spacing ----
$spacer:        1rem;

// ---- Typography ----
$font-family-sans-serif: 'Inter', system-ui, -apple-system, sans-serif;
$font-size-base:          1rem;
$line-height-base:        1.6;

// ---- Borders ----
$border-width:            1px;
$border-radius:           0.5rem;
$border-radius-sm:        0.25rem;
$border-radius-lg:        0.75rem;
$border-radius-pill:      50rem;

// ---- Component Variables ----
$btn-padding-y:           0.5rem;
$btn-padding-x:           1.25rem;
$btn-font-size:           $font-size-base;

$card-border-radius:      $border-radius;
$card-cap-bg:             rgba($primary, 0.05);

// ---- Shadows ----
$box-shadow:              0 0.5rem 1rem rgba($black, 0.15);
$box-shadow-sm:           0 0.125rem 0.25rem rgba($black, 0.075);
$box-shadow-lg:           0 1rem 3rem rgba($black, 0.175);

// ---- Import Bootstrap AFTER overrides ----
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";   // Your overrides above take precedence
@import "bootstrap/scss/mixins";
@import "bootstrap/scss/bootstrap";
```

Understanding how Bootstrap derives component variables from base variables is crucial for coherent customization:

```scss
// Bootstrap derives component variables from base values
// This cascade means changing $primary affects hundreds of derived values

// Base
$primary:       $blue !default;      // #0d6efd

// Derived theme color functions
$primary-bg-subtle:     tint-color($primary, 80%) !default;
$primary-border-subtle: shade-color($primary, 60%) !default;

// Button primary derived from $primary
$btn-primary-bg:                $primary !default;
$btn-primary-border:            $primary !default;
$btn-primary-color:             color-contrast($primary) !default;
$btn-primary-hover-bg:          shade-color($primary, 15%) !default;
$btn-primary-hover-border:      shade-color($primary, 20%) !default;
$btn-primary-active-bg:         shade-color($primary, 20%) !default;
$btn-primary-active-border:     shade-color($primary, 25%) !default;

// Alert primary
$alert-primary-bg:              $primary-bg-subtle !default;
$alert-primary-border:          $primary-border-subtle !default;
$alert-primary-color:           shade-color($primary, 40%) !default;
```

## Advanced Variations

Advanced theming leverages Sass maps, color functions, and the utility API to create comprehensive design systems on top of Bootstrap.

Extending the `$theme-colors` map generates new contextual classes across all components automatically:

```scss
// _advanced-theme.scss

// Define additional theme colors
$custom-theme-colors: (
  "brand":      #6f42c1,
  "brand-dark": #4a2a8a,
  "accent":     #e83e8c,
  "neutral":    #6c757d
);

// Merge with Bootstrap's default theme colors
$theme-colors: map-merge($theme-colors, $custom-theme-colors);

// Generate subtle variants for new theme colors
@each $color, $value in $custom-theme-colors {
  $theme-colors-rgb: map-merge(
    $theme-colors-rgb,
    (#{$color}: to-rgb($value))
  );
  
  #{"--bs-#{$color}-rgb"}: to-rgb($value);
  #{"--bs-#{$color}-bg-subtle"}: #{tint-color($value, 80%)};
  #{"--bs-#{$color}-border-subtle"}: #{shade-color($value, 60%)};
  #{"--bs-#{$color}-text-emphasis"}: #{shade-color($value, 40%)};
}

// After this merge, classes like .btn-brand, .bg-accent, .text-neutral
// are automatically generated by Bootstrap's component and utility systems
```

Sass maps are deeply composable. Bootstrap uses nested maps for complex configurations like spacing scales and gradient stops:

```scss
// Override component-specific maps
$spacer: 0.25rem !default;

$spacers: (
  0: 0,
  1: $spacer * 0.5,    // 0.125rem
  2: $spacer,           // 0.25rem
  3: $spacer * 2,       // 0.5rem
  4: $spacer * 3,       // 0.75rem
  5: $spacer * 4,       // 1rem
  6: $spacer * 5,       // 1.25rem
  7: $spacer * 7.5,     // 1.875rem
  8: $spacer * 10,      // 2.5rem
  9: $spacer * 12.5,    // 3.125rem
  10: $spacer * 15,     // 3.75rem
  11: $spacer * 20,     // 5rem
) !default;

// This generates utilities: .mt-1, .px-5, .mb-10, etc.
// with the values defined above
```

Creating a custom variable partial that mirrors Bootstrap's structure:

```scss
// _variables-custom.scss
// Organized file for all project-level overrides

// =========================================
// COLOR SYSTEM
// =========================================
$primary:       #2563eb;
$secondary:     #475569;

$custom-colors: (
  "ocean":      #0ea5e9,
  "forest":     #16a34a,
  "sunset":     #f97316
);

$theme-colors: map-merge($theme-colors, $custom-colors);

// =========================================
// TYPOGRAPHY
// =========================================
$font-family-base:            'Inter', sans-serif;
$headings-font-weight:        700;
$headings-line-height:        1.2;

$h1-font-size:                $font-size-base * 2.5;
$h2-font-size:                $font-size-base * 2;
$h3-font-size:                $font-size-base * 1.75;
$h4-font-size:                $font-size-base * 1.5;
$h5-font-size:                $font-size-base * 1.25;
$h6-font-size:                $font-size-base;

// =========================================
// SPACING & LAYOUT
// =========================================
$spacer:                      1rem;
$container-max-widths: (
  sm: 540px,
  md: 720px,
  lg: 960px,
  xl: 1140px,
  xxl: 1320px
) !default;

// =========================================
// COMPONENTS
// =========================================
$border-radius:               0.75rem;
$border-radius-sm:            0.375rem;
$border-radius-lg:            1rem;

$card-border-radius:          $border-radius;
$modal-content-border-radius: $border-radius-lg;
$dropdown-border-radius:      $border-radius-sm;
```

## Best Practices

- **Override variables before importing Bootstrap** — variables declared after the import are ignored because `!default` only applies when the variable is undefined.
- **Use a dedicated `_variables-custom.scss` partial** to centralize all overrides in one maintainable file.
- **Derive values from base variables** using Sass functions (`tint-color`, `shade-color`, `mix`) rather than hardcoding color values for component states.
- **Extend `$theme-colors` instead of replacing it** — use `map-merge` to add custom colors while preserving Bootstrap's default palette.
- **Maintain WCAG contrast ratios** when customizing colors; use `color-contrast()` to verify that foreground/background combinations meet accessibility standards.
- **Use semantic variable names** for custom additions (`$brand-primary`, `$surface-elevated`) rather than descriptive names (`$purple`, `$dark-gray`).
- **Document the purpose of each override** with comments explaining why the default was changed, especially for team projects.
- **Test all component variants** after changing base variables — a `$primary` change affects buttons, alerts, badges, list groups, and dozens of other components.
- **Keep the override file minimal** — only change what's necessary; excessive overrides create maintenance burden during Bootstrap version upgrades.
- **Use `$enable-*` feature flags** to disable unused features (`$enable-gradients: false`) rather than overriding their output with custom CSS.
- **Version-control your custom variable files** separately from Bootstrap's source to simplify upgrades.

## Common Pitfalls

- **Importing Bootstrap CSS from CDN after defining custom variables** — Sass variables only affect compiled output; the CDN version ignores all overrides.
- **Forgetting `!default` on custom variable files** — without `!default`, variables cannot be overridden downstream, breaking the customization chain for child projects or shared packages.
- **Overriding variables after the Bootstrap import** — the `!default` flag means the first definition wins; later assignments are silently ignored.
- **Incorrect Sass map syntax** — missing commas, wrong nesting, or using `!default` inside map values causes compilation errors that can be difficult to debug.
- **Modifying `$theme-colors` after importing components** — components that reference `$theme-colors` at import time use the map's state at that moment; later changes don't retroactively update them.
- **Using CSS `var()` syntax inside Sass variables** — Sass processes variables at compile time, before CSS custom properties exist; use `var(--bs-primary)` in CSS output, not in Sass variable definitions.
- **Assuming derived variables update automatically** — changing `$primary` updates `$btn-primary-bg` only if the import order is correct and the variables are properly chained with `!default`.

## Accessibility Considerations

Sass variable customization directly impacts accessibility. Color choices affect contrast ratios, font sizing affects readability, and spacing affects touch target sizes.

```scss
// Ensure contrast-safe color customization
// Bootstrap's color-contrast() function returns white or black
// based on the luminance of the input color

$primary: #6f42c1 !default;

// This automatically selects white (#fff) for dark backgrounds
$btn-primary-color: color-contrast($primary) !default;

// For custom contrast checking in your theme:
$min-contrast-ratio: 4.5 !default; // WCAG AA for normal text

// Verify your custom colors meet contrast requirements
$custom-primary: #2563eb;
$custom-primary-text: if(
  contrast-ratio($custom-primary, $white) >= $min-contrast-ratio,
  $white,
  $black
);

// Responsive font sizing for accessibility
$font-size-root:              null !default; // Set to override html font-size
$font-size-base:              1rem !default;
$h1-font-size:                $font-size-base * 2.5 !default;

// Reduced motion support
$enable-reduced-motion: true !default;
// Generates prefers-reduced-motion media query automatically
```

## Responsive Behavior

Sass variables control responsive behavior through breakpoint maps and container max-widths. Modifying these maps changes how the entire responsive system functions.

```scss
// Customize breakpoints (must be done before importing grid)
$grid-breakpoints: (
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 992px,
  xl: 1200px,
  xxl: 1400px
) !default;

// Add a custom breakpoint
$grid-breakpoints: map-merge(
  $grid-breakpoints,
  ("3xl": 1600px)
);

// Customize container widths at each breakpoint
$container-max-widths: (
  sm: 540px,
  md: 720px,
  lg: 960px,
  xl: 1140px,
  xxl: 1320px
) !default;

// Grid column count
$grid-columns:               12 !default;
$grid-gutter-width:          1.5rem !default;

// Generate responsive spacing tokens
$grid-row-columns:           6 !default;

// This enables classes like .g-xxl-4 and .col-xxl-3
// when custom breakpoints are merged before grid import
```
