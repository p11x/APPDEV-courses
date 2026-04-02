---
title: "Grid Gutter Variations"
topic: "Grid Deep Dive"
subtopic: "Grid Gutter Variations"
difficulty: 1
duration: "25 minutes"
prerequisites: ["Gutter Control", "Bootstrap Grid Basics"]
learning_objectives:
  - Apply horizontal (gx-) and vertical (gy-) gutter classes
  - Use responsive gutter variants at specific breakpoints
  - Combine g-* with gx-* and gy-* for mixed gutter control
---

## Overview

Bootstrap 5 provides granular gutter control through three utility classes: `g-*` for both horizontal and vertical gutters, `gx-*` for horizontal-only, and `gy-*` for vertical-only. Values range from `0` (no gutters) to `5` (3rem / 48px). Responsive variants like `g-md-4` apply gutters only at specific breakpoints, enabling full-bleed mobile layouts with padded desktop grids.

## Basic Implementation

Basic gutter sizes applied to all sides:

```html
<div class="container">
  <div class="row g-3">
    <div class="col-6">
      <div class="bg-primary text-white p-3">g-3 gutter</div>
    </div>
    <div class="col-6">
      <div class="bg-secondary text-white p-3">g-3 gutter</div>
    </div>
    <div class="col-6">
      <div class="bg-success text-white p-3">g-3 gutter</div>
    </div>
    <div class="col-6">
      <div class="bg-warning p-3">g-3 gutter</div>
    </div>
  </div>
</div>
```

Zero gutters for edge-to-edge layouts:

```html
<div class="container-fluid">
  <div class="row g-0">
    <div class="col-md-6">
      <div class="bg-info text-white p-3">No gutter — flush edge</div>
    </div>
    <div class="col-md-6">
      <div class="bg-dark text-white p-3">No gutter — flush edge</div>
    </div>
  </div>
</div>
```

Horizontal-only gutters with `gx-*`:

```html
<div class="container">
  <div class="row gx-5 gy-0">
    <div class="col-6">
      <div class="bg-danger text-white p-3">gx-5 horizontal only</div>
    </div>
    <div class="col-6">
      <div class="bg-primary text-white p-3">gx-5 horizontal only</div>
    </div>
    <div class="col-6">
      <div class="bg-warning p-3">gy-0 vertical — no gap</div>
    </div>
    <div class="col-6">
      <div class="bg-success text-white p-3">gy-0 vertical — no gap</div>
    </div>
  </div>
</div>
```

## Advanced Variations

Responsive gutters — no gutters on mobile, large gutters on desktop:

```html
<div class="container">
  <div class="row g-0 g-md-4">
    <div class="col-12 col-md-6">
      <div class="bg-primary text-white p-3">Full-bleed mobile, padded md+</div>
    </div>
    <div class="col-12 col-md-6">
      <div class="bg-secondary text-white p-3">Full-bleed mobile, padded md+</div>
    </div>
  </div>
</div>
```

Mixed horizontal and vertical responsive gutters:

```html
<div class="container">
  <div class="row gx-2 gx-lg-5 gy-3 gy-lg-4">
    <div class="col-sm-6 col-lg-3">
      <div class="bg-light p-3 border">Card 1</div>
    </div>
    <div class="col-sm-6 col-lg-3">
      <div class="bg-light p-3 border">Card 2</div>
    </div>
    <div class="col-sm-6 col-lg-3">
      <div class="bg-light p-3 border">Card 3</div>
    </div>
    <div class="col-sm-6 col-lg-3">
      <div class="bg-light p-3 border">Card 4</div>
    </div>
  </div>
</div>
```

Using gutters with row-cols for card grids:

```html
<div class="container">
  <div class="row row-cols-1 row-cols-md-2 row-cols-lg-4 g-4">
    <div class="col">
      <div class="card h-100"><div class="card-body">Card A</div></div>
    </div>
    <div class="col">
      <div class="card h-100"><div class="card-body">Card B</div></div>
    </div>
    <div class="col">
      <div class="card h-100"><div class="card-body">Card C</div></div>
    </div>
    <div class="col">
      <div class="card h-100"><div class="card-body">Card D</div></div>
    </div>
  </div>
</div>
```

## Best Practices

1. Use `g-3` or `g-4` as default gutters for most card and content grids.
2. Apply `g-0` for full-bleed image sections or edge-to-edge layouts.
3. Use `gx-*` and `gy-*` independently when horizontal and vertical spacing needs differ.
4. Apply responsive gutters (`g-md-4`) to create edge-to-edge mobile layouts with padded desktop layouts.
5. Combine `g-*` on the row with `p-*` on column content for layered spacing control.
6. Use `g-0` alongside `overflow-hidden` on the container when gutters cause unwanted scrollbars.
7. Prefer gutter classes over manual margin/padding on columns for consistent spacing.
8. Use larger gutters (`g-5`) for feature sections that need breathing room.
9. Match gutter size to design system spacing tokens for consistency.
10. Test gutters at all breakpoints to ensure responsive transitions feel smooth.

## Common Pitfalls

- **Gutters causing horizontal scroll**: Large gutters on narrow screens push content beyond viewport. Use `g-0` on mobile with `g-md-4` for desktop.
- **Forgetting `g-*` on rows**: Without gutters, columns touch each other with no visual separation.
- **Conflicting margin utilities**: Adding `me-3` to columns while using `g-4` on the row creates double spacing.
- **Negative margin on rows**: Rows have negative margins to compensate for column padding — removing `g-*` doesn't remove this behavior.
- **Gutters on nested rows**: Nested rows need their own `g-*` class — they don't inherit the parent row's gutters.
- **Using padding on rows instead of gutters**: Row padding adds space around the entire grid, not between columns.
- **Inconsistent gutter sizes**: Mixing `g-2` on one row and `g-4` on another creates visual inconsistency.

## Accessibility Considerations

- Use sufficient gutter space so content in adjacent columns doesn't visually blend together.
- Ensure gutters don't reduce column content width below readable minimums on mobile screens.
- Maintain consistent vertical gutters between wrapped rows for predictable visual rhythm.
- Avoid `g-0` layouts where adjacent column borders create confusing visual separators.
- Provide adequate spacing between interactive elements in grid columns for touch accessibility.
- Test with zoom (200%) to ensure gutter behavior remains functional when content scales.

## Responsive Behavior

Gutter classes support all Bootstrap breakpoints. The cascade follows the same upward pattern as other utilities:

```html
<div class="container">
  <div class="row g-0 g-sm-2 g-md-3 g-lg-4 g-xl-5">
    <div class="col-6 col-md-4 col-xl-3">
      <div class="bg-primary text-white p-3">1</div>
    </div>
    <div class="col-6 col-md-4 col-xl-3">
      <div class="bg-secondary text-white p-3">2</div>
    </div>
    <div class="col-6 col-md-4 col-xl-3">
      <div class="bg-success text-white p-3">3</div>
    </div>
    <div class="col-6 col-md-4 col-xl-3">
      <div class="bg-warning p-3">4</div>
    </div>
  </div>
</div>
```

Gutters grow progressively: none on mobile, 0.5rem at sm, 1rem at md, 1.5rem at lg, and 3rem at xl. Each breakpoint class overrides the previous one.
