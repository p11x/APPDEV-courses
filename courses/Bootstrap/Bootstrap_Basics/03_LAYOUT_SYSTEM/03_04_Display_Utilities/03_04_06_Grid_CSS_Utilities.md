---
title: "Grid CSS Utilities"
description: "Use Bootstrap 5 d-grid utilities and CSS Grid features for two-dimensional layouts"
difficulty: 2
estimated_time: "20 minutes"
tags: ["css-grid", "d-grid", "gap", "grid-template", "layout"]
---

# Grid CSS Utilities

## Overview

Bootstrap 5 introduces `d-grid` and `d-inline-grid` display utilities that enable CSS Grid layout without custom CSS. Combined with Bootstrap's `gap` utilities and inline `grid-template` styles, these classes provide a powerful two-dimensional layout system that complements the existing flexbox utilities.

CSS Grid differs from Flexbox in that it controls both rows and columns simultaneously. While Bootstrap's 12-column grid system handles responsive column layouts, `d-grid` is ideal for application-style layouts, dashboard panels, gallery grids with uniform gaps, and any scenario requiring explicit row and column control.

## Basic Implementation

### Creating a Grid Container

Apply `d-grid` to create a block-level grid container:

```html
<!-- Basic 3-column grid -->
<div class="d-grid gap-3" style="grid-template-columns: repeat(3, 1fr);">
  <div class="p-3 bg-primary text-white">Cell 1</div>
  <div class="p-3 bg-success text-white">Cell 2</div>
  <div class="p-3 bg-danger text-white">Cell 3</div>
  <div class="p-3 bg-warning text-dark">Cell 4</div>
  <div class="p-3 bg-info text-white">Cell 5</div>
  <div class="p-3 bg-secondary text-white">Cell 6</div>
</div>
```

### Inline Grid

Use `d-inline-grid` for inline-level grid containers:

```html
<div class="d-inline-grid gap-2" style="grid-template-columns: 100px 100px;">
  <div class="p-2 bg-primary text-white">A</div>
  <div class="p-2 bg-success text-white">B</div>
  <div class="p-2 bg-danger text-white">C</div>
  <div class="p-2 bg-warning text-dark">D</div>
</div>
```

### Gap Utilities with Grid

Bootstrap's gap utilities set both row and column gaps simultaneously:

```html
<!-- Different gap sizes -->
<div class="d-grid gap-1 mb-3" style="grid-template-columns: repeat(3, 1fr);">
  <div class="p-3 bg-light border">gap-1</div>
  <div class="p-3 bg-light border">gap-1</div>
  <div class="p-3 bg-light border">gap-1</div>
</div>

<div class="d-grid gap-4 mb-3" style="grid-template-columns: repeat(3, 1fr);">
  <div class="p-3 bg-light border">gap-4</div>
  <div class="p-3 bg-light border">gap-4</div>
  <div class="p-3 bg-light border">gap-4</div>
</div>

<!-- Directional gaps -->
<div class="d-grid gap-row-2 gap-column-4" style="grid-template-columns: repeat(3, 1fr);">
  <div class="p-3 bg-light border">Row 2 Col 4</div>
  <div class="p-3 bg-light border">Row 2 Col 4</div>
  <div class="p-3 bg-light border">Row 2 Col 4</div>
</div>
```

## Advanced Variations

### Responsive Grid Columns

Use inline styles with CSS functions or adjust via JavaScript to create responsive grid columns:

```html
<!-- Auto-fit responsive grid -->
<div class="d-grid gap-3" style="grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));">
  <div class="card p-3">Auto-fit card 1</div>
  <div class="card p-3">Auto-fit card 2</div>
  <div class="card p-3">Auto-fit card 3</div>
  <div class="card p-3">Auto-fit card 4</div>
  <div class="card p-3">Auto-fit card 5</div>
</div>
```

### Dashboard Layout

Create multi-row, multi-column dashboard layouts:

```html
<div class="d-grid gap-3" style="grid-template-columns: 250px 1fr 1fr; grid-template-rows: auto 1fr;">
  <div class="p-3 bg-dark text-white" style="grid-column: 1 / -1;">Header spanning all columns</div>
  <div class="p-3 bg-light border">Sidebar</div>
  <div class="p-3 bg-light border">Main Chart</div>
  <div class="p-3 bg-light border">Stats Panel</div>
</div>
```

### Combining Grid with Flexbox Children

Grid for outer layout, flexbox for inner component alignment:

```html
<div class="d-grid gap-3" style="grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));">
  <div class="card">
    <div class="card-body d-flex flex-column justify-content-between">
      <h5 class="card-title">Card Title</h5>
      <p class="card-text">Content</p>
      <button class="btn btn-primary align-self-start">Action</button>
    </div>
  </div>
  <div class="card">
    <div class="card-body d-flex flex-column justify-content-between">
      <h5 class="card-title">Card Title</h5>
      <p class="card-text">Content</p>
      <button class="btn btn-primary align-self-start">Action</button>
    </div>
  </div>
</div>
```

### Named Grid Areas

Use inline styles for grid-template-areas to create semantic layouts:

```html
<div class="d-grid gap-2 vh-50" style="
  grid-template-areas:
    'header header header'
    'sidebar main main'
    'footer footer footer';
  grid-template-rows: auto 1fr auto;
  grid-template-columns: 200px 1fr 1fr;
  height: 500px;
">
  <div class="bg-dark text-white p-3" style="grid-area: header;">Header</div>
  <div class="bg-light p-3" style="grid-area: sidebar;">Sidebar</div>
  <div class="bg-white p-3 border" style="grid-area: main;">Main Content</div>
  <div class="bg-secondary text-white p-3" style="grid-area: footer;">Footer</div>
</div>
```

## Best Practices

1. **Use `d-grid` for two-dimensional layouts** where you need explicit control over both rows and columns. Use `d-flex` for one-dimensional flow.

2. **Pair with `gap` utilities** for spacing. CSS Grid's gap property is more reliable than margins for creating uniform gutters.

3. **Use `auto-fit` and `minmax()`** in `grid-template-columns` for responsive grids without media queries. Items automatically wrap and fill available space.

4. **Combine Grid with Bootstrap's 12-column system.** Use Grid for application shells and Bootstrap's `.row`/`.col-*` for content sections within grid cells.

5. **Keep `grid-template-*` styles minimal.** Complex grid definitions should move to a CSS file rather than inline styles for maintainability.

6. **Use `grid-column: 1 / -1`** to span an item across all columns without counting columns explicitly.

7. **Prefer `auto-fill` over `auto-fit`** when you want empty tracks to remain visible. Use `auto-fit` when empty tracks should collapse.

8. **Test grid layouts in multiple browsers.** While CSS Grid has excellent support, older Edge versions may require prefixes.

9. **Use `gap-row-*` and `gap-column-*`** utilities when row and column gaps need different values.

10. **Reserve `d-inline-grid`** for small grid components that should flow inline with surrounding text, like icon sets or badge groups.

11. **Layer Grid and Flexbox.** Grid excels at page-level layout; Flexbox excels at component-level alignment. Use both in the same document.

## Common Pitfalls

### Relying on inline styles for grid-template
Inline `grid-template-columns` styles work but cannot be responsive with Bootstrap breakpoints alone. Use `auto-fit`/`auto-fill` with `minmax()` or add custom CSS media queries for responsive column counts.

### Confusing d-grid with Bootstrap's .row
`d-grid` creates a CSS Grid container. `.row` creates a Flexbox-based Bootstrap grid. They are different systems and should not be mixed on the same element.

### Gap not working with margins
CSS Grid gap and manual margins on children can conflict. Use gap alone for spacing between grid items and padding within items for internal spacing.

### Forgetting grid items auto-place
By default, grid items fill cells automatically from left to right, top to bottom. Explicit `grid-column` or `grid-row` placement on children may produce unexpected wrapping if the placement conflicts with auto-flow.

### Over-constraining grid tracks
Setting fixed pixel widths on all columns prevents responsive behavior. Use `fr` units, `minmax()`, or `auto` to create flexible tracks.

### Not setting explicit row heights
Without `grid-template-rows`, rows size to content by default. For full-height layouts, set `grid-template-rows: auto 1fr auto` or similar to distribute space intentionally.

## Accessibility Considerations

CSS Grid does not change DOM order, so screen readers and keyboard navigation follow source order regardless of visual placement. When using explicit grid placement to position items visually, ensure the source order provides a logical reading sequence.

Grid areas with `grid-area` named templates create visual relationships between panels. Ensure these relationships are communicated through ARIA landmarks (`role="navigation"`, `role="main"`, etc.) rather than relying solely on visual proximity.

For dashboard layouts using Grid, ensure that focus management follows a logical pattern. Visual panels arranged in a grid may not correspond to sequential tab order if items are placed with explicit grid lines.

## Responsive Behavior

Bootstrap's `d-grid` and `d-inline-grid` support responsive prefixes (`d-md-grid`, `d-lg-inline-grid`), allowing you to switch between grid and non-grid display at breakpoints. However, `grid-template-*` properties require inline styles or custom CSS with media queries for responsive column counts.

```html
<!-- Switch to grid only on medium+ -->
<div class="d-block d-md-grid gap-3" style="grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));">
  <div class="card p-3">Responsive grid item 1</div>
  <div class="card p-3">Responsive grid item 2</div>
  <div class="card p-3">Responsive grid item 3</div>
</div>
```

For fully responsive grid column counts without custom CSS, use `auto-fit` or `auto-fill` with `minmax()` in the inline style, which adapts columns based on available space at any viewport width.
