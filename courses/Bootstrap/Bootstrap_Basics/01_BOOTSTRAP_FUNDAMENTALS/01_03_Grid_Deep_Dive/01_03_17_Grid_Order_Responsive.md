---
title: "Grid Order Responsive"
topic: "Grid Deep Dive"
subtopic: "Grid Order Responsive"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Column Ordering", "Responsive Breakpoints"]
learning_objectives:
  - Apply responsive order classes for breakpoint-specific reordering
  - Use order-sm-first and order-md-last patterns
  - Understand order cascading behavior across breakpoints
---

## Overview

Bootstrap's `order-*` classes control the visual order of flexbox grid columns. Responsive variants (`order-sm-*`, `order-md-*`, etc.) let you reorder columns at specific breakpoints, enabling layouts where the sidebar appears first on mobile but last on desktop. The `order-first` and `order-last` utility classes provide quick shortcuts, while numeric `order-0` through `order-5` offer granular control.

## Basic Implementation

Basic responsive reordering — second column appears first on desktop:

```html
<div class="container">
  <div class="row">
    <div class="col-md-6 order-md-2">
      <div class="bg-primary text-white p-3">
        Second in HTML, first on md+
      </div>
    </div>
    <div class="col-md-6 order-md-1">
      <div class="bg-secondary text-white p-3">
        First in HTML, second on md+
      </div>
    </div>
  </div>
</div>
```

Using `order-first` and `order-last` utilities:

```html
<div class="container">
  <div class="row">
    <div class="col-md-4">
      <div class="bg-success text-white p-3">Normal order</div>
    </div>
    <div class="col-md-4 order-first">
      <div class="bg-warning p-3">Moved to first position</div>
    </div>
    <div class="col-md-4 order-last">
      <div class="bg-danger text-white p-3">Moved to last position</div>
    </div>
  </div>
</div>
```

Responsive order that only activates at specific breakpoints:

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-4 order-md-1">
      <div class="bg-info text-white p-3">Navigation (first on desktop)</div>
    </div>
    <div class="col-12 col-md-8 order-md-0">
      <div class="bg-dark text-white p-3">Content (first on mobile, second on desktop)</div>
    </div>
  </div>
</div>
```

## Advanced Variations

Combining order with offset for asymmetric desktop layouts:

```html
<div class="container">
  <div class="row">
    <div class="col-md-5 order-md-2">
      <div class="bg-primary text-white p-3">Image (right on desktop)</div>
    </div>
    <div class="col-md-5 offset-md-1 order-md-1">
      <div class="bg-light p-3 border">Text (left on desktop)</div>
    </div>
  </div>
</div>
```

Multiple breakpoint order changes:

```html
<div class="container">
  <div class="row">
    <div class="col-md-4 order-2 order-md-1 order-lg-3">
      <div class="bg-success text-white p-3">
        A: order-2 mobile → order-1 md → order-3 lg
      </div>
    </div>
    <div class="col-md-4 order-1 order-md-3 order-lg-1">
      <div class="bg-warning p-3">
        B: order-1 mobile → order-3 md → order-1 lg
      </div>
    </div>
    <div class="col-md-4 order-3 order-md-2 order-lg-2">
      <div class="bg-danger text-white p-3">
        C: order-3 mobile → order-2 md → order-2 lg
      </div>
    </div>
  </div>
</div>
```

Using `order-last` with responsive breakpoint for sticky footer pattern:

```html
<div class="container">
  <div class="row">
    <div class="col-md-8">
      <div class="bg-white p-3 border" style="min-height: 200px;">Main content</div>
    </div>
    <div class="col-md-4 order-last order-md-0">
      <div class="bg-light p-3 border">Sidebar (last on mobile, normal on desktop)</div>
    </div>
  </div>
</div>
```

## Best Practices

1. Use `order-md-*` to reorder columns on desktop while preserving natural DOM order on mobile.
2. Apply `order-first` or `order-last` for simple reordering, and numeric `order-0` through `order-5` for complex sequences.
3. Use `order-0` at a higher breakpoint to reset order values applied at lower breakpoints.
4. Keep the DOM order logical for screen readers — visual reordering should not confuse content hierarchy.
5. Combine `order-*` with responsive offsets for asymmetric layouts that need both positioning and spacing.
6. Use `order-first order-md-0` to move a column first on mobile and reset to normal on desktop.
7. Test reordered layouts with keyboard navigation to ensure tab order remains logical.
8. Limit reordering to 2-3 columns — extensive reordering suggests the HTML structure needs rethinking.
9. Document order logic in comments when using multi-breakpoint ordering patterns.
10. Prefer CSS `order` over JavaScript DOM manipulation for performance.

## Common Pitfalls

- **Accessibility confusion**: Visual reordering disconnects from DOM order, confusing screen reader users and keyboard navigation.
- **Order cascade**: `order-sm-2` applies at sm, md, lg, xl — use `order-md-0` to reset at larger breakpoints.
- **Forgetting default order**: Columns have `order: 0` by default. Using `order-1` on one column without setting others may not produce expected results.
- **Over-reordering**: Reordering more than 2-3 columns creates maintenance nightmares and accessibility issues.
- **Order + offset conflicts**: Reordering a column with an offset may push it off-screen since the offset applies before the order swap.
- **Testing only visual output**: Reordered content may break screen reader flow — always test with assistive technology.
- **Responsive order not resetting**: `order-sm-last` persists at all larger breakpoints unless explicitly reset.

## Accessibility Considerations

- Maintain DOM order that reflects logical content hierarchy — screen readers and keyboard navigation follow DOM order, not visual order.
- Use `aria-label` or `aria-describedby` to clarify relationships between visually reordered content.
- Avoid reordering navigation elements — tab order must match visual order for keyboard users.
- Provide `role="region"` with descriptive labels on reordered sections to help screen reader users.
- Test with VoiceOver, NVDA, or JAWS to verify reordered content makes sense in linear reading order.
- Consider using CSS `order` only for decorative reordering — critical content should follow DOM order.

## Responsive Behavior

Order classes cascade upward like column classes. `order-sm-2` applies at sm, md, lg, xl, and xxl. Use larger breakpoint classes to override:

```html
<div class="container">
  <div class="row">
    <div class="col-md-4 order-3 order-sm-1 order-lg-3">
      <div class="bg-primary text-white p-3">
        Last on mobile → first on sm → last on lg
      </div>
    </div>
    <div class="col-md-4 order-1 order-sm-3 order-lg-1">
      <div class="bg-secondary text-white p-3">
        First on mobile → last on sm → first on lg
      </div>
    </div>
    <div class="col-md-4 order-2">
      <div class="bg-success text-white p-3">
        Always second on md+
      </div>
    </div>
  </div>
</div>
```

Each breakpoint class overrides the previous one, allowing complex multi-breakpoint reordering patterns within a single row.
