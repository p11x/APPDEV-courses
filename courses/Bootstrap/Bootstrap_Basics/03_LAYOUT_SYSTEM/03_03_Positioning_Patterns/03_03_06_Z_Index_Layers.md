---
title: "Z-Index Layers"
description: "Manage element stacking order in Bootstrap 5 using z-index utilities, Sass variables, and stacking context principles."
difficulty: 2
estimated_time: "20 minutes"
prerequisites:
  - "Position utilities"
  - "Basic CSS stacking context"
tags:
  - z-index
  - stacking
  - layers
  - positioning
  - utilities
---

## Overview

Z-index controls the stacking order of positioned elements along the z-axis. Bootstrap 5 provides utility classes `z-n1`, `z-0`, `z-1`, `z-2`, and `z-3` for quick z-index management. It also defines a structured z-index scale via Sass variables for its components: dropdowns (1000), sticky (1020), fixed (1030), offcanvas (1045), modal backdrops (1050), modals (1055), popovers (1070), and tooltips (1080).

Understanding z-index requires knowledge of stacking contexts, as z-index values only compete within the same stacking context. Elements with `position: static` (the default) ignore z-index entirely.

## Basic Implementation

### Z-Index Utility Classes

```html
<div class="position-relative" style="height: 200px;">
  <div class="position-absolute top-0 start-0 bg-primary text-white p-3 rounded z-0">
    z-0 (behind)
  </div>
  <div class="position-absolute top-2 start-2 bg-success text-white p-3 rounded z-1">
    z-1
  </div>
  <div class="position-absolute top-4 start-4 bg-danger text-white p-3 rounded z-2">
    z-2
  </div>
  <div class="position-absolute top-5 start-5 bg-dark text-white p-3 rounded z-3">
    z-3 (on top)
  </div>
</div>
```

### Negative Z-Index

Push an element behind its stacking context:

```html
<div class="position-relative bg-light p-4" style="height: 150px;">
  <div class="position-absolute top-0 start-0 bg-warning p-3 z-n1 w-100 h-100">
    Behind the parent's content
  </div>
  <p class="position-relative z-0">This text appears above the yellow background.</p>
</div>
```

## Advanced Variations

### Sass Z-Index Variables

Bootstrap defines z-index values in `_variables.scss`:

```scss
$zindex-dropdown:          1000;
$zindex-sticky:            1020;
$zindex-fixed:             1030;
$zindex-offcanvas-backdrop: 1045;
$zindex-offcanvas:         1045;
$zindex-modal-backdrop:    1050;
$zindex-modal:             1055;
$zindex-popover:           1070;
$zindex-tooltip:           1080;
```

Override these to customize the layering system:

```scss
// Before importing Bootstrap
$zindex-dropdown: 1100;
$zindex-modal: 1200;

@import "bootstrap/scss/bootstrap";
```

### Managing Modal Z-Index Conflicts

When custom elements must appear above modals:

```html
<!-- Custom notification above modal -->
<div class="position-fixed top-0 end-0 p-3" style="z-index: 1080;">
  <div class="toast show">
    <div class="toast-body">Always visible notification</div>
  </div>
</div>
```

### Stacking Context Isolation

Create isolated stacking contexts with `position: relative` and explicit z-index:

```html
<!-- Each card has its own stacking context -->
<div class="d-flex gap-3">
  <div class="card position-relative" style="z-index: 1;">
    <div class="card-body">
      <span class="position-absolute top-0 start-100 translate-middle badge bg-danger z-1">
        Badge in Card 1 context
      </span>
      Card 1
    </div>
  </div>
  <div class="card position-relative" style="z-index: 2;">
    <div class="card-body">
      <span class="position-absolute top-0 start-100 translate-middle badge bg-primary z-1">
        Badge in Card 2 context
      </span>
      Card 2
    </div>
  </div>
</div>
```

### Custom Z-Index Scale

Extend Bootstrap's scale for project-specific needs:

```scss
$zindex-custom-tooltip:  1090;
$zindex-custom-banner:   1060;
$zindex-custom-sidebar:  1040;

// Use in custom components
.custom-tooltip {
  z-index: $zindex-custom-tooltip;
}
```

## Best Practices

1. **Use Bootstrap's z-index scale** for built-in components to avoid conflicts with modals, dropdowns, and tooltips.
2. **Set z-index only on positioned elements** (`relative`, `absolute`, `fixed`, `sticky`); it has no effect on `static`.
3. **Keep z-index values consistent** across the project by defining them as Sass variables or CSS custom properties.
4. **Use `z-1` through `z-3` utilities** for simple stacking needs within a single component.
5. **Avoid excessively high z-index values** (e.g., `z-index: 999999`); they create maintenance nightmares and stacking conflicts.
6. **Create stacking contexts intentionally** with `position: relative; z-index: <value>` to isolate child z-index values.
7. **Use `z-n1`** to push background decoration behind content within the same container.
8. **Document custom z-index values** in your design system so team members understand the stacking hierarchy.
9. **Test z-index with modals open** to verify custom elements do not inadvertently appear above or below modal content.
10. **Use CSS custom properties** for runtime z-index management: `z-index: var(--z-notification, 1060)`.
11. **Prefer component-level z-index** (Bootstrap's variables) over global overrides for predictable behavior.
12. **Audit z-index periodically** to remove unused values and consolidate the stacking system.

## Common Pitfalls

1. **Z-index on static elements**: Without `position: relative/absolute/fixed/sticky`, z-index has no effect.
2. **Stacking context confusion**: A z-index of `999` inside a stacking context with `z-index: 1` cannot appear above a sibling stacking context with `z-index: 2`.
3. **Conflicting with Bootstrap's modal stack**: Custom z-index above 1055 will overlap modals, which may not be intended.
4. **Excessive z-index values**: Using `z-index: 9999` as a quick fix leads to z-index wars when new elements are added.
5. **Not understanding `auto`**: `z-index: auto` (the default for positioned elements) creates a stacking context but does not participate in parent stacking order the same way a numeric value does.
6. **Browser differences in stacking**: Some browsers handle z-index on flex/grid children differently in edge cases.

## Accessibility Considerations

- Z-index does not affect screen reader order; DOM order determines reading sequence.
- Ensure high z-index elements (modals, toasts) do not trap focus behind them.
- Provide `aria-live` regions for dynamically appearing high z-index notifications.
- Verify that z-index layering does not hide important content at 200% zoom.
- Use `inert` attribute on background content when modal z-index layers are active to prevent background interaction.

## Responsive Behavior

Z-index utilities do not include responsive variants by default. To apply z-index conditionally, enable responsive support in Bootstrap's utility API:

```scss
$utilities: (
  "zindex": (
    responsive: true,
    property: z-index,
    class: z,
    values: (
      n1: -1,
      0: 0,
      1: 1,
      2: 2,
      3: 3
    )
  )
);
```

This generates classes like `z-md-2` or `z-lg-3` for breakpoint-specific stacking control. Use this when an element should have different stacking behavior on mobile versus desktop.
