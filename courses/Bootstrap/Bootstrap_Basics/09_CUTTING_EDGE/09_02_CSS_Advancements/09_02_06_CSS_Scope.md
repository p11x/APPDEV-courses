---
title: "CSS Scope"
description: "Using @scope for Bootstrap component isolation, scoped styles, and cascade layer management"
difficulty: 3
tags: [@scope, cascade-layers, scoped-styles, bootstrap, isolation]
prerequisites:
  - 09_02_02_CSS_Nesting
---

## Overview

The `@scope` rule creates scoped style blocks that only apply within a defined DOM subtree. Unlike CSS Modules or BEM naming conventions, `@scope` is a native CSS mechanism for style isolation without build tools. Combined with `@layer` for cascade management, you can create Bootstrap component styles that don't leak to other components and have predictable specificity.

`@scope (.card) to (.card-body)` limits styles to elements between `.card` and `.card-body` boundaries — styles don't penetrate past `.card-body` children. This prevents unintended styling of nested third-party components. `@layer` controls cascade order, ensuring utility overrides always win regardless of specificity.

## Basic Implementation

```css
/* Scoped card styles — only apply within .card, not past .card-body boundary */
@scope (.card) to (.card-body) {
  :scope {
    border: 1px solid var(--bs-border-color);
    border-radius: var(--bs-border-radius);
    overflow: hidden;
  }

  .card-title {
    font-size: 1.25rem;
    font-weight: 600;
    margin-bottom: 0.5rem;
  }

  .card-img-top {
    width: 100%;
    object-fit: cover;
  }
}

/* Cascade layers — control specificity order */
@layer reset, base, components, utilities;

@layer base {
  .card { background: var(--bs-body-bg); }
}

@layer components {
  .card-featured { background: var(--bs-primary-bg-subtle); }
}

@layer utilities {
  .bg-dark .card { background: #1a1a1a; }
}
```

```html
<div class="card">
  <img src="photo.jpg" class="card-img-top" alt="...">
  <div class="card-body">
    <h5 class="card-title">Scoped Title</h5>
    <p>Body content — styles don't leak past this boundary.</p>
    <div class="third-party-widget">
      <!-- This widget is NOT styled by @scope rules -->
    </div>
  </div>
</div>
```

```js
// Feature detection
if (CSS.supports('selector(:scope)') && CSS.supports('@scope', '(.test) {}')) {
  document.documentElement.classList.add('supports-scope');
}
```

## Advanced Variations

Anonymous scope (implicit root):

```css
@scope {
  :scope {
    container-type: inline-size;
  }

  .btn {
    padding: 0.5rem 1rem;
    border-radius: 0.375rem;
  }
}
```

Scope with `@layer`:

```css
@layer components {
  @scope (.alert) {
    :scope {
      padding: 1rem;
      border-radius: var(--bs-border-radius);
    }

    .alert-heading { font-weight: 700; }
  }
}
```

## Best Practices

1. Use `@scope` to isolate component styles without Shadow DOM.
2. Define scope boundaries (`.card` to `.card-body`) to prevent style leakage.
3. Use `@layer` to establish cascade order: reset → base → components → utilities.
4. Always place `@layer` declarations at the top of the file.
5. Use `:scope` to reference the scope root element.
6. Combine `@scope` with CSS nesting for fully native component styling.
7. Use scoped styles for third-party widget isolation.
8. Avoid `@scope` for global styles; use it only for component boundaries.
9. Test scope boundary behavior with deeply nested components.
10. Document scope boundaries in component CSS comments.
11. Use `@layer` to ensure Bootstrap utilities always override component styles.
12. Prefer `@scope` over BEM for new projects when browser support allows.

## Common Pitfalls

1. **Browser support** — Chrome 118+ only; Firefox and Safari lack support as of 2024.
2. **Boundary misunderstanding** — The `to` clause excludes the boundary element's children, not the element itself.
3. **`@layer` order matters** — Layers declared first have lowest priority; declare `utilities` last.
4. **Specificity within layers** — `@layer` doesn't reduce specificity within a layer; it only orders layers.
5. **Anonymous scope** — `@scope { }` without a selector scopes to the entire document.
6. **Interaction with Shadow DOM** — `@scope` doesn't work inside Shadow DOM; use it for light DOM isolation.

## Accessibility Considerations

Scoped styles don't affect accessibility. ARIA attributes and semantic HTML work identically within and outside `@scope` boundaries. Use scoped styles to ensure focus indicators and high-contrast modes work within components.

## Responsive Behavior

Media queries work inside `@scope` blocks. Combine with container queries for component-level responsive scoping:

```css
@scope (.dashboard-widget) {
  @container (min-width: 400px) {
    :scope { flex-direction: row; }
  }
}
```
