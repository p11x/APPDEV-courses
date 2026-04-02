---
title: Grid Nesting Patterns
category: Layout System
difficulty: 2
time: 25 min
tags: bootstrap5, grid, nesting, sub-grid, gutters
---

## Overview

Bootstrap's grid system supports nesting: place a new `row` and `col-*` set inside any column to create sub-grids. Nested rows automatically inherit the 12-column system, giving the sub-grid its full set of column classes independent of the parent column's width. This pattern enables complex layouts like sidebars with internal grids, content areas with multi-column subsections, and card groups within larger page structures — all without leaving the Bootstrap grid framework.

## Basic Implementation

To nest a grid, add a `row` inside a `col`, then add `col-*` elements inside that row. The nested columns divide the parent column's width into 12 equal parts.

```html
<!-- Basic nesting -->
<div class="container">
  <div class="row">
    <div class="col-sm-8">
      Parent column (8/12)
      <div class="row mt-3">
        <div class="col-6">Nested 1 (6 of parent's 8)</div>
        <div class="col-6">Nested 2 (6 of parent's 8)</div>
      </div>
    </div>
    <div class="col-sm-4">Sidebar</div>
  </div>
</div>
```

Nested grids can use all the same responsive column classes as the top-level grid.

```html
<!-- Responsive nested columns -->
<div class="container">
  <div class="row">
    <div class="col-md-9">
      <div class="row g-3">
        <div class="col-12 col-sm-6">Content block A</div>
        <div class="col-12 col-sm-6">Content block B</div>
        <div class="col-12 col-md-4">Sub-item 1</div>
        <div class="col-12 col-md-4">Sub-item 2</div>
        <div class="col-12 col-md-4">Sub-item 3</div>
      </div>
    </div>
    <div class="col-md-3">Sidebar</div>
  </div>
</div>
```

## Advanced Variations

Multi-level nesting (nesting inside a nested column) is supported but should be used sparingly. Each nesting level inherits the 12-column system but adds cumulative padding from gutters.

```html
<!-- Multi-level nesting -->
<div class="container">
  <div class="row">
    <div class="col-lg-6">
      <div class="row g-2">
        <div class="col-8">
          <div class="row g-1">
            <div class="col-6 bg-light p-2">Deep nested A</div>
            <div class="col-6 bg-light p-2">Deep nested B</div>
          </div>
        </div>
        <div class="col-4 bg-secondary p-2">Side panel</div>
      </div>
    </div>
    <div class="col-lg-6">Right column</div>
  </div>
</div>
```

Override gutters on nested rows to create tighter or wider spacing within sub-sections compared to the parent grid.

```html
<!-- Custom nested gutters -->
<div class="container">
  <div class="row g-4">
    <div class="col-md-6">
      <div class="row g-1">
        <!-- Tighter gutters inside this section -->
        <div class="col-4"><div class="border p-2">Tile</div></div>
        <div class="col-4"><div class="border p-2">Tile</div></div>
        <div class="col-4"><div class="border p-2">Tile</div></div>
      </div>
    </div>
    <div class="col-md-6">
      <div class="row g-4">
        <!-- Wider gutters matching parent -->
        <div class="col-6"><div class="border p-2">Card</div></div>
        <div class="col-6"><div class="border p-2">Card</div></div>
      </div>
    </div>
  </div>
</div>
```

Nesting works well for dashboard panels where each section has its own internal grid layout.

```html
<!-- Dashboard panel with nested grid -->
<div class="container-fluid">
  <div class="row g-3">
    <div class="col-xl-8">
      <div class="card p-3">
        <div class="row row-cols-1 row-cols-md-2 g-3">
          <div class="col"><div class="bg-light p-3">Chart 1</div></div>
          <div class="col"><div class="bg-light p-3">Chart 2</div></div>
          <div class="col"><div class="bg-light p-3">Chart 3</div></div>
          <div class="col"><div class="bg-light p-3">Chart 4</div></div>
        </div>
      </div>
    </div>
    <div class="col-xl-4">
      <div class="card p-3">
        <div class="row g-2">
          <div class="col-12">Feed item 1</div>
          <div class="col-12">Feed item 2</div>
          <div class="col-12">Feed item 3</div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Limit nesting to two levels maximum to avoid cumulative padding and alignment complexity.
2. Always add a `row` directly inside the parent column before adding nested `col-*` elements.
3. Apply `g-*` gutter utilities on every nested `row` to maintain consistent spacing within sub-grids.
4. Use `mt-*` or `pt-*` utilities on the nested `row` to add vertical separation from the parent column content above it.
5. Keep nested column classes consistent with the parent grid's breakpoint strategy (mobile-first, progressive enhancement).
6. Avoid placing full-width `col-12` elements inside a narrow parent column — they will be very constrained and may look odd.
7. Use `row-cols-*` on nested rows when distributing equal-width items within a sub-grid.
8. Override `--bs-gutter-x` and `--bs-gutter-y` CSS custom properties on nested rows to fine-tune spacing.
9. Test nested layouts at every breakpoint, since the parent column width changes and affects the nested grid's available space.
10. Prefer nesting for content subdivisions, not for page-level structure — use top-level rows for the main layout.

## Common Pitfalls

1. **Missing nested row**: Placing `col-*` elements directly inside a parent `col` without an intermediate `row` breaks flex alignment and removes gutter compensation.
2. **Cumulative padding**: Each nesting level adds column padding, so nested content gets progressively narrower; counteract with negative margins or adjusted gutters.
3. **Over-nesting**: Three or more nesting levels create deeply indented, hard-to-maintain layouts with unpredictable spacing.
4. **Forgetting gutters on nested rows**: A nested `row` without `g-*` produces columns with no spacing between them.
5. **Inconsistent breakpoints**: Defining responsive classes only on the parent but not on nested columns causes layout misalignment at intermediate screen sizes.
6. **Nesting outside a col**: Placing a `row` inside a non-column wrapper div breaks the grid context and invalidates the nested column calculations.
7. **Assuming CSS subgrid**: Bootstrap 5 does not use CSS `grid-template-rows: subgrid`; nesting is implemented with nested flex containers, not CSS Grid subgrid.

## Accessibility Considerations

Nesting does not change DOM order, so screen readers traverse nested content in the same sequence as non-nested content. Ensure that the visual hierarchy created by nested grids matches the semantic heading structure (h1 → h2 → h3). Avoid hiding content inside nested columns with `d-none` at certain breakpoints without also adding `aria-hidden` considerations — content that disappears visually may still need to be accessible. Use landmark elements (`<section>`, `<aside>`) inside nested grid areas to preserve structural semantics.

## Responsive Behavior

Nested columns inherit the same responsive breakpoint system as the parent grid. A nested `col-sm-6` inside a `col-md-8` splits the parent's 8-column space in half at the small breakpoint and above. When the parent column collapses to full width on mobile, nested columns still divide that full width according to their classes. Use responsive nested column classes to create layouts that reflow within subsections — for example, a two-column nested grid that becomes a single column on mobile with `col-12 col-md-6` on each nested column.
