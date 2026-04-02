---
title: CSS Native Nesting
category: [CSS Advancements, Cutting Edge]
difficulty: 2
time: 20 min
tags: bootstrap5, css-nesting, native-css, sass-migration
---

## Overview

Native CSS nesting allows writing nested rules without a preprocessor, reducing build tool dependency. When working with Bootstrap, native nesting enables cleaner custom stylesheets that mirror Sass nesting patterns while running directly in the browser.

## Basic Implementation

Writing Bootstrap component overrides using native CSS nesting syntax.

```html
<style>
  .card {
    border: none;
    border-radius: 1rem;
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.08);
    transition: transform 0.2s ease;

    &:hover {
      transform: translateY(-4px);
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
    }

    & .card-header {
      background: transparent;
      border-bottom: 1px solid var(--bs-border-color);
      font-weight: 600;
    }

    & .card-body {
      padding: 1.5rem;
    }

    & .btn {
      border-radius: 0.5rem;
    }
  }
</style>

<div class="card">
  <div class="card-header">Featured</div>
  <div class="card-body">
    <h5 class="card-title">Native Nesting</h5>
    <p class="card-text">Styled with CSS nesting, no Sass required.</p>
    <button class="btn btn-primary">Learn More</button>
  </div>
</div>
```

## Advanced Variations

Nesting with `@media` queries and pseudo-classes for responsive overrides.

```html
<style>
  .navbar-custom {
    background: var(--bs-body-bg);
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);

    & .nav-link {
      color: var(--bs-body-color);
      padding: 0.75rem 1rem;
      border-radius: 0.375rem;
      transition: background 0.15s;

      &:hover, &:focus-visible {
        background: var(--bs-tertiary-bg);
        color: var(--bs-primary);
      }

      &.active {
        color: var(--bs-primary);
        font-weight: 600;
      }
    }

    & .navbar-toggler {
      border: none;
      padding: 0.5rem;

      &:focus-visible {
        box-shadow: 0 0 0 0.2rem rgba(var(--bs-primary-rgb), 0.25);
      }
    }

    @media (max-width: 767.98px) {
      & .navbar-collapse {
        background: var(--bs-body-bg);
        padding: 1rem;
        border-radius: 0 0 0.5rem 0.5rem;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      }

      & .nav-link {
        padding: 0.5rem 0;
      }
    }
  }
</style>
```

Nesting form controls with validation states.

```html
<style>
  .form-group {
    margin-bottom: 1rem;

    & label {
      font-weight: 500;
      margin-bottom: 0.25rem;
    }

    & .form-control {
      border-radius: 0.5rem;

      &:focus {
        box-shadow: 0 0 0 3px rgba(var(--bs-primary-rgb), 0.15);
      }

      &.is-invalid {
        border-color: var(--bs-danger);

        &:focus {
          box-shadow: 0 0 0 3px rgba(var(--bs-danger-rgb), 0.15);
        }
      }

      &.is-valid {
        border-color: var(--bs-success);

        &:focus {
          box-shadow: 0 0 0 3px rgba(var(--bs-success-rgb), 0.15);
        }
      }
    }

    & .form-text {
      font-size: 0.8rem;
      color: var(--bs-secondary-color);
    }
  }
</style>

<div class="form-group">
  <label>Email</label>
  <input type="email" class="form-control is-valid" value="user@example.com">
  <div class="form-text">We'll never share your email.</div>
</div>
```

## Best Practices

1. Use `&` explicitly at the start of nested selectors for clarity
2. Keep nesting depth to 2-3 levels maximum to maintain readability
3. Nest pseudo-classes (`:hover`, `:focus`, `:disabled`) directly under the parent
4. Use nesting for `@media` queries scoped to a component
5. Migrate Sass nesting to native CSS by removing the preprocessor step
6. Combine with CSS custom properties for theming within nested rules
7. Avoid nesting for simple selectors — `.card .btn` is fine without nesting
8. Use native nesting for component-scoped overrides of Bootstrap classes
9. Test in all target browsers — native nesting requires Chrome 120+, Firefox 117+, Safari 17.2+
10. Keep the `&` symbol for parent reference — omitting it can create descendant selectors unintentionally

## Common Pitfalls

1. **Browser support gaps** — Older browsers don't support native CSS nesting
2. **Over-nesting** — Deeply nested rules become hard to read and debug
3. **Missing `&`** — Without `&`, the selector becomes a descendant combinator, not a compound selector
4. **Specificity creep** — Nested selectors accumulate specificity faster than expected
5. **Not replacing Sass** — Keeping both Sass and native nesting creates confusion
6. **Nesting `@media` incorrectly** — `@media` must be nested inside a rule, not the other way around
7. **Combining with `@import`** — Native nesting doesn't work with `@import`-ed stylesheets in some browsers

## Accessibility Considerations

Nest `:focus-visible` rules alongside `:hover` to ensure keyboard focus styles are preserved. Avoid nesting that overrides Bootstrap's built-in accessibility features. Ensure nested validation states maintain sufficient color contrast for error and success indicators.

## Responsive Behavior

Nest `@media` queries inside component rules for scoped responsive overrides. This keeps breakpoint logic co-located with the component it affects. Use Bootstrap's breakpoint variables (e.g., `767.98px`) within nested media queries for consistency.
