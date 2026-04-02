---
title: "Class Naming Conventions with BEM and Bootstrap"
module: "Code Review"
difficulty: 2
estimated_time: 20
tags: ["naming", "BEM", "conventions", "CSS-architecture"]
prerequisites: ["CSS specificity knowledge", "Bootstrap 5 basics"]
---

## Overview

Combining BEM (Block Element Modifier) methodology with Bootstrap's utility-first approach creates a scalable, maintainable CSS architecture. This guide establishes naming conventions that prevent class conflicts, improve code readability, and ensure custom components integrate seamlessly with Bootstrap's built-in classes. Clear naming rules reduce onboarding time and enforce consistency across teams.

## Basic Implementation

**BEM Structure with Bootstrap**

BEM uses `block__element--modifier` syntax. Prefix custom blocks to avoid collisions with Bootstrap's classes.

```html
<!-- Custom card component using BEM -->
<div class="product-card">
  <div class="product-card__image-wrapper">
    <img class="product-card__image" src="item.jpg" alt="Product">
  </div>
  <div class="product-card__body">
    <h3 class="product-card__title">Item Name</h3>
    <p class="product-card__price">$29.99</p>
    <button class="product-card__button product-card__button--primary btn btn-primary">
      Add to Cart
    </button>
  </div>
</div>
```

**Custom Class Prefixing**

Use a project-specific prefix for all custom classes to prevent namespace collisions with Bootstrap.

```scss
// Prefix convention: proj- for project-specific classes
.proj-header { }
.proj-header__nav { }
.proj-header__logo { }
.proj-header__cta--highlighted { }
```

**Combining Utilities with BEM**

Bootstrap utilities handle layout and spacing while BEM classes manage component-specific styles.

```html
<div class="feature-card d-flex flex-column align-items-center p-4 rounded shadow-sm">
  <div class="feature-card__icon mb-3">
    <i class="bi bi-shield-check fs-1 text-primary"></i>
  </div>
  <h4 class="feature-card__title text-center">Security</h4>
  <p class="feature-card__description text-muted">Enterprise-grade protection.</p>
</div>
```

## Advanced Variations

**Modifier Chaining**

When a component needs multiple variations, chain BEM modifiers clearly.

```html
<!-- Multiple modifiers on a single element -->
<div class="alert-banner alert-banner--dismissible alert-banner--warning d-flex align-items-center">
  <span class="alert-banner__message">Session expires in 5 minutes.</span>
  <button class="alert-banner__close btn-close" aria-label="Close"></button>
</div>
```

**State Classes**

Use BEM modifiers for component states rather than overriding Bootstrap utility classes.

```scss
// State management with BEM
.nav-item__link--active {
  font-weight: 600;
  color: $primary;
  border-bottom: 2px solid $primary;
}

.nav-item__link--disabled {
  opacity: 0.5;
  pointer-events: none;
}
```

**Responsive BEM Modifiers**

Combine BEM with breakpoint-specific modifiers for responsive behavior.

```html
<div class="sidebar sidebar--collapsed sidebar--expanded-lg">
  <nav class="sidebar__menu">
    <a class="sidebar__link sidebar__link--active" href="#">Dashboard</a>
  </nav>
</div>
```

## Best Practices

1. **Prefix all custom classes** with a project namespace (e.g., `app-`, `proj-`, `acme-`)
2. **Use BEM for component structure** and Bootstrap utilities for layout/spacing
3. **Never modify Bootstrap class names** - always create new custom classes for overrides
4. **Keep selectors flat** - avoid nesting BEM selectors deeper than one level
5. **Use descriptive block names** that reflect the component's purpose, not appearance
6. **Separate concerns** - BEM for semantics, utilities for visual adjustments
7. **Document naming conventions** in a shared style guide accessible to all developers
8. **Use kebab-case consistently** for all class names across BEM and custom classes
9. **Limit modifier depth** - prefer single-purpose modifiers over compound ones
10. **Review class lists for redundancy** - remove duplicate utility applications
11. **Avoid abbreviations** in class names unless they are project-wide standards
12. **Use CSS custom properties** with BEM for themeable component variations

## Common Pitfalls

1. **Overriding Bootstrap internals** - Targeting `.card` styles directly causes conflicts during version upgrades
2. **Creating overly specific selectors** - `.product-card .product-card__body .product-card__title` defeats BEM's flat structure
3. **Mixing naming methodologies** - Using `is-active` alongside `--active` modifiers creates confusion
4. **Duplicating utility logic in BEM** - Writing `.product-card__flex` when `d-flex` already exists
5. **Using generic block names** - `.card` as a custom class collides with Bootstrap's `.card` component
6. **Forgetting responsive modifiers** - BEM states that should change at breakpoints get locked to one size
7. **Inconsistent prefix usage** - Mixing `app-` and `proj-` prefixes across the codebase
8. **Modifier without base class** - Applying `--active` without the base block class breaks styling
9. **Nesting BEM blocks** - Placing one BEM block inside another's element creates tight coupling
10. **Using BEM for single-element components** - Adding `__wrapper` to everything adds unnecessary abstraction

## Accessibility Considerations

Class names should not replace semantic HTML or ARIA attributes. Use `<button class="cta-button">` rather than `<div class="cta-button" role="button">`. Screen readers rely on proper element semantics, not CSS classes. Ensure modifier classes that change visual state also update ARIA attributes. For example, `nav-link--active` should coincide with `aria-current="page"`. Avoid using classes like `sr-only` (Bootstrap 4) - use `visually-hidden` (Bootstrap 5) for screen-reader-only content.

## Responsive Behavior

BEM modifiers can encode responsive behavior using breakpoint suffixes: `layout--stacked-md` or `grid--compact-sm`. Combine these with Bootstrap's responsive utilities for a layered approach. The BEM modifier handles component-level restructuring while utilities manage spacing and display changes. Always test BEM-responsive modifiers at every breakpoint to ensure transitions are smooth and content remains accessible on all screen sizes.
