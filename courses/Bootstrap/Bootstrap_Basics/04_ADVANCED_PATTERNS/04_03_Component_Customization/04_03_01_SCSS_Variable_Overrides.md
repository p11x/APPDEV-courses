---
title: "SCSS Variable Overrides"
module: "Component Customization"
difficulty: 2
duration: "25 minutes"
prerequisites: ["Basic SCSS knowledge", "Bootstrap installation"]
tags: ["scss", "variables", "theming", "customization"]
---

# SCSS Variable Overrides

## Overview

Bootstrap's SCSS architecture is built on the `!default` flag, allowing you to override variables before importing Bootstrap's source. This approach lets you customize the entire framework without modifying Bootstrap's source files. By defining variables in your own SCSS file before Bootstrap's imports, you create a maintainable customization layer that survives framework updates.

## Basic Implementation

Override Bootstrap variables by defining them before importing Bootstrap's SCSS files. The `!default` flag in Bootstrap's source means your values take precedence.

```scss
// custom.scss - Define your overrides first
$primary: #6f42c1;
$secondary: #adb5bd;
$success: #28a745;
$danger: #dc3545;
$warning: #ffc107;
$info: #17a2b8;

// Now import Bootstrap
@import "bootstrap/scss/bootstrap";
```

Compile your custom SCSS into CSS and reference it instead of Bootstrap's default CSS.

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <link rel="stylesheet" href="css/custom.css">
</head>
<body>
  <button class="btn btn-primary">Custom Primary</button>
  <button class="btn btn-secondary">Custom Secondary</button>
</body>
</html>
```

Override typography variables to establish a consistent design system:

```scss
// Typography overrides
$font-family-base: 'Inter', sans-serif;
$font-size-base: 1rem;
$line-height-base: 1.6;
$headings-font-weight: 700;

// Spacing overrides
$spacer: 1rem;

@import "bootstrap/scss/bootstrap";
```

## Advanced Variations

Create theme variations by wrapping overrides in SCSS maps and loops:

```scss
// Theme system with variable overrides
$themes: (
  "light": (
    "body-bg": #ffffff,
    "body-color": #212529,
    "card-bg": #ffffff"
  ),
  "dark": (
    "body-bg": #1a1a2e,
    "body-color": #e0e0e0,
    "card-bg": #16213e
  )
);

@each $theme, $values in $themes {
  [data-theme="#{$theme}"] {
    @each $key, $value in $values {
      --#{$key}: #{$value};
    }
  }
}
```

Override Bootstrap's color system with custom palettes:

```scss
// Custom color palette
$custom-colors: (
  "brand": #ff6b35,
  "accent": #004e89,
  "neutral": #6c757d
);

$theme-colors: (
  "primary": map-get($custom-colors, "brand"),
  "secondary": map-get($custom-colors, "accent"),
  "dark": #212529,
  "light": #f8f9fa
) !default;

@import "bootstrap/scss/bootstrap";
```

Create responsive breakpoint customizations:

```scss
// Custom grid breakpoints
$grid-breakpoints: (
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 992px,
  xl: 1200px,
  xxl: 1440px,
  xxxl: 1920px  // Ultra-wide support
);

$container-max-widths: (
  sm: 540px,
  md: 720px,
  lg: 960px,
  xl: 1140px,
  xxl: 1320px,
  xxxl: 1800px
);

@import "bootstrap/scss/bootstrap";
```

## Best Practices

1. Always define overrides before importing Bootstrap source
2. Use `!default` in your own variables for easy downstream customization
3. Keep overrides in a dedicated `_variables.scss` partial
4. Document why each variable is overridden
5. Maintain alphabetical ordering of variable overrides
6. Test overrides across all Bootstrap components
7. Use CSS custom properties alongside SCSS variables for runtime theming
8. Avoid overriding variables mid-import chain
9. Version control your override files separately
10. Create a variables cheat sheet for team reference
11. Use semantic naming for custom variable additions
12. Validate color contrast ratios after color overrides

## Common Pitfalls

1. Placing variable overrides after Bootstrap imports—values won't take effect
2. Forgetting the `!default` flag when adding new variables to the chain
3. Overriding too many variables, making updates difficult
4. Not testing responsive breakpoints after modifying grid variables
5. Creating variable dependencies that cause circular references
6. Ignoring Bootstrap's internal variable dependencies (e.g., `$theme-colors` affects many derived variables)
7. Hardcoding values instead of using Bootstrap's calculation functions
8. Not recompiling SCSS after changing variables

## Accessibility Considerations

- Maintain WCAG contrast ratios when overriding color variables
- Test overridden colors with contrast checking tools
- Ensure focus states remain visible with custom color schemes
- Preserve sufficient font size minimums when overriding typography variables
- Keep line-height values readable for all users

## Responsive Behavior

- Custom breakpoint variables affect all responsive utilities
- Test grid layouts at every custom breakpoint
- Ensure container max-widths scale appropriately
- Verify responsive typography remains legible at all sizes
