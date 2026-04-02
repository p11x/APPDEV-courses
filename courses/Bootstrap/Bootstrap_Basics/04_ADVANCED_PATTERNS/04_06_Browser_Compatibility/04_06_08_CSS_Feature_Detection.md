---
title: CSS Feature Detection with @supports
category: Advanced
difficulty: 3
time: 30 min
tags: bootstrap5, css-feature-detection, supports-query, progressive-css, fallback-strategies
---

# CSS Feature Detection with @supports

## Overview

CSS feature detection using `@supports` allows stylesheets to query browser capabilities and apply styles conditionally. Unlike JavaScript-based feature detection, `@supports` operates entirely in CSS, reducing the need for JavaScript class toggling and improving performance. Bootstrap 5 leverages modern CSS features, making `@supports` essential for providing fallbacks when targeting broad browser support while taking advantage of newer properties like CSS Grid, `aspect-ratio`, `gap`, and `backdrop-filter`.

## Basic Implementation

The `@supports` rule evaluates a CSS declaration and applies nested styles only if the browser supports the property-value pair:

```css
/* Apply grid layout only when supported */
@supports (display: grid) {
  .card-container {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
    gap: 1.5rem;
  }
}

/* Fallback for browsers without grid */
@supports not (display: grid) {
  .card-container {
    display: flex;
    flex-wrap: wrap;
    margin: -0.75rem;
  }
  .card-container > * {
    flex: 1 1 280px;
    margin: 0.75rem;
  }
}
```

Combining multiple conditions with `and` / `or`:

```css
@supports (backdrop-filter: blur(1px)) or (-webkit-backdrop-filter: blur(1px)) {
  .navbar-glass {
    background-color: rgba(255, 255, 255, 0.7);
    backdrop-filter: blur(12px);
    -webkit-backdrop-filter: blur(12px);
  }
}

@supports not ((backdrop-filter: blur(1px)) or (-webkit-backdrop-filter: blur(1px))) {
  .navbar-glass {
    background-color: rgba(255, 255, 255, 0.98);
  }
}
```

## Advanced Variations

Integrating `@supports` with Bootstrap's Sass architecture:

```scss
// _feature-detection.scss
@mixin with-supports($property, $value) {
  @supports (#{$property}: #{$value}) {
    @content;
  }
}

@mixin without-supports($property, $value) {
  @supports not (#{$property}: #{$value}) {
    @content;
  }
}

// Usage in component
@include with-supports('container-type', 'inline-size') {
  .card {
    container-type: inline-size;
    container-name: card;

    @container card (min-width: 400px) {
      .card-body {
        display: grid;
        grid-template-columns: 1fr 1fr;
      }
    }
  }
}
```

Detecting `aspect-ratio` for responsive embeds:

```css
@supports (aspect-ratio: 16 / 9) {
  .video-embed {
    aspect-ratio: 16 / 9;
    width: 100%;
  }
}

@supports not (aspect-ratio: 16 / 9) {
  .video-embed {
    position: relative;
    padding-bottom: 56.25%;
    height: 0;
  }
  .video-embed iframe {
    position: absolute;
    inset: 0;
    width: 100%;
    height: 100%;
  }
}
```

Detecting color functions for theme support:

```css
@supports (color: oklch(0.7 0.15 180)) {
  :root {
    --brand-primary: oklch(0.65 0.2 250);
    --brand-secondary: oklch(0.75 0.15 180);
  }
}

@supports not (color: oklch(0.7 0.15 180)) {
  :root {
    --brand-primary: #3b82f6;
    --brand-secondary: #10b981;
  }
}
```

## Best Practices

1. **Always provide a fallback** - Never rely solely on `@supports` without a base style preceding it
2. **Use `@supports not` for explicit fallbacks** - Make fallback styles intentional rather than accidental
3. **Test with real property-value pairs** - `@supports` checks specific values, not general property support
4. **Combine with Bootstrap's breakpoint system** - Feature detection should complement responsive design
5. **Prefer `@supports` over JavaScript detection** - CSS-based detection avoids layout thrashing and flash of unstyled content
6. **Group related feature checks** - Keep all enhancement styles for a feature together for maintainability
7. **Use vendor-prefixed detection** - Check both prefixed and unprefixed versions for critical features like `backdrop-filter`
8. **Document your feature detection strategy** - Team members need to understand which features have fallbacks
9. **Audit regularly** - Remove `@supports` blocks once baseline browser support reaches target thresholds
10. **Keep selectors identical** - Use the same selectors in both supported and fallback blocks to avoid specificity conflicts

## Common Pitfalls

1. **Checking unsupported syntax** - The property-value syntax must be valid CSS; typos cause silent failures
2. **Nesting `@supports` excessively** - Deeply nested feature checks become unmaintainable
3. **Forgetting vendor prefixes** - `backdrop-filter` needs `-webkit-` prefix detection for Safari
4. **Missing base styles** - Styles outside `@supports` blocks are the true fallback; forgetting them breaks older browsers
5. **Using `@supports` for JavaScript features** - CSS cannot detect JS APIs; use Modernizr or manual checks for those
6. **Ignoring specificity** - `@supports` does not change specificity; fallback and enhancement selectors must match
7. **Over-detecting** - Checking features that have 98%+ support adds complexity without benefit

## Accessibility Considerations

Feature detection should not remove accessibility. When applying enhanced styles like `backdrop-filter` blurs, ensure text remains readable with sufficient contrast. High contrast mode on Windows may override CSS, so test with `forced-colors: active` alongside `@supports`. Reduced motion preferences (`prefers-reduced-motion`) should be combined with feature detection to disable animations even when the browser supports them.

## Responsive Behavior

`@supports` works alongside media queries to create layered responsive designs. Use `@supports` inside `@media` breakpoints or vice versa to apply grid layouts only on wider viewports where they provide the most value. Bootstrap's mobile-first approach means the baseline (no-grid) experience serves mobile users, while `@supports (display: grid)` enhances desktop layouts with more sophisticated grid configurations.
