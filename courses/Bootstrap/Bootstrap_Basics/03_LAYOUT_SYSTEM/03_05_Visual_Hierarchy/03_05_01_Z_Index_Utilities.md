---
title: "Z-Index Utilities"
description: "Control stacking order of elements with Bootstrap 5 z-index utilities"
difficulty: 2
estimated_time: "18 minutes"
tags: ["z-index", "stacking", "overlay", "positioning"]
---

# Z-Index Utilities

## Overview

Bootstrap 5 provides a set of `z-index` utility classes that control the stacking order of positioned elements. The available classes are `z-n1`, `z-0`, `z-1`, `z-2`, and `z-3`, each mapping to predefined z-index values in Bootstrap's CSS variable system.

Z-index only applies to positioned elements (`position: relative`, `absolute`, `fixed`, or `sticky`). Understanding z-index is critical for managing overlapping elements like modals, dropdowns, tooltips, sticky headers, and custom overlays. Bootstrap also exposes its z-index scale as CSS custom properties, enabling consistent stacking across custom components.

## Basic Implementation

### Z-Index Classes

Apply z-index utilities to control which element appears on top when elements overlap:

```html
<div class="position-relative bg-light p-5" style="height: 250px;">
  <div class="position-absolute top-0 start-0 p-4 bg-primary text-white z-0" style="width: 150px;">
    z-0 (base layer)
  </div>
  <div class="position-absolute top-5 start-5 p-4 bg-success text-white z-1" style="width: 150px;">
    z-1 (above z-0)
  </div>
  <div class="position-absolute top-10 start-10 p-4 bg-danger text-white z-2" style="width: 150px;">
    z-2 (above z-1)
  </div>
</div>
```

### Negative Z-Index

Use `z-n1` to place elements behind their parent:

```html
<div class="position-relative bg-light p-5" style="height: 200px;">
  <div class="position-absolute top-0 start-0 p-3 bg-warning z-n1" style="width: 200px; height: 100px;">
    z-n1 (behind parent)
  </div>
  <span class="position-relative z-0 bg-white p-2">Parent content sits above</span>
</div>
```

### Z-Index 3 (Highest Built-in)

```html
<div class="position-relative bg-light p-5" style="height: 200px;">
  <div class="position-absolute top-0 start-0 p-3 bg-secondary text-white z-1" style="width: 150px;">
    z-1
  </div>
  <div class="position-absolute top-2 start-2 p-3 bg-dark text-white z-3" style="width: 150px;">
    z-3 (highest built-in)
  </div>
</div>
```

## Advanced Variations

### CSS Custom Properties

Bootstrap exposes its z-index scale as CSS variables for use in custom styles:

```html
<style>
  .custom-overlay {
    z-index: var(--bs-zindex-tooltip); /* 1080 */
  }
  .custom-dropdown {
    z-index: var(--bs-zindex-dropdown); /* 1000 */
  }
  .custom-sticky {
    z-index: var(--bs-zindex-sticky); /* 1020 */
  }
</style>

<header class="sticky-top bg-dark text-white p-3" style="z-index: var(--bs-zindex-sticky);">
  Sticky header using CSS variable
</header>
```

### Bootstrap's Default Z-Index Scale

Understanding the full scale helps place custom elements correctly:

```
z-n1      → -1
z-0       → 0
z-1       → 1
z-2       → 2
z-3       → 3
dropdown  → 1000
sticky    → 1020
fixed     → 1030
offcanvas-backdrop → 1040
modal     → 1050
offcanvas → 1055
popover   → 1070
tooltip   → 1080
toast     → 1090
```

### Stacking Context

A new stacking context is created when an element has a `z-index` value other than `auto` along with a positioning context:

```html
<div class="position-relative" style="z-index: 1;">
  <!-- This parent creates a stacking context -->
  <div class="position-absolute top-0 start-0 bg-primary text-white p-3" style="z-index: 999;">
    z-index: 999 but constrained by parent's context
  </div>
</div>
<div class="position-relative" style="z-index: 2; margin-top: -20px;">
  <!-- This parent stacks above the previous context regardless of child values -->
  <div class="position-absolute top-0 start-0 bg-danger text-white p-3" style="z-index: 1;">
    z-index: 1 but parent context (2) beats sibling context (1)
  </div>
</div>
```

### Overlay Pattern

```html
<div class="position-relative overflow-hidden rounded" style="height: 300px;">
  <img src="photo.jpg" alt="Background" class="w-100 h-100 object-fit-cover">
  <div class="position-absolute top-0 start-0 w-100 h-100 bg-dark opacity-50 z-1"></div>
  <div class="position-absolute top-50 start-50 translate-middle text-white text-center z-2">
    <h2>Overlay Content</h2>
    <button class="btn btn-light">Call to Action</button>
  </div>
</div>
```

## Best Practices

1. **Use Bootstrap's z-index scale** rather than arbitrary large numbers. Values like `z-1`, `z-2`, `z-3` keep stacking predictable.

2. **Use CSS variables** (`--bs-zindex-modal`, `--bs-zindex-dropdown`) in custom components to align with Bootstrap's stacking conventions.

3. **Always pair z-index with a position property.** Z-index has no effect on statically positioned elements.

4. **Avoid z-index wars** with ever-increasing values. If elements need stacking above Bootstrap modals (1050) or tooltips (1080), reconsider the design.

5. **Create explicit stacking contexts** on container elements using `position: relative` and a z-index value. This prevents child z-index values from affecting sibling containers.

6. **Use `z-n1` sparingly** for decorative elements that should sit behind their parent, such as background patterns or pseudo-element decorations.

7. **Document z-index usage** in complex overlay interfaces. Stacking order bugs become difficult to trace without clear intent.

8. **Test stacking in all target browsers.** While z-index behavior is standardized, stacking context creation can vary subtly.

9. **Prefer Bootstrap's built-in components** (modals, dropdowns, tooltips) which already handle z-index. Custom overlays should reference the same variable scale.

10. **Reset z-index when removing positioning.** An element with `z-3` that becomes `position: static` retains the class but loses the stacking effect, potentially confusing future developers.

## Common Pitfalls

### Forgetting position property
Z-index requires `position: relative`, `absolute`, `fixed`, or `sticky`. Applying `z-3` to a statically positioned element produces no stacking change.

### Stacking context surprises
A child with `z-index: 9999` inside a parent with `z-index: 1` cannot appear above a sibling of the parent with `z-index: 2`. The parent's stacking context constrains the child.

### Conflicting with Bootstrap component z-index
Custom z-index values may conflict with Bootstrap's modal, dropdown, or tooltip z-index. Use the CSS variables to stay in sync.

### Using large arbitrary values
`z-index: 99999` is a code smell indicating stacking context problems. Debug the context hierarchy instead of escalating values.

### Not considering z-index in print styles
Z-index stacking may produce incorrect print output. Use `@media print` rules to reset or hide overlapping elements.

## Accessibility Considerations

Z-index does not affect the accessibility tree or screen reader behavior. However, visual stacking order must align with logical content relationships. If a tooltip appears above content due to z-index, ensure the tooltip content is also programmatically associated via `aria-describedby`.

Modal overlays with z-index should trap focus within the modal. The `z-3` utility or `--bs-zindex-modal` variable ensures visual correctness, but focus management is a separate concern that must be handled with JavaScript.

When using z-index to create decorative overlays that dim background content, ensure the overlaying content is accessible and that background interactivity is properly disabled.

## Responsive Behavior

Bootstrap's z-index utilities do not support responsive prefixes natively. If z-index must change at breakpoints, use custom CSS with media queries or toggle between classes using JavaScript. The CSS variables remain accessible at all breakpoints for custom implementations.

```html
<style>
  @media (min-width: 768px) {
    .z-responsive { z-index: var(--bs-zindex-sticky); }
  }
  @media (max-width: 767.98px) {
    .z-responsive { z-index: 0; }
  }
</style>
```

For most use cases, z-index values remain constant across breakpoints. Responsive changes are typically needed only for overlay patterns that differ between mobile and desktop interfaces.
