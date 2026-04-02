---
title: "CSS Grid Basic Layouts"
description: "Learn grid-template-columns, grid-template-rows, gap, and the fr unit for building CSS Grid layouts with Bootstrap"
difficulty: 2
tags: [css-grid, layout, grid-template, fr-unit, gap]
prerequisites:
  - "CSS Grid vs Bootstrap Grid understanding"
  - "CSS box model fundamentals"
---

## Overview

CSS Grid's foundational properties define the structure of any grid layout. `grid-template-columns` creates column tracks with explicit sizes or flexible `fr` units. `grid-template-rows` defines row heights, from fixed pixels to auto-sized content. The `gap` property (supported natively in Bootstrap 5) controls spacing between grid items without margin hacks. Understanding these basics enables precise, two-dimensional layouts that complement Bootstrap's component system.

## Basic Implementation

### Fixed Column Grid

Explicit pixel or percentage columns create predictable layouts.

```html
<div class="grid-fixed">
  <div class="p-3 bg-light border">Column 1</div>
  <div class="p-3 bg-light border">Column 2</div>
  <div class="p-3 bg-light border">Column 3</div>
</div>

<style>
  .grid-fixed {
    display: grid;
    grid-template-columns: 200px 300px 200px;
    gap: 1rem;
  }
</style>
```

### Flexible `fr` Unit Grid

The `fr` unit distributes available space proportionally among columns.

```html
<div class="grid-fr">
  <div class="p-3 bg-primary text-white">1fr</div>
  <div class="p-3 bg-success text-white">2fr</div>
  <div class="p-3 bg-info text-white">1fr</div>
</div>

<style>
  .grid-fr {
    display: grid;
    grid-template-columns: 1fr 2fr 1fr;
    gap: 1rem;
  }
</style>
```

### Auto-Sized Rows

```html
<div class="grid-auto-rows">
  <div class="p-3 bg-light border">Short content</div>
  <div class="p-3 bg-light border">Much longer content that will cause this row to expand based on its content height.</div>
  <div class="p-3 bg-light border">Medium content here.</div>
</div>

<style>
  .grid-auto-rows {
    display: grid;
    grid-template-columns: 1fr 1fr;
    grid-template-rows: auto;
    gap: 1rem;
  }
</style>
```

## Advanced Variations

### Responsive Grid with `repeat()` and `minmax()`

Create responsive columns without media queries using `auto-fit` and `minmax()`.

```html
<div class="grid-responsive">
  <div class="card h-100"><div class="card-body">Item 1</div></div>
  <div class="card h-100"><div class="card-body">Item 2</div></div>
  <div class="card h-100"><div class="card-body">Item 3</div></div>
  <div class="card h-100"><div class="card-body">Item 4</div></div>
  <div class="card h-100"><div class="card-body">Item 5</div></div>
</div>

<style>
  .grid-responsive {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
    gap: 1.5rem;
  }
</style>
```

### Dashboard Layout with Mixed Units

Combine `fr`, pixels, and `auto` for complex layouts.

```html
<div class="dashboard-grid">
  <header class="bg-dark text-white p-3">Dashboard Header</header>
  <aside class="bg-light p-3">Navigation</aside>
  <main class="p-3">
    <div class="card mb-3"><div class="card-body">Chart Widget</div></div>
    <div class="card"><div class="card-body">Table Widget</div></div>
  </main>
  <section class="p-3 bg-body-secondary">
    <div class="card h-100"><div class="card-body">Activity Feed</div></div>
  </section>
  <footer class="bg-dark text-white p-3 text-center">Footer</footer>
</div>

<style>
  .dashboard-grid {
    display: grid;
    grid-template-columns: 250px 1fr 300px;
    grid-template-rows: auto 1fr auto;
    grid-template-areas:
      "header  header  header"
      "sidebar main    aside"
      "footer  footer  footer";
    min-height: 100vh;
    gap: 0;
  }
  .dashboard-grid header { grid-area: header; }
  .dashboard-grid aside:first-of-type { grid-area: sidebar; }
  .dashboard-grid main { grid-area: main; }
  .dashboard-grid section { grid-area: aside; }
  .dashboard-grid footer { grid-area: footer; }
</style>
```

### Equal-Height Card Grid

```html
<div class="card-grid">
  <div class="card"><div class="card-body">
    <h5>Card 1</h5>
    <p>Short description.</p>
  </div></div>
  <div class="card"><div class="card-body">
    <h5>Card 2</h5>
    <p>Much longer description that spans multiple lines to test equal height behavior in CSS Grid layouts.</p>
    <a href="#" class="btn btn-primary">Learn More</a>
  </div></div>
  <div class="card"><div class="card-body">
    <h5>Card 3</h5>
    <p>Medium description text.</p>
  </div></div>
</div>

<style>
  .card-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 1.5rem;
    align-items: start;
  }
  .card-grid .card { height: 100%; }
</style>
```

## Best Practices

1. **Use `fr` units** for flexible layouts instead of percentages when dividing available space.
2. **Combine `repeat(auto-fit, minmax())`** for responsive grids without media queries.
3. **Use `gap` property** instead of margin utilities for cleaner grid spacing.
4. **Set `grid-template-rows: auto`** when row heights should adapt to content.
5. **Use `minmax()`** to prevent grid items from collapsing below a minimum size.
6. **Combine `1fr` columns** with fixed sidebar widths for common dashboard layouts.
7. **Apply `grid-template-areas`** for complex layouts with named regions.
8. **Keep `gap` values consistent** with Bootstrap's spacing scale (0.5rem, 1rem, 1.5rem, etc.).
9. **Use `align-items: start`** on card grids to prevent stretched card heights when content varies.
10. **Test grid layouts** at multiple viewport sizes to verify `auto-fit` behavior.
11. **Use Bootstrap padding utilities** (`p-3`, `p-4`) inside grid items for consistent internal spacing.
12. **Combine fixed and flexible tracks** (`250px 1fr 300px`) for predictable sidebar layouts.

## Common Pitfalls

1. **Using `auto-fill` when `auto-fit` is needed** - `auto-fill` creates empty tracks, `auto-fit` collapses them.
2. **Forgetting `minmax()`** with `auto-fit` causes items to stretch infinitely on wide screens.
3. **Mixing pixel and `fr` units** without understanding how remaining space is calculated.
4. **Not setting `overflow: auto`** on grid items that may contain long content.
5. **Using grid for single-axis layouts** where flexbox would be simpler.
6. **Hardcoding too many columns** instead of using `repeat()` for maintainability.
7. **Ignoring the `gap` shorthand** - use `gap: row-gap column-gap` for asymmetric spacing.
8. **Not providing `min-height`** on grid containers, causing them to collapse on empty content.

## Accessibility Considerations

- CSS Grid visual reordering must not break logical DOM reading order.
- Use `grid-template-areas` for logical region naming that aligns with ARIA landmarks.
- Ensure keyboard focus follows visual grid order when items are reordered.
- Provide meaningful headings within grid regions for screen reader navigation.
- Grid layouts should degrade gracefully to single-column when CSS is unsupported.

## Responsive Behavior

- `repeat(auto-fit, minmax(280px, 1fr))` automatically adjusts column count based on available width.
- Override grid templates in media queries for mobile-specific layouts.
- Use `grid-template-columns: 1fr` in mobile breakpoint to stack all items.
- Combine CSS Grid with Bootstrap's `d-*` utilities for breakpoint-specific visibility.
- Test `gap` values on small screens - large gaps waste valuable mobile screen space.
