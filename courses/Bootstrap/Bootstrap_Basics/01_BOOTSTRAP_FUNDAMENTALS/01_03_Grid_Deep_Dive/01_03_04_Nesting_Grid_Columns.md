---
tags: [bootstrap5, grid, nesting, columns, rows, layout]
category: Bootstrap Fundamentals
difficulty: 2
estimated_time: 30 minutes
---

# Nesting Grid Columns

## Overview

Grid nesting is the practice of placing a new `.row` with `.col-*` children inside an existing column. This creates a sub-grid that operates within the parent column's width. The nested row again has 12 available column units, but those units represent a fraction of the parent column — not the full viewport or container width.

Nesting enables complex layouts that cannot be achieved with a single flat row. For example, a page with a main content area and sidebar can have its main content area further subdivided into header, body, and aside sections — all using the grid system.

Bootstrap places no technical limit on nesting depth, but practical constraints (markup complexity, maintainability, responsive behavior) suggest limiting nesting to 2-3 levels. Each nested level requires its own `.row` wrapper and applies its own gutter system unless explicitly removed with `.g-0`.

Understanding how nested column widths are calculated is critical. A `.col-6` inside a parent `.col-8` occupies 50% of the 8-column space — effectively 33% of the full row width (4/12 of 8/12).

## Basic Implementation

### Single-Level Nesting

The most common nesting pattern places a row with two columns inside one of the parent row's columns.

```html
<div class="container">
  <div class="row">
    <div class="col-8">
      <div class="row">
        <div class="col-6 bg-light p-3">Nested Left</div>
        <div class="col-6 bg-light p-3">Nested Right</div>
      </div>
    </div>
    <div class="col-4 bg-secondary p-3 text-white">Sidebar</div>
  </div>
</div>
```

The parent `.col-8` contains a nested row. Each nested `.col-6` occupies 50% of the 8-column parent, which equals approximately 33% of the full container width.

### Nested Columns with Different Widths

Nested columns do not need to be equal-width. They follow the same 12-unit rules as the top-level grid.

```html
<div class="container">
  <div class="row">
    <div class="col-9">
      <div class="row">
        <div class="col-8 bg-primary text-white p-3">Nested 2/3</div>
        <div class="col-4 bg-info text-white p-3">Nested 1/3</div>
      </div>
    </div>
    <div class="col-3 bg-light p-3">Right sidebar</div>
  </div>
</div>
```

The nested `.col-8` within the `.col-9` parent occupies roughly 59% of the full width (8/12 of 9/12).

### Three-Column Layout with Nesting

Divide one of the columns further for a three-tier layout.

```html
<div class="container">
  <div class="row">
    <div class="col-6">
      <div class="row">
        <div class="col-12 bg-warning p-3">Full nested width</div>
      </div>
      <div class="row">
        <div class="col-4 bg-light p-3">A</div>
        <div class="col-4 bg-light p-3">B</div>
        <div class="col-4 bg-light p-3">C</div>
      </div>
    </div>
    <div class="col-3 bg-primary text-white p-3">Quarter 1</div>
    <div class="col-3 bg-secondary text-white p-3">Quarter 2</div>
  </div>
</div>
```

The left half of the page contains two nested rows: one full-width and one split into thirds.

### Removing Gutters on Nested Rows

Nested rows inherit the default gutter spacing. Use `.g-0` to remove gutters when nesting creates excessive padding.

```html
<div class="container">
  <div class="row g-2">
    <div class="col-8">
      <div class="row g-0">
        <div class="col-6"><div class="bg-light p-3 border">No nested gutter</div></div>
        <div class="col-6"><div class="bg-light p-3 border">No nested gutter</div></div>
      </div>
    </div>
    <div class="col-4"><div class="bg-secondary p-3 text-white">Sidebar</div></div>
  </div>
</div>
```

The outer row has small gutters (`.g-2`), while the nested row has none (`.g-0`).

### Equal-Width Nested Columns

Using `.col` without a number inside a nested row creates equal-width columns that share the parent's space.

```html
<div class="container">
  <div class="row">
    <div class="col-10">
      <div class="row">
        <div class="col bg-success text-white p-3">Equal A</div>
        <div class="col bg-success text-white p-3">Equal B</div>
        <div class="col bg-success text-white p-3">Equal C</div>
        <div class="col bg-success text-white p-3">Equal D</div>
      </div>
    </div>
    <div class="col-2 bg-light p-3">Narrow</div>
  </div>
</div>
```

Four equal columns inside the `.col-10` parent, each occupying 25% of the parent (2.5/12 of the full width).

## Advanced Variations

### Multi-Level Nesting (Two Levels Deep)

Place a nested row inside a nested column for a third level of subdivision.

```html
<div class="container">
  <div class="row">
    <div class="col-8">
      <div class="row">
        <div class="col-6">
          <div class="row g-1">
            <div class="col-4 bg-danger text-white p-2">Deep A</div>
            <div class="col-4 bg-danger text-white p-2">Deep B</div>
            <div class="col-4 bg-danger text-white p-2">Deep C</div>
          </div>
        </div>
        <div class="col-6 bg-warning p-3">Level 2</div>
      </div>
    </div>
    <div class="col-4 bg-info text-white p-3">Level 1 sidebar</div>
  </div>
</div>
```

Three levels of nesting: the outer row, the first nested row, and the second nested row inside `.col-6`.

### Nested Responsive Grids

Apply breakpoint classes to nested columns for responsive sub-layouts.

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-lg-8">
      <div class="row">
        <div class="col-12 col-md-6 bg-light p-3">
          Nested — full on mobile, half on md
        </div>
        <div class="col-12 col-md-6 bg-light p-3">
          Nested — full on mobile, half on md
        </div>
      </div>
    </div>
    <div class="col-12 col-lg-4 bg-secondary text-white p-3">
      Sidebar — stacks on mobile
    </div>
  </div>
</div>
```

The nested columns are responsive within the parent's responsive width.

### Nesting with row-cols-* Utilities

Apply `row-cols-*` to nested rows for automatic column distribution.

```html
<div class="container">
  <div class="row">
    <div class="col-9">
      <h5>Product Grid</h5>
      <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-3">
        <div class="col"><div class="card h-100">Product 1</div></div>
        <div class="col"><div class="card h-100">Product 2</div></div>
        <div class="col"><div class="card h-100">Product 3</div></div>
        <div class="col"><div class="card h-100">Product 4</div></div>
        <div class="col"><div class="card h-100">Product 5</div></div>
        <div class="col"><div class="card h-100">Product 6</div></div>
      </div>
    </div>
    <div class="col-3">
      <div class="list-group">
        <a class="list-group-item">Category A</a>
        <a class="list-group-item">Category B</a>
      </div>
    </div>
  </div>
</div>
```

The nested product grid adapts its column count at each breakpoint while the parent two-column layout remains intact.

### Nesting for Dashboard Layouts

Dashboard layouts frequently require nesting to create panels of varying sizes.

```html
<div class="container-fluid">
  <div class="row">
    <div class="col-md-3 col-lg-2 bg-dark text-white p-3 min-vh-100">
      Sidebar Navigation
    </div>
    <div class="col-md-9 col-lg-10">
      <div class="row g-3 p-3">
        <div class="col-12 col-lg-8">
          <div class="row g-3">
            <div class="col-6"><div class="card p-3">Chart 1</div></div>
            <div class="col-6"><div class="card p-3">Chart 2</div></div>
            <div class="col-12"><div class="card p-3">Data Table</div></div>
          </div>
        </div>
        <div class="col-12 col-lg-4">
          <div class="card p-3">Notifications</div>
          <div class="card p-3 mt-3">Quick Stats</div>
        </div>
      </div>
    </div>
  </div>
</div>
```

Two levels of nesting create a sidebar, a chart area with subdivisions, and a notification panel.

## Best Practices

1. **Limit nesting to 2-3 levels** — Deeper nesting creates unreadable markup and fragile layouts.
2. **Always apply `.g-0` or explicit gutter classes to nested rows** — Default gutters compound at each nesting level, creating excessive padding.
3. **Recalculate column widths mentally** — A nested `.col-6` inside a `.col-8` is 50% of 66.67%, not 50% of 100%.
4. **Use nesting for genuine sub-layouts** — If a flat row achieves the same result, avoid nesting.
5. **Apply responsive classes to nested columns** — Nested grids should be responsive just like top-level grids.
6. **Keep nested markup visually indented** — Proper indentation makes multi-level nesting navigable.
7. **Avoid mixing nesting with complex ordering** — Combining `order-*` utilities with deeply nested grids produces confusing behavior.
8. **Use semantic elements inside nested columns** — `<section>`, `<article>`, and `<aside>` provide structure beyond visual layout.
9. **Test nested layouts at all breakpoints** — Nested columns may overflow or stack unexpectedly at narrow widths.
10. **Remove unnecessary nesting wrappers** — If a nested row contains only one `.col-12`, eliminate the nesting and place content directly.

## Common Pitfalls

1. **Forgetting that nested columns measure against the parent** — A nested `.col-12` is 100% of the parent column, not 100% of the viewport. Misunderstanding this leads to unexpectedly narrow layouts.

2. **Not removing gutters on nested rows** — Default gutters (0.75rem padding on each side) compound with each nesting level. A column nested two levels deep may have 1.5rem+ of cumulative padding.

3. **Placing a nested `.row` without a `.col-*` wrapper** — The nested row must be the direct child of a `.col-*` element from the parent row. Placing it as a sibling of columns breaks the layout.

4. **Exceeding 12 units in a nested row** — Nested rows follow the same 12-unit rule. Overflows wrap within the parent column's width.

5. **Applying viewport-level responsive logic to nested columns** — Nested `.col-md-6` references the viewport breakpoint (≥768px), not the parent column's width. A parent that is already narrow at `md` may cause nested columns to appear cramped.

6. **Duplicating container classes inside nested grids** — Do not add `.container` inside a nested column. The top-level container manages max-width; nested structures should only use `.row` and `.col-*`.

## Accessibility Considerations

- Use semantic HTML within nested columns. A nested grid that subdivides a `<main>` region should use `<section>` or `<article>` for its children.
- Ensure that DOM order within nested grids reflects the logical reading order. Deep nesting can cause screen reader content to be announced in an unintuitive sequence.
- Add `aria-label` attributes to nested landmark regions to distinguish them from parent landmarks (e.g., `aria-label="Product details section"` within a broader `<main>` region).
- Avoid hiding essential content within deeply nested columns that may be difficult for assistive technology users to locate.
- When nesting creates complex visual layouts, provide a skip navigation link that lets keyboard users bypass repetitive nested structures.

## Responsive Behavior

Nested columns inherit the responsive breakpoint system. A nested `.col-md-6` activates at the same `md` breakpoint (≥768px) as a top-level `.col-md-6`. The difference is the reference width — nested columns calculate percentages based on their parent column's rendered width.

At narrow viewports, deeply nested columns may become impractically small. For example, a `.col-4` inside a `.col-4` inside a `.col-4` occupies only 11% of the viewport width (4/12 × 4/12 × 4/12). In such cases, use responsive classes to stack nested columns on small screens (`.col-12 .col-md-4`).

When a parent column collapses to full-width on mobile, its nested grid also expands to full-width, providing adequate space for nested columns. This natural behavior makes nesting viable even for mobile-first designs.
