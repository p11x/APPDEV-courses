---
title: "Gap Utilities"
description: "Control spacing between flex and grid children using Bootstrap 5 gap utility classes."
difficulty: 1
estimated_time: "15 minutes"
prerequisites:
  - "Basic HTML knowledge"
  - "Bootstrap 5 setup"
  - "Understanding of Flexbox or CSS Grid"
tags:
  - spacing
  - gap
  - flexbox
  - grid
  - utilities
---

## Overview

Gap utilities control the space **between** child elements in flexbox and grid containers. Unlike margins, gaps only appear between items, not around the outer edges. This eliminates the need for complex `:first-child` / `:last-child` margin overrides that were common before gap support.

Bootstrap 5 provides three gap utilities: `gap-{size}` (both directions), `row-gap-{size}` (vertical), and `column-gap-{size}` (horizontal). They work with `d-flex` and `d-grid` containers and follow the same 0-5 size scale as other spacing utilities.

## Basic Implementation

### Gap with Flexbox

Use `gap-*` on flex containers to space children evenly:

```html
<div class="d-flex gap-3">
  <div class="bg-light p-2 rounded">Item 1</div>
  <div class="bg-light p-2 rounded">Item 2</div>
  <div class="bg-light p-2 rounded">Item 3</div>
</div>

<div class="d-flex gap-1">
  <button class="btn btn-primary">Save</button>
  <button class="btn btn-secondary">Cancel</button>
</div>
```

### Gap with CSS Grid

Gaps work naturally with grid layouts:

```html
<div class="d-grid gap-3" style="grid-template-columns: repeat(3, 1fr);">
  <div class="bg-light p-3 rounded">Grid Item 1</div>
  <div class="bg-light p-3 rounded">Grid Item 2</div>
  <div class="bg-light p-3 rounded">Grid Item 3</div>
  <div class="bg-light p-3 rounded">Grid Item 4</div>
  <div class="bg-light p-3 rounded">Grid Item 5</div>
  <div class="bg-light p-3 rounded">Grid Item 6</div>
</div>
```

### Directional Gaps

Control row and column gaps independently:

```html
<!-- Only vertical gaps between rows -->
<div class="d-flex flex-column gap-row-3">
  <div class="bg-light p-2 rounded">Row 1</div>
  <div class="bg-light p-2 rounded">Row 2</div>
  <div class="bg-light p-2 rounded">Row 3</div>
</div>

<!-- Only horizontal gaps between columns -->
<div class="d-flex column-gap-3">
  <div class="bg-light p-2 rounded">Col 1</div>
  <div class="bg-light p-2 rounded">Col 2</div>
  <div class="bg-light p-2 rounded">Col 3</div>
</div>
```

## Advanced Variations

### Responsive Gaps

Adjust spacing between items at different breakpoints:

```html
<!-- Tight gap on mobile, larger on desktop -->
<div class="d-flex gap-1 gap-md-3 gap-lg-4">
  <div class="bg-light p-2 rounded">A</div>
  <div class="bg-light p-2 rounded">B</div>
  <div class="bg-light p-2 rounded">C</div>
</div>

<!-- Grid with responsive gaps -->
<div class="d-grid gap-2 gap-sm-3 gap-md-4 gap-lg-5" style="grid-template-columns: repeat(2, 1fr);">
  <div class="bg-light p-3 rounded">Card 1</div>
  <div class="bg-light p-3 rounded">Card 2</div>
</div>
```

### Gap with Flex Wrap

Gaps work correctly with `flex-wrap`, maintaining consistent spacing across wrapped lines:

```html
<div class="d-flex flex-wrap gap-3">
  <div class="bg-light p-2 rounded">Tag 1</div>
  <div class="bg-light p-2 rounded">Tag 2</div>
  <div class="bg-light p-2 rounded">Tag 3</div>
  <div class="bg-light p-2 rounded">Tag 4</div>
  <div class="bg-light p-2 rounded">Tag 5</div>
  <div class="bg-light p-2 rounded">Tag 6</div>
</div>
```

## Best Practices

1. **Prefer gap over margin** for spacing flex/grid children; it is cleaner and avoids edge-case margin issues.
2. **Use `gap-3` as a standard** spacing value for most flex and grid layouts.
3. **Combine `row-gap-*` and `column-gap-*`** when vertical and horizontal spacing needs differ.
4. **Use responsive gaps** to tighten layouts on mobile and loosen them on desktop.
5. **Apply gaps to the container**, not individual children, to maintain consistent spacing.
6. **Use gaps for button groups** instead of individual button margins.
7. **Pair gaps with `flex-wrap`** for tag lists, chip groups, and similar components.
8. **Remove gaps with `gap-0`** when nesting containers that should have no inter-item spacing.
9. **Use gaps in card grids** to replace the older margin-based column spacing approach.
10. **Test gap behavior in Firefox and Chrome** to ensure consistent rendering across browsers.
11. **Avoid combining gaps with child margins** on the same axis; it creates unpredictable spacing.
12. **Document your gap scale** in your design system for team consistency.

## Common Pitfalls

1. **Gap requires flex or grid context**: `gap-*` only works on elements with `display: flex`, `display: grid`, or `display: inline-grid`. It has no effect on block containers.
2. **Gaps do not add outer spacing**: Unlike margins, gaps do not create space around the container's edges. Use container padding for outer spacing.
3. **Browser support**: Very old browsers may not support gap in flexbox. All modern browsers (2021+) support it fully.
4. **Forgetting to add flex/grid class**: Adding `gap-3` without `d-flex` or `d-grid` produces no visible effect.
5. **Mixing gap and margin on the same axis**: This results in doubled or inconsistent spacing between items.
6. **Assuming gap works like margin on individual items**: Gap is a container property that affects all children uniformly.

## Accessibility Considerations

- Gaps create consistent spacing between interactive elements, improving touch target separation.
- Ensure gaps do not create visual separation that implies content is unrelated when it is semantically grouped.
- Maintain adequate gaps (minimum 8px) between clickable elements for users with motor impairments.
- Gaps do not affect DOM order or screen reader traversal.
- Use semantic grouping elements (`<nav>`, `<ul>`) with gaps rather than purely visual spacing.

## Responsive Behavior

Gap utilities support all Bootstrap breakpoints for responsive inter-item spacing:

```html
<!-- Flex row with responsive gaps -->
<div class="d-flex flex-wrap gap-1 gap-sm-2 gap-md-3 gap-lg-4">
  <span class="badge bg-primary">Tag</span>
  <span class="badge bg-secondary">Tag</span>
  <span class="badge bg-success">Tag</span>
  <span class="badge bg-danger">Tag</span>
</div>

<!-- Grid layout with responsive column and row gaps -->
<div class="d-grid column-gap-2 column-gap-md-4 row-gap-1 row-gap-md-3" style="grid-template-columns: repeat(3, 1fr);">
  <div class="bg-light p-2 p-md-3 rounded">1</div>
  <div class="bg-light p-2 p-md-3 rounded">2</div>
  <div class="bg-light p-2 p-md-3 rounded">3</div>
  <div class="bg-light p-2 p-md-3 rounded">4</div>
  <div class="bg-light p-2 p-md-3 rounded">5</div>
  <div class="bg-light p-2 p-md-3 rounded">6</div>
</div>
```

The mobile-first approach means the smallest breakpoint class applies by default, with larger breakpoints overriding progressively.
