---
title: "Row Columns Class"
topic: "Grid Deep Dive"
subtopic: "Row Columns Class"
difficulty: 1
duration: "25 minutes"
prerequisites: ["Equal Width Columns", "Responsive Breakpoints"]
learning_objectives:
  - Use row-cols classes to define automatic column counts
  - Apply responsive row-cols for breakpoint-based wrapping
  - Understand automatic column wrapping behavior
---

## Overview

The `row-cols-*` classes define how many columns appear per row automatically, eliminating the need to specify widths on each individual column. Instead of adding `col-6` to every column for two-per-row layout, you add `row-cols-2` to the parent `.row`. Bootstrap supports `row-cols-1` through `row-cols-6`, and responsive variants like `row-cols-md-3`.

This is especially useful for card grids, feature lists, and any repeating content pattern where columns should distribute evenly without manual sizing.

## Basic Implementation

Two columns per row using `row-cols-2`:

```html
<div class="container">
  <div class="row row-cols-2">
    <div class="col">
      <div class="bg-light p-3 border">Column 1</div>
    </div>
    <div class="col">
      <div class="bg-light p-3 border">Column 2</div>
    </div>
    <div class="col">
      <div class="bg-light p-3 border">Column 3</div>
    </div>
    <div class="col">
      <div class="bg-light p-3 border">Column 4</div>
    </div>
  </div>
</div>
```

Three columns per row, ideal for card layouts:

```html
<div class="container">
  <div class="row row-cols-3 g-4">
    <div class="col">
      <div class="card h-100">
        <div class="card-body">
          <h5 class="card-title">Card 1</h5>
          <p class="card-text">Content here.</p>
        </div>
      </div>
    </div>
    <div class="col">
      <div class="card h-100">
        <div class="card-body">
          <h5 class="card-title">Card 2</h5>
          <p class="card-text">Content here.</p>
        </div>
      </div>
    </div>
    <div class="col">
      <div class="card h-100">
        <div class="card-body">
          <h5 class="card-title">Card 3</h5>
          <p class="card-text">Content here.</p>
        </div>
      </div>
    </div>
  </div>
</div>
```

Single column per row for stacked layouts:

```html
<div class="container">
  <div class="row row-cols-1 g-3">
    <div class="col">
      <div class="alert alert-primary">Full-width row 1</div>
    </div>
    <div class="col">
      <div class="alert alert-secondary">Full-width row 2</div>
    </div>
  </div>
</div>
```

## Advanced Variations

Responsive row-cols that change column count at different breakpoints:

```html
<div class="container">
  <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
    <div class="col">
      <div class="card h-100">
        <div class="card-body">Card A</div>
      </div>
    </div>
    <div class="col">
      <div class="card h-100">
        <div class="card-body">Card B</div>
      </div>
    </div>
    <div class="col">
      <div class="card h-100">
        <div class="card-body">Card C</div>
      </div>
    </div>
    <div class="col">
      <div class="card h-100">
        <div class="card-body">Card D</div>
      </div>
    </div>
    <div class="col">
      <div class="card h-100">
        <div class="card-body">Card E</div>
      </div>
    </div>
    <div class="col">
      <div class="card h-100">
        <div class="card-body">Card F</div>
      </div>
    </div>
  </div>
</div>
```

Combining `row-cols` with explicit column widths for mixed layouts:

```html
<div class="container">
  <div class="row row-cols-3 g-3">
    <div class="col">
      <div class="bg-primary text-white p-3">Auto 1/3</div>
    </div>
    <div class="col">
      <div class="bg-secondary text-white p-3">Auto 1/3</div>
    </div>
    <div class="col-6">
      <div class="bg-success text-white p-3">Explicit half width</div>
    </div>
  </div>
</div>
```

Five columns per row on large screens for icon grids:

```html
<div class="container">
  <div class="row row-cols-2 row-cols-lg-5 g-3 text-center">
    <div class="col"><div class="bg-light p-3 rounded">Feature 1</div></div>
    <div class="col"><div class="bg-light p-3 rounded">Feature 2</div></div>
    <div class="col"><div class="bg-light p-3 rounded">Feature 3</div></div>
    <div class="col"><div class="bg-light p-3 rounded">Feature 4</div></div>
    <div class="col"><div class="bg-light p-3 rounded">Feature 5</div></div>
  </div>
</div>
```

## Best Practices

1. Use `row-cols-*` for repeating content patterns like card grids to avoid adding sizing classes to every column.
2. Always pair `row-cols` with `g-*` gutter classes for consistent spacing between columns.
3. Apply responsive `row-cols-{breakpoint}-*` to change column counts at different viewports.
4. Use `row-cols-1` as the mobile default, then progressively add columns at larger breakpoints.
5. Combine `row-cols` with `h-100` on cards to achieve equal-height card layouts.
6. Keep the maximum `row-cols` value practical — `row-cols-6` produces very narrow columns on most screens.
7. Use `row-cols-auto` (Bootstrap 5.3+) when you want columns to size based on content within the row-cols context.
8. Test with varying numbers of columns to ensure the last row handles orphan columns gracefully.
9. Use `row-cols-*` inside nested grid structures for complex dashboard layouts.
10. Document the expected column count in component comments for maintainability.

## Common Pitfalls

- **Forgetting gutters**: Using `row-cols-3` without `g-*` classes creates columns with no spacing between them.
- **Orphan columns on the last row**: With `row-cols-3` and 5 items, the last row has 2 columns that stretch to fill space — add `w-auto` or use flex utilities to control this.
- **Not using `h-100` on cards**: Cards in `row-cols` rows may have unequal heights without `h-100` on each card.
- **Overriding column classes**: Adding `col-12` to columns inside `row-cols-2` negates the automatic sizing.
- **Ignoring responsive behavior**: `row-cols-4` without responsive variants produces 4 narrow columns on mobile.
- **Nesting conflicts**: Applying `row-cols` on a nested row may conflict with the parent row's column count.
- **Too many columns**: `row-cols-6` on a mobile screen makes content unreadable.

## Accessibility Considerations

- Use semantic list markup (`<ul>` with `role="list"`) when `row-cols` creates a list of items.
- Ensure columns maintain readable content width at the chosen `row-cols` count.
- Provide `aria-label` attributes on rows that contain distinct groups of content.
- Maintain logical reading order — `row-cols` wraps visually but screen readers follow DOM order.
- Avoid using `row-cols` purely for visual layout when content is semantically a single unit.
- Ensure sufficient contrast on card content within row-cols grids.

## Responsive Behavior

The power of `row-cols` is responsive adaptation. A common pattern shows 1 column on mobile, 2 on tablet, 3 on desktop, and 4 on large screens:

```html
<div class="container">
  <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-xl-4 g-4">
    <div class="col"><div class="card h-100"><div class="card-body">Item</div></div></div>
    <div class="col"><div class="card h-100"><div class="card-body">Item</div></div></div>
    <div class="col"><div class="card h-100"><div class="card-body">Item</div></div></div>
    <div class="col"><div class="card h-100"><div class="card-body">Item</div></div></div>
    <div class="col"><div class="card h-100"><div class="card-body">Item</div></div></div>
    <div class="col"><div class="card h-100"><div class="card-body">Item</div></div></div>
  </div>
</div>
```

Each breakpoint class overrides the previous one, so `row-cols-sm-2` takes effect at 576px+, `row-cols-md-3` at 768px+, and `row-cols-xl-4` at 1200px+.
