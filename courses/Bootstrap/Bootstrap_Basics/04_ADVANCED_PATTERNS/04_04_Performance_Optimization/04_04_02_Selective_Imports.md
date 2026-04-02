---
title: "Selective Imports"
module: "Performance Optimization"
difficulty: 2
duration: "25 minutes"
prerequisites: ["SCSS basics", "Bootstrap structure"]
tags: ["scss", "imports", "optimization", "modular"]
---

# Selective Imports

## Overview

Bootstrap's SCSS architecture is modular—you can import only the components and utilities your project needs. This approach dramatically reduces CSS payload by eliminating unused component styles, utility classes, and helper functions, resulting in faster page loads and smaller bundle sizes.

## Basic Implementation

Create a custom SCSS file with only the components you need:

```scss
// _custom-bootstrap.scss

// Required foundation imports
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/maps";
@import "bootstrap/scss/mixins";
@import "bootstrap/scss/utilities";

// Core essentials
@import "bootstrap/scss/root";
@import "bootstrap/scss/reboot";

// Typography
@import "bootstrap/scss/type";

// Grid system
@import "bootstrap/scss/containers";
@import "bootstrap/scss/grid";

// Forms
@import "bootstrap/scss/forms";
@import "bootstrap/scss/input-group";

// Buttons
@import "bootstrap/scss/buttons";
@import "bootstrap/scss/button-group";

// Utilities (only what you need)
@import "bootstrap/scss/utilities/api";
```

## Advanced Variations

Component-level selective imports for micro-frontend architecture:

```scss
// Component-specific imports for a dashboard module
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/mixins";

// Only card-related styles
@import "bootstrap/scss/card";

// Only table-related styles
@import "bootstrap/scss/tables";

// Only pagination
@import "bootstrap/scss/pagination";

// Only needed utilities
@import "bootstrap/scss/utilities/api";
```

Configure utility-level imports to include only specific utility groups:

```scss
// Selective utility imports
$utilities: () !default;

// Enable only spacing utilities
$utilities: map-merge($utilities, (
  "margin": (
    property: margin,
    class: m,
    responsive: true,
    values: map-merge($spacers, (auto: auto))
  ),
  "padding": (
    property: padding,
    class: p,
    responsive: true,
    values: $spacers
  )
));

// Enable only display utilities
$utilities: map-merge($utilities, (
  "display": (
    property: display,
    class: d,
    responsive: true,
    values: (
      none: none,
      inline: inline,
      block: block,
      flex: flex,
      grid: grid
    )
  )
));

@import "bootstrap/scss/utilities/api";
```

Create a modular import architecture:

```scss
// _bootstrap-core.scss - Shared foundation
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/maps";
@import "bootstrap/scss/mixins";

// _bootstrap-layout.scss - Layout components
@import "_bootstrap-core";
@import "bootstrap/scss/containers";
@import "bootstrap/scss/grid";

// _bootstrap-ui.scss - UI components
@import "_bootstrap-core";
@import "bootstrap/scss/buttons";
@import "bootstrap/scss/card";
@import "bootstrap/scss/modal";
```

## Best Practices

1. Always include functions, variables, and mixins as foundation
2. Import reboot for consistent cross-browser defaults
3. Review Bootstrap's SCSS file dependencies before selective imports
4. Document which components are imported and why
5. Use SCSS partials to organize import groups
6. Test all UI elements after reducing imports
7. Keep utility imports focused on actually-used classes
8. Version your import configuration with your design system
9. Use `@forward` and `@use` for modern SCSS module architecture
10. Maintain an import manifest for team reference

## Common Pitfalls

1. Missing required foundation imports (functions, variables, mixins)
2. Not importing dependencies (e.g., importing modal without transitions)
3. Breaking component dependencies by skipping shared mixins
4. Forgetting to import utilities/api for utility class generation
5. Importing in wrong order causing variable reference errors
6. Not testing all pages after reducing imports
7. Missing responsive utility imports when using responsive classes
8. Overlooking JavaScript component CSS dependencies

## Accessibility Considerations

- Always include `.visually-hidden` utility imports
- Ensure focus-related utilities are imported
- Verify screen reader utilities remain available
- Test keyboard navigation with reduced imports

## Responsive Behavior

- Import grid and container components together
- Verify responsive utility classes work with selective imports
- Test layouts at all breakpoints after optimization
- Ensure responsive visibility utilities are included
