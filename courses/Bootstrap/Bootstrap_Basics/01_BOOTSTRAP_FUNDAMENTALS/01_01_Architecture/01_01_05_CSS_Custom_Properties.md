---
tags: [bootstrap, css-custom-properties, theming, runtime, css-variables]
category: Bootstrap Fundamentals
difficulty: 2
estimated_time: 30 minutes
---

# CSS Custom Properties in Bootstrap 5

## Overview

Bootstrap 5 extensively uses CSS custom properties (CSS variables) prefixed with `--bs-*` to expose design tokens at runtime. Unlike Sass variables, which are compiled away during build time, CSS custom properties remain in the browser and can be modified dynamically through JavaScript, media queries, or additional CSS rules.

This runtime capability enables **dynamic theming** without recompilation — switching themes, adjusting for user preferences, or responding to contextual changes can happen entirely in the browser. CSS custom properties also cascade naturally through the DOM, allowing component-level overrides that don't affect siblings or ancestors.

Bootstrap exposes custom properties at multiple levels: global (`:root`), component-level (`.btn-primary`), and utility-level. The `--bs-*` prefix namespace prevents collisions with application-level CSS variables.

Understanding **custom property fallbacks** — the second argument in `var(--bs-primary, fallback)` — is essential for graceful degradation when custom properties aren't defined or supported.

Browser support for CSS custom properties is excellent across all modern browsers (Chrome 49+, Firefox 31+, Safari 9.1+, Edge 15+). Bootstrap 5 does not support IE11, so CSS custom properties are used freely throughout the framework.

```css
/* Bootstrap 5 exposes CSS custom properties on :root */
:root {
  --bs-primary: #0d6efd;
  --bs-primary-rgb: 13, 110, 253;
  --bs-secondary: #6c757d;
  --bs-secondary-rgb: 108, 117, 125;
  --bs-success: #198754;
  --bs-success-rgb: 25, 135, 84;
  --bs-info: #0dcaf0;
  --bs-warning: #ffc107;
  --bs-danger: #dc3545;
  --bs-light: #f8f9fa;
  --bs-dark: #212529;

  /* Typography */
  --bs-body-font-family: var(--bs-font-sans-serif);
  --bs-body-font-size: 1rem;
  --bs-body-font-weight: 400;
  --bs-body-line-height: 1.5;
  --bs-body-color: #212529;
  --bs-body-bg: #fff;

  /* Spacing */
  --bs-gutter-x: 1.5rem;
  --bs-gutter-y: 0;
}
```

## Basic Implementation

Using CSS custom properties for theming is straightforward. Override `--bs-*` variables on `:root` for global changes, or on specific selectors for scoped modifications.

```css
/* Global theme override */
:root {
  --bs-primary: #6f42c1;
  --bs-primary-rgb: 111, 66, 193;
  --bs-body-font-family: 'Inter', system-ui, sans-serif;
  --bs-body-bg: #f0f2f5;
  --bs-body-color: #1a1a2e;
}

/* Scoped theme for a section */
.promo-section {
  --bs-primary: #e83e8c;
  --bs-primary-rgb: 232, 62, 140;
  --bs-body-bg: #1a1a2e;
  --bs-body-color: #f0f2f5;
  padding: 3rem;
}

/* Dark mode using CSS custom properties */
[data-bs-theme="dark"] {
  --bs-body-bg: #212529;
  --bs-body-color: #dee2e6;
  --bs-primary: #375feb;
  --bs-secondary: #6c757d;
  --bs-tertiary-bg: #343a40;
}
```

```html
<!-- Toggling themes at runtime with JavaScript -->
<button id="themeToggle" class="btn btn-primary">Toggle Theme</button>

<div class="promo-section mt-3">
  <h2 class="text-primary">This uses the promo section's --bs-primary</h2>
  <button class="btn btn-primary">Scoped Primary Button</button>
</div>

<script>
  const toggle = document.getElementById('themeToggle');
  toggle.addEventListener('click', () => {
    const html = document.documentElement;
    const current = html.getAttribute('data-bs-theme');
    html.setAttribute('data-bs-theme', current === 'dark' ? 'light' : 'dark');
  });
</script>
```

Bootstrap components reference custom properties in their CSS rules. This means changing a custom property updates all components that use it simultaneously:

```css
/* How Bootstrap references custom properties internally */
.btn-primary {
  --bs-btn-color: #fff;
  --bs-btn-bg: var(--bs-primary);
  --bs-btn-border-color: var(--bs-primary);
  --bs-btn-hover-color: #fff;
  --bs-btn-hover-bg: shade-color(var(--bs-primary), 15%);
  --bs-btn-hover-border-color: shade-color(var(--bs-primary), 20%);
  --bs-btn-focus-shadow-rgb: var(--bs-primary-rgb);
  --bs-btn-active-color: #fff;
  --bs-btn-active-bg: shade-color(var(--bs-primary), 20%);
  --bs-btn-active-border-color: shade-color(var(--bs-primary), 25%);
}

/* Each button can have its own scoped custom properties */
.btn {
  --bs-btn-padding-x: 1rem;
  --bs-btn-padding-y: 0.375rem;
  --bs-btn-font-size: 1rem;
  --bs-btn-border-radius: var(--bs-border-radius);
}
```

## Advanced Variations

Advanced CSS custom property usage includes runtime theme generation, dynamic color manipulation, and responsive theming without JavaScript.

```css
/* Dynamic color generation using CSS custom properties */
:root {
  /* Define HSL components separately for maximum flexibility */
  --bs-primary-h: 230;
  --bs-primary-s: 75%;
  --bs-primary-l: 58%;
  --bs-primary: hsl(
    var(--bs-primary-h),
    var(--bs-primary-s),
    var(--bs-primary-l)
  );
  --bs-primary-rgb: 
    calc(var(--bs-primary-h)),
    calc(var(--bs-primary-s)),
    calc(var(--bs-primary-l));
}

/* Generate lighter/darker variants at runtime */
.btn-primary {
  --btn-bg: var(--bs-primary);
  --btn-hover-bg: hsl(
    var(--bs-primary-h),
    var(--bs-primary-s),
    calc(var(--bs-primary-l) - 10%)
  );
  --btn-active-bg: hsl(
    var(--bs-primary-h),
    var(--bs-primary-s),
    calc(var(--bs-primary-l) - 15%)
  );
  background-color: var(--btn-bg);
  border-color: var(--btn-bg);
}

.btn-primary:hover {
  background-color: var(--btn-hover-bg);
  border-color: var(--btn-hover-bg);
}
```

Responsive theming adapts styles based on viewport without JavaScript:

```css
/* Responsive theme adjustments */
:root {
  --bs-container-padding: 1rem;
  --bs-body-font-size: 0.875rem;
}

@media (min-width: 768px) {
  :root {
    --bs-container-padding: 1.5rem;
    --bs-body-font-size: 1rem;
  }
}

@media (min-width: 1200px) {
  :root {
    --bs-container-padding: 2rem;
    --bs-body-font-size: 1rem;
  }
}
```

Component-scoped custom properties enable per-instance customization without global impact:

```html
<!-- Per-component theme using scoped CSS custom properties -->
<div class="card" style="
  --bs-card-bg: #1a1a2e;
  --bs-card-color: #f0f2f5;
  --bs-card-border-color: #375feb;
">
  <div class="card-header">Custom Themed Card</div>
  <div class="card-body">
    <p class="card-text">This card uses inline custom properties.</p>
    <button class="btn btn-primary">Primary Action</button>
  </div>
</div>
```

```javascript
// JavaScript-powered dynamic theme system
class ThemeManager {
  constructor() {
    this.themes = {
      light: {
        '--bs-body-bg': '#ffffff',
        '--bs-body-color': '#212529',
        '--bs-primary': '#0d6efd',
        '--bs-primary-rgb': '13, 110, 253'
      },
      dark: {
        '--bs-body-bg': '#212529',
        '--bs-body-color': '#dee2e6',
        '--bs-primary': '#375feb',
        '--bs-primary-rgb': '55, 95, 235'
      },
      highContrast: {
        '--bs-body-bg': '#000000',
        '--bs-body-color': '#ffffff',
        '--bs-primary': '#ffff00',
        '--bs-primary-rgb': '255, 255, 0'
      }
    };
  }

  apply(themeName) {
    const theme = this.themes[themeName];
    if (!theme) return;
    
    Object.entries(theme).forEach(([property, value]) => {
      document.documentElement.style.setProperty(property, value);
    });
    
    document.documentElement.setAttribute('data-bs-theme', themeName);
    localStorage.setItem('bs-theme', themeName);
  }

  init() {
    const saved = localStorage.getItem('bs-theme') || 'light';
    this.apply(saved);
  }
}

const themeManager = new ThemeManager();
themeManager.init();
```

## Best Practices

- **Use CSS custom properties for runtime theming** and Sass variables for build-time configuration — each has its appropriate context.
- **Always provide fallback values** with `var(--bs-primary, #0d6efd)` for properties that may not be defined in certain contexts.
- **Scope custom property overrides** to specific selectors rather than overriding on `:root` when the change should only affect a component or section.
- **Leverage the `--bs-*-rgb` variables** for alpha transparency — `rgba(var(--bs-primary-rgb), 0.5)` enables runtime opacity adjustments.
- **Use `data-bs-theme` attribute** for theme switching instead of class toggling; Bootstrap 5.3+ natively supports this pattern.
- **Define custom properties on component selectors** (e.g., `.btn`, `.card`) to enable per-instance overrides via inline `style` attributes.
- **Respect the `prefers-color-scheme` media query** for automatic dark mode detection.
- **Keep custom property names within the `--bs-*` namespace** only for Bootstrap overrides; use a different prefix (`--app-*`, `--custom-*`) for application-specific variables.
- **Test custom property changes across all components** that reference the modified property to ensure visual consistency.
- **Use `setProperty()` for JavaScript-driven changes** rather than manipulating CSS text, for better performance and maintainability.
- **Document all custom property overrides** in a central theme file with comments explaining the design rationale.

## Common Pitfalls

- **Using `var()` inside Sass variables** — Sass processes at compile time before CSS custom properties exist. Use `var()` only in CSS output, not in Sass variable definitions.
- **Forgetting the fallback value** in `var(--bs-primary, blue)` — if the custom property isn't defined and no fallback exists, the property reverts to its initial/inherited value, which may be unexpected.
- **Overriding `:root` custom properties inside media queries** — this works for responsive theming but can conflict with JavaScript-set values if not managed carefully.
- **Expecting CSS custom properties to work in `@media` queries** — media queries evaluate against viewport dimensions, not custom property values; use container queries or JavaScript for conditional logic based on custom properties.
- **Not handling the `--bs-*-rgb` variables** — components like modals and offcanvas use `rgba(var(--bs-body-bg-rgb), 0.5)` for backdrop transparency; omitting the `-rgb` variant breaks overlay effects.
- **Mixing unit types** — setting `--bs-body-font-size: 16` (no unit) instead of `--bs-body-font-size: 1rem` causes CSS parsing failures.
- **Assuming custom properties inherit everywhere** — custom properties don't cross shadow DOM boundaries; web components using Bootstrap need explicit property forwarding.

## Accessibility Considerations

CSS custom properties enable accessible theming patterns that respect user preferences:

```css
/* Respect user's motion preferences */
@media (prefers-reduced-motion: reduce) {
  :root {
    --bs-transition-duration: 0s;
    --bs-transition-timing: none;
  }
}

/* High contrast mode support */
@media (forced-colors: active) {
  :root {
    --bs-primary: LinkText;
    --bs-danger: Mark;
    --bs-body-color: CanvasText;
    --bs-body-bg: Canvas;
  }
}

/* Respect user's color scheme preference */
@media (prefers-color-scheme: dark) {
  [data-bs-theme="auto"] {
    --bs-body-bg: #212529;
    --bs-body-color: #dee2e6;
    --bs-primary: #375feb;
  }
}
```

- Ensure that custom property overrides maintain WCAG 2.1 contrast ratios (4.5:1 for normal text, 3:1 for large text).
- Use `prefers-contrast` media queries to provide high-contrast variants.
- Test theme switching with screen readers to verify that `data-bs-theme` changes are announced or don't disrupt the reading experience.
- Maintain keyboard focus visibility by ensuring `--bs-btn-focus-shadow-rgb` remains visible against all theme backgrounds.

## Responsive Behavior

CSS custom properties can be modified within media queries for responsive adjustments, and Bootstrap 5.3+ supports container queries for component-level responsiveness:

```css
/* Viewport-responsive custom properties */
:root {
  --bs-container-padding: 1rem;
  --bs-card-padding: 1rem;
  --bs-heading-size: 1.5rem;
}

@media (min-width: 576px) {
  :root {
    --bs-container-padding: 1.5rem;
    --bs-card-padding: 1.5rem;
  }
}

@media (min-width: 992px) {
  :root {
    --bs-container-padding: 2rem;
    --bs-card-padding: 2rem;
    --bs-heading-size: 2rem;
  }
}

/* Component-level responsive behavior with container queries */
@container (min-width: 400px) {
  .card {
    --bs-card-flex-direction: row;
    --bs-card-spacer-x: 1.5rem;
  }
}
```

```html
<!-- Combining responsive custom properties with Bootstrap grid -->
<div class="container">
  <div class="row">
    <div class="col-12 col-lg-6">
      <div class="card">
        <div class="card-body" 
             style="padding: var(--bs-card-padding, 1rem)">
          <h2 style="font-size: var(--bs-heading-size, 1.5rem)">
            Responsive Heading
          </h2>
          <p>Card padding and heading size respond to viewport.</p>
        </div>
      </div>
    </div>
  </div>
</div>
```
