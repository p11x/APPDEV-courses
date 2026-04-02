---
title: "Media Queries Deep Dive"
lesson: "01_06_09"
difficulty: "2"
topics: ["media-queries", "min-width", "max-width", "custom-queries", "breakpoints"]
estimated_time: "30 minutes"
---

# Media Queries Deep Dive

## Overview

Bootstrap 5 uses mobile-first `min-width` media queries exclusively. All base styles apply to the smallest screens, and breakpoint-specific styles layer on top at progressively larger viewports. Understanding Bootstrap's media query conventions - both the SCSS mixins and the raw CSS output - enables you to write custom responsive styles that integrate seamlessly with the framework. Custom media queries using `max-width` or combined conditions provide control for edge cases outside Bootstrap's breakpoint system.

Bootstrap defines six breakpoints: `xs` (default, no query), `sm` (576px), `md` (768px), `lg` (992px), `xl` (1200px), and `xxl` (1400px).

## Basic Implementation

### Bootstrap's Default Breakpoints

```scss
// Defined in _variables.scss
$grid-breakpoints: (
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 992px,
  xl: 1200px,
  xxl: 1400px
);
```

### Using Bootstrap's SCSS Mixins

```scss
// Mobile-first (min-width) - recommended
@include media-breakpoint-up(sm) {
  .custom-element {
    font-size: 1.25rem;
  }
}

// Outputs:
// @media (min-width: 576px) {
//   .custom-element { font-size: 1.25rem; }
// }
```

### Desktop-first (max-width)

```scss
@include media-breakpoint-down(md) {
  .sidebar {
    display: none;
  }
}

// Outputs:
// @media (max-width: 767.98px) {
//   .sidebar { display: none; }
// }
```

### Breakpoint Between Two Sizes

```scss
@include media-breakpoint-only(md) {
  .tablet-only {
    display: block;
  }
}

// Outputs:
// @media (min-width: 768px) and (max-width: 991.98px) {
//   .tablet-only { display: block; }
// }
```

## Advanced Variations

### Between Two Specific Breakpoints

```scss
@include media-breakpoint-between(sm, lg) {
  .content {
    padding: 2rem;
  }
}

// Applies from sm (576px) through lg-1 (991.98px)
```

### Custom Media Queries Outside Bootstrap

```scss
// Extra-large screens (not in Bootstrap by default)
@media (min-width: 1600px) {
  .container { max-width: 1500px; }
}

// Ultra-wide screens
@media (min-width: 2000px) {
  .container { max-width: 1800px; }
}

// Height-based queries
@media (max-height: 600px) {
  .hero { min-height: auto; padding: 1rem 0; }
}

// Orientation query
@media (orientation: landscape) {
  .mobile-nav { display: none; }
}

// Combined conditions
@media (min-width: 768px) and (orientation: landscape) {
  .tablet-landscape-layout { flex-direction: row; }
}
```

### Custom Breakpoint via SCSS

```scss
// Add a custom breakpoint
$grid-breakpoints: (
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 992px,
  xl: 1200px,
  xxl: 1400px,
  xxxl: 1600px  // custom
);

@import "node_modules/bootstrap/scss/bootstrap";

// Now use it
@include media-breakpoint-up(xxxl) {
  .container { max-width: 1500px; }
}
```

### Raw CSS Media Queries (No SCSS)

```css
/* Mobile-first approach */
.custom-class {
  font-size: 1rem;           /* xs: default */
}

@media (min-width: 576px) {
  .custom-class {
    font-size: 1.125rem;     /* sm+ */
  }
}

@media (min-width: 768px) {
  .custom-class {
    font-size: 1.25rem;      /* md+ */
  }
}
```

## Best Practices

1. **Always use mobile-first `min-width` queries** - Matches Bootstrap's own approach.
2. **Use Bootstrap's SCSS mixins** - `media-breakpoint-up()` keeps queries consistent with the framework.
3. **Override variables before importing Bootstrap** - Custom breakpoints must be defined first.
4. **Use `media-breakpoint-down()` sparingly** - Mobile-first is simpler to maintain.
5. **Use `media-breakpoint-only()` for tablet-specific styles** - Targets a single breakpoint range.
6. **Avoid `max-width` queries for layout** - Creates harder-to-maintain specificity chains.
7. **Test at exact breakpoint pixel values** - 576px, 768px, 992px, etc.
8. **Combine orientation and breakpoint queries for mobile landscape** - Handles rotated phones and tablets.
9. **Use CSS custom properties for breakpoint-dependent values** - Reduces media query duplication.
10. **Document custom media queries with comments** - Explain why the breakpoint exists.

## Common Pitfalls

1. **Using `max-width` when Bootstrap expects `min-width`** - Styles conflict with Bootstrap's cascade.
2. **Not accounting for the 0.02px offset in `max-width` queries** - Bootstrap uses `991.98px` not `992px` to avoid subpixel overlap.
3. **Forgetting that `xs` has no media query** - It is the default, un-query layer.
4. **Adding media queries that duplicate Bootstrap utilities** - Use `.d-md-none` instead of custom `@media` rules.
5. **Hardcoding breakpoint values in custom CSS** - Breaks when Bootstrap's breakpoints are customized via SCSS.

## Accessibility Considerations

Media queries should respect user preferences like `prefers-reduced-motion`, `prefers-contrast`, and `prefers-color-scheme`. Bootstrap's Reboot includes `prefers-reduced-motion: reduce` support. When writing custom media queries, always consider users who zoom to 200%+ (WCAG requirement) and those using high-contrast modes. Avoid hiding essential content with `display: none` in media queries without providing an accessible alternative.

## Responsive Behavior

Media queries ARE the responsive behavior layer. Every responsive Bootstrap class (`col-md-6`, `d-lg-flex`, `text-sm-start`) generates a media query under the hood. Understanding the query structure allows you to predict which styles apply at which screen sizes, debug layout issues at specific breakpoints, and write custom responsive code that behaves consistently with Bootstrap's system.
