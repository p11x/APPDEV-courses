---
title: "CSS Nesting"
description: "Using native CSS nesting with Bootstrap, comparison with Sass nesting, and browser support strategies"
difficulty: 2
tags: [css-nesting, native-css, bootstrap, sass-comparison]
prerequisites:
  - 01_03_Customizing_Bootstrap
---

## Overview

CSS nesting allows writing child selectors inside their parent rules, eliminating the need for preprocessors like Sass for basic nesting. As of 2024, all major browsers support native CSS nesting. With Bootstrap 5.3+ moving toward CSS custom properties, native nesting reduces the build step needed for custom Bootstrap themes.

The `&` nesting selector works identically to Sass: it references the parent selector. The key difference is that native CSS nesting requires `&` for element selectors and attribute selectors (e.g., `& .child`, `&:hover`), while Sass is more permissive. Understanding this distinction prevents migration issues.

## Basic Implementation

```css
/* Native CSS nesting — no Sass required */
.custom-card {
  background: var(--bs-body-bg);
  border: 1px solid var(--bs-border-color);
  border-radius: var(--bs-border-radius);
  padding: 1rem;
  transition: box-shadow 0.2s ease;

  &:hover {
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  }

  & .card-title {
    font-weight: 600;
    color: var(--bs-heading-color);
  }

  & .card-body {
    padding: 1rem;

    & p {
      color: var(--bs-secondary-color);
      line-height: 1.6;
    }
  }

  @media (min-width: 768px) {
    padding: 1.5rem;

    & .card-body {
      padding: 1.5rem;
    }
  }
}
```

```html
<div class="custom-card">
  <h3 class="card-title">Nested Styling</h3>
  <div class="card-body">
    <p>This card uses native CSS nesting.</p>
  </div>
</div>
```

```js
// Check browser support
if (CSS.supports('selector(& .test)')) {
  console.log('Native CSS nesting supported');
} else {
  console.log('Fallback to Sass or PostCSS nesting');
}
```

## Advanced Variations

Nesting with Bootstrap utility overrides:

```css
.btn-custom {
  background: var(--bs-primary);
  color: white;
  border: none;
  padding: 0.5rem 1rem;

  &:hover,
  &:focus-visible {
    background: color-mix(in srgb, var(--bs-primary), black 15%);
  }

  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }

  & .icon {
    margin-right: 0.5rem;

    & svg {
      width: 1em;
      height: 1em;
    }
  }
}
```

## Best Practices

1. Always use `&` before element and pseudo-element selectors in native CSS nesting.
2. Limit nesting depth to 3 levels maximum for specificity management.
3. Use nesting for component-scoped styles, not global overrides.
4. Prefer native nesting over Sass for new projects to reduce build complexity.
5. Use `@supports` to provide Sass-compiled fallbacks for older browsers.
6. Combine with CSS `@scope` (see 09_02_06) for fully native component styling.
7. Keep selectors shallow — nesting is for organization, not specificity battles.
8. Use nesting for pseudo-classes (`&:hover`, `&:focus`) to keep related styles together.
9. Group media queries inside the relevant rule rather than at the file level.
10. Document whether the project uses native nesting or Sass nesting in the style guide.
11. Use PostCSS nesting plugin as a build-time fallback for production.
12. Avoid nesting ID selectors; they create excessive specificity.

## Common Pitfalls

1. **Missing `&`** — Native CSS requires `& .child` not `.child` (unlike Sass which allows both).
2. **Specificity increase** — Nesting increases specificity just like Sass; deeply nested rules are hard to override.
3. **Browser support** — Firefox < 117 and Safari < 17.0 don't support native nesting.
4. **Mixed Sass + native** — Mixing Sass nesting syntax with native CSS in the same file causes confusion.
5. **`@nest` deprecation** — The `@nest` rule was removed from the spec; use `&` directly.
6. **Performance** — Deeply nested selectors create more complex selector matching; keep it shallow.

## Accessibility Considerations

Nesting doesn't affect accessibility. Use nesting to group `:focus-visible`, `:focus-within`, and `[aria-expanded]` styles for accessible component states:

```css
.accordion-button {
  &[aria-expanded="true"] {
    background: var(--bs-primary);
    color: white;
  }

  &:focus-visible {
    outline: 2px solid var(--bs-focus-ring-color);
    outline-offset: 2px;
  }
}
```

## Responsive Behavior

Nest media queries directly inside rules for co-located responsive styles. This keeps all responsive variations of a component in one place rather than scattered across media query blocks.
