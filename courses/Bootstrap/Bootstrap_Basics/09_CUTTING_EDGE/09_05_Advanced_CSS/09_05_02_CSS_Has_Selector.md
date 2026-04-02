---
title: "CSS :has() Selector with Bootstrap"
description: "Use the :has() relational pseudo-class for parent selection and conditional styling in Bootstrap 5"
difficulty: 3
tags: [css-has, selectors, parent-selection, conditional-styling, advanced]
prerequisites:
  - "CSS selectors and specificity"
  - "Bootstrap 5 components"
  - "CSS pseudo-classes"
---

## Overview

The `:has()` selector (relational pseudo-class) selects an element based on its children or siblings - solving CSS's long-standing inability to style parents based on child state. Combined with Bootstrap 5, `:has()` enables conditional styling like highlighting a card when its checkbox is checked, styling a form group based on input validation, or changing a navbar when a dropdown is open. Supported in all modern browsers (Chrome 105+, Firefox 121+, Safari 15.4+).

## Basic Implementation

### Select Parent by Child State

```html
<style>
  /* Style card when it contains a checked checkbox */
  .card:has(input:checked) {
    border-color: var(--bs-success);
    background-color: rgba(25, 135, 84, 0.05);
  }
</style>

<div class="card mb-3">
  <div class="card-body">
    <div class="form-check">
      <input class="form-check-input" type="checkbox" id="option1" checked>
      <label class="form-check-label" for="option1">Selected option</label>
    </div>
  </div>
</div>
<div class="card mb-3">
  <div class="card-body">
    <div class="form-check">
      <input class="form-check-input" type="checkbox" id="option2">
      <label class="form-check-label" for="option2">Unselected option</label>
    </div>
  </div>
</div>
```

### Style Form Group Based on Invalid Input

```html
<style>
  .form-group:has(.is-invalid) .form-label {
    color: var(--bs-danger);
  }
  .form-group:has(.is-invalid) {
    background-color: rgba(220, 53, 69, 0.05);
    padding: 1rem;
    border-radius: var(--bs-border-radius);
    border-left: 3px solid var(--bs-danger);
  }
</style>

<div class="form-group mb-3">
  <label class="form-label">Email</label>
  <input type="email" class="form-control is-invalid" value="not-an-email">
  <div class="invalid-feedback">Please enter a valid email.</div>
</div>
```

## Advanced Variations

### Conditional Card Styling

```html
<style>
  /* Highlight card with featured badge */
  .col:has(.badge-featured) .card {
    border: 2px solid var(--bs-warning);
    position: relative;
  }
  .col:has(.badge-featured) .card::after {
    content: "Featured";
    position: absolute;
    top: -10px;
    right: 10px;
    background: var(--bs-warning);
    color: #000;
    padding: 2px 8px;
    border-radius: 4px;
    font-size: 0.75rem;
    font-weight: 600;
  }
</style>

<div class="row g-3">
  <div class="col-md-4">
    <div class="card">
      <div class="card-body">
        <span class="badge bg-warning text-dark badge-featured">Featured</span>
        <h5 class="card-title mt-2">Premium Plan</h5>
        <p class="card-text">Best value for teams.</p>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">Basic Plan</h5>
        <p class="card-text">For individuals.</p>
      </div>
    </div>
  </div>
</div>
```

### Navbar State Changes

```html
<style>
  /* Style navbar differently when dropdown is open */
  .navbar:has(.dropdown-menu.show) {
    background-color: var(--bs-dark) !important;
    box-shadow: 0 4px 12px rgba(0,0,0,0.3);
  }
</style>

<nav class="navbar navbar-expand-lg navbar-light bg-light">
  <div class="container">
    <a class="navbar-brand" href="#">Brand</a>
    <div class="dropdown">
      <button class="btn btn-outline-secondary dropdown-toggle" data-bs-toggle="dropdown">
        Menu
      </button>
      <ul class="dropdown-menu">
        <li><a class="dropdown-item" href="#">Action</a></li>
        <li><a class="dropdown-item" href="#">Another action</a></li>
      </ul>
    </div>
  </div>
</nav>
```

### Empty State Detection

```html
<style>
  .list-group:has(.list-group-item) .empty-state {
    display: none;
  }
  .list-group:not(:has(.list-group-item)) {
    border: 2px dashed var(--bs-secondary);
    border-radius: var(--bs-border-radius);
    padding: 2rem;
    text-align: center;
  }
</style>

<ul class="list-group">
  <li class="empty-state text-muted">No items yet. Add one to get started.</li>
</ul>
```

### Table Row Hover Highlighting

```html
<style>
  /* Highlight entire row when hovering a specific cell */
  tr:has(td:hover) {
    background-color: rgba(13, 110, 253, 0.05);
  }
  /* Style row containing a status badge */
  tr:has(.badge.bg-danger) {
    background-color: rgba(220, 53, 69, 0.05);
  }
</style>

<table class="table">
  <tbody>
    <tr>
      <td>Item 1</td>
      <td><span class="badge bg-success">Active</span></td>
    </tr>
    <tr>
      <td>Item 2</td>
      <td><span class="badge bg-danger">Error</span></td>
    </tr>
  </tbody>
</table>
```

## Best Practices

1. **Use `:has()` for parent styling** - this is the primary use case CSS never had before.
2. **Combine with Bootstrap validation classes** (`.is-invalid`, `.is-valid`) for conditional form styling.
3. **Use `:has(:checked)`** for selection-based card or list styling.
4. **Prefer `:has()` over JavaScript** for conditional visual states to reduce script overhead.
5. **Use with `:not(:has())`** for empty state detection in lists and containers.
6. **Keep selectors specific** - `.card:has(.badge)` rather than `div:has(span)`.
7. **Combine with CSS custom properties** for theming conditional states.
8. **Use `:has()` for accessibility** - highlight invalid form groups with visual indicators.
9. **Test performance** - `:has()` with complex selectors can be slower on large DOMs.
10. **Use for UI feedback** like highlighting cards on checkbox selection.
11. **Combine with `data-bs-*` attributes** for Bootstrap component state detection.
12. **Avoid deeply nested `:has()`** - keep selectors readable and performant.

## Common Pitfalls

1. **Browser support gaps** - `:has()` doesn't work in Firefox < 121 (Dec 2023).
2. **Performance issues** with `:has()` on large DOM trees - scope it to specific containers.
3. **Overusing `:has()`** when simpler selectors (`.active`, `.show`) already exist.
4. **Missing `!important`** when overriding Bootstrap's own `:has()`-like patterns.
5. **Complex `:has()` selectors** become unreadable - break them into separate rules.
6. **Not providing fallbacks** for unsupported browsers.
7. **Using `:has()` for non-visual changes** that should use ARIA attributes instead.
8. **Circular style dependencies** - `:has()` triggering styles that affect the child state.

## Accessibility Considerations

- `:has()` is purely visual - ensure semantic state is communicated via ARIA attributes.
- Use `:has(.is-invalid)` for visual form validation but maintain `aria-invalid` attributes.
- Conditional highlighting should not rely solely on color - add icons or borders.
- `:has()` styling should enhance, not replace, proper focus management.
- Test that `:has()`-based visual changes are announced by screen readers when needed.

## Responsive Behavior

- `:has()` works within media queries for breakpoint-specific conditional styling.
- Use `:has()` to detect responsive navigation states (e.g., `:has(.navbar-toggler[aria-expanded="true"])`).
- Conditional card layouts can change based on content using `:has()` and responsive classes.
- `:has()` selectors inherit responsive behavior from their parent containers.
