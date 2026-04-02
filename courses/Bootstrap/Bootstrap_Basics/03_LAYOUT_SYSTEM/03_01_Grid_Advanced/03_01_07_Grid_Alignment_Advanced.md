---
title: Grid Alignment Advanced
category: Layout System
difficulty: 2
time: 25 min
tags: bootstrap5, grid, alignment, justify-content, align-items, flexbox
---

## Overview

Bootstrap 5's grid system exposes flexbox alignment utilities that control how columns distribute horizontally (`justify-content-*`) and vertically (`align-items-*`, `align-self-*`) within their row. These utilities work because every `row` is a flex container with `display: flex` and `flex-wrap: wrap`. Mastering alignment utilities eliminates the need for custom CSS to center content, distribute space evenly, and create vertically balanced column layouts — all within the standard grid classes.

## Basic Implementation

`justify-content-*` controls horizontal distribution of columns within a row. It applies to the entire row and affects all columns collectively.

```html
<!-- Horizontal alignment with justify-content -->
<div class="container">
  <div class="row justify-content-center">
    <div class="col-4">Centered column</div>
    <div class="col-4">Centered column</div>
  </div>
</div>

<div class="container">
  <div class="row justify-content-between">
    <div class="col-3">Left</div>
    <div class="col-3">Right</div>
  </div>
</div>
```

`align-items-*` controls vertical alignment of all columns within a row. Common values include `start`, `center`, `end`, and `stretch`.

```html
<!-- Vertical alignment with align-items -->
<div class="container">
  <div class="row align-items-center" style="min-height: 200px;">
    <div class="col-4">Vertically centered</div>
    <div class="col-4">Also centered</div>
    <div class="col-4">Me too</div>
  </div>
</div>
```

`align-self-*` overrides vertical alignment for individual columns, letting one column break from the row's default alignment.

```html
<!-- Individual column alignment with align-self -->
<div class="container">
  <div class="row align-items-start" style="min-height: 200px;">
    <div class="col-4 align-self-end">Pushed to bottom</div>
    <div class="col-4">Stays at top</div>
    <div class="col-4 align-self-center">Centered</div>
  </div>
</div>
```

## Advanced Variations

Responsive alignment classes apply alignment changes only at specific breakpoints and above.

```html
<!-- Responsive alignment -->
<div class="container">
  <div class="row justify-content-start justify-content-md-center justify-content-lg-end g-3">
    <div class="col-auto">Left on mobile, centered on tablet, right on desktop</div>
    <div class="col-auto">Same alignment</div>
  </div>
</div>
```

Combining `align-items-center` with `min-height` creates vertically centered content panels — a common hero section pattern.

```html
<!-- Hero section with vertical centering -->
<div class="container">
  <div class="row align-items-center" style="min-height: 80vh;">
    <div class="col-lg-6">
      <h1>Headline</h1>
      <p>Subheadline text vertically centered in viewport</p>
    </div>
    <div class="col-lg-6">
      <img src="hero.png" class="img-fluid" alt="Hero image">
    </div>
  </div>
</div>
```

Using `justify-content-evenly` or `justify-content-around` distributes columns with equal or proportional spacing, useful for icon rows and navigation bars.

```html
<!-- Evenly distributed icon row -->
<div class="container">
  <div class="row justify-content-evenly align-items-center py-3">
    <div class="col-auto"><i class="bi bi-house fs-3"></i></div>
    <div class="col-auto"><i class="bi bi-search fs-3"></i></div>
    <div class="col-auto"><i class="bi bi-person fs-3"></i></div>
    <div class="col-auto"><i class="bi bi-gear fs-3"></i></div>
  </div>
</div>
```

Combining `align-items-md-stretch` with `align-items-start` on mobile prevents columns from stretching to fill height on small screens where vertical space is limited.

```html
<!-- Responsive vertical alignment -->
<div class="container">
  <div class="row align-items-start align-items-md-stretch g-3" style="min-height: 300px;">
    <div class="col-md-4"><div class="bg-light p-3">Short</div></div>
    <div class="col-md-4"><div class="bg-light p-3">Tall content<br>line 2<br>line 3</div></div>
    <div class="col-md-4"><div class="bg-light p-3">Medium</div></div>
  </div>
</div>
```

## Best Practices

1. Use `justify-content-center` to center columns when they do not sum to 12, avoiding invisible offset hacks.
2. Apply `align-items-center` on rows with `min-height` to vertically center content in hero sections and panels.
3. Use `align-self-*` on individual columns to override the row's vertical alignment for specific elements.
4. Add responsive alignment classes (`justify-content-md-between`) to change alignment at different breakpoints.
5. Pair alignment utilities with gutter classes (`g-*`) to ensure spacing remains consistent regardless of alignment.
6. Use `justify-content-evenly` for navigation rows and icon bars where equal spacing between items is desired.
7. Avoid combining `justify-content-*` with `offset-*` classes — offsets consume column space and conflict with distribution logic.
8. Set `min-height` on rows when using vertical alignment — `align-items-*` has no visible effect if columns already fill the row's height.
9. Test alignment at every breakpoint since responsive classes can change the alignment behavior at each size.
10. Use `align-content-*` (e.g., `align-content-between`) on rows with wrapped columns to distribute multiple flex lines vertically.

## Common Pitfalls

1. **No min-height with align-items**: Vertical alignment is invisible when the row height equals the tallest column — add `min-height` to see the effect.
2. **Conflicting justify-content and offset**: Using `offset-*` pushes columns away from their `justify-content-*` position, creating unexpected gaps.
3. **Forgetting responsive classes**: Applying `justify-content-center` without a responsive prefix means it applies at all sizes, which may break mobile layouts.
4. **Using align-items on single-row tall columns**: When all columns are the same height, `align-items-center` produces no visible change.
5. **Overriding individual alignment unintentionally**: Setting `align-items-center` on the row and `align-self-stretch` on every column cancels the centering effect.
6. **Expecting justify-content to work with full 12-unit rows**: When columns sum to 12 and fill the row, there is no extra space for `justify-content-*` to distribute.
7. **Using alignment on non-row elements**: Alignment utilities require a flex container; applying `justify-content-center` on a `col` has no effect.

## Accessibility Considerations

Alignment utilities are purely visual and do not affect DOM order or screen reader behavior. However, visual centering can create reading orders that differ from the HTML sequence — always verify that centered or reordered content maintains a logical flow for assistive technology users. Avoid using alignment to hide content off-screen or compress it into unreadable sizes. Ensure that vertically centered text maintains adequate line height and font size for readability at all viewport sizes.

## Responsive Behavior

Bootstrap provides responsive variants for every alignment utility: `justify-content-{breakpoint}-{value}` and `align-items-{breakpoint}-{value}`. Without a breakpoint prefix, the utility applies at all screen sizes. Adding a breakpoint activates the alignment only at and above that size, following Bootstrap's mobile-first approach. For example, `justify-content-start justify-content-lg-center` aligns columns to the left on small and medium screens, then centers them on large screens and above. The `align-self-{breakpoint}-{value}` pattern works identically for individual column overrides.
