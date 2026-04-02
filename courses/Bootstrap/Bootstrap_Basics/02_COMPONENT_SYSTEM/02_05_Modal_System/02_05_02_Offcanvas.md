---
title: Offcanvas
category: Component System
difficulty: 2
time: 15 min
tags: bootstrap5, offcanvas, sidebar, slide-in, drawer
---

## Overview

Offcanvas is a sidebar or drawer component that slides in from the edge of the viewport. It uses the `.offcanvas` class for the container and `.offcanvas-header` / `.offcanvas-body` for content structure. Positioning is controlled by `.offcanvas-start`, `.offcanvas-end`, `.offcanvas-top`, or `.offcanvas-bottom`. Offcanvas supports both dismissible (backdrop click to close) and non-dismissible (`data-bs-backdrop="static"`) modes, and can be made responsive to only appear at certain breakpoints.

## Basic Implementation

```html
<!-- Trigger -->
<button class="btn btn-primary" type="button" data-bs-toggle="offcanvas" data-bs-target="#myOffcanvas">
  Open Offcanvas
</button>

<!-- Offcanvas -->
<div class="offcanvas offcanvas-start" tabindex="-1" id="myOffcanvas" aria-labelledby="myOffcanvasLabel">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title" id="myOffcanvasLabel">Offcanvas Title</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
  </div>
  <div class="offcanvas-body">
    <p>Offcanvas content goes here.</p>
  </div>
</div>
```

## Advanced Variations

### Position Options

```html
<!-- Slide from right -->
<div class="offcanvas offcanvas-end" id="rightPanel" tabindex="-1">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Right Panel</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">Content slides in from the right.</div>
</div>

<!-- Slide from top -->
<div class="offcanvas offcanvas-top" id="topPanel" tabindex="-1">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Top Panel</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">Content slides down from the top.</div>
</div>

<!-- Slide from bottom -->
<div class="offcanvas offcanvas-bottom" id="bottomPanel" tabindex="-1">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Bottom Panel</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">Content slides up from the bottom.</div>
</div>
```

### Responsive Offcanvas

Add breakpoint classes like `.offcanvas-lg` to make the offcanvas only slide in on smaller screens. On larger screens, it acts as a static sidebar.

```html
<!-- Only appears as offcanvas below lg breakpoint -->
<div class="offcanvas-lg offcanvas-start" tabindex="-1" id="responsiveSidebar" aria-labelledby="sidebarLabel">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title" id="sidebarLabel">Sidebar</h5>
    <button type="button" class="btn-close d-lg-none" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <nav class="nav flex-column">
      <a class="nav-link" href="#">Home</a>
      <a class="nav-link" href="#">Profile</a>
      <a class="nav-link" href="#">Settings</a>
    </nav>
  </div>
</div>
```

### Backdrop Control

```html
<!-- No backdrop -->
<button class="btn btn-primary" data-bs-toggle="offcanvas" data-bs-target="#noBackdrop" data-bs-backdrop="false">
  No Backdrop
</button>

<!-- Static backdrop (cannot click to dismiss) -->
<button class="btn btn-primary" data-bs-toggle="offcanvas" data-bs-target="#staticBackdrop" data-bs-backdrop="static">
  Static Backdrop
</button>
```

## Best Practices

1. Set `tabindex="-1"` on `.offcanvas` for proper keyboard focus management.
2. Include `aria-labelledby` pointing to the `.offcanvas-title` element.
3. Always provide a close button in `.offcanvas-header` with `data-bs-dismiss="offcanvas"`.
4. Use responsive offcanvas classes (`.offcanvas-lg`, `.offcanvas-xl`) for navigation patterns that become static sidebars on larger screens.
5. Use `data-bs-backdrop="static"` when the user must explicitly dismiss the offcanvas.
6. Keep offcanvas content concise to avoid excessive scrolling on mobile.
7. Use `scroll: true` option if you need to allow body scrolling behind the offcanvas.
8. Avoid nesting offcanvas components inside other interactive elements.
9. Use unique `id` values for each offcanvas to prevent selector conflicts.
10. Test keyboard navigation and screen reader behavior across browsers.

## Common Pitfalls

- **Forgetting `tabindex="-1"`:** Focus will not move into the offcanvas when it opens.
- **Missing `aria-labelledby`:** Screen readers will not identify the offcanvas purpose.
- **Placing offcanvas inside `.navbar`:** This can cause layout and z-index issues; place offcanvas elements at the `<body>` level.
- **Responsive offcanvas without hiding close button:** On larger breakpoints, the close button is unnecessary; hide it with `d-lg-none` (or the relevant breakpoint class).
- **Not using `data-bs-backdrop="static"` when needed:** Users may accidentally close an offcanvas containing unsaved form data.

## Accessibility Considerations

Bootstrap sets `role="dialog"` on offcanvas elements and manages `aria-hidden` toggling automatically. The Escape key closes the offcanvas by default. Focus is trapped within the offcanvas while open, and returns to the trigger element on close. Always link `aria-labelledby` to the offcanvas title for screen reader announcements.

## Responsive Behavior

Responsive offcanvas classes (`.offcanvas-sm`, `.offcanvas-md`, `.offcanvas-lg`, `.offcanvas-xl`, `.offcanvas-xxl`) render the component as a static sidebar above the specified breakpoint. Below the breakpoint, it slides in as a drawer. This is ideal for navigation patterns that need a persistent sidebar on desktop and a collapsible drawer on mobile.
