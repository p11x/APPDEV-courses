---
title: "Align Items and Align Self Utilities"
description: "Control cross-axis alignment in Bootstrap 5 flex containers with align-items and align-self utilities"
difficulty: 1
estimated_time: "15 minutes"
tags: ["flexbox", "align-items", "align-self", "vertical-alignment"]
---

# Align Items and Align Self Utilities

## Overview

Bootstrap 5 provides `align-items` and `align-self` utility classes to control how flex items are positioned along the cross axis. The `align-items` class is applied to the flex container and sets the default alignment for all children, while `align-self` is applied to individual flex items to override the container's alignment for that specific item.

The available alignment values are `start`, `end`, `center`, `baseline`, and `stretch`. These utilities solve common layout challenges like vertically centering content, aligning buttons of different sizes, and creating baseline-aligned text alongside icons.

## Basic Implementation

### Align Items on Container

Apply alignment to the flex container to affect all children simultaneously:

```html
<!-- Align items to the start (top in flex-row) -->
<div class="d-flex align-items-start bg-light" style="height: 150px;">
  <div class="p-2 bg-primary text-white">Short</div>
  <div class="p-2 bg-success text-white" style="height: 80px;">Tall</div>
  <div class="p-2 bg-danger text-white">Short</div>
</div>

<!-- Vertically centered -->
<div class="d-flex align-items-center bg-light" style="height: 150px;">
  <div class="p-2 bg-primary text-white">Item A</div>
  <div class="p-2 bg-success text-white">Item B</div>
  <div class="btn btn-danger">Action</div>
</div>

<!-- Stretched to fill (default behavior) -->
<div class="d-flex align-items-stretch bg-light" style="height: 150px;">
  <div class="p-2 bg-primary text-white border">Stretch 1</div>
  <div class="p-2 bg-success text-white border">Stretch 2</div>
</div>
```

### Baseline Alignment

Align items by their text baseline, useful for mixed content:

```html
<div class="d-flex align-items-baseline bg-light p-3">
  <h1 class="me-3">Heading</h1>
  <p class="mb-0">Body text aligns by baseline</p>
  <small class="ms-3">Small text</small>
</div>
```

## Advanced Variations

### Align Self on Individual Items

Override the container's alignment for specific children:

```html
<div class="d-flex align-items-start bg-light" style="height: 200px;">
  <div class="p-2 bg-primary text-white align-self-start">Start</div>
  <div class="p-2 bg-success text-white align-self-center">Center</div>
  <div class="p-2 bg-danger text-white align-self-end">End</div>
  <div class="p-2 bg-warning text-dark align-self-stretch">Stretch</div>
  <div class="p-2 bg-info text-white align-self-baseline">Baseline</div>
</div>
```

### Responsive Alignment

Change alignment behavior at different breakpoints:

```html
<!-- Top on mobile, center on medium, end on large -->
<div class="d-flex align-items-start align-items-md-center align-items-lg-end bg-light p-3" style="height: 200px;">
  <div class="p-2 bg-primary text-white">Responsive</div>
  <div class="p-2 bg-success text-white" style="height: 100px;">Aligned</div>
</div>

<!-- Responsive self alignment -->
<div class="d-flex align-items-stretch bg-light" style="height: 180px;">
  <div class="p-2 align-self-auto align-self-md-center bg-primary text-white">Responsive Self</div>
</div>
```

### Combining with Justify Content

Use both axes simultaneously for precise positioning:

```html
<!-- Perfectly centered in both axes -->
<div class="d-flex justify-content-center align-items-center bg-light" style="height: 300px;">
  <div class="card p-4">
    <h5>Centered Card</h5>
    <p>This card is centered horizontally and vertically.</p>
  </div>
</div>

<!-- Space-between horizontal, center vertical -->
<div class="d-flex justify-content-between align-items-center bg-light p-3">
  <img src="logo.png" alt="Logo" style="width: 40px;">
  <nav class="d-flex gap-3">
    <a href="#">Link 1</a>
    <a href="#">Link 2</a>
  </nav>
</div>
```

## Best Practices

1. **Use `align-items-center`** for vertically centering content in navigation bars, cards, and hero sections. It is the most frequently used alignment utility.

2. **Apply `align-self-*`** when one item needs different alignment than its siblings. This avoids creating additional wrapper elements.

3. **Set a container height** when using `align-items-start`, `align-items-end`, or `align-items-center`. Without an explicit height, the container collapses to content size and alignment changes are not visible.

4. **Use `align-items-baseline`** for headers with mixed font sizes, such as a large title next to a smaller subtitle or badge.

5. **Pair `align-items-center` with `justify-content-center`** for true centering in both axes. This replaces older hacks using transforms or absolute positioning.

6. **Prefer `align-items-stretch`** (the default) when flex children should fill the container height evenly. It creates uniform rows.

7. **Use responsive prefixes** like `align-items-md-center` to adapt vertical alignment for different viewport sizes.

8. **Avoid `align-items-baseline`** on flex containers with images or icon-only items, as baseline alignment may produce unexpected positioning.

9. **Combine with `min-height`** instead of fixed `height` when the container should expand but maintain minimum centering space.

10. **Test with variable content heights.** Alignment effects are most visible when children have different heights.

## Common Pitfalls

### No visible effect without container height
`align-items` and `align-self` control cross-axis positioning. If the container has no explicit height (for `flex-row`) or width (for `flex-column`), items already fill the space and alignment changes are imperceptible.

### Confusing cross axis with main axis
In `flex-row`, `align-items` controls vertical alignment. In `flex-column`, it controls horizontal alignment. Always identify the flex direction first.

### Forgetting that `align-self` overrides `align-items`
If the container sets `align-items-center` but a child has `align-items-start`, the child's `align-self-start` takes precedence. Debug alignment issues by checking both container and child classes.

### Baseline alignment with non-text elements
`align-items-baseline` references the text baseline. Icon-only items, images, or buttons without text may align unpredictably. Use `center` for mixed content types.

### Stretch not working with defined heights
`align-items-stretch` cannot expand children beyond their explicit `height` or `max-height` values. Remove constraining height properties to see stretching behavior.

## Accessibility Considerations

Cross-axis alignment does not affect DOM order or screen reader traversal. Content is announced in source order regardless of visual positioning. When using `align-self` to visually reorder items, ensure the logical reading sequence remains coherent.

Vertically centered modals and dialogs using `align-items-center` should still trap focus correctly. Verify that focus management works independently of visual centering.

For card grids or lists using `align-items-stretch`, ensure that the visual grouping implied by equal-height items is supported by proper semantic markup such as `<article>` or `<section>` elements.

## Responsive Behavior

All alignment utilities support breakpoint prefixes. Apply mobile-first base classes and override at larger breakpoints:

```html
<!-- Start aligned on mobile, center on tablet, end on desktop -->
<div class="d-flex align-items-start align-items-md-center align-items-lg-end bg-light p-3" style="height: 200px;">
  <div class="p-2 bg-primary text-white">Adaptive Item</div>
  <div class="p-2 bg-success text-white" style="height: 120px;">Tall Item</div>
</div>

<!-- Responsive self override -->
<div class="d-flex align-items-center bg-light" style="height: 180px;">
  <div class="align-self-auto align-self-lg-end p-2 bg-primary text-white">Self Override</div>
</div>
```

Breakpoints follow Bootstrap's standard tiers: `sm` (576px), `md` (768px), `lg` (992px), `xl` (1200px), and `xxl` (1400px).
