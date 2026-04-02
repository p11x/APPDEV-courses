---
title: Color CSS Variables
category: Bootstrap Fundamentals
difficulty: 2
time: 20 min
tags: bootstrap5, css-variables, colors, theming, rgba, dynamic-colors
---

## Overview

Bootstrap 5 exposes all theme colors as CSS custom properties, enabling dynamic color manipulation without recompiling Sass. Each semantic color (primary, secondary, success, danger, warning, info, light, dark) has both a direct variable (`--bs-primary`) and an RGB variant (`--bs-primary-rgb`). The RGB variants are particularly powerful because they enable rgba() usage with dynamic opacity. Additionally, contextual text colors (`--bs-*-text-emphasis`), background colors (`--bs-*-bg-subtle`), and border colors (`--bs-*-border-subtle`) provide consistent color application across all component states.

## Basic Implementation

Access Bootstrap's color CSS variables directly in custom styles.

```html
<style>
  .custom-primary-box {
    background-color: var(--bs-primary);
    color: var(--bs-white);
    padding: 1rem;
    border-radius: var(--bs-border-radius);
  }

  .custom-success-border {
    border: 2px solid var(--bs-success);
    padding: 1rem;
  }
</style>

<div class="custom-primary-box mb-3">Primary colored box</div>
<div class="custom-success-border">Success border box</div>
```

Using RGB variants for opacity control.

```html
<!-- RGBA usage with RGB variables -->
<style>
  .primary-subtle {
    background-color: rgba(var(--bs-primary-rgb), 0.1);
    color: var(--bs-primary);
    padding: 1rem;
  }

  .danger-overlay {
    background-color: rgba(var(--bs-danger-rgb), 0.25);
    border: 1px solid rgba(var(--bs-danger-rgb), 0.5);
    padding: 1rem;
  }
</style>

<div class="primary-subtle mb-3">10% opacity primary background</div>
<div class="danger-overlay">25% opacity danger overlay</div>
```

Reading color variables with JavaScript.

```html
<!-- JavaScript color variable access -->
<script>
  const styles = getComputedStyle(document.documentElement);
  const colors = {
    primary: styles.getPropertyValue('--bs-primary'),
    primaryRGB: styles.getPropertyValue('--bs-primary-rgb'),
    success: styles.getPropertyValue('--bs-success'),
    danger: styles.getPropertyValue('--bs-danger'),
    warning: styles.getPropertyValue('--bs-warning'),
  };
  console.log(colors);
  // Output: { primary: '#0d6efd', primaryRGB: '13, 110, 253', ... }
</script>
```

## Advanced Variations

Bootstrap provides subtle and emphasis color variants for each semantic color.

```html
<style>
  /* Using all color variants for a single semantic color */
  .custom-success-theme {
    --my-bg: var(--bs-success-bg-subtle);
    --my-color: var(--bs-success-text-emphasis);
    --my-border: var(--bs-success-border-subtle);

    background-color: var(--my-bg);
    color: var(--my-color);
    border: 1px solid var(--my-border);
    padding: 1rem;
    border-radius: var(--bs-border-radius);
  }
</style>

<div class="custom-success-theme">
  Uses success subtle background, emphasis text, and subtle border
</div>
```

Dynamic color manipulation with CSS variables.

```html
<!-- Color manipulation through variable overrides -->
<style>
  .theme-wrapper {
    --bs-primary: #6f42c1;
    --bs-primary-rgb: 111, 66, 193;
    --bs-primary-text-emphasis: #45267a;
    --bs-primary-bg-subtle: #f3e8ff;
    --bs-primary-border-subtle: #d4bffc;
  }
</style>

<div class="theme-wrapper">
  <button class="btn btn-primary">Purple Primary</button>
  <div class="alert alert-primary mt-2">Purple themed alert</div>
  <div class="text-primary">Purple emphasis text</div>
</div>
```

## Best Practices

1. **Use RGB variants for opacity** - Always use `--bs-primary-rgb` with `rgba()` for transparent overlays instead of hex colors with manual opacity.
2. **Override all color shades together** - When changing a semantic color, update the base, RGB, subtle, emphasis, and border variants for consistency.
3. **Maintain contrast ratios** - Custom color overrides must preserve WCAG contrast requirements for text readability.
4. **Use semantic color variables** - Reference `--bs-primary` instead of hardcoded hex values so theme changes propagate automatically.
5. **Store color overrides in CSS** - Define custom color variables in a dedicated CSS file rather than inline styles for maintainability.
6. **Test all color states** - Verify that overridden colors work correctly in text, backgrounds, borders, and hover/focus states.
7. **Use CSS variables in custom components** - Apply `var(--bs-*)` color variables to custom components to ensure they match the Bootstrap theme.
8. **Document color customizations** - Maintain a color palette reference showing which CSS variables have been overridden and their new values.
9. **Leverage subtle variants for backgrounds** - Use `--bs-*-bg-subtle` for light-tinted backgrounds that pair with their emphasis text colors.
10. **Consider dark mode compatibility** - Ensure custom color overrides provide adequate contrast in both light and dark themes.

## Common Pitfalls

1. **Using hex in rgba()** - Passing `--bs-primary` (hex) to `rgba()` produces invalid CSS. Use the `-rgb` variant instead.
2. **Forgetting to override related variables** - Changing `--bs-primary` without updating `--bs-primary-rgb`, `--bs-primary-bg-subtle`, etc., creates inconsistent theming.
3. **Opacity calculation errors** - When using RGB variables, ensure the alpha value is a decimal (0.1) not a percentage (10%).
4. **Overriding too late in cascade** - CSS variables must be overridden before they are referenced. Ensure overrides load before Bootstrap's component CSS.
5. **Missing RGB values for custom colors** - When adding custom semantic colors, provide both the hex and RGB variants for full compatibility.

## Accessibility Considerations

Color variable overrides directly impact text readability and UI distinguishability. When customizing colors, verify contrast ratios using tools like the WebAIM Contrast Checker. Ensure that information is not conveyed through color alone. Bootstrap's color system includes default contrast-safe pairings, but custom overrides may break these. Test with Windows High Contrast Mode and ensure that overridden colors degrade gracefully. Provide additional visual indicators (icons, underlines) for interactive elements to support color-blind users.

## Responsive Behavior

CSS color variables do not change with viewport size. To implement responsive color changes, define variable overrides within media queries. For example, a section might use a lighter background color on mobile for better outdoor readability and switch to a darker tone on desktop. This is achieved by setting `--bs-body-bg` within `@media (min-width: 768px)` rules. However, dramatic color shifts between breakpoints can be disorienting; use responsive color changes sparingly and only for functional purposes like improving readability.
