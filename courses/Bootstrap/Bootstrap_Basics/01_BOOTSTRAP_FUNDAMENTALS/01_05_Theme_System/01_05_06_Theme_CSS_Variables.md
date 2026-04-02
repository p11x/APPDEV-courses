---
tags:
  - bootstrap
  - css-variables
  - custom-properties
  - theming
  - runtime
category: Bootstrap Fundamentals
difficulty: 2
time: 45 minutes
---

# Theme CSS Variables

## Overview

Bootstrap 5 exposes its entire color system as CSS custom properties (also called CSS variables). Every contextual color — primary, secondary, success, danger, warning, info, light, dark — is available as `--bs-primary`, `--bs-secondary`, and so on. These variables enable runtime theming without recompilation, dynamic color switching via JavaScript, and fine-grained control over color application.

CSS custom properties differ from Sass variables in a fundamental way: Sass variables are compile-time constants that bake values into the output CSS. CSS custom properties are runtime values that can be read, written, and modified in the browser. This means you can change `--bs-primary` in the browser and every element referencing it updates instantly.

Bootstrap generates three forms of each color. The base variable (`--bs-primary`) holds the hex value. The RGB variant (`--bs-primary-rgb`) holds comma-separated RGB values for use with `rgba()`. The RGB variant is what powers the opacity system — `rgba(var(--bs-primary-rgb), 0.5)` produces a semi-transparent primary color.

Understanding CSS custom property fallbacks is important. When Bootstrap's variables are unavailable (e.g., in a non-Bootstrap context), the fallback syntax `var(--bs-primary, #0d6efd)` ensures a sensible default. You can use this pattern in your own CSS to gracefully degrade.

Inspecting CSS variables in browser DevTools is a core debugging skill. The Computed tab shows all custom properties applied to an element, and you can modify them live to preview theme changes without touching source files.

## Basic Implementation

Bootstrap's CSS variables are available on the root element and can be used in any CSS rule:

```css
.custom-element {
  background-color: var(--bs-primary);
  color: var(--bs-white);
  border-color: var(--bs-primary);
}

.custom-card {
  background-color: var(--bs-body-bg);
  color: var(--bs-body-color);
  border: 1px solid var(--bs-border-color);
  border-radius: var(--bs-border-radius);
}
```

All eight contextual colors are available:

```css
.color-demo {
  --color-primary: var(--bs-primary);
  --color-secondary: var(--bs-secondary);
  --color-success: var(--bs-success);
  --color-danger: var(--bs-danger);
  --color-warning: var(--bs-warning);
  --color-info: var(--bs-info);
  --color-light: var(--bs-light);
  --color-dark: var(--bs-dark);
}
```

Using RGB variants for rgba() construction:

```css
.overlay {
  background-color: rgba(var(--bs-primary-rgb), 0.75);
}

.text-overlay {
  color: rgba(var(--bs-body-color-rgb), 0.65);
}

.border-subtle {
  border-color: rgba(var(--bs-danger-rgb), 0.25);
}
```

Bootstrap utility classes use these variables internally:

```html
<!-- These classes resolve to CSS variable references -->
<div class="bg-primary">Uses var(--bs-primary)</div>
<div class="text-danger">Uses var(--bs-danger)</div>
<button class="btn btn-success">Uses var(--bs-success)</button>
```

Fallback values provide graceful degradation:

```css
.safe-element {
  /* Falls back to Bootstrap's default blue if --bs-primary is undefined */
  color: var(--bs-primary, #0d6efd);
  background: var(--bs-body-bg, #ffffff);
}
```

## Advanced Variations

Runtime theme switching via JavaScript:

```javascript
const setTheme = (colors) => {
  const root = document.documentElement;
  Object.entries(colors).forEach(([key, value]) => {
    root.style.setProperty(`--bs-${key}`, value);
    // Also update RGB variant
    const rgb = hexToRgb(value);
    if (rgb) {
      root.style.setProperty(`--bs-${key}-rgb`, `${rgb.r}, ${rgb.g}, ${rgb.b}`);
    }
  });
};

const hexToRgb = (hex) => {
  const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
  return result ? {
    r: parseInt(result[1], 16),
    g: parseInt(result[2], 16),
    b: parseInt(result[3], 16),
  } : null;
};

// Apply a custom theme
setTheme({
  primary: '#6c5ce7',
  secondary: '#a29bfe',
  success: '#00b894',
  danger: '#d63031',
});
```

Component-scoped variable overrides:

```html
<div style="--bs-primary: #e17055; --bs-primary-rgb: 225, 112, 85;">
  <button class="btn btn-primary">Orange Primary</button>
  <div class="alert alert-primary mt-2">Orange alert</div>
</div>
```

CSS-based theming with data attributes:

```css
[data-theme="ocean"] {
  --bs-primary: #0984e3;
  --bs-primary-rgb: 9, 132, 227;
  --bs-secondary: #6c5ce7;
  --bs-secondary-rgb: 108, 92, 231;
}

[data-theme="forest"] {
  --bs-primary: #00b894;
  --bs-primary-rgb: 0, 184, 148;
  --bs-secondary: #55a3ff;
  --bs-secondary-rgb: 85, 163, 255;
}

[data-theme="sunset"] {
  --bs-primary: #e17055;
  --bs-primary-rgb: 225, 112, 85;
  --bs-secondary: #fdcb6e;
  --bs-secondary-rgb: 253, 203, 110;
}
```

```html
<html lang="en" data-theme="ocean">
<body>
  <button class="btn btn-primary">Ocean Primary</button>
  <button class="btn btn-secondary">Ocean Secondary</button>

  <script>
    document.querySelectorAll('[data-theme-switch]').forEach(btn => {
      btn.addEventListener('click', () => {
        document.documentElement.setAttribute('data-theme', btn.dataset.themeSwitch);
      });
    });
  </script>
</body>
</html>
```

Inheriting and extending Bootstrap variables:

```css
:root {
  /* Extend with custom variables using the same pattern */
  --bs-accent: #e17055;
  --bs-accent-rgb: 225, 112, 85;
  --bs-surface: #f8f9fa;
  --bs-surface-rgb: 248, 249, 250;
}

.custom-accent {
  background-color: var(--bs-accent);
  color: var(--bs-white);
}

.custom-accent-subtle {
  background-color: rgba(var(--bs-accent-rgb), 0.1);
  color: var(--bs-accent);
}
```

Inspecting variables in DevTools:

```javascript
// List all Bootstrap CSS variables on :root
const styles = getComputedStyle(document.documentElement);
const allVars = Array.from(styles)
  .filter(prop => prop.startsWith('--bs-'))
  .map(prop => `${prop}: ${styles.getPropertyValue(prop).trim()}`);

console.table(allVars);
```

## Best Practices

1. **Use CSS variables instead of hardcoded colors in custom components.** `var(--bs-primary)` adapts to theme changes. Hardcoded hex values do not.

2. **Always update both base and RGB variants.** When changing `--bs-primary` via JavaScript, also update `--bs-primary-rgb`. The RGB variant powers opacity utilities.

3. **Provide fallback values in custom CSS.** `var(--bs-primary, #0d6efd)` ensures your styles work even if Bootstrap's variables are not loaded.

4. **Scope variable overrides to the narrowest possible selector.** Component-level overrides (`style="--bs-primary: red"`) prevent unintended side effects on sibling elements.

5. **Use CSS variables for spacing and sizing too.** Bootstrap exposes `--bs-border-radius`, `--bs-border-color`, `--bs-body-bg`, and more. Leverage these for consistent custom components.

6. **Inspect variables in DevTools before debugging.** Open the Computed panel, filter by `--bs-`, and verify the values. Incorrect variable values are the most common cause of theming issues.

7. **Define custom variables with the `--bs-` prefix for consistency.** `--bs-my-custom-color` integrates naturally with Bootstrap's namespace. Avoid generic names like `--color` that may conflict.

8. **Use `var()` in combination with `calc()` for dynamic sizing.** `calc(var(--bs-border-radius) * 2)` creates proportional values without hardcoding.

9. **Document custom variable additions in a design token file.** List every custom `--bs-*` variable with its purpose, default value, and usage context.

10. **Test variable overrides in both light and dark modes.** A variable that works in light mode may produce unexpected results in dark mode if the value does not adapt.

11. **Avoid setting CSS variables with `!important`.** CSS custom properties follow cascade rules. Using `!important` on variable declarations creates maintenance problems.

12. **Use feature detection for browsers that lack custom property support.** All modern browsers support CSS custom properties, but if supporting IE11, provide fallback declarations.

## Common Pitfalls

1. **Changing `--bs-primary` without updating `--bs-primary-rgb`.** The opacity system (`bg-opacity-*`) reads from the RGB variant. If the RGB value is stale, opacity utilities display wrong colors.

2. **Expecting CSS variables to work in all CSS contexts.** CSS custom properties cannot be used in media queries, `@keyframes` names, or `@import` URLs. They only work in property values.

3. **Using `var()` without a fallback in non-Bootstrap contexts.** If Bootstrap's CSS is not loaded, `var(--bs-primary)` resolves to the initial value (transparent for colors). Always provide a fallback.

4. **Overriding variables on the wrong element.** Setting `--bs-primary` on a `<div>` only affects that `<div>` and its children. Setting it on `:root` or `<html>` affects the entire page.

5. **Forgetting to include both hex and RGB values.** Some code only sets `--bs-primary` and forgets `--bs-primary-rgb`, breaking every utility that uses `rgba(var(--bs-primary-rgb), ...)`.

6. **Debugging the wrong element.** When a color looks wrong, inspect the element's computed custom properties, not the CSS rule. The variable might be overridden by a parent or by the dark mode attribute.

7. **Assuming CSS variables cascade like normal properties.** CSS custom properties inherit to children but are not global unless set on `:root`. A variable set on `.card` is not available outside `.card`'s descendants.

8. **Not refreshing the page after modifying variables via DevTools.** Some changes require a reflow. If the color does not update, trigger a layout change (e.g., resize the window).

## Accessibility Considerations

CSS custom properties do not affect accessibility directly, but they control colors that impact contrast. When overriding theme variables, verify that the new values maintain WCAG contrast ratios.

Changing `--bs-primary` to a lighter shade can reduce the contrast of primary text on white backgrounds. Changing `--bs-danger` to a muted red can make error messages less noticeable. Always test the full set of components after variable changes.

Focus indicators use CSS variables for their color. If `--bs-primary` is set to a color that blends with the background, keyboard focus becomes invisible:

```css
/* Test focus visibility after changing --bs-primary */
:focus-visible {
  outline: 2px solid var(--bs-focus-ring-color);
  outline-offset: 2px;
}
```

Ensure that variable changes are communicated to assistive technology through proper ARIA attributes. Visual changes through CSS variables are not announced by screen readers.

High contrast mode on Windows overrides CSS custom properties. Ensure your design degrades gracefully when users enable forced colors mode. Use `revert` or provide explicit `color` and `background-color` fallbacks.

## Responsive Behavior

CSS custom properties do not support media queries directly. You cannot write `@media (max-width: 768px) { :root { --bs-primary: red; } }` because custom properties inside media queries are not evaluated the way you might expect.

However, you can achieve responsive theming by using media queries to change which selector applies:

```css
/* Desktop theme */
:root {
  --bs-primary: #0d6efd;
}

/* Mobile overrides via class toggling */
.mobile-theme {
  --bs-primary: #6c5ce7;
  --bs-primary-rgb: 108, 92, 231;
}
```

```javascript
const applyResponsiveTheme = () => {
  const root = document.documentElement;
  if (window.innerWidth < 768) {
    root.classList.add('mobile-theme');
  } else {
    root.classList.remove('mobile-theme');
  }
};

window.addEventListener('resize', applyResponsiveTheme);
applyResponsiveTheme();
```

A simpler approach uses separate stylesheets:

```html
<link rel="stylesheet" href="desktop-theme.css" media="(min-width: 768px)">
<link rel="stylesheet" href="mobile-theme.css" media="(max-width: 767px)">
```

Each stylesheet can define its own `:root` variables. The browser loads only the matching stylesheet, keeping the payload efficient.