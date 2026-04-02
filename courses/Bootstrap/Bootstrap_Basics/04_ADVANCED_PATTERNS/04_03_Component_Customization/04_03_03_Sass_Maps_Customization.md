---
title: "Sass Maps Customization"
module: "Component Customization"
difficulty: 3
duration: "35 minutes"
prerequisites: ["SCSS fundamentals", "Bootstrap SCSS structure"]
tags: ["scss", "maps", "theming", "grid", "colors"]
---

# Sass Maps Customization

## Overview

Bootstrap relies heavily on Sass maps for managing colors, breakpoints, spacers, and other design tokens. Customizing these maps allows you to reshape Bootstrap's entire design system to match your project's requirements. Understanding how to merge, extend, and override these maps is essential for advanced Bootstrap customization.

## Basic Implementation

Override the `$theme-colors` map to create a custom color palette:

```scss
// _variables.scss - Override before Bootstrap import
$theme-colors: (
  "primary": #2563eb,
  "secondary": #64748b,
  "success": #16a34a,
  "danger": #dc2626,
  "warning": #f59e0b,
  "info": #0ea5e9,
  "light": #f1f5f9,
  "dark": #1e293b
);

@import "bootstrap/scss/bootstrap";
```

Add custom colors to the theme map using map-merge:

```scss
// Add brand-specific colors without losing defaults
$theme-colors: (
  "primary": #6366f1,
  "accent": #ec4899,
  "neutral": #6b7280,
  "surface": #f9fafb
);

@import "bootstrap/scss/bootstrap";
```

Customize grid breakpoints for a tailored responsive system:

```scss
$grid-breakpoints: (
  xs: 0,
  sm: 640px,
  md: 768px,
  lg: 1024px,
  xl: 1280px,
  xxl: 1536px
);

$container-max-widths: (
  sm: 600px,
  md: 720px,
  lg: 960px,
  xl: 1200px,
  xxl: 1440px
);

@import "bootstrap/scss/bootstrap";
```

## Advanced Variations

Extend Bootstrap's spacer scale for more granular spacing control:

```scss
// Extend spacers with additional values
$spacer: 1rem;
$spacers: (
  0: 0,
  1: 0.25rem,
  2: 0.5rem,
  3: 1rem,
  4: 1.5rem,
  5: 2rem,
  6: 3rem,
  7: 4rem,
  8: 6rem,
  9: 8rem,
  10: 10rem
);

@import "bootstrap/scss/bootstrap";
```

Create semantic color maps that reference theme colors:

```scss
// Semantic color system
$semantic-colors: (
  "surface-primary": "light",
  "surface-secondary": "white",
  "text-primary": "dark",
  "text-secondary": "secondary",
  "border-default": "light",
  "border-strong": "secondary"
);

// Generate CSS custom properties from semantic map
@each $name, $reference in $semantic-colors {
  --bs-#{$name}: var(--bs-#{$reference});
}
```

Build a complete typography scale map:

```scss
// Custom font size scale
$font-sizes: (
  "xs": 0.75rem,
  "sm": 0.875rem,
  "base": 1rem,
  "lg": 1.125rem,
  "xl": 1.25rem,
  "2xl": 1.5rem,
  "3xl": 1.875rem,
  "4xl": 2.25rem,
  "5xl": 3rem
);

// Generate utility classes from font size map
@each $size, $value in $font-sizes {
  .fs-#{$size} {
    font-size: $value !important;
  }
}
```

Modify border-radius values for consistent component styling:

```scss
// Custom border radius scale
$border-radius: 0.375rem;
$border-radius-sm: 0.25rem;
$border-radius-lg: 0.75rem;
$border-radius-xl: 1rem;
$border-radius-pill: 50rem;
$border-radius-circle: 50%;

@import "bootstrap/scss/bootstrap";
```

## Best Practices

1. Always define map overrides before Bootstrap imports
2. Use `map-merge` to extend rather than replace default maps
3. Document the purpose of each custom map entry
4. Maintain consistent naming conventions across maps
5. Test all affected components after map modifications
6. Keep breakpoint maps logically ordered (small to large)
7. Use descriptive names for custom theme colors
8. Validate color contrast after theme color changes
9. Create a central variables file for all map overrides
10. Use SCSS functions to derive related values from maps
11. Consider downstream effects when modifying shared maps
12. Version control map changes with clear commit messages

## Common Pitfalls

1. Forgetting that map overrides completely replace defaults
2. Not testing components that depend on modified maps
3. Creating breakpoint maps with illogical ordering
4. Adding too many custom colors, creating design inconsistency
5. Breaking Bootstrap's internal calculations by removing required map keys
6. Not updating container max-widths when modifying breakpoints
7. Ignoring the cascade of map-dependent variables
8. Overriding maps after Bootstrap imports (too late to take effect)

## Accessibility Considerations

- Ensure theme colors meet WCAG AA contrast requirements
- Test custom color maps with color blindness simulators
- Maintain focus indicator visibility across theme variations
- Preserve minimum touch target sizes in spacing maps
- Document accessibility implications of color map changes

## Responsive Behavior

- Verify grid behavior at all custom breakpoints
- Test container widths match design specifications
- Ensure responsive text sizes remain legible
- Validate spacing scales work across viewport sizes
