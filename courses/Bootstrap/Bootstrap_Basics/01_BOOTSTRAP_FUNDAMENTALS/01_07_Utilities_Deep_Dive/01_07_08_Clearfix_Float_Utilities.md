---
title: Clearfix and Float Utilities
category: Bootstrap Fundamentals
difficulty: 1
time: 15 min
tags: bootstrap5, clearfix, float, layout, utilities
---

## Overview

Bootstrap 5 provides float utilities to position elements to the left or right of their container, and the clearfix utility to manage the layout consequences of floated elements. Float classes include `float-start` (left in LTR), `float-end` (right in LTR), and `float-none`. The `clearfix` class is applied to parent containers to prevent collapsed heights when all children are floated. While modern CSS layout tools like flexbox and grid are preferred, floats remain useful for wrapping text around images and simple positioning tasks.

## Basic Implementation

Float utilities position elements to either side of their container.

```html
<!-- Basic float utilities -->
<div class="float-start">Floated to start (left)</div>
<div class="float-end">Floated to end (right)</div>
<div class="float-none">Not floated</div>
```

The clearfix utility resolves the issue where a parent container collapses when all children are floated.

```html
<!-- Clearfix for containing floated children -->
<div class="clearfix border p-2">
  <img src="photo.jpg" class="float-start me-3" style="width: 150px;" alt="Floated image">
  <p>This paragraph wraps around the floated image. The clearfix class ensures
  the parent container expands to contain the floated element properly.</p>
</div>

<!-- Without clearfix: parent collapses -->
<div class="border p-2 mb-3">
  <img src="photo.jpg" class="float-end ms-3" style="width: 150px;" alt="Floated image">
  <p>Without clearfix, the parent may not contain the floated element's height.</p>
</div>
```

Floated elements stack horizontally until they run out of space.

```html
<!-- Multiple floated elements -->
<div class="clearfix border">
  <div class="float-start p-2 bg-primary text-white">Item 1</div>
  <div class="float-start p-2 bg-secondary text-white">Item 2</div>
  <div class="float-start p-2 bg-success text-white">Item 3</div>
</div>
```

## Advanced Variations

Responsive float utilities apply floating only at specific breakpoints and above.

```html
<!-- Responsive floats -->
<div class="clearfix">
  <div class="float-none float-md-start bg-light p-3 me-md-3">
    Not floated on mobile, floated left on medium and up
  </div>
  <p>Text wraps around the element only on medium screens and above.</p>
</div>

<!-- Combining floats with width utilities -->
<div class="clearfix">
  <div class="float-start w-25 bg-light p-3">
    25% width floated left
  </div>
  <div class="float-end w-25 bg-light p-3">
    25% width floated right
  </div>
</div>
```

Float can be used alongside other utility classes for specific layout needs.

```html
<!-- Float with media object pattern -->
<div class="clearfix">
  <div class="float-start me-3">
    <img src="avatar.jpg" class="rounded-circle" width="64" height="64" alt="Avatar">
  </div>
  <div>
    <h5 class="mb-1">User Name</h5>
    <p class="mb-0">User bio text that wraps around the floated avatar image.</p>
  </div>
</div>
```

## Best Practices

1. **Always use `clearfix` on parent containers** - Apply `clearfix` to any parent containing only floated children to prevent height collapse.
2. **Use `float-start` and `float-end`** - These logical property names adapt to text direction, replacing the deprecated `float-left`/`float-right`.
3. **Prefer flexbox or grid for layouts** - Modern CSS layout tools are more predictable and powerful than floats for complex layouts.
4. **Use floats for text wrapping** - The primary modern use case for floats is wrapping text around images.
5. **Apply responsive floats for adaptive layouts** - Use `float-md-start` to float elements only on larger screens.
6. **Clear floats after sections** - Ensure subsequent content is not affected by previous floats using clearfix or clear utilities.
7. **Combine with margin utilities** - Add `me-3` or `ms-3` to floated elements for spacing between the float and adjacent content.
8. **Avoid nesting floated elements** - Deeply nested floats create complex clearing requirements. Use flexbox instead.
9. **Test layout in multiple browsers** - Float behavior can vary slightly across browsers, especially with complex content.
10. **Remove float when not needed** - Use `float-none` to explicitly remove floating at certain breakpoints.

## Common Pitfalls

1. **Collapsed parent height** - Without `clearfix`, parents of floated elements collapse to zero height, breaking subsequent layout.
2. **Unexpected content wrapping** - Floated elements affect all subsequent content until a clearing element. Content may wrap around floats unintentionally.
3. **Float stacking order** - Multiple floats in the same direction stack left-to-right, but the DOM order affects visual stacking.
4. **Margin collapse with floats** - Floated elements do not collapse margins with their children, which can cause unexpected spacing.
5. **Overriding float with other utilities** - Flex or grid utilities on a parent can override float behavior on children. Ensure float is actually needed.

## Accessibility Considerations

Float utilities are visual layout tools and do not affect the reading order for screen readers. Ensure that the visual order created by floating matches the logical reading order in the DOM. When text wraps around floated images, verify that the text remains readable at all viewport sizes. Screen readers announce content in DOM order regardless of visual positioning. Avoid using floats to create visual layouts that contradict the semantic document structure.

## Responsive Behavior

Bootstrap provides responsive float utilities using the pattern `float-{breakpoint}-{direction}`. Available breakpoints are `sm`, `md`, `lg`, `xl`, and `xxl`. For example, `float-md-end` floats an element to the right starting at the `md` breakpoint (768px) and above. Below the breakpoint, the element is not floated. This enables layouts where elements stack vertically on mobile but float beside content on larger screens. Combine responsive floats with responsive width utilities for fully adaptive text-wrapping layouts.
