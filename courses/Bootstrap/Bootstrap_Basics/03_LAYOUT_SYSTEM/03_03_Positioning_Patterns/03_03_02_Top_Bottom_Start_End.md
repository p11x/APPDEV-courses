---
title: "Top, Bottom, Start & End Utilities"
description: "Use Bootstrap 5 offset utilities to position elements with top, bottom, start, and end classes."
difficulty: 1
estimated_time: "15 minutes"
prerequisites:
  - "Position utilities"
  - "Basic CSS positioning"
tags:
  - positioning
  - offsets
  - utilities
  - layout
---

## Overview

Bootstrap 5 provides offset utilities for placing positioned elements: `top-*`, `bottom-*`, `start-*`, and `end-*`. These classes set the CSS `top`, `bottom`, `left` (start), and `right` (end) properties. They only affect elements with `position: relative`, `position: absolute`, `position: fixed`, or `position: sticky`.

The available values are `0`, `50`, and `100`, plus `translate-middle`, `translate-middle-x`, and `translate-middle-y` for centering. The `start` and `end` naming follows logical property conventions for RTL (right-to-left) language support.

## Basic Implementation

### Edge Positioning

Place elements at container edges using `top-0`, `bottom-0`, `start-0`, `end-0`:

```html
<div class="position-relative" style="height: 200px; background: #e9ecef;">
  <div class="position-absolute top-0 start-0 bg-primary text-white p-2 rounded">
    Top-left
  </div>
  <div class="position-absolute top-0 end-0 bg-success text-white p-2 rounded">
    Top-right
  </div>
  <div class="position-absolute bottom-0 start-0 bg-warning text-white p-2 rounded">
    Bottom-left
  </div>
  <div class="position-absolute bottom-0 end-0 bg-danger text-white p-2 rounded">
    Bottom-right
  </div>
</div>
```

### 50% Positioning with Translate

Use `top-50 start-50 translate-middle` to center an element:

```html
<div class="position-relative" style="height: 200px; background: #e9ecef;">
  <div class="position-absolute top-50 start-50 translate-middle bg-dark text-white p-3 rounded">
    Perfectly centered
  </div>
</div>
```

### Partial Offset Positioning

```html
<div class="position-relative" style="height: 200px; background: #e9ecef;">
  <div class="position-absolute top-50 start-0 bg-info text-white p-2 rounded">
    Vertically centered, left edge
  </div>
  <div class="position-absolute top-0 start-50 bg-secondary text-white p-2 rounded">
    Horizontally centered, top edge
  </div>
</div>
```

## Advanced Variations

### Translate Utilities

`translate-middle` shifts an element by 50% of its own width and height, enabling true centering:

```html
<!-- Center horizontally only -->
<div class="position-absolute bottom-0 start-50 translate-middle-x bg-primary text-white p-2 rounded">
  Bottom-center
</div>

<!-- Center vertically only -->
<div class="position-absolute top-50 end-0 translate-middle-y bg-success text-white p-2 rounded">
  Right-center
</div>
```

### Badge Positioning

A common pattern for notification badges:

```html
<button type="button" class="btn btn-primary position-relative">
  Notifications
  <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
    99+
    <span class="visually-hidden">unread messages</span>
  </span>
</button>
```

### Overlaid Content

```html
<div class="position-relative d-inline-block">
  <img src="photo.jpg" class="img-fluid rounded" alt="Photo">
  <div class="position-absolute bottom-0 start-0 end-0 bg-dark bg-opacity-75 text-white p-3">
    Photo caption overlay
  </div>
</div>
```

## Best Practices

1. **Always use `position-relative` on the parent** so that `position-absolute` children reference it, not the document.
2. **Use `start-*` and `end-*`** instead of `left-*` and `right-*` for RTL compatibility.
3. **Pair `top-50 start-50` with `translate-middle`** for true centering that accounts for element dimensions.
4. **Use `translate-middle-x` or `translate-middle-y`** when you only need to center on one axis.
5. **Combine offset utilities with position classes** rather than inline styles for maintainability.
6. **Use `top-0 end-0` for close buttons** and dismiss icons in cards and modals.
7. **Apply `start-100 translate-middle`** for badges that hang off the edge of their parent.
8. **Set explicit dimensions on the parent** when using percentage-based offsets (`top-50`, `start-50`).
9. **Test offset behavior in RTL mode** to verify that `start` and `end` swap correctly.
10. **Avoid mixing pixel and percentage offsets** on the same element for predictable behavior.

## Common Pitfalls

1. **Forgetting `translate-middle`**: Using only `top-50 start-50` places the top-left corner at the center, not the element's center. Add `translate-middle` to offset by half the element's size.
2. **No positioned ancestor**: Offset utilities require a positioned parent. Without `position-relative`, absolute children reference the body.
3. **Using offsets on static elements**: Offsets have no effect on `position-static` elements (the default).
4. **`start-100` without `translate-middle`**: To position a badge outside the parent's right edge, you need both `start-100` and `translate-middle`.
5. **Ignoring element dimensions**: `top-50` positions the top edge at 50%, not the center. The element extends below unless you add vertical translation.

## Accessibility Considerations

- Ensure positioned content does not obscure focusable interactive elements for keyboard navigation.
- Use `visually-hidden` for screen-reader-only labels on positioned badges or indicators.
- Maintain reading order consistency; offset positioning is visual-only and does not affect DOM order.
- Verify that overlaid text has sufficient contrast against its background.
- Provide `aria-label` on icon-only positioned buttons (e.g., close buttons in corners).

## Responsive Behavior

Offset utilities do not include responsive variants by default. For responsive positioning, use custom media queries or enable responsive support in Bootstrap's utility API:

```scss
$utilities: (
  "top": (responsive: true, property: top, class: top, values: (0: 0, 50: 50%, 100: 100%)),
  "start": (responsive: true, property: left, class: start, values: (0: 0, 50: 50%, 100: 100%))
);
```

Alternatively, apply conditional positioning with inline styles or custom CSS for breakpoint-specific behavior.
