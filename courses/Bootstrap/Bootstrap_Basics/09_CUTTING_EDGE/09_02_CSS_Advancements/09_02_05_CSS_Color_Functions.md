---
title: "CSS Color Functions"
description: "Using color-mix(), light-dark(), and relative color syntax with Bootstrap themes"
difficulty: 3
tags: [color-mix, light-dark, relative-colors, bootstrap-theming, css-functions]
prerequisites:
  - 01_03_Customizing_Bootstrap
---

## Overview

CSS color functions eliminate much of the Sass color manipulation needed for Bootstrap theming. `color-mix()` blends two colors in a specified color space. `light-dark()` returns one of two values based on the user's color scheme preference. Relative color syntax (`color(from var(--bs-primary) ...)`) derives new colors from an existing one — lighten, darken, saturate — all in CSS without preprocessors.

Bootstrap 5.3 introduced CSS custom properties for all theme colors, making these functions immediately useful. Instead of defining `--bs-primary-rgb` and computing alpha variants with `rgba()`, use `color-mix(in srgb, var(--bs-primary), transparent 50%)` for a 50% opacity variant.

## Basic Implementation

```css
:root {
  --bs-primary: #0d6efd;
  --bs-secondary: #6c757d;
}

/* color-mix — blend colors */
.alert-soft {
  background-color: color-mix(in srgb, var(--bs-primary), white 85%);
  border-color: color-mix(in srgb, var(--bs-primary), white 70%);
  color: var(--bs-primary);
}

/* light-dark() — automatic dark mode */
.card {
  background-color: light-dark(white, #1a1a2e);
  color: light-dark(#212529, #e0e0e0);
  border-color: light-dark(#dee2e6, #333355);
}

/* Relative color syntax — derive from primary */
:root {
  --primary-light: color(from var(--bs-primary) srgb calc(r + 0.3) calc(g + 0.3) calc(b + 0.3));
  --primary-dark: color(from var(--bs-primary) srgb calc(r - 0.2) calc(g - 0.2) calc(b - 0.2));
}
```

```html
<!-- light-dark() requires color-scheme declaration -->
<style>
  :root { color-scheme: light dark; }
</style>

<div class="alert-soft alert">
  Soft color-mixed alert
</div>

<div class="card p-3">
  Card adapts to color scheme automatically
</div>
```

```js
// Toggle color scheme programmatically
function toggleTheme() {
  const scheme = document.documentElement.style.colorScheme;
  document.documentElement.style.colorScheme = scheme === 'dark' ? 'light' : 'dark';
}
```

## Advanced Variations

Relative color syntax for dynamic opacity variants:

```css
:root {
  /* Generate 10%, 25%, 50%, 75% opacity variants of primary */
  --bs-primary-10: color(from var(--bs-primary) srgb r g b / 10%);
  --bs-primary-25: color(from var(--bs-primary) srgb r g b / 25%);
  --bs-primary-50: color(from var(--bs-primary) srgb r g b / 50%);
  --bs-primary-75: color(from var(--bs-primary) srgb r g b / 75%);
}

/* Complementary color */
:root {
  --bs-complementary: color(from var(--bs-primary) srgb calc(1 - r) calc(1 - g) calc(1 - b));
}
```

## Best Practices

1. Use `color-mix(in srgb, ...)` for Sass-less color blending.
2. Use `light-dark()` for automatic dark mode without media queries.
3. Declare `color-scheme: light dark` on `:root` for `light-dark()` to work.
4. Use relative color syntax to generate color scales from a single base color.
5. Prefer `oklch` color space for perceptually uniform color mixing.
6. Use `color-mix()` for hover/focus states: `color-mix(in srgb, var(--bs-primary), black 15%)`.
7. Generate opacity variants with relative color syntax instead of Bootstrap's RGB workaround.
8. Test color combinations against WCAG contrast ratios at every mix percentage.
9. Use `color-mix()` for disabled states: `color-mix(in srgb, var(--bs-primary), gray 60%)`.
10. Combine with `prefers-color-scheme` for users who haven't explicitly chosen a theme.
11. Provide fallback values for browsers without support.
12. Use consistent color space (`srgb`, `oklch`, `lab`) across the project.

## Common Pitfalls

1. **Browser support** — `color-mix()` in Chrome 111+, `light-dark()` in Chrome 123+, relative color syntax in Chrome 119+.
2. **Color space confusion** — `in srgb` vs `in oklch` produces different blending results.
3. **Missing `color-scheme`** — `light-dark()` returns the light value without `color-scheme: light dark`.
4. **Alpha channel handling** — `color-mix()` with already-transparent colors may produce unexpected results.
5. **Fallback strategy** — Without `@supports` fallback, older browsers show no color at all.
6. **Sass conflict** — Sass may try to parse CSS color functions; use `@use 'sass:math'` or escape them.

## Accessibility Considerations

Always verify contrast ratios after mixing colors. `color-mix()` can reduce contrast below WCAG minimums. Use tools or calculated checks:

```css
/* Ensure text on mixed background meets AA contrast */
.alert-soft {
  /* Mix must maintain 4.5:1 contrast ratio */
  background-color: color-mix(in srgb, var(--bs-primary), white 85%);
  color: color-mix(in srgb, var(--bs-primary), black 30%);
}
```

## Responsive Behavior

Color functions work at all viewport sizes. Use them to create responsive themes that adjust color intensity based on ambient light sensors (future) or container queries for context-dependent theming.
