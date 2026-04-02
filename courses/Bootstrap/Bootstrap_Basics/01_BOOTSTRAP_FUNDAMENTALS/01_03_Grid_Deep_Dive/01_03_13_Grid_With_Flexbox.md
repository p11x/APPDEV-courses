---
title: "Grid With Flexbox"
topic: "Grid Deep Dive"
subtopic: "Grid With Flexbox"
difficulty: 2
duration: "35 minutes"
prerequisites: ["Grid Alignment", "Equal Width Columns"]
learning_objectives:
  - Combine Bootstrap grid with flex utility classes
  - Use justify-content for horizontal row alignment
  - Apply align-items for vertical column alignment in grids
---

## Overview

Bootstrap's grid system is built on CSS Flexbox, which means all flex utility classes work directly on rows and columns. Combining grid classes with flex utilities like `justify-content-*`, `align-items-*`, and `d-flex` gives you fine-grained control over content positioning within grid layouts. This allows centering content, distributing space evenly, creating sticky footers, and building complex alignment patterns without custom CSS.

## Basic Implementation

Using `justify-content-center` to center columns horizontally within a row:

```html
<div class="container">
  <div class="row justify-content-center">
    <div class="col-4">
      <div class="bg-primary text-white p-3 text-center">Centered</div>
    </div>
    <div class="col-4">
      <div class="bg-secondary text-white p-3 text-center">Centered</div>
    </div>
  </div>
</div>
```

Using `align-items-center` to vertically center column content:

```html
<div class="container">
  <div class="row align-items-center" style="min-height: 200px;">
    <div class="col">
      <div class="bg-success text-white p-3">Vertically centered</div>
    </div>
    <div class="col">
      <div class="bg-warning p-3">Vertically centered</div>
    </div>
  </div>
</div>
```

Combining `d-flex flex-column` inside a column for full-height layouts:

```html
<div class="container">
  <div class="row">
    <div class="col-md-4">
      <div class="d-flex flex-column h-100 bg-light p-3">
        <h5>Sidebar</h5>
        <p class="flex-grow-1">Content fills available space.</p>
        <button class="btn btn-primary mt-auto">Sticky Button</button>
      </div>
    </div>
    <div class="col-md-8">
      <div class="bg-white p-3 border">Main content area</div>
    </div>
  </div>
</div>
```

## Advanced Variations

Using `justify-content-between` with variable-width columns for spaced-out navigation:

```html
<div class="container">
  <div class="row justify-content-between align-items-center py-3">
    <div class="col-auto">
      <strong class="fs-4">Logo</strong>
    </div>
    <div class="col-auto">
      <nav>
        <a href="#" class="me-3">Home</a>
        <a href="#" class="me-3">About</a>
        <a href="#">Contact</a>
      </nav>
    </div>
  </div>
</div>
```

Using `align-self-*` on individual columns for per-column vertical alignment:

```html
<div class="container">
  <div class="row" style="min-height: 250px;">
    <div class="col align-self-start">
      <div class="bg-primary text-white p-3">Top</div>
    </div>
    <div class="col align-self-center">
      <div class="bg-secondary text-white p-3">Center</div>
    </div>
    <div class="col align-self-end">
      <div class="bg-success text-white p-3">Bottom</div>
    </div>
  </div>
</div>
```

Flex order utilities combined with grid for reordering columns:

```html
<div class="container">
  <div class="row">
    <div class="col-md-4 order-md-2">
      <div class="bg-warning p-3">Second visually, first in HTML</div>
    </div>
    <div class="col-md-4 order-md-1">
      <div class="bg-info text-white p-3">First visually, second in HTML</div>
    </div>
    <div class="col-md-4 order-md-3">
      <div class="bg-danger text-white p-3">Always last</div>
    </div>
  </div>
</div>
```

## Best Practices

1. Use `justify-content-center` on rows to center columns when they don't fill the full 12-column width.
2. Apply `align-items-center` on rows with `min-height` to vertically center content in hero sections.
3. Use `d-flex flex-column h-100` inside columns with `mt-auto` on the footer element for sticky-bottom patterns.
4. Combine `flex-grow-1` on content areas and `mt-auto` on footers to push elements to the bottom of columns.
5. Use `align-self-*` on individual columns when different vertical alignments are needed per column.
6. Apply `justify-content-md-between` for responsive horizontal distribution that stacks on mobile.
7. Use `d-flex` and `gap-*` utilities inside columns for flexbox-based component layouts.
8. Keep `align-items-stretch` (default) when columns should match the tallest column height.
9. Use `flex-wrap` on inline flex containers inside columns to prevent overflow.
10. Combine grid structure with flex utilities rather than replacing the grid with raw flex containers.

## Common Pitfalls

- **Conflicting alignments**: Setting `align-items-center` on the row and `align-self-stretch` on a column creates unexpected behavior — pick one approach.
- **Missing `min-height`**: `align-items-center` has no visible effect if the row height equals the content height. Set `min-height` on the row.
- **Overriding flex properties**: Custom CSS that overrides `flex` shorthand on `col` classes breaks grid sizing.
- **Using `d-flex` on rows unnecessarily**: Rows are already `display: flex`. Adding `d-flex` is redundant unless overriding other display utilities.
- **Forgetting responsive alignment**: `align-items-center` applies at all sizes. Use `align-items-md-center` for desktop-only centering.
- **Nesting flex containers excessively**: Deep nesting of `d-flex` inside grid columns increases complexity and debugging difficulty.
- **Ignoring `flex-shrink: 0`**: Columns with `flex-shrink: 0` won't compress, causing overflow on small screens.

## Accessibility Considerations

- Maintain logical DOM order even when using flex order utilities — screen readers follow DOM order, not visual order.
- Avoid using `order-*` to completely rearrange content — it confuses keyboard navigation and screen readers.
- Use `min-height` for vertically centered sections to ensure adequate touch target sizes on mobile.
- Ensure centered content remains readable at all viewport widths without horizontal scrolling.
- Provide visible focus indicators on interactive elements inside flex-aligned grid layouts.
- Use semantic landmarks (`<nav>`, `<main>`, `<aside>`) inside flex-aligned grid columns for screen reader navigation.

## Responsive Behavior

Flex utilities support responsive variants. Use `justify-content-{breakpoint}-*` and `align-items-{breakpoint}-*` to change alignment at different viewports:

```html
<div class="container">
  <div class="row justify-content-start justify-content-md-center justify-content-lg-between"
       style="min-height: 150px;">
    <div class="col-auto align-self-start align-self-md-center">
      <div class="bg-primary text-white p-3">Responsive alignment</div>
    </div>
    <div class="col-auto align-self-end align-self-md-center">
      <div class="bg-secondary text-white p-3">Responsive alignment</div>
    </div>
  </div>
</div>
```

On mobile, columns align to the start. At `md`, they center. At `lg`, they distribute with space between. Vertical alignment shifts from start/end on mobile to center on `md+`.
