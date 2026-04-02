---
title: CSS :has() Parent Selector
category: [CSS Advancements, Cutting Edge]
difficulty: 3
time: 25 min
tags: bootstrap5, has-selector, parent-selector, conditional-styling, forms
---

## Overview

The `:has()` selector styles a parent element based on the state of its children, enabling patterns previously impossible in CSS. Combined with Bootstrap, it powers conditional card styling, form-state-aware containers, and interactive layouts without JavaScript.

## Basic Implementation

Styling a card differently when it contains an invalid input.

```html
<style>
  .card:has(.is-invalid) {
    border-color: var(--bs-danger);
    box-shadow: 0 0 0 0.2rem rgba(var(--bs-danger-rgb), 0.25);
  }

  .card:has(.is-valid) {
    border-color: var(--bs-success);
  }
</style>

<div class="card p-4">
  <h5 class="mb-3">Registration</h5>
  <div class="mb-3">
    <label class="form-label">Email</label>
    <input type="email" class="form-control is-invalid" value="bad-email">
    <div class="invalid-feedback">Please provide a valid email.</div>
  </div>
</div>
```

## Advanced Variations

Highlighting active navigation items and their parent containers.

```html
<style>
  .list-group-item:has(.form-check-input:checked) {
    background-color: var(--bs-primary-bg-subtle);
    border-color: var(--bs-primary);
  }

  .nav-link:has(+ .dropdown-menu .active) {
    font-weight: bold;
  }

  /* Style the card when any child checkbox is checked */
  .card:has(input[type="checkbox"]:checked) {
    background: var(--bs-success-bg-subtle);
    border-color: var(--bs-success);
  }
</style>

<div class="card p-3">
  <h6>Select Features</h6>
  <div class="list-group">
    <label class="list-group-item d-flex align-items-center gap-2">
      <input class="form-check-input" type="checkbox" checked>
      Dark Mode
    </label>
    <label class="list-group-item d-flex align-items-center gap-2">
      <input class="form-check-input" type="checkbox">
      Notifications
    </label>
    <label class="list-group-item d-flex align-items-center gap-2">
      <input class="form-check-input" type="checkbox" checked>
      Auto-save
    </label>
  </div>
</div>
```

Combining `:has()` with sibling selectors for complex conditional layouts.

```html
<style>
  /* Sidebar collapses when main content has a full-width class */
  .layout:has(.main-content.full-width) .sidebar {
    display: none;
  }

  .layout:has(.main-content.full-width) .main-content {
    flex: 0 0 100%;
    max-width: 100%;
  }

  /* Card grid adjusts when a card is expanded */
  .row:has(.card.expanded) .card:not(.expanded) {
    opacity: 0.5;
    pointer-events: none;
  }

  /* Form group highlights when any input is focused inside */
  .form-group:has(input:focus) {
    background: var(--bs-tertiary-bg);
    border-radius: 0.375rem;
    padding: 1rem;
  }
</style>

<div class="form-group mb-3">
  <label class="form-label">Search</label>
  <input type="text" class="form-control" placeholder="Type to search...">
</div>
```

## Best Practices

1. Use `:has()` to style parent containers based on child validation state
2. Combine `:has()` with Bootstrap's `.is-invalid`/`.is-valid` classes for form feedback
3. Leverage `:has(:checked)` for checkbox/radio-driven card highlighting
4. Pair with `:focus-within` alternatives — `:has(:focus)` offers more flexibility
5. Use for responsive layout switching based on content state, not just viewport
6. Keep selectors specific to avoid unintended parent matches
7. Test performance — deeply nested `:has()` selectors can be expensive
8. Use `:has()` instead of JavaScript class toggling for CSS-only interactions
9. Combine with container queries for component-level conditional styling
10. Document `:has()` usage for team awareness of browser support requirements

## Common Pitfalls

1. **Browser support** — `:has()` requires Chrome 105+, Firefox 121+, Safari 15.4+
2. **Performance** — `:has()` with broad selectors triggers expensive DOM scanning
3. **Specificity confusion** — `:has()` adds specificity that can cascade unexpectedly
4. **Over-nesting** — Chaining multiple `:has()` creates unreadable selectors
5. **Not a replacement for JS** — Complex state logic still needs JavaScript
6. **Shadow DOM boundary** — `:has()` cannot cross into Shadow DOM to inspect web component internals
7. **Infinite-style loops** — Mutating styles via `:has()` that change layout can cause reflow cascades

## Accessibility Considerations

Use `:has()` to visually indicate form error states on parent containers, aiding users in identifying problem areas. Ensure `:has()`-based styling doesn't hide focus indicators. Combine with `aria-invalid` for dual visual and semantic feedback. Test with screen readers to verify that parent-level styling changes don't disrupt reading order.

## Responsive Behavior

Combine `:has()` with media queries for breakpoint-aware conditional styling. Use `:has()` inside container queries for component-level responsive logic. Avoid viewport-based `:has()` patterns that conflict with mobile-first design. Test performance on low-powered devices where `:has()` evaluation cost is magnified.
