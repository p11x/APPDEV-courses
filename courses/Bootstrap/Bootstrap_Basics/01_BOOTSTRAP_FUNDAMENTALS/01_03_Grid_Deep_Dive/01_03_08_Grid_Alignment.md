---
tags: [bootstrap5, grid, alignment, flexbox, justify-content, align-items, layout]
category: Bootstrap Fundamentals
difficulty: 2
estimated_time: 35 minutes
---

# Grid Alignment

## Overview

Bootstrap's grid alignment system leverages CSS Flexbox properties to position columns within rows both horizontally (main axis) and vertically (cross axis). Alignment utilities control where columns sit when they do not consume the full row width or height.

Horizontal alignment uses the `.justify-content-*` family, which maps to Flexbox's `justify-content` property. Options include `start`, `center`, `end`, `around`, `between`, and `evenly`. These classes position columns relative to the row's main axis.

Vertical alignment uses the `.align-items-*` family for row-level control and `.align-self-*` for individual column control. Options include `start`, `center`, `end`, `baseline`, and `stretch`. Vertical alignment is particularly useful when columns contain content of varying heights.

All alignment classes accept breakpoint suffixes (`.justify-content-md-center`, `.align-items-lg-start`) for responsive alignment adjustments. This enables different alignment strategies at different viewport sizes.

Understanding grid alignment eliminates the need for custom CSS centering hacks and ensures consistent positioning across browsers.

## Basic Implementation

### Horizontal Alignment with justify-content

Center columns within a row when they occupy less than the full 12-unit width.

```html
<div class="container">
  <div class="row justify-content-center">
    <div class="col-4 bg-primary text-white p-3">Centered column</div>
  </div>
</div>
```

The `.justify-content-center` class centers the 4-unit column within the row, leaving equal space on both sides.

### Aligning Columns to the End

Push columns to the right edge of the row.

```html
<div class="container">
  <div class="row justify-content-end">
    <div class="col-4 bg-secondary text-white p-3">Right-aligned</div>
    <div class="col-4 bg-dark text-white p-3">Right-aligned</div>
  </div>
</div>
```

Both columns are pushed to the right. The empty space accumulates on the left.

### Distributing Columns with justify-content-between

Spread columns evenly with space between them.

```html
<div class="container">
  <div class="row justify-content-between">
    <div class="col-auto bg-light p-3">Left</div>
    <div class="col-auto bg-light p-3">Center</div>
    <div class="col-auto bg-light p-3">Right</div>
  </div>
</div>
```

Space is distributed between columns, with no space before the first or after the last.

### Vertical Alignment with align-items

Center columns vertically when they have different content heights.

```html
<div class="container">
  <div class="row align-items-center" style="min-height: 200px;">
    <div class="col-4 bg-primary text-white p-3">Short</div>
    <div class="col-4 bg-success text-white p-3">
      Taller content that spans<br>multiple lines of text<br>for demonstration
    </div>
    <div class="col-4 bg-info text-white p-3">Short</div>
  </div>
</div>
```

All columns align to the vertical center of the row regardless of their individual heights.

### Individual Column Alignment with align-self

Override the row's vertical alignment for a specific column.

```html
<div class="container">
  <div class="row align-items-start" style="min-height: 200px;">
    <div class="col-4 bg-light p-3">Top aligned (row default)</div>
    <div class="col-4 bg-light p-3 align-self-center">Center (override)</div>
    <div class="col-4 bg-light p-3 align-self-end">Bottom (override)</div>
  </div>
</div>
```

The row defaults to top alignment. Individual columns override to center and bottom.

## Advanced Variations

### Responsive Horizontal Alignment

Change horizontal alignment at different breakpoints.

```html
<div class="container">
  <div class="row justify-content-start justify-content-md-center justify-content-xl-end">
    <div class="col-auto bg-primary text-white p-3">
      Start on mobile, centered on md, end on xl
    </div>
    <div class="col-auto bg-secondary text-white p-3">
      Responsive alignment
    </div>
  </div>
</div>
```

The columns shift position at each breakpoint: left-aligned on mobile, centered on tablets, right-aligned on desktops.

### justify-content-around for Equal Spacing

Distribute space around columns, with half-space at the edges.

```html
<div class="container">
  <div class="row justify-content-around">
    <div class="col-2 bg-light p-3 text-center">1</div>
    <div class="col-2 bg-light p-3 text-center">2</div>
    <div class="col-2 bg-light p-3 text-center">3</div>
  </div>
</div>
```

Each column has equal space on both sides. Edge columns receive half the space of interior columns.

### justify-content-evenly for Perfect Distribution

Distribute space evenly, including edges.

```html
<div class="container">
  <div class="row justify-content-evenly">
    <div class="col-2 bg-primary text-white p-3 text-center">A</div>
    <div class="col-2 bg-primary text-white p-3 text-center">B</div>
    <div class="col-2 bg-primary text-white p-3 text-center">C</div>
  </div>
</div>
```

Every gap — between columns and at edges — is identical.

### Combining Vertical and Horizontal Alignment

Apply both alignment families to center columns perfectly within the row.

```html
<div class="container">
  <div class="row justify-content-center align-items-center" style="min-height: 300px;">
    <div class="col-4 bg-warning p-4 text-center">
      <h4>Perfectly Centered</h4>
      <p>Both horizontally and vertically.</p>
    </div>
  </div>
</div>
```

The column sits at the exact center of the 300px-tall row.

### Responsive Vertical Alignment

Change vertical alignment per breakpoint for adaptive layouts.

```html
<div class="container">
  <div class="row align-items-stretch align-items-md-center" style="min-height: 250px;">
    <div class="col-12 col-md-4 mb-3 mb-md-0">
      <div class="bg-light p-4 h-100">Card A</div>
    </div>
    <div class="col-12 col-md-4 mb-3 mb-md-0">
      <div class="bg-light p-4" style="height: 100px;">Card B (short)</div>
    </div>
    <div class="col-12 col-md-4">
      <div class="bg-light p-4 h-100">Card C</div>
    </div>
  </div>
</div>
```

On mobile, columns stretch to full width. At `md`, the short card centers vertically within the row.

### Alignment with Column Wrapping

Alignment utilities affect all lines when columns wrap.

```html
<div class="container">
  <div class="row justify-content-between align-items-end" style="min-height: 200px;">
    <div class="col-5 bg-light p-3">First line left</div>
    <div class="col-5 bg-light p-3">First line right</div>
    <div class="col-5 bg-light p-3">Second line (wraps)</div>
  </div>
</div>
```

The wrapped column respects the `justify-content-between` and `align-items-end` settings within its line context.

### Baseline Alignment

Align columns by their text baselines rather than their box edges.

```html
<div class="container">
  <div class="row align-items-baseline" style="min-height: 200px;">
    <div class="col-4 bg-light p-3">
      <h1>Large heading</h1>
    </div>
    <div class="col-4 bg-light p-3">
      <p>Body text baseline</p>
    </div>
    <div class="col-4 bg-light p-3">
      <small>Small text baseline</small>
    </div>
  </div>
</div>
```

All columns align so their first line of text shares the same baseline, regardless of font size differences.

## Best Practices

1. **Use `.justify-content-center` for single-column centering** — It is simpler and more maintainable than calculating offsets.
2. **Apply `.align-items-center` when columns have variable heights** — This prevents visual misalignment in content with mixed-length text.
3. **Use `.align-self-*` sparingly** — Override individual columns only when the row-level alignment does not meet the design need.
4. **Combine responsive alignment with responsive column widths** — Alignment at `lg` should complement the column layout at `lg`.
5. **Prefer `.justify-content-between` for navigation layouts** — Logo on the left, links on the right, with space distributed between them.
6. **Use `.justify-content-evenly` for icon rows or button groups** — Perfect spacing creates a balanced visual rhythm.
7. **Apply `min-height` to rows when testing vertical alignment** — Without explicit height, vertical alignment has no visible effect.
8. **Reset alignment at breakpoints when layouts change** — Use `.justify-content-md-start` to override a mobile-centered layout on desktop.
9. **Use `.align-items-stretch` to make columns equal height** — The default stretch behavior fills the row height, making all columns the same.
10. **Test alignment with realistic content** — Placeholder text behaves differently from production content with varying lengths and media.
11. **Do not use alignment to compensate for incorrect column widths** — Fix column sizing first, then apply alignment for fine-tuning.

## Common Pitfalls

1. **Vertical alignment has no visible effect without row height** — If the row's height equals the tallest column's height, all alignment values look the same. Add `min-height` to the row.

2. **Using `justify-content` on full-width columns** — When columns sum to 12 units, there is no remaining space to distribute. `justify-content` only affects gaps between columns.

3. **Confusing `align-items` with `align-content`** — `align-items` aligns individual columns within the row's cross axis. `align-content` aligns wrapped lines. Bootstrap provides `.align-content-*` for multi-line scenarios.

4. **Applying alignment classes to columns instead of rows** — `.justify-content-*` and `.align-items-*` belong on `.row` elements. Column-level alignment uses `.align-self-*` only.

5. **Not using breakpoint suffixes** — Alignment that works at desktop may not work at mobile. Responsive alignment classes prevent manual overrides.

6. **Expecting `justify-content-between` to work with a single column** — There is nothing to distribute space between with one column. Use `justify-content-center` instead.

7. **Overriding alignment at every breakpoint** — Excessive responsive alignment classes create maintenance burden. Set a sensible default and override only where needed.

8. **Using alignment instead of offsets for spacing** — Alignment positions all columns within the remaining space. Offsets push individual columns. Use the right tool for the task.

## Accessibility Considerations

- Alignment changes are purely visual. Screen readers and keyboard navigation are unaffected by `justify-content` or `align-items` classes.
- Ensure that vertically centered content remains readable at all viewport heights. Centering a column at the vertical middle of a 600px row may push content below the fold on short screens.
- When using `justify-content-between` to separate navigation items, ensure sufficient spacing between links for users with motor impairments. The gap between items should be at least 8px for comfortable tapping.
- Baseline alignment can cause content to shift vertically as font sizes change. Verify that dynamically sized content (user-generated text, translated strings) does not break the layout.
- Maintain consistent alignment patterns across pages. Users with cognitive disabilities benefit from predictable layouts where navigation and content are always in the same position.

## Responsive Behavior

All alignment classes accept breakpoint suffixes for responsive adjustments. The mobile-first principle applies: a class without a suffix applies at all sizes, while suffixed classes activate only at and above the specified breakpoint.

For example, `.justify-content-center .justify-content-lg-start` centers columns on mobile and small screens, then aligns them to the start at `lg` and above. This is useful for centering compact mobile layouts while distributing content across wider desktop screens.

Vertical alignment responds to the row's computed height at each breakpoint. If the row's height changes due to responsive column stacking, vertical alignment recalculates accordingly. A column that is centered vertically in a 300px desktop row may appear at the top of a 500px mobile stack where columns have more cumulative height.

The alignment system does not animate transitions between breakpoints — columns jump to their new positions instantly. If smooth alignment transitions are desired, custom CSS transitions on the `transform` or `margin` properties can be applied, though this is outside Bootstrap's built-in scope.
