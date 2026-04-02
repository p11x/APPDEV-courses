---
title: Bootstrap CSS Variables Overview
category: Bootstrap Fundamentals
difficulty: 2
time: 25 min
tags: bootstrap5, css-variables, custom-properties, theming, --bs-prefix
---

## Overview

Bootstrap 5 extensively uses CSS custom properties (CSS variables) prefixed with `--bs-` throughout its codebase. These variables provide a consistent theming system that allows developers to customize Bootstrap's appearance without recompiling Sass. CSS variables cascade through the DOM, can be overridden at any level, and can be modified at runtime with JavaScript. Understanding how Bootstrap's CSS variable system works is essential for effective theming, component customization, and building dynamic interfaces that support user preferences like dark mode.

## Basic Implementation

Bootstrap CSS variables follow the `--bs-{category}-{variant}` naming pattern and are accessible via the `var()` function.

```html
<!-- Using Bootstrap CSS variables in custom styles -->
<style>
  .custom-box {
    background-color: var(--bs-primary);
    color: var(--bs-white);
    padding: var(--bs-spacer-3);
    border-radius: var(--bs-border-radius);
  }
</style>

<div class="custom-box">
  Styled entirely with Bootstrap CSS variables
</div>
```

Inspecting CSS variables in browser DevTools reveals the full set of available values.

```html
<!-- Elements demonstrating inherited CSS variables -->
<div class="bg-primary text-white p-3 mb-3">
  Primary background uses --bs-primary
</div>

<div class="bg-body-secondary p-3 mb-3">
  Body secondary uses --bs-body-bg-rgb
</div>

<div class="border p-3" style="border-color: var(--bs-border-color);">
  Border using --bs-border-color variable
</div>
```

CSS variables can be queried and modified with JavaScript.

```html
<!-- Reading CSS variables with JavaScript -->
<script>
  // Read a CSS variable value
  const primaryColor = getComputedStyle(document.documentElement)
    .getPropertyValue('--bs-primary');
  console.log('Primary color:', primaryColor);

  // Read RGB variant
  const primaryRGB = getComputedStyle(document.documentElement)
    .getPropertyValue('--bs-primary-rgb');
  console.log('Primary RGB:', primaryRGB);
</script>
```

## Advanced Variables

Overriding CSS variables at different DOM scopes allows localized theming.

```html
<!-- Override CSS variables on a specific section -->
<style>
  .dark-section {
    --bs-body-bg: #212529;
    --bs-body-color: #f8f9fa;
    --bs-emphasis-color: #ffffff;
    --bs-border-color: #495057;
  }
</style>

<div class="dark-section p-4">
  <h3>Dark Themed Section</h3>
  <p>This section overrides body-level CSS variables for a dark appearance.</p>
  <button class="btn btn-primary">Themed Button</button>
</div>
```

CSS variables cascade, so overriding a parent variable affects all children.

```html
<!-- Scoped variable overrides -->
<style>
  .brand-section {
    --bs-primary: #ff6600;
    --bs-primary-rgb: 255, 102, 0;
  }
</style>

<div class="brand-section">
  <button class="btn btn-primary">Orange Primary Button</button>
  <div class="alert alert-primary mt-2">Alert with custom primary color</div>
</div>

<!-- Outside the scope, default primary applies -->
<button class="btn btn-primary">Standard Blue Button</button>
```

## Best Practices

1. **Use `var(--bs-*)` instead of hardcoded values** - Reference Bootstrap's CSS variables for colors, spacing, and typography to maintain consistency.
2. **Override at the root for global theming** - Set CSS variable overrides on `:root` or `body` to affect the entire application.
3. **Use RGB variants for rgba()** - Bootstrap provides `*-rgb` variables (e.g., `--bs-primary-rgb`) for use with `rgba()` for opacity control.
4. **Inspect variables in DevTools** - Use the browser's computed styles panel to discover available CSS variables and their current values.
5. **Scope overrides to specific sections** - Override variables on wrapper elements rather than globally for section-specific theming.
6. **Combine with utility classes** - CSS variables work seamlessly with Bootstrap utility classes for rapid prototyping.
7. **Document custom overrides** - Maintain a reference of all CSS variable overrides in your project's design system documentation.
8. **Use fallback values** - Provide fallbacks with `var(--bs-primary, #0d6efd)` for browser compatibility.
9. **Avoid removing Bootstrap's variable declarations** - Stripping CSS variable declarations from Bootstrap's output breaks components that depend on them.
10. **Leverage variables for dark mode** - Override `--bs-body-bg` and `--bs-body-color` to implement dark mode without modifying component CSS.

## Common Pitfalls

1. **Missing `-rgb` suffix for rgba** - Using `--bs-primary` in an `rgba()` function fails. Use `--bs-primary-rgb` instead.
2. **Assuming variables exist on all elements** - Some CSS variables are only defined on specific components. Check the source before referencing.
3. **Overriding without cascade understanding** - Variables override at the point of declaration. Later declarations in the cascade win.
4. **Typos in variable names** - CSS variable names are case-sensitive and typos silently fail. Use DevTools to copy exact names.
5. **Browser compatibility confusion** - CSS variables are widely supported but older browsers (IE11) do not support them. Bootstrap 5 dropped IE support.

## Accessibility Considerations

CSS variables themselves do not affect accessibility, but the values assigned to them do. When overriding color variables, ensure sufficient contrast ratios are maintained (at least 4.5:1 for normal text, 3:1 for large text per WCAG). Theme switching via CSS variables should respect the `prefers-reduced-motion` and `prefers-color-scheme` media queries. Test themed interfaces with screen readers and high contrast modes to ensure they remain usable.

## Responsive Behavior

CSS variables are static values and do not change with viewport size by default. However, you can define CSS variable overrides within media queries to create responsive theming. For example, override `--bs-body-font-size` at different breakpoints to scale typography. Bootstrap's responsive utilities reference CSS variables that already account for responsive behavior, so using them within responsive classes maintains consistency across breakpoints.
