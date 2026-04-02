---
title: "CSS-First Architecture"
description: "Moving from Sass to CSS custom properties for CSS-only theming without a preprocessor"
difficulty: 2
tags: [css-custom-properties, css-variables, theming, no-sass, architecture]
prerequisites:
  - 01_03_Customizing_Bootstrap
  - 09_02_05_CSS_Color_Functions
---

## Overview

CSS-first architecture replaces Sass variables, mixins, and functions with native CSS features: custom properties for values, `@layer` for cascade, `color-mix()` for color manipulation, and CSS nesting for structure. The result is a theme system that works without a build step — pure CSS that the browser processes natively.

Bootstrap 5.3's shift to CSS custom properties (`--bs-primary`, `--bs-body-bg`) laid the groundwork. A CSS-first architecture extends this to every configurable value: spacing, typography, borders, shadows, z-index, and animation timing. Themes become single CSS files that override custom properties, eliminating Sass recompilation.

## Basic Implementation

```css
/* tokens.css — design tokens as CSS custom properties */
:root {
  /* Colors */
  --ds-color-primary: #0d6efd;
  --ds-color-secondary: #6c757d;
  --ds-color-success: #198754;
  --ds-color-danger: #dc3545;

  /* Spacing */
  --ds-space-xs: 0.25rem;
  --ds-space-sm: 0.5rem;
  --ds-space-md: 1rem;
  --ds-space-lg: 1.5rem;
  --ds-space-xl: 3rem;

  /* Typography */
  --ds-font-family: system-ui, -apple-system, sans-serif;
  --ds-font-size-sm: 0.875rem;
  --ds-font-size-base: 1rem;
  --ds-font-size-lg: 1.25rem;
  --ds-font-size-xl: 1.5rem;

  /* Borders */
  --ds-border-radius: 0.375rem;
  --ds-border-width: 1px;
  --ds-border-color: #dee2e6;

  /* Shadows */
  --ds-shadow-sm: 0 1px 2px rgba(0,0,0,0.05);
  --ds-shadow-md: 0 4px 6px rgba(0,0,0,0.1);
  --ds-shadow-lg: 0 10px 15px rgba(0,0,0,0.1);
}
```

```css
/* Theme: dark mode — just override tokens */
[data-theme="dark"],
.dark {
  --ds-color-primary: #6ea8fe;
  --ds-color-secondary: #adb5bd;
  --ds-border-color: #495057;
  --ds-shadow-sm: 0 1px 2px rgba(0,0,0,0.3);
}

/* Theme: corporate */
[data-theme="corporate"] {
  --ds-color-primary: #1a365d;
  --ds-color-secondary: #718096;
  --ds-font-family: "Inter", sans-serif;
  --ds-border-radius: 0;
}
```

```html
<!-- No Sass compilation needed -->
<link rel="stylesheet" href="tokens.css">
<link rel="stylesheet" href="components.css">
<link rel="stylesheet" href="bootstrap-override.css">

<!-- Switch themes by changing the attribute -->
<button onclick="document.documentElement.dataset.theme='dark'">Dark Mode</button>
```

## Advanced Variations

Responsive tokens:

```css
:root {
  --ds-spacing-base: 1rem;
}

@media (min-width: 768px) {
  :root {
    --ds-spacing-base: 1.5rem;
  }
}

/* Components use the variable — automatically responsive */
.card { padding: var(--ds-spacing-base); }
```

## Best Practices

1. Define all design tokens as CSS custom properties under `:root`.
2. Use a naming convention (`--ds-*` or `--bs-*`) for all custom properties.
3. Override tokens by redefining them in attribute selectors or media queries.
4. Use `color-mix()` for derived colors instead of preprocessor functions.
5. Layer themes with `@layer` to control override precedence.
6. Provide fallback values: `var(--ds-primary, #0d6efd)`.
7. Keep token files separate from component files for modularity.
8. Document every token and its intended usage.
9. Use `@supports` to provide Sass fallbacks for older browsers.
10. Migrate Sass variable overrides to CSS custom property overrides incrementally.
11. Use CSS nesting instead of Sass nesting for new component styles.
12. Generate TypeScript types from CSS custom properties for DX.

## Common Pitfalls

1. **Runtime cost** — CSS custom properties are resolved at runtime, slightly slower than compiled Sass values.
2. **No Sass functions** — `darken()`, `lighten()` have no direct CSS equivalent; use `color-mix()`.
3. **Cascade dependency** — Custom properties cascade; redefining a parent value affects all children.
4. **No loops or maps** — CSS can't iterate; generate repetitive tokens with a build step or manually.
5. **Fallback overload** — Too many fallback values clutter the codebase.
6. **Browser DevTools** — Custom property debugging can be harder than Sass variable inspection.

## Accessibility Considerations

Store high-contrast theme overrides as custom property sets. Switch themes with `data-theme` attributes to respect user preferences:

```css
@media (prefers-contrast: high) {
  :root {
    --ds-border-color: #000;
    --ds-shadow-sm: none;
    --ds-font-size-base: 1.125rem;
  }
}
```

## Responsive Behavior

Override tokens inside media queries for responsive theming. Components that reference tokens automatically adapt without per-component responsive code.
