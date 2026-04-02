---
title: Bootstrap Utility API
category: Advanced
difficulty: 3
time: 30 min
tags: bootstrap5, utility-api, sass, custom-utilities, code-generation
---

# Bootstrap Utility API

## Overview

Bootstrap 5's Utility API provides a programmatic way to generate custom utility classes through Sass configuration. The `$utilities` map defines which utilities are generated, their CSS properties, values, variants, and responsive behavior. This system replaces manual utility class creation with a declarative configuration approach, ensuring consistency with Bootstrap's built-in utilities and enabling tree-shaking of unused classes during the build process.

## Basic Implementation

Bootstrap's `$utilities` map structure defines each utility with its property, class name, values, and variants:

```scss
// _custom-utilities.scss
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/mixins";

// Add custom utilities to the $utilities map
$utilities: (
  "opacity": (
    property: opacity,
    class: opacity,
    values: (
      0: 0,
      25: 0.25,
      50: 0.5,
      75: 0.75,
      100: 1
    )
  ),
  "overflow": (
    property: overflow,
    class: overflow,
    values: (
      auto: auto,
      hidden: hidden,
      visible: visible,
      scroll: scroll
    )
  )
);

@import "bootstrap/scss/utilities";
@import "bootstrap/scss/utilities/api";
```

Enabling responsive and state variants:

```scss
$utilities: (
  "text-truncate": (
    property: text-overflow,
    class: text-truncate,
    values: (ellipsis: ellipsis),
    responsive: true
  ),
  "cursor": (
    property: cursor,
    class: cursor,
    values: (
      auto: auto,
      pointer: pointer,
      default: default,
      not-allowed: not-allowed,
      grab: grab
    ),
    state: hover
  )
);
```

## Advanced Variations

Creating a complete custom utility set:

```scss
// Custom utility: aspect ratio
$utilities: map-merge($utilities, (
  "aspect-ratio": (
    property: aspect-ratio,
    class: ratio,
    values: (
      "1x1": 1 / 1,
      "4x3": 4 / 3,
      "16x9": 16 / 9,
      "21x9": 21 / 9,
      "auto": auto
    ),
    responsive: true
  )
));

// Custom utility: backdrop blur
$utilities: map-merge($utilities, (
  "backdrop-blur": (
    property: backdrop-filter,
    class: backdrop-blur,
    values: (
      none: blur(0),
      sm: blur(4px),
      md: blur(12px),
      lg: blur(20px),
      xl: blur(40px)
    )
  )
));

// Custom utility: gap for flex containers
$utilities: map-merge($utilities, (
  "row-gap": (
    property: row-gap,
    class: rg,
    values: map-merge($spacers, (0: 0)),
    responsive: true
  ),
  "col-gap": (
    property: column-gap,
    class: cg,
    values: map-merge($spacers, (0: 0)),
    responsive: true
  )
));
```

Filtering existing utilities for tree-shaking:

```scss
// Remove utilities you don't need
$utilities: map-remove($utilities, "float", "overflow", "display");

// Disable variants on specific utilities
$utilities: map-merge($utilities, (
  "border": map-merge(map-get($utilities, "border"), (
    state: null  // Remove hover/focus variants
  ))
));
```

Generating utilities with CSS custom property support:

```scss
$utilities: map-merge($utilities, (
  "theme": (
    css-var: true,
    class: theme,
    values: (
      "primary": var(--bs-primary),
      "secondary": var(--bs-secondary),
      "accent": var(--bs-accent, #6f42c1)
    )
  )
));
```

## Best Practices

1. **Use `map-merge` for additions** - Merge custom utilities with Bootstrap's default `$utilities` map instead of replacing it
2. **Remove unused utilities** - Strip out `float`, `overflow`, and other rarely used utilities to reduce CSS size
3. **Enable responsive variants sparingly** - Only set `responsive: true` on utilities that genuinely need breakpoint-specific values
4. **Follow Bootstrap naming conventions** - Match class prefixes and value naming to existing utilities for consistency
5. **Use `css-var: true` for themeable values** - Leverage CSS custom properties for utilities that should adapt to theme changes
6. **Document custom utilities** - Maintain a reference of all custom utility classes for team onboarding
7. **Validate generated output** - Inspect compiled CSS to verify utilities generate expected selectors
8. **Combine with `$enable-*` flags** - Use Bootstrap's feature flags alongside utility configuration
9. **Keep values concise** - Limit utility values to commonly needed options; use custom CSS for edge cases
10. **Test responsive utilities** - Verify that `responsive: true` utilities generate correct `@media` queries

## Common Pitfalls

1. **Replacing `$utilities` instead of merging** - Using `=` instead of `map-merge` destroys all built-in utilities
2. **Incorrect map structure** - Missing `property`, `class`, or `values` keys cause silent generation failures
3. **Too many responsive utilities** - Every `responsive: true` utility multiplies generated CSS by 6 (breakpoints)
4. **Duplicate class names** - Custom utilities with the same `class` as built-ins cause conflicts
5. **Forgetting `@import "bootstrap/scss/utilities/api"`** - Without this import, no utilities are generated
6. **Using invalid CSS property values** - The utility API does not validate CSS; invalid values produce broken rules
7. **Not enabling state variants** - Setting `state: null` removes hover/focus variants, which may be unexpected

## Accessibility Considerations

Custom utilities should not introduce accessibility barriers. Avoid generating utilities that hide content from screen readers without providing alternatives (e.g., `sr-only` should always be available). Ensure that interactive state utilities like `cursor: not-allowed` are paired with actual disabled state handling in JavaScript.

## Responsive Behavior

Set `responsive: true` on utilities that need breakpoint-specific application. Bootstrap generates `.utility-{value}`, `.sm\:utility-{value}`, `.md\:utility-{value}`, etc. for each responsive utility. This follows the mobile-first approach where the base class applies to all sizes and prefixed classes override at larger breakpoints.
