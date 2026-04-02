---
title: "Pointer Events Utilities"
description: "Control click and touch interaction with Bootstrap 5 pointer-events utilities"
difficulty: 1
estimated_time: "8 minutes"
tags: ["pointer-events", "click", "interaction", "accessibility"]
---

# Pointer Events Utilities

## Overview

Bootstrap 5 provides `pe-none` and `pe-auto` utilities to control whether an element responds to mouse clicks, touch events, and hover states. The `pe-none` class disables all pointer events on an element (and its children unless overridden), while `pe-auto` restores the default interactive behavior.

These utilities are essential for creating overlay patterns, disabling links inside larger clickable areas, preventing interaction during loading states, and managing event propagation without JavaScript event handlers.

## Basic Implementation

### Disabling Pointer Events

```html
<div class="pe-none bg-light p-3 border">
  <p>This entire container ignores clicks, hovers, and touch events.</p>
  <button class="btn btn-primary">Button won't respond to clicks</button>
  <a href="#">Link won't respond to clicks</a>
</div>
```

### Re-enabling Pointer Events

Override `pe-none` on specific children to restore interactivity:

```html
<div class="pe-none bg-light p-3 border">
  <p>Parent container is non-interactive.</p>
  <button class="btn btn-primary pe-auto">This button works (pe-auto override)</button>
  <button class="btn btn-secondary">This button does not work</button>
</div>
```

### Default Behavior (pe-auto)

```html
<div class="pe-auto bg-light p-3 border">
  <p>All elements in this container are interactive (default behavior).</p>
  <button class="btn btn-success">Clickable</button>
</div>
```

## Advanced Variations

### Disabled Link Inside Card

Make an entire card visually clickable while disabling a specific link:

```html
<div class="card" style="width: 300px; cursor: pointer;" onclick="window.location='/details'">
  <div class="card-body">
    <h5 class="card-title">Clickable Card</h5>
    <p class="card-text">The whole card is clickable.</p>
    <a href="/other" class="pe-none btn btn-outline-secondary disabled">
      This link is disabled (pe-none)
    </a>
    <span class="btn btn-primary">Click anywhere on card</span>
  </div>
</div>
```

### Loading State Overlay

Prevent interaction during async operations:

```html
<div class="position-relative">
  <div class="pe-none opacity-50">
    <form>
      <div class="mb-3">
        <label class="form-label">Email</label>
        <input type="email" class="form-control" value="user@example.com">
      </div>
      <div class="mb-3">
        <label class="form-label">Password</label>
        <input type="password" class="form-control">
      </div>
      <button class="btn btn-primary">Submit</button>
    </form>
  </div>
  <div class="position-absolute top-50 start-50 translate-middle z-1">
    <div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
</div>
```

### Tooltip on Disabled Elements

Tooltips do not fire on `pe-none` elements. Use a wrapper:

```html
<div class="d-inline-block" data-bs-toggle="tooltip" title="This button is disabled">
  <button class="btn btn-primary pe-none" disabled>Disabled with tooltip</button>
</div>
```

### Event Propagation Control

```html
<div class="pe-none bg-light p-4">
  <p class="text-muted">Outer area captures no events</p>
  <div class="pe-auto bg-white p-3 border">
    <p>Inner area restores pointer events</p>
    <button class="btn btn-success">Interactive</button>
  </div>
</div>
```

## Best Practices

1. **Use `pe-none`** on parent containers to disable all child interactions simultaneously. This is cleaner than disabling individual elements.

2. **Apply `pe-auto`** on specific children that need to remain interactive within a `pe-none` parent.

3. **Pair `pe-none` with `disabled`** and `aria-disabled` attributes on buttons and links for complete accessibility compliance.

4. **Use `pe-none` with `opacity-50`** to create visually disabled states that match Bootstrap's design language.

5. **Apply `pe-none` on overlay masks** so clicking the dimmed background does not trigger events on underlying elements.

6. **Remember that `pe-none` propagates** to all children. Any interactive child within a `pe-none` container needs `pe-auto` explicitly.

7. **Use `pe-none` on decorative images and icons** that should not be draggable or clickable.

8. **Combine with `user-select: none`** via custom CSS for elements where both clicking and text selection should be prevented.

9. **Test with touch devices.** `pe-none` affects touch events as well as mouse events, preventing taps and swipes.

10. **Use `pe-none` on text overlays** on top of clickable areas to prevent the text from intercepting clicks meant for the parent.

## Common Pitfalls

### Forgetting that pe-none affects children
`pe-none` on a parent disables all descendant interactions. Forgetting this can make entire UI sections non-interactive. Add `pe-auto` to restore interaction on specific elements.

### pe-none not preventing focus
`pe-none` prevents pointer events but does not prevent programmatic focus or tab navigation. Elements may still receive keyboard focus.

### Tooltips and popovers not working
Bootstrap's tooltip and popover JavaScript listens for pointer events. `pe-none` on the trigger element prevents these from firing.

### Cursor still showing as pointer
`pe-none` does not change the cursor style. Add `cursor: default` or use Bootstrap's cursor utilities to change the cursor appearance on non-interactive elements.

### pe-auto not working without parent pe-none
`pe-auto` only has a visible effect when overriding an inherited `pe-none`. On elements without a `pe-none` parent, `pe-auto` is the default behavior and the class is redundant.

## Accessibility Considerations

`pe-none` prevents mouse and touch interaction but does not communicate disabled state to assistive technology. Always add `disabled`, `aria-disabled="true"`, or `inert` alongside `pe-none` for proper screen reader support.

When `pe-none` is applied to focusable elements like links or buttons, those elements may still be focusable via keyboard navigation. Use `tabindex="-1"` to remove them from the tab order.

For overlay patterns where `pe-none` disables background content, ensure the overlaying content provides all necessary interactive options. Screen reader users should not be able to navigate to disabled background elements.

## Responsive Behavior

Pointer event utilities do not support responsive prefixes natively. If pointer events must change at breakpoints, use custom CSS with media queries or JavaScript to toggle classes:

```html
<style>
  @media (max-width: 767.98px) {
    .pe-mobile-none { pointer-events: none; }
  }
</style>
```

For most use cases, pointer event behavior remains consistent across viewport sizes. Responsive changes are typically needed for touch-specific interaction patterns on mobile devices.
