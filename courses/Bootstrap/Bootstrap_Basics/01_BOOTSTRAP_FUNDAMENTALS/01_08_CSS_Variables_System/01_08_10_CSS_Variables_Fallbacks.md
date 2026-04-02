---
title: CSS Variables Fallbacks and Browser Support
category: Bootstrap Fundamentals
difficulty: 2
time: 20 min
tags: bootstrap5, css-variables, fallback, var(), browser-support, graceful-degradation
---

## Overview

The CSS `var()` function accepts a fallback value as its second argument, providing a safety net when a CSS custom property is undefined or unsupported. Bootstrap 5 leverages CSS custom properties extensively, and understanding fallback strategies is essential for building resilient themes. The syntax `var(--bs-custom-property, fallback-value)` ensures that elements render correctly even when variables are not available. While modern browsers (all current versions) fully support CSS variables, fallbacks remain important for providing default values when variables are overridden or accidentally removed.

## Basic Implementation

The fallback syntax provides a value when the variable is undefined.

```html
<!-- Basic fallback usage -->
<style>
  .custom-element {
    /* Uses --bs-primary if defined, otherwise falls back to #0d6efd */
    color: var(--bs-primary, #0d6efd);
    background-color: var(--bs-primary-bg-subtle, #cfe2ff);
    border-color: var(--bs-primary-border-subtle, #9ec5fe);
    padding: var(--bs-spacer-3, 1rem);
    border-radius: var(--bs-border-radius, 0.375rem);
  }
</style>

<div class="custom-element">
  Styled with CSS variables and fallback values
</div>
```

Fallbacks work for any CSS property that accepts a value.

```html
<!-- Fallbacks across different properties -->
<style>
  .fallback-examples {
    /* Color fallback */
    color: var(--bs-emphasis-color, #000000);

    /* Spacing fallback */
    margin: var(--bs-spacer-4, 1.5rem);
    padding: var(--bs-spacer-3, 1rem);

    /* Typography fallback */
    font-family: var(--bs-body-font-family, system-ui, sans-serif);
    font-size: var(--bs-body-font-size, 1rem);
    line-height: var(--bs-body-line-height, 1.5);

    /* Border fallback */
    border: var(--bs-border-width, 1px) solid var(--bs-border-color, #dee2e6);
    border-radius: var(--bs-border-radius-lg, 0.5rem);
  }
</style>

<div class="fallback-examples">
  All properties have explicit fallback values
</div>
```

## Advanced Variations

Chaining multiple fallback levels for complex theming scenarios.

```html
<style>
  /* Nested var() for cascading fallbacks */
  .themed-card {
    background-color: var(--card-bg, var(--bs-body-bg, #ffffff));
    color: var(--card-text, var(--bs-body-color, #212529));
    border-color: var(--card-border, var(--bs-border-color, #dee2e6));
    border-radius: var(--card-radius, var(--bs-border-radius-lg, 0.5rem));
    padding: var(--card-padding, var(--bs-spacer-4, 1.5rem));
  }
</style>

<div class="themed-card">
  Card with nested fallback chain: card-specific variable, then Bootstrap variable, then hardcoded value
</div>
```

Using fallbacks for feature detection and progressive enhancement.

```html
<style>
  /* Graceful degradation approach */
  .enhanced-element {
    /* Fallback for browsers without CSS variable support */
    background-color: #0d6efd;
    /* Enhanced version using CSS variable */
    background-color: var(--bs-primary, #0d6efd);
    color: #ffffff;
    color: var(--bs-white, #ffffff);
    padding: 1rem;
    padding: var(--bs-spacer-3, 1rem);
  }

  /* Custom property with fallback for undefined variables */
  .dynamic-theme {
    background: var(--custom-bg, var(--bs-body-bg, #fff));
    color: var(--custom-text, var(--bs-body-color, #212529));
    transition: background-color 0.3s ease, color 0.3s ease;
  }
</style>
```

Building resilient component styles with comprehensive fallbacks.

```html
<style>
  .resilient-button {
    display: inline-block;
    padding: var(--bs-btn-padding-y, 0.375rem) var(--bs-btn-padding-x, 0.75rem);
    font-family: var(--bs-btn-font-family, inherit);
    font-size: var(--bs-btn-font-size, 1rem);
    font-weight: var(--bs-btn-font-weight, 400);
    line-height: var(--bs-btn-line-height, 1.5);
    color: var(--bs-btn-color, #fff);
    background-color: var(--bs-btn-bg, #0d6efd);
    border: var(--bs-btn-border-width, 1px) solid var(--bs-btn-border-color, transparent);
    border-radius: var(--bs-btn-border-radius, 0.375rem);
    cursor: pointer;
    transition: color 0.15s, background-color 0.15s, border-color 0.15s;
  }

  .resilient-button:hover {
    color: var(--bs-btn-hover-color, #fff);
    background-color: var(--bs-btn-hover-bg, #0b5ed7);
    border-color: var(--bs-btn-hover-border-color, #0a58ca);
  }
</style>

<button class="resilient-button">Resilient Button with Full Fallbacks</button>
```

## Best Practices

1. **Always provide fallbacks in custom CSS** - Use `var(--bs-*, fallback)` whenever referencing Bootstrap variables in your own stylesheets.
2. **Use meaningful fallback values** - Fallbacks should be sensible defaults that maintain the design, not arbitrary values.
3. **Match fallback units to variable units** - If the variable uses `rem`, the fallback should also use `rem` for consistency.
4. **Nest fallbacks for layered theming** - Chain `var()` calls for multi-level fallback hierarchies: component variable, then global variable, then hardcoded.
5. **Test with variables undefined** - Temporarily remove variable declarations to verify fallback behavior during development.
6. **Document fallback values** - Record what each fallback is and why it was chosen, especially for complex nested fallbacks.
7. **Keep fallback values in sync** - When updating Bootstrap, verify that hardcoded fallbacks still match Bootstrap's defaults.
8. **Use fallbacks for custom variables** - When defining your own CSS variables, always provide fallbacks where they are consumed.
9. **Prefer fallbacks over `@supports`** - The `var()` fallback mechanism is simpler and more maintainable than `@supports` queries for variable support.
10. **Include generic font families** - When using `--bs-body-font-family`, provide `sans-serif` or `serif` as the ultimate fallback.

## Common Pitfalls

1. **Forgetting fallbacks entirely** - Referencing `var(--bs-primary)` without a fallback in custom CSS breaks silently if the variable is undefined.
2. **Wrong fallback syntax** - The fallback goes inside the `var()` parentheses: `var(--name, fallback)`, not after it.
3. **Fallback unit mismatch** - Using `var(--bs-spacer-3, 16px)` when the variable is `1rem` creates inconsistency if the root font size changes.
4. **Stale fallback values** - Hardcoded fallbacks that matched Bootstrap 5.0 may not match 5.3 defaults after an upgrade.
5. **Over-relying on fallbacks** - If many properties need fallbacks, the CSS variable system may not be set up correctly. Investigate the root cause.

## Accessibility Considerations

CSS variable fallbacks ensure that content remains accessible even when theming fails. If a custom theme sets `--bs-body-color` to a low-contrast value, the fallback to Bootstrap's default ensures minimum readability. When building high-contrast themes, provide fallbacks that already meet WCAG contrast requirements. Test your application with CSS custom properties disabled to verify that the fallback experience remains fully accessible. Users with custom stylesheets or browser extensions that override CSS variables should receive a functional, readable experience through well-chosen fallback values.

## Responsive Behavior

CSS variable fallbacks are static values and do not change with viewport size. For responsive fallback strategies, define variable overrides within media queries and use different fallback values at each breakpoint if needed. For example, a spacing variable might fall back to `1rem` on mobile and `2rem` on desktop. The fallback mechanism works identically at all viewport sizes; the responsive behavior comes from where the variable is defined, not from the fallback itself. Ensure that hardcoded fallbacks remain readable and functional across all device sizes.
