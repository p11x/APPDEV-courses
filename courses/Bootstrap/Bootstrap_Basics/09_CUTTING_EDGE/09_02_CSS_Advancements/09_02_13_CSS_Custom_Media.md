---
title: CSS Custom Media Queries
category: [CSS Advancements, Cutting Edge]
difficulty: 3
time: 25 min
tags: bootstrap5, custom-media, @custom-media, breakpoints, responsive
---

## Overview

`@custom-media` defines named media queries, replacing hardcoded breakpoint values with semantic aliases. This improves maintainability when working with Bootstrap's breakpoints and enables consistent responsive rules across a project without duplicating pixel values.

## Basic Implementation

Defining custom media queries that mirror Bootstrap's breakpoint system.

```html
<style>
  @custom-media --sm (min-width: 576px);
  @custom-media --md (min-width: 768px);
  @custom-media --lg (min-width: 992px);
  @custom-media --xl (min-width: 1200px);
  @custom-media --xxl (min-width: 1400px);

  @custom-media --sm-down (max-width: 575.98px);
  @custom-media --md-down (max-width: 767.98px);
  @custom-media --lg-down (max-width: 991.98px);

  .hero-section {
    padding: 2rem 1rem;
    text-align: center;
  }

  @media (--md) {
    .hero-section {
      padding: 4rem 2rem;
      text-align: left;
    }
  }

  @media (--lg) {
    .hero-section {
      padding: 6rem 3rem;
    }
  }
</style>

<div class="hero-section">
  <h1>Custom Media Breakpoints</h1>
  <p>Named queries improve readability and consistency.</p>
</div>
```

## Advanced Variations

Combining custom media queries with component-specific breakpoints.

```html
<style>
  @custom-media --sm (min-width: 576px);
  @custom-media --md (min-width: 768px);
  @custom-media --lg (min-width: 992px);

  @custom-media --dark-mode (prefers-color-scheme: dark);
  @custom-media --reduced-motion (prefers-reduced-motion: reduce);
  @custom-media --high-contrast (prefers-contrast: high);

  .dashboard-grid {
    display: grid;
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  @media (--sm) {
    .dashboard-grid {
      grid-template-columns: repeat(2, 1fr);
    }
  }

  @media (--lg) {
    .dashboard-grid {
      grid-template-columns: repeat(4, 1fr);
    }
  }

  @media (--dark-mode) {
    .dashboard-grid {
      background: var(--bs-dark-bg);
      color: var(--bs-light);
    }
  }

  @media (--reduced-motion) {
    .dashboard-grid, .dashboard-grid * {
      transition: none !important;
      animation: none !important;
    }
  }

  @media (--high-contrast) {
    .dashboard-grid .card {
      border-width: 2px;
      border-color: currentColor;
    }
  }
</style>
```

Using boolean and combined custom media queries.

```html
<style>
  @custom-media --touch (hover: none);
  @custom-media --mouse (hover: hover);
  @custom-media --md (min-width: 768px);

  /* Touch devices on medium+ screens */
  @media (--touch) and (--md) {
    .nav-link {
      padding: 1rem;
      min-height: 44px;
    }
    .btn {
      padding: 0.75rem 1.5rem;
    }
  }

  /* Mouse devices */
  @media (--mouse) {
    .nav-link:hover {
      background: var(--bs-tertiary-bg);
    }
    .card:hover {
      transform: translateY(-2px);
    }
  }
</style>
```

## Best Practices

1. Define all custom media queries in a single `@custom-media` block at the top of the stylesheet
2. Name breakpoints semantically (e.g., `--sm`, `--md`) matching Bootstrap's system
3. Include both min-width and max-width variants for mobile-first and desktop-first patterns
4. Add user-preference custom media (`--dark-mode`, `--reduced-motion`) alongside viewport breakpoints
5. Use descriptive names that convey the breakpoint's purpose
6. Keep custom media definitions DRY — reference them everywhere instead of hardcoding values
7. Document the custom media registry for team onboarding
8. Combine with `@layer` for organized, maintainable responsive CSS
9. Test that build tools support `@custom-media` or include a PostCSS plugin
10. Use with `@media (--breakpoint) and (feature)` for compound queries

## Common Pitfalls

1. **Browser support** — `@custom-media` is not natively supported in any browser yet (requires PostCSS plugin)
2. **PostCSS dependency** — Without a build step, `@custom-media` rules are ignored
3. **No runtime redefinition** — Custom media values can't be changed after stylesheet load
4. **Specificity unchanged** — Custom media doesn't affect cascade specificity
5. **Naming collisions** — Globally shared names may conflict in large projects
6. **Not composable** — You can't combine two custom media queries into a third without redeclaring
7. **Debugging difficulty** — Transpiled output may be hard to trace back to the custom media source

## Accessibility Considerations

Define custom media queries for `prefers-reduced-motion`, `prefers-contrast`, and `prefers-color-scheme` to centralize accessibility-responsive rules. Use `--reduced-motion` to disable animations project-wide. Ensure `--high-contrast` custom media thickens borders and increases focus indicator visibility.

## Responsive Behavior

Align custom media breakpoints exactly with Bootstrap's defaults (576px, 768px, 992px, 1200px, 1400px). Use named queries inside component styles for scoped responsive rules. Combine viewport and capability queries (e.g., `--md` and `--touch`) for device-aware responsive layouts.
