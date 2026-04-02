---
title: Column Sizing
category: Layout System
difficulty: 1
time: 15 min
tags: bootstrap5, grid, column-sizing, responsive, breakpoints
---

## Overview

Column sizing in Bootstrap 5 uses `col-{n}` classes where `n` is a number from 1 to 12, representing fractions of the 12-column grid. A `col-6` occupies half the row width, `col-4` takes one-third, and `col-12` spans the full width. These sizes are mobile-first: `col-{n}` applies at all screen sizes, while `col-{breakpoint}-{n}` applies only at the specified breakpoint and above. Combining base and responsive column classes gives complete control over layout at every viewport size.

## Basic Implementation

Explicit column sizes divide the row into predictable fractions of the 12-unit grid.

```html
<!-- Fixed column widths -->
<div class="container">
  <div class="row">
    <div class="col-3">25% width</div>
    <div class="col-6">50% width</div>
    <div class="col-3">25% width</div>
  </div>
</div>
```

The `col-auto` class sizes a column to fit its content rather than taking a fraction of the grid.

```html
<!-- Combining fixed and auto columns -->
<div class="container">
  <div class="row">
    <div class="col-auto">
      <nav class="nav">Fixed Nav</nav>
    </div>
    <div class="col-8">
      Main content taking 8/12
    </div>
    <div class="col-auto">
      <span>Info</span>
    </div>
  </div>
</div>
```

Responsive column classes stack with base classes to change sizing at specific breakpoints.

```html
<!-- Responsive column sizing -->
<div class="container">
  <div class="row">
    <div class="col-12 col-md-6 col-lg-4">
      Full width on mobile, half on tablet, third on desktop
    </div>
    <div class="col-12 col-md-6 col-lg-4">
      Full width on mobile, half on tablet, third on desktop
    </div>
    <div class="col-12 col-lg-4">
      Full width below large, one-third on desktop
    </div>
  </div>
</div>
```

## Advanced Variations

You can combine fixed-width columns with remaining-space columns by mixing `col-{n}` and `col` in the same row.

```html
<!-- Fixed sidebar with flexible main content -->
<div class="container">
  <div class="row">
    <div class="col-3">Sidebar (fixed 25%)</div>
    <div class="col">Main (fills remaining 75%)</div>
  </div>
</div>
```

Multiple responsive breakpoints can refine the layout progressively across screen sizes.

```html
<!-- Multi-breakpoint column refinement -->
<div class="container">
  <div class="row g-3">
    <div class="col-12 col-sm-6 col-md-4 col-xl-3">
      Scales: 100% → 50% → 33% → 25%
    </div>
    <div class="col-12 col-sm-6 col-md-4 col-xl-3">
      Scales: 100% → 50% → 33% → 25%
    </div>
    <div class="col-12 col-sm-6 col-md-4 col-xl-3">
      Scales: 100% → 50% → 33% → 25%
    </div>
    <div class="col-12 col-sm-6 col-xl-3">
      Scales: 100% → 50% → 100% → 25%
    </div>
  </div>
</div>
```

Using `col-{breakpoint}-auto` at specific breakpoints lets columns shrink to content width only at certain screen sizes.

```html
<!-- Responsive auto sizing -->
<div class="container">
  <div class="row">
    <div class="col-12 col-md-auto">
      Full width on mobile, content-width on medium+
    </div>
    <div class="col-12 col-md">
      Full width on mobile, fills remaining space on medium+
    </div>
  </div>
</div>
```

## Best Practices

1. Always provide a base `col-{n}` or `col-12` class so columns have a defined size below the smallest responsive breakpoint.
2. Use responsive column classes progressively: `col-12 col-md-6 col-lg-4` enhances layout at each breakpoint.
3. Combine `col-{n}` with `col` (auto-fill) when one column needs a fixed width and another should take remaining space.
4. Avoid setting all columns to explicit widths that sum to less than 12 — remaining space appears as a gap on the right.
5. Use `col-auto` for content-driven widths like buttons, avatars, and timestamps within a sized row.
6. Apply `g-*` gutter utilities on the row for consistent spacing regardless of column size.
7. Test every breakpoint combination to verify the layout transitions are smooth and content remains readable.
8. Prefer `col-12` as the mobile base for most content columns so they stack vertically on small screens.
9. Use `offset-{n}` alongside sized columns to create indented layouts without empty spacer columns.
10. Keep total column units per row at or below 12 to prevent unwanted wrapping.

## Common Pitfalls

1. **No base class**: Using only `col-md-6` without `col-12` leaves columns unstyled below the medium breakpoint, causing unpredictable widths.
2. **Columns exceeding 12**: A row with `col-8` and `col-6` totals 14 units, forcing the second column to wrap.
3. **Ignoring the mobile-first cascade**: Defining `col-lg-4` and expecting it to apply at medium screens — it only activates at large and above.
4. **Fixed widths that break on mobile**: A `col-4` works on desktop but creates cramped three-column layouts on small screens.
5. **Mixing percentage widths manually**: Adding inline `width: 30%` alongside `col-8` overrides Bootstrap's calculated widths and breaks the grid alignment.
6. **Forgetting that col-12 is 100% width**: Wrapping `col-12` elements inside a row and expecting horizontal layout — they each take the full row.
7. **Responsive auto conflicts**: Using `col-md-auto` alongside `col-md-6` on the same element creates conflicting flex rules.

## Accessibility Considerations

Column sizing does not affect DOM order, so screen readers always follow the HTML sequence. Ensure that the visual layout created by sized columns does not create a reading order that conflicts with the content's logical flow. When columns stack on mobile (via `col-12`), verify that the stacked order matches the intended information hierarchy. Maintain adequate text size and line length within narrow columns — a `col-2` on a small screen may render text too narrow for comfortable reading.

## Responsive Behavior

Bootstrap's mobile-first approach means `col-{n}` applies at all sizes, while `col-{breakpoint}-{n}` overrides at and above that breakpoint. To create a three-column desktop layout that stacks on mobile, use `col-12 col-md-4` on each column. To have two columns on tablet and three on desktop, combine `col-12 col-sm-6 col-lg-4`. The grid recalculates column widths at each breakpoint transition, so a `col-6` at medium is 50% of the medium container width, not the viewport width. Gutter values scale with the CSS custom property `--bs-gutter-x` at each breakpoint when using responsive `g-{breakpoint}-{n}` classes.
