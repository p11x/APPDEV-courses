---
title: "Grid Migration"
topic: "Grid Deep Dive"
subtopic: "Grid Migration"
difficulty: 2
duration: "35 minutes"
prerequisites: ["Grid Best Practices Patterns", "Responsive Breakpoints"]
learning_objectives:
  - Migrate float-based grids to Bootstrap 5 flexbox grid
  - Understand Bootstrap 3 to 5 grid class differences
  - Apply migration patterns for common legacy grid structures
---

## Overview

Migrating to Bootstrap 5's flexbox grid from float-based grids (Bootstrap 3/4) or other frameworks involves understanding key differences: the `row` class now uses `display: flex` instead of floats, the `offset-*` system replaces `col-*-offset-*`, and new utilities like `gx-*`/`gy-*` replace manual gutter management. This guide covers the most common migration patterns from legacy grid implementations.

## Basic Implementation

Bootstrap 3 float-based column → Bootstrap 5 flexbox column:

```html
<!-- Bootstrap 3 (OLD) -->
<!-- <div class="row">
  <div class="col-md-4">Content</div>
  <div class="col-md-8">Content</div>
</div> -->

<!-- Bootstrap 5 (NEW) -->
<div class="row">
  <div class="col-md-4">
    <div class="bg-light p-3 border">Migrated column</div>
  </div>
  <div class="col-md-8">
    <div class="bg-light p-3 border">Migrated column</div>
  </div>
</div>
```

Bootstrap 3 offset → Bootstrap 5 offset syntax:

```html
<!-- Bootstrap 3 (OLD) -->
<!-- <div class="col-md-4 col-md-offset-2">Offset content</div> -->

<!-- Bootstrap 5 (NEW) -->
<div class="container">
  <div class="row">
    <div class="col-md-4 offset-md-2">
      <div class="bg-primary text-white p-3">Migrated offset</div>
    </div>
  </div>
</div>
```

Bootstrap 3 push/pull → Bootstrap 5 order classes:

```html
<!-- Bootstrap 3 (OLD) -->
<!-- <div class="col-md-4 col-md-push-8">Pushed right</div>
<div class="col-md-8 col-md-pull-4">Pulled left</div> -->

<!-- Bootstrap 5 (NEW) -->
<div class="container">
  <div class="row">
    <div class="col-md-4 order-md-2">
      <div class="bg-warning p-3">Second visually (was push-8)</div>
    </div>
    <div class="col-md-8 order-md-1">
      <div class="bg-info text-white p-3">First visually (was pull-4)</div>
    </div>
  </div>
</div>
```

## Advanced Variations

Migrating custom clearfix gutters to Bootstrap 5 gutter utilities:

```html
<!-- Bootstrap 3/4 (OLD) — manual gutter via padding + negative margin -->
<!-- <div class="row" style="margin: 0 -15px;">
  <div class="col-md-6" style="padding: 0 15px;">Content</div>
  <div class="col-md-6" style="padding: 0 15px;">Content</div>
</div> -->

<!-- Bootstrap 5 (NEW) — built-in gutter utilities -->
<div class="container">
  <div class="row g-4">
    <div class="col-md-6">
      <div class="bg-light p-3 border">Clean gutter migration</div>
    </div>
    <div class="col-md-6">
      <div class="bg-light p-3 border">Clean gutter migration</div>
    </div>
  </div>
</div>
```

Migrating nested grid structures:

```html
<!-- Bootstrap 3 (OLD) — clearfix needed for nested rows -->
<!-- <div class="row">
  <div class="col-md-8">
    <div class="row">
      <div class="col-md-6">Nested</div>
      <div class="col-md-6">Nested</div>
    </div>
  </div>
  <div class="col-md-4">Sidebar</div>
</div> -->

<!-- Bootstrap 5 (NEW) — flexbox handles nesting naturally -->
<div class="container">
  <div class="row g-3">
    <div class="col-md-8">
      <div class="row g-2">
        <div class="col-md-6"><div class="bg-light p-2 border">Nested 1</div></div>
        <div class="col-md-6"><div class="bg-light p-2 border">Nested 2</div></div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="bg-light p-3 border">Sidebar</div>
    </div>
  </div>
</div>
```

Migrating a float-based card grid to `row-cols-*`:

```html
<!-- Bootstrap 3 (OLD) — manual float and clearfix -->
<!-- <div class="row">
  <div class="col-sm-6 col-md-4" style="float: left;">Card</div>
  <div class="col-sm-6 col-md-4" style="float: left;">Card</div>
  <div class="col-sm-6 col-md-4" style="float: left;">Card</div>
  <div style="clear: both;"></div>
</div> -->

<!-- Bootstrap 5 (NEW) — row-cols for automatic column management -->
<div class="container">
  <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-4">
    <div class="col">
      <div class="card h-100"><div class="card-body">Card</div></div>
    </div>
    <div class="col">
      <div class="card h-100"><div class="card-body">Card</div></div>
    </div>
    <div class="col">
      <div class="card h-100"><div class="card-body">Card</div></div>
    </div>
  </div>
</div>
```

## Best Practices

1. Replace `col-{bp}-offset-*` with `offset-{bp}-*` — the breakpoint moves before the property.
2. Replace `col-{bp}-push-*` and `col-{bp}-pull-*` with `order-{bp}-*` classes.
3. Remove all clearfix (`::after` pseudo-elements, `.clearfix` classes) — flexbox handles this automatically.
4. Replace manual gutter padding/margin with `g-*`, `gx-*`, and `gy-*` classes on the row.
5. Remove `float: left` and `float: right` from column styles — flexbox replaces float behavior.
6. Replace `.container-fluid` edge padding hacks with Bootstrap 5's built-in container padding.
7. Use `row-cols-*` instead of manually adding column sizes to every element.
8. Test migrated grids at all breakpoints — flexbox wrapping behavior differs from float wrapping.
9. Remove `display: flex` overrides on rows — Bootstrap 5 rows are already flex containers.
10. Update any JavaScript that queries float-based layout properties (like `offsetTop`) to work with flex layout.

## Common Purifalls

- **Mixing float and flex styles**: Leaving `float: left` on columns inside flex rows causes alignment issues.
- **Forgetting clearfix removal**: Legacy clearfix pseudo-elements can interfere with flex alignment.
- **Push/pull confusion**: `col-md-push-4` doesn't map directly to `order-md-4` — push/pull shift by column units, order rearranges by sequence number.
- **Gutter migration gaps**: Replacing manual `margin: 0 -15px` with `g-3` produces different spacing values.
- **Container class changes**: Bootstrap 5 containers use CSS `max-width` media queries differently than Bootstrap 3.
- **Responsive breakpoint naming**: Bootstrap 3's `xs` is implicit in Bootstrap 5 (no prefix needed for xs).
- **Testing only one breakpoint**: Float-to-flex migration affects all breakpoints — test mobile, tablet, and desktop.

## Accessibility Considerations

- Verify that migrated order classes don't create confusing screen reader output — maintain logical DOM order.
- Test keyboard navigation after migration — flexbox tab order follows DOM order, not visual order.
- Ensure `aria-label` and `aria-describedby` references remain valid after grid restructuring.
- Check that migrated nested grids maintain proper heading hierarchy for screen readers.
- Validate that gutter changes don't reduce touch target sizes on mobile.
- Test with assistive technology after migration to catch any accessibility regressions.

## Responsive Behavior

Bootstrap 5 responsive classes follow the same pattern as Bootstrap 3/4 but with cleaner syntax. The `col-sm-6` class works the same way, but offsets and ordering use the new `{property}-{breakpoint}-{value}` pattern:

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-6 offset-md-3">
      <div class="bg-primary text-white p-3 text-center">
        Centered on desktop — same concept, cleaner syntax
      </div>
    </div>
  </div>
  <div class="row mt-3">
    <div class="col-12 col-md-4 order-md-2">
      <div class="bg-warning p-3">Reordered on desktop</div>
    </div>
    <div class="col-12 col-md-8 order-md-1">
      <div class="bg-info text-white p-3">Content first on desktop</div>
    </div>
  </div>
</div>
```

The responsive behavior is identical — columns stack on mobile and arrange side-by-side at specified breakpoints. The migration primarily involves syntactic changes, not behavioral ones.
