---
tags: [bootstrap5, grid, rows, columns, containers, layout]
category: Bootstrap Fundamentals
difficulty: 1
estimated_time: 25 minutes
---

# Row and Column Containers

## Overview

The Bootstrap 5 grid is a hierarchy of three core elements: the container, the row, and the column. Each serves a distinct structural purpose. The container establishes the viewport context and max-width constraints. The row creates a horizontal grouping of columns with negative margin compensation. The column holds actual content and provides gutter spacing through padding.

Understanding how `.row` and column containers interact is essential for predictable layouts. The `.row` element is a Flexbox container that enables columns to sit side by side. It applies negative margins (`margin-left: -0.75rem; margin-right: -0.75rem` by default) to counteract the padding that columns apply (`padding-left: 0.75rem; padding-right: 0.75rem`). This technique creates consistent gutters between columns while ensuring content aligns flush with the container edges.

Beyond basic rows, Bootstrap provides `row-cols-*` utilities to control how many columns appear per row automatically, and the `.g-0` class family to remove gutters entirely. Column wrapping — where columns that exceed 12 units flow to the next line — is managed entirely by the row's Flexbox context.

## Basic Implementation

### Standard Row and Column Structure

The fundamental grid pattern nests columns inside a row inside a container.

```html
<div class="container">
  <div class="row">
    <div class="col-4">Column A</div>
    <div class="col-4">Column B</div>
    <div class="col-4">Column C</div>
  </div>
</div>
```

Each `.col-4` occupies one-third of the row. The `.row` flexbox context places them horizontally, and gutters are automatically applied.

### Multiple Rows

Stack multiple `.row` elements to create distinct horizontal sections.

```html
<div class="container">
  <div class="row">
    <div class="col-12">Full-width header</div>
  </div>
  <div class="row">
    <div class="col-8">Main content</div>
    <div class="col-4">Sidebar</div>
  </div>
  <div class="row">
    <div class="col-12">Full-width footer</div>
  </div>
</div>
```

Each row independently manages its own 12-column grid.

### Column Wrapping Within a Row

When columns within a single row exceed 12 total units, the overflow wraps to a new visual line while remaining inside the same `.row` element.

```html
<div class="container">
  <div class="row">
    <div class="col-9">First line: 9 units</div>
    <div class="col-4">Wraps: 4 units (9+4 > 12)</div>
    <div class="col-4">Next: 4 units</div>
  </div>
</div>
```

The second `.col-4` wraps because 9 + 4 = 13, exceeding the 12-unit limit.

### Removing Gutters with g-0

The `.g-0` class removes all gutters (both horizontal and vertical) from a row.

```html
<div class="container">
  <div class="row g-0">
    <div class="col-6">
      <div class="p-3 bg-primary text-white">No gutter left</div>
    </div>
    <div class="col-6">
      <div class="p-3 bg-secondary text-white">No gutter right</div>
    </div>
  </div>
</div>
```

Content in adjacent columns will touch with no gap. Use inner padding (`.p-3`) on content wrappers to reintroduce spacing selectively.

### Using row-cols-* for Automatic Column Count

The `row-cols-{n}` classes automatically distribute children into `n` columns per row without requiring individual `.col-{n}` classes.

```html
<div class="container">
  <div class="row row-cols-3">
    <div class="col">Auto 1</div>
    <div class="col">Auto 2</div>
    <div class="col">Auto 3</div>
    <div class="col">Auto 4</div>
    <div class="col">Auto 5</div>
    <div class="col">Auto 6</div>
  </div>
</div>
```

Each `.col` automatically occupies one-third of the row. Items 4-6 wrap to a second line.

## Advanced Variations

### Responsive row-cols-* Classes

Combine `row-cols-*` with breakpoint suffixes to change column counts at different viewport sizes.

```html
<div class="container">
  <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-4">
    <div class="col">
      <div class="card h-100">Card 1</div>
    </div>
    <div class="col">
      <div class="card h-100">Card 2</div>
    </div>
    <div class="col">
      <div class="card h-100">Card 3</div>
    </div>
    <div class="col">
      <div class="card h-100">Card 4</div>
    </div>
  </div>
</div>
```

This stacks cards on mobile, shows 2 per row on small screens, and 4 per row on large screens.

### Mixing row-cols with Explicit Column Sizes

You can override `row-cols-*` defaults on specific columns using explicit `.col-{n}` classes.

```html
<div class="container">
  <div class="row row-cols-3">
    <div class="col">Auto third</div>
    <div class="col-6">Explicit half</div>
    <div class="col">Auto fills remaining</div>
  </div>
</div>
```

The `.col-6` overrides the row-level 3-column distribution for that specific column.

### Nesting Rows Inside Columns

A column can contain a new `.row`, creating a nested grid with its own 12-column system.

```html
<div class="container">
  <div class="row">
    <div class="col-8">
      <div class="row row-cols-2 g-3">
        <div class="col">Nested A</div>
        <div class="col">Nested B</div>
        <div class="col">Nested C</div>
        <div class="col">Nested D</div>
      </div>
    </div>
    <div class="col-4">Sidebar content</div>
  </div>
</div>
```

The nested `.row-cols-2` creates a 2-column grid within the 8-unit parent column.

### Rows Without Containers

While not recommended for full-page layouts, rows can exist without a `.container` for edge-to-edge designs.

```html
<div class="row g-0">
  <div class="col-6 bg-primary p-5 text-white">Full bleed left</div>
  <div class="col-6 bg-dark p-5 text-white">Full bleed right</div>
</div>
```

This produces a section that extends to the viewport edges with no horizontal padding.

## Best Practices

1. **Always use `.row` as the direct parent of `.col-*` classes** — This ensures proper negative margin compensation and Flexbox alignment.
2. **Use `.container` for page-level layouts** — It provides max-width constraints and horizontal centering at each breakpoint.
3. **Use `.container-fluid` for full-width layouts** — When content should span the entire viewport width.
4. **Apply `row-cols-*` for uniform card or item grids** — It eliminates the need to repeat `.col-{n}` on every child.
5. **Remove gutters with `.g-0` for edge-to-edge designs** — Then add inner padding to content wrappers as needed.
6. **Do not place non-column elements as direct children of `.row`** — Only `.col-*`, `.g-*` gutter wrappers, or column-break elements belong directly in a row.
7. **Use `row-cols-auto` for content-driven sizing** — When you want columns to size naturally without fixed proportions.
8. **Limit nesting depth to 2-3 levels** — Deep nesting creates overly complex markup and makes responsive behavior harder to manage.
9. **Combine `row-cols-*` with responsive breakpoints** — Avoid hardcoding a single column count that does not adapt to screen size.
10. **Use `.row-cols-1` as the mobile default** — Single-column stacking is the most common mobile pattern.
11. **Apply consistent gutter classes across rows** — Mixing `.g-0` and `.g-4` in adjacent rows creates visual inconsistency.

## Common Pitfalls

1. **Placing content directly inside `.row`** — Without a `.col-*` wrapper, content lacks gutter padding and may overflow. Always wrap content in a column class.

2. **Forgetting that `.row` has negative margins** — The `-0.75rem` margins on `.row` compensate for column padding. Without a `.container` parent, these negative margins can cause horizontal scrollbars.

3. **Assuming `row-cols-*` sets fixed widths** — `row-cols-3` creates three equal Flexbox items. If you add a fourth child, it wraps — it does not squeeze into the row.

4. **Not using `.g-0` when nesting rows** — Nested rows inherit parent gutters, creating double padding. Apply `.g-0` to nested rows to reset.

5. **Overriding row display properties** — Changing `.row` from `display: flex` to `display: grid` or `display: block` breaks the entire grid system.

6. **Wrapping columns in extra divs** — Adding a `<div>` between `.row` and `.col-*` breaks the Flexbox relationship. Columns must be direct children of the row.

7. **Mixing `.container` and `.container-fluid` inconsistently** — Choose one approach per page section. Mixing them creates unpredictable max-width behavior.

## Accessibility Considerations

- Use semantic landmarks (`<header>`, `<main>`, `<aside>`, `<footer>`) as row or container elements when they represent page regions.
- Ensure that the visual column order matches the DOM order or use `order-*` utilities and ARIA to reconcile differences for screen reader users.
- Avoid using `.row` and `.col-*` as the only structural cues — supplement with `role` attributes when the grid serves a non-obvious layout purpose.
- When using `row-cols-*` to create grid listings, wrap the row in a `<section>` with an `aria-label` describing the collection (e.g., "Product catalog").
- Maintain a logical tab order. Grid layouts that visually rearrange columns can confuse keyboard users if DOM order diverges significantly.

## Responsive Behavior

Rows and columns adapt to their container's width at each breakpoint. Inside a `.container`, the max-width changes at `sm` (540px), `md` (720px), `lg` (960px), `xl` (1140px), and `xxl` (1320px). Columns respect these boundaries and reflow accordingly.

The `row-cols-*` utilities accept breakpoint suffixes (`row-cols-md-3`) to change column counts responsively. Without a suffix, the column count applies at all viewport sizes. At breakpoints below the specified suffix, columns fall back to single-column stacking unless overridden by a smaller breakpoint class.

Column wrapping within rows is purely mathematical — when the sum of column units in a logical group exceeds 12, the overflow wraps. This behavior is consistent across all breakpoints unless individual column widths change via responsive classes.
