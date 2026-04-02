---
title: Theme CSS Variables
category: Bootstrap Fundamentals
difficulty: 2
time: 25 min
tags: bootstrap5, css-variables, theming, dark-mode, body-bg, body-color
---

## Overview

Bootstrap 5's theme CSS variables control the fundamental color scheme of the entire page. Key variables include `--bs-body-bg` (page background), `--bs-body-color` (body text color), `--bs-emphasis-color` (high-emphasis text), and `--bs-secondary-color` (muted text). These variables cascade to every component, making them the primary mechanism for implementing light/dark theme switching. By overriding these root-level variables, you can transform the entire appearance of a Bootstrap site without modifying any component-level CSS.

## Basic Implementation

The core theme variables define the page's base appearance.

```html
<style>
  /* Default light theme variables (Bootstrap's defaults) */
  :root {
    --bs-body-bg: #fff;
    --bs-body-color: #212529;
    --bs-emphasis-color: #000;
    --bs-secondary-color: rgba(33, 37, 41, 0.75);
    --bs-secondary-bg: #e9ecef;
  }
</style>

<!-- Elements using theme variables -->
<div class="p-4" style="background-color: var(--bs-body-bg); color: var(--bs-body-color);">
  <h3 style="color: var(--bs-emphasis-color);">High Emphasis Heading</h3>
  <p>Body text styled with --bs-body-color</p>
  <p style="color: var(--bs-secondary-color);">Secondary muted text</p>
</div>
```

Override theme variables to create a dark mode.

```html
<!-- Dark theme override -->
<style>
  .dark-theme {
    --bs-body-bg: #212529;
    --bs-body-color: #dee2e6;
    --bs-emphasis-color: #fff;
    --bs-secondary-color: rgba(222, 226, 230, 0.75);
    --bs-secondary-bg: #343a40;
    --bs-tertiary-bg: #2d3238;
    --bs-border-color: #495057;
  }
</style>

<div class="dark-theme p-4">
  <h3>Dark Themed Content</h3>
  <p>Body text adapts to the dark background.</p>
  <div class="card">
    <div class="card-body">
      Cards also inherit the dark theme variables.
    </div>
  </div>
</div>
```

## Advanced Variations

Building a complete theme switcher using CSS variables.

```html
<style>
  [data-theme="dark"] {
    --bs-body-bg: #121212;
    --bs-body-color: #e0e0e0;
    --bs-emphasis-color: #ffffff;
    --bs-secondary-color: rgba(224, 224, 224, 0.7);
    --bs-secondary-bg: #1e1e1e;
    --bs-tertiary-bg: #252525;
    --bs-body-bg-rgb: 18, 18, 18;
    --bs-body-color-rgb: 224, 224, 224;
    --bs-border-color: #333333;
    --bs-link-color: #90caf9;
    --bs-link-hover-color: #bbdefb;
  }

  [data-theme="warm"] {
    --bs-body-bg: #fdf6e3;
    --bs-body-color: #5c4a32;
    --bs-emphasis-color: #3c2f1e;
    --bs-primary: #b58900;
    --bs-primary-rgb: 181, 137, 0;
  }
</style>

<div data-theme="warm" class="p-4">
  <h3>Warm Theme</h3>
  <p>Solarized-inspired warm color scheme.</p>
  <button class="btn btn-primary">Themed Button</button>
</div>
```

Combining theme variables with Bootstrap's data attributes for automatic dark mode detection.

```html
<!-- Respect system preference -->
<style>
  @media (prefers-color-scheme: dark) {
    :root:not([data-theme="light"]) {
      --bs-body-bg: #212529;
      --bs-body-color: #dee2e6;
      --bs-emphasis-color: #fff;
      --bs-secondary-color: rgba(222, 226, 230, 0.75);
      --bs-secondary-bg: #343a40;
    }
  }
</style>
```

## Best Practices

1. **Override at `:root` for global theming** - Set theme variables on `:root` to affect the entire document.
2. **Use `data-theme` attributes** - Scope themes with data attributes like `data-theme="dark"` for flexible switching.
3. **Override all related variables together** - When changing `--bs-body-bg`, also update `--bs-body-color`, `--bs-emphasis-color`, and `--bs-border-color`.
4. **Respect `prefers-color-scheme`** - Use the media query to detect the user's system theme preference.
5. **Provide a manual override** - Allow users to override system preference with an explicit theme toggle.
6. **Test all components** - Verify that every Bootstrap component looks correct with your custom theme variables.
7. **Use RGB variants for opacity** - Set `--bs-body-bg-rgb` alongside `--bs-body-bg` for rgba() compatibility.
8. **Update border colors** - Light borders on dark backgrounds are invisible. Always override `--bs-border-color` with theme changes.
9. **Maintain link colors** - Override `--bs-link-color` and `--bs-link-hover-color` to ensure links remain distinguishable.
10. **Store preference in localStorage** - Persist the user's theme choice across sessions.

## Common Pitfalls

1. **Incomplete theme overrides** - Changing only `--bs-body-bg` without updating text colors makes content unreadable on dark backgrounds.
2. **Hardcoded colors in components** - Custom components with hardcoded hex colors ignore theme variable overrides, breaking visual consistency.
3. **Flash of wrong theme** - Applying the theme after page load causes a visible flash. Use a blocking script or CSS-only approach.
4. **Not updating border colors** - Default Bootstrap borders are designed for light backgrounds. Dark themes require explicit border color updates.
5. **Forgetting secondary backgrounds** - Components like cards use `--bs-secondary-bg`. Missing this override leaves bright cards in a dark theme.

## Accessibility Considerations

Theme switching must maintain accessibility standards. Ensure sufficient contrast ratios in both light and dark modes (minimum 4.5:1 for body text). Respect the `prefers-reduced-motion` media query when animating theme transitions. Announce theme changes to screen readers using ARIA live regions. Provide a clearly visible theme toggle button that is keyboard accessible. Ensure that focus indicators remain visible in all themes. Test with assistive technologies to verify that themed content is fully navigable and readable.

## Responsive Behavior

Theme CSS variables do not inherently change with viewport size. However, you can use media queries to adjust theme variables at different breakpoints. For instance, a darker background might be preferable on mobile for better outdoor readability. Define responsive theme overrides within `@media` blocks targeting specific breakpoints. Combine theme variables with Bootstrap's responsive utilities to create layouts that adapt both structure and appearance across device sizes.
